using QA.Application.DTOs;
using QA.Application.Mappers.Persistence;
using QA.Application.Ports.Inbound;
using QA.Application.Ports.Outbound;
using QA.Domain.Entities;
using QA.Domain.Enums;
using QA.Domain.Factories;
using QA.Infrastructure.Persistence.Models;
using QA.ClientInterface.Utils;

namespace Qa.Application.UseCases
{
    public class CreateQAProcessUseCase(
        ICorporationInfoAdapter corporationAdapter,
        IProcessingInfoAdater processingInfoAdapter,
        IQAProcessRepository repository,
        IOrchestratorAdapter orchestratorAdapter
         ) : IQAProcessCreator<CreateQAProcessDTO>
    {
        public async Task CreateProcess(CreateQAProcessDTO info)
        {
            try
            {
                CorporationInfoResponse corporationInfo = await corporationAdapter.GetInfo(info.ShootingId);

                QAProcess initQaProcess = new (
                    corporationInfo.SelfQA, 
                    QAProcessType.Original, 
                    corporationInfo.BillingPlan, 
                    corporationInfo.CorporationId, 
                    info.ShootingId
                );
                Console.Write($"Billing plan is {corporationInfo.BillingPlan}");
                if (corporationInfo.BillingPlan != BillingPlan.Platinum)
                {                    
                    QAProcessModel initQaProcessModel = QAProcessMapper.ToDbModel(
                        initQaProcess.UpdateStatus(QAProcessStatus.NA)
                    );
                    await repository.Save(initQaProcessModel);

                    Console.WriteLine($"Build image entity and save in DB: {corporationInfo.BillingPlan}");

                    await orchestratorAdapter.Publish(initQaProcess);

                    return;
                }

                Console.Write("Creating processing info....");
                ProcessingInfoResponse processingInfo = await processingInfoAdapter.GetInfo(info.ShootingId);

                Console.Write("Creating media list....");
                List<Media> mediaList = info.Media.ConvertAll(inputMedia =>
                {
                    ViewConfig viewProcessingInfo = processingInfo.ViewConfiguration
                        .Find(view => view.Position == inputMedia.Position)
                        ?? throw new Exception("Image position not found in the input");

                    MediaType InputMediaType = EnumParser.ParseEnum<MediaType>(inputMedia.Type);

                    return MediaFactory.Build(
                        inputMedia.Id,
                        inputMedia.OriginalUrl,
                        inputMedia.OptimizedUrl,
                        inputMedia.Position,
                        info.ShootingId,
                        viewProcessingInfo.View,
                        viewProcessingInfo.ImageType,
                        ProcessingInfoFactory.Build(
                            InputMediaType, 
                            viewProcessingInfo.View == VehicleViews.Exterior 
                            ? processingInfo.GeneralConfiguration.ExteriorCroppingProvider 
                            : processingInfo.GeneralConfiguration.InteriorCroppingProvider, 
                            viewProcessingInfo
                        )
                    );
                });


                Console.Write("Creating shooting...");
                Shooting shooting = new (info.ShootingId, mediaList);

                Console.Write("Creating Remarketing...");
                RemarketingProcess remarketingProcess = new (shooting, processingInfo.GeneralConfiguration);

                Console.Write("Mapping Domain to DB model...");
                QAProcessModel qAProcessModel = QAProcessMapper.ToDbModel(
                    initQaProcess.LinkRemarketingProcess(remarketingProcess)
                );

                // Save to the database
                Console.Write($"Saving QA Process {qAProcessModel.ProcessId} to DB...");
                await repository.Save(qAProcessModel);
            }
            catch (System.Exception)
            {
                throw;
            }
           
        }
    }
}

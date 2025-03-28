using QA.Application.DTOs;
using QA.Domain.Entities;
using QA.Infrastructure.Persistence.Models;


namespace QA.Application.Mappers.Persistence
{
    public static class QAProcessMapper
    {
        public static QAProcessModel ToDbModel(QAProcess domain)
        {
            long createdAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long updatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return new QAProcessModel
            {
                ProcessId = domain.ID,
                RemarketingId = domain.RemarketingProcess?.ID ?? Guid.Empty,
                ShootingId = domain.ShootingId,
                Status = domain.Status,
                SelfQA = domain.SelfQA,
                Priority = domain.Priority,
                CorporationId = domain.CorporationId,
                PriorityTimestamp = (1 + (int) domain.Priority) * createdAt,
                BillingPlan = domain.BillingPlan,
                UserId = domain.AssignedUser?.ID,
                Type = domain.QAProcessType,
                ProcessingInfo = RemarketingProcessingInfoMapper.ToDbModel(domain.RemarketingProcess?.ProcessingInfo),
                Media = domain.RemarketingProcess?.Shooting?.Media?.ConvertAll(MediaMapper.ToDbModel),
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };
        }

        public static QAProcess FromDbModel(QAProcessModel model)
        {
            Shooting shooting = new (model.ShootingId, model.Media?.ConvertAll((media) => MediaMapper.FromDbModel(media, model.ShootingId)) ?? []) ;

            RemarketingProcess remarketingProcess = new (
                    shooting,
                    RemarketingProcessingInfoMapper.FromDbModel(model.ProcessingInfo!),
                    model.RemarketingId
            );

            QAProcess qAProcess = new(model.SelfQA, model.Type, model.BillingPlan, model.CorporationId, model.ShootingId);

            qAProcess.SetPriority(model.Priority).UpdateStatus(model.Status).LinkRemarketingProcess(remarketingProcess);
            return qAProcess;
        }

        public static QAProcessDto ToDto(QAProcess process)
        {
            return new QAProcessDto(
                process.ID,
                process.RemarketingProcess?.ID ?? Guid.Empty,
                process.ShootingId,
                process.CorporationId,
                process.Status.ToString(),
                process.SelfQA,
                process.Priority.ToString(),
                process.AssignedUser?.ID,
                process.BillingPlan.ToString(),
                process.QAProcessType.ToString()
            );
        }
    }
}

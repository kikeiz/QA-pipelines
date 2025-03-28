using QA.Domain.Enums;
using QA.Domain.Entities.RepushProcess.Steps;
using QA.Domain.ValueObjects;
using QA.Domain.ValueObjects.StepPayloads;
using QA.Domain.Interfaces;



namespace QA.Domain.Entities.RepushProcess
{
    public record AvailableStep<T>(bool AddStep, Func<T, RemarketingProcess, IStep<StepPayloadBase>> StepCreator);

    public abstract class RepushProcess<MediaType>
    {
        protected List<IStep<StepPayloadBase>> Steps { get; }
        protected MediaType Media { get; }
        protected ProcessType Type { get; }
        protected Source Source { get; }
        protected RemarketingProcess RemarketingProcess { get; }

        protected RepushProcess(MediaType media, RemarketingProcess remarketingProcess, ProcessType type, Source source)
        {
            Media = media;
            RemarketingProcess = remarketingProcess;
            Type = type;
            Source = source;
            Steps = Build(AvailableSteps(media, remarketingProcess));
        }

        protected abstract List<AvailableStep<MediaType>> AvailableSteps(MediaType media, RemarketingProcess remarketingProcess);
        private List<IStep<StepPayloadBase>> Build(List<AvailableStep<MediaType>> steps)
        {
            return steps
                .FindAll(step => step.AddStep)                    
                .ConvertAll(step => step.StepCreator(Media, RemarketingProcess));
        }
    }

    public class ImageProcess(Image Media, RemarketingProcess RemarketingProcess, ProcessType Type, Source Source)
        : RepushProcess<Image>(Media, RemarketingProcess, Type, Source)
    {
        protected override List<AvailableStep<Image>> AvailableSteps(Image image, RemarketingProcess remarketingProcess)
        {
            ImageProcessingInfo imageInfo = image.ProcessingInfo;
            RemarketingProcessingInfo processingInfo = remarketingProcess.ProcessingInfo;

            return
            [
                new(imageInfo.Crop && imageInfo.CroppingProvider == CroppingProvider.NEXTLANE_AI, 
                    (media, remProcess) => new CroppingStep(media, remProcess)),

                new(imageInfo.Crop && imageInfo.CroppingProvider == CroppingProvider.REMOVE_BG, 
                    (media, remProcess) => new RemoveBgStep(media, remProcess)),

                new(imageInfo.AddShadow && image.View == VehicleViews.Exterior, 
                    (media, remProcess) => new ShadowStep(media, remProcess)),

                new(processingInfo.BackgroundType != BackgroundType.None, 
                    (media, remProcess) => new BackgroundStep(media, remProcess)),

                new(imageInfo.AddLpc, 
                    (media, remProcess) => new LPCStep(media, remProcess)),

                new(imageInfo.AddStamp, 
                    (media, remProcess) => new StampStep(media, remProcess)),

                new(imageInfo.ZoomPercentage != 0 || imageInfo.CorrectRotation || imageInfo.Center || imageInfo.Enhance || imageInfo.DesaturateYellow, 
                    (media, remProcess) => new PostProcessingStep(media, remProcess))
            ];
        }
    }

}

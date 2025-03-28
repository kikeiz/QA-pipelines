
namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;

    public class PostProcessingStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, PostProcessingStepPayload>(Media, RemarketingProcess)
    {
        public override StepNames Name => StepNames.POSTPROCESSING;

        protected override PostProcessingStepPayload Build() =>
            new(
                Name,
                new PostProcessingStepInfo(
                    Media.ProcessingInfo.DesaturateYellow,
                    Media.ProcessingInfo.Enhance,
                    Media.ProcessingInfo.CorrectRotation,
                    Media.ProcessingInfo.Center,
                    Media.ProcessingInfo.ZoomPercentage)
                );
    }
}
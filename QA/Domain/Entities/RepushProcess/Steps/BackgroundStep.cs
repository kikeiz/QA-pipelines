namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;
    public class BackgroundStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, BackgroundStepPayload>(Media, RemarketingProcess)
    {
        
        public override StepNames Name => StepNames.BACKGROUND;

        protected override BackgroundStepPayload Build() =>
            new(
                Name,
                new BackgroundStepInfo(
                    Media.View,
                    RemarketingProcess.ProcessingInfo.BackgroundType,
                    false,
                    RemarketingProcess.ProcessingInfo.BackgroundUrl,
                    Media.ProcessingInfo.Margins,
                    RemarketingProcess.ProcessingInfo.OutputDimensions,
                    RemarketingProcess.ProcessingInfo.BlurringValue)
            );
    }
}
namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;

    public class RemoveBgStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, RemoveBgStepPayload>(Media, RemarketingProcess)
    {
        public override StepNames Name => StepNames.REMOVE_BG;

        protected override RemoveBgStepPayload Build() =>
            new(Name,  new RemoveBgStepInfo(Media.View, RemarketingProcess.ProcessingInfo.BackgroundType, Media.ProcessingInfo.AddShadow));
    }
}
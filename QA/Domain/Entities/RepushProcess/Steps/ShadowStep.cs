namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;

    public class ShadowStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, ShadowStepPayload>(Media, RemarketingProcess)
    {
        public override StepNames Name => StepNames.SHADOW;

        protected override ShadowStepPayload Build() =>
            new(Name, new ShadowStepInfo(Media.Type));
    }
}
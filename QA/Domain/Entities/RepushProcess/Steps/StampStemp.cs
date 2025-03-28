namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;

    public class StampStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, StampStepPayload>(Media, RemarketingProcess)
    {
        public override StepNames Name => StepNames.STAMP;

        protected override StampStepPayload Build() =>
            new(Name, new StampStepInfo(RemarketingProcess.ProcessingInfo.StampUrl));
    }
}
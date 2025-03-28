namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;
    public class LPCStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, LPCStepPayload>(Media, RemarketingProcess)
    {
        public override StepNames Name => StepNames.LPC;

        protected override LPCStepPayload Build() =>
            new(Name, new LPCStepInfo(RemarketingProcess.ProcessingInfo.LpcUrl));
    }
}


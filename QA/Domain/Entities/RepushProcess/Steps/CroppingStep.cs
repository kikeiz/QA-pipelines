namespace QA.Domain.Entities.RepushProcess.Steps
{
    using QA.Domain.Entities;
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects.StepPayloads;

    public class CroppingStep(Image Media, RemarketingProcess RemarketingProcess) : Step<Image, CroppingStepPayload>(Media, RemarketingProcess)
    {
        public override StepNames Name => StepNames.CROP;

        protected override CroppingStepPayload Build() =>
            new(Name, new CroppingStepInfo(Media.View));
    }
}
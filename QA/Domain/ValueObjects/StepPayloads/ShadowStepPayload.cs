    using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{

    public record ShadowStepPayload(StepNames Name, ShadowStepInfo Info): StepPayloadBase(Name);
    public record ShadowStepInfo(ImageTypes? Angle);

}
    using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{

    public record LPCStepPayload(StepNames Name, LPCStepInfo Info): StepPayloadBase(Name);
    public record LPCStepInfo(string? LpcUrl = null);

}
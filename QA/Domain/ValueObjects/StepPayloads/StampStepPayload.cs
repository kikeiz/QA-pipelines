using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{
        
    public record StampStepPayload(StepNames Name, StampStepInfo Info): StepPayloadBase(Name);
    public record StampStepInfo(string? StampUrl = null);

}
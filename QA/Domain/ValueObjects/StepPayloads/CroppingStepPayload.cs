using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{
    public record CroppingStepPayload(StepNames Name, CroppingStepInfo Info): StepPayloadBase(Name);
    public record CroppingStepInfo(VehicleViews View);

}
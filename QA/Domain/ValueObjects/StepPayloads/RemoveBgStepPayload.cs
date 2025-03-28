using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{
    public record RemoveBgStepPayload(StepNames Name, RemoveBgStepInfo Info): StepPayloadBase(Name);
    public record RemoveBgStepInfo(VehicleViews View, BackgroundType BackgroundType, bool AddShadow);

}
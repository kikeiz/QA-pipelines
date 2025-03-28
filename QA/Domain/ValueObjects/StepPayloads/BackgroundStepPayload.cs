using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{
    public record BackgroundStepPayload(StepNames Name, BackgroundStepInfo Info) : StepPayloadBase(Name);
    public record BackgroundStepInfo(VehicleViews View, BackgroundType BackgroundType, bool UseSensorData, string? BackgroundUrl = null, Margins? Margins = null, OutputDimensions? Dimensions = null, double? BlurringValue = null);

}
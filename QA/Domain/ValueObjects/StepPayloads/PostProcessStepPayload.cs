using QA.Domain.Enums;

namespace QA.Domain.ValueObjects.StepPayloads
{
    public record PostProcessingStepPayload(StepNames Name, PostProcessingStepInfo Info): StepPayloadBase(Name);
    public record PostProcessingStepInfo(bool? DesaturateYellow = null, bool? Enhance = null, bool? CorrectRotation = null, bool? Center = null, double? ZoomPercentage = null);

}
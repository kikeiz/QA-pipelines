using QA.Domain.Enums;
using QA.Domain.ValueObjects.StepPayloads;

namespace QA.Domain.Interfaces
{
    public interface IStep<out PayloadType> where PayloadType : StepPayloadBase
    {
        StepNames Name { get; }
        PayloadType RequestPayload { get; }
    }
}
using QA.Domain.Enums;
using QA.Domain.Interfaces;
using QA.Domain.ValueObjects.StepPayloads;


namespace QA.Domain.Entities.RepushProcess {

    public abstract class Step<MediaType, PayloadType> : IStep<PayloadType> where PayloadType : StepPayloadBase
    {
        public abstract StepNames Name { get; }
        public PayloadType RequestPayload { get; }

        protected MediaType Media { get; }
        protected RemarketingProcess RemarketingProcess { get; }

        protected Step(MediaType media, RemarketingProcess remarketingProcess)
        {
            Media = media;
            RemarketingProcess = remarketingProcess;
            RequestPayload = Build();
        }

        protected abstract PayloadType Build();
    }

}

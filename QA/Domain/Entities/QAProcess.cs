namespace QA.Domain.Entities
{
    using System.Text.Json.Serialization;
    using QA.Domain.Enums;

    public class QAProcess(
        bool selfQA,
        QAProcessType qAProcessType,
        BillingPlan billingPlan,
        Guid corporationId,
        Guid shootingId
    )
    {
        public Guid ID = Guid.NewGuid();

        public ProcessPriority Priority = ProcessPriority.Normal;
        public QAProcessType QAProcessType = qAProcessType;
        
        [JsonInclude] public RemarketingProcess? RemarketingProcess;
        public QAProcessStatus Status { get; set; } = QAProcessStatus.Pending;
        public int ProcessedMediaCount { get; private set; } = 0;
        public bool SelfQA { get; set; } = selfQA;
        public Guid CorporationId { get; set; } = corporationId;
        public Guid ShootingId { get; set; } = shootingId;
        public BillingPlan BillingPlan { get; set; } = billingPlan;

        public User? AssignedUser { get; private set; }

        public QAProcess LinkRemarketingProcess(RemarketingProcess remarketingProcess)
        {
            RemarketingProcess = remarketingProcess;
            return this;
        }

        public bool IsAssignable(User user)
        {
            return Status == QAProcessStatus.Pending && AssignedUser == null &&
                   user.AllowedActions == AllowedActions.Full;
        }

        public QAProcess UpdateStatus(QAProcessStatus status)
        {
            Status = status;
            return this;
        }

        public QAProcess SetPriority(ProcessPriority priority)
        {
            Priority = priority;
            return this;
        }

        public QAProcess UnassignUser()
        {
            AssignedUser = null;
            return this;
        }

        public void AssignTo(User user)
        {
            if (!IsAssignable(user))
                throw new InvalidOperationException("Shooting is not assignable.");

            AssignedUser = user;
        }

        public QAProcess MarkAsProcessed()
        {
            Status = QAProcessStatus.Processed;
            return this;
        }

    }
}

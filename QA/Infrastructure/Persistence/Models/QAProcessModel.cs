using Amazon.DynamoDBv2.DataModel;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using MediaType = QA.Domain.Enums.MediaType;

namespace QA.Infrastructure.Persistence.Models
{
    [DynamoDBTable("QaProcess-dev")]
    public class QAProcessModel
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("processId")]
        public Guid ProcessId { get; set; }

        [DynamoDBProperty("remarketingId")]
        public Guid RemarketingId { get; set; }

        [DynamoDBProperty("shootingId")]
        public Guid ShootingId { get; set; }

        [DynamoDBProperty("corporationId")]
        public Guid CorporationId { get; set; }

        [DynamoDBProperty("status")]
        public string StatusString
        {
            get => Status.ToString();
            set => Status = Enum.Parse<QAProcessStatus>(value);
        }

        [DynamoDBIgnore]
        public QAProcessStatus Status { get; set; }

        [DynamoDBProperty("selfQA")]
        public bool SelfQA { get; set; }

        [DynamoDBProperty("priority")]
        public string PriorityString
        {
            get => Priority.ToString();
            set => Priority = Enum.Parse<ProcessPriority>(value);
        }

        [DynamoDBIgnore]
        public ProcessPriority Priority { get; set; }

        [DynamoDBProperty("userId")]
        public Guid? UserId { get; set; }

        [DynamoDBProperty("billingPlan")]
        public string BillingPlanString
        {
            get => BillingPlan.ToString();
            set => BillingPlan = Enum.Parse<BillingPlan>(value);
        }

        [DynamoDBIgnore]
        public BillingPlan BillingPlan { get; set; }

        [DynamoDBProperty("type")]
        public string TypeString
        {
            get => Type.ToString();
            set => Type = Enum.Parse<QAProcessType>(value);
        }

        [DynamoDBIgnore]
        public QAProcessType Type { get; set; }

        [DynamoDBProperty("processingInfo")]
        public RemarketingProcessingInfoModel? ProcessingInfo { get; set; }

        [DynamoDBProperty("media")]
        public List<MediaModel>? Media { get; set; } = [];

        [DynamoDBProperty("priorityDate")]
        public long PriorityTimestamp { get; set; }

        [DynamoDBProperty("createdAt")]
        public long CreatedAt { get; set; }

        [DynamoDBProperty("updatedAt")]
        public long UpdatedAt { get; set; }
    }

}

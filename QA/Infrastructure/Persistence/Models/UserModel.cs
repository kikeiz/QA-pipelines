using Amazon.DynamoDBv2.DataModel;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;

namespace QA.Infrastructure.Persistence.Models
{
    [DynamoDBTable("User")]
    public class UserModel
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("userId")]
        public Guid UserId { get; set; }

        [DynamoDBProperty("corporationId")]
        public Guid CorporationId { get; set; }

        [DynamoDBProperty("selfQA")]
        public bool SelfQA { get; set; }

        [DynamoDBProperty("managementLevel")]
        public ManagementLevel ManagementLevel { get; set; }

        [DynamoDBProperty("allowedActions")]
        public List<AllowedActions> AllowedActions { get; set; } = [];

        [DynamoDBProperty("createdAt")]
        public int CreatedAt { get; set; } // UNIX timestamp as int
    }
}
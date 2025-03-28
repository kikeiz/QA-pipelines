namespace QA.Domain.Entities
{
    using QA.Domain.ValueObjects;
    using QA.Domain.Enums;

    public class User(
        AllowedActions allowedActions, 
        Guid? ID, 
        ManagementLevel? managementLevel, 
        List<QAProcess>? qaProcesses,
        bool? selfQA,
        Guid? corporationId
        )
    {
        public Guid ID { get; } = ID ?? Guid.NewGuid();
        public AllowedActions AllowedActions { get; } = allowedActions;
        public ManagementLevel? ManagementLevel { get; } = managementLevel;
        public List<QAProcess> QAProcesses { get; } = qaProcesses ?? [];
        public Guid? CorporationId { get; } = corporationId;
        public bool? SelfQA { get; } = selfQA;
    }
}

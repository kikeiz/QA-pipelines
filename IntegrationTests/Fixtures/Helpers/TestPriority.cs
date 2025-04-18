namespace IntegrationTests.Fixtures.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute(int priority) : Attribute
    {
        public int Priority { get; private set; } = priority;
    }
}
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationTests.Fixtures.Helpers
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            string assemblyName = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

            foreach (TTestCase testCase in testCases)
            {
                int priority = testCase.TestMethod.Method
                    .GetCustomAttributes(assemblyName)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            foreach (TTestCase testCase in sortedMethods
                .Keys
                .SelectMany(priority => sortedMethods[priority].OrderBy(tc => tc.TestMethod.Method.Name)))
            {
                yield return testCase;
            }
        }

        private static TValue GetOrCreate<TKey, TValue>(
            SortedDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new() =>
            dictionary.TryGetValue(key, out TValue? result)
                ? result
                : (dictionary[key] = new TValue());
    }
}

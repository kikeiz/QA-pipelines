using QA.Infrastructure.Persistence.Contexts.Resolvers.Interfaces;
using QA.Infrastructure.Persistence.Models;

namespace QA.Infrastructure.Persistence.Contexts.Resolvers
{
    public class EnvironmentTableNameResolver : ITableNameResolver
    {
        public string Resolve<T>() where T : class
        {
            var type = typeof(T);

            var tableName = type switch
            {
                Type t when t == typeof(QAProcessModel) =>
                    GetTableNameFromEnv("QA_PROCESS_TABLE", "QaProcess"),

                Type t when t == typeof(UserModel) =>
                    GetTableNameFromEnv("USER_TABLE", "UserTable"),

                _ => type.Name
            };

            return tableName;
        }

        private static string GetTableNameFromEnv(string envVar, string fallback)
        {
            var fromEnv = Environment.GetEnvironmentVariable(envVar);
            return string.IsNullOrWhiteSpace(fromEnv) ? fallback : fromEnv;
        }
    }
}

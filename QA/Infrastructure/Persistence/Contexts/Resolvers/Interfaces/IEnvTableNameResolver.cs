namespace QA.Infrastructure.Persistence.Contexts.Resolvers.Interfaces
{
    public interface ITableNameResolver
    {
        string Resolve<T>() where T : class;
    }
}

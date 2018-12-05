namespace Core.Patterns
{
    public interface IRepositoryFactory
    {
        IRepository<T> Create<T>() where T : class;
    }
}
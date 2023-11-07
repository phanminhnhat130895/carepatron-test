namespace Application.Repositories
{
    public interface IUnitOfWork
    {
        int Save();
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}

namespace Application.Repositories
{
    public interface IEmailRepository
    {
        Task Send(string email, string message);
    }
}

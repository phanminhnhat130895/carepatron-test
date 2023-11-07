namespace Application.Repositories
{
    public interface IDocumentRepository
    {
        Task SyncDocumentsFromExternalSource(string email);
    }
}

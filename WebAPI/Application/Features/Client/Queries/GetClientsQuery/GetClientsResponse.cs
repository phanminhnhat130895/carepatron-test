namespace Application.Features.Client.Queries.GetClientsQuery
{
    public class GetClientsResponse
    {
        public int TotalCount { get; set; }
        public IList<Domain.Entities.Client> Clients { get; set; }
    }
}

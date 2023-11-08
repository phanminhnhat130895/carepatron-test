using MediatR;

namespace Application.Features.Client.Queries.GetClientsQuery
{
    public sealed record GetClientsRequest : IRequest<GetClientsResponse>
    {
        public string SearchString { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}

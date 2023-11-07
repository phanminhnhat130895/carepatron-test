using MediatR;

namespace Application.Features.Client.Queries.GetClientsQuery
{
    public sealed record GetClientsRequest : IRequest<GetClientsResponse>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}

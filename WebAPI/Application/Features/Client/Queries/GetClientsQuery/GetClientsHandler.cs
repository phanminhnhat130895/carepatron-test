using Application.Common.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Features.Client.Queries.GetClientsQuery
{
    public sealed class GetClientsHandler : IRequestHandler<GetClientsRequest, GetClientsResponse>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientsHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<GetClientsResponse> Handle(GetClientsRequest request, CancellationToken cancellationToken)
        {
            return  await _clientRepository.GetClientsAsync(request, cancellationToken);
        }
    }
}

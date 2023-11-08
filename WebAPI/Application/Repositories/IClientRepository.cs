using Application.Features.Client.Queries.GetClientsQuery;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Repositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<Client?> GetClientByEmail(string email, CancellationToken cancellationToken);
        Task CreateClientAsync(Client client, CancellationToken cancellationToken);
        void UpdateClient(Client client);
        Task<GetClientsResponse> GetClientsAsync(GetClientsRequest request, CancellationToken cancellationToken);
    }
}

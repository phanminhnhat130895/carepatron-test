using Application.Features.Client.Queries.GetClientsQuery;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Repositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<EntityEntry<Client>> CreateClientAsync(Client client, CancellationToken cancellationToken);
        void UpdateClient(Client client);
        Task<List<Client>> GetClientsAsync(GetClientsRequest request, CancellationToken cancellationToken);
    }
}

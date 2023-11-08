using Application.Features.Client.Queries.GetClientsQuery;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(DataContext context) : base(context)
        {

        }

        public async Task<Client?> GetClientByEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Clients.FirstOrDefaultAsync(x => x.Email == email ,cancellationToken);
        }

        public async Task CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            await base.CreateAsync(client, cancellationToken);
        }

        public void UpdateClient(Client client)
        {
            base.Update(client);
        }

        public async Task<GetClientsResponse> GetClientsAsync(GetClientsRequest request, CancellationToken cancellationToken)
        {
            var countClient = await _context.Clients.CountAsync(cancellationToken);
            var clients = await _context.Clients.AsNoTracking()
                                    .Where(x => string.IsNullOrEmpty(request.SearchString) ||
                                                x.FirstName.ToLower().StartsWith(request.SearchString.ToLower()) || 
                                                x.LastName.ToLower().StartsWith(request.SearchString.ToLower())
                                    )
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync(cancellationToken);

            var result = new GetClientsResponse();
            result.Clients = clients;
            result.TotalCount = countClient;

            return result;
        }
    }
}

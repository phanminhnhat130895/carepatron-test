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
            var countClientTask = _context.Clients.CountAsync(cancellationToken);
            var getClientTask = _context.Clients.AsNoTracking()
                                    .Where(x => string.IsNullOrEmpty(request.SearchString) ||
                                                x.FirstName.ToLower().StartsWith(request.SearchString.ToLower()) || 
                                                x.LastName.ToLower().StartsWith(request.SearchString.ToLower())
                                    )
                                    .Skip(request.Page * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync(cancellationToken);

            await Task.WhenAll(countClientTask, getClientTask);

            var result = new GetClientsResponse();
            result.Clients = getClientTask.Result;
            result.TotalCount = countClientTask.Result;

            return result;
        }
    }
}

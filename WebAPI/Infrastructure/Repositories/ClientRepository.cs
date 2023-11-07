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
        private readonly IDocumentRepository _documentRepository;
        private readonly IEmailRepository _emailRepository;

        public ClientRepository(DataContext context, IDocumentRepository documentRepository, IEmailRepository emailRepository) : base(context)
        {
            _documentRepository = documentRepository;
            _emailRepository = emailRepository;
        }

        public async Task<EntityEntry<Client>> CreateClientAsync(Client client, CancellationToken cancellationToken)
        {
            return await base.CreateAsync(client, cancellationToken);
        }

        public void UpdateClient(Client client)
        {
            base.Update(client);
        }

        public async Task<List<Client>> GetClientsAsync(GetClientsRequest request, CancellationToken cancellationToken)
        {
            return await _context.Clients
                                    .Skip(request.Page * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync(cancellationToken);
        }
    }
}

using Application.Repositories;
using MediatR;

namespace Application.Features.Client.Commands.CreateClientCommand
{
    public sealed class CreateClientHandler : IRequestHandler<CreateClientRequest, CreateClientResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;

        public CreateClientHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
        }

        public async Task<CreateClientResponse> Handle(CreateClientRequest request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            var client = new Domain.Entities.Client(id);
            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;
            client.DateCreated = DateTime.Now;

            var data = await _clientRepository.CreateClientAsync(client, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateClientResponse()
            {
                Id = data.Entity.Id,
                FirstName = data.Entity.FirstName,
                LastName = data.Entity.LastName,
                Email = data.Entity.Email,
                PhoneNumber = data.Entity.PhoneNumber
            };
        }
    }
}

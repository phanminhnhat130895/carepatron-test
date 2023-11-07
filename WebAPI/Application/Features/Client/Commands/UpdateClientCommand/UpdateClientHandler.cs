using Application.Common.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Features.Client.Commands.UpdateClientCommand
{
    public sealed class UpdateClientHandler : IRequestHandler<UpdateClientRequest, UpdateClientResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;

        public UpdateClientHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
        }

        public async Task<UpdateClientResponse> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetAsync(request.Id, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException("Client not found.");
            }

            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;
            client.DateUpdated = DateTime.Now;

            _clientRepository.UpdateClient(client);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateClientResponse()
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };
        }
    }
}

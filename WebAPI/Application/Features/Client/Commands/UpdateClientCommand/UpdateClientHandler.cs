using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Repositories;
using MediatR;

namespace Application.Features.Client.Commands.UpdateClientCommand
{
    public sealed class UpdateClientHandler : IRequestHandler<UpdateClientRequest, UpdateClientResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceBusHelper _serviceBusHelper;
        private readonly ServiceBusQueue _serviceBusQueue;

        public UpdateClientHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository, IServiceBusHelper serviceBusHelper, ServiceBusQueue serviceBusQueue)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _serviceBusHelper = serviceBusHelper;
            _serviceBusQueue = serviceBusQueue;
        }

        public async Task<UpdateClientResponse> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetAsync(request.Id, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException("Client not found.");
            }

            var oldEmail = client.Email;
            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;
            client.DateUpdated = DateTime.Now;

            _clientRepository.UpdateClient(client);
            await _unitOfWork.SaveAsync(cancellationToken);

            if (oldEmail != request.Email)
            {
                await _serviceBusHelper.SendMessage(client.Email, _serviceBusQueue.QueueName, _serviceBusQueue.ConnectionString);
            }

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

﻿using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Repositories;
using MediatR;

namespace Application.Features.Client.Commands.CreateClientCommand
{
    public sealed class CreateClientHandler : IRequestHandler<CreateClientRequest, CreateClientResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceBusHelper _serviceBusHelper;

        public CreateClientHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository, IServiceBusHelper serviceBusHelper)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _serviceBusHelper = serviceBusHelper;
        }

        public async Task<CreateClientResponse> Handle(CreateClientRequest request, CancellationToken cancellationToken)
        {
            var clientByEmail = await _clientRepository.GetClientByEmail(request.Email, cancellationToken);

            if (clientByEmail != null)
            {
                throw new ResourceConflictException("Client already exists.");
            }

            var id = Guid.NewGuid();

            var client = new Domain.Entities.Client(id);
            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;
            client.DateCreated = DateTime.Now;

            await _clientRepository.CreateClientAsync(client, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            await _serviceBusHelper.SendMessage(client.Email);

            return new CreateClientResponse()
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

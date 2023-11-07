using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Features.Client.Commands.CreateClientCommand;
using Application.Features.Client.Commands.UpdateClientCommand;
using Application.Features.Client.Queries.GetClientsQuery;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnitTest
{
    public class Client_Test : BaseService_Test, IDisposable
    {
        private Mock<IClientRepository> _clientRepository;
        private Mock<IServiceBusHelper> _serviceBusHelper;
        private Mock<ServiceBusQueue> _serviceBusQueue;

        protected override void Setup()
        {
            _clientRepository = new Mock<IClientRepository>();
            _serviceBusHelper = new Mock<IServiceBusHelper>();
            _serviceBusQueue = new Mock<ServiceBusQueue>();
        }

        [Fact]
        public async Task Create_Client_Success()
        {
            var request = new CreateClientRequest()
            {
                Email = "nhat.phan@gmail.com",
                FirstName = "Nhat",
                LastName = "Phan",
                PhoneNumber = "+84366016101"
            };

            var command = new CreateClientHandler(_mockUnitOfWork.Object, _clientRepository.Object, _serviceBusHelper.Object, _serviceBusQueue.Object);

            var result = await command.Handle(request, default);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(CancellationToken.None), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public void Invalid_Create_Client_Request()
        {
            var request = new CreateClientRequest();

            var validator = new CreateClientValidator();

            var validationResult = validator.Validate(request);

            Assert.False(validationResult.IsValid);
            Assert.Equal("FirstName cannot be empty.", validationResult.Errors[0].ErrorMessage);
            Assert.Equal("LastName cannot be empty.", validationResult.Errors[1].ErrorMessage);
            Assert.Equal("Email cannot be empty.", validationResult.Errors[2].ErrorMessage);
            Assert.Equal("PhoneNumber cannot be empty.", validationResult.Errors[3].ErrorMessage);
        }

        [Fact]
        public async Task Update_Client_Success()
        {
            var updateClientId = new Guid("c9d98a47-cb0b-4150-9ca7-2d8bbf979656");
            var request = new UpdateClientRequest()
            {
                Id = updateClientId,
                Email = "nhat.phan@gmail.com",
                FirstName = "Nhat",
                LastName = "Phan",
                PhoneNumber = "+84366016101"
            };

            var updatedClient = new Client(updateClientId)
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateUpdated = DateTime.Now
            };

            _clientRepository
                .Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClient);

            var command = new UpdateClientHandler(_mockUnitOfWork.Object, _clientRepository.Object, _serviceBusHelper.Object, _serviceBusQueue.Object);

            var result = await command.Handle(request, default);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(CancellationToken.None), Times.Once);
            _clientRepository.Verify(repo => repo.UpdateClient(It.IsAny<Client>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(updateClientId, result.Id);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public async Task Update_Client_Throw_Exception()
        {
            var request = new UpdateClientRequest()
            {
                Id = Guid.NewGuid(),
                Email = "nhat.phan@gmail.com",
                FirstName = "Nhat",
                LastName = "Phan",
                PhoneNumber = "+84366016101"
            };

            var command = new UpdateClientHandler(_mockUnitOfWork.Object, _clientRepository.Object, _serviceBusHelper.Object, _serviceBusQueue.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await command.Handle(request, default));
        }

        [Fact]
        public void Invalid_Update_Client_Request()
        {
            var request = new UpdateClientRequest();

            var validator = new UpdateClientValidator();

            var validationResult = validator.Validate(request);

            Assert.False(validationResult.IsValid);
            Assert.Equal("FirstName cannot be empty.", validationResult.Errors[0].ErrorMessage);
            Assert.Equal("LastName cannot be empty.", validationResult.Errors[1].ErrorMessage);
            Assert.Equal("Email cannot be empty.", validationResult.Errors[2].ErrorMessage);
            Assert.Equal("PhoneNumber cannot be empty.", validationResult.Errors[3].ErrorMessage);
        }

        [Fact]
        public async Task Get_Client_Success()
        {
            var request = new GetClientsRequest();

            var expectedClients = new List<Client>
            {
                new Client(Guid.NewGuid()) { FirstName = "John", LastName = "Doe", Email = "johndoe@example.com" },
                new Client(Guid.NewGuid()) { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
            };

            var response = new GetClientsResponse { Clients = expectedClients };

            _clientRepository
                .Setup(repo => repo.GetClientsAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var command = new GetClientsHandler(_clientRepository.Object);

            var result = await command.Handle(request, default);

            Assert.NotNull(result);
            Assert.NotNull(result.Clients);
            Assert.Equal(expectedClients.Count, result.Clients.Count);
        }

        [Fact]
        public void Get_Client_Failure()
        {
            var request = new GetClientsRequest();
            request.Page = -1;
            request.PageSize = -1;

            var validator = new GetClientValidator();

            var validationResult = validator.Validate(request);
            Assert.False(validationResult.IsValid);
            Assert.Equal("Invalid page.", validationResult.Errors[0].ErrorMessage);
            Assert.Equal("Invalid page size.", validationResult.Errors[1].ErrorMessage);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
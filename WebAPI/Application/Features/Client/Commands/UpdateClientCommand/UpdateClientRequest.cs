using MediatR;

namespace Application.Features.Client.Commands.UpdateClientCommand
{
    public sealed record UpdateClientRequest : IRequest<UpdateClientResponse>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}

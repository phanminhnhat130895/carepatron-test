using MediatR;

namespace Application.Features.Client.Commands.CreateClientCommand
{
    public sealed record CreateClientRequest : IRequest<CreateClientResponse>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}

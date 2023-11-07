using FluentValidation;

namespace Application.Features.Client.Queries.GetClientsQuery
{
    public sealed class GetClientValidator : AbstractValidator<GetClientsRequest>
    {
        public GetClientValidator()
        {
            RuleFor(x => x.Page).Must((x, v) => v >= 0).WithMessage("Invalid page.");

            RuleFor(x => x.PageSize).Must((x, v) => v >= 0).WithMessage("Invalid page size.");
        }
    }
}

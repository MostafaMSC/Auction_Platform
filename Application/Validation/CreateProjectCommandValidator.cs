    using FluentValidation;

namespace AuctionSystem.Application.Commands.Projects
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty().MaximumLength(1000);

        RuleFor(x => x.Location)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.EstimatedBudget)
            .Must(budget => budget != null && budget > 0)
            .WithMessage("Estimated budget must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}

}

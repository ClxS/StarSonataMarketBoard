namespace SSMB.Application.Alerts.Commands.CreateAlert
{
    using FluentValidation;

    public class CreateAlertValidator : AbstractValidator<CreateAlertCommand>
    {
        public CreateAlertValidator()
        {
            this.RuleFor(x => x.AlertName).MaximumLength(50);
            this.RuleFor(x => x.UserId).NotEqual(0);
            this.RuleFor(x => x.Conditions).NotEmpty();
        }
    }
}

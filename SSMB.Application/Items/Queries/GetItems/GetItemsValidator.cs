namespace SSMB.Application.Items.Queries.GetItems
{
    using FluentValidation;

    public class GetItemsValidator : AbstractValidator<GetItemsQuery>
    {
        public GetItemsValidator()
        {
            this.RuleFor(v => v.Count).LessThan(30);
        }
    }
}

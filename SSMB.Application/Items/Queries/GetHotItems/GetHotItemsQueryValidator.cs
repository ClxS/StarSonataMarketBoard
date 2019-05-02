namespace SSMB.Application.Items.Queries.GetHotItems
{
    using FluentValidation;

    public class GetHotItemsQueryValidator : AbstractValidator<GetHotItemsQuery>
    {
        public GetHotItemsQueryValidator()
        {
            this.RuleFor(v => v.Count).LessThan(30);
        }
    }
}

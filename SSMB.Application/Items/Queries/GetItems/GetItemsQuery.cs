namespace SSMB.Application.Items.Queries.GetItems
{
    using Domain;
    using MediatR;

    public class GetItemsQuery : IRequest<Item[]>
    {
        public int Count { get; set; } = 20;

        public string Filter { get; set; }
    }
}

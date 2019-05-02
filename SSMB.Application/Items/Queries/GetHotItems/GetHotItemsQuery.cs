namespace SSMB.Application.Items.Queries.GetHotItems
{
    using Domain;
    using MediatR;

    public class GetHotItemsQuery : IRequest<Item[]>
    {
        public int Count { get; set; } = 20;
    }
}

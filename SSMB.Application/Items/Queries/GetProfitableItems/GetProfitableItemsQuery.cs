namespace SSMB.Application.Items.Queries.GetProfitableItems
{
    using MediatR;
    using Models;

    public class GetProfitableItemsQuery : IRequest<ItemProfit[]>
    {
    }
}

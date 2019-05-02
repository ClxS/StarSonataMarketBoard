namespace SSMB.Application.Items.Queries.GetRecentItems
{
    using MediatR;
    using Models;

    public class GetRecentItemsQuery : IRequest<RecentItem[]>
    {
    }
}

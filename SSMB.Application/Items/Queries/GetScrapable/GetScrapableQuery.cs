namespace SSMB.Application.Items.Queries.GetRecentItems
{
    using MediatR;
    using Models;

    public class GetScrapableQuery : IRequest<Scrapable[]>
    {
        public double Factor { get; set; }
    }
}

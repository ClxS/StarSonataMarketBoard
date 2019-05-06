namespace SSMB.Application.Items.Queries.GetFullItemDetails
{
    using MediatR;
    using Models;

    public class GetFullItemDetailsQuery : IRequest<FullDetailItem>
    {
        public int ItemId { get; set; }
    }
}

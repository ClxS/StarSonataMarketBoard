namespace SSMB.Application.Items.Queries.GetBulkAppraisal
{
    using MediatR;
    using Models;

    public class GetBulkUnderCutQuery : IRequest<ItemRecommendedPrice[]>
    {
        public string[] Items { get; set; }
    }
}

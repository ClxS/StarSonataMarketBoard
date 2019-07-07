namespace SSMB.Application.Items.Queries.GetBulkAppraisal
{
    using MediatR;
    using Models;

    public class GetBulkAppraisalQuery : IRequest<ItemAppraisal[]>
    {
        public (string Name, int Count)[] Items { get; set; }
    }
}

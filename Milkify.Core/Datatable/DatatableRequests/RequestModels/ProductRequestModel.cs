namespace Milkify.Core.Datatable.DatatableRequests.RequestModels
{
    public class ProductRequestModel : DefaultDataTablesRequest
    {
        public long? CategoryId { get; set; }
        public string QueryString { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
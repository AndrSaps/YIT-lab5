using System;

namespace Milkify.Core.Datatable.DatatableRequests.RequestModels
{
    public class OrderRequestModel: DefaultDataTablesRequest
    {
        public long? CustomerId { get; set; }
        public long? SellerId { get; set; }
        public long? StatusId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
    
}
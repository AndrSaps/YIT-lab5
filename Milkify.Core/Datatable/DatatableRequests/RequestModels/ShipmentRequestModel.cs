using System;

namespace Milkify.Core.Datatable.DatatableRequests.RequestModels
{
    public class ShipmentRequestModel : DefaultDataTablesRequest
    {
        public long? DriverId { get; set; }
        public long? StatusId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
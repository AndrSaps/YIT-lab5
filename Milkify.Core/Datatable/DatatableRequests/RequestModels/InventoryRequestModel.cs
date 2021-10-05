using Milkify.Core.Datatable.DatatableRequests;

namespace Milkify.Core.DatatableRequests.RequestModels
{
    public class InventoryRequestModel: DefaultDataTablesRequest
    {
        public long? ProductId { get; set; }
        public long? WarehouseId { get; set; }
    }
}
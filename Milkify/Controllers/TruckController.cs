using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 12)]
    public class TruckController : BaseCrudController<Truck, TruckDto, ITruckService, DefaultDataTablesRequest>
    {
        public TruckController(ITruckService crudService) : base(crudService)
        {

        }
    }
}
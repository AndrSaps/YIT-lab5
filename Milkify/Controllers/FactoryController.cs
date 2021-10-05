using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 9)]
    public class FactoryController : BaseCrudController<Factory, FactoryDto, IFactoryService, DefaultDataTablesRequest>
    {
        public FactoryController(IFactoryService crudService) : base(crudService)
        {
        }

        protected override void PopulateNeededInfo(FactoryDto dto = null, DefaultDataTablesRequest request = null)
        {
            PopulateSellerDropDownList();
        }

        private void PopulateSellerDropDownList(object customerType = null)
        {
        }
    }
}
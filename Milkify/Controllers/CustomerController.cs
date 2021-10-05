using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 9)]
    public class CustomerController : BaseCrudController<Customer, CustomerDto, ICustomerService, DefaultDataTablesRequest>
    {
        public CustomerController(ICustomerService crudService) : base(crudService)
        {
        }

        protected override void PopulateNeededInfo(CustomerDto dto = null, DefaultDataTablesRequest request = null)
        {
            PopulateSellerDropDownList();
        }
        
        private void PopulateSellerDropDownList(object customerType = null)
        {
        }
    }
}
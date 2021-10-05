using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 5, roles:"Admin,Driver")]
    public class InventoryController : BaseCrudController<Inventory, InventoryDto, IInventoryService, InventoryRequestModel>
    {
        private readonly IProductService _productService;
        private readonly IFactoryService _factoryService;

        public InventoryController(IInventoryService crudService, IProductService productService, IFactoryService factoryService) : base(crudService)
        {
            _productService = productService;
            _factoryService = factoryService;
        }
        
        protected override void PopulateNeededInfo(InventoryDto dto, InventoryRequestModel request = null)
        {
            PopulateWarehouseDropDownList(request?.WarehouseId);
            PopulateProductDropDownList(request?.ProductId);
        }

        private void PopulateProductDropDownList(object product = null)
        {
            var productQuery = _productService.GetAllDtos();
            ViewBag.ProductId = new SelectList(productQuery, "Id", "ProductName", product);
        }

        private void PopulateWarehouseDropDownList(object factory = null)
        {
            var factoryQuery = _factoryService.GetAllDtos();
            ViewBag.FactoryId = new SelectList(factoryQuery, "Id", "Name", factory);
        }
    }
}
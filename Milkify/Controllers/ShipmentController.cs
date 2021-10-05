using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Datatable.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order:2, roles:"Admin,Driver")]
    public class
        ShipmentController : BaseCrudController<Shipment, ShipmentDto, IShipmentService, ShipmentRequestModel>
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public ShipmentController(IShipmentService crudService, IUserService userService, IOrderService orderService) : base(crudService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        public override IActionResult Index([FromQuery] ShipmentRequestModel filter)
        {
            if (string.IsNullOrEmpty(filter.SortOrder))
            {
                filter.SortOrder = "Order.OrderNumber_desc";
            }
            return base.Index(filter);
        }

        public override IActionResult Create()
        {
            long.TryParse(HttpContext.Request.Query["orderId"], out var orderId);
            PopulateNeededInfo(new ShipmentDto
            {
                OrderId = orderId
            });
            return View();
        }
        
        protected override void PopulateNeededInfo(ShipmentDto dto = null, ShipmentRequestModel request = null)
        {
            PopulateOrderDropDownList(dto?.OrderId);
        }
        

        private void PopulateOrderDropDownList(long? id = null)
        {
            var orderQuery = _orderService.GetAllDtos();
            ViewBag.OrderId = new SelectList(orderQuery, nameof(OrderDto.Id), nameof(OrderDto.OrderNumber), id);
        }
    }
}
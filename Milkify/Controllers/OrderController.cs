using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Datatable.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;
using Milkify.Core.Exceptions;
using Milkify.Core.Models;
using Milkify.Services;
using X.PagedList;

namespace Milkify.Controllers
{
    [LayoutLink(order: 4, roles:"Admin,Seller")]
    public class OrderController: BaseCrudController<Order, OrderDto, IOrderService, OrderRequestModel>
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly UserManager<User> _userManager;

        private readonly long _currentUserId;

        public OrderController(IHttpContextAccessor httpContextAccessor, 
                                IOrderService crudService, 
                                IUserService userService, 
                                ICustomerService customerService, 
                                IProductService productService, 
                                UserManager<User> userManager) : base(crudService)
        {
            _userService = userService;
            _customerService = customerService;
            _productService = productService;
            _userManager = userManager;
            long.TryParse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out _currentUserId);
        }

        public override IActionResult Index([FromQuery] OrderRequestModel filter)
        {
            if (string.IsNullOrEmpty(filter.SortOrder))
            {
                filter.SortOrder = "OrderCreated_desc";
            }
            return base.Index(filter);
        }

        public override IActionResult Create(OrderDto model)
        {
            if ((!model.SellerId.HasValue || model.SellerId == 0) && _userService.GetRoles(_currentUserId).Contains("Seller"))
            {
                model.SellerId = _currentUserId;
                ModelState.Remove("SellerId");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = base.CrudService.Create(model);
                    return RedirectToAction(nameof(Edit), new {order.Id});
                }
                catch (CreateUpdateException exception)
                {
                    ModelState.AddModelError(exception.ErrorKey, exception.Message);
                }
                
            }

            return Create();

        }

        public override IActionResult Edit(long id, OrderDto model)
        {
            try
            {
                base.Edit(id, model);
            }
            catch (QuantityException exception)
            {
                ModelState.AddModelError("Items", exception.Message);
            }

            return Edit(id);
        }

        [HttpPost]
        public IActionResult AddProductToOrder(AddProductToOrderRequestModel model)
        {
            try
            {
                CrudService.AddProductToOrder(model.OrderId, model.ProductId);
            }
            catch (CreateUpdateException exception)
            {
                ModelState.AddModelError(exception.ErrorKey, exception.Message);
            }

            return RedirectToAction(nameof(Edit), new { id = model.OrderId });
        }

        [HttpGet]
        public IActionResult MarkOrderAsPaid(long orderId, bool isShipmentNeeded)
        {
            CrudService.MarkOrderAsPaid(orderId, isShipmentNeeded);
            return RedirectToAction(nameof(Edit), new { id = orderId });
        }

        [HttpGet]
        public IActionResult RemoveProductFromOrder(long orderId, long productOrderId)
        {
            CrudService.RemoveItemFromOrder(orderId, productOrderId);
            return RedirectToAction(nameof(Edit), new { id = orderId });
        }

        protected override void PopulateNeededInfo(OrderDto order = null, OrderRequestModel request = null)
        {
            ViewBag.ProductsDatatalbe = new DatatableResult
            {
                ColumnConfiguration = new List<Column>()
                {
                    new Column() {Name = "ProductName", Label = "Name", SortOrder = "ProductName"},
                    new Column() {Name = "ProductPrice", Label = "Price", SortOrder = "ProductPrice"},
                    new Column() {Name = "ProductWeight", Label = "Weight", SortOrder = "ProductWeight"}
                },
                Data = _productService.GetAllDtos().ToPagedList(1, 10),
                DatatableActions = new List<DatatableAction>()
            };

            PopulateCustomerDropDownList(request?.CustomerId);
            PopulateSellerDropDownList(request?.SellerId);
            PopulateProductDropDownList(order);
            //PopulateStatusDropDownList((long)order?.Status);
        }

        //private void PopulateStatusDropDownList(long? id = null)
        //{
        //    var statusQuery = CrudService.GetOrderStatuses();
        //    ViewBag.StatusId = new SelectList(statusQuery, nameof(StatusDto.Id), nameof(StatusDto.StatusName), id);
        //}

        private void PopulateSellerDropDownList(long? id = null)
        {
            var userQuery = _userService.GetByRole(UserRoles.Seller.ToString());
            ViewBag.SellerId = new SelectList(userQuery, "Id", "DisplayName", id);
        }

        private void PopulateCustomerDropDownList(long? id = null)
        {
            var customerQuery = _customerService.GetAllDtos();
            ViewBag.CustomerId = new SelectList(customerQuery, "Id", "Name", id);
        }

        private void PopulateProductDropDownList(OrderDto order, object product = null)
        {
            var products = _productService.GetAllDtos();

            if (order != null)
            {
                products = products.Where(x => order.Items.All(y => y.ProductId != x.Id));
            }

            ViewBag.ProductId = new SelectList(products, "Id", nameof(ProductDto.ProductName), product);
        }

        [HttpPost]
        public IActionResult PrintInvoice(long orderId)
        {
            var stream = CrudService.GenerateInvoicePdf(orderId);
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}

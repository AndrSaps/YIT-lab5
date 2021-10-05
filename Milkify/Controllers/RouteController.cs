using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Enums;
using Milkify.Core.Exceptions;
using Milkify.Core.Models;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 10)]
    public class RouteController : BaseCrudController<Route, RouteDto, IRouteService, DefaultDataTablesRequest>
    {
        private readonly ITruckService _truckService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IShipmentService _shipmentService;

        public RouteController(IRouteService crudService, ITruckService truckService, ICustomerService customerService, IUserService userService, IShipmentService shipmentService) : base(crudService)
        {
            _truckService = truckService;
            _customerService = customerService;
            _userService = userService;
            _shipmentService = shipmentService;
        }

        protected override void PopulateNeededInfo(RouteDto dto = null, DefaultDataTablesRequest request = null)
        {
            PopulateTruckDropDownList();
            PopulateDriverDropDownList();
            PopulateShipmentDropDownList();
            base.PopulateNeededInfo(dto, request);
        }

        private void PopulateTruckDropDownList(long? id = null)
        {
            var truckQuery = _truckService.GetAllDtos();
            ViewBag.TruckId = new SelectList(truckQuery, "Id", "SerialNumber", id);
        }

        private void PopulateDriverDropDownList(long? id = null)
        {
            var userQuery = _userService.GetByRole(UserRoles.Driver.ToString());
            ViewBag.DriverId = new SelectList(userQuery, "Id", "DisplayName", id);
        }

        private void PopulateShipmentDropDownList(long? id = null)
        {
            var shipmentQuery = _shipmentService.GetAllDtos().Where(x => x.ShipmentStatus == ShipmentStatus.Placed && x.RouteId == null);
            ViewBag.ShipmentId = new SelectList(shipmentQuery, "Id", "ShipmentNumber", id);
        }

        [HttpPost]
        public IActionResult AddShipmentToRoute(AddShipmentToRouteModel model)
        {
            try
            {
                CrudService.AddShipmentToRoute(model.RouteId, model.ShipmentId);
            }
            catch (CreateUpdateException exception)
            {
                ModelState.AddModelError(exception.ErrorKey, exception.Message);
            }

            return RedirectToAction(nameof(Edit), new { id = model.RouteId });
        }

        [HttpPost]
        public IActionResult RemoveShipmentFromRoute(AddShipmentToRouteModel model)
        {
            try
            {
                CrudService.RemoveShipmentFromRoute(model.RouteId, model.ShipmentId);
            }
            catch (CreateUpdateException exception)
            {
                ModelState.AddModelError(exception.ErrorKey, exception.Message);
            }

            return RedirectToAction(nameof(Edit), new { id = model.RouteId });
        }
        
        public IActionResult StartRoute(long routeId)
        {
            CrudService.StartRoute(routeId);
            return RedirectToAction(nameof(Edit), new { id = routeId });
        }
        
        public IActionResult FinalizeRoute(long routeId)
        {
            CrudService.FinalizeRoute(routeId);
            return RedirectToAction(nameof(Edit), new { id = routeId });
        }
    }
}
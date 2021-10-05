using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Milkify.Core;
using Milkify.Core.Datatable.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IShipmentService: IBaseService<Shipment, ShipmentDto>, IDependency
    {
    }
    
    public class ShipmentService : BaseService<Shipment, ShipmentDto>, IShipmentService
    {
        private readonly IOrderService _orderService;
        private readonly IBaseRepository _repository;
        private readonly IMapper _mappingService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public ShipmentService(IMapper mappingService, IBaseRepository repository, IOrderService orderService, UserManager<User> userManager, IUserService userService) : base(mappingService, repository)
        {
            _mappingService = mappingService;
            _repository = repository;
            _orderService = orderService;
            _userManager = userManager;
            _userService = userService;
        }

        public override Shipment PrepareForInserting(Shipment entity, ShipmentDto model)
        {
            var shipment = base.PrepareForInserting(entity, model);
            shipment.ShipmentStatus = ShipmentStatus.Placed;
            var order = _orderService.GetEntity(shipment.OrderId);
            shipment.ShipmentLocation = order.Customer.Location;
            return shipment;
        }

        public override Shipment PrepareForUpdating(Shipment entity, ShipmentDto model)
        {
            var e = base.PrepareForUpdating(entity, model);
            e.Order = _repository.Get<Order>(x => x.Id == model.OrderId);
            if (model.ShippedDate.HasValue)
            {
                e.ShippedDate = model.ShippedDate;
            }
            if (model.DeliveredDate.HasValue)
            {
                e.DeliveredDate = model.DeliveredDate;
            }
            return e;
        }

        public override IEnumerable<ShipmentDto> FilterForDataTables<TDatatableRequestModel>(IEnumerable<ShipmentDto> dtos, TDatatableRequestModel requestModel,
            ClaimsPrincipal currentUser)
        {
            var userId = long.Parse(_userManager.GetUserId(currentUser));
            var roles = _userManager
                .GetRolesAsync(_userService.GetEntity(userId)).Result;
            if (roles.Contains("Admin")) { }
            else
            {
                return new List<ShipmentDto>();
            }

            var request = requestModel as ShipmentRequestModel;

            if (request != null)
            {
                //if (request.DriverId.HasValue)
                //{
                //    dtos = dtos.Where(x => x.DriverId == request.DriverId);
                //}

                //if (request.StatusId.HasValue)
                //{
                //    dtos = dtos.Where(x => x.StatusId == request.StatusId.Value);
                //}
                
                if (request.DateFrom.HasValue)
                {
                    dtos = dtos.Where(x => x.Order.OrderCreated >= request.DateFrom);
                }

                if (request.DateTo.HasValue)
                {
                    DateTime createdToDate = request.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                    dtos = dtos.Where(x => x.Order.OrderCreated <= createdToDate);
                }
            }

            return base.FilterForDataTables(dtos, requestModel, currentUser);
        }
    }
}
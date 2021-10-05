using System;
using System.Linq;
using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Enums;
using Milkify.Core.Exceptions;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IRouteService : IBaseService<Route, RouteDto>, IDependency
    {
        void AddShipmentToRoute(long routeId, long shipmentId);
        void RemoveShipmentFromRoute(long routeId, long shipmentId);
        void StartRoute(long routeId);
        void FinalizeRoute(long routeId);
    }

    public class RouteService : BaseService<Route, RouteDto>, IRouteService
    {
        private readonly IShipmentService _shipmentService;

        public RouteService(IMapper mappingService, IBaseRepository repository, IShipmentService shipmentService) : base(mappingService, repository)
        {
            _shipmentService = shipmentService;
        }

        public void AddShipmentToRoute(long routeId, long shipmentId)
        {
            var route = GetEntity(routeId);
            var shipment = _shipmentService.GetEntity(shipmentId);
            route.Shipments.Add(shipment);
            UpdateEntity(route);
        }

        public void RemoveShipmentFromRoute(long routeId, long shipmentId)
        {
            var route = GetEntity(routeId);
            var shipment = _shipmentService.GetEntity(shipmentId);
            route.Shipments.Remove(shipment);
            UpdateEntity(route);
        }

        public void StartRoute(long routeId)
        {
            var route = GetEntity(routeId);

            var currentTime = DateTime.Now;

            foreach (var shipment in route.Shipments)
            {
                shipment.ShipmentStatus = ShipmentStatus.Shipped;
                shipment.ShippedDate = currentTime;
            }

            route.RouteStarted = currentTime;

            UpdateEntity(route);
        }

        public void FinalizeRoute(long routeId)
        {
            var route = GetEntity(routeId);

            var currentTime = DateTime.Now;

            foreach (var shipment in route.Shipments)
            {
                shipment.ShipmentStatus = ShipmentStatus.Delivered;
                shipment.DeliveredDate = currentTime;
            }

            route.RouteFinished = currentTime;

            UpdateEntity(route);
        }
    }
}
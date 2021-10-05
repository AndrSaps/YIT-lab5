using AutoMapper;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;

namespace Milkify.Core
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap()
                .ForMember(user => user.UserName, expression => expression.MapFrom(dto => dto.Email));

            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<Order, OrderDto>();

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.AudioRecords, opt => opt.Ignore())
                .ForMember(dest => dest.Shipment, opt => opt.Ignore())
                .ForMember(dest => dest.Payments, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore());

            CreateMap<OrderLineItem, OrderLineItemDto>();
            CreateMap<OrderLineItemDto, OrderLineItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Shipment, ShipmentDto>();
            CreateMap<ShipmentDto, Shipment>()
                .ForMember(dest => dest.ShipmentLocation, opt => opt.Ignore());

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Payment, PaymentDto>().ReverseMap();
            
            CreateMap<Inventory, InventoryDto>().ReverseMap();

            CreateMap<Factory, FactoryDto>();

            CreateMap<FactoryDto, Factory>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());

            CreateMap<Location, Location>().ReverseMap();

            CreateMap<Route, RouteDto>();
            CreateMap<RouteDto, RouteDto>()
                .ForMember(dest => dest.Shipments, opt => opt.Ignore());

            CreateMap<AudioRecord, AudioRecordDto>().ReverseMap();

            CreateMap<Truck, TruckDto>().ReverseMap();
        }
    }
}
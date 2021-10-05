using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IOrderProductService: IBaseService<OrderLineItem, OrderLineItemDto>, IDependency
    {
        
    }

    public class OrderProductService : BaseService<OrderLineItem, OrderLineItemDto>, IOrderProductService
    {
        public OrderProductService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }
    }
}
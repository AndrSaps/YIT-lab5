using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface ITruckService : IBaseService<Truck, TruckDto>, IDependency
    {
    }

    public class TruckService : BaseService<Truck, TruckDto>, ITruckService
    {
        public TruckService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }
    }
}
using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IFactoryService : IBaseService<Factory, FactoryDto>, IDependency
    {
    }

    public class FactoryService : BaseService<Factory, FactoryDto>, IFactoryService
    {
        public FactoryService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }
    }
}
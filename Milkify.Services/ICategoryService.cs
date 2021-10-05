using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface ICategoryService : IBaseService<Category, CategoryDto>, IDependency
    {
    }

    public class CategoryService : BaseService<Category, CategoryDto>, ICategoryService
    {
        public CategoryService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }
    }
}
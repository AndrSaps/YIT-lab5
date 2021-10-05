using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 8)]
    public class CategoryController : BaseCrudController<Category, CategoryDto, ICategoryService, DefaultDataTablesRequest>
    {
        public CategoryController(ICategoryService crudService) : base(crudService)
        {

        }
    }
}
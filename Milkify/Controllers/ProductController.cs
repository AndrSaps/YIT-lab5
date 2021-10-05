using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Datatable.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 7)]
    public class ProductController : BaseCrudController<Product, ProductDto, IProductService, ProductRequestModel>
    {
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService crudService, ICategoryService categoryService) : base(crudService)
        {
            _categoryService = categoryService;
        }

        protected override void PopulateNeededInfo(ProductDto dto, ProductRequestModel request = null)
        {
            PopulateCategoryDropDownList(request?.CategoryId);
        } 

        private void PopulateCategoryDropDownList(object category = null)
        {
            var categoryQuery = _categoryService.GetAllDtos();
            ViewBag.CategoryId = new SelectList(categoryQuery, "Id", "Name", category);
        }

    }
}
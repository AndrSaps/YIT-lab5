using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Milkify.Core.Attributes;
using Milkify.Core.Dtos;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: -1, roles: "Admin")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public AnalyticsController(IAnalyticsService analyticsService, IProductService productService, ICategoryService categoryService)
        {
            _analyticsService = analyticsService;
            _productService = productService;
            _categoryService = categoryService;
        }

        private void PopulateCategoryDropDownList(object categoryId = null)
        {
            var categories = _categoryService.GetAllDtos();
            ViewBag.CategoryId = new SelectList(categories, "Id", nameof(CategoryDto.Name), categoryId);
        }

        private void PopulateProductDropDownList(object productId = null)
        {
            var products = _productService.GetAllDtos();
            ViewBag.ProductId = new SelectList(products, "Id", nameof(ProductDto.ProductName), productId);
        }

        public IActionResult Index([FromQuery] AnalyticsFilter filter)
        {
            PopulateProductDropDownList(filter.ProductId);
            PopulateCategoryDropDownList(filter.CategoryId);
            var result = _analyticsService.ProductsSoldGroupBySeller(filter.DateFrom, filter.DateTo,
                filter.CategoryId,
                filter.ProductId);

            return View(new AnalyticsRequestModel
            {
                Filter = filter,
                Data = result
            });
        }
    }
}
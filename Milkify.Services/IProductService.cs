using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Milkify.Core;
using Milkify.Core.Datatable.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IProductService : IBaseService<Product, ProductDto>, IDependency
    {
    }

    public class ProductService : BaseService<Product, ProductDto>, IProductService
    {
        public ProductService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }

        public override IEnumerable<ProductDto> FilterForDataTables<TDatatableRequestModel>(IEnumerable<ProductDto> dtos, TDatatableRequestModel requestModel,
            ClaimsPrincipal currentUser)
        {
            var request = requestModel as ProductRequestModel;

            if (request != null)
            {
                if (request.CategoryId.HasValue)
                {
                    dtos = dtos.Where(x => x.CategoryId == request.CategoryId.Value);
                }

                if (!string.IsNullOrEmpty(request.QueryString))
                {
                    dtos = dtos.Where(x => x.ProductName.Contains(request.QueryString));
                }

                if (request.MinPrice.HasValue)
                {
                    dtos = dtos.Where(x => x.Price >= request.MinPrice);
                }

                if (request.MaxPrice.HasValue)
                {
                    dtos = dtos.Where(x => x.Price <= request.MaxPrice);
                }
            }

            return dtos;
        }
    }
}
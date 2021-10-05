using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Milkify.Core;
using Milkify.Core.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Exceptions;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IInventoryService : IBaseService<Inventory, InventoryDto>, IDependency
    {
    }

    public class InventoryService : BaseService<Inventory, InventoryDto>, IInventoryService
    {
        public InventoryService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }

        public override IEnumerable<InventoryDto> FilterForDataTables<TDatatableRequestModel>(IEnumerable<InventoryDto> dtos, TDatatableRequestModel requestModel,
            ClaimsPrincipal currentUser)
        {
            var request = requestModel as InventoryRequestModel;

            if (request != null)
            {
                if (request.ProductId.HasValue)
                {
                    dtos = dtos.Where(x => x.ProductId == request.ProductId.Value);
                }

                if (request.WarehouseId.HasValue)
                {
                    dtos = dtos.Where(x => x.FactoryId == request.WarehouseId.Value);
                }
            }

            return dtos;
        }
    }
}
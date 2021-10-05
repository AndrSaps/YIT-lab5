using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IPaymentService: IBaseService<Payment, PaymentDto>, IDependency
    {
        
    }

    public class PaymentService : BaseService<Payment, PaymentDto>, IPaymentService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public PaymentService(IMapper mappingService, IBaseRepository repository, UserManager<User> userManager, IUserService userService) : base(mappingService, repository)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public override IEnumerable<PaymentDto> FilterForDataTables<TDatatableRequestModel>(IEnumerable<PaymentDto> dtos, TDatatableRequestModel requestModel,
            ClaimsPrincipal currentUser)
        {
            var userId = long.Parse(_userManager.GetUserId(currentUser));
            var roles = _userManager
                .GetRolesAsync(_userService.GetEntity(userId)).Result;
            if (roles.Contains("Admin")) { }
            else if (roles.Contains("Seller"))
            {
                dtos = dtos.Where(x => x.Order.SellerId == userId);
            }
            return dtos;
        }
    }
}
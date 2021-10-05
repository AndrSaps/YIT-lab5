using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities.Membership;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IUserRoleService : IBaseService<UserRole, UserRoleDto>, IDependency
    {
    }

    public class UserRoleService : BaseService<UserRole, UserRoleDto>, IUserRoleService
    {
        public UserRoleService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }
    }
}
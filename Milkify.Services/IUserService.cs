using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface IUserService : IBaseService<User, UserDto>, IDependency
    {
        IEnumerable<UserDto> GetByRole(string role);

        IEnumerable<string> GetRoles(long userId);

        long GetId(ClaimsPrincipal userClaims);
    }

    public class UserService : BaseService<User, UserDto>, IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mappingService, IBaseRepository repository, UserManager<User> userManager) : base(mappingService, repository)
        {
            _userManager = userManager;
        }

        public override UserDto ToDto(User entity)
        {
            var dto = base.ToDto(entity);
            var getRolesAsync = _userManager.GetRolesAsync(entity);
            dto.UserRole = string.Join(", ", getRolesAsync.Result);
            return dto;
        }

        public override IEnumerable<UserDto> GetAllDtos()
        {
            return base.GetAllDtos().Where(x => x.UserRole == UserRoles.Seller.ToString() || x.UserRole == UserRoles.Driver.ToString());
        }

        public override User Create(UserDto model)
        {
            var entity = Activator.CreateInstance<User>();

            MappingService.Map(model, entity);
            var createAdminUser = _userManager.CreateAsync(entity, model.NewPassword);
            if (createAdminUser.Result.Succeeded)
            {
                _userManager.AddToRoleAsync(entity, model.UserRole).Wait();
            }
            return entity;
        }

        public override User Update(UserDto model, long id)
        {
            var entity = GetEntity(id);
            model.Email = entity.Email;
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                _userManager.ChangePasswordAsync(entity, model.CurrentPassword, model.NewPassword).Wait();
            }
            MappingService.Map(model, entity);
            _userManager.UpdateAsync(entity).Wait();
            var roles = _userManager.GetRolesAsync(entity).Result;
            _userManager.RemoveFromRolesAsync(entity, roles).Wait();
            _userManager.AddToRoleAsync(entity, model.UserRole).Wait();
            return entity;
        }

        public override IEnumerable<UserDto> ToDtos(IEnumerable<User> entities, bool wireup = true)
        {
            var enumerable = entities.ToList();
            var dtos = base.ToDtos(enumerable, wireup);
            var userDtos = dtos.ToList();
            foreach (var dto in userDtos)
            {
                var getRolesAsync = _userManager.GetRolesAsync(enumerable.FirstOrDefault(x => x.Id == dto.Id));
                dto.UserRole = string.Join(", ", getRolesAsync.Result);
            }
            return userDtos;
        }

        public IEnumerable<UserDto> GetByRole(string role)
        {
            return MappingService.Map<IEnumerable<UserDto>>(_userManager.GetUsersInRoleAsync(role).Result);
        }

        public IEnumerable<string> GetRoles(long userId)
        {
            return _userManager.GetRolesAsync(GetEntity(userId)).Result;
        }

        public long GetId(ClaimsPrincipal userClaims)
        {
            return long.Parse(_userManager.GetUserId(userClaims));
        }
    }
}
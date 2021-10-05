using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Reflection;
using System.Security.Claims;
using System.Transactions;
using AutoMapper;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Repository;
using X.PagedList;

namespace Milkify.Core
{
    public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
        where TEntity : class, new()
        where TDto : class
    {
        protected readonly IMapper MappingService;
        protected readonly IBaseRepository Repository;

        protected BaseService(IMapper mappingService, IBaseRepository repository)
        {
            MappingService = mappingService;
            Repository = repository;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Repository.Query<TEntity>();
        }

        public virtual IEnumerable<TDto> GetAllDtos()
        {
            return ToDtos(GetAll());
            //return MappingService.Map<IEnumerable<TDto>>();
        }

        public virtual IEnumerable<Column> GetColumnsForTable(ClaimsPrincipal currentUser)
        {
            Dictionary<string, Tuple<string, int>> dict = new Dictionary<string, Tuple<string, int>>();

            PropertyInfo[] props = typeof(TDto).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                IEnumerable<TableColumn> tableColumns = prop.GetCustomAttributes(true).OfType<TableColumn>();
                List<RequireUserRoles> requireUserRoleses =
                    prop.GetCustomAttributes(true).OfType<RequireUserRoles>().ToList();
                foreach (TableColumn tableColumn in tableColumns)
                {
                    string propName = prop.Name;
                    string auth = tableColumn.Label;
                    int order = tableColumn.Order;
                    var permissionConfirmed = true;
                    if (requireUserRoleses.Any())
                    {
                        foreach (var requireUserRoles in requireUserRoleses)
                        {
                            var roles = requireUserRoles.UserRoles.Split(',').Select(x => x.Trim());
                            permissionConfirmed = roles.Any(currentUser.IsInRole);
                        }
                    }

                    if (permissionConfirmed)
                    {
                        dict.Add(propName, new Tuple<string, int>(auth, order));
                    }
                }
            }

            return dict.OrderBy(x => x.Value.Item2).Select(x => new Column()
                {Name = x.Key, Label = x.Value.Item1, SortOrder = x.Key});
        }

        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Query<TEntity>(expression);
        }

        public IEnumerable<TDto> FetchDto(Expression<Func<TEntity, bool>> expression)
        {
            return MappingService.Map<IEnumerable<TDto>>(Repository.Query(expression).ToArray());
        }

        public virtual DatatableResult GetDataTableResponse(DefaultDataTablesRequest requestModel,
            ClaimsPrincipal currentUser)
        {
            var dto = GetAllDtos();
            dto = FilterForDataTables(dto, requestModel, currentUser);
            var columns = GetColumnsForTable(currentUser).ToList();
            var actions = GetActionsForTable(currentUser);
            dto = SortForDatatable(dto, requestModel, columns);

            return new DatatableResult()
            {
                ColumnConfiguration = columns,
                Data = dto.ToPagedList(requestModel.Page ?? 1, requestModel.Length ?? 10),
                Filter = requestModel,
                DatatableActions = actions
            };
        }

        public virtual IEnumerable<TDto> FilterForDataTables<TDatatableRequestModel>(IEnumerable<TDto> dtos,
            TDatatableRequestModel requestModel, ClaimsPrincipal currentUser) where TDatatableRequestModel : DefaultDataTablesRequest
        {
            return dtos;
        }

        public virtual IEnumerable<TDto> SortForDatatable<TDatatableRequestModel>(IEnumerable<TDto> dtos,
            TDatatableRequestModel requestModel, IEnumerable<Column> columns)
            where TDatatableRequestModel : DefaultDataTablesRequest
        {
            if (requestModel != null && !string.IsNullOrEmpty(requestModel.SortOrder))
            {
                var sortColumn = columns.FirstOrDefault(x => requestModel.SortOrder.Split('_').Contains(x.SortOrder));
                if (sortColumn != null)
                {
                    var attribute = typeof(TDto).GetProperty(sortColumn.Name).GetCustomAttribute<TableColumn>();
                    if (requestModel.SortOrder.Contains("desc"))
                    {
                        dtos = dtos.AsQueryable()
                            .OrderBy((!string.IsNullOrEmpty(attribute?.Path)
                                         ? sortColumn.Name + "." + attribute.Path
                                         : sortColumn.SortOrder) + " desc");
                    }
                    else
                    {
                        dtos = dtos.AsQueryable()
                            .OrderBy((!string.IsNullOrEmpty(attribute?.Path)
                                         ? sortColumn.Name + "." + attribute.Path
                                         : sortColumn.SortOrder) + " asc");
                        sortColumn.SortOrder += "_desc";
                    }
                }
            }

            return dtos;
        }

        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Get(predicate);
        }

        public virtual TEntity GetEntity(long id)
        {
            return Repository.Get<TEntity>(id);
        }

        public virtual TDto GetDto(long id)
        {
            var entity = GetEntity(id);
            return ToDto(entity);
        }

        public virtual TEntity Create(TDto model)
        {
            var entity = PrepareForInserting(default(TEntity), model);

            Repository.Insert(entity);
            Repository.SaveChanges();

            return entity;
        }

        public virtual TEntity PrepareForInserting(TEntity entity, TDto model)
        {
            if (entity == null)
            {
                entity = Activator.CreateInstance<TEntity>();
            }

            MappingService.Map(model, entity);
            return entity;
        }

        public virtual TEntity Update(TDto model, long id)
        {
            var entity = GetEntity(id);
            entity = PrepareForUpdating(entity, model);
            Repository.Update(entity);
            Repository.SaveChanges();
            return entity;
        }

        public virtual TEntity UpdateEntity(TEntity entity)
        {
            Repository.Update(entity);
            Repository.SaveChanges();

            return entity;
        }

        public virtual TEntity PrepareForUpdating(TEntity entity, TDto model)
        {
            MappingService.Map(model, entity);
            return entity;
        }

        public virtual void Delete(long key)
        {
            var entity = GetEntity(key);

            Repository.Remove(entity);
            Repository.SaveChanges();
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Any(expression);
        }

        public bool All(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.All(expression);
        }

        public virtual IEnumerable<TDto> ToDtos(IEnumerable<TEntity> entities, bool wireup = true)
        {
            return MappingService.Map<IEnumerable<TDto>>(entities);
        }

        public virtual IQueryable<TDto> ToQueryableDtos(IQueryable<TEntity> entities, bool wireup = true)
        {
            var enumerable = MappingService.Map<IEnumerable<TDto>>(entities);
            return enumerable.AsQueryable();
        }

        public virtual TDto ToDto(TEntity entity)
        {
            return MappingService.Map<TDto>(entity);
        }

        public IList<DatatableAction> GetActionsForTable(ClaimsPrincipal currentUser)
        {
            var method = typeof(TDto).GetMethod("GetDtoActions");
            var instance = (TDto) Activator.CreateInstance(typeof(TDto));
            var actions = (IList<DatatableAction>) method?.Invoke(instance, new object[] {currentUser}) ??
                          new List<DatatableAction>();
            return actions;
        }
    }
}
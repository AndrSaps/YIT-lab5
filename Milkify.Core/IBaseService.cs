using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Milkify.Core.Datatable;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;

namespace Milkify.Core
{
    public interface IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        IQueryable<TEntity> GetAll();

        IEnumerable<TDto> GetAllDtos();
        
        IEnumerable<Column> GetColumnsForTable(ClaimsPrincipal currentUser);

        IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TDto> FetchDto(Expression<Func<TEntity, bool>> expression);

        DatatableResult GetDataTableResponse(DefaultDataTablesRequest requestModel, ClaimsPrincipal currentUser);

        TEntity First(Expression<Func<TEntity, bool>> predicate);

        TEntity GetEntity(long id);

        TDto GetDto(long id);

        TEntity Create(TDto model);

        TEntity Update(TDto model, long keys);

        TEntity UpdateEntity(TEntity entity);

        void Delete(long key);

        bool Any(Expression<Func<TEntity, bool>> expression);

        bool All(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TDto> ToDtos(IEnumerable<TEntity> entities, bool wireup = true);

        IQueryable<TDto> ToQueryableDtos(IQueryable<TEntity> entities, bool wireup = true);

        TDto ToDto(TEntity entity);

        IList<DatatableAction> GetActionsForTable(ClaimsPrincipal currentUser);
    }
}
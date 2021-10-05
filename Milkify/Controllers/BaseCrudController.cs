using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milkify.Core;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Exceptions;
using X.PagedList;
using DefaultDataTablesRequest = Milkify.Core.Datatable.DatatableRequests.DefaultDataTablesRequest;

namespace Milkify.Controllers
{
    [Authorize]
    public abstract class BaseCrudController<TEntity, TDto, TService, TDatatableRequest> : Controller
        where TService : IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
        where TDatatableRequest : DefaultDataTablesRequest
    {
        protected readonly TService CrudService;

        protected BaseCrudController(TService crudService)
        {
            CrudService = crudService;
        }

        public virtual IActionResult Index([FromQuery]TDatatableRequest filter)
        {
            EntityPermissionCheck(null);
            PopulateNeededInfo(request: filter);
            return View(CrudService.GetDataTableResponse(filter, this.User));
        }

        public virtual IActionResult GetById(long id)
        {
            return View(CrudService.GetDto(id));
        }

        [HttpGet]
        public virtual IActionResult Create()
        {
            PopulateNeededInfo();
            return View();
        }

        [HttpPost]
        public virtual IActionResult Create(TDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CrudService.Create(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (CreateUpdateException exception)
                {
                    ModelState.AddModelError(exception.ErrorKey, exception.Message);
                }
            }

            return Create();
        }

        [HttpGet]
        public virtual IActionResult Edit(long id)
        {
            EntityPermissionCheck(id);
            var dto = CrudService.GetDto(id);
            if (dto == null)
            {
                return View("NotFound");
            }
            PopulateNeededInfo(dto);
            return View(dto);
        }

        [HttpPost]
        public virtual IActionResult Edit(long id, TDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EntityPermissionCheck(id);
                    CrudService.Update(model, id);
                    return RedirectToAction(nameof(Edit), new { id });
                }
                catch (CreateUpdateException exception)
                {
                    ModelState.AddModelError(exception.ErrorKey, exception.Message);
                }
              
               
            }
            return View(model);

        }

        public virtual IActionResult Delete(long id)
        {
            EntityPermissionCheck(id);
            CrudService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check for user authorization of managing specific entity
        /// </summary>
        /// <param name="entityId">[Optional]Identifier of the entity</param>
        /// <exception cref="UnauthorizedAccessException">Throw exception if user is not authorized to perform some actiton</exception>
        protected virtual void EntityPermissionCheck(long? entityId)
        {

        }

        protected virtual void PopulateNeededInfo(TDto dto = null, TDatatableRequest request = null)
        {

        }
    }
}

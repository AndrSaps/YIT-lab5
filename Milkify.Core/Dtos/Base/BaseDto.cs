using System;
using System.Collections.Generic;
using System.Security.Claims;
using Milkify.Core.Datatable;

namespace Milkify.Core.Dtos
{
    public class BaseDto
    {
        public virtual IList<DatatableAction> GetDtoActions(ClaimsPrincipal currentUser)
        {
            return new List<DatatableAction>();
        }
    }
}

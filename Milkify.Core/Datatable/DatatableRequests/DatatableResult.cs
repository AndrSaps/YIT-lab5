using System.Collections.Generic;
using X.PagedList;

namespace Milkify.Core.Datatable.DatatableRequests
{
    public class DatatableResult
    {
        public IEnumerable<Column> ColumnConfiguration { get; set; }
        public IPagedList<object> Data { get; set; }
        public DefaultDataTablesRequest Filter { get; set; }
        public IList<DatatableAction> DatatableActions { get; set; }
    }
}
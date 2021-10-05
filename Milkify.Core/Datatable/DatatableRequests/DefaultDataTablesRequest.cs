namespace Milkify.Core.Datatable.DatatableRequests
{
    public class DefaultDataTablesRequest
    {
        public int? Page { get; set; }
        public int? Length { get; set; }
        public string SortOrder { get; set; }
    }
}
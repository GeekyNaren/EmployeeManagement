namespace EmployeeManagement.ExtensionService
{
    public class PagedResponse<T>
    {
        public IReadOnlyList<T> Data { get; init; } = System.Array.Empty<T>();
        public int PageNumber { get; init; }
        public int? PageSize { get; init; }
        public int TotalRecords { get; init; }
        public int TotalPages { get; init; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }
}

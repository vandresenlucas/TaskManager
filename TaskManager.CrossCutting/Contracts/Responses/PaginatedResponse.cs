using TaskManager.CrossCutting.Contracts.Bases;

namespace TaskManager.CrossCutting.Contracts.Responses
{
    public class PaginatedResponse<T> : PaginationBase
    {
        public PaginatedResponse(IEnumerable<T> items, int totalCount, int pageSize, int currentPage)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
        }

        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}

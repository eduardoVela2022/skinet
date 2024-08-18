// File path
namespace API.RequestHelpers;

// Creates a generic pagination class that can be used with different entities
public class Pagination<T>(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
{
    // The page index
    public int PageIndex { get; set; } = pageIndex;

    // The page size
    public int PageSize { get; set; } = pageSize;

    // Count of available entities
    public int Count { get; set; } = count;

    // The retrived data
    public IReadOnlyList<T> Data { get; set; } = data;
}

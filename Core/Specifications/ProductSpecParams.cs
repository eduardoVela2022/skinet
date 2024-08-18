namespace Core.Specifications;

// Allows the user to filter by multiple brands and types, and adds more sorting options
public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    // Page size spec
    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    // Brands spec
    private List<string> _brands = [];

    public List<string> Brands
    {
        get { return _brands; }
        set
        {
            // Creates the list of brands
            _brands = value
                .SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }

    // Types spec
    private List<string> _types = [];

    public List<string> Types
    {
        get { return _types; }
        set
        {
            // Creates the list of types
            _types = value
                .SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }

    // Sort spec
    public string? Sort { get; set; }

    // Search spec
    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
}

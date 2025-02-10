namespace Core.Specification;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private List<string> _brands = []; // الحقل الخاص لتخزين العلامات التجارية

    public List<string> Brands
    {
        get => _brands;
        set
        {
            // تقسيم العناصر باستخدام الفاصلة (,) وحذف العناصر الفارغة ثم تخزينها في _brands
            _brands = value
                .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }

    // مثال:
    // إدخال: ["Nike,Adidas", "Puma"]
    // النتيجة: ["Nike", "Adidas", "Puma"]


    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        set
        {
            _types = value
                .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
}

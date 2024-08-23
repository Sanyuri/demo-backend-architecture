namespace DemoBackendArchitecture.Application.Helpers;

public class Pagination<T>
{
    public int TotalItemsCount { get; set; }
    public int PageSize { get; set; }

    public int TotalPagesCount
    {
        get
        {
            var temp = TotalItemsCount / PageSize;
            return TotalItemsCount % PageSize == 0 ? temp : temp + 1;
        }
    }
    
    public int PageIndex { get; set; }
    
    //page number start to 1
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPagesCount;
    public ICollection<T>? Items { get; set; }
}


using Structure_Core.TransactionManagement;

namespace Structure_Core.Extensions;
public static class OrderExtension
{
    public static IQueryable<Order> Sort(this IQueryable<Order> query,string? orderBy)
    {
        if(string.IsNullOrWhiteSpace(orderBy))
            return query.OrderBy(o => o.ID);
        string lowerOrderBy = orderBy.ToLower();
        query = lowerOrderBy switch
        {
            "ordernumberasc" => query.OrderBy(x => x.OrderNumber),
            "ordernumberdesc" => query.OrderByDescending(x => x.OrderNumber),
            "customeridasc" => query.OrderBy(x => x.CustomerID),
            "customeriddesc" => query.OrderByDescending(x => x.CustomerID),
            "orderdateasc" => query.OrderBy(x => x.OrderDate),
            "orderdatedesc" => query.OrderByDescending(x => x.OrderDate),
            "totalamountasc" => query.OrderBy(x => x.TotalAmount),
            "totalamountdesc" => query.OrderByDescending(x => x.TotalAmount),
            _ => query.OrderBy(x => x.CreatedDate)
        };
        return query;
    }
    public static IQueryable<Order> Search(this IQueryable<Order> query, string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return query;
        string lowerKeyword = keyword.ToLower();
        return query.Where(o => o.OrderNumber.ToLower().Contains(lowerKeyword) ||
                                o.CustomerID.ToLower().Contains(lowerKeyword));
    }
    public static IQueryable<Order> FilterByDateRange(this IQueryable<Order> query, DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            return query.Where(x => x.OrderDate >= startDate.Value && x.OrderDate <= endDate.Value);
        }
        else if (startDate.HasValue)
        {
            return query.Where(x => x.OrderDate >= startDate.Value);
        }
        else if (endDate.HasValue)
        {
            return query.Where(x => x.OrderDate <= endDate.Value);
        }
        return query; 
    }
}

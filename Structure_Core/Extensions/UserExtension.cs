using Structure_Core.User;

namespace Structure_Core.Extensions;
public static class UserExtension
{
    public static IQueryable<Structure_Core.User.User> Sort(this IQueryable<Structure_Core.User.User> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderBy(u => u.ID); // default sort
                                             // Convert to lowercase for case-insensitive comparison
        string lowerOrderBy = orderBy.ToLower();

        // Explicit sorting with "asc" and "desc" suffixes
        query = lowerOrderBy switch
        {
            // FirstName sorting
            "firstnameasc" => query.OrderBy(x => x.FirstName),
            "firstnamedesc" => query.OrderByDescending(x => x.FirstName),

            // LastName sorting
            "lastnameasc" => query.OrderBy(x => x.LastName),
            "lastnamedesc" => query.OrderByDescending(x => x.LastName),

            // Email sorting
            "emailasc" => query.OrderBy(x => x.Email),
            "emaildesc" => query.OrderByDescending(x => x.Email),

            // CreatedDate sorting
            "createddateasc" => query.OrderBy(x => x.CreatedDate),
            "createddatedesc" => query.OrderByDescending(x => x.CreatedDate),

            // UpdatedDate sorting
            "updateddateasc" => query.OrderBy(x => x.UpdatedDate),
            "updateddatedesc" => query.OrderByDescending(x => x.UpdatedDate),

            // Default to CreatedDate ascending
            _ => query.OrderBy(x => x.CreatedDate)
        };

        return query;
    }
    public static IQueryable<Structure_Core.User.User> Search(this IQueryable<Structure_Core.User.User> query, string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return query;
        // Convert to lowercase for case-insensitive comparison
        string lowerKeyword = keyword.ToLower();
        return query.Where(u => u.FirstName.ToLower().Contains(lowerKeyword) ||
                                u.LastName.ToLower().Contains(lowerKeyword) ||
                                u.Email.ToLower().Contains(lowerKeyword));
    }
    public static IQueryable<Structure_Core.User.User> FilterByDateRange(this IQueryable<Structure_Core.User.User> query, DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            return query.Where(x => x.CreatedDate >= startDate.Value && x.CreatedDate <= endDate.Value);
        }
        else if (startDate.HasValue)
        {
            return query.Where(x => x.CreatedDate >= startDate.Value);
        }
        else if (endDate.HasValue)
        {
            return query.Where(x => x.CreatedDate <= endDate.Value);
        }
        return query;
    }
}

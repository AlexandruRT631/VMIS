namespace user_backend.Utils;

public static class QueryUtilities
{
    public static IQueryable<T> Paginate<T>(IQueryable<T> query, int pageIndex, int pageSize)
    {
        return query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
    }
}
using System.Linq.Expressions;
using System.Text;

namespace listing_backend.Utils;

public static class QueryUtilities
{
    public static IQueryable<T> Paginate<T>(IQueryable<T> query, int pageIndex, int pageSize)
    {
        return query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<T> AddEqual<T, TValue>(IQueryable<T> query, Expression<Func<T, TValue>> expression, TValue value)
    {
        var parameter = expression.Parameters[0];
        var body = Expression.Equal(expression.Body, Expression.Constant(value, typeof(TValue)));
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> AddGreaterOrEqual<T>(IQueryable<T> query, Expression<Func<T, int>> expression, int value)
    {
        var parameter = expression.Parameters[0];
        var body = Expression.GreaterThanOrEqual(expression.Body, Expression.Constant(value));
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> AddLessOrEqual<T>(IQueryable<T> query, Expression<Func<T, int>> expression, int value)
    {
        var parameter = expression.Parameters[0];
        var body = Expression.LessThanOrEqual(expression.Body, Expression.Constant(value));
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> AddContains<T>(IQueryable<T> query, Expression<Func<T, string>> expression, string value)
    {
        var parameter = expression.Parameters[0];
        var body = Expression.Call(expression.Body, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, Expression.Constant(value));
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> AddContains<T, TValue>(IQueryable<T> query, Expression<Func<T, List<TValue>>> expression, TValue value)
    {
        var parameter = expression.Parameters[0];
        var body = Expression.Call(expression.Body, typeof(List<TValue>).GetMethod("Contains")!, Expression.Constant(value, typeof(TValue)));
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> AddIn<T>(IQueryable<T> query, Expression<Func<T, int>> expression, List<int> values)
    {
        var parameter = expression.Parameters[0];
        var body = Expression.Call(Expression.Constant(values), typeof(List<int>).GetMethod("Contains")!, expression.Body);
        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }
}
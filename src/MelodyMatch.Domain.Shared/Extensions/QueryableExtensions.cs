using System;
using System.Linq;
using System.Linq.Expressions;

namespace MelodyMatch.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> FilterBy<TEntity>(
        this IQueryable<TEntity> source,
        bool valueExists,
        Expression<Func<TEntity, bool>>? expression)
        where TEntity : class
    {
        return valueExists && expression != null ? source.Where(expression) : source;
    }
}
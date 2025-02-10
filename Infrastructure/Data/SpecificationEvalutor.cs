using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

// Class to evaluate specifications and apply them to database queries
public class SpecificationEvaluator<T>
    where T : BaseEntity
{
    // Method that applies specification criteria to the query and returns an updated query
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        // Check if there is a filter condition (Criteria) in the specification
        if (spec.Criteria != null)
        {
            // If a condition exists, apply it to the query (e.g., x => x.Brand == brand)
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        // Return the query after applying the specification (or as-is if no conditions)
        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(
        IQueryable<T> query,
        ISpecification<T, TResult> spec
    )
    {
        // Check if there is a filter condition (Criteria) in the specification
        if (spec.Criteria != null)
        {
            // If a condition exists, apply it to the query (e.g., x => x.Brand == brand)
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        var selectQuery = query as IQueryable<TResult>;
        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);
        }

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        if (spec.IsPagingEnabled)
        {
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        }

        // Return the query after applying the specification (or as-is if no conditions)
        return selectQuery ?? query.Cast<TResult>();
    }
}

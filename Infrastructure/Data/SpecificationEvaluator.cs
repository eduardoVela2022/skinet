// Imports
using Core.Entities;
using Core.Interfaces;

// File path
namespace Infrastructure.Data;

// This evaluates the specification and creates a query based on it
public class SpecificationEvaluator<T> where T :BaseEntity
{
    // Builds query
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        // Where query
        if(spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // Criteria is the filter condition
        }
        // Sort by ascending
        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy); // OrderBy is the sort condition
        }
        // Sort by descending
        if(spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending); // OrderByDescending is the sort condition
        }
        // To get distinct values
        if(spec.IsDistinct)
        {
            query = query.Distinct();
        }
        
        // Returns query
        return query;
    }

    // Builds query that can select and return something different from T
    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, 
    ISpecification<T, TResult> spec)
    {
        // Where query
        if(spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // Criteria is the filter condition
        }
        // Sort by ascending
        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy); // OrderBy is the sort condition
        }
        // Sort by descending
        if(spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending); // OrderByDescending is the sort condition
        }
        
        // Query to return something different from T
        var selectQuery = query as IQueryable<TResult>;
        
        // Builds select query
        if(spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);
        }

        // To get distinct values
        if(spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        // Returns query
        return selectQuery ?? query.Cast<TResult>();
    }
}

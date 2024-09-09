// File path
using System.Linq.Expressions;

namespace Core.Interfaces;

// Used to the specify queries of a generic repository
public interface ISpecification<T>
{
    // Where query
    Expression<Func<T, bool>>? Criteria { get; }

    // Order query ascending
    Expression<Func<T, object>>? OrderBy { get; }

    // Order query desending
    Expression<Func<T, object>>? OrderByDescending { get; }

    // Include related entities
    List<Expression<Func<T, object>>> Includes { get; }

    List<string> IncludeStrings { get; } // For ThenInclude for related entities of related entities

    // To get distinc values
    bool IsDistinct { get; }

    // How many items the pagination will show
    int Take { get; }

    // How many will it skip
    int Skip { get; }

    // Is pagination enabled?
    bool IsPagingEnabled { get; }

    // Applies criteria
    IQueryable<T> ApplyCriteria(IQueryable<T> query);
}

// Used to specify queries that return something different from T
public interface ISpecification<T, TResult> : ISpecification<T>
{
    // Selects something that is not T (brands or types)
    Expression<Func<T, TResult>>? Select { get; }
}

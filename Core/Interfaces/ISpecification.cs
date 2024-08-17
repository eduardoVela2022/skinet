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

    // To get distinc values
    bool IsDistinct { get; }
}

// Used to specify queries that return something different from T
public interface ISpecification<T, TResult> : ISpecification<T>
{
    // Selects something that is not T (brands or types)
    Expression<Func<T, TResult>>? Select { get; }
}

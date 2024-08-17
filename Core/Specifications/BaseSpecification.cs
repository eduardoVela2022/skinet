// Imports
using System.Linq.Expressions;
using Core.Interfaces;

// File path
namespace Core.Specifications;

// This class implements the methods of the specification interface
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    // Empty constructor
    protected BaseSpecification() : this(null) {}

    // Where query
    public Expression<Func<T, bool>>? Criteria => criteria;

    // Order by ascending
    public Expression<Func<T, object>>? OrderBy { get; private set;}

    // Order by descending
    public Expression<Func<T, object>>? OrderByDescending { get; private set;}

    // To get distinc values
    public bool IsDistinct { get; private set; }

    // Set order by ascending
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    // Set order by descending
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    // Set to get distinct values
    protected void ApplyDistinct() 
    {
        IsDistinct = true;
    }
}

// This class implements the methods of the specification interface that returns somthing different from T
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
    : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
     // Empty constructor
    protected BaseSpecification() : this(null) {}

    // Selects something that is not T (brands or types)
    public Expression<Func<T, TResult>>? Select { get; private set; }

    // Set what you want to select
    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}

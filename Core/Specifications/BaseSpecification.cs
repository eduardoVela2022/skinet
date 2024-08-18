// Imports
using System.Linq.Expressions;
using Core.Interfaces;

// File path
namespace Core.Specifications;

// This class implements the methods of the specification interface
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    // Empty constructor
    protected BaseSpecification()
        : this(null) { }

    // Where query
    public Expression<Func<T, bool>>? Criteria => criteria;

    // Order by ascending
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    // Order by descending
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    // To get distinc values
    public bool IsDistinct { get; private set; }

    // How many items the pagination will show
    public int Take { get; private set; }

    // How many will it skip
    public int Skip { get; private set; }

    // Is pagination enabled?
    public bool IsPagingEnabled { get; private set; }

    // Returns a query with the applied criteria
    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if (Criteria != null)
        {
            query = query.Where(Criteria);
        }

        return query;
    }

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

    // Sets the pagination
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}

// This class implements the methods of the specification interface that returns somthing different from T
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
    : BaseSpecification<T>(criteria),
        ISpecification<T, TResult>
{
    // Empty constructor
    protected BaseSpecification()
        : this(null) { }

    // Selects something that is not T (brands or types)
    public Expression<Func<T, TResult>>? Select { get; private set; }

    // Set what you want to select
    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}

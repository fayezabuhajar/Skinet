using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specification;

// Base implementation of the ISpecification interface for applying filtering logic
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    protected BaseSpecification() : this(null) {}


    // Stores the criteria expression passed in the constructor
    public Expression<Func<T, bool>>? Criteria => criteria;

    public Expression<Func<T,object>>? OrderBy {get; private set;}

    public Expression<Func<T,object>>? OrderByDescending {get; private set;}

    public bool IsDistinct {get; private set;}

    protected void AddOrderBy(Expression<Func<T,object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T,object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplayDistinct()
    {
        IsDistinct = true;
    }


}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria) :
    BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null!) {}
    public Expression<Func<T, TResult>>? Select {get; private set;}

    protected void AddSelect(Expression<Func<T,TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}

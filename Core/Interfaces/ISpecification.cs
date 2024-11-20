using System.Linq.Expressions;

namespace Core.Interfaces;

// Interface defining the specification pattern for filtering data of type T
public interface ISpecification<T>
{
    // Property to define filter criteria as an expression
    // Example usage: x => x.Brand == "BrandName"
    Expression<Func<T, bool>>? Criteria { get; }

    Expression<Func<T,object>>? OrderBy {get;}

    Expression<Func<T,object>>? OrderByDescending {get;}

    bool IsDistinct {get;}

    int Take {get;}
    int Skip {get;}

    bool IsPagingEnabled {get;}

    IQueryable<T> ApplyCriteria(IQueryable<T> query);

}

public interface ISpecification<T,TResult> : ISpecification<T>
{
    Expression<Func<T,TResult>>? Select {get;}
}

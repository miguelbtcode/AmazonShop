using System.Linq.Expressions;

namespace Ecommerce.Application.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification() { }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; }

    public List<Expression<Func<T, object>>> Includes => new();

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnable { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression; 
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByExpression)
    {
        OrderByDescending = orderByExpression; 
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnable = true;
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}
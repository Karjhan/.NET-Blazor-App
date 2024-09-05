using Domain.Abstractions;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Evaluator;

public class SpecificationEvaluator<TEntity> where TEntity : Entity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        return query;
    }
}
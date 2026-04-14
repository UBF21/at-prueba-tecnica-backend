using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Evaluators;
using Vali_Flow.Interfaces.Specification;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for Order entity operations.
/// Encapsulates ValiFlowEvaluator to provide read/write operations with Vali-Flow specifications.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ValiFlowEvaluator<Order> _evaluator;

    public OrderRepository(AppDbContext dbContext)
    {
        _evaluator = new ValiFlowEvaluator<Order>(dbContext);
    }

    #region IEvaluatorRead<Order> implementation

    public Task<bool> EvaluateAsync(Vali_Flow.Core.Builder.ValiFlow<Order> valiFlow, Order entity)
        => _evaluator.EvaluateAsync(valiFlow, entity);

    public Task<bool> EvaluateAnyAsync(IBasicSpecification<Order> specification, CancellationToken ct = default)
        => _evaluator.EvaluateAnyAsync(specification, ct);

    public Task<int> EvaluateCountAsync(IBasicSpecification<Order> specification, CancellationToken ct = default)
        => _evaluator.EvaluateCountAsync(specification, ct);

    public Task<Order?> EvaluateGetFirstAsync(IBasicSpecification<Order> specification, CancellationToken ct = default)
        => _evaluator.EvaluateGetFirstAsync(specification, ct);

    public Task<Order?> EvaluateGetFirstFailedAsync(IBasicSpecification<Order> specification, CancellationToken ct = default)
        => _evaluator.EvaluateGetFirstFailedAsync(specification, ct);

    public Task<Order?> EvaluateGetLastAsync(IQuerySpecification<Order> specification, CancellationToken ct = default)
        => _evaluator.EvaluateGetLastAsync(specification, ct);

    public Task<Order?> EvaluateGetLastFailedAsync(IQuerySpecification<Order> specification, CancellationToken ct = default)
        => _evaluator.EvaluateGetLastFailedAsync(specification, ct);

    public Task<IQueryable<Order>> EvaluateQueryAsync(IQuerySpecification<Order> specification)
        => _evaluator.EvaluateQueryAsync(specification);

    public Task<IQueryable<Order>> EvaluateQueryFailedAsync(IQuerySpecification<Order> specification)
        => _evaluator.EvaluateQueryFailedAsync(specification);

    public Task<IEnumerable<Order>> EvaluateDistinctAsync<TKey>(IQuerySpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> selector, CancellationToken ct = default)
        => _evaluator.EvaluateDistinctAsync(spec, selector, ct);

    public Task<IEnumerable<Order>> EvaluateDuplicatesAsync<TKey>(IQuerySpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> selector, CancellationToken ct = default)
        => _evaluator.EvaluateDuplicatesAsync(spec, selector, ct);

    public Task<decimal> EvaluateMinAsync<TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TResult>> selector, CancellationToken ct = default)
        => _evaluator.EvaluateMinAsync(spec, selector, ct);

    public Task<decimal> EvaluateMaxAsync<TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TResult>> selector, CancellationToken ct = default)
        => _evaluator.EvaluateMaxAsync(spec, selector, ct);

    public Task<decimal> EvaluateAverageAsync<TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TResult>> selector, CancellationToken ct = default)
        => _evaluator.EvaluateAverageAsync(spec, selector, ct);

    public Task<decimal> EvaluateSumAsync<TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TResult>> selector, CancellationToken ct = default)
        => _evaluator.EvaluateSumAsync(spec, selector, ct);

    public Task<IEnumerable<Order>> EvaluateTopAsync(IQuerySpecification<Order> specification, int count, CancellationToken ct = default)
        => _evaluator.EvaluateTopAsync(specification, count, ct);

    public Task<Dictionary<TKey, int>> EvaluateCountByGroupAsync<TKey>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, CancellationToken ct = default)
        => _evaluator.EvaluateCountByGroupAsync(spec, keySelector, ct);

    public Task<Dictionary<TKey, decimal>> EvaluateSumByGroupAsync<TKey, TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Order, TResult>> valueSelector, CancellationToken ct = default)
        => _evaluator.EvaluateSumByGroupAsync(spec, keySelector, valueSelector, ct);

    public Task<Dictionary<TKey, decimal>> EvaluateMinByGroupAsync<TKey, TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Order, TResult>> valueSelector, CancellationToken ct = default)
        => _evaluator.EvaluateMinByGroupAsync(spec, keySelector, valueSelector, ct);

    public Task<Dictionary<TKey, decimal>> EvaluateMaxByGroupAsync<TKey, TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Order, TResult>> valueSelector, CancellationToken ct = default)
        => _evaluator.EvaluateMaxByGroupAsync(spec, keySelector, valueSelector, ct);

    public Task<Dictionary<TKey, decimal>> EvaluateAverageByGroupAsync<TKey, TResult>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Order, TResult>> valueSelector, CancellationToken ct = default)
        => _evaluator.EvaluateAverageByGroupAsync(spec, keySelector, valueSelector, ct);

    public Task<Dictionary<TKey, IEnumerable<Order>>> EvaluateDuplicatesByGroupAsync<TKey>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, CancellationToken ct = default)
        => _evaluator.EvaluateDuplicatesByGroupAsync(spec, keySelector, ct);

    public Task<Dictionary<TKey, IEnumerable<Order>>> EvaluateUniquesByGroupAsync<TKey>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, CancellationToken ct = default)
        => _evaluator.EvaluateUniquesByGroupAsync(spec, keySelector, ct);

    public Task<Dictionary<TKey, IEnumerable<Order>>> EvaluateTopByGroupAsync<TKey>(IBasicSpecification<Order> spec, System.Linq.Expressions.Expression<Func<Order, TKey>> keySelector, int count, CancellationToken ct = default)
        => _evaluator.EvaluateTopByGroupAsync(spec, keySelector, count, ct);

    #endregion

    #region IEvaluatorWrite<Order> implementation

    public Task<Order> AddAsync(Order entity, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.AddAsync(entity, saveChanges, ct);

    public Task<IEnumerable<Order>> AddRangeAsync(IEnumerable<Order> entities, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.AddRangeAsync(entities, saveChanges, ct);

    public Task<Order> UpdateAsync(Order entity, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.UpdateAsync(entity, saveChanges, ct);

    public Task<IEnumerable<Order>> UpdateRangeAsync(IEnumerable<Order> entities, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.UpdateRangeAsync(entities, saveChanges, ct);

    public Task DeleteAsync(Order entity, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.DeleteAsync(entity, saveChanges, ct);

    public Task DeleteRangeAsync(IEnumerable<Order> entities, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.DeleteRangeAsync(entities, saveChanges, ct);

    public Task<Order> UpsertAsync(Order entity, System.Linq.Expressions.Expression<Func<Order, bool>> matchCondition, bool saveChanges = true, CancellationToken ct = default)
        => _evaluator.UpsertAsync(entity, matchCondition, saveChanges, ct);

    public Task<IEnumerable<Order>> UpsertRangeAsync<TProperty>(IEnumerable<Order> entities, System.Linq.Expressions.Expression<Func<Order, TProperty>> keySelector, bool saveChanges = true, CancellationToken ct = default) where TProperty : notnull
        => _evaluator.UpsertRangeAsync(entities, keySelector, saveChanges, ct);

    public Task DeleteByConditionAsync(System.Linq.Expressions.Expression<Func<Order, bool>> condition, CancellationToken ct = default)
        => _evaluator.DeleteByConditionAsync(condition, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _evaluator.SaveChangesAsync(ct);

    public Task ExecuteTransactionAsync(Func<Task> operations, CancellationToken ct = default)
        => _evaluator.ExecuteTransactionAsync(operations, ct);

    public Task BulkInsertAsync(IEnumerable<Order> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default)
        => _evaluator.BulkInsertAsync(entities, bulkConfig, ct);

    public Task BulkUpdateAsync(IEnumerable<Order> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default)
        => _evaluator.BulkUpdateAsync(entities, bulkConfig, ct);

    public Task BulkDeleteAsync(IEnumerable<Order> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default)
        => _evaluator.BulkDeleteAsync(entities, bulkConfig, ct);

    public Task BulkInsertOrUpdateAsync(IEnumerable<Order> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default)
        => _evaluator.BulkInsertOrUpdateAsync(entities, bulkConfig, ct);

    #endregion
}

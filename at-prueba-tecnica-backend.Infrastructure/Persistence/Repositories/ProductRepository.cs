using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Evaluators;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for Product entity operations.
/// Encapsulates ValiFlowEvaluator to provide read/write operations with Vali-Flow specifications.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ValiFlowEvaluator<Product> _evaluator;

    public ProductRepository(AppDbContext dbContext)
    {
        _evaluator = new ValiFlowEvaluator<Product>(dbContext);
    }

    // Delegate all operations to the evaluator
    public Task<bool> EvaluateAsync(Vali_Flow.Core.Builder.ValiFlow<Product> valiFlow, Product entity) => _evaluator.EvaluateAsync(valiFlow, entity);
    public Task<bool> EvaluateAnyAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> specification, CancellationToken ct = default) => _evaluator.EvaluateAnyAsync(specification, ct);
    public Task<int> EvaluateCountAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> specification, CancellationToken ct = default) => _evaluator.EvaluateCountAsync(specification, ct);
    public Task<Product?> EvaluateGetFirstAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> specification, CancellationToken ct = default) => _evaluator.EvaluateGetFirstAsync(specification, ct);
    public Task<Product?> EvaluateGetFirstFailedAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> specification, CancellationToken ct = default) => _evaluator.EvaluateGetFirstFailedAsync(specification, ct);
    public Task<Product?> EvaluateGetLastAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> specification, CancellationToken ct = default) => _evaluator.EvaluateGetLastAsync(specification, ct);
    public Task<Product?> EvaluateGetLastFailedAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> specification, CancellationToken ct = default) => _evaluator.EvaluateGetLastFailedAsync(specification, ct);
    public Task<IQueryable<Product>> EvaluateQueryAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> specification) => _evaluator.EvaluateQueryAsync(specification);
    public Task<IQueryable<Product>> EvaluateQueryFailedAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> specification) => _evaluator.EvaluateQueryFailedAsync(specification);
    public Task<IEnumerable<Product>> EvaluateDistinctAsync<TKey>(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> selector, CancellationToken ct = default) => _evaluator.EvaluateDistinctAsync(spec, selector, ct);
    public Task<IEnumerable<Product>> EvaluateDuplicatesAsync<TKey>(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> selector, CancellationToken ct = default) => _evaluator.EvaluateDuplicatesAsync(spec, selector, ct);
    public Task<decimal> EvaluateMinAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateMinAsync(spec, selector, ct);
    public Task<decimal> EvaluateMaxAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateMaxAsync(spec, selector, ct);
    public Task<decimal> EvaluateAverageAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateAverageAsync(spec, selector, ct);
    public Task<decimal> EvaluateSumAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateSumAsync(spec, selector, ct);
    public Task<IEnumerable<Product>> EvaluateTopAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<Product> specification, int count, CancellationToken ct = default) => _evaluator.EvaluateTopAsync(specification, count, ct);
    public Task<Dictionary<TKey, int>> EvaluateCountByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, CancellationToken ct = default) => _evaluator.EvaluateCountByGroupAsync(spec, keySelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateSumByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Product, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateSumByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateMinByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Product, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateMinByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateMaxByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Product, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateMaxByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateAverageByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, System.Linq.Expressions.Expression<Func<Product, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateAverageByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, IEnumerable<Product>>> EvaluateDuplicatesByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, CancellationToken ct = default) => _evaluator.EvaluateDuplicatesByGroupAsync(spec, keySelector, ct);
    public Task<Dictionary<TKey, IEnumerable<Product>>> EvaluateUniquesByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, CancellationToken ct = default) => _evaluator.EvaluateUniquesByGroupAsync(spec, keySelector, ct);
    public Task<Dictionary<TKey, IEnumerable<Product>>> EvaluateTopByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<Product> spec, System.Linq.Expressions.Expression<Func<Product, TKey>> keySelector, int count, CancellationToken ct = default) => _evaluator.EvaluateTopByGroupAsync(spec, keySelector, count, ct);
    public Task<Product> AddAsync(Product entity, bool saveChanges = true, CancellationToken ct = default) => _evaluator.AddAsync(entity, saveChanges, ct);
    public Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities, bool saveChanges = true, CancellationToken ct = default) => _evaluator.AddRangeAsync(entities, saveChanges, ct);
    public Task<Product> UpdateAsync(Product entity, bool saveChanges = true, CancellationToken ct = default) => _evaluator.UpdateAsync(entity, saveChanges, ct);
    public Task<IEnumerable<Product>> UpdateRangeAsync(IEnumerable<Product> entities, bool saveChanges = true, CancellationToken ct = default) => _evaluator.UpdateRangeAsync(entities, saveChanges, ct);
    public Task DeleteAsync(Product entity, bool saveChanges = true, CancellationToken ct = default) => _evaluator.DeleteAsync(entity, saveChanges, ct);
    public Task DeleteRangeAsync(IEnumerable<Product> entities, bool saveChanges = true, CancellationToken ct = default) => _evaluator.DeleteRangeAsync(entities, saveChanges, ct);
    public Task<Product> UpsertAsync(Product entity, System.Linq.Expressions.Expression<Func<Product, bool>> matchCondition, bool saveChanges = true, CancellationToken ct = default) => _evaluator.UpsertAsync(entity, matchCondition, saveChanges, ct);
    public Task<IEnumerable<Product>> UpsertRangeAsync<TProperty>(IEnumerable<Product> entities, System.Linq.Expressions.Expression<Func<Product, TProperty>> keySelector, bool saveChanges = true, CancellationToken ct = default) where TProperty : notnull => _evaluator.UpsertRangeAsync(entities, keySelector, saveChanges, ct);
    public Task DeleteByConditionAsync(System.Linq.Expressions.Expression<Func<Product, bool>> condition, CancellationToken ct = default) => _evaluator.DeleteByConditionAsync(condition, ct);
    public Task SaveChangesAsync(CancellationToken ct = default) => _evaluator.SaveChangesAsync(ct);
    public Task ExecuteTransactionAsync(Func<Task> operations, CancellationToken ct = default) => _evaluator.ExecuteTransactionAsync(operations, ct);
    public Task BulkInsertAsync(IEnumerable<Product> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkInsertAsync(entities, bulkConfig, ct);
    public Task BulkUpdateAsync(IEnumerable<Product> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkUpdateAsync(entities, bulkConfig, ct);
    public Task BulkDeleteAsync(IEnumerable<Product> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkDeleteAsync(entities, bulkConfig, ct);
    public Task BulkInsertOrUpdateAsync(IEnumerable<Product> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkInsertOrUpdateAsync(entities, bulkConfig, ct);
}

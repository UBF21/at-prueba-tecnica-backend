using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Evaluators;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for User entity operations.
/// Encapsulates ValiFlowEvaluator to provide read/write operations with Vali-Flow specifications.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ValiFlowEvaluator<User> _evaluator;

    public UserRepository(AppDbContext dbContext)
    {
        _evaluator = new ValiFlowEvaluator<User>(dbContext);
    }

    // Delegate all operations to the evaluator
    public Task<bool> EvaluateAsync(Vali_Flow.Core.Builder.ValiFlow<User> valiFlow, User entity) => _evaluator.EvaluateAsync(valiFlow, entity);
    public Task<bool> EvaluateAnyAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> specification, CancellationToken ct = default) => _evaluator.EvaluateAnyAsync(specification, ct);
    public Task<int> EvaluateCountAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> specification, CancellationToken ct = default) => _evaluator.EvaluateCountAsync(specification, ct);
    public Task<User?> EvaluateGetFirstAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> specification, CancellationToken ct = default) => _evaluator.EvaluateGetFirstAsync(specification, ct);
    public Task<User?> EvaluateGetFirstFailedAsync(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> specification, CancellationToken ct = default) => _evaluator.EvaluateGetFirstFailedAsync(specification, ct);
    public Task<User?> EvaluateGetLastAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> specification, CancellationToken ct = default) => _evaluator.EvaluateGetLastAsync(specification, ct);
    public Task<User?> EvaluateGetLastFailedAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> specification, CancellationToken ct = default) => _evaluator.EvaluateGetLastFailedAsync(specification, ct);
    public Task<IQueryable<User>> EvaluateQueryAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> specification) => _evaluator.EvaluateQueryAsync(specification);
    public Task<IQueryable<User>> EvaluateQueryFailedAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> specification) => _evaluator.EvaluateQueryFailedAsync(specification);
    public Task<IEnumerable<User>> EvaluateDistinctAsync<TKey>(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> selector, CancellationToken ct = default) => _evaluator.EvaluateDistinctAsync(spec, selector, ct);
    public Task<IEnumerable<User>> EvaluateDuplicatesAsync<TKey>(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> selector, CancellationToken ct = default) => _evaluator.EvaluateDuplicatesAsync(spec, selector, ct);
    public Task<decimal> EvaluateMinAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateMinAsync(spec, selector, ct);
    public Task<decimal> EvaluateMaxAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateMaxAsync(spec, selector, ct);
    public Task<decimal> EvaluateAverageAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateAverageAsync(spec, selector, ct);
    public Task<decimal> EvaluateSumAsync<TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TResult>> selector, CancellationToken ct = default) => _evaluator.EvaluateSumAsync(spec, selector, ct);
    public Task<IEnumerable<User>> EvaluateTopAsync(Vali_Flow.Interfaces.Specification.IQuerySpecification<User> specification, int count, CancellationToken ct = default) => _evaluator.EvaluateTopAsync(specification, count, ct);
    public Task<Dictionary<TKey, int>> EvaluateCountByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, CancellationToken ct = default) => _evaluator.EvaluateCountByGroupAsync(spec, keySelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateSumByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, System.Linq.Expressions.Expression<Func<User, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateSumByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateMinByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, System.Linq.Expressions.Expression<Func<User, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateMinByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateMaxByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, System.Linq.Expressions.Expression<Func<User, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateMaxByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, decimal>> EvaluateAverageByGroupAsync<TKey, TResult>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, System.Linq.Expressions.Expression<Func<User, TResult>> valueSelector, CancellationToken ct = default) => _evaluator.EvaluateAverageByGroupAsync(spec, keySelector, valueSelector, ct);
    public Task<Dictionary<TKey, IEnumerable<User>>> EvaluateDuplicatesByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, CancellationToken ct = default) => _evaluator.EvaluateDuplicatesByGroupAsync(spec, keySelector, ct);
    public Task<Dictionary<TKey, IEnumerable<User>>> EvaluateUniquesByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, CancellationToken ct = default) => _evaluator.EvaluateUniquesByGroupAsync(spec, keySelector, ct);
    public Task<Dictionary<TKey, IEnumerable<User>>> EvaluateTopByGroupAsync<TKey>(Vali_Flow.Interfaces.Specification.IBasicSpecification<User> spec, System.Linq.Expressions.Expression<Func<User, TKey>> keySelector, int count, CancellationToken ct = default) => _evaluator.EvaluateTopByGroupAsync(spec, keySelector, count, ct);
    public Task<User> AddAsync(User entity, bool saveChanges = true, CancellationToken ct = default) => _evaluator.AddAsync(entity, saveChanges, ct);
    public Task<IEnumerable<User>> AddRangeAsync(IEnumerable<User> entities, bool saveChanges = true, CancellationToken ct = default) => _evaluator.AddRangeAsync(entities, saveChanges, ct);
    public Task<User> UpdateAsync(User entity, bool saveChanges = true, CancellationToken ct = default) => _evaluator.UpdateAsync(entity, saveChanges, ct);
    public Task<IEnumerable<User>> UpdateRangeAsync(IEnumerable<User> entities, bool saveChanges = true, CancellationToken ct = default) => _evaluator.UpdateRangeAsync(entities, saveChanges, ct);
    public Task DeleteAsync(User entity, bool saveChanges = true, CancellationToken ct = default) => _evaluator.DeleteAsync(entity, saveChanges, ct);
    public Task DeleteRangeAsync(IEnumerable<User> entities, bool saveChanges = true, CancellationToken ct = default) => _evaluator.DeleteRangeAsync(entities, saveChanges, ct);
    public Task<User> UpsertAsync(User entity, System.Linq.Expressions.Expression<Func<User, bool>> matchCondition, bool saveChanges = true, CancellationToken ct = default) => _evaluator.UpsertAsync(entity, matchCondition, saveChanges, ct);
    public Task<IEnumerable<User>> UpsertRangeAsync<TProperty>(IEnumerable<User> entities, System.Linq.Expressions.Expression<Func<User, TProperty>> keySelector, bool saveChanges = true, CancellationToken ct = default) where TProperty : notnull => _evaluator.UpsertRangeAsync(entities, keySelector, saveChanges, ct);
    public Task DeleteByConditionAsync(System.Linq.Expressions.Expression<Func<User, bool>> condition, CancellationToken ct = default) => _evaluator.DeleteByConditionAsync(condition, ct);
    public Task SaveChangesAsync(CancellationToken ct = default) => _evaluator.SaveChangesAsync(ct);
    public Task ExecuteTransactionAsync(Func<Task> operations, CancellationToken ct = default) => _evaluator.ExecuteTransactionAsync(operations, ct);
    public Task BulkInsertAsync(IEnumerable<User> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkInsertAsync(entities, bulkConfig, ct);
    public Task BulkUpdateAsync(IEnumerable<User> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkUpdateAsync(entities, bulkConfig, ct);
    public Task BulkDeleteAsync(IEnumerable<User> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkDeleteAsync(entities, bulkConfig, ct);
    public Task BulkInsertOrUpdateAsync(IEnumerable<User> entities, EFCore.BulkExtensions.BulkConfig? bulkConfig = null, CancellationToken ct = default) => _evaluator.BulkInsertOrUpdateAsync(entities, bulkConfig, ct);
}

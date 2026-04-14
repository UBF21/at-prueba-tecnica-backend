using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Abstractions;
using Vali_Flow.Interfaces.Evaluators.Read;
using Vali_Flow.Interfaces.Evaluators.Write;

namespace at_prueba_tecnica_backend.Domain.Interfaces;

/// <summary>
/// Repository interface for Order entity.
/// Provides read/write operations via Vali-Flow evaluator delegation.
/// </summary>
public interface IOrderRepository : IEvaluatorRead<Order>, IEvaluatorWrite<Order> { }

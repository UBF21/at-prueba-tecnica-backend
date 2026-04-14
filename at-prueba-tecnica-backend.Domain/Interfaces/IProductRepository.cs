using at_prueba_tecnica_backend.Domain.Entities;
using Vali_Flow.Interfaces.Evaluators.Read;
using Vali_Flow.Interfaces.Evaluators.Write;

namespace at_prueba_tecnica_backend.Domain.Interfaces;

/// <summary>
/// Repository interface for Product entity.
/// Combines read and write operations using Vali-Flow evaluators.
/// </summary>
public interface IProductRepository : IEvaluatorRead<Product>, IEvaluatorWrite<Product> { }

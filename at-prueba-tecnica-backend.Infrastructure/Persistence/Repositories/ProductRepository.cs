using at_prueba_tecnica_backend.Domain.Entities;
using at_prueba_tecnica_backend.Domain.Interfaces;
using Vali_Flow.Classes.Evaluators;

namespace at_prueba_tecnica_backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for Product entity operations.
/// Inherits from ValiFlowEvaluator to provide read/write operations with Vali-Flow specifications.
/// </summary>
public class ProductRepository : ValiFlowEvaluator<Product>, IProductRepository
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}

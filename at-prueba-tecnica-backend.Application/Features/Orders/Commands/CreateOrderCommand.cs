using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;
using Vali_Mediator_Resilience.Core.Enums;
using Vali_Mediator_Resilience.Core.Policies;
using Vali_Mediator_Resilience.Integration;

namespace at_prueba_tecnica_backend.Application.Features.Orders.Commands;

public record CreateOrderCommand(string OrderNumber, Guid CustomerId)
    : IRequest<Result<OrderDto>>, IResilient
{
    private static readonly ResiliencePolicy _policy = ResiliencePolicy
        .Create("create-order")
        .Retry(o =>
        {
            o.MaxRetries = 3;
            o.BackoffType = BackoffType.ExponentialWithJitter;
            o.InitialDelay = TimeSpan.FromMilliseconds(200);
            o.MaxDelay = TimeSpan.FromSeconds(5);
        })
        .CircuitBreaker(o =>
        {
            o.CircuitKey = "create-order";
            o.FailureThreshold = 5;
            o.SamplingDuration = TimeSpan.FromSeconds(30);
            o.BreakDuration = TimeSpan.FromSeconds(60);
        })
        .Timeout(TimeSpan.FromSeconds(30))
        .Build();

    public ResiliencePolicy Policy => _policy;
}

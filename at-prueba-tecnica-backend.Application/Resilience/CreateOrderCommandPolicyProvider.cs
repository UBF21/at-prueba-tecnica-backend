using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using Vali_Mediator_Resilience.Core.Enums;
using Vali_Mediator_Resilience.Core.Policies;
using Vali_Mediator_Resilience.Integration;

namespace at_prueba_tecnica_backend.Application.Resilience;

public sealed class CreateOrderCommandPolicyProvider : IResiliencePolicyProvider<CreateOrderCommand>
{
    public ResiliencePolicy GetPolicy(CreateOrderCommand request) =>
        ResiliencePolicy.Create("create-order")
            .Bulkhead(o =>
            {
                o.MaxConcurrentCalls = 20;
                o.MaxQueuedCalls = 10;
                o.QueueTimeout = TimeSpan.FromSeconds(5);
            })
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
}

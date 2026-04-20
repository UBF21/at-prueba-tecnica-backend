using at_prueba_tecnica_backend.Application.Features.Auth.Commands;
using Vali_Mediator_Resilience.Core.Enums;
using Vali_Mediator_Resilience.Core.Policies;
using Vali_Mediator_Resilience.Integration;

namespace at_prueba_tecnica_backend.Application.Resilience;

public sealed class LoginCommandPolicyProvider : IResiliencePolicyProvider<LoginCommand>
{
    public ResiliencePolicy GetPolicy(LoginCommand request) =>
        ResiliencePolicy.Create("login")
            .RateLimiter(o =>
            {
                o.Algorithm = RateLimiterAlgorithm.SlidingWindow;
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(1);
                o.PartitionKeyResolver = r => ((LoginCommand)r).Email;
            })
            .Retry(o =>
            {
                o.MaxRetries = 3;
                o.BackoffType = BackoffType.ExponentialWithJitter;
                o.InitialDelay = TimeSpan.FromMilliseconds(100);
                o.MaxDelay = TimeSpan.FromSeconds(3);
            })
            .CircuitBreaker(o =>
            {
                o.CircuitKey = "login";
                o.FailureThreshold = 5;
                o.SamplingDuration = TimeSpan.FromSeconds(30);
                o.BreakDuration = TimeSpan.FromSeconds(60);
            })
            .Timeout(TimeSpan.FromSeconds(10))
            .Build();
}

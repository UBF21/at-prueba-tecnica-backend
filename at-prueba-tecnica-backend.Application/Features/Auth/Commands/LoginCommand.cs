using at_prueba_tecnica_backend.Application.Features.DTOs;
using Vali_Mediator.Core.Request;
using Vali_Mediator.Core.Result;
using Vali_Mediator.Core.General.Behavior;
using Vali_Mediator_Resilience.Core.Enums;
using Vali_Mediator_Resilience.Core.Policies;
using Vali_Mediator_Resilience.Integration;

namespace at_prueba_tecnica_backend.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password)
    : IRequest<Result<LoginResponseDto>>, IResilient
{
    private static readonly ResiliencePolicy _policy = ResiliencePolicy
        .Create("login")
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

    public ResiliencePolicy Policy => _policy;
}

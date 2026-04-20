using at_prueba_tecnica_backend.Application.Features.Auth.Commands;
using at_prueba_tecnica_backend.Application.Features.Orders.Commands;
using at_prueba_tecnica_backend.Application.Resilience;
using Microsoft.Extensions.DependencyInjection;
using Vali_Mediator_Resilience.Core.Enums;
using Vali_Mediator_Resilience.Core.Policies;
using Vali_Mediator_Resilience.Integration;

namespace at_prueba_tecnica_backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddResiliencePolicies();
        return services;
    }

    private static IServiceCollection AddResiliencePolicies(this IServiceCollection services)
    {
        // Políticas por command — la librería cachea GetPolicy tras el primer build (v1.2.2)
        services.AddResiliencePolicyProvider<LoginCommand, LoginCommandPolicyProvider>();
        services.AddResiliencePolicyProvider<CreateOrderCommand, CreateOrderCommandPolicyProvider>();

        // Global fallback para todos los commands sin política explícita
        services.AddGlobalResiliencePolicy(
            ResiliencePolicy.Create("global")
                .Retry(o =>
                {
                    o.MaxRetries = 2;
                    o.BackoffType = BackoffType.Linear;
                    o.InitialDelay = TimeSpan.FromMilliseconds(150);
                })
                .Timeout(TimeSpan.FromSeconds(30))
                .Build());

        return services;
    }
}

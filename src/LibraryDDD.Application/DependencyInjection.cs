using MediatR;
using LibraryDDD.Application.Common.Events;
using LibraryDDD.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDDD.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.Scan(scan => scan
            .FromAssemblyOf<IDomainEventHandler<IDomainEvent>>()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddMediatR(typeof(DependencyInjection).Assembly);

        return services;
    }


}
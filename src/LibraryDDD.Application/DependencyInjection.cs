using MediatR;
using LibraryDDD.Application.Common.Events;
using LibraryDDD.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using LibraryDDD.Application.Members.RegisterMember;

namespace LibraryDDD.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<RegisterMemberHandler>();

        services.Scan(scan => scan
            .FromAssemblyOf<IDomainEventHandler<IDomainEvent>>()
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddMediatR(typeof(DependencyInjection).Assembly);

        return services;
    }


}
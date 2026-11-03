using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDDD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register the in-memory repository as the implementation
        services.AddSingleton<IMemberRepository, InMemoryMemberRepository>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

        return services;
    }
}
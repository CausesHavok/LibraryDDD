using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDDD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // 1. Create the repository once, for the whole life of the app.
        services.AddSingleton<InMemoryMemberRepository>();

        // 2. "When someone asks for IMemberRepository, hand them that same instance."
        services.AddSingleton<IMemberRepository>(
            sp => sp.GetRequiredService<InMemoryMemberRepository>());

        // 3. "When someone asks for ITracksAggregates, hand them that same instance too."
        services.AddSingleton<ITracksAggregates>(
            sp => sp.GetRequiredService<InMemoryMemberRepository>());

        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

        return services;
    }
}
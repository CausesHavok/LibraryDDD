using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDDD.Infrastructure.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_RegistersRepositoryAsSingletonAndUnitOfWorkAsScoped()
    {
        var services = new ServiceCollection();
        services.AddInfrastructure();

        var repositoryRegistration = Assert.Single(
            services, d => d.ServiceType == typeof(IMemberRepository));
        Assert.Equal(ServiceLifetime.Singleton, repositoryRegistration.Lifetime);

        var unitOfWorkRegistration = Assert.Single(
            services, d => d.ServiceType == typeof(IUnitOfWork));
        Assert.Equal(ServiceLifetime.Scoped, unitOfWorkRegistration.Lifetime);
        Assert.Equal(typeof(InMemoryUnitOfWork), unitOfWorkRegistration.ImplementationType);

        Assert.Same(services, services.AddInfrastructure());
    }

    [Fact]
    public void AddInfrastructure_RegistersTheSameRepositoryInstanceForStorageAndTracking()
    {
        var services = new ServiceCollection();
        services.AddInfrastructure();
        using var provider = services.BuildServiceProvider();

        var repository = provider.GetRequiredService<IMemberRepository>();
        var tracker = Assert.Single(provider.GetServices<ITracksAggregates>());

        Assert.Same(repository, tracker);
    }

}

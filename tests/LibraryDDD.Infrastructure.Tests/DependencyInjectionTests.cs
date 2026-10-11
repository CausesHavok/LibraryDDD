using LibraryDDD.Application.Common.Events;
using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Infrastructure;
using LibraryDDD.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDDD.Infrastructure.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_RegistersRepositoryAndUnitOfWorkAsSingletons()
    {
        var services = new ServiceCollection();

        services.AddInfrastructure();

        var repositoryRegistration = Assert.Single(
            services,
            descriptor => descriptor.ServiceType == typeof(IMemberRepository));
        Assert.Equal(ServiceLifetime.Singleton, repositoryRegistration.Lifetime);
        Assert.Equal(typeof(InMemoryMemberRepository), repositoryRegistration.ImplementationType);

        var unitOfWorkRegistration = Assert.Single(
            services,
            descriptor => descriptor.ServiceType == typeof(IUnitOfWork));
        Assert.Equal(ServiceLifetime.Singleton, unitOfWorkRegistration.Lifetime);
        Assert.Equal(typeof(InMemoryUnitOfWork), unitOfWorkRegistration.ImplementationType);

        Assert.Same(services, services.AddInfrastructure());
    }
}

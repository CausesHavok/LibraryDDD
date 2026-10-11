using LibraryDDD.Application;
using LibraryDDD.Application.Common.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDDD.Application.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_RegistersRequiredServices()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        Assert.Contains(
            services,
            s => s.ServiceType == typeof(IDomainEventDispatcher));

        Assert.Contains(
            services,
            s => s.ServiceType == typeof(IMediator));
    }
}
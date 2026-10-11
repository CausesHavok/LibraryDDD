using Microsoft.Extensions.DependencyInjection;
using LibraryDDD.Application.Common.Events;
using LibraryDDD.Domain.Events;

namespace LibraryDDD.Application.Tests.Common.Events;

public class DomainEventDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_WhenHandlersExist_InvokesThemForMatchingEvents()
    {
        var calls = new List<string>();
        var services = new ServiceCollection();
        services.AddSingleton<IDomainEventHandler<TestEvent>>(new TestEventHandler(calls));
        services.AddSingleton<IDomainEventHandler<OtherTestEvent>>(new OtherTestEventHandler(calls));

        var dispatcher = new DomainEventDispatcher(services.BuildServiceProvider());

        await dispatcher.DispatchAsync(
        [
            new TestEvent("first"),
            new OtherTestEvent("second")
        ]);

        Assert.Equal(["first", "second"], calls);
    }

    [Fact]
    public async Task DispatchAsync_WhenNoHandlersExist_DoesNothing()
    {
        var services = new ServiceCollection();
        var dispatcher = new DomainEventDispatcher(services.BuildServiceProvider());

        var exception = await Record.ExceptionAsync(() => dispatcher.DispatchAsync([new TestEvent("only") ]));

        Assert.Null(exception);
    }

    [Fact]
    public async Task DispatchAsync_WhenRegisteredHandlerIsNull_ThrowsInvalidOperationException()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDomainEventHandler<TestEvent>>(_ => null!);

        var dispatcher = new DomainEventDispatcher(services.BuildServiceProvider());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            dispatcher.DispatchAsync([new TestEvent("missing")]));

        Assert.Equal("No handler found for domain event of type TestEvent.", exception.Message);
    }

    private sealed record TestEvent(string Name) : IDomainEvent;
    private sealed record OtherTestEvent(string Name) : IDomainEvent;

    private sealed class TestEventHandler(List<string> calls) : IDomainEventHandler<TestEvent>
    {
        public Task HandleAsync(TestEvent domainEvent)
        {
            calls.Add(domainEvent.Name);
            return Task.CompletedTask;
        }
    }

    private sealed class OtherTestEventHandler(List<string> calls) : IDomainEventHandler<OtherTestEvent>
    {
        public Task HandleAsync(OtherTestEvent domainEvent)
        {
            calls.Add(domainEvent.Name);
            return Task.CompletedTask;
        }
    }
}

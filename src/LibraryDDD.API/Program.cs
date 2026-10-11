using LibraryDDD.Infrastructure;
using LibraryDDD.Application;
using LibraryDDD.Api.Endpoints;
namespace LibraryDDD.Api;   

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();

        var app = builder.Build();
        app.MapMemberEndpoints();
        app.Run();
    }
}

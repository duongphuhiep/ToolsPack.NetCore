using Radzen;
using RadzenBlazorDemos;
using RadzenBlazorDemos.Data;
using RadzenBlazorDemos.Services;

namespace BlazorApp1.Client;

public static class Register
{
    public static IServiceCollection AddDemoApplication(this IServiceCollection services)
    {
        services.AddRadzenComponents();
        services.AddScoped<ExampleService>();
        services.AddDbContextFactory<NorthwindContext>();
        services.AddScoped<NorthwindService>();
        services.AddScoped<NorthwindODataService>();
        services.AddSingleton<GitHubService>();
        return services;
    }
}
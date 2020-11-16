using System;
using Microsoft.Extensions.DependencyInjection;

namespace Funda.Assignment.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Build configuration
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

        // Add access to generic IConfigurationRoot
        serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

        // Add app
        serviceCollection.AddTransient<App>();
    }
}

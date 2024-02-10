using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Configuration;

[assembly: FunctionsStartup(typeof(CloudSecurity.Startup))]
namespace CloudSecurity
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var executionContextOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;
            var appDirectory = executionContextOptions.AppDirectory;

            var config = new ConfigurationBuilder()
                    .SetBasePath(appDirectory)
                    .AddEnvironmentVariables()
                    .Build();

            builder.Services.Configure<Configuration>(config);

            ConfigureServices(builder.Services);
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
        }
    }
}

using System.Threading.Tasks;
using Convey;
using Convey.Secrets.Vault;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Availability.Application;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Infrastructure;

namespace Pacco.Services.Availability.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseRouting()
                    .UseEndpoints(e =>
                    {
                        e.MapControllers();
                        e.MapGet("ping", context => context.Response.WriteAsync("pong"));
                    })
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Post<AddResource>("resources", afterDispatch: (cmd, ctx) => 
                            ctx.Response.Created($"resources/{cmd.ResourceId}"))))
                .UseLogging()
                .UseVault();
    }
}

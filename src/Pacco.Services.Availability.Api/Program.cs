using System.Threading.Tasks;
using Convey;
using Convey.Configurations.Vault;
using Convey.Docs.Swagger;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Availability.Application;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;
using Pacco.Services.Availability.Infrastructure;

namespace Pacco.Services.Availability.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await GetWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder GetWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddOpenTracing()
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .AddWebApiSwaggerDocs()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseSwaggerDocs()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetResourceReservation, ResourceDto>("resources/{resourceId}")
                        .Post<AddResource>("resources",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"resources/{cmd.ResourceId}"))
                        .Post<ReserveResource>("resources/{resourceId}/reservations")
                        .Delete<ReleaseResource>("resources/{resourceId}/reservations/{dateTime}")
                        .Delete<DeleteResource>("resources/{resourceId}")))
                .UseLogging()
                .UseVault();
    }
}

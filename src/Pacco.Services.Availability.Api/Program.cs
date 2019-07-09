using System.Threading.Tasks;
using Convey;
using Convey.Docs.Swagger;
using Convey.Logging;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
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
                        .Get("", ctx => ctx.Response.WriteAsync("Welcome to Pacco Availability Service!"))
                        .Get<GetResourceReservation, ResourceDto>("resources/{id}")
                        .Post<AddResource>("resources",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"resources/{cmd.Id}"))
                        .Post<ReserveResource>("resources/{id}/reservations")
                        .Delete<ReleaseResource>("resources/{id}/reservations/{dateTime}")
                        .Delete<DeleteResource>("resources/{id}")))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}

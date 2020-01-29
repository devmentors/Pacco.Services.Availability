using System;
using System.Threading.Tasks;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Xunit;

namespace Pacco.Services.Availability.Tests.Performance
{
    public class PerformanceTests
    {
        [Fact]
        public void get_resources()
        {
            const string url = "http://localhost:5001";
            const string stepName = "init";
            const int duration = 3;
            const int expectedRps = 100;
            var endpoint = $"{url}/resources";

            var step = HttpStep.Create(stepName, ctx =>
                Task.FromResult(Http.CreateRequest("GET", endpoint)
                    .WithCheck(response => Task.FromResult(response.IsSuccessStatusCode))));

            var assertions = new[]
            {
                Assertion.ForStep(stepName, s => s.RPS >= expectedRps),
                Assertion.ForStep(stepName, s => s.OkCount >= expectedRps * duration)
            };

            var scenario = ScenarioBuilder.CreateScenario("GET resources", step)
                .WithConcurrentCopies(1)
                .WithOutWarmUp()
                .WithDuration(TimeSpan.FromSeconds(duration))
                .WithAssertions(assertions);
            
            NBomberRunner.RegisterScenarios(scenario)
                .RunTest();
        }
    }
}
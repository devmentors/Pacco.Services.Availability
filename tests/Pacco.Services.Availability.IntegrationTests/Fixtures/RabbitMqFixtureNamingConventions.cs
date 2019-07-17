using System;
using System.Reflection;
using Convey.MessageBrokers;
using RawRabbit.Common;

namespace Pacco.Services.Availability.IntegrationTests.Fixtures
{
    internal sealed class RabbitMqFixtureNamingConventions : NamingConventions
    {
        public RabbitMqFixtureNamingConventions(string defaultNamespace)
        {
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            ExchangeNamingConvention = type => GetExchange(type, defaultNamespace);
            RoutingKeyConvention = type => GetRoutingKey(type, defaultNamespace);
            QueueNamingConvention = type => GetQueueName(assemblyName, type, defaultNamespace);
            ErrorExchangeNamingConvention = () => $"{defaultNamespace}.error";
            RetryLaterExchangeConvention = span => $"{defaultNamespace}.retry";
            RetryLaterQueueNameConvetion = (exchange, span) =>
                $"{defaultNamespace}.retry_for_{exchange.Replace(".", "_")}_in_{span.TotalMilliseconds}_ms"
                    .ToLowerInvariant();
        }

        private static string GetExchange(Type type, string defaultNamespace)
        {
            var (@namespace, key) = GetNamespaceAndKey(type, defaultNamespace);

            return (string.IsNullOrWhiteSpace(@namespace) ? key : $"{@namespace}").ToLowerInvariant();
        }

        private static string GetRoutingKey(Type type, string defaultNamespace)
        {
            var (@namespace, key) = GetNamespaceAndKey(type, defaultNamespace);
            var separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{separatedNamespace}{key}".ToLowerInvariant();
        }

        private static string GetQueueName(string assemblyName, Type type, string defaultNamespace)
        {
            var (@namespace, key) = GetNamespaceAndKey(type, defaultNamespace);
            var separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{assemblyName}/{separatedNamespace}{key}".ToLowerInvariant();
        }

        private static (string @namespace, string key) GetNamespaceAndKey(Type type, string defaultNamespace)
        {
            var attribute = type.GetCustomAttribute<MessageNamespaceAttribute>();
            var @namespace = attribute?.Namespace ?? defaultNamespace;
            var key = string.IsNullOrWhiteSpace(attribute?.Key) ? type.Name.Underscore() : attribute.Key;

            return (@namespace, key);
        }
    }
}
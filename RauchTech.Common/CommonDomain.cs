using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Demo.Common.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using System;
using System.Globalization;

namespace Demo.Common
{
    public static class CommonDomain
    {
        #region Constants
        private const string AppInsightsPropName = "{0}:AppInsights";

        private const string LogAnalyticsAuthId = "{0}:LogAnalyticsAuthId";

        private const string LogAnalyticsWorkspaceId = "{0}:LogAnalyticsWorkspaceId";
        #endregion

        #region Properties
        private static IConfiguration Configuration { get; set; }
        #endregion

        public static IServiceCollection RegisterCommons(this IServiceCollection services, string configPrefix, HostBuilderContext context)
        {
            return context is null
                 ? throw new ArgumentNullException(nameof(context))
                 : services.RegisterCommons(configPrefix, context.Configuration);
        }

        public static IServiceCollection RegisterCommons(this IServiceCollection services, string configPrefix, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(configPrefix))
            {
                throw new ArgumentNullException(nameof(configPrefix));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Configuration = configuration;

            return services.AddSingleton(configuration)
                            .AddScoped<ICustomLogFactory, CustomLog>()
                            .ConfigureApplicationInsights(configPrefix)
                            .RegisterTelemetryClient(configPrefix, $"{configPrefix}")
                            .ConfigureSerilog(configPrefix);
        }

        public static IServiceCollection RegisterTelemetryClient(this IServiceCollection services, string configPrefix, string roleName)
        {
            if (string.IsNullOrWhiteSpace(configPrefix))
            {
                throw new ArgumentNullException(nameof(configPrefix));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            string instrumentationKey = GetConfiguration(AppInsightsPropName, configPrefix);

            if (!string.IsNullOrEmpty(instrumentationKey))
            {
                _ = services.AddScoped(_ =>
                {
                    TelemetryClient client = new TelemetryClient(TelemetryConfiguration.CreateDefault())
                    {
                        InstrumentationKey = instrumentationKey
                    };

                    client.Context.Cloud.RoleName = roleName;
                    return client;
                });
            }

            return services;
        }

        private static IServiceCollection ConfigureApplicationInsights(this IServiceCollection services, string configPrefix)
        {
            string instrumentationKey = GetConfiguration(AppInsightsPropName, configPrefix);

            if (!string.IsNullOrEmpty(instrumentationKey))
            {
                _ = services.AddScoped(_ =>
                {
                    TelemetryClient client = new TelemetryClient(TelemetryConfiguration.CreateDefault())
                    {
                        InstrumentationKey = instrumentationKey
                    };

                    client.Context.Cloud.RoleName = $"{configPrefix}";
                    return client;
                });
            }

            return services;
        }

        private static IServiceCollection ConfigureSerilog(this IServiceCollection services, string configPrefix)
        {
            return services.AddLogging(builder =>
            {
                string authId;
                string workspaceId;
                string instrumentationKey;
                Logger logger;


                authId = GetConfiguration(LogAnalyticsAuthId, configPrefix);
                workspaceId = GetConfiguration(LogAnalyticsWorkspaceId, configPrefix);
                instrumentationKey = GetConfiguration(AppInsightsPropName, configPrefix);

                if (string.IsNullOrEmpty(instrumentationKey)
                    || string.IsNullOrEmpty(workspaceId)
                    || string.IsNullOrEmpty(authId))
                {
                    throw new ArgumentNullException(nameof(configPrefix));
                }

                logger = new LoggerConfiguration()
                               .MinimumLevel.Verbose()
                               .Enrich.FromLogContext()
                               .Enrich.WithExceptionDetails()
                               .WriteTo.AzureAnalytics(workspaceId, authId, logName: $"{configPrefix}")
                               .CreateLogger();

                _ = builder.AddSerilog(logger);

                //_ = builder.AddApplicationInsights(instrumentationKey)
                //           .AddSerilog(logger, dispose: true);
            });
        }

        private static string GetConfiguration(string propName, string prefix)
        {
            string configKey = string.Format(CultureInfo.InvariantCulture, propName, prefix);
            return Configuration[configKey];
        }
    }
}

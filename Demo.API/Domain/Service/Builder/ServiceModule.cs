using Autofac;
using Autofac.Integration.Mef;
using System;

namespace Demo.API.Domain.Service.Builder
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.Load(builder);
            builder.RegisterMetadataRegistrationSources();


            _ = builder
                .RegisterType<UserService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<ProductService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
               .RegisterType<OrderService>()
               .InstancePerLifetimeScope()
               .PropertiesAutowired();
        }
    }
}

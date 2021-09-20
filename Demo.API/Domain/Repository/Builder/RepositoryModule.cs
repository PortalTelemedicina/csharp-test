using Autofac;
using Autofac.Integration.Mef;
using Demo.API.Domain.Model;
using Demo.Domain.Repository;
using System;

namespace Demo.API.Domain.Repository.Builder
{
    public class RepositoryModule : Module
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
                .RegisterType<UsersRepository>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();


            _ = builder
                .RegisterType<ProductRepository>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<OrdersRepository>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}

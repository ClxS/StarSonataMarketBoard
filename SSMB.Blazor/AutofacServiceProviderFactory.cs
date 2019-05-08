namespace SSMB.Blazor
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Hangfire;
    using Microsoft.Extensions.DependencyInjection;

    public class AutofacServiceProviderFactory : IServiceProviderFactory<IContainer>
    {
        private IContainer container;

        /// <inheritdoc />
        public IContainer CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DataCollection.Modules.DataCollectionModule>();
            builder.Populate(services);
            this.container = builder.Build();

            GlobalConfiguration.Configuration.UseAutofacActivator(this.container);
            return this.container;
        }

        /// <inheritdoc />
        public IServiceProvider CreateServiceProvider(IContainer container)
        {
            return new AutofacServiceProvider(container);
        }
    }
}

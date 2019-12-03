namespace SSMB.DataCollection.Modules
{
    using Autofac;
    using Items;
    using StarSonata.API;

    public class DataCollectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TemporaryItemsProvider>().As<IItemProvider>();
            builder.RegisterType<DataCollection.DataCollectionService>().AsImplementedInterfaces();
            builder.RegisterType<StarSonataApi>()
                   .SingleInstance()
                   .WithParameter(new TypedParameter(typeof(string), "liberty.starsonata.com"));
            builder.RegisterType<MarketCheckService>()
                   .As<IMarketCheckService>()
                   .SingleInstance();
        }
    }
}

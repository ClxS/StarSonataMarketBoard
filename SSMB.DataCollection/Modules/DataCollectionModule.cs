namespace SSMB.DataCollection.Modules
{
    using Autofac;
    using Items;
    using StarSonata.API;

    public class DataCollectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XmlFileItemProvider>().As<IItemProvider>().WithParameter(
                new TypedParameter(typeof(string),
                    "Z:\\Projects\\StarSonataSVN\\Dev\\ssserver\\server\\data\\items"));
            builder.RegisterType<DataCollection.DataCollectionService>().AsImplementedInterfaces();
            builder.RegisterType<StarSonataApi>().SingleInstance()
                   .WithParameter(new TypedParameter(typeof(string), "liberty.starsonata.com"));
            builder.RegisterType<MarketCheckService>().As<IMarketCheckService>().SingleInstance();
        }
    }
}

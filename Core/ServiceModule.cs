using Autofac;
using Core.Abstract;
using Core.Services;

namespace Core
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Services
            builder.RegisterType<DataService>().As<IDataService>().InstancePerRequest();

            //AutoMapper
            var mapper = new MapperConfig().GetMapper();
            builder.RegisterInstance(mapper);

            base.Load(builder);
        }
    }
}

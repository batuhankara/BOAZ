using Autofac;

namespace BAOZ.Api.Modules
{
    public class WebIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {


            builder.RegisterModule<UserModule>();

        }

    }
}

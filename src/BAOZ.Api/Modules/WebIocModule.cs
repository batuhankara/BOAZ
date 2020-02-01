using Autofac;
using User.Application.Modules;

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

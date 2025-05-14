using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CitasMedicasFront.StartupOwin))]

namespace CitasMedicasFront
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}

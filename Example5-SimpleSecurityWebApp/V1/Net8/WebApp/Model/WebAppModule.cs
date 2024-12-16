using System.Reflection;

namespace WebApp.Model
{
    public class WebAppModule : ServiceBricks.Module
    {
        public static WebAppModule Instance = new WebAppModule();

        public WebAppModule()
        {
            AutomapperAssemblies = new List<Assembly>()
            {
                typeof(WebAppModule).Assembly
            };
        }
    }
}
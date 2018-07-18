using AppointmentBot.Helpers;
using AutoMapper;
using System.Web.Http;

namespace AppointmentBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ////Configure Automapper
            //AutoMapperConfiguration.Configure();
        }
        public class AutoMapperConfiguration
        {
            public static void Configure()
            {
                Mapper.Initialize(x =>
                {
                    x.AddProfile<MappingProfile>();
                });
            }
        }
    }
}

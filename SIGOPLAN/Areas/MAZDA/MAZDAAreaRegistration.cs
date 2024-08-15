using System.Web.Mvc;

namespace SIGOPLAN.Areas.MAZDA
{
    public class MAZDAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MAZDA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MAZDA_default",
                "MAZDA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
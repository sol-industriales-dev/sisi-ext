using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol
{
    public class EnkontrolAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Enkontrol";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Enkontrol_default",
                "Enkontrol/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
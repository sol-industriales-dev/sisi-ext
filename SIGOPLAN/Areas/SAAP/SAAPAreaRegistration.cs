using System.Web.Mvc;

namespace SIGOPLAN.Areas.SAAP
{
    public class SAAPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SAAP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SAAP_default",
                "SAAP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
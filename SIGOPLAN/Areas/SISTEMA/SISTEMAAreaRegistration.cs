using System.Web.Mvc;

namespace SIGOPLAN.Areas.SISTEMA
{
    public class SISTEMAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SISTEMA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SISTEMA_default",
                "SISTEMA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
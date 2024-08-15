using System.Web.Mvc;

namespace SIGOPLAN.Areas.SubContratistas
{
    public class SubContratistasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SubContratistas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SubContratistas_default",
                "SubContratistas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
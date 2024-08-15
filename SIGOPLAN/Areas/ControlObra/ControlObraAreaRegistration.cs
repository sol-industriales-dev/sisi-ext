using System.Web.Mvc;

namespace SIGOPLAN.Areas.ControlObra
{
    public class ControlObraAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ControlObra";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ControlObra_default",
                "ControlObra/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
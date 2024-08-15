using System.Web.Mvc;

namespace SIGOPLAN.Areas.GestorArchivos
{
    public class GestorArchivosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GestorArchivos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GestorArchivos_default",
                "GestorArchivos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
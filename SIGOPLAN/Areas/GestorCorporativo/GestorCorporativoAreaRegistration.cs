using System.Web.Mvc;

namespace SIGOPLAN.Areas.GestorCorporativo
{
    public class GestorCorporativoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GestorCorporativo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GestorCorporativo_default",
                "GestorCorporativo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using System.Web.Mvc;

namespace SIGOPLAN.Areas.ReportesContabilidad
{
    public class ReportesContabilidadAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReportesContabilidad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReportesContabilidad_default",
                "ReportesContabilidad/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
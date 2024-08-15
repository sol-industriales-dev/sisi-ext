using Core.DTO;
using Core.Entity.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias
{
    public class ConfiguracionDiariaController : BaseController
    {

        #region Factory
      
        PrecioDieselFactoryServices precioDieselFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            precioDieselFactoryServices = new PrecioDieselFactoryServices();
            base.OnActionExecuting(filterContext);

        }
        // GET: ConfiguracionDiaria
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveOrUpdate_Ritmo(tblM_CapPrecioDiesel obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.fecha = DateTime.Now;
                obj.idUsuario = vSesiones.sesionUsuarioDTO.id;
                precioDieselFactoryServices.getPrecioDieselService().Guardar(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(1));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //SaveOrUpdate_PrecioDiesel
    }
}
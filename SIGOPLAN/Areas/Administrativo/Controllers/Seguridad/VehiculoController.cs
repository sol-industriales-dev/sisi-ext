using Core.Entity.Administrativo.Seguridad;
using Core.Enum;
using Core.Enum.Administracion.Seguridad;
using Data.Factory.Administracion.Seguridad;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class VehiculoController : BaseController
    {
        VehiculoFactoryService vfs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            vfs = new VehiculoFactoryService();
            base.OnActionExecuting(filterContext);
        }
        // GET: Administrativo/Vehiculo
        public ActionResult Revision()
        {
            return View();
        }
        public ActionResult getObjVehiculo(string cc, string eco, DateTime fecha)
        {
             var result = new Dictionary<string, object>();
            try
            {
                var v = vfs.getVehiculoService().getVehiculo(cc, eco, fecha);
                var lstP = vfs.getVehiculoService().getLstPartes();
                var lstO = new List<tblS_Observaciones>();
                if (v.id.Equals(0))
                    lstP.ForEach(p =>
                    {
                        lstO.Add(new tblS_Observaciones() { idParte = p.id, idTipo = p.idTipo, });
                    });
                else
                    lstO = vfs.getVehiculoService().getLstObs(v.id);
                result.Add("vehiculo", v);
                result.Add("obs", lstO.Select(o => new {
                    id = o.id,
                    idParte = o.idParte,
                    idTipo = o.idTipo,
                    idVehiculo = o.idVehiculo,
                    actual = o.actual,
                    anterior = o.anterior,
                    observaciones = o.observaciones ?? string.Empty,
                    tipo = EnumHelper.GetDescription((TipoPiezaEnum)o.idTipo),
                    parte = lstP.FirstOrDefault(w => w.id.Equals(o.idParte)).parte
                }).ToList());
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region combobox
        public ActionResult fillCboEconomico(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, vfs.getVehiculoService().fillCboEconomico(cc));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillComboTipoLicencia()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, Enum.GetValues(typeof(TipoLicenciaEnum)).Cast<TipoLicenciaEnum>().ToList().Select(x => new
                {
                    Text = x.GetDescription(),
                    Value = x.GetHashCode(),
                    Prefijo = x.GetDescription()
                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillComboComentario()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, Enum.GetValues(typeof(ComentarioEnum)).Cast<ComentarioEnum>().ToList().Select(x => new
                {
                    Text = x.GetDescription(),
                    Value = x.GetHashCode(),
                    Prefijo = x.GetDescription()
                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
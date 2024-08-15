using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Componentes
{
    public class CatSubConjuntoController : BaseController
    {


        #region Factory
        SubConjuntoFactoryServices subConjuntoFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            subConjuntoFactoryServices = new SubConjuntoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: CatSubConjunto
        public ActionResult Index()
        {
            ViewBag.pagina = "catalogo";
            return View();
        }
        public ActionResult FillGrid_SubConjunto(tblM_CatSubConjunto subConjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = subConjuntoFactoryServices.getSubConjuntoService().FillGridSubConjunto(subConjunto).Select(x =>
                    new
                    {
                        id = x.id,
                        descripcion = x.descripcion,
                        conjunto = x.conjunto.descripcion,
                        conjuntoID = x.conjunto.id,
                        idPosicion = x.posicionID,
                        prefijo = x.prefijo,
                        //posicion = x.posicionID > 0 ? EnumExtensions.GetDescription((PosicionesEnum)Convert.ToInt32(x.posicionID)) : "No Aplica",
                        estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO"
                    });

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                result.Add("rows", listResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCbo_Posiciones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<PosicionesEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCbo_Conjunto(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, subConjuntoFactoryServices.getSubConjuntoService().FillCboConjuntos(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOrUpdate_SubConjunto(tblM_CatSubConjunto obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                subConjuntoFactoryServices.getSubConjuntoService().Guardar(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
using Core.DAO.RecursosHumanos.Mural;
using Core.Entity.Administrativo.RecursosHumanos.Mural;
using Data.Factory.RecursosHumanos.Mural;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Mural
{
    public class MuralController : BaseController
    {
        private IMuralDAO muralFS = new MuralFactoryServices().GetMuralService();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            muralFS = new MuralFactoryServices().GetMuralService();
            base.OnActionExecuting(filterContext);
        }

        #region Mural no canvas
        public ActionResult Mural2()
        {
            return View();
        }

        public ActionResult Mural()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EliminarSeccion(int idSeccion)
        {
            var r = muralFS.EliminarSeccion(idSeccion);
            return Json(r);
        }

        [HttpPost]
        public JsonResult GuardarSeccion(tblRH_Mural_Seccion seccion)
        {
            var r = muralFS.GuardarSeccion(seccion);
            return Json(r);
        }

        [HttpPost]
        public JsonResult EliminarPostIt(int idPostIt)
        {
            var r = muralFS.EliminarPostIt(idPostIt);
            return Json(r);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SavePostIt(tblRH_Mural_PostIt postIt)
        {
            var r = muralFS.SavePostIt(postIt);
            return Json(r);
        }

        [HttpPost]
        public JsonResult SaveMural(tblRH_Mural mural, List<tblRH_Mural_PostIt> postIt)
        {
            var r = muralFS.SaveMural(mural, postIt);
            return Json(r);
        }

        [HttpGet]
        public JsonResult GetMural(int idMural)
        {
            var r = muralFS.GetMural(idMural);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CrearMural(tblRH_Mural mural)
        {
            var r = muralFS.CrearMural(mural);
            return Json(r);
        }

        [HttpPost]
        public JsonResult CboxMural()
        {
            var r = muralFS.CboxMural();
            return Json(r);
        }
        #endregion
        #region Mural canvas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Workspace()
        {
            return View();
        }
        //[HttpPost]
        //public JsonResult addWorkSpace()
        //{
        //    var r = muralFS.EliminarSeccion(idSeccion);
        //    return Json(r);
        //}
        public JsonResult setMural(int id,string datos,string icono)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.setMural(id, datos,icono);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {

                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getMural(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = muralFS.getMural(id);
                result.Add("data", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {

                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getMuralList(bool propio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = muralFS.getMuralList(propio);
                result.Add("data", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {

                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult createNewMural(string nombre, string desc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.createNewMural(nombre,desc);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult renameMural(int id,string nombre)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.renameMural(id,nombre);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult duplicateNewMural(int id, string nombre, string desc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.duplicateNewMural(id,nombre, desc);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteMural(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.deleteMural(id);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult setUsuarioMural(int idUsuario, int idMural, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.setUsuarioMural(idUsuario, idMural, tipo);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateUsuarioMural(int id , int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.updateUsuarioMural(id , tipo);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteUsuarioMural(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                muralFS.deleteUsuarioMural(id);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getUserMuralList(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = muralFS.getUserMuralList(id);
                result.Add("data", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {

                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTipoPermiso(int muralID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tipo = muralFS.getTipoPermiso(muralID);
                result.Add("tipo", tipo);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {

                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
using Core.DTO;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Maquinaria
{
    public class CatMarcasEquipoController : BaseController
    {
        #region Atributos
        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        #endregion

        #region Factory
        MarcaEquipoFactoryServices marcaEquipoFactoryServices;
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            marcaEquipoFactoryServices = new MarcaEquipoFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: MarcasMaquinas
        public ActionResult Index()
        {
            var usuarioDTO = vSesiones.sesionUsuarioDTO;
            if (usuarioDTO != null)
            {
                ViewBag.pagina = "catalogo";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult FillGrid_MarcaEquipo(tblM_CatMarcaEquipo marcaEquipo)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var grupos = marcaEquipoFactoryServices.getMarcaEquipoService().FillCboGrupoMaquinaria(true);
                var listResult = marcaEquipoFactoryServices.getMarcaEquipoService().FillGridMarcaEquipo(marcaEquipo).Select(x => new { id = x.id, descripcion = x.descripcion, idGrupo = x.grupoEquipoID, grupo =  x.grupo ,estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });

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
        public ActionResult SaveOrUpdate_MarcaEquipo(MarcaDTO obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {

                tblM_CatGrupoMaquinaria grupo = marcaEquipoFactoryServices.getMarcaEquipoService().getEntidadGrupo(obj.grupoEquipoID);
                tblM_CatMarcaEquipo entidad = new tblM_CatMarcaEquipo
                {
                    id = obj.id,
                    descripcion = obj.descripcion,
                    estatus = obj.estatus,
                    //grupo = new List<tblM_CatGrupoMaquinaria>()
                };


                marcaEquipoFactoryServices.getMarcaEquipoService().Guardar(entidad, grupo);

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

        public ActionResult FillCboGrupoMaquinaria(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, marcaEquipoFactoryServices.getMarcaEquipoService().FillCboGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
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
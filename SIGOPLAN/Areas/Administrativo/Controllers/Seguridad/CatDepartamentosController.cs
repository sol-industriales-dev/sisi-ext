using Core.DAO.Administracion.Seguridad.CatDepartamentos;
using Core.DTO.Administracion.Seguridad.CatDepartamentos;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Data.Factory.Administracion.Seguridad;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class CatDepartamentosController : BaseController
    {
        private CatDepartamentosFactoryService CatDeptFS;

        //private ICapacitacionDAO capacitacionService;
        //private CatHorasHombreFactoryService CatHorasHombreFS;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CatDeptFS = new CatDepartamentosFactoryService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getClaveDepto()
        {
            return Json(CatDeptFS.getCatDepartamentosService().getClaveDepto(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAreaOperativa()
        {
            try
            {
                var data = CatDeptFS.getCatDepartamentosService().getAreaOperativa();
                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarDepartamento(tblS_CatDepartamentos objDepartamento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objDepartamento != null)
                {
                    if (objDepartamento.id > 0)
                    {
                        //var ExisteRegistro = CatDeptFS.getCatDepartamentosService().EsRegistroUnico(objDepartamento);
                        //if (ExisteRegistro)
                        //    throw new Exception("Ya existe un registro con esta información.");

                        var ActualizarDepartamento = CatDeptFS.getCatDepartamentosService().ActualizarDepartamento(objDepartamento);
                        if (!ActualizarDepartamento)
                        {
                            result.Add(MESSAGE, "Ocurrió un error al actualizar.");
                            result.Add(SUCCESS, false);
                        }
                        else
                        {
                            result.Add(SUCCESS, true);
                        }
                    }
                    else
                    {
                        var ExisteRegistro = CatDeptFS.getCatDepartamentosService().EsRegistroUnico(objDepartamento);
                        if (ExisteRegistro)
                            throw new Exception("Ya existe un registro con esta información.");

                        var CrearDepartamento = CatDeptFS.getCatDepartamentosService().CrearDepartamento(objDepartamento);
                        if (!CrearDepartamento)
                        {
                            result.Add(MESSAGE, "Ocurrió un error al registrar.");
                            result.Add(SUCCESS, false);
                        }
                        else
                        {
                            result.Add(SUCCESS, true);
                        }
                    }
                }
                else
                {
                    result.Add(MESSAGE, "Ocurrió un error al registrar/actualizar.");
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatDepartamentos(CatDepartamentosDTO objCatDepartamentos)
        {
            try
            {
                var dataCatDepartamentos = CatDeptFS.getCatDepartamentosService().GetCatDepartamentos(objCatDepartamentos);
                var lstCatDepartamentos = dataCatDepartamentos.Select(x => new
                {
                    id = x.id,
                    cc = x.cc,
                    clave_depto = x.clave_depto,
                    departamento = ObtenerDepartamento(x.clave_depto),
                    idEmpresa = x.idEmpresa,
                    idAreaOperativa = x.idAreaOperativa,
                    NombreAreaOperativa = x.NombreAreaOperativa,
                    descripcion = x.descripcion,
                    esActivo = x.esActivo
                }).ToList();

                //conjunto = x.subconjunto.conjunto.descripcion,

                result.Add("lstCatDepartamentos", lstCatDepartamentos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<string> ObtenerDepartamento(int clave_depto)
        {
            return CatDeptFS.getCatDepartamentosService().ObtenerDepartamento(clave_depto);
        }

        public List<string> ObtenerAreaOperativa(int idAreaOperativa)
        {
            return CatDeptFS.getCatDepartamentosService().ObtenerAreaOperativa(idAreaOperativa);
        }

        public ActionResult ActivarDesactivarDepartamento(int id)
        {
            try
            {
                if (id > 0)
                {
                    var Eliminar = CatDeptFS.getCatDepartamentosService().ActivarDesactivarDepartamento(id);
                    if (!Eliminar)
                    {
                        result.Add(MESSAGE, "Ocurrió un error al eliminar el registro.");
                        result.Add(SUCCESS, false);
                    }
                    else
                    {
                        result.Add(SUCCESS, true);
                    }
                }
                else
                {
                    result.Add(MESSAGE, "Ocurrió un error al lanzar la petición.");
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCC()
        {
            try
            {
                var lstClaveDepto = CatDeptFS.getCatDepartamentosService().getCC();
                result.Add(ITEMS, lstClaveDepto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerComboCCAmbasEmpresas()
        {
            return Json(CatDeptFS.getCatDepartamentosService().ObtenerComboCCAmbasEmpresas(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerCCporDepartamento(string cc, int idEmpresa)
        {
            return Json(CatDeptFS.getCatDepartamentosService().ObtenerCCporDepartamento(cc, idEmpresa), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerCCporDepartamentoEditar(string claveDepto, int idEmpresa)
        {
            return Json(CatDeptFS.getCatDepartamentosService().ObtenerCCporDepartamentoEditar(claveDepto, idEmpresa), JsonRequestBehavior.AllowGet);
        }

    }
}
using Core.Entity.Administrativo.Seguridad.CatHorasHombre;
using Core.Enum;
using Core.Enum.Administracion.Seguridad.Evaluacion;
using Data.Factory.Administracion.Seguridad.CatHorasHombre;
using Data.Factory.Administracion.Seguridad.Incidencias;
using Infrastructure.DTO;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SIGOPLAN.Areas.Administrativo.Controllers.Seguridad;
using Core.DAO.Administracion.Seguridad;
using Data.Factory.Administracion.Seguridad.Capacitacion;
using Core.DTO;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class CatHorasHombreController : BaseController
    {
        private ICapacitacionDAO capacitacionService;
        private CatHorasHombreFactoryService CatHorasHombreFS;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            capacitacionService = new CapacitacionFactoryService().GetCapacitacionService();
            CatHorasHombreFS = new CatHorasHombreFactoryService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getCC()
        {
            try
            {
                var lstCC = CatHorasHombreFS.getCatHorasHombreService().getCC();
                result.Add(ITEMS, lstCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRoles()
        {
            try
            {
                var lstRoles = CatHorasHombreFS.getCatHorasHombreService().getRoles();
                result.Add(ITEMS, lstRoles);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objHorasHombre != null)
                {
                    if (objHorasHombre.id > 0)
                    {
                        var ActualizarHorasHombre = CatHorasHombreFS.getCatHorasHombreService().ActualizarHorasHombre(objHorasHombre);
                        if (!ActualizarHorasHombre)
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
                        var ExisteRegistro = CatHorasHombreFS.getCatHorasHombreService().ValidarRegistroUnico(objHorasHombre);
                        if (ExisteRegistro)
                            throw new Exception("Ya existe un registro con esta información.");

                        var CrearHorasHombre = CatHorasHombreFS.getCatHorasHombreService().CrearHorasHombre(objHorasHombre);
                        if (!CrearHorasHombre)
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

        [HttpGet]
        public ActionResult GetHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            try
            {
                var HorasHombre = CatHorasHombreFS.getCatHorasHombreService().GetHorasHombre(objHorasHombre);
                if(vSesiones.sesionEmpresaActual==3 || vSesiones.sesionEmpresaActual==6)
                {

                    var lstHorasHombre = HorasHombre.Select(x => new
                    {
                        id = x.id,
                        cc = x.cc,
                        idEmpresa =vSesiones.sesionEmpresaActual == 3 ? "COLOMBIA" : "PERÚ",
                        //CC = x.catCC.cc + " - " + x.catCC.descripcion,
                        clave_depto = x.clave_depto,
                        departamento = ObtenerDepartamento(x.clave_depto),
                        descripcion = EnumHelper.GetDescription((RolEnum)x.idGrupo),
                        idGrupo = x.idGrupo,
                        esActivo = x.esActivo,
                        horasDia = x.horas,
                        fechaInicio = x.fechaInicio
                    }).ToList();
                    result.Add("lstHorasHombre", lstHorasHombre);
                }else
                {
                    var lstHorasHombre = HorasHombre.Select(x => new
                    {
                        id = x.id,
                        cc = x.cc,
                        idEmpresa = x.idEmpresa == 1 ? "CONSTRUPLAN" : "ARRENDADORA",
                        //CC = x.catCC.cc + " - " + x.catCC.descripcion,
                        clave_depto = x.clave_depto,
                        departamento = ObtenerDepartamento(x.clave_depto),
                        descripcion = EnumHelper.GetDescription((RolEnum)x.idGrupo),
                        idGrupo = x.idGrupo,
                        esActivo = x.esActivo,
                        horasDia = x.horas,
                        fechaInicio = x.fechaInicio
                    }).ToList();
                    result.Add("lstHorasHombre", lstHorasHombre);
                }
                //var lstHorasHombre = HorasHombre.Select(x => new
                //{
                //    id = x.id,
                //    cc = x.cc,
                //    idEmpresa = x.idEmpresa == 1 ? "CONSTRUPLAN" : "ARRENDADORA",
                //    //CC = x.catCC.cc + " - " + x.catCC.descripcion,
                //    clave_depto = x.clave_depto,
                //    departamento = ObtenerDepartamento(x.clave_depto),
                //    descripcion = EnumHelper.GetDescription((RolEnum)x.idGrupo),
                //    idGrupo = x.idGrupo,
                //    esActivo = x.esActivo,
                //    horasDia = x.horas,
                //    fechaInicio = x.fechaInicio
                //}).ToList();

                //result.Add("lstHorasHombre", lstHorasHombre);
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
            return CatHorasHombreFS.getCatHorasHombreService().ObtenerDepartamento(clave_depto);
        }

        public ActionResult EliminarHorasHombre(int id)
        {
            try
            {
                if (id > 0)
                {
                    var Eliminar = CatHorasHombreFS.getCatHorasHombreService().EliminarHorasHombre(id);
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

        public ActionResult ObtenerComboCCAmbasEmpresas()
        {
            return Json(CatHorasHombreFS.getCatHorasHombreService().ObtenerComboCCAmbasEmpresas(), JsonRequestBehavior.AllowGet);
        }



        //public ActionResult GetCCHorasHombre(int idCC)
        //{
        //    try
        //    {
        //        var dataCC = CatHorasHombreFS.getCatHorasHombreService().GetCCHorasHombre(idCC);
        //        var descripcionCC = dataCC.Select(x => new
        //        {
        //            descripcion = x.descripcion.Trim()
        //        }).ToList();

        //        result.Add("descripcionCC", descripcionCC);
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult ObtenerAreasPorCC(List<string> ccsCplan, int idEmpresa)
        {
            return Json(CatHorasHombreFS.getCatHorasHombreService().ObtenerAreasPorCC(ccsCplan, idEmpresa), JsonRequestBehavior.AllowGet);
        }
    }
}
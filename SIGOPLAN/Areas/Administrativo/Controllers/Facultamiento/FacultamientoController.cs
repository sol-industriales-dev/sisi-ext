using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Facultamiento;
using Core.Entity.Principal.Alertas;
using Core.Enum;
using Core.Enum.Administracion.Facultamiento;
using Core.Enum.Maquinaria.ConciliacionHorometros;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Alertas;
using Data.Factory.Administracion.Facultamiento;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Usuarios;
using Data.Factory.RecursosHumanos.Captura;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Facultamiento
{
    public class FacultamientoController : BaseController
    {
        FacultamientoFactoryServices ffs;
        CentroCostosFactoryServices ccfs;
        AlertaFactoryServices affs;
        UsuarioFactoryServices uffs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ffs = new FacultamientoFactoryServices();
            ccfs = new CentroCostosFactoryServices();
            affs = new AlertaFactoryServices();
            uffs = new UsuarioFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Administrativo/Facultamiento
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult Historico()
        {
            return View();
        }
        public ActionResult Autorizacion()
        {
            return View();
        }
        public ActionResult Catalogo()
        {
            return View();
        }
        public ActionResult Asignacion()
        {
            return View();
        }
        public ActionResult HistoricoFA()
        {
            return View();
        }
        public ActionResult AutorizacionFA()
        {
            return View();
        }
        public ActionResult PorEmpleado()

        {
            return View();
        }
        public ActionResult Grupos()
        {
            return View();
        }
        public ActionResult getLstGestion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstcc = ccfs.getCentroCostosService().ListCC();
                result.Add("lstGestion", ffs.getFacutamientoService().getCuadro()
                    .Where(w => lstcc.Any(c => c.Value.Equals(w.cc)))
                    .Select(x => new
                    {
                        id = x.id,
                        cc = x.cc,
                        ccNombre = lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text,
                        obra = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Construplan) && x.cc.ParseInt() < 101 ? lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text : x.obra,
                        fecha = x.fecha.ToShortDateString(),
                        estatus = ffs.getFacutamientoService().geSTtAuth(x.id)
                    }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstGestionNoAuth()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var thisUser = getUsuario();
                var nombre = (thisUser.nombre + thisUser.apellidoPaterno + thisUser.apellidoMaterno).ToUpper().Replace(System.Environment.NewLine, string.Empty);
                var lstcc = ccfs.getCentroCostosService().ListCC();
                var data = ffs.getFacutamientoService().getCuadroNoR()
                    .Where(w => !ffs.getFacutamientoService().getAutorizacion(w.id))
                    .Where(e => ffs.getFacutamientoService().isUsuarioAutorisable(e.id, nombre))
                    .Select(x => new
                    {
                        id = x.id,
                        cc = x.cc,
                        ccNombre = ccfs.getCentroCostosService().getNombreCCFix(x.cc),
                        obra = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Construplan) && x.cc.ParseInt() < 101 ? lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text : x.obra,
                        fecha = x.fecha.ToShortDateString(),
                        estatus = x.estatus == 3
                    });
                result.Add("lstGestion", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstAutorizacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var thisUser = getUsuario();
                result.Add("lstAuth", ffs.getFacutamientoService().getLstAutorizacion(id, (thisUser.nombre + thisUser.apellidoPaterno + thisUser.apellidoMaterno).ToUpper().Replace(System.Environment.NewLine, string.Empty)));
                result.Add("isAuth", ffs.getFacutamientoService().getAutorizacion(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getHistAutorizacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("lstAuth", ffs.getFacutamientoService().getLstAutorizacion(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCCCompleto(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstcc = ccfs.getCentroCostosService().ListCC();



                result.Add("lstGestion", ffs.getFacutamientoService().getCCCompleto(cc, fechaInicio, fechaFin).Select(x => new
                {
                    id = x.id,
                    cc = x.cc,
                    ccNombre = lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text,
                    obra = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Construplan) && x.cc.ParseInt() < 101 ? lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text : x.obra,
                    fecha = x.fecha.ToShortDateString(),
                    registro = x.fechaRegistro.ToShortDateString(),
                    //estatus = x.estatus.Equals(1) ? "En espera" : x.estatus.Equals(2) ? "Autorizado" : "Rechazado"
                    estatus = ffs.getFacutamientoService().geSTtAuth(x.id),
                    comentario = ffs.getFacutamientoService().ObtenerMotivoRechazo(x.id)
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCuadro(string cc, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objCuadro = ffs.getFacutamientoService().getCuadro(cc, fecha);
                if (objCuadro.estatus.Equals(3))
                    objCuadro = new tblFa_CatFacultamiento() { cc = cc };
                var lstMonto = ffs.getFacutamientoService().getMonto(objCuadro.id, cc);
                var lstPuesto = ffs.getFacutamientoService().GetLstPuesto(objCuadro.id);
                var lstAutorizacion = new List<tblFa_CatAutorizacion>();
                lstMonto.ForEach(e =>
                {
                    lstAutorizacion.AddRange(ffs.getFacutamientoService().getAutorizacion(e.id, e.renglon));
                });
                result.Add("cuadro", objCuadro);
                result.Add("fecha", objCuadro.fecha.ToShortDateString());
                var isAdmin = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) || cc.ParseInt(101) < 100;
                var isEditObra = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) ? false : isAdmin;
                result.Add("isEditObra", isEditObra);
                result.Add("isAdmin", isAdmin);
                if (isAdmin)
                {
                    result.Add("lstAdmin", lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Administrativo).Select(x => new
                    {
                        monto = x,
                        lstAuto = lstAutorizacion.Where(w => x.renglon == w.renglon && x.id == w.idMonto).ToList()
                    }).ToList());
                }
                else
                {
                    result.Add("lstRefact", lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Refacciones).Select(x => new
                    {
                        monto = x,
                        lstAuto = lstAutorizacion.Where(w => w.renglon == x.renglon && x.id == w.idMonto).ToList()
                    }).ToList());
                    result.Add("lstMat", lstMonto.Where(w => w.idTabla == (int)TipoTablaEnum.Materiales).Select(x => new
                    {
                        monto = x,
                        lstAuto = lstAutorizacion.Where(w => w.renglon == x.renglon && x.id == w.idMonto).ToList()
                    }).ToList());
                }
                result.Add("lstPuesto", lstPuesto
                    .OrderBy(o => o.idTabla)
                    .ThenBy(o => o.orden)
                    .Select(p => new
                    {
                        id = p.id,
                        idFacultamiento = p.idFacultamiento,
                        idTabla = p.idTabla,
                        tipo = EnumHelper.GetDescription((TipoPuestoEnum)p.idTabla),
                        orden = p.orden,
                        auth = EnumHelper.GetDescription((TipoAutorizacionEnum)p.orden),
                        puesto = p.puesto
                    }));
                result.Add("lstAuth", ffs.getFacutamientoService().getLstAuth(objCuadro.id));
                result.Add("isAuth", ffs.getFacutamientoService().getAutorizacion(objCuadro.id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPuesto(string nombre)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("puesto", ffs.getFacutamientoService().getPuestoFromNombreCompleto(nombre));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveFacultamiento(tblFa_CatFacultamiento obj, List<tblFa_CatMonto> lstMonto, List<tblFa_CatAutorizacion> lstAut, List<tblFa_CatAuth> lstAuth, List<tblFa_CatPuesto> lstPuesto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var anterio = ffs.getFacutamientoService().getCuadro(obj.cc, obj.fecha);
                if (!anterio.id.Equals(0) && anterio.estatus.Equals(1))
                    ffs.getFacutamientoService().deleteNoCompleto(obj.cc);
                result.Add("vobo", ffs.getFacutamientoService().saveFacultamiento(obj, lstAut, lstMonto, lstAuth, lstPuesto, getUsuario().id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setAutorizacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("vobo", ffs.getFacutamientoService().setAutorizacion(id, getUsuario().id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setRechazo(int id, string motivo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (motivo == null || motivo.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó el facultamiento. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                result.Add("vobo", ffs.getFacutamientoService().setRechazo(id, motivo));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sendCorreo(tblFa_CatAuth vobo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var pdf = (List<Byte[]>)Session["downloadPDF"];
                var usuarioEnvia = getUsuario();
                var usuarioRecibe = uffs.getUsuarioService().UserByName(vobo.nombre);
                var cc = ffs.getFacutamientoService().getCuadro(vobo.idFacultamiento).cc;
                result.Add(SUCCESS, ffs.getFacutamientoService().sendCorreo(usuarioRecibe.id, usuarioEnvia.id, pdf, cc, vobo.orden));

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            finally
            {
                Session["downloadPDF"] = null;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult remMonto(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(SUCCESS, ffs.getFacutamientoService().remMonto(id));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleadosSigoplan(string term)
        {
            return Json(ffs.getFacutamientoService().getEmpleadosSigoplan(term), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleadosSigoplanNOAG(string term)
        {
            return Json(ffs.getFacutamientoService().getEmpleadosSigoplanNOAG(term), JsonRequestBehavior.AllowGet);
        }
        public ActionResult geDesctPuesto(string term)
        {
            return Json(ffs.getFacutamientoService().geDesctPuesto(term), JsonRequestBehavior.AllowGet);
        }
        #region combobox
        public ActionResult fillComboTitulo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, Enum.GetValues(typeof(TituloEnum)).Cast<TituloEnum>().ToList().Select(x => new
                {
                    Text = x.GetDescription(),
                    Value = x.GetHashCode()
                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getComboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ffs.getFacutamientoService().getComboCC());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getComboCCEnkontrol()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ffs.getFacutamientoService().getComboCCEnkontrol());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        // Comparativa Facultamiento update
        public ActionResult getFacAnterior(string cc, DateTime fecha, int idActual)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var isAdmin = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora) || cc.ParseInt(101) < 100;
                var esArrendadora = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Arrendadora);
                var lstcc = ccfs.getCentroCostosService().ListCC();

                // FACULTAMIENTO ANTERIOR       
                var anterior = ffs.getFacutamientoService().getCCCompleto(cc).Where(x=>x.estatus==2).OrderByDescending(x=>x.id).Select(x => new
                {
                    id = x.id,
                    cc = x.cc,
                    ccNombre = lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text,
                    obra = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Construplan) && x.cc.ParseInt() < 101 ? lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text : x.obra,
                    fecha = x.fecha.ToShortDateString(),
                    registro = x.fechaRegistro.ToShortDateString(),
                    estatus = "Autorizado"
                }).FirstOrDefault();

                if (anterior != null)
                {

                    var lstMontoAnt = ffs.getFacutamientoService().getMonto(anterior.id, anterior.cc);
                    var lstPuestoAnt = ffs.getFacutamientoService().GetLstPuesto(anterior.id);
                    var lstAutorizacionAnt = new List<tblFa_CatAutorizacion>();
                    lstMontoAnt.ForEach(e =>
                    {
                        lstAutorizacionAnt.AddRange(ffs.getFacutamientoService().getAutorizacion(e.id, e.renglon));
                    });

                    //FACULTAMIENTO ACTUAL
                    var actual = ffs.getFacutamientoService().getCCCompleto(cc).Select(x => new
                    {
                        id = x.id,
                        cc = x.cc,
                        ccNombre = lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text,
                        obra = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Construplan) && x.cc.ParseInt() < 101 ? lstcc.FirstOrDefault(c => c.Value.Equals(x.cc)).Text : x.obra,
                        fecha = x.fecha.ToShortDateString(),
                        registro = x.fechaRegistro.ToShortDateString(),
                        estatus = ffs.getFacutamientoService().geSTtAuth(x.id),
                        usuario = uffs.getUsuarioService().ListUsersById(x.usuarioID).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault().ToString()

                    }).Where(x => x.id == idActual).FirstOrDefault();
                    var lstMontoAct = ffs.getFacutamientoService().getMonto(actual.id, actual.cc);
                    var lstPuestoAct = ffs.getFacutamientoService().GetLstPuesto(actual.id);
                    var lstAutorizacionAct = new List<tblFa_CatAutorizacion>();
                    lstMontoAct.ForEach(e =>
                    {
                        lstAutorizacionAct.AddRange(ffs.getFacutamientoService().getAutorizacion(e.id, e.renglon));
                    });

                    //GENERAL
                    if (isAdmin)
                    {
                        // ANTERIOR   
                        result.Add("lstAdmin", lstMontoAnt.Where(w => w.idTabla == (int)TipoTablaEnum.Administrativo).Select(x => new
                        {
                            monto = x,
                            lstAuto = lstAutorizacionAnt.Where(w => x.renglon == w.renglon && x.id == w.idMonto).ToList()
                        }).ToList());

                        //ACTUAL
                        result.Add("lstAdminActual", lstMontoAct.Where(w => w.idTabla == (int)TipoTablaEnum.Administrativo).Select(x => new
                        {
                            monto = x,
                            lstAuto = lstAutorizacionAct.Where(w => x.renglon == w.renglon && x.id == w.idMonto).ToList()
                        }).ToList());
                    }
                    else
                    {
                        // ANTERIOR
                        result.Add("lstRefactAnt", lstMontoAnt.Where(w => w.idTabla == (int)TipoTablaEnum.Refacciones).Select(x => new
                        {
                            monto = x,
                            lstAuto = lstAutorizacionAnt.Where(w => w.renglon == x.renglon && x.id == w.idMonto).ToList()
                        }).ToList());
                        result.Add("lstMatAnt", lstMontoAnt.Where(w => w.idTabla == (int)TipoTablaEnum.Materiales).Select(x => new
                        {
                            monto = x,
                            lstAuto = lstAutorizacionAnt.Where(w => w.renglon == x.renglon && x.id == w.idMonto).ToList()
                        }).ToList());

                        //ACTUAL
                        result.Add("lstRefactAct", lstMontoAct.Where(w => w.idTabla == (int)TipoTablaEnum.Refacciones).Select(x => new
                        {
                            monto = x,
                            lstAuto = lstAutorizacionAct.Where(w => w.renglon == x.renglon && x.id == w.idMonto).ToList()
                        }).ToList());
                        result.Add("lstMatAct", lstMontoAct.Where(w => w.idTabla == (int)TipoTablaEnum.Materiales).Select(x => new
                        {
                            monto = x,
                            lstAuto = lstAutorizacionAct.Where(w => w.renglon == x.renglon && x.id == w.idMonto).ToList()
                        }).ToList());
                    }

                    result.Add("isAdmin", isAdmin);
                    result.Add("esArrendadora", esArrendadora);

                    result.Add("objAnterior", anterior);
                    result.Add("objPuestoAnt", lstPuestoAnt.OrderBy(o => o.idTabla).ThenBy(o => o.orden).Select(p => new
                    {
                        id = p.id,
                        idFacultamiento = p.idFacultamiento,
                        idTabla = p.idTabla,
                        tipo = EnumHelper.GetDescription((TipoPuestoEnum)p.idTabla),
                        orden = p.orden,
                        auth = EnumHelper.GetDescription((TipoAutorizacionEnum)p.orden),
                        puesto = p.puesto
                    }));

                    result.Add("objActual", actual);
                    result.Add("objPuestoAct", lstPuestoAct.OrderBy(o => o.idTabla).ThenBy(o => o.orden).Select(p => new
                    {
                        id = p.id,
                        idFacultamiento = p.idFacultamiento,
                        idTabla = p.idTabla,
                        tipo = EnumHelper.GetDescription((TipoPuestoEnum)p.idTabla),
                        orden = p.orden,
                        auth = EnumHelper.GetDescription((TipoAutorizacionEnum)p.orden),
                        puesto = p.puesto
                    }));
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(MESSAGE, "No existe version anterior autorizada");
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
    }
}
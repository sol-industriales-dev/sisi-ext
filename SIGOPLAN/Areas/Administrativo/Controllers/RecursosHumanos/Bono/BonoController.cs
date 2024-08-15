using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.RecursosHumanos.Captura;
using Data.Factory.RecursosHumanos.Captura;
using SIGOPLAN.Controllers;
using Core.DTO.Principal.Generales;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.Enum.Principal;
using Infrastructure.Utils;
using Core.Enum.Administracion.Propuesta.Nomina;
using Core.Enum.RecursosHumanos;
using Core.DTO.RecursosHumanos;
using Core.DTO.Utils.Auth;
using Core.DAO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Maquinaria.Reportes;
using Newtonsoft.Json;
using Data.DAO.Principal.Usuarios;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Bono
{
    public class PlantillaDetDTO
    {
        public int empleadoID { get; set; }
        public string empleado { get; set; }
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public int tipoNominaID { get; set; }
        public string tipoNomina { get; set; }
        public int deptoID { get; set; }
        public string depto { get; set; }
        public decimal monto { get; set; }
        public decimal salario { get; set; }
    }
    public class BonoController : BaseController
    {
        #region Factory
        IAditivaDeductivaDAO aditivaDeductivafs;
        IBonoDAO bonofs;
        IUsuarioDAO usuariofs;
        private Data.Factory.RecursosHumanos.ReportesRH.ReportesRHFactoryServices reportesRHFactoryServices;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            aditivaDeductivafs = new AditivaDeductivaFactoryService().getAditivaDeductivaService();
            bonofs = new BonoFactoryService().getBonoService();
            usuariofs = new UsuarioFactoryServices().getUsuarioService();
            reportesRHFactoryServices = new Data.Factory.RecursosHumanos.ReportesRH.ReportesRHFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Administrativo/Bono
        #region Guardar
        public ActionResult GuardarPlantilla(tblRH_BN_Plantilla plan, List<tblRH_BN_Plantilla_Det> lst, List<tblRH_BN_Plantilla_Aut> lstAuth,bool isGestion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = plan.cc.Length == 3 &&
                    lst.Count > 0 && lstAuth.Count > 1
                    && lst.All(a =>
                       a.monto > 0
                    && a.periodicidad > 0) &&
                    lstAuth.All(a => a.aprobadorClave > 0);
                if (esGuardado)
                {
                    lstAuth.ForEach(auth =>
                    {
                        switch (auth.orden)
                        {
                            case 0:
                                auth.aprobadorPuesto = "Responsable del Centro de Costos";
                                auth.tipo = "Solicita";
                                break;
                            case 1:
                                auth.aprobadorPuesto = "Capital Humano";
                                auth.tipo = "VoBo";
                                break;
                            case 2:
                                auth.aprobadorPuesto = "Gerente / Director de Área";
                                auth.tipo = "Autoriza 1";
                                break;
                            case 3:
                                auth.aprobadorPuesto = "Gerente / Director de Área";
                                auth.tipo = "Autoriza 2";
                                break;
                            case 4:
                                auth.aprobadorPuesto = "Director de Línea de Negocios";
                                auth.tipo = "Autoriza 1";
                                break;
                            case 5:
                                auth.aprobadorPuesto = "Director de Línea de Negocios";
                                auth.tipo = "Autoriza 2";
                                break;
                            case 6:
                                auth.aprobadorPuesto = "Alta Dirección";
                                auth.tipo = "Autoriza 1";
                                break;
                            case 7:
                                auth.aprobadorPuesto = "Alta Dirección";
                                auth.tipo = "Autoriza 2";
                                break;
                            default: break;
                        }
                        auth.fecha = DateTime.Now;
                    });
                    lstAuth.FirstOrDefault().autorizando = true;
                    plan.estatus = (int)authEstadoEnum.EnEspera;
                    plan.usuarioCapturoID = vSesiones.sesionUsuarioDTO.id;
                    plan.listDetalle = lst;
                    plan.listAutorizadores = lstAuth;
                    esGuardado = bonofs.CrearPlantilla(plan,isGestion);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarBonoAdministrativo(authDTO auth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var stAuth = (int)authEstadoEnum.Autorizado;
                var plan = bonofs.getPlantilla(auth.idPadre);
                var det = plan.listAutorizadores.FirstOrDefault(w => w.id == auth.idRegistro);
                det.autorizando = false;
                det.estatus = stAuth;
                det.firma = GlobalUtils.CrearFirmaDigital(auth.idPadre, DocumentosEnum.BonoAdministrativo, vSesiones.sesionUsuarioDTO.id);
                if (plan.listAutorizadores.All(a => a.estatus == stAuth))
                {
                    plan.estatus = stAuth;
                }
                else
                {
                    var sigOrden = plan.listAutorizadores.Where(w => w.estatus == (int)authEstadoEnum.EnEspera).Min(m => m.orden);
                    var sigAuth = plan.listAutorizadores.FirstOrDefault(w => w.orden == sigOrden);
                    sigAuth.autorizando = true;
                }
                var exito = bonofs.ActualizarPlantilla(plan);
                result.Add(SUCCESS, exito);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RechazarBonoAdministrativo(authDTO auth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (auth.comentario == null || auth.comentario.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var plan = bonofs.getPlantilla(auth.idPadre);
                var bd = plan.listAutorizadores.FirstOrDefault(w => w.id == auth.idRegistro);
                bd.comentario = auth.comentario;
                bd.estatus = plan.estatus = (int)authEstadoEnum.Rechazado;
                bd.autorizando = false;
                var exito = bonofs.ActualizarPlantilla(plan);
                result.Add(SUCCESS, exito);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarEvaluacion(authDTO auth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var stAuth = (int)authEstadoEnum.Autorizado;
                var plan = bonofs.getEvaluacionByID(auth.idPadre);
                var det = plan.listAutorizadores.FirstOrDefault(w => w.id == auth.idRegistro);
                det.autorizando = false;
                det.estatus = stAuth;
                det.firma = GlobalUtils.CrearFirmaDigital(auth.idPadre, DocumentosEnum.BonoAdministrativo, vSesiones.sesionUsuarioDTO.id);

                if (!plan.listAutorizadores.Any(a => a.estatus == (int)authEstadoEnum.EnEspera))
                {
                    plan.estatus = stAuth;
                }
                else
                {
                    var sigOrden = plan.listAutorizadores.Where(w => w.estatus == (int)authEstadoEnum.EnEspera).Min(m => m.orden);
                    var sigAuth = plan.listAutorizadores.FirstOrDefault(w => w.orden == sigOrden);
                    sigAuth.autorizando = true;
                }
                var exito = bonofs.ActualizarEvaluacion(plan);
                result.Add(SUCCESS, exito);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RechazarEvaluacion(authDTO auth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (auth.comentario == null || auth.comentario.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var plan = bonofs.getEvaluacionByID(auth.idPadre);
                plan.estatus = (int)authEstadoEnum.Rechazado;
                var bd = plan.listAutorizadores.FirstOrDefault(w => w.id == auth.idRegistro);
                bd.comentario = auth.comentario;
                bd.estatus = plan.estatus = (int)authEstadoEnum.Rechazado;
                bd.autorizando = false;
                bd.firma = "RECHAZADA";
                var exito = bonofs.ActualizarEvaluacion(plan);
                result.Add(SUCCESS, exito);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GestionPLantilla
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult BonoUnico()
        {
            return View();
        }
        public ActionResult GestionUnico()
        {
            return View();
        }
        public ActionResult getLstGestionBono(BusqBono busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = bonofs.getLstGestionBono(busq);
                var esSucces = true;
                if (esSucces)
                {
                    result.Add("lst", lst.Select(s => new
                        {
                            id = s.id,
                            cc = s.cc,
                            ccNombre = s.ccNombre,
                            estatus = s.estatus,
                            estatusDesc = ((authEstadoEnum)s.estatus).GetDescription(),
                            fechaCaptura = s.fechaCaptura.ToShortDateString(),
                            fechaInicio = s.fechaInicio.ToShortDateString(),
                            fechaFin = s.fechaFin.ToShortDateString(),
                            usuarioCapturoID = s.usuarioCapturoID,
                            usuarioCapturoNombre = string.Format("{0} {1} {2}", s.usuarioCapturo.nombre, s.usuarioCapturo.apellidoPaterno, s.usuarioCapturo.apellidoMaterno)
                        }).ToList());
                }
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstGestionEvaluacion(BusqBono busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = bonofs.getLstGestionEvaluacion(busq);
                var esSucces = true;
                if (esSucces)
                {
                    result.Add("lst", lst.Select(s => new
                    {
                        id = s.id,
                        cc = s.cc,
                        nomina = ((Tipo_NominaEnum)s.tipoNomina).GetDescription(),
                        periodo = s.periodo + ": ( "+s.fechaInicio.ToShortDateString()+" - "+s.fechaFin.ToShortDateString()+" )",
                        ccNombre = s.plantilla.ccNombre,
                        mes = GlobalUtils.getMonthName(s.fechaInicio),
                        fechaCaptura = s.fecha.ToShortDateString(),
                        usuarioCapturoNombre = string.Format("{0} {1} {2}", s.usuarioEvaluo.nombre, s.usuarioEvaluo.apellidoPaterno, s.usuarioEvaluo.apellidoMaterno),
                        estatusDesc = ((authEstadoEnum)s.estatus).GetDescription(),
                        estatus = s.estatus,
                        fechaInicio = s.fechaInicio.ToShortDateString(),
                        fechaFin = s.fechaFin.ToShortDateString(),
                        usuarioEvaluID = s.usuarioEvaluoID,
                        tipoNomina = s.tipoNomina,
                        periodoInt = s.periodo
                        
                    }).ToList());
                }
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstAuth(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var plan = bonofs.getPlantilla(id);
                var esSucces = plan.listAutorizadores.Count > 0;
                if (esSucces)
                {
                    var clase = string.Empty;
                    var lstUsuario = usuariofs.ListUsersAll();
                    var idUsuario = vSesiones.sesionUsuarioDTO.id;
                    var lst = plan.listAutorizadores.Select(a => new authDTO()
                    {
                        idRegistro = a.id,
                        idAuth = a.aprobadorClave,
                        idPadre = a.plantillaID,
                        orden = a.orden,
                        comentario = a.comentario ?? string.Empty,
                        descripcion = a.aprobadorPuesto,
                        firma = a.firma ?? string.Empty,
                        nombre = string.Format("{0} {1} {2}", a.aprobador.nombre, a.aprobador.apellidoPaterno, a.aprobador.apellidoMaterno),
                        authEstado = a.autorizando && a.aprobadorClave == idUsuario ? authEstadoEnum.EnTurno : (authEstadoEnum)a.estatus,
                        clase = a.autorizando && a.aprobadorClave == idUsuario ? authEstadoEnum.EnTurno.GetDescription() : ((authEstadoEnum)a.estatus).GetDescription(),
                    }).ToList();
                    result.Add(AUTORIZANTES, lst);
                    result.Add(MESSAGE, string.Format("Capturó: {0} {1} {2}", plan.usuarioCapturo.nombre, plan.usuarioCapturo.apellidoPaterno, plan.usuarioCapturo.apellidoMaterno));

                    var resultadoComparacion = bonofs.ObtenerComparativaVersionesPlantilla(plan);
                    result.Add("comparacion", resultadoComparacion);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstAuthEvaluacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var plan = bonofs.getEvaluacionByID(id);
                var esSucces = plan.listAutorizadores.Count > 0;
                if (esSucces)
                {
                    var clase = string.Empty;
                    var lstUsuario = usuariofs.ListUsersAll();
                    var idUsuario = vSesiones.sesionUsuarioDTO.id;
                    var lst = plan.listAutorizadores.Select(a => new authDTO()
                    {
                        idRegistro = a.id,
                        idAuth = a.aprobadorClave,
                        idPadre = a.evaluacionID,
                        orden = a.orden,
                        comentario = a.comentario ?? string.Empty,
                        descripcion = a.aprobadorPuesto,
                        firma = a.firma ?? string.Empty,
                        nombre = string.Format("{0} {1} {2}", a.aprobador.nombre, a.aprobador.apellidoPaterno, a.aprobador.apellidoMaterno),
                        authEstado = a.autorizando && a.aprobadorClave == idUsuario ? authEstadoEnum.EnTurno : (authEstadoEnum)a.estatus,
                        clase = a.autorizando && a.aprobadorClave == idUsuario ? authEstadoEnum.EnTurno.GetDescription() : ((authEstadoEnum)a.estatus).GetDescription(),
                    }).ToList();
                    result.Add(AUTORIZANTES, lst);
                    result.Add(MESSAGE, string.Format("Capturó: {0} {1} {2}", plan.usuarioEvaluo.nombre, plan.usuarioEvaluo.apellidoPaterno, plan.usuarioEvaluo.apellidoMaterno));

       
                }
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CapturaPlantilla
        public ActionResult BonoAdministrativo()
        {
            return View();
        }

        public ActionResult getPuestos(string CC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ListaPuestos = aditivaDeductivafs.getCatPuestos(CC)
                    .GroupBy(a => a.descripcion)
                    .Select(b => b.FirstOrDefault()).ToList();
                var esSucces = ListaPuestos.Count > 0;
                if (esSucces)
                {
                    result.Add("lst", ListaPuestos);
                    result.Add(SUCCESS, esSucces);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTblPuesto(string cc,bool isGestion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var puestos = aditivaDeductivafs.getCatPuestos(cc).OrderBy(x=>x.descripcion).ThenBy(x=>x.tipo_nomina).ToList();
                var esSuccess = puestos.Count > 0;
                if (esSuccess)
                {
                    var lst = bonofs.ActualizarPuestos(puestos, cc,isGestion);
                    var plantilla = bonofs.getPlantilla(cc,isGestion);
                    esSuccess = lst.Count > 0;
                    if (esSuccess)
                    {
                        result.Add("fecha", new 
                        {
                            cc = plantilla.cc,
                            ccNombre = plantilla.ccNombre,
                            fechaCaptura = plantilla.fechaCaptura,
                            fechaFin = plantilla.fechaFin.ToShortDateString(),
                            fechaInicio = plantilla.fechaInicio.ToShortDateString(),
                            estatus = plantilla.estatus,
                            id = plantilla.id,
                            usuarioCapturoID = plantilla.usuarioCapturoID,
                            aplica=""
                        });
                        result.Add("lst", lst);
                        result.Add("lstAuth", plantilla.listAutorizadores.Select(s => new tblRH_BN_Plantilla_Aut()
                        {
                            id = s.id,
                            aprobadorClave = s.aprobadorClave,
                            aprobadorNombre = s.aprobadorNombre,
                            aprobadorPuesto = s.aprobadorPuesto,
                            autorizando = s.autorizando,
                            comentario = s.comentario,
                            estatus = s.estatus,
                            fecha = s.fecha,
                            firma = s.firma,
                            orden = s.orden,
                            plantillaID = s.plantillaID,
                            tipo = s.tipo
                        }).ToList());
                    }
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Evaluación Mensual
        public ActionResult Evaluacion()
        {
            return View();
        }
        public ActionResult GestionEvaluaciones(int? autID)
        {
            var cc = bonofs.getCCNotificacion(autID);
            ViewBag.cc = cc;

            return View();
        }
        public ActionResult getEmpleadosUnicos(string cc,int tipoNomina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = bonofs.getEmpleadosUnicos(cc, tipoNomina);
                result.Add("lst", lst);
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
        #region Incidencia
        public ActionResult Incidencias()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult GestionIncidencias()
        {
            return View();
        }
        public ActionResult IncidenciasPeru()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult getPeriodoActual(int tipoNomina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = bonofs.getPeriodoActual(tipoNomina);
                result.Add("periodo", obj.FirstOrDefault());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPeriodo(int anio, int periodo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = periodo == 0 ? bonofs.getPeriodoActual() : bonofs.getPeriodo(anio, periodo);
                var esSuccess = lst.Count > 0;
                if (esSuccess)
                {
                    result.Add("periodo", lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPeriodos(int? anio,int? tipoNomina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (!anio.HasValue || !tipoNomina.HasValue)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "");
                }
                else
                {
                    var lst = bonofs.getPeriodos(anio.Value, tipoNomina.Value).Select(x => new ComboDTO { Value = x.periodo.ToString(), Text = (x.periodo + " : " + x.fecha_inicial.ToShortDateString() + " - " + x.fecha_final.ToShortDateString()), Prefijo = x.fecha_final.ToShortDateString(), Id = x.fecha_inicial.ToShortDateString() });
                    result.Add(ITEMS, lst);
                    result.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                result = new Dictionary<string, object>();

                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPeriodosRestantes(int anio, int tipoNomina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = bonofs.getPeriodosRestantes(anio, tipoNomina).Select(x => new ComboDTO { Value = x.periodo.ToString(), Text = (x.periodo + " : " + x.fecha_inicial.ToShortDateString() + " - " + x.fecha_final.ToShortDateString()), Prefijo = x.fecha_final.ToShortDateString(), Id = x.fecha_inicial.ToShortDateString() });
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstIncidencias(BusqIncidenciaDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                IncidenciasPaqueteDTO lst = new IncidenciasPaqueteDTO();
                switch (vSesiones.sesionEmpresaActual) 
                {
                    case 6:
                        lst = bonofs.getLstIncidenciasPeru(busq);
                        break;
                    default:
                        lst = bonofs.getLstIncidenciasEnk(busq);
                        break;
                }
                
                var esSuccess = lst.incidencia != null;
                if (esSuccess)
                {
                    var data = JsonConvert.SerializeObject(lst);
                    result.Add("lst", data);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstIncidenciasEnkontrol(BusqIncidenciaDTO busq)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            var lst = bonofs.getLstIncidenciasEnk(busq);
            var esSuccess = lst.incidencia != null;
            if (esSuccess)
            {
                var ud = new UsuarioDAO();
                var isAuth = ud.getViewAction(vSesiones.sesionCurrentView, "Autorizar");
                result.Add("isAuth", isAuth);
                result.Add("lst", lst);
            }
            result.Add(SUCCESS, esSuccess);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarIncidencia(List<HttpPostedFileBase> archivos, string datos, string strClaveEmpleados)
        {
            // SE OBTIENE LISTADO DE CLAVE DE EMPLEADOS A ENTERO
            List<int> lstClaveEmpleados = new List<int>();
            if (!string.IsNullOrEmpty(strClaveEmpleados))
            {
                string[] arrClaveEmpleados = strClaveEmpleados.Split(',');
                foreach (var item in arrClaveEmpleados)
                {
                    lstClaveEmpleados.Add(Convert.ToInt32(item));
                }
            }
            // END

            Dictionary<string, object> result = new Dictionary<string, object>();
            IncidenciasPaqueteDTO paquete = JsonConvert.DeserializeObject<IncidenciasPaqueteDTO>(datos);
            paquete.busq.fechaInicio = Convert.ToDateTime(paquete.busq.stfechaInicio);
            paquete.busq.fechaFin = Convert.ToDateTime(paquete.busq.stfechaFin);
            var save = bonofs.GuardarIncidencia(paquete, archivos, lstClaveEmpleados);

            if (save.Item1)
                result.Add(SUCCESS, true);
            else
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, save.Item2);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarIncidenciaSincronizar(string datos)
        {
            var result = new Dictionary<string, object>();
            try
            {

                IncidenciasPaqueteDTO paquete = JsonConvert.DeserializeObject<IncidenciasPaqueteDTO>(datos);
                var save = bonofs.GuardarIncidenciaSincronizar(paquete);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult authIncidencia(tblRH_BN_Incidencia obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var save = bonofs.authIncidenciaSIGOPLAN_ENKONTROL(obj);
                if ((bool)save[SUCCESS])
                {
                    var cc = save["cc"] as string;
                    var tipoNomina = (int)save["tipoNomina"];
                    var periodo = (int)save["periodo"];
                    var anio = (int)save["anio"];
                    var descNomina = "";

                    switch (tipoNomina)
                    {
                        case (int)tipoNominaPropuestaEnum.Semanal:
                            descNomina = "SEMANAL";
                            break;
                        case (int)tipoNominaPropuestaEnum.Quincenal:
                            descNomina = "QUINCENAL";
                            break;
                        default:
                            descNomina = "-";
                            break;
                    }

                    //var correo = new Infrastructure.DTO.CorreoDTO();
                    //correo.asunto = "AUTORIZACIÓN INCIDENCIAS PARA EL CC [" + cc + "], " + descNomina + " #" + periodo + " " + anio;
                    //correo.correos.Add(vSesiones.sesionUsuarioDTO.correo);
                    //correo.cuerpo = "SE INFORMA DE LA AUTORIZACIÓN DE INCIDENCIAS PARA EL CC [" + cc + "], " + descNomina + " #" + periodo + " " + anio;
                    //correo.Enviar();
                    result.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult desAuthIncidencia(tblRH_BN_Incidencia obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var save = bonofs.desAuthIncidenciaSIGOPLAN_ENKONTROL(obj);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getIncidenciaAuth(int incidenciaID, int anio = 0, int periodo = 0, int tipo_nomina = 0, string cc = "")
        {
            var result = new Dictionary<string, object>();
            try
            {
                IncidenciasPaqueteDTO obj = new IncidenciasPaqueteDTO();
                switch (vSesiones.sesionEmpresaActual)
                {
                    case 6:
                        obj = bonofs.getIncidenciaAuthPeru(incidenciaID, anio, periodo, tipo_nomina, cc);
                        break;
                    default:
                        obj = bonofs.getIncidenciaAuth(incidenciaID, anio, periodo, tipo_nomina, cc);
                        break;
                }
                
                result.Add("datos", obj.incidencia_det/*.OrderBy(x => x.deptoDesc).ThenBy(x => x.ape_paterno + x.ape_materno + x.nombre)*/);
                result.Add("completa", obj.incidencia_det.Any(x=>!x.estatus));
                result.Add("evaluacion_pendiente", obj.evaluacion_pendiente);
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
        #region combobx
        public ActionResult getTblP_CC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = bonofs.getTblP_CC().OrderBy(x => x.Value).ToList();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTblP_CCconPlantilla()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = bonofs.getTblP_CCconPlantilla().OrderBy(x => x.Value).ToList();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getcboCcMonto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = bonofs.getcboCcMonto().OrderBy(x => x.Value).ToList();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getcboCcDepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = bonofs.getcboCcDepto();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getcboTipoNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = GlobalUtils.ParseEnumToCombo<Tipo_Nomina2Enum>();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getcboAuthEstado()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = GlobalUtils.ParseEnumToCombo<authEstadoEnum2>();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboAutorizante(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = bonofs.getCboAutorizante(cc);
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboIncidecnciaConcepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = bonofs.getCboIncidecnciaConcepto();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboMes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> data = new List<ComboDTO>();
                for (int i = 0; i < 12; i++)
                {
                    ComboDTO aux = new ComboDTO();
                    aux.Value = (i + 1).ToString();
                    aux.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((i + 1)).ToUpper();
                    data.Add(aux);
                }
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

        public ActionResult FillCboPorcentaje()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> data = new List<ComboDTO>();
                data.Add(new ComboDTO { Value = "0" , Text = "0%" });
                data.Add(new ComboDTO { Value = "10" , Text = "10%" });
                data.Add(new ComboDTO { Value = "20" , Text = "20%" });
                data.Add(new ComboDTO { Value = "30" , Text = "30%" });
                data.Add(new ComboDTO { Value = "40" , Text = "40%" });
                data.Add(new ComboDTO { Value = "50" , Text = "50%" });
                data.Add(new ComboDTO { Value = "60" , Text = "60%" });
                data.Add(new ComboDTO { Value = "70" , Text = "70%" });
                data.Add(new ComboDTO { Value = "80" , Text = "80%" });
                data.Add(new ComboDTO { Value = "90" , Text = "90%" });
                data.Add(new ComboDTO { Value = "100" , Text = "100%" });
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
        #endregion
        #region Lista_Negra
        public ActionResult ListaNegra()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GuardarEnListaNegra(tblRH_BN_ListaNegra empleado)
        {
            var r = bonofs.GuardarEnListaNegra(empleado);
            return Json(r);
        }

        [HttpGet]
        public JsonResult EmpleadosListaNegra()
        {
            var r = bonofs.EmpleadosListaNegra();
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarDeListaNegra(int claveEmpleado)
        {
            var r = bonofs.EliminarDeListaNegra(claveEmpleado);
            return Json(r);
        }
        #endregion
        #region lista blanca
        public ActionResult ListaBlanca()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GuardarEnListaBlanca(tblRH_BN_ListaBlanca empleado)
        {
            var r = bonofs.GuardarEnListaBlanca(empleado);
            return Json(r);
        }

        [HttpGet]
        public JsonResult EmpleadosListaBlanca()
        {
            var r = bonofs.EmpleadosListaBlanca();
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarDeListaBlanca(int claveEmpleado)
        {
            var r = bonofs.EliminarDeListaBlanca(claveEmpleado);
            return Json(r);
        }
        #endregion
        #region CapturaPlantilla_BonoCuadrado
        public ActionResult BonoCuadrado()
        {
            return View();
        }
        public ActionResult GuardarPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan, List<tblRH_BN_Plantilla_Cuadrado_Det> lst, bool isGestion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = plan.cc.Length == 3 &&
                    lst.Count > 0
                    && lst.All(a =>
                       a.monto > 0
                    && a.periodicidad > 0);
                if (esGuardado)
                {
                    plan.estatus = (int)authEstadoEnum.EnEspera;
                    plan.usuarioCapturoID = vSesiones.sesionUsuarioDTO.id;
                    plan.listDetalle = lst;
                    esGuardado = bonofs.CrearPlantilla_Cuadrado(plan, isGestion);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult getTblPuesto_Cuadrado(string cc, bool isGestion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var plantilla = bonofs.getPlantilla_Cuadrado(cc, isGestion);
                var esSuccess = plantilla != null;
                if (esSuccess)
                {
                    var lst = reportesRHFactoryServices.getReportesRHService().GetEmpleadosActivos_CC(cc);
                    esSuccess = lst.Count > 0;
                    if (esSuccess)
                    {
                        List<PlantillaDetDTO> empleados = new List<PlantillaDetDTO>();
                        foreach (var emp in lst)
                        {
                            var obj = new PlantillaDetDTO();
                            obj.empleadoID = emp.empleadoID;
                            obj.empleado = emp.empleado;
                            obj.deptoID = emp.departamentoID;
                            obj.depto = emp.departamento;
                            obj.puestoID = emp.idPuesto;
                            obj.puesto = emp.puesto;
                            obj.tipoNominaID = emp.tipo_nominaID;
                            obj.tipoNomina = emp.tipo_nomina;
                            obj.monto = emp.bono_cuadrado;
                            obj.salario = emp.salario_base;
                            empleados.Add(obj);
                        }
                        result.Add("fecha", new
                        {
                            cc = plantilla.cc,
                            ccNombre = plantilla.ccNombre,
                            fechaCaptura = plantilla.fechaCaptura,
                            fechaFin = plantilla.fechaFin.ToShortDateString(),
                            fechaInicio = plantilla.fechaInicio.ToShortDateString(),
                            estatus = plantilla.estatus,
                            id = plantilla.id,
                            usuarioCapturoID = plantilla.usuarioCapturoID,
                            aplica = ""
                        });
                        result.Add("lst", empleados);
                    }
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult getIdPanelReporte()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("idPanelReporte", ReportesEnum.rh_bono_plantilla_cc);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        bool esBonoAdminValido(tblRH_BN_Plantilla bono)
        {
            var esCC = bono.cc.Trim().Length > 0;
            var esInicio = bono.fechaInicio.Month > 0 && bono.fechaInicio.Year > 0;
            var esFin = bono.fechaFin.Month > 0 && bono.fechaFin.Year > 0;
            var esComentario = false;
            var esAuth = true;
            var esPuesto = true;
            return esCC && esInicio && esFin && esComentario && esAuth;
        }
        string getNombreUsuario(tblP_Usuario user)
        {
            return string.Format("{0} {1} {2}", user.nombre, user.apellidoPaterno, user.apellidoMaterno);
        }
        #region Evaluacion
        public ActionResult getEmpleadosEvaluar(string cc,int periodo, int tipoNomina, DateTime fechaPeriodo)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var lst = bonofs.getEmpleadosEvaluar(cc,periodo,tipoNomina,fechaPeriodo);
                var esSucces = true;
                if (esSucces)
                {
                    result.Add("lst", lst.validos.Select(s => new tblRH_BN_Evaluacion_Det
                    {
                        id = s.id,
                        plantillaID = s.plantillaID,
                        plantillaDetID = s.plantillaDetID,
                        evaluacionID = s.evaluacionID,
                        cve_Emp = s.cve_Emp,
                        nombre_Emp = s.nombre_Emp,
                        puestoCve_Emp = s.puestoCve_Emp,
                        puesto_Emp = s.puesto_Emp,
                        base_Emp = s.base_Emp,
                        complemento_Emp = s.complemento_Emp,
                        bono_FC = s.bono_FC,
                        bono_Emp = s.bono_Emp,
                        porcentaje_Asig = s.porcentaje_Asig,
                        monto_Asig = s.monto_Asig,
                        total_Nom = s.total_Nom,
                        tipo_Nom = s.tipo_Nom,
                        tipoCve_Nom = s.tipoCve_Nom,
                        total_Mensual = s.total_Mensual,
                        periodicidad = s.periodicidad,
                        periodicidadCve = s.periodicidadCve,
                        con_Bono = (s.evaluacionID > 0 ? s.con_Bono :s.total_Mensual)
                    }).ToList());
                    result.Add("lstnovalidos", lst.novalidos.Select(s => new 
                    {
                        cve_Emp = s.cve_Emp,
                        nombre_Emp = s.nombre_Emp,
                        puestoCve_Emp = s.puestoCve_Emp,
                        puesto_Emp = s.puesto_Emp,
                        base_Emp = s.base_Emp,
                        complemento_Emp = s.complemento_Emp,
                        bono_FC = s.bono_FC,
                        bono_Emp = s.bono_Emp,
                        fechaAlta = s.fechaAltaStr,
                        fechaRe = s.fechaRe,
                        diasAntiguedad = s.antiguedad.dias,
                        diasOBraParaBono = s.antiguedad.diasOBraParaBono,
                        antiguedad = s.antiguedad,
                        isBonoSistema = s.bono_FC > 0 ? "SI" : "NO",
                        isLstNegra = s.tipo_Nom.Equals("lstnegra") ? "SI" : "NO",
                        isAntiguedad = s.antiguedad.dias >= s.antiguedad.diasOBraParaBono ? "SI" : "NO",
                        isPlantilla = !s.periodicidad.Equals("NOPLANTILLA") ? "SI" : "NO"
                    }).ToList());
                }
                result.Add("evaluacionID", lst.evaluacionID);
                result.Add("estatus", lst.estatus);
                result.Add(SUCCESS, esSucces);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Aut> aut)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var id = bonofs.guardarEvaluacion(obj, aut);

                result.Add("evaluacionID", id);
                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarEvaluacionDet(List<tblRH_BN_Evaluacion_Det> lst)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var data = bonofs.guardarEvaluacionDet(lst);
                var esSucces = data;

                result.Add(SUCCESS, esSucces);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult actualizarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Det> det)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var data = bonofs.actualizarEvaluacion(obj, det);
                var esSucces = data;

                result.Add(SUCCESS, esSucces);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getIncidenciasPendiente()
        {
            var result = new Dictionary<string, object>();
            List<incidenciasPendientesDTO> data = new List<incidenciasPendientesDTO>();

            switch (vSesiones.sesionEmpresaActual) 
            {
                case 6:
                    data = bonofs.getIncidenciasPendientePeru();
                    break;
                default:
                    data = bonofs.getIncidenciasPendiente();
                    break;
            }

            result.Add("data", data);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult authListIncidencia(List<tblRH_BN_Incidencia> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var renglonesString = "";

                foreach (var obj in lst)
                {
                    var save = bonofs.authIncidenciaSIGOPLAN_ENKONTROL(obj);

                    if (((bool)save[SUCCESS]))
                    {
                        var descNomina = "";

                        switch (obj.tipo_nomina)
                        {
                            case (int)tipoNominaPropuestaEnum.Semanal:
                                descNomina = "SEMANAL";
                                break;
                            case (int)tipoNominaPropuestaEnum.Quincenal:
                                descNomina = "QUINCENAL";
                                break;
                            default:
                                descNomina = "-";
                                break;
                        }

                        renglonesString += string.Format(@"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>
                            </tr>",
                            obj.cc, //Centro Costo
                            descNomina, //Tipo Nómina
                            obj.periodo, //Periodo
                            obj.anio //Año
                        );
                    }
                }

                #region Enviar Correo
                var correo = new Infrastructure.DTO.CorreoDTO()
                {
                    asunto = "AUTORIZACIÓN INCIDENCIAS",
                    cuerpo = "SE INFORMA DE LA AUTORIZACIÓN DE LAS SIGUIENTES INCIDENCIAS:<br><br>" + @"
                    <table>
                        <thead>
                            <tr>
                                <th>Centro de Costo</th>
                                <th>Tipo Nómina</th>
                                <th>Periodo</th>
                                <th>Año</th>
                            </tr>
                        </thead>
                        <tbody>" + renglonesString + @"</tbody>
                    </table>",
                    correos = new List<string> { "martin.valle@construplan.com.mx", vSesiones.sesionUsuarioDTO.correo }
                };

#if DEBUG
               
#endif

                correo.Enviar();
                #endregion

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RevisarFechaCierre(List<tblRH_BN_Incidencia> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var estadoIncidencias = bonofs.RevisarFechaCierre(lst);
                result.Add("estadoIncidencias", estadoIncidencias);
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
    }
}
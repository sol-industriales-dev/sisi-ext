using Core.DAO.RecursosHumanos.Desempeno;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Desempeno;
using Core.Entity.RecursosHumanos.Desempeno;
using Data.Factory.RecursosHumanos.Desempeno;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Desempeno
{
    public class DesempenoController : BaseController
    {
        #region init
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        public IDesempenoDAO desempenoFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            desempenoFS = new DesempenoFactoryServices().getDesempenoService();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region Dashboard
        public ActionResult Dashboard()
        {
            if (Session["usuarioDesempeño"] != null)
            {

            }
            else
            {
                var respuesta = desempenoFS.UsuarioSesion();
                if (respuesta.Success)
                {
                    Session["usuarioDesempeño"] = respuesta.Value;
                }
            }

            return View();
        }

        public ActionResult SalirVerComo()
        {
            Session["usuarioDesempeño"] = null;
            return RedirectToAction("Dashboard");
        }
        #endregion
        #region Semaforo
        public ActionResult getLstSemaforo(int idProceso)
        {
            try
            {
                var lst = desempenoFS.getLstSemaforo()
                    .Select(s => new
                    {
                        s.color,
                        s.maximo,
                        s.minimo
                    }).OrderBy(o => o.maximo).ToList();
                resultado.Add("lst", lst);
                resultado.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Meta
        public ActionResult _FormMeta()
        {
            return PartialView();
        }
        public ActionResult _FormPesos()
        {
            return PartialView();
        }
        public ActionResult _FormMetaEvaluar()
        {
            return PartialView();
        }
        public ActionResult guardarMeta(tblRH_ED_DetMetas meta)
        {
            try
            {
                var usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];

                if (usuario.JefeId == null)
                    usuario.JefeId = usuario.EmpleadoId;           

                var esSuccess = false;
                var mensaje = string.Empty;
                if(string.IsNullOrEmpty(meta.nombre) || meta.nombre.Trim().Length < 3)
                {
                    mensaje += "El nombre es muy corto. <br>";
                }
                if(string.IsNullOrEmpty(meta.descripcion) || meta.descripcion.Trim().Length < 3)
                {
                    mensaje += "La descripción es muy corta. <br>";
                }
                if(meta.tipo <= 0)
                {
                    mensaje += "No has seleccionado alguna estrategia. <br>";
                }
                if(meta.peso <= 0)
                {
                    mensaje += "No has asignado algún peso. <br>";
                }
                if(mensaje.Length > 0)
                {
                    throw new System.InvalidOperationException(mensaje);
                }
                else
                {
                    if(meta.idUsuario == 0)
                    {
                        meta.idUsuario = usuario.EmpleadoId;
                    }
                    if (meta.idUsuario != usuario.EmpleadoId)
                    {
                        //meta.esVobo = true;
                        //meta.notificado = true;
                        meta.idJefe = usuario.EmpleadoId;
                    }
                    esSuccess = desempenoFS.guardarMeta(meta);
                }
                resultado.Add(SUCCESS, esSuccess);
                resultado.Add(MESSAGE, esSuccess ? "" : "Ocurrió un error al guarda la meta");
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarMetasPesos(List<tblRH_ED_DetMetas> lst)
        {
            try
            {
                var esSuccess = lst.Sum(s => s.peso) <= 100;
                if(esSuccess)
                {
                    esSuccess = desempenoFS.guardarMeta(lst);
                }
                else
                {
                    throw new System.InvalidOperationException("El peso es demasiado alto. Ajuste para tener un maximo de 100.");
                }
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarMeta(int id)
        {
            try
            {
                var esSuccess = desempenoFS.eliminarMeta(id);
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstMetaPorProceso(BusqMetaPorProcesoDTO busq)
        {
            try
            {
                var fechaHoy = DateTime.Now.Date;
                var infoEvaluacion = desempenoFS.GetInfoEvaluacion(busq.idEvaluacion);
                var infoProceso = desempenoFS.GetProceso(busq.idProceso);
                var lstFiltro = new List<tblRH_ED_DetMetas>();
                var lstSemaforo = desempenoFS.getLstSemaforo();
                resultado.Add("lstSemaforo", lstSemaforo);
                var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
                if(busq.idUsuario == 0)
                {
                    busq.idUsuario = sesion.EmpleadoId;
                }
                var empleado = desempenoFS.getEmpleado(busq.idUsuario);

                var lstObservacion = desempenoFS.getLstObservacionesPorUsuario(busq.idUsuario).Where(w => w.idEvaluacion == busq.idEvaluacion && w.meta.idProceso == busq.idProceso).ToList();
                var lst = desempenoFS.getLstMetaPorProceso(busq.idProceso, busq.idUsuario);
                if (busq.esVobo)
                {
                    //lstFiltro.AddRange(lst.Where(w => w.idProceso == busq.idProceso && w.proceso.lstEvaluacion.Any(a => a.id == busq.idEvaluacion) && !w.esVobo && (lstObservacion.Any(o => o.esActivo && o.idMeta == w.id && !o.esAutoEvaluado && !o.esJefeEvaluado ) || lstObservacion.Count == 0)));
                    lstFiltro.AddRange(lst.Where(w => w.idProceso == busq.idProceso && w.proceso.lstEvaluacion.Any(a => a.id == busq.idEvaluacion) && !w.esVobo));
                }
                if (busq.esCalificar)
                {
                    //lstFiltro.AddRange(lst.Where(w => w.idProceso == busq.idProceso && w.proceso.lstEvaluacion.Any(a => a.id == busq.idEvaluacion) && w.esVobo && (lstObservacion.Any(o => o.esActivo && o.idMeta == w.id && !o.esAutoEvaluado && !o.esJefeEvaluado) || lstObservacion.Count == 0)));
                    lstFiltro.AddRange(lst.Where(w => w.idProceso == busq.idProceso && w.proceso.lstEvaluacion.Any(a => a.id == busq.idEvaluacion) && w.esVobo && (!lstObservacion.Any(o => o.esActivo && o.idMeta == w.id))));
                }
                if (busq.esJefe)
                {
                    lstFiltro.AddRange(lst.Where(w => lstObservacion.Any(a => a.idMeta == w.id && w.esVobo && a.esAutoEvaluado && !a.esJefeEvaluado)));
                }
                if (busq.esRevisados)
                {
                    lstFiltro.AddRange(lst.Where(w => lstObservacion.Any(a => a.idMeta == w.id && w.esVobo && a.esAutoEvaluado && a.esJefeEvaluado)));
                }

                ComboDTO _puesto = null;
                if (lstFiltro != null && lstFiltro.Count > 0 && lst.First().usuario != null && !string.IsNullOrEmpty(lst.First().usuario.cveEmpleado))
                {
                    _puesto = desempenoFS.CargarPuestosEmpleados(new List<string> { lst.First().usuario.cveEmpleado }).FirstOrDefault();
                }
                var lstMeta = lstFiltro.Select(s => new
                {
                    s.id,
                    s.idProceso,
                    s.idUsuario,
                    s.descripcion,
                    s.nombre,
                    s.peso,
                    s.tipo,
                    s.esVobo,
                    evaluacion = getEvaluacionMeta(s, lstObservacion, busq.idEvaluacion),
                    //tieneAutoEvaluacion = lst.Any(x => lstObservacion.Any(a => a.esAutoEvaluado)),
                    tieneAutoEvaluacion = getAutoEvaluacion(s, lstObservacion, busq.idEvaluacion),
                    esUsuario = sesion.EmpleadoId == s.idUsuario,
                    descripcionPuesto = _puesto != null ? _puesto.Text : "",
                    estrategia = s.estrategia.descripcion,
                    s.idJefe,
                    jefePuedeEvaluar = lst.Any(w => lstObservacion.Any(a => a.idMeta == s.id && w.esVobo && a.esAutoEvaluado && !a.notificadoJefeAUsuario && a.notificado)),
                    evaluadaPorJefe = lst.Any(w => lstObservacion.Any(a => a.idMeta == s.id && w.esVobo && a.esJefeEvaluado)),
                    puedeEvaluar = !lstObservacion.Any(a => a.idMeta == s.id && a.notificado)
                }).OrderBy(o => o.id);
                resultado.Add("lst", lstMeta);
                var lstEmpleado = desempenoFS.getLstEmpleadoJefe(sesion.EmpleadoId);
                var lstMetas = desempenoFS.getLstMetaPorProceso(busq.idProceso, lstEmpleado.Select(s => s.empleadoID).ToList());
                resultado.Add("totalPesoMetas", lstMetas.Where(w => w.idUsuario == busq.idUsuario).Sum(s => s.peso));
                var lstObservacionUsuario = desempenoFS.getLstObservacionesPorProcesosYJefe(busq.idProceso, busq.idUsuario).Where(w => w.idEvaluacion == busq.idEvaluacion).ToList();
                var lstEmpleadoIndicadores = lstEmpleado.Select(s => new
                {
                    nombre = string.Format("{0} {1} {2}", s.usuario.nombre, s.usuario.apellidoPaterno, s.usuario.apellidoMaterno),
                    puesto = s.usuario.puesto.descripcion,
                    idUsuario = s.usuario.id,
                    idEmpleado = s.id,
                    noVobo = lstMetas.Where(w => w.idUsuario == s.empleadoID && !w.esVobo && w.notificado).Count(),
                    noCalificar = lstObservacionUsuario.Count != 0 ? lstMetas.Where(x => x.esVobo && x.notificado && x.idUsuario == s.empleadoID).Count() - lstObservacionUsuario.Where(w => s.empleadoID == w.idUsuario && w.meta.esVobo && w.esAutoEvaluado).Count() : lstMetas.Where(x => x.esVobo && x.notificado && x.idUsuario == s.empleadoID).Count(),
                    noJefe = lstObservacionUsuario.Where(w => s.empleadoID == w.idUsuario && w.meta.esVobo && w.esAutoEvaluado && !w.esJefeEvaluado && w.notificado).Count(),
                    noRevisados = lstObservacionUsuario.Where(w => s.empleadoID == w.idUsuario && w.meta.esVobo && w.esAutoEvaluado && w.esJefeEvaluado).Count(),
                    esJefe = lstEmpleado.Count > 1 && s.empleadoID == sesion.EmpleadoId,
                    estatus = s.estatus,
                    jefeInmediato = s.jefe == null ? "" : s.jefe.usuario.nombre + " " + s.jefe.usuario.apellidoPaterno + " " + s.jefe.usuario.apellidoMaterno,
                    jefeInmediatoID = s.jefeID
                }).ToList();
                resultado.Add("lstEmpleadoIndicadores", lstEmpleadoIndicadores);
                resultado.Add("notificado", Notificado(busq.idUsuario, busq.idEvaluacion));
                resultado.Add
                    ("permiteAgregarMasMetas",
                        (
                            lst.Sum(s => s.peso) >= 100 ||
                            busq.idEvaluacion == 0 ||
                            empleado.jefeID == null ||
                            (
                                lst.FirstOrDefault() != null &&
                                lst.FirstOrDefault().idJefe == 0
                            ) ||
                            (
                                lst.FirstOrDefault(x => !x.notificado) != null &&
                                empleado.empleadoID != sesion.EmpleadoId
                            ) ||
                            (
                                lst.Count == 0 && empleado.empleadoID != sesion.EmpleadoId
                            )
                        ) ? false : true
                    );
                resultado.Add
                    ("esPeriodoActual",
                        infoEvaluacion != null &&
                        infoEvaluacion.fechaInicio <= fechaHoy &&
                        infoEvaluacion.fechaFin >= fechaHoy ?
                        true : false
                    );
                resultado.Add
                    ("esProcesoActual",
                        infoProceso != null &&
                        infoProceso.fechaInicio <= fechaHoy &&
                        infoProceso.fechaFin >= fechaHoy ?
                        true : false
                    );
                resultado.Add("notificarMetas", lst.Sum(s => s.peso) == 100 && lst.FirstOrDefault(x => !x.esVobo) != null && lst.FirstOrDefault(x => !x.notificado) != null && busq.idUsuario == sesion.EmpleadoId);
                bool editarMetas = lst.Sum(s => s.peso) > 0 && (bool)resultado["esProcesoActual"] &&
                                (
                                    (
                                        lst.FirstOrDefault(x => !x.notificado) != null && sesion.EmpleadoId == empleado.usuario.id
                                    )
                                    ||
                                    (
                    //lst.FirstOrDefault(x => x.notificado && !x.esVobo) != null && sesion.EmpleadoId != empleado.usuario.id && lst.FirstOrDefault().jefe.id == sesion.EmpleadoId                                        
                                        lst.FirstOrDefault(x => x.notificado && !x.esVobo) != null && sesion.EmpleadoId != empleado.usuario.id && empleado.jefeID == sesion.EmpleadoId
                                    )
                                );
                //si es jefe siempre puede editar metas
                resultado.Add("editarMetas", busq.esJefe && (bool)resultado["esProcesoActual"] ? true : editarMetas);
                //if (empleado.jefe != null)
                //{
                    resultado.Add("puedeDarVoBo", lst.Sum(s => s.peso) == 100 && lst.FirstOrDefault(x => !x.esVobo) != null && lst.FirstOrDefault(x => !x.notificado) == null && (empleado.jefe == null ? true : empleado.jefe.empleadoID == sesion.EmpleadoId));
                //}
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado);
        }
        
        decimal getEvaluacionMeta(tblRH_ED_DetMetas meta, List<tblRH_ED_DetObservacion> lstObservacion, int idEvaluacion)
        {
            var observacion = lstObservacion.FirstOrDefault(o => o.idMeta == meta.id && o.idEvaluacion == idEvaluacion) ?? new tblRH_ED_DetObservacion();
            if(observacion.esJefeEvaluado)
            {
                return observacion.jefeEvaluacion;
            }
            if(observacion.esAutoEvaluado)
            {
                return observacion.autoEvaluacion;
            }
            return 0;
        }
        bool getAutoEvaluacion(tblRH_ED_DetMetas meta, List<tblRH_ED_DetObservacion> lstObservacion, int idEvaluacion)
        {
            var observacion = lstObservacion.FirstOrDefault(o => o.idMeta == meta.id && o.idEvaluacion == idEvaluacion) ?? new tblRH_ED_DetObservacion();
            if (observacion.esJefeEvaluado)
            {
                return observacion.esJefeEvaluado;
            }
            if (observacion.esAutoEvaluado)
            {
                return observacion.esAutoEvaluado;
            }
            return false;
        }
        public ActionResult eliminarEvidencia(int id)
        {
            try
            {
                var esSuccess = desempenoFS.eliminarEvidencia(id);
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult darVoboMeta(int idMeta)
        {
            try
            {
                var esSuccess = false;
                var mensaje = string.Empty;
                var meta = desempenoFS.getMeta(idMeta);
                var lstMetas = desempenoFS.getLstMetaPorProceso(meta.idProceso, meta.idUsuario);
                var totalPeso = lstMetas.Sum(s => s.peso);
                if(totalPeso != 100)
                {
                    mensaje += string.Format(" El proceso {0} cuenta con un total de {1} en peso. Se necesita que sea un peso de 100.", meta.proceso.proceso, totalPeso);
                }
                if(mensaje.Length > 0)
                {
                    throw new System.InvalidOperationException(mensaje);
                }
                else
                {
                    meta.esVobo = true;
                    meta.idJefe = vSesiones.sesionUsuarioDTO.id;
                    esSuccess = desempenoFS.darVoboMeta(meta);
                }
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult VoBoMetas(int idProceso, int idEmpleado, int idEvaluacion)
        {
            var respuesta = desempenoFS.VoBoMetas(idProceso, idEmpleado, idEvaluacion);

            return Json(respuesta);
        }
        public ActionResult getMetaIndividual(int idMeta)
        {
            try
            {
                var usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
                var lstEvaluaciones = new List<MetaGraficoDTO>();
                var meta = desempenoFS.getMeta(idMeta);
                var lstSemaforo = desempenoFS.getLstSemaforo();
                var cboSeguimiento = desempenoFS.getCboEvaluacionPorProceso(meta.idProceso);
                var lstSeguimineto = cboSeguimiento.Select(s => s.Text).ToList();
                var lstObservacion = desempenoFS.getLstObservacionesPorUsuario(meta.idUsuario);
                var lstSubordinado = desempenoFS.getSubordinado(usuario.EmpleadoId);
                var maxSeguimiento = cboSeguimiento.Count;
                var arrAutoEvaluacion = new decimal[maxSeguimiento];
                var arrJefeEvaluacion = new decimal[maxSeguimiento];
                for(int i = 0; i < maxSeguimiento; i++)
                {
                    var eva =  cboSeguimiento[i];
                    var lstMetaObservacion = lstObservacion.Where(w => w.idEvaluacion == eva.Value.ParseInt() && w.idMeta == meta.id);
                    arrAutoEvaluacion[i] = lstMetaObservacion.Sum(s => s.autoEvaluacion);
                    arrJefeEvaluacion[i] = lstMetaObservacion.Sum(s => s.jefeEvaluacion);
                }
                lstEvaluaciones.Add(new MetaGraficoDTO()
                {
                    name = "AutoEvaluacion",
                    stack = "AutoEvaluacion",
                    data = arrAutoEvaluacion,
                    type = "line",
                    tooltip = new { valueDecimals = 2 },
                    visible = true
                });
                lstEvaluaciones.Add(new MetaGraficoDTO()
                {
                    name = "Evaluacion",
                    stack = "JefeEvaluacion",
                    data = arrJefeEvaluacion,
                    type = "line",
                    tooltip = new { valueDecimals = 2 },
                    visible = true
                });
                resultado.Add("proceso", new
                {
                    id = meta.idProceso,
                    meta.proceso.proceso,
                    meta.proceso.fechaInicio,
                    meta.proceso.fechaFin
                });
                resultado.Add("meta", new 
                {
                    meta.id,
                    meta.idUsuario,
                    meta.nombre,
                    meta.descripcion,
                    meta.esVobo,
                    puedeDarVobo = !meta.esVobo && lstSubordinado.Any(s => s.empleadoID == meta.idUsuario),
                    estrategia = meta.estrategia.descripcion,
                    meta.peso,
                    meta.notificado
                });
                resultado.Add("lstSemaforo", lstSemaforo.Select(s => new 
                {
                    s.color,
                    s.minimo,
                    s.maximo,
                }).ToList());
                resultado.Add("lstSeguimientos", cboSeguimiento.Select(s => s.Text).ToArray());
                resultado.Add("lstEvaluaciones", lstEvaluaciones);
                resultado.Add(SUCCESS, meta.id > 0);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstMetaPorPesos(int idProceso, int idUsuario)
        {
            try
            {
                var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
                if(idUsuario == 0)
                {
                    idUsuario = sesion.EmpleadoId;
                }
                var lst = desempenoFS.getLstMetaPorProceso(idProceso, idUsuario).Select(s => new 
                {
                    s.id,
                    s.nombre,
                    s.peso,
                    s.idUsuario,
                    s.esVobo
                }).ToList();
                resultado.Add("lst", lst);
                resultado.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DiasSeguimiento(int idProceso)
        {
            return Json(desempenoFS.DiasSeguimiento(idProceso), JsonRequestBehavior.AllowGet);
        }

        private bool Notificado(int idEmpleado, int idEvaluacion)
        {
            var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
            int? idJefe = null;

            if (sesion.EmpleadoId != idEmpleado)
            {
                idJefe = sesion.EmpleadoId;
            }
            return desempenoFS.Notificado(idEmpleado, idEvaluacion, idJefe);
        }

        public JsonResult Notificar(int idEmpleado, int idEvaluacion, int idProceso)
        {
            Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO usuario = null;
            int? idJefe = null;
            int idEmpleadoTemp = idEmpleado;
            usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
            idEmpleado = usuario.EmpleadoId;
            idJefe = usuario.VerComoActivado ? idEmpleadoTemp != 0 ? usuario.Id : usuario.JefeId : null;
            //if (idEmpleado == 0)
            //{
            //    usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
            //    idEmpleado = usuario.EmpleadoId;
            //    idJefe = usuario.VerComoActivado ? usuario.JefeId : null;
            //}
            if (idEmpleadoTemp == 0)
            {
                if (usuario.VerComoActivado)
                {
                    idJefe = 0;
                }
                else
                {
                    idJefe = null;
                }
            }
            if (idEmpleadoTemp != 0)
            {
                idEmpleado = idEmpleadoTemp;
            }

            return Json(desempenoFS.Notificar(idEmpleado, idEvaluacion, idProceso, idJefe));
        }
        public JsonResult NotificarMetas(int idEmpleado, int idProceso)
        {
            var usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
            return Json(desempenoFS.NotificarMetas(usuario.EmpleadoId, idProceso));
        }
        #endregion
        #region Escritorio
        public ActionResult _Escritorio()
        {
            return PartialView();
        }
        #endregion
        #region Inicio
        public ActionResult _Inicio()
        {
            return PartialView();
        }
        #endregion
        #region Empleados
        public ActionResult _Empleados()
        {
            return PartialView();
        }

        public ActionResult CargarTblEmpleados(bool estatus = true)
        {
            try
            {
                int contador = 0;
                var auxLst = desempenoFS.CargarTblEmpleados(estatus);
                var empleadosIDs = auxLst.Select(x => x.usuario.cveEmpleado).ToList();

                var puestos = desempenoFS.CargarPuestosEmpleados(empleadosIDs);

                var lst = auxLst
                    .Select(s =>
                    {
                        var puesto = puestos.FirstOrDefault(x => x.Value == s.usuario.cveEmpleado);
                        return new
                        {
                            id = s.id,
                            contador = ++contador,
                            nombre = s.usuario.nombre + " " + s.usuario.apellidoPaterno + " " + s.usuario.apellidoMaterno,
                            idEmpleado = s.usuario.id,
                            puesto = puesto == null ? "--" : puesto.Text,
                            jefe = s.jefe != null ? s.jefe.usuario.nombre + " " + s.jefe.usuario.apellidoPaterno + " " + s.jefe.usuario.apellidoMaterno : "",
                            idJefe = s.jefeID != null ? s.jefeID : (int?)null,
                            procesos = s.procesos.Where(x => x.Estatus).Select(m => m.Proceso.id).ToList(),
                            tipo = (int)s.tipo,
                            s.estatus
                        };
                    }

                    ).ToList();
                resultado.Add("lst", lst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleados(string term)
        {
            var componentes = desempenoFS.getEmpleados(term).ToList().Take(10);
            var componentesFiltrados = componentes.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });
            return Json(componentesFiltrados, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleadosDesempeno(string term)
        {
            var componentes = desempenoFS.getEmpleadosDesempeno(term).ToList().Take(10);
            var componentesFiltrados = componentes.Select(x => new { id = x.id, label = x.usuario.nombre + " " + x.usuario.apellidoPaterno + " " + x.usuario.apellidoMaterno });
            return Json(componentesFiltrados, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarEmpleado(int empleadoID, int? jefeID, int tipo)
        {
            try
            {
                var exito = desempenoFS.GuardarEmpleado(empleadoID, jefeID, tipo);
                resultado.Add("exito", exito.Success);
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, exito.Message);

                var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
                resultado.Add("recargar", vSesiones.sesionUsuarioDTO.id == empleadoID);
                if (sesion.EmpleadoId == empleadoID)
                {
                    Session["usuarioDesempeño"] = null;
                }
            }
            catch (Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Mvc.HttpGet]
        public JsonResult VerComo(int idUsuario)
        {
            var respuesta = desempenoFS.VerComo(idUsuario);
            if (respuesta.Success)
            {
                Session["usuarioDesempeño"] = respuesta.Value;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Mvc.HttpPost]
        public JsonResult ModificarProcesoEmpleado(List<int> idProceso, int idEmpleado)
        {
            return Json(desempenoFS.ModificarProcesoEmpleado(idProceso, idEmpleado));
        }
        public JsonResult EliminarEmpleado(int idEmpleado)
        {
            var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
            var resultado = desempenoFS.EliminarEmpleado(idEmpleado);
            if (resultado.Success)
            {
                resultado.Message = vSesiones.sesionUsuarioDTO.id == sesion.EmpleadoId ? "recargar" : "Ok";
            }
            return Json(resultado);
        }
        #endregion
        #region Observacion
        public ActionResult getObservacion(int idMeta, int idEvaluacion, int idUsuarioCalificar)
        {
            try
            {
                var lstSemaforo = desempenoFS.getLstSemaforo();
                var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
                if(idUsuarioCalificar == 0)
                {
                    idUsuarioCalificar = sesion.EmpleadoId;
                }
                var i = 0;
                var observacion = desempenoFS.getObservacion(idMeta, idEvaluacion, idUsuarioCalificar);
                var lstEvidencia = observacion.lstEvidencia.Where(x => x.esActivo).Select(s => new {
                    s.id,
                    s.nombre,
                    contador = ++i,
                    //source = GlobalUtils.getBase64FromNombreRuta(s.ruta, s.nombre),
                    extencion = Path.GetExtension(s.ruta),
                    source = s.ruta,
                    url = true
                }).ToList();
                
                resultado.Add("observacion", new
                {
                    observacion.id,
                    observacion.resultado,
                    observacion.autoEvaluacion,
                    observacion.autoObservacion,
                    observacion.jefeEvaluacion,
                    observacion.jefeObservacion,
                    idUsuario = idUsuarioCalificar,
                    observacion.idJefe,
                    observacion.esJefeEvaluado,
                    puedeAutoEvaluar = (observacion.idUsuario == sesion.EmpleadoId && !observacion.esJefeEvaluado),
                    puedeJefeEvaluar = (observacion.idJefe == sesion.EmpleadoId && observacion.esAutoEvaluado),
                    meta = new
                    {
                        observacion.meta.id,
                        observacion.meta.idProceso,
                        observacion.meta.nombre,
                        observacion.meta.descripcion,
                        tipo = observacion.meta.estrategia.descripcion,
                    },
                    lstSemaforo = lstSemaforo.Where(w => w.esActivo).Select(s => new
                    {
                        s.color,
                        s.maximo,
                        s.minimo
                    }).OrderBy(o => o.maximo).ToList(),
                    lstEvidencia
                });
                resultado.Add(SUCCESS, observacion.meta.id > 0);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult metaEvidenciaGuardar()
        {
            try
            {
                var obs = JsonUtils.convertJsonToNetObject<tblRH_ED_DetObservacion>(Request.Form["obs"]);
                var esSuccess = verificaObjObservacion(obs);
                var msjResultado = string.Empty;
                if (esSuccess)
                {
                    var lstArchivo = Request.Files.GetMultiple("files[]").ToList();
                    //if (lstArchivo.Count() <= 0)
                    //    throw new Exception(string.Format("lstArchivo se encuentra vacia"));

                    msjResultado = desempenoFS.metaEvidenciaGuardar(obs, lstArchivo);
                    if (msjResultado == "false")
                        throw new Exception(string.Format(msjResultado));

                    string archivoVacio = msjResultado.Trim().Split(' ').LastOrDefault().Trim();
                    if (archivoVacio == "vacio")
                        throw new Exception(string.Format(msjResultado));
                }
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        public ActionResult DescargarEvidenciasMeta(int idObservacion)
        {
            var DescargarEvidencia = desempenoFS.DescargarEvidenciasMeta(idObservacion);

            if (DescargarEvidencia != null)
            {
                string nombreArchivo = DescargarEvidencia.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(DescargarEvidencia.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View("ErrorDescarga");
            }
        }
        bool verificaObjObservacion(tblRH_ED_DetObservacion obs)
        {
            obs.meta = desempenoFS.getMeta(obs.idMeta);
            var lstSemaforo = desempenoFS.getLstSemaforo();
            var mensaje = string.Empty;
            var minCalif = lstSemaforo.Min(m => m.minimo);
            var maxCalif = lstSemaforo.Max(m => m.maximo);
            if(obs.esAutoEvaluado)
            {
                if(obs.resultado <= -1)
                {
                    mensaje += "Debe de tener algún resultado. <br>";
                }
                if(string.IsNullOrEmpty(obs.autoObservacion) || obs.autoObservacion.Trim().Length < 3)
                {
                    mensaje += "La auto observación es muy corta. <br>";
                }
                if(!(obs.autoEvaluacion >= minCalif && obs.autoEvaluacion <= maxCalif))
                {
                    mensaje += "La autoevaluación está fuera del semáforo. <br>";
                }
            }
            if(obs.esJefeEvaluado)
            {
                if(string.IsNullOrEmpty(obs.jefeObservacion) || obs.jefeObservacion.Trim().Length < 3)
                {
                    mensaje += "La observación de jefe es muy corta. <br>";
                }
                if(!(obs.jefeEvaluacion >= minCalif && obs.jefeEvaluacion <= maxCalif))
                {
                    mensaje += "La evaluación de jefe está fuera del semáforo. <br>";
                }
            }
            //if(obs.lstEvidencia.Count == 0)
            //{
            //    mensaje += "Se necesita adjuntar evidencia. <br>";
            //}
            if(mensaje.Length > 0)
            {
                throw new System.InvalidOperationException(mensaje);
            }
            return true;
        }
        #endregion
        #region combobox
        public ActionResult getCboProceso(int idEmpleado)
        {
            try
            {
                var cbo = desempenoFS.getCboProceso(idEmpleado);
                resultado.Add(ITEMS, cbo);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboEvaluacionPorProceso(int idProceso)
        {
            try
            {
                var cbo = desempenoFS.getCboEvaluacionPorProceso(idProceso);

                resultado.Add(ITEMS, cbo);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboEstrategias()
        {
            try
            {
                var cbo = desempenoFS.getCboEstrategias();
                resultado.Add(ITEMS, cbo);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboMetaPorProceso(int idProceso)
        {
            var usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];

            try
            {
                var cbo = desempenoFS.getCboMetaPorProceso(idProceso, usuario.EmpleadoId);
                resultado.Add(ITEMS, cbo);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboTodosLosProcesos()
        {
            try
            {
                var cbo = desempenoFS.getCboTodosLosProcesos();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, cbo);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Calendario
        public ActionResult _Calendario()
        {
            return PartialView();
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetEvaluaciones(int? idUsuarioVerComo)
        {
            if (Session["usuarioDesempeño"] != null)
            {

            }
            else
            {
                var respuesta = desempenoFS.UsuarioSesion();
                if (respuesta.Success)
                {
                    Session["usuarioDesempeño"] = respuesta.Value;
                }
            }
            var usuario = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];

            var r = desempenoFS.GetEvaluaciones(usuario.EmpleadoId);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult FechaActual()
        {
            return Json(DateTime.Now, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Procesos
        [System.Web.Mvc.HttpPost]
        public JsonResult CRUDProceso(CRUDProcesoDTO objProceso)
        {
            return Json(desempenoFS.CRUDProceso(objProceso));
        }

        public JsonResult ObtenerTodosLosProcesos()
        {
            return Json(desempenoFS.ObtenerTodosLosProcesos(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Empleados Meta
        public ActionResult _FormEmpleadosMeta()
        {
            return PartialView();
        }
        public ActionResult getEmpleadosJefe()
        {
            try
            {
                var sesion = (Core.DTO.RecursosHumanos.Desempeno.UsuarioDTO)Session["usuarioDesempeño"];
                var idJefe = sesion.EmpleadoId;
                var usuario = desempenoFS.getEmpleado(sesion.EmpleadoId);
                var jefeDesc = "";
                if (usuario != null && usuario.jefe != null) {
                    jefeDesc = usuario.jefe.usuario.nombre + " " + usuario.jefe.usuario.apellidoPaterno + " " + usuario.jefe.usuario.apellidoMaterno;
                }
                var lstEmpleado = desempenoFS.getLstEmpleadoJefe(idJefe);
                var lst = lstEmpleado.Select(s => new 
                { 
                    nombre = string.Format("{0} {1} {2}", s.usuario.nombre, s.usuario.apellidoPaterno, s.usuario.apellidoMaterno),
                    puesto = s.usuario.puesto.descripcion,
                    idUsuario = s.usuario.id,
                    idEmpleado = s.id
                }).ToList();
                resultado.Add("lst", lst);
                resultado.Add("jefe", jefeDesc);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Evaluaciones
        public JsonResult GetEvaluaciones()
        {
            return Json(desempenoFS.GetEvaluaciones(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEvaluacionesPorProceso(int idProceso)
        {
            return Json(desempenoFS.GetEvaluacionesPorProceso(idProceso), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CUDEvaluacion(EvaluacionDTO objEvaluacion)
        {
            return Json(desempenoFS.CUDEvaluacion(objEvaluacion));
        }

        public ActionResult getEvaluacionVigenteID(int idProceso)
        {
            //return Json(kpiFs.KPIFactoruServices().GetDatosGraficaParosReservaSinUso(lstCC, lstGrupoID, lstModeloID, lstNoEconomico));
            return Json(desempenoFS.getEvaluacionVigenteID(idProceso));
        }
        #endregion
        #region Empleados
        public ActionResult _Reportes()
        {
            return PartialView();
        }

        public ActionResult CargarTblPersonalEvaluado(int proceso, int periodo)
        {
            try
            {
         
                var auxLst = desempenoFS.CargarTblPersonalEvaluado(proceso, periodo);

                resultado.Add("lst", auxLst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
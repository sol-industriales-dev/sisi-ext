using Core.DAO.RecursosHumanos.Desempeno;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.Entity.RecursosHumanos.Desempeno;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Enum.RecursosHumanos;
using Core.DTO.RecursosHumanos.Desempeno;
using System.Web;
using Data.DAO.Principal.Archivos;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using System.IO;
using Data.EntityFramework.Context;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.Enum.Multiempresa;

namespace Data.DAO.RecursosHumanos.Desempeno
{
    public class DesempenoDAO : GenericDAO<tblRH_ED_CatProceso>, IDesempenoDAO
    {
        string nombreControlador = "Desempeno";

        //private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO";
        private readonly string RutaTemp;

        public DesempenoDAO()
        {
            
        }

        #region coloresEventos
        const string colorProceso = "#0561b0";
        const string colorEvaluacion = "#35b445";
        const string colorWarning = "#f1ce11";
        const string colorDanger = "#b63939";
        #endregion
        public Respuesta UsuarioSesion()
        {
            var r = new Respuesta();
            try
            {
                var usuarioSesion = new UsuarioDTO();

                var empleado = _context.tblRH_ED_Empleado.FirstOrDefault(x => x.empleadoID == vSesiones.sesionUsuarioDTO.id && x.estatus);
                if (empleado == null)
                {
                    usuarioSesion.esAdmin = false;
                    usuarioSesion.EmpleadoId = vSesiones.sesionUsuarioDTO.id;
                    usuarioSesion.Id = 0;
                    usuarioSesion.JefeId = 0;
                    usuarioSesion.Nombre = vSesiones.sesionUsuarioDTO.nombre + " " + vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno;
                    usuarioSesion.TipoUsuario = 3;
                }
                else
                {
                    usuarioSesion.esAdmin = empleado.tipo == ED_TipoEmpleado.ADMIN;
                    usuarioSesion.EmpleadoId = empleado.empleadoID;
                    usuarioSesion.Id = empleado.id;
                    usuarioSesion.JefeId = empleado.jefeID;
                    usuarioSesion.Nombre = vSesiones.sesionUsuarioDTO.nombre + " " + vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno;
                    usuarioSesion.TipoUsuario = (int)empleado.tipo;
                }

                r.Success = true;
                r.Message = "Ok";
                r.Value = usuarioSesion;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }
            return r;
        }
        #region Meta
        public bool guardarMeta(tblRH_ED_DetMetas meta)
        {
            //meta.idUsuario = vSesiones.sesionUsuarioDTO.id;
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    if (meta.idJefe == 0)
                    {
                        var jefe = _context.tblRH_ED_Empleado.First(x => x.empleadoID == meta.idUsuario && x.estatus);
                        if (jefe.jefeID != null)
                        {
                            meta.idJefe = jefe.jefe.empleadoID;
                            //meta.idJefe = Convert.ToInt32(jefe.jefeID);
                        }
                    }
                    var metas = _context.tblRH_ED_DetMetas.Where(x => x.idProceso == meta.idProceso && x.idUsuario == meta.idUsuario && x.esActivo).ToList();
                    var bd = metas.FirstOrDefault(m => m.id == meta.id);
                    if (bd == null)
                    {
                        meta.fechaRegistro = DateTime.Now;
                        if (metas.Sum(s => s.peso) + meta.peso > 100)
                        {
                            dbTransaction.Rollback();
                            return esGuardado = false;
                        }
                        if (vSesiones.sesionUsuarioDTO.id != meta.idUsuario)
                        {
                            meta.notificado = true;
                        }
                    }
                    else
                    {
                        meta.notificado = bd.notificado;
                        if (metas.Sum(s => s.peso) - bd.peso + meta.peso > 100)
                        {
                            dbTransaction.Rollback();
                            return esGuardado = false;
                        }
                        meta.id = bd.id;
                        meta.fechaRegistro = bd.fechaRegistro;
                    }
                    meta.esActivo = true;
                    if (meta.id > 0)
                    {
                        tblRH_ED_DetMetas objActualizarMeta = _context.tblRH_ED_DetMetas.Where(w => w.id == meta.id).FirstOrDefault();
                        objActualizarMeta.nombre = meta.nombre;
                        objActualizarMeta.tipo = meta.tipo;
                        objActualizarMeta.descripcion = meta.descripcion;
                        objActualizarMeta.peso = meta.peso;
                        objActualizarMeta.esVobo = meta.esVobo;
                        objActualizarMeta.notificado = meta.notificado;
                        objActualizarMeta.esActivo = meta.esActivo;
                        objActualizarMeta.fechaRegistro = meta.fechaRegistro;
                        _context.SaveChanges();
                    }
                    else if (meta.id == 0)
                    {
                        _context.tblRH_ED_DetMetas.Add(meta);
                        _context.SaveChanges();
                    }
                    //_context.tblRH_ED_DetMetas.AddOrUpdate(meta);
                    //_context.SaveChanges();
                    dbTransaction.Commit();
                    esGuardado = meta.id > 0;
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarMeta", o_O, AccionEnum.ACTUALIZAR, meta.id, meta);
                }
            return esGuardado;
        }

        public bool guardarMeta(List<tblRH_ED_DetMetas> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    lst.ForEach(meta =>
                    {
                        var bd = _context.tblRH_ED_DetMetas.FirstOrDefault(m => m.id == meta.id);
                        bd.peso = meta.peso;
                        bd.fechaRegistro = DateTime.Now;
                        if (bd != null)
                        {
                            meta.notificado = bd.notificado;
                        }
                        //if (vSesiones.sesionUsuarioDTO.id == bd.idJefe)
                        //{
                        //    //bd.esVobo = true;
                        //    bd.notificado = true;
                        //}
                        //if(bd == null)
                        //{
                        //    meta.fechaRegistro = DateTime.Now;
                        //}
                        //else
                        //{
                        //    meta.id = bd.id;
                        //    meta.fechaRegistro = bd.fechaRegistro;
                        //}
                        //meta.esActivo = true;
                        //_context.tblRH_ED_DetMetas.AddOrUpdate(bd);
                        _context.SaveChanges();
                    });
                    dbTransaction.Commit();
                    esGuardado = lst.All(a => a.id > 0);
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarMeta", o_O, AccionEnum.ACTUALIZAR, lst.FirstOrDefault().id, lst.FirstOrDefault());
                }
            return esGuardado;
        }

        public bool eliminarMeta(int id)
        {
            var esEliminado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var bd = _context.tblRH_ED_DetMetas.FirstOrDefault(m => m.id == id);
                    esEliminado = bd != null;
                    if (esEliminado)
                    {
                        bd.esActivo = false;
                        _context.tblRH_ED_DetMetas.AddOrUpdate(bd);
                        _context.SaveChanges();

                        var evaluaciones = _context.tblRH_ED_DetObservacion.Where(x => x.esActivo && x.idMeta == id).ToList();
                        foreach (var item in evaluaciones)
                        {
                            item.esActivo = false;
                            foreach (var evidencia in item.lstEvidencia)
                            {
                                evidencia.esActivo = false;
                            }
                        }
                        _context.SaveChanges();

                        dbTransaction.Commit();
                    }
                    else
                    {
                        throw new System.InvalidOperationException("La meta no existe. Verifique su información");
                    }
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "eliminarMeta", o_O, AccionEnum.ELIMINAR, id, id);
                }
            return esEliminado;
        }

        public bool darVoboMeta(tblRH_ED_DetMetas meta)
        {
            var esSuccess = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    _context.tblRH_ED_DetMetas.AddOrUpdate(meta);
                    _context.SaveChanges();
                    dbTransaction.Commit();
                    esSuccess = true;
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "darVoboMeta", o_O, AccionEnum.ELIMINAR, meta.id, meta);
                }
            return esSuccess;
        }

        public Respuesta VoBoMetas(int idProceso, int idEmpleado, int idEvaluacion)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var metas = _context.tblRH_ED_DetMetas.Where(x => x.idProceso == idProceso && x.esActivo && x.idUsuario == idEmpleado && x.notificado).ToList();

                    foreach (var meta in metas)
                    {
                        meta.esVobo = true;
                    }

                    _context.SaveChanges();

                    var objId = int.Parse(idEmpleado.ToString() + idProceso.ToString());
                    var notificaciones = _context.tblP_Alerta.Where(x => x.userRecibeID == vSesiones.sesionUsuarioDTO.id && x.userEnviaID == idEmpleado && !x.visto && x.objID == objId).ToList();

                    foreach (var notificacion in notificaciones)
                    {
                        notificacion.visto = true;
                    }

                    _context.SaveChanges();

                    var nuevaAlerta = new tblP_Alerta();
                    nuevaAlerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                    nuevaAlerta.userRecibeID = idEmpleado;
                    nuevaAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                    nuevaAlerta.sistemaID = (int)SistemasEnum.RH;
                    nuevaAlerta.visto = false;
                    nuevaAlerta.url = "/Administrativo/Desempeno/Dashboard?pro=" + idProceso + "&eva=" + idEvaluacion;
                    nuevaAlerta.objID = int.Parse(idProceso.ToString() + idEvaluacion.ToString());
                    nuevaAlerta.obj = null;
                    nuevaAlerta.msj = "Metas listas para evaluar";
                    nuevaAlerta.documentoID = null;
                    nuevaAlerta.moduloID = (int)BitacoraEnum.Metas;

                    _context.tblP_Alerta.Add(nuevaAlerta);
                    _context.SaveChanges();

                    //CORREO
                    string cuerpo = string.Format(@"
                                    <h3>Desempeño</h3>
                                    <p>Las metas del proceso <strong>{0}</strong> ya cuentan con visto bueno y listas para evaluar.</p>
                                    <p>Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx'>SIGOPLAN</a>, seleccionar empresa <strong>CONSTRUPLAN</strong>, sección <strong>Capital Humano</strong>, menú <strong>Evaluación de Desempeño</strong>.</p>",
                                    metas.First().proceso.proceso
                            );

                    var resultadoMail = Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Desempeño: Metas del Proceso - " + metas.First().proceso.proceso + " - listas para evaluar"), cuerpo, new List<string> { metas.First().usuario.correo /*"omar.nunez@construplan.com.mx"*/ });

                    if (resultadoMail)
                    {
                        transaction.Commit();

                        r.Success = true;
                        r.Message = "Ok";
                    }
                    else
                    {
                        transaction.Rollback();
                        r.Message = "Ha ocurrido un error al enviar el correo de notificación";
                    }
                    //CORREO FIN
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }

            return r;
        }

        public List<tblRH_ED_DetMetas> getLstMetaPorProceso(int idProceso, int usuarioID)
        {
            var empleado = _context.tblRH_ED_Empleado.FirstOrDefault(w => w.empleadoID == usuarioID);
            var relProcesoUsuario = _context.tblRH_ED_RelacionesEmpleadoProceso.Where(w => w.Estatus && w.EmpleadoId == empleado.id && idProceso == w.ProcesoId).Select(s => s.ProcesoId).ToList();
            var lst = _context.tblRH_ED_DetMetas.Where(w => w.esActivo && relProcesoUsuario.Contains(w.idProceso) && w.idUsuario == usuarioID).ToList();
            return lst;
        }

        public List<tblRH_ED_DetMetas> getLstMetaPorProceso(int idProceso, List<int> lstIdUsuario)
        {
            var lstEmpleado = _context.tblRH_ED_Empleado.Where(w => lstIdUsuario.Contains(w.empleadoID)).Select(s => s.id).ToList();
            var relProcesoUsuario = _context.tblRH_ED_RelacionesEmpleadoProceso.Where(w => w.Estatus && lstEmpleado.Contains(w.EmpleadoId) && idProceso == w.ProcesoId).Select(s => s.ProcesoId).ToList();
            var lst = _context.tblRH_ED_DetMetas.Where(w => w.esActivo && relProcesoUsuario.Contains(w.idProceso) && lstIdUsuario.Contains(w.idUsuario)).ToList();
            return lst;
        }

        public tblRH_ED_DetMetas getMeta(int idMeta)
        {
            return _context.tblRH_ED_DetMetas.FirstOrDefault(m => m.id == idMeta);
        }

        public Respuesta DiasSeguimiento(int idProceso)
        {
            var r = new Respuesta();
            try
            {
                var fechaActual = DateTime.Now;
                var proceso = _context.tblRH_ED_CatProceso.FirstOrDefault(x => x.id == idProceso && x.esActivo);

                var mensaje = new FechaSeguimientoDTO();
                var dias = 0;

                if (proceso != null)
                {
                    if (proceso.lstEvaluacion.Count == 0)
                    {
                        if (proceso.fechaInicio <= fechaActual && proceso.fechaFin >= fechaActual)
                        {
                            mensaje.Titulo = "Periodo del proceso iniciado";
                            dias = (proceso.fechaFin - fechaActual).Days;
                            mensaje.Dias = dias.ToString();
                            mensaje.MensajeFinal = "días restantes";
                            mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
                        }
                    }
                    else
                    {
                        var evaluacion = proceso.lstEvaluacion.FirstOrDefault(f => f.esActivo && f.fechaInicio <= fechaActual && f.fechaFin >= fechaActual);
                        if (evaluacion != null)
                        {
                            mensaje.Titulo = "Periodo de seguimiento de " + evaluacion.descripcion + " iniciado";
                            dias = (evaluacion.fechaFin - fechaActual).Days;
                            mensaje.Dias = dias.ToString();
                            mensaje.MensajeFinal = "días restantes";
                            mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
                            mensaje.EvaluacionId = evaluacion.id;
                        }
                        else
                        {
                            evaluacion = proceso.lstEvaluacion.Where(x => x.esActivo && x.fechaInicio >= fechaActual).OrderBy(o => o.fechaInicio).FirstOrDefault();
                            if (evaluacion != null)
                            {
                                mensaje.Titulo = "Próximo seguimiento en";
                                dias = (evaluacion.fechaInicio - fechaActual).Days;
                                mensaje.Dias = dias.ToString();
                                mensaje.MensajeFinal = "días";
                                mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
                            }
                        }
                    }
                }

                if (mensaje.Titulo == null)
                {
                    mensaje.Titulo = "Próximo seguimiento en";
                    mensaje.Dias = dias.ToString();
                    mensaje.MensajeFinal = "días";
                    mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
                }

                r.Success = true;
                r.Message = "Ok";
                r.Value = mensaje;

            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }
            return r;
            //var r = new Respuesta();

            //try
            //{
            //    var mensaje = new FechaSeguimientoDTO();
            //    var fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //    var dias = 0;

            //    List<tblRH_ED_RelacionEmpleadoProceso> relacionEmpleadoProcesos = null;
            //    tblRH_ED_CatProceso proceso = null;
            //    tblRH_ED_CatEvaluacion evaluacion = null;

            //    if (idEvaluacion != 0)
            //    {
            //        relacionEmpleadoProcesos = _context.tblRH_ED_RelacionesEmpleadoProceso.Where(x => x.Estatus && x.Empleado.id == idEmpleado).ToList();
            //        if (relacionEmpleadoProcesos.Count > 0)
            //        {
            //            proceso = relacionEmpleadoProcesos.First(x => x.Proceso.lstEvaluacion.Where(y => y.esActivo).Select(m => m.id).Contains(idEvaluacion)).Proceso;

            //            evaluacion = proceso.lstEvaluacion.FirstOrDefault(x => x.esActivo && x.id == idEvaluacion && x.fechaInicio <= fechaActual && x.fechaFin >= fechaActual);

            //            if (evaluacion != null)
            //            {
            //                mensaje.Titulo = "Periodo de evaluación " + evaluacion.descripcion + " iniciado";
            //                dias = (evaluacion.fechaFin - fechaActual).Days + 1;
            //                mensaje.Dias = dias.ToString();
            //                mensaje.MensajeFinal = "Días restantes";
            //                mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
            //            }
            //            else
            //            {
            //                evaluacion = proceso.lstEvaluacion.Where(x => x.esActivo && x.id != idEvaluacion && x.fechaFin >= fechaActual && x.fechaInicio <= fechaActual).OrderBy(o => o.fechaFin).FirstOrDefault();
            //                if (evaluacion != null)
            //                {
            //                    mensaje.Titulo = "Periodo de evaluación de " + evaluacion.descripcion + " iniciado";
            //                    dias = (evaluacion.fechaFin - fechaActual).Days + 1;
            //                    mensaje.Dias = dias.ToString();
            //                    mensaje.MensajeFinal = "Días restantes";
            //                    mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
            //                }
            //                if (evaluacion == null)
            //                {
            //                    evaluacion = proceso.lstEvaluacion.Where(x => x.esActivo && x.id != idEvaluacion && x.fechaInicio >= fechaActual).OrderBy(o => o.fechaInicio).FirstOrDefault();
            //                    if (evaluacion != null)
            //                    {
            //                        mensaje.Titulo = "Próxima evaluación de " + evaluacion.descripcion + " inicia en:";
            //                        dias = (evaluacion.fechaInicio - fechaActual).Days + 1;
            //                        mensaje.Dias = dias.ToString();
            //                        mensaje.MensajeFinal = "días";
            //                        mensaje.Estatus = "verde";
            //                    }
            //                }
            //                if (evaluacion == null)
            //                {
            //                    var procesos = relacionEmpleadoProcesos.Where(x => !x.Proceso.lstEvaluacion.Select(m => m.id).Contains(idEvaluacion)).ToList();

            //                    if (procesos.Count > 0)
            //                    {
            //                        var pro = procesos.Where(x => x.Proceso.fechaFin >= fechaActual && x.Proceso.fechaInicio <= fechaActual).OrderBy(o => o.Proceso.fechaFin).FirstOrDefault();
            //                        if (pro != null)
            //                        {
            //                            mensaje.Titulo = "Proceso " + pro.Proceso.proceso + " iniciado";
            //                            dias = (pro.Proceso.fechaFin - fechaActual).Days + 1;
            //                            mensaje.Dias = dias.ToString();
            //                            mensaje.MensajeFinal = "días restantes";
            //                            mensaje.Estatus = dias > 5 ? "verde" : dias > 3 ? "amarillo" : "rojo";
            //                        }
            //                        if (pro == null)
            //                        {
            //                            pro = procesos.Where(x => x.Proceso.fechaInicio >= fechaActual).OrderBy(o => o.Proceso.fechaInicio).FirstOrDefault();
            //                            if (pro != null)
            //                            {
            //                                mensaje.Titulo = "Próximo proceso " + pro.Proceso.proceso + " inicia en:";
            //                                dias = (pro.Proceso.fechaInicio - fechaActual).Days + 1;
            //                                mensaje.Dias = dias.ToString();
            //                                mensaje.MensajeFinal = "días";
            //                                mensaje.Estatus = "verde";
            //                            }
            //                            else
            //                            {
            //                                mensaje.Titulo = "No hay eventos";
            //                                mensaje.Dias = "";
            //                                mensaje.MensajeFinal = "";
            //                                mensaje.Estatus = "verde";
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            mensaje.Titulo = "";
            //            mensaje.Dias = "";
            //            mensaje.Estatus = "rojo";
            //            mensaje.MensajeFinal = "";
            //        }
            //    }
            //    else
            //    {
            //        mensaje.Titulo = "";
            //        mensaje.Dias = "";
            //        mensaje.Estatus = "rojo";
            //        mensaje.MensajeFinal = "";
            //    }

            //    r.Success = true;
            //    r.Message = "Ok";
            //    r.Value = mensaje;
            //}
            //catch (Exception ex)
            //{
            //    r.Message += ex.Message;
            //}

            //return r;
        }

        public bool Notificado(int idEmpleado, int idEvaluacion, int? idJefe)
        {
            var empleado = _context.tblRH_ED_Empleado.First(x => x.empleadoID == idEmpleado);
            if (empleado.jefeID == null)
            {
                return true;
            }
            var resultado = false;
            var metas = _context.tblRH_ED_DetMetas.Where(x => x.idUsuario == idEmpleado && x.esActivo && x.proceso.lstEvaluacion.Select(m => m.id).Contains(idEvaluacion)).ToList();
            if (metas.Count > 0)
            {
                var observaciones = metas.Where(x => x.observaciones.Where(w => w.idEvaluacion == idEvaluacion).Count() > 0).ToList();
                if (observaciones.Count != metas.Count)
                {
                    return true;
                }
                if (idJefe == null)
                {
                    resultado = metas.Where(x => x.observaciones.Where(w => w.idEvaluacion == idEvaluacion && w.notificado).Count() > 0).Count() == metas.Count;
                }
                else
                {
                    resultado = metas.Where(x => x.observaciones.Where(w => w.idEvaluacion == idEvaluacion && w.notificado && w.esJefeEvaluado && !w.notificadoJefeAUsuario).Count() > 0).Count() != metas.Count;
                }
                //resultado = metas.Where(x => x.observaciones.Where(w => w.idEvaluacion == idEvaluacion && ((idJefe == null  && w.notificado) || (idJefe != null && w.notificado && !w.esJefeEvaluado)) ).Select(m => idJefe == null ? m.esAutoEvaluado : m.esJefeEvaluado).Contains(idJefe == null ? true : false)).Count() == (idJefe == null ? metas.Count : 0);
                //resultado = metas.Where(x => x.observaciones.Where(w => w.idEvaluacion == idEvaluacion && ((idJefe == null && w.notificado) || (idJefe != null && w.notificado))).Select(m => m.esAutoEvaluado).Contains(true)).Count() == metas.Count;
                //if (resultado)
                //{
                //    resultado = false;
                //}
                //resultado = metas.Where(x => x.proceso.lstEvaluacion.Where(y => y.observaciones.Any(a => a.esAutoEvaluado)));
                //resultado = metas.First().proceso.lstEvaluacion.First().observaciones.Any(x => x.idMeta == metas.First().id && !x.notificado && x.idEvaluacion == idEvaluacion && x.esActivo);
                //resultado = _context.tblRH_ED_DetObservacion.Any(x => x.esActivo && x.idMeta == metas.First().id && x.idEvaluacion == idEvaluacion && x.notificado);
            }
            else
            {
                resultado = true;
            }
            //var notificado = _context.tblRH_ED_DetObservacion.Where(x => x.esActivo && x.idEvaluacion == idEvaluacion && x.idUsuario == idEmpleado).ToList();
            //if (notificado.Count == 0)
            //{
            //    resultado = true;
            //}
            //else
            //{
            //    resultado = notificado.Any(x => x.notificado);
            //}
            return resultado;
        }

        public Respuesta Notificar(int idEmpleado, int idEvaluacion, int idProceso, int? idJefe)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var empleado = _context.tblRH_ED_Empleado.First(x => x.empleadoID == idEmpleado);
                    if (empleado.jefeID != null)
                    {
                        var detObservacion = _context.tblRH_ED_DetObservacion.Where(x => x.esActivo && x.idUsuario == idEmpleado && x.idEvaluacion == idEvaluacion);
                        if (detObservacion != null && detObservacion.Count() > 0)
                        {
                            if (idJefe == null ? empleado.jefe.usuario.id == vSesiones.sesionUsuarioDTO.id : empleado.jefe.id == idJefe.Value)
                            {
                                if (detObservacion.Any(a => a.esJefeEvaluado))
                                {
                                    var evaluacion = detObservacion.First().evaluacion;

                                    foreach (var item in detObservacion.Where(w => w.esJefeEvaluado))
                                    {
                                        item.notificadoJefeAUsuario = true;
                                    }

                                    string nombre = string.Empty;
                                    string apellidoPaterno = string.Empty;
                                    string apellidoMaterno = string.Empty;

                                    if (detObservacion.First().jefe.nombre != "")
                                        nombre = detObservacion.First().jefe.nombre;

                                    if (detObservacion.First().jefe.apellidoPaterno != "")
                                        apellidoPaterno = detObservacion.First().jefe.apellidoPaterno;

                                    if (detObservacion.First().jefe.apellidoMaterno != "")
                                        apellidoMaterno = detObservacion.First().jefe.apellidoMaterno;

                                    var objIdString = empleado.empleadoID.ToString() + idEvaluacion.ToString();
                                    var objId = int.Parse(objIdString);

                                    var notificaciones = _context.tblP_Alerta.Where
                                        (w =>
                                            w.userRecibeID == empleado.jefe.empleadoID &&
                                            w.userEnviaID == empleado.empleadoID &&
                                            !w.visto &&
                                            w.objID == objId &&
                                            w.sistemaID == (int)SistemasEnum.RH &&
                                            w.moduloID == (int)BitacoraEnum.Metas
                                        ).ToList();

                                    foreach (var notificacion in notificaciones)
                                    {
                                        notificacion.visto = true;
                                    }

                                    _context.SaveChanges();

                                    string cuerpo = string.Format(@"
                                    <h3>Desempeño - {0}</h3>
                                    <p>Se ha realizado tu evaluación de <strong>{1}</strong> por {2}</p>
                                    <p>Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx'>SIGOPLAN</a>, seleccionar empresa <strong>CONSTRUPLAN</strong>, sección <strong>Capital Humano</strong>, menú <strong>Evaluación de Desempeño</strong>.</p>",
                                                        evaluacion.proceso.proceso,
                                                        evaluacion.descripcion,
                                                        nombre + " " +
                                                        apellidoPaterno + " " +
                                                        apellidoMaterno
                                    );

                                    var resultadoMail = Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Desempeño: Evaluación " + evaluacion.descripcion), cuerpo, new List<string> { empleado.usuario.correo /*"omar.nunez@construplan.com.mx"*/ });

                                    if (resultadoMail)
                                    {
                                        _context.SaveChanges();

                                        transaction.Commit();

                                        r.Success = true;
                                        r.Message = "Ok";
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        r.Message = "Ocurrió un error al enviar la notificación al usuario";
                                    }
                                }
                                else
                                {
                                    r.Message = "No se encontraron evaluaciones del jefe hacia el usuario para notificar al usuario";
                                }
                                
                            }
                            else
                            {
                                foreach (var item in detObservacion)
                                {
                                    item.notificado = true;
                                }
                                _context.SaveChanges();

                                var asd = new string[1] { " " };
                                string nombre = string.Empty;
                                string apellidoPaterno = string.Empty;
                                string apellidoMaterno = string.Empty;

                                if (empleado.usuario.nombre.Split(asd, StringSplitOptions.None).First() != "")
                                    nombre = empleado.usuario.nombre.Split(asd, StringSplitOptions.None).First();

                                if (!string.IsNullOrEmpty(empleado.usuario.apellidoPaterno))
                                    apellidoPaterno = empleado.usuario.apellidoPaterno.ToUpper().ElementAt(0).ToString();

                                if (!string.IsNullOrEmpty(empleado.usuario.apellidoMaterno))
                                    apellidoMaterno = empleado.usuario.apellidoMaterno.ToUpper().ElementAt(0).ToString();

                                var notificacion = new tblP_Alerta();
                                notificacion.userEnviaID = idEmpleado;
                                notificacion.userRecibeID = empleado.jefe.empleadoID;
                                notificacion.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                                notificacion.sistemaID = (int)SistemasEnum.RH;
                                notificacion.visto = false;
                                notificacion.url = "/Administrativo/Desempeno/Dashboard?info=" + idEmpleado + "&eva=" + idEvaluacion + "&pro=" + idProceso;
                                notificacion.objID = Convert.ToInt32(idEmpleado + "" + idEvaluacion);
                                notificacion.msj = "Evaluación " +
                                                    nombre + " " + 
                                                    apellidoPaterno + 
                                                    apellidoMaterno;
                                notificacion.moduloID = (int)BitacoraEnum.Metas;

                                _context.tblP_Alerta.Add(notificacion);
                                _context.SaveChanges();

                                //CORREO
                                string cuerpo = string.Format(@"
                                    <h3>Desempeño</h3>
                                    <p>El usuario <strong>{0}</strong>, a realizado su autoevaluación del seguimiento de <strong>{1}</strong> del proceso <strong>{2}</strong>.</p>
                                    <p>Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx'>SIGOPLAN</a>, seleccionar empresa <strong>CONSTRUPLAN</strong>, sección <strong>Capital Humano</strong>, menú <strong>Evaluación de Desempeño</strong>.</p>",
                                            empleado.usuario.nombre + " " + empleado.usuario.apellidoPaterno ?? "" + " " + empleado.usuario.apellidoMaterno ?? "",
                                            detObservacion.First().evaluacion.descripcion,
                                            detObservacion.First().evaluacion.proceso.proceso
                                        );

                                var resultadoMail = Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Desempeño: autoevaluación de " + detObservacion.First().evaluacion.descripcion + " del usuario " + empleado.usuario.nombre + " " + empleado.usuario.apellidoPaterno ?? "" + " " + empleado.usuario.apellidoMaterno ?? ""), cuerpo, new List<string> { empleado.jefe.usuario.correo /*"omar.nunez@construplan.com.mx"*/ });

                                if (resultadoMail)
                                {
                                    transaction.Commit();

                                    r.Success = true;
                                    r.Message = "Ok";
                                }
                                else
                                {
                                    transaction.Rollback();
                                    r.Message = "Ha ocurrido un error al enviar el correo de notificación";
                                }
                                //CORREO FIN
                            }
                        }
                        else
                        {
                            r.Message = "No se encontró evaluación que notificar";
                        }
                    }
                    else
                    {
                        r.Message = "No hay a quien notificar";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }

            return r;
        }

        public Respuesta NotificarMetas(int idEmpleado, int idProceso)
        {
            var r = new Respuesta();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var empleado = _context.tblRH_ED_Empleado.FirstOrDefault(x => x.empleadoID == idEmpleado && x.estatus);
                    if (empleado.jefeID == null)
                        empleado.jefeID = empleado.empleadoID;

                    if (empleado != null && empleado.jefeID != null)
                    {
                        var metas = _context.tblRH_ED_DetMetas.Where(x => x.idUsuario == idEmpleado && x.idProceso == idProceso && x.esActivo);
                        foreach (var meta in metas)
                        {
                            meta.notificado = true;
                        }
                        _context.SaveChanges();

                        string nombre = string.Empty;
                        string apellidoPaterno = string.Empty;
                        string apellidoMaterno = string.Empty;

                        if (empleado.usuario.nombre.Split(new string[1] { " " }, StringSplitOptions.None).First() != "")
                            nombre = empleado.usuario.nombre.Split(new string[1] { " " }, StringSplitOptions.None).First();

                        if (!string.IsNullOrEmpty(empleado.usuario.apellidoPaterno))
                            apellidoPaterno = empleado.usuario.apellidoPaterno.ToUpper().ElementAt(0).ToString();

                        if (!string.IsNullOrEmpty(empleado.usuario.apellidoMaterno))
                            apellidoMaterno = empleado.usuario.apellidoMaterno.ToUpper().ElementAt(0).ToString();

                        var notificacion = new tblP_Alerta();
                        notificacion.userEnviaID = idEmpleado;
                        notificacion.userRecibeID = Convert.ToInt32(empleado.jefe.empleadoID);
                        notificacion.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        notificacion.sistemaID = (int)SistemasEnum.RH;
                        notificacion.visto = false;
                        notificacion.url = "/Administrativo/Desempeno/Dashboard?info=" + idEmpleado + "&pro=" + idProceso;
                        notificacion.objID = Convert.ToInt32(idEmpleado + "" + idProceso);
                        notificacion.msj = "Metas " +
                                            nombre + " " +
                                            apellidoPaterno +
                                            apellidoMaterno;
                        notificacion.moduloID = (int)BitacoraEnum.Metas;

                        _context.tblP_Alerta.Add(notificacion);
                        _context.SaveChanges();

                        //CORREO
                        string cuerpo = string.Format(@"
                                    <h3>Desempeño</h3>
                                    <p>El usuario <strong>{0}</strong>, a creado metas para el proceso: <strong>{1}</strong> y esta en espera de que le de visto bueno.</p>
                                    <p>Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx'>SIGOPLAN</a>, seleccionar empresa <strong>CONSTRUPLAN</strong>, sección <strong>Capital Humano</strong>, menú <strong>Evaluación de Desempeño</strong>.</p>",
                                    empleado.usuario.nombre + " " + empleado.usuario.apellidoPaterno ?? "" + " " + empleado.usuario.apellidoMaterno ?? "",
                                    metas.First().proceso.proceso
                                );

                        var resultadoMail = Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Desempeño: Metas para VoBo del Proceso - " + metas.First().proceso.proceso), cuerpo, new List<string> { empleado.jefe.usuario.correo /*"omar.nunez@construplan.com.mx"*/ });

                        if (resultadoMail)
                        {
                            transaction.Commit();

                            r.Success = true;
                            r.Message = "Ok";
                        }
                        else
                        {
                            transaction.Rollback();
                            r.Message = "Ha ocurrido un error al enviar el correo de notificación";
                        }
                        //CORREO FIN
                    }
                    else
                    {
                        //r.Message = "No hay a quien notificar";
                        r.Message = "No se encuentra al usuario a notificar.";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    r.Message += ex.Message;
                }
            }
            return r;
        }
        #endregion
        #region Observacion
        public List<tblRH_ED_DetObservacion> getLstObservacionesPorUsuario(int idUsuario)
        {
            return _context.tblRH_ED_DetObservacion.Where(w => w.esActivo && w.idUsuario == idUsuario).ToList();
        }

        //public List<tblRH_ED_DetObservacion> getLstObservacionesPorUsuario(int idUsuario, int idProceso, int idEvaluacion)
        //{
        //    var lst = _context.tblRH_ED_DetObservacion.Where(w => w.esActivo && w.idUsuario == idUsuario && w.idEvaluacion == idEvaluacion && w.evaluacion.idProceso == idProceso).ToList();
        //    if(lst.Count == 0)
        //    {

        //    }
        //    return lst;
        //}

        public List<tblRH_ED_DetObservacion> getLstObservacionesPorProcesosYJefe(int idProceso, int idJefe)
        {
            return _context.tblRH_ED_DetObservacion.Where(w => w.esActivo && w.meta.esActivo && w.meta.idProceso == idProceso && (w.idUsuario == idJefe || w.idJefe == idJefe)).ToList();
        }

        public tblRH_ED_DetObservacion getObservacion(int idMeta, int idEvaluacion, int idUsuarioCalificar)
        {
            var observacion = _context.tblRH_ED_DetObservacion.FirstOrDefault(o => o.idEvaluacion == idEvaluacion && o.idMeta == idMeta && o.idUsuario == idUsuarioCalificar)
             ?? new tblRH_ED_DetObservacion();
            if (observacion.id == 0)
            {
                observacion.idMeta = idMeta;
                observacion.meta = getMeta(idMeta);
                observacion.idUsuario = idUsuarioCalificar;
                observacion.usuario = _context.tblP_Usuario.FirstOrDefault(u => u.id == observacion.idUsuario);
                observacion.lstEvidencia = new List<tblRH_ED_DetObservacionEvidencia>();
            }
            if (observacion.idJefe == 0)
            {
                var empleado = _context.tblRH_ED_Empleado.FirstOrDefault(e => e.empleadoID == idUsuarioCalificar) ?? new tblRH_ED_Empleado();
                //observacion.idJefe = empleado.jefe.empleadoID;
                observacion.idJefe = Convert.ToInt32(empleado.jefeID);
                //observacion.idJefe = empleado.jefeID.Value;
                observacion.jefe = _context.tblP_Usuario.FirstOrDefault(u => u.id == observacion.idJefe) ?? new tblP_Usuario();
            }
            return observacion;
        }

        public string metaEvidenciaGuardar(tblRH_ED_DetObservacion eva, List<HttpPostedFileBase> lstArchivo)
        {
            var msjResultado = string.Empty;
            string resultado = string.Empty;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    foreach (var item in eva.lstEvidencia)
                    {
                        item.fechaRegistro = DateTime.Now;
                    }
                    var ahora = DateTime.Now;
                    var bd = _context.tblRH_ED_DetObservacion.FirstOrDefault(m => m.id == eva.id) ?? new tblRH_ED_DetObservacion()
                    {
                        lstEvidencia = new List<tblRH_ED_DetObservacionEvidencia>(),
                        fechaRegistro = ahora,
                        esActivo = true
                    };

                    var lstEvidencia = _context.tblRH_ED_DetObservacionEvidencia.Where(w => w.idObservacion == eva.id && w.esActivo);
                    var dirArchivos = new DirArchivosDAO().getRegistro(1020);
                    #region Cargar Propiedades Virtuales
                    eva.notificado = bd.notificado;
                    if (eva.fechaRegistro == default(DateTime))
                    {
                        eva.fechaRegistro = bd.fechaRegistro;
                    }
                    if (eva.evaluacion == null || eva.evaluacion.id == 0)
                    {
                        eva.evaluacion = _context.tblRH_ED_CatEvaluacion.FirstOrDefault(e => e.id == eva.idEvaluacion);
                    }
                    if (eva.meta == null || eva.meta.id == 0)
                    {
                        eva.meta = getMeta(eva.idMeta);
                    }
                    if (eva.lstEvidencia == null)
                    {
                        eva.lstEvidencia = new List<tblRH_ED_DetObservacionEvidencia>();
                    }

                    eva.usuario = _context.tblP_Usuario.FirstOrDefault(u => u.id == eva.idUsuario) ?? new tblP_Usuario();
                    tblRH_ED_Empleado jefe = null;
                    //if (!eva.esJefeEvaluado)
                    //{
                    //    //jefe = _context.tblRH_ED_Empleado.FirstOrDefault(u => u.id == eva.idJefe);
                    //    jefe = _context.tblRH_ED_Empleado.FirstOrDefault(u => u.jefeID == eva.idJefe);
                    //}
                    //else
                    //{
                    //    jefe = _context.tblRH_ED_Empleado.FirstOrDefault(u => u.empleadoID == eva.idJefe);
                    //}

                    var __empleado = _context.tblRH_ED_Empleado.First(x => x.empleadoID == eva.usuario.id);
                    eva.jefe = __empleado.jefe.usuario;
                    eva.idJefe = __empleado.jefe.empleadoID;
                    
                    //if (jefe == null)
                    //{
                    //    jefe = _context.tblRH_ED_Empleado.Where(x => x.empleadoID == eva.idJefe).FirstOrDefault();
                    //    jefe.jefeID = jefe.id;
                    //}

                    //var lstEmpleados = _context.tblRH_ED_Empleado.Where(x => x.id == eva.idJefe || x.id == jefe.jefeID).FirstOrDefault();
                    //int idJefe = lstEmpleados.empleadoID;
                    //eva.jefe = _context.tblP_Usuario.FirstOrDefault(u => u.id == idJefe) ?? new tblP_Usuario();
                    //eva.jefe = jefe.usuario;
                    //eva.idJefe = eva.idJefe;
                    #endregion

                    var cantMetasNotificadas = _context.tblRH_ED_DetMetas.Where(x => x.id == eva.idMeta && x.esActivo && x.notificado).Count();
                    if (cantMetasNotificadas > 0)
                    {
                        //eva.notificado = true;
                    }

                    if (eva.esJefeEvaluado)
                        eva.notificado = true;

                    if (eva.id == 0)
                    {
                        _context.tblRH_ED_DetObservacion.Add(eva);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var __eva = _context.tblRH_ED_DetObservacion.First(f => f.id == eva.id);

                        __eva.autoEvaluacion = eva.autoEvaluacion;
                        __eva.autoObservacion = eva.autoObservacion;
                        __eva.esAutoEvaluado = eva.esAutoEvaluado;
                        __eva.esJefeEvaluado = eva.esJefeEvaluado;
                        __eva.evaluacion = eva.evaluacion;
                        __eva.fechaRegistro = eva.fechaRegistro;
                        __eva.idEvaluacion = eva.idEvaluacion;
                        __eva.idJefe = eva.idJefe;
                        __eva.idMeta = eva.idMeta;
                        __eva.idUsuario = eva.idUsuario;
                        __eva.jefe = eva.jefe;
                        __eva.jefeEvaluacion = eva.jefeEvaluacion;
                        __eva.jefeObservacion = eva.jefeObservacion;
                        __eva.meta = eva.meta;
                        __eva.notificado = eva.notificado;
                        __eva.notificadoJefeAUsuario = eva.notificadoJefeAUsuario;
                        __eva.resultado = eva.resultado;
                        __eva.usuario = eva.usuario;

                        _context.SaveChanges();
                    }
                    //_context.tblRH_ED_DetObservacion.AddOrUpdate(eva);
                    //_context.SaveChanges();
                    #region Cargar Evidencias
                    var i = 0;
                    var lstEviEliminar = new List<tblRH_ED_DetObservacionEvidencia>();
                    foreach (var item in lstEvidencia)
                    {
                        if (!eva.lstEvidencia.Select(m => m.nombre).Contains(item.nombre))
                        {
                            lstEviEliminar.Add(item);
                        }
                    }
                    foreach (var item in lstEviEliminar)
                    {
                        item.esActivo = false;
                    }
                    _context.SaveChanges();
                    eva.lstEvidencia.ToList().ForEach(evidencia =>
                    {
                        ++i;
                        if (!lstEvidencia.Select(m => m.nombre).Contains(evidencia.nombre))
                        {
                            var archivo = lstArchivo[i - 1];
                            var nombreEvidenca = string.Format("{0} - {1}{2} - {3}", eva.meta.idProceso, i + "-" + DateTime.Now.ToString("ddMMyyyyHHmmssfff"), GlobalUtils.getExtencionArchivo(archivo.ContentType), evidencia.nombre);
                            evidencia.fechaRegistro = ahora;
                            evidencia.idObservacion = eva.id;
                            evidencia.nombre = nombreEvidenca;
                            evidencia.esActivo = true;

                            var evidenciaBD = lstEvidencia.FirstOrDefault(e => e.nombre == nombreEvidenca) ?? null;
                            if (evidenciaBD != null)
                            {
                                evidencia.id = evidenciaBD.id;
                            }
#if DEBUG
                            evidencia.ruta = dirArchivos.dirFisico;
#else
                    evidencia.ruta = dirArchivos.dirVirtual;
#endif
                            GlobalUtils.SaveArchivo(archivo, evidencia.ruta, evidencia.nombre);
                            evidencia.ruta += "\\" + evidencia.nombre;
                            _context.tblRH_ED_DetObservacionEvidencia.AddOrUpdate(x => x.id, evidencia);
                            _context.SaveChanges();
                        }
                    });
                    #endregion

                    //                    if (eva.esJefeEvaluado)
                    //                    {
                    //                        //var faltaEvaluacionJefe = _context.tblRH_ED_DetObservacion.Any(x => x.esActivo && x.idUsuario == eva.idUsuario && x.idJefe == eva.idJefe && x.idEvaluacion == eva.idEvaluacion && x.jefeEvaluacion == 0.0M);
                    //                        var faltaEvaluacionJefe = _context.tblRH_ED_DetObservacion.Where(x => x.esActivo && x.idUsuario == eva.idUsuario && x.idEvaluacion == eva.idEvaluacion && x.esJefeEvaluado).Count() == _context.tblRH_ED_DetMetas.Where(x => x.esActivo && x.idUsuario == eva.idUsuario && x.idProceso == eva.evaluacion.idProceso).Count();
                    //                        if (faltaEvaluacionJefe)
                    //                        {
                    //                            string cuerpo = string.Format(@"
                    //                                    <h3>Desempeño - {0}</h3>
                    //                                    <p>Se ha realizado tu evaluación de <strong>{1}</strong> por {2}</p>
                    //                                    <p>Favor de ingresar al sistema <a href='http://sigoplan.construplan.com.mx/'>SIGOPLAN</a>, en el apartado de Administrativo, menú Desempeño en la opción de evaluación.</p>", eva.evaluacion.proceso.proceso, eva.evaluacion.descripcion, eva.jefe.nombre + " " + eva.jefe.apellidoPaterno + " " + eva.jefe.apellidoMaterno);
                    //                            var resultadoMail = Infrastructure.Utils.GlobalUtils.sendEmail("Desempeño: Evaluación " + eva.evaluacion.descripcion, cuerpo, new List<string> { /*eva.jefe.correo*/ "martin.zayas@construplan.com.mx" });
                    //                        }
                    //                    }

                    /**** QUITAR NOTIFICACIÓN ****/
                    var alerta = _context.tblP_Alerta.Where
                        (x => 
                            (
                                (eva.esJefeEvaluado && x.userRecibeID == eva.jefe.id) || (x.userRecibeID == eva.idUsuario)
                            ) &&
                            x.sistemaID == (int)SistemasEnum.RH &&
                            x.moduloID == (int)BitacoraEnum.Metas &&
                            !x.visto &&
                            (
                                (
                                    eva.esJefeEvaluado &&
                                    x.userEnviaID == eva.usuario.id
                                ) ||
                                (
                                    x.userRecibeID == eva.idUsuario
                                )
                            ) &&
                            x.objID.ToString().Contains(eva.evaluacion.idProceso.ToString())
                        ).FirstOrDefault();

                    if (alerta != null)
                    {
                        alerta.visto = true;
                    }
                    _context.SaveChanges();
                    ///////////////////////////////

                    dbTransaction.Commit();
                    //msjResultado = eva.id > 0 ? "true" : "false"; v1
                }
                catch (Exception o_O) //TODO
                {
                    string archivoVacio = o_O.Message.Trim().Split(' ').LastOrDefault().Trim();
                    if (archivoVacio == "vacio")
                        msjResultado = o_O.Message;
                    if (archivoVacio == "grande")
                        msjResultado = o_O.Message;

                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "metaEvidenciaGuardar", o_O, AccionEnum.ACTUALIZAR, eva.id, eva);
                }
            return msjResultado;
        }

        public List<ComboDTO> CargarPuestosEmpleados(List<string> empleadosIDs) 
        {
            List<ComboDTO> data = new List<ComboDTO>();
            var lstParametros = new List<OdbcParameterDTO>();
            empleadosIDs.Remove(null);
            lstParametros.AddRange(empleadosIDs.Select(s => new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = s }));

            //            var obj = new OdbcConsultaDTO()
            //            {
            //                consulta = string.Format(@"
            //                    SELECT 
            //                        empleado.clave_empleado as Value, puesto.descripcion  as Text
            //                    FROM 
            //                        DBA.sn_empleados empleado INNER JOIN DBA.si_puestos puesto ON empleado.puesto = puesto.puesto
            //                    WHERE clave_empleado in {0}", empleadosIDs.ToParamInValue()),
            //                parametros = lstParametros
            //            };

            //            var lstCplan = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanRh, obj);
            //            data.AddRange(lstCplan);
            //            var lstArr = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenRh, obj);
            //            data.AddRange(lstArr);

            List<ComboDTO> lstCP = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT t1.clave_empleado AS Value, t2.descripcion AS Text
                                                    FROM tblRH_EK_Empleados AS t1 
                                                    INNER JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
                                                                   WHERE t1.clave_empleado IN (@clave_empleado)",
                parametros = new { clave_empleado = empleadosIDs }
            }).ToList();
            data.AddRange(lstCP);

            List<ComboDTO> lstARR = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT t1.clave_empleado AS Value, t2.descripcion AS Text
                                                    FROM tblRH_EK_Empleados AS t1 
                                                    INNER JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
                                                                   WHERE t1.clave_empleado IN (@clave_empleado)",
                parametros = new { clave_empleado = empleadosIDs }
            }).ToList();
            data.AddRange(lstARR);

            return data;

        }

        public bool eliminarEvidencia(int id)
        {
            var esEliminado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var bd = _context.tblRH_ED_DetObservacionEvidencia.FirstOrDefault(m => m.id == id);
                    esEliminado = bd != null;
                    if (esEliminado)
                    {
                        bd.esActivo = false;
                        _context.tblRH_ED_DetObservacionEvidencia.AddOrUpdate(bd);
                        _context.SaveChanges();
                        dbTransaction.Commit();
                    }
                    else
                    {
                        throw new System.InvalidOperationException("La evidencia no existe. Verifique su información");
                    }
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "eliminarEvidencia", o_O, AccionEnum.ELIMINAR, id, id);
                }
            return esEliminado;
        }

        public Respuesta NotificacionEvaluacion(tblRH_ED_DetObservacion info)
        {
            var r = new Respuesta();

            try
            {
                //var infoNotificacion = new InfoNotificacionDTO();
                //var empleado = _context.tblRH_ED_Empleado.First(x => x.empleadoID == info.idUsuario);
                //var jefe = _context.tblRH_ED_Empleado.First(x => x.empleadoID == info.idJefe);
                //infoNotificacion.DeEmpleadoAJefe = info.esAutoEvaluado;
                //if (infoNotificacion.DeEmpleadoAJefe)
                //{
                //    infoNotificacion.MensajePushUp = "" +
                //                                     "El usuario " +
                //                                     empleado.usuario.nombre + " " +
                //                                     empleado.usuario.apellidoPaterno + " " +
                //                                     empleado.usuario.apellidoMaterno +
                //                                     " ha realizado la evaluación de la meta " +
                //                                     info.meta.descripcion + " de la evaluación de " +
                //                                     info.evaluacion.descripcion + " del proceso " +
                //                                     info.meta.proceso.proceso;
                //}
                //else
                //{
                //    infoNotificacion.MensajePushUp = "" +
                //                                     "Tu Jefe " +
                //                                     " ha evaluado la meta " +
                //                                     info.meta.descripcion + " de la evaluación de " +
                //                                     info.evaluacion.descripcion + " del proceso " +
                //                                     info.meta.proceso.proceso;
                //}
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public tblRH_ED_CatEvaluacion GetInfoEvaluacion(int idEvaluacion)
        {
            var eva = _context.tblRH_ED_CatEvaluacion.FirstOrDefault(f => f.id == idEvaluacion);

            return eva;
        }

        //TODO
        public Tuple<Stream, string> DescargarEvidenciasMeta(int idObservacion)
        {
            string rutaFolderTemp = string.Empty;
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var lstEvidencias = _context.tblRH_ED_DetObservacionEvidencia.Where(x => x.idObservacion == idObservacion && x.esActivo).ToList();
                if (lstEvidencias.Count() > 0)
                {
                    var ruta = new DirArchivosDAO().getRegistro(1020);
                    var RutaTemp = string.Empty;
#if DEBUG
                    RutaTemp = ruta.dirFisico;
#else
                    RutaTemp = ruta.dirVirtual;
#endif
                    RutaTemp = Path.Combine(RutaTemp, "TEMP");

                    var nombreFolderTemp = String.Format("{0} {1}", "tmp", ObtenerFormatoCarpetaFechaActual());
                    rutaFolderTemp = Path.Combine(RutaTemp, nombreFolderTemp);
                    Directory.CreateDirectory(rutaFolderTemp);

                    //SE COPIAN LOS ARCHIVOS AL FOLDER TEMPORAL.
                    CopiarArchivosCarpetasEvidencias(rutaFolderTemp, lstEvidencias);

                    //UNA VEZ QUE ESTE LA CARPETA TEMPORAL CREADA, SE CREA EL ZIP.
                    string rutaNuevoZip = Path.Combine(RutaTemp, nombreFolderTemp + ".zip");
                    GlobalUtils.ComprimirCarpeta(rutaFolderTemp, rutaNuevoZip);

                    //UNA VEZ CREADO EL ZIP, SE ELIMINA EL FOLDER TEMPORAL Y SE OBTIENE EL STREAM DE BYTES DEL ZIP.
                    Directory.Delete(rutaFolderTemp, true);
                    var zipStream = GlobalUtils.GetFileAsStream(rutaNuevoZip);

                    //UNA VEZ CARGADO EL STREAM, SE ELIMINA EL ZIP.
                    File.Delete(rutaNuevoZip);
                    string nombreZip = String.Format("Evidencia " + nombreFolderTemp + ".zip");
                    return Tuple.Create(zipStream, nombreZip);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                try 
                { 
                    Directory.Delete(rutaFolderTemp);
                }
                catch (Exception) { }

                LogError(0, 0, nombreControlador, "DescargarEvidenciasMeta", e, AccionEnum.DESCARGAR, idObservacion, 0);
                return null;
            }
        }
        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        private static void CopiarArchivosCarpetasEvidencias(string rutaFolderTemp, List<tblRH_ED_DetObservacionEvidencia> lstEvidencias)
        {
            var rutaCarpetaEvidencia = Path.Combine(rutaFolderTemp, "Evidencia");
            Directory.CreateDirectory(rutaCarpetaEvidencia);

            for (int i = 0; i < lstEvidencias.Count(); i++)
            {
                var Archivo = String.Format("{0}", lstEvidencias[i].nombre);
                var rutaListaAsistencia = Path.Combine(rutaCarpetaEvidencia, Archivo);
                File.Copy(lstEvidencias[i].ruta, rutaListaAsistencia);
            }
        }

        private static bool verificarExisteCarpeta(string path, bool crear = false)
        {
            bool existe = false;
            try
            {
                existe = Directory.Exists(path);
                if (!existe && crear)
                {
                    Directory.CreateDirectory(path);
                    existe = true;
                }
            }
            catch (Exception e)
            {
                existe = false;
            }
            return existe;
        }
        //TODO
        #endregion
        #region Semaforo
        public List<tblRH_ED_CatSemaforo> getLstSemaforo()
        {
            return _context.tblRH_ED_CatSemaforo.ToList().Where(w => w.esActivo).ToList();
        }
        #endregion
        #region Empleados
        public tblRH_ED_Empleado getEmpleado(int idEmpleado)
        {
            return _context.tblRH_ED_Empleado.FirstOrDefault(w => w.empleadoID == idEmpleado) ?? new tblRH_ED_Empleado();
        }
        public List<tblRH_ED_Empleado> getSubordinado(int idUsuario)
        {
            var jefe = _context.tblRH_ED_Empleado.FirstOrDefault(j => j.estatus && j.empleadoID == idUsuario);
            return _context.tblRH_ED_Empleado.Where(w => jefe.estatus && w.jefeID == jefe.id).ToList();
        }
        public List<tblRH_ED_Empleado> CargarTblEmpleados(bool estatus)
        {
            var empleados = _context.tblRH_ED_Empleado.Where(x => x.estatus == estatus).ToList();
            return empleados;
        }
        public List<ReporteDesempenoDTO> CargarTblPersonalEvaluado(int proceso, int periodo)
        {
            var empleados = new List<ReporteDesempenoDTO>();
            var data = _context.tblRH_ED_DetObservacion.Where(x => x.idEvaluacion == periodo).ToList();
            var usersID = data.Select(x=>x.idUsuario).Distinct().ToList();
            var p = _context.tblRH_ED_CatProceso.FirstOrDefault(x=>x.id==proceso);
            var e = _context.tblRH_ED_CatEvaluacion.FirstOrDefault(x => x.id == periodo);
            foreach (var i in usersID)
            {
                var primero = data.FirstOrDefault(x => x.idUsuario == i);
                var u = _context.tblP_Usuario.FirstOrDefault(x=>x.id == i);
                var uj = _context.tblP_Usuario.FirstOrDefault(x => x.id == primero.idJefe);
                var a = data.Where(x=>x.idUsuario == i).ToList();
                var o = new ReporteDesempenoDTO();
                var noEvaluado = a.Any(x => x.esJefeEvaluado == false);
                var Evaluado = a.Any(x => x.esJefeEvaluado == true);
                o.empleadoID = i;
                o.empleado = u.apellidoPaterno + " " + u.apellidoMaterno + " " + u.nombre;
                o.evaluadorID = primero.idJefe;
                o.evaluador = uj.apellidoPaterno + " " + uj.apellidoMaterno + " " + uj.nombre;
                o.procesoID = p.id;
                o.proceso = p.proceso;
                o.periodoID = e.id;
                o.periodo = e.descripcion;
                o.porcentaje = Math.Round((a.Sum(x=>x.jefeEvaluacion) / a.Count),2);
                o.strEstatus = noEvaluado && Evaluado ? "Parcialmente" : noEvaluado ? "No Evaluado" : "Evaluado";
                empleados.Add(o);
            }
            return empleados.OrderBy(x=>x.empleado).ToList();
        }
        
        public List<tblP_Usuario> getEmpleados(string term)
        {
            return _context.tblP_Usuario.Where(x => (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).Contains(term) && x.estatus).ToList();
        }
        public List<tblRH_ED_Empleado> getEmpleadosDesempeno(string term)
        {
            return _context.tblRH_ED_Empleado.Where(x => (x.usuario.nombre + " " + x.usuario.apellidoPaterno + " " + x.usuario.apellidoMaterno).Contains(term) && x.estatus).ToList();
        }
        public Respuesta GuardarEmpleado(int empleadoID, int? jefeID, int tipo)
        {
            var r = new Respuesta();
            tblRH_ED_Empleado bd = new tblRH_ED_Empleado();
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    bd = _context.tblRH_ED_Empleado.FirstOrDefault(e => e.empleadoID == empleadoID);
                    if (bd == null)
                    {
                        bd = new tblRH_ED_Empleado();
                        bd.empleadoID = empleadoID;
                        bd.estatus = true;
                        bd.jefeID = jefeID;
                        bd.tipo = (ED_TipoEmpleado)tipo;
                    }
                    else
                    {
                        bd.jefeID = jefeID;
                        bd.tipo = (ED_TipoEmpleado)tipo;
                        bd.estatus = true;
                    }
                    if (1 == 2/*bd != null && bd.jefeID != null && bd.jefeID == bd.id*/)
                    {
                        r.Message = "No se puede realizar esta relación";
                    }
                    else
                    {
                        _context.tblRH_ED_Empleado.AddOrUpdate(bd);
                        _context.SaveChanges();

                        //SI SE CAMBIA EL JEFE DE UN EMPLEADO TAMBIEN SE CAMBIARA DE LOS REGISTROS DE OBESRVACIONES(EVALUACIONES) DEL EMPLEADO.
                        //SI SE QUITA EL JEFE DE UN EMPLEADO Y EL EMPLEADO TENIA EVALUACIONES CON ESE JEFE SE MANTENDRA ESE JEFE EN LAS EVALUACIONES.

                        if (jefeID != null)
                        {
                            var metas = _context.tblRH_ED_DetMetas.Where(w => w.idUsuario == empleadoID).ToList();

                            foreach (var meta in metas)
                            {
                                meta.idJefe = bd.jefe.empleadoID;
                            }

                            SaveChanges();

                            var observaciones = _context.tblRH_ED_DetObservacion.Where(w => w.idUsuario == empleadoID).ToList();

                            foreach (var observacion in observaciones)
                            {
                                observacion.idJefe = bd.jefe.empleadoID;
                            }

                            SaveChanges();
                        }

                        dbTransaction.Commit();
                        r.Success = true;
                        r.Message = "Ok";
                    }
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarMeta", o_O, AccionEnum.ACTUALIZAR, bd.id, bd);
                    r.Message += o_O.Message;
                }
            return r;
        }
        public Respuesta VerComo(int idUsuario)
        {
            var r = new Respuesta();

            try
            {
                var admin = _context.tblRH_ED_Empleado.FirstOrDefault(x => x.empleadoID == vSesiones.sesionUsuarioDTO.id && x.estatus);
                var usuario = _context.tblRH_ED_Empleado.FirstOrDefault(x => x.id == idUsuario && x.estatus);

                if (admin != null && admin.tipo == ED_TipoEmpleado.ADMIN)
                {
                    if (usuario != null)
                    {
                        var usuarioVirtual = new UsuarioDTO();
                        usuarioVirtual.EmpleadoId = usuario.empleadoID;
                        usuarioVirtual.Id = usuario.id;
                        usuarioVirtual.JefeId = usuario.jefeID;
                        usuarioVirtual.Nombre = usuario.usuario.nombre + " " + usuario.usuario.apellidoPaterno + " " + usuario.usuario.apellidoMaterno;
                        usuarioVirtual.TipoUsuario = (int)usuario.tipo;
                        usuarioVirtual.esAdmin = true;
                        usuarioVirtual.VerComoActivado = true;

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = usuarioVirtual;
                    }
                    else
                    {
                        r.Message = "El usuario a visualizar no existe";
                    }
                }
                else
                {
                    r.Message = "No eres administrador, no puedes realizar esta acción";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public Respuesta ModificarProcesoEmpleado(List<int> idProceso, int idEmpleado)
        {
            var r = new Respuesta();

            try
            {
                var proceso = _context.tblRH_ED_RelacionesEmpleadoProceso.Where(x => x.Empleado.id == idEmpleado).ToList();
                proceso = proceso == null ? new List<tblRH_ED_RelacionEmpleadoProceso>() : proceso;
                foreach (var item in idProceso)
                {
                    if (proceso.Select(m => m.ProcesoId).Contains(item))
                    {
                        proceso.First(x => x.ProcesoId == item).Estatus = proceso.First(x => x.ProcesoId == item).Estatus ? false : true;
                    }
                    else
                    {
                        var nuevaRelacion = new tblRH_ED_RelacionEmpleadoProceso();
                        nuevaRelacion.ProcesoId = item;
                        nuevaRelacion.EmpleadoId = idEmpleado;
                        nuevaRelacion.Estatus = true;
                        _context.tblRH_ED_RelacionesEmpleadoProceso.Add(nuevaRelacion);
                    }
                }
                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        public Respuesta EliminarEmpleado(int idEmpleado)
        {
            var r = new Respuesta();

            try
            {
                var empleado = _context.tblRH_ED_Empleado.First(x => x.id == idEmpleado);
                empleado.estatus = false;
                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        #endregion
        #region Combobox
        public List<ComboDTO> getCboProceso(int idEmpleado)
        {
            var usuario = _context.tblRH_ED_Empleado.FirstOrDefault(x => x.empleadoID == idEmpleado && x.estatus);
            var lst = new List<tblRH_ED_CatProceso>();
            var relEmplProceso = _context.tblRH_ED_RelacionesEmpleadoProceso.ToList();
            if (usuario != null)
            {
                var procesos = _context.tblRH_ED_RelacionesEmpleadoProceso.Where(x => x.EmpleadoId == usuario.id && x.Estatus && x.Proceso.esActivo).ToList();
                if (procesos != null)
                {
                    foreach (var item in procesos)
                    {
                        lst.Add(item.Proceso);
                    }
                }
            }
            var cbo = lst.Select(s => new ComboDTO()
            {
                Text = s.proceso,
                Value = s.id.ToString(),
                Prefijo = relEmplProceso.Where(w => w.ProcesoId == s.id).Select(r => r.EmpleadoId.ToString()).ToList().ToLine(",").Replace("'", string.Empty)
            }).OrderByDescending(o => o.Value).ToList();
            return cbo;
        }

        public List<ComboDTO> getCboEvaluacionPorProceso(int idProceso)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            //int idEvaluacionFocus = 0;
            //DateTime fechaActual = DateTime.Now;
            //var getEvaluacionProxima = _context.tblRH_ED_CatEvaluacion.Where(x => x.esActivo && x.idProceso == idProceso && x.fechaFin >= fechaActual).OrderByDescending(x => x.fechaFin).FirstOrDefault();
            //if (getEvaluacionProxima != null)
            //    idEvaluacionFocus = getEvaluacionProxima.id;

            var lstEvaluaciones = _context.tblRH_ED_CatEvaluacion.Where(w => w.esActivo && w.idProceso == idProceso).OrderBy(x => x.fechaInicio).ToList();
            //var eva = DiasSeguimiento(idProceso).Value as FechaSeguimientoDTO;
            tblRH_ED_CatEvaluacion eva = null;
            var esteMomento = DateTime.Now;
            var fechaHoy = new DateTime(esteMomento.Year, esteMomento.Month, esteMomento.Day);
            foreach (var evaluacion in lstEvaluaciones)
            {
                if (
                    evaluacion.fechaInicio <= fechaHoy &&
                    evaluacion.fechaFin >= fechaHoy)
                {
                    eva = evaluacion;
                    break;
                }
            }
            if (eva == null)
            {
                eva = lstEvaluaciones.FirstOrDefault(f => f.fechaInicio >= fechaHoy);
            }
            if (eva == null)
            {
                eva = lstEvaluaciones.LastOrDefault();
            }

            var dataCboEvaluaciones = lstEvaluaciones.Select(s => new ComboDTO()
            {
                Text = s.descripcion,
                Value = s.id.ToString(),
                Prefijo = eva != null && s.id == eva.id ? "seleccionadoPorFecha" : ""
            }).ToList();
            //result.Add("idEvaluacionFocus", idEvaluacionFocus);
            //result.Add(SUCCESS, true);
            return dataCboEvaluaciones;
        }
        public Dictionary<string, object> getEvaluacionVigenteID(int idProceso)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                int idEvaluacionFocus = 0;
                DateTime fechaActual = DateTime.Now;
                var getEvaluacionProxima = _context.tblRH_ED_CatEvaluacion.Where(x => x.esActivo && x.idProceso == idProceso && x.fechaFin >= fechaActual).OrderByDescending(x => x.fechaFin).FirstOrDefault();
                if (getEvaluacionProxima != null)
                    idEvaluacionFocus = getEvaluacionProxima.id;

                result.Add("idEvaluacionFocus", idEvaluacionFocus);
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, ex.Message);
            }
            return result;
        }
        public List<ComboDTO> getCboMetaPorProceso(int idProceso, int idEmpleado)
        {
            var lst = _context.tblRH_ED_DetMetas.Where(w => w.esActivo && w.idProceso == idProceso && w.idUsuario == idEmpleado).ToList();
            var cbo = lst.Select(s => new ComboDTO()
            {
                Text = s.nombre,
                Value = s.id.ToString()
            }).ToList();
            return cbo;
        }
        public List<ComboDTO> getCboEstrategias()
        {
            var lst = _context.tblRH_ED_CatEstrategia.ToList();
            var cbo = lst.Select(s => new ComboDTO()
            {
                Text = s.descripcion,
                Value = s.id.ToString()
            }).ToList();
            return cbo;
        }

        public List<ComboDTO> getCboTodosLosProcesos()
        {
            return _context.tblRH_ED_CatProceso.Where(x => x.esActivo).Select(m => new ComboDTO
            {
                Text = m.proceso,
                Value = m.id.ToString()
            }).ToList();
        }
        #endregion
        #region Calendario
        public Respuesta GetEvaluaciones(int? idUsuarioVerComo)
        {
            var r = new Respuesta();

            var fechaActual = DateTime.Now;

            try
            {
                var eventos = new List<EventoCalendarioDTO>();

                var procesos = _context.tblRH_ED_RelacionesEmpleadoProceso.Where(x => x.Empleado.empleadoID == idUsuarioVerComo.Value && x.Proceso.esActivo && x.Estatus).ToList();

                foreach (var proceso in procesos)
                {
                    var fechaInicioResultado = proceso.Proceso.lstEvaluacion.Where(x => x.esActivo).OrderBy(o => o.fechaInicio).FirstOrDefault();
                    var fechaFinResultado = proceso.Proceso.lstEvaluacion.Where(x => x.esActivo).OrderByDescending(o => o.fechaFin).FirstOrDefault();

                    if (fechaInicioResultado == null || fechaFinResultado == null)
                    {
                        continue;
                    }

                    var fechaInicio = fechaInicioResultado.fechaInicio;
                    var fechaFin = fechaFinResultado.fechaFin;
                    //var fechaInicio = proceso.Proceso.lstEvaluacion.Where(x => x.esActivo).OrderBy(o => o.fechaInicio).FirstOrDefault().fechaInicio;
                    //var fechaFin = proceso.Proceso.lstEvaluacion.Where(x => x.esActivo).OrderByDescending(o => o.fechaFin).FirstOrDefault().fechaFin;

                    var eventoProceso = new EventoCalendarioDTO();
                    eventoProceso.title = "Inicio: " + proceso.Proceso.proceso;
                    eventoProceso.start = fechaInicio.ToString("yyyy-MM-dd");
                    eventoProceso.classNames = "procesoStart";
                    eventoProceso.backgroundColor = colorProceso;
                    eventoProceso.inicio = true;

                    eventos.Add(eventoProceso);

                    eventoProceso = new EventoCalendarioDTO();
                    eventoProceso.title = "Fin: " + proceso.Proceso.proceso;
                    eventoProceso.start = fechaFin.ToString("yyyy-MM-dd");
                    eventoProceso.classNames = "procesoEnd";
                    eventoProceso.backgroundColor = (fechaFin.Date - fechaActual.Date).Days > 5 ? colorProceso : (fechaFin.Date - fechaActual.Date).Days > 1 ? colorWarning : colorDanger;

                    eventos.Add(eventoProceso);

                    foreach (var evaluacion in proceso.Proceso.lstEvaluacion.Where(x => x.esActivo))
                    {
                        var eventoEvaluacion = new EventoCalendarioDTO();

                        var evaFechaInicio = evaluacion.fechaInicio;
                        var evaFechaFin = evaluacion.fechaFin;

                        eventoEvaluacion.title = "Inicio: evaluación " + evaluacion.descripcion;
                        eventoEvaluacion.start = evaFechaInicio.ToString("yyyy-MM-dd");
                        eventoEvaluacion.classNames = "evaluacionStart";
                        eventoEvaluacion.backgroundColor = colorEvaluacion;
                        eventoEvaluacion.inicio = true;

                        eventos.Add(eventoEvaluacion);

                        eventoEvaluacion = new EventoCalendarioDTO();
                        eventoEvaluacion.title = "Fin: evaluación " + evaluacion.descripcion;
                        eventoEvaluacion.start = evaFechaFin.ToString("yyyy-MM-dd");
                        eventoEvaluacion.classNames = "evaluacionEnd";
                        eventoEvaluacion.backgroundColor = (evaFechaFin.Date - fechaActual.Date).Days > 5 ? colorEvaluacion : (evaFechaFin.Date - fechaActual.Date).Days > 1 ? colorWarning : colorDanger;

                        eventos.Add(eventoEvaluacion);
                    }
                }

                r.Success = true;
                r.Message = "Ok";
                r.Value = eventos;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message + ". " + ex.TargetSite;
            }

            return r;
        }
        #endregion
        #region Procesos
        public Respuesta CRUDProceso(CRUDProcesoDTO objProceso)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var proceso = new tblRH_ED_CatProceso();
                    if (objProceso.IdProceso != null)
                    {
                        proceso = _context.tblRH_ED_CatProceso.FirstOrDefault(x => x.id == objProceso.IdProceso.Value && x.esActivo);
                        if (proceso == null)
                        {
                            r.Message = "El proceso no existe";
                            return r;
                        }
                        else
                        {
                            if (objProceso.EsEliminacion)
                            {
                                if (proceso.lstEvaluacion.Where(x => x.esActivo && x.observaciones.Where(m => m.esActivo).Count() > 0).Count() > 0)
                                {
                                    r.Message = "El proceso cuenta con evaluaciones, no se puede eliminar";
                                    return r;
                                }
                                else
                                {
                                    proceso.esActivo = false;
                                    foreach (var item in proceso.relacionEP)
                                    {
                                        item.Estatus = false;
                                    }
                                    foreach (var evaluacion in proceso.lstEvaluacion)
                                    {
                                        evaluacion.esActivo = false;
                                        foreach (var observacion in evaluacion.observaciones)
                                        {
                                            observacion.esActivo = false;
                                        }
                                    }
                                    foreach (var meta in proceso.Metas)
                                    {
                                        meta.esActivo = false;
                                    }
                                }
                            }
                            else
                            {
                                if (proceso.lstEvaluacion.Where(x => x.fechaFin > objProceso.FechaFin).Count() > 0)
                                {
                                    r.Message = "El proceso cuenta con evaluaciones con fecha de finalización posterior a la ingresada en el proceso actual";
                                    return r;
                                }

                                proceso.esActivo = true;
                                proceso.proceso = objProceso.Proceso;
                                proceso.fechaInicio = objProceso.FechaInicio;
                                proceso.fechaFin = objProceso.FechaFin;
                                proceso.fechaRegistro = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        proceso.proceso = objProceso.Proceso;
                        proceso.fechaInicio = objProceso.FechaInicio;
                        proceso.fechaFin = objProceso.FechaFin;
                        proceso.esActivo = true;
                        proceso.fechaRegistro = DateTime.Now;
                    }

                    _context.tblRH_ED_CatProceso.AddOrUpdate(proceso);
                    _context.SaveChanges();

                    transaction.Commit();

                    r.Success = true;
                    r.Message = "Ok";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message + ex.TargetSite;
                }
            }

            return r;
        }

        public Respuesta ObtenerTodosLosProcesos()
        {
            var r = new Respuesta();

            try
            {
                var procesos = _context.tblRH_ED_CatProceso.Where(x => x.esActivo).Select(m => new CRUDProcesoDTO
                {
                    IdProceso = m.id,
                    Proceso = m.proceso,
                    FechaInicio = m.fechaInicio,
                    FechaFin = m.fechaFin
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = procesos;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public tblRH_ED_CatProceso GetProceso(int idProceso)
        {
            return _context.tblRH_ED_CatProceso.FirstOrDefault(f => f.id == idProceso && f.esActivo);
        }
        #endregion
        #region Empleados Metas
        public List<tblRH_ED_Empleado> getLstEmpleadoJefe(int idJefe)
        {
            var jefe = _context.tblRH_ED_Empleado.FirstOrDefault(w => w.empleadoID == idJefe);
            var lstEmpl = _context.tblRH_ED_Empleado.Where(w => w.jefeID == jefe.id && w.estatus).ToList();
            lstEmpl.Insert(0, jefe);
            return lstEmpl;
        }
        #endregion
        #region Evaluaciones
        public Respuesta GetEvaluaciones()
        {
            var r = new Respuesta();

            try
            {
                var evaluaciones = _context.tblRH_ED_CatEvaluacion.Select(m => new EvaluacionDTO
                {
                    Id = m.id,
                    ProcesoId = m.idProceso,
                    Descripcion = m.descripcion,
                    FechaInicio = m.fechaInicio,
                    FechaFin = m.fechaFin,
                    EsEliminacion = false,
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = evaluaciones;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta GetEvaluacionesPorProceso(int idProceso)
        {
            var r = new Respuesta();

            try
            {
                var evaluaciones = _context.tblRH_ED_CatEvaluacion.Where(x => x.idProceso == idProceso && x.esActivo).Select(m => new EvaluacionDTO()
                {
                    Id = m.id,
                    ProcesoId = m.idProceso,
                    Descripcion = m.descripcion,
                    FechaInicio = m.fechaInicio,
                    FechaFin = m.fechaFin,
                    EsEliminacion = false
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = evaluaciones;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta CUDEvaluacion(EvaluacionDTO objEvaluacion)
        {
            var r = new Respuesta();

            try
            {
                var proceso = _context.tblRH_ED_CatProceso.First(x => x.id == objEvaluacion.ProcesoId);
                //if (!objEvaluacion.EsEliminacion && (objEvaluacion.FechaFin > proceso.fechaFin || objEvaluacion.FechaInicio > proceso.fechaFin))
                //{
                //    r.Message = "El período de evaluación no puede ser mayor a la fecha de finalización del proceso";
                //    return r;
                //}
                if (!objEvaluacion.EsEliminacion && (objEvaluacion.FechaInicio < proceso.fechaInicio || objEvaluacion.FechaFin < proceso.fechaInicio))
                {
                    r.Message = "El período de evaluación no puede ser menor a la fecha de inicio del proceso";
                    return r;
                }

                var evaluacion = new tblRH_ED_CatEvaluacion();
                if (objEvaluacion.Id != null)
                {
                    evaluacion = _context.tblRH_ED_CatEvaluacion.FirstOrDefault(x => x.id == objEvaluacion.Id.Value && x.esActivo);
                    if (evaluacion == null)
                    {
                        r.Message = "La evaluación no existe";
                        return r;
                    }
                    else
                    {
                        if (objEvaluacion.EsEliminacion)
                        {
                            if (evaluacion.observaciones.Where(x => x.esActivo).Count() > 0)
                            {
                                r.Message = "La evaluación cuenta con observaciones, no se puede eliminar";
                                return r;
                            }
                            else
                            {
                                evaluacion.esActivo = false;
                            }
                        }
                        else
                        {
                            evaluacion.descripcion = objEvaluacion.Descripcion;
                            evaluacion.fechaInicio = objEvaluacion.FechaInicio;
                            evaluacion.fechaFin = objEvaluacion.FechaFin;
                            evaluacion.esActivo = true;
                            evaluacion.fechaRegistro = DateTime.Now;
                        }
                    }
                }
                else
                {
                    evaluacion.descripcion = objEvaluacion.Descripcion;
                    evaluacion.fechaInicio = objEvaluacion.FechaInicio;
                    evaluacion.fechaFin = objEvaluacion.FechaFin;
                    evaluacion.esActivo = true;
                    evaluacion.fechaRegistro = DateTime.Now;
                    evaluacion.idProceso = objEvaluacion.ProcesoId;
                }

                _context.tblRH_ED_CatEvaluacion.AddOrUpdate(evaluacion);
                _context.SaveChanges();

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        #endregion
    }
}

using Core.DAO.Administracion.Seguridad.SeguimientoCompromisos;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.SeguimientoCompromisos;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SeguimientoCompromisos;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.SeguimientoCompromisos;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Administracion.Seguridad.SeguimientoCompromisos
{
    public class SeguimientoCompromisosDAO : GenericDAO<tblP_Usuario>, ISeguimientoCompromisosDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "SeguimientoCompromisosController";
        private const string NombreBaseEvidencia = @"Evidencia";
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\SEGUIMIENTO_COMPROMISOS";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\SEGUIMIENTO_COMPROMISOS";
        private readonly string RutaEvidencia;

        public SeguimientoCompromisosDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaEvidencia = Path.Combine(RutaBase, "EVIDENCIA");
        }

        public Dictionary<string, object> GetAgrupacionCombo()
        {
            try
            {
                var listaAgrupacion = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).OrderBy(x => x.nomAgrupacion).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.nomAgrupacion
                }).ToList();

                resultado.Add(ITEMS, listaAgrupacion);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "GetAgrupacionCombo", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> GetActividades()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var listaAreas = _context.tblSC_Area.Where(x => x.registroActivo).ToList();

                var actividades = _context.tblSC_Actividad.Where(x => x.registroActivo).ToList().Select(x => new
                {
                    id = x.id,
                    nombre = x.nombre,
                    descripcion = x.descripcion,
                    area = x.area,
                    areaDesc = listaAreas.Where(y => y.id == x.area).Select(z => z.descripcion).FirstOrDefault(),
                    areaEvaluadora = x.areaEvaluadora,
                    areaEvaluadoraDesc = listaAreas.Where(y => y.id == x.areaEvaluadora).Select(z => z.descripcion).FirstOrDefault(),
                    clasificacion = x.clasificacion,
                    clasificacionDesc = x.clasificacion.GetDescription(),
                    porcentaje = x.porcentaje,
                    diasCompromiso = x.diasCompromiso
                }).ToList();
                var combo = actividades.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = string.Format(@"{0}", x.nombre)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", actividades);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "GetActividades", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevaActividad(tblSC_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    actividad.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    actividad.fechaCreacion = DateTime.Now;
                    actividad.usuarioModificacion_id = 0;
                    actividad.fechaModificacion = null;
                    actividad.registroActivo = true;

                    _context.tblSC_Actividad.Add(actividad);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, actividad.id, JsonUtils.convertNetObjectToJson(actividad));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "GuardarNuevaActividad", e, AccionEnum.AGREGAR, 0, actividad);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarActividad(tblSC_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var actividadSIGOPLAN = _context.tblSC_Actividad.FirstOrDefault(x => x.id == actividad.id);

                    actividadSIGOPLAN.nombre = actividad.nombre;
                    actividadSIGOPLAN.descripcion = actividad.descripcion;
                    actividadSIGOPLAN.area = actividad.area;
                    actividadSIGOPLAN.areaEvaluadora = actividad.areaEvaluadora;
                    actividadSIGOPLAN.clasificacion = actividad.clasificacion;
                    actividadSIGOPLAN.porcentaje = actividad.porcentaje;
                    actividadSIGOPLAN.diasCompromiso = actividad.diasCompromiso;
                    actividadSIGOPLAN.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    actividadSIGOPLAN.fechaModificacion = DateTime.Now;

                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ACTUALIZAR, actividad.id, JsonUtils.convertNetObjectToJson(actividad));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "EditarActividad", e, AccionEnum.ACTUALIZAR, 0, actividad);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarActividad(tblSC_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var actividadSIGOPLAN = _context.tblSC_Actividad.FirstOrDefault(x => x.id == actividad.id);

                    actividadSIGOPLAN.registroActivo = false;
                    actividadSIGOPLAN.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    actividadSIGOPLAN.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ELIMINAR, actividad.id, JsonUtils.convertNetObjectToJson(actividad));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "EliminarActividad", e, AccionEnum.ELIMINAR, 0, actividad);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetRelacionesEmpleadoAreaAgrupacion()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var listaAreas = _context.tblSC_Area.Where(x => x.registroActivo).ToList();
                var listaAgrupaciones = _context.tblS_IncidentesAgrupacionCC.ToList();
                var relaciones = _context.tblSC_RelacionEmpleadoAreaAgrupacion.Where(x => x.registroActivo).ToList().Select(x => new
                {
                    id = x.id,
                    usuario_id = x.usuario_id,
                    nombreEmpleado = listaUsuarios.Where(y => y.id == x.usuario_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    claveEmpleado = listaUsuarios.Where(y => y.id == x.usuario_id).Select(z => z.cveEmpleado).FirstOrDefault(),
                    area = x.area,
                    areaDesc = listaAreas.Where(y => y.id == x.area).Select(z => z.descripcion).FirstOrDefault(),
                    agrupacion_id = x.agrupacion_id,
                    agrupacionDesc = listaAgrupaciones.Where(y => y.id == x.agrupacion_id).Select(z => z.nomAgrupacion).FirstOrDefault(),
                    tipoUsuario = x.tipoUsuario,
                    tipoUsuarioDesc = x.tipoUsuario.GetDescription()
                }).ToList();

                resultado.Add("data", relaciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "GetRelacionesEmpleadoAreaAgrupacion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevaRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.id == relacion.usuario_id);

                    if (usuarioSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra el usuario de SIGOPLAN.");
                    }

                    relacion.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    relacion.fechaCreacion = DateTime.Now;
                    relacion.usuarioModificacion_id = 0;
                    relacion.fechaModificacion = null;
                    relacion.registroActivo = true;

                    _context.tblSC_RelacionEmpleadoAreaAgrupacion.Add(relacion);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, relacion.id, JsonUtils.convertNetObjectToJson(relacion));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "GuardarNuevaRelacion", e, AccionEnum.AGREGAR, 0, relacion);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var relacionSIGOPLAN = _context.tblSC_RelacionEmpleadoAreaAgrupacion.FirstOrDefault(x => x.id == relacion.id);

                    relacionSIGOPLAN.usuario_id = relacion.usuario_id;
                    relacionSIGOPLAN.area = relacion.area;
                    relacionSIGOPLAN.agrupacion_id = relacion.agrupacion_id;
                    relacionSIGOPLAN.tipoUsuario = relacion.tipoUsuario;
                    relacionSIGOPLAN.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    relacionSIGOPLAN.fechaModificacion = DateTime.Now;

                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ACTUALIZAR, relacion.id, JsonUtils.convertNetObjectToJson(relacion));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "EditarRelacion", e, AccionEnum.ACTUALIZAR, 0, relacion);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var relacionSIGOPLAN = _context.tblSC_RelacionEmpleadoAreaAgrupacion.FirstOrDefault(x => x.id == relacion.id);

                    relacionSIGOPLAN.registroActivo = false;
                    relacionSIGOPLAN.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    relacionSIGOPLAN.fechaModificacion = DateTime.Now;

                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ELIMINAR, relacion.id, JsonUtils.convertNetObjectToJson(relacion));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "EliminarRelacion", e, AccionEnum.ELIMINAR, 0, relacion);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetUsuarioPorClave(int claveEmpleado)
        {
            try
            {
                var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == claveEmpleado.ToString());

                if (usuario != null)
                {
                    var data = new
                    {
                        usuario_id = usuario.id,
                        claveEmpleado = claveEmpleado,
                        nombre = usuario.nombre,
                        apellidoPaterno = usuario.apellidoPaterno ?? "",
                        apellidoMaterno = usuario.apellidoMaterno ?? ""
                    };

                    resultado.Add("data", data);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    throw new Exception("No se encuentra el usuario de SIGOPLAN con la clave \"" + claveEmpleado + "\".");
                }
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "GetUsuarioPorClave", e, AccionEnum.CONSULTA, 0, claveEmpleado);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetAreaCombo()
        {
            try
            {
                resultado.Add(ITEMS, _context.tblSC_Area.Where(x => x.registroActivo).Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.descripcion }).OrderBy(x => x.Text).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "GetAreaCombo", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> GetClasificacionCombo(List<int> areas)
        {
            try
            {
                if (areas == null)
                {
                    resultado.Add(ITEMS, GlobalUtils.ParseEnumToCombo<ClasificacionActividadSCEnum>());
                }
                else
                {
                    var actividadesArea = _context.tblSC_Actividad.Where(x => x.registroActivo && areas.Contains(x.area)).GroupBy(x => x.clasificacion).ToList().Select(x => new ComboDTO
                    {
                        Value = ((int)x.Key).ToString(),
                        Text = x.Key.GetDescription()
                    }).ToList();

                    resultado.Add(ITEMS, actividadesArea);
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "GetClasificacionCombo", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> GetActividadesAplicables(List<int> areas, List<ClasificacionActividadSCEnum> clasificaciones)
        {
            try
            {
                var listaAreas = _context.tblSC_Area.Where(x => x.registroActivo).ToList();
                var actividades = _context.tblSC_Actividad.Where(x => x.registroActivo).ToList().Where(x =>
                    (areas != null ? areas.Contains(x.area) : true) &&
                    (clasificaciones != null ? clasificaciones.Contains(x.clasificacion) : true)
                ).Select(x => new
                {
                    id = x.id,
                    nombre = x.nombre,
                    descripcion = x.descripcion,
                    area = x.area,
                    areaDesc = listaAreas.Where(y => y.id == x.area).Select(z => z.descripcion).FirstOrDefault(),
                    areaEvaluadora = x.areaEvaluadora,
                    areaEvaluadoraDesc = listaAreas.Where(y => y.id == x.areaEvaluadora).Select(z => z.descripcion).FirstOrDefault(),
                    clasificacion = x.clasificacion,
                    clasificacionDesc = x.clasificacion.GetDescription(),
                    porcentaje = x.porcentaje,
                    diasCompromiso = x.diasCompromiso
                }).ToList();

                resultado.Add("data", actividades);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "GetActividadesAplicables", e, AccionEnum.CONSULTA, 0, new { areas = areas, clasificaciones = clasificaciones });
            }

            return resultado;
        }

        public Dictionary<string, object> GetAsignacionActividades(int agrupacion_id)
        {
            try
            {
                var listaAreas = _context.tblSC_Area.Where(x => x.registroActivo).ToList();
                var asignaciones = (
                    from asig in _context.tblSC_Asignacion.Where(x => x.registroActivo && x.agrupacion_id == agrupacion_id).ToList()
                    join act in _context.tblSC_Actividad.Where(x => x.registroActivo).ToList() on asig.actividad_id equals act.id
                    select new
                    {
                        id = asig.id,
                        actividad_id = asig.actividad_id,
                        nombre = act.nombre,
                        descripcion = act.descripcion,
                        area = act.area,
                        areaDesc = listaAreas.Where(y => y.id == act.area).Select(z => z.descripcion).FirstOrDefault(),
                        areaEvaluadora = act.areaEvaluadora,
                        areaEvaluadoraDesc = listaAreas.Where(y => y.id == act.areaEvaluadora).Select(z => z.descripcion).FirstOrDefault(),
                        clasificacion = act.clasificacion,
                        clasificacionDesc = act.clasificacion.GetDescription(),
                        porcentaje = act.porcentaje,
                        diasCompromiso = act.diasCompromiso,
                        fechaInicioEvaluacion = asig.fechaInicio,
                        fechaInicioEvaluacionString = asig.fechaInicio.ToShortDateString()
                    }
                ).ToList();

                resultado.Add("data", asignaciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "GetAsignacionActividades", e, AccionEnum.CONSULTA, 0, agrupacion_id);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var agrupacion in agrupaciones)
                    {
                        foreach (var actividad in actividades)
                        {
                            _context.tblSC_Asignacion.Add(new tblSC_Asignacion
                            {
                                agrupacion_id = agrupacion,
                                area = 0,
                                actividad_id = actividad,
                                fechaInicio = fechaInicioEvaluacion,
                                usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                usuarioModificacion_id = 0,
                                fechaModificacion = null,
                                registroActivo = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(actividades));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "GuardarAsignacionActividades", e, AccionEnum.AGREGAR, 0, new { fechaInicioEvaluacion = fechaInicioEvaluacion, actividades = actividades });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarAsignacionActividad(int asignacion_id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var asignacion = _context.tblSC_Asignacion.FirstOrDefault(x => x.id == asignacion_id);

                    asignacion.registroActivo = false;
                    asignacion.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    asignacion.fechaModificacion = DateTime.Now;

                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ELIMINAR, asignacion_id, JsonUtils.convertNetObjectToJson(asignacion_id));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "EliminarAsignacionActividad", e, AccionEnum.AGREGAR, 0, asignacion_id);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetAsignacionActividadesCaptura(int agrupacion_id)
        {
            try
            {
                var agrupaciones = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
                var listaAreas = _context.tblSC_Area.Where(x => x.registroActivo).ToList();
                var relacionEmpleadoAreaAgrupacion = _context.tblSC_RelacionEmpleadoAreaAgrupacion.Where(x => x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id && x.tipoUsuario == TipoUsuarioSCEnum.capturista).ToList();
                var asignaciones = (
                    from asig in _context.tblSC_Asignacion.Where(x => x.registroActivo && (agrupacion_id > 0 ? x.agrupacion_id == agrupacion_id : true)).ToList()
                    join act in _context.tblSC_Actividad.Where(x => x.registroActivo).ToList() on asig.actividad_id equals act.id
                    join rel in relacionEmpleadoAreaAgrupacion on new { act.area, asig.agrupacion_id } equals new { rel.area, rel.agrupacion_id }
                    select new
                    {
                        asignacion_id = asig.id,
                        agrupacion_id = asig.agrupacion_id,
                        agrupacionDesc = agrupaciones.Where(x => x.id == asig.agrupacion_id).Select(x => x.nomAgrupacion).FirstOrDefault(),
                        actividad_id = asig.actividad_id,
                        nombre = act.nombre,
                        descripcion = act.descripcion,
                        area = act.area,
                        areaDesc = listaAreas.Where(y => y.id == act.area).Select(z => z.descripcion).FirstOrDefault(),
                        areaEvaluadora = act.areaEvaluadora,
                        areaEvaluadoraDesc = listaAreas.Where(y => y.id == act.areaEvaluadora).Select(z => z.descripcion).FirstOrDefault(),
                        clasificacion = act.clasificacion,
                        clasificacionDesc = act.clasificacion.GetDescription(),
                        porcentaje = act.porcentaje,
                        diasCompromiso = act.diasCompromiso,
                        fechaInicioEvaluacion = asig.fechaInicio,
                        fechaInicioEvaluacionString = asig.fechaInicio.ToShortDateString()
                    }
                ).ToList();

                var evidenciasCapturadas = _context.tblSC_Evidencia.Where(x => x.registroActivo && (agrupacion_id > 0 ? x.agrupacion_id == agrupacion_id : true)).ToList();
                var data = new List<object>();

                foreach (var asignacion in asignaciones)
                {
                    var evidencias = evidenciasCapturadas.Where(x => x.agrupacion_id == asignacion.agrupacion_id && x.actividad_id == asignacion.actividad_id).ToList(); ;

                    data.Add(new
                    {
                        asignacion_id = asignacion.asignacion_id,
                        agrupacion_id = asignacion.agrupacion_id,
                        agrupacionDesc = asignacion.agrupacionDesc,
                        actividad_id = asignacion.actividad_id,
                        nombre = asignacion.nombre,
                        descripcion = asignacion.descripcion,
                        area = asignacion.area,
                        areaDesc = asignacion.areaDesc,
                        areaEvaluadora = asignacion.areaEvaluadora,
                        areaEvaluadoraDesc = asignacion.areaEvaluadoraDesc,
                        clasificacion = asignacion.clasificacion,
                        clasificacionDesc = asignacion.clasificacionDesc,
                        porcentaje = asignacion.porcentaje,
                        diasCompromiso = asignacion.diasCompromiso,
                        fechaCompromiso = asignacion.fechaInicioEvaluacion.AddDays(asignacion.diasCompromiso),
                        fechaCompromisoString = asignacion.fechaInicioEvaluacion.AddDays(asignacion.diasCompromiso).ToShortDateString(),
                        fechaInicioEvaluacion = asignacion.fechaInicioEvaluacion,
                        fechaInicioEvaluacionString = asignacion.fechaInicioEvaluacionString,
                        evidencias = evidencias,
                        evaluacionCompletada = evidencias.Where(x => x.terminacion).FirstOrDefault() != null,
                        progresoReal = evidencias.Count() > 0 ? evidencias.Last().progreso : 0m
                    });
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetAsignacionActividades", e, AccionEnum.CONSULTA, 0, agrupacion_id);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarEvidencia(tblSC_Evidencia captura, List<HttpPostedFileBase> evidencias)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (captura.agrupacion_id == 0)
                    {
                        throw new Exception("No se capturó la información de la agrupación.");
                    }

                    var evidenciasCapturadas = _context.tblSC_Evidencia.Where(x => x.registroActivo && x.agrupacion_id == captura.agrupacion_id && x.actividad_id == captura.actividad_id).ToList();

                    if (evidenciasCapturadas.Count() == 5)
                    {
                        throw new Exception("No se puede capturar más de cinco evidencias.");
                    }

                    var usuario = vSesiones.sesionUsuarioDTO;
                    var fechaActual = DateTime.Now.Date;

                    string rutaArchivoEvidencia = "";

                    if (evidencias != null && evidencias.Count() > 0)
                    {
                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidencia, evidencias[0].FileName);
                        rutaArchivoEvidencia = Path.Combine(RutaEvidencia, nombreArchivoEvidencia);

                        if (File.Exists(rutaArchivoEvidencia))
                        {
                            int count = 1;

                            string fileNameOnly = Path.GetFileNameWithoutExtension(rutaArchivoEvidencia);
                            string extension = Path.GetExtension(rutaArchivoEvidencia);
                            string path = Path.GetDirectoryName(rutaArchivoEvidencia);
                            string newFullPath = rutaArchivoEvidencia;

                            while (File.Exists(newFullPath))
                            {
                                string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                                newFullPath = Path.Combine(path, tempFileName + extension);
                            }

                            rutaArchivoEvidencia = newFullPath;
                        }

                        if (GlobalUtils.SaveHTTPPostedFile(evidencias[0], rutaArchivoEvidencia) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    captura.area = 0;
                    captura.consecutivo = evidenciasCapturadas.Count() > 0 ? evidenciasCapturadas.Max(x => x.consecutivo) + 1 : 1;
                    captura.rutaEvidencia = rutaArchivoEvidencia;
                    captura.usuarioEvaluador_id = 0;
                    captura.comentariosEvaluador = "";
                    captura.progreso = 0;
                    captura.terminacion = false;
                    captura.fechaEvaluacion = null;
                    captura.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    captura.fechaCreacion = DateTime.Now;
                    captura.usuarioModificacion_id = 0;
                    captura.fechaModificacion = null;
                    captura.registroActivo = true;

                    _context.tblSC_Evidencia.Add(captura);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(captura));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "GuardarEvidencia", e, AccionEnum.AGREGAR, 0, captura);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(fileName));
        }

        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        public Dictionary<string, object> CargarDatosArchivoEvidencia(int evidencia_id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblSC_Evidencia.FirstOrDefault(x => x.id == evidencia_id);

                Stream fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(captura.rutaEvidencia).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "CargarDatosArchivoEvidencia", e, AccionEnum.CONSULTA, 0, evidencia_id);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivoEvidencia(int evidencia_id)
        {
            try
            {
                var captura = _context.tblSC_Evidencia.FirstOrDefault(x => x.id == evidencia_id);

                var fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                string name = Path.GetFileName(captura.rutaEvidencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "DescargarArchivoEvidencia", e, AccionEnum.CONSULTA, 0, evidencia_id);
                return null;
            }
        }

        public Dictionary<string, object> GetActividadesEvaluacion(int agrupacion_id, int estatus)
        {
            try
            {
                var agrupaciones = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
                var relacionEmpleadoAreaAgrupacion = _context.tblSC_RelacionEmpleadoAreaAgrupacion.Where(x =>
                    x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id && x.tipoUsuario == TipoUsuarioSCEnum.evaluador && (agrupacion_id > 0 ? x.agrupacion_id == agrupacion_id : true)
                ).ToList();

                var actividades = (
                    from act in _context.tblSC_Actividad.Where(x => x.registroActivo).ToList()
                    join rel in relacionEmpleadoAreaAgrupacion on act.areaEvaluadora equals rel.area
                    join are in _context.tblSC_Area.Where(x => x.registroActivo).ToList() on act.area equals are.id
                    join areE in _context.tblSC_Area.Where(x => x.registroActivo).ToList() on act.areaEvaluadora equals areE.id
                    select new ActividadDTO
                    {
                        id = act.id,
                        agrupacion_id = rel.agrupacion_id,
                        agrupacionDesc = agrupaciones.Where(x => x.id == rel.agrupacion_id).Select(x => x.nomAgrupacion).FirstOrDefault(),
                        area = act.area,
                        areaDesc = are.descripcion,
                        areaEvaluadora = act.areaEvaluadora,
                        areaEvaluadoraDesc = areE.descripcion,
                        nombre = act.nombre,
                        descripcion = act.descripcion,
                        clasificacion = act.clasificacion,
                        clasificacionDesc = act.clasificacion.GetDescription(),
                        porcentaje = act.porcentaje,
                        evidencias = new List<tblSC_Evidencia>()
                    }
                ).ToList();
                var actividades_id = actividades.Select(x => x.id).ToList();
                var evidencias = _context.tblSC_Evidencia.Where(x =>
                    x.registroActivo &&
                    (agrupacion_id > 0 ? x.agrupacion_id == agrupacion_id : true) &&
                    actividades_id.Contains(x.actividad_id)
                ).ToList();

                foreach (var act in actividades)
                {
                    var evidenciasActividad = evidencias.Where(x => x.agrupacion_id == act.agrupacion_id && x.actividad_id == act.id).ToList();

                    act.evidencias = evidenciasActividad;
                    act.progresoReal = evidenciasActividad.Count() > 0 ? evidenciasActividad.Last().progreso : 0m;
                }

                //Se filtran las actividades que sí tengan evidencias capturadas.
                actividades = actividades.Where(x => x.evidencias.Count() > 0).ToList();

                switch (estatus)
                {
                    case 1: //Evaluadas
                        actividades = actividades.Where(x => x.evidencias.All(y => y.usuarioEvaluador_id > 0) || x.evidencias.Any(y => y.terminacion)).ToList();
                        break;
                    case 2: //No Evaluadas
                        actividades = actividades.Where(x => x.evidencias.All(y => y.usuarioEvaluador_id == 0)).ToList();
                        break;
                    default: //Todas
                        break;
                }

                resultado.Add("data", actividades);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "GetEvidenciasEvaluacion", e, AccionEnum.CONSULTA, 0, new { agrupacion_id = agrupacion_id, estatus = estatus });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id)
        {
            try
            {
                var evidencias = _context.tblSC_Evidencia.Where(x => x.registroActivo && x.agrupacion_id == agrupacion_id && x.actividad_id == actividad_id).ToList();
                var data = evidencias.Select(x => new
                {
                    id = x.id,
                    agrupacion_id = x.agrupacion_id,
                    area = x.area,
                    actividad_id = x.actividad_id,
                    consecutivo = x.consecutivo,
                    rutaEvidencia = x.rutaEvidencia,
                    progresoEstimado = x.progresoEstimado,
                    comentariosCaptura = x.comentariosCaptura,
                    usuarioEvaluador_id = x.usuarioEvaluador_id,
                    comentariosEvaluador = x.comentariosEvaluador,
                    progreso = x.progreso,
                    terminacion = x.terminacion,
                    fechaEvaluacion = x.fechaEvaluacion,
                    fechaEvaluacionString = x.fechaEvaluacion != null ? ((DateTime)x.fechaEvaluacion).ToShortDateString() : "",
                    usuarioCreacion_id = x.usuarioCreacion_id,
                    fechaCreacion = x.fechaCreacion,
                    fechaCreacionString = x.fechaCreacion.ToShortDateString(),
                    usuarioModificacion_id = x.usuarioModificacion_id,
                    fechaModificacion = x.fechaModificacion,
                    fechaModificacionString = x.fechaModificacion != null ? ((DateTime)x.fechaModificacion).ToShortDateString() : "",
                    evaluacionCompletada = evidencias.Any(y => y.terminacion) || evidencias.All(y => y.usuarioEvaluador_id > 0)
                }).ToList();

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "GetEvidenciasActividad", e, AccionEnum.CONSULTA, 0, new { agrupacion_id = agrupacion_id, area = area, actividad_id = actividad_id });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarEvaluaciones(List<tblSC_Evidencia> evaluaciones)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuario = vSesiones.sesionUsuarioDTO;
                    var fechaActual = DateTime.Now.Date;
                    var listaAgrupaciones = _context.tblS_IncidentesAgrupacionCC.ToList();
                    var listaActividades = _context.tblSC_Actividad.ToList();
                    var listaCapturistas = new List<Tuple<int, string>>();

                    foreach (var eva in evaluaciones)
                    {
                        var evidenciaSIGOPLAN = _context.tblSC_Evidencia.FirstOrDefault(x => x.id == eva.id);
                        var listaEvidenciasSIGOPLAN = _context.tblSC_Evidencia.Where(x => x.registroActivo && x.agrupacion_id == evidenciaSIGOPLAN.agrupacion_id && x.actividad_id == evidenciaSIGOPLAN.actividad_id).ToList();

                        if (evidenciaSIGOPLAN != null)
                        {
                            evidenciaSIGOPLAN.usuarioEvaluador_id = usuario.id;
                            evidenciaSIGOPLAN.comentariosEvaluador = eva.comentariosEvaluador ?? "";
                            evidenciaSIGOPLAN.progreso = eva.progreso;
                            evidenciaSIGOPLAN.terminacion = eva.progreso == 100 ? true : eva.consecutivo < 5 ? eva.terminacion : !listaEvidenciasSIGOPLAN.Any(x => x.terminacion) ? true : false;
                            evidenciaSIGOPLAN.fechaEvaluacion = fechaActual;

                            _context.SaveChanges();

                            listaCapturistas.Add(Tuple.Create(evidenciaSIGOPLAN.usuarioCreacion_id, listaAgrupaciones.FirstOrDefault(x => x.id == evidenciaSIGOPLAN.agrupacion_id).nomAgrupacion));

                            if (eva.progreso < evidenciaSIGOPLAN.progresoEstimado)
                            {
                                //Se envia correo y alerta de evaluación con calificación menor a la capturada por el evaluado

                                #region CORREO
                                var capturistaCorreo = _context.tblP_Usuario.Where(x => x.id == evidenciaSIGOPLAN.usuarioCreacion_id).Select(x => x.correo).FirstOrDefault();

                                if (capturistaCorreo != null)
                                {
                                    if (capturistaCorreo.Contains("@construplan.com.mx"))
                                    {
                                        var listaCorreos = new List<string> { capturistaCorreo };

#if DEBUG
                                        listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                                        var actividadDesc = listaActividades.FirstOrDefault(x => x.id == evidenciaSIGOPLAN.actividad_id).nombre;
                                        var evaluadorDesc = _context.tblP_Usuario.Where(x => x.id == usuario.id).Select(x => x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).FirstOrDefault();

                                        var asunto = "Actividad Evaluada - " + listaAgrupaciones.FirstOrDefault(x => x.id == evidenciaSIGOPLAN.agrupacion_id).nomAgrupacion;
                                        var mensaje = string.Format(@"Se informa que fue realizada una evaluación inferior a la captura de en la actividad {0} por el usuario {1}. Esta información podrá ser revisada en el Dashboard del módulo correspondiente.", actividadDesc, evaluadorDesc);

                                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos);
                                    }
                                    else
                                    {
                                        LogError(3, 0, NombreControlador, "GuardarEvaluaciones_CorreoInvalido", null, AccionEnum.ACTUALIZAR, 0, evaluaciones);
                                    }
                                }
                                else
                                {
                                    LogError(3, 0, NombreControlador, "GuardarEvaluaciones_CorreoInvalido", null, AccionEnum.ACTUALIZAR, 0, evaluaciones);
                                }
                                #endregion

                                #region ALERTA
                                var alerta = new tblP_Alerta();

                                alerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                                alerta.userRecibeID = evidenciaSIGOPLAN.usuarioCreacion_id;

#if DEBUG
                                alerta.userRecibeID = 3807;
#endif

                                alerta.tipoAlerta = (int)AlertasEnum.MENSAJE_VISTO_CLICK;
                                alerta.sistemaID = 3;
                                alerta.visto = false;
                                alerta.url = "/Administrativo/SeguimientoCompromisos/Captura";
                                alerta.objID = evidenciaSIGOPLAN.id;
                                alerta.msj = "Actividad Evaluada Inferior - " + listaAgrupaciones.FirstOrDefault(x => x.id == evidenciaSIGOPLAN.agrupacion_id).nomAgrupacion;
                                alerta.moduloID = 3;

                                _context.tblP_Alerta.Add(alerta);
                                _context.SaveChanges();
                                #endregion
                            }
                        }
                        else
                        {
                            throw new Exception("No se encuentra la evidencia.");
                        }
                    }

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(evaluaciones));

                    foreach (var capturista in listaCapturistas.Distinct().ToList())
                    {
                        var evidenciasCapturista = _context.tblSC_Evidencia.Where(x => x.registroActivo && x.usuarioCreacion_id == capturista.Item1).ToList();

                        if (evidenciasCapturista.All(x => x.usuarioEvaluador_id > 0))
                        {
                            // Se envia correo y alerta en caso de que se hayan evaluado todas las evidencias de cada empleado

                            #region CORREO
                            var capturistaCorreo = _context.tblP_Usuario.Where(x => x.id == capturista.Item1).Select(x => x.correo).FirstOrDefault();

                            if (capturistaCorreo != null)
                            {
                                if (capturistaCorreo.Contains("@construplan.com.mx"))
                                {
                                    var listaCorreos = new List<string> { capturistaCorreo };

#if DEBUG
                                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                                    var nombreAgrupacion = capturista.Item2;
                                    var asunto = "Evaluación de Arranque Seguimiento Compromisos - " + nombreAgrupacion;
                                    var mensaje = string.Format(@"Se informa que se ha realizado una evaluación en las actividades a cargo para el Arranque de Proyecto {0}. Esta información podrá ser revisada en el Dashboard del módulo correspondiente.", nombreAgrupacion);

                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos);
                                }
                                else
                                {
                                    LogError(3, 0, NombreControlador, "GuardarEvaluaciones_CorreoInvalido", null, AccionEnum.ACTUALIZAR, 0, evaluaciones);
                                }
                            }
                            else
                            {
                                LogError(3, 0, NombreControlador, "GuardarEvaluaciones_CorreoInvalido", null, AccionEnum.ACTUALIZAR, 0, evaluaciones);
                            }
                            #endregion

                            #region ALERTA
                            var alerta = new tblP_Alerta();

                            alerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                            alerta.userRecibeID = capturista.Item1;

#if DEBUG
                            alerta.userRecibeID = 3807;
#endif

                            alerta.tipoAlerta = (int)AlertasEnum.MENSAJE_VISTO_CLICK;
                            alerta.sistemaID = 3;
                            alerta.visto = false;
                            alerta.url = "/Administrativo/SeguimientoCompromisos/Captura";
                            alerta.objID = 0;
                            alerta.msj = string.Format(@"Se informa que se ha realizado una evaluación en las actividades a cargo para el Arranque de Proyecto {0}. Esta información podrá ser revisada en el Dashboard del módulo correspondiente", capturista.Item2);
                            alerta.moduloID = 3;

                            _context.tblP_Alerta.Add(alerta);
                            _context.SaveChanges();
                            #endregion
                        }
                    }

                    //Crear funcionalidad para tipo de alerta nuevo (4)

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(3, 0, NombreControlador, "GuardarEvaluaciones", e, AccionEnum.ACTUALIZAR, 0, evaluaciones);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CargarDashboard(List<int> listaAgrupaciones)
        {
            try
            {
                var listaAsignaciones = _context.tblSC_Asignacion.Where(x => x.registroActivo).ToList().Where(x => (listaAgrupaciones != null ? listaAgrupaciones.Contains(x.agrupacion_id) : true)).ToList();
                var listaEvidencias = _context.tblSC_Evidencia.Where(x => x.registroActivo).ToList().Where(x => (listaAgrupaciones != null ? listaAgrupaciones.Contains(x.agrupacion_id) : true)).ToList();
                var listaActividades = _context.tblSC_Actividad.Where(x => x.registroActivo).ToList();
                var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var listaResponsables = _context.tblSC_RelacionEmpleadoAreaAgrupacion.Where(x => x.registroActivo && x.tipoUsuario == TipoUsuarioSCEnum.capturista).ToList();

                #region Gráfica Progreso de Arranque
                var graficaProgresoArranque = new GraficaDTO();

                graficaProgresoArranque.categorias.Add("Desempeño Global");
                graficaProgresoArranque.serie1Descripcion = "Cumplidos";
                graficaProgresoArranque.serie2Descripcion = "No Cumplidos";

                decimal ponderacionTotalCumplimiento = 0m;
                decimal ponderacionTotalCumplido = 0m;

                foreach (var asignacion in listaAsignaciones)
                {
                    var actividad = listaActividades.FirstOrDefault(x => x.id == asignacion.actividad_id);

                    if (actividad != null)
                    {
                        ponderacionTotalCumplimiento += actividad.porcentaje;

                        var evidenciasActividad = listaEvidencias.Where(x => x.agrupacion_id == asignacion.agrupacion_id && x.actividad_id == asignacion.actividad_id).ToList();

                        if (evidenciasActividad.Count() > 0)
                        {
                            var ultimaEvidencia = evidenciasActividad.OrderByDescending(x => x.consecutivo).First();

                            if (ultimaEvidencia.progreso > 0)
                            {
                                ponderacionTotalCumplido += Math.Round(((actividad.porcentaje * ultimaEvidencia.progreso) / 100), 2);
                            }
                            else
                            {
                                ponderacionTotalCumplido += Math.Round(((actividad.porcentaje * ultimaEvidencia.progresoEstimado) / 100), 2);
                            }
                        }
                    }
                }

                decimal cumplimientoGlobal = Math.Round(((ponderacionTotalCumplido * 100) / (ponderacionTotalCumplimiento > 0 ? ponderacionTotalCumplimiento : 1)), 2);
                decimal incumplimientoGlobal = 100m - cumplimientoGlobal;

                graficaProgresoArranque.serie1.Add(cumplimientoGlobal);
                graficaProgresoArranque.serie2.Add(incumplimientoGlobal);
                #endregion

                #region Gráfica Avance por Categoría
                var graficaAvanceCategoria = new GraficaDTO();

                foreach (var clasificacion in (ClasificacionActividadSCEnum[])Enum.GetValues(typeof(ClasificacionActividadSCEnum)))
                {
                    graficaAvanceCategoria.categorias.Add(clasificacion.GetDescription());

                    var actividadesClasificacion = listaActividades.Where(x => x.clasificacion == clasificacion).ToList();
                    var asignacionesClasificacion = listaAsignaciones.Where(x => actividadesClasificacion.Select(y => y.id).Contains(x.actividad_id)).ToList();

                    decimal ponderacionTotalCumplimientoClasificacion = 0m;
                    decimal ponderacionTotalCumplidoClasificacion = 0m;

                    foreach (var asignacion in asignacionesClasificacion)
                    {
                        var actividad = listaActividades.FirstOrDefault(x => x.id == asignacion.actividad_id);

                        if (actividad != null)
                        {
                            ponderacionTotalCumplimientoClasificacion += actividad.porcentaje;

                            var evidenciasActividad = listaEvidencias.Where(x => x.agrupacion_id == asignacion.agrupacion_id && x.actividad_id == asignacion.actividad_id).ToList();

                            if (evidenciasActividad.Count() > 0)
                            {
                                var ultimaEvidencia = evidenciasActividad.OrderByDescending(x => x.consecutivo).First();

                                if (ultimaEvidencia.progreso > 0)
                                {
                                    ponderacionTotalCumplidoClasificacion += Math.Round(((actividad.porcentaje * ultimaEvidencia.progreso) / 100), 2);
                                }
                                else
                                {
                                    ponderacionTotalCumplidoClasificacion += Math.Round(((actividad.porcentaje * ultimaEvidencia.progresoEstimado) / 100), 2);
                                }
                            }
                        }
                    }

                    decimal cumplimientoClasificacion = Math.Round(((ponderacionTotalCumplidoClasificacion * 100) / (ponderacionTotalCumplimientoClasificacion > 0 ? ponderacionTotalCumplimientoClasificacion : 1)), 2);

                    graficaAvanceCategoria.serie1.Add(cumplimientoClasificacion);
                }
                #endregion

                #region Gráfica Desempeño por Área
                var graficaDesempenoArea = new GraficaDTO();
                var listaAreas = _context.tblSC_Area.Where(x => x.registroActivo).ToList();

                foreach (var area in listaAreas)
                {
                    graficaDesempenoArea.categorias.Add(area.descripcion);

                    var actividadesArea = listaActividades.Where(x => x.area == area.id).ToList();
                    var asignacionesArea = listaAsignaciones.Where(x => actividadesArea.Select(y => y.id).Contains(x.actividad_id)).ToList();

                    decimal ponderacionTotalCumplimientoArea = 0m;
                    decimal ponderacionTotalCumplidoArea = 0m;

                    foreach (var asignacion in asignacionesArea)
                    {
                        var actividad = listaActividades.FirstOrDefault(x => x.id == asignacion.actividad_id);

                        if (actividad != null)
                        {
                            ponderacionTotalCumplimientoArea += actividad.porcentaje;

                            var evidenciasActividad = listaEvidencias.Where(x => x.agrupacion_id == asignacion.agrupacion_id && x.actividad_id == asignacion.actividad_id).ToList();

                            if (evidenciasActividad.Count() > 0)
                            {
                                var ultimaEvidencia = evidenciasActividad.OrderByDescending(x => x.consecutivo).First();

                                if (ultimaEvidencia.progreso > 0)
                                {
                                    ponderacionTotalCumplidoArea += Math.Round(((actividad.porcentaje * ultimaEvidencia.progreso) / 100), 2);
                                }
                                else
                                {
                                    ponderacionTotalCumplidoArea += Math.Round(((actividad.porcentaje * ultimaEvidencia.progresoEstimado) / 100), 2);
                                }
                            }
                        }
                    }

                    decimal cumplimientoArea = Math.Round(((ponderacionTotalCumplidoArea * 100) / (ponderacionTotalCumplimientoArea > 0 ? ponderacionTotalCumplimientoArea : 1)), 2);

                    graficaDesempenoArea.serie1.Add(cumplimientoArea);
                }
                #endregion

                #region Tablas de Actividades Próximas a Vencer y Actividades Cumplidas
                var tablaActividadesDashboard = new List<ActividadDashboardDTO>();
                var hoy = DateTime.Now;

                foreach (var asignacion in listaAsignaciones)
                {
                    var actividad = listaActividades.FirstOrDefault(x => x.id == asignacion.actividad_id);

                    if (actividad != null)
                    {
                        var diasRestantes = (asignacion.fechaInicio.AddDays(actividad.diasCompromiso) - hoy).TotalDays > 0 ? (asignacion.fechaInicio.AddDays(actividad.diasCompromiso) - hoy).TotalDays : 0;
                        var evidenciasActividad = listaEvidencias.Where(x => x.actividad_id == actividad.id).ToList();
                        var avance = 0m;
                        var responsable = listaResponsables.FirstOrDefault(x => x.agrupacion_id == asignacion.agrupacion_id && x.area == actividad.area);

                        if (evidenciasActividad.Count() > 0)
                        {
                            var ultimaEvidencia = evidenciasActividad.OrderByDescending(x => x.consecutivo).First();

                            avance = ultimaEvidencia.progreso > 0 ? ultimaEvidencia.progreso : ultimaEvidencia.progresoEstimado;
                        }

                        tablaActividadesDashboard.Add(new ActividadDashboardDTO
                        {
                            actividad_id = actividad.id,
                            actividadDesc = actividad.descripcion,
                            diasRestantes = (int)diasRestantes,
                            avance = avance,
                            responsable_id = responsable != null ? responsable.usuario_id : 0,
                            responsableDesc = responsable != null ? listaUsuarios.Where(x => x.id == responsable.usuario_id).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() : "",
                            porcentaje = actividad.porcentaje
                        });
                    }
                }

                var tablaActividadesVencer = tablaActividadesDashboard.Where(x => x.diasRestantes <= 10).OrderByDescending(x => x.porcentaje).ToList();
                var tablaActividadesCumplidas = tablaActividadesDashboard.Where(x => x.avance == 100).OrderByDescending(x => x.porcentaje).ToList();
                #endregion

                resultado.Add("graficaProgresoArranque", graficaProgresoArranque);
                resultado.Add("graficaAvanceCategoria", graficaAvanceCategoria);
                resultado.Add("graficaDesempenoArea", graficaDesempenoArea);
                resultado.Add("tablaActividadesVencer", tablaActividadesVencer);
                resultado.Add("tablaActividadesCumplidas", tablaActividadesCumplidas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "CargarDashboard", e, AccionEnum.CONSULTA, 0, listaAgrupaciones);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
    }
}

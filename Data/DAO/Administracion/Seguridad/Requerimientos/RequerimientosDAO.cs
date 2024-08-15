using Core.DAO.Administracion.Seguridad.Requerimientos;
using Core.DTO;
using Core.DTO.Administracion.Seguridad;
using Core.DTO.Administracion.Seguridad.Requerimientos;
using Core.DTO.Utils.ChartJS;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contratistas;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using Core.Enum.Administracion.Seguridad.Requerimientos;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Usuarios;
using Infrastructure.DTO;
using Infrastructure.Utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Administracion.Seguridad.Requerimientos
{
    public class RequerimientosDAO : GenericDAO<tblS_Req_Requerimiento>, IRequerimientosDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        UsuarioFactoryServices ufs = new UsuarioFactoryServices();

        private readonly bool productivo = true;
        private readonly int divisionActual = 0;
        private const string NombreBaseEvidencia = @"Evidencia";
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\SEGURIDAD_REQUERIMIENTOS";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\SEGURIDAD_REQUERIMIENTOS";
        private readonly string RutaEvidencia;

        #region Constructor
        public RequerimientosDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            divisionActual = vSesiones.sesionDivisionActual;
            RutaEvidencia = Path.Combine(RutaBase, "EVIDENCIA");
        }
        #endregion

        #region Catálogos
        public Dictionary<string, object> getRequerimientos()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;

                var requerimientos = _context.tblS_Req_Requerimiento.Where(x => x.estatus && x.division == divisionActual).ToList().Select(x => new
                {
                    id = x.id,
                    requerimiento = x.requerimiento,
                    descripcion = x.descripcion,
                    clasificacion = x.clasificacion,
                    clasificacionDesc = x.clasificacion.GetDescription(),
                    fechaCreacion = x.fechaCreacion
                }).ToList();
                var combo = requerimientos.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = string.Format(@"{0} - {1}", x.requerimiento, x.descripcion)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", requerimientos);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getPuntos()
        {
            try
            {
                var puntos = _context.tblS_Req_Punto.Where(x => x.estatus && x.division == divisionActual).ToList();
                var actividades = _context.tblS_Req_Actividad.Where(x => x.estatus && x.division == divisionActual).ToList();
                var condicionantes = _context.tblS_Req_Condicionante.Where(x => x.estatus && x.division == divisionActual).ToList();
                var secciones = _context.tblS_Req_Seccion.Where(x => x.estatus && x.division == divisionActual).ToList();
                var data = puntos.Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    verificacion = x.verificacion,
                    verificacionDesc = x.verificacion.ToString(),
                    porcentaje = x.porcentaje,
                    fechaCreacion = x.fechaCreacion,
                    fechaCreacionString = x.fechaCreacion.ToShortDateString(),
                    indice = x.indice,
                    periodicidad = x.periodicidad,
                    periodicidadDesc = x.periodicidad.ToString(),
                    actividad = x.actividad,
                    actividadDesc = actividades.Where(y => y.id == x.actividad).Select(z => z.descripcion).FirstOrDefault(),
                    condicionante = x.condicionante,
                    condicionanteDesc = condicionantes.Where(y => y.id == x.condicionante).Select(z => z.descripcion).FirstOrDefault(),
                    seccion = x.seccion,
                    seccionDesc = secciones.Where(y => y.id == x.seccion).Select(z => z.descripcion).FirstOrDefault(),
                    codigo = x.codigo,
                    area = x.area,
                    requerimientoID = x.requerimientoID
                });

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getPuntosRequerimiento(int requerimientoID)
        {
            try
            {
                var puntos = _context.tblS_Req_Punto.Where(x => x.estatus && x.division == divisionActual && x.requerimientoID == requerimientoID).ToList();
                var actividades = _context.tblS_Req_Actividad.Where(x => x.estatus && x.division == divisionActual).ToList();
                var condicionantes = _context.tblS_Req_Condicionante.Where(x => x.estatus && x.division == divisionActual).ToList();
                var secciones = _context.tblS_Req_Seccion.Where(x => x.estatus && x.division == divisionActual).ToList();
                var data = puntos.Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    verificacion = x.verificacion,
                    verificacionDesc = x.verificacion.ToString(),
                    porcentaje = x.porcentaje,
                    fechaCreacion = x.fechaCreacion,
                    fechaCreacionString = x.fechaCreacion.ToShortDateString(),
                    indice = x.indice,
                    periodicidad = x.periodicidad,
                    periodicidadDesc = x.periodicidad.ToString(),
                    actividad = x.actividad,
                    actividadDesc = actividades.Where(y => y.id == x.actividad).Select(z => z.descripcion).FirstOrDefault(),
                    condicionante = x.condicionante,
                    condicionanteDesc = condicionantes.Where(y => y.id == x.condicionante).Select(z => z.descripcion).FirstOrDefault(),
                    seccion = x.seccion,
                    seccionDesc = secciones.Where(y => y.id == x.seccion).Select(z => z.descripcion).FirstOrDefault(),
                    codigo = x.codigo,
                    area = x.area,
                    requerimientoID = x.requerimientoID
                });

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var requerimientoExistente = _context.tblS_Req_Requerimiento.Where(x => x.estatus && x.division == divisionActual && x.requerimiento == requerimiento.requerimiento).FirstOrDefault();

                    if (requerimientoExistente != null)
                    {
                        throw new Exception(string.Format(@"Ya existe el requerimiento {0}.", requerimientoExistente.requerimiento));
                    }

                    requerimiento.fechaCreacion = DateTime.Now;
                    requerimiento.division = divisionActual;

                    _context.tblS_Req_Requerimiento.Add(requerimiento);
                    _context.SaveChanges();

                    if (puntos != null)
                    {
                        foreach (var pun in puntos)
                        {
                            _context.tblS_Req_Punto.Add(new tblS_Req_Punto
                            {
                                descripcion = pun.descripcion,
                                verificacion = pun.verificacion,
                                porcentaje = pun.porcentaje,
                                fechaCreacion = DateTime.Now,
                                indice = pun.indice,
                                periodicidad = pun.periodicidad,
                                actividad = pun.actividad,
                                condicionante = pun.condicionante,
                                seccion = pun.seccion,
                                codigo = pun.codigo,
                                area = pun.area,
                                requerimientoID = requerimiento.id,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var requerimientoSIGOPLAN = _context.tblS_Req_Requerimiento.FirstOrDefault(x => x.id == requerimiento.id && x.division == divisionActual);

                    requerimientoSIGOPLAN.requerimiento = requerimiento.requerimiento;
                    requerimientoSIGOPLAN.descripcion = requerimiento.descripcion;
                    requerimientoSIGOPLAN.clasificacion = requerimiento.clasificacion;
                    requerimientoSIGOPLAN.division = divisionActual;
                    _context.SaveChanges();

                    var puntosAnteriores = _context.tblS_Req_Punto.Where(x => x.requerimientoID == requerimiento.id && x.division == divisionActual).ToList();
                    var puntosAnterioresID = puntosAnteriores.Select(x => x.id).ToList();

                    foreach (var pun in puntosAnteriores)
                    {
                        pun.estatus = false;
                        _context.SaveChanges();
                    }

                    var listaAsignaciones = _context.tblS_Req_Asignacion.Where(x => x.estatus && puntosAnterioresID.Contains(x.puntoID) && x.division == divisionActual).ToList();

                    foreach (var asig in listaAsignaciones)
                    {
                        asig.estatus = false;
                        _context.SaveChanges();
                    }

                    if (puntos != null)
                    {
                        foreach (var pun in puntos)
                        {
                            _context.tblS_Req_Punto.Add(new tblS_Req_Punto
                            {
                                descripcion = pun.descripcion,
                                verificacion = pun.verificacion,
                                porcentaje = pun.porcentaje,
                                fechaCreacion = DateTime.Now,
                                indice = pun.indice,
                                periodicidad = pun.periodicidad,
                                actividad = pun.actividad,
                                condicionante = pun.condicionante,
                                seccion = pun.seccion,
                                codigo = pun.codigo,
                                area = pun.area,
                                requerimientoID = requerimiento.id,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarRequerimiento(tblS_Req_Requerimiento requerimiento)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var requerimientoSIGOPLAN = _context.tblS_Req_Requerimiento.FirstOrDefault(x => x.id == requerimiento.id && x.division == divisionActual);

                    requerimientoSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    #region Eliminar puntos
                    var puntos = _context.tblS_Req_Punto.Where(x => x.requerimientoID == requerimiento.id && x.division == divisionActual).ToList();

                    foreach (var pun in puntos)
                    {
                        pun.estatus = false;
                        _context.SaveChanges();

                        #region Eliminar asignación
                        var asignaciones = _context.tblS_Req_Asignacion.Where(x => x.estatus && x.puntoID == pun.id).ToList();

                        foreach (var asig in asignaciones)
                        {
                            asig.estatus = false;
                            _context.SaveChanges();
                        }
                        #endregion

                        #region Eliminar evidencia
                        var evidencias = _context.tblS_Req_Evidencia.Where(x => x.estatus && x.requerimientoID == requerimiento.id && x.puntoID == pun.id).ToList();
                        var flagEvidenciaAprobada = false;

                        foreach (var evi in evidencias)
                        {
                            if (evi.aprobado)
                            {
                                flagEvidenciaAprobada = true;
                            }

                            evi.estatus = false;
                            _context.SaveChanges();

                            SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(new { funcion = "eliminarRequerimiento", evi = evi }));
                        }

                        if (flagEvidenciaAprobada)
                        {
                            LogError(0, 0, "RequerimientosController", "eliminarRequerimiento - Se ha eliminado evidencia aprobada.", null, AccionEnum.ACTUALIZAR, 0, requerimiento);
                        }
                        #endregion

                        #region Eliminar evidencia auditoría
                        var evidenciasAuditoria = _context.tblS_Req_Evidencia_Auditoria.Where(x => x.estatus && x.requerimientoID == requerimiento.id && x.puntoID == pun.id).ToList();

                        foreach (var eviA in evidenciasAuditoria)
                        {
                            eviA.estatus = false;
                            _context.SaveChanges();
                        }
                        #endregion
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getActividades()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;

                var actividades = _context.tblS_Req_Actividad.Where(x => x.estatus && x.division == divisionActual).Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    fechaCreacion = x.fechaCreacion
                }).ToList();
                var combo = actividades.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = string.Format(@"{0}", x.descripcion)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", actividades);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaActividad(tblS_Req_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    actividad.fechaCreacion = DateTime.Now;
                    actividad.division = divisionActual;

                    _context.tblS_Req_Actividad.Add(actividad);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarActividad(tblS_Req_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var actividadSIGOPLAN = _context.tblS_Req_Actividad.FirstOrDefault(x => x.id == actividad.id && x.division == divisionActual);

                    actividadSIGOPLAN.descripcion = actividad.descripcion;
                    actividadSIGOPLAN.division = divisionActual;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarActividad(tblS_Req_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var actividadSIGOPLAN = _context.tblS_Req_Actividad.FirstOrDefault(x => x.id == actividad.id && x.division == divisionActual);

                    actividadSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getCondicionantes()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;

                var condicionantes = _context.tblS_Req_Condicionante.Where(x => x.estatus && x.division == divisionActual).Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    fechaCreacion = x.fechaCreacion
                }).ToList();
                var combo = condicionantes.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = string.Format(@"{0}", x.descripcion)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", condicionantes);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaCondicionante(tblS_Req_Condicionante condicionante)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    condicionante.fechaCreacion = DateTime.Now;
                    condicionante.division = divisionActual;

                    _context.tblS_Req_Condicionante.Add(condicionante);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarCondicionante(tblS_Req_Condicionante condicionante)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var condicionanteSIGOPLAN = _context.tblS_Req_Condicionante.FirstOrDefault(x => x.id == condicionante.id && x.division == divisionActual);

                    condicionanteSIGOPLAN.descripcion = condicionante.descripcion;
                    condicionanteSIGOPLAN.division = divisionActual;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarCondicionante(tblS_Req_Condicionante condicionante)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var condicionanteSIGOPLAN = _context.tblS_Req_Condicionante.FirstOrDefault(x => x.id == condicionante.id && x.division == divisionActual);

                    condicionanteSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getSecciones()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;

                var secciones = _context.tblS_Req_Seccion.Where(x => x.estatus && x.division == divisionActual).Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    fechaCreacion = x.fechaCreacion
                }).ToList();
                var combo = secciones.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = string.Format(@"{0}", x.descripcion)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", secciones);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaSeccion(tblS_Req_Seccion seccion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    seccion.fechaCreacion = DateTime.Now;
                    seccion.division = divisionActual;

                    _context.tblS_Req_Seccion.Add(seccion);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarSeccion(tblS_Req_Seccion seccion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var seccionSIGOPLAN = _context.tblS_Req_Seccion.FirstOrDefault(x => x.id == seccion.id && x.division == divisionActual);

                    seccionSIGOPLAN.descripcion = seccion.descripcion;
                    seccionSIGOPLAN.division = divisionActual;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarSeccion(tblS_Req_Seccion seccion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var seccionSIGOPLAN = _context.tblS_Req_Seccion.FirstOrDefault(x => x.id == seccion.id && x.division == divisionActual);

                    seccionSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getRelacionCentroCostoDivision()
        {
            try
            {
                var data = new List<dynamic>();

                List<tblS_Req_Division> divisiones = _context.tblS_Req_Division.Where(x => x.estatus).ToList();
                List<tblS_Req_CentroCostoDivision> relacionCentroCosto = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus).ToList();

                #region Agrupaciones de Centros de Costos
                List<tblS_IncidentesAgrupacionCC> listaAgrupacionesCentroCosto = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();

                foreach (var agrupacionCentroCosto in listaAgrupacionesCentroCosto)
                {
                    var relacion = relacionCentroCosto.FirstOrDefault(x => x.idEmpresa < 1000 && x.idAgrupacion == agrupacionCentroCosto.id);

                    data.Add(new
                    {
                        grupo = agrupacionCentroCosto.id,
                        empresa = 0,
                        descripcion = agrupacionCentroCosto.nomAgrupacion,
                        division = relacion != null ? relacion.division : 0,
                        divisionDescripcion = relacion != null ? (divisiones.FirstOrDefault(x => x.id == relacion.division) != null ? divisiones.FirstOrDefault(x => x.id == relacion.division).descripcion : "") : "",
                        lineaNegocio_id = relacion != null ? relacion.lineaNegocio_id : 0
                    });
                }
                #endregion

                List<tblS_IncidentesEmpresasContratistas> lstEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();

                foreach (var empresa in lstEmpresas)
                {
                    var relacion = relacionCentroCosto.FirstOrDefault(x => x.idEmpresa == 1000 && x.idAgrupacion == empresa.id);

                    data.Add(new
                    {
                        grupo = empresa.id,
                        empresa = 1000,
                        descripcion = empresa.nombreEmpresa,
                        division = relacion != null ? relacion.division : 0,
                        divisionDescripcion = relacion != null ? (divisiones.FirstOrDefault(x => x.id == relacion.division) != null ? divisiones.FirstOrDefault(x => x.id == relacion.division).descripcion : "") : "",
                        lineaNegocio_id = relacion != null ? relacion.lineaNegocio_id : 0
                    });
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getDivisiones()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;

                var divisiones = _context.tblS_Req_Division.Where(x => x.estatus).Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    fechaCreacion = x.fechaCreacion
                }).ToList();
                var combo = divisiones.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = string.Format(@"{0}", x.descripcion)
                }).OrderBy(x => +x.Value).ToList();

                resultado.Add("data", divisiones);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetLineaNegocioCombo(int division)
        {
            try
            {
                var listaLineasNegocio = _context.tblS_Req_LineaNegocio.Where(x => x.registroActivo && (division > 0 ? x.division == division : true)).Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.descripcion,
                    Prefijo = x.division.ToString()
                }).OrderBy(x => x.Text).ToList();

                resultado.Add(ITEMS, listaLineasNegocio);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarRelacionCentroCostoDivision(int idEmpresa, int idAgrupacion, int division, int lineaNegocio_id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validar que la linea de negocio sea de la división a capturar
                    if (lineaNegocio_id > 0)
                    {
                        var registroLineaNegocio = _context.tblS_Req_LineaNegocio.FirstOrDefault(x => x.id == lineaNegocio_id);

                        if (registroLineaNegocio.division != division) //Si la división es distinta se quita la linea de negocio para que se vuelva a capturar en la división correcta.
                        {
                            lineaNegocio_id = 0;
                        }
                    }
                    #endregion

                    var registroExistente = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && x.idEmpresa == idEmpresa && x.idAgrupacion == idAgrupacion).FirstOrDefault();

                    if (registroExistente == null)
                    {
                        tblS_Req_CentroCostoDivision registroNuevo = new tblS_Req_CentroCostoDivision();

                        registroNuevo.cc = "";
                        registroNuevo.idEmpresa = idEmpresa;
                        registroNuevo.idAgrupacion = idAgrupacion;
                        registroNuevo.division = division;
                        registroNuevo.lineaNegocio_id = lineaNegocio_id;
                        registroNuevo.fechaCreacion = DateTime.Now;
                        registroNuevo.estatus = true;

                        _context.tblS_Req_CentroCostoDivision.Add(registroNuevo);
                        _context.SaveChanges();
                    }
                    else
                    {
                        registroExistente.division = division;
                        registroExistente.lineaNegocio_id = lineaNegocio_id;
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getRelacionesEmpleadoAreaCC()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;

                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    #region PERSONAL INTERNO
                    //var odbcCC = new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" };

                    //List<dynamic> listaCCEnkontrol = _contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, odbcCC);
                    var listaCCEnkontrol = _context.tblC_Nom_CatalogoCC.Select(x => new { cc = x.cc, descripcion = x.ccDescripcion }).ToList();
                    var agrups = _context.tblS_IncidentesAgrupacionCC.ToList();
                    List<tblS_IncidentesEmpresasContratistas> lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                    List<tblS_IncidentesAgrupacionContratistas> lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                    var listaAreasAC = _context.tblSAC_Departamentos.Where(x => x.estatus).ToList();

                    var relaciones = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.estatus && x.division == divisionActual).ToList().Select(x => new RelacionEmpleadoAreaCCDTO
                    {
                        id = x.id,
                        idUsuario = x.idUsuario,
                        empleado = x.empleado,
                        area = x.area,
                        areaDesc = listaAreasAC.Where(y => y.id == x.area).Select(z => z.descripcion).FirstOrDefault(), //areaDesc = x.area.GetDescription(),
                        //cc = x.cc,
                        //ccDesc = x.cc + "-" + (string)listaCCEnkontrol.FirstOrDefault(y => (string)y.cc == x.cc).descripcion,
                        cc = x.idAgrupacion != null ? x.idAgrupacion.ToString() : "",
                        ccDesc =
                            (x.idAgrupacion > 0 && x.idAgrupacion != null && x.idEmpresa == 0) ?
                                (agrups.FirstOrDefault(y => y.id == x.idAgrupacion) != null ? agrups.FirstOrDefault(y => y.id == x.idAgrupacion).nomAgrupacion : "")
                            : x.idEmpresa == 1000 ?
                                (lstContratistas.FirstOrDefault(y => y.id == x.idAgrupacion) != null ? lstContratistas.FirstOrDefault(y => y.id == x.idAgrupacion).nombreEmpresa : "")
                            : (lstAgrupacionContratistas.FirstOrDefault(y => y.id == x.idAgrupacion) != null ? lstAgrupacionContratistas.FirstOrDefault(y => y.id == x.idAgrupacion).nomAgrupacion : ""),
                        division = x.division,
                        esContratista = x.esContratista
                    }).ToList();

                    if (relaciones.Count() > 0)
                    {
                        //                    var odbcEmpleados = new OdbcConsultaDTO()
                        //                    {
                        //                        consulta = @"
                        //                        SELECT 
                        //                            e.clave_empleado AS claveEmpleado, 
                        //                            e.nombre, 
                        //                            e.ape_paterno AS apellidoPaterno, 
                        //                            e.ape_materno AS apellidoMaterno, 
                        //                            (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreCompleto,
                        //                            p.puesto AS puesto, 
                        //                            p.descripcion AS puestoDesc 
                        //                        FROM sn_empleados AS e 
                        //                            INNER JOIN si_puestos AS p on e.puesto = p.puesto"
                        //                    };

                        //                    List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, odbcEmpleados);

                        var listaEmpleados = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                            consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        e.nombre, 
                                        e.ape_paterno AS apellidoPaterno, 
                                        e.ape_materno AS apellidoMaterno, 
                                        (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreCompleto,
                                        p.puesto AS puesto, 
                                        p.descripcion AS puestoDesc 
                                    FROM tblRH_EK_Empleados AS e 
                                        INNER JOIN tblRH_EK_Puestos AS p on e.puesto = p.puesto",
                        });

                        var relSIGOPLAN = relaciones.Where(x => x.esContratista == false).ToList();
                        foreach (var rel in relSIGOPLAN)
                        {
                            var empleado = listaEmpleados.FirstOrDefault(x => (int)x.claveEmpleado == rel.empleado);

                            if (empleado != null)
                            {
                                rel.nombre = (string)empleado.nombre;
                                rel.apellidoPaterno = (string)empleado.apellidoPaterno;
                                rel.apellidoMaterno = (string)empleado.apellidoMaterno;
                                rel.nombreCompleto = (string)empleado.nombreCompleto;
                                rel.puesto = (int)empleado.puesto;
                                rel.puestoDesc = (string)empleado.puestoDesc;
                            }
                        }

                        List<tblS_IncidentesEmpleadoContratistas> lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.esActivo).ToList();
                        var relContratistas = relaciones.Where(x => x.esContratista == true).ToList();
                        foreach (var rel in relContratistas)
                        {
                            var empleado = new tblS_IncidentesEmpleadoContratistas();
                            empleado = lstEmpleadosContratistas.FirstOrDefault(x => x.id == rel.empleado);

                            if (empleado != null)
                            {
                                rel.nombre = (string)empleado.nombre;
                                rel.apellidoPaterno = (string)empleado.apePaterno;
                                rel.apellidoMaterno = (string)empleado.apeMaterno;
                                rel.nombreCompleto = (string)empleado.nombre + " " + (string)empleado.apePaterno + " " + (string)empleado.apeMaterno;
                                //rel.puesto = (int)empleado.puesto;
                                rel.puestoDesc = (string)empleado.puesto;
                            }
                        }
                    }
                    #endregion
                    resultado.Add("data", relaciones);
                }
                else
                {


                    var listaCCEmpresas = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                     {
                         Value = x.cc,
                         Text = x.cc + " - " + x.descripcion
                     }).ToList();

                    var agrups = _context.tblS_IncidentesAgrupacionCC.ToList();
                    List<tblS_IncidentesEmpresasContratistas> lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                    List<tblS_IncidentesAgrupacionContratistas> lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                    var listaAreasAC = _context.tblSAC_Departamentos.Where(x => x.estatus).ToList();

                    var relaciones = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.estatus && x.division == divisionActual).ToList().Select(x => new RelacionEmpleadoAreaCCDTO
                    {
                        id = x.id,
                        idUsuario = x.idUsuario,
                        empleado = x.empleado,
                        area = x.area,
                        areaDesc = listaAreasAC.Where(y => y.id == x.area).Select(z => z.descripcion).FirstOrDefault(), //areaDesc = x.area.GetDescription(),
                        //cc = x.cc,
                        //ccDesc = x.cc + "-" + (string)listaCCEnkontrol.FirstOrDefault(y => (string)y.cc == x.cc).descripcion,
                        cc = x.idAgrupacion != null ? x.idAgrupacion.ToString() : "",
                        ccDesc =
                            (x.idAgrupacion > 0 && x.idAgrupacion != null && x.idEmpresa == (int)vSesiones.sesionEmpresaActual) ?
                                (agrups.FirstOrDefault(y => y.id == x.idAgrupacion) != null ? agrups.FirstOrDefault(y => y.id == x.idAgrupacion).nomAgrupacion : "")
                            : x.idEmpresa == 1000 ?
                                (lstContratistas.FirstOrDefault(y => y.id == x.idAgrupacion) != null ? lstContratistas.FirstOrDefault(y => y.id == x.idAgrupacion).nombreEmpresa : "")
                            : (lstAgrupacionContratistas.FirstOrDefault(y => y.id == x.idAgrupacion) != null ? lstAgrupacionContratistas.FirstOrDefault(y => y.id == x.idAgrupacion).nomAgrupacion : ""),
                        division = x.division,
                        esContratista = x.esContratista
                    }).ToList();

                    if (relaciones.Count() > 0)
                    {
                        var listaEmpleadosEmpresas = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        e.nombre, 
                                        e.ape_paterno AS apellidoPaterno, 
                                        e.ape_materno AS apellidoMaterno, 
                                        (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreCompleto,
                                        p.puesto AS puesto, 
                                        p.descripcion AS puestoDesc 
                                    FROM tblRH_EK_Empleados AS e 
                                        INNER JOIN tblRH_EK_Puestos AS p on e.puesto = p.puesto",
                        });

                        var relEmpresas = relaciones.Where(x => x.esContratista == false).ToList();
                        foreach (var rel in relEmpresas)
                        {
                            var empleado = listaEmpleadosEmpresas.FirstOrDefault(x => (int)x.claveEmpleado == rel.empleado);

                            if (empleado != null)
                            {
                                rel.nombre = (string)empleado.nombre;
                                rel.apellidoPaterno = (string)empleado.apellidoPaterno;
                                rel.apellidoMaterno = (string)empleado.apellidoMaterno;
                                rel.nombreCompleto = (string)empleado.nombreCompleto;
                                rel.puesto = (int)empleado.puesto;
                                rel.puestoDesc = (string)empleado.puestoDesc;
                            }
                        }

                        List<tblS_IncidentesEmpleadoContratistas> lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.esActivo).ToList();
                        var relContratistas = relaciones.Where(x => x.esContratista == true).ToList();
                        foreach (var rel in relContratistas)
                        {
                            var empleado = new tblS_IncidentesEmpleadoContratistas();
                            empleado = lstEmpleadosContratistas.FirstOrDefault(x => x.id == rel.empleado);

                            if (empleado != null)
                            {
                                rel.nombre = (string)empleado.nombre;
                                rel.apellidoPaterno = (string)empleado.apePaterno;
                                rel.apellidoMaterno = (string)empleado.apeMaterno;
                                rel.nombreCompleto = (string)empleado.nombre + " " + (string)empleado.apePaterno + " " + (string)empleado.apeMaterno;
                                //rel.puesto = (int)empleado.puesto;
                                rel.puestoDesc = (string)empleado.puesto;
                            }
                        }
                        resultado.Add("data", relaciones);
                    }
                }


                //resultado.Add("data", relaciones);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!esContratista)
                    {
                        #region PERSONAL INTERNO
                        var usuarioSIGOPLAN = _context.tblP_Usuario.ToList().FirstOrDefault(x => x.estatus && x.cveEmpleado == relacion.empleado.ToString());

                        if (usuarioSIGOPLAN == null)
                        {
                            throw new Exception("No se encuentra el usuario de SIGOPLAN para el empleado \"" + relacion.empleado + "\".");
                        }

                        if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                        {
                            relacion.idEmpresa = vSesiones.sesionEmpresaActual;
                        }

                        relacion.idUsuario = usuarioSIGOPLAN.id;
                        relacion.division = divisionActual;
                        relacion.esContratista = false;

                        _context.tblS_Req_EmpleadoAreaCC.Add(relacion);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region USUARIO CONTRATISTA
                        var usuarioContratista = _context.tblS_IncidentesEmpleadoContratistas.ToList().FirstOrDefault(x => x.id == relacion.empleado && x.esActivo);
                        if (usuarioContratista == null)
                            throw new Exception("No se encuentra el usuario de SIGOPLAN para el empleado \"" + relacion.empleado + "\".");

                        relacion.idUsuario = usuarioContratista.id;
                        relacion.division = divisionActual;
                        relacion.esContratista = true;
                        _context.tblS_Req_EmpleadoAreaCC.Add(relacion);
                        _context.SaveChanges();
                        #endregion
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!esContratista)
                    {
                        #region PERSONAL INTERNO
                        var usuarioSIGOPLAN = _context.tblP_Usuario.ToList().FirstOrDefault(x => x.estatus && x.cveEmpleado == relacion.empleado.ToString());
                        var relacionSIGOPLAN = _context.tblS_Req_EmpleadoAreaCC.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                        //if (usuarioSIGOPLAN == null)
                        //{
                        //    throw new Exception("No se encuentra el usuario de SIGOPLAN para el empleado \"" + relacion.empleado + "\".");
                        //}

                        //if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                        //{
                        //    relacion.idEmpresa = vSesiones.sesionEmpresaActual;
                        //}

                        relacionSIGOPLAN.idUsuario = usuarioSIGOPLAN.id;
                        relacionSIGOPLAN.empleado = relacion.empleado;
                        relacionSIGOPLAN.area = relacion.area;
                        //relacionSIGOPLAN.cc = relacion.cc;
                        relacionSIGOPLAN.idEmpresa = relacion.idEmpresa;
                        relacionSIGOPLAN.idAgrupacion = relacion.idAgrupacion;
                        relacionSIGOPLAN.division = divisionActual;
                        relacionSIGOPLAN.esContratista = esContratista;
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region USUARIO CONTRATISTA
                        var usuarioContratista = _context.tblS_IncidentesEmpleadoContratistas.ToList().FirstOrDefault(x => x.id == relacion.empleado && x.esActivo);
                        var relacionContratista = _context.tblS_Req_EmpleadoAreaCC.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                        relacionContratista.idUsuario = usuarioContratista.id;
                        relacionContratista.empleado = relacion.empleado;
                        relacionContratista.area = relacion.area;
                        relacionContratista.idEmpresa = relacion.idEmpresa;
                        relacionContratista.idAgrupacion = relacion.idAgrupacion;
                        relacionContratista.division = divisionActual;
                        relacionContratista.esContratista = esContratista;
                        _context.SaveChanges();
                        #endregion
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarRelacion(tblS_Req_EmpleadoAreaCC relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var relacionSIGOPLAN = _context.tblS_Req_EmpleadoAreaCC.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                    relacionSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        #endregion

        public Dictionary<string, object> getAsignacion(int idEmpresa, int idAgrupacion)
        {
            try
            {

                if (idEmpresa == null)
                {
                    idEmpresa = (int)vSesiones.sesionEmpresaActual;
                }

                var puntos = from asig in _context.tblS_Req_Asignacion.Where(x => x.division == divisionActual).ToList()
                             join pun in _context.tblS_Req_Punto.Where(x => x.division == divisionActual).ToList() on asig.puntoID equals pun.id
                             join req in _context.tblS_Req_Requerimiento.Where(x => x.division == divisionActual).ToList() on pun.requerimientoID equals req.id
                             where asig.estatus && asig.idEmpresa == idEmpresa && asig.idAgrupacion == idAgrupacion
                             select new
                             {
                                 id = asig.id,
                                 cc = 0,
                                 idEmpresa = asig.idEmpresa,
                                 idAgrupacion = asig.idAgrupacion,
                                 puntoID = pun.id,
                                 fechaAsignacion = asig.fechaAsignacion,
                                 fechaAsignacionString = asig.fechaAsignacion.ToShortDateString(),
                                 fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                 fechaInicioEvaluacionString = asig.fechaInicioEvaluacion.ToShortDateString(),
                                 indice = pun.indice,
                                 descripcion = pun.descripcion,
                                 fechaCreacion = pun.fechaCreacion,
                                 fechaCreacionString = pun.fechaCreacion.ToShortDateString(),
                                 requerimiento = req.requerimiento,
                                 requerimientoDesc = req.descripcion,
                                 requerimientoClasificacion = req.clasificacion,
                                 requerimientoClasificacionDesc = req.clasificacion.GetDescription(),
                                 codigo = pun.codigo,
                                 area = pun.area
                             };

                var combo = puntos.Select(x => new ComboDTO
                {
                    Value = x.puntoID,
                    Text = string.Format(@"{0} {1}", x.indice, x.descripcion)
                }).ToList();

                resultado.Add("data", puntos);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarAsignacion(AsignacionDTO asignacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var puntos = (from pun in _context.tblS_Req_Punto
                                  join req in _context.tblS_Req_Requerimiento on pun.requerimientoID equals req.id
                                  where pun.division == divisionActual
                                  select new
                                  {
                                      id = pun.id,
                                      division = pun.division,
                                      estatus = pun.estatus,
                                      clasificacion = req.clasificacion,
                                      requerimientoID = pun.requerimientoID,
                                      actividad = pun.actividad,
                                      condicionante = pun.condicionante,
                                      seccion = pun.seccion
                                  }).ToList();

                    puntos = puntos.Where(x =>
                        x.estatus &&
                        asignacion.clasificaciones.Contains((int)x.clasificacion) &&
                        asignacion.requerimientos.Contains(x.requerimientoID) &&
                        asignacion.actividades.Contains(x.actividad) &&
                        asignacion.condicionantes.Contains(x.condicionante) &&
                        asignacion.secciones.Contains(x.seccion)
                    ).ToList();

                    foreach (var gpo in asignacion.arrGrupos)
                    {
                        #region Se quitan las asignaciones anteriores
                        //var asignacionAnterior = _context.tblS_Req_Asignacion.Where(x => x.estatus && x.cc == cc && x.division == divisionActual).ToList();

                        //foreach (var asigAnt in asignacionAnterior)
                        //{
                        //    asigAnt.estatus = false;
                        //    _context.SaveChanges();
                        //}
                        #endregion

                        var listaAsignacionesAnteriores = _context.tblS_Req_Asignacion.Where(x => x.estatus && x.idEmpresa == gpo.idEmpresa && x.idAgrupacion == gpo.idAgrupacion && x.division == divisionActual).ToList();

                        foreach (var pun in puntos)
                        {
                            var mismaAsignacionExistente = listaAsignacionesAnteriores.FirstOrDefault(x => x.puntoID == pun.id);

                            if (mismaAsignacionExistente != null)
                            {
                                mismaAsignacionExistente.fechaAsignacion = DateTime.Now;
                                mismaAsignacionExistente.fechaInicioEvaluacion = new DateTime(asignacion.fechaInicioEvaluacion.Year, asignacion.fechaInicioEvaluacion.Month, 1);
                                _context.SaveChanges();
                            }
                            else
                            {
                                tblS_Req_Asignacion nuevaAsignacion = new tblS_Req_Asignacion();

                                nuevaAsignacion.cc = "";
                                nuevaAsignacion.idEmpresa = gpo.idEmpresa;
                                nuevaAsignacion.idAgrupacion = gpo.idAgrupacion;
                                nuevaAsignacion.puntoID = pun.id;
                                nuevaAsignacion.fechaAsignacion = DateTime.Now;
                                nuevaAsignacion.fechaInicioEvaluacion = new DateTime(asignacion.fechaInicioEvaluacion.Year, asignacion.fechaInicioEvaluacion.Month, 1);
                                nuevaAsignacion.division = divisionActual;
                                nuevaAsignacion.estatus = true;

                                _context.tblS_Req_Asignacion.Add(nuevaAsignacion);
                                _context.SaveChanges();
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarAsignacionPunto(int asignacionID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var asignacionSIGOPLAN = _context.tblS_Req_Asignacion.FirstOrDefault(x => x.id == asignacionID && x.division == divisionActual);

                    asignacionSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    #region Eliminar evidencia
                    var evidencias = _context.tblS_Req_Evidencia.Where(x =>
                        x.estatus &&
                        x.puntoID == asignacionSIGOPLAN.puntoID &&
                        x.idEmpresa == asignacionSIGOPLAN.idEmpresa &&
                        x.idAgrupacion == asignacionSIGOPLAN.idAgrupacion
                    ).ToList();
                    var flagEvidenciaAprobada = false;

                    foreach (var evi in evidencias)
                    {
                        if (evi.aprobado)
                        {
                            flagEvidenciaAprobada = true;
                        }

                        evi.estatus = false;
                        _context.SaveChanges();

                        SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(new { funcion = "eliminarAsignacionPunto", evi = evi }));
                    }

                    if (flagEvidenciaAprobada)
                    {
                        LogError(0, 0, "RequerimientosController", "eliminarAsignacionPunto - Se ha eliminado evidencia aprobada.", null, AccionEnum.ACTUALIZAR, 0, asignacionID);
                    }
                    #endregion

                    #region Eliminar evidencia auditoria
                    var evidenciasAuditoria = _context.tblS_Req_Evidencia_Auditoria.Where(x =>
                        x.estatus &&
                        x.puntoID == asignacionSIGOPLAN.puntoID &&
                        x.idEmpresa == asignacionSIGOPLAN.idEmpresa &&
                        x.idAgrupacion == asignacionSIGOPLAN.idAgrupacion
                    ).ToList();

                    foreach (var eviA in evidenciasAuditoria)
                    {
                        eviA.estatus = false;
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getAsignacionCaptura(FiltrosAsignacionCapturaDTO filtros)
        {
            try
            {
                var listaAgrupacionesCC = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
                var listaContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                var listaAgrupacionesContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                var listaAreasAC = _context.tblSAC_Departamentos.Where(x => x.estatus).ToList();

                var puntos = (from asig in _context.tblS_Req_Asignacion.Where(x => x.division == divisionActual).ToList()
                              join pun in _context.tblS_Req_Punto.Where(x => x.division == divisionActual).ToList() on asig.puntoID equals pun.id
                              join req in _context.tblS_Req_Requerimiento.Where(x => x.division == divisionActual).ToList() on pun.requerimientoID equals req.id
                              where
                                 asig.estatus &&
                                 (asig.idEmpresa == filtros.idEmpresa && asig.idAgrupacion == filtros.idAgrupacion) &&
                                 (filtros.clasificacion > 0 ? req.clasificacion == filtros.clasificacion : true) &&
                                 (filtros.requerimientoID > 0 ? req.id == filtros.requerimientoID : true) &&
                                  //(filtros.responsable > 0 ?  : true)
                                 asig.fechaInicioEvaluacion.Date <= DateTime.Now.Date
                              select new CapturaDTO
                              {
                                  id = pun.id,
                                  idEmpresa = asig.idEmpresa,
                                  idAgrupacion = asig.idAgrupacion,
                                  proyecto = asig.idEmpresa < 1000 ?
                                  (listaAgrupacionesCC.FirstOrDefault(m => m.id == asig.idAgrupacion) != null ? listaAgrupacionesCC.FirstOrDefault(m => m.id == asig.idAgrupacion).nomAgrupacion : "") :
                                  asig.idEmpresa == 1000 ?
                                  (listaContratistas.FirstOrDefault(m => m.id == asig.idAgrupacion) != null ? listaContratistas.FirstOrDefault(m => m.id == asig.idAgrupacion).nombreEmpresa : "") :
                                  asig.idEmpresa == 2000 ?
                                  (listaAgrupacionesContratistas.FirstOrDefault(m => m.id == asig.idAgrupacion) != null ? listaAgrupacionesContratistas.FirstOrDefault(m => m.id == asig.idAgrupacion).nomAgrupacion : "") :
                                  "",
                                  requerimientoID = req.id,
                                  requerimiento = req.requerimiento,
                                  requerimientoDesc = string.Format(@"{0} {1}", req.requerimiento, req.descripcion),
                                  requerimientoClasificacion = req.clasificacion,
                                  requerimientoClasificacionDesc = req.clasificacion.GetDescription(),
                                  indice = pun.indice,
                                  puntoDesc = string.Format(@"{0} {1}", pun.indice, pun.descripcion),
                                  descripcion = pun.descripcion,
                                  asignacionID = asig.id,
                                  fechaAsignacion = asig.fechaAsignacion,
                                  fechaAsignacionString = asig.fechaAsignacion.ToShortDateString(),
                                  fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                  fechaInicioEvaluacionString = asig.fechaInicioEvaluacion.ToShortDateString(),
                                  codigo = pun.codigo,
                                  periodicidad = pun.periodicidad,
                                  periodicidadDesc = pun.periodicidad.GetDescription(),
                                  area = pun.area,
                                  areaDesc = listaAreasAC.Where(y => y.id == pun.area).Select(z => z.descripcion).FirstOrDefault(), //areaDesc = pun.area.GetDescription(),
                                  responsable = 0,
                                  responsableDesc = ""
                              }).ToList();

                var puntosGrupos = puntos.Select(x => x.idAgrupacion).ToList();
                var puntosID = puntos.Select(x => x.id).ToList();

                var evidencias = _context.tblS_Req_Evidencia.ToList().Where(x => x.estatus && puntosGrupos.Contains(x.idAgrupacion) && puntosID.Contains(x.puntoID) && x.division == divisionActual).ToList();
                var listaEmpleadoAreaCC = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.estatus && x.division == divisionActual).ToList();

                foreach (var pun in puntos)
                {
                    //var mesesPeriodicidad = (int)pun.periodicidad == 1 ? 0 : (int)pun.periodicidad == 2 ? 3 : (int)pun.periodicidad == 3 ? 6 : (int)pun.periodicidad == 4 ? 12 : 0;
                    //var fechaInicioEvaluacion = pun.fechaInicioEvaluacion;
                    //var fechaFinEvaluacion = pun.fechaInicioEvaluacion;
                    //var mesesTranscurridos = fnDiferenciaMeses(pun.fechaInicioEvaluacion, DateTime.Now);
                    //int numPeriodoActual = mesesTranscurridos <= 0 ? 1 : Convert.ToInt32(Math.Ceiling((decimal)(mesesPeriodicidad / mesesTranscurridos)));

                    //DateTime mesInicioPeriodo = fechaInicioEvaluacion.AddMonths(
                    //    numPeriodoActual == 1 ? 0 : ((numPeriodoActual - 1) * mesesPeriodicidad)
                    //);
                    //DateTime mesFinPeriodo = fechaFinEvaluacion.AddMonths(
                    //    numPeriodoActual == 1 ? (mesesPeriodicidad == 0 ? 0 : (mesesPeriodicidad)) : ((numPeriodoActual * mesesPeriodicidad) - 1)
                    //);

                    //var mesesPeriodicidad = (int)pun.periodicidad == 1 ? 1 : (int)pun.periodicidad == 2 ? 3 : (int)pun.periodicidad == 3 ? 6 : (int)pun.periodicidad == 4 ? 12 : 0;
                    //var fechaInicioEvaluacion = pun.fechaInicioEvaluacion;
                    //var fechaFinEvaluacion = pun.fechaInicioEvaluacion;
                    //var mesesTranscurridos = fnDiferenciaMeses(pun.fechaInicioEvaluacion, DateTime.Now);
                    //int numPeriodoActual = mesesTranscurridos <= 0 ? 1 : Convert.ToInt32(Math.Ceiling((decimal)(mesesPeriodicidad / mesesTranscurridos)));

                    //DateTime mesInicioPeriodo = fechaInicioEvaluacion.AddMonths(
                    //    numPeriodoActual == 1 ? 0 : ((numPeriodoActual - 1) * mesesPeriodicidad)
                    //);
                    //DateTime mesFinPeriodo = fechaFinEvaluacion.AddMonths(
                    //    numPeriodoActual == 1 ? (mesesPeriodicidad == 0 ? 0 : (mesesPeriodicidad)) : ((numPeriodoActual * mesesPeriodicidad) - 1)
                    //);

                    DateTime fechaActual = DateTime.Now.Date;
                    tblS_Req_Evidencia evidenciaCapturada = null;

                    if (pun.periodicidad == PeriodicidadRequerimientoEnum.Mensual)
                    {
                        DateTime inicioPeriodo = new DateTime(fechaActual.Year, fechaActual.Month, 1); //Primer día del mes actual.
                        DateTime finPeriodo = new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month)); //Último día del mes actual.

                        evidenciaCapturada = evidencias.FirstOrDefault(x =>
                            x.idEmpresa == pun.idEmpresa && x.idAgrupacion == pun.idAgrupacion && x.puntoID == pun.id && (x.fechaPunto.Date >= inicioPeriodo.Date && x.fechaPunto.Date <= finPeriodo.Date)
                        );
                    }
                    else
                    {
                        int cantidadSumaMeses = 0;

                        switch (pun.periodicidad)
                        {
                            case PeriodicidadRequerimientoEnum.Trimestral:
                                cantidadSumaMeses = 3;
                                break;
                            case PeriodicidadRequerimientoEnum.Semestral:
                                cantidadSumaMeses = 6;
                                break;
                            case PeriodicidadRequerimientoEnum.Anual:
                                cantidadSumaMeses = 12;
                                break;
                        }

                        DateTime inicioPeriodo = pun.fechaInicioEvaluacion.Date;
                        DateTime finPeriodo = pun.fechaInicioEvaluacion.AddMonths(cantidadSumaMeses).Date; //Suma inicial de meses para la fecha del final del primer periodo

                        while ((fechaActual.Date < inicioPeriodo.Date || fechaActual.Date > finPeriodo.Date) && inicioPeriodo.Date <= fechaActual.Date) //Se verifica que la fecha actual no se encuentre dentro del rango.
                        {
                            inicioPeriodo = inicioPeriodo.AddMonths(cantidadSumaMeses);
                            finPeriodo = finPeriodo.AddMonths(cantidadSumaMeses);
                        }

                        evidenciaCapturada = evidencias.FirstOrDefault(x =>
                            x.idAgrupacion == pun.idAgrupacion && x.idEmpresa == pun.idEmpresa && x.puntoID == pun.id && (x.fechaPunto.Date >= inicioPeriodo.Date && x.fechaPunto.Date <= finPeriodo.Date)
                        );
                    }

                    if (evidenciaCapturada != null)
                    {
                        pun.evidenciaID = evidenciaCapturada.id;
                        pun.evidenciaCapturada = true;
                        pun.rutaEvidencia = evidenciaCapturada.rutaEvidencia;
                        pun.fechaEvidencia = evidenciaCapturada.fechaPunto;
                        pun.fechaEvidenciaString = evidenciaCapturada.fechaPunto.ToShortDateString();
                        pun.usuarioEvaluadorID = evidenciaCapturada.usuarioEvaluadorID;
                        pun.aprobado = evidenciaCapturada.aprobado;
                    }

                    var responsable = listaEmpleadoAreaCC.FirstOrDefault(x => x.area == pun.area && x.idAgrupacion == pun.idAgrupacion);

                    if (responsable != null)
                    {
                        //var responsableEK = (_contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                        //{
                        //    consulta = @"SELECT * FROM sn_empleados WHERE clave_empleado = ?",
                        //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = responsable.empleado } }
                        //}))[0];

                        var responsableEK = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == responsable.empleado);

                        pun.responsable = responsable.empleado;
                        pun.responsableDesc = string.Format(@"{0} {1} {2}", (string)responsableEK.nombre, (string)responsableEK.ape_paterno, (string)responsableEK.ape_materno);
                    }
                }

                switch (filtros.estatus)
                {
                    case 1: //Pendiente
                        puntos = puntos.Where(x => !x.evidenciaCapturada).ToList();
                        break;
                    case 2: //Completo
                        puntos = puntos.Where(x => x.evidenciaCapturada).ToList();
                        break;
                    case 3: //Evaluado
                        puntos = puntos.Where(x => x.evidenciaCapturada && x.usuarioEvaluadorID > 0).ToList();
                        break;
                    default: //Todos
                        break;
                }

                resultado.Add("data", puntos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public static int fnDiferenciaMeses(DateTime FechaFin, DateTime FechaInicio)
        {
            return Math.Abs((FechaFin.Month - FechaInicio.Month) + 12 * (FechaFin.Year - FechaInicio.Year));

        }

        public Dictionary<string, object> getEvidencias(string cc, int requerimientoID)
        {
            try
            {
                var centrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();
                var usuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var requerimientos = _context.tblS_Req_Requerimiento.Where(x => x.division == divisionActual).ToList();
                var puntos = _context.tblS_Req_Punto.Where(x => x.division == divisionActual).ToList();

                var evidencias = _context.tblS_Req_Evidencia.ToList().Where(x => x.estatus && x.division == divisionActual).Select(x => new
                {
                    id = x.id,
                    cc = x.cc,
                    ccDesc = centrosCosto.Where(y => y.cc == x.cc).Select(z => z.cc + "-" + z.descripcion).FirstOrDefault(),
                    requerimientoID = x.requerimientoID,
                    requerimiento = requerimientos.FirstOrDefault(y => y.id == x.requerimientoID).requerimiento,
                    requerimientoDesc = requerimientos.Where(y => y.id == x.requerimientoID).Select(z => string.Format(@"{0} {1}", z.requerimiento, z.descripcion)).FirstOrDefault(),
                    requerimientoClasificacion = requerimientos.FirstOrDefault(y => y.id == x.requerimientoID).clasificacion,
                    requerimientoClasificacionDesc = requerimientos.FirstOrDefault(y => y.id == x.requerimientoID).clasificacion.GetDescription(),
                    puntoID = x.puntoID,
                    indice = puntos.FirstOrDefault(y => y.id == x.puntoID).indice,
                    puntoDesc = puntos.Where(y => y.id == x.puntoID).Select(z => string.Format(@"{0} {1}", z.indice, z.descripcion)).FirstOrDefault(),
                    fechaPunto = x.fechaPunto,
                    fechaPuntoString = x.fechaPunto.ToShortDateString(),
                    rutaEvidencia = x.rutaEvidencia,
                    comentariosCaptura = x.comentariosCaptura,
                    fechaCaptura = x.fechaCaptura,
                    fechaCapturaString = x.fechaCaptura.ToShortDateString(),
                    usuarioCapturaID = x.usuarioCapturaID,
                    usuarioCapturaDesc = usuarios.Where(y => y.id == x.usuarioCapturaID).Select(z => string.Format(@"{0} {1} {2}", z.nombre, z.apellidoPaterno, z.apellidoMaterno)).FirstOrDefault(),
                    usuarioEvaluadorID = x.usuarioEvaluadorID,
                    usuarioEvaluadorDesc = usuarios.Where(y => y.id == x.usuarioEvaluadorID).Select(z => string.Format(@"{0} {1} {2}", z.nombre, z.apellidoPaterno, z.apellidoMaterno)).FirstOrDefault(),
                    comentariosEvaluador = x.comentariosEvaluador,
                    porcentaje = puntos.Where(y => y.id == x.puntoID).Select(z => z.porcentaje).FirstOrDefault(),
                    aprobado = x.aprobado,
                    calificacion = x.calificacion,
                    fechaEvaluacion = x.fechaEvaluacion,
                    fechaEvaluacionString = x.fechaEvaluacion != null ? ((DateTime)x.fechaEvaluacion).ToShortDateString() : ""
                }).ToList();

                #region Filtros
                if (cc != "")
                {
                    evidencias = evidencias.Where(x => x.cc == cc).ToList();
                }

                if (requerimientoID > 0)
                {
                    evidencias = evidencias.Where(x => x.requerimientoID == requerimientoID).ToList();
                }
                #endregion

                resultado.Add("data", evidencias);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarEvidencia(List<EvidenciaDTO> captura, List<HttpPostedFileBase> evidencias)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (captura.Any(x => x.idAgrupacion == 0))
                    {
                        throw new Exception("No se capturó la información de la agrupación.");
                    }

                    var usuario = vSesiones.sesionUsuarioDTO;
                    var fechaActual = DateTime.Now.Date;

                    int index = 0;
                    foreach (var cap in captura)
                    {
                        #region Validación Fecha Límite
                        if (usuario.id != 3807)
                        {
                            var fechaInicioEvaluacion = cap.fechaInicioEvaluacion.Date;
                            var fechaRango = fechaInicioEvaluacion;
                            var fechaRangoLimite = fechaInicioEvaluacion.AddDays(4);

                            switch (cap.periodicidad)
                            {
                                case PeriodicidadRequerimientoEnum.Mensual:
                                    if (fechaActual.Day >= 6)
                                    {
                                        if ((cap.fechaPunto.Month < fechaActual.Month && cap.fechaPunto.Year == fechaActual.Year) || (cap.fechaPunto.Year < fechaActual.Year))
                                        {
                                            throw new Exception("No se guardó la información. Se superó la fecha límite de captura para el mes anterior.");
                                        }
                                    }
                                    else
                                    {
                                        if (vSesiones.sesionEmpresaActual == 6)
                                        {

                                        }
                                        else
                                        {
                                            if (cap.fechaPunto.Month == fechaActual.Month)
                                            {
                                                throw new Exception("No se guardó la información. La fecha de captura para el mes actual empieza a partir del día 6 y termina el día 5 del mes siguiente.");
                                            }
                                        }


                                        var fechaValidacionAnio = new DateTime(cap.fechaPunto.AddMonths(1).Year, cap.fechaPunto.AddMonths(1).Month, fechaActual.Day);

                                        if (fechaValidacionAnio.Date != fechaActual.Date) //Validación para no permitir la captura de meses que no sean el anterior al actual.
                                        {
                                            throw new Exception("No se guardó la información. No se puede capturar información de meses pasados que no sean el anterior al actual.");
                                        }
                                    }
                                    break;
                                case PeriodicidadRequerimientoEnum.Trimestral:
                                    while (fechaRango.Date < cap.fechaPunto.Date)
                                    {
                                        fechaRango = fechaRango.AddMonths(3);
                                        fechaRangoLimite = fechaRangoLimite.AddMonths(3);
                                    }

                                    if (cap.fechaPunto.Date > fechaRangoLimite.Date)
                                    {
                                        throw new Exception("No se guardó la información. Se superó la fecha límite de captura para la evidencia.");
                                    }
                                    break;
                                case PeriodicidadRequerimientoEnum.Semestral:
                                    while (fechaRango.Date < cap.fechaPunto.Date)
                                    {
                                        fechaRango = fechaRango.AddMonths(6);
                                        fechaRangoLimite = fechaRangoLimite.AddMonths(6);
                                    }

                                    if (cap.fechaPunto.Date > fechaRangoLimite.Date)
                                    {
                                        throw new Exception("No se guardó la información. Se superó la fecha límite de captura para la evidencia.");
                                    }
                                    break;
                                case PeriodicidadRequerimientoEnum.Anual:
                                    while (fechaRango.Date < cap.fechaPunto.Date)
                                    {
                                        fechaRango = fechaRango.AddMonths(12);
                                        fechaRangoLimite = fechaRangoLimite.AddMonths(12);
                                    }

                                    if (cap.fechaPunto.Date > fechaRangoLimite.Date)
                                    {
                                        throw new Exception("No se guardó la información. Se superó la fecha límite de captura para la evidencia.");
                                    }
                                    break;
                                default:
                                    throw new Exception("Periodicidad no válida. PuntoID: " + cap.puntoID);
                            }
                        }
                        #endregion

                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidencia, evidencias[index].FileName);
                        string rutaArchivoEvidencia = Path.Combine(RutaEvidencia, nombreArchivoEvidencia);

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

                        if (GlobalUtils.SaveHTTPPostedFile(evidencias[index], rutaArchivoEvidencia) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }

                        tblS_Req_Evidencia nuevaEvidencia = new tblS_Req_Evidencia();

                        nuevaEvidencia.cc = "";
                        nuevaEvidencia.idEmpresa = cap.idEmpresa;
                        nuevaEvidencia.idAgrupacion = cap.idAgrupacion;
                        nuevaEvidencia.requerimientoID = cap.requerimientoID;
                        nuevaEvidencia.puntoID = cap.puntoID;
                        nuevaEvidencia.fechaPunto = cap.fechaPunto;
                        nuevaEvidencia.rutaEvidencia = rutaArchivoEvidencia;
                        nuevaEvidencia.comentariosCaptura = cap.comentariosCaptura ?? "";
                        nuevaEvidencia.fechaCaptura = DateTime.Now;
                        nuevaEvidencia.usuarioCapturaID = usuario.id;
                        nuevaEvidencia.usuarioEvaluadorID = 0;
                        nuevaEvidencia.comentariosEvaluador = "";
                        nuevaEvidencia.aprobado = false;
                        nuevaEvidencia.calificacion = 0;
                        nuevaEvidencia.fechaEvaluacion = null;
                        nuevaEvidencia.division = divisionActual;
                        nuevaEvidencia.estatus = true;

                        _context.tblS_Req_Evidencia.Add(nuevaEvidencia);
                        _context.SaveChanges();

                        index++;
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
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

        public Dictionary<string, object> cargarDatosArchivoEvidencia(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblS_Req_Evidencia.FirstOrDefault(x => x.id == evidenciaID && x.division == divisionActual);

                Stream fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(captura.rutaEvidencia).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Tuple<Stream, string> descargarArchivoEvidencia(int evidenciaID)
        {
            try
            {
                var captura = _context.tblS_Req_Evidencia.FirstOrDefault(x => x.id == evidenciaID && x.division == divisionActual);

                var fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                string name = Path.GetFileName(captura.rutaEvidencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, object> getEvidenciasEvaluacion(int clasificacion, int idEmpresa, int idAgrupacion, int requerimientoID, DateTime fechaInicio, DateTime fechaFin, int estatus)
        {
            try
            {
                var listaAgrupacionesCC = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
                var listaContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                var listaAgrupacionesContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                var usuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var requerimientos = _context.tblS_Req_Requerimiento.Where(x => x.division == divisionActual).ToList();
                var puntos = _context.tblS_Req_Punto.Where(x => x.division == divisionActual).ToList();
                var evidencias = (from evi in _context.tblS_Req_Evidencia
                                  join req in _context.tblS_Req_Requerimiento on evi.requerimientoID equals req.id
                                  where evi.estatus && evi.division == divisionActual
                                  select new
                                  {
                                      id = evi.id,
                                      idEmpresa = evi.idEmpresa,
                                      idAgrupacion = evi.idAgrupacion,
                                      requerimientoID = evi.requerimientoID,
                                      puntoID = evi.puntoID,
                                      fechaPunto = evi.fechaPunto,
                                      rutaEvidencia = evi.rutaEvidencia,
                                      comentariosCaptura = evi.comentariosCaptura,
                                      fechaCaptura = evi.fechaCaptura,
                                      usuarioCapturaID = evi.usuarioCapturaID,
                                      usuarioEvaluadorID = evi.usuarioEvaluadorID,
                                      comentariosEvaluador = evi.comentariosEvaluador,
                                      aprobado = evi.aprobado,
                                      calificacion = evi.calificacion,
                                      fechaEvaluacion = evi.fechaEvaluacion,
                                      clasificacion = req.clasificacion
                                  }).ToList();

                #region Filtros
                if (clasificacion > 0)
                {
                    evidencias = evidencias.Where(x => (int)x.clasificacion == clasificacion).ToList();
                }

                if (idAgrupacion != null)
                {
                    evidencias = evidencias.Where(x => x.idEmpresa == idEmpresa && x.idAgrupacion == idAgrupacion).ToList();
                }

                if (requerimientoID > 0)
                {
                    evidencias = evidencias.Where(x => x.requerimientoID == requerimientoID).ToList();
                }

                evidencias = evidencias.Where(x => x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date).ToList();

                switch (estatus)
                {
                    case 1: //Evaluadas
                        evidencias = evidencias.Where(x => x.usuarioEvaluadorID > 0).ToList();
                        break;
                    case 2: //No Evaluadas
                        evidencias = evidencias.Where(x => x.usuarioEvaluadorID == 0).ToList();
                        break;
                    default: //Todas
                        break;
                }
                #endregion

                var data = evidencias.Select(x => new
                {
                    id = x.id,
                    idAgrupacion = x.idAgrupacion,
                    idEmpresa = x.idEmpresa,
                    proyecto = x.idEmpresa < 1000 ?
                        (listaAgrupacionesCC.FirstOrDefault(m => m.id == x.idAgrupacion) != null ? listaAgrupacionesCC.FirstOrDefault(m => m.id == x.idAgrupacion).nomAgrupacion : "") :
                        x.idEmpresa == 1000 ?
                        (listaContratistas.FirstOrDefault(m => m.id == x.idAgrupacion) != null ? listaContratistas.FirstOrDefault(m => m.id == x.idAgrupacion).nombreEmpresa : "") :
                        x.idEmpresa == 2000 ?
                        (listaAgrupacionesContratistas.FirstOrDefault(m => m.id == x.idAgrupacion) != null ? listaAgrupacionesContratistas.FirstOrDefault(m => m.id == x.idAgrupacion).nomAgrupacion : "") :
                        "",
                    requerimientoID = x.requerimientoID,
                    requerimiento = requerimientos.FirstOrDefault(y => y.id == x.requerimientoID).requerimiento,
                    requerimientoDesc = requerimientos.Where(y => y.id == x.requerimientoID).Select(z => string.Format(@"{0} {1}", z.requerimiento, z.descripcion)).FirstOrDefault(),
                    requerimientoClasificacion = requerimientos.FirstOrDefault(y => y.id == x.requerimientoID).clasificacion,
                    requerimientoClasificacionDesc = requerimientos.FirstOrDefault(y => y.id == x.requerimientoID).clasificacion.GetDescription(),
                    puntoID = x.puntoID,
                    indice = puntos.FirstOrDefault(y => y.id == x.puntoID).indice,
                    puntoDesc = puntos.Where(y => y.id == x.puntoID).Select(z => string.Format(@"{0} {1}", z.indice, z.descripcion)).FirstOrDefault(),
                    fechaPunto = x.fechaPunto,
                    fechaPuntoString = x.fechaPunto.ToShortDateString(),
                    rutaEvidencia = x.rutaEvidencia,
                    comentariosCaptura = x.comentariosCaptura,
                    fechaCaptura = x.fechaCaptura,
                    fechaCapturaString = x.fechaCaptura.ToShortDateString(),
                    usuarioCapturaID = x.usuarioCapturaID,
                    usuarioCapturaDesc = usuarios.Where(y => y.id == x.usuarioCapturaID).Select(z => string.Format(@"{0} {1} {2}", z.nombre, z.apellidoPaterno, z.apellidoMaterno)).FirstOrDefault(),
                    usuarioEvaluadorID = x.usuarioEvaluadorID,
                    usuarioEvaluadorDesc = usuarios.Where(y => y.id == x.usuarioEvaluadorID).Select(z => string.Format(@"{0} {1} {2}", z.nombre, z.apellidoPaterno, z.apellidoMaterno)).FirstOrDefault(),
                    comentariosEvaluador = x.comentariosEvaluador,
                    porcentaje = puntos.Where(y => y.id == x.puntoID).Select(z => z.porcentaje).FirstOrDefault(),
                    aprobado = x.aprobado,
                    calificacion = x.calificacion,
                    fechaEvaluacion = x.fechaEvaluacion,
                    fechaEvaluacionString = x.fechaEvaluacion != null ? ((DateTime)x.fechaEvaluacion).ToShortDateString() : "",
                    codigo = puntos.Where(y => y.id == x.puntoID).Select(y => y.codigo).FirstOrDefault()
                }).ToList();

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarEvaluacion(List<EvidenciaDTO> evaluacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuario = vSesiones.sesionUsuarioDTO;

                    foreach (var eva in evaluacion)
                    {
                        var evidencia = _context.tblS_Req_Evidencia.FirstOrDefault(x => x.estatus && x.id == eva.id && x.division == divisionActual);

                        if (evidencia != null)
                        {
                            evidencia.usuarioEvaluadorID = usuario.id;
                            evidencia.comentariosEvaluador = eva.comentariosEvaluador ?? "";
                            evidencia.aprobado = eva.aprobado;
                            evidencia.calificacion = eva.calificacion;
                            evidencia.fechaEvaluacion = DateTime.Now;
                            evidencia.division = divisionActual;

                            _context.SaveChanges();

                            if (!evidencia.estatus)
                            {
                                LogError(0, 0, "RequerimientosController", "guardarEvaluacion - Se cambió el estatus de una evidencia evaluada.", null, AccionEnum.ACTUALIZAR, 0, eva);
                            }

                            SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(new { funcion = "guardarEvaluacion", eva = eva }));
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información de la evidencia.");
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public bool cargarExcelRequerimientosMasivo(HttpPostedFileBase archivo)
        {
            List<List<string>> tabla = new List<List<string>>();

            #region Convertir Archivo a Arreglo de bytes.
            byte[] data;

            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;

                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }

                data = memoryStream.ToArray();
            }
            #endregion

            #region Leer Arreglo de bytes.
            using (MemoryStream stream = new MemoryStream(data))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    //loop all rows
                    for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                    {
                        List<string> fila = new List<string>();

                        //loop all columns in a row
                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {
                            //add the cell data to the List
                            if (worksheet.Cells[i, j].Value != null)
                            {
                                fila.Add(worksheet.Cells[i, j].Value.ToString());
                            }
                            else
                            {
                                fila.Add("");
                            }
                        }

                        if (i > 1 && fila[0] != "")
                        {
                            tabla.Add(fila);
                        }
                    }
                }
            }
            #endregion

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var fila in tabla)
                    {
                        var requerimiento = fila[0];
                        var clasificacion = fila[1];
                        var titulo = fila[2];
                        var indice = fila[3];
                        var actividad = fila[4];
                        var condicionante = fila[5];
                        var seccion = fila[6];
                        var codigo = fila[7];
                        var descripcion = fila[8];
                        var verificacion = fila[9];
                        var porcentaje = Convert.ToDecimal(fila[10], CultureInfo.InvariantCulture);
                        var periodicidad = fila[11];
                        var area = fila[12];

                        #region Clasificación
                        ClasificacionEnum clasificacionEnum = new ClasificacionEnum();

                        foreach (var e in Enum.GetValues(typeof(ClasificacionEnum)).Cast<ClasificacionEnum>().ToList())
                        {
                            if (clasificacion == e.GetDescription())
                            {
                                clasificacionEnum = e;
                            }
                        }

                        if (clasificacionEnum == 0)
                        {
                            throw new Exception("No se encuentra la clasificación \"" + clasificacion + "\".");
                        }
                        #endregion

                        #region Requerimiento
                        var requerimientoSIGOPLAN = _context.tblS_Req_Requerimiento.FirstOrDefault(x => x.estatus && x.requerimiento == requerimiento && x.division == divisionActual);

                        if (requerimientoSIGOPLAN == null)
                        {
                            tblS_Req_Requerimiento nuevoRequerimiento = new tblS_Req_Requerimiento();

                            nuevoRequerimiento.requerimiento = requerimiento;
                            nuevoRequerimiento.descripcion = titulo;
                            nuevoRequerimiento.clasificacion = clasificacionEnum;
                            nuevoRequerimiento.fechaCreacion = DateTime.Now;
                            nuevoRequerimiento.division = divisionActual;
                            nuevoRequerimiento.estatus = true;

                            _context.tblS_Req_Requerimiento.Add(nuevoRequerimiento);
                            _context.SaveChanges();

                            requerimientoSIGOPLAN = nuevoRequerimiento;
                        }
                        #endregion

                        #region Actividad
                        var actividadSIGOPLAN = _context.tblS_Req_Actividad.FirstOrDefault(x => x.estatus && x.descripcion == actividad && x.division == divisionActual);

                        if (actividadSIGOPLAN == null)
                        {
                            tblS_Req_Actividad nuevaActividad = new tblS_Req_Actividad();

                            nuevaActividad.descripcion = actividad;
                            nuevaActividad.fechaCreacion = DateTime.Now;
                            nuevaActividad.division = divisionActual;
                            nuevaActividad.estatus = true;

                            _context.tblS_Req_Actividad.Add(nuevaActividad);
                            _context.SaveChanges();

                            actividadSIGOPLAN = nuevaActividad;
                        }
                        #endregion

                        #region Condicionante
                        var condicionanteSIGOPLAN = _context.tblS_Req_Condicionante.FirstOrDefault(x => x.estatus && x.descripcion == condicionante && x.division == divisionActual);

                        if (condicionanteSIGOPLAN == null)
                        {
                            tblS_Req_Condicionante nuevaCondicionante = new tblS_Req_Condicionante();

                            nuevaCondicionante.descripcion = condicionante;
                            nuevaCondicionante.fechaCreacion = DateTime.Now;
                            nuevaCondicionante.division = divisionActual;
                            nuevaCondicionante.estatus = true;

                            _context.tblS_Req_Condicionante.Add(nuevaCondicionante);
                            _context.SaveChanges();

                            condicionanteSIGOPLAN = nuevaCondicionante;
                        }
                        #endregion

                        #region Sección
                        var seccionSIGOPLAN = _context.tblS_Req_Seccion.FirstOrDefault(x => x.estatus && x.descripcion == seccion && x.division == divisionActual);

                        if (seccionSIGOPLAN == null)
                        {
                            tblS_Req_Seccion nuevaSeccion = new tblS_Req_Seccion();

                            nuevaSeccion.descripcion = seccion;
                            nuevaSeccion.fechaCreacion = DateTime.Now;
                            nuevaSeccion.division = divisionActual;
                            nuevaSeccion.estatus = true;

                            _context.tblS_Req_Seccion.Add(nuevaSeccion);
                            _context.SaveChanges();

                            seccionSIGOPLAN = nuevaSeccion;
                        }
                        #endregion

                        #region Área
                        //var areaEnum = Enum.GetValues(typeof(AreaEnum)).Cast<AreaEnum>().FirstOrDefault(v => v.GetDescription() == area);
                        var registroAreaAC = _context.tblSAC_Departamentos.FirstOrDefault(x => x.estatus && x.descripcion == area);
                        var area_id = 0;

                        if (registroAreaAC != null)
                        {
                            area_id = registroAreaAC.id;
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del área \"" + area + "\".");
                        }
                        #endregion

                        var puntoSIGOPLAN = _context.tblS_Req_Punto.FirstOrDefault(x => x.estatus && x.indice == indice && x.requerimientoID == requerimientoSIGOPLAN.id && x.division == divisionActual);

                        if (puntoSIGOPLAN == null)
                        {
                            VerificacionEnum enumClasificacion = Enum.GetValues(typeof(VerificacionEnum)).Cast<VerificacionEnum>().FirstOrDefault(x => x.GetDescription() == verificacion);

                            _context.tblS_Req_Punto.Add(new tblS_Req_Punto
                            {
                                descripcion = descripcion,
                                verificacion = enumClasificacion,
                                porcentaje = porcentaje,
                                fechaCreacion = DateTime.Now,
                                indice = indice,
                                periodicidad = (PeriodicidadRequerimientoEnum)Enum.Parse(typeof(PeriodicidadRequerimientoEnum), periodicidad),
                                actividad = actividadSIGOPLAN.id,
                                condicionante = condicionanteSIGOPLAN.id,
                                seccion = seccionSIGOPLAN.id,
                                codigo = codigo,
                                area = area_id, //area = areaEnum,
                                requerimientoID = requerimientoSIGOPLAN.id,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbSigoplanTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();

                    throw new Exception(e.Message);
                }
            }

            return true;
        }

        public Dictionary<string, object> cargarDashboard(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.id == usuario.id);

                var fechaInicioPrimerDia = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
                var fechaFinUltimoDia = new DateTime(fechaFin.Year, fechaFin.Month, 1).AddMonths(1).AddDays(-1);

                var evidencias = _context.tblS_Req_Evidencia.ToList().Where(x =>
                    x.estatus && x.fechaPunto.Date >= fechaInicioPrimerDia.Date && x.fechaPunto.Date <= fechaFinUltimoDia.Date
                ).ToList();
                var grupos = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();

                #region Filtrar por division y lineas de negocios
                if (listaDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                    evidencias = evidencias.Join(
                        listaCentrosCostoDivision,
                        e => new { e.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (e, cd) => new { e, cd }
                    ).Select(x => x.e).ToList();
                }

                if (listaLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    evidencias = evidencias.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        e => new { e.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (e, cd) => new { e, cd }
                    ).Select(x => x.e).ToList();
                }
                #endregion

                #region Filtros
                if (arrGrupos != null && arrGrupos.Count() > 0)
                {
                    evidencias = evidencias.Where(x => arrGrupos.Any(y => y.idAgrupacion == x.idAgrupacion)).ToList();
                }

                if (listaRequerimientos != null && listaRequerimientos.Count() > 0)
                {
                    evidencias = evidencias.Where(x => listaRequerimientos.Contains(x.requerimientoID)).ToList();
                }
                #endregion

                var puntos = _context.tblS_Req_Punto.ToList();

                var data = (from evi in evidencias
                            join pun in puntos on evi.puntoID equals pun.id
                            select new EvidenciaDTO
                            {
                                id = evi.id,
                                cc = evi.cc,
                                idEmpresa = evi.idEmpresa,
                                idAgrupacion = (int)evi.idAgrupacion,
                                requerimientoID = evi.requerimientoID,
                                puntoID = evi.puntoID,
                                fechaPunto = evi.fechaPunto,
                                fechaCaptura = evi.fechaCaptura,
                                aprobado = evi.aprobado,
                                calificacion = evi.calificacion,
                                porcentajePunto = pun.porcentaje,
                                periodicidad = pun.periodicidad,
                                actividad = pun.actividad,
                                condicionante = pun.condicionante,
                                seccion = pun.seccion
                            }).ToList();

                var chartGeneral = ObtenerChartGeneral(listaDivisiones, listaLineasNegocio, data, arrGrupos, listaRequerimientos, fechaInicio, fechaFin);
                var chartRequerimientos = ObtenerChartRequerimientos(listaDivisiones, listaLineasNegocio, data, arrGrupos, listaRequerimientos, fechaInicio, fechaFin);
                var chartSecciones = ObtenerChartSecciones(listaDivisiones, listaLineasNegocio, data, arrGrupos, listaRequerimientos, fechaInicio, fechaFin);

                resultado.Add("chartGeneral", chartGeneral);
                resultado.Add("chartRequerimientos", chartRequerimientos);
                resultado.Add("chartSecciones", chartSecciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("data", null);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private ChartDataDTO ObtenerChartGeneral(List<int> listaDivisiones, List<int> listaLineasNegocio, List<EvidenciaDTO> data, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin)
        {
            var asignaciones = (from asig in _context.tblS_Req_Asignacion.ToList()
                                join pun in _context.tblS_Req_Punto.ToList() on asig.puntoID equals pun.id
                                join req in _context.tblS_Req_Requerimiento.ToList() on pun.requerimientoID equals req.id
                                where
                                    asig.estatus && arrGrupos.Any(y => y.idAgrupacion == asig.idAgrupacion) && asig.fechaInicioEvaluacion.Date <= fechaFin.Date &&
                                    (listaRequerimientos != null && listaRequerimientos.Count() > 0 ? listaRequerimientos.Contains(req.id) : true)
                                select new
                                {
                                    id = asig.id,
                                    cc = asig.cc,
                                    idEmpresa = asig.idEmpresa,
                                    idAgrupacion = asig.idAgrupacion,
                                    puntoID = asig.puntoID,
                                    fechaAsignacion = asig.fechaAsignacion,
                                    fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                    periodicidadPunto = pun.periodicidad,
                                    porcentajePunto = pun.porcentaje,
                                    requerimientoID = pun.requerimientoID,
                                    actividad = pun.actividad,
                                    condicionante = pun.condicionante,
                                    seccion = pun.seccion,
                                    clasificacion = req.clasificacion
                                }).ToList();

            #region Filtrar por division y lineas de negocios
            if (listaDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivision,
                    a => new { a.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }

            if (listaLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    a => new { a.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }
            #endregion

            var grupos = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
            var labelsCC = data.Select(x => new { idAgrupacion = x.idAgrupacion }).Distinct().ToList();
            List<decimal> datosCC = new List<decimal>();

            int cantidadMesesResultados = (((fechaFin.Year - fechaInicio.Year) * 12) + fechaFin.Month - fechaInicio.Month) + 1;

            foreach (var labelCC in arrGrupos)
            {
                var asignacionCC = asignaciones.Where(x => x.idAgrupacion == labelCC.idAgrupacion).ToList();
                List<decimal> listaCumplimientoCC = new List<decimal>();

                foreach (var asig in asignacionCC)
                {
                    int cantidadEvidenciaNecesaria = 0;

                    switch (asig.periodicidadPunto)
                    {
                        case PeriodicidadRequerimientoEnum.Mensual:
                            cantidadEvidenciaNecesaria = cantidadMesesResultados;
                            break;
                        case PeriodicidadRequerimientoEnum.Trimestral:
                            cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 3));
                            break;
                        case PeriodicidadRequerimientoEnum.Semestral:
                            cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 6));
                            break;
                        case PeriodicidadRequerimientoEnum.Anual:
                            cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 12));
                            break;
                    }

                    decimal cumplimiento100porciento = asig.porcentajePunto * cantidadEvidenciaNecesaria;
                    decimal cumplimientoCapturado = data.Where(x => x.idAgrupacion == labelCC.idAgrupacion && x.puntoID == asig.puntoID && x.aprobado).Select(x => x.porcentajePunto).Sum();
                    decimal cumplimientoReal = cumplimiento100porciento > 0 ? ((cumplimientoCapturado * 100) / cumplimiento100porciento) : 0;

                    cumplimientoReal = cumplimientoReal > 100 ? 100 : cumplimientoReal; //Validación para que el porcentaje de cumplimiento no sobrepase el 100%.

                    listaCumplimientoCC.Add(cumplimientoReal);
                }

                datosCC.Add(listaCumplimientoCC.Count() > 0 ? Convert.ToDecimal(listaCumplimientoCC.Average().ToString("0.##"), CultureInfo.InvariantCulture) : 0);
            }

            List<string> labelsCCDesc = new List<string>();
            List<tblS_IncidentesEmpresasContratistas> lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
            List<tblS_IncidentesAgrupacionContratistas> lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
            foreach (var cc in arrGrupos)
            {
                labelsCCDesc.Add(
                    cc.idEmpresa == 0 ? grupos.FirstOrDefault(x => x.id == cc.idAgrupacion).nomAgrupacion :
                    cc.idEmpresa == 1000 ? lstContratistas.Where(s => s.id == cc.idAgrupacion).Select(s => s.nombreEmpresa).First() :
                    lstAgrupacionContratistas.Where(s => s.id == cc.idAgrupacion).Select(s => s.nomAgrupacion).FirstOrDefault());
            }

            List<string> listaColores = getListaColores(arrGrupos.Count());

            return new ChartDataDTO
            {
                labels = labelsCCDesc,
                datasets = new List<DatasetDTO>{ new DatasetDTO
                    {
                        backgroundColor = listaColores,//backgroundColor = dataPorCC.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                        borderColor = listaColores, //borderColor = dataPorCC.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                        borderWidth = 2,
                        fill = true,
                        data = datosCC
                    }
                }
            };
        }

        private ChartDataDTO ObtenerChartRequerimientos(List<int> listaDivisiones, List<int> listaLineasNegocio, List<EvidenciaDTO> data, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin)
        {
            var asignaciones = (from asig in _context.tblS_Req_Asignacion.ToList()
                                join pun in _context.tblS_Req_Punto.ToList() on asig.puntoID equals pun.id
                                join req in _context.tblS_Req_Requerimiento.ToList() on pun.requerimientoID equals req.id
                                where
                                    asig.estatus && arrGrupos.Any(y => y.idAgrupacion == asig.idAgrupacion) && asig.fechaInicioEvaluacion.Date <= fechaFin.Date &&
                                    (listaRequerimientos != null && listaRequerimientos.Count() > 0 ? listaRequerimientos.Contains(req.id) : true)
                                select new
                                {
                                    id = asig.id,
                                    cc = asig.cc,
                                    idEmpresa = asig.idEmpresa,
                                    idAgrupacion = asig.idAgrupacion,
                                    puntoID = asig.puntoID,
                                    fechaAsignacion = asig.fechaAsignacion,
                                    fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                    periodicidadPunto = pun.periodicidad,
                                    porcentajePunto = pun.porcentaje,
                                    requerimientoID = pun.requerimientoID,
                                    requerimiento = req.requerimiento,
                                    actividad = pun.actividad,
                                    condicionante = pun.condicionante,
                                    seccion = pun.seccion,
                                    clasificacion = req.clasificacion
                                }).ToList();

            #region Filtrar por division y lineas de negocios
            if (listaDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivision,
                    a => new { a.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }

            if (listaLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    a => new { a.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }
            #endregion

            var grupos = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();

            //var requerimientosSIGOPLAN = _context.tblS_Req_Requerimiento.Where(x => x.division == division).ToList();
            List<int> requerimientos = data.Select(x => x.requerimientoID).Distinct().ToList();
            List<string> requerimientosAsignados = asignaciones.Select(x => x.requerimiento).Distinct().ToList();
            List<string> requerimientosFiltro = new List<string>();

            if (listaRequerimientos != null && listaRequerimientos.Count() > 0)
            {
                requerimientosFiltro = _context.tblS_Req_Requerimiento.ToList().Where(x => listaRequerimientos.Contains(x.id)).Select(x => x.requerimiento).ToList();
            }

            var chartData = new ChartDataDTO
            {
                labels = (listaRequerimientos != null && listaRequerimientos.Count() > 0 ? requerimientosFiltro : requerimientosAsignados)
                    //requerimientosAsignados //requerimientosSIGOPLAN.Where(x => x.estatus).Select(x => x.requerimiento).ToList()
                ,
                datasets = new List<DatasetDTO>()
            };

            List<string> listaColores = getListaColores(arrGrupos.Count());
            int index = 0;
            foreach (var cc in arrGrupos)
            {
                List<decimal> listaPorcentajes = new List<decimal>();
                int cantidadMesesResultados = (((fechaFin.Year - fechaInicio.Year) * 12) + fechaFin.Month - fechaInicio.Month) + 1;
                List<string> listaColoresCC = new List<string>();

                foreach (var req in (listaRequerimientos != null && listaRequerimientos.Count() > 0 ? listaRequerimientos : requerimientos))
                {
                    var asignacionCC = asignaciones.Where(x => x.idAgrupacion == cc.idAgrupacion && x.requerimientoID == req).ToList();
                    List<decimal> listaCumplimientoCC = new List<decimal>();

                    foreach (var asig in asignacionCC)
                    {
                        int cantidadEvidenciaNecesaria = 0;

                        switch (asig.periodicidadPunto)
                        {
                            case PeriodicidadRequerimientoEnum.Mensual:
                                cantidadEvidenciaNecesaria = cantidadMesesResultados;
                                break;
                            case PeriodicidadRequerimientoEnum.Trimestral:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 3));
                                break;
                            case PeriodicidadRequerimientoEnum.Semestral:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 6));
                                break;
                            case PeriodicidadRequerimientoEnum.Anual:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 12));
                                break;
                        }

                        decimal cumplimiento100porciento = asig.porcentajePunto * cantidadEvidenciaNecesaria;
                        decimal cumplimientoCapturado = data.Where(x => x.idAgrupacion == cc.idAgrupacion && x.puntoID == asig.puntoID && x.aprobado).Select(x => x.porcentajePunto).Sum();
                        decimal cumplimientoReal = cumplimiento100porciento > 0 ? ((cumplimientoCapturado * 100) / cumplimiento100porciento) : 0;

                        cumplimientoReal = cumplimientoReal > 100 ? 100 : cumplimientoReal; //Validación para que el porcentaje de cumplimiento no sobrepase el 100%.

                        listaCumplimientoCC.Add(cumplimientoReal);
                    }

                    //var dataFiltrada = data.Where(x => x.requerimientoID == req && x.cc == cc).ToList();
                    listaPorcentajes.Add(listaCumplimientoCC.Count() > 0 ? Convert.ToDecimal(listaCumplimientoCC.Average().ToString("0.##"), CultureInfo.InvariantCulture) : 0);
                    listaColoresCC.Add(listaColores[index]);
                }

                List<tblS_IncidentesEmpresasContratistas> lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                List<tblS_IncidentesAgrupacionContratistas> lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
                var dataSet = new DatasetDTO
                {
                    label =
                        cc.idEmpresa == 0 ? grupos.FirstOrDefault(y => y.id == (int)cc.idAgrupacion).nomAgrupacion :
                        cc.idEmpresa == 1000 ? lstContratistas.FirstOrDefault(y => y.id == (int)cc.idAgrupacion).nombreEmpresa :
                        lstAgrupacionContratistas.FirstOrDefault(y => y.id == (int)cc.idAgrupacion).nomAgrupacion,
                    backgroundColor = listaColoresCC,
                    borderColor = listaColoresCC,
                    borderWidth = 2,
                    fill = true,
                    data = listaPorcentajes
                };

                chartData.datasets.Add(dataSet);
                index++;
            }

            return chartData;
        }

        private ChartDataDTO ObtenerChartSecciones(List<int> listaDivisiones, List<int> listaLineasNegocio, List<EvidenciaDTO> data, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin)
        {
            var asignaciones = (from asig in _context.tblS_Req_Asignacion.ToList()
                                join pun in _context.tblS_Req_Punto.ToList() on asig.puntoID equals pun.id
                                join req in _context.tblS_Req_Requerimiento.ToList() on pun.requerimientoID equals req.id
                                where
                                    asig.estatus && arrGrupos.Any(y => y.idAgrupacion == asig.idAgrupacion) && asig.fechaInicioEvaluacion.Date <= fechaFin.Date &&
                                    (listaRequerimientos != null && listaRequerimientos.Count() > 0 ? listaRequerimientos.Contains(req.id) : true)
                                select new
                                {
                                    id = asig.id,
                                    cc = asig.cc,
                                    idEmpresa = asig.idEmpresa,
                                    idAgrupacion = asig.idAgrupacion,
                                    puntoID = asig.puntoID,
                                    fechaAsignacion = asig.fechaAsignacion,
                                    fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                    periodicidadPunto = pun.periodicidad,
                                    porcentajePunto = pun.porcentaje,
                                    requerimientoID = pun.requerimientoID,
                                    actividad = pun.actividad,
                                    condicionante = pun.condicionante,
                                    seccion = pun.seccion,
                                    clasificacion = req.clasificacion
                                }).ToList();

            #region Filtrar por division y lineas de negocios
            if (listaDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivision,
                    a => new { a.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }

            if (listaLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    a => new { a.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }
            #endregion

            var grupos = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
            var seccionesSIGOPLAN = _context.tblS_Req_Seccion.Where(x => x.estatus).ToList().Where(x => listaDivisiones != null ? x.division == listaDivisiones.First() : true).ToList();

            List<int> secciones = data.Select(x => x.seccion).Distinct().ToList();

            var chartData = new ChartDataDTO { labels = seccionesSIGOPLAN.Where(x => x.estatus).Select(x => x.descripcion).ToList(), datasets = new List<DatasetDTO>() };

            List<string> listaColores = getListaColores(seccionesSIGOPLAN.Count());
            int index = 0;
            foreach (var cc in arrGrupos)
            {
                List<decimal> listaPorcentajes = new List<decimal>();
                int cantidadMesesResultados = (((fechaFin.Year - fechaInicio.Year) * 12) + fechaFin.Month - fechaInicio.Month) + 1;
                List<string> listaColoresCC = new List<string>();

                foreach (var sec in seccionesSIGOPLAN)
                {
                    var asignacionCC = asignaciones.Where(x => x.idAgrupacion == cc.idAgrupacion && x.seccion == sec.id).ToList();
                    List<decimal> listaCumplimientoCC = new List<decimal>();

                    foreach (var asig in asignacionCC)
                    {
                        int cantidadEvidenciaNecesaria = 0;

                        switch (asig.periodicidadPunto)
                        {
                            case PeriodicidadRequerimientoEnum.Mensual:
                                cantidadEvidenciaNecesaria = cantidadMesesResultados;
                                break;
                            case PeriodicidadRequerimientoEnum.Trimestral:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 3));
                                break;
                            case PeriodicidadRequerimientoEnum.Semestral:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 6));
                                break;
                            case PeriodicidadRequerimientoEnum.Anual:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 12));
                                break;
                        }

                        decimal cumplimiento100porciento = asig.porcentajePunto * cantidadEvidenciaNecesaria;
                        decimal cumplimientoCapturado = data.Where(x => x.idAgrupacion == cc.idAgrupacion && x.puntoID == asig.puntoID && x.aprobado).Select(x => x.porcentajePunto).Sum();
                        decimal cumplimientoReal = cumplimiento100porciento > 0 ? ((cumplimientoCapturado * 100) / cumplimiento100porciento) : 0;

                        cumplimientoReal = cumplimientoReal > 100 ? 100 : cumplimientoReal; //Validación para que el porcentaje de cumplimiento no sobrepase el 100%.

                        listaCumplimientoCC.Add(cumplimientoReal);
                    }

                    var dataFiltrada = data.Where(x => x.seccion == sec.id && x.idAgrupacion == cc.idAgrupacion).ToList();
                    listaPorcentajes.Add(listaCumplimientoCC.Count() > 0 ? Convert.ToDecimal(listaCumplimientoCC.Average().ToString("0.##"), CultureInfo.InvariantCulture) : 0);
                    listaColoresCC.Add(listaColores[index]);
                }

                List<tblS_IncidentesEmpresasContratistas> lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                List<tblS_IncidentesAgrupacionContratistas> lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
                var dataSet = new DatasetDTO
                {
                    label =
                        cc.idEmpresa == 0 ? grupos.FirstOrDefault(y => y.id == (int)cc.idAgrupacion).nomAgrupacion :
                        cc.idEmpresa == 1000 ? lstContratistas.FirstOrDefault(y => y.id == (int)cc.idAgrupacion).nombreEmpresa :
                        lstAgrupacionContratistas.FirstOrDefault(y => y.id == (int)cc.idAgrupacion).nomAgrupacion,
                    backgroundColor = listaColoresCC,
                    borderColor = listaColoresCC,
                    borderWidth = 2,
                    fill = true,
                    data = listaPorcentajes
                };

                chartData.datasets.Add(dataSet);
                index++;
            }

            return chartData;
        }

        public Dictionary<string, object> cargarDashboardClasificacion(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<ClasificacionEnum> listaClasificaciones, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.id == usuario.id);
                var evidencias = _context.tblS_Req_Evidencia.ToList().Where(x =>
                    x.estatus && x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date
                ).ToList();

                #region Filtrar por division y lineas de negocios
                if (listaDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                    evidencias = evidencias.Join(
                        listaCentrosCostoDivision,
                        e => new { e.idEmpresa, e.idAgrupacion },
                        cd => new { cd.idEmpresa, cd.idAgrupacion },
                        (e, cd) => new { e, cd }
                    ).Select(x => x.e).ToList();
                }

                if (listaLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    evidencias = evidencias.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        e => new { e.idEmpresa, e.idAgrupacion },
                        cd => new { cd.idEmpresa, cd.idAgrupacion },
                        (e, cd) => new { e, cd }
                    ).Select(x => x.e).ToList();
                }
                #endregion

                #region Filtro Centros de Costo
                if (arrGrupos != null && arrGrupos.Count() > 0)
                {
                    evidencias = evidencias.Where(x => arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == x.idAgrupacion)).ToList();
                }
                #endregion

                var puntos = _context.tblS_Req_Punto.ToList();
                var requerimientos = _context.tblS_Req_Requerimiento.ToList();
                var data = (from evi in evidencias
                            join pun in puntos on evi.puntoID equals pun.id
                            join req in requerimientos on pun.requerimientoID equals req.id
                            select new EvidenciaDTO
                            {
                                id = evi.id,
                                cc = evi.cc,
                                idEmpresa = evi.idEmpresa,
                                idAgrupacion = (int)evi.idAgrupacion,
                                requerimientoID = evi.requerimientoID,
                                puntoID = evi.puntoID,
                                fechaPunto = evi.fechaPunto,
                                fechaCaptura = evi.fechaCaptura,
                                aprobado = evi.aprobado,
                                calificacion = evi.calificacion,
                                porcentajePunto = pun.porcentaje,
                                periodicidad = pun.periodicidad,
                                actividad = pun.actividad,
                                condicionante = pun.condicionante,
                                seccion = pun.seccion,
                                clasificacion = req.clasificacion,
                                clasificacionDesc = req.clasificacion.GetDescription()
                            }).ToList();

                var chartClasificacion = ObtenerChartClasificacion(listaDivisiones, listaLineasNegocio, data, arrGrupos, listaClasificaciones, fechaInicio, fechaFin);

                resultado.Add("chartClasificacion", chartClasificacion);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("data", null);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private ChartDataDTO ObtenerChartClasificacion(List<int> listaDivisiones, List<int> listaLineasNegocio, List<EvidenciaDTO> data, List<MultiSegDTO> arrGrupos, List<ClasificacionEnum> listaClasificaciones, DateTime fechaInicio, DateTime fechaFin)
        {
            var asignaciones = (from asig in _context.tblS_Req_Asignacion.ToList()
                                join pun in _context.tblS_Req_Punto.ToList() on asig.puntoID equals pun.id
                                join req in _context.tblS_Req_Requerimiento.ToList() on pun.requerimientoID equals req.id
                                where asig.estatus && arrGrupos.Any(y => y.idEmpresa == asig.idEmpresa && y.idAgrupacion == asig.idAgrupacion) && asig.fechaInicioEvaluacion.Date <= fechaFin.Date
                                select new
                                {
                                    id = asig.id,
                                    cc = asig.cc,
                                    idEmpresa = asig.idEmpresa,
                                    idAgrupacion = asig.idAgrupacion,
                                    puntoID = asig.puntoID,
                                    fechaAsignacion = asig.fechaAsignacion,
                                    fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                    periodicidadPunto = pun.periodicidad,
                                    porcentajePunto = pun.porcentaje,
                                    requerimientoID = pun.requerimientoID,
                                    actividad = pun.actividad,
                                    condicionante = pun.condicionante,
                                    seccion = pun.seccion,
                                    clasificacion = req.clasificacion
                                }).ToList();

            #region Filtrar por division y lineas de negocios
            if (listaDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivision,
                    a => new { a.idEmpresa, a.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }

            if (listaLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                asignaciones = asignaciones.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    a => new { a.idEmpresa, a.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (a, cd) => new { a, cd }
                ).Select(x => x.a).ToList();
            }
            #endregion

            var grupos = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
            List<tblS_IncidentesEmpresasContratistas> lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
            List<tblS_IncidentesAgrupacionContratistas> lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
            //List<int> clasificaciones = data.Select(x => (int)x.clasificacion).Distinct().ToList();
            var clasificacionesActuales = Enum.GetValues(typeof(ClasificacionEnum)).Cast<ClasificacionEnum>().ToList();

            if (listaClasificaciones != null)
            {
                clasificacionesActuales = clasificacionesActuales.Where(x => listaClasificaciones.Contains(x)).ToList();
            }

            var chartData = new ChartDataDTO { labels = clasificacionesActuales.Select(x => x.GetDescription()).ToList(), datasets = new List<DatasetDTO>() };

            List<string> listaColores = getListaColores(arrGrupos.Count());
            int index = 0;
            foreach (var cc in arrGrupos)
            {
                List<decimal> listaPorcentajes = new List<decimal>();
                int cantidadMesesResultados = (((fechaFin.Year - fechaInicio.Year) * 12) + fechaFin.Month - fechaInicio.Month) + 1;
                List<string> listaColoresCC = new List<string>();

                //foreach (var cla in clasificaciones)
                foreach (var cla in clasificacionesActuales)
                {
                    var asignacionCC = asignaciones.Where(x => x.idEmpresa == cc.idEmpresa && x.idAgrupacion == cc.idAgrupacion && x.clasificacion == cla).ToList();
                    List<decimal> listaCumplimientoCC = new List<decimal>();

                    foreach (var asig in asignacionCC)
                    {
                        int cantidadEvidenciaNecesaria = 0;

                        switch (asig.periodicidadPunto)
                        {
                            case PeriodicidadRequerimientoEnum.Mensual:
                                cantidadEvidenciaNecesaria = cantidadMesesResultados;
                                break;
                            case PeriodicidadRequerimientoEnum.Trimestral:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 3));
                                break;
                            case PeriodicidadRequerimientoEnum.Semestral:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 6));
                                break;
                            case PeriodicidadRequerimientoEnum.Anual:
                                cantidadEvidenciaNecesaria = Convert.ToInt32(Math.Ceiling((double)cantidadMesesResultados / 12));
                                break;
                        }

                        decimal cumplimiento100porciento = asig.porcentajePunto * cantidadEvidenciaNecesaria;
                        decimal cumplimientoCapturado = data.Where(x => x.idEmpresa == cc.idEmpresa && x.idAgrupacion == cc.idAgrupacion && x.puntoID == asig.puntoID && x.aprobado).Select(x => x.porcentajePunto).Sum();
                        decimal cumplimientoReal = cumplimiento100porciento > 0 ? ((cumplimientoCapturado * 100) / cumplimiento100porciento) : 0;

                        cumplimientoReal = cumplimientoReal > 100 ? 100 : cumplimientoReal; //Validación para que el porcentaje de cumplimiento no sobrepase el 100%.

                        listaCumplimientoCC.Add(cumplimientoReal);
                    }

                    var dataFiltrada = data.Where(x => x.clasificacion == cla && x.idEmpresa == cc.idEmpresa && x.idAgrupacion == cc.idAgrupacion).ToList();
                    listaPorcentajes.Add(listaCumplimientoCC.Count() > 0 ? Convert.ToDecimal(listaCumplimientoCC.Average().ToString("0.##"), CultureInfo.InvariantCulture) : 0);
                    listaColoresCC.Add(listaColores[index]);
                }

                var dataSet = new DatasetDTO
                {
                    label =
                        cc.idEmpresa == 0 ? grupos.FirstOrDefault(x => x.id == cc.idAgrupacion).nomAgrupacion :
                        cc.idEmpresa == 1000 ? lstContratistas.FirstOrDefault(x => x.id == cc.idAgrupacion).nombreEmpresa :
                        lstAgrupacionContratistas.FirstOrDefault(x => x.id == cc.idAgrupacion).nomAgrupacion,
                    backgroundColor = listaColoresCC, //dataPorClasificacion.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                    borderColor = listaColoresCC, //dataPorClasificacion.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                    borderWidth = 2,
                    fill = true,
                    data = listaPorcentajes
                };

                chartData.datasets.Add(dataSet);
                index++;
            }

            return chartData;
        }

        public Dictionary<string, object> getAsignacionCapturaAuditoria(int idEmpresa, int idAgrupacion, ClasificacionEnum clasificacion)
        {
            try
            {
                var centrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();

                var puntos = from asig in _context.tblS_Req_Asignacion.Where(x => x.division == divisionActual).ToList()
                             join pun in _context.tblS_Req_Punto.Where(x => x.division == divisionActual).ToList() on asig.puntoID equals pun.id
                             join req in _context.tblS_Req_Requerimiento.Where(x => x.division == divisionActual).ToList() on pun.requerimientoID equals req.id
                             where asig.estatus && (asig.idEmpresa == idEmpresa && asig.idAgrupacion == idAgrupacion) && ((int)clasificacion > 0 ? req.clasificacion == clasificacion : true)
                             select new
                             {
                                 id = pun.id,
                                 cc = asig.cc,
                                 ccDesc = centrosCosto.Where(y => y.cc == asig.cc).Select(z => z.cc + "-" + z.descripcion).FirstOrDefault(),
                                 requerimientoID = req.id,
                                 requerimiento = req.requerimiento,
                                 requerimientoDesc = string.Format(@"{0} {1}", req.requerimiento, req.descripcion),
                                 requerimientoClasificacion = req.clasificacion,
                                 requerimientoClasificacionDesc = req.clasificacion.GetDescription(),
                                 indice = pun.indice,
                                 puntoDesc = string.Format(@"{0} {1}", pun.indice, pun.descripcion),
                                 descripcion = pun.descripcion,
                                 asignacionID = asig.id,
                                 fechaAsignacion = asig.fechaAsignacion,
                                 fechaAsignacionString = asig.fechaAsignacion.ToShortDateString(),
                                 fechaInicioEvaluacion = asig.fechaInicioEvaluacion,
                                 fechaInicioEvaluacionString = asig.fechaInicioEvaluacion.ToShortDateString(),
                                 codigo = pun.codigo
                             };

                Random rand = new Random();
                int cantidadPuntosMuestreo = Convert.ToInt32(Math.Ceiling(puntos.Count() * 0.3)); //30% de la cantidad total de puntos asignados para el muestreo.

                puntos = puntos.OrderBy(x => rand.Next()).Take(cantidadPuntosMuestreo).ToList();

                resultado.Add("data", puntos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarEvidenciaAuditoria(List<EvidenciaDTO> captura, List<HttpPostedFileBase> evidencias)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuario = vSesiones.sesionUsuarioDTO;
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    foreach (var cap in captura)
                    {
                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidencia, evidencias[index].FileName);
                        string rutaArchivoEvidencia = Path.Combine(RutaEvidencia + " AUDITORIA", nombreArchivoEvidencia);
                        listaRutaArchivos.Add(Tuple.Create(evidencias[index], rutaArchivoEvidencia));

                        tblS_Req_Evidencia_Auditoria nuevaEvidencia = new tblS_Req_Evidencia_Auditoria();

                        nuevaEvidencia.cc = cap.cc;
                        nuevaEvidencia.requerimientoID = cap.requerimientoID;
                        nuevaEvidencia.puntoID = cap.puntoID;
                        nuevaEvidencia.fechaPunto = DateTime.Now;
                        nuevaEvidencia.rutaEvidencia = rutaArchivoEvidencia;
                        nuevaEvidencia.comentariosCaptura = cap.comentariosCaptura ?? "";
                        nuevaEvidencia.fechaCaptura = DateTime.Now;
                        nuevaEvidencia.usuarioCapturaID = usuario.id;
                        nuevaEvidencia.usuarioEvaluadorID = usuario.id;
                        nuevaEvidencia.comentariosEvaluador = "";
                        nuevaEvidencia.aprobado = cap.aprobado;
                        nuevaEvidencia.calificacion = cap.calificacion;
                        nuevaEvidencia.fechaEvaluacion = DateTime.Now;
                        nuevaEvidencia.division = divisionActual;
                        nuevaEvidencia.estatus = true;

                        _context.tblS_Req_Evidencia_Auditoria.Add(nuevaEvidencia);
                        _context.SaveChanges();

                        index++;
                    }

                    foreach (var archivo in listaRutaArchivos)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(archivo.Item1, archivo.Item2) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> guardarEvidenciaCargaMasiva(HttpPostedFileBase evidencias, DateTime fechaPuntos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuario = vSesiones.sesionUsuarioDTO;

                    using (ZipArchive archive = new ZipArchive(evidencias.InputStream))
                    {
                        var archivos = archive.Entries.Where(x => !string.IsNullOrEmpty(Path.GetExtension(x.FullName))).ToList(); //Filtro para quitar carpetas.
                        var asignaciones = _context.tblS_Req_Asignacion.Where(x => x.estatus && x.division == divisionActual).ToList();
                        var listaAgrupacion = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
                        var listaAgrupacionDet = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo).ToList().Where(x =>
                            listaAgrupacion.Select(y => y.id).Contains(x.idAgrupacionCC)
                        ).ToList();

                        #region Validación para verificar que todos los centros de costo estén agrupados.
                        List<string> listaCCNoAgrupados = new List<string>();

                        foreach (ZipArchiveEntry archivo in archivos)
                        {
                            var listaRuta = archivo.FullName.Split('/');
                            var centroCosto = listaRuta[0];

                            var existeAgrupacion = listaAgrupacionDet.FirstOrDefault(x => x.cc == centroCosto);

                            if (existeAgrupacion == null)
                            {
                                listaCCNoAgrupados.Add(centroCosto);
                            }
                        }

                        if (listaCCNoAgrupados.Count() > 0)
                        {
                            var stringListaCCNoAgrupados = string.Join(", ", listaCCNoAgrupados.Distinct().Select(x => "\"" + x + "\""));

                            throw new Exception("Los siguientes centros de costo no están agrupados: " + stringListaCCNoAgrupados);
                        }
                        #endregion

                        #region Validación para determinar que todos los puntos están asignados al centro de costo.
                        List<Tuple<string, string>> listaPuntosNoAsignadosPorCC = new List<Tuple<string, string>>();
                        var puntosSIGOPLAN = _context.tblS_Req_Punto.Where(x => x.estatus).ToList();

                        foreach (ZipArchiveEntry archivo in archivos)
                        {
                            var listaRuta = archivo.FullName.Split('/');
                            var centroCosto = listaRuta[0];
                            var nombreArchivoCodigo = Path.GetFileNameWithoutExtension(archivo.Name);
                            var agrupacion = listaAgrupacionDet.FirstOrDefault(x => x.cc == centroCosto);
                            var puntosAsignados = asignaciones.Where(x => x.idAgrupacion == agrupacion.idAgrupacionCC).Select(x => x.puntoID).ToList();
                            var puntosAsignadosPorCodigo = puntosSIGOPLAN.Where(x =>
                                x.codigo == nombreArchivoCodigo && puntosAsignados.Contains(x.id) && x.division == divisionActual
                            ).ToList();

                            if (puntosAsignadosPorCodigo.Count() == 0)
                            {
                                listaPuntosNoAsignadosPorCC.Add(new Tuple<string, string>(agrupacion.idAgrupacion.nomAgrupacion, nombreArchivoCodigo));
                            }
                        }

                        if (listaPuntosNoAsignadosPorCC.Count() > 0)
                        {
                            var stringListaPuntosNoAsignadosPorCC = string.Join(", ", listaPuntosNoAsignadosPorCC.Select(x => "[" + x.Item1 + " -> " + x.Item2 + "]"));

                            throw new Exception("No se encuentra la información en relación a los puntos asignados con sus centros de costo: " + stringListaPuntosNoAsignadosPorCC);
                        }
                        #endregion

                        foreach (ZipArchiveEntry archivo in archivos)
                        {
                            var listaRuta = archivo.FullName.Split('/');
                            var centroCosto = listaRuta[0];
                            var nombreArchivoCodigo = Path.GetFileNameWithoutExtension(archivo.Name);
                            var agrupacion = listaAgrupacionDet.FirstOrDefault(x => x.cc == centroCosto);
                            var puntosAsignados = asignaciones.Where(x => x.idAgrupacion == agrupacion.idAgrupacionCC).Select(x => x.puntoID).ToList();

                            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                            {
                                var centroCostoEK = _contextEnkontrol.Select<dynamic>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, new OdbcConsultaDTO()
                                {
                                    consulta = @"SELECT * FROM cc WHERE cc = ?",
                                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.Text, valor = centroCosto } }
                                });

                                if (centroCostoEK.Count() == 0)
                                {
                                    throw new Exception("No se encuentra el centro de costo \"" + centroCosto + "\".");
                                }
                            }

                            var puntosAsignadosPorCodigo = _context.tblS_Req_Punto.ToList().Where(x =>
                                x.estatus && x.codigo == nombreArchivoCodigo && puntosAsignados.Contains(x.id) && x.division == divisionActual
                            ).ToList();

                            if (puntosAsignadosPorCodigo.Count() == 0)
                            {
                                throw new Exception("No se encuentra la información de los puntos con el código \"" + nombreArchivoCodigo + "\" asignados al centro de costo \"" + agrupacion.idAgrupacion.nomAgrupacion + "\".");
                            }

                            var requerimientosSIGOPLAN = _context.tblS_Req_Requerimiento.ToList().Where(x =>
                                x.estatus && puntosAsignadosPorCodigo.GroupBy(z => z.requerimientoID).Select(y => y.Key).Contains(x.id) && x.division == divisionActual
                            ).ToList();

                            if (requerimientosSIGOPLAN.Count() == 0)
                            {
                                throw new Exception("No se encuentra la información de los requerimientos asignados al centro de costo \"" + agrupacion.idAgrupacion.nomAgrupacion + "\" con el código \"" + nombreArchivoCodigo + "\".");
                            }

                            string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidencia, archivo.Name);
                            string rutaArchivoEvidencia = Path.Combine(RutaEvidencia, nombreArchivoEvidencia);

                            #region Guardar el archivo
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

                            archivo.ExtractToFile(Path.Combine(rutaArchivoEvidencia));

                            if (File.Exists(rutaArchivoEvidencia) == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                return resultado;
                            }
                            #endregion

                            foreach (var pun in puntosAsignadosPorCodigo)
                            {
                                #region Registro nuevo de evidencia en la base de datos.
                                tblS_Req_Evidencia nuevaEvidencia = new tblS_Req_Evidencia();

                                nuevaEvidencia.cc = centroCosto;
                                nuevaEvidencia.requerimientoID = pun.requerimientoID;
                                nuevaEvidencia.puntoID = pun.id;
                                nuevaEvidencia.fechaPunto = fechaPuntos;
                                nuevaEvidencia.rutaEvidencia = rutaArchivoEvidencia;
                                nuevaEvidencia.comentariosCaptura = "";
                                nuevaEvidencia.fechaCaptura = DateTime.Now;
                                nuevaEvidencia.usuarioCapturaID = usuario.id;
                                nuevaEvidencia.usuarioEvaluadorID = 0;
                                nuevaEvidencia.comentariosEvaluador = "";
                                nuevaEvidencia.aprobado = false;
                                nuevaEvidencia.calificacion = 0;
                                nuevaEvidencia.fechaEvaluacion = null;
                                nuevaEvidencia.division = divisionActual;
                                nuevaEvidencia.estatus = true;
                                nuevaEvidencia.idEmpresa = 0;
                                nuevaEvidencia.idAgrupacion = agrupacion.idAgrupacionCC;

                                _context.tblS_Req_Evidencia.Add(nuevaEvidencia);
                                _context.SaveChanges();
                                #endregion
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        private string ObtenerColorGraficaAleatorio()
        {
            int r = RandomInteger(0, 255);
            int g = RandomInteger(0, 255);
            int b = RandomInteger(0, 255);
            float a = 0.6f; // Valor constante para colores definidos.

            return String.Format("rgba({0},{1},{2},{3})", r, g, b, a);
        }

        private List<string> getListaColores(int listaCount)
        {
            List<tblS_Req_Color> colores = _context.tblS_Req_Color.Where(x => x.estatus).ToList();
            List<string> listaColores = new List<string>();

            for (int i = 0; i < listaCount; i++)
            {
                if (listaColores.Count() == listaCount)
                {
                    break;
                }

                if (colores.ElementAtOrDefault(i) == null)
                {
                    i = 0; //Reset del contador para cuando se sobrepase la lista de colores.
                }

                listaColores.Add("rgb" + colores[i].rgb);
            }

            return listaColores;
        }

        private List<string> getListaColoresOrdenAleatorio(int listaCount)
        {
            List<tblS_Req_Color> colores = _context.tblS_Req_Color.Where(x => x.estatus).ToList();
            Random rng = new Random();
            colores = colores.OrderBy(a => rng.Next()).ToList();
            List<string> listaColores = new List<string>();

            for (int i = 0; i < listaCount; i++)
            {
                if (listaColores.Count() == listaCount)
                {
                    break;
                }

                if (colores.ElementAtOrDefault(i) == null)
                {
                    i = 0; //Reset del contador para cuando se sobrepase la lista de colores.
                }

                listaColores.Add("rgb" + colores[i].rgb);
            }

            return listaColores;
        }

        //string[] colores = new string[] 
        //    { 
        //        "rgb(255,102,0)",
        //        "rgb(128,128,128)",
        //        "rgb(146,208,80)",
        //        "rgb(0,112,192)",
        //        "rgb(102,0,102)",
        //        "rgb(153,102,0)",
        //        "rgb(255,0,102)",
        //        "rgb(0, 255, 140)",
        //        "rgb(255, 98, 0)",
        //        "rgb(119, 0, 255)"
        //    };

        private int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                new RNGCryptoServiceProvider().GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> getRequerimientosAsignacionCombo(List<int> clasificaciones)
        {
            try
            {
                var requerimientos = _context.tblS_Req_Requerimiento.ToList().Where(x =>
                    x.estatus &&
                    x.division == divisionActual &&
                    (clasificaciones != null && clasificaciones.Count() > 0 ? clasificaciones.Contains((int)x.clasificacion) : true)
                ).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = string.Format(@"{0} - {1}", x.requerimiento, x.descripcion)
                }).OrderBy(x => x.Text).ToList();

                return requerimientos;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> getActividadesAsignacionCombo(List<int> requerimientos)
        {
            try
            {
                var actividadesPuntos = _context.tblS_Req_Punto.ToList().Where(x =>
                    x.estatus && x.division == divisionActual && requerimientos.Contains(x.requerimientoID)
                ).Select(x => x.actividad).ToList();
                var actividades = _context.tblS_Req_Actividad.ToList().Where(x =>
                    x.estatus &&
                    x.division == divisionActual &&
                    (requerimientos != null && requerimientos.Count() > 0 ? actividadesPuntos.Contains(x.id) : true)
                ).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).OrderBy(x => x.Text).ToList();

                return actividades;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> getCondicionantesAsignacionCombo(List<int> requerimientos, List<int> actividades)
        {
            try
            {
                var condicionantesPuntos = _context.tblS_Req_Punto.ToList().Where(x =>
                    x.estatus &&
                    x.division == divisionActual &&
                    (requerimientos != null ? requerimientos.Contains(x.requerimientoID) : true) &&
                    (actividades != null ? actividades.Contains(x.actividad) : true)
                ).Select(x => x.condicionante).ToList();
                var condicionantes = _context.tblS_Req_Condicionante.ToList().Where(x =>
                    x.estatus &&
                    x.division == divisionActual &&
                    (requerimientos == null && actividades == null ? true : condicionantesPuntos.Contains(x.id))
                ).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).OrderBy(x => x.Text).ToList();

                return condicionantes;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> getSeccionesAsignacionCombo(List<int> requerimientos, List<int> actividades, List<int> condicionantes)
        {
            try
            {
                var seccionesPuntos = _context.tblS_Req_Punto.ToList().Where(x =>
                    x.estatus &&
                    x.division == divisionActual &&
                    (requerimientos != null ? requerimientos.Contains(x.requerimientoID) : true) &&
                    (actividades != null ? actividades.Contains(x.actividad) : true) &&
                    (condicionantes != null ? condicionantes.Contains(x.condicionante) : true)
                ).Select(x => x.seccion).ToList();
                var secciones = _context.tblS_Req_Seccion.ToList().Where(x =>
                    x.estatus &&
                    x.division == divisionActual &&
                    (requerimientos == null && actividades == null && condicionantes == null ? true : seccionesPuntos.Contains(x.id))
                ).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).OrderBy(x => x.Text).ToList();

                return secciones;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboDivision()
        {
            try
            {
                var divisiones = _context.tblS_Req_Division.Where(x => x.estatus).ToList().Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).ToList();

                return divisiones;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboRequerimientosDashboard(int division, List<string> listaCC)
        {
            try
            {
                if (division == 0)
                {
                    division = divisionActual;
                }

                var requerimientos = _context.tblS_Req_Requerimiento.Where(x => x.estatus && x.division == division).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.requerimiento + "-" + x.descripcion
                }).ToList();

                //var requerimientos = (from asig in _context.tblS_Req_Asignacion.ToList()
                //                      join pun in _context.tblS_Req_Punto.ToList() on asig.puntoID equals pun.id
                //                      join req in _context.tblS_Req_Requerimiento.ToList() on pun.requerimientoID equals req.id
                //                      where
                //                        asig.estatus && asig.division == division &&
                //                        pun.estatus && pun.division == division &&
                //                        req.estatus && req.division == division &&
                //                        (listaCC != null && listaCC.Count() > 0 ? listaCC.Contains(asig.cc) : true)
                //                      group new { requerimiento = req.requerimiento, descripcion = req.descripcion } by req.id into g
                //                      select new Core.DTO.Principal.Generales.ComboDTO
                //                      {
                //                          Value = g.Key.ToString(),
                //                          Text = g.First().requerimiento + "-" + g.First().descripcion
                //                      }).ToList();

                return requerimientos;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public Dictionary<string, object> getResponsables()
        {
            try
            {
                var responsables = _context.tblS_Req_EmpleadoAreaCC.Join(_context.tblP_Usuario,
                rel => rel.idUsuario, usu => usu.id, (rel, usu) => new
                {
                    rel = rel,
                    usu = usu
                }).Where(relUsu => relUsu.rel.estatus && relUsu.rel.division == divisionActual).GroupBy(x => x.rel.idUsuario).Select(grp => grp.FirstOrDefault()).ToList();
                var combo = responsables.Select(x => new ComboDTO
                {
                    Value = x.rel.idUsuario,
                    Text = string.Format(@"{0} {1} {2}", x.usu.nombre, x.usu.apellidoPaterno, x.usu.apellidoMaterno)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", responsables);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcPorDivision(int division)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"SELECT 
                                    c.cc AS Value, 
                                    (c.cc + '-' + c.descripcion) AS Text 
                                FROM cc c 
                                WHERE c.st_ppto != 'T' 
                                ORDER BY Value",
                    parametros = new List<OdbcParameterDTO>() { }
                };
                var resultado = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

                var listaRelacionCCDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && x.division == division).Select(x => x.cc).ToList();

                resultado = resultado.Where(x => listaRelacionCCDivision.Contains(x.Value)).ToList();

                return resultado;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public Dictionary<string, object> GetAreaCombo()
        {
            try
            {
                var listaAreasAC = _context.tblSAC_Departamentos.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.descripcion
                }).ToList();

                resultado.Add(ITEMS, listaAreasAC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
    }
}

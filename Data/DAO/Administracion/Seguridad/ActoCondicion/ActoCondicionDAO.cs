using Core.DAO.Administracion.Seguridad;
using Core.DAO.Administracion.Seguridad.ActoCondicion;
using Core.DAO.Principal.Archivos;
using Core.DTO;
using Core.DTO.Administracion.Seguridad;
using Core.DTO.Administracion.Seguridad.ActoCondicion;
using Core.DTO.Administracion.Seguridad.ActoCondicion.Acta;
using Core.DTO.Administracion.Seguridad.ActoCondicion.CargaZipActoCondicion;
using Core.DTO.Administracion.Seguridad.ActoCondicion.Graficas;
using Core.DTO.Administracion.Seguridad.ActoCondicion.Historial;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using Core.DTO.Principal.Generales;
using Core.DTO.Reportes;
using Core.DTO.Utils.ChartJS;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contratistas;
using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using Core.Entity.Principal.Alertas;
using Core.Enum.Administracion.Seguridad;
using Core.Enum.Administracion.Seguridad.ActoCondicion;
using Core.Enum.Administracion.Seguridad.Requerimientos;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Administracion.Seguridad.Incidencias;
using Data.Factory.Principal.Archivos;
using Infrastructure.Utils;
using MoreLinq.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

namespace Data.DAO.Administracion.Seguridad.ActoCondicion
{
    public class ActoCondicionDAO : GenericDAO<tblSAC_Acto>, IActoCondicionDAO
    {
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\ACTOS_CONDICIONES";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\ACTOS_CONDICIONES";

        private const string NombreBaseActo = @"Acto";
        private const string NombreBaseCondicion = @"Condicion";
        private const string NombreBaseImagenAntes = @"ImagenAntes";
        private const string NombreBaseImagenDespues = @"ImagenDespues";
        private const string NombreBaseEvidencia = @"Evidencia";
        private const string NombreBaseActa = @"Acta";

        private readonly string RutaActos;
        private readonly string RutaCondiciones;
        private readonly string RutaZip;

        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string NombreControlador = "ActoCondicionController";

        IDirArchivosDAO archivoFS = new ArchivoFactoryServices().getArchivo();
        ISeguridadIncidentesDAO incidenteFS = new SeguridadIncidentesFactoryService().getSeguridadIncidenteService();

        #region Constructor
        public ActoCondicionDAO()
        {
            resultado.Clear();
#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaActos = Path.Combine(RutaBase, "ACTOS");
            RutaCondiciones = Path.Combine(RutaBase, "CONDICIONES");
            RutaZip = Path.Combine(RutaBase, "ZIP");

        }
        #endregion

        #region Actos y Condiciones
        public Dictionary<string, object> ObtenerCentrosCostos()
        {
            try
            {
                var listaCC = _context.tblP_CC.Where(x => x.estatus).Select(x => new
                {
                    Value = x.cc,
                    Text = x.cc + " - " + x.descripcion.Trim()
                }).OrderBy(x => x.Text).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listaCC);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerCentrosCostos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerSupervisores()
        {
            try
            {
                var listaSupervisores = new List<ComboDTO>();

                var listaSupervisoresActos = _context.tblSAC_Acto
                    .Where(x => x.activo)
                    .Select(x => new ComboDTO
                    {
                        Value = x.claveSupervisor.ToString(),
                        Text = x.nombreSupervisor.ToUpper()
                    }).ToList();

                var listaSupervisoresCondiciones = _context.tblSAC_Condicion
                    .Where(x => x.activo)
                    .Select(x => new ComboDTO
                    {
                        Value = x.claveSupervisor.ToString(),
                        Text = x.nombreSupervisor.ToUpper()
                    }).ToList();

                listaSupervisores.AddRange(listaSupervisoresActos);
                listaSupervisores.AddRange(listaSupervisoresCondiciones);

                listaSupervisores = listaSupervisores.DistinctBy(x => x.Value).OrderBy(x => x.Text).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listaSupervisores);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerSupervisores", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerDepartamentos()
        {
            try
            {
                //var departamentos = _context.tblSAC_Departamentos
                //    .ToList()
                //    .Select(x => new ComboDTO
                //    {
                //        Value = x.id.ToString(),
                //        Text = x.descripcion.ToUpper()
                //    }).ToList();

                //resultado.Add(SUCCESS, true);
                //resultado.Add(ITEMS, departamentos);

                #region FILL COMBO DEPARTAMENTOS
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    List<ComboDTO> lstCboDepartamentos = _context.Select<ComboDTO>(new DapperDTO
                    {

                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT id AS Value, UPPER(descripcion) AS text FROM tblSAC_Departamentos WHERE estatus = @estatus ORDER BY descripcion",
                        parametros = new { estatus = true }
                    });
                    resultado.Add(ITEMS, lstCboDepartamentos);
                }
                else if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    List<ComboDTO> lstCboDepartamentos = _context.Select<ComboDTO>(new DapperDTO
                {

                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, UPPER(descripcion) AS text FROM tblSAC_Departamentos WHERE estatus = @estatus ORDER BY descripcion",
                    parametros = new { estatus = true }
                });
                    resultado.Add(ITEMS, lstCboDepartamentos);
                }

                //resultado.Add(ITEMS, lstCboDepartamentos);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerDepartamentos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboSubclasificacionesDepartamentos(int idDepartamento)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO SUBCLASIFICACIONES DEPARTAMENTOS

                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    List<ComboDTO> lstSubclasificacionesDep = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT id AS Value, subclasificacionDep AS text FROM tblSAC_SubclasificacionDepartamentos WHERE registroActivo = @registroActivo AND idDepartamento = @idDepartamento ORDER BY subclasificacionDep",
                        parametros = new { registroActivo = true, idDepartamento = idDepartamento }
                    });
                    resultado.Add(ITEMS, lstSubclasificacionesDep);
                }
                else if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    List<ComboDTO> lstSubclasificacionesDep = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id AS Value, subclasificacionDep AS text FROM tblSAC_SubclasificacionDepartamentos WHERE registroActivo = @registroActivo AND idDepartamento = @idDepartamento ORDER BY subclasificacionDep",
                        parametros = new { registroActivo = true, idDepartamento = idDepartamento }
                    });
                    resultado.Add(ITEMS, lstSubclasificacionesDep);
                }

                //resultado.Add(ITEMS, lstSubclasificacionesDep);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "ActoCondicionController", "FillCboSubclasificacionesDepartamentos", e, AccionEnum.CONSULTA, idDepartamento, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboSubclasificaciones()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var items = _context.tblSAC_SubclasificacionDepartamentos.Where(x => x.registroActivo).OrderBy(x => x.subclasificacionDep).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.subclasificacionDep
                }).ToList(); ;

                resultado.Add(ITEMS, items);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(16, 16, "ActoCondicionController", "FillCboSubclasificaciones", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }


        public Dictionary<string, object> ObtenerAcciones()
        {
            try
            {
                var acciones = _context.tblSAC_Accion
                    .ToList()
                    .Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.descripcion.ToUpper()
                    }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, acciones);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerAcciones", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerClasificaciones()
        {
            try
            {
                var clasificaciones = _context.tblSAC_Clasificacion.ToList();

                var clasificacionesActo = clasificaciones
                    .Where(x => x.tipoRiesgo == TipoRiesgo.Acto)
                    .Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.descripcion.ToUpper()
                    })
                    .ToList();

                var clasificacionesCondicion = clasificaciones
                    .Where(x => x.tipoRiesgo == TipoRiesgo.Condicion)
                    .Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.descripcion.ToUpper()
                    })
                    .ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("clasificacionesActo", clasificacionesActo);
                resultado.Add("clasificacionesCondicion", clasificacionesCondicion);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerClasificaciones", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> CargarActosCondiciones(FiltroActoCondicionDTO filtro)
        {
            try
            {
                var actosCondiciones = new List<ActoCondicionDTO>();

                List<tblS_IncidentesEmpresasContratistas> listaContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                List<tblS_IncidentesAgrupacionContratistas> listaAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    #region SE OBTIENE LISTADO DE SUBCLASIFICACIONES DEPARTAMENTOS

                    List<tblSAC_Departamentos> lstDepartamentos = _context.Select<tblSAC_Departamentos>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT id, descripcion FROM tblSAC_Departamentos WHERE estatus = @estatus",
                        parametros = new { estatus = true }
                    }).ToList();

                    #endregion

                    #region SE OBTIENE LISTADO DE SUBCLASIFICACIONES DEPARTAMENTOS

                    List<tblSAC_SubclasificacionDepartamentos> lstSubclasificacionesDep = _context.Select<tblSAC_SubclasificacionDepartamentos>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT id, subclasificacionDep, idDepartamento FROM tblSAC_SubclasificacionDepartamentos WHERE registroActivo = @registroActivo",
                        parametros = new { registroActivo = true }
                    }).ToList();

                    #endregion

                    string actoDesc = TipoRiesgo.Acto.GetDescription();
                    string condicionDesc = TipoRiesgo.Condicion.GetDescription();
                    string estatusCompletoDesc = EstatusActoCondicion.Completo.GetDescription();

                    filtro.fechaFinal = filtro.fechaFinal.AddHours(23).AddMinutes(59);

                    bool estatusVencido = filtro.estatus == (int)EstatusActoCondicion.Vencido;

                    #region Actos
                    if (filtro.estatus == (int)EstatusActoCondicion.Vencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.EnProceso;
                    }

                    bool puedeEliminar = new UsuarioDAO().getViewAction(7320, "eliminarEvidencia");

                    var actos = _context.tblSAC_Acto
                        .Where(x =>
                            x.fechaSuceso >= filtro.fechaInicial &&
                            x.fechaSuceso <= filtro.fechaFinal &&
                            x.idAgrupacion == filtro.idAgrupacion &&
                            x.idEmpresa == filtro.idEmpresa &&
                            (filtro.claveSupervisor == 0 ? true : x.claveSupervisor == filtro.claveSupervisor) &&
                            (filtro.departamentoID == 0 ? true : x.departamentoID == filtro.departamentoID) &&
                            (filtro.estatus == -1 ? true : x.estatus == (EstatusActoCondicion)filtro.estatus) &&
                            (filtro.subclasificacionDepID == 0 ? true : x.subclasificacionDepID == filtro.subclasificacionDepID) &&
                            (filtro.clasificacionID == -1 ? true : x.clasificacionGeneralID == filtro.clasificacionID) &&
                            x.activo)
                        .Select(x => new ActoCondicionDTO
                        {
                            id = x.id,
                            folio = x.folio,
                            cc = "",
                            idEmpresa = x.idEmpresa,
                            idAgrupacion = (int)x.idAgrupacion,
                            agrupacion = x.agrupacion,
                            //nomAgrupacion =
                            //    x.idEmpresa == 1000 ?
                            //        listaContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nombreEmpresa).FirstOrDefault() :
                            //    x.idEmpresa == 2000 ?
                            //        listaAgrupacionContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault()
                            //    : x.agrupacion.nomAgrupacion,
                            departamentoID = x.departamentoID,
                            subclasificacionDepID = x.subclasificacionDepID,
                            subclasificacionDep = string.Empty,
                            tipoRiesgo = TipoRiesgo.Acto,
                            tipoActo = x.tipoActo,
                            estatus = x.estatus,
                            fechaFirmado = x.fechaFirmado,
                            fechaSucesoDT = x.fechaSuceso,
                            puedeEliminar = puedeEliminar,
                            firmado = x.fechaFirmado != null
                        }).ToList();

                    if (estatusVencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.Vencido;
                    }

                    foreach (var item in actos)
                    {
                        EstatusActoCondicion estatus = 0;
                        string estatusDesc = null;
                        if (item.estatus == EstatusActoCondicion.Completo)
                        {
                            estatus = item.estatus;
                            estatusDesc = EstatusActoCondicion.Completo.GetDescription();
                        }
                        else if (item.fechaFirmado.HasValue && item.fechaFirmado.Value.AddDays(14) < DateTime.Now.Date)
                        {
                            estatus = EstatusActoCondicion.Vencido;
                            estatusDesc = EstatusActoCondicion.Vencido.GetDescription();
                        }
                        else
                        {
                            estatus = EstatusActoCondicion.EnProceso;
                            estatusDesc = EstatusActoCondicion.EnProceso.GetDescription();
                        }

                        if (filtro.estatus == -1 || (int)estatus == filtro.estatus)
                        {
                            string proyecto = null;
                            if (item.idEmpresa == 1000)
                            {
                                proyecto = listaContratistas.First(x => x.id == (int)item.idAgrupacion).nombreEmpresa;
                            }
                            else if (item.idEmpresa < 1000)
                            {
                                proyecto = item.agrupacion.nomAgrupacion;
                            }
                            else
                            {
                                proyecto = listaAgrupacionContratistas.First(x => x.id == item.idAgrupacion).nomAgrupacion;
                            }

                            item.tipoRiesgoDesc = actoDesc + " " + item.tipoActo.GetDescription();
                            item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                            item.proyecto = proyecto;
                            item.estatus = estatus;
                            item.estatusDesc = estatusDesc;
                            actosCondiciones.Add(item);
                        }

                        #region SE OBTIENE SUBCLASIFICACION/DEPARTAMENTO
                        if (item.subclasificacionDepID > 0)
                        {
                            #region SE OBTIENE LA SUBCLASIFICACIÓN DEL DEPARTAMENTO
                            item.subclasificacionDep = lstSubclasificacionesDep.Where(w => w.id == item.subclasificacionDepID).Select(s => s.subclasificacionDep).FirstOrDefault();
                            #endregion
                        }
                        else if (item.departamentoID > 0)
                        {
                            #region SE OBTIENE NOMBRE DEL DEPARTAMENTO
                            item.subclasificacionDep = lstDepartamentos.Where(w => w.id == item.departamentoID).Select(s => s.descripcion).FirstOrDefault();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    #region Condiciones
                    if (filtro.estatus == (int)EstatusActoCondicion.Vencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.EnProceso;
                    }

                    var condiciones = _context.tblSAC_Condicion
                        .Where(x =>
                            x.fechaSuceso >= filtro.fechaInicial &&
                            x.fechaSuceso <= filtro.fechaFinal &&
                            x.idAgrupacion == filtro.idAgrupacion &&
                            x.idEmpresa == filtro.idEmpresa &&
                            (filtro.claveSupervisor == 0 ? true : x.claveSupervisor == filtro.claveSupervisor) &&
                            (filtro.departamentoID == 0 ? true : x.departamentoID == filtro.departamentoID) &&
                            (filtro.estatus == -1 ? true : x.estatus == (EstatusActoCondicion)filtro.estatus) &&
                            (filtro.subclasificacionDepID == 0 ? true : x.subclasificacionDepID == filtro.subclasificacionDepID) &&
                            (filtro.clasificacionID == -1 ? true : x.clasificacionGeneralID == filtro.clasificacionID) &&
                            x.activo)
                        .Select(x => new ActoCondicionDTO
                        {
                            id = x.id,
                            folio = x.folio,
                            cc = "",
                            idEmpresa = x.idEmpresa,
                            idAgrupacion = (int)x.idAgrupacion,
                            agrupacion = x.agrupacion,
                            //nomAgrupacion =
                            //    x.idEmpresa == 1000 ?
                            //        listaContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nombreEmpresa).FirstOrDefault() :
                            //    x.idEmpresa == 2000 ?
                            //        listaAgrupacionContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault()
                            //    : x.agrupacion.nomAgrupacion,
                            tipoRiesgo = TipoRiesgo.Condicion,
                            tipoRiesgoDesc = condicionDesc,
                            departamentoID = x.departamentoID,
                            subclasificacionDepID = x.subclasificacionDepID,
                            subclasificacionDep = string.Empty,
                            estatus = x.estatus,
                            fechaFirmado = x.fechaFirmado,
                            fechaSucesoDT = x.fechaSuceso,
                            puedeEliminar = puedeEliminar,
                            firmado = x.fechaFirmado != null,
                            nivelPrioridad = x.nivelPrioridad,
                            prioridad = x.prioridad
                        }).ToList();

                    if (estatusVencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.Vencido;
                    }

                    foreach (var item in condiciones)
                    {
                        var diasParaCorreccion = 0;
                        var fechaParaCorreccion = item.fechaSucesoDT;
                        if (item.nivelPrioridad.HasValue)
                        {
                            diasParaCorreccion = item.prioridad.diasParaCorreccion;
                            fechaParaCorreccion = fechaParaCorreccion.AddDays(diasParaCorreccion);
                        }

                        EstatusActoCondicion estatus = 0;
                        string estatusDesc = null;
                        if (item.estatus == EstatusActoCondicion.Completo)
                        {
                            estatus = item.estatus;
                            estatusDesc = EstatusActoCondicion.Completo.GetDescription();
                        }
                        else if (fechaParaCorreccion <= DateTime.Now.Date)
                        {
                            estatus = EstatusActoCondicion.Vencido;
                            estatusDesc = EstatusActoCondicion.Vencido.GetDescription();
                        }
                        else
                        {
                            estatus = EstatusActoCondicion.EnProceso;
                            estatusDesc = EstatusActoCondicion.EnProceso.GetDescription();
                        }

                        if (filtro.estatus == -1 || (int)estatus == filtro.estatus)
                        {
                            string proyecto = null;
                            if (item.idEmpresa == 1000)
                            {
                                proyecto = listaContratistas.First(x => x.id == (int)item.idAgrupacion).nombreEmpresa;
                            }
                            else if (item.idEmpresa < 1000)
                            {
                                proyecto = item.agrupacion.nomAgrupacion;
                            }
                            else
                            {
                                proyecto = listaAgrupacionContratistas.First(x => x.id == item.idAgrupacion).nomAgrupacion;
                            }

                            item.proyecto = proyecto;
                            item.estatus = estatus;
                            item.estatusDesc = estatusDesc;
                            item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                            actosCondiciones.Add(item);
                        }

                        #region SE OBTIENE SUBCLASIFICACION/DEPARTAMENTO
                        if (item.subclasificacionDepID > 0)
                        {
                            #region SE OBTIENE LA SUBCLASIFICACIÓN DEL DEPARTAMENTO
                            item.subclasificacionDep = lstSubclasificacionesDep.Where(w => w.id == item.subclasificacionDepID).Select(s => s.subclasificacionDep).FirstOrDefault();
                            #endregion
                        }
                        else if (item.departamentoID > 0)
                        {
                            #region SE OBTIENE NOMBRE DEL DEPARTAMENTO
                            item.subclasificacionDep = lstDepartamentos.Where(w => w.id == item.departamentoID).Select(s => s.descripcion).FirstOrDefault();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {

                    #region SE OBTIENE LISTADO DE SUBCLASIFICACIONES DEPARTAMENTOS

                    List<tblSAC_Departamentos> lstDepartamentos = _context.Select<tblSAC_Departamentos>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id, descripcion FROM tblSAC_Departamentos WHERE estatus = @estatus",
                        parametros = new { estatus = true }
                    }).ToList();

                    #endregion

                    #region SE OBTIENE LISTADO DE SUBCLASIFICACIONES DEPARTAMENTOS

                    List<tblSAC_SubclasificacionDepartamentos> lstSubclasificacionesDep = _context.Select<tblSAC_SubclasificacionDepartamentos>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id, subclasificacionDep, idDepartamento FROM tblSAC_SubclasificacionDepartamentos WHERE registroActivo = @registroActivo",
                        parametros = new { registroActivo = true }
                    }).ToList();

                    #endregion

                    string actoDesc = TipoRiesgo.Acto.GetDescription();
                    string condicionDesc = TipoRiesgo.Condicion.GetDescription();
                    string estatusCompletoDesc = EstatusActoCondicion.Completo.GetDescription();

                    filtro.fechaFinal = filtro.fechaFinal.AddHours(23).AddMinutes(59);

                    bool estatusVencido = filtro.estatus == (int)EstatusActoCondicion.Vencido;

                    #region Actos
                    if (filtro.estatus == (int)EstatusActoCondicion.Vencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.EnProceso;
                    }

                    bool puedeEliminar = new UsuarioDAO().getViewAction(7320, "eliminarEvidencia");

                    var actos = _context.tblSAC_Acto
                        .Where(x =>
                            x.fechaSuceso >= filtro.fechaInicial &&
                            x.fechaSuceso <= filtro.fechaFinal &&
                            x.idAgrupacion == filtro.idAgrupacion &&
                            x.idEmpresa == vSesiones.sesionEmpresaActual &&
                            (filtro.claveSupervisor == 0 ? true : x.claveSupervisor == filtro.claveSupervisor) &&
                            (filtro.departamentoID == 0 ? true : x.departamentoID == filtro.departamentoID) &&
                            (filtro.estatus == -1 ? true : x.estatus == (EstatusActoCondicion)filtro.estatus) &&
                            (filtro.subclasificacionDepID == 0 ? true : x.subclasificacionDepID == filtro.subclasificacionDepID) &&
                            (filtro.clasificacionID == -1 ? true : x.clasificacionGeneralID == filtro.clasificacionID) &&
                            x.activo)
                        .Select(x => new ActoCondicionDTO
                        {
                            id = x.id,
                            folio = x.folio,
                            cc = "",
                            idEmpresa = x.idEmpresa,
                            idAgrupacion = (int)x.idAgrupacion,
                            agrupacion = x.agrupacion,
                            //nomAgrupacion =
                            //    x.idEmpresa == 1000 ?
                            //        listaContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nombreEmpresa).FirstOrDefault() :
                            //    x.idEmpresa == 2000 ?
                            //        listaAgrupacionContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault()
                            //    : x.agrupacion.nomAgrupacion,
                            departamentoID = x.departamentoID,
                            subclasificacionDepID = x.subclasificacionDepID,
                            subclasificacionDep = string.Empty,
                            tipoRiesgo = TipoRiesgo.Acto,
                            tipoActo = x.tipoActo,
                            estatus = x.estatus,
                            fechaFirmado = x.fechaFirmado,
                            fechaSucesoDT = x.fechaSuceso,
                            puedeEliminar = puedeEliminar,
                            firmado = x.fechaFirmado != null
                        }).ToList();

                    if (estatusVencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.Vencido;
                    }

                    foreach (var item in actos)
                    {
                        EstatusActoCondicion estatus = 0;
                        string estatusDesc = null;
                        if (item.estatus == EstatusActoCondicion.Completo)
                        {
                            estatus = item.estatus;
                            estatusDesc = EstatusActoCondicion.Completo.GetDescription();
                        }
                        else if (item.fechaFirmado.HasValue && item.fechaFirmado.Value.AddDays(14) < DateTime.Now.Date)
                        {
                            estatus = EstatusActoCondicion.Vencido;
                            estatusDesc = EstatusActoCondicion.Vencido.GetDescription();
                        }
                        else
                        {
                            estatus = EstatusActoCondicion.EnProceso;
                            estatusDesc = EstatusActoCondicion.EnProceso.GetDescription();
                        }

                        if (filtro.estatus == -1 || (int)estatus == filtro.estatus)
                        {
                            string proyecto = null;
                            if (item.idEmpresa == 1000)
                            {
                                proyecto = listaContratistas.First(x => x.id == (int)item.idAgrupacion).nombreEmpresa;
                            }
                            else if (item.idEmpresa < 1000)
                            {
                                proyecto = item.agrupacion.nomAgrupacion;
                            }
                            else
                            {
                                proyecto = listaAgrupacionContratistas.First(x => x.id == item.idAgrupacion).nomAgrupacion;
                            }

                            item.tipoRiesgoDesc = actoDesc + " " + item.tipoActo.GetDescription();
                            item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                            item.proyecto = proyecto;
                            item.estatus = estatus;
                            item.estatusDesc = estatusDesc;
                            actosCondiciones.Add(item);
                        }

                        #region SE OBTIENE SUBCLASIFICACION/DEPARTAMENTO
                        if (item.subclasificacionDepID > 0)
                        {
                            #region SE OBTIENE LA SUBCLASIFICACIÓN DEL DEPARTAMENTO
                            item.subclasificacionDep = lstSubclasificacionesDep.Where(w => w.id == item.subclasificacionDepID).Select(s => s.subclasificacionDep).FirstOrDefault();
                            #endregion
                        }
                        else if (item.departamentoID > 0)
                        {
                            #region SE OBTIENE NOMBRE DEL DEPARTAMENTO
                            item.subclasificacionDep = lstDepartamentos.Where(w => w.id == item.departamentoID).Select(s => s.descripcion).FirstOrDefault();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    #region Condiciones
                    if (filtro.estatus == (int)EstatusActoCondicion.Vencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.EnProceso;
                    }

                    var condiciones = _context.tblSAC_Condicion
                        .Where(x =>
                            x.fechaSuceso >= filtro.fechaInicial &&
                            x.fechaSuceso <= filtro.fechaFinal &&
                            x.idAgrupacion == filtro.idAgrupacion &&
                            x.idEmpresa == vSesiones.sesionEmpresaActual &&
                            (filtro.claveSupervisor == 0 ? true : x.claveSupervisor == filtro.claveSupervisor) &&
                            (filtro.departamentoID == 0 ? true : x.departamentoID == filtro.departamentoID) &&
                            (filtro.estatus == -1 ? true : x.estatus == (EstatusActoCondicion)filtro.estatus) &&
                            (filtro.subclasificacionDepID == 0 ? true : x.subclasificacionDepID == filtro.subclasificacionDepID) &&
                            (filtro.clasificacionID == -1 ? true : x.clasificacionGeneralID == filtro.clasificacionID) &&
                            x.activo)
                        .Select(x => new ActoCondicionDTO
                        {
                            id = x.id,
                            folio = x.folio,
                            cc = "",
                            idEmpresa = x.idEmpresa,
                            idAgrupacion = (int)x.idAgrupacion,
                            agrupacion = x.agrupacion,
                            //nomAgrupacion =
                            //    x.idEmpresa == 1000 ?
                            //        listaContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nombreEmpresa).FirstOrDefault() :
                            //    x.idEmpresa == 2000 ?
                            //        listaAgrupacionContratistas.Where(y => y.id == x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault()
                            //    : x.agrupacion.nomAgrupacion,
                            tipoRiesgo = TipoRiesgo.Condicion,
                            tipoRiesgoDesc = condicionDesc,
                            departamentoID = x.departamentoID,
                            subclasificacionDepID = x.subclasificacionDepID,
                            subclasificacionDep = string.Empty,
                            estatus = x.estatus,
                            fechaFirmado = x.fechaFirmado,
                            fechaSucesoDT = x.fechaSuceso,
                            puedeEliminar = puedeEliminar,
                            firmado = x.fechaFirmado != null,
                            nivelPrioridad = x.nivelPrioridad,
                            prioridad = x.prioridad
                        }).ToList();

                    if (estatusVencido)
                    {
                        filtro.estatus = (int)EstatusActoCondicion.Vencido;
                    }

                    foreach (var item in condiciones)
                    {
                        var diasParaCorreccion = 0;
                        var fechaParaCorreccion = item.fechaSucesoDT;
                        if (item.nivelPrioridad.HasValue)
                        {
                            diasParaCorreccion = item.prioridad.diasParaCorreccion;
                            fechaParaCorreccion = fechaParaCorreccion.AddDays(diasParaCorreccion);
                        }

                        EstatusActoCondicion estatus = 0;
                        string estatusDesc = null;
                        if (item.estatus == EstatusActoCondicion.Completo)
                        {
                            estatus = item.estatus;
                            estatusDesc = EstatusActoCondicion.Completo.GetDescription();
                        }
                        else if (fechaParaCorreccion <= DateTime.Now.Date)
                        {
                            estatus = EstatusActoCondicion.Vencido;
                            estatusDesc = EstatusActoCondicion.Vencido.GetDescription();
                        }
                        else
                        {
                            estatus = EstatusActoCondicion.EnProceso;
                            estatusDesc = EstatusActoCondicion.EnProceso.GetDescription();
                        }

                        if (filtro.estatus == -1 || (int)estatus == filtro.estatus)
                        {
                            string proyecto = null;
                            if (item.idEmpresa == 1000)
                            {
                                proyecto = listaContratistas.First(x => x.id == (int)item.idAgrupacion).nombreEmpresa;
                            }
                            else if (item.idEmpresa < 1000)
                            {
                                proyecto = item.agrupacion.nomAgrupacion;
                            }
                            else
                            {
                                proyecto = listaAgrupacionContratistas.First(x => x.id == item.idAgrupacion).nomAgrupacion;
                            }

                            item.proyecto = proyecto;
                            item.estatus = estatus;
                            item.estatusDesc = estatusDesc;
                            item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                            actosCondiciones.Add(item);
                        }

                        #region SE OBTIENE SUBCLASIFICACION/DEPARTAMENTO
                        if (item.subclasificacionDepID > 0)
                        {
                            #region SE OBTIENE LA SUBCLASIFICACIÓN DEL DEPARTAMENTO
                            item.subclasificacionDep = lstSubclasificacionesDep.Where(w => w.id == item.subclasificacionDepID).Select(s => s.subclasificacionDep).FirstOrDefault();
                            #endregion
                        }
                        else if (item.departamentoID > 0)
                        {
                            #region SE OBTIENE NOMBRE DEL DEPARTAMENTO
                            item.subclasificacionDep = lstDepartamentos.Where(w => w.id == item.departamentoID).Select(s => s.descripcion).FirstOrDefault();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }

                resultado.Add(ITEMS, actosCondiciones.OrderByDescending(x => x.fechaSuceso));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "CargarActosCondiciones", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar los actos y condiciones.");
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarActo(ActoDTO acto) // OMAR
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (acto.fechaIngreso == null) //Los contratistas no llevan fecha de ingreso, por lo tanto se coloca la fecha actual.
                    {
                        acto.fechaIngreso = DateTime.Now.ToShortDateString();
                    }

                    if (acto.idAgrupacion == 0)
                    {
                        throw new Exception("Debe capturar la agrupación.");
                    }

                    //if (acto.subclasificacionDepID == 0 && acto.id == 0)
                    //{
                    //    throw new Exception("Debe seleccionar una subclasificación departamento.");
                    //}

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    // Es nueva
                    if (acto.id == 0)
                    {
                        tblSAC_Acto nuevoActo = InicializarActo(acto);
                        nuevoActo.cc = "";

                        if (nuevoActo.tipoActo == TipoActo.Inseguro)
                        {
                            nuevoActo.numeroFalta = nuevoActo.numeroFalta == 0 ? 1 : nuevoActo.numeroFalta;

                            if (nuevoActo.accionID != (int)AccionInfraccionEnum.AMONESTACION)
                            {
                                nuevoActo.estatus = EstatusActoCondicion.EnProceso;
                            }
                        }

                        _context.tblSAC_Acto.Add(nuevoActo);
                        _context.SaveChanges();

                        #region verificar si es clasificación vacunación covid19 y eliminar acto de vacunación50%
                        if (nuevoActo.tipoActo == TipoActo.Seguro && nuevoActo.clasificacionID == 32) //32 == Vacunación 100%
                        {
                            var actosDelEmpleadoVacunacion50 = _context.tblSAC_Acto
                                .Where(x =>
                                    x.claveEmpleado == nuevoActo.claveEmpleado &&
                                    x.activo &&
                                    (x.clasificacionID == 31 || x.clasificacionID == 32) && //31 == Vacunación 50%
                                    x.id != nuevoActo.id).ToList();

                            foreach (var item in actosDelEmpleadoVacunacion50)
                            {
                                item.activo = false;
                            }

                            _context.SaveChanges();
                        }
                        #endregion

                        if (acto.tipoActo == TipoActo.Inseguro && acto.accionID == (int)AccionInfraccionEnum.AMONESTACION)
                        {
                            var causasAcciones = new List<tblSAC_ActoAccionReaccion>();
                            foreach (var item in acto.causas)
                            {
                                var causaAccion = new tblSAC_ActoAccionReaccion();
                                causaAccion.accionReaccionID = item.id;
                                causaAccion.seleccionado = item.check;
                                causaAccion.actoID = nuevoActo.id;
                                causaAccion.estatus = true;

                                causasAcciones.Add(causaAccion);
                            }
                            foreach (var item in acto.acciones)
                            {
                                var causaAccion = new tblSAC_ActoAccionReaccion();
                                causaAccion.accionReaccionID = item.id;
                                causaAccion.seleccionado = item.check;
                                causaAccion.actoID = nuevoActo.id;
                                causaAccion.estatus = true;

                                causasAcciones.Add(causaAccion);
                            }

                            _context.tblSAC_ActoAccionReaccion.AddRange(causasAcciones);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        // Actualización
                        var actoExistente = _context.tblSAC_Acto.First(x => x.id == acto.id);
                        actoExistente.accionID = acto.accionID;
                        actoExistente.descripcion = acto.descripcion;
                        actoExistente.clasificacionID = acto.clasificacionID;
                        actoExistente.procedimientoID = acto.procedimientoID;
                    }

                    if (acto.accionID != (int)AccionInfraccionEnum.AMONESTACION)
                    {
                        if (ObtenerRelacionEmpleadoAreaCC(TipoRiesgo.Acto, acto.idAgrupacion, acto.idEmpresa, (AreaEnum)acto.departamentoID).Count == 0)
                        {
                            resultado.Add(MESSAGE, "Al terminar de capturar las firmas no se podran enviar las notificaciones a los usuarios correspondiente debido a que no existe una relación Empleado-Area-CC para este grupo en la división de minería");
                        }
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarActo", e, AccionEnum.AGREGAR, 0, new { acto = acto });
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarCondicion(CondicionDTO condicion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (condicion.idAgrupacion == 0)
                    {
                        throw new Exception("Debe capturar la agrupación.");
                    }

                    //if (condicion.subclasificacionDepID == 0 && condicion.id == 0)
                    //{
                    //    throw new Exception("Debe seleccionar una subclasificación departamento.");
                    //}

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    var esNuevaCondicion = true;
                    tblSAC_Condicion nuevaCondicion = null;

                    // Es nueva
                    if (condicion.id == 0)
                    {
                        nuevaCondicion = InicializarCondicion(condicion);
                        nuevaCondicion.cc = "";
                        //nuevaCondicion.estatus = condicion.imagenAntes != null && condicion.imagenDespues != null ? EstatusActoCondicion.Completo : EstatusActoCondicion.EnProceso;
                        nuevaCondicion.estatus = EstatusActoCondicion.EnProceso;

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                        {
                            nuevaCondicion.idEmpresa = (int)EmpresaEnum.Colombia;
                        }

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                        {
                            nuevaCondicion.idEmpresa = (int)EmpresaEnum.Peru;
                        }

                        _context.tblSAC_Condicion.Add(nuevaCondicion);
                        _context.SaveChanges();

                        string rutaCarpetaCarpetaCondicion = ObtenerRutaCarpetaCondicion(nuevaCondicion.id);

                        // Verifica si existe la carpeta y si no, la crea.
                        if (GlobalUtils.VerificarExisteCarpeta(rutaCarpetaCarpetaCondicion, true) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                            return resultado;
                        }

                        // Imagen antes.
                        string nombreArchivoImagenAntes = ObtenerFormatoNombreArchivo(NombreBaseImagenAntes, condicion.imagenAntes.FileName);
                        string rutaArchivoImagenAntes = Path.Combine(rutaCarpetaCarpetaCondicion, nombreArchivoImagenAntes);
                        listaRutaArchivos.Add(Tuple.Create(condicion.imagenAntes, rutaArchivoImagenAntes));
                        nuevaCondicion.rutaImagenAntes = rutaArchivoImagenAntes;

                        // Si trae la imagen después.
                        if (condicion.imagenDespues != null)
                        {
                            string nombreArchivoImagenDespues = ObtenerFormatoNombreArchivo(NombreBaseImagenDespues, condicion.imagenDespues.FileName);
                            string rutaArchivoImagenDespues = Path.Combine(rutaCarpetaCarpetaCondicion, nombreArchivoImagenDespues);
                            listaRutaArchivos.Add(Tuple.Create(condicion.imagenDespues, rutaArchivoImagenDespues));
                            nuevaCondicion.rutaImagenDespues = rutaArchivoImagenDespues;
                            nuevaCondicion.fechaResolucion = DateTime.Now;
                        }

                        nuevaCondicion.departamento = _context.tblSAC_Departamentos.First(x => x.id == nuevaCondicion.departamentoID);
                        nuevaCondicion.prioridad = _context.tblSAC_ClasificacionPrioridad.First(x => x.id == nuevaCondicion.nivelPrioridad);

                        //var area = _context.tblS_IncidentesDepartamentos.First(x => x.id == condicion.departamentoID);
                        //if (ObtenerRelacionEmpleadoAreaCC(TipoRiesgo.Condicion, condicion.idAgrupacion, condicion.idEmpresa, area.departamento).Count == 0)
                        //{
                        //    resultado.Add(MESSAGE, "Al terminar de capturar las firmas no se podran enviar las notificaciones a los usuarios correspondiente debido a que no existe una relación Empleado-Area-CC para este grupo en la división de minería");
                        //}
                    }
                    else
                    {
                        // Actualización
                        esNuevaCondicion = false;

                        var condicionExistente = _context.tblSAC_Condicion.First(x => x.id == condicion.id);

                        // Si trae la imagen después.
                        if (condicion.imagenDespues != null)
                        {
                            string rutaCarpetaCarpetaCondicion = ObtenerRutaCarpetaCondicion(condicionExistente.id);
                            string nombreArchivoImagenDespues = ObtenerFormatoNombreArchivo(NombreBaseImagenDespues, condicion.imagenDespues.FileName);
                            string rutaArchivoImagenDespues = Path.Combine(rutaCarpetaCarpetaCondicion, nombreArchivoImagenDespues);
                            listaRutaArchivos.Add(Tuple.Create(condicion.imagenDespues, rutaArchivoImagenDespues));
                            condicionExistente.rutaImagenDespues = rutaArchivoImagenDespues;
                            //condicionExistente.estatus = EstatusActoCondicion.Completo;
                            condicionExistente.estatus = EstatusActoCondicion.EnProceso;

                            if (condicionExistente.fechaFirmado.HasValue)
                            {
                                condicionExistente.estatus = EstatusActoCondicion.Completo;
                            }

                            condicionExistente.fechaResolucion = DateTime.Now;
                        }

                        condicionExistente.descripcion = condicion.descripcion;
                        condicionExistente.clasificacionID = condicion.clasificacionID;
                        condicionExistente.procedimientoID = condicion.procedimientoID;
                        condicionExistente.nivelPrioridad = condicion.nivelPrioridad;
                        condicionExistente.accionCorrectiva = condicion.accionCorrectiva;

                        VistoAlerta(condicionExistente.id);
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);

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

                    if (esNuevaCondicion && nuevaCondicion != null)
                    {
                        if (string.IsNullOrEmpty(nuevaCondicion.rutaImagenDespues))
                        {
                            NotificarCondicion(nuevaCondicion);
                        }
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarCondicion", e, AccionEnum.AGREGAR, 0, new { condicion = condicion });
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerActoCondicion(TipoRiesgo tipoRiesgo, int id)
        {
            try
            {
                switch (tipoRiesgo)
                {
                    case TipoRiesgo.Acto:

                        var tblActo = _context.tblSAC_Acto.First(x => x.id == id);
                        var acto = new ActoDTO();
                        acto.id = tblActo.id;
                        acto.folio = tblActo.folio;
                        acto.claveEmpleado = tblActo.claveEmpleado;
                        acto.nombre = tblActo.nombre;
                        acto.esExterno = tblActo.esExterno;
                        acto.claveContratista = tblActo.claveContratista;
                        acto.fechaIngreso = tblActo.fechaIngreso.ToShortDateString();
                        acto.accionID = tblActo.accionID;
                        acto.puesto = tblActo.puesto;
                        acto.tipoActo = tblActo.tipoActo;
                        acto.cc = "";
                        acto.idEmpresa = tblActo.idEmpresa;
                        acto.idAgrupacion = tblActo.idAgrupacion.Value;
                        acto.departamentoID = tblActo.departamentoID;
                        acto.subclasificacionDepID = tblActo.subclasificacionDepID;
                        acto.fechaSuceso = tblActo.fechaSuceso.ToShortDateString();
                        acto.claveSupervisor = tblActo.claveSupervisor;
                        acto.nombreSupervisor = tblActo.nombreSupervisor;
                        acto.fechaCreacion = tblActo.fechaCreacion.ToShortDateString();
                        acto.claveInformo = tblActo.claveInformo;
                        acto.nombreInformo = tblActo.nombreInformo;
                        acto.descripcion = tblActo.descripcion;
                        acto.clasificacionID = tblActo.clasificacionID;
                        acto.procedimientoID = tblActo.procedimientoID;
                        acto.estatus = (int)tblActo.estatus;
                        acto.tipoRiesgo = TipoRiesgo.Acto;
                        acto.numeroInfraccion = tblActo.numeroInfraccion.HasValue ? tblActo.numeroInfraccion.Value : 0;
                        acto.descripcionInfraccion = tblActo.numeroInfraccion.HasValue && tblActo.numeroInfraccion.Value != 0 ? _context.tblSAC_MatrizAccionesDisciplinarias.First(y => y.numero == tblActo.numeroInfraccion.Value).tipoInfraccion : "";
                        acto.nivelInfraccion = tblActo.nivelInfraccion.HasValue ? tblActo.nivelInfraccion.Value : 0;
                        acto.nivelInfraccionAcumulado = tblActo.nivelInfraccionAcumulado.HasValue ? tblActo.nivelInfraccionAcumulado.Value : 0;
                        acto.numeroFalta = tblActo.tipoActo == TipoActo.Seguro ? 0 : tblActo.numeroFalta ?? 1;
                        acto.compromiso = tblActo.compromiso;
                        acto.tieneEvidencia = !string.IsNullOrEmpty(tblActo.rutaEvidencia);
                        acto.firmadoPorEmpleado = !string.IsNullOrEmpty(tblActo.firmaEmpleado);
                        acto.firmadoPorSupervisor = !string.IsNullOrEmpty(tblActo.firmaSupervisor);
                        acto.firmadoPorSST = !string.IsNullOrEmpty(tblActo.firmaSST);
                        acto.clasificacionGeneralID = tblActo.clasificacionGeneralID;

                        if (acto.accionID == (int)AccionInfraccionEnum.AMONESTACION)
                        {
                            var causas = _context.tblSAC_ActoAccionReaccion.Where(x => x.actoID == acto.id).ToList();

                            if (causas.Count > 0)
                            {
                                acto.causas = causas.Where(x => x.accionReaccion.tipo == 1 && x.estatus).Select(x => new CausaAccionDTO
                                {
                                    id = x.accionReaccionID,
                                    check = x.seleccionado
                                }).ToList();

                                acto.acciones = causas.Where(x => x.accionReaccion.tipo == 2 && x.estatus).Select(x => new CausaAccionDTO
                                {
                                    id = x.accionReaccionID,
                                    check = x.seleccionado
                                }).ToList();
                            }
                        }

                        resultado.Add(ITEMS, acto);

                        break;

                    case TipoRiesgo.Condicion:

                        var condicion = _context.tblSAC_Condicion
                            .Where(x => x.id == id)
                            .ToList()
                            .Select(x => new CondicionDTO
                            {
                                id = x.id,
                                folio = x.folio,
                                tieneImagenDespues = x.rutaImagenDespues != null,
                                fechaResolucion = x.fechaResolucion.HasValue ? x.fechaResolucion.Value.ToShortDateString() : null,
                                cc = "",
                                idEmpresa = x.idEmpresa,
                                idAgrupacion = (int)x.idAgrupacion,
                                departamentoID = x.departamentoID,
                                subclasificacionDepID = x.subclasificacionDepID,
                                fechaSuceso = x.fechaSuceso.ToShortDateString(),
                                claveSupervisor = x.claveSupervisor,
                                nombreSupervisor = x.nombreSupervisor,
                                fechaCreacion = x.fechaCreacion.ToShortDateString(),
                                claveInformo = x.claveInformo,
                                nombreInformo = x.nombreInformo,
                                descripcion = x.descripcion,
                                clasificacionID = x.clasificacionID,
                                procedimientoID = x.procedimientoID,
                                estatus = (int)x.estatus,
                                tipoRiesgo = TipoRiesgo.Condicion,
                                nivelPrioridad = x.nivelPrioridad.HasValue ? x.nivelPrioridad.Value : 0,
                                tieneEvidencia = !string.IsNullOrEmpty(x.rutaEvidencia),
                                accionCorrectiva = x.accionCorrectiva ?? "",
                                firmadoPorSupervisor = !string.IsNullOrEmpty(x.firmaSupervisor),
                                firmadoPorSST = !string.IsNullOrEmpty(x.firmaSST),
                                clasificacionGeneralID = x.clasificacionGeneralID
                            }).First();

                        resultado.Add(ITEMS, condicion);

                        break;

                    default:
                        throw new Exception("Tipo de riesgo no definido.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerActoCondicion", e, AccionEnum.CONSULTA, id, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar el elemento.");
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarActoCondicion(TipoRiesgo tipoRiesgo, int id)
        {
            try
            {
                switch (tipoRiesgo)
                {
                    case TipoRiesgo.Acto:

                        var acto = _context.tblSAC_Acto
                            .First(x => x.id == id);

                        acto.activo = false;
                        break;

                    case TipoRiesgo.Condicion:

                        var condicion = _context.tblSAC_Condicion
                            .First(x => x.id == id);

                        condicion.activo = false;
                        break;

                    default:
                        throw new Exception("Tipo de riesgo no definido.");
                }

                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "EliminarActoCondicion", e, AccionEnum.ELIMINAR, id, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al intentar eliminar el elemento.");
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo)
        {
            try
            {
                string rutaArchivo = ObtenerRutaFisicaArchivo(sucesoID, tipoRiesgo, tipoArchivo);

                var fileStream = GlobalUtils.GetFileAsStream(rutaArchivo);
                string name = Path.GetFileName(rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarArchivo", e, AccionEnum.DESCARGAR, sucesoID, 0);
                return null;
            }
        }

        public Dictionary<string, object> CargarDatosArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                string rutaArchivo = ObtenerRutaFisicaArchivo(sucesoID, tipoRiesgo, tipoArchivo);

                Stream fileStream = GlobalUtils.GetFileAsStream(rutaArchivo);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(rutaArchivo).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "CargarDatosArchivo", e, AccionEnum.CONSULTA, sucesoID, null);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerInformacionInfraccion(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            try
            {
                var infraccion = CalcularNivelFaltaAccion(numeroInfraccion, claveEmpleado, fechaSuceso);
                if (infraccion.estatus)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, infraccion);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, infraccion.mensaje);
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerInformacionInfraccion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerInformacionInfraccionContratista(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            try
            {
                InfoInfraccionDTO data = new InfoInfraccionDTO();

                var infoMatriz = _context.tblSAC_MatrizAccionesDisciplinarias.FirstOrDefault(x => x.estatus && x.numero == numeroInfraccion);

                if (infoMatriz == null)
                {
                    throw new Exception("No hay un tipo de infracción con número: " + numeroInfraccion);
                }

                var infraccionesAcumuladas = _context.tblSAC_Acto.Where(x => x.claveEmpleado == claveEmpleado && x.tipoActo == TipoActo.Inseguro && x.activo && x.numeroInfraccion != null).ToList();
                var fecha30DiasAntes = fechaSuceso.AddDays(-30);
                var fecha30DiasDespues = fechaSuceso.AddDays(30);

                #region Primera infracción del empleado
                if (infraccionesAcumuladas.Count == 0)
                {
                    data.estatus = true;
                    data.descripcion = infoMatriz.tipoInfraccion;
                    data.nivelInfraccion = infoMatriz.nivel;
                    data.nivelInfraccionAcumulado = infoMatriz.nivel;
                    data.numeroFalta = 1;
                }
                #endregion

                #region El empleado ya tuvo una infracción de rescisión de contrato y aun esta en el periodo de castigo
                var existeInfraccionRescision = infraccionesAcumuladas.Where(x => x.accionID == (int)AccionInfraccionEnum.RESCISION).OrderByDescending(x => x.fechaSuceso).FirstOrDefault();

                if (existeInfraccionRescision != null)
                {
                    var tiempoDeSancion = _context.tblSAC_MatrizAccionesDisciplinarias.FirstOrDefault(x => x.numero == existeInfraccionRescision.numeroInfraccion).sancion;
                    var fechaFinSancion = existeInfraccionRescision.fechaSuceso.AddMonths(tiempoDeSancion);

                    if (fechaFinSancion >= fechaSuceso)
                    {
                        data.estatus = false;
                        data.mensaje = "El empleado con clave " + claveEmpleado + " ya cuenta con una infracción de nivel " + (int)AccionInfraccionEnum.RESCISION + " la cual cuenta con una sanción de " + tiempoDeSancion + " meses y termina el " + fechaFinSancion.ToString("dd/MM/yyyy");
                    }
                }
                #endregion

                #region El empleado ya tiene infracciones y se calcula el # de falta de la nueva infracción o el nivel de infracción es de rescisión
                var infraccionesCon20DiasDeVigencia = infraccionesAcumuladas.Where(x => x.fechaSuceso >= fecha30DiasAntes && x.fechaSuceso <= fecha30DiasDespues).ToList();
                var ultimoNivelInfraccion = 0;
                var nuevoNivelInfraccion = 0;

                if (infraccionesCon20DiasDeVigencia.Count > 0)
                {
                    foreach (var item in infraccionesCon20DiasDeVigencia)
                    {
                        ultimoNivelInfraccion += infraccionesCon20DiasDeVigencia.OrderByDescending(x => x.id).FirstOrDefault().accionID;
                    }
                }

                nuevoNivelInfraccion = ultimoNivelInfraccion + infoMatriz.nivel;
                nuevoNivelInfraccion = nuevoNivelInfraccion == (int)AccionInfraccionEnum.ACTA_ADMINISTRATIVA ? (int)AccionInfraccionEnum.SUSPENSION : nuevoNivelInfraccion;

                if (nuevoNivelInfraccion > (int)AccionInfraccionEnum.RESCISION)
                {
                    nuevoNivelInfraccion = (int)AccionInfraccionEnum.RESCISION;
                }

                data.estatus = true;
                data.descripcion = infoMatriz.tipoInfraccion;
                data.nivelInfraccion = infoMatriz.nivel;
                data.nivelInfraccionAcumulado = nuevoNivelInfraccion;
                data.numeroFalta = infraccionesCon20DiasDeVigencia.Count + 1;
                #endregion

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerInformacionInfraccionExterno", e, AccionEnum.CONSULTA, 0, new { numeroInfraccion = numeroInfraccion, fechaSuceso = fechaSuceso });
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        private InfoInfraccionDTO CalcularNivelFaltaAccion(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            var matriz = _context.tblSAC_MatrizAccionesDisciplinarias.Where(x => x.estatus).ToList();
            InfoInfraccionDTO infoInfraccion = new InfoInfraccionDTO();

            var infoMatriz = matriz.FirstOrDefault(x => x.numero == numeroInfraccion);

            if (infoMatriz == null)
            {
                infoInfraccion.estatus = false;
                infoInfraccion.mensaje = "No hay un tipo de infracción con número: " + numeroInfraccion;
                return infoInfraccion;
            }

            var infraccionesAcumuladas = _context.tblSAC_Acto.Where(x => x.claveEmpleado == claveEmpleado && x.tipoActo == TipoActo.Inseguro && x.activo && x.numeroInfraccion != null).ToList();
            var fecha30DiasAntes = fechaSuceso.AddDays(-30);
            var fecha30DiasDespues = fechaSuceso.AddDays(30);

            #region Primera infracción del empleado
            if (infraccionesAcumuladas.Count == 0)
            {
                infoInfraccion.estatus = true;
                infoInfraccion.descripcion = infoMatriz.tipoInfraccion;
                infoInfraccion.nivelInfraccion = infoMatriz.nivel;
                infoInfraccion.nivelInfraccionAcumulado = infoMatriz.nivel;
                infoInfraccion.numeroFalta = 1;
                return infoInfraccion;
            }
            #endregion

            #region El empleado ya tuvo una infracción de rescisión de contrato y aun esta en el periodo de castigo
            var existeInfraccionRescision = infraccionesAcumuladas.Where(x => x.accionID == (int)AccionInfraccionEnum.RESCISION)
                .OrderByDescending(x => x.fechaSuceso).FirstOrDefault();
            if (existeInfraccionRescision != null)
            {
                var tiempoDeSancion = matriz.First(x => x.numero == existeInfraccionRescision.numeroInfraccion).sancion;
                var fechaFinSancion = existeInfraccionRescision.fechaSuceso.AddMonths(tiempoDeSancion);

                if (fechaFinSancion >= fechaSuceso)
                {
                    infoInfraccion.estatus = false;
                    infoInfraccion.mensaje = "El empleado con clave " + claveEmpleado + " ya cuenta con una infracción de nivel " + (int)AccionInfraccionEnum.RESCISION +
                        " la cual cuenta con una sanción de " + tiempoDeSancion + " meses y termina el " + fechaFinSancion.ToString("dd/MM/yyyy");
                    return infoInfraccion;
                }
            }
            #endregion

            #region El empleado ya tiene infracciones y se calcula el # de falta de la nueva infracción o el nivel de infracción es de rescisión
            var infraccionesCon20DiasDeVigencia = infraccionesAcumuladas.Where(x => x.fechaSuceso >= fecha30DiasAntes && x.fechaSuceso <= fecha30DiasDespues).ToList();
            var ultimoNivelInfraccion = 0;
            var nuevoNivelInfraccion = 0;
            if (infraccionesCon20DiasDeVigencia.Count > 0)
            {
                foreach (var item in infraccionesCon20DiasDeVigencia)
                {
                    ultimoNivelInfraccion += infraccionesCon20DiasDeVigencia.OrderByDescending(x => x.id).FirstOrDefault().accionID;
                }

            }
            nuevoNivelInfraccion = ultimoNivelInfraccion + infoMatriz.nivel;

            nuevoNivelInfraccion = nuevoNivelInfraccion == (int)AccionInfraccionEnum.ACTA_ADMINISTRATIVA ? (int)AccionInfraccionEnum.SUSPENSION : nuevoNivelInfraccion;

            if (nuevoNivelInfraccion > (int)AccionInfraccionEnum.RESCISION)
            {
                nuevoNivelInfraccion = (int)AccionInfraccionEnum.RESCISION;
            }

            infoInfraccion.estatus = true;
            infoInfraccion.descripcion = infoMatriz.tipoInfraccion;
            infoInfraccion.nivelInfraccion = infoMatriz.nivel;
            infoInfraccion.nivelInfraccionAcumulado = nuevoNivelInfraccion;
            infoInfraccion.numeroFalta = infraccionesCon20DiasDeVigencia.Count + 1;
            return infoInfraccion;
            #endregion
        }

        public Dictionary<string, object> ObtenerAccionReaccion(int tipo)
        {
            try
            {
                var opciones = _context.tblSAC_AccionReaccionContactoPersonal.Where(x => x.tipo == tipo && x.estatus).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, opciones);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerAccionReaccion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerPrioridades()
        {
            try
            {
                var listaPrioridades = _context.tblSAC_ClasificacionPrioridad.Where(x => x.estatus).Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion,
                    Prefijo = ""
                }).Distinct().OrderBy(x => x.Value).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listaPrioridades);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerPrioridades", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public reporteActoCondicionDTO ObtenerReporteActoCondicion(int id, int tipo)
        {
            reporteActoCondicionDTO objActoCondicion = new reporteActoCondicionDTO();
            try
            {
                if (tipo == (int)TipoRiesgo.Acto)
                {
                    var acto = _context.tblSAC_Acto.First(x => x.id == id);
                    objActoCondicion.fueContactoPersonal = acto.accionID == (int)AccionInfraccionEnum.AMONESTACION;
                    objActoCondicion.codigoControl = "";
                    objActoCondicion.esConstruplan = acto.idEmpresa == 0;
                    objActoCondicion.esContratista = acto.idEmpresa != 0;
                    objActoCondicion.esActoSeguro = acto.tipoActo == TipoActo.Seguro;
                    objActoCondicion.esActoInseguro = acto.tipoActo == TipoActo.Inseguro;
                    objActoCondicion.cc = _context.tblS_IncidentesAgrupacionCC.First(r => r.id == acto.idAgrupacion).nomAgrupacion;
                    objActoCondicion.departamento = acto.departamento.descripcion;
                    objActoCondicion.fechaSuceso = acto.fechaSuceso.ToString("dd/MM/yyyy");
                    objActoCondicion.claveSupervisor = acto.claveSupervisor.ToString();
                    objActoCondicion.nombreSupervisor = acto.nombreSupervisor;
                    objActoCondicion.claveEmpleadoInformo = acto.nombreInformo;
                    objActoCondicion.nombreEmpleadoInformo = acto.nombreInformo;
                    objActoCondicion.descripcionActoCondicion = acto.descripcion;
                    objActoCondicion.clasificacion = acto.clasificacion.descripcion;
                    objActoCondicion.procedimiento = acto.procedimientoViolado.Procedimineto;
                    objActoCondicion.nivelPrioridad = "";
                    objActoCondicion.claveEmpleadoActo = acto.claveEmpleado.ToString();
                    objActoCondicion.nombreEmpleadoActo = acto.nombre;
                    objActoCondicion.claveActoInseguro = acto.numeroInfraccion.HasValue ? acto.numeroInfraccion.ToString() : "";
                    objActoCondicion.accionID = acto.accionID;
                    objActoCondicion.compromisoPersonal = acto.compromiso;
                    objActoCondicion.nombrePersonaObservada = acto.nombre;
                    objActoCondicion.nombreSuperior = acto.nombreSupervisor;
                    objActoCondicion.nombreSMA = acto.nombreSST ?? "";
                    objActoCondicion.firmaEmpleado = !string.IsNullOrEmpty(acto.firmaEmpleado) ? System.Convert.FromBase64String(acto.firmaEmpleado) /*System.Convert.FromBase64String(acto.firmaEmpleado.Split(',')[1])*/ : null;
                    objActoCondicion.firmaSupervisor = !string.IsNullOrEmpty(acto.firmaSupervisor) ? System.Convert.FromBase64String(acto.firmaSupervisor) : null;
                    objActoCondicion.firmaSST = !string.IsNullOrEmpty(acto.firmaSST) ? System.Convert.FromBase64String(acto.firmaSST) : null;

                    if (objActoCondicion.fueContactoPersonal)
                    {
                        var causasAcciones = _context.tblSAC_ActoAccionReaccion.Where(x => x.actoID == acto.id && x.estatus).ToList();

                        foreach (var ca in causasAcciones)
                        {
                            switch (ca.accionReaccion.tipo)
                            {
                                case 1:
                                    typeof(reporteActoCondicionDTO).GetProperty("causa" + ca.accionReaccion.id).SetValue(objActoCondicion, ca.seleccionado);
                                    break;
                                case 2:
                                    typeof(reporteActoCondicionDTO).GetProperty("accion" + ca.accionReaccion.id).SetValue(objActoCondicion, ca.seleccionado);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    var condicion = _context.tblSAC_Condicion.First(x => x.id == id);
                    objActoCondicion.codigoControl = "";
                    objActoCondicion.esConstruplan = condicion.idEmpresa == 0;
                    objActoCondicion.esContratista = condicion.idEmpresa != 0;
                    objActoCondicion.esCondicion = true;
                    objActoCondicion.cc = _context.tblS_IncidentesAgrupacionCC.First(r => r.id == condicion.idAgrupacion).nomAgrupacion;
                    objActoCondicion.departamento = condicion.departamento.descripcion;
                    objActoCondicion.fechaSuceso = condicion.fechaSuceso.ToString("dd/MM/yyyy");
                    objActoCondicion.claveSupervisor = condicion.claveSupervisor.ToString();
                    objActoCondicion.nombreSupervisor = condicion.nombreSupervisor;
                    objActoCondicion.claveEmpleadoInformo = condicion.nombreInformo;
                    objActoCondicion.nombreEmpleadoInformo = condicion.nombreInformo;
                    objActoCondicion.descripcionActoCondicion = condicion.descripcion;
                    objActoCondicion.clasificacion = condicion.clasificacion.descripcion;
                    objActoCondicion.procedimiento = condicion.procedimientoViolado.Procedimineto;
                    objActoCondicion.nivelPrioridad = condicion.nivelPrioridad.HasValue ? condicion.prioridad.descripcion : "";
                    objActoCondicion.firmaSupervisor = !string.IsNullOrEmpty(condicion.firmaSupervisor) ? System.Convert.FromBase64String(condicion.firmaSupervisor) : null;
                    objActoCondicion.firmaSST = !string.IsNullOrEmpty(condicion.firmaSST) ? System.Convert.FromBase64String(condicion.firmaSST) : null;
                    objActoCondicion.nombreSMA = condicion.nombreSST ?? "";
                }
            }
            catch (Exception e)
            {
                resultado = new Dictionary<string, object>();
                LogError(0, 0, NombreControlador, "ObtenerReporteActoCondicion", e, AccionEnum.CONSULTA, 0, new { id, tipo });
                resultado.Add(SUCCESS, false);
            }
            return objActoCondicion;
        }

        public Dictionary<string, object> GuardarFirma(GuardarFirmaDTO data)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    bool firmado = false;

                    switch (data.tipoRiesgo)
                    {
                        case TipoRiesgo.Acto:
                            var acto = _context.tblSAC_Acto.First(x => x.id == data.idActoCondicion);
                            switch (data.tipoFirma)
                            {
                                case Core.Enum.Administracion.Seguridad.ActoCondicion.TipoFirmaEnum.EMPLEADO:
                                    acto.firmaEmpleado = firmaBase64SinTransparencia(data.imagen);
                                    break;
                                case Core.Enum.Administracion.Seguridad.ActoCondicion.TipoFirmaEnum.SUPERVISOR:
                                    acto.firmaSupervisor = firmaBase64SinTransparencia(data.imagen);
                                    break;
                                case Core.Enum.Administracion.Seguridad.ActoCondicion.TipoFirmaEnum.SST:
                                    acto.firmaSST = firmaBase64SinTransparencia(data.imagen);
                                    acto.claveSST = data.claveEmpleadoSST;
                                    acto.nombreSST = data.nombreEmpleadoSST;
                                    break;
                            }
                            firmado = !string.IsNullOrEmpty(acto.firmaEmpleado) && !string.IsNullOrEmpty(acto.firmaSupervisor) && !string.IsNullOrEmpty(acto.firmaSST);

                            if (firmado)
                            {
                                acto.fechaFirmado = DateTime.Now;
                                if (acto.accionID != (int)AccionInfraccionEnum.AMONESTACION)
                                {
                                    _context.SaveChanges();

                                    if (!acto.cargaMasiva.HasValue || (!acto.cargaMasiva != null ? !acto.cargaMasiva.Value : true)) //v1
                                    {
                                        if (HttpContext.Current.Session["rptNotificacionActoCondicionError"] != null)
                                        {
                                            var errorActa = HttpContext.Current.Session["rptNotificacionActoCondicionError"] as Dictionary<string, object>;

                                            HttpContext.Current.Session.Remove("rptNotificacionActoCondicionError");

                                            throw new Exception(errorActa[MESSAGE].ToString());
                                        }

                                        NotificarActo(acto);
                                    }
                                }
                                else
                                {
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                _context.SaveChanges();
                            }
                            break;
                        case TipoRiesgo.Condicion:
                            var condicion = _context.tblSAC_Condicion.First(x => x.id == data.idActoCondicion);
                            switch (data.tipoFirma)
                            {
                                case Core.Enum.Administracion.Seguridad.ActoCondicion.TipoFirmaEnum.SUPERVISOR:
                                    condicion.firmaSupervisor = firmaBase64SinTransparencia(data.imagen);
                                    break;
                                case Core.Enum.Administracion.Seguridad.ActoCondicion.TipoFirmaEnum.SST:
                                    condicion.firmaSST = firmaBase64SinTransparencia(data.imagen);
                                    condicion.claveSST = data.claveEmpleadoSST;
                                    condicion.nombreSST = data.nombreEmpleadoSST;
                                    break;
                            }
                            firmado = !string.IsNullOrEmpty(condicion.firmaSupervisor) && !string.IsNullOrEmpty(condicion.firmaSST);

                            if (firmado)
                            {
                                condicion.fechaFirmado = DateTime.Now;

                                if (condicion.rutaImagenDespues != null)
                                {
                                    condicion.estatus = EstatusActoCondicion.Completo;
                                }

                                _context.SaveChanges();
                                //if (condicion.nivelPrioridad == 1)
                                //{
                                //    _context.SaveChanges();
                                //    NotificarCondicion(condicion);
                                //}
                            }
                            else
                            {
                                _context.SaveChanges();
                            }
                            break;
                    }

                    HttpContext.Current.Session.Remove("rptNotificacionActoCondicion");

                    transaccion.Commit();

                    resultado.Add(SUCCESS, true);
                    resultado.Add("firmado", firmado);
                }
                catch (Exception e)
                {
                    HttpContext.Current.Session.Remove("rptNotificacionActoCondicion");
                    HttpContext.Current.Session.Remove("rptNotificacionActoCondicionError");
                    transaccion.Rollback();
                    resultado = new Dictionary<string, object>();
                    LogError(0, 0, NombreControlador, "GuardarFirma", e, AccionEnum.AGREGAR, 0, data);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> VistoAlerta(int id)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                var alerta = _context.tblP_Alerta
                    .FirstOrDefault(x =>
                        //x.userRecibeID == vSesiones.sesionUsuarioDTO.id &&
                        x.tipoAlerta == (int)AlertasEnum.REDIRECCION &&
                        x.sistemaID == (int)SistemasEnum.SEGURIDAD &&
                        x.msj.Contains("Condici") &&
                        x.visto == false &&
                        x.objID == id &&
                        x.moduloID == 0
                    );

                if (alerta != null)
                {
                    alerta.visto = true;
                    _context.SaveChanges();
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result = new Dictionary<string, object>();
                LogError(0, 0, NombreControlador, "VisorAlerta", e, AccionEnum.ACTUALIZAR, 0, id);
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }

            return result;
        }

        public Dictionary<string, object> DescargarActa(int id)
        {
            try
            {
                var acto = _context.tblSAC_Acto.First(x => x.id == id);

                var fecha = DateTime.Now;

                var relacionEmpleadoAreaCC = ObtenerRelacionEmpleadoAreaCC(TipoRiesgo.Acto, acto.idAgrupacion.Value, acto.idEmpresa, (AreaEnum)acto.departamentoID);
                if (relacionEmpleadoAreaCC.Count > 0)
                {
                    var responsableCapitalHumanoDelCC = relacionEmpleadoAreaCC.FirstOrDefault(x => x.area == (int)AreaEnum.CapitalHumano);
                    if (responsableCapitalHumanoDelCC != null)
                    {
                        var usuarioCH = _context.tblP_Usuario.First(x => x.id == responsableCapitalHumanoDelCC.idUsuario);
                        var sexoCH = InfoEmpleado(responsableCapitalHumanoDelCC.empleado).sexo;
                        var nombreCH = (sexoCH == "M" ? "el C. " : "la C. ") +
                            usuarioCH.nombre + (!string.IsNullOrEmpty(usuarioCH.apellidoPaterno) ? " " : "") +
                            (usuarioCH.apellidoPaterno ?? "") +
                            (!string.IsNullOrEmpty(usuarioCH.apellidoMaterno) ? " " : "") +
                            usuarioCH.apellidoMaterno ?? "";

                        var sexoEmpleado = InfoEmpleado(acto.claveEmpleado).sexo;
                        var nombreEmpleado = (sexoEmpleado == "M" ? "el C. " : "la C. ") + acto.nombre;

                        var infraccion = _context.tblSAC_MatrizAccionesDisciplinarias.First(x => x.numero == acto.numeroInfraccion);

                        var hora = fecha.ToString("t");
                        var dia = fecha.ToString("D");
                        var empresa = _context.tblP_Encabezado.First(x => x.id == vSesiones.sesionEmpresaActual).nombreEmpresa;
                        var puesto = acto.puesto;

                        var infoActa = new ActaAdministrativaDTO();
                        infoActa.parrafo1 = string.Format("" +
                            "<div align=\"justify\">En Hermosillo Sonora, siendo las {0} del día {1}, se reunieron en las instalaciones de <b>{2}</b>, " +
                            "<b>{3}</b>, en calidad de Coordinador de Relaciones Laborales de la empresa {4}, así como <b>{5}</b> que ostenta el cargo de <b>" +
                            "{6}</b> de la empresa ya mencionada, a efecto de hacer constar los siguientes:</div>",
                            hora, dia, empresa, nombreCH, empresa, nombreEmpleado, puesto);
                        infoActa.compromiso = string.Format("" +
                            "<div align=\"justify\"><p>Yo {0}, {1}.</p><p>Por otro lado y no habiendo nada más que hacer constar en la presente acta circunstanciada, " +
                            "se da por concluida la cual se levanta por duplicado, siendo las {2} horas del día {3}, " +
                            "firman al calce y al margen los que en ella intervinieron, para constancia, damos fe.</p></div>",
                            nombreEmpleado, acto.compromiso, hora, dia);
                        infoActa.declaraciones = string.Format("" +
                            "<div align=\"justify\"><p>En uso de la palabra {0}, en calidad de Coordinador de Relaciones Laborales, declara que ante esta situación y " +
                            "en base a los elementos presentados y resultantes de la situación, se determina que {1}, ha incurrido en la violación de los {2} " +
                            "correspondientes a la Ley Federal del Trabajo, vigente al día de hoy en los Estados Unidos Mexicanos.</p><p>Enseguida y en uso de la palabra {3}, " +
                            "quien manifestó llamarse como ha quedado escrito y en el puesto mencionado declara: Yo {4}, (falta la declaración del trabajador) que si incurrí " +
                            "en no realizar mi trabajo con esmero y el no cumplir con los compromisos establecidos.</p></div>",
                            nombreCH, nombreEmpleado, "(agregar artículos)", nombreEmpleado, nombreEmpleado);
                        infoActa.hallazgos = string.Format("" +
                            "<div align=\"justify\"><p>{0}</p><p>&nbsp;</p><p>Siendo parte de sus actividades y responsabilidades que realiza diariamente y como lo establece " +
                            "el y/o los procedimientos y procesos del departamento.</p></div>",
                            infraccion.tipoInfraccion);
                        infoActa.hechos = string.Format("" +
                            "<div align=\"justify\">El día {0}, en el área de trabajo de la empresa {1}, se presentó una situación donde {2}, {3}</div>",
                            acto.fechaSuceso.ToString("D"), empresa, nombreEmpleado, acto.descripcion);
                        infoActa.nombreEmpleado = nombreEmpleado.Substring(3);
                        infoActa.nombreResponsableCHdelCC = nombreCH.Substring(3);

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, infoActa);
                    }
                    else
                    {
                        throw new Exception("No se encontró relación empleado-area-cc para la área de Capital Humano");
                    }
                }
                else
                {
                    throw new Exception("No se encontró relación empleado-area-cc");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarActa", e, AccionEnum.CONSULTA, 0, id);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public sn_empleadosDTO InfoEmpleado(int claveEmpleado)
        {
            //            var odbc = new OdbcConsultaDTO();
            //            odbc.consulta =
            //                @"SELECT
            //                    clave_empleado,
            //                    nombre,
            //                    ape_paterno,
            //                    ape_materno,
            //                    sexo,
            //                    clave_depto,
            //                    puesto,
            //                    estatus_empleado
            //                FROM
            //                    sn_empleados AS sne
            //                WHERE
            //                    sne.clave_empleado = ?";

            //            odbc.parametros.Add(new OdbcParameterDTO
            //            {
            //                nombre = "clave_empleado",
            //                tipo = OdbcType.Int,
            //                valor = claveEmpleado
            //            });

            //            var resultadoConstruplan = _contextEnkontrol.Select<sn_empleadosDTO>(EnkontrolAmbienteEnum.Rh, odbc).FirstOrDefault();
            //            var resultadoArrendadora = _contextEnkontrol.Select<sn_empleadosDTO>(EnkontrolAmbienteEnum.RhArre, odbc).FirstOrDefault();

            var resultadoConstruplan = _context.Select<sn_empleadosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT
                                clave_empleado,
                                nombre,
                                ape_paterno,
                                ape_materno,
                                sexo,
                                clave_depto,
                                puesto,
                                estatus_empleado
                            FROM
                                tblRH_EK_Empleados AS sne
                            WHERE
                                sne.clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado }
            }).FirstOrDefault();

            var resultadoGCPLAN = _context.Select<sn_empleadosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.GCPLAN,
                consulta = @"SELECT
                                clave_empleado,
                                nombre,
                                ape_paterno,
                                ape_materno,
                                sexo,
                                clave_depto,
                                puesto,
                                estatus_empleado
                            FROM
                                tblRH_EK_Empleados AS sne
                            WHERE
                                sne.clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado }
            }).FirstOrDefault();

            var resultadoArrendadora = _context.Select<sn_empleadosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT
                                clave_empleado,
                                nombre,
                                ape_paterno,
                                ape_materno,
                                sexo,
                                clave_depto,
                                puesto,
                                estatus_empleado
                            FROM
                                tblRH_EK_Empleados AS sne
                            WHERE
                                sne.clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado }
            }).FirstOrDefault();

            if (resultadoConstruplan != null)
            {
                return resultadoConstruplan;
            }

            if (resultadoGCPLAN != null) 
            {
                return resultadoGCPLAN;
            }

            if (resultadoConstruplan == null && resultadoArrendadora == null && resultadoGCPLAN == null)
            {
                sn_empleadosDTO obj = new sn_empleadosDTO();
                obj.sexo = "";
                return obj;
            }

            return resultadoArrendadora;
        }

        public Dictionary<string, object> CargarActa(HttpPostedFileBase acta, int id)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var acto = _context.tblSAC_Acto.First(x => x.id == id && x.activo && x.estatus == EstatusActoCondicion.EnProceso);

                    var carpeta = ObtenerRutaCarpetaActo(acto.id);
                    var nombreArchivoActa = ObtenerFormatoNombreArchivo(NombreBaseActa, acta.FileName);
                    var rutaArchivoActa = Path.Combine(carpeta, nombreArchivoActa);

                    var directorio = new DirectoryInfo(carpeta);
                    if (!directorio.Exists)
                    {
                        directorio.Create();
                    }

                    if (GlobalUtils.SaveHTTPPostedFile(acta, rutaArchivoActa))
                    {
                        acto.estatus = EstatusActoCondicion.Completo;
                        acto.rutaActa = rutaArchivoActa;
                        acto.fechaCargaActa = DateTime.Now;
                        _context.SaveChanges();

                        transaccion.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        transaccion.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al guardar el archivo en el servidor");
                    }
                }
                catch (Exception e)
                {
                    transaccion.Rollback();
                    LogError(0, 0, NombreControlador, "CargaActa", e, AccionEnum.ACTUALIZAR, 0, id);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        private List<tblS_Req_EmpleadoAreaCC> ObtenerRelacionEmpleadoAreaCC(TipoRiesgo riesgo, int idAgrupacion, int idEmpresa, AreaEnum area)
        {
            List<tblS_Req_EmpleadoAreaCC> relacionEmpleadoGrupo = null;


            //if(vSesiones.sesionEmpresaActual==1 || vSesiones.sesionEmpresaActual==2)
            //{
            //   public int empresa = idEmpresa;
            //}else
            //{
            //   public int empresa = (int)vSesiones.sesionEmpresaActual;
            //}

            switch (riesgo)
            {
                case TipoRiesgo.Acto:
                    {
                        var areasActo = new List<int>
                        {
                            (int)AreaEnum.CapitalHumano,
                            (int)AreaEnum.Seguridad,
                            (int)AreaEnum.GerenciaProyecto,
                            (int)area
                        };
                        if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                        {
                            relacionEmpleadoGrupo = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.idAgrupacion == idAgrupacion && x.idEmpresa == idEmpresa && areasActo.Contains(x.area) && x.division == 1 && x.estatus).ToList();
                        }
                        else
                        {
                            relacionEmpleadoGrupo = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.idAgrupacion == idAgrupacion && x.idEmpresa == (int)vSesiones.sesionEmpresaActual && areasActo.Contains(x.area) && x.division == 1 && x.estatus).ToList();
                        }

                        foreach (var item in areasActo)
                        {
                            if (!relacionEmpleadoGrupo.Any(x => x.area == item))
                            {
                                throw new Exception("No se encontró al responsable de " + _context.tblSAC_Departamentos.Where(x => x.id == item).Select(x => x.descripcion).FirstOrDefault() + " para notificar el acto, favor de registrarlo. ATENCIÓN: La información de los responsables se toma del módulo de requerimientos en la división de \"Minería\".");
                            }
                        }
                    }
                    break;
                case TipoRiesgo.Condicion:
                    {
                        var areasCondicion = new List<int>
                        {
                            (int)AreaEnum.GerenciaProyecto,
                            (int)AreaEnum.Seguridad,
                            (int)area
                        };
                        if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                        {
                            relacionEmpleadoGrupo = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.idAgrupacion == idAgrupacion && x.idEmpresa == idEmpresa && areasCondicion.Contains(x.area) && x.division == 1 && x.estatus).ToList();
                        }
                        else
                        {
                            relacionEmpleadoGrupo = _context.tblS_Req_EmpleadoAreaCC.Where(x => x.idAgrupacion == idAgrupacion && x.idEmpresa == (int)vSesiones.sesionEmpresaActual && areasCondicion.Contains(x.area) && x.division == 1 && x.estatus).ToList();
                        }
                        foreach (var item in areasCondicion)
                        {
                            if (!relacionEmpleadoGrupo.Any(x => x.area == item))
                            {
                                throw new Exception("No se encontró el responsable de " + _context.tblSAC_Departamentos.Where(x => x.id == item).Select(x => x.descripcion).FirstOrDefault() + " para notificar la condición, favor de registrarlo. ATENCIÓN: La información de los responsables se toma del módulo de requerimientos en la división de \"Minería\".");
                            }
                        }
                    }
                    break;
            }

            return relacionEmpleadoGrupo;
        }

        private void NotificarActo(tblSAC_Acto acto)
        {
            List<MultiplesDTO> reportes = null;
            if (HttpContext.Current.Session["rptNotificacionActoCondicion"] != null)
            {
                reportes = HttpContext.Current.Session["rptNotificacionActoCondicion"] as List<MultiplesDTO>;
            }

            var infoEmpleados = new List<InfoEmpleadoNotificacionDTO>();

            var relacionEmpleadoGrupo = ObtenerRelacionEmpleadoAreaCC(TipoRiesgo.Acto, acto.idAgrupacion.Value, acto.idEmpresa, (AreaEnum)acto.departamentoID);

            /*
              idEmpresa = 0: ES UNA AGRUPACION DE CC DE CONSTRUPLAN
              idEmpresa = 1000: ES UN SOLO CONTRATISTA
              idEmpresa = 2000: ES UNA AGRUPACION DE CONTRATISTAS
             */
            var grupo = "";
            if (acto.idEmpresa == 2000)
            {
                var g = _context.tblS_IncidentesAgrupacionContratistas.FirstOrDefault(x => x.id == acto.idAgrupacion);
                if (g != null)
                {
                    grupo = g.nomAgrupacion;
                }
            }
            else
            {
                var g = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == acto.idAgrupacion);
                if (g != null)
                {
                    grupo = g.nomAgrupacion;
                }
            }

            var infraccion = _context.tblSAC_MatrizAccionesDisciplinarias.First(x => x.numero == acto.numeroInfraccion);

            foreach (var item in relacionEmpleadoGrupo)
            {
                var infoEmpleado = new InfoEmpleadoNotificacionDTO();
                var usuario = _context.tblP_Usuario.First(x => x.id == item.idUsuario);
                infoEmpleado.idUsuario = usuario.id;
                infoEmpleado.nombre = usuario.nombre + (!string.IsNullOrEmpty(usuario.apellidoPaterno) ? " " + usuario.apellidoPaterno : "") + (!string.IsNullOrEmpty(usuario.apellidoMaterno) ? " " + usuario.apellidoMaterno : "");
                infoEmpleado.correo = usuario.correo;
                infoEmpleado.area = item.area;
                infoEmpleados.Add(infoEmpleado);
            }

            if (infoEmpleados.Count > 0)
            {
                var fechaActual = DateTime.Now;
                string diaTardeNoche = fechaActual.Hour >= 5 && fechaActual.Hour < 12 ? "Buen día" : fechaActual.Hour >= 12 && fechaActual.Hour < 20 ? "Buenas tardes" : "Buenas noches";

                var correo = new Infrastructure.DTO.CorreoDTO();
                correo.asunto = "Acto inseguro folio: " + acto.folio + ", empleado " + acto.nombre;
                correo.cuerpo = "<p>" + diaTardeNoche + " " + infoEmpleados.First(x => x.area == (int)AreaEnum.CapitalHumano).nombre + ".</p>";
                correo.cuerpo += "<p>Se informa que ha sido creado un acto inseguro del empleado " + acto.claveEmpleado + " " + acto.nombre + " ";
                correo.cuerpo += " del centro de costos " + grupo + " por haber incurrido en la violación de la regla \"";
                correo.cuerpo += infraccion.tipoInfraccion + "\", por lo que con base en la matriz de acciones disciplinarias se ha determinado que es candidato a ";
                correo.cuerpo += acto.accion.descripcion + ".</p>";
                correo.cuerpo += "<p>El presente correo se anexa en copia al jefe directo de departamento, responsable del centro de costo y seguridad para darse por informados, ";
                correo.cuerpo += "además del seguimiento correspondiente.</p><br>";

                if (reportes != null)
                {
                    foreach (var item in reportes)
                    {
                        MemoryStream reporteActo = new MemoryStream(item.reporte);
                        correo.archivos.Add(new Attachment(reporteActo, item.nombre + ".pdf"));
                    }
                }
#if DEBUG
                correo.correos = new List<string> { "martin.zayas@construplan.com.mx" };
                correo.correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#else
                correo.correos = infoEmpleados.Where(x => (AreaEnum)x.area == AreaEnum.CapitalHumano).Select(x => x.correo).ToList();
                correo.correosCC = infoEmpleados.Where(x => (AreaEnum)x.area != AreaEnum.CapitalHumano).Select(x => x.correo).ToList();
#endif
                correo.Enviar();
            }
            else
            {
                throw new Exception("No se guardó el evento debido a que no se pudo realizar la notificación porque no se encontraron relaciones Empleado-Area-CC");
            }
        }

        private void NotificarCondicion(tblSAC_Condicion condicion)
        {
            List<MultiplesDTO> reportes = null;
            if (HttpContext.Current.Session["rptNotificacionActoCondicion"] != null)
            {
                reportes = HttpContext.Current.Session["rptNotificacionActoCondicion"] as List<MultiplesDTO>;
            }

            var infoEmpleados = new List<InfoEmpleadoNotificacionDTO>();

            var relacionEmpleadoGrupo = ObtenerRelacionEmpleadoAreaCC(TipoRiesgo.Condicion, condicion.idAgrupacion.Value, condicion.idEmpresa, (AreaEnum)condicion.departamentoID);
            var grupo = _context.tblS_IncidentesAgrupacionCC.First(x => x.id == condicion.idAgrupacion);

            foreach (var item in relacionEmpleadoGrupo)
            {
                var infoEmpleado = new InfoEmpleadoNotificacionDTO();
                var usuario = _context.tblP_Usuario.First(x => x.id == item.idUsuario);
                infoEmpleado.nombre = usuario.nombre + (!string.IsNullOrEmpty(usuario.apellidoPaterno) ? " " + usuario.apellidoPaterno : "") + (!string.IsNullOrEmpty(usuario.apellidoMaterno) ? " " + usuario.apellidoMaterno : "");
                infoEmpleado.correo = usuario.correo;
                infoEmpleado.area = item.area;
                infoEmpleado.idUsuario = usuario.id;
                infoEmpleados.Add(infoEmpleado);
            }

            if (infoEmpleados.Count > 0)
            {
                if (condicion.nivelPrioridad == 1)
                {
                    var fechaActual = DateTime.Now;
                    string diaTardeNoche = fechaActual.Hour >= 5 && fechaActual.Hour < 12 ? "Buen día" : fechaActual.Hour >= 12 && fechaActual.Hour < 20 ? "Buenas tardes" : "Buenas noches";

                    var correo = new Infrastructure.DTO.CorreoDTO();
                    correo.asunto = "Condición folio: " + condicion.folio;
                    correo.cuerpo = "<p>" + diaTardeNoche + " " + infoEmpleados.First(x => x.area == (int)AreaEnum.GerenciaProyecto).nombre + ".</p>";
                    correo.cuerpo += "<p>Se informa que ha sido creado una condición insegura del centro de costos " + grupo.nomAgrupacion + " con prioridad ALTA. ";
                    correo.cuerpo += "Por lo cual se envía la presente alerta automática para informar que se tiene un máximo de " + condicion.prioridad.diasParaCorreccion + " ";
                    correo.cuerpo += "días naturales para corregir la situación de riesgo.</p>";
                    correo.cuerpo += "<p>" + condicion.descripcion + "</p>";
                    correo.cuerpo += "El presente correo se anexa en copia al jefe de departamento, responsable del centro de costo y seguridad para darse por informados, además del seguimiento correspondiente.</p>";

                    if (reportes != null)
                    {
                        foreach (var item in reportes)
                        {
                            MemoryStream reporteActo = new MemoryStream(item.reporte);
                            correo.archivos.Add(new Attachment(reporteActo, item.nombre + ".pdf"));
                        }
                    }
#if DEBUG
                    correo.archivos.Add(new Attachment("C:\\Proyecto\\SIGOPLAN\\ACTOS_CONDICIONES\\CONDICIONES\\Condicion253903\\ImagenAntes 27-10-21 17-34-11.bmp", "image/bmp"));
                    correo.correos = new List<string> { "martin.zayas@construplan.com.mx" };
                    correo.correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#else
                    var nombreArchivo = Path.GetFileName(condicion.rutaImagenAntes);
                    correo.archivos.Add(new Attachment(condicion.rutaImagenAntes, MimeMapping.GetMimeMapping(nombreArchivo)));
                    correo.correos = infoEmpleados.Select(x => x.correo).ToList();
#endif
                    correo.Enviar();
                }

                if (string.IsNullOrEmpty(condicion.rutaImagenDespues))
                {
                    var alerta = new tblP_Alerta();
                    alerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
#if DEBUG
                    alerta.userRecibeID = 6571;
#else
                    alerta.userRecibeID = infoEmpleados.First(x => (AreaEnum)x.area == (AreaEnum)condicion.departamentoID).idUsuario;
#endif
                    alerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                    alerta.sistemaID = (int)SistemasEnum.SEGURIDAD;
                    alerta.visto = false;
                    alerta.url = "/Administrativo/ActoCondicion?id=" + condicion.id;
                    alerta.objID = condicion.id;
                    alerta.msj = "Condición folio: " + condicion.folio;
                    alerta.moduloID = 0;

                    _context.tblP_Alerta.Add(alerta);
                    _context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("No se guardó el evento debido a que no se pudo realizar la notificación porque no se encontraron relaciones Empleado-Area-CC");
            }
        }

        private string firmaBase64SinTransparencia(string imagenBase64)
        {
            //Todos esto para quitarle la transparencia a la imagen.
            byte[] imagenBytes = Convert.FromBase64String(imagenBase64.Split(',')[1]);
            using (var ms = new MemoryStream(imagenBytes, 0, imagenBytes.Length))
            {
                Image image = Image.FromStream(ms, true);

                var bmpImage = (System.Drawing.Bitmap)image;
                var bmp = new System.Drawing.Bitmap(bmpImage.Size.Width, bmpImage.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var g = System.Drawing.Graphics.FromImage(bmp);
                g.Clear(System.Drawing.Color.White);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.DrawImage(bmpImage, 0, 0);

                //var directorio = new DirectoryInfo(@"c:\ACTOCONDICION\");
                //if (!directorio.Exists)
                //{
                //    directorio.Create();
                //}
                //var path = System.IO.Path.Combine(directorio.ToString(), acto.nombre + DateTime.Now.ToString("dd-MM-yyyy-HHmmssfff") + ".bmp");
                //bmp.Save(path);

                using (MemoryStream t = new MemoryStream())
                {
                    bmp.Save(t, System.Drawing.Imaging.ImageFormat.Bmp);
                    byte[] imageBytesBmp = t.ToArray();
                    string base64 = Convert.ToBase64String(imageBytesBmp);
                    return base64;
                }
            }
        }

        public Dictionary<string, object> CargarComprimido(HttpPostedFileBase archivoComprimido)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    #region guardar archivo
                    var nombreArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff") + "_" + archivoComprimido.FileName;

                    var directorio = new DirectoryInfo(RutaZip + @"\");
                    if (!directorio.Exists)
                    {
                        directorio.Create();
                    }

                    var pathCompleto = Path.Combine(directorio.ToString(), nombreArchivo);
                    archivoComprimido.SaveAs(pathCompleto);
                    #endregion

                    #region procesar archivos (excel e imagenes) en el comprimido
                    //Se abre el archivo comprimido
                    using (var zip = new ZipArchive(archivoComprimido.InputStream, ZipArchiveMode.Read))
                    {
                        //Obtenemos el archivo excel comprimido
                        var archivoExcelComprimido = zip.Entries.FirstOrDefault(x => Path.GetExtension(x.FullName).ToUpper().Equals(".XLSX"));
                        if (archivoExcelComprimido != null)
                        {
                            #region procesar excel
                            using (var ms = new MemoryStream())
                            {
                                //Pasamos el archivo descomprimido a memoria
                                archivoExcelComprimido.Open().CopyTo(ms);

                                using (var excel = new ExcelPackage(ms))
                                {
                                    var hoja = excel.Workbook.Worksheets[1];

                                    #region revisar header
                                    #region columnas header
                                    var header = new List<HeaderDTO>
                                    {
                                        new HeaderDTO{columna = 1, columnaConLetras = "A", nombreHeader = "FOLIO" },
                                        new HeaderDTO{columna = 2, columnaConLetras = "B", nombreHeader = "TIPO RIESGO" },
                                        new HeaderDTO{columna = 3, columnaConLetras = "C", nombreHeader = "TIPO ACTO" },
                                        new HeaderDTO{columna = 4, columnaConLetras = "D", nombreHeader = "EMPRESA" },
                                        new HeaderDTO{columna = 5, columnaConLetras = "E", nombreHeader = "CC" },
                                        new HeaderDTO{columna = 6, columnaConLetras = "F", nombreHeader = "DEPARTAMENTO" },
                                        new HeaderDTO{columna = 7, columnaConLetras = "G", nombreHeader = "FECHA SUCESO" },
                                        new HeaderDTO{columna = 8, columnaConLetras = "H", nombreHeader = "CLAVE SUPERVISOR" },
                                        new HeaderDTO{columna = 9, columnaConLetras = "I", nombreHeader = "CLAVE EMPLEADO INFORMO" },
                                        new HeaderDTO{columna = 10, columnaConLetras = "J", nombreHeader = "DESCRIPCION SUCESO" },
                                        new HeaderDTO{columna = 11, columnaConLetras = "K", nombreHeader = "CLASIFICACION" },
                                        new HeaderDTO{columna = 12, columnaConLetras = "L", nombreHeader = "PROCEDIMIENTO" },
                                        new HeaderDTO{columna = 13, columnaConLetras = "M", nombreHeader = "NIVEL PRIORIDAD" },
                                        new HeaderDTO{columna = 14, columnaConLetras = "N", nombreHeader = "CLAVE EMPLEADO" },
                                        new HeaderDTO{columna = 15, columnaConLetras = "O", nombreHeader = "NOMBRE EMPLEADO" },
                                        new HeaderDTO{columna = 16, columnaConLetras = "P", nombreHeader = "EXTERNO" },
                                        new HeaderDTO{columna = 17, columnaConLetras = "Q", nombreHeader = "FECHA INGRESO" },
                                        new HeaderDTO{columna = 18, columnaConLetras = "R", nombreHeader = "PUESTO EMPLEADO" },
                                        new HeaderDTO{columna = 19, columnaConLetras = "S", nombreHeader = "CONTRATISTA" },
                                        new HeaderDTO{columna = 20, columnaConLetras = "T", nombreHeader = "TIPO INFRACCION" },
                                        new HeaderDTO{columna = 21, columnaConLetras = "U", nombreHeader = "MAL ENTENDIDO", esCausaAccion = true, causaAccionId = 1 },
                                        new HeaderDTO{columna = 22, columnaConLetras = "V", nombreHeader = "NO SABIA", esCausaAccion = true, causaAccionId = 2 },
                                        new HeaderDTO{columna = 23, columnaConLetras = "W", nombreHeader = "MAL DISEÑO", esCausaAccion = true, causaAccionId = 3 },
                                        new HeaderDTO{columna = 24, columnaConLetras = "X", nombreHeader = "NO QUERIA", esCausaAccion = true, causaAccionId = 4 },
                                        new HeaderDTO{columna = 25, columnaConLetras = "Y", nombreHeader = "FALTA DE INDUCCION", esCausaAccion = true, causaAccionId = 5 },
                                        new HeaderDTO{columna = 26, columnaConLetras = "Z", nombreHeader = "P.O. INAPROPIADO", esCausaAccion = true, causaAccionId = 6 },
                                        new HeaderDTO{columna = 27, columnaConLetras = "AA", nombreHeader = "OTROS", esCausaAccion = true, causaAccionId = 7 },
                                        new HeaderDTO{columna = 28, columnaConLetras = "AB", nombreHeader = "ESCUCHARLO", esCausaAccion = true, causaAccionId = 8 },
                                        new HeaderDTO{columna = 29, columnaConLetras = "AC", nombreHeader = "INDUCIRLO", esCausaAccion = true, causaAccionId = 9 },
                                        new HeaderDTO{columna = 30, columnaConLetras = "AD", nombreHeader = "CAPACITARLO", esCausaAccion = true, causaAccionId = 10 },
                                        new HeaderDTO{columna = 31, columnaConLetras = "AE", nombreHeader = "ORIENTARLO", esCausaAccion = true, causaAccionId = 11 },
                                        new HeaderDTO{columna = 32, columnaConLetras = "AF", nombreHeader = "CONSCIENTIZARLO", esCausaAccion = true, causaAccionId = 12 },
                                        new HeaderDTO{columna = 33, columnaConLetras = "AG", nombreHeader = "AMONESTARLO", esCausaAccion = true, causaAccionId = 13 },
                                        new HeaderDTO{columna = 34, columnaConLetras = "AH", nombreHeader = "INVESTIGARLO", esCausaAccion = true, causaAccionId = 14 },
                                        new HeaderDTO{columna = 35, columnaConLetras = "AI", nombreHeader = "OTRAS", esCausaAccion = true, causaAccionId = 15 },
                                        new HeaderDTO{columna = 36, columnaConLetras = "AJ", nombreHeader = "COMPROMISO PERSONAL" },
                                        new HeaderDTO{columna = 37, columnaConLetras = "AK", nombreHeader = "CLASIFICACION GENERAL" },
                                        new HeaderDTO{columna = 38, columnaConLetras = "AL", nombreHeader = "SUBCLASIFICACION DEPARTAMENTO" }
                                    };
                                    #endregion

                                    foreach (var item in header)
                                    {
                                        if (item.nombreHeader != hoja.Cells[2, item.columna].Value.ToString())
                                        {
                                            throw new Exception("En el header se esperaba la columna [" + item.nombreHeader + "] en la posición [" + item.columnaConLetras + 3 + "]");
                                        }
                                    }
                                    #endregion

                                    #region procesar contenido
                                    if (hoja.Dimension.Rows >= 3)
                                    {
                                        for (int i = 3; i <= hoja.Dimension.Rows; i++)
                                        {
                                            if (string.IsNullOrWhiteSpace(hoja.Cells["A" + i].GetValue<string>()))
                                            {
                                                break;
                                            }

                                            ValidarCamposExcel(hoja.Cells["A" + i + ":AL" + i], i);

                                            var tipoRiesgo = hoja.Cells["B" + i].GetValue<string>().Trim().ToUpper();
                                            var empresa = hoja.Cells["D" + i].GetValue<string>().Trim().ToUpper();
                                            var esContratista = empresa == "CONTRATISTA";
                                            var fechaSuceso = Convert.ToDateTime(hoja.Cells["G" + i].GetValue<string>());

                                            #region infoCC
                                            var agrupacion = hoja.Cells["E" + i].GetValue<string>().Trim().ToUpper();
                                            var idEmpresa = Convert.ToInt32(agrupacion.Split('-')[1]);
                                            var idAgrupacion = Convert.ToInt32(agrupacion.Split('-')[0]);
                                            #endregion

                                            #region infoInformo
                                            empleadoIncidenteDTO infoEmpleadoInformo = null;
                                            var claveInformo = hoja.Cells["I" + i].GetValue<int>();
                                            var resultadoInfoEmpleadoInformo = incidenteFS.getInfoEmpleado(claveInformo, esContratista, idEmpresa);
                                            if ((bool)resultadoInfoEmpleadoInformo[SUCCESS])
                                            {
                                                infoEmpleadoInformo = resultadoInfoEmpleadoInformo["empleadoInfo"] as empleadoIncidenteDTO;
                                            }
                                            else
                                            {
                                                throw new Exception("No se encontró un empleado informo con el número de clave " + claveInformo + " en el renglon " + i);
                                            }
                                            #endregion

                                            #region infoSupervisor
                                            empleadoIncidenteDTO infoEmpleadoSupervisor = null;
                                            var claveSupervisor = hoja.Cells["H" + i].GetValue<int>();
                                            var resultadoInfoEmpleadoSupervisor = incidenteFS.getInfoEmpleado(claveSupervisor, esContratista, idEmpresa);
                                            if ((bool)resultadoInfoEmpleadoSupervisor[SUCCESS])
                                            {
                                                infoEmpleadoSupervisor = resultadoInfoEmpleadoSupervisor["empleadoInfo"] as empleadoIncidenteDTO;
                                            }
                                            else
                                            {
                                                throw new Exception("No se encontró un supervisor con el número de clave " + claveSupervisor + " en el renglon " + i);
                                            }
                                            #endregion

                                            #region campos segun tipo de riesgo
                                            switch (tipoRiesgo)
                                            {
                                                #region ACTO
                                                case "ACTO":
                                                    {
                                                        var acto = new tblSAC_Acto();

                                                        var tipoActoTexto = hoja.Cells["C" + i].GetValue<string>().Trim().ToUpper();

                                                        #region infoEmpleado
                                                        empleadoIncidenteDTO infoEmpleado = null;
                                                        var claveEmpleado = hoja.Cells["N" + i].GetValue<int>();
                                                        var resultadoInfoEmpleado = incidenteFS.getInfoEmpleado(claveEmpleado, esContratista, idEmpresa);
                                                        if ((bool)resultadoInfoEmpleado[SUCCESS])
                                                        {
                                                            infoEmpleado = resultadoInfoEmpleado["empleadoInfo"] as empleadoIncidenteDTO;
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("No se encontró un empleado con el número de clave " + claveEmpleado + " en el renglon " + i);
                                                        }

                                                        var esExterno = false;
                                                        var campoExterno = hoja.Cells["P" + i].GetValue<string>();

                                                        int? claveContratista = null;
                                                        var contratista = hoja.Cells["S" + i].GetValue<int>();
                                                        claveContratista = contratista == 0 ? null : (int?)contratista;
                                                        #endregion

                                                        acto.claveEmpleado = claveEmpleado;
                                                        acto.nombre = infoEmpleado.nombreEmpleado;
                                                        acto.puesto = infoEmpleado.puestoEmpleado;
                                                        acto.fechaIngreso = infoEmpleado.antiguedadEmpleado;
                                                        acto.esExterno = esExterno;
                                                        acto.claveContratista = claveContratista;
                                                        acto.claveInformo = claveInformo;
                                                        acto.nombreInformo = infoEmpleadoInformo.nombreEmpleado;
                                                        acto.fechaCreacion = DateTime.Now;
                                                        acto.cc = "";
                                                        acto.folio = ObtenerNuevoFolioActoCondicion(idEmpresa, idAgrupacion);
                                                        acto.descripcion = hoja.Cells["J" + i].GetValue<string>().Trim().ToUpper();
                                                        acto.clasificacionID = hoja.Cells["K" + i].GetValue<int>();
                                                        acto.procedimientoID = hoja.Cells["L" + i].GetValue<int>();
                                                        acto.fechaSuceso = Convert.ToDateTime(hoja.Cells["G" + i].GetValue<string>().Trim());
                                                        acto.claveSupervisor = claveSupervisor;
                                                        acto.nombreSupervisor = infoEmpleadoSupervisor.nombreEmpleado;
                                                        acto.departamentoID = hoja.Cells["F" + i].GetValue<int>();
                                                        acto.subclasificacionDepID = hoja.Cells["AL" + i].GetValue<int>();
                                                        acto.activo = true;
                                                        acto.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                                        acto.idEmpresa = idEmpresa;
                                                        acto.idAgrupacion = idAgrupacion;
                                                        acto.clasificacionGeneralID = hoja.Cells["AK" + i].GetValue<int>();
                                                        acto.cargaMasiva = true;

                                                        switch (tipoActoTexto)
                                                        {
                                                            #region SEGURO
                                                            case "SEGURO":
                                                                acto.tipoActo = TipoActo.Seguro;
                                                                acto.estatus = EstatusActoCondicion.Completo;
                                                                acto.accionID = (int)AccionInfraccionEnum.AMONESTACION;

                                                                _context.tblSAC_Acto.Add(acto);
                                                                _context.SaveChanges();
                                                                break;
                                                            #endregion
                                                            #region INSEGURO
                                                            case "INSEGURO":
                                                                #region accion
                                                                var tipoInfraccion = hoja.Cells["T" + i].GetValue<int>();
                                                                InfoInfraccionDTO falta = CalcularNivelFaltaAccion(tipoInfraccion, claveEmpleado, fechaSuceso);

                                                                if (!falta.estatus)
                                                                {
                                                                    throw new Exception(falta.mensaje);
                                                                }

                                                                EstatusActoCondicion estatus = 0;
                                                                if (acto.accionID != (int)AccionInfraccionEnum.AMONESTACION)
                                                                {
                                                                    estatus = EstatusActoCondicion.EnProceso;
                                                                }
                                                                else
                                                                {
                                                                    estatus = EstatusActoCondicion.Completo;
                                                                }
                                                                #endregion

                                                                acto.accionID = falta.nivelInfraccionAcumulado;
                                                                acto.tipoActo = TipoActo.Inseguro;
                                                                acto.estatus = estatus;
                                                                acto.numeroInfraccion = tipoInfraccion;
                                                                acto.nivelInfraccion = falta.nivelInfraccion;
                                                                acto.nivelInfraccionAcumulado = falta.nivelInfraccionAcumulado;
                                                                acto.numeroFalta = falta.numeroFalta == 0 ? 1 : falta.numeroFalta;
                                                                acto.compromiso = hoja.Cells["AJ" + i].GetValue<string>();

                                                                _context.tblSAC_Acto.Add(acto);
                                                                _context.SaveChanges();

                                                                if (acto.accionID == (int)AccionInfraccionEnum.AMONESTACION)
                                                                {
                                                                    var causasAcciones = new List<tblSAC_ActoAccionReaccion>();

                                                                    foreach (var causaAccion in header.Where(x => x.esCausaAccion))
                                                                    {
                                                                        var ca = new tblSAC_ActoAccionReaccion();
                                                                        ca.accionReaccionID = causaAccion.causaAccionId.Value;
                                                                        ca.seleccionado = !string.IsNullOrEmpty(hoja.Cells[causaAccion.columnaConLetras + i].GetValue<string>());
                                                                        ca.actoID = acto.id;
                                                                        ca.estatus = true;
                                                                        causasAcciones.Add(ca);
                                                                    }

                                                                    _context.tblSAC_ActoAccionReaccion.AddRange(causasAcciones);
                                                                    _context.SaveChanges();
                                                                }

                                                                if (acto.accionID != (int)AccionInfraccionEnum.AMONESTACION)
                                                                {
                                                                    acto.departamento = _context.tblSAC_Departamentos.First(x => x.id == acto.departamentoID);
                                                                    acto.accion = _context.tblSAC_Accion.First(x => x.id == acto.accionID);

                                                                    NotificarActo(acto);
                                                                }

                                                                break;
                                                            #endregion
                                                            default:
                                                                throw new Exception("No se reconoce el tipo de acto en el renglon " + i + " columna C");
                                                        }
                                                    }
                                                    break;
                                                #endregion

                                                #region CONDICION
                                                case "CONDICION":
                                                    {
                                                        var archivoImagenAntes = zip.Entries.FirstOrDefault(x => x.FullName.ToUpper().Contains(hoja.Cells["A" + i].GetValue<string>().Trim().ToUpper()));
                                                        if (archivoImagenAntes != null)
                                                        {
                                                            var condicion = new tblSAC_Condicion();

                                                            condicion.claveInformo = claveInformo;
                                                            condicion.nombreInformo = infoEmpleadoInformo.nombreEmpleado;
                                                            condicion.fechaCreacion = DateTime.Now;
                                                            condicion.cc = "";
                                                            condicion.folio = ObtenerNuevoFolioActoCondicion(idEmpresa, idAgrupacion);
                                                            condicion.descripcion = hoja.Cells["J" + i].GetValue<string>().Trim().ToUpper();
                                                            condicion.clasificacionID = hoja.Cells["K" + i].GetValue<int>();
                                                            condicion.procedimientoID = hoja.Cells["L" + i].GetValue<int>();
                                                            condicion.fechaSuceso = Convert.ToDateTime(hoja.Cells["G" + i].GetValue<string>().Trim());
                                                            condicion.claveSupervisor = claveSupervisor;
                                                            condicion.nombreSupervisor = infoEmpleadoSupervisor.nombreEmpleado;
                                                            condicion.departamentoID = hoja.Cells["F" + i].GetValue<int>();
                                                            condicion.subclasificacionDepID = hoja.Cells["AL" + i].GetValue<int>();
                                                            condicion.estatus = EstatusActoCondicion.EnProceso;
                                                            condicion.nivelPrioridad = hoja.Cells["M" + i].GetValue<int>();
                                                            condicion.activo = true;
                                                            condicion.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                                            condicion.idEmpresa = idEmpresa;
                                                            condicion.idAgrupacion = idAgrupacion;
                                                            condicion.clasificacionGeneralID = hoja.Cells["AK" + i].GetValue<int>();

                                                            _context.tblSAC_Condicion.Add(condicion);
                                                            _context.SaveChanges();

                                                            string rutaCarpetaCondicion = ObtenerRutaCarpetaCondicion(condicion.id);
                                                            var carpetaCondicion = new DirectoryInfo(rutaCarpetaCondicion);
                                                            if (!carpetaCondicion.Exists)
                                                            {
                                                                carpetaCondicion.Create();
                                                            }
                                                            string nombreArchivoImagenAntes = ObtenerFormatoNombreArchivo(NombreBaseImagenAntes, archivoImagenAntes.Name);
                                                            string rutaArchivoImagenAntes = Path.Combine(rutaCarpetaCondicion, nombreArchivoImagenAntes);
                                                            condicion.rutaImagenAntes = rutaArchivoImagenAntes;
                                                            archivoImagenAntes.ExtractToFile(rutaArchivoImagenAntes);

                                                            _context.SaveChanges();

                                                            condicion.departamento = _context.tblSAC_Departamentos.First(x => x.id == condicion.departamentoID);
                                                            condicion.prioridad = _context.tblSAC_ClasificacionPrioridad.First(x => x.id == condicion.nivelPrioridad);

                                                            NotificarCondicion(condicion);
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("No se encontró un archivo de imagen de antes para la condición del renglon " + i);
                                                        }
                                                    }
                                                    break;
                                                #endregion
                                                default:
                                                    throw new Exception("No se reconoce el tipo de riesgo en el renglon " + i + " columna B, Se espera la palabra ACTO o CONDICION");
                                            }
                                            #endregion
                                        }
                                    }

                                    transaccion.Commit();
                                    resultado.Add(SUCCESS, true);
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            string mensajeError = "El archivo zip cargado no cuenta con un archivo excel compatible.";
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, mensajeError);
                            LogError(0, 0, NombreControlador, "CargarComprimido", null, AccionEnum.AGREGAR, 0, mensajeError + " Nombre archivo: " + nombreArchivo);
                        }
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    transaccion.Rollback();

                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "CargarComprimido", e, AccionEnum.AGREGAR, 0, 0);
                }
            }

            return resultado;
        }

        private void ValidarCamposExcel(ExcelRange rango, int renglon)
        {
            #region generales
            var xl_folio = rango["A" + renglon].GetValue<string>();
            if (string.IsNullOrEmpty(xl_folio))
            {
                throw new Exception("El campo FOLIO debe tener información en el renglon " + renglon + " columna A");
            }

            var xl_tipoRiesgo = rango["B" + renglon].GetValue<string>();
            if (string.IsNullOrEmpty(xl_tipoRiesgo))
            {
                throw new Exception("El campo TIPO RIESGO debe tener información en el renglon " + renglon + " columna B");
            }
            else
            {
                xl_tipoRiesgo = xl_tipoRiesgo.Trim().ToUpper();
                if (xl_tipoRiesgo != "ACTO" && xl_tipoRiesgo != "CONDICION" && xl_tipoRiesgo != "CONDICIÓN")
                {
                    throw new Exception("No se reconoce el TIPO RIESGO en el renglon " + renglon + " columna B");
                }
            }

            var xl_empresa = rango["D" + renglon].GetValue<string>();
            if (string.IsNullOrEmpty(xl_empresa))
            {
                xl_empresa = xl_empresa.Trim().ToUpper();
                if (xl_empresa != "CONSTRUPLAN" && xl_empresa != "CONTRATISTA")
                {
                    throw new Exception("No se reconcoe la EMPRESA en el renglon " + renglon + " columna D");
                }
            }

            var xl_cc = rango["E" + renglon].GetValue<string>();
            if (string.IsNullOrEmpty(xl_cc))
            {
                throw new Exception("Debe ingresar un CC valido en el renglon " + renglon + " columna E");
            }

            var xl_departamento = rango["F" + renglon].GetValue<int>();
            if (xl_departamento == 0)
            {
                throw new Exception("Debe ingresar un DEPARTAMENTO en el renglon " + renglon + " columna F");
            }

            var xl_fechaSuceso = rango["G" + renglon].GetValue<string>();
            if (string.IsNullOrEmpty(xl_fechaSuceso))
            {
                throw new Exception("Debe ingresar una FECHA SUCESO valida en el renglon " + renglon + " columna G");
            }
            else
            {
                xl_fechaSuceso = xl_fechaSuceso.Trim();
                DateTime fechaSuceso = new DateTime();
                if (!DateTime.TryParse(xl_fechaSuceso, out fechaSuceso))
                {
                    throw new Exception("Debe ingresar una FECHA SUCESO valida (DD/MM/YYYY) en el renglon " + renglon + " columna G");
                }
            }

            var xl_claveSupervisor = rango["H" + renglon].GetValue<int>();
            if (xl_claveSupervisor == 0)
            {
                throw new Exception("Debe ingresar una CLAVE SUPERVISOR valida en el renglon " + renglon + " columna H");
            }

            var xl_claveEmpleadoInformo = rango["I" + renglon].GetValue<int>();
            if (xl_claveEmpleadoInformo == 0)
            {
                throw new Exception("Debe ingresar una CLAVE EMPLEADO INFORMO valida en el renglon " + renglon + " columna I");
            }

            var xl_descripcionSuceso = rango["J" + renglon].GetValue<string>();
            if (string.IsNullOrEmpty(xl_descripcionSuceso))
            {
                throw new Exception("Debe ingresar una DESCRIPCION SUCESO en el renglon " + renglon + " columna J");
            }

            var xl_clasificacion = rango["K" + renglon].GetValue<int>();
            if (xl_clasificacion == 0)
            {
                throw new Exception("Debe ingresar una CLASIFICACION valida en el renglon " + renglon + " columna K");
            }

            var xl_procedimiento = rango["L" + renglon].GetValue<int>();
            if (xl_procedimiento == 0)
            {
                throw new Exception("Debe ingresar un PROCEDIMIENTO valido en el renglon " + renglon + " columna L");
            }

            var xl_clasificacionGeneral = rango["AK" + renglon].GetValue<int>();
            if (xl_clasificacionGeneral == 0)
            {
                throw new Exception("Debe ingresar una CLASIFICACIÓN GENERAL valida en el renglon " + renglon + " columna AK");
            }

            var xl_subclasificacionDepartamento = rango["AL" + renglon].GetValue<int>();
            if (xl_subclasificacionDepartamento == 0)
            {
                throw new Exception("Debe ingresar una SUBCLASIFICACION DEPARTAMENTO valida en el renglon " + renglon + " columna AL");
            }
            #endregion

            #region actos
            if (xl_tipoRiesgo == "ACTO")
            {
                var xl_tipoActo = rango["C" + renglon].GetValue<string>();
                if (string.IsNullOrEmpty(xl_tipoActo))
                {
                    throw new Exception("El campo TIPO ACTO debe tener información en el renglon " + renglon + " columna C");
                }
                else
                {
                    xl_tipoActo = xl_tipoActo.Trim().ToUpper();
                    if (xl_tipoActo != "SEGURO" && xl_tipoActo != "INSEGURO")
                    {
                        throw new Exception("No se reconoce el TIPO ACTO en el renglon " + renglon + " columna C");
                    }

                    var xl_claveEmpleado = rango["N" + renglon].GetValue<int>();
                    if (xl_claveEmpleado == 0)
                    {
                        throw new Exception("Debe ingresar una CLAVE EMPLEADO valida en el renglon " + renglon + " columna N");
                    }

                    var xl_externo = rango["P" + renglon].GetValue<string>();
                    xl_externo = string.IsNullOrEmpty(xl_externo) ? "" : xl_externo.Trim().ToUpper();
                    if (xl_externo == "X")
                    {
                        var xl_nombreEmpleado = rango["O" + renglon].GetValue<string>();
                        if (string.IsNullOrEmpty(xl_nombreEmpleado))
                        {
                            throw new Exception("Debe ingresar un NOMBRE EMPLEADO valido en el renglon " + renglon + " columna O");
                        }

                        var xl_fechaIngreso = rango["Q" + renglon].GetValue<string>();
                        if (string.IsNullOrEmpty(xl_fechaIngreso))
                        {
                            throw new Exception("Debe ingresar una FECHA INGRESO valida (DD/MM/YYYY) en el renglon " + renglon + " columna Q");
                        }
                        else
                        {
                            xl_fechaIngreso = xl_fechaIngreso.Trim();
                            DateTime fechaIngreso = new DateTime();
                            if (!DateTime.TryParse(xl_fechaIngreso, out fechaIngreso))
                            {
                                throw new Exception("Debe ingresar una FECHA INGRESO valida (DD/MM/YYYY) en el renglon " + renglon + " columna Q");
                            }
                        }

                        var xl_puestoEmpleado = rango["R" + renglon].GetValue<string>();
                        if (string.IsNullOrEmpty(xl_puestoEmpleado))
                        {
                            throw new Exception("Debe ingresar un PUESTO EMPLEADO en el renglon " + renglon + " columna R");
                        }

                        var xl_contratista = rango["S" + renglon].GetValue<int>();
                        if (xl_contratista == 0)
                        {
                            throw new Exception("Debe ingresar un número de CONSTRATISTA valido en el renglon " + renglon + " columna S");
                        }
                    }

                    if (xl_tipoActo == "INSEGURO")
                    {
                        var xl_tipoInfraccion = rango["T" + renglon].GetValue<int>();
                        if (xl_tipoInfraccion == 0)
                        {
                            throw new Exception("Debe ingresar un número de TIPO INFRACCION valido en el renglon " + renglon + " columna T");
                        }

                        var xl_compromisoPersonal = rango["AJ" + renglon].GetValue<string>();
                        if (string.IsNullOrEmpty(xl_compromisoPersonal))
                        {
                            throw new Exception("Debe ingresar un COMPROMISO PERSONAL en el renglon " + renglon + " columna AJ");
                        }
                    }
                }
            }
            #endregion

            #region condiciones
            if (xl_tipoRiesgo == "CONDICION" || xl_tipoRiesgo == "CONDICIÓN")
            {
                var xl_nivelPrioridad = rango["M" + renglon].GetValue<int>();
                if (xl_nivelPrioridad == 0)
                {
                    throw new Exception("Debe ingresar un NIVEL PRIORIDAD valido en el renglon " + renglon + " columna M");
                }
            }
            #endregion
        }

        public Dictionary<string, object> GuardarExcelActoCondicionCargaMasiva(HttpPostedFileBase _archivoExcel)
        {
            var resultado = new Dictionary<string, object>();

            //using (var transaccion = _context.Database.BeginTransaction())
            //{
            using (var excel = new ExcelPackage(_archivoExcel.InputStream))
            {
                try
                {
                    #region SE GUARDAR EL ARCHIVO
                    var fechaArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff");
                    var ruta = archivoFS.getUrlDelServidor(1023);

                    var idUsuario = vSesiones.sesionUsuarioDTO.id.ToString();
                    var extension = System.IO.Path.GetExtension(_archivoExcel.FileName);
                    var archivoNombreNuevo = fechaArchivo + "_UsuarioID_" + (int)vSesiones.sesionUsuarioDTO.id + extension;

#if DEBUG
                    DirectoryInfo directorio = new DirectoryInfo(@"c:\CARGA_MASIVA_EXCEL\" + idUsuario);
#else
                    DirectoryInfo directorio = new DirectoryInfo(ruta);
#endif
                    if (!directorio.Exists)
                        directorio.Create();

                    var pathCompleto = System.IO.Path.Combine(directorio.ToString(), archivoNombreNuevo);

                    _archivoExcel.SaveAs(pathCompleto);
                    #endregion

                    var hoja = excel.Workbook.Worksheets[1];
                    int renglonInicial = 1;
                    for (int i = 2; i <= hoja.Dimension.End.Row; i++)
                    {
                        if (!string.IsNullOrEmpty(hoja.Cells["A" + i].GetValue<string>()) && hoja.Cells["A" + i].GetValue<string>().ToUpper() == "FOLIO")
                        {
                            renglonInicial = i + 1;
                            break;
                        }
                    }

                    List<ActoDTO> lstActoDTO = new List<ActoDTO>();
                    tblSAC_Acto objActo = null;
                    int renglonNulo = 0;
                    for (int i = renglonInicial; i <= hoja.Dimension.End.Row; i++)
                    {
                        if (hoja.Cells["A" + i].Value != null)
                        {
                            if (objActo == null)
                            {
                                renglonNulo = 0;

                                #region SE CONSTRUYE OBJETO EN BASE A LA INFORMACIÓN POR CADA ROW DEL EXCEL
                                ActoDTO objActoDTO = new ActoDTO();

                                string nombreImagen = hoja.Cells["A" + i].GetValue<string>() ?? string.Empty; // NOMBRE DE LA IMAGEN (COLUMNA: FOLIO)

                                #region SE OBTIENE EL TIPO DE RIESGO
                                string tipoRiesgo = hoja.Cells["B" + i].GetValue<string>() ?? string.Empty;
                                if (!string.IsNullOrEmpty(tipoRiesgo))
                                {
                                    if (tipoRiesgo.Trim().ToUpper() == "ACTO")
                                        objActoDTO.tipoRiesgo = TipoRiesgo.Acto;
                                    else if (tipoRiesgo.Trim().ToUpper() == "CONDICION" || tipoRiesgo.Trim().ToUpper() == "CONDICIÓN")
                                        objActoDTO.tipoRiesgo = TipoRiesgo.Condicion;
                                }
                                #endregion

                                #region SE OBTIENE EL TIPO DE ACTO
                                string tipoActo = hoja.Cells["C" + i].GetValue<string>() ?? string.Empty;
                                if (!string.IsNullOrEmpty(tipoActo))
                                {
                                    if (tipoActo.Trim().ToUpper() == "SEGURO")
                                        objActoDTO.tipoActo = TipoActo.Seguro;
                                    else if (tipoActo.Trim().ToUpper() == "INSEGURO")
                                        objActoDTO.tipoActo = TipoActo.Inseguro;
                                }
                                #endregion

                                #region SE OBTIENE EL ID AGRUPACION
                                string empresa = hoja.Cells["D" + i].GetValue<string>() ?? string.Empty;
                                string cc = hoja.Cells["E" + i].GetValue<string>() ?? string.Empty;
                                if (!string.IsNullOrEmpty(empresa) && !string.IsNullOrEmpty(cc))
                                {
                                    if (empresa.Trim().ToUpper() == "CONSTRUPLAN" || empresa.Trim().ToUpper() == "CP")
                                    {
                                        // SE BUSCA LA AGRUPACIÓN DE CONSTRUPLAN
                                        objActoDTO.idAgrupacion = _context.tblS_IncidentesAgrupacionCC.Where(w => cc.Contains(w.nomAgrupacion) && w.esActivo).Select(s => s.id).FirstOrDefault();
                                        objActoDTO.idEmpresa = 0;
                                    }
                                    else
                                    {
                                        // SE BUSCA LA AGRUPACION DEL CONTRATISTA
                                        #region EXPLICACIÓN DE idEmpresa
                                        /* 
                                             * idEmpresa = 0: ES UNA AGRUPACION DE CC DE CONSTRUPLAN
                                             * idEmpresa = 1000: ES UN SOLO CONTRATISTA
                                             * idEmpresa = 2000: ES UNA AGRUPACION DE CONTRATISTAS
                                            */
                                        #endregion
                                        objActoDTO.idAgrupacion = _context.tblS_IncidentesAgrupacionContratistas.Where(w => cc.Contains(w.nomAgrupacion) && w.esActivo).Select(s => s.id).FirstOrDefault();
                                        objActoDTO.idEmpresa = 2000;
                                    }
                                }
                                #endregion

                                #region SE OBTIENE ID DEPARTAMENTO
                                string departamento = hoja.Cells["F" + i].GetValue<string>() ?? string.Empty;
                                if (!string.IsNullOrEmpty(departamento))
                                    objActoDTO.departamentoID = _context.tblS_IncidentesDepartamentos.Where(w => departamento.Contains(w.departamento)).Select(s => s.id).FirstOrDefault();
                                #endregion

                                DateTime fechaActoCondicion = hoja.Cells["G" + i].GetValue<DateTime>(); // FALTA POR ASIGNAR A ActoDTO

                                #region SE OBTIENE EL NOMBRE DEL SUPERVISOR
                                int claveSupervisor = hoja.Cells["H" + i].GetValue<int>();
                                if (claveSupervisor > 0)
                                {
                                    string strClaveSupervisor = claveSupervisor.ToString();
                                    objActoDTO.claveSupervisor = claveSupervisor;
                                    objActoDTO.nombreSupervisor = _context.tblP_Usuario.Where(w => w.cveEmpleado == strClaveSupervisor).Select(s => s.nombre + " " + s.apellidoPaterno + " " + s.apellidoMaterno).FirstOrDefault();
                                }
                                #endregion

                                #region SE OBTIENE EL NOMBRE DE LA PERSONA QUE INFORMO
                                int claveEmpleadoInformo = hoja.Cells["I" + i].GetValue<int>();
                                if (claveEmpleadoInformo > 0)
                                {
                                    string strClaveInformo = claveEmpleadoInformo.ToString();
                                    objActoDTO.claveInformo = claveEmpleadoInformo;
                                    objActoDTO.nombreInformo = _context.tblP_Usuario.Where(w => w.cveEmpleado == strClaveInformo).Select(s => s.nombre + " " + s.apellidoPaterno + " " + s.apellidoMaterno).FirstOrDefault();
                                }
                                #endregion

                                string descripcionActoCondicion = hoja.Cells["J" + i].GetValue<string>() ?? string.Empty;
                                objActoDTO.descripcion = descripcionActoCondicion;

                                #region SE OBTIENE CLASIFICACION ID
                                string clasificacion = hoja.Cells["K" + i].GetValue<string>() ?? string.Empty;
                                if (!string.IsNullOrEmpty(clasificacion))
                                {
                                    List<tblSAC_Clasificacion> lstClasificaciones = _context.tblSAC_Clasificacion.ToList();
                                    objActoDTO.clasificacionID = lstClasificaciones.Where(w => w.tipoRiesgo == objActoDTO.tipoRiesgo && clasificacion.Contains(w.descripcion)).Select(s => s.id).FirstOrDefault();
                                }
                                #endregion

                                #region SE OBTIENE PROCEDIMIENTO ID
                                string procedimiento = hoja.Cells["L" + i].GetValue<string>() ?? string.Empty;
                                if (!string.IsNullOrEmpty(procedimiento))
                                    objActoDTO.procedimientoID = _context.tblS_IncidentesTipoProcedimientosViolados.Where(w => procedimiento.Contains(w.Procedimineto)).Select(s => s.id).FirstOrDefault();
                                #endregion
                                #endregion

                                if (objActoDTO.tipoRiesgo == TipoRiesgo.Acto)
                                {
                                    #region TIPO RIESGO ACTO
                                    int claveEmpleado = hoja.Cells["N" + i].GetValue<int>();
                                    string nombreEmpleado = hoja.Cells["O" + i].GetValue<string>() ?? string.Empty;
                                    bool externo = hoja.Cells["P" + i].GetValue<string>() == "X" ? true : false;
                                    DateTime fechaAlta = hoja.Cells["Q" + i].GetValue<DateTime>();
                                    string puestoEmpleado = hoja.Cells["R" + i].GetValue<string>() ?? string.Empty;
                                    string contratista = hoja.Cells["S" + i].GetValue<string>() ?? string.Empty;
                                    int tipoInfraccion = hoja.Cells["T" + i].GetValue<int>();

                                    // CAUSADO POR
                                    bool malEntendido = hoja.Cells["U" + i].GetValue<string>() == "X" ? true : false;
                                    bool noSabia = hoja.Cells["V" + i].GetValue<string>() == "X" ? true : false;
                                    bool malDisenio = hoja.Cells["W" + i].GetValue<string>() == "X" ? true : false;
                                    bool noQueria = hoja.Cells["X" + i].GetValue<string>() == "X" ? true : false;
                                    bool fallaInduccion = hoja.Cells["Y" + i].GetValue<string>() == "X" ? true : false;
                                    bool poInapropiado = hoja.Cells["Z" + i].GetValue<string>() == "X" ? true : false;
                                    bool otrosCausadoPor = hoja.Cells["AA" + i].GetValue<string>() == "X" ? true : false;

                                    // ACCIONES CORRECTIVAS
                                    bool escucharlo = hoja.Cells["AB" + i].GetValue<string>() == "X" ? true : false;
                                    bool inducirlo = hoja.Cells["AC" + i].GetValue<string>() == "X" ? true : false;
                                    bool capacitarlo = hoja.Cells["AD" + i].GetValue<string>() == "X" ? true : false;
                                    bool orientarlo = hoja.Cells["AE" + i].GetValue<string>() == "X" ? true : false;
                                    bool conscientizarlo = hoja.Cells["AF" + i].GetValue<string>() == "X" ? true : false;
                                    bool amonestarlo = hoja.Cells["AG" + i].GetValue<string>() == "X" ? true : false;
                                    bool investigarlo = hoja.Cells["AH" + i].GetValue<string>() == "X" ? true : false;
                                    bool otrosAccionesCorrectivas = hoja.Cells["AI" + i].GetValue<string>() == "X" ? true : false;

                                    string compromisoPersonal = hoja.Cells["AJ" + i].GetValue<string>() ?? string.Empty;
                                    #endregion
                                }
                                else if (objActoDTO.tipoRiesgo == TipoRiesgo.Condicion)
                                {
                                    #region TIPO RIESGO CONDICIÓN
                                    string nivelPrioridad = hoja.Cells["M" + i].GetValue<string>() ?? string.Empty;
                                    #endregion
                                }

                                //_context.tblC_Nom_Nomina.Add(nomina);
                                //_context.SaveChanges();
                            }
                        }
                        else
                        {
                            renglonNulo++;
                            objActo = null;

                            if (renglonNulo == 2)
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //transaccion.Rollback();

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);

                    LogError(0, 0, "ActoCondicionController", "CargarRaya", ex, AccionEnum.AGREGAR, 0, 0);
                }
            }
            //}

            return resultado;
        }

        public Dictionary<string, object> DescargarFormato()
        {
            try
            {
                var nombreArchivo = "FormatoExcel.xlsx";
                var rutaArchivo = "";
#if DEBUG
                rutaArchivo = RutaLocal + @"\" + nombreArchivo;
#else
                rutaArchivo = RutaBase + @"\" + nombreArchivo;
#endif
                var fileStream = GlobalUtils.GetFileAsStream(rutaArchivo);
                string name = Path.GetFileName(rutaArchivo);

                resultado.Add("name", name);
                resultado.Add("archivo", fileStream);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarFormato", e, AccionEnum.DESCARGAR, 0, 0);
                return null;
            }
        }

        public List<CapturaActoCondicion_AutocompleteEmpleadoDTO> GetInfoEmpleado(string term)
        {
            try
            {
                //                var query_infoEmpleados = new OdbcConsultaDTO();
                //                query_infoEmpleados.consulta = string.Format(@"
                //                    SELECT TOP 5
                //                        emp.clave_empleado,
                //                        TRIM(emp.nombre) AS nombre,
                //                        TRIM(emp.ape_paterno) as ape_paterno,
                //                        emp.ape_materno,
                //                        emp.fecha_alta,
                //                        p.descripcion AS puestoDescripcion,
                //                        (
                //                            emp.nombre + ' ' + emp.ape_paterno +
                //                            (
                //                                CASE
                //                                    WHEN ape_materno IS NULL or ape_materno = '' THEN ''
                //                                    ELSE ' ' + TRIM(ape_materno)
                //                                END
                //                            )
                //                         ) AS nombreCompleto
                //                    FROM
                //                        sn_empleados AS emp
                //                    INNER JOIN
                //                        si_puestos AS p
                //                        ON
                //                            p.puesto = emp.puesto
                //                    WHERE
                //                        emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno LIKE ? AND
                //                        emp.estatus_empleado = 'A'");
                //                query_infoEmpleados.parametros.Add(new OdbcParameterDTO
                //                {
                //                    nombre = "nombre",
                //                    tipo = OdbcType.NVarChar,
                //                    valor = "%" + term + "%"
                //                });

                //                var consultaCplan = _contextEnkontrol.Select<CapturaActoCondicion_InfoEmpleadoDTO>(EnkontrolAmbienteEnum.RhCplan, query_infoEmpleados);
                //                var consultaArre = _contextEnkontrol.Select<CapturaActoCondicion_InfoEmpleadoDTO>(EnkontrolAmbienteEnum.RhArre, query_infoEmpleados);

                var consultaCplan = _context.Select<CapturaActoCondicion_InfoEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT TOP 5
                                    emp.clave_empleado,
                                    TRIM(emp.nombre) AS nombre,
                                    TRIM(emp.ape_paterno) as ape_paterno,
                                    emp.ape_materno,
                                    emp.fecha_alta,
                                    p.descripcion AS puestoDescripcion,
                                    (
                                        emp.nombre + ' ' + emp.ape_paterno +
                                        (
                                            CASE
                                                WHEN ape_materno IS NULL or ape_materno = '' THEN ''
                                                ELSE ' ' + TRIM(ape_materno)
                                            END
                                        )
                                     ) AS nombreCompleto
                                FROM
                                    tblRH_EK_Empleados AS emp
                                INNER JOIN
                                    tblRH_EK_Puestos AS p
                                    ON
                                        p.puesto = emp.puesto
                                WHERE
                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno LIKE '%@term%' AND
                                    emp.estatus_empleado = 'A'",
                    parametros = new { term }
                });

                var consultaGCPLAN = _context.Select<CapturaActoCondicion_InfoEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT TOP 5
                                    emp.clave_empleado,
                                    TRIM(emp.nombre) AS nombre,
                                    TRIM(emp.ape_paterno) as ape_paterno,
                                    emp.ape_materno,
                                    emp.fecha_alta,
                                    p.descripcion AS puestoDescripcion,
                                    (
                                        emp.nombre + ' ' + emp.ape_paterno +
                                        (
                                            CASE
                                                WHEN ape_materno IS NULL or ape_materno = '' THEN ''
                                                ELSE ' ' + TRIM(ape_materno)
                                            END
                                        )
                                     ) AS nombreCompleto
                                FROM
                                    tblRH_EK_Empleados AS emp
                                INNER JOIN
                                    tblRH_EK_Puestos AS p
                                    ON
                                        p.puesto = emp.puesto
                                WHERE
                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno LIKE '%@term%' AND
                                    emp.estatus_empleado = 'A'",
                    parametros = new { term }
                });

                var consultaArre = _context.Select<CapturaActoCondicion_InfoEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT TOP 5
                                    emp.clave_empleado,
                                    TRIM(emp.nombre) AS nombre,
                                    TRIM(emp.ape_paterno) as ape_paterno,
                                    emp.ape_materno,
                                    emp.fecha_alta,
                                    p.descripcion AS puestoDescripcion,
                                    (
                                        emp.nombre + ' ' + emp.ape_paterno +
                                        (
                                            CASE
                                                WHEN ape_materno IS NULL or ape_materno = '' THEN ''
                                                ELSE ' ' + TRIM(ape_materno)
                                            END
                                        )
                                     ) AS nombreCompleto
                                FROM
                                    tblRH_EK_Empleados AS emp
                                INNER JOIN
                                    tblRH_EK_Puestos AS p
                                    ON
                                        p.puesto = emp.puesto
                                WHERE
                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno LIKE '%@term%' AND
                                    emp.estatus_empleado = 'A'",
                    parametros = new { term }
                });

                var empleados = new List<CapturaActoCondicion_AutocompleteEmpleadoDTO>();

                foreach (var item in consultaCplan)
                {
                    item.empresa = "CONSTRUPLAN";

                    consultaArre.RemoveAll(x => x.clave_empleado == item.clave_empleado);

                    var empleadoAutocomplete = new CapturaActoCondicion_AutocompleteEmpleadoDTO();
                    empleadoAutocomplete.id = item.clave_empleado + "-" + item.puestoDescripcion + "-" + item.fecha_alta.Value.ToString("dd/MM/yyyy");
                    empleadoAutocomplete.label = item.nombreCompleto + " [" + item.empresa + "]";
                    empleados.Add(empleadoAutocomplete);
                }

                foreach (var item in consultaGCPLAN)
                {
                    item.empresa = "CONSTRUPLAN";

                    consultaArre.RemoveAll(x => x.clave_empleado == item.clave_empleado);

                    var empleadoAutocomplete = new CapturaActoCondicion_AutocompleteEmpleadoDTO();
                    empleadoAutocomplete.id = item.clave_empleado + "-" + item.puestoDescripcion + "-" + item.fecha_alta.Value.ToString("dd/MM/yyyy");
                    empleadoAutocomplete.label = item.nombreCompleto + " [" + item.empresa + "]";
                    empleados.Add(empleadoAutocomplete);
                }

                foreach (var item in consultaArre)
                {
                    item.empresa = "ARRENDADORA";

                    var empleadoAutocomplete = new CapturaActoCondicion_AutocompleteEmpleadoDTO();
                    empleadoAutocomplete.id = item.clave_empleado + "-" + item.puestoDescripcion + "-" + item.fecha_alta.Value.ToString("dd/MM/yyyy");
                    empleadoAutocomplete.label = item.nombreCompleto + " [" + item.empresa + "]";
                    empleados.Add(empleadoAutocomplete);
                }

                return empleados;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetInfoEmpleado", e, AccionEnum.CONSULTA, 0, 0);
            }

            return new List<CapturaActoCondicion_AutocompleteEmpleadoDTO>();
        }

        public List<CapturaActoCondicion_AutocompleteEmpleadoDTO> GetInfoEmpleadoInternoContratista(string term, bool esContratista, int idEmpresaContratista)
        {
            List<CapturaActoCondicion_AutocompleteEmpleadoDTO> resultado = new List<CapturaActoCondicion_AutocompleteEmpleadoDTO>();

            if (!esContratista)
            {
                #region SE OBTIENE INFORMACIÓN DE PERSONAL INTERNO

                string termFormateado = "%" + term.Replace("[", "[[]").Replace("%", "[%]") + "%";

                var resultadoCP = _context.Select<CapturaActoCondicion_AutocompleteEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"
                        SELECT
                            e.clave_empleado AS id, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno + ' [CONSTRUPLAN]') AS label
                        FROM tblRH_EK_Empleados as e
                        WHERE UPPER(TRIM(e.nombre + e.ape_paterno + e.ape_materno)) LIKE @term
                        ORDER BY e.ape_paterno DESC",
                    parametros = new { term = termFormateado }
                });

                var resultadoGCPLAN = _context.Select<CapturaActoCondicion_AutocompleteEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"
                        SELECT
                            e.clave_empleado AS id, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno + ' [CONSTRUPLAN]') AS label
                        FROM tblRH_EK_Empleados as e
                        WHERE UPPER(TRIM(e.nombre + e.ape_paterno + e.ape_materno)) LIKE @term
                        ORDER BY e.ape_paterno DESC",
                    parametros = new { term = termFormateado }
                });

                var resultadoARR = _context.Select<CapturaActoCondicion_AutocompleteEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"
                        SELECT
                            e.clave_empleado AS id, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno + ' [ARRENDADORA]') AS label
                        FROM tblRH_EK_Empleados as e
                        WHERE UPPER(TRIM(e.nombre + e.ape_paterno + e.ape_materno)) LIKE @term
                        ORDER BY e.ape_paterno DESC",
                    parametros = new { term = termFormateado }
                });

                resultado.AddRange(resultadoCP);
                resultado.AddRange(resultadoGCPLAN);
                resultado.AddRange(resultadoARR);
                #endregion
            }
            else
            {
                #region SE OBTIENE INFORMACIÓN DE EMPLEADOS EN CASO DE SER CONTRATISTA EL QUE INICIO SESIÓN
                var termFormateado = term.Replace(" ", "").Trim().ToUpper();

                List<tblS_IncidentesEmpleadoContratistas> lstEmpleadosContratistas = new List<tblS_IncidentesEmpleadoContratistas>();
                if ((int)vSesiones.sesionUsuarioDTO.idPerfil == 4 && idEmpresaContratista > 0)
                    lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.esActivo && x.idEmpresaContratista == idEmpresaContratista).ToList().Where(x => (x.nombre + x.apePaterno + x.apeMaterno).Replace(" ", "").Trim().ToUpper().Contains(termFormateado)).ToList();
                else if ((int)vSesiones.sesionUsuarioDTO.idPerfil != 4)
                    lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.esActivo).ToList().Where(x => (x.nombre + x.apePaterno + x.apeMaterno).Replace(" ", "").Trim().ToUpper().Contains(termFormateado)).ToList();

                if (lstEmpleadosContratistas.Count() > 0)
                {
                    empleadoIncidenteDTO objEmpleadoContratista = new empleadoIncidenteDTO();
                    objEmpleadoContratista.claveEmpleado = lstEmpleadosContratistas[0].id;
                    objEmpleadoContratista.nombreEmpleado = lstEmpleadosContratistas[0].nombre + " " + lstEmpleadosContratistas[0].apePaterno + " " + lstEmpleadosContratistas[0].apeMaterno;
                    objEmpleadoContratista.puestoEmpleado = lstEmpleadosContratistas[0].puesto;

                    resultado.AddRange(lstEmpleadosContratistas.Select(x => new CapturaActoCondicion_AutocompleteEmpleadoDTO
                    {
                        id = x.id.ToString(),
                        label = x.nombre + " " + x.apePaterno + " " + x.apeMaterno
                    }).ToList());
                }
                #endregion
            }

            return resultado;
        }

        public Dictionary<string, object> DescargarReporteExcel(FiltroActoCondicionDTO filtro)
        {
            try
            {
                var infoReporte = new List<ReporteActoCondicionExcelDTO>();

                /*Limitación a un mes*/
                filtro.fechaInicial = new DateTime(filtro.fechaFinal.Year, filtro.fechaFinal.Month, 1);
                filtro.fechaFinal = new DateTime(filtro.fechaFinal.Year, filtro.fechaFinal.Month, DateTime.DaysInMonth(filtro.fechaFinal.Year, filtro.fechaFinal.Month));

                var listaFormatoImagen = new List<string>
                {
                    ".BMP",
                    ".EMF",
                    ".EXIF",
                    ".GIF",
                    ".ICON",
                    ".JPEG",
                    ".JPG",
                    ".PNG",
                    ".TIFF",
                    ".WMF",
                    ".JFIF"
                };

                #region obtener actos
                var matrizAccionesDisciplinarias = _context.tblSAC_MatrizAccionesDisciplinarias.Where(x => x.estatus);

                var actos = _context.tblSAC_Acto
                    .Where(x =>
                        x.fechaSuceso >= filtro.fechaInicial &&
                        x.fechaSuceso <= filtro.fechaFinal &&
                        x.idAgrupacion == filtro.idAgrupacion &&
                        x.idEmpresa == filtro.idEmpresa &&
                        (filtro.claveSupervisor == 0 ? true : x.claveSupervisor == filtro.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : x.departamentoID == filtro.departamentoID) &&
                        (filtro.estatus == -1 ? true : filtro.estatus == (int)EstatusActoCondicion.Vencido ? x.estatus == EstatusActoCondicion.EnProceso : x.estatus == (EstatusActoCondicion)filtro.estatus) &&
                        x.activo)
                    .Select(x => new ReporteActoCondicionExcelDTO
                    {
                        folio = x.folio,
                        descripcion = x.descripcion,
                        fechaSuceso = x.fechaSuceso,
                        fechaCorreccion = x.fechaProcesoCompleto,
                        numeroInfraccion = x.numeroInfraccion,
                        nombreSupervisor = x.nombreSupervisor,
                        nombreInformo = x.nombreInformo,
                        nombre = x.nombre,
                        puesto = x.puesto,
                        tipoRiesgoDescripcion = "Acto",
                        tipoActo = x.tipoActo == TipoActo.Seguro ? "Seguro" : "Inseguro"
                    }).ToList();

                foreach (var item in actos)
                {
                    if (item.numeroInfraccion.HasValue && item.numeroInfraccion.Value > 0)
                    {
                        item.descripcionInfraccion = matrizAccionesDisciplinarias.First(x => x.numero == item.numeroInfraccion.Value).tipoInfraccion;
                    }
                }

                infoReporte.AddRange(actos);
                #endregion

                #region obtener condiciones
                var condiciones = _context.tblSAC_Condicion
                    .Where(x =>
                        x.fechaSuceso >= filtro.fechaInicial &&
                        x.fechaSuceso <= filtro.fechaFinal &&
                        x.idAgrupacion == filtro.idAgrupacion &&
                        x.idEmpresa == filtro.idEmpresa &&
                        (filtro.claveSupervisor == 0 ? true : x.claveSupervisor == filtro.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : x.departamentoID == filtro.departamentoID) &&
                        (filtro.estatus == -1 ? true : filtro.estatus == (int)EstatusActoCondicion.Vencido ? x.estatus == EstatusActoCondicion.EnProceso : x.estatus == (EstatusActoCondicion)filtro.estatus) &&
                        x.activo)
                    .Select(x => new ReporteActoCondicionExcelDTO
                    {
                        folio = x.folio,
                        descripcion = x.descripcion,
                        fechaSuceso = x.fechaSuceso,
                        fechaCorreccion = x.fechaResolucion,
                        rutaImagenAntes = x.rutaImagenAntes,
                        rutaImagenDespues = x.rutaImagenDespues,
                        accionCorrectiva = x.accionCorrectiva,
                        nombreSupervisor = x.nombreSupervisor,
                        nombreInformo = x.nombreInformo,
                        tipoRiesgoDescripcion = "Condición",
                        nivelPrioridadDescripcion = x.prioridad.descripcion
                    }).ToList();

                infoReporte.AddRange(condiciones);
                #endregion

                #region excel
                using (var excel = new ExcelPackage())
                {
                    #region por hojas
                    //                    foreach (var gbTipoRiesgo in infoReporte.GroupBy(x => x.tipoRiesgoDescripcion))
                    //                    {
                    //                        foreach (var gbYear in gbTipoRiesgo.GroupBy(x => x.fechaSuceso.Year).OrderBy(x => x.Key))
                    //                        {
                    //                            foreach (var gbMes in gbYear.GroupBy(x => x.fechaSuceso.Month).OrderBy(x => x.Key))
                    //                            {
                    //                                var hoja = excel.Workbook.Worksheets.Add(gbTipoRiesgo.Key + "_" + gbYear.Key + "_" + gbMes.Key);

                    //                                #region header
                    //                                var header = new List<HeaderDTO>
                    //                                {
                    //                                    new HeaderDTO{ columna = 1, columnaConLetras = "A", nombreHeader = "NO." },
                    //                                    new HeaderDTO{ columna = 2, columnaConLetras = "B", nombreHeader = "DETECCIÓN DE CONDICIÓN/ACTIVIDAD, RIESGO Y/O CONSECUENCIA" },
                    //                                    new HeaderDTO{ columna = 3, columnaConLetras = "C", nombreHeader = "ACTO/CONDICIÓN" },
                    //                                    new HeaderDTO{ columna = 4, columnaConLetras = "D", nombreHeader = "EVIDENCIA FOTOGRÁFICA" },
                    //                                    new HeaderDTO{ columna = 5, columnaConLetras = "E", nombreHeader = "MEDIDAS CORRECTIVAS A SEGUIR" },
                    //                                    new HeaderDTO{ columna = 6, columnaConLetras = "F", nombreHeader = "FECHA DETECCIÓN" },
                    //                                    new HeaderDTO{ columna = 7, columnaConLetras = "G", nombreHeader = "PRIORIDAD" },
                    //                                    new HeaderDTO{ columna = 8, columnaConLetras = "H", nombreHeader = "FECHA DE CORRECCIÓN" },
                    //                                    new HeaderDTO{ columna = 9, columnaConLetras = "I", nombreHeader = "RESPONSABLE DE RESOLVER" },
                    //                                    new HeaderDTO{ columna = 10, columnaConLetras = "J", nombreHeader = "EVIDENCIA FOTOGRÁFICA DE CORRECCIÓN" },
                    //                                    new HeaderDTO{ columna = 11, columnaConLetras = "K", nombreHeader = "OBSERVACIONES" },
                    //                                    new HeaderDTO{ columna = 12, columnaConLetras = "L", nombreHeader = "SUP. SEG. EN TURNO" },
                    //                                    new HeaderDTO{ columna = 13, columnaConLetras = "M", nombreHeader = "NOMBRE EMPLEADO" },
                    //                                    new HeaderDTO{ columna = 14, columnaConLetras = "N", nombreHeader = "PUESTO" },
                    //                                    new HeaderDTO{ columna = 15, columnaConLetras = "O", nombreHeader = "EMPRESA" },
                    //                                    new HeaderDTO{ columna = 16, columnaConLetras = "P", nombreHeader = "NORMATIVA, PROCEDIMIENTO, REGLAMENTO QUE SE ESTA INCUMPLIENDO" }
                    //                                };

                    //                                int renglonHeader = 5;
                    //                                for (int i = 1; i <= header.Count; i++)
                    //                                {
                    //                                    hoja.Cells[renglonHeader, i].Value = header[i - 1].nombreHeader;
                    //                                }

                    //                                hoja.Row(4).Height = 46.5D;
                    //                                hoja.Cells[1, 1, 4, header.Count].Merge = true;
                    //                                var titulo = hoja.MergedCells[0];
                    //                                hoja.Cells[titulo].Value = "Grupo Construcciones Planificadas S.A. de C.V.\nPrograma de Atención a Seguridad, Salud y Medio Ambiente";
                    //                                hoja.Cells[titulo].Style.Font.Bold = true;
                    //                                hoja.Cells[titulo].Style.Font.Size = 16;
                    //                                hoja.Cells[titulo].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //                                hoja.Cells[titulo].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //                                hoja.Cells[5, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column].AutoFitColumns();


                    //                                var imgLogo = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logo.jpg"));
                    //                                var logo = hoja.Drawings.AddPicture("logo", imgLogo);
                    //                                logo.SetPosition(1, 0, 1, 0);
                    //                                #endregion

                    //                                #region registros
                    //                                int renglonRegistro = renglonHeader + 1;
                    //                                int contador = 0;
                    //                                foreach (var item in gbMes.OrderBy(x => x.fechaSuceso))
                    //                                {
                    //                                    hoja.Cells[renglonRegistro, 1].Value = ++contador;
                    //                                    hoja.Cells[renglonRegistro, 2].Value = item.descripcion;
                    //                                    hoja.Cells[renglonRegistro, 3].Value = item.tipoRiesgoDescripcion;

                    //                                    #region imagenAntes para condiciones
                    //                                    if (!string.IsNullOrEmpty(item.rutaImagenAntes) && listaFormatoImagen.Contains(Path.GetExtension(item.rutaImagenAntes).ToUpper()))
                    //                                    {
                    //                                        hoja.Row(renglonRegistro).Height = 91.5D;
                    //                                        var rutaImagenAntes = "";
                    //#if DEBUG
                    //                                        rutaImagenAntes = RutaLocal + "\\ImagenAntes.jpeg";
                    //#else
                    //                                        rutaImagenAntes = item.rutaImagenAntes;
                    //#endif
                    //                                        using (var imgAntes = Image.FromFile(rutaImagenAntes))
                    //                                        {
                    //                                            Bitmap myBitmap;
                    //                                            ImageCodecInfo myImageCodecInfo;
                    //                                            Encoder myEncoder;
                    //                                            EncoderParameter myEncoderParameter;
                    //                                            EncoderParameters myEncoderParameters;

                    //                                            myBitmap = new Bitmap(imgAntes, 100, 100);
                    //                                            myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    //                                            myEncoder = Encoder.Quality;
                    //                                            myEncoderParameters = new EncoderParameters(1);
                    //                                            myEncoderParameter = new EncoderParameter(myEncoder, 25L);
                    //                                            myEncoderParameters.Param[0] = myEncoderParameter;

                    //                                            using (MemoryStream memStream = new MemoryStream())
                    //                                            {
                    //                                                myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                    //                                                Image newImage = Image.FromStream(memStream);
                    //                                                ImageAttributes imageAttributes = new ImageAttributes();
                    //                                                using (Graphics g = Graphics.FromImage(newImage))
                    //                                                {
                    //                                                    g.InterpolationMode =
                    //                                                      System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //                                                    g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                    //                                                      newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                    //                                                }

                    //                                                var antes = hoja.Drawings.AddPicture(contador.ToString(), newImage);
                    //                                                antes.SetPosition(renglonRegistro - 1, 3, 3, 3);
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                    #endregion

                    //                                    hoja.Cells[renglonRegistro, 5].Value = item.accionCorrectiva ?? "";
                    //                                    hoja.Cells[renglonRegistro, 6].Value = item.fechaSuceso.ToString("dd/MM/yyyy");
                    //                                    hoja.Cells[renglonRegistro, 7].Value = item.nivelPrioridadDescripcion ?? "";
                    //                                    hoja.Cells[renglonRegistro, 8].Value = item.fechaCorreccion.HasValue ? item.fechaCorreccion.Value.ToString("dd/MM/yyyy") : "";
                    //                                    hoja.Cells[renglonRegistro, 9].Value = item.nombreSupervisor;

                    //                                    #region imagenDespues para condiciones
                    //                                    if (!string.IsNullOrEmpty(item.rutaImagenDespues) && listaFormatoImagen.Contains(Path.GetExtension(item.rutaImagenDespues).ToUpper()))
                    //                                    {
                    //                                        var rutaImagenDespues = "";
                    //#if DEBUG
                    //                                        rutaImagenDespues = RutaLocal + "\\ImagenDespues.jpg";
                    //#else
                    //                                        rutaImagenDespues = item.rutaImagenDespues;
                    //#endif
                    //                                        using (var imgDespues = Image.FromFile(rutaImagenDespues))
                    //                                        {
                    //                                            Bitmap myBitmap;
                    //                                            ImageCodecInfo myImageCodecInfo;
                    //                                            Encoder myEncoder;
                    //                                            EncoderParameter myEncoderParameter;
                    //                                            EncoderParameters myEncoderParameters;

                    //                                            myBitmap = new Bitmap(imgDespues, 100, 100);
                    //                                            myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    //                                            myEncoder = Encoder.Quality;
                    //                                            myEncoderParameters = new EncoderParameters(1);
                    //                                            myEncoderParameter = new EncoderParameter(myEncoder, 25L);
                    //                                            myEncoderParameters.Param[0] = myEncoderParameter;

                    //                                            using (MemoryStream memStream = new MemoryStream())
                    //                                            {
                    //                                                myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                    //                                                Image newImage = Image.FromStream(memStream);
                    //                                                ImageAttributes imageAttributes = new ImageAttributes();
                    //                                                using (Graphics g = Graphics.FromImage(newImage))
                    //                                                {
                    //                                                    g.InterpolationMode =
                    //                                                      System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;  //**
                    //                                                    g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                    //                                                      newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                    //                                                }

                    //                                                var despues = hoja.Drawings.AddPicture(contador.ToString() + "(2)", newImage);
                    //                                                despues.SetPosition(renglonRegistro - 1, 3, 9, 3);
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                    #endregion

                    //                                    hoja.Cells[renglonRegistro, 11].Value = "";
                    //                                    hoja.Cells[renglonRegistro, 12].Value = item.nombreInformo;
                    //                                    hoja.Cells[renglonRegistro, 13].Value = item.nombre ?? "";
                    //                                    hoja.Cells[renglonRegistro, 14].Value = item.puesto ?? "";
                    //                                    hoja.Cells[renglonRegistro, 15].Value = "";
                    //                                    hoja.Cells[renglonRegistro, 16].Value = item.descripcionInfraccion ?? "";

                    //                                    renglonRegistro++;
                    //                                }
                    //                                #endregion

                    //                                ExcelRange rangeComplete = hoja.Cells[6, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
                    //                                rangeComplete.Style.WrapText = true;
                    //                                rangeComplete.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //                                hoja.Column(4).Width = 33D;

                    //                                ExcelTable xlsxTable = hoja.Tables.Add(hoja.Cells[5, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column], "Tabla_" + gbTipoRiesgo.Key + "_" + gbYear.Key + "_" + gbMes.Key);
                    //                                xlsxTable.TableStyle = TableStyles.Medium17;
                    //                                xlsxTable.ShowRowStripes = true;
                    //                            }
                    //                        }
                    //                    }
                    #endregion

                    #region toda la info en una hoja
                    var hoja = excel.Workbook.Worksheets.Add("Reporte");

                    #region header
                    var header = new List<HeaderDTO>
                    {
                        new HeaderDTO{ columna = 1, columnaConLetras = "A", nombreHeader = "NO." },
                        new HeaderDTO{ columna = 2, columnaConLetras = "B", nombreHeader = "FOLIO" },
                        new HeaderDTO{ columna = 3, columnaConLetras = "C", nombreHeader = "DETECCIÓN DE CONDICIÓN/ACTIVIDAD, RIESGO Y/O CONSECUENCIA" },
                        new HeaderDTO{ columna = 4, columnaConLetras = "D", nombreHeader = "ACTO/CONDICIÓN" },
                        new HeaderDTO{ columna = 5, columnaConLetras = "E", nombreHeader = "EVIDENCIA FOTOGRÁFICA" },
                        new HeaderDTO{ columna = 6, columnaConLetras = "F", nombreHeader = "MEDIDAS CORRECTIVAS A SEGUIR" },
                        new HeaderDTO{ columna = 7, columnaConLetras = "G", nombreHeader = "FECHA DETECCIÓN" },
                        new HeaderDTO{ columna = 8, columnaConLetras = "H", nombreHeader = "PRIORIDAD" },
                        new HeaderDTO{ columna = 9, columnaConLetras = "I", nombreHeader = "FECHA DE CORRECCIÓN" },
                        new HeaderDTO{ columna = 10, columnaConLetras = "J", nombreHeader = "RESPONSABLE DE RESOLVER" },
                        new HeaderDTO{ columna = 11, columnaConLetras = "K", nombreHeader = "EVIDENCIA FOTOGRÁFICA DE CORRECCIÓN" },
                        new HeaderDTO{ columna = 12, columnaConLetras = "L", nombreHeader = "OBSERVACIONES" },
                        new HeaderDTO{ columna = 13, columnaConLetras = "M", nombreHeader = "SUP. SEG. EN TURNO" },
                        new HeaderDTO{ columna = 14, columnaConLetras = "N", nombreHeader = "NOMBRE EMPLEADO" },
                        new HeaderDTO{ columna = 15, columnaConLetras = "O", nombreHeader = "PUESTO" },
                        new HeaderDTO{ columna = 16, columnaConLetras = "P", nombreHeader = "EMPRESA" },
                        new HeaderDTO{ columna = 17, columnaConLetras = "Q", nombreHeader = "NORMATIVA, PROCEDIMIENTO, REGLAMENTO QUE SE ESTA INCUMPLIENDO" }
                    };

                    int renglonHeader = 5;
                    for (int i = 1; i <= header.Count; i++)
                    {
                        hoja.Cells[renglonHeader, i].Value = header[i - 1].nombreHeader;
                    }

                    hoja.Row(4).Height = 46.5D;
                    hoja.Cells[1, 1, 4, header.Count].Merge = true;
                    var titulo = hoja.MergedCells[0];
                    hoja.Cells[titulo].Value = "Grupo Construcciones Planificadas S.A. de C.V.\nPrograma de Atención a Seguridad, Salud y Medio Ambiente";
                    hoja.Cells[titulo].Style.Font.Bold = true;
                    hoja.Cells[titulo].Style.Font.Size = 16;
                    hoja.Cells[titulo].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    hoja.Cells[titulo].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    hoja.Cells[5, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column].AutoFitColumns();


                    var imgLogo = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logo.jpg"));
                    var logo = hoja.Drawings.AddPicture("logo", imgLogo);
                    logo.SetPosition(1, 0, 1, 0);
                    #endregion

                    #region registros
                    int renglonRegistro = renglonHeader + 1;
                    int contador = 0;
                    foreach (var item in infoReporte.OrderBy(x => x.fechaSuceso))
                    {
                        hoja.Cells[renglonRegistro, 1].Value = ++contador;
                        hoja.Cells[renglonRegistro, 2].Value = item.folio;
                        hoja.Cells[renglonRegistro, 3].Value = item.descripcion;
                        hoja.Cells[renglonRegistro, 4].Value = item.tipoRiesgoDescripcion;

                        #region imagenAntes para condiciones
                        hoja.Row(renglonRegistro).Height = 91.5D;
                        if (!string.IsNullOrEmpty(item.rutaImagenAntes) && listaFormatoImagen.Contains(Path.GetExtension(item.rutaImagenAntes).ToUpper()))
                        {
                            var rutaImagenAntes = "";
#if DEBUG
                            rutaImagenAntes = RutaLocal + "\\ImagenAntes.jpeg";
#else
                            rutaImagenAntes = item.rutaImagenAntes;
#endif
                            using (var imgAntes = Image.FromFile(rutaImagenAntes))
                            {
                                Bitmap myBitmap;
                                ImageCodecInfo myImageCodecInfo;
                                Encoder myEncoder;
                                EncoderParameter myEncoderParameter;
                                EncoderParameters myEncoderParameters;

                                myBitmap = new Bitmap(imgAntes, 100, 100);
                                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                                myEncoder = Encoder.Quality;
                                myEncoderParameters = new EncoderParameters(1);
                                myEncoderParameter = new EncoderParameter(myEncoder, 45L);
                                myEncoderParameters.Param[0] = myEncoderParameter;

                                using (MemoryStream memStream = new MemoryStream())
                                {
                                    myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                                    Image newImage = Image.FromStream(memStream);
                                    ImageAttributes imageAttributes = new ImageAttributes();
                                    using (Graphics g = Graphics.FromImage(newImage))
                                    {
                                        g.InterpolationMode =
                                          System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                                          newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                                    }

                                    var antes = hoja.Drawings.AddPicture(contador.ToString(), newImage);
                                    antes.SetPosition(renglonRegistro - 1, 3, 4, 3);
                                }
                            }
                        }
                        #endregion

                        hoja.Cells[renglonRegistro, 6].Value = item.accionCorrectiva ?? "";
                        hoja.Cells[renglonRegistro, 7].Value = item.fechaSuceso.ToString("dd/MM/yyyy");
                        hoja.Cells[renglonRegistro, 8].Value = item.nivelPrioridadDescripcion ?? "";
                        hoja.Cells[renglonRegistro, 9].Value = item.fechaCorreccion.HasValue ? item.fechaCorreccion.Value.ToString("dd/MM/yyyy") : "";
                        hoja.Cells[renglonRegistro, 10].Value = item.nombreSupervisor;

                        #region imagenDespues para condiciones
                        if (!string.IsNullOrEmpty(item.rutaImagenDespues) && listaFormatoImagen.Contains(Path.GetExtension(item.rutaImagenDespues).ToUpper()))
                        {
                            var rutaImagenDespues = "";
#if DEBUG
                            rutaImagenDespues = RutaLocal + "\\ImagenDespues.jpg";
#else
                            rutaImagenDespues = item.rutaImagenDespues;
#endif
                            using (var imgDespues = Image.FromFile(rutaImagenDespues))
                            {
                                Bitmap myBitmap;
                                ImageCodecInfo myImageCodecInfo;
                                Encoder myEncoder;
                                EncoderParameter myEncoderParameter;
                                EncoderParameters myEncoderParameters;

                                myBitmap = new Bitmap(imgDespues, 100, 100);
                                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                                myEncoder = Encoder.Quality;
                                myEncoderParameters = new EncoderParameters(1);
                                myEncoderParameter = new EncoderParameter(myEncoder, 45L);
                                myEncoderParameters.Param[0] = myEncoderParameter;

                                using (MemoryStream memStream = new MemoryStream())
                                {
                                    myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                                    Image newImage = Image.FromStream(memStream);
                                    ImageAttributes imageAttributes = new ImageAttributes();
                                    using (Graphics g = Graphics.FromImage(newImage))
                                    {
                                        g.InterpolationMode =
                                          System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;  //**
                                        g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                                          newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                                    }

                                    var despues = hoja.Drawings.AddPicture(contador.ToString() + "(2)", newImage);
                                    despues.SetPosition(renglonRegistro - 1, 3, 10, 3);
                                }
                            }
                        }
                        #endregion

                        hoja.Cells[renglonRegistro, 12].Value = "";
                        hoja.Cells[renglonRegistro, 13].Value = item.nombreInformo;
                        hoja.Cells[renglonRegistro, 14].Value = item.nombre ?? "";
                        hoja.Cells[renglonRegistro, 15].Value = item.puesto ?? "";
                        hoja.Cells[renglonRegistro, 16].Value = "";
                        hoja.Cells[renglonRegistro, 17].Value = item.descripcionInfraccion ?? "";

                        renglonRegistro++;
                    }
                    #endregion

                    ExcelRange rangeComplete = hoja.Cells[6, 1, hoja.Dimension.End.Row + 1, hoja.Dimension.End.Column];
                    rangeComplete.Style.WrapText = true;
                    rangeComplete.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    hoja.Column(5).Width = 33D;

                    ExcelTable xlsxTable = hoja.Tables.Add(hoja.Cells[5, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column], "Tabla");
                    xlsxTable.TableStyle = TableStyles.Medium17;
                    xlsxTable.ShowRowStripes = true;
                    #endregion

                    var bytes = new MemoryStream();
                    using (var ms = new MemoryStream())
                    {
                        excel.SaveAs(ms);
                        bytes = ms;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, bytes);
                }
                #endregion
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "DescargarReporteExcel", e, AccionEnum.CONSULTA, 0, filtro);
            }

            return resultado;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public Dictionary<string, object> ObtenerClasificacionesGenerales()
        {
            try
            {
                var listaClasificaciones = _context.tblSAC_ClasificacionGeneral.Where(x => x.estatus).Select(x => new
                {
                    Value = x.id,
                    Text = x.descripcion,
                    Prefijo = ""
                }).Distinct().OrderBy(x => x.Value).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listaClasificaciones);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerClasificacionesGenerales", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region Dashboard
        public Dictionary<string, object> CargarDatosDashboard(FiltroDashboardDTO filtro)
        {
            try
            {
                filtro.fechaFinal = filtro.fechaFinal.AddHours(23).AddMinutes(59);

                List<string> grupos = new List<string>();
                if (filtro.arrGrupos != null)
                {
                    if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                    {
                        foreach (var item in filtro.arrGrupos)
	                    {
                            item.idEmpresa = vSesiones.sesionEmpresaActual;
	                    }
                    }
                    grupos = filtro.arrGrupos.Select(x => x.idEmpresa + "-" + x.idAgrupacion).ToList();
                }

                var actosActivos = _context.tblSAC_Acto
                    .Where(x =>
                        (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                        (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID) &&
                        x.activo)
                    .Select(x => new GfxActoCondicionDTO
                    {
                        fechaSuceso = x.fechaSuceso,
                        tipoActo = x.tipoActo,
                        departamentoID = x.departamentoID,
                        clasificacionID = x.clasificacionID,
                        accionID = x.accionID
                    }).ToList();

                #region Filtrar por division y lineas de negocios
                //if (filtro.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaDivisiones.Contains(x.division)).ToList();

                //    actosActivos = actosActivos.Join(
                //        listaCentrosCostoDivision,
                //        a => new { a.idEmpresa, idAgrupacion = a.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (a, cd) => new { a, cd }
                //    ).Select(x => x.a).ToList();
                   
                //}

                //if (filtro.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    actosActivos = actosActivos.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        a => new { a.idEmpresa, idAgrupacion = a.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (a, cd) => new { a, cd }
                //    ).Select(x => x.a).ToList();
                //}
                #endregion

                var condicionesActivas = _context.tblSAC_Condicion
                    .Where(x =>
                        (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                        (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID) &&
                        x.activo
                    ).Select(x => new GfxActoCondicionDTO
                    {
                        fechaSuceso = x.fechaSuceso,
                        departamentoID = x.departamentoID,
                        clasificacionID = x.clasificacionID,
                        estatus = x.estatus
                    }).ToList();

                #region Filtrar por division y lineas de negocios
                //if (filtro.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaDivisiones.Contains(x.division)).ToList();

                //    condicionesActivas = condicionesActivas.Join(
                //        listaCentrosCostoDivision,
                //        c => new { c.idEmpresa, idAgrupacion = c.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (c, cd) => new { c, cd }
                //    ).Select(x => x.c).ToList();
                //}

                //if (filtro.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    condicionesActivas = condicionesActivas.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        c => new { c.idEmpresa, idAgrupacion = c.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (c, cd) => new { c, cd }
                //    ).Select(x => x.c).ToList();
                //}
                #endregion

                CargarGraficasAño(ref resultado, filtro, actosActivos, condicionesActivas);
                CargarGraficasPorFiltroFecha(ref resultado, filtro, actosActivos, condicionesActivas);

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "CargarDatosDashboard", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la información.");
            }

            return resultado;
        }

        public Dictionary<string, object> obtenerGraficaTotalDep(FiltroDashboardDTO filtro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                filtro.fechaFinal = filtro.fechaFinal.AddHours(23).AddMinutes(59);

                List<string> grupos = new List<string>();
                if (filtro.arrGrupos != null)
                {
                    grupos = filtro.arrGrupos.Select(x => x.idEmpresa + "-" + x.idAgrupacion).ToList();
                }

                var departamentos = _context.tblSAC_Departamentos.ToList();

                #region Condicion
                var condiciones = _context.tblSAC_Condicion.Where(x =>
                        (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                        (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID) &&
                        (x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal) &&
                        x.activo)
                    .Select(x => new GfxActoCondicionDTO
                    {
                        fechaSuceso = x.fechaSuceso,
                        departamentoID = x.departamentoID,
                        clasificacionID = x.clasificacionID,
                        estatus = x.estatus
                    }).ToList();

                #region Filtrar por division y lineas de negocios
                //if (filtro.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaDivisiones.Contains(x.division)).ToList();

                //    condiciones = condiciones.Join(
                //        listaCentrosCostoDivision,
                //        c => new { idAgrupacion = c.idAgrupacion },
                //        cd => new {  idAgrupacion = (int)cd.idAgrupacion },
                //        (c, cd) => new { c, cd }
                //    ).Select(x => x.c).ToList();
                //}

                //if (filtro.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    condiciones = condiciones.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        c => new {  idAgrupacion = c.idAgrupacion },
                //        cd => new {  idAgrupacion = (int)cd.idAgrupacion },
                //        (c, cd) => new { c, cd }
                //    ).Select(x => x.c).ToList();
                //}
                #endregion

                var serie = new List<dynamic>();
                var data = new List<dynamic>();
                foreach (var item in departamentos)
                {
                    data.Add(new
                    {
                        name = item.descripcion,
                        y = retornarPorcentaje(condiciones.Where(x => x.departamentoID == item.id && x.estatus == EstatusActoCondicion.Completo).Count(), condiciones.Where(x => x.departamentoID == item.id).Count())
                    });
                }

                serie.Add(new
                {
                    name = "Condición",
                    colorByPoint = true,
                    data
                });

                resultado.Add("gpxGrafica", serie);
                #endregion

                #region Acto
                var actos = _context.tblSAC_Acto.Where(x =>
                        (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                        (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID) &&
                        (x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal) &&
                        x.activo &&
                        x.tipoActo == TipoActo.Inseguro)
                    .Select(x => new GfxActoCondicionDTO
                    {
                        fechaSuceso = x.fechaSuceso,
                        departamentoID = x.departamentoID,
                        clasificacionID = x.clasificacionID,
                        estatus = x.estatus
                    }).ToList();

                #region Filtrar por division y lineas de negocios
                //if (filtro.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaDivisiones.Contains(x.division)).ToList();

                //    actos = actos.Join(
                //        listaCentrosCostoDivision,
                //        a => new { a.idEmpresa, idAgrupacion = a.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (a, cd) => new { a, cd }
                //    ).Select(x => x.a).ToList();
                //}

                //if (filtro.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    actos = actos.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        a => new { a.idEmpresa, idAgrupacion = a.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (a, cd) => new { a, cd }
                //    ).Select(x => x.a).ToList();
                //}
                #endregion

                var serieActo = new List<dynamic>();
                var dataActo = new List<dynamic>();

                decimal incompletos = actos.Where(x => x.estatus != EstatusActoCondicion.Completo).Count();
                decimal porcentajeIncompleto = 0;

                if (incompletos>0)
                {
                    porcentajeIncompleto = (incompletos / (decimal)actos.Count) * 100.00M;
                }

                dataActo.Add(new
                {
                    name = "En proceso",
                    y = porcentajeIncompleto
                });
                dataActo.Add(new
                {
                    name = "Completos",
                    y = 100 - porcentajeIncompleto
                });

                serieActo.Add(new
                {
                    name = "Actos",
                    colorByPoint = true,
                    data = dataActo
                });

                resultado.Add("gpxActos", serieActo);
                #endregion

                #region vacunación
                var actosVacunacion = _context.tblSAC_Acto.Where(x =>
                        (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                        (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                        (filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID) &&
                        (x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal) &&
                        x.activo &&
                        (x.clasificacionID == 31 || x.clasificacionID == 32))
                    .Select(x => new GfxActoCondicionDTO
                    {
                        departamentoID = x.departamentoID,
                        clasificacionID = x.clasificacionID,
                        idEmpresa = x.idEmpresa,
                        idAgrupacion = x.idAgrupacion.Value,
                        nombreAgrupacion = x.idEmpresa == 0 ? x.agrupacion.nomAgrupacion : ""
                    }).ToList();

                #region Filtrar por division y lineas de negocios
                if (filtro.listaDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaDivisiones.Contains(x.division)).ToList();

                    actosVacunacion = actosVacunacion.Join(
                        listaCentrosCostoDivision,
                        a => new { a.idEmpresa, idAgrupacion = a.idAgrupacion },
                        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                        (a, cd) => new { a, cd }
                    ).Select(x => x.a).ToList();
                }

                if (filtro.listaLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && filtro.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    actosVacunacion = actosVacunacion.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        a => new { a.idEmpresa, idAgrupacion = a.idAgrupacion },
                        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                        (a, cd) => new { a, cd }
                    ).Select(x => x.a).ToList();
                }
                #endregion

                gfxVacunacionDTO dataVacunacion = new gfxVacunacionDTO();

                gfxVacunacionSeries seriesCompleta = new gfxVacunacionSeries();
                seriesCompleta.name = "Esquema completo";
                seriesCompleta.stack = "esquema";
                gfxVacunacionSeries seriesIncompleta = new gfxVacunacionSeries();
                seriesIncompleta.name = "Esquema incompleto";
                seriesIncompleta.stack = "esquema";

                foreach (var item in departamentos)
                {
                    dataVacunacion.categorias.Add(item.descripcion);

                    var total = actosVacunacion.Where(x => x.departamentoID == item.id).ToList();
                    if (total.Count == 0)
                    {
                        seriesCompleta.data.Add(0);
                        seriesIncompleta.data.Add(0);
                        continue;
                    }
                    else
                    {
                        var completos = total.Where(x => x.clasificacionID == 32).ToList();

                        var porcentajeCompletos = ((decimal)completos.Count / (decimal)total.Count) * 100.00M;
                        seriesCompleta.data.Add(porcentajeCompletos);
                        seriesIncompleta.data.Add(100 - porcentajeCompletos);
                    }
                }

                dataVacunacion.series.Add(seriesIncompleta);
                dataVacunacion.series.Add(seriesCompleta);

                resultado.Add("barraVacunacion", dataVacunacion);

                #region gfxPastel vacunación total
                if (actosVacunacion.Count > 0)
                {
                    var serieVacunacionTotal = new List<dynamic>();
                    var dataVacunacionTotal = new List<dynamic>();

                    decimal esquemaIncompleto = actosVacunacion.Where(x => x.clasificacionID == 31).Count();
                    if (esquemaIncompleto == 0)
                    {
                        serieVacunacionTotal.Add(new
                        {
                            name = "Vacunacion",
                            colorByPoint = true,
                            data = dataVacunacionTotal
                        });
                    }
                    else
                    {
                        decimal porcentajeVacunacionIncompleto = (esquemaIncompleto / (decimal)actosVacunacion.Count) * 100.00M;
                        dataVacunacionTotal.Add(new
                        {
                            name = "Esquema incompleto",
                            y = porcentajeVacunacionIncompleto
                        });
                        dataVacunacionTotal.Add(new
                        {
                            name = "Esquema completo",
                            y = 100 - porcentajeVacunacionIncompleto
                        });

                        serieVacunacionTotal.Add(new
                        {
                            name = "Vacunacion",
                            colorByPoint = true,
                            data = dataVacunacionTotal
                        });
                    }

                    resultado.Add("gpxVacunacionTotal", serieVacunacionTotal);
                #endregion
                }
                else
                {
                    resultado.Add("gpxVacunacionTotal", null);
                }
                #region vacunacion de todos los empleados
                {
                    var listaAgrupacion = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();

                    gfxVacunacionDTO dataVacunacionEmpresa = new gfxVacunacionDTO();
                    var seriesEmpresaCompleta = new gfxVacunacionSeries();
                    seriesEmpresaCompleta.name = "Esquema completo";
                    var seriesEmpresaIncompleta = new gfxVacunacionSeries();
                    seriesEmpresaIncompleta.name = "Esquema incompleto";
                    var seriesEmpresaSinVacunar = new gfxVacunacionSeries();
                    seriesEmpresaSinVacunar.name = "Sin vacunar";

                    var gbAgrupacion = actosVacunacion.Where(x => x.idEmpresa == 0).GroupBy(x => x.idAgrupacion);

                    foreach (var item in gbAgrupacion)
                    {
                        dataVacunacionEmpresa.categorias.Add(item.First().nombreAgrupacion);

                        var empleadosAgrupacion = ObtenerTodosLosEmpleadosDeLaAgrupacion(item.Key, listaAgrupacion);

                        empleadosAgrupacion.RemoveAll(x => item.Select(y => y.clave_empleado).Contains(x.clave_empleado));

                        var total = empleadosAgrupacion.Count + item.Count();

                        if (total == 0)
                        {
                            seriesEmpresaCompleta.data.Add(0);
                            seriesEmpresaIncompleta.data.Add(0);
                            seriesEmpresaSinVacunar.data.Add(0);
                        }
                        else
                        {
                            var completosEmpresa = item.Where(x => x.clasificacionID == 32).ToList();
                            var incompletosEmpresa = item.Where(x => x.clasificacionID == 31).ToList();
                            var porcentajeCompletos = Math.Round(((decimal)completosEmpresa.Count / (decimal)total) * 100.00M, 2);
                            seriesEmpresaCompleta.data.Add(porcentajeCompletos);
                            var porcentajeIncompletos = Math.Round(((decimal)incompletosEmpresa.Count / (decimal)total) * 100.00M, 2);
                            seriesEmpresaIncompleta.data.Add(porcentajeIncompletos);
                            seriesEmpresaSinVacunar.data.Add(Math.Round(100.00M - porcentajeCompletos - porcentajeIncompletos, 2));
                        }
                    }

                    dataVacunacionEmpresa.series.Add(seriesEmpresaSinVacunar);
                    dataVacunacionEmpresa.series.Add(seriesEmpresaIncompleta);
                    dataVacunacionEmpresa.series.Add(seriesEmpresaCompleta);

                    resultado.Add("barraVacunacionAgrupacion", dataVacunacionEmpresa);
                }
                #endregion
                #endregion

                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "CargarDatosArchivo", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public double retornarPorcentaje(double numero1, double numero2)
        {
            double suma = 0;
            if (numero2 == 0)
            {
                return suma = 0;
            }
            else
            {
                suma = (numero1 / numero2) * 100;
            }
            return Math.Round(suma, 2);
        }

        private List<EmpleadoAgrupacionDTO> ObtenerTodosLosEmpleadosDeLaAgrupacion(int idAgrupacion, List<tblS_IncidentesAgrupacionCC> catAgrupacion)
        {
            List<string> ccAgrupacion = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.idAgrupacionCC == idAgrupacion).Select(x => x.cc).Distinct().ToList();
            string ccs = getStringInlineArray(ccAgrupacion);
            //            var query_empleadosEnAgrupacion = new OdbcConsultaDTO();
            //            query_empleadosEnAgrupacion.consulta = string.Format(@"
            //                SELECT
            //                    dep.cc,
            //                    emp.clave_empleado
            //                FROM
            //                    sn_empleados AS emp
            //                INNER JOIN
            //                    sn_departamentos AS dep
            //                    ON
            //                        dep.clave_depto = emp.clave_depto
            //                WHERE
            //                    emp.estatus_empleado = 'A' AND
            //                    dep.cc in {0}", ccAgrupacion.ToParamInValue());
            //            foreach (var item in ccAgrupacion)
            //            {
            //                query_empleadosEnAgrupacion.parametros.Add(new OdbcParameterDTO
            //                {
            //                    nombre = "cc",
            //                    tipo = OdbcType.NVarChar,
            //                    valor = item.ToUpper()
            //                });
            //            }

            //            var resultadoConstruplan = _contextEnkontrol.Select<EmpleadoAgrupacionDTO>(EnkontrolAmbienteEnum.Rh, query_empleadosEnAgrupacion);
            //            var resultadoArrendadora = _contextEnkontrol.Select<EmpleadoAgrupacionDTO>(EnkontrolAmbienteEnum.RhArre, query_empleadosEnAgrupacion);

            var resultadoConstruplan = _context.Select<EmpleadoAgrupacionDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT
                                dep.cc,
                                emp.clave_empleado
                            FROM
                                tblRH_EK_Empleados AS emp
                            INNER JOIN
                                tblRH_EK_Departamentos AS dep
                                ON
                                    dep.clave_depto = emp.clave_depto
                            WHERE
                                emp.estatus_empleado = 'A' AND
                                dep.cc in (@ccs)",
                parametros = new { ccs }
            });

            var resultadoGCPLAN = _context.Select<EmpleadoAgrupacionDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.GCPLAN,
                consulta = @"SELECT
                                dep.cc,
                                emp.clave_empleado
                            FROM
                                tblRH_EK_Empleados AS emp
                            INNER JOIN
                                tblRH_EK_Departamentos AS dep
                                ON
                                    dep.clave_depto = emp.clave_depto
                            WHERE
                                emp.estatus_empleado = 'A' AND
                                dep.cc in (@ccs)",
                parametros = new { ccs }
            });

            var resultadoArrendadora = _context.Select<EmpleadoAgrupacionDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT
                                dep.cc,
                                emp.clave_empleado
                            FROM
                                tblRH_EK_Empleados AS emp
                            INNER JOIN
                                tblRH_EK_Departamentos AS dep
                                ON
                                    dep.clave_depto = emp.clave_depto
                            WHERE
                                emp.estatus_empleado = 'A' AND
                                dep.cc in (@ccs)",
                parametros = new { ccs }
            });

            foreach (var item in resultadoGCPLAN)
            {
                if (!resultadoConstruplan.Any(x => x.clave_empleado == item.clave_empleado))
                {
                    resultadoConstruplan.Add(item);
                }
            }

            foreach (var item in resultadoArrendadora)
            {
                if (!resultadoConstruplan.Any(x => x.clave_empleado == item.clave_empleado))
                {
                    resultadoConstruplan.Add(item);
                }
            }

            return resultadoConstruplan;
        }
        #endregion
        public string getStringInlineArray(List<string> lista)
        {
            string result = @"";
            foreach (var i in lista)
            {
                result += "'" + i + "',";
            }
            result = result.TrimEnd(',');
            return result;
        }

        #region Historial

        public Dictionary<string, object> ObtenerHistorialEmpleado(int claveEmpleado)
        {
            try
            {
                var actos = _context.tblSAC_Acto
                    .Where(x =>
                        x.claveEmpleado == claveEmpleado &&
                        x.activo)
                    .Select(x => new HistorialEmpleadoDTO
                    {
                        id = x.id,
                        folio = x.folio,
                        proyecto = x.agrupacion.nomAgrupacion,
                        tipoActo = x.tipoActo,
                        accionDesc = x.accion.descripcion,
                        departamentoDesc = x.departamento.descripcion,
                        clasificacionDesc = x.clasificacion.descripcion,
                        procedimientoDesc = x.procedimientoViolado.Procedimineto,
                        fechaSucesoDT = x.fechaSuceso,
                        tieneEvidencia = !string.IsNullOrEmpty(x.rutaEvidencia)
                    }).ToList();

                foreach (var item in actos)
                {
                    item.tipoActoDesc = item.tipoActo.GetDescription();
                    item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                }

                resultado.Add(ITEMS, actos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerHistorialEmpleado", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar los actos del empleado.");
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarActo(int actoID)
        {
            try
            {
                string rutaArchivo = ObtenerRutaFisicaArchivo(actoID, TipoRiesgo.Acto, TipoArchivo.Evidencia);

                var fileStream = GlobalUtils.GetFileAsStream(rutaArchivo);
                string name = Path.GetFileName(rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarActo", e, AccionEnum.DESCARGAR, actoID, 0);
                return null;
            }
        }

        public Dictionary<string, object> ObtenerMatrices(FiltroDashboardDTO filtro)
        {
            try
            {
                if (filtro.arrGrupos != null)
                {
                    if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                    {
                        foreach (var item in filtro.arrGrupos)
                        {
                            item.idEmpresa = vSesiones.sesionEmpresaActual;
                        }
                    }
                }

                filtro.fechaFinal = filtro.fechaFinal.AddHours(23).AddMinutes(59);

                var clasificaciones = _context.tblSAC_Clasificacion.ToList();
                var subclasificaciones = _context.tblSAC_SubclasificacionDepartamentos.Where(x => x.registroActivo).ToList();
                var procedimientos = _context.tblS_IncidentesTipoProcedimientosViolados.ToList();

                List<string> grupos = new List<string>();

                if (filtro.arrGrupos != null)
                {
                    grupos = filtro.arrGrupos.Select(x => x.idEmpresa + "-" + x.idAgrupacion).ToList();
                }

                var actos = _context.tblSAC_Acto.Where(x =>
                    x.fechaSuceso >= filtro.fechaInicial &&
                    x.fechaSuceso <= filtro.fechaFinal &&
                    (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                    (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                    (filtro.subclasificacion == 0 ? true : filtro.subclasificacion == x.subclasificacionDepID) &&
                    x.activo
                ).Select(x => new ActoExcelDTO
                {
                    folio = x.folio.ToString(),
                    fechaSucesoDT = x.fechaSuceso,
                    proyecto = x.cc + " - " + x.agrupacion.nomAgrupacion.ToUpper(),
                    claveEmpleado = x.claveEmpleado,
                    nombreEmpleado = x.nombre,
                    puestoEmpleado = x.puesto,
                    departamento = x.departamento.descripcion,
                    tipoActoEnum = x.tipoActo,
                    accionDesc = x.accion.descripcion,
                    descripcion = x.descripcion,
                    clasificacion = x.clasificacion.descripcion,
                    procedimiento = x.procedimientoViolado.Procedimineto,
                    subClasificacionDepID = x.subclasificacionDepID,
                    empleadoInformo = x.nombreInformo
                }).OrderByDescending(o => o.fechaSucesoDT).ToList();

                foreach (var item in actos)
                {
                    item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                    item.tipoActo = item.tipoActoEnum.GetDescription();
                    item.subclasificacion = subclasificaciones.Where(y => y.id == item.subClasificacionDepID).Select(z => z.subclasificacionDep).FirstOrDefault();
                }

                #region anterior
                //var actos = _context.tblSAC_Acto
                //    .Where(x => x.activo)
                //    .ToList()
                //    .Where(x => x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal)
                //    .Where(x => filtro.arrGrupos != null ? filtro.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true)
                //    .Where(x => filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor)
                //    .Where(x => filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID)
                //    .OrderByDescending(x => x.fechaSuceso)
                //    .Select(x => new ActoExcelDTO
                //    {
                //        folio = x.folio.ToString(),
                //        fechaSuceso = x.fechaSuceso.ToString("yyyy/MM/dd"),
                //        proyecto = String.Format("{0} - {1}", x.cc, tablaCC.First(y => y.id == (int)x.idAgrupacion).nomAgrupacion.ToUpper()),
                //        claveEmpleado = x.claveEmpleado,
                //        nombreEmpleado = x.nombre,
                //        puestoEmpleado = x.puesto,
                //        departamento = x.departamento.departamento,
                //        tipoActo = x.tipoActo.GetDescription(),
                //        accionDesc = x.accion.descripcion,
                //        descripcion = x.descripcion,
                //        clasificacion = clasificaciones.FirstOrDefault(y => y.id == x.clasificacionID).descripcion,
                //        procedimiento = procedimientos.FirstOrDefault(y => y.id == x.procedimientoID).Procedimineto
                //    }).ToList();
                #endregion

                var condiciones = _context.tblSAC_Condicion.Where(x =>
                    x.fechaSuceso >= filtro.fechaInicial &&
                    x.fechaSuceso <= filtro.fechaFinal &&
                    (grupos.Count > 0 ? grupos.Contains(x.idEmpresa + "-" + x.idAgrupacion) : true) &&
                    (filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor) &&
                    (filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID) &&
                    (filtro.subclasificacion == 0 ? true : filtro.subclasificacion == x.subclasificacionDepID) &&
                    x.activo
                ).Select(x => new CondicionExcelDTO
                {
                    folio = x.folio.ToString(),
                    fechaSucesoDT = x.fechaSuceso,
                    proyecto = x.cc + " - " + x.agrupacion.nomAgrupacion,
                    departamento = x.departamento.descripcion,
                    descripcion = x.descripcion,
                    clasificacion = x.clasificacion.descripcion,
                    procedimiento = x.procedimientoViolado.Procedimineto,
                    fechaResolucionDT = x.fechaResolucion,
                    subClasificacionDepID = x.subclasificacionDepID,
                    empleadoInformo = x.nombreInformo
                }).OrderByDescending(o => o.fechaSucesoDT).ToList();

                foreach (var item in condiciones)
                {
                    item.fechaSuceso = item.fechaSucesoDT.ToString("yyyy/MM/dd");
                    item.fechaResolucion = item.fechaResolucionDT.HasValue ? item.fechaResolucionDT.Value.ToString("yyyy/MM/dd") : "Sin resolver";
                    item.subclasificacion = subclasificaciones.Where(y => y.id == item.subClasificacionDepID).Select(z => z.subclasificacionDep).FirstOrDefault();
                }

                #region anterior
                //var condiciones = _context.tblSAC_Condicion
                //    .Where(x => x.activo)
                //    .ToList()
                //    .Where(x => x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal)
                //    .Where(x => filtro.arrGrupos != null ? filtro.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true)
                //    .Where(x => filtro.claveSupervisor == 0 ? true : filtro.claveSupervisor == x.claveSupervisor)
                //    .Where(x => filtro.departamentoID == 0 ? true : filtro.departamentoID == x.departamentoID)
                //    .OrderByDescending(x => x.fechaSuceso)
                //    .Select(x => new CondicionExcelDTO
                //    {
                //        folio = x.folio.ToString(),
                //        fechaSuceso = x.fechaSuceso.ToString("yyyy/MM/dd"),
                //        proyecto = String.Format("{0} - {1}", x.cc, tablaCC.First(y => y.id == (int)x.idAgrupacion).nomAgrupacion.ToUpper()),
                //        departamento = x.departamento.departamento,
                //        descripcion = x.descripcion,
                //        clasificacion = clasificaciones.FirstOrDefault(y => y.id == x.clasificacionID).descripcion,
                //        procedimiento = procedimientos.FirstOrDefault(y => y.id == x.procedimientoID).Procedimineto,
                //        fechaResolucion = x.fechaResolucion.HasValue ? x.fechaResolucion.Value.ToString("yyyy/MM/dd") : "Sin resolver"
                //    }).ToList();
                #endregion

                // Se agregan las variables a sesión en caso de que se quiera exportar a Excel.
                HttpContext.Current.Session["actos"] = actos;
                HttpContext.Current.Session["condiciones"] = condiciones;

                resultado.Add("actos", actos);
                resultado.Add("condiciones", condiciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerMatrices", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la información.");
            }

            return resultado;
        }

        public Tuple<MemoryStream, string> DescargarExcelMatrizActos()
        {
            try
            {
                var actos = HttpContext.Current.Session["actos"] as List<ActoExcelDTO>;

                if (actos == null)
                {
                    return null;
                }

                // Se agregan las columnas.
                string[] headersExcel = new string[] {
                    "No. Folio",
                    "Fecha Detección",
                    "Proyecto",
                    "Clave Empleado",
                    "Nombre Empleado",
                    "Puesto Empleado",
                    "Departamento",
                    "Tipo Acto",
                    "Acciones",
                    "Descripción",
                    "Clasificación",
                    "Subclasificación",
                    "Procedimiento",
                    "Empleado Informó"
                };

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Matriz Actos");

                    List<string[]> headerRow = new List<string[]>() { headersExcel };

                    string headerRange = "A1:" + ExcelUtilities.GetExcelColumnName(headersExcel.Count()) + "1";

                    hoja.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var acto in actos)
                    {
                        var cellDataEmployee = new object[14];

                        cellDataEmployee[0] = acto.folio;
                        cellDataEmployee[1] = acto.fechaSuceso;
                        cellDataEmployee[2] = acto.proyecto;
                        cellDataEmployee[3] = acto.claveEmpleado;
                        cellDataEmployee[4] = acto.nombreEmpleado;
                        cellDataEmployee[5] = acto.puestoEmpleado;
                        cellDataEmployee[6] = acto.departamento;
                        cellDataEmployee[7] = acto.tipoActo;
                        cellDataEmployee[8] = acto.accionDesc;
                        cellDataEmployee[9] = acto.descripcion;
                        cellDataEmployee[10] = acto.clasificacion;
                        cellDataEmployee[11] = acto.subclasificacion;
                        cellDataEmployee[12] = acto.procedimiento;
                        cellDataEmployee[13] = acto.empleadoInformo;

                        cellData.Add(cellDataEmployee);
                    }

                    hoja.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestCompression;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return Tuple.Create(bytes, "Matriz Actos.xlsx");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarExcelMatrizActos", e, AccionEnum.DESCARGAR, 0, 0);
                return null;
            }
        }

        public Tuple<MemoryStream, string> DescargarExcelMatrizCondiciones()
        {
            try
            {
                var condiciones = HttpContext.Current.Session["condiciones"] as List<CondicionExcelDTO>;

                if (condiciones == null)
                {
                    return null;
                }

                // Se agregan las columnas.
                string[] headersExcel = new string[] { "No. Folio", "Fecha Detección", "Proyecto", "Departamento", "Descripción", "Clasificación", "Subclasificación", "Procedimiento", "Fecha Corrección", "Empleado Informó" };

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Matriz Condiciones");

                    List<string[]> headerRow = new List<string[]>() { headersExcel };

                    string headerRange = "A1:" + ExcelUtilities.GetExcelColumnName(headersExcel.Count()) + "1";

                    hoja.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var condicion in condiciones)
                    {
                        var cellDataEmployee = new object[10];

                        cellDataEmployee[0] = condicion.folio;
                        cellDataEmployee[1] = condicion.fechaSuceso;
                        cellDataEmployee[2] = condicion.proyecto;
                        cellDataEmployee[3] = condicion.departamento;
                        cellDataEmployee[4] = condicion.descripcion;
                        cellDataEmployee[5] = condicion.clasificacion;
                        cellDataEmployee[6] = condicion.subclasificacion;
                        cellDataEmployee[7] = condicion.procedimiento;
                        cellDataEmployee[8] = condicion.fechaResolucion;
                        cellDataEmployee[9] = condicion.empleadoInformo;

                        cellData.Add(cellDataEmployee);
                    }

                    hoja.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestCompression;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return Tuple.Create(bytes, "Matriz Condiciones.xlsx");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarExcelMatrizCondiciones", e, AccionEnum.DESCARGAR, 0, 0);
                return null;
            }
        }

        #endregion

        #region Helper Methods
        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(fileName));
        }

        private string ObtenerRutaCarpetaCondicion(int id)
        {
            string nombreCarpetaCondicion = NombreBaseCondicion + id;
            return Path.Combine(RutaCondiciones, nombreCarpetaCondicion);
        }

        private string ObtenerRutaCarpetaActo(int id)
        {
            string nombreCarpetaActo = NombreBaseActo + id;
            return Path.Combine(RutaActos, nombreCarpetaActo);
        }

        private tblSAC_Condicion InicializarCondicion(CondicionDTO condicion)
        {
            var fechaSuceso = Convert.ToDateTime(condicion.fechaSuceso);
            return new tblSAC_Condicion
            {
                claveInformo = condicion.claveInformo,
                nombreInformo = condicion.nombreInformo,
                fechaCreacion = DateTime.Now,
                cc = condicion.cc,
                folio = ObtenerNuevoFolioActoCondicion(condicion.idEmpresa, condicion.idAgrupacion),
                descripcion = condicion.descripcion,
                clasificacionID = condicion.clasificacionID,
                procedimientoID = condicion.procedimientoID,
                fechaSuceso = fechaSuceso,
                claveSupervisor = condicion.claveSupervisor,
                nombreSupervisor = condicion.nombreSupervisor,
                departamentoID = condicion.departamentoID,
                subclasificacionDepID = condicion.subclasificacionDepID,
                activo = true,
                usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                idEmpresa = condicion.idEmpresa,
                idAgrupacion = condicion.idAgrupacion,
                nivelPrioridad = condicion.nivelPrioridad,
                accionCorrectiva = condicion.accionCorrectiva,
                clasificacionGeneralID = condicion.clasificacionGeneralID
            };
        }

        private tblSAC_Acto InicializarActo(ActoDTO acto)
        {
            var fechaSuceso = Convert.ToDateTime(acto.fechaSuceso);
            var fechaIngreso = Convert.ToDateTime(acto.fechaIngreso);

            int empresa = 0;

            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
            {
                empresa = vSesiones.sesionEmpresaActual;
            }
            else
            {
                empresa = acto.idEmpresa;
            }

            return new tblSAC_Acto
            {
                claveEmpleado = acto.claveEmpleado,
                nombre = acto.nombre,
                puesto = acto.puesto,
                fechaIngreso = fechaIngreso,
                accionID = acto.accionID,
                tipoActo = acto.tipoActo,
                esExterno = acto.esExterno,
                claveContratista = acto.claveContratista,
                claveInformo = acto.claveInformo,
                nombreInformo = acto.nombreInformo,
                fechaCreacion = DateTime.Now,
                cc = acto.cc,
                folio = ObtenerNuevoFolioActoCondicion(acto.idEmpresa, acto.idAgrupacion),
                descripcion = acto.descripcion,
                clasificacionID = acto.clasificacionID,
                procedimientoID = acto.procedimientoID,
                fechaSuceso = fechaSuceso,
                claveSupervisor = acto.claveSupervisor,
                nombreSupervisor = acto.nombreSupervisor,
                departamentoID = acto.departamentoID,
                subclasificacionDepID = acto.subclasificacionDepID,
                estatus = EstatusActoCondicion.Completo,
                activo = true,
                usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                idEmpresa = empresa,
                idAgrupacion = acto.idAgrupacion,
                numeroInfraccion = acto.numeroInfraccion,
                nivelInfraccion = acto.nivelInfraccion,
                nivelInfraccionAcumulado = acto.nivelInfraccionAcumulado,
                numeroFalta = acto.numeroFalta,
                compromiso = acto.compromiso,
                clasificacionGeneralID = acto.clasificacionGeneralID
            };
        }

        private int ObtenerNuevoFolioActoCondicion(int idEmpresa, int idAgrupacion)
        {
            var ultimoFolioActo = _context.tblSAC_Acto.Where(x => x.idEmpresa == idEmpresa && x.idAgrupacion == idAgrupacion).Select(x => x.folio).DefaultIfEmpty().Max();
            var ultimoFolioCondicion = _context.tblSAC_Condicion.Where(x => x.idEmpresa == idEmpresa && x.idAgrupacion == idAgrupacion).Select(x => x.folio).DefaultIfEmpty().Max();
            var maxFolio = Math.Max(ultimoFolioActo, ultimoFolioCondicion);
            return maxFolio + 1;
        }

        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        private string ObtenerRutaFisicaArchivo(int sucesoID, TipoRiesgo tipoRiesgo, TipoArchivo tipoArchivo)
        {
            switch (tipoRiesgo)
            {
                case TipoRiesgo.Acto:

                    var acto = _context.tblSAC_Acto.First(x => x.id == sucesoID);

                    return acto.rutaEvidencia;

                case TipoRiesgo.Condicion:

                    var condicion = _context.tblSAC_Condicion.First(x => x.id == sucesoID);

                    switch (tipoArchivo)
                    {
                        case TipoArchivo.Evidencia:
                            return condicion.rutaEvidencia;

                        case TipoArchivo.ImagenAntes:
                            return condicion.rutaImagenAntes;

                        case TipoArchivo.ImagenDespues:
                            return condicion.rutaImagenDespues;

                        default:
                            throw new Exception("Tipo de archivo no definido.");
                    }

                default:
                    throw new Exception("Tipo de riesgo no definido.");
            }
        }

        // Return a random integer between a min and max value.
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

        private string ObtenerColorGraficaAleatorio()
        {
            int r = RandomInteger(0, 255);
            int g = RandomInteger(0, 255);
            int b = RandomInteger(0, 255);
            float a = 0.6f; // Valor constante para colores definidos.

            return String.Format("rgba({0},{1},{2},{3})", r, g, b, a);
        }

        private void CargarGraficasAño(ref Dictionary<string, object> resultado, FiltroDashboardDTO filtro, List<GfxActoCondicionDTO> actos, List<GfxActoCondicionDTO> condiciones)
        {
            //int añoActual = DateTime.Now.Year;
            //var fechaInicioAño = new DateTime(añoActual, 1, 1);
            //var fechaFinAño = new DateTime(añoActual, 12, 31).AddHours(23).AddMinutes(59);

            //var months = Enumerable.Range(0, 12).Select(m => new KeyValuePair<int, string>(m + 1, DateTimeFormatInfo.CurrentInfo.MonthNames[m])).ToList();
            var meses = MonthsBetween(filtro.fechaInicial, filtro.fechaFinal);

            var actosAño = actos.Where(x => x.fechaSuceso.Date >= filtro.fechaInicial.Date && x.fechaSuceso.Date <= filtro.fechaFinal.Date).ToList();
            var condicionesAño = condiciones.Where(x => x.fechaSuceso.Date >= filtro.fechaInicial.Date && x.fechaSuceso.Date <= filtro.fechaFinal.Date).ToList();
            var accidentesAño = _context.tblS_IncidentesInformePreliminar.Where(x =>
                DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(filtro.fechaInicial) &&
                DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(filtro.fechaFinal)
            ).ToList().Where(x =>
                (filtro.arrGrupos != null ? filtro.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) &&
                (filtro.claveSupervisor == 0 ? true : x.claveSupervisor.HasValue ? filtro.claveSupervisor == x.claveSupervisor : false) &&
                (filtro.departamentoID == 0 ? true : x.departamento_id != 0 ? filtro.departamentoID == x.departamento_id : false) &&
                (x.tipoAccidente_id.HasValue && x.tipoAccidente_id == 5 ? x.subclasificacionID == 1 : true) //Filtramos a los PD que sólo sean de mala operación.
            ).ToList();

            var chartSucesosPorMes = ObtenerChartSucesosPorMes(meses, actosAño, condicionesAño);
            resultado.Add("chartSucesosPorMes", chartSucesosPorMes);

            var chartComportamiento = ObtenerChartComportamiento(meses, actosAño, condicionesAño, accidentesAño);
            resultado.Add("chartComportamiento", chartComportamiento);
        }

        public static IEnumerable<Tuple<string, int, int>> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(dateTimeFormat.GetMonthName(iterator.Month), iterator.Year, iterator.Month);
                iterator = iterator.AddMonths(1);
            }
        }

        private void CargarGraficasPorFiltroFecha(ref Dictionary<string, object> resultado, FiltroDashboardDTO filtro, List<GfxActoCondicionDTO> actos, List<GfxActoCondicionDTO> condiciones)
        {
            var actosFiltroFecha = actos.Where(x => x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal).ToList();
            var condicionesFiltroFecha = condiciones.Where(x => x.fechaSuceso >= filtro.fechaInicial && x.fechaSuceso <= filtro.fechaFinal).ToList();

            var chartSucesosPorDepartamento = ObtenerChartSucesosPorDepartamento(actosFiltroFecha, condicionesFiltroFecha);
            resultado.Add("chartSucesosPorDepartamento", chartSucesosPorDepartamento);

            var chartActosClasificacion = ObtenerChartActosClasificacion(actosFiltroFecha);
            resultado.Add("chartActosClasificacion", chartActosClasificacion);

            var chartCondicionesClasificacion = ObtenerChartCondicionesClasificacion(condicionesFiltroFecha);
            resultado.Add("chartCondicionesClasificacion", chartCondicionesClasificacion);
        }

        private ChartDataDTO ObtenerChartSucesosPorMes(IEnumerable<Tuple<string, int, int>> months, List<GfxActoCondicionDTO> actosAño, List<GfxActoCondicionDTO> condicionesAño)
        {
            var chartSucesosPorMes = new ChartDataDTO
            {
                labels = months.Select(x => x.Item2).Distinct().Skip(1).Any() ? months.Select(x => x.Item1.ToUpper() + " - " + x.Item2).ToList() : months.Select(x => x.Item1.ToUpper()).ToList(),
                datasets = new List<DatasetDTO>()
            };

            var colorActosSeguros = "rgb(0,176,80)";
            var datasetActosSeguros = new DatasetDTO
            {
                label = "Actos Seguros",
                backgroundColor = months.Select(x => colorActosSeguros).ToList(),
                borderColor = months.Select(x => colorActosSeguros).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)actosAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.tipoActo == TipoActo.Seguro).Count()).ToList()
            };
            chartSucesosPorMes.datasets.Add(datasetActosSeguros);

            var colorActosInseguros = "rgb(255,0,0)";
            var datasetActosInseguros = new DatasetDTO
            {
                label = "Actos Inseguros",
                backgroundColor = months.Select(x => colorActosInseguros).ToList(),
                borderColor = months.Select(x => colorActosInseguros).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)actosAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.tipoActo == TipoActo.Inseguro).Count()).ToList()
            };
            chartSucesosPorMes.datasets.Add(datasetActosInseguros);

            var colorCondiciones = "rgb(255,255,0)";
            var datasetCondiciones = new DatasetDTO
            {
                label = "Condiciones",
                backgroundColor = months.Select(x => colorCondiciones).ToList(),
                borderColor = months.Select(x => colorCondiciones).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)condicionesAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3).Count()).ToList()
            };
            chartSucesosPorMes.datasets.Add(datasetCondiciones);

            return chartSucesosPorMes;
        }

        private ChartDataDTO ObtenerChartComportamiento(IEnumerable<Tuple<string, int, int>> months, List<GfxActoCondicionDTO> actosAño, List<GfxActoCondicionDTO> condicionesAño, List<tblS_IncidentesInformePreliminar> accidentesAño)
        {
            var chartComportamiento = new ChartDataDTO
            {
                labels = months.Select(x => x.Item2).Distinct().Skip(1).Any() ? months.Select(x => x.Item1.ToUpper() + " - " + x.Item2).ToList() : months.Select(x => x.Item1.ToUpper()).ToList(),
                datasets = new List<DatasetDTO>()
            };

            string stackAccidentes = "Accidentes";
            string stackComportamientos = "Comportamientos";
            string stackAcciones = "Acciones";

            int indexColor = 0;

            #region Accidentes

            var tiposEventos = _context.tblS_IncidentesTipoEventos.ToList();

            string[] coloresEventos = new string[] { "rgb(255,0,0)", "rgb(255,255,0)" };

            foreach (var tipoEvento in tiposEventos)
            {
                var colorClasificacion = coloresEventos[indexColor++];
                var datasetAccidentesClasificacion = new DatasetStackedDTO
                {
                    label = tipoEvento.tipoEvento.ToUpper(),
                    backgroundColor = months.Select(x => colorClasificacion).ToList(),
                    borderColor = months.Select(x => colorClasificacion).ToList(),
                    borderWidth = 2,
                    fill = true,
                    data = months.Select(x => (decimal)accidentesAño.Where(y => y.fechaIncidente.Year == x.Item2 && y.fechaIncidente.Month == x.Item3 && y.TiposAccidente.clasificacion.tipoEvento_id == tipoEvento.id).Count()).ToList(),
                    stack = stackAccidentes
                };
                chartComportamiento.datasets.Add(datasetAccidentesClasificacion);
            }

            #endregion

            indexColor = 0;

            #region Comportamientos

            var colorActosInseguros = "rgb(255,102,0)";
            var datasetActosInseguros = new DatasetStackedDTO
            {
                label = "ACTOS INSEGUROS",
                backgroundColor = months.Select(x => colorActosInseguros).ToList(),
                borderColor = months.Select(x => colorActosInseguros).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)actosAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.tipoActo == TipoActo.Inseguro).Count()).ToList(),
                stack = stackComportamientos
            };
            chartComportamiento.datasets.Add(datasetActosInseguros);

            var colorCondicionesSinCorreccion = "rgb(128,128,128)";
            var datasetCondicionesSinCorreccion = new DatasetStackedDTO
            {
                label = "CONDICIONES SIN CORRECCIÓN",
                backgroundColor = months.Select(x => colorCondicionesSinCorreccion).ToList(),
                borderColor = months.Select(x => colorCondicionesSinCorreccion).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)condicionesAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.estatus == EstatusActoCondicion.EnProceso).Count()).ToList(),
                stack = stackComportamientos
            };
            chartComportamiento.datasets.Add(datasetCondicionesSinCorreccion);

            var colorCondicionesCorregidas = "rgb(146,208,80)";
            var datasetCondicionesCorregidas = new DatasetStackedDTO
            {
                label = "CONDICIONES CORREGIDAS",
                backgroundColor = months.Select(x => colorCondicionesCorregidas).ToList(),
                borderColor = months.Select(x => colorCondicionesCorregidas).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)condicionesAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.estatus == EstatusActoCondicion.Completo).Count()).ToList(),
                stack = stackComportamientos
            };
            chartComportamiento.datasets.Add(datasetCondicionesCorregidas);

            #endregion

            #region Acciones

            var acciones = _context.tblSAC_Accion.ToList();

            var colorContactoPersonalPositivo = "rgb(0,112,192)";
            var datasetContactoPersonalPositivo = new DatasetStackedDTO
            {
                label = "CONTACTO PERSONAL POSITIVO",
                backgroundColor = months.Select(x => colorContactoPersonalPositivo).ToList(),
                borderColor = months.Select(x => colorContactoPersonalPositivo).ToList(),
                borderWidth = 2,
                fill = true,
                data = months.Select(x => (decimal)actosAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.tipoActo == TipoActo.Seguro && y.accionID == 1).Count()).ToList(),
                stack = stackAcciones
            };
            chartComportamiento.datasets.Add(datasetContactoPersonalPositivo);

            string[] coloresAcciones = new string[] { "rgb(102,0,102)", "rgb(255,0,102)", "rgb(153,102,0)", "rgb(0,0,0)" };

            foreach (var accion in acciones)
            {
                var colorActoAccion = coloresAcciones[indexColor++];
                var datasetActoAccion = new DatasetStackedDTO
                {
                    label = accion.descripcion.ToUpper(),
                    backgroundColor = months.Select(x => colorActoAccion).ToList(),
                    borderColor = months.Select(x => colorActoAccion).ToList(),
                    borderWidth = 2,
                    fill = true,
                    data = months.Select(x => (decimal)actosAño.Where(y => y.fechaSuceso.Year == x.Item2 && y.fechaSuceso.Month == x.Item3 && y.tipoActo == TipoActo.Inseguro && y.accionID == accion.id).Count()).ToList(),
                    stack = stackAcciones
                };
                chartComportamiento.datasets.Add(datasetActoAccion);
            }

            #endregion


            return chartComportamiento;
        }

        private ChartDataDTO ObtenerChartSucesosPorDepartamento(List<GfxActoCondicionDTO> actos, List<GfxActoCondicionDTO> condiciones)
        {
            var departamentos = _context.tblSAC_Departamentos.ToList().Select(x => new KeyValuePair<int, string>(x.id, x.descripcion)).ToList();

            var chartSucesosPorDepartamento = new ChartDataDTO { labels = departamentos.Select(x => x.Value).ToList(), datasets = new List<DatasetDTO>() };

            var colorDepartamentos = ObtenerColorGraficaAleatorio();
            var datasetDepartamentos = new DatasetDTO
            {
                backgroundColor = new List<string>(),
                borderColor = new List<string>(),
                borderWidth = 1,
                fill = true,
                data = new List<decimal>()
            };

            string[] coloresDepartamento = new string[] 
            { 
                "rgb(255,102,0)",   // Capital Humano
                "rgb(128,128,128)",  // Control de Procesos
                "rgb(146,208,80)",   // Seguridad
                "rgb(0,112,192)",   // Mantenimiento
                "rgb(102,0,102)",  // Gerencia de Proyecto
                "rgb(153,102,0)",  // Alta Gerencia Op
                "rgb(255,0,102)",  // Alta Gerencia Mtto
                "rgb(0, 255, 140)", // Alta Gerencia SST
                "rgb(255, 98, 0)", // Almacén
                "rgb(119, 0, 255)", // Compras
                "rgb(0, 187, 255)", // Residente
                "rgb(241, 192, 232)", //Alta Gerencia
                "rgb(185, 251, 192)", //Administración
                "rgb(155, 93, 229)",
                "rgb(241, 91, 181)",
                "rgb(254, 228, 64)",
                "rgb(0, 187, 249)",
                "rgb(0, 245, 212)",
                "rgb(142, 202, 230)",
                "rgb(33, 158, 188)",
                "rgb(2, 48, 71)",
                "rgb(255, 183, 3)",
                "rgb(251, 133, 0)",
                "rgb(183, 133, 0)", //Calidad (Perú)
                "rgb(155, 193, 224)", //Operación (Perú)
            };
            int colorIndex = 0;

            foreach (var departamento in departamentos)
            {
                var cantidadCondiciones = condiciones.Where(x => x.departamentoID == departamento.Key).ToList();
                var cantidadActosInseguros = actos.Where(x => x.departamentoID == departamento.Key && x.tipoActo == TipoActo.Inseguro).ToList();
                datasetDepartamentos.data.Add(cantidadCondiciones.Count + cantidadActosInseguros.Count);

                var colorDepartamento = coloresDepartamento[colorIndex++];
                datasetDepartamentos.backgroundColor.Add(colorDepartamento);
                datasetDepartamentos.borderColor.Add(colorDepartamento);
            }

            chartSucesosPorDepartamento.datasets.Add(datasetDepartamentos);

            return chartSucesosPorDepartamento;
        }

        private ChartDataDTO ObtenerChartActosClasificacion(List<GfxActoCondicionDTO> actosFiltroFecha)
        {
            var clasificacionesActo = _context.tblSAC_Clasificacion.Where(x => x.tipoRiesgo == TipoRiesgo.Acto && x.id != 31 && x.id != 32).OrderBy(x => x.descripcion).ToList();

            var chartActosClasificacion = new ChartDataDTO { labels = clasificacionesActo.Select(x => x.descripcion).ToList(), datasets = new List<DatasetDTO>() };

            var colorActosClasificacion = "rgb(0,0,0)";
            var datasetActosClasificacion = new DatasetDTO
            {
                backgroundColor = clasificacionesActo.Select(x => colorActosClasificacion).ToList(),
                borderColor = clasificacionesActo.Select(x => colorActosClasificacion).ToList(),
                borderWidth = 2,
                fill = true,
                data = clasificacionesActo.Select(x => (decimal)actosFiltroFecha.Where(y => y.clasificacionID == x.id && y.tipoActo == TipoActo.Inseguro).Count()).ToList()
            };
            chartActosClasificacion.datasets.Add(datasetActosClasificacion);

            return chartActosClasificacion;
        }

        private ChartDataDTO ObtenerChartCondicionesClasificacion(List<GfxActoCondicionDTO> condicionesFiltroFecha)
        {
            var clasificacionesCondicion = _context.tblSAC_Clasificacion.Where(x => x.tipoRiesgo == TipoRiesgo.Condicion).OrderBy(x => x.descripcion).ToList();

            var chartCondicionesClasificacion = new ChartDataDTO { labels = clasificacionesCondicion.Select(x => x.descripcion).ToList(), datasets = new List<DatasetDTO>() };

            var colorCondicionesClasificacion = "rgb(255,102,0)";

            var datasetCondicionesClasificacion = new DatasetDTO
            {
                backgroundColor = clasificacionesCondicion.Select(x => colorCondicionesClasificacion).ToList(),
                borderColor = clasificacionesCondicion.Select(x => colorCondicionesClasificacion).ToList(),
                borderWidth = 2,
                fill = true,
                data = clasificacionesCondicion.Select(x => (decimal)condicionesFiltroFecha.Where(y => y.clasificacionID == x.id).Count()).ToList()
            };
            chartCondicionesClasificacion.datasets.Add(datasetCondicionesClasificacion);

            return chartCondicionesClasificacion;
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            try
            {
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();

                #region SE VERIFICA SI EL USUARIO LOGUEADO ES CONTRATISTA
                int idContratista = (int)vSesiones.sesionUsuarioDTO.id;
                int idPerfilUsuario = (int)vSesiones.sesionUsuarioDTO.idPerfil;
                if (idPerfilUsuario == 4) // 4: CONTRATISTA
                    incContratista = true;
                #endregion

                if (incContratista)
                {
                    #region SE CREA LISTADO DE CONTRATISTAS
                    List<int> lstRelEmpleadoEmpresa = _context.tblS_IncidentesRelEmpresaContratistas.Where(x => x.idContratista == idContratista && x.esActivo).Select(x => x.idEmpresa).ToList();
                    List<tblS_IncidentesEmpresasContratistas> lstEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => lstRelEmpleadoEmpresa.Contains(x.id) && x.esActivo).ToList();
                    var lstContratistasCboDTO = lstEmpresas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = x.nombreEmpresa,
                        Value = x.id.ToString(),
                        Prefijo = ((int)GruposSeguridadEnum.CONTRATISTA).ToString()
                    }).ToList();
                    string strLabel = string.Empty;
                    switch (lstContratistasCboDTO.Count())
                    {
                        case 1:
                            strLabel = "EMPRESA";
                            break;
                        default:
                            strLabel = "EMPRESAS";
                            break;
                    }
                    if (lstContratistasCboDTO.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = strLabel, options = lstContratistasCboDTO });
                    #endregion
                }
                else
                {
                    #region SE CREA LISTADO DE CC DE AGRUPACIONES
                    var agrups = new List<int>();
                    if (division != null)
                        agrups = _context.tblS_Req_CentroCostoDivision.Where(x => x.division == (int)division).Select(x => (int)x.idAgrupacion).ToList();

                    var lstAgrupaciones = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo && (division != null ? agrups.Contains(x.id) : true)).OrderBy(y => y.nomAgrupacion).ToList();
                    var lstAgrupacionesCboDTO = lstAgrupaciones.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = x.nomAgrupacion,
                        Value = x.id.ToString(),
                        Prefijo = (EmpresaEnum)vSesiones.sesionEmpresaActual != EmpresaEnum.Colombia ? ((int)GruposSeguridadEnum.GRUPO).ToString() : ((int)GruposSeguridadEnum.COLOMBIA).ToString()
                    }).ToList();
                    if (lstAgrupaciones.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "AGRUPACIONES", options = lstAgrupacionesCboDTO });
                    #endregion

                    //#region SE CREA LISTADO DE CONTRATISTAS
                    //var lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                    //var lstContratistasCboDTO = lstContratistas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    //{
                    //    Text = x.nombreEmpresa,
                    //    Value = "c_" + x.id.ToString(),
                    //    Prefijo = ((int)GruposSeguridadEnum.CONTRATISTA).ToString()
                    //}).ToList();
                    //if (lstContratistas.Count() > 0)
                    //    lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONTRATISTAS", options = lstContratistasCboDTO });
                    //#endregion

                    #region SE CREA LISTADO DE AGRUPACIONES DE CONTRATISTAS
                    var lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
                    var lstAgrupacionContratistasDTO = lstAgrupacionContratistas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = x.nomAgrupacion,
                        Value = "a_" + x.id.ToString(),
                        Prefijo = ((int)GruposSeguridadEnum.agrupacionContratistas).ToString()
                    }).ToList();
                    if (lstAgrupacionContratistas.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "AGRUPACIÓN CONTRATISTAS", options = lstAgrupacionContratistasDTO });
                    #endregion
                }

                if (lstCC.Count > 0)
                {
                    resultado.Add(ITEMS, lstCC);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerComboCCAmbasEmpresas", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }
        #endregion
    }
}

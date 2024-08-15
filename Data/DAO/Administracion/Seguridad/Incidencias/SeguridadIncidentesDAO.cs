using Core.DAO.Administracion.Seguridad;
using Core.DTO;
using Core.DTO.Administracion.Seguridad;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.Utils;
using Core.DTO.Utils.ChartJS;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Maquinaria;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SeguimientoAcuerdos;
using Core.Enum.Administracion.Seguridad.Indicadores;
using Core.Enum.Administracion.Seguridad.Indicadores.ReporteGlobal;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Administracion.Seguridad.Indicadores;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using Infrastructure.Utils;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using System.Dynamic;
using System.Data;
using Core.DTO.Administracion.Seguridad.CatHorasHombre;
using Core.DTO.Administracion.Seguridad.AgrupacionCC;
using Core.Enum.Principal;
using Core.Enum.Administracion.Seguridad;
using Core.Entity.Administrativo.Contratistas;
using Core.Entity.Administrativo.Seguridad.Requerimientos;

namespace Data.DAO.Administracion.Seguridad.Incidencias
{
    class SeguridadIncidentesDAO : GenericDAO<tblS_IncidentesInformePreliminar>, ISeguridadIncidentesDAO
    {
        public readonly string ERROR = "error";

        private const string _NOMBRE_CONTROLADOR = "IndicadoresSeguridadController";
        private const int _SISTEMA = (int)SistemasEnum.SEGURIDAD;
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\INCIDENTES";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\INCIDENTES";
        private readonly string RutaEvidencias;
        private readonly string RutaEvidenciasRIA;

        private const int PonderacionFatalitdad = 1;

        string Controller = "IndicadoresSeguridadController";
        private Dictionary<string, object> resultado = new Dictionary<string, object>();

        public SeguridadIncidentesDAO()
        {
#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaEvidencias = Path.Combine(RutaBase, "EVIDENCIAS");
            RutaEvidenciasRIA = Path.Combine(RutaBase, "EVIDENCIAS_RIA");
        }

        #region CAPTURA INFORME PRELIMINAR
        public Dictionary<string, object> GetDatosGeneralesIncidentes(int agrupacionID, int empresa, DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var incidentes = _context.tblS_IncidentesInformePreliminar.Where(x =>
                    DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(fechaFin)
                ).ToList();

                if (agrupacionID > 0)
                {
                    incidentes = incidentes.Where(x => x.idAgrupacion == agrupacionID && x.idEmpresa == empresa).ToList();
                }

                var incidentesInvestigables = incidentes.Where(x => x.aplicaRIA).ToList();
                var incidentesInvestigablesCompletos = incidentesInvestigables.Where(x => x.estatusAvance == EstatusIncidenteEnum.Completo).Count();
                var porcentajeAccidentesInvestigablesCompletos = String.Format("{0} %", (incidentesInvestigablesCompletos * 100) / (incidentesInvestigables.Count() > 0 ? incidentesInvestigables.Count() : 1));

                resultado.Add(SUCCESS, true);
                resultado.Add("totalAccidentes", incidentes.Count);
                resultado.Add("totalAccidentesInvestigables", incidentesInvestigables.Count());
                resultado.Add("porcentajeAccidentesInvestigablesCompletos", porcentajeAccidentesInvestigablesCompletos);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los datos generales de los incidentes");
            }

            return resultado;
        }

        public Dictionary<string, object> getInformesPreliminares(List<int> listaDivisiones, List<int> listaLineasNegocio, int idAgrupacion, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, int tipoAccidenteID, int supervisorID, int departamentoID, int estatus)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                fechaFin = fechaFin.AddHours(23).AddMinutes(59);

                //string ccs;

                //if (idEmpresa == 0)
                //{
                //    var agrupador = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == idAgrupacion);
                //    ccs = agrupador!=null ? agrupador.nomAgrupacion: "No relacionado";
                //}
                //else if (idEmpresa == 1000)
                //{
                //    var agrupador = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == idAgrupacion);
                //    ccs = agrupador != null ? agrupador.nomAgrupacion : "No relacionado";
                //}
                //else
                //{
                //    ccs = "No relacionado";
                //}

                var informes = new List<tblS_IncidentesInformePreliminar>();

                informes = _context.tblS_IncidentesInformePreliminar
                    .Where(x => idAgrupacion != 0 ? (x.idAgrupacion == idAgrupacion && x.idEmpresa == idEmpresa) : true)
                    .Where(x => x.fechaIncidente >= fechaInicio && x.fechaIncidente <= fechaFin)
                    .ToList()
                    .Where(x =>
                        tipoAccidenteID == -1 ? true :
                            ((x.terminado ? x.Incidentes.First().tipoAccidente_id == tipoAccidenteID : false) ||
                            (x.tipoAccidente_id == tipoAccidenteID)))
                    .Where(x =>
                        supervisorID == -1 ? true :
                            ((x.terminado ? x.Incidentes.First().claveSupervisor.GetValueOrDefault() == supervisorID : false) ||
                            (x.claveSupervisor.GetValueOrDefault() == supervisorID)))
                    .Where(x =>
                        departamentoID == -1 ? true : (x.departamento_id != 0 && x.departamento_id == departamentoID))
                    .Where(x =>
                        estatus == -1 ? true : (int)x.estatusAvance == estatus)
                    .OrderByDescending(x => x.fechaIncidente)
                    .ThenByDescending(x => x.folio)
                    .ToList();

                #region Filtrar por division y lineas de negocios
                if (listaDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division)).ToList();

                    informes = informes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idEmpresa, i.idAgrupacion },
                        cd => new { cd.idEmpresa, cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (listaLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    informes = informes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idEmpresa, i.idAgrupacion },
                        cd => new { cd.idEmpresa, cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                bool puedeEliminar = (new UsuarioDAO().getViewAction(vSesiones.sesionCurrentView, "eliminarEvidencia"));

                var listaAgrupaciones = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).ToList();
                var lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                var lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                var informesPreliminares = informes
                    .Select(x => new informePreliminarDTO
                    {
                        id = x.id,
                        folio = x.folio,
                        cc = x.cc,
                        idEmpresa = x.idEmpresa,
                        idAgrupacion = (int)x.idAgrupacion,
                        proyecto = idEmpresa == 0 ? listaAgrupaciones.Where(y => y.id == (int)x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault() :
                                   idEmpresa == 1000 ? lstContratistas.Where(y => y.id == (int)x.idAgrupacion).Select(z => z.nombreEmpresa).FirstOrDefault() :
                                   lstAgrupacionContratistas.Where(y => y.id == (int)x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault(),
                        claveEmpleado = x.claveEmpleado,
                        empleado = x.esExterno ? x.nombreExterno : ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                        fechaIncidente = x.fechaIncidente.ToString("yyyy/MM/dd"),
                        abreviacionTipoIncidente = x.tipoAccidente_id.HasValue ? x.TiposAccidente.clasificacion.abreviatura : "Sin Definir",
                        tipoIncidenteDesc = x.tipoAccidente_id.HasValue ? x.TiposAccidente.clasificacion.clasificacion : "Sin Definir",
                        terminado = x.terminado,
                        aplicaRIA = Convert.ToInt16(x.aplicaRIA),
                        tienePreliminar = x.rutaPreliminar != null,
                        tieneRIA = x.rutaRIA != null,
                        estatusAvance = x.estatusAvance,
                        estatusAvanceDesc = x.estatusAvance.GetDescription(),
                        puedeEliminar = puedeEliminar,
                        descripcionIncidente = x.descripcionIncidente,
                        accionInmediata = x.accionInmediata,
                        riesgo = x.riesgo,
                        MedidasControl = x.Incidentes.SelectMany(y => y.MedidasControl).Count() > 0 ? x.Incidentes.SelectMany(y => y.MedidasControl).Select(y => y.accionPreventiva).ToList() : new List<string>()
                    }).ToList();


                if (informesPreliminares.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, informesPreliminares);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de informes preliminares");
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el nombre completo de un empleado en base a su clave de empleado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        private string ObtenerNombreEmpleadoPorClave(int claveEmpleado)
        {
            //            var odbc = new OdbcConsultaDTO();

            //            odbc.consulta = @"
            //                    SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM sn_empleados 
            //                    WHERE clave_empleado =  ?";

            //            odbc.parametros.Add(new OdbcParameterDTO()
            //            {
            //                nombre = "claveEmpleado",
            //                tipo = OdbcType.Decimal,
            //                valor = Convert.ToDecimal(claveEmpleado)
            //            });

            //            var resultadoARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);
            //            var resultadoCP = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);


            var resultadoCP = _context.Select<dynamic>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                //baseDatos=(MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM tblRH_EK_Empleados 
                            WHERE clave_empleado =  @claveEmpleado",
                parametros = new { claveEmpleado }
            });

            var resultadoARR = _context.Select<dynamic>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM tblRH_EK_Empleados 
                            WHERE clave_empleado =  @claveEmpleado",
                parametros = new { claveEmpleado }
            });

            var empleado = resultadoCP.Count > 0 ? resultadoCP.FirstOrDefault() : resultadoARR.FirstOrDefault();
            if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
            {
                var resultadoCol = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Colombia,
                    consulta = @"SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM tblRH_EK_Empleados 
                            WHERE clave_empleado =  @claveEmpleado",
                    parametros = new { claveEmpleado }
                });
                var resultadoPeru = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.PERU,
                    consulta = @"SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM tblRH_EK_Empleados 
                            WHERE clave_empleado =  @claveEmpleado",
                    parametros = new { claveEmpleado }
                });

                empleado = resultadoCP.Count > 0 ? resultadoCP.FirstOrDefault() : resultadoPeru.Count() > 0 ? resultadoPeru.FirstOrDefault() : resultadoCol.Count() > 0 ? resultadoCol.FirstOrDefault() : resultadoARR.FirstOrDefault();
            }

            return empleado != null ? empleado.label : "INDEFINIDO";
        }

        public Dictionary<string, object> getInformePreliminarByID(int id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var informe = _context.tblS_IncidentesInformePreliminar.Where(x => x.id == id).ToList().Select(x => new informePreliminarDTO
                {
                    id = x.id,
                    folio = x.folio,
                    cc = x.cc,
                    idEmpresa = x.idEmpresa,
                    idAgrupacion = (int)x.idAgrupacion,
                    //proyecto = _context.tblP_CC.ToList().Where(y => y.cc == x.cc).FirstOrDefault().descripcion,
                    claveEmpleado = x.claveEmpleado,
                    fechaInforme = x.fechaInforme.ToShortDateString(),
                    fechaIncidenteComplete = x.fechaIncidente,
                    fechaIngresoEmpleado = x.fechaIngresoEmpleado.ToShortDateString(),
                    personaInformo = x.personaInformo,
                    departamentoEmpleado = x.departamentoEmpleado,
                    departamento_id = x.departamento_id,
                    puestoEmpleado = x.puestoEmpleado,
                    claveSupervisor = x.claveSupervisor.GetValueOrDefault(),
                    supervisorEmpleado = x.supervisorEmpleado,
                    tipoLesion = x.tipoLesion,
                    descripcionIncidente = x.descripcionIncidente,
                    accionInmediata = x.accionInmediata,
                    aplicaRIA = x.aplicaRIA == true ? 1 : 0,
                    riesgo = x.riesgo,
                    tipoAccidente_id = x.tipoAccidente_id,
                    subclasificacionID = x.subclasificacionID,
                    tipoContacto_id = x.tipoContacto_id,
                    parteCuerpo_id = x.parteCuerpo_id,
                    agenteImplicado_id = x.agenteImplicado_id,
                    esExterno = x.esExterno,
                    nombreExterno = x.nombreExterno,
                    terminado = x.terminado,
                    claveContratista = x.claveContratista,
                    nombreEmpleado = x.esExterno ? x.nombreExterno : ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                    nombrePersonaInformo = ObtenerNombreEmpleadoPorClave(x.personaInformo),
                    procedimientosViolados = x.procedimientosViolados.Select(s => s.id).ToList(),
                    estatusAvance = x.estatusAvance,
                    estatusAvanceDesc = x.estatusAvance.GetDescription(),

                    experienciaEmpleado_id = x.experienciaEmpleado_id,
                    antiguedadEmpleado_id = x.antiguedadEmpleado_id,
                    turnoEmpleado_id = x.turnoEmpleado_id,
                    horasTrabajadasEmpleado = x.horasTrabajadasEmpleado,
                    diasTrabajadosEmpleado = x.diasTrabajadosEmpleado,
                    capacitadoEmpleado = x.capacitadoEmpleado,
                    accidentesAnterioresEmpleado = x.accidentesAnterioresEmpleado,
                    lugarAccidente = x.lugarAccidente,
                    tipoLesion_id = x.tipoLesion_id,
                    actividadRutinaria = x.actividadRutinaria,
                    trabajoPlaneado = x.trabajoPlaneado,
                    trabajoRealizaba = x.trabajoRealizaba,
                    protocoloTrabajo_id = x.protocoloTrabajo_id,
                    descripcionAccidente = x.descripcionAccidente
                }).FirstOrDefault();

                if (informe != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informacion", informe);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de informacion");
            }

            return resultado;
        }
        public Dictionary<string, object> getFolio(string cc)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var folios = _context.tblS_IncidentesInformePreliminar.Where(x => x.cc == cc).Select(x => x.folio).ToList();

                var folio = folios.Count > 0 ? folios.Max() + 1 : 1;

                resultado.Add("folio", folio);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el folio");
            }

            return resultado;
        }
        public Dictionary<string, object> getEvaluacionesRiesgo()
        {

            var resultado = new Dictionary<string, object>();

            try
            {
                List<ComboDTO> potenciales = new List<ComboDTO>();

                potenciales.Add(new ComboDTO() { Text = "Leve", Value = 1 });
                potenciales.Add(new ComboDTO() { Text = "Tolerable", Value = 2 });
                potenciales.Add(new ComboDTO() { Text = "Moderado", Value = 3 });
                potenciales.Add(new ComboDTO() { Text = "Critico", Value = 6 });
                potenciales.Add(new ComboDTO() { Text = "Intolerable", Value = 9 });

                if (potenciales.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", potenciales);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de clasificaciones");
            }

            return resultado;
        }
        public Dictionary<string, object> getUsuariosCCSigoPlan(int idEmpresa, int idAgrupacion)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                if (idEmpresa == 0) //Agrupaciones de Centros de costos
                {
                    var agrupacionCC = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.esActivo && x.id == idAgrupacion);

                    if (agrupacionCC == null)
                    {
                        throw new Exception("No se encuentra la información de la agrupación.");
                    }

                    var listaAgrupacionDetalle = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo && x.idAgrupacionCC == idAgrupacion).ToList();
                    var stringListaCentroCosto = string.Join(", ", listaAgrupacionDetalle.Select(x => "'" + x.cc + "'"));

                    //var odbc = new OdbcConsultaDTO
                    //{
                    //    consulta = string.Format("SELECT clave_empleado as Text FROM DBA.sn_empleados WHERE cc_contable IN (" + stringListaCentroCosto + ") AND estatus_empleado = 'A'"),
                    //    parametros = new List<OdbcParameterDTO>()
                    //};
                    //var listARR = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                    //var listCP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanRh, odbc);

                    var listCP = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        //baseDatos=(MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_empleado as Text FROM tblRH_EK_Empleados WHERE cc_contable IN (" + stringListaCentroCosto + ") AND estatus_empleado = 'A'",

                    });

                    var listARR = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado as Text FROM tblRH_EK_Empleados WHERE cc_contable IN (" + stringListaCentroCosto + ") AND estatus_empleado = 'A'",

                    });
                    if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                    {
                        var listCol = _context.Select<ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Colombia,
                            consulta = @"SELECT clave_empleado as Text FROM tblRH_EK_Empleados WHERE cc_contable IN (" + stringListaCentroCosto + ") AND estatus_empleado = 'A'",

                        });
                        var listPeru = _context.Select<ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.PERU,
                            consulta = @"SELECT clave_empleado as Text FROM tblRH_EK_Empleados WHERE cc_contable IN (" + stringListaCentroCosto + ") AND estatus_empleado = 'A'",

                        });
                        listARR.AddRange(listCol);
                        listARR.AddRange(listPeru);
                    }


                    listARR.AddRange(listCP);


                    var Listempleados = listARR.Select(x => x.Text);

                    var usuarios = _context.tblP_Usuario.Where(x => x.estatus && Listempleados.Contains(x.cveEmpleado)).ToList().Select(x => new
                    {
                        Value = x.id,
                        Text = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                    }).OrderBy(x => x.Text);

                    if (usuarios.Count() > 0)
                    {
                        resultado.Add(SUCCESS, true);
                        resultado.Add("items", usuarios);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add("EMPTY", true);
                    }
                }
                else if (idEmpresa == 1000) //Contratistas.
                {
                    resultado.Add(SUCCESS, true);

                    if (vSesiones.sesionEmpresaActual == 6)
                    {
                        resultado.Add("items", new List<ComboDTO> {
                            new ComboDTO { Value = 0, Text = "comitedirectivo.ssoma-peru@construplan.com.mx" },
                            new ComboDTO { Value = 0, Text = "comitedirectivo.ssoma-peru@construplan.com.pe" }
                        });
                    }
                    else if (vSesiones.sesionEmpresaActual == 3)
                    {
                        resultado.Add("items", new List<ComboDTO> { new ComboDTO { Value = 0, Text = "comitedirectivo.colombia@construplan.com.mx" } });
                    }
                    else
                    {
                        resultado.Add("items", new List<ComboDTO> { new ComboDTO { Value = 0, Text = "comitedirectivo.sst@construplan.com.mx" } });
                    }
                }
                else if (idEmpresa == 2000) //Agrupaciones de Contratistas.
                {
                    resultado.Add(SUCCESS, true);
                    if (vSesiones.sesionEmpresaActual == 6)
                    {
                        resultado.Add("items", new List<ComboDTO> {
                            new ComboDTO { Value = 0, Text = "comitedirectivo.ssoma-peru@construplan.com.mx" },
                            new ComboDTO { Value = 0, Text = "comitedirectivo.ssoma-peru@construplan.com.pe" }
                        });
                    }
                    else if (vSesiones.sesionEmpresaActual == 3)
                    {
                        resultado.Add("items", new List<ComboDTO> { new ComboDTO { Value = 0, Text = "comitedirectivo.colombia@construplan.com.mx" } });
                    }
                    else
                    {
                        resultado.Add("items", new List<ComboDTO> { new ComboDTO { Value = 0, Text = "comitedirectivo.sst@construplan.com.mx" } });
                    }
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo usuarios");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarInforme(InformeDTO captura)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (captura.informe.idAgrupacion == null || captura.informe.idAgrupacion == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Debe seleccionar un centro de costos.");
                        return resultado;
                    }

                    #region Guardar Informe
                    //Se saca el folio desde aquí en vez desde el front-end.
                    var folios = _context.tblS_IncidentesInformePreliminar.Where(x => x.idAgrupacion == captura.informe.idAgrupacion && x.idEmpresa == captura.informe.idEmpresa).Select(x => x.folio).ToList();
                    var folio = folios.Count > 0 ? folios.Max() + 1 : 1;

                    captura.informe.folio = folio;

                    // Validar si el informe no existe
                    var informeExistente = _context.tblS_IncidentesInformePreliminar.FirstOrDefault(x =>
                        x.folio == captura.informe.folio && x.idAgrupacion == captura.informe.idAgrupacion
                    ); //var informeExistente = _context.tblS_IncidentesInformePreliminar.FirstOrDefault(x => x.folio == captura.informe.folio && x.cc == captura.informe.cc);

                    if (informeExistente != null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ya existe un informe para este CC con este número de folio.");
                        return resultado;
                    }

                    if (captura.informe.procedimientosViolados != null && captura.informe.procedimientosViolados.Count > 0)
                    {
                        var lstProcedimientos = _context.tblS_IncidentesTipoProcedimientosViolados.ToList();
                        captura.informe.procedimientosViolados = captura.informe.procedimientosViolados.Select(pv => pv = lstProcedimientos.SingleOrDefault(t => t.id == pv.id)).ToList();
                    }

                    if (captura.informe.esExterno && (captura.informe.nombreExterno == "" || captura.informe.nombreExterno == null))
                    {
                        throw new Exception("No se ha capturado el nombre de la persona externa.");
                    }

                    captura.informe.estatusAvance = EstatusIncidenteEnum.PendienteCargaIP;
                    _context.tblS_IncidentesInformePreliminar.Add(captura.informe);
                    _context.SaveChanges();
                    #endregion

                    if (captura.informe.id == 0)
                    {
                        throw new Exception("El ID del informe no se guardó.");
                    }
                    else
                    {
                        resultado.Add("informeID", captura.informe.id);
                    }

                    #region Guardar Evidencias
                    var nombreFolderInforme = ObtenerNombreFolderInforme(captura.informe);

                    var rutaFolderInforme = Path.Combine(RutaEvidencias, nombreFolderInforme);

                    bool carpetaExistente = verificarExisteCarpeta(rutaFolderInforme, true);

                    if (carpetaExistente == false)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al intentar acceder a la carpeta del informe.");
                        return resultado;
                    }

                    var evidenciasExistentes = _context.tblS_IncidentesEvidencias.Where(x => x.informe_id == captura.informe.id).ToList();

                    if (evidenciasExistentes.Where(x => x.activa).Count() + captura.evidencias.Count > 5)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El número de evidencias a cargar es mayor al límite establecido. Actualmente sólo se pueden cargar hasta 5 evidencias por informe");
                        return resultado;
                    }

                    var consecutivo = (evidenciasExistentes.Count > 0 ? evidenciasExistentes.Max(x => x.numero) : 0) + 1;

                    var usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    var listaArchivosPorCargar = new List<Tuple<HttpPostedFileBase, string>>();

                    foreach (var evidencia in captura.evidencias)
                    {
                        var extensionArchivo = Path.GetExtension(evidencia.FileName);

                        var nombreArchivo = String.Format("{0} - {1} - Evidencia {2}{3}", captura.informe.cc, captura.informe.folio, consecutivo, extensionArchivo);

                        var nuevaEvidencia = new tblS_IncidentesEvidencias
                        {
                            informe_id = captura.informe.id,
                            nombre = nombreArchivo,
                            numero = consecutivo,
                            ruta = Path.Combine(rutaFolderInforme, nombreArchivo),
                            usuarioCreadorID = usuarioCreadorID,
                            fechaCreacion = DateTime.Now,
                            activa = true
                        };
                        _context.tblS_IncidentesEvidencias.Add(nuevaEvidencia);
                        listaArchivosPorCargar.Add(Tuple.Create(evidencia, nuevaEvidencia.ruta));
                        consecutivo++;
                    }

                    foreach (var nuevaEvidencia in listaArchivosPorCargar)
                    {
                        if (SaveArchivo(nuevaEvidencia.Item1, nuevaEvidencia.Item2) == false)
                        {
                            dbTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo guardar la evidencia en el servidor.");
                            return resultado;
                        }
                    }

                    _context.SaveChanges();
                    #endregion

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, "IndicadoresSeguridadController", "GuardarInformePreliminar", e, AccionEnum.AGREGAR, 0, captura.informe);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> updateInforme(tblS_IncidentesInformePreliminar informe)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var registro = _context.tblS_IncidentesInformePreliminar.Where(x => x.id == informe.id).FirstOrDefault();

                registro.fechaInforme = informe.fechaInforme;
                registro.fechaIncidente = informe.fechaIncidente;
                registro.departamentoEmpleado = informe.departamentoEmpleado;
                registro.departamento_id = informe.departamento_id;
                registro.fechaIngresoEmpleado = informe.fechaIngresoEmpleado;
                registro.tipoLesion = informe.tipoLesion;
                registro.descripcionIncidente = informe.descripcionIncidente;
                registro.accionInmediata = informe.accionInmediata;
                registro.aplicaRIA = informe.aplicaRIA;
                registro.riesgo = informe.riesgo;
                registro.tipoAccidente_id = informe.tipoAccidente_id;

                //Se actualiza el tipo de accidente del registro de RIA en el caso que exista.
                var registroRIA = _context.tblS_Incidentes.FirstOrDefault(x => x.informe_id == informe.id);

                if (registroRIA != null)
                {
                    registroRIA.tipoAccidente_id = informe.tipoAccidente_id;
                }

                registro.subclasificacionID = informe.subclasificacionID;
                registro.tipoContacto_id = informe.tipoContacto_id;
                registro.parteCuerpo_id = informe.parteCuerpo_id;
                registro.agenteImplicado_id = informe.agenteImplicado_id;
                registro.procedimientosViolados.Clear();

                if (informe.procedimientosViolados != null && informe.procedimientosViolados.Count > 0)
                {
                    var lstProcedimientos = _context.tblS_IncidentesTipoProcedimientosViolados.ToList();

                    informe.procedimientosViolados = informe.procedimientosViolados.Select(pv => pv = lstProcedimientos.First(t => t.id == pv.id)).ToList();
                }

                if (informe.aplicaRIA)
                {
                    if (registro.rutaPreliminar != null)
                    {
                        if (registro.estatusAvance != EstatusIncidenteEnum.PendienteCargaRIA && registro.estatusAvance != EstatusIncidenteEnum.Completo)
                        {
                            registro.estatusAvance = EstatusIncidenteEnum.PendienteGeneracionRIA;
                        }
                    }
                }
                else
                {
                    registro.estatusAvance = registro.rutaPreliminar != null ? EstatusIncidenteEnum.Completo : EstatusIncidenteEnum.PendienteCargaIP;
                }

                registro.procedimientosViolados = informe.procedimientosViolados ?? new List<tblS_IncidentesTipoProcedimientosViolados>();
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error al actualizar");
                LogError(0, 0, "IndicadoresSeguridadController", "updateInforme", e, AccionEnum.ACTUALIZAR, 0, new { informe = informe });
            }

            return resultado;
        }

        public bool enviarCorreoPreliminar(InformeDTO captura, FormatoRIADTO informacionReporte, List<Byte[]> archivoInformePreliminar)
        {
            bool correoEnviado = false;

            try
            {
                #region Correos
                List<string> correos = new List<string>();

                #region Correos por agrupación
                var registroAgrupacionSIGOPLAN = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.esActivo && x.id == (int)captura.informe.idAgrupacion);

                if (registroAgrupacionSIGOPLAN != null)
                {
                    var listaAgrupacionCC = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo && x.idAgrupacionCC == registroAgrupacionSIGOPLAN.id).ToList();

                    if (listaAgrupacionCC.Count() > 0)
                    {
                        var listaUsuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                        foreach (var cc in listaAgrupacionCC)
                        {
                            //var odbc = new OdbcConsultaDTO
                            //{
                            //    consulta = string.Format(@"SELECT clave_empleado AS Value FROM sn_empleados WHERE cc_contable = ? AND estatus_empleado = 'A'"),
                            //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "cc_contable", tipo = OdbcType.Char, valor = cc.cc } }
                            //};

                            if (cc.idEmpresa == 1)
                            {
                                //var listaEmpleados = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanRh, odbc);

                                var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Construplan,
                                    consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                    parametros = new { cc = cc.cc }
                                });

                                if (listaEmpleados.Count() > 0)
                                {
                                    var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                    var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                        listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com")
                                    ).Select(x => x.correo).ToList();

                                    correos.AddRange(listaCorreos);
                                }
                            }
                            else if (cc.idEmpresa == 2)
                            {
                                //var listaEmpleados = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenRh, odbc);

                                var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Arrendadora,
                                    consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                    parametros = new { cc = cc.cc }

                                });

                                if (listaEmpleados.Count() > 0)
                                {
                                    var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                    var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                        listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com")
                                    ).Select(x => x.correo).ToList();

                                    correos.AddRange(listaCorreos);
                                }
                            }
                            else if (cc.idEmpresa == 3)
                            {
                                var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.Colombia,
                                    consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                    parametros = new { cc = cc.cc }
                                });

                                if (listaEmpleados.Count() > 0)
                                {
                                    var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                    var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                        listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com")
                                    ).Select(x => x.correo).ToList();

                                    correos.AddRange(listaCorreos);
                                }
                            }
                            else if (cc.idEmpresa == 6)
                            {
                                var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                                {
                                    baseDatos = MainContextEnum.PERU,
                                    consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                    parametros = new { cc = cc.cc }
                                });

                                if (listaEmpleados.Count() > 0)
                                {
                                    var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                    var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                        listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com")
                                    ).Select(x => x.correo).ToList();

                                    correos.AddRange(listaCorreos);
                                }
                            }
                        }
                    }
                }
                #endregion

                var empresaActual = vSesiones.sesionEmpresaActual;

                switch ((EmpresaEnum)empresaActual)
                {
                    case EmpresaEnum.Peru:
                        correos.Add("comitedirectivo.ssoma-peru@construplan.com.mx");
                        correos.Add("comitedirectivo.ssoma-peru@construplan.com.pe");
                        break;
                    case EmpresaEnum.Colombia:
                        correos.Add("comitedirectivo.colombia@construplan.com.mx");
                        break;
                    default:
                        correos.Add("comitedirectivo.sst@construplan.com.mx");
                        break;
                }
                //correos.Add("comitedirectivo.sst@construplan.com.mx");

#if DEBUG
                correos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif
                #endregion

                string nombreEmpleado = "";

                if (captura.informe.esExterno)
                {
                    nombreEmpleado = captura.informe.nombreExterno;
                }
                else
                {
                    var info = InfEmpleado(captura.informe.claveEmpleado, false, 0); //TODO
                    nombreEmpleado = info != null ? info.nombreEmpleado : "";
                }

                #region Agrupación
                var agrupacionDesc = "";
                var agrupacionSIGOPLAN = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.esActivo && x.id == (int)captura.informe.idAgrupacion);

                if (agrupacionSIGOPLAN != null)
                {
                    agrupacionDesc = agrupacionSIGOPLAN.nomAgrupacion;
                }
                #endregion

                var asunto = informacionReporte.tipoAccidente + @" Folio: " + agrupacionDesc + @" #" + captura.informe.folio +
                    @" Fecha: " + captura.informe.fechaIncidente.ToShortDateString();
                var mensaje = string.Format(@"
                Proyecto: {0} <br/>
                Fecha y hora: {1} <br/>
                Tipo de Accidente: {2} <br/><br/>
                Empleado: {3} <br/>
                Puesto: {4} Antigüedad: {5} <br/>
                Departamento: {6} <br/><br/>
                Trabajo Realizado: {7} <br/>
                Descripción: {8} <br/><br/>
                Acciones Inmediatas y Pronostico de Lesión o Daños: {9}
            ",
                 agrupacionDesc,
                 captura.informe.fechaIncidente.ToString(),
                 informacionReporte.tipoAccidente,
                 nombreEmpleado,
                 captura.informe.puestoEmpleado,
                 informacionReporte.antiguedadEmpleado,
                 informacionReporte.departamento,
                 informacionReporte.trabajoRealizaba,
                 informacionReporte.descripcionAccidente,
                 captura.informe.accionInmediata
                 );

                var evidencias = _context.tblS_IncidentesEvidencias.Where(x => x.informe_id == captura.informe.id && x.activa).ToList();

                var archivosAdjuntos = new List<adjuntoCorreoDTO>();

                // Se adjuntan evidencias si tiene
                if (evidencias.Count > 0)
                {
                    archivosAdjuntos.AddRange(evidencias.Select(x => new adjuntoCorreoDTO
                    {
                        archivo = File.ReadAllBytes(x.ruta),
                        extArchivo = Path.GetExtension(x.nombre),
                        nombreArchivo = x.nombre
                    }).ToList());
                }

                if (archivoInformePreliminar != null)
                {
                    archivosAdjuntos.Add(new adjuntoCorreoDTO
                    {
                        archivo = archivoInformePreliminar[0],
                        extArchivo = ".pdf",
                        nombreArchivo = "Informe Preliminar"
                    });
                }

                //// Se adjunta informe preliminar si tiene
                //if (captura.informe.rutaPreliminar != null)
                //{
                //    archivosAdjuntos.Add(new adjuntoCorreoDTO
                //    {
                //        archivo = File.ReadAllBytes(captura.informe.rutaPreliminar),
                //        extArchivo = Path.GetExtension(captura.informe.rutaPreliminar),
                //        nombreArchivo = Path.GetFileNameWithoutExtension(captura.informe.rutaPreliminar)
                //    });
                //}

                if (archivosAdjuntos.Count > 0)
                {
                    //correoEnviado = GlobalUtils.sendMailWithFiles(asunto, mensaje, correos, archivosAdjuntos);
                    correoEnviado = GlobalUtils.sendMailWithFilesSeguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos, archivosAdjuntos);
                }
                else
                {
                    //correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, correos);
                    correoEnviado = GlobalUtils.sendEmailSeguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos);
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "EnviarCorreoPreliminar", e, AccionEnum.AGREGAR, 0, captura.informe);
            }

            return correoEnviado;
        }

        public bool enviarCorreoIncidente(IncidenteDTO captura, FormatoRIADTO informacionReporte, List<Byte[]> archivoIncidente)
        {
            bool correoEnviado = false;

            #region Correos
            List<string> correos = new List<string>();

            #region Correos por agrupación
            var registroAgrupacionSIGOPLAN = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.esActivo && x.id == (int)captura.incidente.idAgrupacion);

            if (registroAgrupacionSIGOPLAN != null)
            {
                var listaAgrupacionCC = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.esActivo && x.idAgrupacionCC == registroAgrupacionSIGOPLAN.id).ToList();

                if (listaAgrupacionCC.Count() > 0)
                {
                    var listaUsuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                    foreach (var cc in listaAgrupacionCC)
                    {
                        if (cc.idEmpresa == 1)
                        {
                            var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Construplan,
                                consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                parametros = new { cc = cc.cc }
                            });

                            if (listaEmpleados.Count() > 0)
                            {
                                var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                    listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com.mx")
                                ).Select(x => x.correo).ToList();

                                correos.AddRange(listaCorreos);
                            }
                        }
                        else if (cc.idEmpresa == 2)
                        {
                            var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Arrendadora,
                                consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                parametros = new { cc = cc.cc }
                            });

                            if (listaEmpleados.Count() > 0)
                            {
                                var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                    listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com.mx")
                                ).Select(x => x.correo).ToList();

                                correos.AddRange(listaCorreos);
                            }
                        }
                        else if (cc.idEmpresa == 6)
                        {
                            var listaEmpleados = _context.Select<ComboDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.PERU,
                                consulta = @"SELECT clave_empleado AS Value FROM tblRH_EK_Empleados WHERE cc_contable = @cc AND estatus_empleado = 'A'",
                                parametros = new { cc = cc.cc }
                            });

                            if (listaEmpleados.Count() > 0)
                            {
                                var listaClaves = listaEmpleados.Select(y => y.Value.ToString());
                                var listaCorreos = listaUsuariosSIGOPLAN.Where(x =>
                                    listaClaves.Contains(x.cveEmpleado) && x.correo.Contains("@construplan.com")
                                ).Select(x => x.correo).ToList();

                                correos.AddRange(listaCorreos);
                            }
                        }
                    }
                }
            }
            #endregion

            var empresaActual = vSesiones.sesionEmpresaActual;

            switch ((EmpresaEnum)empresaActual)
            {
                case EmpresaEnum.Peru:
                    correos.Add("comitedirectivo.ssoma-peru@construplan.com.mx");
                    correos.Add("comitedirectivo.ssoma-peru@construplan.com.pe");
                    break;
                case EmpresaEnum.Colombia:
                    correos.Add("comitedirectivo.colombia@construplan.com.mx");
                    break;
                default:
                    correos.Add("comitedirectivo.sst@construplan.com.mx");
                    break;
            }

            //correos.Add("comitedirectivo.sst@construplan.com.mx");

#if DEBUG
            correos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif
            #endregion

            string nombreEmpleado = "";

            if (captura.incidente.esExterno)
            {
                nombreEmpleado = captura.incidente.nombreEmpleadoExterno;
            }
            else
            {
                var info = InfEmpleado(captura.incidente.claveEmpleado, false, 0); //TODO
                nombreEmpleado = info != null ? info.nombreEmpleado : "";
            }

            #region Agrupación
            var agrupacionDesc = "";
            var agrupacionSIGOPLAN = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.esActivo && x.id == (int)captura.incidente.idAgrupacion);

            if (agrupacionSIGOPLAN != null)
            {
                agrupacionDesc = agrupacionSIGOPLAN.nomAgrupacion;
            }
            #endregion

            var asunto = @"Reporte Investigación: Folio: " + agrupacionDesc + @" #" + captura.incidente.Informe.folio +
                @" Fecha: " + captura.incidente.fechaAccidente.ToShortDateString();
            var mensaje = string.Format(@"
                Proyecto: {0} <br/>
                Fecha y hora: {1} <br/>
                Tipo de Accidente: {2} <br/><br/>
                Empleado: {3} <br/>
                Puesto: {4} Antigüedad: {5} <br/>
                Departamento: {6} <br/><br/>
                Trabajo Realizado: {7} <br/>
                Descripción: {8} <br/><br/>
                Acciones Inmediatas y Pronostico de Lesión o Daños: {9}
            ",
             agrupacionDesc,
             captura.incidente.fechaAccidente.ToString(),
             informacionReporte.tipoAccidente,
             nombreEmpleado,
             captura.incidente.puestoEmpleado,
             informacionReporte.antiguedadEmpleado,
             informacionReporte.departamento,
             informacionReporte.trabajoRealizaba,
             informacionReporte.descripcionAccidente,
             captura.incidente.Informe.accionInmediata
             );

            var archivosAdjuntos = new List<adjuntoCorreoDTO>();

            #region Archivos Evidencias
            var evidencias = _context.tblS_IncidentesEvidenciasRIA.Where(x => x.incidente_id == captura.incidente.id && x.estatus).ToList();

            if (evidencias.Count > 0)
            {
                archivosAdjuntos.AddRange(evidencias.Select(x => new adjuntoCorreoDTO
                {
                    archivo = File.ReadAllBytes(x.ruta),
                    extArchivo = Path.GetExtension(x.nombre),
                    nombreArchivo = x.nombre
                }).ToList());
            }
            #endregion

            #region Archivo Formato RIA
            if (archivoIncidente != null)
            {
                archivosAdjuntos.Add(new adjuntoCorreoDTO
                {
                    archivo = archivoIncidente[0],
                    extArchivo = ".pdf",
                    nombreArchivo = "Incidente"
                });
            }
            #endregion

            //// Se adjunta informe preliminar si tiene
            //if (captura.informe.rutaPreliminar != null)
            //{
            //    archivosAdjuntos.Add(new adjuntoCorreoDTO
            //    {
            //        archivo = File.ReadAllBytes(captura.informe.rutaPreliminar),
            //        extArchivo = Path.GetExtension(captura.informe.rutaPreliminar),
            //        nombreArchivo = Path.GetFileNameWithoutExtension(captura.informe.rutaPreliminar)
            //    });
            //}

            if (archivosAdjuntos.Count > 0)
            {
                //correoEnviado = GlobalUtils.sendMailWithFiles(asunto, mensaje, correos, archivosAdjuntos);
                correoEnviado = GlobalUtils.sendMailWithFilesSeguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos, archivosAdjuntos);
            }
            else
            {
                //correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, correos);
                correoEnviado = GlobalUtils.sendEmailSeguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos);
            }

            return correoEnviado;
        }

        public Dictionary<string, object> enviarCorreo(int informe_id, List<int> usuarios)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<string> correos = new List<string>();

                bool correoEnviado = false;

                if (usuarios != null)
                {
                    correos = _context.tblP_Usuario.Where(x => usuarios.Contains(x.id) && x.correo.Contains("@construplan.com.mx")).Select(x => x.correo).ToList();
                }

                var informe = _context.tblS_IncidentesInformePreliminar.First(x => x.id == informe_id);

                if (informe.idEmpresa >= 1000)
                {
                    var empresaActual = vSesiones.sesionEmpresaActual;
                    switch ((EmpresaEnum)empresaActual)
                    {
                        case EmpresaEnum.Peru:
                            correos = new List<string>() {
                                "comitedirectivo.ssoma-peru@construplan.com.mx",
                                "comitedirectivo.ssoma-peru@construplan.com.pe"
                            };
                            break;
                        case EmpresaEnum.Colombia:
                            correos.Add("comitedirectivo.colombia@construplan.com.mx");
                            break;
                        default:
                            correos = new List<string>() { "comitedirectivo.sst@construplan.com.mx" };
                            break;
                    }
                }

                string nombreEmpleado = "";

                if (informe.esExterno)
                {
                    nombreEmpleado = informe.nombreExterno;
                }
                else
                {
                    var info = InfEmpleado(informe.claveEmpleado, false, 0); //TODO
                    nombreEmpleado = info != null ? info.nombreEmpleado : "";
                }

                #region Agrupación
                var agrupacionDesc = "";
                var agrupacionSIGOPLAN = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.esActivo && x.id == (int)informe.idAgrupacion);

                if (agrupacionSIGOPLAN != null)
                {
                    agrupacionDesc = agrupacionSIGOPLAN.nomAgrupacion;
                }
                #endregion

                var asunto = "Reporte Incidente: Folio: " + agrupacionDesc + " #" + informe.folio + " Fecha: " + informe.fechaIncidente.ToShortDateString();
                var mensaje = @"Proyecto: " + agrupacionDesc;
                mensaje += @"<br/>Fecha y hora: " + informe.fechaIncidente.ToString() + @"<br/>";
                mensaje += @"<br/>Empleado: " + nombreEmpleado + @"<br/>";
                mensaje += @"<br/>Puesto: " + informe.puestoEmpleado + @"<br/>";
                mensaje += @"<br/>Descripción: " + informe.descripcionIncidente + @"<br/>";

                var evidencias = _context.tblS_IncidentesEvidencias.Where(x => x.informe_id == informe_id && x.activa).ToList();

                var archivosAdjuntos = new List<adjuntoCorreoDTO>();

                // Se adjuntan evidencias si tiene
                if (evidencias.Count > 0)
                {
                    archivosAdjuntos.AddRange(evidencias.Select(x => new adjuntoCorreoDTO
                    {
                        archivo = File.ReadAllBytes(x.ruta),
                        extArchivo = Path.GetExtension(x.nombre),
                        nombreArchivo = x.nombre
                    }).ToList());
                }

                // Se adjunta informe preliminar si tiene
                if (informe.rutaPreliminar != null)
                {
                    archivosAdjuntos.Add(new adjuntoCorreoDTO
                    {
                        archivo = File.ReadAllBytes(informe.rutaPreliminar),
                        extArchivo = Path.GetExtension(informe.rutaPreliminar),
                        nombreArchivo = Path.GetFileNameWithoutExtension(informe.rutaPreliminar)
                    });
                }

                // Se adjunta RIA si tiene
                if (informe.rutaRIA != null)
                {
                    archivosAdjuntos.Add(new adjuntoCorreoDTO
                    {
                        archivo = File.ReadAllBytes(informe.rutaRIA),
                        extArchivo = Path.GetExtension(informe.rutaRIA),
                        nombreArchivo = Path.GetFileNameWithoutExtension(informe.rutaRIA)
                    });
                }

                if (archivosAdjuntos.Count > 0)
                {
                    //correoEnviado = GlobalUtils.sendMailWithFiles(asunto, mensaje, correos, archivosAdjuntos);
                    correoEnviado = GlobalUtils.sendMailWithFilesSeguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos, archivosAdjuntos);
                }
                else
                {
                    //correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, correos);
                    correoEnviado = GlobalUtils.sendEmailSeguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos);
                }

                if (correoEnviado)
                {
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(ERROR, "Ocurrió un error al mandar correo");
                }
            }
            catch (Exception e)
            {
                resultado.Add(ERROR, "Ocurrió un error al mandar correo");
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerEvidenciasInforme(int informeID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                bool permisoEliminar = (new UsuarioDAO().getViewAction(vSesiones.sesionCurrentView, "eliminarEvidencia"));

                var evidencias = _context.tblS_IncidentesEvidencias
                    .Where(x => x.informe_id == informeID && x.activa)
                    .OrderBy(x => x.numero)
                    .ToList()
                    .Select(x => new
                    {
                        x.id,
                        x.nombre,
                        fecha = x.fechaCreacion.ToShortDateString(),
                        tieneEvidencia = true,
                        puedeEliminar = permisoEliminar || x.usuarioCreadorID == vSesiones.sesionUsuarioDTO.id
                    });

                resultado.Add(ITEMS, evidencias);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerEvidenciasInforme", e, AccionEnum.CONSULTA, informeID, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar cargar las evidencias del informe.");
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerEvidenciasRIA(int informeID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                bool permisoEliminar = (new UsuarioDAO().getViewAction(vSesiones.sesionCurrentView, "eliminarEvidencia"));
                var incidente = _context.tblS_Incidentes.FirstOrDefault(x => x.informe_id == informeID);

                if (incidente == null)
                {
                    throw new Exception("No se encuentra la información del RIA.");
                }

                var evidencias = _context.tblS_IncidentesEvidenciasRIA
                    .Where(x => x.incidente_id == incidente.id && x.estatus)
                    .OrderBy(x => x.numero)
                    .ToList()
                    .Select(x => new
                    {
                        x.id,
                        x.nombre,
                        fecha = x.fechaCreacion != null ? ((DateTime)x.fechaCreacion).ToShortDateString() : "",
                        tieneEvidencia = true,
                        puedeEliminar = permisoEliminar || x.usuarioCreadorID == vSesiones.sesionUsuarioDTO.id
                    });

                if (evidencias.Count() == 0)
                {
                    throw new Exception("El RIA no tiene evidencias capturadas.");
                }

                resultado.Add(ITEMS, evidencias);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerEvidenciasInforme", e, AccionEnum.CONSULTA, informeID, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private string ObtenerNombreFolderInforme(tblS_IncidentesInformePreliminar informe)
        {
            return String.Format("{0}[{1}] - {2}", informe.cc, informe.folio, informe.fechaIncidente.ToShortDateString().Replace("/", "-"));
        }

        private string ObtenerNombreFolderIncidente(tblS_Incidentes incidente)
        {
            return String.Format("{0}[{1}] - {2}", incidente.cc, incidente.Informe.folio, incidente.fechaAccidente.ToShortDateString().Replace("/", "-"));
        }

        public Dictionary<string, object> GuardarEvidencias(List<HttpPostedFileBase> evidencias, int informeID)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (evidencias == null || evidencias.Count == 0 || informeID == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Faltan datos para poder realizar la operación");
                        return resultado;
                    }

                    var informe = _context.tblS_IncidentesInformePreliminar.First(x => x.id == informeID);

                    var nombreFolderInforme = ObtenerNombreFolderInforme(informe);

                    var rutaFolderInforme = Path.Combine(RutaEvidencias, nombreFolderInforme);

                    bool carpetaExistente = verificarExisteCarpeta(rutaFolderInforme, true);

                    if (carpetaExistente == false)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al intentar acceder a la carpeta del informe.");
                        return resultado;
                    }

                    var evidenciasExistentes = _context.tblS_IncidentesEvidencias.Where(x => x.informe_id == informeID).ToList();

                    if (evidenciasExistentes.Where(x => x.activa).Count() + evidencias.Count > 5)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El número de evidencias a cargar es mayor al límite establecido. Actualmente sólo se pueden cargar hasta 5 evidencias por informe");
                        return resultado;
                    }

                    var consecutivo = (evidenciasExistentes.Count > 0 ? evidenciasExistentes.Max(x => x.numero) : 0) + 1;

                    var usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    var listaArchivosPorCargar = new List<Tuple<HttpPostedFileBase, string>>();

                    foreach (var evidencia in evidencias)
                    {
                        var extensionArchivo = Path.GetExtension(evidencia.FileName);

                        var nombreArchivo = String.Format("{0} - {1} - Evidencia {2}{3}", informe.cc, informe.folio, consecutivo, extensionArchivo);

                        var nuevaEvidencia = new tblS_IncidentesEvidencias
                        {
                            informe_id = informeID,
                            nombre = nombreArchivo,
                            numero = consecutivo,
                            ruta = Path.Combine(rutaFolderInforme, nombreArchivo),
                            usuarioCreadorID = usuarioCreadorID,
                            fechaCreacion = DateTime.Now,
                            activa = true
                        };
                        _context.tblS_IncidentesEvidencias.Add(nuevaEvidencia);
                        listaArchivosPorCargar.Add(Tuple.Create(evidencia, nuevaEvidencia.ruta));
                        consecutivo++;
                    }

                    foreach (var nuevaEvidencia in listaArchivosPorCargar)
                    {
                        if (SaveArchivo(nuevaEvidencia.Item1, nuevaEvidencia.Item2) == false)
                        {
                            dbTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo guardar la evidencia en el servidor.");
                            return resultado;
                        }
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, "IndicadoresSeguridadController", "GuardarEvidencias", e, AccionEnum.AGREGAR, informeID, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar los exámenes en el servidor.");
                }

                return resultado;
            }
        }

        public Tuple<Stream, string> DescargarEvidenciaInforme(int evidenciaID)
        {
            try
            {
                var evidencia = _context.tblS_IncidentesEvidencias.First(x => x.id == evidenciaID);

                var fileStream = GlobalUtils.GetFileAsStream(evidencia.ruta);
                string name = Path.GetFileName(evidencia.ruta);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "DescargarEvidenciaExtracurricular", e, AccionEnum.DESCARGAR, evidenciaID, 0);
                return null;
            }
        }

        public Tuple<Stream, string> DescargarEvidenciaRIA(int evidenciaID)
        {
            try
            {
                var evidencia = _context.tblS_IncidentesEvidenciasRIA.First(x => x.id == evidenciaID);

                var fileStream = GlobalUtils.GetFileAsStream(evidencia.ruta);
                string name = Path.GetFileName(evidencia.ruta);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "DescargarEvidenciaRIA", e, AccionEnum.DESCARGAR, evidenciaID, 0);
                return null;
            }
        }

        public Dictionary<string, object> EliminarEvidencia(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var evidencia = _context.tblS_IncidentesEvidencias.First(x => x.id == evidenciaID);

                evidencia.activa = false;
                resultado.Add(SUCCESS, true);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "EliminarEvidencia", e, AccionEnum.ELIMINAR, evidenciaID, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al interntar eliminar la evidencia en el servidor.");
                return resultado;
            }

            return resultado;
        }

        public Dictionary<string, object> CargarDatosEvidencia(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var evidencia = _context.tblS_IncidentesEvidencias.First(x => x.id == evidenciaID);

                // Se valida la extensión del archivo:
                if (EsExtensionInvalidaVisor(evidencia.nombre))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Archivo inválido. Este tipo de archivo no es compatible con el visor de documentos.");
                    return resultado;
                }

                Stream fileStream = GlobalUtils.GetFileAsStream(evidencia.ruta);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(evidencia.nombre).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "CargarDatosEvidencia", e, AccionEnum.CONSULTA, evidenciaID, null);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CargarDatosEvidenciaRIA(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var evidencia = _context.tblS_IncidentesEvidenciasRIA.First(x => x.id == evidenciaID);

                // Se valida la extensión del archivo:
                if (EsExtensionInvalidaVisor(evidencia.nombre))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Archivo inválido. Este tipo de archivo no es compatible con el visor de documentos.");
                    return resultado;
                }

                Stream fileStream = GlobalUtils.GetFileAsStream(evidencia.ruta);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(evidencia.nombre).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "CargarDatosEvidenciaRIA", e, AccionEnum.CONSULTA, evidenciaID, null);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> SubirReporteIncidente(HttpPostedFileBase archivo, int informeID, bool esRIA)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var informe = _context.tblS_IncidentesInformePreliminar.First(x => x.id == informeID);

                    var rutaFolderIncidente = Path.Combine(RutaEvidencias, ObtenerNombreFolderInforme(informe));

                    // Verifica si existe la carpeta y si no, la crea.
                    if (verificarExisteCarpeta(rutaFolderIncidente, true) == false)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo acceder a la carpeta en el servidor.");
                        return resultado;
                    }

                    var nombreArchivo = String.Format("{0}{1}", esRIA ? "Reporte de Investigación de Accidentes" : "Informe Preliminar", Path.GetExtension(archivo.FileName));

                    var rutaArchivo = Path.Combine(rutaFolderIncidente, nombreArchivo);

                    // Se actualizan campos de la entidad
                    if (esRIA)
                    {
                        informe.estatusAvance = EstatusIncidenteEnum.Completo;
                        informe.rutaRIA = rutaArchivo;
                    }
                    else
                    {
                        if (informe.rutaRIA == null)
                        {
                            if (informe.estatusAvance != EstatusIncidenteEnum.PendienteCargaRIA && informe.estatusAvance != EstatusIncidenteEnum.Completo)
                            {
                                informe.estatusAvance = informe.aplicaRIA ? EstatusIncidenteEnum.PendienteGeneracionRIA : EstatusIncidenteEnum.Completo;
                            }
                        }
                        // Validación registros antigüos. Si ya tenía RIA cargado y no preliminar, se pone como completo al subir el prelminar.
                        else
                        {
                            informe.estatusAvance = EstatusIncidenteEnum.Completo;
                        }
                        informe.rutaPreliminar = rutaArchivo;
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);

                    // Si creó el archivo.
                    if (SaveArchivo(archivo, rutaArchivo))
                    {
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        // Si falla al guardar el archivo
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo guardar el archivo en el servidor.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "IndicadoresSeguridadController", "SubirReporteIncidente", e, AccionEnum.AGREGAR, informeID, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar el archivo en el servidor.");
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarReporte(int informeID, bool esRIA)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var informe = _context.tblS_IncidentesInformePreliminar.First(x => x.id == informeID);

                var rutaArchivo = esRIA ? informe.rutaRIA : informe.rutaPreliminar;

                var fileStream = GlobalUtils.GetFileAsStream(rutaArchivo);
                string name = Path.GetFileName(rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "DescargarReporte", e, AccionEnum.DESCARGAR, informeID, 0);
                return null;
            }
        }

        public Dictionary<string, object> LlenarComboSupervisorIncidente()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var listaSupervisores = _context.tblS_IncidentesInformePreliminar
                    .Where(x => x.claveSupervisor != null)
                    .ToList()
                    .Select(x => new ComboDTO
                    {
                        Value = x.claveSupervisor.GetValueOrDefault(),
                        Text = x.supervisorEmpleado.Trim(),
                        Prefijo = x.claveSupervisor.ToString()
                    }).ToList();

                var auxListaSupervisores = _context.tblS_Incidentes
                    .Where(x => x.claveSupervisor != null)
                    .ToList()
                    .Select(x => new ComboDTO
                    {
                        Value = x.claveSupervisor.GetValueOrDefault(),
                        Text = x.supervisorCargoEmpleado.Trim(),
                        Prefijo = x.claveSupervisor.ToString()
                    }).ToList();

                listaSupervisores.AddRange(auxListaSupervisores);

                listaSupervisores = listaSupervisores.DistinctBy(x => x.Value).OrderBy(x => x.Text).ToList();

                if (listaSupervisores.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", listaSupervisores);
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
            }
            return resultado;
        }

        public Dictionary<string, object> LlenarComboDepartamentoIncidente()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var listaDepartamentos = _context.tblS_IncidentesDepartamentos.Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.departamento,
                }).Distinct().OrderBy(x => x.Text).ToList();
                if (listaDepartamentos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", listaDepartamentos);
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
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarIncidente(int id)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var informe = _context.tblS_IncidentesInformePreliminar.First(x => x.id == id);

                    if (informe == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró el incidente que desea eliminar.");
                        return resultado;
                    }

                    // Eliminamos RIA
                    if (informe.Incidentes.Count > 0)
                    {
                        var incidentes = _context.tblS_Incidentes.Where(x => x.informe_id == informe.id).ToList();
                        _context.tblS_Incidentes.RemoveRange(incidentes);
                    }

                    // Eliminamos procedimientos violados.
                    informe.procedimientosViolados.Clear();

                    // Eliminamos evidencias en caso de que tenga.
                    var evidencias = _context.tblS_IncidentesEvidencias.Where(x => x.informe_id == informe.id).ToList();
                    if (evidencias.Count > 0)
                    {
                        _context.tblS_IncidentesEvidencias.RemoveRange(evidencias);
                    }

                    var rutaFolderIncidente = Path.Combine(RutaEvidencias, ObtenerNombreFolderInforme(informe));

                    _context.tblS_IncidentesInformePreliminar.Remove(informe);
                    _context.SaveChanges();

                    // Verifica si existe la carpeta.
                    if (verificarExisteCarpeta(rutaFolderIncidente))
                    {
                        // Si existe, se intenta eliminar el folder con todos sus archivos.
                        try
                        {
                            Directory.Delete(rutaFolderIncidente, true);
                        }
                        catch (Exception) { }
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "IndicadoresSeguridadController", "EliminarIncidente", e, AccionEnum.ELIMINAR, id, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar eliminar el incidente en el servidor.");
                }
            }

            return resultado;
        }
        #endregion

        #region CAPTURA ACCIDENTE
        public Dictionary<string, object> getTiposAccidentesList()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var tiposAccidentes = _context.tblS_IncidentesTipos.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = String.Format("{0} ({1})", x.incidenteTipo, x.clasificacion.abreviatura)
                }).OrderBy(x => x.Value).ToList();

                if (tiposAccidentes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", tiposAccidentes);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo tipos de accidentes");
            }

            return resultado;
        }
        public Dictionary<string, object> GetSubclasificacionesAccidente()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subclasificaciones = _context.tblS_IncidentesSubclasificacion
                    .Where(x => x.activo)
                    .ToList().
                    Select(x => new
                    {
                        Value = x.id,
                        Text = x.subclasificacion
                    })
                    .OrderBy(x => x.Value)
                    .ToList();

                if (subclasificaciones.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", subclasificaciones);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de subclasificaciones");
            }

            return resultado;
        }
        public Dictionary<string, object> getDepartamentosList()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var departamentos = _context.tblS_IncidentesDepartamentos.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.departamento
                }).OrderBy(x => x.Text).ToList();

                if (departamentos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", departamentos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo departamentos");
            }
            return resultado;
        }
        public Dictionary<string, object> getSupervisoresList()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cbo = new List<ComboDTO>();
                var lstInfo = _context.tblS_IncidentesInformePreliminar.ToList()
                    .Where(w => w.claveSupervisor != null)
                    .GroupBy(g => g.claveSupervisor)
                    .Select(s => new ComboDTO()
                    {
                        Value = s.Key.ParseLong(0),
                        Text = s.FirstOrDefault().supervisorEmpleado
                    }).ToList();
                var lstIncidentes = _context.tblS_Incidentes.ToList()
                    .Where(w => w.claveSupervisor != null)
                    .GroupBy(g => g.claveSupervisor)
                    .Select(s => new ComboDTO()
                    {
                        Value = s.Key.ParseLong(0),
                        Text = s.FirstOrDefault().supervisorCargoEmpleado
                    }).ToList();
                cbo.AddRange(lstInfo);
                cbo.AddRange(lstIncidentes);
                cbo = cbo
                    .GroupBy(g => g.Value)
                    .Select(s => s.FirstOrDefault())
                    .DistinctBy(x => x.Text)
                    .OrderBy(x => x.Text)
                    .ToList();

                if (cbo.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", cbo);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo departamentos");
            }
            return resultado;
        }
        public Dictionary<string, object> getSupervisoresIncidentesList()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var cbo = new List<ComboDTO>();
                var lstIncidentes = _context.tblS_Incidentes.ToList()
                    .Where(w => w.claveSupervisor != null)
                    .GroupBy(g => g.claveSupervisor)
                    .Select(s => new ComboDTO()
                    {
                        Value = s.Key.ParseLong(0),
                        Text = s.FirstOrDefault().supervisorCargoEmpleado
                    }).ToList();
                cbo.AddRange(lstIncidentes);
                cbo = cbo
                    .GroupBy(g => g.Value)
                    .Select(s => s.FirstOrDefault())
                    .DistinctBy(x => x.Text)
                    .OrderBy(x => x.Text)
                    .ToList();

                if (cbo.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", cbo);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo departamentos");
            }
            return resultado;
        }
        public Dictionary<string, object> getTipoProcedimientosVioladosList()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var departamentos = _context.tblS_IncidentesTipoProcedimientosViolados.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.Procedimineto
                }).OrderBy(x => x.Value).ToList();

                if (departamentos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", departamentos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo departamentos");
            }
            return resultado;
        }
        public Dictionary<string, object> getTiposLesionList()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var tiposLesion = _context.tblS_IncidentesTipoLesion.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.tipoLesion
                }).OrderBy(x => x.Value).ToList();

                if (tiposLesion.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", tiposLesion);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de tipos de lesiones");
            }

            return resultado;
        }
        public Dictionary<string, object> getPartesCuerposList()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var partesCuerpo = _context.tblS_IncidentesPartesCuerpo.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.parteCuerpo
                }).OrderBy(x => x.Value).ToList();

                if (partesCuerpo.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", partesCuerpo);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de partes de cuerpo");
            }

            return resultado;
        }
        public Dictionary<string, object> getTiposContactoList()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var tiposContacto = _context.tblS_IncidentesTipoContacto.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.tipoContacto
                }).OrderBy(x => x.Value).ToList();

                if (tiposContacto.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", tiposContacto);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de tipos de contacto");
            }

            return resultado;
        }
        public Dictionary<string, object> getProtocolosTrabajoList()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var protocolos = _context.tblS_IncidentesProtocolosTrabajo.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.protocoloTrabajo
                }).OrderBy(x => x.Value).ToList();

                if (protocolos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", protocolos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de protocolo de trabajo");
            }

            return resultado;
        }
        public Dictionary<string, object> getAgentesImplicados()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var agentes = _context.tblS_IncidentesAgentesImplicados.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.agenteImplicado
                }).OrderBy(x => x.Value).ToList();

                if (agentes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", agentes);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de agentes implicados");
            }

            return resultado;
        }
        public Dictionary<string, object> getExperienciaEmpleados()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var experiencias = _context.tblS_IncidentesEmpleadoExperiencia.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.empleadoExperiencia
                }).OrderBy(x => x.Value).ToList();

                if (experiencias.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", experiencias);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de agentes implicados");
            }

            return resultado;
        }
        public Dictionary<string, object> getAntiguedadEmpleados()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var antiguedades = _context.tblS_IncidentesEmpleadoAntiguedad.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.antiguedadEmpleado
                }).OrderBy(x => x.Value).ToList();

                if (antiguedades.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", antiguedades);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de antigüedad");
            }

            return resultado;
        }
        public Dictionary<string, object> getTurnosEmpleado()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var turnos = _context.tblS_IncidentesEmpleadosTurno.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.turno
                }).OrderBy(x => x.Value).ToList();

                if (turnos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", turnos);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de turnos");
            }

            return resultado;
        }
        public Dictionary<string, object> getEmpleadosContratistasList(int claveContratista)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var empleados = _context.tblS_IncidentesEmpleadosContratistas.Where(x => x.claveContratista == claveContratista).Select(x => new
                {
                    Value = x.id,
                    Text = x.nombre
                }).OrderBy(x => x.Value).ToList();

                if (empleados.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", empleados);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de empleados de subcontratistas");
            }

            return resultado;
        }
        public Dictionary<string, object> obtenerCentrosCostos()
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var listaCC = _context.tblP_CC.Where(x => x.estatus == true).Select(x => new
                {
                    Value = x.cc,
                    Text = x.cc + " - " + x.descripcion.Trim(),
                    Prefijo = x.id
                }).OrderBy(x => x.Text).ToList();

                if (listaCC.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", listaCC);
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
            }

            return resultado;
        }
        public Dictionary<string, object> ObtenerCentrosCostosUsuario()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var usuarioActualID = vSesiones.sesionUsuarioDTO.id;

                var ccsUsuario = _context.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioActualID).Select(x => x.cc).ToList();

                var ccsAplicables = new List<string>();

                ccsAplicables.AddRange(_context.tblS_IncidentesInformePreliminar.Select(x => x.cc).Distinct().ToList());

                ccsAplicables.AddRange(_context.tblS_IncidentesInformacionColaboradores.Select(x => x.cc).Distinct().ToList());
                ccsAplicables = ccsAplicables.Distinct().ToList();

                bool esAdmin = (new UsuarioDAO().getViewAction(7320, "eliminarEvidencia"));

                var listaCC = _context.tblP_CC.Where(x => x.estatus)
                    .ToList()
                    .Where(x => esAdmin ? true : (ccsUsuario.Contains(x.cc) && ccsAplicables.Contains(x.cc)))
                    .Select(x => new
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion.Trim(),
                        Prefijo = EmpresaEnum.Colombia
                    }).OrderBy(x => x.Text).ToList();

                if (listaCC.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", listaCC);
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
            }

            return resultado;
        }
        public Dictionary<string, object> getSubcontratistas()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var subcontratistas = getListaSubContratistas().Select(x => new
                {
                    Value = x.claveContratista,
                    Text = x.subcontratista
                }).OrderBy(x => x.Text).ToList();

                if (subcontratistas.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", subcontratistas);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo tipos de accidentes");
            }

            return resultado;
        }
        public Dictionary<string, object> getTecnicasInvestigacion()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var tecnicasInvestigacion = _context.tblS_IncidentesTecnicasInvestigacion.ToList().Select(x => new
                {
                    Value = x.id,
                    Text = x.tecnicaInvestigacion
                }).OrderBy(x => x.Value).ToList();

                if (tecnicasInvestigacion.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", tecnicasInvestigacion);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de protocolo de trabajo");
            }

            return resultado;
        }
        public Dictionary<string, object> getEmpleadosCCList(string cc)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<ComboDTO> lstCatEmpleado = new List<ComboDTO>();

                if (cc.Trim().Length > 0)
                {

                    string ccValido = getQueryCCArrendadoraCP(cc);

                    //var odbc = new OdbcConsultaDTO()
                    //{
                    //    consulta = string.Format("SELECT clave_empleado as Value, (nombre+' '+ape_paterno+' '+ape_materno) as Text FROM DBA.sn_empleados WHERE " + ccValido + " AND estatus_empleado = 'A' ORDER BY ape_paterno, ape_materno, nombre"),
                    //    parametros = new List<OdbcParameterDTO>()
                    //};
                    //var listARR = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                    //var listCP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanRh, odbc);

                    var listCP = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        //baseDatos=(MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_empleado as Value, (nombre+' '+ape_paterno+' '+ape_materno) as Text 
                                    FROM DBA.sn_empleados WHERE " + ccValido + " AND estatus_empleado = 'A' ORDER BY ape_paterno, ape_materno, nombre",
                    });

                    var listARR = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado as Value, (nombre+' '+ape_paterno+' '+ape_materno) as Text 
                                    FROM DBA.sn_empleados WHERE " + ccValido + " AND estatus_empleado = 'A' ORDER BY ape_paterno, ape_materno, nombre",
                    });
                    if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                    {
                        var listCol = _context.Select<ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Colombia,
                            consulta = @"SELECT clave_empleado as Value, (nombre+' '+ape_paterno+' '+ape_materno) as Text 
                                    FROM DBA.sn_empleados WHERE " + ccValido + " AND estatus_empleado = 'A' ORDER BY ape_paterno, ape_materno, nombre",
                        });
                        var listPeru = _context.Select<ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.PERU,
                            consulta = @"SELECT clave_empleado as Value, (nombre+' '+ape_paterno+' '+ape_materno) as Text 
                                    FROM DBA.sn_empleados WHERE " + ccValido + " AND estatus_empleado = 'A' ORDER BY ape_paterno, ape_materno, nombre",
                        });

                        lstCatEmpleado.AddRange(listCol);
                        lstCatEmpleado.AddRange(listPeru);
                    }

                    lstCatEmpleado.AddRange(listARR);
                    lstCatEmpleado.AddRange(listCP);

                }

                if (lstCatEmpleado.DistinctBy(x => x.Value).ToList().Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", lstCatEmpleado.DistinctBy(x => x.Value).ToList());
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
            }

            return resultado;
        }
        public Dictionary<string, object> getInfoEmpleado(int claveEmpleado, bool esContratista, int idEmpresaContratista)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                empleadoIncidenteDTO empleadoInfo = InfEmpleado(claveEmpleado, esContratista, idEmpresaContratista);

                if (empleadoInfo != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("empleadoInfo", empleadoInfo);
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
            }

            return resultado;
        }
        public Dictionary<string, object> getInfoEmpleadoContratista(int empleado_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var empleadoInfo = _context.tblS_IncidentesEmpleadosContratistas.Where(x => x.id == empleado_id).Select(x => new empleadoIncidenteDTO
                {
                    nombreEmpleado = x.nombre,
                    edadEmpleado = x.fechaNacimiento,
                    puestoEmpleado = x.puesto
                }).FirstOrDefault();


                if (empleadoInfo != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("empleadoInfo", empleadoInfo);
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
            }

            return resultado;
        }
        public Dictionary<string, object> getUsersEnkontrol(string user)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<empleadoIncidenteDTO> empleadoInfo = listUsuariosEnkontrol(user);

                if (user.Length == 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("empleadoInfo", empleadoInfo.OrderBy(x => x.nombreEmpleado).Take(10).ToList().Select(x => new { id = x.claveEmpleado, label = x.nombreEmpleado }));
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("empleadoInfo", empleadoInfo.OrderBy(x => x.nombreEmpleado).Take(10).ToList().Select(x => new { id = x.claveEmpleado, label = x.nombreEmpleado }));
                }
            }
            catch (Exception e)
            {


                resultado.Add(SUCCESS, false);
            }

            return resultado;

        }
        public Dictionary<string, object> getUsersEnkontrolByClave(string clave)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<empleadoIncidenteDTO> empleadoInfo = listUsuariosEnkontrolByClave(clave);

                if (clave.Length == 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("empleadoInfo", empleadoInfo.OrderBy(x => x.nombreEmpleado).Take(10).ToList().Select(x => new { id = x.claveEmpleado, label = x.nombreEmpleado }));
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("empleadoInfo", empleadoInfo.OrderBy(x => x.nombreEmpleado).Take(10).ToList().Select(x => new { id = x.claveEmpleado, label = x.nombreEmpleado }));
                }
            }
            catch (Exception e)
            {


                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> getPrioridadesActividad()
        {
            var result = new Dictionary<string, object>();
            result.Add("items", GlobalUtils.ParseEnumToCombo<PrioridadActividadEnum>().Reverse());
            result.Add(SUCCESS, true);

            return result;
        }
        public Dictionary<string, object> guardarEmpleadoSubcontratista(tblS_IncidentesEmpleadosContratistas empleado)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.tblS_IncidentesEmpleadosContratistas.Add(empleado);
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(ERROR, "Ocurrió un error al guardar el empleado");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarIncidente(IncidenteDTO captura)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var informe = _context.tblS_IncidentesInformePreliminar.Where(x => x.id == captura.incidente.informe_id).First();
                    var incidenteCapturado = _context.tblS_Incidentes.FirstOrDefault(x => x.informe_id == informe.id);

                    if (incidenteCapturado != null)
                    {
                        throw new Exception("Ya se ha capturado un RIA para este informe preliminar.");
                    }

                    #region Guardar Incidente
                    captura.incidente.idEmpresa = informe.idEmpresa;

                    captura.incidente.GrupoInvestigacion = new List<tblS_IncidentesGrupoInvestigacion>();
                    captura.incidente.GrupoInvestigacion.AddRange(captura.grupoTrabajo);

                    captura.incidente.OrdenCronologico = new List<tblS_IncidentesOrdenCronologico>();
                    captura.incidente.OrdenCronologico.AddRange(captura.ordenCronologico);

                    captura.incidente.EventosDedonador = new List<tblS_IncidentesEventoDetonador>();
                    captura.incidente.EventosDedonador.AddRange(captura.eventoDetonador);

                    captura.incidente.CausasInmediatas = new List<tblS_IncidentesCausasInmediatas>();
                    captura.incidente.CausasInmediatas.AddRange(captura.causasInmediatas);

                    captura.incidente.CausasBasicas = new List<tblS_IncidentesCausasBasicas>();
                    captura.incidente.CausasBasicas.AddRange(captura.causasBasicas);

                    captura.incidente.CausasRaiz = new List<tblS_IncidentesCausasRaiz>();
                    captura.incidente.CausasRaiz.AddRange(captura.causasRaiz);

                    captura.incidente.MedidasControl = new List<tblS_IncidentesMedidasControl>();
                    captura.incidente.MedidasControl.AddRange(captura.medidasControl);

                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru && captura.incidente.idEmpresa == (int)EmpresaEnum.Peru)
                    {
                        captura.incidente.idEmpresa = 0; //Se coloca cero en Perú para que funcione con el resto del módulo. Revisar esto ya que podría cambiar.
                    }

                    _context.tblS_Incidentes.Add(captura.incidente);
                    _context.SaveChanges();


                    informe.estatusAvance = EstatusIncidenteEnum.PendienteCargaRIA;
                    informe.terminado = true;
                    _context.SaveChanges();
                    #endregion

                    // Se genera la minuta
                    List<int> listaUsuariosID;

                    int minutaID = 0;
#if DEBUG
                    minutaID = 0;
#else
                    minutaID = CrearMinutaRIA(captura.incidente, captura.grupoTrabajo, captura.medidasControl, out listaUsuariosID);
#endif

                    if (captura.incidente.informe_id == 0)
                    {
                        throw new Exception("El incidente capturado no tiene informe relacionado.");
                    }
                    else
                    {
                        resultado.Add("informeID", captura.incidente.informe_id); //Variable "informeID" para generar el reporte del incidente y que se envíe el correo.
                    }

                    #region Guardar Evidencias
                    var nombreFolderInforme = ObtenerNombreFolderIncidente(captura.incidente);

                    var rutaFolderInforme = Path.Combine(RutaEvidenciasRIA, nombreFolderInforme);

                    bool carpetaExistente = verificarExisteCarpeta(rutaFolderInforme, true);

                    if (carpetaExistente == false)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al intentar acceder a la carpeta del informe.");
                        return resultado;
                    }

                    var evidenciasExistentes = _context.tblS_IncidentesEvidenciasRIA.Where(x => x.incidente_id == captura.incidente.id).ToList();

                    if (evidenciasExistentes.Where(x => x.estatus).Count() + captura.evidencias.Count > 5)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El número de evidencias a cargar es mayor al límite establecido. Actualmente sólo se pueden cargar hasta 5 evidencias por informe");
                        return resultado;
                    }

                    var consecutivo = (evidenciasExistentes.Count > 0 ? evidenciasExistentes.Max(x => x.numero) : 0) + 1;

                    var usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    var listaArchivosPorCargar = new List<Tuple<HttpPostedFileBase, string>>();

                    int contador = 0;
                    foreach (var evidencia in captura.evidencias)
                    {
                        var extensionArchivo = Path.GetExtension(evidencia.FileName);

                        var nombreArchivo = String.Format("{0} - {1} - Evidencia {2}{3}", captura.incidente.cc, captura.incidente.Informe.folio, consecutivo, extensionArchivo);

                        var nuevaEvidencia = new tblS_IncidentesEvidenciasRIA
                        {
                            incidente_id = captura.incidente.id,
                            nombre = nombreArchivo,
                            numero = consecutivo,
                            ruta = Path.Combine(rutaFolderInforme, nombreArchivo),
                            usuarioCreadorID = usuarioCreadorID,
                            fechaCreacion = DateTime.Now,
                            tipoEvidenciaRIA = (TipoEvidenciaRIAEnum)captura.tipoEvidenciaRIA[contador],
                            estatus = true
                        };

                        _context.tblS_IncidentesEvidenciasRIA.Add(nuevaEvidencia);
                        listaArchivosPorCargar.Add(Tuple.Create(evidencia, nuevaEvidencia.ruta));

                        consecutivo++;
                        contador++;
                    }

                    foreach (var nuevaEvidencia in listaArchivosPorCargar)
                    {
                        if (SaveArchivo(nuevaEvidencia.Item1, nuevaEvidencia.Item2) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo guardar la evidencia en el servidor.");
                            return resultado;
                        }
                    }

                    _context.SaveChanges();
                    #endregion

#if DEBUG
                    listaUsuariosID = new List<int> { 3807 };
#endif

                    resultado.Add(SUCCESS, true);
                    resultado.Add("minutaID", minutaID);
                    resultado.Add("usuarios", listaUsuariosID);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "IndicadoresSeguridadController", "guardarIncidente", e, AccionEnum.AGREGAR, 0, 0);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Ocurrió un error al guardar el empleado");
                }
            }

            return resultado;
        }

        private int CrearMinutaRIA(tblS_Incidentes incidente, List<tblS_IncidentesGrupoInvestigacion> grupoInvestigacion, List<tblS_IncidentesMedidasControl> medidasControl, out List<int> listaUsuariosID)
        {
            var agrupacionSIGOPLAN = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == incidente.idAgrupacion);
            var agrupacionDesc = "";

            if (agrupacionSIGOPLAN != null)
            {
                agrupacionDesc = agrupacionSIGOPLAN.nomAgrupacion;
            }
            //var centroCostos = _context.tblP_CC.First(x => x.cc == incidente.cc);
            //var nombreCC = centroCostos.descripcion.Trim();

            listaUsuariosID = new List<int>();

            var fechaCompromiso = medidasControl.Max(x => x.fechaCump);

            var grupoInvestigacionValido = grupoInvestigacion.Where(x => x.esExterno == false).ToList();

            var minuta = new tblSA_Minuta
            {
                proyecto = agrupacionDesc,
                titulo = ObtenerTituloMinuta(agrupacionDesc, incidente.Informe.folio),
                lugar = incidente.lugarJunta,
                fecha = incidente.fechaJunta.Value,
                fechaInicio = DateTime.Now,
                fechaCompromiso = fechaCompromiso,
                horaInicio = incidente.horaInicio,
                horaFin = incidente.horaFin,
                descripcion = ObtenerDescripcionMinuta(incidente.fechaAccidente, agrupacionDesc, incidente.Informe.folio),
                creadorID = vSesiones.sesionUsuarioDTO.id,
            };

            _context.tblSA_Minuta.Add(minuta);
            _context.SaveChanges();

            // Responsables 
            var participantes = grupoInvestigacionValido.Select(x => new tblSA_Participante
            {
                minutaID = minuta.id,
                participanteID = x.usuarioID,
                participante = x.nombreEmpleado
            }).ToList();
            _context.tblSA_Participante.AddRange(participantes);
            _context.SaveChanges();

            var autorizante = ObtenerUsuarioAutorizanteActividad();
            var nombreAutorizante = GlobalUtils.ObtenerNombreCompletoUsuario(autorizante);

            var interesados = ObtenerInteresadosActividadMinuta();

            listaUsuariosID.AddRange(grupoInvestigacionValido.Select(x => x.usuarioID).ToList());
            listaUsuariosID.AddRange(medidasControl.Select(x => x.usuarioID).ToList());
            listaUsuariosID.AddRange(interesados.Select(x => x.id).ToList());

            listaUsuariosID = listaUsuariosID.Distinct().ToList();

            int numActividad = 1;
            medidasControl.ForEach(actividad =>
            {
                var actividadMinuta = new tblSA_Actividades
                {
                    minutaID = minuta.id,
                    columna = 0,
                    orden = numActividad,
                    tipo = "new",
                    actividad = "Actividad " + numActividad,
                    descripcion = actividad.accionPreventiva.Trim(),
                    //responsableID = actividad.usuarioID,
                    //responsable = actividad.responsableNombre,
                    fechaInicio = DateTime.Now,
                    fechaCompromiso = actividad.fechaCump,
                    prioridad = (int)actividad.prioridad,
                    enVersion = true,
                    revisaID = autorizante.id,
                    revisa = nombreAutorizante
                };

                _context.tblSA_Actividades.Add(actividadMinuta);
                _context.SaveChanges();

                var responsable = new tblSA_Responsables
                {
                    minutaID = minuta.id,
                    actividadID = actividadMinuta.id,
                    usuarioID = actividad.usuarioID,
                    usuarioText = actividad.responsableNombre
                };
                _context.tblSA_Responsables.Add(responsable);
                _context.SaveChanges();

                GuardarInteresadosActividadMinuta(minuta.id, actividadMinuta.id, interesados);

                numActividad++;
            });

            _context.SaveChanges();
            return minuta.id;
        }

        private void GuardarInteresadosActividadMinuta(int minutaID, int actividadID, List<tblP_Usuario> interesados)
        {
            var interesadosActividad = interesados.Select(x => new tblSA_Interesados
            {
                minutaID = minutaID,
                actividadID = actividadID,
                interesadoID = x.id,
                interesado = GlobalUtils.ObtenerNombreCompletoUsuario(x)
            }).ToList();

            _context.tblSA_Interesados.AddRange(interesadosActividad);
            _context.SaveChanges();
        }

        private List<tblP_Usuario> ObtenerInteresadosActividadMinuta()
        {
            var usuariosInteresadosID = new List<int> { 3377, 3372, 1105, 3390, 3357 };

            return _context.tblP_Usuario.Where(x => x.estatus).ToList().Where(x => usuariosInteresadosID.Contains(x.id)).ToList();
        }

        private tblP_Usuario ObtenerUsuarioAutorizanteActividad()
        {
            int JoseIribeUsuarioID = 3372;
            return _context.tblP_Usuario.First(x => x.id == JoseIribeUsuarioID);
        }

        public object GetUsuariosAutocomplete(string term)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var usuarios = _context.tblP_Usuario
                .Where(x =>
                    x.estatus &&
                    x.cveEmpleado != null &&
                    (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).Contains(term))
                    .OrderBy(x => x.id).Take(12).ToList();

                return
                    usuarios.Select(x => new
                    {
                        id = x.cveEmpleado,
                        value = GlobalUtils.ObtenerNombreCompletoUsuario(x),
                        usuarioID = x.id
                    });
            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "GetUsuariosAutocomplete", e, AccionEnum.CONSULTA, 0, 0);
                return new List<object> { new { id = 0, value = "", usuarioID = 0 } };
            }
        }

        private string ObtenerDescripcionMinuta(DateTime fechaAccidente, string cc, int folio)
        {
            return String.Format("Establecer medidas preventivas o correctivas al accidente ocurrido el {0} en el proyecto {1} con folio {2}", fechaAccidente.ToShortDateString(), cc, folio);
        }

        private string ObtenerTituloMinuta(string agrupacion, int folio)
        {
            return String.Format("Seguimiento RIA - {0} Folio: {1}", agrupacion, folio);
        }

        public Dictionary<string, object> ObtenerIncidentePorInformeID(int informeID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var incidente = _context.tblS_Incidentes.FirstOrDefault(x => x.informe_id == informeID);

                var incidenteData = new FormatoRIADTO
                {
                    // Datos generales
                    tipoAccidente_id = incidente.tipoAccidente_id,
                    subclasificacionID = incidente.subclasificacionID,
                    tipoAccidente = incidente.TiposAccidente.incidenteTipo,
                    esExterno = incidente.esExterno,
                    empresa = incidente.esExterno ? ObtenerSubcontratista(incidente.claveContratista) : "CONSTRUPLAN",
                    cc = incidente.cc,
                    claveContratista = incidente.claveContratista,
                    departamento_id = incidente.departamento_id,
                    departamento = incidente.Departamentos.departamento,
                    lugarAccidente = incidente.lugarAccidente,
                    fechaAccidente = incidente.fechaAccidente.ToShortDateString(),
                    horaAccidente = incidente.fechaAccidente.ToLongTimeString(),
                    diaSemana = ObtenerDiaSemanaFecha(incidente.fechaAccidente),
                    tipoLesion_id = incidente.tipoLesion_id,
                    tipoLesion = incidente.TiposLesion.tipoLesion,
                    parteCuerpo_id = incidente.parteCuerpo_id,
                    actividadRutinaria = incidente.actividadRutinaria,
                    agenteImplicado_id = incidente.agenteImplicado_id,
                    agenteImplicado = incidente.AgentesImplicados.agenteImplicado,
                    trabajoPlaneado = incidente.trabajoPlaneado,
                    trabajoRealizaba = incidente.trabajoRealizaba,
                    tipoContacto_id = incidente.tipoContacto_id,
                    tipoContacto = incidente.TiposContacto.tipoContacto,
                    protocoloTrabajo_id = incidente.protocoloTrabajo_id,
                    protocoloTrabajo = incidente.ProtocolosTrabajo.protocoloTrabajo,
                    // Persona implicada accidente
                    claveEmpleado = incidente.claveEmpleado,
                    edadEmpleado = incidente.edadEmpleado,
                    puestoEmpleado = incidente.puestoEmpleado,
                    experienciaEmpleado_id = incidente.experienciaEmpleado_id,
                    experienciaEmpleado = incidente.ExperienciaEmpleado.empleadoExperiencia,
                    antiguedadEmpleado_id = incidente.antiguedadEmpleado_id,
                    antiguedadEmpleado = incidente.AntiguedadEmpleado.antiguedadEmpleado,
                    turnoEmpleado_id = incidente.turnoEmpleado_id,
                    turnoEmpleado = incidente.TurnoEmpleado.turno,
                    horasTrabajadasEmpleado = incidente.horasTrabajadasEmpleado,
                    diasTrabajadosEmpleado = incidente.diasTrabajadosEmpleado,
                    capacitadoEmpleado = incidente.capacitadoEmpleado,
                    accidentesAnterioresEmpleado = incidente.accidentesAnterioresEmpleado,
                    claveSupervisor = incidente.claveSupervisor.GetValueOrDefault(),
                    supervisorCargoEmpleado = incidente.supervisorCargoEmpleado,
                    nombreEmpleadoExterno = incidente.nombreEmpleadoExterno,
                    nombreEmpleado = incidente.esExterno ? incidente.nombreEmpleadoExterno : ObtenerNombreEmpleadoPorClave(incidente.claveEmpleado),
                    // Descripción del accidente
                    descripcionAccidente = incidente.descripcionAccidente,
                    // Evaluación de la pérdida si no es corregida
                    riesgo = incidente.riesgo,
                    descripcionRiesgo = ObtenerDescripcionRiesgo(incidente.riesgo),
                    // Grupo de trabajo para la investigación
                    grupoInvestigacion = incidente.GrupoInvestigacion
                    .Select(x => new GrupoInvestigacionDTO
                        {
                            nombreEmpleado = x.nombreEmpleado,
                            puestoEmpleado = x.puestoEmpleado,
                            departamentoEmpleado = x.departamentoEmpleado,
                            informeID = informeID.ToString(),
                            esExterno = x.esExterno
                        })
                        .ToList(),
                    // Orden cronológico del accidente
                    instruccionTrabajo = incidente.instruccionTrabajo,
                    porqueSehizo = incidente.porqueSehizo,
                    ordenCronologico = incidente.OrdenCronologico.Select(x => x.ordenCronologico).ToList(),
                    // Técnica de Investigación
                    tecnicaInvestigacion_id = incidente.tecnicaInvestigacion_id,
                    tecnicaInvestigacion = incidente.TecnicasInvestigacion.tecnicaInvestigacion,
                    // Análisis de causas
                    eventoDetonador = incidente.EventosDedonador.Select(x => x.eventoDetonador).ToList(),
                    causaInmediata = incidente.CausasInmediatas.Select(x => x.causaInmediata).ToList(),
                    causaBasica = incidente.CausasBasicas.Select(x => x.causaBasica).ToList(),
                    causaRaiz = incidente.CausasRaiz.Select(x => x.causaRaiz).ToList(),
                    // Medidas de control
                    lugarJunta = incidente.lugarJunta,
                    fechaJunta = incidente.fechaJunta.HasValue ? incidente.fechaJunta.Value.ToShortDateString() : "",
                    horaInicio = incidente.horaInicio,
                    horaFin = incidente.horaFin,
                    medidasControl = incidente.MedidasControl
                    .Where(x => x.responsable_id != 0)
                    .Select(x => new MedidaControlDTO
                    {
                        accionPreventiva = x.accionPreventiva,
                        fechaCump = x.fechaCump.ToShortDateString(),
                        responsableNombre = x.responsableNombre,
                        estatus = ((estatusMedidasControlEnum)x.estatus).GetDescription(),
                        prioridad = x.prioridad.GetDescription(),
                        informeID = informeID.ToString()
                    }).ToList()
                };

                resultado.Add(SUCCESS, true);
                resultado.Add("informacion", incidenteData);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la información sobre el incidente");
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerInformeParaReporte(int informeID)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var informe = _context.tblS_IncidentesInformePreliminar.FirstOrDefault(x => x.id == informeID);
                var edadEmpleado = 0;

                if (!informe.esExterno)
                {
                    if (InfEmpleado(informe.claveEmpleado, false, 0) == null)
                    {
                        edadEmpleado = 0;
                    }
                    else
                    {
                        edadEmpleado = DateTime.Now.Year - InfEmpleado(informe.claveEmpleado, false, 0).edadEmpleado.Year; //TODO

                        // Go back to the year the person was born in case of a leap year
                        if (InfEmpleado(informe.claveEmpleado, false, 0).edadEmpleado.Date > DateTime.Now.AddYears(-edadEmpleado))
                        {
                            edadEmpleado--;
                        }
                    }
                }
                var incidenteData = new FormatoRIADTO
                {
                    // Datos generales
                    tipoAccidente_id = informe.tipoAccidente_id,
                    subclasificacionID = informe.subclasificacionID,
                    tipoAccidente = informe.TiposAccidente.incidenteTipo,
                    esExterno = informe.esExterno,
                    empresa = informe.esExterno ? ObtenerSubcontratista(informe.claveContratista) : "CONSTRUPLAN",
                    cc = informe.cc,
                    claveContratista = informe.claveContratista,
                    departamento_id = informe.departamento_id,
                    departamento = _context.tblS_IncidentesDepartamentos.Where(x => x.id == informe.departamento_id).Select(x => x.departamento).FirstOrDefault(),
                    lugarAccidente = informe.lugarAccidente,
                    fechaAccidente = informe.fechaIncidente.ToShortDateString(),
                    horaAccidente = informe.fechaIncidente.ToLongTimeString(),
                    diaSemana = ObtenerDiaSemanaFecha(informe.fechaIncidente),
                    tipoLesion_id = informe.tipoLesion_id,
                    tipoLesion = informe.tipoLesion_id > 0 ? informe.TiposLesion.tipoLesion : "",
                    parteCuerpo_id = informe.parteCuerpo_id,
                    actividadRutinaria = informe.actividadRutinaria,
                    agenteImplicado_id = informe.agenteImplicado_id,
                    agenteImplicado = _context.tblS_IncidentesAgentesImplicados.Where(x => x.id == informe.agenteImplicado_id).Select(x => x.agenteImplicado).FirstOrDefault(),
                    trabajoPlaneado = informe.trabajoPlaneado,
                    trabajoRealizaba = informe.trabajoRealizaba,
                    tipoContacto_id = informe.tipoContacto_id,
                    tipoContacto = _context.tblS_IncidentesTipoContacto.Where(x => x.id == informe.tipoContacto_id).Select(x => x.tipoContacto).FirstOrDefault(),
                    protocoloTrabajo_id = informe.protocoloTrabajo_id,
                    protocoloTrabajo = informe.protocoloTrabajo_id > 0 ? informe.ProtocolosTrabajo.protocoloTrabajo : "",
                    // Persona implicada accidente
                    claveEmpleado = informe.claveEmpleado,
                    edadEmpleado = edadEmpleado,
                    puestoEmpleado = informe.puestoEmpleado,
                    experienciaEmpleado_id = informe.experienciaEmpleado_id,
                    experienciaEmpleado = informe.experienciaEmpleado_id > 0 ? informe.ExperienciaEmpleado.empleadoExperiencia : "",
                    antiguedadEmpleado_id = informe.antiguedadEmpleado_id,
                    antiguedadEmpleado = informe.antiguedadEmpleado_id > 0 ? informe.AntiguedadEmpleado.antiguedadEmpleado : "",
                    turnoEmpleado_id = informe.turnoEmpleado_id,
                    turnoEmpleado = informe.turnoEmpleado_id > 0 ? informe.TurnoEmpleado.turno : "",
                    horasTrabajadasEmpleado = informe.horasTrabajadasEmpleado,
                    diasTrabajadosEmpleado = informe.diasTrabajadosEmpleado,
                    capacitadoEmpleado = informe.capacitadoEmpleado,
                    accidentesAnterioresEmpleado = informe.accidentesAnterioresEmpleado,
                    claveSupervisor = informe.claveSupervisor ?? 0,
                    //supervisorCargoEmpleado = (informe.claveSupervisor != null && informe.claveSupervisor > 0) ? InfEmpleado((int)informe.claveSupervisor, false, 0).nombreEmpleado : "", //TODO
                    supervisorCargoEmpleado = (InfEmpleado((int)informe.claveSupervisor, false, 0) == null) ? "" : InfEmpleado((int)informe.claveSupervisor, false, 0).nombreEmpleado, //TODO
                    nombreEmpleadoExterno = informe.nombreExterno,
                    nombreEmpleado = informe.esExterno ? informe.nombreExterno : ObtenerNombreEmpleadoPorClave(informe.claveEmpleado),
                    // Descripción del accidente
                    descripcionAccidente = informe.descripcionAccidente,
                    // Evaluación de la pérdida si no es corregida
                    riesgo = informe.riesgo != null ? (int)informe.riesgo : 0,
                    descripcionRiesgo = informe.riesgo != null ? ObtenerDescripcionRiesgo((int)informe.riesgo) : "",
                    accionInmediata = informe.accionInmediata
                };

                resultado.Add(SUCCESS, true);
                resultado.Add("informacion", incidenteData);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener la información sobre el incidente");
            }

            return resultado;
        }

        private string ObtenerDescripcionRiesgo(int riesgo)
        {
            switch (riesgo)
            {
                case 1:
                    return "Leve [1]";
                case 2:
                    return "Tolerable [2]";
                case 3:
                    return "Moderado [3]";
                case 6:
                    return "Crítico [6]";
                case 9:
                    return "Intolerable [9]";
                default:
                    return "Indefinido";
            }
        }

        private string ObtenerDiaSemanaFecha(DateTime fecha)
        {
            var culture = new CultureInfo("es-MX");
            return culture.DateTimeFormat.GetDayName(fecha.DayOfWeek).ToUpper();
        }

        private string getQueryCCArrendadoraCP(string cc)
        {
            string[] ccTMC = new string[] { cc, "100", "101", "002", "114", "004" };
            string joinedTMC = String.Join(", ", ccTMC.Select(item => "'" + item + "'"));

            string[] ccCerroPelon = new string[] { cc, "121", "123" };
            string joinedCerroPelon = String.Join(", ", ccCerroPelon.Select(item => "'" + item + "'"));

            string[] ccColorada = new string[] { cc, "103" };
            string joinedColorada = String.Join(", ", ccColorada.Select(item => "'" + item + "'"));

            string[] ccYaqui = new string[] { cc, "105" };
            string joinedYaqui = String.Join(", ", ccYaqui.Select(item => "'" + item + "'"));

            string[] ccJales = new string[] { cc, "126" };
            string joinedJales = String.Join(", ", ccJales.Select(item => "'" + item + "'"));

            string[] ccAcueducto = new string[] { cc, "125" };
            string joinedAcueducto = String.Join(", ", ccAcueducto.Select(item => "'" + item + "'"));

            string queryCC = "";

            switch (cc)
            {
                case "1010":
                    queryCC = "cc_contable IN (" + joinedTMC + ")";
                    break;
                case "165":
                    queryCC = "cc_contable IN (" + joinedCerroPelon + ")";
                    break;
                case "146":
                    queryCC = "cc_contable IN (" + joinedColorada + ")";
                    break;
                case "155":
                    queryCC = "cc_contable IN (" + joinedYaqui + ")";
                    break;
                case "169":
                    queryCC = "cc_contable IN (" + joinedJales + ")";
                    break;
                case "227":
                    queryCC = "cc_contable IN (" + joinedAcueducto + ")";
                    break;
                default:
                    queryCC = "cc_contable IN ('" + cc + "')";
                    break;

            }

            return queryCC;
        }
        private empleadoIncidenteDTO InfEmpleado(decimal claveEmpleado, bool esContratista, int idEmpresaContratista)
        {

            try
            {
                if (!esContratista)
                {
                    #region SE OBTIENE INFORMACIÓN DE PERSONAL INTERNO
                    empleadoIncidenteDTO empleado = new empleadoIncidenteDTO();
                    //string inf_Empleado = "SELECT";
                    //inf_Empleado += " e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado, e.fecha_nac AS edadEmpleado, e.fecha_antiguedad AS antiguedadEmpleado, e.fecha_antiguedad AS antiguedadEmpleadoStr, p.descripcion AS puestoEmpleado,";
                    //inf_Empleado += " e.cc_contable as ccID,";
                    //inf_Empleado += " c.descripcion as cc,";
                    //inf_Empleado += " dp.desc_depto as deptoEmpleado,";
                    //inf_Empleado += " (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from DBA.sn_empleados as e2 where e2.clave_empleado = e.jefe_inmediato) AS 'supervisorEmpleado'";
                    //inf_Empleado += " FROM DBA.sn_empleados as e";
                    //inf_Empleado += " inner join DBA.si_puestos as p on e.puesto=p.puesto";
                    //inf_Empleado += " inner join DBA.sn_tipos_nomina as tn on e.tipo_nomina=tn.tipo_nomina";
                    //inf_Empleado += " inner join DBA.cc as c on e.cc_contable=c.cc";
                    //inf_Empleado += " inner join DBA.sn_departamentos as dp on e.clave_depto = dp.clave_depto";
                    //inf_Empleado += " where e.clave_empleado= ? ";
                    //inf_Empleado += " order by e.ape_paterno DESC";

                    //var odbc = new OdbcConsultaDTO()
                    //{
                    //    consulta = inf_Empleado,
                    //    parametros = new List<OdbcParameterDTO>()
                    //};
                    //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Decimal, valor = claveEmpleado });
                    //var resultadoARR = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.ArrenRh, odbc);
                    //var resultadoCP = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.CplanRh, odbc);

                    var resultadoCP = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,

                        consulta = @"SELECT
                                        e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado, e.fecha_nac AS edadEmpleado, e.fecha_antiguedad AS antiguedadEmpleado, e.fecha_antiguedad AS antiguedadEmpleadoStr, p.descripcion AS puestoEmpleado,
                                        e.cc_contable as ccID,
                                        c.descripcion as cc,
                                        dp.desc_depto as deptoEmpleado,
                                        (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato) AS 'supervisorEmpleado'
                                    FROM tblRH_EK_Empleados as e
                                        inner join tblRH_EK_Puestos as p on e.puesto=p.puesto
                                        inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina
                                        inner join tblP_CC as c on e.cc_contable=c.cc
                                        inner join tblRH_EK_Departamentos as dp on e.clave_depto = dp.clave_depto
                                        where e.clave_empleado= @claveEmpleado
                                        order by e.ape_paterno DESC",
                        parametros = new { claveEmpleado }
                    });

                    var resultadoARR = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT
                                        e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado, e.fecha_nac AS edadEmpleado, e.fecha_antiguedad AS antiguedadEmpleado, e.fecha_antiguedad AS antiguedadEmpleadoStr, p.descripcion AS puestoEmpleado,
                                        e.cc_contable as ccID,
                                        c.descripcion as cc,
                                        dp.desc_depto as deptoEmpleado,
                                        (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato) AS 'supervisorEmpleado'
                                    FROM tblRH_EK_Empleados as e
                                        inner join tblRH_EK_Puestos as p on e.puesto=p.puesto
                                        inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina
                                        inner join tblP_CC as c on e.cc_contable=c.cc
                                        inner join tblRH_EK_Departamentos as dp on e.clave_depto = dp.clave_depto
                                        where e.clave_empleado= @claveEmpleado
                                        order by e.ape_paterno DESC",
                        parametros = new { claveEmpleado }
                    });
                    if (resultadoARR.Count() > 0)
                        resultadoARR.FirstOrDefault().antiguedadEmpleadoStr = resultadoARR.FirstOrDefault().antiguedadEmpleado.ToShortDateString();

                    if (resultadoCP.Count() > 0)
                        resultadoCP.FirstOrDefault().antiguedadEmpleadoStr = resultadoCP.FirstOrDefault().antiguedadEmpleado.ToShortDateString();

                    empleado = resultadoARR.Count() > 0 ? resultadoARR.FirstOrDefault() : resultadoCP.FirstOrDefault();

                    if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                    {

                        var resultadoCol = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Colombia,
                            consulta = @"SELECT
                                        e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado, e.fecha_nac AS edadEmpleado, e.fecha_antiguedad AS antiguedadEmpleado, e.fecha_antiguedad AS antiguedadEmpleadoStr, p.descripcion AS puestoEmpleado,
                                        e.cc_contable as ccID,
                                        c.descripcion as cc,
                                        dp.desc_depto as deptoEmpleado,
                                        (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato) AS 'supervisorEmpleado'
                                    FROM tblRH_EK_Empleados as e
                                        inner join tblRH_EK_Puestos as p on e.puesto=p.puesto
                                        inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina
                                        inner join tblP_CC as c on e.cc_contable=c.cc
                                        inner join tblRH_EK_Departamentos as dp on e.clave_depto = dp.clave_depto
                                        where e.clave_empleado= @claveEmpleado
                                        order by e.ape_paterno DESC",
                            parametros = new { claveEmpleado }
                        });

                        var resultadoPeru = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.PERU,
                            consulta = @"SELECT
                                        e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado, e.fecha_nac AS edadEmpleado, e.fecha_antiguedad AS antiguedadEmpleado, e.fecha_antiguedad AS antiguedadEmpleadoStr, p.descripcion AS puestoEmpleado,
                                        e.cc_contable as ccID,
                                        c.descripcion as cc,
                                        dp.desc_depto as deptoEmpleado,
                                        (Select (e2.nombre+' '+e2.ape_paterno+' '+e2.ape_materno) from tblRH_EK_Empleados as e2 where e2.clave_empleado = e.jefe_inmediato) AS 'supervisorEmpleado'
                                    FROM tblRH_EK_Empleados as e
                                        inner join tblRH_EK_Puestos as p on e.puesto=p.puesto
                                        inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina=tn.tipo_nomina
                                        inner join tblP_CC as c on e.cc_contable=c.cc
                                        inner join tblRH_EK_Departamentos as dp on e.clave_depto = dp.clave_depto
                                        where e.clave_empleado= @claveEmpleado
                                        order by e.ape_paterno DESC",
                            parametros = new { claveEmpleado }
                        });



                        if (resultadoPeru.Count() > 0)
                            resultadoPeru.FirstOrDefault().antiguedadEmpleadoStr = resultadoPeru.FirstOrDefault().antiguedadEmpleado.ToShortDateString();

                        if (resultadoPeru.Count > 0 || resultadoCP.Count() > 0 || resultadoARR.Count() > 0 || resultadoCol.Count() > 0)
                        {
                            empleado = resultadoARR.Count() > 0 ? resultadoARR.FirstOrDefault() : resultadoPeru.Count() > 0 ? resultadoPeru.FirstOrDefault() : resultadoCol.Count() > 0 ? resultadoCol.FirstOrDefault() : resultadoCP.FirstOrDefault();
                        }
                        else
                        {
                            throw new Exception("El Empleado no se encuentra , favor de revisar la clave ");
                        }
                    }

                    return empleado;
                    #endregion
                }
                else
                {
                    #region SE OBTIENE INFORMACIÓN DE EMPLEADOS EN CASO DE SER CONTRATISTA EL QUE INICIO SESIÓN

                    List<tblS_IncidentesEmpleadoContratistas> lstEmpleadosContratistas = new List<tblS_IncidentesEmpleadoContratistas>();
                    if ((int)vSesiones.sesionUsuarioDTO.idPerfil == 4 && idEmpresaContratista > 0)
                        lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.id == claveEmpleado && x.esActivo && x.idEmpresaContratista == idEmpresaContratista).ToList();
                    else if ((int)vSesiones.sesionUsuarioDTO.idPerfil != 4)
                        lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas.Where(x => x.id == claveEmpleado && x.esActivo).ToList();

                    if (lstEmpleadosContratistas.Count() > 0)
                    {
                        empleadoIncidenteDTO objEmpleadoContratista = new empleadoIncidenteDTO();
                        objEmpleadoContratista.claveEmpleado = lstEmpleadosContratistas[0].id;
                        objEmpleadoContratista.nombreEmpleado = lstEmpleadosContratistas[0].nombre + " " + lstEmpleadosContratistas[0].apePaterno + " " + lstEmpleadosContratistas[0].apeMaterno;
                        objEmpleadoContratista.puestoEmpleado = lstEmpleadosContratistas[0].puesto;
                        return objEmpleadoContratista;
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        private List<empleadoIncidenteDTO> InfEmpleados(List<decimal> clavesEmpleados)
        {
            try
            {
                List<empleadoIncidenteDTO> empleados = new List<empleadoIncidenteDTO>();
                string clavesEmpleado = getStringInlineArray(clavesEmpleados);
                //                string inf_Empleado = "SELECT e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado FROM DBA.sn_empleados as e where e.clave_empleado= ? AND e.estatus_empleado ='A' order by e.ape_paterno DESC";
                //                inf_Empleado = string.Format(
                //                    @"SELECT e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado 
                //                        FROM DBA.sn_empleados as e 
                //                        WHERE e.clave_empleado in {0} AND e.estatus_empleado ='A' 
                //                        ORDER BY e.ape_paterno DESC"
                //                    , clavesEmpleados.ToParamInValue());
                //                var odbc = new OdbcConsultaDTO()
                //                {
                //                    consulta = inf_Empleado,
                //                    parametros = new List<OdbcParameterDTO>()
                //                };
                //                foreach (var item in clavesEmpleados) { odbc.parametros.Add(new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Decimal, valor = item }); }
                //                var resultadoARR = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.ArrenRh, odbc);
                //                var resultadoCP = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.CplanRh, odbc)

                var resultadoCP = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado 
                                FROM tblRH_EK_Empleados as e 
                                WHERE e.clave_empleado in @clavesEmpleado AND e.estatus_empleado ='A' 
                                ORDER BY e.ape_paterno DESC",
                    parametros = new { clavesEmpleado }
                });

                var resultadoARR = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado 
                                FROM tblRH_EK_Empleados as e 
                                WHERE e.clave_empleado in @clavesEmpleado AND e.estatus_empleado ='A' 
                                ORDER BY e.ape_paterno DESC",
                    parametros = new { clavesEmpleado }
                });
                if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    var resultadoCol = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = @"SELECT e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado 
                                FROM tblRH_EK_Empleados as e 
                                WHERE e.clave_empleado in @clavesEmpleado AND e.estatus_empleado ='A' 
                                ORDER BY e.ape_paterno DESC",
                        parametros = new { clavesEmpleado }
                    });
                    var resultadoPeru = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = @"SELECT e.clave_empleado AS claveEmpleado, (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado 
                                FROM tblRH_EK_Empleados as e 
                                WHERE e.clave_empleado in @clavesEmpleado AND e.estatus_empleado ='A' 
                                ORDER BY e.ape_paterno DESC",
                        parametros = new { clavesEmpleado }
                    });
                    resultadoARR.AddRange(resultadoCol);
                    resultadoARR.AddRange(resultadoPeru);
                }

                resultadoARR.AddRange(resultadoCP);
                empleados = resultadoARR.ToList();
                return empleados;
            }
            catch (Exception)
            {
                return null;
            }
        }

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

        public string getStringInlineArray(List<decimal> lista)
        {
            string result = @"";
            foreach (var i in lista)
            {
                result += "'" + i.ToString() + "',";
            }
            result = result.TrimEnd(',');
            return result;
        }

        public string getStringInlineArray(List<int> lista)
        {
            string result = @"";
            foreach (var i in lista)
            {
                result += "'" + i.ToString() + "',";
            }
            result = result.TrimEnd(',');
            return result;
        }
        private List<empleadoIncidenteDTO> listUsuariosEnkontrol(string user)
        {

            try
            {
                List<empleadoIncidenteDTO> lstCatEmpleado = new List<empleadoIncidenteDTO>();

                //string inf_Empleado = "SELECT";
                //inf_Empleado += " clave_empleado AS claveEmpleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombreEmpleado ";
                //inf_Empleado += " FROM DBA.sn_empleados";
                //inf_Empleado += " WHERE estatus_empleado ='A' ";
                //inf_Empleado += " GROUP BY clave_empleado, nombre, ape_paterno, ape_materno";
                //inf_Empleado += " HAVING (nombre + ' ' + ape_paterno + ' ' + ape_materno) like ? ";

                //var odbc = new OdbcConsultaDTO()
                //{
                //    consulta = inf_Empleado,
                //    parametros = new List<OdbcParameterDTO>()
                //};

                //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "user", tipo = OdbcType.VarChar, valor = string.Format("%{0}%", user) });
                //var listARR = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.ArrenRh, odbc);
                //var listCP = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.CplanRh, odbc);

                var listCP = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT
                                    clave_empleado AS claveEmpleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombreEmpleado 
                                 FROM tblRH_EK_Empleados
                                 WHERE estatus_empleado ='A' 
                                 GROUP BY clave_empleado, nombre, ape_paterno, ape_materno
                                 HAVING (nombre + ' ' + ape_paterno + ' ' + ape_materno) like '%@user%'",
                    parametros = new { user }
                });

                var listARR = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT
                                    clave_empleado AS claveEmpleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombreEmpleado 
                                 FROM tblRH_EK_Empleados
                                 WHERE estatus_empleado ='A' 
                                 GROUP BY clave_empleado, nombre, ape_paterno, ape_materno
                                 HAVING (nombre + ' ' + ape_paterno + ' ' + ape_materno) like '%@user%'",
                    parametros = new { user }
                });

                foreach (var item in listARR)
                {
                    lstCatEmpleado.Add(item);
                }

                foreach (var item in listCP)
                {
                    lstCatEmpleado.Add(item);
                }

                return lstCatEmpleado.DistinctBy(x => x.claveEmpleado).ToList();
            }
            catch (Exception)
            {


            }

            return null;
        }

        private List<empleadoIncidenteDTO> listUsuariosEnkontrolByClave(string clave)
        {

            try
            {
                List<empleadoIncidenteDTO> lstCatEmpleado = new List<empleadoIncidenteDTO>();

                //string inf_Empleado = "SELECT";
                //inf_Empleado += " clave_empleado AS claveEmpleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombreEmpleado ";
                //inf_Empleado += " FROM DBA.sn_empleados";
                //inf_Empleado += " WHERE clave_empleado = ? AND estatus_empleado ='A' ";

                //var odbc = new OdbcConsultaDTO()
                //{
                //    consulta = inf_Empleado,
                //    parametros = new List<OdbcParameterDTO>()
                //};

                //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.VarChar, valor = clave });
                //var listARR = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.ArrenRh, odbc);
                //var listCP = _contextEnkontrol.Select<empleadoIncidenteDTO>(EnkontrolEnum.CplanRh, odbc);

                var listCP = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"
                                SELECT
                                 clave_empleado AS claveEmpleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombreEmpleado 
                                 FROM tblRH_EK_Empleados
                                 WHERE clave_empleado = @clave AND estatus_empleado = 'A' ",
                    parametros = new { clave }
                });

                var listARR = _context.Select<empleadoIncidenteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"
                                SELECT
                                 clave_empleado AS claveEmpleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombreEmpleado 
                                 FROM tblRH_EK_Empleados
                                 WHERE clave_empleado = @clave AND estatus_empleado = 'A' ",
                    parametros = new { clave }
                });

                foreach (var item in listARR)
                {
                    lstCatEmpleado.Add(item);
                }

                foreach (var item in listCP)
                {
                    lstCatEmpleado.Add(item);
                }

                return lstCatEmpleado.DistinctBy(x => x.claveEmpleado).ToList();
            }
            catch (Exception)
            {


            }

            return null;
        }
        private List<subcontratistaIncidenteDTO> getListaSubContratistas()
        {
            if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
            {
                List<subcontratistaIncidenteDTO> res = new List<subcontratistaIncidenteDTO>();
                string Consulta = @"SELECT 
                                                    A.numPro AS claveContratista, 
                                                    A.DSContratista AS subcontratista 
                                                FROM su_contratistas A 
                                                ORDER BY A.DSContratista";
                try
                {
                    var datosEnKontrol = (List<subcontratistaIncidenteDTO>)_contextEnkontrol.Where(Consulta).ToObject<List<subcontratistaIncidenteDTO>>();
                    return datosEnKontrol;
                }
                catch
                {
                    return new List<subcontratistaIncidenteDTO>();
                }
            }
            //            else if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
            //            {
            //                List<subcontratistaIncidenteDTO> res = new List<subcontratistaIncidenteDTO>();
            //                string Consulta = @"SELECT 
            //                                                    A.numPro AS claveContratista, 
            //                                                    A.DSContratista AS subcontratista 
            //                                                FROM DBA.su_contratistas A 
            //                                                ORDER BY A.DSContratista";
            //                try
            //                {
            //                    var datosEnKontrol = (List<subcontratistaIncidenteDTO>)_contextEnkontrol.Where(Consulta).ToObject<List<subcontratistaIncidenteDTO>>();
            //                    return datosEnKontrol;
            //                }
            //                catch
            //                {
            //                    return new List<subcontratistaIncidenteDTO>();
            //                }
            //            }
            else if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
            {
                var datosEnKontrol = _context.tblS_IncidentesEmpresasContratistas.ToList().Select(x => new subcontratistaIncidenteDTO
                    {
                        claveContratista = x.id,
                        subcontratista = x.nombreEmpresa,
                    }).ToList();
                return datosEnKontrol;

            }
            return new List<subcontratistaIncidenteDTO>();

        }
        private string ObtenerSubcontratista(int claveContratista)
        {
            if (claveContratista == 0)
            {
                return "NO DEFINIDO";
            }

            var res = new subcontratistaIncidenteDTO();
            string Consulta = @"SELECT 
                                    A.numPro AS claveContratista, 
                                    A.DSContratista AS subcontratista 
                                FROM su_contratistas A WHERE claveContratista = " + claveContratista;
            try
            {
                var datosEnKontrol = (List<subcontratistaIncidenteDTO>)_contextEnkontrol.Where(Consulta).ToObject<List<subcontratistaIncidenteDTO>>();
                return datosEnKontrol.First().subcontratista;
            }
            catch
            {
                return "INDEFINIDO";
            }
        }
        #endregion

        #region CAPTURA INFORMACION COLABORADORES
        public Dictionary<string, object> getInformacionColaboradores(int idAgrupacion, DateTime fechaInicio, DateTime fechaFin, int idEmpresa)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                bool esContratista = (int)vSesiones.sesionUsuarioDTO.idPerfil == 4 ? true : false;

                var ccs = _context.tblP_CC.ToList();

                //var grups = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == idAgrupacion);
                //var lstEmpresasContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.id == idAgrupacion && x.esActivo).FirstOrDefault();
                var lstEmpresasContratistas = _context.tblS_IncidentesEmpresasContratistas.ToList();
                //var lstEmpresasContratistasAgrupacion = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.id == idAgrupacion && x.esActivo).FirstOrDefault();
                var lstEmpresasContratistasAgrupacion = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
                var informacionColaboradores = _context.tblS_IncidentesInformacionColaboradores
                    .Where(x =>
                        (idEmpresa == -1 ? true : x.idEmpresa == idEmpresa) &&
                        (idEmpresa == -1 ? true : x.idAgrupacion == idAgrupacion))
                    .Select(x => new informacionColaboradorDTO
                    {
                        id = x.id,
                        horasHombre = x.horasHombre,
                        lostDay = x.lostDay,
                        //idAgrupacion = idAgrupacion,
                        idAgrupacion = (int)x.idAgrupacion,
                        //idEmpresa = 0,
                        idEmpresa = x.idEmpresa,
                        //cc = idEmpresa == 0 ? grups.nomAgrupacion : idEmpresa == 1000 ? lstEmpresasContratistas.nombreEmpresa : lstEmpresasContratistasAgrupacion.nomAgrupacion,
                        cc = x.cc,
                        fechaInicio = x.fechaInicio,
                        //fechaInicioStr = x.fechaInicio.ToString("yyyy/MM/dd"),
                        fechaFin = x.fechaFin,
                        //fechaFinStr = x.fechaFin.ToString("yyyy/MM/dd"),
                        agrupacion = x.agrupacion
                    }).OrderByDescending(x => x.fechaFin).ToList();

                foreach (var item in informacionColaboradores)
                {
                    item.fechaInicioStr = item.fechaInicio.ToString("yyyy/MM/dd");
                    item.fechaFinStr = item.fechaFin.ToString("yyyy/MM/dd");

                    if (item.idAgrupacion == 0)
                    {
                        var ccCoincidencia = ccs.FirstOrDefault(x => x.cc == item.cc);
                        if (ccCoincidencia != null)
                        {
                            item.cc = ccCoincidencia.descripcion;
                        }
                        else
                        {
                            item.cc = "";
                        }
                    }
                    else
                    {
                        if (item.idEmpresa == 1000)
                        {
                            var contratistaCoincidencia = lstEmpresasContratistas.FirstOrDefault(x => x.id == (int)item.idAgrupacion);
                            if (contratistaCoincidencia != null)
                            {
                                item.cc = contratistaCoincidencia.nombreEmpresa;
                            }
                            else
                            {
                                item.cc = "NO SE ENCONTRÓ";
                            }
                            //item.cc = lstEmpresasContratistas.First(x => x.id == (int)item.idAgrupacion).nombreEmpresa;
                        }
                        else if (item.idEmpresa < 1000)
                        {
                            item.cc = item.agrupacion.nomAgrupacion;
                        }
                        else
                        {
                            item.cc = lstEmpresasContratistasAgrupacion.First(x => x.id == item.idAgrupacion).nomAgrupacion;
                        }
                    }
                }

                bool puedeEliminar = (new UsuarioDAO().getViewAction(7320, "eliminarEvidencia"));

                if (puedeEliminar && informacionColaboradores.Count > 0)
                {
                    var listaCentrosCosto = informacionColaboradores.Select(x => x.cc).Distinct();

                    foreach (var centroCosto in listaCentrosCosto)
                    {
                        var registrosPorCC = informacionColaboradores.Where(x => x.cc == centroCosto).ToList();
                        var ultimaFecha = registrosPorCC.Max(x => x.fechaFin);
                        var ultimoRegistroPorCC = registrosPorCC.First(x => x.fechaFin == ultimaFecha);

                        var registro = informacionColaboradores.First(x => x.id == ultimoRegistroPorCC.id);

                        registro.puedeSerEliminado = true;
                    }
                }

                var data = informacionColaboradores.Where(x => x.fechaInicio.Date >= fechaInicio.Date && x.fechaFin.Date <= fechaFin.Date).ToList();

                //if (puedeEliminar && informacionColaboradores.Count > 0)
                //{
                //    informacionColaboradores.GroupBy(x => x.cc).ToList().ForEach(x =>
                //    {
                //        x.OrderByDescending(y => y.fechaFin).First().puedeSerEliminado = true;
                //    });
                //}

                //if (informacionColaboradores.Count > 0)
                if (data.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", informacionColaboradores);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de informacion");
            }

            return resultado;
        }
        public Dictionary<string, object> getInformacionColaboradoresByID(int id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {

                var informacionColaboradores = _context.tblS_IncidentesInformacionColaboradores.Where(x => x.id == id).ToList().Select(x => new informacionColaboradorDTO
                {
                    id = x.id,
                    horasHombre = x.horasHombre,
                    lostDay = x.lostDay,
                    cc = x.cc,
                    idEmpresa = x.idEmpresa,
                    idAgrupacion = (int)x.idAgrupacion,
                    fechaInicio = x.fechaInicio,
                    fechaInicioStr = x.fechaInicio.ToShortDateString(),
                    fechaFin = x.fechaFin,
                    fechaFinStr = x.fechaFin.ToShortDateString()
                }).FirstOrDefault();

                if (informacionColaboradores != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informacion", informacionColaboradores);

                    var listaInformacionColaboradoresClasificacion = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(
                        x => x.estatus && x.informacionColaboradoresID == informacionColaboradores.id
                    ).ToList();

                    if (listaInformacionColaboradoresClasificacion.Count() != 0)
                    {
                        if (listaInformacionColaboradoresClasificacion.Count() > 7 || listaInformacionColaboradoresClasificacion.Count() < 7)
                        {
                            throw new Exception("Existen " +
                                (listaInformacionColaboradoresClasificacion.Count() > 7 ? "más" : "menos")
                                + " de 7 registros capturados para la semana del incidente. IncidenteID = " + informacionColaboradores.id);
                        }

                        var listaClasificacion = listaInformacionColaboradoresClasificacion.Select(x => new
                        {
                            id = x.id,
                            cc = x.cc,
                            idEmpresa = x.idEmpresa,
                            idAgrupacion = (int)x.idAgrupacion,
                            fecha = x.fecha,
                            fechaString = x.fecha.ToShortDateString(),
                            horasMantenimiento = x.horasMantenimiento,
                            horasOperativo = x.horasOperativo,
                            horasAdministrativo = x.horasAdministrativo,
                            horasContratistas = x.horasContratistas,
                            informacionColaboradoresID = x.informacionColaboradoresID
                        }).ToList();

                        resultado.Add("listaClasificacion", listaClasificacion);
                    }
                    else
                    {
                        var listaFechas = new List<DateTime>();

                        for (var dt = informacionColaboradores.fechaInicio; dt <= informacionColaboradores.fechaFin; dt = dt.AddDays(1))
                        {
                            listaFechas.Add(dt);
                        }

                        resultado.Add("listaFechas", listaFechas);
                        resultado.Add("listaClasificacion", null);
                    }
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de informacion." + e.Message);
            }

            return resultado;
        }
        //raguilar 24/12/19
        public Dictionary<string, object> getInformacionColaboradoresByIDDetalle(int id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var informacionColaboradoresDetalle = _context.tblS_IncidentesInformacionColaboradoresDetalle
                    .Where(x => x.idIncidente == id && x.estatus)
                    .ToList()
                    .Select(x => new informacionColaboradorDetalleDTO
                    {
                        id = x.id,
                        cveEmpleado = x.cveEmpleado,
                        lostDayEmpleado = x.lostDayEmpleado,
                        nombre = x.nombre,
                        clasificacion = x.clasificacion,
                        clasificacionDesc = x.clasificacion.GetDescription()
                    }).ToList();

                if (informacionColaboradoresDetalle != null && informacionColaboradoresDetalle.Count() != 0)
                {
                    //resultado.Add(SUCCESS, true);
                    resultado.Add("informacionDetalle", informacionColaboradoresDetalle);
                }
                else
                {
                    //si no cuenta con registros hay que analizar la logica 
                    //resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTYDETALLE", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el catálogo de informacion");
            }

            return resultado;
        }
        public Dictionary<string, object> getFechasUltimoCorte(int idEmpresa, int idAgrupacion)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var ultimoRegistro = _context.tblS_IncidentesInformacionColaboradores.Where(x => x.idEmpresa == idEmpresa && x.idAgrupacion == idAgrupacion).OrderBy(x => x.id).ToList().Select(x => new informacionColaboradorDTO
                {
                    horasHombre = x.horasHombre,
                    lostDay = x.lostDay,
                    cc = x.cc,
                    fechaInicio = x.fechaInicio,
                    fechaInicioStr = x.fechaInicio.ToShortDateString(),
                    fechaFin = x.fechaFin,
                    fechaFinStr = x.fechaFin.ToShortDateString()
                }).LastOrDefault();

                if (ultimoRegistro != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("ultimoRegistro", ultimoRegistro);

                    var listaFechas = new List<DateTime>();

                    //Se empieza la siguiente semana.
                    for (var dt = ultimoRegistro.fechaFin.AddDays(1); dt <= ultimoRegistro.fechaFin.AddDays(7); dt = dt.AddDays(1))
                    {
                        listaFechas.Add(dt);
                    }

                    resultado.Add("listaFechas", listaFechas);
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("EMPTY", true);

                    var agrupacion = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == idAgrupacion);

                    var listaFechas = new List<DateTime>();

                    if (agrupacion == null)
                    {
                        var primerDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var primerDiaSemana = FirstDayOfWeek(primerDiaMes, DayOfWeek.Monday);
                        var ultimoDiaSemana = primerDiaSemana.AddDays(6);
                        for (var dt = primerDiaSemana; dt <= ultimoDiaSemana; dt = dt.AddDays(1))
                        {
                            listaFechas.Add(dt);
                        }
                    }

                    else 
                    {
                        var primerDiaMes = agrupacion.fechaCreacion;
                        var primerDiaSemana = FirstDayOfWeek(primerDiaMes, DayOfWeek.Monday);
                        var ultimoDiaSemana = primerDiaSemana.AddDays(6);
                        for (var dt = primerDiaSemana; dt <= ultimoDiaSemana; dt = dt.AddDays(1))
                        {
                            listaFechas.Add(dt);
                        }
                    }                   

                    resultado.Add("listaFechas", listaFechas);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener el ultimo registro");
            }

            return resultado;
        }

        public static DateTime FirstDayOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime LastDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public Dictionary<string, object> GuardarRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        )
        {
            var resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int idEmpresa = 0;
                    if (registroInformacion.idEmpresa > 0)
                        idEmpresa = registroInformacion.idEmpresa;

                    if (registroInformacion == null)
                    {
                        resultado.Add(SUCCESS, false);
                        return resultado;
                    }
                    registroInformacion.cc = "";
                    _context.tblS_IncidentesInformacionColaboradores.Add(registroInformacion);
                    _context.SaveChanges();

                    int id = registroInformacion.id;//idRegistro
                    int idAgrupacion = (int)registroInformacion.idAgrupacion;
                    GuardarRegistroInformacionDetalle(id, lstDetalle);
                    GuardarRegistroInformacionClasificacion(id, listaClasificacion, idEmpresa, idAgrupacion);

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();

                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, 0, new { registroInformacion = registroInformacion, lstDetalle = lstDetalle, listaClasificacion = listaClasificacion });

                    resultado.Add(SUCCESS, false);
                    resultado.Add(ERROR, "Ocurrió un error al guardar el registro");
                }
                return resultado;
            }
        }
        private void GuardarRegistroInformacionDetalle(int id, List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle)
        {
            try
            {
                lstDetalle.ForEach(x =>
                 {
                     x.idIncidente = id;
                     x.estatus = true;
                 });
                _context.tblS_IncidentesInformacionColaboradoresDetalle.AddRange(lstDetalle);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, 0, new { id = id, lstDetalle = lstDetalle });
            }
        }
        private void GuardarRegistroInformacionClasificacion(int id, List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion, int idEmpresa, int idAgrupacion)
        {
            try
            {
                listaClasificacion.ForEach(x =>
                {
                    x.cc = "";
                    x.informacionColaboradoresID = id;
                    x.estatus = true;
                    x.idEmpresa = idEmpresa;
                    x.idAgrupacion = idAgrupacion;
                });
                _context.tblS_IncidentesInformacionColaboradoresClasificacion.AddRange(listaClasificacion);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, 0, new { id = id, listaClasificacion = listaClasificacion, idEmpresa = idEmpresa, idAgrupacion = idAgrupacion });
            }
        }
        public Dictionary<string, object> UpdateRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        )
        {
            var resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registro = _context.tblS_IncidentesInformacionColaboradores.Where(x => x.id == registroInformacion.id).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.horasHombre = registroInformacion.horasHombre;
                        registro.lostDay = registroInformacion.lostDay;

                        _context.Entry(registro).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        int id = registroInformacion.id;//idRegistro

                        ActualizarRegistroInformacionDetalle(id, lstDetalle);
                        ActualizarRegistroInformacionClasificacion(id, listaClasificacion);
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();

                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, 0, new { registroInformacion = registroInformacion, lstDetalle = lstDetalle, listaClasificacion = listaClasificacion });

                    resultado.Add(ERROR, "Ocurrió un error al actualizar");
                }
            }

            return resultado;
        }
        private void ActualizarRegistroInformacionDetalle(int id, List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle)
        {

            var registrosActuales = _context.tblS_IncidentesInformacionColaboradoresDetalle.Where(x => x.idIncidente == id && x.estatus == true).ToList();

            //eliminado, modificado, agregado
            foreach (var item in registrosActuales)//elimina
            {
                if (lstDetalle == null)
                {
                    item.estatus = false;
                    _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
                else if (!lstDetalle.Any(c => c.id == item.id))
                {
                    item.estatus = false;
                    _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            if (lstDetalle != null)
            {
                foreach (var detalle in lstDetalle)
                {
                    var registro = _context.tblS_IncidentesInformacionColaboradoresDetalle.Where(x => x.id == detalle.id && x.estatus == true).FirstOrDefault();
                    if (registro != null)//modificado
                    {
                        registro.cveEmpleado = detalle.cveEmpleado;
                        registro.nombre = detalle.nombre;
                        registro.lostDayEmpleado = detalle.lostDayEmpleado;
                        _context.Entry(registro).State = System.Data.Entity.EntityState.Modified;
                    }
                    else//agregado
                    {
                        detalle.idIncidente = id;
                        detalle.estatus = true;
                        _context.tblS_IncidentesInformacionColaboradoresDetalle.Add(detalle);
                    }
                    _context.SaveChanges();
                }
            }
        }
        private void ActualizarRegistroInformacionClasificacion(int id, List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion)
        {
            var registrosActuales = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(x => x.informacionColaboradoresID == id && x.estatus == true).ToList();

            //eliminado, modificado, agregado
            foreach (var item in registrosActuales)//elimina
            {
                if (listaClasificacion == null)
                {
                    item.estatus = false;
                    _context.SaveChanges();
                }
                else if (!listaClasificacion.Any(c => c.id == item.id))
                {
                    item.estatus = false;
                    _context.SaveChanges();
                }
            }

            if (listaClasificacion != null)
            {
                foreach (var cla in listaClasificacion)
                {
                    var registro = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(x => x.id == cla.id && x.estatus == true).FirstOrDefault();

                    if (registro != null) //Modificado
                    {
                        registro.horasMantenimiento = cla.horasMantenimiento;
                        registro.horasOperativo = cla.horasOperativo;
                        registro.horasAdministrativo = cla.horasAdministrativo;
                        registro.horasContratistas = cla.horasContratistas;
                    }
                    else //Agregado
                    {
                        cla.cc = cla.cc ?? "";
                        cla.informacionColaboradoresID = id;
                        cla.estatus = true;
                        _context.tblS_IncidentesInformacionColaboradoresClasificacion.Add(cla);
                    }

                    _context.SaveChanges();
                }
            }
        }

        public Dictionary<string, object> EliminarHHT(int id)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registroHHT = _context.tblS_IncidentesInformacionColaboradores.First(x => x.id == id);

                    if (registroHHT == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró el registros que desea eliminar.");
                        return resultado;
                    }

                    // Eliminamos detalles en caso de que tenga.
                    var detalles = _context.tblS_IncidentesInformacionColaboradoresDetalle.Where(x => x.idIncidente == registroHHT.id).ToList();
                    if (detalles.Count > 0)
                    {
                        _context.tblS_IncidentesInformacionColaboradoresDetalle.RemoveRange(detalles);
                    }

                    var listaClasificacion = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(x => x.estatus && x.informacionColaboradoresID == registroHHT.id).ToList();

                    if (listaClasificacion.Count > 0)
                    {
                        foreach (var cla in listaClasificacion)
                        {
                            cla.estatus = false;
                            _context.Entry(cla).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }

                    _context.tblS_IncidentesInformacionColaboradores.Remove(registroHHT);
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "IndicadoresSeguridadController", "EliminarHHT", e, AccionEnum.ELIMINAR, id, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar eliminar el registros en el servidor.");
                }
            }

            return resultado;
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> getIncidentesRegistrables(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            var incidentes = ObtenerIncidentesFechasCC(busq);
            var informes = ObtenerInformesFechasCC(busq);

            var listaIncidentes_tipoAccidente_id = incidentes.Select(x => x.tipoAccidente_id).ToList();
            var listaInformes_tipoAccidente_id = informes.Select(x => x.tipoAccidente_id).ToList();

            var listatotal = new List<int?>();

            listatotal.AddRange(listaIncidentes_tipoAccidente_id);
            listatotal.AddRange(listaInformes_tipoAccidente_id);

            var incidentesRegistrables = _context.tblS_IncidentesTipos.Where(x => x.clasificacion.tipoEvento.tipoEvento == "Registrable").ToList().Select(x => new IncidentesRegistrablesDTO
            {
                incidenteTipo = x.clasificacion.abreviatura,
                incidenteCantidad = listatotal.Count(y => y != null && y.Value == x.id)
            });

            if (incidentesRegistrables.Count() > 0)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add("items", incidentesRegistrables);
            }
            else
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public Dictionary<string, object> getIncidentesReportables(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var incidentes = ObtenerIncidentesFechasCC(busq);

                var informes = ObtenerInformesFechasCC(busq);

                // Se filtran los accidentes que sean de daño material, para solo tomar en cuenta los de la subclasificación de mala operación.
                //incidentes = incidentes.Where(x => x.tipoAccidente_id.HasValue && x.tipoAccidente_id == 5 ? x.subclasificacionID == 1 : true).ToList();

                //informes = informes.Where(x => x.tipoAccidente_id.HasValue && x.tipoAccidente_id == 5 ? x.subclasificacionID == 1 : true).ToList();

                var listatotal = incidentes.Select(x => x.tipoAccidente_id).ToList();

                var inf = informes.Select(x => x.tipoAccidente_id).ToList();

                listatotal.AddRange(inf);

                var incidentesRegistrables = _context.tblS_IncidentesTipos
                    .Where(x => x.clasificacion.tipoEvento.tipoEvento == "Reportable")
                    .ToList()
                    .Select(x => new IncidentesRegistrablesDTO
                    {
                        incidenteTipo = x.clasificacion.abreviatura,
                        incidenteCantidad = listatotal.Count(y => y != null && y.Value == x.id)
                    }).ToList();


                if (incidentesRegistrables.Count() > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", incidentesRegistrables);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes registrables");
            }

            return resultado;
        }

        public Dictionary<string, object> getHorasHombreLostDay(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            #region Información Inicial
            #region Empleados
            List<empleadosCCDTO> lstCatEmpleado = new List<empleadosCCDTO>();

            var listCP = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.Construplan, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });
            var listARR = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.Arrendadora, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });
            var listCOL = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.Colombia, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });
            var listPER = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.PERU, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });

            lstCatEmpleado.AddRange(listCP);
            lstCatEmpleado.AddRange(listARR);
            lstCatEmpleado.AddRange(listCOL);
            lstCatEmpleado.AddRange(listPER);

            var listaAgrupacionDet = _context.tblS_IncidentesAgrupacionCCDet.ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Select(y => y.idAgrupacion).Contains(x.idAgrupacionCC) : true)
            ).ToList();

            #region Filtrar por divisiones y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaAgrupacionDet = listaAgrupacionDet.Join(
                    listaCentrosCostoDivision,
                    l => new { idAgrupacion = l.idAgrupacionCC },
                    cd => new { idAgrupacion = (int)cd.idAgrupacion },
                    (l, cd) => new { l, cd }
                ).Select(x => x.l).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaAgrupacionDet = listaAgrupacionDet.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    l => new { idAgrupacion = l.idAgrupacionCC },
                    cd => new { idAgrupacion = (int)cd.idAgrupacion },
                    (l, cd) => new { l, cd }
                ).Select(x => x.l).ToList();
            }
            #endregion

            var empleadosCC = lstCatEmpleado.DistinctBy(x => x.clave_empleado).Where(x =>
                listaAgrupacionDet != null ? listaAgrupacionDet.Select(y => y.cc).Contains(x.cc_contable) : true
            ).ToList();
            #endregion

            #region Informes
            var listaInformesFiltrada = _context.tblS_IncidentesInformePreliminar.Where(x =>
                DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaInformesFiltrada = listaInformesFiltrada.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaInformesFiltrada = listaInformesFiltrada.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            listaInformesFiltrada = listaInformesFiltrada.Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true).ToList();

            var ultimoInformeLTI = listaInformesFiltrada.Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                x.TiposAccidente != null &&
                (x.TiposAccidente.clasificacion.abreviatura == "LTI" || x.TiposAccidente.clasificacion.abreviatura == "Fatal") &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).OrderByDescending(x => x.fechaIncidente).FirstOrDefault();
            #endregion

            #region Incidentes
            var listaIncidentesFiltrada = _context.tblS_Incidentes.Where(x => x.idEmpresa == 0 &&
                DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaIncidentesFiltrada = listaIncidentesFiltrada.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaIncidentesFiltrada = listaIncidentesFiltrada.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            listaIncidentesFiltrada = listaIncidentesFiltrada.Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true).ToList();

            var ultimoIncidenteLTI = listaIncidentesFiltrada.Where(x =>
                (x.TiposAccidente.clasificacion.abreviatura == "LTI" || x.TiposAccidente.clasificacion.abreviatura == "Fatal") &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).OrderByDescending(x => x.fechaAccidente).FirstOrDefault();
            #endregion

            #region Fecha Último LTI
            DateTime fechaUltimoLTI = DateTime.MinValue;

            if (ultimoIncidenteLTI != null)
            {
                fechaUltimoLTI = ultimoIncidenteLTI.fechaAccidente;

                if (ultimoInformeLTI != null && ultimoInformeLTI.fechaIncidente > fechaUltimoLTI)
                {
                    fechaUltimoLTI = ultimoInformeLTI.fechaIncidente;
                }
            }
            else if (ultimoInformeLTI != null)
            {
                fechaUltimoLTI = ultimoInformeLTI.fechaIncidente;
            }
            #endregion

            #region Horas Hombre Registro Principal
            var listaInformacionColaboradores = _context.tblS_IncidentesInformacionColaboradores.ToList().Where(x =>
                x.idEmpresa < 1000 &&
                (x.fechaInicio.Date >= busq.fechaInicio.Date && x.fechaFin.Date <= busq.fechaFin.Date) &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaInformacionColaboradores = listaInformacionColaboradores.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaInformacionColaboradores = listaInformacionColaboradores.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion
            #endregion

            #region Horas Hombre Registro Clasificación
            var horasHombre = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(x =>
                x.estatus && x.idEmpresa < 1000 &&
                DbFunctions.TruncateTime(x.fecha) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fecha) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
            ).OrderBy(x => x.fecha).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                horasHombre = horasHombre.Join(
                    listaCentrosCostoDivision,
                    h => new { h.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                horasHombre = horasHombre.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    h => new { h.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }
            #endregion
            #endregion
            #endregion

            #region Tabla 1
            var hhtConstruplan = horasHombre.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo);
            var hhtContratistas = horasHombre.Sum(x => x.horasContratistas);
            var hhtTotal = hhtConstruplan + hhtContratistas;
            var hhtSinLTI = horasHombre.Where(x => x.fecha >= fechaUltimoLTI).Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
            var colaboradoresActivos = empleadosCC.Count();
            var tablaHorasHombre1 = new
            {
                hhtConstruplan = hhtConstruplan,
                hhtContratistas = hhtContratistas,
                hhtTotal = hhtTotal,
                hhtSinLTI = hhtSinLTI,
                colaboradoresActivos = colaboradoresActivos
            };
            #endregion

            #region Tabla 2
            var informesLTI = listaInformesFiltrada.Where(x => x.TiposAccidente.clasificacion.abreviatura == "Fatal" || x.TiposAccidente.clasificacion.abreviatura == "LTI").ToList();
            var incidentesLTI = listaIncidentesFiltrada.Where(x => x.TiposAccidente.clasificacion.abreviatura == "Fatal" || x.TiposAccidente.clasificacion.abreviatura == "LTI").ToList();
            var cantidadLTI = informesLTI.Where(x => !incidentesLTI.Select(y => y.informe_id).Contains(x.id)).Count() + incidentesLTI.Count();

            var informesTRI = listaInformesFiltrada.Where(x =>
                x.TiposAccidente.clasificacion.abreviatura == "Fatal" ||
                x.TiposAccidente.clasificacion.abreviatura == "LTI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MDI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MTI"
            ).ToList();
            var incidentesTRI = listaIncidentesFiltrada.Where(x =>
                x.TiposAccidente.clasificacion.abreviatura == "Fatal" ||
                x.TiposAccidente.clasificacion.abreviatura == "LTI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MDI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MTI"
            ).ToList();
            var cantidadTRI = informesTRI.Where(x => !incidentesTRI.Select(y => y.informe_id).Contains(x.id)).Count() + incidentesTRI.Count();

            var informesTI = listaInformesFiltrada.Where(x =>
                x.TiposAccidente.clasificacion.abreviatura == "Fatal" ||
                x.TiposAccidente.clasificacion.abreviatura == "LTI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MDI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MTI" ||
                x.TiposAccidente.clasificacion.abreviatura == "OI" ||
                x.TiposAccidente.clasificacion.abreviatura == "FAI" ||
                (x.TiposAccidente.clasificacion.abreviatura == "PD" && x.subclasificacionID == 1) || //Para la clasificación "PD" se obtienen nomás los que tengan la subclasificación "Mala Operación" (id = 1)
                x.TiposAccidente.clasificacion.abreviatura == "NM"
            ).ToList();
            var incidentesTI = listaIncidentesFiltrada.Where(x =>
                x.TiposAccidente.clasificacion.abreviatura == "Fatal" ||
                x.TiposAccidente.clasificacion.abreviatura == "LTI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MDI" ||
                x.TiposAccidente.clasificacion.abreviatura == "MTI" ||
                x.TiposAccidente.clasificacion.abreviatura == "OI" ||
                x.TiposAccidente.clasificacion.abreviatura == "FAI" ||
                (x.TiposAccidente.clasificacion.abreviatura == "PD" && x.subclasificacionID == 1) || //Para la clasificación "PD" se obtienen nomás los que tengan la subclasificación "Mala Operación" (id = 1)
                x.TiposAccidente.clasificacion.abreviatura == "NM"
            ).ToList();
            var cantidadTI = informesTI.Where(x => !incidentesTI.Select(y => y.informe_id).Contains(x.id)).Count() + incidentesTI.Count();

            var LTIFR = (cantidadLTI * 200000) / (hhtTotal > 0 ? hhtTotal : 1);
            var TRIFR = (cantidadTRI * 200000) / (hhtTotal > 0 ? hhtTotal : 1);
            var TIFR = (cantidadTI * 200000) / (hhtTotal > 0 ? hhtTotal : 1);
            var lostDays = listaInformacionColaboradores.Sum(x => x.lostDay);
            var severityRate = (lostDays * 1000) / (hhtTotal > 0 ? hhtTotal : 1);
            var tablaHorasHombre2 = new
            {
                LTIFR = LTIFR,
                TRIFR = TRIFR,
                TIFR = TIFR,
                lostDays = lostDays,
                severityRate = severityRate
            };
            #endregion

            resultado.Add("tablaHorasHombre1", tablaHorasHombre1);
            resultado.Add("tablaHorasHombre2", tablaHorasHombre2);
            resultado.Add(SUCCESS, true);

            return resultado;
        }

        public Dictionary<string, object> getHorasHombreLostDayAnterior(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            #region REGULAR
            List<empleadosCCDTO> lstCatEmpleado = new List<empleadosCCDTO>();

            var listCP = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.Construplan, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });
            var listARR = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.Arrendadora, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });
            var listCOL = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.Colombia, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });
            var listPER = _context.Select<empleadosCCDTO>(new DapperDTO { baseDatos = MainContextEnum.PERU, consulta = @"SELECT clave_empleado, cc_contable FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'" });

            lstCatEmpleado.AddRange(listCP);
            lstCatEmpleado.AddRange(listARR);
            lstCatEmpleado.AddRange(listCOL);
            lstCatEmpleado.AddRange(listPER);

            var lista = _context.tblS_IncidentesAgrupacionCCDet.ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Select(y => y.idAgrupacion).Contains(x.idAgrupacionCC) : true)
            ).ToList();

            #region Filtrar por divisiones y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                lista = lista.Join(
                    listaCentrosCostoDivision,
                    l => new { idAgrupacion = l.idAgrupacionCC },
                    cd => new { idAgrupacion = (int)cd.idAgrupacion },
                    (l, cd) => new { l, cd }
                ).Select(x => x.l).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                lista = lista.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    l => new { idAgrupacion = l.idAgrupacionCC },
                    cd => new { idAgrupacion = (int)cd.idAgrupacion },
                    (l, cd) => new { l, cd }
                ).Select(x => x.l).ToList();
            }
            #endregion

            var empleadosCC = lstCatEmpleado.DistinctBy(x => x.clave_empleado).Where(x =>
                lista != null ? lista.Select(y => y.cc).Contains(x.cc_contable) : true
            ).ToList();

            var listaIncidentesFiltrada = _context.tblS_Incidentes.Where(x => x.idEmpresa == 0 &&
                DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaIncidentesFiltrada = listaIncidentesFiltrada.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaIncidentesFiltrada = listaIncidentesFiltrada.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var incidente = listaIncidentesFiltrada.Where(x =>
                (x.TiposAccidente.clasificacion.abreviatura == "LTI" || x.TiposAccidente.clasificacion.abreviatura == "Fatal") &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).OrderByDescending(x => x.fechaAccidente).FirstOrDefault();

            var listaInformesFiltrada = _context.tblS_IncidentesInformePreliminar.Where(x =>
                DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaInformesFiltrada = listaInformesFiltrada.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaInformesFiltrada = listaInformesFiltrada.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var informePreliminar = listaInformesFiltrada.Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                x.TiposAccidente != null &&
                (x.TiposAccidente.clasificacion.abreviatura == "LTI" || x.TiposAccidente.clasificacion.abreviatura == "Fatal") &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).OrderByDescending(x => x.fechaIncidente).FirstOrDefault();

            DateTime fechaIncidente = DateTime.MinValue;

            if (incidente != null)
            {
                fechaIncidente = incidente.fechaAccidente;
                if (informePreliminar != null && informePreliminar.fechaIncidente > fechaIncidente)
                {
                    fechaIncidente = informePreliminar.fechaIncidente;
                }
            }
            else if (informePreliminar != null)
            {
                fechaIncidente = informePreliminar.fechaIncidente;
            }

            string[] months = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            var horasHombre = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(x =>
                x.estatus && x.idEmpresa == 0 &&
                DbFunctions.TruncateTime(x.fecha) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fecha) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
            ).OrderBy(x => x.fecha).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                horasHombre = horasHombre.Join(
                    listaCentrosCostoDivision,
                    h => new { h.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                horasHombre = horasHombre.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    h => new { h.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }
            #endregion

            decimal totalHorasHombre = 0;
            decimal totalHorasHombreUltIncap = 0;
            decimal totalHH = 0;

            if (incidente != null || informePreliminar != null)
            {
                for (int i = 0; i < months.Length; i++)
                {

                    if (fechaIncidente.Month - 1 == i)
                    {
                        if (horasHombre.Count() > 0)
                        {
                            decimal cantidadDias = (decimal)(horasHombre.Last().fecha - horasHombre.First().fecha).TotalDays + 1;
                            decimal cantidadDiasUltIncap = (decimal)(horasHombre.Last().fecha - fechaIncidente).TotalDays + 1;
                            decimal hh = default(decimal);

                            switch (busq.clasificacion)
                            {
                                case ClasificacionHHTEnum.Mantenimiento:
                                    hh += horasHombre.Sum(x => x.horasMantenimiento);
                                    break;
                                case ClasificacionHHTEnum.Operativo:
                                    hh += horasHombre.Sum(x => x.horasOperativo);
                                    break;
                                case ClasificacionHHTEnum.Administrativo:
                                    hh += horasHombre.Sum(x => x.horasAdministrativo);
                                    break;
                                case ClasificacionHHTEnum.Contratista:
                                    hh += horasHombre.Sum(x => x.horasContratistas);
                                    break;
                                default:
                                    hh += horasHombre.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
                                    break;
                            }

                            decimal horarHombreProrrateo = hh / cantidadDias;
                            decimal horarHombreProrrateoUltIncap = hh / cantidadDiasUltIncap;

                            foreach (DateTime day in EachDay(horasHombre.First().fecha, horasHombre.Last().fecha))
                            {
                                if (day.Date >= busq.fechaInicio.Date && day.Date <= busq.fechaFin.Date)
                                {
                                    if (day.Date >= fechaIncidente.Date)
                                    {
                                        var hht = horasHombre.FirstOrDefault(x => x.fecha.Date == day.Date);

                                        if (hht != null)
                                        {
                                            totalHorasHombre += (hht.horasMantenimiento + hht.horasOperativo + hht.horasAdministrativo + hht.horasContratistas);
                                        }

                                        totalHorasHombreUltIncap += horarHombreProrrateoUltIncap;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (horasHombre.Count() > 0)
                {
                    fechaIncidente = horasHombre.First().fecha;

                    decimal cantidadDias = (decimal)(horasHombre.Last().fecha - horasHombre.First().fecha).TotalDays + 1;
                    decimal cantidadDiasUltIncap = (decimal)(horasHombre.Last().fecha - fechaIncidente).TotalDays + 1;
                    decimal hh = default(decimal);

                    switch (busq.clasificacion)
                    {
                        case ClasificacionHHTEnum.Mantenimiento:
                            hh += horasHombre.Sum(x => x.horasMantenimiento);
                            break;
                        case ClasificacionHHTEnum.Operativo:
                            hh += horasHombre.Sum(x => x.horasOperativo);
                            break;
                        case ClasificacionHHTEnum.Administrativo:
                            hh += horasHombre.Sum(x => x.horasAdministrativo);
                            break;
                        case ClasificacionHHTEnum.Contratista:
                            hh += horasHombre.Sum(x => x.horasContratistas);
                            break;
                        default:
                            hh += horasHombre.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
                            break;
                    }

                    decimal horarHombreProrrateo = hh / cantidadDias;
                    decimal horarHombreProrrateoUltIncap = hh / cantidadDiasUltIncap;

                    foreach (DateTime day in EachDay(horasHombre.First().fecha, horasHombre.Last().fecha))
                    {
                        if (day.Date >= busq.fechaInicio.Date && day.Date <= busq.fechaFin.Date)
                        {
                            totalHorasHombre += horarHombreProrrateo;
                            totalHorasHombreUltIncap += horarHombreProrrateoUltIncap;
                        }
                    }
                }
            }

            totalHH = 0;

            int cantDias = 0;
            decimal hh2 = 0m;

            int cantDiasUltIncap = 0;
            decimal hh2UltIncap = 0m;

            if (horasHombre.Count() > 0)
            {
                cantDias = (int)(horasHombre.Last().fecha - horasHombre.First().fecha).TotalDays + 1;
                hh2 = default(decimal);

                cantDiasUltIncap = (int)(horasHombre.Last().fecha - fechaIncidente).TotalDays + 1;
                hh2UltIncap = default(decimal);

                switch (busq.clasificacion)
                {
                    case ClasificacionHHTEnum.Mantenimiento:
                        hh2 += horasHombre.Sum(x => x.horasMantenimiento);
                        hh2UltIncap += horasHombre.Where(x => x.fecha >= fechaIncidente).Sum(x => x.horasMantenimiento);
                        break;
                    case ClasificacionHHTEnum.Operativo:
                        hh2 += horasHombre.Sum(x => x.horasOperativo);
                        hh2UltIncap += horasHombre.Where(x => x.fecha >= fechaIncidente).Sum(x => x.horasOperativo);
                        break;
                    case ClasificacionHHTEnum.Administrativo:
                        hh2 += horasHombre.Sum(x => x.horasAdministrativo);
                        hh2UltIncap += horasHombre.Where(x => x.fecha >= fechaIncidente).Sum(x => x.horasAdministrativo);
                        break;
                    case ClasificacionHHTEnum.Contratista:
                        hh2 += horasHombre.Sum(x => x.horasContratistas);
                        hh2UltIncap += horasHombre.Where(x => x.fecha >= fechaIncidente).Sum(x => x.horasContratistas);
                        break;
                    default:
                        hh2 += horasHombre.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
                        hh2UltIncap += horasHombre.Where(x => x.fecha >= fechaIncidente).Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
                        break;
                }
            }

            var listaInformacionColaboradores = _context.tblS_IncidentesInformacionColaboradores.ToList().Where(x =>
                ((x.fechaInicio.Date >= busq.fechaInicio.Date && x.fechaFin.Date <= busq.fechaFin.Date) || (x.fechaInicio.AddDays(7).Date >= busq.fechaInicio.Date && x.fechaInicio.AddDays(7).Date <= busq.fechaFin.Date)) &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaInformacionColaboradores = listaInformacionColaboradores.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaInformacionColaboradores = listaInformacionColaboradores.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            decimal horarHombreProrrateo2 = hh2 / (cantDias > 0 ? cantDias : 1);
            decimal horarHombreProrrateo2UltIncap = hh2UltIncap / (cantDiasUltIncap > 0 ? cantDiasUltIncap : 1);

            decimal auxHH = 0;

            if (horasHombre.Count() > 0)
            {
                foreach (DateTime day in EachDay(horasHombre.First().fecha, horasHombre.Last().fecha))
                {
                    if (day.Date >= busq.fechaInicio.Date && day.Date <= busq.fechaFin.Date)
                    {
                        totalHH += horarHombreProrrateo2;
                    }
                }

                foreach (DateTime day in EachDay(horasHombre.First().fecha, horasHombre.Last().fecha))
                {
                    if (day.Date >= fechaIncidente.Date && day.Date <= busq.fechaFin.Date)
                    {
                        auxHH += horarHombreProrrateo2UltIncap;
                    }
                }
            }
            #endregion

            horasHombreLostDayDTO horasHombres = new horasHombreLostDayDTO
            {
                lostDays = listaInformacionColaboradores.Select(x => x.lostDay).Sum(),
                trabadoresPromedio = empleadosCC.Count(),
                horasHombres = totalHH,
                horasHombresSinIncidentes = auxHH,
                //HHTContratistas = totalHHContratistas,
                HHTTotales = totalHH,
                //HHTSinLTI = totalHHSinLTI,
                //LTIFR = (numTotalLTIS * 200000) / HHTTotales,
                //TRIFR = (numTotalIncidentesRegistrables * 200000) / HHTTotales,
                //TIFR = ((numTotalIncidentesRegistrables + incidentesReportables.Count()) * 200000) / HHTTotales,
                //SeverityRate = (listaInformacionColaboradores.Select(x => x.lostDay).Sum())
            };

            resultado.Add("tablaHorasHombre1", horasHombres);
            resultado.Add("tablaHorasHombre2", horasHombres);
            resultado.Add(SUCCESS, true);

            return resultado;
        }

        public Dictionary<string, object> getPotencialSeveridad(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaInformes = _context.tblS_IncidentesInformePreliminar.Where(x =>
                    DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin) &&
                    (x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.riesgo.HasValue))
                ).ToList().Where(x =>
                    (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                    (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                ).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    listaInformes = listaInformes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    listaInformes = listaInformes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                var listaIncidentes = _context.tblS_Incidentes.Where(x =>
                    DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin)
                ).ToList().Where(x =>
                    (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                    (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                ).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    listaIncidentes = listaIncidentes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    listaIncidentes = listaIncidentes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                decimal menorT = listaInformes.Count(x => x.riesgo == 1 || x.riesgo == 2) + listaIncidentes.Count(x => x.riesgo == 1 || x.riesgo == 2);
                decimal moderadoT = listaInformes.Count(x => x.riesgo == 3) + listaIncidentes.Count(x => x.riesgo == 3);
                decimal mayorT = listaInformes.Count(x => x.riesgo == 6) + listaIncidentes.Count(x => x.riesgo == 6);
                decimal catastroticoT = listaInformes.Count(x => x.riesgo == 9) + listaIncidentes.Count(x => x.riesgo == 9);
                int cantidadTotal = listaIncidentes.Count + listaInformes.Count;

                potencialSeveridadDTO potencial = new potencialSeveridadDTO
                {
                    menor = CalcularPotencialSeveridad(menorT, cantidadTotal),
                    moderado = CalcularPotencialSeveridad(moderadoT, cantidadTotal),
                    mayor = CalcularPotencialSeveridad(mayorT, cantidadTotal),
                    catastrotico = CalcularPotencialSeveridad(catastroticoT, cantidadTotal)
                };

                if (potencial != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", potencial);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener potencial de severidad");
            }

            return resultado;
        }

        public Dictionary<string, object> getIncidentesPorMes(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                string[] months = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
                List<incidentesPorMesDTO> incidentesMes = new List<incidentesPorMesDTO>();
                var incidentes = ObtenerIncidentesFechasCC(busq);
                var informes = ObtenerInformesFechasCC(busq);

                // Se filtran los accidentes que sean de daño material, para solo tomar en cuenta los de la subclasificación de mala operación.
                //incidentes = incidentes.Where(x => x.tipoAccidente_id.HasValue && x.tipoAccidente_id == 5 ? x.subclasificacionID == 1 : true);

                //informes = informes.Where(x => x.tipoAccidente_id.HasValue && x.tipoAccidente_id == 5 ? x.subclasificacionID == 1 : true);

                var inf = informes.Select(x => x.fechaIncidente).ToList();
                var listatotal = incidentes.Select(x => x.fechaAccidente).ToList();
                listatotal.AddRange(inf);
                for (int i = 0; i < months.Length; i++)
                {
                    incidentesMes.Add(new incidentesPorMesDTO()
                    {
                        mes = months[i],
                        cantidad = listatotal.Where(x => x.Month == (i + 1)).Count()
                    });
                }

                var datasets = new List<DatasetDTO>();

                datasets.Add(new DatasetDTO
                {
                    label = "Real",
                    data = incidentesMes.Select(x => Convert.ToDecimal(x.cantidad)).ToList(),
                    borderColor = incidentesMes.Select(x => "rgba(255, 99, 132, 0.2)").ToList(),
                    backgroundColor = incidentesMes.Select(x => "rgba(255, 99, 132, 0.2)").ToList(),
                    fill = true,
                    borderWidth = 2
                });

                CargarMetasIncidentesPorMes(busq.fechaInicio, busq.fechaFin, incidentesMes, ref datasets);

                if (incidentesMes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("labels", incidentesMes.Select(x => x.mes));
                    resultado.Add("datasets", datasets);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes reportables");
            }
            return resultado;
        }

        private void CargarMetasIncidentesPorMes(DateTime fechaInicio, DateTime fechaFin, List<incidentesPorMesDTO> incidentesMes, ref List<DatasetDTO> datasets)
        {
            // Se obtienen todos los años aplicables en el rango de fecha
            var listaAños = new List<int>();

            listaAños.Add(fechaInicio.Year);
            fechaInicio = fechaInicio.AddYears(1);

            while (fechaInicio.Year <= fechaFin.Year)
            {
                listaAños.Add(fechaInicio.Year);
                fechaInicio = fechaInicio.AddYears(1);
            }

            var metasTasaMensual = _context.tblS_IncidentesMetasGrafica
                .Where(x => x.tipoGrafica == TipoGraficaEnum.IncidentesPorMes)
                .Where(x => listaAños.Contains(x.año))
                .ToList();

            foreach (var meta in metasTasaMensual)
            {
                datasets.Add(new DatasetDTO
                {
                    label = meta.nombre,
                    data = incidentesMes.Select(x => meta.valor).ToList(),
                    borderColor = incidentesMes.Select(x => meta.colorString).ToList(),
                    backgroundColor = incidentesMes.Select(x => meta.colorString).ToList(),
                    type = "line",
                    borderWidth = 1
                });
            }
        }

        public Dictionary<string, object> getIncidentesRegistrablesXmes(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            var resultado = new Dictionary<string, object>();
            var fechaBusqInicio = busq.fechaInicio;
            var fechaBusqFin = busq.fechaFin;

            try
            {
                string[] months = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
                List<incidentesRegistrablesXmes> incidentesMes = new List<incidentesRegistrablesXmes>();
                List<incidentesRegistrablesXmes> hhMes = new List<incidentesRegistrablesXmes>();

                Tuple<DateTime, DateTime> rangoFechas = ObtenerRangoFechasTipoCarga(tipoCarga);
                busq.fechaInicio = rangoFechas.Item1;
                busq.fechaFin = rangoFechas.Item2;

                var listatotal = GetListaFechaAccidentesTRIFR(busq);

                var hh = _context.tblS_IncidentesInformacionColaboradoresClasificacion.ToList().Where(x =>
                    x.estatus && x.fecha.Date >= busq.fechaInicio.Date && x.fecha.Date <= busq.fechaFin.Date && (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
                ).OrderBy(x => x.fecha).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    hh = hh.Join(
                        listaCentrosCostoDivision,
                        h => new { h.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (h, cd) => new { h, cd }
                    ).Select(x => x.h).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    hh = hh.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        h => new { h.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (h, cd) => new { h, cd }
                    ).Select(x => x.h).ToList();
                }
                #endregion

                decimal totalHorasHombre = 0;
                decimal cantidadIncidentes = 0;
                decimal cantidadFormula = 200000;

                for (int i = 0; i < months.Length; i++)
                {
                    totalHorasHombre = 0;

                    if (hh.Count() > 0)
                    {
                        decimal cantidadDias = (decimal)(hh.Last().fecha - hh.First().fecha).TotalDays + 1;
                        decimal horasHombre = default(decimal);

                        switch (busq.clasificacion)
                        {
                            case ClasificacionHHTEnum.Mantenimiento:
                                horasHombre += hh.Sum(x => x.horasMantenimiento);
                                break;
                            case ClasificacionHHTEnum.Operativo:
                                horasHombre += hh.Sum(x => x.horasOperativo);
                                break;
                            case ClasificacionHHTEnum.Administrativo:
                                horasHombre += hh.Sum(x => x.horasAdministrativo);
                                break;
                            default:
                                horasHombre += hh.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo);
                                break;
                        }

                        decimal horarHombreProrrateo = horasHombre / cantidadDias;

                        foreach (DateTime day in EachDay(hh.First().fecha, hh.Last().fecha))
                        {
                            if (day >= busq.fechaInicio && day <= busq.fechaFin)
                            {
                                if (day.Month - 1 >= i && day.Month - 1 <= i)
                                {
                                    totalHorasHombre += Math.Round(horarHombreProrrateo, 2);
                                }
                            }
                        }
                    }

                    hhMes.Add(new incidentesRegistrablesXmes()
                    {
                        mesID = i + 1,
                        mes = months[i],
                        cantidad = totalHorasHombre
                    });
                }

                totalHorasHombre = 0;

                foreach (var element in hhMes)
                {
                    cantidadIncidentes = listatotal.Where(x => x.Month == element.mesID).Count();
                    totalHorasHombre = element.cantidad;
                    incidentesMes.Add(new incidentesRegistrablesXmes()
                    {
                        mes = element.mes,
                        cantidad = cantidadIncidentes > 0 ? Math.Round((cantidadIncidentes * cantidadFormula) / (totalHorasHombre == 0 ? 1 : totalHorasHombre), 2, MidpointRounding.AwayFromZero) : 0,
                    });
                }

                var datasets = new List<DatasetDTO>();

                datasets.Add(new DatasetDTO
                {
                    label = "Real",
                    data = incidentesMes.Select(x => x.cantidad).ToList(),
                    borderColor = incidentesMes.Select(x => "rgba(54, 162, 235, 0.2)").ToList(),
                    backgroundColor = incidentesMes.Select(x => "rgba(54, 162, 235, 0.2)").ToList(),
                    fill = true,
                    borderWidth = 2
                });

                CargarMetasTasaMensual(tipoCarga, incidentesMes, ref datasets);

                if (incidentesMes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("labels", incidentesMes.Select(x => x.mes));
                    resultado.Add("datasets", datasets);
                    resultado.Add("items", incidentesMes);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                    resultado.Add("items", new List<incidentesRegistrablesXmes>());
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes reportables");
            }
            busq.fechaInicio = fechaBusqInicio;
            busq.fechaFin = fechaBusqFin;
            return resultado;
        }

        private void CargarMetasTasaMensual(TipoCargaGraficaEnum tipoCarga, List<incidentesRegistrablesXmes> incidentesMes, ref List<DatasetDTO> datasets)
        {
            var metasTasaMensual = _context.tblS_IncidentesMetasGrafica.Where(x => x.tipoGrafica == TipoGraficaEnum.TasaMensual).ToList();

            switch (tipoCarga)
            {
                case TipoCargaGraficaEnum.Actual:
                    metasTasaMensual = metasTasaMensual.Where(x => x.año == DateTime.Now.Year).ToList();
                    break;
                case TipoCargaGraficaEnum.Anterior:
                    metasTasaMensual = metasTasaMensual.Where(x => x.año == (DateTime.Now.Year - 1)).ToList();
                    break;
                default:
                    throw new NotImplementedException("Tipo de carga no definido.");
            }

            foreach (var meta in metasTasaMensual)
            {
                datasets.Add(new DatasetDTO
                {
                    label = meta.nombre,
                    data = incidentesMes.Select(x => meta.valor).ToList(),
                    borderColor = incidentesMes.Select(x => meta.colorString).ToList(),
                    backgroundColor = incidentesMes.Select(x => meta.colorString).ToList(),
                    type = "line"
                });
            }
        }

        public Dictionary<string, object> getDanoInstalacionEquipo(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var incidentes = _context.tblS_Incidentes.Where(x =>
                    DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin) &&
                    x.tipoAccidente_id == 5 &&
                    x.subclasificacionID > 0
                ).ToList().Where(x =>
                    (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                    (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                ).Select(x => new { x.subclasificacionID, x.idAgrupacion }).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    incidentes = incidentes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    incidentes = incidentes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                var departamentos = _context.tblS_IncidentesDepartamentos.ToList().Where(x => (busq.arrDepto != null) ? busq.arrDepto.Contains(x.id) : true).Select(x => new
                {
                    id = x.id,
                    departamento = x.departamento.ToUpper().Trim()
                }).ToList();

                var informes = _context.tblS_IncidentesInformePreliminar.Where(x =>
                    DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin) &&
                    (x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue)) &&
                    x.tipoAccidente_id == 5 &&
                    x.subclasificacionID > 0
                ).ToList().Where(x =>
                    (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                    departamentos.Select(y => y.departamento.ToUpper().Trim()).Contains(x.departamentoEmpleado.ToUpper().Trim()) &&
                    (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                ).Select(x => new { x.subclasificacionID, x.idAgrupacion }).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    informes = informes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    informes = informes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                incidentes.AddRange(informes);

                decimal cantidadIncidentes = incidentes.Count();

                var subclasificaciones = _context.tblS_IncidentesSubclasificacion.Where(x => x.activo).ToList();

                List<dynamic> listaResultado = new List<dynamic>();

                foreach (var subc in subclasificaciones)
                {
                    decimal cantidad = incidentes.Count(x => x.subclasificacionID == subc.id);

                    listaResultado.Add(new
                    {
                        desc = subc.subclasificacion.ToUpper(),
                        cantidad = cantidad,
                    });
                }

                if (listaResultado.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", listaResultado);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los datos sobre daños materiales.");
            }

            return resultado;
        }

        public Dictionary<string, object> getIncidentesDepartamento(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var incidentes = _context.tblS_Incidentes.Where(x =>
                    DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin)
                ).ToList().Where(x =>
                    (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                    (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                ).Select(x => new { x.departamento_id, x.idAgrupacion }).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    incidentes = incidentes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    incidentes = incidentes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                var departamentos = _context.tblS_IncidentesDepartamentos.ToList().Where(x => busq.arrDepto != null ? busq.arrDepto.Contains(x.id) : true).Select(x => new
                {
                    id = x.id,
                    departamento = x.departamento.ToUpper().Trim()
                }).ToList();

                var informes = (
                    from inf in _context.tblS_IncidentesInformePreliminar.ToList()
                    where
                        (inf.fechaIncidente >= busq.fechaInicio && inf.fechaIncidente <= busq.fechaFin) &&
                        (inf.aplicaRIA == false || (inf.aplicaRIA && inf.terminado == false && inf.tipoAccidente_id.HasValue)) &&
                        (inf.departamento_id != 0) &&
                        (busq.arrDepto != null ? busq.arrDepto.Contains(inf.departamento_id) : true) &&
                        (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)inf.idAgrupacion && y.idEmpresa == inf.idEmpresa) : true)
                    select new { departamento_id = inf.departamento_id, inf.idAgrupacion }
                ).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    informes = informes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    informes = informes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idAgrupacion },
                        cd => new { cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                incidentes.AddRange(informes);

                List<dynamic> listaResultado = new List<dynamic>();

                foreach (var dep in departamentos)
                {
                    var cantidad = incidentes.Count(x => x.departamento_id == dep.id).ParseDecimal();

                    listaResultado.Add(new
                    {
                        departamentoID = dep.id,
                        departamentoDesc = dep.departamento,
                        cantidad = cantidad
                    });
                }

                if (listaResultado.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("data", listaResultado);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes por departamento.");
            }

            return resultado;
        }

        public Dictionary<string, object> getTasaIncidencias(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            var resultado = new Dictionary<string, object>();
            var fechaBusqInicio = busq.fechaInicio;
            var fechaBusqFin = busq.fechaFin;

            try
            {
                List<incidentesRegistrablesXmes> incidentesMes = new List<incidentesRegistrablesXmes>();
                Tuple<DateTime, DateTime> rangoFechas = ObtenerRangoFechasTipoCarga(tipoCarga);

                busq.fechaInicio = rangoFechas.Item1;
                busq.fechaFin = rangoFechas.Item2;

                var mesesTotales = ObtenerListaMesesTotales(tipoCarga);
                var listaTotal = GetListaFechaAccidentesTRIFR(busq);

                //HH X MES
                List<incidentesRegistrablesXmes> hhMes = GetIncidentesRegistrablesPorMes(mesesTotales, busq);

                //TRIFR
                decimal totalHH = 0;
                decimal totalIncidentes = 0;
                decimal cantidadFormula = 200000;

                foreach (var element in hhMes)
                {
                    totalIncidentes += listaTotal.Count(x => x.Month == element.mesAño.Month && x.Year == element.mesAño.Year);
                    totalHH += element.cantidad;
                    incidentesMes.Add(new incidentesRegistrablesXmes()
                    {
                        mes = element.mes,
                        cantidad = totalIncidentes > 0 && totalHH > 0 ? Math.Round((totalIncidentes * cantidadFormula) / totalHH, 2) : 0,
                        tasaIncidencia = (decimal)2.5
                    });
                }

                var datasets = new List<DatasetDTO>();

                datasets.Add(new DatasetDTO
                {
                    label = "Real",
                    data = incidentesMes.Select(x => x.cantidad).ToList(),
                    borderColor = new List<string> { "rgba(89, 171, 227, 1)" },
                    backgroundColor = new List<string> { "rgba(89, 171, 227, 1)" },
                    fill = false
                });

                CargarMetasTasaAnual(tipoCarga, incidentesMes, ref  datasets);

                if (incidentesMes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("labels", incidentesMes.Select(x => x.mes));
                    resultado.Add("datasets", datasets);
                    resultado.Add("items", incidentesMes);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                    resultado.Add("items", new List<incidentesRegistrablesXmes>());
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes reportables");
            }
            busq.fechaInicio = fechaBusqInicio;
            busq.fechaFin = fechaBusqFin;
            return resultado;
        }

        public Dictionary<string, object> getTIFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<incidentesRegistrablesXmes> incidentesMes = new List<incidentesRegistrablesXmes>();
                Tuple<DateTime, DateTime> rangoFechas = ObtenerRangoFechasTipoCarga(tipoCarga);

                busq.fechaInicio = rangoFechas.Item1;
                busq.fechaFin = rangoFechas.Item2;

                var mesesTotales = ObtenerListaMesesTotales(tipoCarga);
                var listaTotal = GetListaFechaAccidentesTIFR(busq);

                //HH X MES
                List<incidentesRegistrablesXmes> hhMes = GetIncidentesPorMes(mesesTotales, busq);

                //TRIFR
                decimal totalHH = 0;
                decimal totalIncidentes = 0;
                decimal cantidadFormula = 200000;

                foreach (var element in hhMes)
                {
                    totalIncidentes += listaTotal.Count(x => x.Month == element.mesAño.Month && x.Year == element.mesAño.Year);
                    totalHH += element.cantidad;

                    incidentesMes.Add(new incidentesRegistrablesXmes()
                    {
                        mes = element.mes,
                        cantidad = totalIncidentes > 0 && totalHH > 0 ? Math.Round((totalIncidentes * cantidadFormula) / totalHH, 2, MidpointRounding.AwayFromZero) : 0,
                        tasaIncidencia = (decimal)2.5
                    });
                }

                var datasets = new List<DatasetDTO>();

                datasets.Add(new DatasetDTO
                {
                    label = "Real",
                    data = incidentesMes.Select(x => x.cantidad).ToList(),
                    borderColor = new List<string> { "rgba(89, 171, 227, 1)" },
                    backgroundColor = new List<string> { "rgba(89, 171, 227, 1)" },
                    fill = false
                });

                CargarMetasTIFR(tipoCarga, incidentesMes, ref  datasets);

                if (incidentesMes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("labels", incidentesMes.Select(x => x.mes));
                    resultado.Add("datasets", datasets);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes reportables");
            }

            return resultado;
        }

        public Dictionary<string, object> getTPDFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga)
        {
            var resultado = new Dictionary<string, object>();

            //try
            //{
            List<incidentesRegistrablesXmes> incidentesMes = new List<incidentesRegistrablesXmes>();

            Tuple<DateTime, DateTime> rangoFechas = ObtenerRangoFechasTipoCarga(tipoCarga);
            busq.fechaInicio = rangoFechas.Item1;
            busq.fechaFin = rangoFechas.Item2;

            var mesesTotales = ObtenerListaMesesTotales(tipoCarga);

            var listaTotal = GetListaFechaAccidentesTPDFR(busq);

            //HH X MES
            List<incidentesRegistrablesXmes> hhMes = GetIncidentesPorMesTPDFR(mesesTotales, busq);

            //TRIFR
            decimal totalHH = 0;
            decimal totalIncidentes = 0;
            decimal cantidadFormula = 30000;
            foreach (var element in hhMes)
            {

                totalIncidentes += listaTotal.Count(x => x.Month == element.mesAño.Month && x.Year == element.mesAño.Year);
                totalHH += element.cantidad;
                incidentesMes.Add(new incidentesRegistrablesXmes()
                {
                    mes = element.mes,
                    cantidad = totalIncidentes > 0 && totalHH > 0 ? Math.Round((totalIncidentes * cantidadFormula) / totalHH, 2, MidpointRounding.AwayFromZero) : 0,
                    tasaIncidencia = (decimal)2.5
                });
            }

            var datasets = new List<DatasetDTO>();

            datasets.Add(new DatasetDTO
            {
                label = "Real",
                data = incidentesMes.Select(x => x.cantidad).ToList(),
                borderColor = new List<string> { "rgba(89, 171, 227, 1)" },
                backgroundColor = new List<string> { "rgba(89, 171, 227, 1)" },
                fill = false
            });

            CargarMetasTPDFR(tipoCarga, incidentesMes, ref  datasets);

            if (incidentesMes.Count > 0)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add("labels", incidentesMes.Select(x => x.mes));
                resultado.Add("datasets", datasets);
            }
            else
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("EMPTY", true);
            }
            //}
            //catch (Exception e)
            //{
            //    resultado.Add(SUCCESS, false);
            //    resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes reportables");
            //}

            return resultado;
        }

        private void CargarMetasTasaAnual(TipoCargaGraficaEnum tipoCarga, List<incidentesRegistrablesXmes> incidentesMes, ref List<DatasetDTO> datasets)
        {
            var metasTasaAnual = _context.tblS_IncidentesMetasGrafica.Where(x => x.tipoGrafica == TipoGraficaEnum.TasaAnual).ToList();

            switch (tipoCarga)
            {
                case TipoCargaGraficaEnum.AnteriorYActual:
                case TipoCargaGraficaEnum.Actual:
                    metasTasaAnual = metasTasaAnual.Where(x => x.año == DateTime.Now.Year).ToList();
                    break;
                case TipoCargaGraficaEnum.Anterior:
                    metasTasaAnual = metasTasaAnual.Where(x => x.año == (DateTime.Now.Year - 1)).ToList();
                    break;
                default:
                    throw new NotImplementedException("Tipo de carga no definido.");
            }

            foreach (var meta in metasTasaAnual)
            {
                datasets.Add(new DatasetDTO
                {
                    label = meta.nombre,
                    data = incidentesMes.Select(x => meta.valor).ToList(),
                    borderColor = new List<string> { meta.colorString },
                    backgroundColor = new List<string> { meta.colorString },
                });
            }
        }

        private void CargarMetasTIFR(TipoCargaGraficaEnum tipoCarga, List<incidentesRegistrablesXmes> incidentesMes, ref List<DatasetDTO> datasets)
        {
            var metasTasaAnual = _context.tblS_IncidentesMetasGrafica.Where(x => x.tipoGrafica == TipoGraficaEnum.TIFR).ToList();

            switch (tipoCarga)
            {
                case TipoCargaGraficaEnum.AnteriorYActual:
                case TipoCargaGraficaEnum.Actual:
                    metasTasaAnual = metasTasaAnual.Where(x => x.año == DateTime.Now.Year).ToList();
                    break;
                case TipoCargaGraficaEnum.Anterior:
                    metasTasaAnual = metasTasaAnual.Where(x => x.año == (DateTime.Now.Year - 1)).ToList();
                    break;
                default:
                    throw new NotImplementedException("Tipo de carga no definido.");
            }

            foreach (var meta in metasTasaAnual)
            {
                datasets.Add(new DatasetDTO
                {
                    label = meta.nombre,
                    data = incidentesMes.Select(x => meta.valor).ToList(),
                    borderColor = new List<string> { meta.colorString },
                    backgroundColor = new List<string> { meta.colorString },
                });
            }
        }

        private void CargarMetasTPDFR(TipoCargaGraficaEnum tipoCarga, List<incidentesRegistrablesXmes> incidentesMes, ref List<DatasetDTO> datasets)
        {
            var metasTasaAnual = _context.tblS_IncidentesMetasGrafica.Where(x => x.tipoGrafica == TipoGraficaEnum.TPDFR).ToList();

            switch (tipoCarga)
            {
                case TipoCargaGraficaEnum.AnteriorYActual:
                case TipoCargaGraficaEnum.Actual:
                    metasTasaAnual = metasTasaAnual.Where(x => x.año == DateTime.Now.Year).ToList();
                    break;
                case TipoCargaGraficaEnum.Anterior:
                    metasTasaAnual = metasTasaAnual.Where(x => x.año == (DateTime.Now.Year - 1)).ToList();
                    break;
                default:
                    throw new NotImplementedException("Tipo de carga no definido.");
            }

            foreach (var meta in metasTasaAnual)
            {
                datasets.Add(new DatasetDTO
                {
                    label = meta.nombre,
                    data = incidentesMes.Select(x => meta.valor).ToList(),
                    borderColor = new List<string> { meta.colorString },
                    backgroundColor = new List<string> { meta.colorString },
                });
            }
        }

        private List<incidentesRegistrablesXmes> GetIncidentesRegistrablesPorMes(List<Tuple<DateTime, string>> mesesTotales, busqDashboardDTO busq)
        {
            //var HHT = _context.tblS_IncidentesInformacionColaboradores
            //    .ToList()
            //    .SkipWhile(x => x.fechaFin < busq.fechaInicio)
            //    .TakeWhile(x => x.fechaInicio < busq.fechaFin)
            //    .Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any( y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion ) : true).ToList();
            var HHT = _context.tblS_IncidentesInformacionColaboradoresClasificacion.ToList().Where(x =>
                x.estatus && (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) && x.fecha.Date >= busq.fechaInicio.Date && x.fecha.Date <= busq.fechaFin.Date
            ).OrderBy(x => x.fecha).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                HHT = HHT.Join(
                    listaCentrosCostoDivision,
                    h => new { h.idEmpresa, h.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                HHT = HHT.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    h => new { h.idEmpresa, h.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }
            #endregion

            List<incidentesRegistrablesXmes> hhMes = new List<incidentesRegistrablesXmes>();

            foreach (var fechaMes in mesesTotales)
            {
                decimal totalHH = 0;
                //foreach (var registro in HHT)
                //{
                if (HHT.Count() > 0)
                {
                    decimal cantidadDias = (decimal)(HHT.Last().fecha - HHT.First().fecha).TotalDays + 1;
                    foreach (DateTime day in EachDay(HHT.First().fecha, HHT.Last().fecha))
                    {
                        if (day >= busq.fechaInicio && day <= busq.fechaFin)
                        {
                            if (day.Month == fechaMes.Item1.Month && day.Year == fechaMes.Item1.Year)
                            {
                                decimal horasHombre = default(decimal);

                                switch (busq.clasificacion)
                                {
                                    case ClasificacionHHTEnum.Mantenimiento:
                                        horasHombre += HHT.Sum(x => x.horasMantenimiento);
                                        break;
                                    case ClasificacionHHTEnum.Operativo:
                                        horasHombre += HHT.Sum(x => x.horasOperativo);
                                        break;
                                    case ClasificacionHHTEnum.Administrativo:
                                        horasHombre += HHT.Sum(x => x.horasAdministrativo);
                                        break;
                                    case ClasificacionHHTEnum.Contratista:
                                        horasHombre += HHT.Sum(x => x.horasContratistas);
                                        break;
                                    default:
                                        horasHombre += HHT.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
                                        break;
                                }

                                totalHH += Math.Round((horasHombre / cantidadDias), 2);
                            }
                        }
                    }
                }

                //}

                hhMes.Add(new incidentesRegistrablesXmes()
                {
                    mesAño = fechaMes.Item1,
                    mes = fechaMes.Item2,
                    cantidad = totalHH
                });

            }

            return hhMes;
        }

        private List<incidentesRegistrablesXmes> GetIncidentesPorMes(List<Tuple<DateTime, string>> mesesTotales, busqDashboardDTO busq)
        {
            var HHT = _context.tblS_IncidentesInformacionColaboradoresClasificacion.ToList().Where(x =>
                x.estatus && (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) && x.fecha.Date >= busq.fechaInicio.Date && x.fecha.Date <= busq.fechaFin.Date
            ).OrderBy(x => x.fecha).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                HHT = HHT.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                HHT = HHT.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            List<incidentesRegistrablesXmes> hhMes = new List<incidentesRegistrablesXmes>();

            foreach (var fechaMes in mesesTotales)
            {
                decimal totalHH = 0;
                if (HHT.Count() > 0)
                {
                    decimal cantidadDias = (decimal)(HHT.Last().fecha - HHT.First().fecha).TotalDays + 1;
                    foreach (DateTime day in EachDay(HHT.First().fecha, HHT.Last().fecha))
                    {
                        if (day >= busq.fechaInicio && day <= busq.fechaFin)
                        {
                            if (day.Month == fechaMes.Item1.Month && day.Year == fechaMes.Item1.Year)
                            {
                                decimal horasHombre = default(decimal);

                                switch (busq.clasificacion)
                                {
                                    case ClasificacionHHTEnum.Mantenimiento:
                                        horasHombre += HHT.Sum(x => x.horasMantenimiento);
                                        break;
                                    case ClasificacionHHTEnum.Operativo:
                                        horasHombre += HHT.Sum(x => x.horasOperativo);
                                        break;
                                    case ClasificacionHHTEnum.Administrativo:
                                        horasHombre += HHT.Sum(x => x.horasAdministrativo);
                                        break;
                                    case ClasificacionHHTEnum.Contratista:
                                        horasHombre += HHT.Sum(x => x.horasContratistas);
                                        break;
                                    default:
                                        horasHombre += HHT.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);
                                        break;
                                }

                                totalHH += Math.Round((horasHombre / cantidadDias), 2);
                            }
                        }
                    }
                }

                hhMes.Add(new incidentesRegistrablesXmes()
                {
                    mesAño = fechaMes.Item1,
                    mes = fechaMes.Item2,
                    cantidad = totalHH
                });
            }

            return hhMes;
        }

        private List<incidentesRegistrablesXmes> GetIncidentesPorMesTPDFR(List<Tuple<DateTime, string>> mesesTotales, busqDashboardDTO busq)
        {
            List<string> listaAreaCuenta = new List<string>();

            var lista = _context.tblS_IncidentesAgrupacionCCDet.ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == x.idAgrupacionCC) : true)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                lista = lista.Join(
                    listaCentrosCostoDivision,
                    l => new { idEmpresa = l.idEmpresa, idAgrupacion = l.idAgrupacionCC },
                    cd => new { idEmpresa = cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                    (l, cd) => new { l, cd }
                ).Select(x => x.l).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                lista = lista.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    l => new { idEmpresa = l.idEmpresa, idAgrupacion = l.idAgrupacionCC },
                    cd => new { idEmpresa = cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                    (l, cd) => new { l, cd }
                ).Select(x => x.l).ToList();
            }
            #endregion

            if (lista.Count > 0)
            {
                foreach (var cc in lista)
                {
                    var registroCC = _context.tblP_CC.FirstOrDefault(x => x.cc == cc.cc);

                    if (registroCC != null)
                    {
                        listaAreaCuenta.Add(registroCC.areaCuenta);
                    }
                }
            }

            List<tblM_CapHorometro> HHT = new List<tblM_CapHorometro>();

            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            {
                HHT = _db.tblM_CapHorometro.ToList().Where(x =>
                    (busq.arrCC != null ? listaAreaCuenta.Contains(x.CC) : true) && x.Fecha.Date >= busq.fechaInicio.Date && x.Fecha.Date <= busq.fechaFin.Date
                ).OrderBy(x => x.Fecha).ToList();
            }

            List<incidentesRegistrablesXmes> hhMes = new List<incidentesRegistrablesXmes>();

            foreach (var fechaMes in mesesTotales)
            {
                decimal totalHH = 0;

                if (HHT.Count > 0)
                {
                    decimal cantidadDias = (decimal)(HHT.Last().Fecha - HHT.First().Fecha).TotalDays + 1;
                    foreach (DateTime day in EachDay(HHT.First().Fecha, HHT.Last().Fecha))
                    {
                        if (day >= busq.fechaInicio && day <= busq.fechaFin)
                        {
                            if (day.Month == fechaMes.Item1.Month && day.Year == fechaMes.Item1.Year)
                            {
                                totalHH += Math.Round((HHT.Sum(x => x.HorasTrabajo) / cantidadDias), 2);
                            }
                        }
                    }
                }

                hhMes.Add(new incidentesRegistrablesXmes()
                {
                    mesAño = fechaMes.Item1,
                    mes = fechaMes.Item2,
                    cantidad = totalHH
                });
            }

            return hhMes;
        }

        public Dictionary<string, object> getIncidenciasPresentadas(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            // Se obtienen todos los informes que no apliquen RIA o que apliquen pero no esté completo.
            var listaInformesNoRIA = _context.tblS_IncidentesInformePreliminar.ToList().Where(x =>
                x.fechaIncidente.Date >= busq.fechaInicio.Date && x.fechaIncidente.Date <= busq.fechaFin.Date &&
                (x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue)) &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).Select(x => new InformesRIADTO
            {
                idEmpresa = x.idEmpresa,
                idAgrupacion = x.idAgrupacion,
                abreviaturaClasificacion = x.TiposAccidente.clasificacion.abreviatura,
                subclasificacionID = x.subclasificacionID
            }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaInformesNoRIA = listaInformesNoRIA.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaInformesNoRIA = listaInformesNoRIA.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var listaIncidentes = _context.tblS_Incidentes.ToList().Where(x =>
                x.fechaAccidente.Date >= busq.fechaInicio.Date && x.fechaAccidente.Date <= busq.fechaFin.Date &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).Select(x => new InformesRIADTO
            {
                idEmpresa = x.idEmpresa,
                idAgrupacion = x.idAgrupacion,
                abreviaturaClasificacion = x.TiposAccidente.clasificacion.abreviatura,
                subclasificacionID = x.subclasificacionID
            }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                listaIncidentes = listaIncidentes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                listaIncidentes = listaIncidentes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            // Se agregan los informes que (no apliquen RIA o no tengan RIA completo) a la lista total de incidentes.
            var listaTotalIncidentes = listaIncidentes.Concat(listaInformesNoRIA).ToList();

            // Abreviaciones para lesiones.
            var listaAbreviaciones = new List<string>() { "Fatal", "LTI", "MDI", "MTI", "OI", "FAI" };

            var tablaClasificacionIncidentes = _context.tblS_IncidentesClasificacion.ToList();

            var registrables = tablaClasificacionIncidentes.Where(x => x.tipoEvento.tipoEvento == "Registrable").Select(x => x.abreviatura);
            var incapacitantes = tablaClasificacionIncidentes.Where(x => x.abreviatura == "Fatal" || x.abreviatura == "LTI").Select(x => x.abreviatura);
            var lesionesTotales = tablaClasificacionIncidentes.Where(x => listaAbreviaciones.Contains(x.abreviatura)).Select(x => x.abreviatura);
            var lesionesDanios = tablaClasificacionIncidentes.Where(x => listaAbreviaciones.Contains(x.abreviatura) || x.abreviatura == "PD").Select(x => x.abreviatura).ToList();

            var horasHombrePorCC = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(x =>
                x.estatus && x.idAgrupacion > 0 && x.idEmpresa < 1000 &&
                DbFunctions.TruncateTime(x.fecha) >= DbFunctions.TruncateTime(busq.fechaInicio) &&
                DbFunctions.TruncateTime(x.fecha) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true)
            ).GroupBy(x => new { x.idAgrupacion, x.idEmpresa }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                horasHombrePorCC = horasHombrePorCC.Join(
                    listaCentrosCostoDivision,
                    h => new { h.Key.idEmpresa, h.Key.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                horasHombrePorCC = horasHombrePorCC.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    h => new { h.Key.idEmpresa, h.Key.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (h, cd) => new { h, cd }
                ).Select(x => x.h).ToList();
            }
            #endregion

            var hhCC = new List<incidentesRegistrablesXmes>();

            foreach (var cc in horasHombrePorCC)
            {
                var cantidad = 0m;
                var cantidadLostDay = 0m;

                var registrosPorTablaGeneral = cc.GroupBy(x => x.informacionColaboradoresID).ToList();
                var listaInformacionColaboradores = _context.tblS_IncidentesInformacionColaboradores.ToList().Where(x =>
                    x.idAgrupacion == cc.Key.idAgrupacion && x.idEmpresa == cc.Key.idEmpresa &&
                    x.fechaInicio.Date >= busq.fechaInicio.Date && x.fechaFin.Date <= busq.fechaFin.Date
                ).ToList();

                cantidadLostDay += listaInformacionColaboradores.Sum(x => x.lostDay);
                cantidad += cc.Sum(x => x.horasMantenimiento + x.horasOperativo + x.horasAdministrativo + x.horasContratistas);

                hhCC.Add(new incidentesRegistrablesXmes { cc = "", idEmpresa = cc.Key.idEmpresa, idAgrupacion = (int)cc.Key.idAgrupacion, cantidad = cantidad, lostDay = cantidadLostDay });
            }

            var incidentesReportados = new List<incidenciasPresentadasDTO>();

            var tablaCC = _context.tblS_IncidentesAgrupacionCC.ToList();
            var listaAgrupacionesContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();
            var listaContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();

            hhCC.ForEach(x =>
            {
                var proyectoDesc = "";

                if (x.idEmpresa == 1000) //Contratista
                {
                    proyectoDesc = listaContratistas.Where(y => y.id == (int)x.idAgrupacion).Select(z => z.nombreEmpresa).FirstOrDefault();
                }
                else if (x.idEmpresa == 2000) //Agrupación de contratistas
                {
                    proyectoDesc = listaAgrupacionesContratistas.Where(y => y.id == (int)x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault();
                }
                else //Agrupación de centros de costo
                {
                    proyectoDesc = tablaCC.Where(y => y.id == (int)x.idAgrupacion).Select(z => z.nomAgrupacion).FirstOrDefault();
                }

                var listaIncidentesPorCC = listaTotalIncidentes.Where(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == x.idAgrupacion).ToList();
                var cantidadRegistrables = PonderarEventosFatales(registrables, listaIncidentesPorCC);
                var cantidadIncidentesReportados = PonderarEventosFatales(incapacitantes, listaIncidentesPorCC);
                var cantidadLesionesTotales = PonderarEventosFatales(lesionesTotales, listaIncidentesPorCC);
                var cantidadLesionesDanios = PonderarEventosFatales(lesionesDanios, listaIncidentesPorCC);

                var row = new incidenciasPresentadasDTO
                {
                    centroCosto = proyectoDesc,
                    cantidadFatal = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "Fatal"),
                    cantidadLTA = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "LTI"),
                    cantidadATR = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "MDI"),
                    cantidadATM = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "MTI"),
                    cantidadAPA = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "FAI"),
                    cantidadDAMEQ = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "PD"),
                    cantidadNM = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "NM"),
                    cantidadOI = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "OI"),
                    cantidadEI = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "EI"),
                    severidad = (x.lostDay * 1000) / (x.cantidad > 0 ? x.cantidad : 1),
                    cantidadHH = horasHombrePorCC.FirstOrDefault(y => y.Key.idAgrupacion == x.idAgrupacion && y.Key.idEmpresa == x.idEmpresa).Sum(y => y.horasMantenimiento + y.horasOperativo + y.horasAdministrativo + y.horasContratistas),
                    horasHombre = x.cantidad,

                    LesionesRegistrables = ObtenerTotalIncidencias(cantidadRegistrables, x.cantidad),
                    LesionesIncapacitantes = ObtenerTotalIncidencias(cantidadIncidentesReportados, x.cantidad),
                    LesionesTotales = ObtenerTotalIncidencias(cantidadLesionesTotales, x.cantidad),
                    LesionesDanios = ObtenerTotalIncidencias(cantidadLesionesDanios, x.cantidad),

                    LTIFR = ObtenerTotalIncidencias(listaIncidentesPorCC.Where(w =>
                        w.abreviaturaClasificacion == "Fatal" ||
                        w.abreviaturaClasificacion == "LTI").Count(), x.cantidad),
                    TRIFR = ObtenerTotalIncidencias(listaIncidentesPorCC.Where(w =>
                        w.abreviaturaClasificacion == "Fatal" ||
                        w.abreviaturaClasificacion == "LTI" ||
                        w.abreviaturaClasificacion == "MDI" ||
                        w.abreviaturaClasificacion == "MTI").Count(), x.cantidad),
                    TIFR = ObtenerTotalIncidencias(listaIncidentesPorCC.Where(w =>
                        w.abreviaturaClasificacion == "Fatal" ||
                        w.abreviaturaClasificacion == "LTI" ||
                        w.abreviaturaClasificacion == "MDI" ||
                        w.abreviaturaClasificacion == "MTI" ||
                        w.abreviaturaClasificacion == "FAI" ||
                        (w.abreviaturaClasificacion == "PD" && w.subclasificacionID == 1) || //Para la clasificación "PD" se obtienen nomás los que tengan la subclasificación "Mala Operación" (id = 1)
                        w.abreviaturaClasificacion == "NM").Count(), x.cantidad),
                    lostDays = x.lostDay
                };

                row.IFA = (listaIncidentesPorCC.Where(w =>
                    w.abreviaturaClasificacion == "Fatal" ||
                    w.abreviaturaClasificacion == "LTI").Count() * 1000000) / (row.horasHombre > 0 ? row.horasHombre : 1);
                row.ISA = (x.lostDay * 1000000) / (row.horasHombre > 0 ? row.horasHombre : 1);
                row.IA = (row.IFA * row.ISA) / 1000;

                incidentesReportados.Add(row);
            });

            incidentesReportados = incidentesReportados.Where(x =>
                x.cantidadFatal > 0 ||
                x.cantidadLTA > 0 ||
                x.cantidadATR > 0 ||
                x.cantidadATM > 0 ||
                x.cantidadAPA > 0 ||
                x.cantidadDAMEQ > 0 ||
                x.cantidadNM > 0 ||
                x.cantidadEI > 0 ||
                x.cantidadOI > 0 ||
                x.LesionesRegistrables > 0 ||
                x.LesionesIncapacitantes > 0 ||
                x.LesionesTotales > 0 ||
                x.LesionesDanios > 0 ||
                x.severidad > 0 ||
                x.cantidadHH > 0
            ).OrderByDescending(x => x.LesionesTotales).ThenByDescending(x => x.LesionesDanios).ToList();

            // Totales
            incidenciasPresentadasDTO incidentesIndicadores = new incidenciasPresentadasDTO
            {
                cantidadFatalIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "Fatal"),
                cantidadLTAIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "LTI"),
                cantidadATRIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "MDI"),
                cantidadATMIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "MTI"),
                cantidadAPAIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "FAI"),
                cantidadDAMEQIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "PD"),
                cantidadNMIndicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "NM"),
                cantidadOIindicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "OI"),
                cantidadEIindicador = listaTotalIncidentes.Count(x => x.abreviaturaClasificacion == "EI"),
                cantidadTotalIndicador = listaTotalIncidentes.Count(),
            };

            //Row totales
            incidenciasPresentadasDTO rowTotales = new incidenciasPresentadasDTO()
            {
                centroCosto = "CORPORATIVO",
                cantidadFatal = incidentesReportados.Sum(e => e.cantidadFatal),
                cantidadLTA = incidentesReportados.Sum(e => e.cantidadLTA),
                cantidadATR = incidentesReportados.Sum(e => e.cantidadATR),
                cantidadATM = incidentesReportados.Sum(e => e.cantidadATM),
                cantidadAPA = incidentesReportados.Sum(e => e.cantidadAPA),
                cantidadDAMEQ = incidentesReportados.Sum(e => e.cantidadDAMEQ),
                cantidadNM = incidentesReportados.Sum(e => e.cantidadNM),
                cantidadEI = incidentesReportados.Sum(e => e.cantidadEI),
                cantidadOI = incidentesReportados.Sum(e => e.cantidadOI),
                cantidadHH = incidentesReportados.Sum(e => e.cantidadHH),
                cantidadFatalIndicador = incidentesReportados.Sum(e => e.cantidadFatalIndicador),
                cantidadLTAIndicador = incidentesReportados.Sum(e => e.cantidadLTAIndicador),
                cantidadATRIndicador = incidentesReportados.Sum(e => e.cantidadATRIndicador),
                cantidadATMIndicador = incidentesReportados.Sum(e => e.cantidadATMIndicador),
                cantidadAPAIndicador = incidentesReportados.Sum(e => e.cantidadAPAIndicador),
                cantidadDAMEQIndicador = incidentesReportados.Sum(e => e.cantidadDAMEQIndicador),
                cantidadNMIndicador = incidentesReportados.Sum(e => e.cantidadNMIndicador),
                cantidadOIindicador = incidentesReportados.Sum(e => e.cantidadOIindicador),
                cantidadEIindicador = incidentesReportados.Sum(e => e.cantidadEIindicador),
                cantidadTotalIndicador = incidentesReportados.Sum(e => e.cantidadTotalIndicador),
                cantidadMaxIndicador = incidentesReportados.Sum(e => e.cantidadMaxIndicador),
                cantidadMinIndicador = incidentesReportados.Sum(e => e.cantidadMinIndicador),
                cantidadTotalTipo = incidentesReportados.Sum(e => e.cantidadTotalTipo),

                LesionesRegistrables = incidentesReportados.Sum(e => e.LesionesRegistrables),
                LesionesIncapacitantes = incidentesReportados.Sum(e => e.LesionesIncapacitantes),
                LesionesTotales = incidentesReportados.Sum(e => e.LesionesTotales),
                LesionesDanios = incidentesReportados.Sum(e => e.LesionesDanios),

                horasHombre = incidentesReportados.Sum(e => e.horasHombre),
                lostDays = incidentesReportados.Sum(e => e.lostDays),
                orden = "total"
            };

            rowTotales.severidad = (rowTotales.lostDays * 1000) / (rowTotales.horasHombre > 0 ? rowTotales.horasHombre : 1);
            rowTotales.LTIFR = (listaTotalIncidentes.Where(w => w.abreviaturaClasificacion == "Fatal" || w.abreviaturaClasificacion == "LTI").Count() * 200000) / (rowTotales.horasHombre > 0 ? rowTotales.horasHombre : 1);
            rowTotales.TRIFR = (listaTotalIncidentes.Where(w =>
                w.abreviaturaClasificacion == "Fatal" ||
                w.abreviaturaClasificacion == "LTI" ||
                w.abreviaturaClasificacion == "MDI" ||
                w.abreviaturaClasificacion == "MTI").Count() * 200000) / (rowTotales.horasHombre > 0 ? rowTotales.horasHombre : 1);
            rowTotales.TIFR = (listaTotalIncidentes.Where(w =>
                w.abreviaturaClasificacion == "Fatal" ||
                w.abreviaturaClasificacion == "LTI" ||
                w.abreviaturaClasificacion == "MDI" ||
                w.abreviaturaClasificacion == "MTI" ||
                w.abreviaturaClasificacion == "FAI" ||
                (w.abreviaturaClasificacion == "PD" && w.subclasificacionID == 1) || //Para la clasificación "PD" se obtienen nomás los que tengan la subclasificación "Mala Operación" (id = 1)
                w.abreviaturaClasificacion == "NM").Count() * 200000) / (rowTotales.horasHombre > 0 ? rowTotales.horasHombre : 1);
            rowTotales.IFA = (listaTotalIncidentes.Where(w => w.abreviaturaClasificacion == "Fatal" || w.abreviaturaClasificacion == "LTI").Count() * 1000000) / (rowTotales.horasHombre > 0 ? rowTotales.horasHombre : 1);
            rowTotales.ISA = (rowTotales.lostDays * 1000000) / (rowTotales.horasHombre > 0 ? rowTotales.horasHombre : 1);
            rowTotales.IA = (rowTotales.IFA * rowTotales.ISA) / 1000;

            var lstIncidentesReportadosConTotales = new List<incidenciasPresentadasDTO>();

            lstIncidentesReportadosConTotales.Add(rowTotales);
            lstIncidentesReportadosConTotales.AddRange(incidentesReportados);

            if (incidentesReportados.Count > 0)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add("items", lstIncidentesReportadosConTotales);
                resultado.Add("indicadores", incidentesIndicadores);
                resultado.Add("permisoEliminar", new UsuarioDAO().getViewAction(7320, "eliminarEvidencia"));
            }
            else
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("EMPTY", true);
                resultado.Add("items", new List<incidenciasPresentadasDTO>());
            }

            return resultado;
        }

        public Dictionary<string, object> getIncidenciasPresentadasTipo(string tipo, busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var fechaFinal = busq.fechaFin.AddHours(23).AddMinutes(59);

                var tablaIncidentes = _context.tblS_Incidentes.ToList();

                var tablaInformes = _context.tblS_IncidentesInformePreliminar.ToList();

                // Se obtienen todos los informes que no apliquen RIA o que apliquen pero no esté completo.
                var listaInformesNoRIA = tablaInformes
                    .Where(x => x.fechaIncidente >= busq.fechaInicio && x.fechaIncidente <= fechaFinal)
                    .Where(x => x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue))
                    .Where(x => (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true))
                    .Where(x => busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true)
                    .Where(x => busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                    .Where(x => tipo == "Total Incidencias" ? true : x.TiposAccidente.clasificacion.abreviatura == tipo)
                    .Select(x => new InformesRIADTO { cc = x.cc, idAgrupacion = x.idAgrupacion, abreviaturaClasificacion = x.TiposAccidente.clasificacion.abreviatura });

                var listaIncidentes = tablaIncidentes
                    .Where(x => x.fechaAccidente >= busq.fechaInicio && x.fechaAccidente <= fechaFinal)
                    .Where(x => busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.ParseInt()) : true)
                    .Where(x => (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true))
                    .Where(x => busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                    .Where(x => tipo == "Total Incidencias" ? true : x.TiposAccidente.clasificacion.abreviatura == tipo)
                    .Select(x => new InformesRIADTO { cc = x.cc, idAgrupacion = x.idAgrupacion, abreviaturaClasificacion = x.TiposAccidente.clasificacion.abreviatura });

                // Se agregan los informes que (no apliquen RIA o no tengan RIA completo) a la lista total de incidentes.
                var listaTotalIncidentes = listaIncidentes.Concat(listaInformesNoRIA); ;

                var tablaCC = _context.tblP_CC.ToList();
                var lstAgrupaciones = _context.tblS_IncidentesAgrupacionCC.ToList();

                var tablaHHT = _context.tblS_IncidentesInformacionColaboradores.ToList();

                var listaIncidentesTipo = new List<incidenciasPresentadasDTO>();

                listaTotalIncidentes
                    .GroupBy(x => x.idAgrupacion)
                    .ForEach(x =>
                    {
                        var objAgrupacion = lstAgrupaciones.FirstOrDefault(e => e.id == x.Key);

                        var cantidadHH = tablaHHT.Where(y => y.idAgrupacion == x.Key && y.horasHombre > 0).ToList();
                        var totalHorasHombre = ObtenerHHTPeriodo(cantidadHH, busq.fechaInicio, fechaFinal);
                        string centroCosto = objAgrupacion != null ? objAgrupacion.nomAgrupacion : "S/N";

                        listaIncidentesTipo.Add(new incidenciasPresentadasDTO
                         {
                             centroCosto = centroCosto,
                             cantidadHH = totalHorasHombre,
                             cantidadTotalTipo = x.Count(),
                         });
                    });

                listaIncidentesTipo = listaIncidentesTipo
                    .OrderByDescending(x => x.cantidadTotalTipo)
                    .ThenByDescending(x => x.cantidadHH)
                    .ThenBy(x => x.centroCosto)
                    .ToList();

                listaIncidentesTipo.Add(new incidenciasPresentadasDTO
                {
                    centroCosto = "TOTAL",
                    cantidadHH = Math.Round(listaIncidentesTipo.Sum(x => x.cantidadHH), 2),
                    cantidadTotalTipo = listaIncidentesTipo.Sum(x => x.cantidadTotalTipo),
                });

                if (listaIncidentesTipo.Count() > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("indicadores", listaIncidentesTipo);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes presentados");
            }

            return resultado;
        }

        public Dictionary<string, object> getAccidentabilidad(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();
            //try
            //{
            var informes = _context.tblS_IncidentesInformePreliminar.ToList().Where(x =>
                x.fechaIncidente.Date >= busq.fechaInicio.Date &&
                x.fechaIncidente.Date <= busq.fechaFin.Date &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) &&
                    //(busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                informes = informes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                informes = informes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var incidencias = _context.tblS_Incidentes.ToList().Where(x =>
                x.fechaAccidente.Date >= busq.fechaInicio.Date &&
                x.fechaAccidente.Date <= busq.fechaFin.Date &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) &&
                    //(busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                incidencias = incidencias.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                incidencias = incidencias.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var informesConRIA = incidencias.Select(x => x.informe_id).ToList();

            //Se filtran los informes que no tengan RIA para que no se repita la información.
            informes = informes.Where(x => !informesConRIA.Contains(x.id)).ToList();

            var incidenciasF = incidencias.Where(x =>
                x.puestoEmpleado != "GERENTE DE SEGURIDAD Y M.A. DIV. CONSTRUCC" &&
                x.Departamentos.departamento != "ND" &&
                x.TurnoEmpleado.turno != "ND" &&
                x.AgentesImplicados.agenteImplicado != "ND" &&
                x.ExperienciaEmpleado.empleadoExperiencia != "ND" &&
                x.AntiguedadEmpleado.antiguedadEmpleado != "ND" &&
                x.lugarAccidente != "ND" &&
                x.diasTrabajadosEmpleado != 0 &&
                x.TiposContacto.tipoContacto != "ND" &&
                x.ProtocolosTrabajo.protocoloTrabajo != "ND"
            ).ToList();

            var informesF = informes.Where(x =>
                x.puestoEmpleado != "GERENTE DE SEGURIDAD Y M.A. DIV. CONSTRUCC" &&
                x.Departamentos != null && x.Departamentos.departamento != "ND" &&
                x.TurnoEmpleado != null && x.TurnoEmpleado.turno != "ND" &&
                x.AgenteImplicado != null && x.AgenteImplicado.agenteImplicado != "ND" &&
                x.ExperienciaEmpleado != null && x.ExperienciaEmpleado.empleadoExperiencia != "ND" &&
                x.AntiguedadEmpleado != null && x.AntiguedadEmpleado.antiguedadEmpleado != "ND" &&
                x.lugarAccidente != "ND" &&
                x.diasTrabajadosEmpleado != 0 &&
                x.TipoContacto != null && x.TipoContacto.tipoContacto != "ND" &&
                x.ProtocolosTrabajo != null && x.ProtocolosTrabajo.protocoloTrabajo != "ND"
            ).ToList();

            informesF = informesF.Where(x => !informesConRIA.Contains(x.id)).ToList();

            if (incidenciasF.Count == 0 && informesF.Count() == 0)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("EMPTY", true);
                return resultado;
            }

            var rangoHoras = new List<rangoHorasDTO>();
            var rangoInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 00);
            var rangoFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 7, 0, 00);
            var ts = rangoFinal - rangoInicial;

            IEnumerable<int> hoursBetween = Enumerable.Range(0, (int)ts.TotalHours).Select(i => rangoInicial.AddHours(i).Hour);

            foreach (int hour in hoursBetween)
            {
                var minutesInicial = TimeSpan.FromMinutes(0);
                var minutes = TimeSpan.FromMinutes(59);
                var resultInicial = TimeSpan.FromHours(hour).Add(minutesInicial);
                var result = TimeSpan.FromHours(hour).Add(minutes);
                var fromTimeInicial = resultInicial.ToString("hh':'mm");
                var fromTimeString = result.ToString("hh':'mm");

                rangoHoras.Add(new rangoHorasDTO()
                {
                    hora = hour,
                    rango = fromTimeInicial + " - " + fromTimeString
                });
            }

            var ci = new CultureInfo("Es-Es");
            var diasIncidencias = incidencias.Select(x => x.fechaAccidente.DayOfWeek).ToList();

            string cc = "";

            if (busq.arrGrupos != null)
            {
                cc = _context.tblS_IncidentesAgrupacionCC.ToList().FirstOrDefault(x => x.id == busq.arrGrupos[0].idAgrupacion).nomAgrupacion;
            }

            tendenciasAccidentabilidadDTO tendenciasAccidentabilidad = new tendenciasAccidentabilidadDTO();

            #region Asignación de valores.
            var diaJoin = incidencias.Select(x => ci.DateTimeFormat.GetDayName(x.fechaAccidente.DayOfWeek)).ToList();
            var diaJoinInformes = informes.Select(x => ci.DateTimeFormat.GetDayName(x.fechaIncidente.DayOfWeek)).ToList();
            diaJoin.AddRange(diaJoinInformes);

            var diaAgrupado = diaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.dia = diaAgrupado.Select(x => x.Key).ToList().FirstOrDefault().ToString();
            tendenciasAccidentabilidad.porcentajeDia = Math.Round((decimal)(diaAgrupado.Select(x => x.Count()).ToList().FirstOrDefault() * 100) / diaJoin.Count(), 2);

            var horaJoin = incidencias.Select(x => x.fechaAccidente.Hour).ToList();
            var horaJoinInformes = informes.Select(x => x.fechaIncidente.Hour).ToList();
            horaJoin.AddRange(horaJoinInformes);

            var horaAgrupada = horaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.hora = rangoHoras.Where(z => z.hora == horaAgrupada.Select(x => x.Key).ToList().FirstOrDefault()).Select(z => z.rango).FirstOrDefault();
            tendenciasAccidentabilidad.porcentajeHora = Math.Round((decimal)(horaAgrupada.Select(x => x.Count()).ToList().FirstOrDefault() * 100) / horaJoin.Count(), 2);

            var turnoJoin = incidenciasF.Select(x => x.TurnoEmpleado.turno).ToList();
            var turnoJoinInformes = informesF.Select(x => x.TurnoEmpleado.turno).ToList();
            turnoJoin.AddRange(turnoJoinInformes);

            var turnoAgrupado = turnoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.turno = turnoJoin.Count > 0 ? turnoAgrupado.Select(x => x.Key).ToList().FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeTurno = turnoJoin.Count > 0 ? Math.Round((decimal)(turnoAgrupado.Select(x => x.Count()).ToList().FirstOrDefault() * 100) / turnoJoin.Count(), 2) : 0;

            var ccJoin = incidencias.Select(x => x.cc).ToList();
            var ccJoinInformes = informes.Select(x => x.cc).ToList();
            ccJoin.AddRange(ccJoinInformes);

            var ccAgrupado = ccJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            //tendenciasAccidentabilidad.cc = listaCC.Where(y => y.cc == ccAgrupado.Select(x => x.Key).ToList().FirstOrDefault().ToString()).Select(y => y.descripcion).FirstOrDefault();
            tendenciasAccidentabilidad.cc = cc;
            tendenciasAccidentabilidad.porcentajeCC = Math.Round((decimal)(ccAgrupado.Select(x => x.Count()).ToList().FirstOrDefault() * 100) / ccJoin.Count(), 2);

            var actividadJoin = incidencias.Select(x => x.actividadRutinaria).ToList();
            var actividadJoinInformes = informes.Select(x => x.actividadRutinaria).ToList();
            actividadJoin.AddRange(actividadJoinInformes);

            var actividadAgrupada = actividadJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.actividad = actividadAgrupada.Select(x => x.Key).FirstOrDefault() == true ? "Rutinaria" : "No Rutinaria";
            tendenciasAccidentabilidad.porcentajeActividad = Math.Round((decimal)(actividadAgrupada.Select(x => x.Count()).FirstOrDefault() * 100) / actividadJoin.Count(), 2);

            var tareaJoin = incidencias.Select(x => x.trabajoPlaneado).ToList();
            var tareaJoinInformes = informes.Select(x => x.trabajoPlaneado).ToList();
            tareaJoin.AddRange(tareaJoinInformes);

            var tareaAgrupado = tareaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.tarea = tareaAgrupado.Select(x => x.Key).FirstOrDefault() == true ? "Planeada" : "No Planeada";
            tendenciasAccidentabilidad.porcentajeTarea = Math.Round((decimal)(tareaAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / tareaJoin.Count(), 2);

            var agenteJoin = incidenciasF.Select(x => x.AgentesImplicados.agenteImplicado).ToList();
            var agenteJoinInformes = informesF.Select(x => x.AgenteImplicado.agenteImplicado).ToList();
            agenteJoin.AddRange(agenteJoinInformes);

            var agenteAgrupado = agenteJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.agente = agenteJoin.Count > 0 ? agenteAgrupado.Select(x => x.Key).ToList().FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeAgente = agenteJoin.Count > 0 ? Math.Round((decimal)(agenteAgrupado.Select(x => x.Count()).ToList().FirstOrDefault() * 100) / agenteJoin.Count(), 2) : 0;

            //var edadJoin = incidencias.Select(x => x.edadEmpleado).ToList();
            //var edadJoinInformes = informes.Select(x => x.emple).ToList();
            //edadJoin.AddRange(edadJoinInformes);
            //Informe Preliminar no contiene edad del empleado
            var edadAgrupado = incidencias.GroupBy(x => x.edadEmpleado).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.edad = edadAgrupado.Select(x => x.Key).FirstOrDefault().ToString();
            tendenciasAccidentabilidad.porcentajeEdad = Math.Round((decimal)(edadAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / incidencias.Count(), 2);

            var puestoJoin = incidenciasF.Select(x => x.puestoEmpleado).ToList();
            var puestoJoinInformes = informesF.Select(x => x.puestoEmpleado).ToList();
            puestoJoin.AddRange(puestoJoinInformes);

            var puestoAgrupado = puestoJoin.GroupBy(x => x);
            tendenciasAccidentabilidad.puesto = puestoJoin.Count > 0 ? puestoAgrupado.Where(x => x.Key != "GERENTE DE SEGURIDAD Y M.A. DIV. CONSTRUCC").OrderByDescending(x => x.Count()).Take(1).Select(x => x.Key).FirstOrDefault().ToString() : "";
            tendenciasAccidentabilidad.porcentajePuesto = puestoJoin.Count > 0 ? Math.Round((decimal)(puestoAgrupado.OrderByDescending(x => x.Count()).Take(1).Select(x => x.Count()).FirstOrDefault() * 100) / puestoJoin.Count(), 2) : 0;

            var experienciaJoin = incidenciasF.Select(x => x.ExperienciaEmpleado.empleadoExperiencia).ToList();
            var experienciaJoinInformes = informesF.Select(x => x.ExperienciaEmpleado.empleadoExperiencia).ToList();
            experienciaJoin.AddRange(experienciaJoinInformes);

            var experienciaAgrupado = experienciaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.experiencia = experienciaJoin.Count > 0 ? experienciaAgrupado.Select(x => x.Key).FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeExperiencia = experienciaJoin.Count > 0 ? Math.Round((decimal)(experienciaAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / experienciaJoin.Count(), 2) : 0;

            var antiguedadEmpresaJoin = incidenciasF.Select(x => x.AntiguedadEmpleado.antiguedadEmpleado).ToList();
            var antiguedadEmpresaJoinInformes = informesF.Select(x => x.AntiguedadEmpleado.antiguedadEmpleado).ToList();
            antiguedadEmpresaJoin.AddRange(antiguedadEmpresaJoinInformes);

            var antiguedadEmpresaAgrupado = antiguedadEmpresaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.antiguedadEmpresa = antiguedadEmpresaJoin.Count > 0 ? antiguedadEmpresaAgrupado.Select(x => x.Key).FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeAntiguedadEmpresa = antiguedadEmpresaJoin.Count > 0 ? Math.Round((decimal)(antiguedadEmpresaAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / antiguedadEmpresaJoin.Count(), 2) : 0;

            var diasTrabajadosJoin = incidenciasF.Select(x => x.diasTrabajadosEmpleado).ToList();
            var diasTrabajadosJoinInformes = informesF.Select(x => x.diasTrabajadosEmpleado).ToList();
            diasTrabajadosJoin.AddRange(diasTrabajadosJoinInformes);

            var diasTrabajadosAgrupados = diasTrabajadosJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.diasTrabajados = diasTrabajadosJoin.Count > 0 ? diasTrabajadosAgrupados.Select(x => x.Key).FirstOrDefault().ToString() : "";
            tendenciasAccidentabilidad.porcentajeDiasTrabajados = diasTrabajadosJoin.Count > 0 ? Math.Round((decimal)(diasTrabajadosAgrupados.Select(x => x.Count()).FirstOrDefault() * 100) / diasTrabajadosJoin.Count(), 2) : 0;

            var departamentoJoin = incidenciasF.Select(x => x.Departamentos.departamento).ToList();
            var departamentoJoinInformes = informesF.Select(x => x.Departamentos.departamento).ToList();
            departamentoJoin.AddRange(departamentoJoinInformes);

            var departamentoAgrupado = departamentoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.departamento = departamentoJoin.Count > 0 ? departamentoAgrupado.Select(x => x.Key).FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeDepartamento = departamentoJoin.Count > 0 ? Math.Round((decimal)(departamentoAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / departamentoJoin.Count(), 2) : 0;

            var lugarJoin = incidenciasF.Select(x => x.lugarAccidente).ToList();
            var lugarJoinInformes = informesF.Select(x => x.lugarAccidente).ToList();
            lugarJoin.AddRange(lugarJoinInformes);

            var lugarAgrupado = lugarJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.lugar = lugarJoin.Count > 0 ? lugarAgrupado.Select(x => x.Key).FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeLugar = lugarJoin.Count > 0 ? Math.Round((decimal)(lugarAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / lugarJoin.Count(), 2) : 0;

            var tipoContactoJoin = incidenciasF.Select(x => x.TiposContacto.tipoContacto).ToList();
            var tipoContactoJoinInformes = informesF.Select(x => x.TipoContacto.tipoContacto).ToList();
            tipoContactoJoin.AddRange(tipoContactoJoinInformes);

            var tipoContactoAgrupado = tipoContactoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.tipoContacto = tipoContactoJoin.Count > 0 ? tipoContactoAgrupado.Select(x => x.Key).FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeTipoContacto = tipoContactoJoin.Count > 0 ? Math.Round((decimal)(tipoContactoAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / tipoContactoJoin.Count(), 2) : 0;

            var capacitadoJoin = incidencias.Select(x => x.capacitadoEmpleado).ToList();
            var capacitadoJoinInformes = informes.Select(x => x.capacitadoEmpleado).ToList();
            capacitadoJoin.AddRange(capacitadoJoinInformes);

            var capacitadoAgrupado = capacitadoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.capacitado = capacitadoAgrupado.Select(x => x.Key).FirstOrDefault() == true ? "Capacitado" : "No Capacitado";
            tendenciasAccidentabilidad.porcentajeCapacitado = Math.Round((decimal)(capacitadoAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / capacitadoJoin.Count(), 2);

            var protocoloTrabajoJoin = incidenciasF.Select(x => x.ProtocolosTrabajo.protocoloTrabajo).ToList();
            var protocoloTrabajoJoinInformes = informesF.Select(x => x.ProtocolosTrabajo.protocoloTrabajo).ToList();
            protocoloTrabajoJoin.AddRange(protocoloTrabajoJoinInformes);

            var protocoloTrabajoAgrupado = protocoloTrabajoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.protocoloTrabajo = protocoloTrabajoJoin.Count > 0 ? protocoloTrabajoAgrupado.Select(x => x.Key).FirstOrDefault() : "";
            tendenciasAccidentabilidad.porcentajeProtocolo = protocoloTrabajoJoin.Count > 0 ? Math.Round((decimal)(protocoloTrabajoAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / protocoloTrabajoJoin.Count(), 2) : 0;

            var severidadJoin = incidencias.Select(x => getTipoSeveridad(x.riesgo)).ToList();
            var severidadJoinInformes = informes.Select(x => getTipoSeveridad(x.riesgo ?? default(int))).ToList();
            severidadJoin.AddRange(severidadJoinInformes);

            var severidadAgrupado = severidadJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(1);
            tendenciasAccidentabilidad.potencialSeveridad = severidadAgrupado.Select(x => x.Key).FirstOrDefault();
            tendenciasAccidentabilidad.porcentajePotencial = Math.Round((decimal)(severidadAgrupado.Select(x => x.Count()).FirstOrDefault() * 100) / severidadJoin.Count(), 2);
            #endregion

            resultado.Add(SUCCESS, true);
            resultado.Add("indicadores", tendenciasAccidentabilidad);
            //}
            //catch (Exception e)
            //{
            //    resultado.Add(SUCCESS, false);
            //    resultado.Add("EMPTY", true);
            //    resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes presentados");
            //}

            return resultado;
        }

        public Dictionary<string, object> getAccidentabilidadTop(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                busq.fechaFin = busq.fechaFin.AddHours(23).AddMinutes(59);

                var informes = _context.tblS_IncidentesInformePreliminar
                    .Where(x => x.fechaIncidente >= busq.fechaInicio && x.fechaIncidente <= busq.fechaFin)
                    .ToList()
                    .Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true)
                    .Where(x => busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true)
                    .Where(x => busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                    .ToList();


                var incidencias = _context.tblS_Incidentes
                    .Where(x => x.fechaAccidente >= busq.fechaInicio && x.fechaAccidente <= busq.fechaFin)
                    .ToList()
                    .Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true)
                    .Where(x => busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true)
                    .Where(x => busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                    .ToList();

                var informesConRIA = incidencias.Select(x => x.informe_id).ToList();
                informes = informes.Where(x => !informesConRIA.Contains(x.id)).ToList();

                var incidenciasF = incidencias
                    .Where(x =>
                        x.puestoEmpleado != "GERENTE DE SEGURIDAD Y M.A. DIV. CONSTRUCC" &&
                        x.Departamentos.departamento != "ND" &&
                        x.TurnoEmpleado.turno != "ND" &&
                        x.AgentesImplicados.agenteImplicado != "ND" &&
                        x.ExperienciaEmpleado.empleadoExperiencia != "ND" &&
                        x.AntiguedadEmpleado.antiguedadEmpleado != "ND" &&
                        x.lugarAccidente != "ND" &&
                        x.diasTrabajadosEmpleado != 0 &&
                        x.TiposContacto.tipoContacto != "ND" &&
                        x.ProtocolosTrabajo.protocoloTrabajo != "ND")
                    .ToList();

                var informesF = informes.Where(x =>
                    x.puestoEmpleado != "GERENTE DE SEGURIDAD Y M.A. DIV. CONSTRUCC" &&
                    x.Departamentos != null && x.Departamentos.departamento != "ND" &&
                    x.TurnoEmpleado != null && x.TurnoEmpleado.turno != "ND" &&
                    x.AgenteImplicado != null && x.AgenteImplicado.agenteImplicado != "ND" &&
                    x.ExperienciaEmpleado != null && x.ExperienciaEmpleado.empleadoExperiencia != "ND" &&
                    x.AntiguedadEmpleado != null && x.AntiguedadEmpleado.antiguedadEmpleado != "ND" &&
                    x.lugarAccidente != "ND" &&
                    x.diasTrabajadosEmpleado != 0 &&
                    x.TipoContacto != null && x.TipoContacto.tipoContacto != "ND" &&
                    x.ProtocolosTrabajo != null && x.ProtocolosTrabajo.protocoloTrabajo != "ND"
                ).ToList();

                informesF = informesF.Where(x => !informesConRIA.Contains(x.id)).ToList();

                if (incidenciasF.Count == 0)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                    return resultado;
                }

                var diasIncidencias = incidencias.Select(x => x.fechaAccidente.DayOfWeek).ToList();
                var diasInformes = informes.Select(x => x.fechaIncidente.DayOfWeek).ToList();

                //var listaCC = _context.tblP_CC.ToList().Where(x => (busq.arrCC != null ? busq.arrCC.Contains(x.cc) : true)).ToList();
                var listaCC = "";
                if (busq.arrGrupos != null && busq.arrGrupos.Count() > 0)
                {
                    var agrup = busq.arrGrupos[0].idAgrupacion;

                    listaCC = _context.tblS_IncidentesAgrupacionCC.FirstOrDefault(x => x.id == agrup).nomAgrupacion;
                }

                var rangoHoras = new List<rangoHorasDTO>();
                var ci = new CultureInfo("Es-Es");
                var rangoInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 00);
                var rangoFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 7, 0, 00);
                var ts = rangoFinal - rangoInicial;
                IEnumerable<int> hoursBetween = Enumerable.Range(0, (int)ts.TotalHours).Select(i => rangoInicial.AddHours(i).Hour);
                foreach (int hour in hoursBetween)
                {
                    var minutesInicial = TimeSpan.FromMinutes(0);
                    var minutes = TimeSpan.FromMinutes(59);
                    var resultInicial = TimeSpan.FromHours(hour).Add(minutesInicial);
                    var result = TimeSpan.FromHours(hour).Add(minutes);
                    var fromTimeInicial = resultInicial.ToString("hh':'mm");
                    var fromTimeString = result.ToString("hh':'mm");
                    rangoHoras.Add(new rangoHorasDTO()
                    {
                        hora = hour,
                        rango = fromTimeInicial + " - " + fromTimeString
                    });
                }
                var modal = new AccidentabilidadTopDTO() { titulo = busq.tipoAccidente.GetDescription() };
                switch (busq.tipoAccidente)
                {
                    case tipoAccientabilidadEnum.Proyecto:
                        var incidenciasCCJoin = incidencias.Select(x => x.cc).ToList();
                        var informesCCJoin = informes.Select(x => x.cc).ToList();
                        incidenciasCCJoin.AddRange(informesCCJoin);

                        var resProy = incidenciasCCJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resProy.Select(r => new AccidentabilidadDTO()
                        {
                            //descripcion = listaCC.FirstOrDefault(c => c.cc == r.Key).descripcion,
                            descripcion = listaCC,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasCCJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Puesto:
                        var incidenciasPuestoJoin = incidenciasF.Select(x => x.puestoEmpleado).ToList();
                        var informesPuestoJoin = informesF.Select(x => x.puestoEmpleado).ToList();
                        incidenciasPuestoJoin.AddRange(informesPuestoJoin);

                        var resPuesto = incidenciasPuestoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resPuesto.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasPuestoJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Departamento:
                        var incidenciasDeptoJoin = incidenciasF.Select(x => x.Departamentos.departamento).ToList();
                        var informesDeptoJoin = informesF.Select(x => x.Departamentos.departamento).ToList();
                        incidenciasDeptoJoin.AddRange(informesDeptoJoin);

                        var resDepto = incidenciasDeptoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resDepto.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasDeptoJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Severidad:
                        var incidenciasSeveridadJoin = incidencias.Select(x => getTipoSeveridad(x.riesgo)).ToList();
                        var informesSeveridadJoin = informes.Select(x => getTipoSeveridad(x.riesgo ?? default(int))).ToList();
                        incidenciasSeveridadJoin.AddRange(informesSeveridadJoin);

                        var resSeveridad = incidenciasSeveridadJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resSeveridad.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasSeveridadJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Dia:
                        var incidenciasDiaJoin = incidencias.Select(x => x.fechaAccidente.DayOfWeek).ToList();
                        var informesDiaJoin = informes.Select(x => x.fechaIncidente.DayOfWeek).ToList();
                        incidenciasDiaJoin.AddRange(informesDiaJoin);

                        var resDia = incidenciasDiaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resDia.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = ci.DateTimeFormat.GetDayName(r.Key),
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasDiaJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Hora:
                        var incidenciasHoraJoin = incidencias.Select(x => x.fechaAccidente.Hour).ToList();
                        var informesHoraJoin = informes.Select(x => x.fechaIncidente.Hour).ToList();
                        incidenciasHoraJoin.AddRange(informesHoraJoin);

                        var resHora = incidenciasHoraJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resHora.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = string.Format("{0:D2}:00 - {0:D2}:59", r.Key),
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasHoraJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Turno:
                        var incidenciasTurnoJoin = incidenciasF.Select(x => x.TurnoEmpleado.turno).ToList();
                        var informesTurnoJoin = informesF.Select(x => x.TurnoEmpleado.turno).ToList();
                        incidenciasTurnoJoin.AddRange(informesTurnoJoin);

                        var resTurno = incidenciasTurnoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(3).ToList();
                        modal.lst = resTurno.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key.ToString(),
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasTurnoJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Actividad:
                        var incidenciasActividadJoin = incidencias.Select(x => x.actividadRutinaria).ToList();
                        var informesActividadJoin = informes.Select(x => x.actividadRutinaria).ToList();
                        incidenciasActividadJoin.AddRange(informesActividadJoin);

                        var resActividad = incidenciasActividadJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(2).ToList();
                        modal.lst = resActividad.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key ? "Rutinaria" : "No Rutinaria",
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasActividadJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Tarea:
                        var incidenciasTareaJoin = incidencias.Select(x => x.trabajoPlaneado).ToList();
                        var informesTareaJoin = informes.Select(x => x.trabajoPlaneado).ToList();
                        incidenciasTareaJoin.AddRange(informesTareaJoin);

                        var resTarea = incidenciasTareaJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(2).ToList();
                        modal.lst = resTarea.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key ? "Planeada" : "No Planeada",
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasTareaJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Agente:
                        var incidenciasAgenteJoin = incidenciasF.Select(x => x.AgentesImplicados.agenteImplicado).ToList();
                        var informesAgenteJoin = informesF.Select(x => x.AgenteImplicado.agenteImplicado).ToList();
                        incidenciasAgenteJoin.AddRange(informesAgenteJoin);

                        var resAgente = incidenciasAgenteJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resAgente.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasAgenteJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Edad:
                        var resEdad = incidencias.GroupBy(x => x.edadEmpleado).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resEdad.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key.ToString(),
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidencias.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Experiencia:
                        var incidenciasExpJoin = incidenciasF.Select(x => x.ExperienciaEmpleado.empleadoExperiencia).ToList();
                        var informesExpJoin = informesF.Select(x => x.ExperienciaEmpleado.empleadoExperiencia).ToList();
                        incidenciasExpJoin.AddRange(informesExpJoin);

                        var resExp = incidenciasExpJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resExp.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasExpJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Antigüedad:
                        var incidenciasAntiJoin = incidenciasF.Select(x => x.AntiguedadEmpleado.antiguedadEmpleado).ToList();
                        var informesAntiJoin = informesF.Select(x => x.AntiguedadEmpleado.antiguedadEmpleado).ToList();
                        incidenciasAntiJoin.AddRange(informesAntiJoin);

                        var resAnti = incidenciasAntiJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resAnti.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasAntiJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.DiasTrabajados:
                        var incidenciasDiasTrabJoin = incidenciasF.Select(x => x.diasTrabajadosEmpleado).ToList();
                        var informesDiasTrabJoin = informesF.Select(x => x.diasTrabajadosEmpleado).ToList();
                        incidenciasDiasTrabJoin.AddRange(informesDiasTrabJoin);

                        var resDiasTrab = incidenciasDiasTrabJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resDiasTrab.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key.ToString(),
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasDiasTrabJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Lugar:
                        var incidenciasLugarJoin = incidenciasF.Select(x => x.lugarAccidente).ToList();
                        var informesLugarJoin = informesF.Select(x => x.lugarAccidente).ToList();
                        incidenciasLugarJoin.AddRange(informesLugarJoin);

                        var resLugar = incidenciasLugarJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resLugar.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key.ToString(),
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasLugarJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.Capacitado:
                        var incidenciasCapacitadoJoin = incidencias.Select(x => x.capacitadoEmpleado).ToList();
                        var informesCapacitadoJoin = informes.Select(x => x.capacitadoEmpleado).ToList();
                        incidenciasCapacitadoJoin.AddRange(informesCapacitadoJoin);

                        var resCapacitado = incidenciasCapacitadoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(2).ToList();
                        modal.lst = resCapacitado.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key ? "Capacitado" : "No Capacitado",
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasCapacitadoJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.ProtocoloTrabajo:
                        var incidenciasProtoJoin = incidenciasF.Select(x => x.ProtocolosTrabajo.protocoloTrabajo).ToList();
                        var informesProtoJoin = informesF.Select(x => x.ProtocolosTrabajo.protocoloTrabajo).ToList();
                        incidenciasProtoJoin.AddRange(informesProtoJoin);

                        var resProto = incidenciasProtoJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resProto.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasProtoJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    case tipoAccientabilidadEnum.TipoContacto:
                        var incidenciasTPJoin = incidenciasF.Select(x => x.TiposContacto.tipoContacto).ToList();
                        var informesTPJoin = informesF.Select(x => x.TipoContacto.tipoContacto).ToList();
                        incidenciasTPJoin.AddRange(informesTPJoin);

                        var resTP = incidenciasTPJoin.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(5).ToList();
                        modal.lst = resTP.Select(r => new AccidentabilidadDTO()
                        {
                            descripcion = r.Key,
                            cantidad = r.Count(),
                            porcentaje = Math.Round((decimal)r.Count() / incidenciasTPJoin.Count() * 100m, 2)
                        }).ToList();
                        break;
                    default:
                        break;
                }
                var esSuccess = modal.lst.Count > 0;
                resultado.Add(SUCCESS, esSuccess);
                if (esSuccess)
                {
                    resultado.Add("indicadores", modal);
                }
                else
                {
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes presentados");
            }
            return resultado;
        }

        public Dictionary<string, object> getCausasIncidencias(busqDashboardDTO busq)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                busq.fechaFin = busq.fechaFin.AddHours(23).AddMinutes(59);

                var informes = _context.tblS_IncidentesInformePreliminar.ToList().Where(x =>
                    x.fechaIncidente.Date >= busq.fechaInicio.Date &&
                    x.fechaIncidente.Date <= busq.fechaFin.Date &&
                    (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true) &&
                        //(busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true) &&
                    (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                ).ToList();

                #region Filtrar por division y lineas de negocios
                if (busq.arrDivisiones != null)
                {
                    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                    informes = informes.Join(
                        listaCentrosCostoDivision,
                        i => new { i.idEmpresa, i.idAgrupacion },
                        cd => new { cd.idEmpresa, cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }

                if (busq.arrLineasNegocio != null)
                {
                    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    informes = informes.Join(
                        listaCentrosCostoDivisionLineaNegocio,
                        i => new { i.idEmpresa, i.idAgrupacion },
                        cd => new { cd.idEmpresa, cd.idAgrupacion },
                        (i, cd) => new { i, cd }
                    ).Select(x => x.i).ToList();
                }
                #endregion

                var cantidadTotalInformes = informes.Count();

                //Se quitó que tome en cuenta los RIA
                //var incidentes = _context.tblS_Incidentes
                //    .Where(x => x.fechaAccidente >= busq.fechaInicio && x.fechaAccidente <= busq.fechaFin)
                //    .ToList()
                //    .Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idEmpresa == x.idEmpresa && y.idAgrupacion == (int)x.idAgrupacion) : true)
                //    .Where(x => busq.arrSupervisor != null ? busq.arrSupervisor.Contains(x.claveSupervisor.GetValueOrDefault()) : true)
                //    .Where(x => busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
                //    .ToList();

                //var informesConRIA = incidentes.Select(x => x.informe_id).ToList();

                //informes = informes.Where(x => !informesConRIA.Contains(x.id) && x.ProtocolosTrabajo != null).ToList();

                if (
                    //incidentes.Count == 0 && 
                    cantidadTotalInformes == 0)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                    return resultado;
                }

                causasIncidenciasDTO causasIncidencias = new causasIncidenciasDTO
                {
                    alturas = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo == "Alturas") * 100) /
                        cantidadTotalInformes, 2),
                    corteSoldadura = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Corte y Soldadura") * 100) /
                        cantidadTotalInformes, 2),
                    espaciosConfinados = Math.Round(
                        (decimal)((informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Espacios Confinados") * 100)) /
                        cantidadTotalInformes, 2),
                    excavaciones = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Excavaciones") * 100) /
                        cantidadTotalInformes, 2),
                    controlEnergias = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Control de Energias") * 100) /
                        cantidadTotalInformes, 2),
                    manejoDefensivo = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Manejo Defensivo") * 100) /
                        cantidadTotalInformes, 2),
                    manipulacionCargas = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Manipulacion de Cargas") * 100) /
                        cantidadTotalInformes, 2),
                    estabilizacionTaludez = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Estabilización  de taludes") * 100) /
                        cantidadTotalInformes, 2),
                    sustanciasQuimicas = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Sustancias Quimicas") * 100) /
                        cantidadTotalInformes, 2),
                    voladura = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Voladura") * 100) /
                        cantidadTotalInformes, 2),
                    nd = Math.Round(
                        (decimal)(informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "ND") * 100) /
                        cantidadTotalInformes, 2)
                };

                //causasIncidenciasDTO causasIncidencias = new causasIncidenciasDTO
                //{
                //    alturas = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo == "Alturas") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo == "Alturas"))) / (incidentes.Count() + informes.Count()), 2),
                //    corteSoldadura = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Corte y Soldadura") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Corte y Soldadura") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    espaciosConfinados = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Espacios Confinados") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Espacios Confinados") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    excavaciones = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Excavaciones") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Excavaciones") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    controlEnergias = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Control de Energias") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Control de Energias") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    manejoDefensivo = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Manejo Defensivo") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Manejo Defensivo") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    manipulacionCargas = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Manipulacion de Cargas") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Manipulacion de Cargas") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    estabilizacionTaludez = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Estabilización  de taludes") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Estabilización  de taludes") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    sustanciasQuimicas = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Sustancias Quimicas") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Sustancias Quimicas") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //    voladura = Math.Round((decimal)((incidentes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Voladura") * 100) + (informes.Count(x => x.ProtocolosTrabajo.protocoloTrabajo.Trim().Replace("\r", "").Replace("\n", "") == "Voladura") * 100)) / (incidentes.Count() + informes.Count()), 2),
                //};

                if (causasIncidencias != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("indicadores", causasIncidencias);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener los incidentes presentados");
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerMetasGrafica()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var metas = _context.tblS_IncidentesMetasGrafica
                    .ToList()
                    .Select(x => new
                    {
                        x.id,
                        x.nombre,
                        x.valor,
                        x.año,
                        tipoGrafica = x.tipoGrafica.GetDescription(),
                        x.colorString
                    });

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, metas);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener las metas.");
            }
            return resultado;
        }

        public Dictionary<string, object> AgregarMetaGrafica(tblS_IncidentesMetasGrafica meta)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (String.IsNullOrEmpty(meta.nombre) || meta.valor <= 0 || meta.año <= 0 || String.IsNullOrEmpty(meta.colorString))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Valores inválidos.");
                    return resultado;
                }

                var metaExistente = _context.tblS_IncidentesMetasGrafica.FirstOrDefault(x => x.nombre == meta.nombre && x.año == meta.año && x.tipoGrafica == meta.tipoGrafica);

                if (metaExistente != null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ya existe una meta con ese nombre y ese año para ese tipo de gráfica.");
                    return resultado;
                }

                meta.fechaCreacion = DateTime.Now;
                meta.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                _context.tblS_IncidentesMetasGrafica.Add(meta);
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar agregar la meta.");
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarMetaGrafica(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (id <= 0)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Identificador de la meta inválido.");
                    return resultado;
                }

                var metaExistente = _context.tblS_IncidentesMetasGrafica.FirstOrDefault(x => x.id == id);

                if (metaExistente == null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró la meta que desea eliminar.");
                    return resultado;
                }

                _context.tblS_IncidentesMetasGrafica.Remove(metaExistente);
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar eliminar la meta.");
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosLesionesPersonal(busqDashboardDTO busq)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    //8 PERSONAL
                    var lstIncidentes = ctx.tblS_IncidentesInformePreliminar.Where(e => e.agenteImplicado_id == 8).ToList();
                    var lstTipoContacto = ctx.tblS_IncidentesTipoContacto.ToList();
                    var lstPartesCuerpo = ctx.tblS_IncidentesPartesCuerpo.ToList();

                    var lstGrpIncidentesRA = lstIncidentes.GroupBy(e => e.parteCuerpo_id).Select(e => new { Key = (e.Key != 0 ? lstPartesCuerpo.FirstOrDefault(el => el.id == e.Key).parteCuerpo : "S/N"), porc = (((decimal)e.Count() / (decimal)lstIncidentes.Count()) * 100m) }).ToList();
                    var lstGrpIncidentesCI = lstIncidentes.GroupBy(e => e.tipoContacto_id).Select(e => new { Key = (e.Key != 0 ? lstTipoContacto.FirstOrDefault(el => el.id == e.Key).tipoContacto : "S/N"), porc = (((decimal)e.Count() / (decimal)lstIncidentes.Count()) * 100m) }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add("datosRegionAnatomica", lstGrpIncidentesRA.Select(e => Math.Round(e.porc)));
                    resultado.Add("datosCausasInmediatas", lstGrpIncidentesCI.Select(e => Math.Round(e.porc)));
                    resultado.Add("conceptosRegionAnatomica", lstGrpIncidentesRA.Select(e => e.Key));
                    resultado.Add("conceptosCausasInmediatas", lstGrpIncidentesCI.Select(e => e.Key));
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetDatosDañosMateriales(busqDashboardDTO busq)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                try
                {
                    //1 PERSONAL
                    var lstIncidentes = ctx.tblS_IncidentesInformePreliminar.Where(e => e.agenteImplicado_id == 1).ToList();
                    var lstTipoContacto = ctx.tblS_IncidentesTipoContacto.ToList();
                    var lstProtocolos = ctx.tblS_IncidentesProtocolosTrabajo.ToList();

                    var lstGrpIncidentesCausasInmediatas = lstIncidentes.GroupBy(e => e.tipoContacto_id).Select(e => new { Key = (e.Key != 0 ? lstTipoContacto.FirstOrDefault(el => el.id == e.Key).tipoContacto : "S/N"), porc = (((decimal)e.Count() / (decimal)lstIncidentes.Count()) * 100m) }).ToList();
                    var lstGrpIncidentesProtocoloFatalidad = lstIncidentes.GroupBy(e => e.protocoloTrabajo_id).Select(e => new { Key = (e.Key != 0 ? lstProtocolos.FirstOrDefault(el => el.id == e.Key).protocoloTrabajo : "S/N"), porc = (((decimal)e.Count() / (decimal)lstIncidentes.Count()) * 100m) }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add("datosCausasInmediatas", lstGrpIncidentesCausasInmediatas.Select(e => Math.Round(e.porc)));
                    resultado.Add("datosProtocoloFatalidada", lstGrpIncidentesProtocoloFatalidad.Select(e => Math.Round(e.porc)));
                    resultado.Add("conceptosCausasInmediatas", lstGrpIncidentesCausasInmediatas.Select(e => e.Key));
                    resultado.Add("conceptosProtocoloFatalidada", lstGrpIncidentesProtocoloFatalidad.Select(e => e.Key));
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        #endregion

        #region  Reporte Global

        public ReporteGlobalDTO ObtenerDatosReporteGlobal(TipoReporteGlobalEnum tipoReporte)
        {
            ReporteGlobalDTO reporteGlobalDTO;
            try
            {

                switch (tipoReporte)
                {
                    case TipoReporteGlobalEnum.Semanal:
                        ObtenerDatosReporteGlobalSemanal(out reporteGlobalDTO);
                        break;
                    case TipoReporteGlobalEnum.Mensual:
                        throw new NotImplementedException("Falta crear implementación para reporte mensual.");
                    default:
                        throw new NotImplementedException("Falta crear implementación para reporte mensual.");
                }


            }
            catch (Exception e)
            {
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerDatosReporteGlobal", e, AccionEnum.REPORTE, 0, null);
                reporteGlobalDTO = null;
            }

            return reporteGlobalDTO;
        }

        private void ObtenerDatosReporteGlobalSemanal(out ReporteGlobalDTO reporteGlobalDTO)
        {
            reporteGlobalDTO = new ReporteGlobalDTO { tituloPeriodo = "Reporte Semanal" };

            // Periodo de fechas
            var fechasPeriodo = ObtenerFechasSemanaPasada();
            reporteGlobalDTO.rangoPeriodo = String.Format("Del {0} al {1}",
                                            fechasPeriodo.Item1.ToShortDateString(),
                                            fechasPeriodo.Item2.ToShortDateString());

            busqDashboardDTO busqDTO = new busqDashboardDTO { arrCC = null, fechaInicio = fechasPeriodo.Item1, fechaFin = fechasPeriodo.Item2 };

            // Tasa de Incidencia Anual
            var resultTasaIncidencias = getTasaIncidencias(busqDTO, TipoCargaGraficaEnum.Actual);
            reporteGlobalDTO.tasaIncidentesAnual = resultTasaIncidencias["items"] as List<incidentesRegistrablesXmes>;

            // Incidentes por mes TRIFR
            var resultadoIncidentesMes = getIncidentesRegistrablesXmes(busqDTO, TipoCargaGraficaEnum.Actual);
            reporteGlobalDTO.incidentesPorMesTRIFR = resultadoIncidentesMes["items"] as List<incidentesRegistrablesXmes>;

            // Incidencias presentadas
            var incidentesReportados = new List<incidenciasPresentadasDTO>();

            var resultadoIncidenciasPresentadas = getIncidenciasPresentadas(busqDTO);
            var listaIncidenciasPresentadas = resultadoIncidenciasPresentadas["items"] as List<incidenciasPresentadasDTO>;
            reporteGlobalDTO.incidenciasPresentadas = listaIncidenciasPresentadas.Where(x =>
                x.cantidadFatal > 0 ||
                x.cantidadLTA > 0 ||
                x.cantidadATR > 0 ||
                x.cantidadATM > 0 ||
                x.cantidadAPA > 0 ||
                x.cantidadDAMEQ > 0 ||
                x.cantidadNM > 0 ||
                x.cantidadEI > 0 ||
                x.cantidadOI > 0 ||
                x.LesionesRegistrables > 0 ||
                x.LesionesIncapacitantes > 0 ||
                x.LesionesTotales > 0 ||
                x.LesionesDanios > 0 ||
                x.severidad > 0
            ).OrderByDescending(x => x.LesionesDanios).ToList();

            busqDTO.fechaInicio = new DateTime(DateTime.Now.Year, 1, 1);
            busqDTO.fechaFin = DateTime.Now.Date;

            var resultadoIncidenciasPresentadasGlobal = getIncidenciasPresentadas(busqDTO);
            var listaIncidenciasPresentadasGlobal = resultadoIncidenciasPresentadasGlobal["items"] as List<incidenciasPresentadasDTO>;
            reporteGlobalDTO.incidenciasPresentadasGlobal = listaIncidenciasPresentadasGlobal.Where(x =>
                x.cantidadFatal > 0 ||
                x.cantidadLTA > 0 ||
                x.cantidadATR > 0 ||
                x.cantidadATM > 0 ||
                x.cantidadAPA > 0 ||
                x.cantidadDAMEQ > 0 ||
                x.cantidadNM > 0 ||
                x.cantidadEI > 0 ||
                x.cantidadOI > 0 ||
                x.LesionesRegistrables > 0 ||
                x.LesionesIncapacitantes > 0 ||
                x.LesionesTotales > 0 ||
                x.LesionesDanios > 0 ||
                x.severidad > 0
            ).OrderByDescending(x => x.LesionesRegistrables).ToList();
        }

        private Tuple<DateTime, DateTime> ObtenerFechasSemanaPasada()
        {
            DayOfWeek weekStart = DayOfWeek.Monday;
            DateTime startingDate = DateTime.Today;

            while (startingDate.DayOfWeek != weekStart)
            {
                startingDate = startingDate.AddDays(-1);
            }

            DateTime previousWeekStart = startingDate.AddDays(-7);
            DateTime previousWeekEnd = startingDate.AddDays(-1).AddHours(23).AddMinutes(59);

            return Tuple.Create(previousWeekStart, previousWeekEnd);
        }

        public Dictionary<string, object> EnviarCorreoReporteGlobal(List<Byte[]> pdf)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                //var correoUsuario = new List<string> { "oscar.valencia@construplan.com.mx" };
                var correoUsuario = new List<string> { "comitedirectivo.sst@construplan.com.mx", "corporativo@construplan.com.mx" };
                if (vSesiones.sesionEmpresaActual == 6)
                {
                    correoUsuario = new List<string> {
                        "comitedirectivo.ssoma-peru@construplan.com.mx",
                        "comitedirectivo.ssoma-peru@construplan.com.pe",
                        "corporativo@construplan.com.mx"
                    };
                }

#if DEBUG
                correoUsuario = new List<string> { "rene.olea@construplan.com.mx" };
#endif

                string cuerpoCorreo = @"<html>
                                        <head>
                                            <style>
                                                p {
                                                    font-family: arial, sans-serif;
                                                }
                                            </style>
                                        </head>
                                        <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                            <div class=WordSection1>
                                                <p class=MsoNormal>
                                                    Hola, <o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Se envía reporte semanal de seguridad como archivo adjunto.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";

                var correoEnviado =
                    //GlobalUtils.sendEmailAdjuntoInMemory2("Reporte Semanal de Seguridad", cuerpoCorreo, correoUsuario, pdf, "Reporte Semanal");
                    GlobalUtils.sendEmailAdjuntoInMemory2Seguridad(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Reporte Semanal de Seguridad"), cuerpoCorreo, correoUsuario, pdf, "Reporte Semanal");
                resultado.Add(SUCCESS, correoEnviado);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, "IndicadoresSeguridadController", "EnviarCorreoReporteGlobal", e, AccionEnum.CORREO, 0, null);
            }
            return resultado;
        }
        #endregion

        #region Helper methods
        private List<Tuple<DateTime, string>> ObtenerListaMesesTotales(TipoCargaGraficaEnum tipoCarga)
        {
            string[] months = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
            var año = DateTime.Now.Year;
            int contadorMes = 1;

            var mesesTotales = new List<Tuple<DateTime, string>>();

            switch (tipoCarga)
            {
                case TipoCargaGraficaEnum.Actual:
                    mesesTotales = months.Select(x =>
                    {
                        var texto = String.Format("{0} {1}", x, año.ToString().Substring(2));
                        var fecha = new DateTime(año, contadorMes++, 1);
                        return Tuple.Create(fecha, texto);
                    }).ToList();
                    break;
                case TipoCargaGraficaEnum.Anterior:
                    mesesTotales = months.Select(x =>
                    {
                        var texto = String.Format("{0} {1}", x, (año - 1).ToString().Substring(2));
                        var fecha = new DateTime((año - 1), contadorMes++, 1);
                        return Tuple.Create(fecha, texto);
                    }).ToList();
                    break;
                case TipoCargaGraficaEnum.AnteriorYActual:

                    string añoPasadoText = (año - 1).ToString().Substring(2);
                    for (int i = 0; i < months.Length; i++)
                    {
                        var texto = String.Format("{0} {1}", months[i], añoPasadoText);
                        var fecha = new DateTime((año - 1), contadorMes++, 1);
                        mesesTotales.Add(Tuple.Create(fecha, texto));
                    }

                    contadorMes = 1;

                    string añoActualText = año.ToString().Substring(2);
                    for (int i = 0; i < months.Length; i++)
                    {
                        var texto = String.Format("{0} {1}", months[i], añoActualText);
                        var fecha = new DateTime(año, contadorMes++, 1);
                        mesesTotales.Add(Tuple.Create(fecha, texto));
                    }
                    break;
                default:
                    throw new NotImplementedException("Tipo de carga no definido.");
            }

            return mesesTotales;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        private decimal CalcularPotencialSeveridad(decimal cantidad, decimal total)
        {
            return cantidad > 0 ? Math.Round((cantidad / total) * 100, 2) : cantidad;
        }

        private bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            try
            {
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
                File.WriteAllBytes(ruta, data);
            }
            catch (Exception e)
            {
                return false;
            }

            return File.Exists(ruta);
        }

        /// <summary>
        /// Verifica si el nombre contiene caracteres inválidos para archivos.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo a verificar.</param>
        /// <returns>Verdadero si el archivo contiene caracteres inválidos.</returns>
        private bool EsNombreArchivoInvalido(string nombreArchivo)
        {
            string invalidFileNameRegex = @"[^a-zA-Z0-9áéíóúüñÑ_.\- ]+";
            return Regex.Match(nombreArchivo, invalidFileNameRegex, RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Verifica si existe una carpeta física en el servidor.
        /// </summary>
        /// <param name="path">Ruta de la carpeta física.</param>
        /// <param name="crear">Bandera que al activarla, si no existe la carpeta, la creará.</param>
        /// <returns></returns>
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


        private bool EsExtensionInvalidaVisor(string nombreArchivo)
        {
            var extension = Path.GetExtension(nombreArchivo).ToUpper();

            var extensionesValidasVisor = GlobalUtils.ObtenerExtensionesValidasVisor();

            return !extensionesValidasVisor.Contains(extension);
        }

        private IEnumerable<tblS_IncidentesInformePreliminar> ObtenerInformesFechasCC(busqDashboardDTO busq)
        {
            var informes = _context.tblS_IncidentesInformePreliminar.Where(x =>
                DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList().Where(x =>
                (x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue)) &&
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).ToList();

            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                informes = informes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                informes = informes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idEmpresa, i.idAgrupacion },
                    cd => new { cd.idEmpresa, cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            return informes;
        }

        private IEnumerable<tblS_Incidentes> ObtenerIncidentesFechasCC(busqDashboardDTO busq)
        {
            var incidentes = _context.tblS_Incidentes.Where(x =>
                DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true) &&
                (busq.arrDepto != null ? busq.arrDepto.Contains(x.departamento_id) : true)
            ).ToList();

            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                incidentes = incidentes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                incidentes = incidentes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            return incidentes;
        }

        private List<DateTime> GetListaFechaAccidentesTRIFR(busqDashboardDTO busq)
        {
            var incidentes = _context.tblS_Incidentes.Where(x =>
                DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin) &&
                x.TiposAccidente.clasificacion.tipoEvento.tipoEvento == "Registrable"
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true)
            ).Select(x => new { x.fechaAccidente, abreviatura = x.TiposAccidente.clasificacion.abreviatura, idAgrupacion = x.idAgrupacion }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                incidentes = incidentes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                incidentes = incidentes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var informes = _context.tblS_IncidentesInformePreliminar.Where(x =>
                DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin) &&
                (x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue)) &&
                x.TiposAccidente.clasificacion.tipoEvento.tipoEvento == "Registrable"
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true)
            ).Select(x => new { fechaAccidente = x.fechaIncidente, abreviatura = x.TiposAccidente.clasificacion.abreviatura, idAgrupacion = x.idAgrupacion }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                informes = informes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                informes = informes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            incidentes.AddRange(informes);

            // Se separan los fatales para multiplicarlos
            var incidentesTotales = incidentes.Where(x => x.abreviatura != "Fatal").ToList();
            var incidentesFatales = incidentes.Where(x => x.abreviatura == "Fatal").ToList();

            foreach (var incidenteFatal in incidentesFatales)
            {
                for (int i = 0; i < PonderacionFatalitdad; i++)
                {
                    incidentesTotales.Add(incidenteFatal);
                }
            }

            return incidentesTotales.Select(x => x.fechaAccidente).ToList();
        }

        private List<DateTime> GetListaFechaAccidentesTIFR(busqDashboardDTO busq)
        {
            var incidentes = _context.tblS_Incidentes
                .Where(x => x.fechaAccidente >= busq.fechaInicio && x.fechaAccidente <= busq.fechaFin)
                .ToList()
                .Where(x => x.TiposAccidente.clasificacion.abreviatura == "PD" ? x.subclasificacionID == 1 : true)
                .Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
                .Select(x => new { x.fechaAccidente, abreviatura = x.TiposAccidente.clasificacion.abreviatura, idAgrupacion = x.idAgrupacion })
                .ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                incidentes = incidentes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                incidentes = incidentes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var informes = _context.tblS_IncidentesInformePreliminar
                .Where(x => x.fechaIncidente >= busq.fechaInicio && x.fechaIncidente <= busq.fechaFin)
                .ToList()
                .Where(x => x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue))
                .Where(x => x.TiposAccidente.clasificacion.abreviatura == "PD" ? x.subclasificacionID == 1 : true)
                .Where(x => busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion) : true)
                .Select(x => new { fechaAccidente = x.fechaIncidente, abreviatura = x.TiposAccidente.clasificacion.abreviatura, idAgrupacion = x.idAgrupacion })
                .ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                informes = informes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                informes = informes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            incidentes.AddRange(informes);

            // Se separan los fatales para multiplicarlos
            var incidentesTotales = incidentes.Where(x => x.abreviatura != "Fatal").ToList();
            var incidentesFatales = incidentes.Where(x => x.abreviatura == "Fatal").ToList();
            foreach (var incidenteFatal in incidentesFatales)
            {
                for (int i = 0; i < PonderacionFatalitdad; i++)
                {
                    incidentesTotales.Add(incidenteFatal);
                }
            }
            return incidentesTotales.Select(x => x.fechaAccidente).ToList();
        }

        private List<DateTime> GetListaFechaAccidentesTPDFR(busqDashboardDTO busq)
        {
            var incidentes = _context.tblS_Incidentes.Where(x =>
                DbFunctions.TruncateTime(x.fechaAccidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaAccidente) <= DbFunctions.TruncateTime(busq.fechaFin)
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true)
            ).Select(x => new { x.fechaAccidente, abreviatura = x.TiposAccidente.clasificacion.abreviatura, x.idAgrupacion }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                incidentes = incidentes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                incidentes = incidentes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            var informes = _context.tblS_IncidentesInformePreliminar.Where(x =>
                DbFunctions.TruncateTime(x.fechaIncidente) >= DbFunctions.TruncateTime(busq.fechaInicio) && DbFunctions.TruncateTime(x.fechaIncidente) <= DbFunctions.TruncateTime(busq.fechaFin) &&
                (x.aplicaRIA == false || (x.aplicaRIA && x.terminado == false && x.tipoAccidente_id.HasValue))
            ).ToList().Where(x =>
                (busq.arrGrupos != null ? busq.arrGrupos.Any(y => y.idAgrupacion == (int)x.idAgrupacion && y.idEmpresa == x.idEmpresa) : true)
            ).Select(x => new { fechaAccidente = x.fechaIncidente, abreviatura = x.TiposAccidente.clasificacion.abreviatura, x.idAgrupacion }).ToList();

            #region Filtrar por division y lineas de negocios
            if (busq.arrDivisiones != null)
            {
                var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrDivisiones.Contains(x.division)).ToList();

                informes = informes.Join(
                    listaCentrosCostoDivision,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }

            if (busq.arrLineasNegocio != null)
            {
                var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && busq.arrLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                informes = informes.Join(
                    listaCentrosCostoDivisionLineaNegocio,
                    i => new { i.idAgrupacion },
                    cd => new { cd.idAgrupacion },
                    (i, cd) => new { i, cd }
                ).Select(x => x.i).ToList();
            }
            #endregion

            incidentes.AddRange(informes);

            // Se separan los fatales para multiplicarlos
            var incidentesTotales = incidentes.Where(x => x.abreviatura != "Fatal").ToList();
            var incidentesFatales = incidentes.Where(x => x.abreviatura == "Fatal").ToList();

            foreach (var incidenteFatal in incidentesFatales)
            {
                for (int i = 0; i < PonderacionFatalitdad; i++)
                {
                    incidentesTotales.Add(incidenteFatal);
                }
            }

            return incidentesTotales.Select(x => x.fechaAccidente).ToList();
        }

        private Tuple<DateTime, DateTime> ObtenerRangoFechasTipoCarga(TipoCargaGraficaEnum tipoCarga)
        {
            int añoActual = DateTime.Now.Year;

            var fechaInicio = new DateTime();
            var fechaFin = new DateTime();

            switch (tipoCarga)
            {
                case TipoCargaGraficaEnum.Actual:
                    fechaInicio = new DateTime(añoActual, 1, 1);
                    fechaFin = new DateTime(añoActual, 12, 31).AddHours(23).AddMinutes(59);
                    break;
                case TipoCargaGraficaEnum.Anterior:
                    fechaInicio = new DateTime(añoActual - 1, 1, 1);
                    fechaFin = new DateTime(añoActual - 1, 12, 31).AddHours(23).AddMinutes(59);
                    break;
                case TipoCargaGraficaEnum.AnteriorYActual:
                    fechaInicio = new DateTime(añoActual - 1, 1, 1);
                    fechaFin = new DateTime(añoActual, 12, 31).AddHours(23).AddMinutes(59);
                    break;
                default:
                    throw new NotImplementedException("Tipo de carga no definido.");
            }

            return Tuple.Create(fechaInicio, fechaFin);
        }

        private static int PonderarEventosFatales(IEnumerable<string> abreviaturas, IEnumerable<InformesRIADTO> listaIncidentesPorCC)
        {
            var cantidadIncidentesNoFatales = listaIncidentesPorCC.Count(y => abreviaturas.Contains(y.abreviaturaClasificacion) && y.abreviaturaClasificacion != "Fatal");

            var cantidadIncidentesFatales = listaIncidentesPorCC.Count(y => y.abreviaturaClasificacion == "Fatal") * PonderacionFatalitdad;

            return cantidadIncidentesNoFatales + cantidadIncidentesFatales;
        }
        private Decimal ObtenerTotalIncidencias(int cantidadIncidentes, decimal cantidadTotal)
        {
            if (cantidadIncidentes == 0)
            {
                return 0;
            }

            decimal formula = 200000;

            return Math.Round((cantidadIncidentes * formula) / (cantidadTotal > 0 ? cantidadTotal : 1), 2);
        }
        private decimal ObtenerHHTPeriodo(List<tblS_IncidentesInformacionColaboradores> cantidadHH, DateTime fechaInicio, DateTime fechaFinal)
        {
            decimal totalHH = 0;

            foreach (var hhFiltro in cantidadHH)
            {
                var fechaI = hhFiltro.fechaInicio;
                var fechaF = hhFiltro.fechaFin;
                var cantidadDias = (decimal)(fechaF - fechaI).TotalDays + 1;
                var hhTotal = hhFiltro.horasHombre;
                var horasPorDia = hhTotal / cantidadDias;

                totalHH += EachDay(fechaI, fechaF)
                    .Where(day => day >= fechaInicio && day <= fechaFinal)
                    .Sum(x => horasPorDia);
            }

            return Math.Round(totalHH, 2);
        }
        private List<string> addCCArrendadoraCP(List<string> arrCC)
        {

            if (arrCC != null)
            {
                string[] ccTMC = new string[] { "100", "101", "002", "114", "004" };
                string[] ccCerroPelon = new string[] { "121", "123" };
                string[] ccColorada = new string[] { "103" };
                string[] ccYaqui = new string[] { "105" };
                string[] ccJales = new string[] { "126" };
                string[] ccAcueducto = new string[] { "125" };



                if (arrCC.Contains("1010"))
                {
                    arrCC.AddRange(ccTMC);
                }

                if (arrCC.Contains("165"))
                {
                    arrCC.AddRange(ccCerroPelon);
                }

                if (arrCC.Contains("146"))
                {
                    arrCC.AddRange(ccColorada);
                }

                if (arrCC.Contains("155"))
                {
                    arrCC.AddRange(ccYaqui);
                }

                if (arrCC.Contains("169"))
                {
                    arrCC.AddRange(ccJales);
                }

                if (arrCC.Contains("227"))
                {
                    arrCC.AddRange(ccAcueducto);
                }
            }


            return arrCC;
        }

        private string getTipoSeveridad(int nivel)
        {
            string severidad = "";

            switch (nivel)
            {
                case 1:
                    severidad = "Menor";
                    break;
                case 2:
                    severidad = "Menor";
                    break;
                case 3:
                    severidad = "Moderado";
                    break;
                case 4:
                    severidad = "Moderado";
                    break;
                case 5:
                    severidad = "Mayor";
                    break;
                case 6:
                    severidad = "Mayor";
                    break;
                case 9:
                    severidad = "Catastrofico";
                    break;
                default:
                    break;
            }

            return severidad;
        }

        #endregion

        #region CALCULO DE HORAS TRABAJADAS - HORAS HOMBRE5
        public Dictionary<string, object> GetDatos(CalculosHorasHombreDTO objSelected)
        {
            try
            {
                #region LISTAS PARA ALMACENAR LAS HORAS A MOSTRAR EN EL FRONT-END
                List<int> lstHorasLaboradasSemanalMantenimiento = new List<int>();
                List<int> lstHorasLaboradasSemanalOperativo = new List<int>();
                List<int> lstHorasLaboradasSemanalAdministrativo = new List<int>();

                List<int> lstHorasLaboradasQuincenalMantenimiento = new List<int>();
                List<int> lstHorasLaboradasQuincenalOperativo = new List<int>();
                List<int> lstHorasLaboradasQuincenalAdministrativo = new List<int>();
                #endregion

                string _cc = objSelected.cc;
                int _idEmpresa = objSelected.idEmpresa;
                int _idAgrupacion = objSelected.idAgrupacion;
                string arrFechas = objSelected.fechas != null ? arrFechas = objSelected.fechas : arrFechas = string.Empty;
                if (objSelected != null && _idAgrupacion != null && _idEmpresa > -1 && !string.IsNullOrEmpty(arrFechas))
                {
                    #region SE VERIFICA SI EL REGISTO SELECCIONADO, SEA UNA AGRUPACIÓN O UNA EMPRESA
                    bool esAgrupacion = false;
                    esAgrupacion = _idEmpresa == 0 ? esAgrupacion = true : esAgrupacion = false;
                    #endregion

                    #region EN CASO DE SER UNA AGRUPACIÓN, SE OBTIENE LOS CC DE DICHA AGRUPACIÓN
                    List<string> lstCentroCostosCP = new List<string>();
                    List<string> lstCentroCostosArr = new List<string>();
                    List<string> lstCentroCostosCol = new List<string>();
                    List<string> lstCentroCostosPer = new List<string>();
                    List<int> lstEmpresasCP = new List<int>();
                    List<int> lstEmpresasArr = new List<int>();
                    List<int> lstEmpresasCol = new List<int>();
                    List<int> lstEmpresasPer = new List<int>();
                    if (esAgrupacion)
                    {
                        // SE OBTIENEN LOS CC Y LA EMPRESA QUE PERTENECEN, DE LA AGRUPACIÓN SELECCIONADA.
                        var data = _context.tblS_IncidentesAgrupacionCCDet.Where(x => x.idAgrupacionCC == _idAgrupacion && x.esActivo).ToList();
                        if (data.Count() > 0)
                        {
                            foreach (var item in data)
                            {
                                if (item.idEmpresa == (int)EmpresaEnum.Construplan)
                                {
                                    lstCentroCostosCP.Add(item.cc.Trim());
                                    lstEmpresasCP.Add(item.idEmpresa);
                                }
                                else if (item.idEmpresa == (int)EmpresaEnum.Arrendadora)
                                {
                                    lstCentroCostosArr.Add(item.cc.Trim());
                                    lstEmpresasArr.Add(item.idEmpresa);
                                }
                                else if (item.idEmpresa == (int)EmpresaEnum.Peru)
                                {
                                    lstCentroCostosCol.Add(item.cc.Trim());
                                    lstEmpresasCol.Add(item.idEmpresa);
                                }
                                else if (item.idEmpresa == (int)EmpresaEnum.Colombia)
                                {
                                    lstCentroCostosPer.Add(item.cc.Trim());
                                    lstEmpresasPer.Add(item.idEmpresa);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_idEmpresa == (int)EmpresaEnum.Construplan)
                        {
                            lstCentroCostosCP.Add(_cc);
                            lstEmpresasCP.Add(_idEmpresa);
                        }
                        else if (_idEmpresa == (int)EmpresaEnum.Arrendadora)
                        {
                            lstCentroCostosArr.Add(_cc);
                            lstEmpresasArr.Add(_idEmpresa);
                        }
                    }
                    #endregion

                    #region SE VERIFICA QUE EL CC EXISTA EN EL CATALOGO DE HORAS HOMBRE, EN EL CASO DE AGRUPACIONES, SE VERIFICA QUE TODOS LOS CC QUE CONTENGAN, EXISTAN EN EL CATALOGO DE HORAS HOMBRE
                    var lstCatHorasHombre = _context.tblS_CatHorasHombre.Where(x => x.esActivo).ToList();
                    for (int i = 0; i < lstCentroCostosCP.Count(); i++)
                    {
                        string cc = lstCentroCostosCP[i];
                        int idEmpresa = lstEmpresasCP[i];
                        if (!string.IsNullOrEmpty(cc) && idEmpresa > 0)
                        {
                            int existeRegistro = lstCatHorasHombre.Where(x => x.cc == cc && x.idEmpresa == idEmpresa && x.esActivo).Count();
                            if (existeRegistro <= 0)
                                throw new Exception("El CC " + cc + " no se encuentra en el Catálogo de horas hombre.");
                        }
                    }

                    for (int i = 0; i < lstCentroCostosArr.Count(); i++)
                    {
                        string cc = lstCentroCostosArr[i];
                        int idEmpresa = lstEmpresasArr[i];
                        if (!string.IsNullOrEmpty(cc) && idEmpresa > 0)
                        {
                            int existeRegistro = lstCatHorasHombre.Where(x => x.cc == cc && x.idEmpresa == idEmpresa && x.esActivo).Count();
                            if (existeRegistro <= 0)
                                throw new Exception("El CC " + cc + " no se encuentra en el Catálogo de horas hombre.");
                        }
                    }
                    #endregion

                    #region SE OBTIENE LOS ESTATUS DE LOS DÍAS QUE SE LABORARON TIPO NOMINA 1 (SEMANAL).
                    for (int i = 0; i < 7; i++)
                    {
                        lstHorasLaboradasSemanalMantenimiento.Add(0);
                        lstHorasLaboradasSemanalOperativo.Add(0);
                        lstHorasLaboradasSemanalAdministrativo.Add(0);
                    }
                    #endregion

                    #region SE OBTIENE LOS ESTATUS DE LOS DÍAS QUE SE LABORARON TIPO NOMINA 4 (QUINCENAL).
                    for (int i = 0; i < 7; i++)
                    {
                        lstHorasLaboradasQuincenalMantenimiento.Add(0);
                        lstHorasLaboradasQuincenalOperativo.Add(0);
                        lstHorasLaboradasQuincenalAdministrativo.Add(0);
                    }
                    #endregion

                    #region SE OBTIENE UNA LISTA DE PUESTOS DE LOS EMPLEADOS EN BASE AL CC SELECCIONADO O AGRUPACIÓN SELECCIONADO
                    //string strQueryPuestos = string.Empty;
                    //strQueryPuestos += "SELECT clave_empleado, puesto, clave_depto FROM DBA.sn_empleados ";
                    //strQueryPuestos += "WHERE cc_contable IN ({0}) ";
                    ////strQueryPuestos += "WHERE cc_contable IN ({0}) AND ";
                    ////strQueryPuestos += "estatus_empleado = 'A' ";
                    //strQueryPuestos += "ORDER BY nombre";
                    //var odbc = new OdbcConsultaDTO();

                    string ccs = getStringInlineArray(lstCentroCostosCP);

                    #region SE OBTIENE EL LISTADO DE PUESTOS DE CP
                    List<string> lstCentroCostosCPEnKontrol = new List<string>();
                    List<PuestosDTO> lstPuestosCP = new List<PuestosDTO>();
                    if (lstCentroCostosCP.Count() > 0)
                    {
                        for (int i = 0; i < lstCentroCostosCP.Count(); i++)
                        {
                            string ccEnKontrol = lstCentroCostosCP[i];
                            lstCentroCostosCPEnKontrol.Add(ccEnKontrol);
                        }
                        //odbc.consulta = String.Format(strQueryPuestos, string.Join(", ", lstCentroCostosCPEnKontrol));
                        //lstPuestosCP = _contextEnkontrol.Select<PuestosDTO>(EnkontrolEnum.CplanRh, odbc);

                        lstPuestosCP = _context.Select<PuestosDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = string.Format(@"SELECT clave_empleado, puesto, clave_depto FROM tblRH_EK_Empleados 
                                        WHERE cc_contable IN ({0}) 
                                        ORDER BY nombre", string.Join(", ", lstCentroCostosCP.Select(x => "'" + x + "'")))
                        });
                    }
                    #endregion

                    #region SE OBTIENE EL LISTADO DE PUESTOS DE ARR
                    List<string> lstCentroCostosArrEnKontrol = new List<string>();
                    List<PuestosDTO> lstPuestosArr = new List<PuestosDTO>();
                    if (lstCentroCostosArr.Count() > 0)
                    {
                        for (int i = 0; i < lstCentroCostosArr.Count(); i++)
                        {
                            string ccEnKontrol = "'" + lstCentroCostosArr[i] + "'";
                            lstCentroCostosArrEnKontrol.Add(ccEnKontrol);
                        }
                        //odbc.consulta = String.Format(strQueryPuestos, string.Join(", ", lstCentroCostosArrEnKontrol));
                        //lstPuestosArr = _contextEnkontrol.Select<PuestosDTO>(EnkontrolEnum.ArrenRh, odbc);

                        lstPuestosArr = _context.Select<PuestosDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = string.Format(@"SELECT clave_empleado, puesto, clave_depto FROM tblRH_EK_Empleados 
                                        WHERE cc_contable IN ({0}) 
                                        ORDER BY nombre", string.Join(", ", lstCentroCostosCP.Select(x => "'" + x + "'")))
                        });
                    }
                    #endregion

                    #endregion

                    #region SE OBTIENE EL PERIODO EN BASE A LA FECHA DE INICIO. COMENTADO
                    List<string> lstFechas = new List<string>();
                    string[] fechaSplit = arrFechas.Split('|');
                    for (int i = 0; i < 7; i++)
                    {
                        lstFechas.Add(fechaSplit[i]);
                    }

                    List<int> lstDias = new List<int>();
                    List<int> lstMeses = new List<int>();
                    List<int> lstAnios = new List<int>();
                    for (int i = 0; i < 7; i++)
                    {
                        lstDias.Add(Convert.ToInt32(lstFechas[i].Substring(0, 2)));
                        lstMeses.Add(Convert.ToInt32(lstFechas[i].Substring(3, 2)));
                        lstAnios.Add(Convert.ToInt32(lstFechas[i].Substring(6, 4)));
                    }
                    #endregion

                    #region SE OBTIENE EL LISTADO DE EMPLEADOS CON RELACIÓN AL DEPARTAMENTO QUE PERTENECEN, LOS DIAS QUE TRABAJAN Y LA CANTIDAD DE HORAS.

                    #region SE OBTIENE LISTADO DE EMPLEADOS DE CP
                    var lstEmpleadosCP = (from t1 in _context.tblS_CatDepartamentos.ToList()
                                          join t2 in lstPuestosCP on t1.clave_depto equals t2.clave_depto
                                          join t3 in _context.tblS_CatHorasHombre on t2.clave_depto equals t3.clave_depto
                                          join t4 in _context.tblP_RolesGrupoTrabajo on t3.idGrupo equals t4.id
                                          where t1.idEmpresa == (int)EmpresaEnum.Construplan
                                          select new { t1.clave_depto, t1.idAreaOperativa, t2.clave_empleado, t3.idGrupo, t3.fechaInicio, t3.horas, t4.cantDiasLaborales, t1.idEmpresa }).ToList();
                    #endregion

                    #region SE OBTIENE LISTADO DE EMPLEADOS DE ARR
                    var lstEmpleadosArr = (from t1 in _context.tblS_CatDepartamentos.ToList()
                                           join t2 in lstPuestosArr on t1.clave_depto equals t2.clave_depto
                                           join t3 in _context.tblS_CatHorasHombre on t2.clave_depto equals t3.clave_depto
                                           join t4 in _context.tblP_RolesGrupoTrabajo on t3.idGrupo equals t4.id
                                           where t1.idEmpresa == (int)EmpresaEnum.Arrendadora
                                           select new { t1.clave_depto, t1.idAreaOperativa, t2.clave_empleado, t3.idGrupo, t3.fechaInicio, t3.horas, t4.cantDiasLaborales, t1.idEmpresa }).ToList();
                    #endregion

                    if (lstEmpleadosCP.Count() <= 0 && lstEmpleadosArr.Count() <= 0)
                        throw new Exception("No se encontro personal");

                    #endregion

                    #region SE OBTIENEN TODOS LOS PERIODOS DEL AÑO ACTUAL.
                    string strQueryGetPeriodos = string.Empty;
                    strQueryGetPeriodos += "SELECT periodo, fecha_inicial, fecha_final, tipo_nomina FROM DBA.sn_periodos ";
                    //strQueryGetPeriodos += "WHERE year = {0} AND tipo_nomina = 4 ";
                    strQueryGetPeriodos += "WHERE year = {0} ";
                    strQueryGetPeriodos += "ORDER BY fecha_inicial";

                    //odbc = new OdbcConsultaDTO();
                    //odbc.consulta = String.Format(strQueryGetPeriodos, DateTime.Now.Year);
                    //List<PeriodosDTO> lstPeriodos = _contextEnkontrol.Select<PeriodosDTO>(EnkontrolEnum.CplanRh, odbc);

                    List<PeriodosDTO> lstPeriodos = _context.Select<PeriodosDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT periodo, fecha_inicial, fecha_final, tipo_nomina FROM tblRH_EK_Periodos 
                                    WHERE year = @year 
                                    ORDER BY fecha_inicial",
                        parametros = new { DateTime.Now.Year }
                    });
                    #endregion

                    #region SE OBTIENE LOS PERIODOS CON TIPO NOMINA 4
                    DateTime fechaInicio = Convert.ToDateTime(lstFechas[0]);
                    var objPeriodosQuincena = lstPeriodos.Where(x => x.fecha_inicial <= fechaInicio && x.fecha_final >= fechaInicio && x.tipo_nomina == 4).ToList();
                    List<int> lstPeriodosQuincena = objPeriodosQuincena.Select(x => x.periodo).ToList();
                    #endregion

                    #region SE OBTIENE LOS PERIODOS
                    List<int> lstPeriodosSemanalEK = new List<int>();
                    #region TEST
                    DateTime fecha1 = Convert.ToDateTime(lstFechas[0]);
                    DateTime fecha2 = Convert.ToDateTime(lstFechas[1]);
                    DateTime fecha3 = Convert.ToDateTime(lstFechas[2]);
                    DateTime fecha4 = Convert.ToDateTime(lstFechas[3]);
                    DateTime fecha5 = Convert.ToDateTime(lstFechas[4]);
                    DateTime fecha6 = Convert.ToDateTime(lstFechas[5]);
                    DateTime fecha7 = Convert.ToDateTime(lstFechas[6]);

                    #region TIPO NOMINA SEMANAL
                    var objFecha1 = lstPeriodos.Where(x => x.fecha_inicial <= fecha1 && x.fecha_final >= fecha1 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal1 = objFecha1.Select(x => x.periodo).FirstOrDefault();

                    var objFecha2 = lstPeriodos.Where(x => x.fecha_inicial <= fecha2 && x.fecha_final >= fecha2 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal2 = objFecha2.Select(x => x.periodo).FirstOrDefault();

                    var objFecha3 = lstPeriodos.Where(x => x.fecha_inicial <= fecha3 && x.fecha_final >= fecha3 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal3 = objFecha3.Select(x => x.periodo).FirstOrDefault();

                    var objFecha4 = lstPeriodos.Where(x => x.fecha_inicial <= fecha4 && x.fecha_final >= fecha4 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal4 = objFecha4.Select(x => x.periodo).FirstOrDefault();

                    var objFecha5 = lstPeriodos.Where(x => x.fecha_inicial <= fecha5 && x.fecha_final >= fecha5 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal5 = objFecha5.Select(x => x.periodo).FirstOrDefault();

                    var objFecha6 = lstPeriodos.Where(x => x.fecha_inicial <= fecha6 && x.fecha_final >= fecha6 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal6 = objFecha6.Select(x => x.periodo).FirstOrDefault();

                    var objFecha7 = lstPeriodos.Where(x => x.fecha_inicial <= fecha7 && x.fecha_final >= fecha7 && x.tipo_nomina == 1).ToList();
                    int periodoSemanal7 = objFecha7.Select(x => x.periodo).FirstOrDefault();

                    lstPeriodosSemanalEK.Add(periodoSemanal1);
                    for (int i = 0; i < 1; i++)
                    {
                        if (periodoSemanal1 != periodoSemanal2) { lstPeriodosSemanalEK.Add(periodoSemanal2); break; }
                        if (periodoSemanal1 != periodoSemanal3) { lstPeriodosSemanalEK.Add(periodoSemanal3); break; }
                        if (periodoSemanal1 != periodoSemanal4) { lstPeriodosSemanalEK.Add(periodoSemanal4); break; }
                        if (periodoSemanal1 != periodoSemanal5) { lstPeriodosSemanalEK.Add(periodoSemanal5); break; }
                        if (periodoSemanal1 != periodoSemanal6) { lstPeriodosSemanalEK.Add(periodoSemanal6); break; }
                        if (periodoSemanal1 != periodoSemanal7) { lstPeriodosSemanalEK.Add(periodoSemanal7); break; }
                    }
                    #endregion

                    #region TIPO NOMINA QUINCENAL
                    var objQuincenalFecha1 = lstPeriodos.Where(x => x.fecha_inicial <= fecha1 && x.fecha_final >= fecha1 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal1 = objQuincenalFecha1.Select(x => x.periodo).FirstOrDefault();

                    var objQuincenalFecha2 = lstPeriodos.Where(x => x.fecha_inicial <= fecha2 && x.fecha_final >= fecha2 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal2 = objQuincenalFecha2.Select(x => x.periodo).FirstOrDefault();

                    var objQuincenalFecha3 = lstPeriodos.Where(x => x.fecha_inicial <= fecha3 && x.fecha_final >= fecha3 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal3 = objQuincenalFecha3.Select(x => x.periodo).FirstOrDefault();

                    var objQuincenalFecha4 = lstPeriodos.Where(x => x.fecha_inicial <= fecha4 && x.fecha_final >= fecha4 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal4 = objQuincenalFecha4.Select(x => x.periodo).FirstOrDefault();

                    var objQuincenalFecha5 = lstPeriodos.Where(x => x.fecha_inicial <= fecha5 && x.fecha_final >= fecha5 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal5 = objQuincenalFecha5.Select(x => x.periodo).FirstOrDefault();

                    var objQuincenalFecha6 = lstPeriodos.Where(x => x.fecha_inicial <= fecha6 && x.fecha_final >= fecha6 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal6 = objQuincenalFecha6.Select(x => x.periodo).FirstOrDefault();

                    var objQuincenalFecha7 = lstPeriodos.Where(x => x.fecha_inicial <= fecha7 && x.fecha_final >= fecha7 && x.tipo_nomina == 4).ToList();
                    int periodoQuincenal7 = objQuincenalFecha7.Select(x => x.periodo).FirstOrDefault();

                    List<int> lstPeriodosQuincenalEK = new List<int>();
                    lstPeriodosQuincenalEK.Add(periodoQuincenal1);
                    for (int i = 0; i < 1; i++)
                    {
                        if (periodoQuincenal1 != periodoQuincenal2) { lstPeriodosQuincenalEK.Add(periodoQuincenal2); break; }
                        if (periodoQuincenal1 != periodoQuincenal3) { lstPeriodosQuincenalEK.Add(periodoQuincenal3); break; }
                        if (periodoQuincenal1 != periodoQuincenal4) { lstPeriodosQuincenalEK.Add(periodoQuincenal4); break; }
                        if (periodoQuincenal1 != periodoQuincenal5) { lstPeriodosQuincenalEK.Add(periodoQuincenal5); break; }
                        if (periodoQuincenal1 != periodoQuincenal6) { lstPeriodosQuincenalEK.Add(periodoQuincenal6); break; }
                        if (periodoQuincenal1 != periodoQuincenal7) { lstPeriodosQuincenalEK.Add(periodoQuincenal7); break; }
                    }
                    #endregion
                    #endregion

                    fechaInicio = Convert.ToDateTime(lstFechas[0]);
                    var objPeriodosSemanal = lstPeriodos.Where(x => x.fecha_inicial <= fechaInicio && x.fecha_final >= fechaInicio && x.tipo_nomina == 1).ToList();
                    List<int> lstPeriodosSemanal = objPeriodosSemanal.Select(x => x.periodo).ToList();
                    #endregion

                    List<int> lstPeriodosCalculo = new List<int>();
                    //for (int i = 0; i < lstPeriodosEK.Count(); i++)
                    //{
                    //    lstPeriodosCalculo.Add(lstPeriodosEK[i]);
                    //}

                    #region COMENTADO
                    //v1
                    //DateTime primerFechaModal = Convert.ToDateTime(lstFechas[0]);
                    //var objPeriodos = lstPeriodos.Where(x => x.fecha_inicial <= primerFechaModal && x.fecha_final >= primerFechaModal).ToList();
                    //int periodo = objPeriodos.Select(x => x.periodo).FirstOrDefault();

                    //v2

                    //#region SE OBTIENE LOS PERIODOS QUINCENALES
                    //#endregion
                    //List<int> lstPeriodosCalculo = new List<int>();
                    //var objPeriodo = lstPeriodos.ToList();
                    //DateTime fechaInicial = new DateTime(2000, 01, 01);
                    //DateTime fechaFinal = Convert.ToDateTime(lstFechas[5]);
                    //for (int i = 0; i < lstFechas.Count(); i++)
                    //{
                    //    if (lstDias[i] == 15 || lstDias[i] == 30)
                    //    {
                    //        fechaInicial = Convert.ToDateTime(lstFechas[i]);
                    //        objPeriodo = lstPeriodos.Where(x => (x.fecha_inicial <= fechaInicial && x.fecha_final >= fechaFinal) || (x.fecha_inicial == fechaInicial) || (x.fecha_final == fechaInicial)).ToList();
                    //        //lstPeriodosCalculo.Add(objPeriodo.Select(x => x.periodo).FirstOrDefault());
                    //        for (int w = 0; w < objPeriodo.Count(); w++)
                    //        {
                    //            lstPeriodos.Add(objPeriodo[w]);
                    //        }
                    //        break;
                    //    }
                    //}

                    //fechaInicial = Convert.ToDateTime(lstFechas[0]);
                    //objPeriodo = lstPeriodos.Where(x => x.fecha_inicial <= fechaInicial && x.fecha_final >= fechaInicial).ToList();
                    //int _periodo = objPeriodo.Where(x => !lstPeriodosCalculo.Contains(x.periodo)).Select(x => x.periodo).FirstOrDefault();
                    //lstPeriodosCalculo.Add(_periodo);
                    //#endregion
                    #endregion

                    #region SE OBTIENE id_incidencia EN BASE AL PERIODO CONSULTADO.

                    #region SE OBTIENE LA INCIDENCIA DE CP
                    List<IncidenciasDTO> lstIncidenciasCP = new List<IncidenciasDTO>();
                    List<IncidenciasDTO> lstIncidenciasSemanalCP = new List<IncidenciasDTO>();
                    List<IncidenciasDTO> lstIncidenciasQuincenalCP = new List<IncidenciasDTO>();

                    //string inlineCC = getStringInlineArray(lstCentroCostosCPEnKontrol);
                    //string inlineWeeks = getStringInlineArray(lstPeriodosSemanalEK);
                    //string inlineFortnight = getStringInlineArray(lstPeriodosSemanalEK);

                    if (lstCentroCostosCPEnKontrol.Count() > 0)
                    {
                        string strQueryIncidencia = string.Empty;

                        #region INCIDENCIAS SEMANAL
                        //strQueryIncidencia += "SELECT id_incidencia, tipo_nomina FROM DBA.sn_incidencias_empl ";
                        //strQueryIncidencia += "WHERE estatus = 'A' AND anio = {0} AND cc IN ({1}) AND periodo IN ({2}) AND tipo_nomina = 1 ";
                        //strQueryIncidencia += "ORDER BY periodo";
                        //odbc = new OdbcConsultaDTO();
                        //odbc.consulta = String.Format(strQueryIncidencia, DateTime.Now.Year, string.Join(", ", lstCentroCostosCPEnKontrol), string.Join(",", lstPeriodosSemanalEK.ToList()));
                        //lstIncidenciasSemanalCP = _contextEnkontrol.Select<IncidenciasDTO>(EnkontrolEnum.CplanRh, odbc);



                        lstIncidenciasSemanalCP = _context.Select<IncidenciasDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT id AS id_incidencia, tipo_nomina FROM tblRH_BN_Incidencia 
                                            WHERE estatus = 'A' AND anio = @anio AND cc IN @inlineCC AND periodo IN @inlineWeeks AND tipo_nomina = 1 
                                            ORDER BY periodo",
                            parametros = new { anio = DateTime.Now.Year, inlineCC = lstCentroCostosCPEnKontrol, inlineWeeks = lstPeriodosSemanalEK }
                        });

                        for (int i = 0; i < lstIncidenciasSemanalCP.Count(); i++)
                        {
                            IncidenciasDTO objIncidencia = new IncidenciasDTO();
                            objIncidencia.id_incidencia = lstIncidenciasSemanalCP[i].id_incidencia;
                            objIncidencia.tipo_nomina = lstIncidenciasSemanalCP[i].tipo_nomina;
                            lstIncidenciasCP.Add(objIncidencia);
                        }
                        #endregion

                        #region INCIDENCIAS QUINCENAL
                        //strQueryIncidencia = string.Empty;
                        //strQueryIncidencia += "SELECT id_incidencia, tipo_nomina FROM DBA.sn_incidencias_empl ";
                        //strQueryIncidencia += "WHERE estatus = 'A' AND anio = {0} AND cc IN ({1}) AND periodo IN ({2}) AND tipo_nomina = 4 ";
                        //strQueryIncidencia += "ORDER BY periodo";
                        //odbc = new OdbcConsultaDTO();
                        //odbc.consulta = String.Format(strQueryIncidencia, DateTime.Now.Year, string.Join(", ", lstCentroCostosCPEnKontrol), string.Join(",", lstPeriodosQuincenalEK.ToList()));
                        //lstIncidenciasQuincenalCP = _contextEnkontrol.Select<IncidenciasDTO>(EnkontrolEnum.CplanRh, odbc);

                        lstIncidenciasQuincenalCP = _context.Select<IncidenciasDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT id AS id_incidencia, tipo_nomina FROM tblRH_BN_Incidencia 
                                            WHERE estatus = 'A' AND anio = @anio AND cc IN @inlineCC AND periodo IN @inlineFortnight AND tipo_nomina = 4
                                            ORDER BY periodo",
                            parametros = new { anio = DateTime.Now.Year, inlineCC = lstCentroCostosCPEnKontrol, inlineFortnight = lstPeriodosSemanalEK }
                        });

                        for (int i = 0; i < lstIncidenciasQuincenalCP.Count(); i++)
                        {
                            IncidenciasDTO objIncidencia = new IncidenciasDTO();
                            objIncidencia.id_incidencia = lstIncidenciasQuincenalCP[i].id_incidencia;
                            objIncidencia.tipo_nomina = lstIncidenciasQuincenalCP[i].tipo_nomina;
                            lstIncidenciasCP.Add(objIncidencia);
                        }
                        #endregion
                    }
                    #endregion

                    #region SE OBTIENE LA INCIDENCIA DE ARR
                    List<IncidenciasDTO> lstIncidenciasArr = new List<IncidenciasDTO>();
                    List<IncidenciasDTO> lstIncidenciasSemanalArr = new List<IncidenciasDTO>();
                    List<IncidenciasDTO> lstIncidenciasQuincenalArr = new List<IncidenciasDTO>();
                    if (lstCentroCostosArrEnKontrol.Count() > 0)
                    {
                        string strQueryIncidencia = string.Empty;

                        #region INCIDENCIAS SEMANAL
                        //strQueryIncidencia += "SELECT id_incidencia, tipo_nomina FROM DBA.sn_incidencias_empl ";
                        //strQueryIncidencia += "WHERE estatus = 'A' AND anio = {0} AND cc IN ({1}) AND periodo IN ({2}) AND tipo_nomina = 1 ";
                        //strQueryIncidencia += "ORDER BY periodo";
                        //odbc = new OdbcConsultaDTO();
                        //odbc.consulta = String.Format(strQueryIncidencia, DateTime.Now.Year, string.Join(", ", lstCentroCostosArrEnKontrol), string.Join(",", lstPeriodosSemanalEK.ToList()));
                        //lstIncidenciasSemanalArr = _contextEnkontrol.Select<IncidenciasDTO>(EnkontrolEnum.ArrenRh, odbc);

                        lstIncidenciasSemanalArr = _context.Select<IncidenciasDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT id AS id_incidencia, tipo_nomina FROM tblRH_BN_Incidencia 
                                            WHERE estatus = 'A' AND anio = @anio AND cc IN @inlineCC AND periodo IN @inlineWeeks AND tipo_nomina = 1 
                                            ORDER BY periodo",
                            parametros = new { anio = DateTime.Now.Year, inlineCC = lstCentroCostosCPEnKontrol, inlineWeeks = lstPeriodosSemanalEK }
                        });

                        for (int i = 0; i < lstIncidenciasSemanalArr.Count(); i++)
                        {
                            IncidenciasDTO objIncidencia = new IncidenciasDTO();
                            objIncidencia.id_incidencia = lstIncidenciasSemanalArr[i].id_incidencia;
                            objIncidencia.tipo_nomina = lstIncidenciasSemanalArr[i].tipo_nomina;
                            lstIncidenciasArr.Add(objIncidencia);
                        }
                        #endregion

                        #region INCIDENCIAS QUINCENAL
                        //strQueryIncidencia = string.Empty;
                        //strQueryIncidencia += "SELECT id_incidencia, tipo_nomina FROM DBA.sn_incidencias_empl ";
                        //strQueryIncidencia += "WHERE estatus = 'A' AND anio = {0} AND cc IN ({1}) AND periodo IN ({2}) AND tipo_nomina = 4 ";
                        //strQueryIncidencia += "ORDER BY periodo";
                        //odbc = new OdbcConsultaDTO();
                        //odbc.consulta = String.Format(strQueryIncidencia, DateTime.Now.Year, string.Join(", ", lstCentroCostosArrEnKontrol), string.Join(",", lstPeriodosQuincenalEK.ToList()));
                        //lstIncidenciasQuincenalArr = _contextEnkontrol.Select<IncidenciasDTO>(EnkontrolEnum.ArrenRh, odbc);

                        lstIncidenciasQuincenalArr = _context.Select<IncidenciasDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT id AS id_incidencia, tipo_nomina FROM tblRH_BN_Incidencia 
                                            WHERE estatus = 'A' AND anio = @anio AND cc IN @inlineCC AND periodo IN @inlineFortnight AND tipo_nomina = 4
                                            ORDER BY periodo",
                            parametros = new { anio = DateTime.Now.Year, inlineCC = lstCentroCostosCPEnKontrol, inlineFortnight = lstPeriodosSemanalEK }
                        });

                        for (int i = 0; i < lstIncidenciasQuincenalArr.Count(); i++)
                        {
                            IncidenciasDTO objIncidencia = new IncidenciasDTO();
                            objIncidencia.id_incidencia = lstIncidenciasQuincenalArr[i].id_incidencia;
                            objIncidencia.tipo_nomina = lstIncidenciasQuincenalArr[i].tipo_nomina;
                            lstIncidenciasArr.Add(objIncidencia);
                        }
                        #endregion
                    }
                    #endregion

                    #endregion

                    #region SE OBTIENE EL DETALLE DE LAS INCIDENCIAS EN BASE AL id_incidencia QUE SE CONSULTO
                    //string strQueryDetIncidencias = string.Empty;
                    //strQueryDetIncidencias += "SELECT id_incidencia, clave_empleado, dia1, dia2, dia3, dia4, dia5, dia6, dia7, dia8, dia9, dia10, dia11, dia12, dia13, dia14, dia15 FROM DBA.sn_incidencias_empl_det ";
                    //strQueryDetIncidencias += "WHERE id_incidencia IN ({0}) ";
                    //strQueryDetIncidencias += "ORDER BY clave_empleado";
                    //odbc = new OdbcConsultaDTO();

                    #region SE OBTIENE LOS DETALLES DE LAS INCIDENCIAS DE CP
                    List<decimal> lstIncidenciasDecimalCP = new List<decimal>();
                    List<IncidenciasDetDTO> lstDetIncidenciasCP = new List<IncidenciasDetDTO>();
                    lstIncidenciasDecimalCP = lstIncidenciasCP.Select(x => x.id_incidencia).ToList();
                    string inlineIncidenciasCP = getStringInlineArray(lstIncidenciasDecimalCP);

                    if (lstIncidenciasDecimalCP.Count() > 0)
                    {
                        //odbc.consulta = String.Format(strQueryDetIncidencias, string.Join(", ", lstIncidenciasDecimalCP));
                        //lstDetIncidenciasCP = _contextEnkontrol.Select<IncidenciasDetDTO>(EnkontrolEnum.CplanRh, odbc);

                        lstDetIncidenciasCP = _context.Select<IncidenciasDetDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT incidenciaID AS id_incidencia, clave_empleado, dia1, dia2, dia3, dia4, dia5, dia6, dia7, dia8, dia9, dia10, dia11, dia12, dia13, dia14, dia15 FROM tblRH_BN_Incidencia_det 
                                        WHERE incidenciaID IN @inlineIncidenciasCP
                                        ORDER BY clave_empleado",
                            parametros = new { inlineIncidenciasCP = lstIncidenciasDecimalCP }
                        });
                    }
                    #endregion

                    #region SE OBTIENE LOS DETALLES DE LAS INCIDENCIAS DE ARR
                    List<decimal> lstIncidenciasDecimalArr = new List<decimal>();
                    List<IncidenciasDetDTO> lstDetIncidenciasArr = new List<IncidenciasDetDTO>();
                    lstIncidenciasDecimalArr = lstIncidenciasArr.Select(x => x.id_incidencia).ToList();

                    string inlineIncidenciasArr = getStringInlineArray(lstIncidenciasDecimalArr);

                    if (lstIncidenciasDecimalArr.Count() > 0)
                    {
                        //odbc.consulta = String.Format(strQueryDetIncidencias, string.Join(", ", lstIncidenciasDecimalArr));
                        //lstDetIncidenciasArr = _contextEnkontrol.Select<IncidenciasDetDTO>(EnkontrolEnum.ArrenRh, odbc);
                        lstDetIncidenciasArr = _context.Select<IncidenciasDetDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT incidenciaID AS id_incidencia, clave_empleado, dia1, dia2, dia3, dia4, dia5, dia6, dia7, dia8, dia9, dia10, dia11, dia12, dia13, dia14, dia15 FROM tblRH_BN_Incidencia_det 
                                        WHERE incidenciaID IN @inlineIncidenciasArr
                                        ORDER BY clave_empleado",
                            parametros = new { inlineIncidenciasCP = lstIncidenciasDecimalArr }
                        });
                    }
                    #endregion

                    #endregion

                    #region CALCULOS DE EMPLEADOS CON TIPO NOMINA 1 (SEMANAL)
                    int tipoNomina = Convert.ToInt32(CatTipoNominas.semanal);
                    int esIncidenciaSemanalCP = lstIncidenciasCP.Where(x => x.tipo_nomina == tipoNomina).Count();
                    int esIncidenciaSemanalArr = lstIncidenciasArr.Where(x => x.tipo_nomina == tipoNomina).Count();
                    if (esIncidenciaSemanalCP > 0 || esIncidenciaSemanalArr > 0)
                    {
                        #region CALCULOS CC DE CP
                        if (lstCentroCostosCP.Count() > 0 && lstDetIncidenciasCP.Count() > 0)
                        {
                            #region LISTADO DE EMPLEADOS CON TIPO NOMINA 1 (SEMANAL)
                            tipoNomina = 0;
                            tipoNomina = Convert.ToInt32(CatTipoNominas.semanal);
                            decimal idIncidenciaSemanal = lstIncidenciasCP.Where(x => x.tipo_nomina == tipoNomina).Select(x => x.id_incidencia).FirstOrDefault();
                            List<IncidenciasDetDTO> lstEmpleadosNominaSemanal = lstDetIncidenciasCP.Where(x => x.id_incidencia == idIncidenciaSemanal).ToList();
                            #endregion

                            #region SE DESGLOSA LOS EMPLEADOS AL ÁREA OPERATIVA QUE PERTENECEN A SUS LISTAS CORRESPONDIENTES
                            int idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.mantenimiento);
                            var lstEmpleadosMantenimientoCP = lstEmpleadosCP.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Construplan).Select(x => x.clave_empleado).ToList();

                            idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.operativo);
                            var lstEmpleadosOperativoCP = lstEmpleadosCP.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Construplan).Select(x => x.clave_empleado).ToList();

                            idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.administrativo);
                            var lstEmpleadosAdministrativoCP = lstEmpleadosCP.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Construplan).Select(x => x.clave_empleado).ToList();
                            #endregion

                            #region CALCULOS DE HORAS DE MANTENIMIENTO
                            for (int i = 0; i < lstEmpleadosMantenimientoCP.Count(); i++)
                            {
                                int claveEmpleado = lstEmpleadosMantenimientoCP[i];

                                int esLaboradoDia1 = lstEmpleadosNominaSemanal.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasSemanalMantenimiento[0] += esLaboradoDia1 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia2 = lstEmpleadosNominaSemanal.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasSemanalMantenimiento[1] += esLaboradoDia2 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia3 = lstEmpleadosNominaSemanal.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasSemanalMantenimiento[2] += esLaboradoDia3 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia4 = lstEmpleadosNominaSemanal.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasSemanalMantenimiento[3] += esLaboradoDia4 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia5 = lstEmpleadosNominaSemanal.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasSemanalMantenimiento[4] += esLaboradoDia5 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia6 = lstEmpleadosNominaSemanal.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasSemanalMantenimiento[5] += esLaboradoDia6 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia7 = lstEmpleadosNominaSemanal.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasSemanalMantenimiento[6] += esLaboradoDia7 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                            #endregion

                            #region CALCULOS DE HORAS DE OPERATIVO
                            for (int i = 0; i < lstEmpleadosOperativoCP.Count(); i++)
                            {
                                int claveEmpleado = lstEmpleadosOperativoCP[i];

                                int esLaboradoDia1 = lstEmpleadosNominaSemanal.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasSemanalOperativo[0] += esLaboradoDia1 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia2 = lstEmpleadosNominaSemanal.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasSemanalOperativo[1] += esLaboradoDia2 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia3 = lstEmpleadosNominaSemanal.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasSemanalOperativo[2] += esLaboradoDia3 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia4 = lstEmpleadosNominaSemanal.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasSemanalOperativo[3] += esLaboradoDia4 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia5 = lstEmpleadosNominaSemanal.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasSemanalOperativo[4] += esLaboradoDia5 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia6 = lstEmpleadosNominaSemanal.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasSemanalOperativo[5] += esLaboradoDia6 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia7 = lstEmpleadosNominaSemanal.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasSemanalOperativo[6] += esLaboradoDia7 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                            #endregion

                            #region CALCULOS DE HORAS DE ADMINISTRATIVO
                            for (int i = 0; i < lstEmpleadosAdministrativoCP.Count(); i++)
                            {
                                int claveEmpleado = lstEmpleadosAdministrativoCP[i];

                                int esLaboradoDia1 = lstEmpleadosNominaSemanal.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasSemanalAdministrativo[0] += esLaboradoDia1 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia2 = lstEmpleadosNominaSemanal.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasSemanalAdministrativo[1] += esLaboradoDia2 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia3 = lstEmpleadosNominaSemanal.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasSemanalAdministrativo[2] += esLaboradoDia3 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia4 = lstEmpleadosNominaSemanal.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasSemanalAdministrativo[3] += esLaboradoDia4 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia5 = lstEmpleadosNominaSemanal.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasSemanalAdministrativo[4] += esLaboradoDia5 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia6 = lstEmpleadosNominaSemanal.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasSemanalAdministrativo[5] += esLaboradoDia6 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia7 = lstEmpleadosNominaSemanal.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasSemanalAdministrativo[6] += esLaboradoDia7 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                            #endregion
                        }
                        #endregion

                        #region CALCULOS CC DE ARR
                        if (lstCentroCostosArr.Count() > 0 && lstDetIncidenciasArr.Count() > 0)
                        {
                            #region LISTADO DE EMPLEADOS CON TIPO NOMINA 1 (SEMANAL)
                            tipoNomina = 0;
                            tipoNomina = Convert.ToInt32(CatTipoNominas.semanal);
                            decimal idIncidenciaSemanal = lstIncidenciasArr.Where(x => x.tipo_nomina == tipoNomina).Select(x => x.id_incidencia).FirstOrDefault();
                            List<IncidenciasDetDTO> lstEmpleadosNominaSemanal = lstDetIncidenciasArr.Where(x => x.id_incidencia == idIncidenciaSemanal).ToList();
                            #endregion

                            #region SE DESGLOSA LOS EMPLEADOS AL ÁREA OPERATIVA QUE PERTENECEN A SUS LISTAS CORRESPONDIENTES
                            int idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.mantenimiento);
                            var lstEmpleadosMantenimientoArr = lstEmpleadosArr.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Arrendadora).Select(x => x.clave_empleado).ToList();

                            idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.operativo);
                            var lstEmpleadosOperativoArr = lstEmpleadosArr.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Arrendadora).Select(x => x.clave_empleado).ToList();

                            idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.administrativo);
                            var lstEmpleadosAdministrativoArr = lstEmpleadosArr.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Arrendadora).Select(x => x.clave_empleado).ToList();
                            #endregion

                            #region CALCULOS DE HORAS DE MANTENIMIENTO
                            for (int i = 0; i < lstEmpleadosMantenimientoArr.Count(); i++)
                            {
                                int claveEmpleado = lstEmpleadosMantenimientoArr[i];

                                int esLaboradoDia1 = lstEmpleadosNominaSemanal.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasSemanalMantenimiento[0] += esLaboradoDia1 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia2 = lstEmpleadosNominaSemanal.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasSemanalMantenimiento[1] += esLaboradoDia2 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia3 = lstEmpleadosNominaSemanal.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasSemanalMantenimiento[2] += esLaboradoDia3 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia4 = lstEmpleadosNominaSemanal.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasSemanalMantenimiento[3] += esLaboradoDia4 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia5 = lstEmpleadosNominaSemanal.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasSemanalMantenimiento[4] += esLaboradoDia5 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia6 = lstEmpleadosNominaSemanal.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasSemanalMantenimiento[5] += esLaboradoDia6 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia7 = lstEmpleadosNominaSemanal.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasSemanalMantenimiento[6] += esLaboradoDia7 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                            #endregion

                            #region CALCULOS DE HORAS DE OPERATIVO
                            for (int i = 0; i < lstEmpleadosOperativoArr.Count(); i++)
                            {
                                int claveEmpleado = lstEmpleadosOperativoArr[i];

                                int esLaboradoDia1 = lstEmpleadosNominaSemanal.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasSemanalOperativo[0] += esLaboradoDia1 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia2 = lstEmpleadosNominaSemanal.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasSemanalOperativo[1] += esLaboradoDia2 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia3 = lstEmpleadosNominaSemanal.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasSemanalOperativo[2] += esLaboradoDia3 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia4 = lstEmpleadosNominaSemanal.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasSemanalOperativo[3] += esLaboradoDia4 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia5 = lstEmpleadosNominaSemanal.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasSemanalOperativo[4] += esLaboradoDia5 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia6 = lstEmpleadosNominaSemanal.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasSemanalOperativo[5] += esLaboradoDia6 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia7 = lstEmpleadosNominaSemanal.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasSemanalOperativo[6] += esLaboradoDia7 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                            #endregion

                            #region CALCULOS DE HORAS DE ADMINISTRATIVO
                            for (int i = 0; i < lstEmpleadosAdministrativoArr.Count(); i++)
                            {
                                int claveEmpleado = lstEmpleadosAdministrativoArr[i];

                                int esLaboradoDia1 = lstEmpleadosNominaSemanal.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasSemanalAdministrativo[0] += esLaboradoDia1 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia2 = lstEmpleadosNominaSemanal.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasSemanalAdministrativo[1] += esLaboradoDia2 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia3 = lstEmpleadosNominaSemanal.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasSemanalAdministrativo[2] += esLaboradoDia3 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia4 = lstEmpleadosNominaSemanal.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasSemanalAdministrativo[3] += esLaboradoDia4 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia5 = lstEmpleadosNominaSemanal.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasSemanalAdministrativo[4] += esLaboradoDia5 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia6 = lstEmpleadosNominaSemanal.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasSemanalAdministrativo[5] += esLaboradoDia6 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();

                                int esLaboradoDia7 = lstEmpleadosNominaSemanal.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasSemanalAdministrativo[6] += esLaboradoDia7 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    #region CALCULOS DE EMPLEADOS CON TIPO NOMINA 4 (QUINCENAL)
                    tipoNomina = Convert.ToInt32(CatTipoNominas.quincenal);
                    int esIncidenciaQuincenalCP = lstIncidenciasCP.Where(x => x.tipo_nomina == tipoNomina).Count();
                    int esIncidenciaQuincenalArr = lstIncidenciasArr.Where(x => x.tipo_nomina == tipoNomina).Count();
                    if (esIncidenciaQuincenalCP > 0 || esIncidenciaQuincenalArr > 0)
                    {
                        #region CALCULOS CC DE CP

                        #region LISTADO DE EMPLEADOS CON TIPO NOMINA 4 (QUINCENAL)
                        tipoNomina = 0;
                        tipoNomina = Convert.ToInt32(CatTipoNominas.quincenal);
                        decimal idIncidenciaQuincenalCP = lstIncidenciasCP.Where(x => x.tipo_nomina == tipoNomina).Select(x => x.id_incidencia).FirstOrDefault();
                        List<IncidenciasDetDTO> lstEmpleadosNominaQuincenalCP = lstDetIncidenciasCP.Where(x => x.id_incidencia == idIncidenciaQuincenalCP).ToList();
                        #endregion

                        #region SE DESGLOSA LOS EMPLEADOS AL ÁREA OPERATIVA QUE PERTENECEN A SUS LISTAS CORRESPONDIENTES.
                        int idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.mantenimiento);
                        var lstEmpleadosMantenimientoCP = lstEmpleadosCP.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Construplan).Select(x => x.clave_empleado).ToList();

                        idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.operativo);
                        var lstEmpleadosOperativoCP = lstEmpleadosCP.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Construplan).Select(x => x.clave_empleado).ToList();

                        idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.administrativo);
                        var lstEmpleadosAdministrativoCP = lstEmpleadosCP.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Construplan).Select(x => x.clave_empleado).ToList();
                        #endregion

                        #region SE OBTIENE LA DIFERENCIA DE DÍAS EN BASE A LA FECHA INICIAL MOSTRADA EN EL MODAL Y LA FECHA INICIAL DE CUANDO INICIA EL PERIODO DE LA QUINCENA.
                        //int difDias = 0;
                        //DateTime fechaInicialQuincena = Convert.ToDateTime(lstFechas[0]);
                        //TimeSpan restaDifDias = Convert.ToDateTime(fechaInicialQuincena) - Convert.ToDateTime(lstFechas[0]);
                        //difDias = restaDifDias.Days;
                        int difDias = 0;
                        DateTime fechaInicialQuincena = objPeriodosQuincena.Select(s => s.fecha_inicial).FirstOrDefault();
                        DateTime fechaInicioModal = Convert.ToDateTime(lstFechas[0]);
                        difDias = (fechaInicioModal - fechaInicialQuincena).Days;
                        #endregion

                        #region SE OBTIENE LOS DÍAS A CONSULTAR PARA LOS EMPLEADOS CON NOMINA 4 (QUINCENAL).
                        string strQueryDiasQuincena = string.Empty;
                        int diaQuincena = 0;
                        diaQuincena = difDias > 0 ? 1 : difDias++;

                        List<int> lstDiasQuincenaConsultar = new List<int>();
                        while (diaQuincena <= 6)
                        {
                            diaQuincena++;
                            lstDiasQuincenaConsultar.Add(diaQuincena);
                        }
                        #endregion

                        #region CALCULOS DE HORAS DE MANTENIMIENTO
                        for (int i = 0; i < lstEmpleadosMantenimientoCP.Count(); i++)
                        {
                            int claveEmpleado = lstEmpleadosMantenimientoCP[i];
                            int existeRegistroIncidenciaEmpleado = lstEmpleadosNominaQuincenalCP.Where(w => w.clave_empleado == claveEmpleado).Count();
                            if (existeRegistroIncidenciaEmpleado > 0)
                            {
                                bool esDia1 = lstDiasQuincenaConsultar.Any(x => x == 1);
                                if (esDia1)
                                {
                                    int esLaboradoDia1 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                    lstHorasLaboradasQuincenalMantenimiento[0] += esLaboradoDia1 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia2 = lstDiasQuincenaConsultar.Any(x => x == 2);
                                if (esDia2)
                                {
                                    int esLaboradoDia2 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                    lstHorasLaboradasQuincenalMantenimiento[1] += esLaboradoDia2 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia3 = lstDiasQuincenaConsultar.Any(x => x == 3);
                                if (esDia3)
                                {
                                    int esLaboradoDia3 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                    lstHorasLaboradasQuincenalMantenimiento[2] += esLaboradoDia3 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia4 = lstDiasQuincenaConsultar.Any(x => x == 4);
                                if (esDia4)
                                {
                                    int esLaboradoDia4 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                    lstHorasLaboradasQuincenalMantenimiento[3] += esLaboradoDia4 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia5 = lstDiasQuincenaConsultar.Any(x => x == 5);
                                if (esDia5)
                                {
                                    int esLaboradoDia5 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                    lstHorasLaboradasQuincenalMantenimiento[4] += esLaboradoDia5 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia6 = lstDiasQuincenaConsultar.Any(x => x == 6);
                                if (esDia6)
                                {
                                    int esLaboradoDia6 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                    lstHorasLaboradasQuincenalMantenimiento[5] += esLaboradoDia6 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia7 = lstDiasQuincenaConsultar.Any(x => x == 7);
                                if (esDia7)
                                {
                                    int esLaboradoDia7 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                    lstHorasLaboradasQuincenalMantenimiento[6] += esLaboradoDia7 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia8 = lstDiasQuincenaConsultar.Any(x => x == 8);
                                if (esDia8)
                                {
                                    int esLaboradoDia8 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia8 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia8);
                                    lstHorasLaboradasQuincenalMantenimiento[7] += esLaboradoDia8 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia9 = lstDiasQuincenaConsultar.Any(x => x == 9);
                                if (esDia9)
                                {
                                    int esLaboradoDia9 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia9 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia9);
                                    lstHorasLaboradasQuincenalMantenimiento[8] += esLaboradoDia9 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia10 = lstDiasQuincenaConsultar.Any(x => x == 10);
                                if (esDia10)
                                {
                                    int esLaboradoDia10 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia10 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia10);
                                    lstHorasLaboradasQuincenalMantenimiento[9] += esLaboradoDia10 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia11 = lstDiasQuincenaConsultar.Any(x => x == 11);
                                if (esDia11)
                                {
                                    int esLaboradoDia11 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia11 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia11);
                                    lstHorasLaboradasQuincenalMantenimiento[10] += esLaboradoDia11 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia12 = lstDiasQuincenaConsultar.Any(x => x == 12);
                                if (esDia12)
                                {
                                    int esLaboradoDia12 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia12 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia12);
                                    lstHorasLaboradasQuincenalMantenimiento[11] += esLaboradoDia12 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia13 = lstDiasQuincenaConsultar.Any(x => x == 13);
                                if (esDia13)
                                {
                                    int esLaboradoDia13 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia13 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia13);
                                    lstHorasLaboradasQuincenalMantenimiento[11] += esLaboradoDia13 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia14 = lstDiasQuincenaConsultar.Any(x => x == 14);
                                if (esDia14)
                                {
                                    int esLaboradoDia14 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia14 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia14);
                                    lstHorasLaboradasQuincenalMantenimiento[13] += esLaboradoDia14 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia15 = lstDiasQuincenaConsultar.Any(x => x == 15);
                                if (esDia15)
                                {
                                    int esLaboradoDia15 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia15 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia15);
                                    lstHorasLaboradasQuincenalMantenimiento[14] += esLaboradoDia15 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }
                            }
                        }
                        #endregion

                        #region CALCULOS DE HORAS DE OPERATIVO
                        for (int i = 0; i < lstEmpleadosOperativoCP.Count(); i++)
                        {
                            int claveEmpleado = lstEmpleadosOperativoCP[i];
                            int existeRegistroIncidenciaEmpleado = lstEmpleadosNominaQuincenalCP.Where(w => w.clave_empleado == claveEmpleado).Count();
                            if (existeRegistroIncidenciaEmpleado > 0)
                            {
                                bool esDia1 = lstDiasQuincenaConsultar.Any(x => x == 1);
                                if (esDia1)
                                {
                                    int esLaboradoDia1 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                    lstHorasLaboradasQuincenalOperativo[0] += esLaboradoDia1 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia2 = lstDiasQuincenaConsultar.Any(x => x == 2);
                                if (esDia2)
                                {
                                    int esLaboradoDia2 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                    lstHorasLaboradasQuincenalOperativo[1] += esLaboradoDia2 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia3 = lstDiasQuincenaConsultar.Any(x => x == 3);
                                if (esDia3)
                                {
                                    int esLaboradoDia3 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                    lstHorasLaboradasQuincenalOperativo[2] += esLaboradoDia3 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia4 = lstDiasQuincenaConsultar.Any(x => x == 4);
                                if (esDia4)
                                {
                                    int esLaboradoDia4 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                    lstHorasLaboradasQuincenalOperativo[3] += esLaboradoDia4 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia5 = lstDiasQuincenaConsultar.Any(x => x == 5);
                                if (esDia5)
                                {
                                    int esLaboradoDia5 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                    lstHorasLaboradasQuincenalOperativo[4] += esLaboradoDia5 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia6 = lstDiasQuincenaConsultar.Any(x => x == 6);
                                if (esDia6)
                                {
                                    int esLaboradoDia6 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                    lstHorasLaboradasQuincenalOperativo[5] += esLaboradoDia6 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia7 = lstDiasQuincenaConsultar.Any(x => x == 7);
                                if (esDia7)
                                {
                                    int esLaboradoDia7 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                    lstHorasLaboradasQuincenalOperativo[6] += esLaboradoDia7 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia8 = lstDiasQuincenaConsultar.Any(x => x == 8);
                                if (esDia8)
                                {
                                    int esLaboradoDia8 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia8 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia8);
                                    lstHorasLaboradasQuincenalOperativo[7] += esLaboradoDia8 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia9 = lstDiasQuincenaConsultar.Any(x => x == 9);
                                if (esDia9)
                                {
                                    int esLaboradoDia9 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia9 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia9);
                                    lstHorasLaboradasQuincenalOperativo[8] += esLaboradoDia9 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia10 = lstDiasQuincenaConsultar.Any(x => x == 10);
                                if (esDia10)
                                {
                                    int esLaboradoDia10 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia10 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia10);
                                    lstHorasLaboradasQuincenalOperativo[9] += esLaboradoDia10 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia11 = lstDiasQuincenaConsultar.Any(x => x == 11);
                                if (esDia11)
                                {
                                    int esLaboradoDia11 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia11 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia11);
                                    lstHorasLaboradasQuincenalOperativo[10] += esLaboradoDia11 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia12 = lstDiasQuincenaConsultar.Any(x => x == 12);
                                if (esDia12)
                                {
                                    int esLaboradoDia12 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia12 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia12);
                                    lstHorasLaboradasQuincenalOperativo[11] += esLaboradoDia12 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia13 = lstDiasQuincenaConsultar.Any(x => x == 13);
                                if (esDia13)
                                {
                                    int esLaboradoDia13 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia13 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia13);
                                    lstHorasLaboradasQuincenalOperativo[11] += esLaboradoDia13 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia14 = lstDiasQuincenaConsultar.Any(x => x == 14);
                                if (esDia14)
                                {
                                    int esLaboradoDia14 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia14 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia14);
                                    lstHorasLaboradasQuincenalOperativo[13] += esLaboradoDia14 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia15 = lstDiasQuincenaConsultar.Any(x => x == 15);
                                if (esDia15)
                                {
                                    int esLaboradoDia15 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia15 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia15);
                                    lstHorasLaboradasQuincenalOperativo[14] += esLaboradoDia15 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }
                            }
                        }
                        #endregion

                        #region CALCULOS DE HORAS DE ADMINISTRATIVO
                        for (int i = 0; i < lstEmpleadosAdministrativoCP.Count(); i++)
                        {
                            int claveEmpleado = lstEmpleadosAdministrativoCP[i];
                            int existeRegistroIncidenciaEmpleado = lstEmpleadosNominaQuincenalCP.Where(w => w.clave_empleado == claveEmpleado).Count();
                            if (existeRegistroIncidenciaEmpleado > 0)
                            {
                                bool esDia1 = lstDiasQuincenaConsultar.Any(x => x == 1);
                                if (esDia1)
                                {
                                    int esLaboradoDia1 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                    lstHorasLaboradasQuincenalAdministrativo[0] += esLaboradoDia1 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia2 = lstDiasQuincenaConsultar.Any(x => x == 2);
                                if (esDia2)
                                {
                                    int esLaboradoDia2 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                    lstHorasLaboradasQuincenalAdministrativo[1] += esLaboradoDia2 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia3 = lstDiasQuincenaConsultar.Any(x => x == 3);
                                if (esDia3)
                                {
                                    int esLaboradoDia3 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                    lstHorasLaboradasQuincenalAdministrativo[2] += esLaboradoDia3 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia4 = lstDiasQuincenaConsultar.Any(x => x == 4);
                                if (esDia4)
                                {
                                    int esLaboradoDia4 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                    lstHorasLaboradasQuincenalAdministrativo[3] += esLaboradoDia4 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia5 = lstDiasQuincenaConsultar.Any(x => x == 5);
                                if (esDia5)
                                {
                                    int esLaboradoDia5 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                    lstHorasLaboradasQuincenalAdministrativo[4] += esLaboradoDia5 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia6 = lstDiasQuincenaConsultar.Any(x => x == 6);
                                if (esDia6)
                                {
                                    int esLaboradoDia6 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                    lstHorasLaboradasQuincenalAdministrativo[5] += esLaboradoDia6 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia7 = lstDiasQuincenaConsultar.Any(x => x == 7);
                                if (esDia7)
                                {
                                    int esLaboradoDia7 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                    lstHorasLaboradasQuincenalAdministrativo[6] += esLaboradoDia7 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia8 = lstDiasQuincenaConsultar.Any(x => x == 8);
                                if (esDia8)
                                {
                                    int esLaboradoDia8 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia8 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia8);
                                    lstHorasLaboradasQuincenalAdministrativo[7] += esLaboradoDia8 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia9 = lstDiasQuincenaConsultar.Any(x => x == 9);
                                if (esDia9)
                                {
                                    int esLaboradoDia9 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia9 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia9);
                                    lstHorasLaboradasQuincenalAdministrativo[8] += esLaboradoDia9 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia10 = lstDiasQuincenaConsultar.Any(x => x == 10);
                                if (esDia10)
                                {
                                    int esLaboradoDia10 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia10 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia10);
                                    lstHorasLaboradasQuincenalAdministrativo[9] += esLaboradoDia10 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia11 = lstDiasQuincenaConsultar.Any(x => x == 11);
                                if (esDia11)
                                {
                                    int esLaboradoDia11 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia11 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia11);
                                    lstHorasLaboradasQuincenalAdministrativo[10] += esLaboradoDia11 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia12 = lstDiasQuincenaConsultar.Any(x => x == 12);
                                if (esDia12)
                                {
                                    int esLaboradoDia12 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia12 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia12);
                                    lstHorasLaboradasQuincenalAdministrativo[11] += esLaboradoDia12 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia13 = lstDiasQuincenaConsultar.Any(x => x == 13);
                                if (esDia13)
                                {
                                    int esLaboradoDia13 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia13 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia13);
                                    lstHorasLaboradasQuincenalAdministrativo[11] += esLaboradoDia13 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia14 = lstDiasQuincenaConsultar.Any(x => x == 14);
                                if (esDia14)
                                {
                                    int esLaboradoDia14 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia14 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia14);
                                    lstHorasLaboradasQuincenalAdministrativo[13] += esLaboradoDia14 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }

                                bool esDia15 = lstDiasQuincenaConsultar.Any(x => x == 15);
                                if (esDia15)
                                {
                                    int esLaboradoDia15 = lstEmpleadosNominaQuincenalCP.Where(x => x.dia15 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia15);
                                    lstHorasLaboradasQuincenalAdministrativo[14] += esLaboradoDia15 * lstEmpleadosCP.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                                }
                            }
                        }
                        #endregion

                        #endregion

                        #region CALCULOS CC DE ARR
                        #region LISTADO DE EMPLEADOS CON TIPO NOMINA 4 (QUINCENAL)
                        tipoNomina = 0;
                        tipoNomina = Convert.ToInt32(CatTipoNominas.quincenal);
                        decimal idIncidenciaQuincenalArr = lstIncidenciasCP.Where(x => x.tipo_nomina == tipoNomina).Select(x => x.id_incidencia).FirstOrDefault();
                        List<IncidenciasDetDTO> lstEmpleadosNominaQuincenalArr = lstDetIncidenciasCP.Where(x => x.id_incidencia == idIncidenciaQuincenalArr).ToList();
                        #endregion

                        #region SE DESGLOSA LOS EMPLEADOS AL ÁREA OPERATIVA QUE PERTENECEN A SUS LISTAS CORRESPONDIENTES.
                        idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.mantenimiento);
                        var lstEmpleadosMantenimientoArr = lstEmpleadosArr.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Arrendadora).Select(x => x.clave_empleado).ToList();

                        idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.operativo);
                        var lstEmpleadosOperativoArr = lstEmpleadosArr.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Arrendadora).Select(x => x.clave_empleado).ToList();

                        idAreaOperativa = Convert.ToInt32(CatAreasOperativasEnum.administrativo);
                        var lstEmpleadosAdministrativoArr = lstEmpleadosArr.Where(x => x.idAreaOperativa == idAreaOperativa && x.idEmpresa == (int)EmpresaEnum.Arrendadora).Select(x => x.clave_empleado).ToList();
                        #endregion

                        #region CALCULOS DE HORAS DE MANTENIMIENTO
                        for (int i = 0; i < lstEmpleadosMantenimientoArr.Count(); i++)
                        {
                            int claveEmpleado = lstEmpleadosMantenimientoArr[i];

                            bool esDia1 = lstDiasQuincenaConsultar.Any(x => x == 1);
                            if (esDia1)
                            {
                                int esLaboradoDia1 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasQuincenalMantenimiento[0] += esLaboradoDia1 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia2 = lstDiasQuincenaConsultar.Any(x => x == 2);
                            if (esDia2)
                            {
                                int esLaboradoDia2 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasQuincenalMantenimiento[1] += esLaboradoDia2 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia3 = lstDiasQuincenaConsultar.Any(x => x == 3);
                            if (esDia3)
                            {
                                int esLaboradoDia3 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasQuincenalMantenimiento[2] += esLaboradoDia3 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia4 = lstDiasQuincenaConsultar.Any(x => x == 4);
                            if (esDia4)
                            {
                                int esLaboradoDia4 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasQuincenalMantenimiento[3] += esLaboradoDia4 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia5 = lstDiasQuincenaConsultar.Any(x => x == 5);
                            if (esDia5)
                            {
                                int esLaboradoDia5 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasQuincenalMantenimiento[4] += esLaboradoDia5 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia6 = lstDiasQuincenaConsultar.Any(x => x == 6);
                            if (esDia6)
                            {
                                int esLaboradoDia6 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasQuincenalMantenimiento[5] += esLaboradoDia6 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia7 = lstDiasQuincenaConsultar.Any(x => x == 7);
                            if (esDia7)
                            {
                                int esLaboradoDia7 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasQuincenalMantenimiento[6] += esLaboradoDia7 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia8 = lstDiasQuincenaConsultar.Any(x => x == 8);
                            if (esDia8)
                            {
                                int esLaboradoDia8 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia8 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia8);
                                lstHorasLaboradasQuincenalMantenimiento[7] += esLaboradoDia8 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia9 = lstDiasQuincenaConsultar.Any(x => x == 9);
                            if (esDia9)
                            {
                                int esLaboradoDia9 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia9 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia9);
                                lstHorasLaboradasQuincenalMantenimiento[8] += esLaboradoDia9 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia10 = lstDiasQuincenaConsultar.Any(x => x == 10);
                            if (esDia10)
                            {
                                int esLaboradoDia10 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia10 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia10);
                                lstHorasLaboradasQuincenalMantenimiento[9] += esLaboradoDia10 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia11 = lstDiasQuincenaConsultar.Any(x => x == 11);
                            if (esDia11)
                            {
                                int esLaboradoDia11 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia11 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia11);
                                lstHorasLaboradasQuincenalMantenimiento[10] += esLaboradoDia11 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia12 = lstDiasQuincenaConsultar.Any(x => x == 12);
                            if (esDia12)
                            {
                                int esLaboradoDia12 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia12 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia12);
                                lstHorasLaboradasQuincenalMantenimiento[11] += esLaboradoDia12 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia13 = lstDiasQuincenaConsultar.Any(x => x == 13);
                            if (esDia13)
                            {
                                int esLaboradoDia13 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia13 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia13);
                                lstHorasLaboradasQuincenalMantenimiento[11] += esLaboradoDia13 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia14 = lstDiasQuincenaConsultar.Any(x => x == 14);
                            if (esDia14)
                            {
                                int esLaboradoDia14 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia14 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia14);
                                lstHorasLaboradasQuincenalMantenimiento[13] += esLaboradoDia14 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia15 = lstDiasQuincenaConsultar.Any(x => x == 15);
                            if (esDia15)
                            {
                                int esLaboradoDia15 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia15 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia15);
                                lstHorasLaboradasQuincenalMantenimiento[14] += esLaboradoDia15 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                        }
                        #endregion

                        #region CALCULOS DE HORAS DE OPERATIVO
                        for (int i = 0; i < lstEmpleadosOperativoArr.Count(); i++)
                        {
                            int claveEmpleado = lstEmpleadosOperativoArr[i];

                            bool esDia1 = lstDiasQuincenaConsultar.Any(x => x == 1);
                            if (esDia1)
                            {
                                int esLaboradoDia1 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasQuincenalOperativo[0] += esLaboradoDia1 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia2 = lstDiasQuincenaConsultar.Any(x => x == 2);
                            if (esDia2)
                            {
                                int esLaboradoDia2 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasQuincenalOperativo[1] += esLaboradoDia2 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia3 = lstDiasQuincenaConsultar.Any(x => x == 3);
                            if (esDia3)
                            {
                                int esLaboradoDia3 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasQuincenalOperativo[2] += esLaboradoDia3 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia4 = lstDiasQuincenaConsultar.Any(x => x == 4);
                            if (esDia4)
                            {
                                int esLaboradoDia4 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasQuincenalOperativo[3] += esLaboradoDia4 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia5 = lstDiasQuincenaConsultar.Any(x => x == 5);
                            if (esDia5)
                            {
                                int esLaboradoDia5 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasQuincenalOperativo[4] += esLaboradoDia5 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia6 = lstDiasQuincenaConsultar.Any(x => x == 6);
                            if (esDia6)
                            {
                                int esLaboradoDia6 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasQuincenalOperativo[5] += esLaboradoDia6 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia7 = lstDiasQuincenaConsultar.Any(x => x == 7);
                            if (esDia7)
                            {
                                int esLaboradoDia7 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasQuincenalOperativo[6] += esLaboradoDia7 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia8 = lstDiasQuincenaConsultar.Any(x => x == 8);
                            if (esDia8)
                            {
                                int esLaboradoDia8 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia8 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia8);
                                lstHorasLaboradasQuincenalOperativo[7] += esLaboradoDia8 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia9 = lstDiasQuincenaConsultar.Any(x => x == 9);
                            if (esDia9)
                            {
                                int esLaboradoDia9 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia9 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia9);
                                lstHorasLaboradasQuincenalOperativo[8] += esLaboradoDia9 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia10 = lstDiasQuincenaConsultar.Any(x => x == 10);
                            if (esDia10)
                            {
                                int esLaboradoDia10 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia10 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia10);
                                lstHorasLaboradasQuincenalOperativo[9] += esLaboradoDia10 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia11 = lstDiasQuincenaConsultar.Any(x => x == 11);
                            if (esDia11)
                            {
                                int esLaboradoDia11 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia11 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia11);
                                lstHorasLaboradasQuincenalOperativo[10] += esLaboradoDia11 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia12 = lstDiasQuincenaConsultar.Any(x => x == 12);
                            if (esDia12)
                            {
                                int esLaboradoDia12 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia12 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia12);
                                lstHorasLaboradasQuincenalOperativo[11] += esLaboradoDia12 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia13 = lstDiasQuincenaConsultar.Any(x => x == 13);
                            if (esDia13)
                            {
                                int esLaboradoDia13 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia13 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia13);
                                lstHorasLaboradasQuincenalOperativo[11] += esLaboradoDia13 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia14 = lstDiasQuincenaConsultar.Any(x => x == 14);
                            if (esDia14)
                            {
                                int esLaboradoDia14 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia14 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia14);
                                lstHorasLaboradasQuincenalOperativo[13] += esLaboradoDia14 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia15 = lstDiasQuincenaConsultar.Any(x => x == 15);
                            if (esDia15)
                            {
                                int esLaboradoDia15 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia15 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia15);
                                lstHorasLaboradasQuincenalOperativo[14] += esLaboradoDia15 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                        }
                        #endregion

                        #region CALCULOS DE HORAS DE ADMINISTRATIVO
                        for (int i = 0; i < lstEmpleadosAdministrativoArr.Count(); i++)
                        {
                            int claveEmpleado = lstEmpleadosAdministrativoArr[i];

                            bool esDia1 = lstDiasQuincenaConsultar.Any(x => x == 1);
                            if (esDia1)
                            {
                                int esLaboradoDia1 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia1 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia1);
                                lstHorasLaboradasQuincenalAdministrativo[0] += esLaboradoDia1 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia2 = lstDiasQuincenaConsultar.Any(x => x == 2);
                            if (esDia2)
                            {
                                int esLaboradoDia2 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia2 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia2);
                                lstHorasLaboradasQuincenalAdministrativo[1] += esLaboradoDia2 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia3 = lstDiasQuincenaConsultar.Any(x => x == 3);
                            if (esDia3)
                            {
                                int esLaboradoDia3 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia3 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia3);
                                lstHorasLaboradasQuincenalAdministrativo[2] += esLaboradoDia3 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia4 = lstDiasQuincenaConsultar.Any(x => x == 4);
                            if (esDia4)
                            {
                                int esLaboradoDia4 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia4 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia4);
                                lstHorasLaboradasQuincenalAdministrativo[3] += esLaboradoDia4 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia5 = lstDiasQuincenaConsultar.Any(x => x == 5);
                            if (esDia5)
                            {
                                int esLaboradoDia5 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia5 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia5);
                                lstHorasLaboradasQuincenalAdministrativo[4] += esLaboradoDia5 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia6 = lstDiasQuincenaConsultar.Any(x => x == 6);
                            if (esDia6)
                            {
                                int esLaboradoDia6 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia6 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia6);
                                lstHorasLaboradasQuincenalAdministrativo[5] += esLaboradoDia6 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia7 = lstDiasQuincenaConsultar.Any(x => x == 7);
                            if (esDia7)
                            {
                                int esLaboradoDia7 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia7 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia7);
                                lstHorasLaboradasQuincenalAdministrativo[6] += esLaboradoDia7 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia8 = lstDiasQuincenaConsultar.Any(x => x == 8);
                            if (esDia8)
                            {
                                int esLaboradoDia8 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia8 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia8);
                                lstHorasLaboradasQuincenalAdministrativo[7] += esLaboradoDia8 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia9 = lstDiasQuincenaConsultar.Any(x => x == 9);
                            if (esDia9)
                            {
                                int esLaboradoDia9 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia9 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia9);
                                lstHorasLaboradasQuincenalAdministrativo[8] += esLaboradoDia9 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia10 = lstDiasQuincenaConsultar.Any(x => x == 10);
                            if (esDia10)
                            {
                                int esLaboradoDia10 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia10 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia10);
                                lstHorasLaboradasQuincenalAdministrativo[9] += esLaboradoDia10 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia11 = lstDiasQuincenaConsultar.Any(x => x == 11);
                            if (esDia11)
                            {
                                int esLaboradoDia11 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia11 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia11);
                                lstHorasLaboradasQuincenalAdministrativo[10] += esLaboradoDia11 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia12 = lstDiasQuincenaConsultar.Any(x => x == 12);
                            if (esDia12)
                            {
                                int esLaboradoDia12 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia12 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia12);
                                lstHorasLaboradasQuincenalAdministrativo[11] += esLaboradoDia12 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia13 = lstDiasQuincenaConsultar.Any(x => x == 13);
                            if (esDia13)
                            {
                                int esLaboradoDia13 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia13 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia13);
                                lstHorasLaboradasQuincenalAdministrativo[11] += esLaboradoDia13 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia14 = lstDiasQuincenaConsultar.Any(x => x == 14);
                            if (esDia14)
                            {
                                int esLaboradoDia14 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia14 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia14);
                                lstHorasLaboradasQuincenalAdministrativo[13] += esLaboradoDia14 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }

                            bool esDia15 = lstDiasQuincenaConsultar.Any(x => x == 15);
                            if (esDia15)
                            {
                                int esLaboradoDia15 = lstEmpleadosNominaQuincenalArr.Where(x => x.dia15 == 1 && x.clave_empleado == claveEmpleado).Sum(x => x.dia15);
                                lstHorasLaboradasQuincenalAdministrativo[14] += esLaboradoDia15 * lstEmpleadosArr.Where(x => x.clave_empleado == claveEmpleado).Select(x => x.horas).FirstOrDefault();
                            }
                        }
                        #endregion

                        #endregion
                    }
                    #endregion
                }

                if (lstHorasLaboradasSemanalMantenimiento.Count() > 0 || lstHorasLaboradasSemanalOperativo.Count() > 0 || lstHorasLaboradasSemanalAdministrativo.Count() > 0 ||
                    lstHorasLaboradasQuincenalMantenimiento.Count() > 0 || lstHorasLaboradasQuincenalOperativo.Count() > 0 || lstHorasLaboradasQuincenalAdministrativo.Count() > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstHorasLaboradasSemanalMantenimiento", lstHorasLaboradasSemanalMantenimiento);
                    resultado.Add("lstHorasLaboradasSemanalOperativo", lstHorasLaboradasSemanalOperativo);
                    resultado.Add("lstHorasLaboradasSemanalAdministrativo", lstHorasLaboradasSemanalAdministrativo);

                    resultado.Add("lstHorasLaboradasQuincenalMantenimiento", lstHorasLaboradasQuincenalMantenimiento);
                    resultado.Add("lstHorasLaboradasQuincenalOperativo", lstHorasLaboradasQuincenalOperativo);
                    resultado.Add("lstHorasLaboradasQuincenalAdministrativo", lstHorasLaboradasQuincenalAdministrativo);
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
                return resultado;
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            try
            {
                var lstCC = new List<Core.DTO.Principal.Generales.ComboGroupDTO>();

                #region COMENTADO
                //string strQuery = @"SELECT cc as Value, (cc + ' - ' + descripcion) as Text FROM cc ORDER BY cc";
                //var odbc = new OdbcConsultaDTO() { consulta = strQuery };

                //#region SE CREA LISTADO DE CC DE CONSTRUPLAN
                //var lstCCConstruplan = _context.tblP_CC.Where(x => x.estatus).ToList();
                //var lstCCConstruplanCboDTO = lstCCConstruplan.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                //{
                //    Text = x.cc + " - " + x.descripcion,
                //    Value = x.cc,
                //    Prefijo = GruposSeguridadEnum.CONSTRUPLAN.ToString()
                //}).ToList();
                //if (lstCCConstruplanCboDTO.Count() > 0)
                //    lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONSTRUPLAN", options = lstCCConstruplanCboDTO });
                //#endregion

                //#region SE CREA LISTADO DE CC DE ARRENDADORA
                //var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbc);
                //if (lstCCArrendadora.Count() > 0)
                //{
                //    foreach (var itemArr in lstCCArrendadora)
                //    {
                //        itemArr.Prefijo = GruposSeguridadEnum.ARRENDADORA.ToString();
                //    }
                //    lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "ARRENDADORA", options = lstCCArrendadora });
                //}
                //#endregion
                #endregion

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
                        //Prefijo = vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru ? ((int)GruposSeguridadEnum.GRUPO).ToString() : ((int)GruposSeguridadEnum.PERU).ToString()
                        Prefijo = ((int)GruposSeguridadEnum.GRUPO).ToString()
                    }).ToList();
                    if (lstAgrupaciones.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "AGRUPACIONES", options = lstAgrupacionesCboDTO });
                    #endregion

                    #region SE CREA LISTADO DE CONTRATISTAS
                    var lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                    var lstContratistasCboDTO = lstContratistas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = x.nombreEmpresa,
                        Value = "c_" + x.id.ToString(),
                        Prefijo = ((int)GruposSeguridadEnum.CONTRATISTA).ToString()
                    }).ToList();
                    if (lstContratistas.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONTRATISTAS", options = lstContratistasCboDTO });
                    #endregion

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
                    //#if DEBUG
                    //                    List<ComboDTO> lstPersonal = _context.Select<ComboDTO>(new DapperDTO
                    //                    {
                    //                        baseDatos = MainContextEnum.Construplan,
                    //                        consulta = @"Select id as value, nomAgrupacion as text from tblS_IncidentesAgrupacionCC"
                    //                    }).ToList();

                    //                    resultado.Add(SUCCESS, true);
                    //                    resultado.Add(ITEMS, lstPersonal);

                    //#else
                    //                     resultado.Add(SUCCESS, false);
                    //                    resultado.Add("EMPTY", true);
                    //#endif

                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerComboCCAmbasEmpresas", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }
        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas_SoloGrupos(bool incContratista, int? division)
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
                        Prefijo = ((int)GruposSeguridadEnum.GRUPO).ToString()
                    }).ToList();
                    if (lstAgrupaciones.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "AGRUPACIONES", options = lstAgrupacionesCboDTO });
                    #endregion

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

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresasDivisionesLineas(bool incContratista, List<int> listaDivisiones, List<int> listaLineasNegocio)
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
                    var lstAgrupaciones = _context.tblS_IncidentesAgrupacionCC.Where(x => x.esActivo).OrderBy(y => y.nomAgrupacion).ToList();

                    #region Filtrar por division y lineas de negocios
                    if (listaDivisiones != null)
                    {
                        var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division) && x.idEmpresa < 1000).ToList();

                        lstAgrupaciones = lstAgrupaciones.Join(
                            listaCentrosCostoDivision,
                            a => a.id,
                            cd => cd.idAgrupacion,
                            (a, cd) => new { a, cd }
                        ).Select(x => x.a).ToList();
                    }

                    if (listaLineasNegocio != null)
                    {
                        var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id) && x.idEmpresa < 1000).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                        lstAgrupaciones = lstAgrupaciones.Join(
                            listaCentrosCostoDivisionLineaNegocio,
                            a => a.id,
                            cd => cd.idAgrupacion,
                            (a, cd) => new { a, cd }
                        ).Select(x => x.a).ToList();
                    }
                    #endregion

                    var lstAgrupacionesCboDTO = lstAgrupaciones.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = x.nomAgrupacion,
                        Value = x.id.ToString(),
                        //Prefijo = vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru ? ((int)GruposSeguridadEnum.GRUPO).ToString() : ((int)GruposSeguridadEnum.PERU).ToString()
                        Prefijo = ((int)GruposSeguridadEnum.GRUPO).ToString()
                    }).ToList();
                    if (lstAgrupaciones.Count() > 0)
                        lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "AGRUPACIONES", options = lstAgrupacionesCboDTO });
                    #endregion

                    #region SE CREA LISTADO DE CONTRATISTAS
                    //var lstContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();

                    //#region Filtrar por division y lineas de negocios
                    //if (listaDivisiones != null)
                    //{
                    //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division) && x.idEmpresa == 1000).ToList();

                    //    lstContratistas = lstContratistas.Join(
                    //        listaCentrosCostoDivision,
                    //        a => a.id,
                    //        cd => cd.idAgrupacion,
                    //        (a, cd) => new { a, cd }
                    //    ).Select(x => x.a).ToList();
                    //}

                    //if (listaLineasNegocio != null)
                    //{
                    //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id) && x.idEmpresa == 1000).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                    //    lstContratistas = lstContratistas.Join(
                    //        listaCentrosCostoDivisionLineaNegocio,
                    //        a => a.id,
                    //        cd => cd.idAgrupacion,
                    //        (a, cd) => new { a, cd }
                    //    ).Select(x => x.a).ToList();
                    //}
                    //#endregion

                    //var lstContratistasCboDTO = lstContratistas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    //{
                    //    Text = x.nombreEmpresa,
                    //    Value = "c_" + x.id.ToString(),
                    //    Prefijo = ((int)GruposSeguridadEnum.CONTRATISTA).ToString()
                    //}).ToList();
                    //if (lstContratistas.Count() > 0)
                    //    lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "CONTRATISTAS", options = lstContratistasCboDTO });
                    #endregion

                    #region SE CREA LISTADO DE AGRUPACIONES DE CONTRATISTAS
                    if (vSesiones.sesionCurrentView != 7322 && vSesiones.sesionCurrentView != 7320) //No se agregan en las vistas del módulo de incidentes: 7322 - Dashboard, 7320 - Captura Incidentes
                    {
                        var lstAgrupacionContratistas = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).ToList();

                        #region Filtrar por division y lineas de negocios
                        if (listaDivisiones != null)
                        {
                            var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaDivisiones.Contains(x.division) && x.idEmpresa == 2000).ToList();

                            lstAgrupacionContratistas = lstAgrupacionContratistas.Join(
                                listaCentrosCostoDivision,
                                a => a.id,
                                cd => cd.idAgrupacion,
                                (a, cd) => new { a, cd }
                            ).Select(x => x.a).ToList();
                        }

                        if (listaLineasNegocio != null)
                        {
                            var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && listaLineasNegocio.Contains(x.lineaNegocio_id) && x.idEmpresa == 2000).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                            lstAgrupacionContratistas = lstAgrupacionContratistas.Join(
                                listaCentrosCostoDivisionLineaNegocio,
                                a => a.id,
                                cd => cd.idAgrupacion,
                                (a, cd) => new { a, cd }
                            ).Select(x => x.a).ToList();
                        }
                        #endregion

                        var lstAgrupacionContratistasDTO = lstAgrupacionContratistas.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Text = x.nomAgrupacion,
                            Value = "a_" + x.id.ToString(),
                            Prefijo = ((int)GruposSeguridadEnum.agrupacionContratistas).ToString()
                        }).ToList();
                        if (lstAgrupacionContratistas.Count() > 0)
                            lstCC.Add(new Core.DTO.Principal.Generales.ComboGroupDTO { label = "AGRUPACIÓN CONTRATISTAS", options = lstAgrupacionContratistasDTO });
                    }
                    #endregion
                }

                resultado.Add(ITEMS, lstCC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, "IndicadoresSeguridadController", "ObtenerComboCCAmbasEmpresas", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        #endregion

        #region CONTRATISTAS
        public bool ValidarAccesoContratista()
        {
            try
            {
                if ((int)vSesiones.sesionUsuarioDTO.idPerfil == 4) //CONTRATISTA
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "ValidarAccesoContratista", ex, AccionEnum.ELIMINAR, 0, 0);
                return false;
            }
        }
        #endregion

        #region CATÁLOGO AGRUPACIÓN CONTRATISTAS
        public List<IncidentesAgrupacionesContratistasDTO> GetAgrupacionesContratistas(IncidentesAgrupacionesContratistasDTO objFiltro)
        {
            try
            {
                List<IncidentesAgrupacionesContratistasDTO> lstAgrupaciones = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).Select(x => new IncidentesAgrupacionesContratistasDTO
                {
                    id = x.id,
                    nomAgrupacion = x.nomAgrupacion
                }).ToList();
                return lstAgrupaciones;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "GetAgrupacionesContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool existeNomAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            try
            {
                int existeRegistro = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.nomAgrupacion == objAgrupacion.nomAgrupacion.Trim() && x.esActivo).Count();
                if (existeRegistro > 0)
                    throw new Exception("Ya existe una agrupación con este nombre.");
                else
                    return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "existeNomAgrupacion", e, AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(objAgrupacion));
                return false;
            }
        }

        public bool CrearAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            try
            {
                tblS_IncidentesAgrupacionContratistas objGuardarAgrupacion = new tblS_IncidentesAgrupacionContratistas();
                objGuardarAgrupacion.nomAgrupacion = objAgrupacion.nomAgrupacion;
                objGuardarAgrupacion.esActivo = true;
                objGuardarAgrupacion.fechaCreacion = DateTime.Now;
                objGuardarAgrupacion.fechaModificacion = DateTime.Now;
                _context.tblS_IncidentesAgrupacionContratistas.Add(objGuardarAgrupacion);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "CrearAgrupacion", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objAgrupacion));
                return false;
            }
        }

        public bool ActualizarAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            try
            {
                tblS_IncidentesAgrupacionContratistas ActualizarAgrupacion = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.id == objAgrupacion.id).First();
                ActualizarAgrupacion.nomAgrupacion = objAgrupacion.nomAgrupacion.Trim();
                ActualizarAgrupacion.fechaModificacion = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "ActualizarAgrupacion", ex, AccionEnum.ACTUALIZAR, objAgrupacion.id, JsonUtils.convertNetObjectToJson(objAgrupacion));
                return false;
            }
        }

        public bool EliminarAgrupacion(int idAgrupacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE ELIMINAN LOS CONTRATISTAS RELACIONADOS A LA AGRUPACIÓN
                    List<tblS_IncidentesAgrupacionContratistasDet> lstEliminarContratistasAgrupacion = _context.tblS_IncidentesAgrupacionContratistasDet.Where(x => x.idAgruContratista == idAgrupacion && x.esActivo).ToList();
                    if (lstEliminarContratistasAgrupacion.Count() > 0)
                    {
                        foreach (var item in lstEliminarContratistasAgrupacion)
                        {
                            item.esActivo = false;
                        }
                        _context.SaveChanges();
                    }
                    #endregion

                    #region SE ELIMINA LA AGRUPACIÓN
                    tblS_IncidentesAgrupacionContratistas EliminarAgrupacion = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.id == idAgrupacion && x.esActivo).First();
                    if (EliminarAgrupacion != null)
                    {
                        EliminarAgrupacion.esActivo = false;
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    LogError(2, 0, "IndicadoresSeguridadController", "EliminarAgrupacion", ex, AccionEnum.ELIMINAR, idAgrupacion, JsonUtils.convertNetObjectToJson(idAgrupacion));
                    return false;
                }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillCboAgrupaciones()
        {
            var dataAgrupaciones = _context.tblS_IncidentesAgrupacionContratistas.Where(x => x.esActivo).OrderBy(x => x.nomAgrupacion).Select(x => new Core.DTO.Principal.Generales.ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.nomAgrupacion
            }).ToList();
            return dataAgrupaciones;
        }

        public List<IncidentesAgrupacionesContratistasDTO> GetContratistas(int idAgrupacion)
        {
            try
            {
                List<IncidentesAgrupacionesContratistasDTO> lstContratistas = _context.tblS_IncidentesAgrupacionContratistasDet
                .Where(x => x.idAgruContratista == idAgrupacion && x.esActivo).Select(x => new IncidentesAgrupacionesContratistasDTO
                {
                    id = x.id,
                    idAgruContratista = x.idAgruContratista,
                    idContratista = x.idContratista,
                    nomAgrupacion = x.agrupacionContratistas.nomAgrupacion.Trim(),
                    nomContratista = x.contratistas.nombreEmpresa.Trim()
                }).ToList();
                return lstContratistas;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "GetContratistas", e, AccionEnum.CONSULTA, idAgrupacion, 0);
                return null;
            }
        }

        public bool existeContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            try
            {
                int existeRegistro = _context.tblS_IncidentesAgrupacionContratistasDet.Where(x => x.idAgruContratista == objAgrupacion.idAgruContratista && x.idContratista == objAgrupacion.idContratista && x.esActivo).Count();
                if (existeRegistro > 0)
                    throw new Exception("Este contratista ya se encuentra en una agrupación.");
                else
                    return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "existeContratistaEnAgrupacion", e, AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(objAgrupacion));
                return false;
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillCboContratistas()
        {
            var dataContratistas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).OrderBy(x => x.nombreEmpresa).Select(x => new Core.DTO.Principal.Generales.ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.nombreEmpresa.Trim()
            }).ToList();
            return dataContratistas;
        }

        public bool CrearContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            try
            {
                tblS_IncidentesAgrupacionContratistasDet objGuardarContratista = new tblS_IncidentesAgrupacionContratistasDet();
                objGuardarContratista.idAgruContratista = objAgrupacion.idAgruContratista;
                objGuardarContratista.idContratista = objAgrupacion.idContratista;
                objGuardarContratista.esActivo = true;
                objGuardarContratista.fechaCreacion = DateTime.Now;
                objGuardarContratista.fechaModificacion = DateTime.Now;
                _context.tblS_IncidentesAgrupacionContratistasDet.Add(objGuardarContratista);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "CrearAgrupacion", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objAgrupacion));
                return false;
            }
        }

        public bool EliminarContratistaEnAgrupacion(int idAgrupacionDet)
        {
            try
            {
                tblS_IncidentesAgrupacionContratistasDet EliminarContratista = _context.tblS_IncidentesAgrupacionContratistasDet.Where(x => x.id == idAgrupacionDet && x.esActivo).First();
                if (EliminarContratista != null)
                {
                    EliminarContratista.esActivo = false;
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "EliminarContratista", ex, AccionEnum.ELIMINAR, idAgrupacionDet, JsonUtils.convertNetObjectToJson(idAgrupacionDet));
                return false;
            }
        }
        #endregion

        #region CATÁLOGO RELACIÓN CONTRATISTA - EMPRESA
        public List<IncidentesRelEmpresaContratistasDTO> GetEmpresaRelContratistas(IncidentesRelEmpresaContratistasDTO objFiltro)
        {
            try
            {
                #region LISTADO DE EMPRESAS CONTRATISTAS Y USUARIOS CON PERFIL CONTRATISTAS
                //List<tblS_IncidentesEmpresasContratistas> lstEmpresas = _context.tblS_IncidentesEmpresasContratistas.Where(x => x.esActivo).ToList();
                List<tblP_Usuario> lstUsuariosContratistas = _context.tblP_Usuario.Where(x => x.perfilID == 4 && x.estatus).ToList();
                #endregion

                List<IncidentesRelEmpresaContratistasDTO> lstRelEmpresaContratistas = _context.tblS_IncidentesRelEmpresaContratistas.Where(x => x.idEmpresa == objFiltro.idEmpresa && x.esActivo).Select(x => new IncidentesRelEmpresaContratistasDTO
                {
                    id = x.id,
                    nomContratista = x.empleadosContratistas.nombre + " " + x.empleadosContratistas.apellidoPaterno + " " + x.empleadosContratistas.apellidoMaterno,
                    idContratista = x.idContratista
                }).ToList();
                return lstRelEmpresaContratistas;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "GetAgrupacionesContratistas", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public string GetNombreContratista(int idContratista)
        {
            try
            {
                tblP_Usuario objUsuario = _context.tblP_Usuario.Where(x => x.id == idContratista && x.estatus && x.perfilID == 4).First();
                if (objUsuario != null)
                {
                    string nombre = !string.IsNullOrEmpty(objUsuario.nombre) ? objUsuario.nombre : string.Empty;
                    string apePaterno = !string.IsNullOrEmpty(objUsuario.apellidoPaterno) ? objUsuario.apellidoPaterno : string.Empty;
                    string apeMaterno = !string.IsNullOrEmpty(objUsuario.apellidoMaterno) ? objUsuario.apellidoMaterno : string.Empty;
                    string nombreCompleto = nombre + " " + apePaterno + " " + apeMaterno;
                    return nombreCompleto;
                }
                else
                    return "N/A";
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        public bool CrearRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            try
            {
                tblS_IncidentesRelEmpresaContratistas objGuardarRel = new tblS_IncidentesRelEmpresaContratistas();
                objGuardarRel.idEmpresa = objRel.idEmpresa;
                objGuardarRel.idContratista = objRel.idContratista;
                objGuardarRel.esActivo = true;
                objGuardarRel.fechaCreacion = DateTime.Now;
                objGuardarRel.fechaModificacion = DateTime.Now;
                _context.tblS_IncidentesRelEmpresaContratistas.Add(objGuardarRel);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "CrearRelEmpresaContratista", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objRel));
                return false;
            }
        }

        public bool ActualizarRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            try
            {
                tblS_IncidentesRelEmpresaContratistas objActualizarRel = _context.tblS_IncidentesRelEmpresaContratistas.Where(x => x.id == objRel.id).First();
                objActualizarRel.idContratista = objRel.idContratista;
                objActualizarRel.fechaModificacion = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "ActualizarRelEmpresaContratista", ex, AccionEnum.ACTUALIZAR, objRel.id, JsonUtils.convertNetObjectToJson(objRel));
                return false;
            }
        }

        public bool EliminarRelEmpresaContratista(int idRel)
        {
            try
            {
                tblS_IncidentesRelEmpresaContratistas objEliminarRel = _context.tblS_IncidentesRelEmpresaContratistas.Where(x => x.id == idRel && x.esActivo).First();
                if (objEliminarRel != null)
                {
                    objEliminarRel.esActivo = false;
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "EliminarRelEmpresaContratista", ex, AccionEnum.ELIMINAR, idRel, JsonUtils.convertNetObjectToJson(idRel));
                return false;
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillCboContratistasSP()
        {
            var dataContratistasSP = _context.tblP_Usuario.Where(x => x.perfilID == 4 && x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno
            }).ToList();
            return dataContratistasSP;
        }

        public bool DisponibleRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            try
            {
                int disponibleContratista = _context.tblS_IncidentesRelEmpresaContratistas.Where(x => x.idEmpresa == objRel.idEmpresa && x.idContratista == objRel.idContratista && x.esActivo).Count();
                if (disponibleContratista > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "IndicadoresSeguridadController", "DisponibleRelEmpresaContratista", e, AccionEnum.CONSULTA, objRel.id, JsonUtils.convertNetObjectToJson(objRel));
                return false;
            }
        }
        #endregion
    }
}
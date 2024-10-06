using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Core.Entity.Principal.Usuarios;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Bitacoras;
using Core.Entity.Principal.Menus;
using Core.Entity.Maquinaria.SOS;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Reporte;
using Core.Entity.SeguimientoAcuerdos;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Alertas;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Encuestas;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Proyecciones;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Facturacion.Prefacturacion;
using Core.Entity.Administrativo.cotizaciones;
using Core.Entity.Maquinaria.Inventario.Movimiento_Interno;
using Core.Entity.Cursos;
using Core.Entity.Maquinaria.Mantenimiento;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Kubrix.Analisis;
using Core.Entity.Kubrix;
using Core.Entity.Administrativo.Facultamiento;
using Core.Entity.RecursosHumanos.Reportes;
using Core.DTO.Maquinaria.Mantenimiento;
using Core.Entity.Administrativo.ControlInterno.Almacen;
using Core.Entity.Administrativo.Seguridad;
using Core.Entity.MAZDA;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Sistemas;
using Core.DTO;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.GestorArchivos;
using Core.Entity.Maquinaria.Catalogo.Cararatulas;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Entity.ControlObra;
using Core.Entity.Administrativo.ReservacionVehiculo;
using Data.DAO.Maquinaria.Inventario;
using Core.Entity.FileManager;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using Core.Entity.Facturacion.Estimacion;
using Core.Entity.Maquinaria.Mantenimiento2;
using Core.Entity.Maquinaria.Barrenacion;
using Core.Entity.Administrativo.Seguridad.Capacitacion;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.GestorCorporativo;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using Core.Entity.Maquinaria.Rentabilidad;
using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using Core.Entity.Administrativo.Seguridad.Evaluacion;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using Core.Entity.Administrativo.Contabilidad.Cheques;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.DocumentosXPagar;
using Core.DTO.Utils.Data;
using System.Data.SqlClient;
using System.Data;
using Core.Entity.Principal.Configuraciones;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using Core.Entity.RecursosHumanos.Desempeno;
using Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar;
using Core.Entity.Administrativo.ControlInterno.Obra;
using Core.Entity.Encuestas.Proveedores;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using Core.Enum.Multiempresa;
using System.Configuration;
using Core.Entity.Maquinaria.KPI;
using Core.Entity.AdministradorProyectos.CGP;
using Core.Entity.Administrativo.RecursosHumanos.Mural;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
using Core.Entity.Administrativo.Seguridad.CatHorasHombre;
using Core.Entity.Administrativo.Seguridad.MedioAmbiente;
using Infrastructure.Utils;
using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using Core.Entity.Maquinaria.BackLogs;
using Core.Entity.Administrativo.Seguridad.CatDepartamentos;
using Dapper;
using Core.Entity.Administrativo.Contratistas;
using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using Core.Entity.SubContratistas;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.Entity.Administrativo.Almacen;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Entity.Maquinaria.Caratulas;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Casos_especiales;
using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Administrativo.Contabilidad.ControlPresupuestal;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia;
using Core.Entity.Administrativo.DocumentosXPagar.Especiales;
using Core.Entity.Principal.Catalogos;
using Core.Entity.SAAP;
using Core.Entity.ControlObra.MatrizDeRiesgo;
using Core.Entity.ControlObra.GestionDeCambio;
using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using Core.Entity.RecursosHumanos.Bajas;
using Core.Entity.Administrativo.RecursosHumanos.Reclutamientos;
using Core.Entity.Administrativo.ControlPresupuestalOficinasCentrales;
using Core.Entity.RecursosHumanos.Vacaciones;
using Core.Entity.Administrativo.CtrlPresupuestalOficinasCentrales;
using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using Core.Entity.Administrativo.TransferenciasBancarias;
using Core.Entity.AdministradorProyectos;
using Core.Entity.RecursosHumanos.BajasPersonal;
using Core.Entity.SeguimientoCompromisos;
using Core.Entity.ControlObra.Evaluacion;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Enkontrol.Compras;
using Core.Entity.Administrativo.RecursosHumanos.Capacitacion;
using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using Core.Entity.Administrativo.AgendarJunta;
using Core.Entity.Administrativo.SalaJuntas;
using Core.DTO.RecursosHumanos.Constancias;
using Core.Entity.RecursosHumanos.Reportes;
using Core.Entity.RecursosHumanos.Evaluacion360;
using Core.Entity.RecursosHumanos.ActoCondicion;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.RecursosHumanos.Demandas;
using Core.Entity.CuentasPorCobrar;
using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using Core.Entity.Administrativo.RecursosHumanos.Starsoft;
using Core.Entity.RecursosHumanos.CatNotificantes;
using Core.Entity.Maquinaria.StandBy;
using Core.Entity.Administrativo.Comercializacion.CRM;

namespace Data.EntityFramework.Context
{
    public class MainContext : DbContext
    {
        #region CONTROL OBRA

        public DbSet<tblCO_ADP_EvalSubContratista> tblCO_ADP_EvalSubContratista { get; set; }
        public DbSet<tblCO_ADP_EvalSubContratistaDet> tblCO_ADP_EvalSubContratistaDet { get; set; }
        public DbSet<tblCO_ADP_EvaluacionDiv> tblCO_ADP_EvaluacionDiv { get; set; }
        public DbSet<tblCO_ADP_EvaluacionReq> tblCO_ADP_EvaluacionReq { get; set; } 

        #endregion

        #region Maquinaria.KPI
        public DbSet<tblM_KPI_CodigosParo> tblM_KPI_CodigosParo { get; set; }
        public DbSet<tblM_KPI_Homologado> tblM_KPI_Homologado { get; set; }
        public DbSet<tblM_KPI_Homologado_Turnos> tblM_KPI_Homologado_Turnos { get; set; }
        public DbSet<tblM_KPI_AuthHomologado> tblM_KPI_AuthHomologado { get; set; }
        public DbSet<tblM_KPI_KPICapturaBit> tblM_KPI_KPICapturaBit { get; set; }
        #endregion

        #region SEGURIDAD / CATALOGO HORAS HOMBRE
        public DbSet<tblS_CatHorasHombre> tblS_CatHorasHombre { get; set; }
        #endregion

        #region MAQUINARIA / BACKLOGS
        public DbSet<tblBL_CatBackLogs> tblBL_CatBackLogs { get; set; }
        public DbSet<tblBL_CatSubconjuntos> tblBL_CatSubconjuntos { get; set; }
        public DbSet<tblBL_CatConjuntos> tblBL_CatConjuntos { get; set; }
        public DbSet<tblBL_Partes> tblBL_Partes { get; set; }
        public DbSet<tblBL_ManoObra> tblBL_ManoObra { get; set; }
        public DbSet<tblBL_Requisiciones> tblBL_Requisiciones { get; set; }
        public DbSet<tblBL_OrdenesCompra> tblBL_OrdenesCompra { get; set; }
        public DbSet<tblBL_Inspecciones> tblBL_Inspecciones { get; set; }
        public DbSet<tblBL_CatFrentes> tblbl_CatFrentes { get; set; }
        public DbSet<tblBL_SeguimientoPptos> tblBL_SeguimientoPptos { get; set; }
        public DbSet<tblBL_InspeccionesTMC> tblBL_InspeccionesTMC { get; set; }
        public DbSet<tblBL_DetFrentes> tblBL_DetFrentes { get; set; }
        public DbSet<tblBL_Evidencias> tblBL_Evidencias { get; set; }
        public DbSet<tblBL_MotivoCancelacionReq> tblBL_MotivoCancelacionReq { get; set; }
        public DbSet<tblBL_BitacoraEstatusBL> tblBL_BitacoraEstatusBL { get; set; }
        public DbSet<tblBL_OT> tblBL_OT { get; set; }
        public DbSet<tblBL_FoliosTraspasos> tblBL_FoliosTraspasos { get; set; }
        #endregion

        #region SEGURIDAD / CATALOGO DEPARTAMENTOS
        public DbSet<tblS_CatDepartamentos> tblS_CatDepartamentos { get; set; }
        public DbSet<tblS_CatAreasOperativas> tblS_CatAreasOperativas { get; set; }
        #endregion

        #region ADMIN Y FINANZAS / CATALOGO DIVISIONES
        public DbSet<tblAF_DxP_Divisiones> tblAF_DxP_Divisiones { get; set; }
        public DbSet<tblAF_DxP_Divisiones_Proyecto> tblAF_DxP_Divisiones_Proyecto { get; set; }
        #endregion

        #region SEGURIDAD / CATÁLOGO AGRUPACIÓN CONTRATISTAS
        public DbSet<tblS_IncidentesAgrupacionContratistas> tblS_IncidentesAgrupacionContratistas { get; set; }
        public DbSet<tblS_IncidentesAgrupacionContratistasDet> tblS_IncidentesAgrupacionContratistasDet { get; set; }
        public DbSet<tblS_IncidentesRelEmpresaContratistas> tblS_IncidentesRelEmpresaContratistas { get; set; }

        public DbSet<tblS_IncidentesSubcontratistas> tblS_IncidentesSubcontratistas { get; set; }
        #endregion

        #region CH / BAJAS DE PERSONAL
        public DbSet<tblRH_Baja_Entrevista> tblRH_Baja_Entrevista { get; set; }
        public DbSet<tblRH_Baja_Entrevista_Conceptos> tblRH_Baja_Entrevista_Conceptos { get; set; }
        public DbSet<tblRH_Baja_Registro> tblRH_Baja_Registro { get; set; }
        public DbSet<tblRH_Baja_Finiquitos> tblRH_Baja_Finiquitos { get; set; }
        public DbSet<tblRH_Baja_AutorizantesCancel> tblRH_Baja_AutorizantesCancel { get; set; }
        public DbSet<tblRH_Bajas_DiasPermitidos> tblRH_Bajas_DiasPermitidos { get; set; }
        #endregion

        #region RH EK
        public DbSet<tblRH_EK_Bancos> tblRH_EK_Bancos { get; set; }
        public DbSet<tblRH_EK_Cuidades> tblRH_EK_Cuidades { get; set; }
        public DbSet<tblRH_EK_Contratos_Empleados> tblRH_EK_Contratos_Empleados { get; set; }
        public DbSet<tblRH_EK_Departamentos> tblRH_EK_Departamentos { get; set; }
        public DbSet<tblRH_EK_Empl_Baja> tblRH_EK_Empl_Baja { get; set; }
        public DbSet<tblRH_EK_Empl_CC_Historial> tblRH_EK_Empl_CC_Historial { get; set; }
        public DbSet<tblRH_EK_Empl_Complementaria> tblRH_EK_Empl_Complementaria { get; set; }
        public DbSet<tblRH_EK_Empl_Duracion_Contrato> tblRH_EK_Empl_Duracion_Contrato { get; set; }
        public DbSet<tblRH_EK_Empl_Familia> tblRH_EK_Empl_Familia { get; set; }
        public DbSet<tblRH_EK_Empl_Grales> tblRH_EK_Empl_Grales { get; set; }
        public DbSet<tblRH_EK_Empl_Recontratacion> tblRH_EK_Empl_Recontratacion { get; set; }
        public DbSet<tblRH_EK_Empleados> tblRH_EK_Empleados { get; set; }
        public DbSet<tblRH_EK_SustentosHijos> tblRH_EK_SustentosHijos { get; set; }
        public DbSet<tblRH_EK_Geo_Departamentos> tblRH_EK_Geo_Departamentos { get; set; }
        public DbSet<tblRH_EK_Estados> tblRH_EK_Estados { get; set; }
        public DbSet<tblRH_EK_Incidencias_Conceptos> tblRH_EK_Incidencias_Conceptos { get; set; }
        public DbSet<tblRH_EK_Paises> tblRH_EK_Paises { get; set; }
        public DbSet<tblRH_EK_Parentesco> tblRH_EK_Parentesco { get; set; }
        public DbSet<tblRH_EK_Periodos> tblRH_EK_Periodos { get; set; }
        public DbSet<tblRH_EK_Plantilla_Aditiva> tblRH_EK_Plantilla_Aditiva { get; set; }
        public DbSet<tblRH_EK_Plantilla_Personal> tblRH_EK_Plantilla_Personal { get; set; }
        public DbSet<tblRH_EK_Plantilla_Puesto> tblRH_EK_Plantilla_Puesto { get; set; }
        public DbSet<tblRH_EK_Puestos> tblRH_EK_Puestos { get; set; }
        public DbSet<tblRH_EK_Razones_Baja> tblRH_EK_Razones_Baja { get; set; }
        public DbSet<tblRH_EK_Registros_Patronales> tblRH_EK_Registros_Patronales { get; set; }
        public DbSet<tblRH_EK_Requisicion_Personal> tblRH_EK_Requisicion_Personal { get; set; }
        public DbSet<tblRH_EK_Requisicion_Razon> tblRH_EK_Requisicion_Razon { get; set; }
        public DbSet<tblRH_EK_Requisicion_Tipo_Contrato> tblRH_EK_Requisicion_Tipo_Contrato { get; set; }
        public DbSet<tblRH_EK_Tabulador_Historial> tblRH_EK_Tabulador_Historial { get; set; }
        public DbSet<tblRH_EK_Tabulador_Puesto> tblRH_EK_Tabulador_Puesto { get; set; }
        public DbSet<tblRH_EK_Tabuladores> tblRH_EK_Tabuladores { get; set; }
        public DbSet<tblRH_EK_Tipos_Nomina> tblRH_EK_Tipos_Nomina { get; set; }
        public DbSet<tblRH_EK_Turnos> tblRH_EK_Turnos { get; set; }
        public DbSet<tblRH_EK_Unidad_Medica> tblRH_EK_Unidad_Medica { get; set; }
        public DbSet<tblRH_EK_Zonas_Econom> tblRH_EK_Zonas_Econom { get; set; }
        public DbSet<tblRH_EK_CatTipoEmpleados> tblRH_EK_CatTipoEmpleados { get; set; }
        #endregion

        #region RH STARSOFT
        public DbSet<tblRH_SS_TiposTrab> tblRH_SS_TiposTrab { get; set; }
        public DbSet<tblRH_SS_Bancos> tblRH_SS_Bancos { get; set; }
        public DbSet<tblRH_SS_RegimenLaboral> tblRH_SS_RegimenLaboral { get; set; }
        
        #endregion

        #region CH / RECLUTAMIENTOS
        public DbSet<tblRH_REC_Solicitudes> tblRH_REC_Solicitudes { get; set; }
        public DbSet<tblRH_REC_CatMotivos> tblRH_REC_CatMotivos { get; set; }
        public DbSet<tblRH_REC_CatEscolaridades> tblRH_REC_CatEscolaridades { get; set; }
        public DbSet<tblRH_REC_GestionSolicitudes> tblRH_REC_GestionSolicitudes { get; set; }
        public DbSet<tblRH_REC_GestionCandidatos> tblRH_REC_GestionCandidatos { get; set; }
        public DbSet<tblRH_REC_Fases> tblRH_REC_Fases { get; set; }
        public DbSet<tblRH_REC_Actividades> tblRH_REC_Actividades { get; set; }
        public DbSet<tblRH_REC_PuestosRelFases> tblRH_REC_PuestosRelFases { get; set; }
        public DbSet<tblRH_REC_SegCandidatos> tblRH_REC_SegCandidatos { get; set; }
        public DbSet<tblRH_REC_Archivos> tblRH_REC_Archivos { get; set; }
        public DbSet<tblRH_REC_Empleados> tblRH_REC_Empleados { get; set; }
        public DbSet<tblRH_REC_EmplFamiliares> tblRH_REC_EmplFamiliares { get; set; }
        public DbSet<tblRH_REC_SegDetCandidatos> tblRH_REC_SegDetCandidatos { get; set; }
        public DbSet<tblRH_REC_EmplGenContacto> tblRH_REC_EmplGenContacto { get; set; }
        public DbSet<tblRH_REC_EmplContEmergencias> tblRH_REC_EmplContEmergencias { get; set; }
        public DbSet<tblRH_REC_EmplBeneficiarios> tblRH_REC_EmplBeneficiarios { get; set; }
        public DbSet<tblRH_REC_EmplCompania> tblRH_REC_EmplCompania { get; set; }
        public DbSet<tblRH_REC_CatTipoFormulaIMSS> tblRH_REC_CatTipoFormulaIMSS { get; set; }
        public DbSet<tblRH_REC_Uniformes> tblRH_REC_Uniformes { get; set; }
        public DbSet<tblRH_REC_Tabuladores> tblRH_REC_Tabuladores { get; set; }
        public DbSet<tblRH_REC_TabuladoresPuesto> tblRH_REC_TabuladoresPuesto { get; set; }
        public DbSet<tblRH_REC_CatBancos> tblRH_REC_CatBancos { get; set; }
        public DbSet<tblRH_REC_CatPlataformas> tblRH_REC_CatPlataformas { get; set; }
        public DbSet<tblRH_REC_EntrevistasIniciales> tblRH_REC_EntrevistasIniciales { get; set; }
        public DbSet<tblRH_REC_CatCorreos> tblRH_REC_CatCorreos { get; set; }
        public DbSet<tblRH_REC_ED_Archivo> tblRH_REC_ED_Archivo { get; set; }
        public DbSet<tblRH_REC_ED_Expediente> tblRH_REC_ED_Expediente { get; set; }
        public DbSet<tblRH_REC_ED_RelacionExpedienteArchivo> tblRH_REC_ED_RelacionExpedienteArchivo { get; set; }
        public DbSet<tblRH_REC_FasesCC> tblRH_REC_FasesCC { get; set; }
        public DbSet<tblRH_REC_FasesUsuarios> tblRH_REC_FasesUsuarios { get; set; }
        public DbSet<tblRH_REC_PermisoTabuladorLibre> tblRH_REC_PermisoTabuladorLibre { get; set; }
        public DbSet<tblRH_REC_PuestoSindicato> tblRH_REC_PuestoSindicato { get; set; }
        public DbSet<tblRH_REC_RelacionRegistroPatronalCC> tblRH_REC_RelacionRegistroPatronalCC { get; set; }
        public DbSet<tblRH_REC_Requisicion> tblRH_REC_Requisicion { get; set; }
        public DbSet<tblRH_REC_Requisicion_UsuariosAdmn> tblRH_REC_Requisicion_UsuariosAdmn { get; set; }
        public DbSet<tblRH_REC_Comentarios> tblRH_REC_Comentarios { get; set; }
        public DbSet<tblRH_REC_TipoArchivo> tblRH_REC_TipoArchivo { get; set; }
        public DbSet<tblRH_UsuariosCorreoAutorizacionAlta> tblRH_UsuariosCorreoAutorizacionAlta { get; set; }
        public DbSet<tblRH_REC_MotivoSueldo> tblRH_REC_MotivoSueldo { get; set; }
        public DbSet<tblRH_REC_Notificantes_Actividades> tblRH_REC_Notificantes_Actividades { get; set; }
        public DbSet<tblRH_REC_DiasPermitidos> tblRH_REC_DiasPermitidos { get; set; }
        public DbSet<tblRH_REC_Notificantes_Tabulador> tblRH_REC_Notificantes_Tabulador { get; set; }
        public DbSet<tblRH_REC_Notificantes_Rel_Candidato> tblRH_REC_Notificantes_Rel_Candidato { get; set; }
        public DbSet<tblRH_REC_Notificantes_Altas> tblRH_REC_Notificantes_Altas { get; set; }
        public DbSet<tblRH_REC_ExamenMedico> tblRH_REC_ExamenMedico { get; set; }
        public DbSet<tblRH_REC_ExamenMedico_Antecedentes> tblRH_REC_ExamenMedico_Antecedentes { get; set; }
        public DbSet<tblRH_REC_Expediciones> tblRH_REC_Expediciones { get; set; }
        public DbSet<tblRH_REC_Expediciones_Archivos> tblRH_REC_Expediciones_Archivos { get; set; }
        public DbSet<tblRH_REC_OcultarCC> tblRH_REC_OcultarCC { get; set; }
        public DbSet<tblRH_REC_InfoEmpleadoPeru> tblRH_REC_InfoEmpleadoPeru { get; set; }
        #endregion

        #region CH / VACACIONES
        public DbSet<tblRH_Vacaciones_Periodos> tblRH_Vacaciones_Periodos { get; set; }
        public DbSet<tblRH_Vacaciones_Vacaciones> tblRH_Vacaciones_Vacaciones { get; set; }
        public DbSet<tblRH_Vacaciones_Fechas> tblRH_Vacaciones_Fechas { get; set; }
        public DbSet<tblRH_Vacaciones_Responsables> tblRH_Vacaciones_Responsables { get; set; }
        public DbSet<tblRH_Vacaciones_Responsables_Dias> tblRH_Vacaciones_Responsables_Dias { get; set; }
        public DbSet<tblRH_Vacaciones_Incapacidades> tblRH_Vacaciones_Incapacidades { get; set; }
        public DbSet<tblRH_Vacaciones_Archivos> tblRH_Vacaciones_Archivos { get; set; }
        public DbSet<tblRH_Vacaciones_TipoArchivo> tblRH_Vacaciones_TipoArchivo { get; set; }
        public DbSet<tblRH_Vacaciones_Gestion> tblRH_Vacaciones_Gestion { get; set; }
        public DbSet<tblRH_Vacaciones_Saldos> tblRH_Vacaciones_Saldos { get; set; }
        public DbSet<tblRH_Vacaciones_Retardos> tblRH_Vacaciones_Retardos { get; set; }
        public DbSet<tblRH_Vacaciones_Retardos_Motivos> tblRH_Vacaciones_Retardos_Motivos { get; set; }
        public DbSet<tblRH_Vacaciones_Retardos_Gestion> tblRH_Vacaciones_Retardos_Gestion { get; set; }
        
        #endregion

        #region MEDIO AMBIENTE
        public DbSet<tblS_MedioAmbienteAspectoAmbiental> tblS_MedioAmbienteAspectoAmbiental { get; set; }
        public DbSet<tblS_MedioAmbienteResiduoFactorPeligro> tblS_MedioAmbienteResiduoFactorPeligro { get; set; }
        public DbSet<tblS_MedioAmbienteClasificacion> tblS_MedioAmbienteClasificacion { get; set; }
        public DbSet<tblS_MedioAmbienteCaptura> tblS_MedioAmbienteCaptura { get; set; }
        public DbSet<tblS_MedioAmbienteTransportistas> tblS_MedioAmbienteTransportistas { get; set; }
        public DbSet<tblS_MedioAmbienteClasificacionesTransportistas> tblS_MedioAmbienteClasificacionesTransportistas { get; set; }
        public DbSet<tblS_MedioAmbienteCapturaDet> tblS_MedioAmbienteCapturaDet { get; set; }
        public DbSet<tblS_MedioAmbienteArchivos> tblS_MedioAmbienteArchivos { get; set; }
        public DbSet<tblS_MedioAmbienteTrayectos> tblS_MedioAmbienteTrayectos { get; set; }
        public DbSet<tblS_MedioAmbienteDestinoFinal> tblS_MedioAmbienteDestinoFinal { get; set; }
        #endregion
        
        #region TABLAS PRINCIPALES
        public DbSet<tblP_CatTipoSangre> tblP_CatTipoSangre { get; set; }
        public DbSet<tblP_CatTipoCasa> tblP_CatTipoCasa { get; set; }
        public DbSet<tblP_CatEscolaridades> tblP_CatEscolaridades { get; set; }
        public DbSet<tblP_EstadoCivil> tblP_EstadoCivil { get; set; }
        #endregion

        #region SALUD OCUPACIONAL
        public DbSet<tblS_SO_Medicos> tblS_SO_Medicos { get; set; }
        public DbSet<tblS_SO_AtencionMedica> tblS_SO_AtencionMedica { get; set; }
        public DbSet<tblS_SO_AtencionMedica_Revision> tblS_SO_AtencionMedica_Revision { get; set; }
        public DbSet<tblS_SO_HistorialesClinicos> tblS_SO_HistorialesClinicos { get; set; }
        public DbSet<tblS_SO_Archivos> tblS_SO_Archivos { get; set; }
        #endregion

        #region CONTROL PRESUPUESTAL OFICINAS CENTRALES
        public DbSet<tblAF_CtrlPptalOfCe_ControlImpactos> tblAF_CtrlPptalOfCe_ControlImpactos { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_MatrizEstrategicas> tblAF_CtrlPresupuestalOfCe_MatrizEstrategicas { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CatConceptos> tblAF_CtrlPptalOfCe_CatConceptos { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CapPptos> tblAF_CtrlPptalOfCe_CapPptos { get; set; }
        public DbSet<tblAF_CtrllPptalOfCe_CatAgrupaciones> tblAF_CtrllPptalOfCe_CatAgrupaciones { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_PptoAnual> tblAF_CtrlPptalOfCe_PptoAnual { get; set; }
        public DbSet<tblAF_CtrlAutorizacionPresupuesto> tblAF_CtrlAutorizacionPresupuesto { get; set; }
        public DbSet<tblAF_CtrllPptalOfCe_CatAutorizantes> tblAF_CtrllPptalOfCe_CatAutorizantes { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_PptoInicial> tblAF_CtrlPptalOfCe_PptoInicial { get; set; }
        public DbSet<tblAF_CtrlAditiva> tblAF_CtrlAditiva { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_UsuarioRelCC> tblAF_CtrlPptalOfCe_UsuarioRelCC { get; set; }
        public DbSet<tblAF_CtrlAutorizacionAditiva> tblAF_CtrlAutorizacionAditiva { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_RN_CatAgrupaciones> tblAF_CtrlPptalOfCe_RN_CatAgrupaciones { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_RN_CatConceptos> tblAF_CtrlPptalOfCe_RN_CatConceptos { get; set; }
        public DbSet<tblAF_CtrlCuenta> tblAF_CtrlCuenta { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_RN_PlanMaestro> tblAF_CtrlPptalOfCe_RN_PlanMaestro { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_RN_MedicionesIndicadores> tblAF_CtrlPptalOfCe_RN_MedicionesIndicadores { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_RN_PlanMaestroDet> tblAF_CtrlPptalOfCe_RN_PlanMaestroDet { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CatCtaInsumos> tblAF_CtrlPptalOfCe_CatCtaInsumos { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CatCtaInsumos_Cuentas> tblAF_CtrlPptalOfCe_CatCtaInsumos_Cuentas { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CCRelServicio> tblAF_CtrlPptalOfCe_CCRelServicio { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CCRelServicios> tblAF_CtrlPptalOfCe_CCRelServicios { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_Comentarios> tblAF_CtrlPptalOfCe_Comentarios { get; set; }
        public DbSet<tblAF_CtrlAutorizanteAditiva> tblAF_CtrlAutorizanteAditiva { get; set; }
        public DbSet<tblAF_CtrlPptal_GastoIngresoRatio> tblAF_CtrlPptal_GastoIngresoRatio { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_PlanAccion> tblAF_CtrlPptalOfCe_PlanAccion { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_BaseConceptos> tblAF_CtrlPptalOfCe_BaseConceptos { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_CatPorcIngresosPptoAnual> tblAF_CtrlPptalOfCe_CatPorcIngresosPptoAnual { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_EnvioInforme> tblAF_CtrlPptalOfCe_EnvioInforme { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_Mensaje> tblAF_CtrlPptalOfCe_Mensaje { get; set; }
        public DbSet<tblC_Cta_RelCC> tblC_Cta_RelCC { get; set; }
        public DbSet<tblAF_CtrlPptalOfCe_EstimadoRealMensual> tblAF_CtrlPptalOfCe_EstimadoRealMensual { get; set; }
        #endregion

        #region ADMIN DE PROYECTOS / SUBCONTRATISTAS
        public DbSet<tblCO_ADP_UsuariosFirmantesRelSubcontratistas> tblCO_ADP_UsuariosFirmantesRelSubcontratistas { get; set; }
        #endregion

        #region CH / EVALUACION 360
        public DbSet<tblRH_Eval360_CatCompetencias> tblRH_Eval360_CatCompetencias { get; set; }
        public DbSet<tblRH_Eval360_CatConductas> tblRH_Eval360_CatConductas { get; set; }
        public DbSet<tblRH_Eval360_CatCriterios> tblRH_Eval360_CatCriterios { get; set; }
        public DbSet<tblRH_Eval360_CatGrupos> tblRH_Eval360_CatGrupos { get; set; }
        public DbSet<tblRH_Eval360_CatPersonal> tblRH_Eval360_CatPersonal { get; set; }
        public DbSet<tblRH_Eval360_CatTipoUsuarios> tblRH_Eval360_CatTipoUsuarios { get; set; }
        public DbSet<tblRH_Eval360_Cuestionarios> tblRH_Eval360_Cuestionarios { get; set; }
        public DbSet<tblRH_Eval360_CuestionariosDet> tblRH_Eval360_CuestionariosDet { get; set; }
        public DbSet<tblRH_Eval360_CatPeriodos> tblRH_Eval360_CatPeriodos { get; set; }
        public DbSet<tblRH_Eval360_CatPlantillas> tblRH_Eval360_CatPlantillas { get; set; }
        public DbSet<tblRH_Eval360_Relaciones> tblRH_Eval360_Relaciones { get; set; }
        public DbSet<tblRH_Eval360_RelacionesDet> tblRH_Eval360_RelacionesDet { get; set; }
        public DbSet<tblRH_Eval360_EvaluacionesEvaluador> tblRH_Eval360_EvaluacionesEvaluador { get; set; }
        public DbSet<tblRH_Eval360_EvaluacionesEvaluadorDet> tblRH_Eval360_EvaluacionesEvaluadorDet { get; set; }
        public DbSet<tblRH_Eval360_BitacoraEnvioCorreos> tblRH_Eval360_BitacoraEnvioCorreos { get; set; }
        #endregion

        #region CH / DEMANDAS
        public DbSet<tblRH_DMS_CapturaDemandas> tblRH_DMS_CapturaDemandas { get; set; }
        public DbSet<tblRH_DMS_ArchivosAdjuntos> tblRH_DMS_ArchivosAdjuntos { get; set; }
        public DbSet<tblRH_DMS_SeguimientoDemanda> tblRH_DMS_SeguimientoDemanda { get; set; }
        #endregion

        #region CH / TABULADORES
        public DbSet<tblRH_TAB_CatAreaDepartamento> tblRH_TAB_CatAreaDepartamento { get; set; }
        public DbSet<tblRH_TAB_CatCategorias> tblRH_TAB_CatCategorias { get; set; }
        public DbSet<tblRH_TAB_CatEsquemaPago> tblRH_TAB_CatEsquemaPago { get; set; }
        public DbSet<tblRH_TAB_CatLineaNegocio> tblRH_TAB_CatLineaNegocio { get; set; }
        public DbSet<tblRH_TAB_CatLineaNegocioDet> tblRH_TAB_CatLineaNegocioDet { get; set; }
        public DbSet<tblRH_TAB_CatNivelMando> tblRH_TAB_CatNivelMando { get; set; }
        public DbSet<tblRH_TAB_CatSindicato> tblRH_TAB_CatSindicato { get; set; }
        public DbSet<tblRH_TAB_CatTipoModificacion> tblRH_TAB_CatTipoModificacion { get; set; }
        public DbSet<tblRH_TAB_Tabuladores> tblRH_TAB_Tabuladores { get; set; }
        public DbSet<tblRH_TAB_TabuladoresDet> tblRH_TAB_TabuladoresDet { get; set; }
        public DbSet<tblRH_TAB_GestionAutorizantes> tblRH_TAB_GestionAutorizantes { get; set; }
        public DbSet<tblRH_TAB_PlantillasPersonal> tblRH_TAB_PlantillasPersonal { get; set; }
        public DbSet<tblRH_TAB_PlantillasPersonalDet> tblRH_TAB_PlantillasPersonalDet { get; set; }
        public DbSet<tblRH_TAB_GestionModificacionTabulador> tblRH_TAB_GestionModificacionTabulador { get; set; }
        public DbSet<tblRH_TAB_GestionModificacionTabuladorDet> tblRH_TAB_GestionModificacionTabuladorDet { get; set; }
        public DbSet<tblRH_TAB_TabuladoresHistorial> tblRH_TAB_TabuladoresHistorial { get; set; }
        public DbSet<tblRH_TAB_TabuladoresDetHistorial> tblRH_TAB_TabuladoresDetHistorial { get; set; }
        public DbSet<tblRH_TAB_AccesosMenu> tblRH_TAB_AccesosMenu { get; set; }
        public DbSet<tblRH_TAB_Reportes> tblRH_TAB_Reportes { get; set; }
        public DbSet<tblRH_TAB_Reporte_LineaNegocio> tblRH_TAB_Reporte_LineaNegocio { get; set; }
        public DbSet<tblRH_TAB_Reporte_Puestos> tblRH_TAB_Reporte_Puestos { get; set; }
        public DbSet<tblRH_TAB_Reporte_CC> tblRH_TAB_Reporte_CC { get; set; }
        
        #endregion

        #region CH / PUESTOS
        public DbSet<tblRH_EK_Puesto_ArchivoDescriptor> tblRH_EK_Puesto_ArchivoDescriptor { get; set; }
        #endregion

        #region CH / CAT NOTIFICANTES
        public DbSet<tblRH_Notis_Conceptos> tblRH_Notis_Conceptos { get; set; }
        public DbSet<tblRH_Notis_RelConceptoUsuario> tblRH_Notis_RelConceptoUsuario { get; set; }
        public DbSet<tblRH_Notis_RelConceptoCorreo> tblRH_Notis_RelConceptoCorreo { get; set; }

        #endregion

        #region ADMIN Y FINANZAS / CRM
        public DbSet<tblAF_CRM_Clientes> tblAF_CRM_Clientes { get; set; }
        public DbSet<tblAF_CRM_Contactos> tblAF_CRM_Contactos { get; set; }
        public DbSet<tblAF_CRM_Proyectos> tblAF_CRM_Proyectos { get; set; }
        public DbSet<tblAF_CRM_CatPrioridades> tblAF_CRM_CatPrioridades { get; set; }
        public DbSet<tblAF_CRM_CatPrioridadEstatus> tblAF_CRM_CatPrioridadEstatus { get; set; }
        public DbSet<tblAF_CRM_Divisiones> tblAF_CRM_Divisiones { get; set; }
        public DbSet<tblAF_CRM_ComentariosProyectos> tblAF_CRM_ComentariosProyectos { get; set; }
        public DbSet<tblAF_CRM_ProximasAcciones> tblAF_CRM_ProximasAcciones { get; set; }
        public DbSet<tblAF_CRM_ProximasAccionesDetalle> tblAF_CRM_ProximasAccionesDetalle { get; set; }
        public DbSet<tblAF_CRM_CatRiesgos> tblAF_CRM_CatRiesgos { get; set; }
        public DbSet<tblAF_CRM_Cotizaciones> tblAF_CRM_Cotizaciones { get; set; }
        public DbSet<tblAF_CRM_CatCanales> tblAF_CRM_CatCanales { get; set; }
        public DbSet<tblAF_CRM_CatCanalesDivisiones> tblAF_CRM_CatCanalesDivisiones { get; set; }
        public DbSet<tblAF_CRM_UsuariosCRM> tblAF_CRM_UsuariosCRM { get; set; }
        #endregion

        public DbSet<tblAP_Rep_Costos_Accesos> tblAP_Rep_Costos_Accesos { get; set; }
        public DbSet<tblAlm_RelAreaCuentaXAlmacen> tblAlm_RelAreaCuentaXAlmacen { get; set; }
        public DbSet<tblAlm_RelAreaCuentaXAlmacenDet> tblAlm_RelAreaCuentaXAlmacenDet { get; set; }
        public DbSet<tblAlm_Ubicacion> tblAlm_Ubicacion { get; set; }
        public DbSet<tblAlm_Grupos_Insumo> tblAlm_Grupos_Insumo { get; set; }

        public DbSet<tblM_CatMarcaEquipotblM_CatGrupoMaquinaria> tblM_CatMarcaEquipotblM_CatGrupoMaquinaria { get; set; }
        public DbSet<tblP_RolesGrupoTrabajo> tblP_RolesGrupoTrabajo { get; set; }
        public DbSet<tblB_CapturaLitrosAgua> tblB_CapturaLitrosAgua { get; set; }
        public DbSet<tblAF_DxP_ProgramacionPagos> tblAF_DxP_ProgramacionPagos { get; set; }
        //public DbSet<tblAF_DxP_ContratoMaquina> tblAF_DxP_ContratoMaquina { get; set; }
        public DbSet<tblM_ControlMovimientoMaquinaria> tblM_ControlMovimientoMaquinaria { get; set; }
        public DbSet<tblM_PMComponenteFiltro> tblM_PMComponenteFiltro { get; set; }
        public DbSet<tblM_PMComponenteLubricante> tblM_PMComponenteLubricante { get; set; }
        public DbSet<tblM_catAutorizaciones> tblM_catAutorizaciones { get; set; }
        public DbSet<tblM_BitacoraDetActividadesMantProy> tblM_BitacoraDetActividadesMantProy { get; set; }
        public DbSet<tblP_DirArchivos> tblP_DirArchivos { get; set; }
        public DbSet<tblM_AutorizaConciliacionHorometros> tblM_AutorizaConciliacionHorometros { get; set; }
        public DbSet<tblM_CapEncConciliacionHorometros> tblM_CapEncConciliacionHorometros { get; set; }
        public DbSet<tblP_CCRH> tblP_CCRH { get; set; }
        public DbSet<tblM_CorreosEnvioInventario> tblM_CorreosEnvioInventario { get; set; }
        public DbSet<tblM_CorteInventarioMaq> tblM_CorteInventarioMaq { get; set; }
        public DbSet<tblM_CorteInventarioMaq_Detalle> tblM_CorteInventarioMaq_Detalle { get; set; }
        public DbSet<tblAlm_MergeInsumos> tblAlm_MergeInsumos { get; set; }
        public DbSet<tblRH_LayautBajaEmpleados> tblRH_LayautBajaEmpleados { get; set; }
        public DbSet<tblM_CatJornadasPersonalCC> tblM_CatJornadasPersonalCC { get; set; }
        public DbSet<tblM_CatPuestosMaquinaria> tblM_CatPuestosMaquinaria { get; set; }
        public DbSet<Core.Entity.Principal.Usuarios.tblP_Usuario> tblP_Usuario { get; set; }
        public DbSet<tblP_MenutblP_Usuario> tblP_MenutblP_Usuario { get; set; }
        public DbSet<tblP_MenutblP_Empresas> tblP_MenutblP_Empresas { get; set; }
        public DbSet<tblP_MenuUsuarioBloqueado> tblP_MenuUsuarioBloqueado { get; set; }
        public DbSet<tblP_UsuariotblP_Empresas> tblP_UsuariotblP_Empresas { get; set; }
        public DbSet<tblP_CC_Usuario> tblP_CC_Usuario { get; set; }
        public DbSet<tblP_AccionesVistatblP_Usuario> tblP_AccionesVistatblP_Usuario { get; set; }
        public DbSet<tblP_UsuarioFacultamientoFactura> tblP_UsuarioFacultamientoFactura { get; set; }
        public DbSet<tblP_Perfil> tblP_Perfil { get; set; }
        public DbSet<tblM_CatTipoMaquinaria> tblM_CatTipoMaquinaria { get; set; }
        public DbSet<tblM_CatModeloEquipo> tblM_CatModeloEquipo { get; set; }
        public DbSet<tblM_CatMarcaEquipo> tblM_CatMarcaEquipo { get; set; }
        public DbSet<tblM_CatGrupoMaquinaria> tblM_CatGrupoMaquinaria { get; set; }
        public DbSet<tblP_Bitacora> tblP_Bitacora { get; set; }
        public DbSet<tblM_CatAseguradora> tblM_CatAseguradora { get; set; }
        public DbSet<tblM_CatConjunto> tblM_CatConjunto { get; set; }
        public DbSet<tblM_CatSubConjunto> tblM_CatSubConjunto { get; set; }
        public DbSet<tblM_CatComponente> tblM_CatComponente { get; set; }
        public DbSet<tblM_CatMaquina> tblM_CatMaquina { get; set; }
        public DbSet<tblM_CatMaquina_EstatusDiario> tblM_CatMaquina_EstatusDiario { get; set; }
        public DbSet<tblM_CatMaquina_EstatusDiario_Det> tblM_CatMaquina_EstatusDiario_Det { get; set; }
        public DbSet<tblM_CatMaquina_EstatusDiario_Usuario_CC> tblM_CatMaquina_EstatusDiario_Usuario_CC { get; set; }
        public DbSet<tblM_CatMaquina_EstatusDiario_Economico_Especial> tblM_CatMaquina_EstatusDiario_Economico_Especial { get; set; }
        
        public DbSet<tblM_CatMarcasComponentes> tblM_CatMarcasComponentes { get; set; }
        public DbSet<tblM_FolioComponente> tblM_FolioComponente { get; set; }
        public DbSet<tblP_Menu> tblP_Menu { get; set; }
        public DbSet<tblP_Sistema> tblP_Sistema { get; set; }
        public DbSet<MinadoEntity> MinadoEntity { get; set; }

        public DbSet<tblP_ArchivosComprimir> tblP_ArchivosComprimir { get; set; }

        public DbSet<tblM_CapturaDatosDiariosMaquinaria> tblM_CapturaDatosDiariosMaquinaria { get; set; }
        public DbSet<tblM_CapRitmoHorometro> tblM_CapRitmoHorometro { get; set; }
        public DbSet<tblM_CapHorometro> tblM_CapHorometro { get; set; }
        public DbSet<tblM_CapCombustible> tblM_CapCombustible { get; set; }
        public DbSet<tblM_CapDesfase> tblM_CapDesfase { get; set; }
        public DbSet<tblM_CapPrecioDiesel> tblM_CapPrecioDiesel { get; set; }
        public DbSet<tblM_EconomicoPuedeAnsul> tblM_EconomicoPuedeAnsul { get; set; }
        public DbSet<tblM_DocumentoMantenimientoPM> tblM_DocumentoMantenimientoPM { get; set; }
        
        

        public DbSet<tblP_Encabezado> tblP_Encabezado { get; set; }
        public DbSet<tblSA_Minuta> tblSA_Minuta { get; set; }
        public DbSet<tblSA_Actividades> tblSA_Actividades { get; set; }
        public DbSet<tblSA_Comentarios> tblSA_Comentarios { get; set; }
        public DbSet<tblSA_Participante> tblSA_Participante { get; set; }
        public DbSet<tblP_Departamento> tblP_Departamento { get; set; }
        public DbSet<tblP_Puesto> tblP_Puesto { get; set; }
        public DbSet<tblSA_PromoverActividad> tblSA_PromoverActividad { get; set; }
        public DbSet<tblSA_Interesados> tblSA_Interesados { get; set; }
        public DbSet<tblM_CatRendimientoTeorico> tblM_CatRendimientoTeorico { get; set; }
        public DbSet<tblRH_FormatoCambio> tblRH_FormatoCambio { get; set; }
        public DbSet<tblRH_AutorizacionFormatoCambio> tblRH_AutorizacionFormatoCambio { get; set; }
        #region Bono
        public DbSet<tblRH_BN_REGLA_CC> tblRH_BN_REGLA_CC { get; set; }
        public DbSet<tblRH_BN_REGLA_Puestos> tblRH_BN_REGLA_Puestos { get; set; }
        public DbSet<tblRH_BN_EstatusPeriodos> tblRH_BN_EstatusPeriodos { get; set; }
        public DbSet<tblRH_BN_Usuario_CC> tblRH_BN_Usuario_CC { get; set; }
        public DbSet<tblRH_BN_CCExepcion> tblRH_BN_CCExepcion { get; set; }
        public DbSet<tblRH_BN_Evaluacion> tblRH_BN_Evaluacion { get; set; }
        public DbSet<tblRH_BN_Evaluacion_Aut> tblRH_BN_Evaluacion_Aut { get; set; }
        public DbSet<tblRH_BN_Evaluacion_Det> tblRH_BN_Evaluacion_Det { get; set; }

        public DbSet<tblRH_BN_Evento> tblRH_BN_Evento { get; set; }
        public DbSet<tblRH_BN_Evento_Aut> tblRH_BN_Evento_Aut { get; set; }
        public DbSet<tblRH_BN_Evento_Det> tblRH_BN_Evento_Det { get; set; }

        public DbSet<tblRH_BN_Incidencias> tblRH_BN_Incidencias { get; set; }
        public DbSet<tblRH_BN_Incidencia> tblRH_BN_Incidencia { get; set; }
        public DbSet<tblRH_BN_Incidencia_Evidencias> tblRH_BN_Incidencia_Evidencias { get; set; }
        public DbSet<tblRH_BN_Incidencia_det> tblRH_BN_Incidencia_det { get; set; }
        public DbSet<tblRH_BN_Incidencia_det_Peru> tblRH_BN_Incidencia_det_Peru { get; set; }
        public DbSet<tblRH_BN_Incidencia_Concepto> tblRH_BN_Incidencia_Concepto { get; set; }
        public DbSet<tblRH_BN_Incidencia_Concepto_CC> tblRH_BN_Incidencia_Concepto_CC { get; set; }
        public DbSet<tblRH_BN_ListaNegra> tblRH_BN_ListaNegra { get; set; }
        public DbSet<tblRH_BN_ListaBlanca> tblRH_BN_ListaBlanca { get; set; }

        public DbSet<tblRH_BN_Plantilla> tblRH_BN_Plantilla { get; set; }
        public DbSet<tblRH_BN_Plantilla_Aut> tblRH_BN_Plantilla_Aut { get; set; }
        public DbSet<tblRH_BN_Plantilla_Det> tblRH_BN_Plantilla_Det { get; set; }
        public DbSet<tblRH_BN_Plantilla_Cuadrado> tblRH_BN_Plantilla_Cuadrado { get; set; }
        public DbSet<tblRH_BN_Plantilla_Cuadrado_Det> tblRH_BN_Plantilla_Cuadrado_Det { get; set; }
        #endregion

        #region nomina
        public DbSet<tblC_Nom_Nomina> tblC_Nom_Nomina { get; set; }
        public DbSet<tblC_Nom_ResumenRaya> tblC_Nom_ResumenRaya { get; set; }
        public DbSet<tblC_Nom_Prenomina> tblC_Nom_Prenomina { get; set; }
        public DbSet<tblC_Nom_PreNomina_Aut> tblC_Nom_PreNomina_Aut { get; set; }
        public DbSet<tblC_Nom_PreNomina_Det> tblC_Nom_PreNomina_Det { get; set; }
        public DbSet<tblC_Nom_PreNominaPeru_Det> tblC_Nom_PreNominaPeru_Det { get; set; }
        public DbSet<tblC_Nom_PreNomina_Descuento> tblC_Nom_PreNomina_Descuento { get; set; }
        public DbSet<tblC_Nom_CatPeriodo> tblC_Nom_CatPeriodo { get; set; }
        public DbSet<tblC_Nom_Raya> tblC_Nom_Raya { get; set; }
        public DbSet<tblC_Nom_Cuenta> tblC_Nom_Cuenta { get; set; }
        public DbSet<tblC_Nom_CuentaEmpleado> tblC_Nom_CuentaEmpleado { get; set; }
        public DbSet<tblC_Nom_EmpleadoSinRelacionar> tblC_Nom_EmpleadoSinRelacionar { get; set; }
        public DbSet<tblC_Nom_TipoCuenta> tblC_Nom_TipoCuenta { get; set; }
        public DbSet<tblC_Nom_UsuarioValida> tblC_Nom_UsuarioValida { get; set; }
        public DbSet<tblC_Nom_EstructuraResumenNominaCC> tblC_Nom_EstructuraResumenNominaCC { get; set; }
        public DbSet<tblC_Nom_EstructuraPolizaNominaCC> tblC_Nom_EstructuraPolizaNominaCC { get; set; }
        public DbSet<tblC_Nom_ColumnaRaya> tblC_Nom_ColumnaRaya { get; set; }
        public DbSet<tblC_Nom_TipoNomina> tblC_Nom_TipoNomina { get; set; }
        public DbSet<tblC_Nom_ClasificacionCC> tblC_Nom_ClasificacionCC { get; set; }
        public DbSet<tblC_Nom_CatalogoCC> tblC_Nom_CatalogoCC { get; set; }
        public DbSet<tblC_Nom_CatalogoCCtblFA_Grupos> tblC_Nom_CatalogoCCtblFA_Grupos { get; set; }
        public DbSet<tblC_Nom_TipoRaya> tblC_Nom_TipoRaya { get; set; }
        public DbSet<tblC_Nom_CatSolicitudCheque> tblC_Nom_CatSolicitudCheque { get; set; }
        public DbSet<tblC_Nom_SUA_Det> tblC_Nom_SUA_Det { get; set; }
        public DbSet<tblC_Nom_SUA> tblC_Nom_SUA { get; set; }
        public DbSet<tblC_Nom_SUA_Resumen> tblC_Nom_SUA_Resumen { get; set; }
        public DbSet<tblC_Nom_SUA_BancoCuenta> tblC_Nom_SUA_BancoCuenta { get; set; }
        //Peru
        public DbSet<tblC_Nom_PeruAFP> tblC_Nom_PeruAFP { get; set; }
        public DbSet<tblC_Nom_CatPeruAFP> tblC_Nom_CatPeruAFP { get; set; }
        public DbSet<tblC_Nom_AFP> tblC_Nom_AFP { get; set; }
        public DbSet<tblC_Nom_AFPtblRH_EK_Empleados> tblC_Nom_AFPtblRH_EK_Empleados { get; set; }
        public DbSet<tblC_Nom_RequisicionGlobal> tblC_Nom_RequisicionGlobal { get; set; }
        public DbSet<tblC_Nom_Compra_Nomina> tblC_Nom_Compra_Nomina { get; set; }
        public DbSet<tblC_Nom_Compra_SUA> tblC_Nom_Compra_SUA { get; set; }
        #endregion
        public DbSet<tblM_SolicitudEquipo> tblM_SolicitudEquipo { get; set; }
        public DbSet<tblM_SolicitudEquipoDet> tblM_SolicitudEquipoDet { get; set; }
        public DbSet<tblM_AutorizacionSolicitudes> tblM_AutorizacionSolicitudes { get; set; }
        public DbSet<tblM_ControlEnvioMaquinaria> tblM_ControlEnvioMaquinaria { get; set; }
        public DbSet<tblM_AsignacionEquipos> tblM_AsignacionEquipos { get; set; }
        public DbSet<tblM_Paros> tblM_Paros { get; set; }
        public DbSet<tblB_PagoMensual> tblB_PagoMensual { get; set; }
        public DbSet<tblP_Alerta> tblP_Alerta { get; set; }
        public DbSet<tblM_CatCategoriasHH> tblM_CatCategoriasHH { get; set; }
        public DbSet<tblM_CatSubCategoriasHH> tblM_CatSubCategoriasHH { get; set; }
        public DbSet<tblM_CatControlCalidad> tblM_CatControlCalidad { get; set; }
        public DbSet<tblM_CatGrupoPreguntasCalidad> tblM_CatGrupoPreguntasCalidad { get; set; }
        public DbSet<tblM_CatPreguntasCalidad> tblM_CatPreguntasCalidad { get; set; }
        public DbSet<tblM_RelPreguntaControlCalidad> tblM_RelPreguntaControlCalidad { get; set; }
        public DbSet<tblEN_Encuesta> tblEN_Encuesta { get; set; }
        public DbSet<tblEN_Preguntas> tblEN_Preguntas { get; set; }
        public DbSet<tblEN_Resultado> tblEN_Resultado { get; set; }
        public DbSet<tblEN_ResultadoProveedores> tblEN_ResultadoProveedores { get; set; }
        public DbSet<tblEN_Encuesta_Usuario> tblEN_Encuesta_Usuario { get; set; }
        public DbSet<tblEN_EncuestaAsignaUsuario> tblEN_EncuestaAsignaUsuario { get; set; }
        public DbSet<tblEN_Estrellas> tblEN_Estrellas { get; set; }
        public DbSet<tblEN_GrupoInsumo> tblEN_GrupoInsumo { get; set; }
        public DbSet<tblEN_TipoTop20> tblEN_TipoTop20 { get; set; }
        public DbSet<tblEN_Top20Proveedores> tblEN_Top20Proveedores { get; set; }
        public DbSet<tblEN_UsuarioInsumo> tblEN_UsuarioInsumo { get; set; }
        public DbSet<tblEN_UsuariosAC> tblEN_UsuariosAC { get; set; }
        public DbSet<tblEN_ProvCorreosNotificacion> tblEN_ProvCorreosNotificacion { get; set; }
        public DbSet<tblEN_TipoPregunta> tblEN_TipoPregunta { get; set; }
        public DbSet<tblEN_UsuariosExcepcionTop20> tblEN_UsuariosExcepcionTop20 { get; set; }
        public DbSet<tblEN_UsuarioProveedorConsigna> tblEN_UsuarioProveedorConsigna { get; set; }
        public DbSet<tblP_AlertaMantenimiento> tblP_AlertaMantenimiento { get; set; }
        public DbSet<tblM_AutorizacionStandBy> tblM_AutorizacionStandBy { get; set; }
        public DbSet<tblM_MaquinariaRentada> tblM_MaquinariaRentada { get; set; }
        public DbSet<tblM_RN_Maquinaria> tblM_RN_Maquinaria { get; set; }
        public DbSet<tblP_Empresas> tblP_Empresas { get; set; }

        #region CARATULAS 
        public DbSet<tblM_Caratulas> tblM_Caratulas { get; set; }
        public DbSet<tblM_CaratulaDet> tblM_CaratulaDet { get; set; }
        public DbSet<tblM_IndicadoresCaratula> tblM_IndicadoresCaratula { get; set; }
        public DbSet<tblM_CaratulaAut> tblM_CaratulaAut { get; set; }
        public DbSet<tblM_CaratulaConceptos> tblM_CaratulaConceptos { get; set; }

        public DbSet<tblM_Caratula> tblM_Caratula { get; set; }
        public DbSet<tblM_CaratulaAgrupacionEnc> tblM_CaratulaAgrupacionEnc { get; set; }
        public DbSet<tblM_CaratulaAgrupacionDet> tblM_CaratulaAgrupacionDet { get; set; }


        #endregion 


        #region Encuestas subContratistas
        public DbSet<tblEN_EncuestaSubContratista> tblEN_EncuestaSubContratista { get; set; }
        public DbSet<tblEN_PreguntasSubContratistas> tblEN_PreguntasSubContratistas { get; set; }
        public DbSet<tblEN_ResultadoSubContratistas> tblEN_ResultadoSubContratistas { get; set; }
        public DbSet<tblEN_ResultadoSubContratistasDet> tblEN_ResultadoSubContratistasDet { get; set; }

        #endregion

        #region Encuestas Proveedores
        public DbSet<tblEN_EncuestaProveedores> tblEN_EncuestaProveedores { get; set; }
        public DbSet<tblEN_PreguntasProveedores> tblEN_PreguntasProveedores { get; set; }

        public DbSet<tblEN_ResultadoProveedorRequisiciones> tblEN_ResultadoProveedorRequisiciones { get; set; }
        public DbSet<tblEN_ResultadoProveedorRequisicionDet> tblEN_ResultadoProveedorRequisicionDet { get; set; }
        public DbSet<tblEN_TipoEncuestaProveedor> tblEN_TipoEncuestaProveedor { get; set; }

        #endregion

        public DbSet<tblM_CapHorasHombre> tblM_CapHorasHombre { get; set; }
        public DbSet<tblP_PerfilAutoriza> tblP_PerfilAutoriza { get; set; }
        public DbSet<tblP_Autoriza> tblP_Autoriza { get; set; }
        #region Proveedor
        public DbSet<tblC_CatNumNafin> tblC_CatNumNafin { get; set; }
        public DbSet<tblC_CatGiro> tblC_CatGiro { get; set; }
        #endregion
        #region Cadena Productiva
        public DbSet<tblC_CadenaPrincipal> tblC_CadenaPrincipal { get; set; }
        public DbSet<tblC_CadenaProductiva> tblC_CadenaProductiva { get; set; }
        public DbSet<tblC_InteresesNafin> tblC_InteresesNafin { get; set; }
        public DbSet<tblC_InteresesNafinDetalle> tblC_InteresesNafinDetalle { get; set; }
        public DbSet<tblC_FechaPago> tblC_FechaPago { get; set; }
        public DbSet<tblC_FacturaParcial> tblC_FacturaParcial { get; set; }
        public DbSet<tblC_EstimacionProveedor> tblC_EstimacionProveedor { get; set; }
        public DbSet<tblC_CCDivision> tblC_CCDivision { get; set; }
        public DbSet<tblC_CCProrrateo> tblc_CCProrrateo { get; set; }
        public DbSet<tblC_RelCCPropuesta> tblC_RelCCPropuesta { get; set; }
        public DbSet<tblC_EstimacionCobranza> tblC_EstimacionCobranza { get; set; }
        public DbSet<tblC_Linea> tblC_Linea { get; set; }
        public DbSet<tblC_Anticipo> tblC_Anticipo { get; set; }
        public DbSet<tblC_CatCCBase> tblC_CatCCBase { get; set; }
        #endregion
        public DbSet<tblPro_Obras> tblPro_Obras { get; set; }
        public DbSet<tbl_TESTDATA> tbl_TESTDATA { get; set; }
        public DbSet<tblPro_CatAreas> tblPro_CatAreas { get; set; }
        public DbSet<tblPro_CapturadeObras> tblPro_CapturadeObras { get; set; }
        public DbSet<tblP_AccionesVista> tblP_AccionesVista { get; set; }
        public DbSet<tblPro_ActivoFijo> tblPro_ActivoFijo { get; set; }
        public DbSet<tblPro_Premisas> tblPro_Premisas { get; set; }
        public DbSet<tblPro_SaldosIniciales> tblPro_SaldosIniciales { get; set; }
        public DbSet<tblPro_CxC> tblPro_CxC { get; set; }
        public DbSet<tblPro_CatResponsables> tblPro_CatResponsables { get; set; }
        public DbSet<tblPro_Administracion> tblPro_Administracion { get; set; }
        public DbSet<tblPro_CobrosDiversos> tblPro_CobrosDiversos { get; set; }
        public DbSet<tblPro_PagosDiversos> tblPro_PagosDiversos { get; set; }
        public DbSet<tblP_ReglasSubcontratistasBloqueo> tblP_ReglasSubcontratistasBloqueo { get; set; }
        public DbSet<tblM_CapNotaCredito> tblM_CapNotaCredito { get; set; }
        public DbSet<tblM_SolicitudReemplazoEquipo> tblM_SolicitudReemplazoEquipo { get; set; }
        public DbSet<tblM_AutorizacionSolicitudReemplazo> tblM_AutorizacionSolicitudReemplazo { get; set; }
        public DbSet<tblM_Eficiencia> tblM_Eficiencia { get; set; }
        public DbSet<tblM_SolicitudReemplazoDet> tblM_SolicitudReemplazoDet { get; set; }
        public DbSet<tblM_ComentariosNotaCredito> tblM_ComentariosNotaCredito { get; set; }
        public DbSet<tblM_ArchivosNotasCredito> tblM_ArchivosNotasCredito { get; set; }
        public DbSet<tblM_RptIndicador> tblM_RptIndicador { get; set; }
        public DbSet<tblM_CatPreguntaResguardoVehiculo> tblM_CatPreguntaResguardoVehiculo { get; set; }
        public DbSet<tblM_ResguardoVehiculosServicio> tblM_ResguardoVehiculosServicio { get; set; }
        public DbSet<tblF_CapPrefactura> tblF_CapPrefactura { get; set; }
        public DbSet<tblF_RepPrefactura> tblF_RepPrefactura { get; set; }
        public DbSet<tblF_FilePrefactura> tblF_FilePrefactura { get; set; }
        public DbSet<tblF_CapImporte> tblF_CapImporte { get; set; }
        public DbSet<tblF_CatImporte> tblF_CatImporte { get; set; }
        public DbSet<tblM_CordinadorSeguridadAreaCuenta> tblM_CordinadorSeguridadAreaCuenta { get; set; }

        #region EKONTROL FACTURAS
        public DbSet<tblF_EK_Factura_Retencion> tblF_EK_Factura_Retencion { get; set; }
        public DbSet<tblF_EK_Facturas> tblF_EK_Facturas { get; set; }
        public DbSet<tblF_EK_Facturas_Det> tblF_EK_Facturas_Det { get; set; }
        public DbSet<tblF_EK_Pedidos_Det> tblF_EK_Pedido_Det { get; set; }
        public DbSet<tblF_EK_Pedidos_Retencion> tblF_EK_Pedido_Retencion { get; set; }
        public DbSet<tblF_EK_Pedidos> tblF_EK_Pedidos { get; set; }
        public DbSet<tblF_EK_Pedidos_Detalle_Partidas> tblF_EK_Pedidos_Detalle_Partidas { get; set; }
        public DbSet<tblF_EK_Remision> tblF_EK_Remision { get; set; }
        public DbSet<tblF_EK_Remision_Det> tblF_EK_Remision_Det { get; set; }
        public DbSet<tblF_EK_Remision_Retencion> tblF_EK_Remision_Retencion { get; set; }
        public DbSet<tblF_EK_MetodoPagoSat> tblF_EK_MetodoPagoSat { get; set; }
        public DbSet<tblF_EK_RegimenFiscal> tblF_EK_RegimenFiscal { get; set; }
        public DbSet<tblF_EK_TM> tblF_EK_TM { get; set; }
        public DbSet<tblF_EK_CondEntrega> tblF_EK_CondEntrega { get; set; }
        public DbSet<tblF_EK_TipoFlete> tblF_EK_TipoFlete { get; set; }
        public DbSet<tblF_EK_TipoPedido> tblF_EK_TipoPedido { get; set; }
        public DbSet<tblF_EK_FormaPagoSat> tblF_EK_FormaPagoSat { get; set; }
        public DbSet<tblF_EK_TipoFactura> tblF_EK_TipoFactura { get; set; }
        public DbSet<tblF_EK_Serie> tblF_EK_Serie { get; set; }
        public DbSet<tblF_EK_Insumos> tblF_EK_Insumos { get; set; }
        public DbSet<tblF_EK_InsumosSAT> tblF_EK_InsumosSAT { get; set; }
        public DbSet<tblF_EK_InsumosRel> tblF_EK_InsumosRel { get; set; }
        public DbSet<tblF_EK_Movcltes> tblF_EK_Movcltes { get; set; }
        public DbSet<tblF_EK_Cfd> tblF_EK_Cfd { get; set; }

        #endregion

        public DbSet<tblM_AutorizacionResguardo> tblM_AutorizacionResguardo { get; set; }

        public DbSet<tblM_ArchivosModelos> tblM_ArchivosModelos { get; set; }

        public DbSet<tblM_RespuestaResguardoVehiculos> tblM_RespuestaResguardoVehiculos { get; set; }
        public DbSet<tblM_DocumentosResguardos> tblM_DocumentosResguardos { get; set; }
        public DbSet<tblM_CapOrdenTrabajo> tblM_CapOrdenTrabajo { get; set; }
        public DbSet<tblM_DetOrdenTrabajo> tblM_DetOrdenTrabajo { get; set; }
        public DbSet<tblSA_Responsables> tblSA_Responsables { get; set; }

        public DbSet<tblEN_ResultadoProveedoresDet> tblEN_ResultadoProveedoresDet { get; set; }
        public DbSet<tblM_CapStandBy> tblM_CapStandBy { get; set; }
        public DbSet<tblEN_Encuesta_Update> tblEN_Encuesta_Update { get; set; }
        public DbSet<tblM_DetStandby> tblM_DetStandby { get; set; }
        public DbSet<tblM_CapProyeccionesKPI> tblM_CapProyeccionesKPI { get; set; }

        public DbSet<tblM_AutorizaStandby> tblM_AutorizaStandby { get; set; }
        public DbSet<tblM_CatTipoBaja> tblM_CatTipoBaja { get; set; }

        public DbSet<tblM_MaquinariaAceitesLubricantes> tblM_MaquinariaAceitesLubricantes { get; set; }
        public DbSet<tblM_CatAceitesLubricantes> tblM_CatAceitesLubricantes { get; set; }
        public DbSet<tblM_KPI> tblM_KPI { get; set; }


        public DbSet<tblM_ControMovimientoInterno> tblM_ControInterno { get; set; }
        public DbSet<tblM_ControMovimientoInterno_Permisos> tblM_ControMovimientoInterno_Permisos { get; set; }
        public DbSet<tblM_AutorizaMovimientoInterno> tblM_AutorizaMovimientoInterno { get; set; }
        public DbSet<tblM_HistInventario> tblM_HistInventario { get; set; }
        public DbSet<tblM_CatCriteriosCausaParo> tblM_CatCriteriosCausaParo { get; set; }
        public DbSet<tblAD_Cotizaciones> tblAD_Cotizaciones { get; set; }
        public DbSet<tblAD_CotizacionComentarios> tblAD_CotizacionComentarios { get; set; }
        public DbSet<tblM_CatTiposAceites> tblM_CatTiposAceites { get; set; }
        public DbSet<tblPro_ComentariosObras> tblPro_ComentariosObras { get; set; }

        public DbSet<tblM_CapReporteFallaOverhaul> tblM_CapReporteFallaOverhaul { get; set; }
        public DbSet<tblPro_CatEscenarios> tblPro_CatEscenarios { get; set; }
        public DbSet<tblPro_CierreObra> tblPro_CierreObra { get; set; }
        public DbSet<tblM_DocumentosMaquinaria> tblM_DocumentosMaquinaria { get; set; }


        public DbSet<tblM_CatRelacionMovimientoInterno> tblM_CatRelacionMovimientoInterno { get; set; }
        public DbSet<tblPro_CapCifrasPrincipales> tblPro_CapCifrasPrincipales { get; set; }
        //reaguilar agregar tabla al repositorio....
        public DbSet<tblRH_AditivaDeductiva> tblRH_AditivaDeductiva { get; set; }
        public DbSet<tblRH_AditivaDeductivaDet> tblRH_AditivaDeductivaDet { get; set; }
        public DbSet<tblRH_AutorizacionAditivaDeductiva> tblRH_AutorizacionAditivaDeductiva { get; set; }
        //reaguilar tablas repositorio Cursos....
        public DbSet<tblCU_Curso> tblCu_Curso { get; set; }
        public DbSet<tblCU_Modulo> tblCu_Modulo { get; set; }
        public DbSet<tblCU_ModuloDet> tblCu_ModuloDet { get; set; }
        //reaguilar tablas repositorio Examen....
        public DbSet<tblCU_Examen> tblCU_Examen { get; set; }
        public DbSet<tblCU_ExamenPregunta> tblCU_ExamenPregunta { get; set; }
        public DbSet<tblCU_ExamenRespuesta> tblCU_ExamenRespuesta { get; set; }
        public DbSet<tblPo_Poliza> tblPo_Poliza { get; set; }
        public DbSet<tblPo_MovimientoPoliza> tblPo_MovimientoPoliza { get; set; }
        public DbSet<tblC_SC_ConversionPoliza> tblC_SC_ConversionPoliza { get; set; }
        public DbSet<tblC_SC_CuentasExcepcion> tblC_SC_CuentasExcepcion { get; set; }
        //reaguilar tabla asignacion de curso....
        public DbSet<tblCU_Asignacion> tblCU_Asignacion { get; set; }
        //reaguilar tablas Mantenimiento- maquinaria....
        public DbSet<tblM_MatenimientoPm> tblM_MatenimientoPm { get; set; }
        public DbSet<tblM_MantenimientoPm_Archivo> tblM_MantenimientoPm_Archivo { get; set; }
        public DbSet<tblM_CatPM> tblM_catPM { get; set; }
        public DbSet<tblM_CatActividadPM> tblM_CatActividadPM { get; set; }
        public DbSet<tblM_CatPM_CatActividadPM> tblM_CatPM_CatActividadPM { get; set; }
        public DbSet<tblM_ActvContPM> tblM_ActvContPM { get; set; }
        public DbSet<tblM_CatParteVidaUtil> tblM_CatParteVidaUtil { get; set; }
        public DbSet<tblM_CatActividadPM_tblM_CatParte> tblM_CatActividadPM_tblM_CatParte { get; set; }
        public DbSet<tblM_SubConjuntoModelo> tblM_SubConjuntoModelo { get; set; }
        public DbSet<tblM_CatTipoActividad> tblM_CatTipoActividad { get; set; }
        public DbSet<tblP_EnvioCorreos> tblP_EnvioCorreos { get; set; }
        //raguilar tabla componente viscosidades
        public DbSet<tblM_CatComponentesViscosidades> tblM_CatComponentesViscosidades { get; set; }
        //raguilar tabla relacional formatos mantenimiento 24/04/18
        public DbSet<tblM_FormatoManteniento> tblM_FormatoManteniento { get; set; }
        //raguilar prueba catalogo filtros 25/04/18
        public DbSet<tblM_CatFiltroMant> tblM_CatFiltroMant { get; set; }
        //raguilar vinculacion componente de lubricacion con actividad modelo mantenimiento 02/05/18
        public DbSet<tblM_ComponenteMantenimiento> tblM_ComponenteMantenimiento { get; set; }
        //raguilar mapeo de componentes relacionados  02/05/18
        public DbSet<tblM_MiscelaneoMantenimiento> tblM_MiscelaneoMantenimiento { get; set; }
        public DbSet<tblRH_Finiquito> tblRH_Finiquito { get; set; }
        public DbSet<tblRH_FiniquitoDetalle> tblRH_FiniquitoDetalle { get; set; }
        public DbSet<tblRH_FiniquitoConceptos> tblRH_FiniquitoConceptos { get; set; }
        public DbSet<tblRH_FiniquitoFirmas> tblRH_FiniquitoFirmas { get; set; }
        public DbSet<tblRH_FiniquitoSalarioMin> tblRH_FiniquitoSalarioMin { get; set; }
        public DbSet<tblM_CatSuministros> tblM_CatSuministros { get; set; }
        public DbSet<tblM_IconMantenimiento> tblM_IconMantenimiento { get; set; }
        //raguilar bitacora jg 01/06/18
        public DbSet<tblM_BitacoraControlAceiteMant> tblM_BitacoraControlAceiteMant { get; set; }
        public DbSet<tblM_BitacoraControlActExt> tblM_BitacoraControlActExt { get; set; }
        public DbSet<tblEN_Encuesta_Check_Usuario> tblEN_Encuesta_Check_Usuario { get; set; }
        //raguilar vinculacion cc acceso economicoid 08/06/18
        public DbSet<tblM_RelacionCCMant> tblM_RelacionCCMant { get; set; }
        //raguilar renderizado full 11/06/18
        public DbSet<tblM_RenderFullCalendar> tblM_RenderFullCalendar { get; set; }
        #region Seguridad
        public DbSet<tblS_Vehiculo> tblS_Vehiculo { get; set; }
        public DbSet<tblS_Observaciones> tblS_Observaciones { get; set; }
        public DbSet<tblS_CatPartes> tblS_CatPartes { get; set; }
        #endregion
        //raguilar guardado bitacora db
        public DbSet<tblM_BitacoraControlDN> tblM_BitacoraControlDN { get; set; }
        //raguilar bitacora  aproyectar lubricantes 16/07/18 12:09pm
        public DbSet<tblM_BitacoraControlAceiteMantProy> tblM_BitacoraControlAceiteMantProy { get; set; }
        //raguilar bitacora  aproyectar Actividades 19/07/18 05:16pm
        public DbSet<tblM_BitacoraActividadesMantProy> tblM_BitacoraActividadesMantProy { get; set; }
        //raguilar bitacora  aproyectar Actividades extras y dns 27/07/18 
        public DbSet<tblM_BitacoraControlAEMantProy> tblM_BitacoraControlAEMantProy { get; set; }
        public DbSet<tblM_BitacoraControlDNMantProy> tblM_BitacoraControlDNMantProy { get; set; }
        //raguilar 090818 catalogo pm 
        public DbSet<tblM_CatPM1> tblM_catPM1 { get; set; }

        public DbSet<tblK_CapturaMaq> tblK_CapturaMaq { get; set; }

        public DbSet<tblP_CC> tblP_CC { get; set; }

        public DbSet<tblM_CC_Exclusion> tblM_CC_Exclusion { get; set; }

        public DbSet<tblM_CapNominaCC> tblM_CapNominaCC { get; set; }
        public DbSet<tblM_CapNominaCC_Proyectos> tblM_CapNominaCC_Proyectos { get; set; }
        public DbSet<tblRH_FormatoCambioExclusion> tblRH_FormatoCambioExclusion { get; set; }
        public DbSet<tblRH_AutorizanteExclusion> tblRH_AutorizanteExclusion { get; set; }
        public DbSet<tblM_CapNominaCC_Detalles> tblM_CapNominaCC_Detalles { get; set; }
        public DbSet<tblP_PermisosAutorizaCorreo> tblP_PermisosAutorizaCorreo { get; set; }

        public DbSet<tblM_CatPipas> tblM_CatPipa { get; set; }
        public DbSet<tblM_CatMarcaMant> tblM_CatMarcaMant { get; set; }

        #region Cuadros COmparativos Alta Equipo
        public DbSet<tblM_ComparativoAdquisicionyRenta> tblM_ComparativoAdquisicionyRenta { get; set; }
        public DbSet<tblM_ComparativoAdquisicionyRentaDet> tblM_ComparativoAdquisicionyRentaDet { get; set; }
        public DbSet<tblM_ComparativoFinanciero> tblM_ComparativoFinanciero { get; set; }
        public DbSet<tblM_ComparativoFinancieroDet> tblM_ComparativoFinancieroDet { get; set; }
        public DbSet<tblM_ComparativosAdquisicionyRentaCaracteristicasDet> tblM_ComparativosAdquisicionyRentaCaracteristicasDet { get; set; }
        public DbSet<tblM_ComparativoAdquisicionyRentaAutorizante> tblM_ComparativoAdquisicionyRentaAutorizante { get; set; }
        public DbSet<tblM_ComparativoFinancieroAutorizante> tblM_ComparativoFinancieroAutorizante { get; set; }
        public DbSet<tblM_Comp_CatFinanciero> tblM_Comp_CatFinanciero { get; set; }
        public DbSet<tblM_Comp_CapPlazo> tblM_Comp_CapPlazo { get; set; }
        #endregion

        #region Contabilidad
        public DbSet<tblC_TipoMoneda> tblC_TipoMoneda { get; set; }
        public DbSet<tblC_Poliza> tblC_Poliza { get; set; }
        public DbSet<tblC_Movpol> tblC_Movpol { get; set; }
        public DbSet<tblC_TipoMovimiento> tblC_TipoMovimiento { get; set; }
        #endregion

        #region ActivoFijo
        public DbSet<tblC_AF_InsumosOverhaul> tblC_AF_InsumosOverhaul { get; set; }
        public DbSet<tblC_AF_Cuentas> tblC_AF_Cuentas { get; set; }
        public DbSet<tblC_AF_RelSubCuentas> tblC_AF_RelSubCuentas { get; set; }
        public DbSet<tblM_CatMaquinaDepreciacion> tblM_CatMaquinaDepreciacion { get; set; }
        public DbSet<tblM_CatMaquinaDepreciacion_Extras> tblM_CatMaquinaDepreciacion_Extras { get; set; }
        public DbSet<tblC_AF_PolizaAltaBaja> tblC_AF_PolizaAltaBaja { get; set; }
        public DbSet<tblC_AF_PolizaAltaBaja_Extras> tblC_AF_PolizaAltaBaja_Extras { get; set; }
        public DbSet<tblC_AF_PolizaAltaBaja_NoMaquina> tblC_AF_PolizaAltaBaja_NoMaquina { get; set; }
        public DbSet<tblC_AF_PolizaDeAjuste> tblC_AF_PolizaDeAjuste { get; set; }
        public DbSet<tblC_AF_TipoPolizaDeAjuste> tblC_AF_TipoPolizaDeAjuste { get; set; }
        public DbSet<tblC_AF_PolizasExcluidasParaCapturaAutomatica> tblC_AF_PolizasExcluidasParaCapturaAutomatica { get; set; }
        public DbSet<tblC_AF_CatalogoMaquina> tblC_AF_CatalogoMaquina { get; set; }
        public DbSet<tblC_AF_1210Excel> tblC_AF_1210Excel { get; set; }
        public DbSet<tblC_AF_1210ExcelFaltantes> tblC_AF_1210ExcelFaltantes { get; set; }
        public DbSet<tblC_AF_ExcelNoMaquinas> tblC_AF_ExcelNoMaquinas { get; set; }
        public DbSet<tblC_AF_ExcelNoMaquinasFaltantes> tblC_AF_ExcelNoMaquinasFaltantes { get; set; }
        public DbSet<tblC_AF_CatTipoMovimiento> tblC_AF_CatTipoMovimiento { get; set; }
        public DbSet<tblC_AF_InfoDepreciacion> tblC_AF_InfoDepreciacion { get; set; }
        public DbSet<tblC_AF_EnviarCosto> tblC_AF_EnviarCosto { get; set; }
        public DbSet<tblC_AF_DepreciacionEspecial> tblC_AF_DepreciacionEspecial { get; set; }
        public DbSet<tblC_AF_ComportamientoDePoliza> tblC_AF_ComportamientoDePoliza { get; set; }
        public DbSet<tblC_AF_PolizaEspecial> tblC_AF_PolizaEspecial { get; set; }
        public DbSet<tblC_AF_CambioAreaCuenta> tblC_AF_CambioAreaCuenta { get; set; }
        public DbSet<tblC_AF_AjusteSubConjuntos> tblC_AF_AjusteSubConjuntos { get; set; }

        #region Colombia
        public DbSet<tblC_AF_CuentaColombia> tblC_AF_CuentaColombia { get; set; }
        public DbSet<tblC_AF_SubcuentaColombia> tblC_AF_SubcuentaColombia { get; set; }
        public DbSet<tblC_AF_RelacionCuentaYearColombia> tblC_AF_RelacionCuentaYearColombia { get; set; }
        public DbSet<tblC_AF_RelacionPolizaColombia> tblC_AF_RelacionPolizaColombia { get; set; }
        public DbSet<tblC_AF_RelacionPolizaColombiaAjuste> tblC_AF_RelacionPolizaColombiaAjuste { get; set; }
        public DbSet<tblC_AF_AjusteDepreciacionColombia> tblC_AF_AjusteDepreciacionColombia { get; set; }
        #endregion

        #region Perú
        public DbSet<tblC_AF_CuentaPeru> tblC_AF_CuentaPeru { get; set; }
        public DbSet<tblC_AF_SubcuentaPeru> tblC_AF_SubcuentaPeru { get; set; }
        public DbSet<tblC_AF_RelacionCuentaYearPeru> tblC_AF_RelacionCuentaYearPeru { get; set; }
        public DbSet<tblC_AF_RelacionPolizaPeru> tblC_AF_RelacionPolizaPeru { get; set; }
        public DbSet<tblC_AF_RelacionPolizaPeruAjuste> tblC_AF_RelacionPolizaPeruAjuste { get; set; }
        public DbSet<tblC_AF_AjusteDepreciacionPeru> tblC_AF_AjusteDepreciacionPeru { get; set; }
        public DbSet<tblC_AF_TipoAjusteDepreciacionPeru> tblC_AF_TipoAjusteDepreciacionPeru { get; set; }
        #endregion
        #endregion

        #region ActivoFijoPolizaDepreciacion
        public DbSet<tblC_AF_TipoMovimiento> tblC_AF_TiposMovimiento { get; set; }
        public DbSet<tblC_AF_EstatusPoliza> tblC_AF_EstatusPolizas { get; set; }
        public DbSet<tblC_AF_ModuloEnkontrol> tblC_AF_ModulosEnkontrol { get; set; }
        public DbSet<tblC_AF_TipoCuenta> tblC_AF_TiposCuenta { get; set; }
        public DbSet<tblC_AF_Cuenta> tblC_AF_Cuenta { get; set; }
        public DbSet<tblC_AF_CuentasCostosSegunCC> tblC_AF_CuentasCostosSegunCC { get; set; }
        public DbSet<tblC_AF_Subcuenta> tblC_AF_Subcuentas { get; set; }
        public DbSet<tblC_AF_RelacionCuentaAño> tblC_AF_RelacionesCuentaAño { get; set; }
        public DbSet<tblC_AF_Poliza> tblC_AF_Polizas { get; set; }
        public DbSet<tblC_AF_PolizaDetalle> tblC_AF_PolizasDetalle { get; set; }
        public DbSet<tblC_AF_CambioCuentaDepreciacion> tblC_AF_CambioCuentaDepreciacion { get; set; }
        #endregion

        #region Mantenimiento
        public DbSet<tblM_PMComponenteModelo> tblM_PMComponenteModelo { get; set; }
        #endregion

        #region CONTROL OBRA
        public DbSet<tblCO_Capitulos> tblCO_Capitulos { get; set; }
        public DbSet<tblCO_Subcapitulos_Nivel1> tblCO_Subcapitulos_Nivel1 { get; set; }
        public DbSet<tblCO_Subcapitulos_Nivel2> tblCO_Subcapitulos_Nivel2 { get; set; }
        public DbSet<tblCO_Subcapitulos_Nivel3> tblCO_Subcapitulos_Nivel3 { get; set; }
        public DbSet<tblCO_Actividades> tblCO_Actividades { get; set; }
        public DbSet<tblCO_Unidades> tblCO_Unidades { get; set; }
        public DbSet<tblCO_Unidades_Actividad> tblCO_Unidades_Actividad { get; set; }
        public DbSet<tblCO_Actividades_Avance> tblCO_Actividades_Avance { get; set; }
        public DbSet<tblCO_Actividades_Avance_Detalle> tblCO_Actividades_Avance_Detalle { get; set; }
        public DbSet<tblCO_Actividades_Facturado> tblCO_Actividades_Facturado { get; set; }
        public DbSet<tblCO_Actividades_Facturado_Detalle> tblCO_Actividades_Facturado_Detalle { get; set; }
        public DbSet<tblCO_Divisiones> tblCO_Divisiones { get; set; }
        public DbSet<tblCO_DivisionCC> tblCO_DivisionCC { get; set; }
        public DbSet<tblCO_PlantillaInforme> tblCO_PlantillaInforme { get; set; }
        public DbSet<tblCO_PlantillaInforme_detalle> tblCO_PlantillaInforme_detalle { get; set; }
        public DbSet<tblCO_informeSemanal> tblCO_informeSemanal { get; set; }
        public DbSet<tblCO_informeSemanal_detalle> tblCO_informeSemanal_detalle { get; set; }
        #endregion

        #region PRINCIPALES
        public DbSet<tblP_Pais> tblP_Pais { get; set; }
        public DbSet<tblP_Estado> tblP_Estado { get; set; }
        public DbSet<tblP_Municipio> tblP_Municipio { get; set; }
        #endregion

        public DbSet<tblP_Medico> tblP_Medico { get; set; }






        #region INDICADORES SEGURIDAD

        public DbSet<tblS_IncidentesEmpleadoContratistas> tblS_IncidentesEmpleadoContratistas { get; set; }
        public DbSet<tblS_IncidentesEmpresasContratistas> tblS_IncidentesEmpresasContratistas { get; set; }

        public DbSet<tblS_Incidentes> tblS_Incidentes { get; set; }
        public DbSet<tblS_IncidentesAgentesImplicados> tblS_IncidentesAgentesImplicados { get; set; }
        public DbSet<tblS_IncidentesCausasBasicas> tblS_IncidentesCausasBasicas { get; set; }
        public DbSet<tblS_IncidentesCausasInmediatas> tblS_IncidentesCausasInmediatas { get; set; }
        public DbSet<tblS_IncidentesCausasRaiz> tblS_IncidentesCausasRaiz { get; set; }
        public DbSet<tblS_IncidentesClasificacion> tblS_IncidentesClasificacion { get; set; }
        public DbSet<tblS_IncidentesDepartamentos> tblS_IncidentesDepartamentos { get; set; }
        public DbSet<tblS_IncidentesEmpleadoAntiguedad> tblS_IncidentesEmpleadoAntiguedad { get; set; }
        public DbSet<tblS_IncidentesEmpleadoExperiencia> tblS_IncidentesEmpleadoExperiencia { get; set; }
        public DbSet<tblS_IncidentesEmpleadosContratistas> tblS_IncidentesEmpleadosContratistas { get; set; }
        public DbSet<tblS_IncidentesEmpleadosTurno> tblS_IncidentesEmpleadosTurno { get; set; }
        public DbSet<tblS_IncidentesEventoDetonador> tblS_IncidentesEventoDetonador { get; set; }
        public DbSet<tblS_IncidentesGrupoInvestigacion> tblS_IncidentesGrupoInvestigacion { get; set; }
        public DbSet<tblS_IncidentesInformacionColaboradores> tblS_IncidentesInformacionColaboradores { get; set; }
        //raguilar detalle incidencia
        public DbSet<tblS_IncidentesInformacionColaboradoresDetalle> tblS_IncidentesInformacionColaboradoresDetalle { get; set; }
        public DbSet<tblS_IncidentesInformacionColaboradoresClasificacion> tblS_IncidentesInformacionColaboradoresClasificacion { get; set; }
        public DbSet<tblS_IncidentesMedidasControl> tblS_IncidentesMedidasControl { get; set; }
        public DbSet<tblS_IncidentesOrdenCronologico> tblS_IncidentesOrdenCronologico { get; set; }
        public DbSet<tblS_IncidentesPartesCuerpo> tblS_IncidentesPartesCuerpo { get; set; }
        public DbSet<tblS_IncidentesProtocolosTrabajo> tblS_IncidentesProtocolosTrabajo { get; set; }
        public DbSet<tblS_IncidentesTecnicasInvestigacion> tblS_IncidentesTecnicasInvestigacion { get; set; }
        public DbSet<tblS_IncidentesTipoContacto> tblS_IncidentesTipoContacto { get; set; }
        public DbSet<tblS_IncidentesTipoEventos> tblS_IncidentesTipoEventos { get; set; }
        public DbSet<tblS_IncidentesTipoLesion> tblS_IncidentesTipoLesion { get; set; }
        public DbSet<tblS_IncidentesTipos> tblS_IncidentesTipos { get; set; }
        public DbSet<tblS_IncidentesInformePreliminar> tblS_IncidentesInformePreliminar { get; set; }
        public DbSet<tblS_IncidentesEvidencias> tblS_IncidentesEvidencias { get; set; }
        public DbSet<tblS_IncidentesEvidenciasRIA> tblS_IncidentesEvidenciasRIA { get; set; }
        public DbSet<tblS_IncidentesSubclasificacion> tblS_IncidentesSubclasificacion { get; set; }
        public DbSet<tblS_IncidentesTipoProcedimientosViolados> tblS_IncidentesTipoProcedimientosViolados { get; set; }
        public DbSet<tblS_IncidentesMetasGrafica> tblS_IncidentesMetasGrafica { get; set; }
        public DbSet<tblS_IncidentesAgrupacionCC> tblS_IncidentesAgrupacionCC { get; set; }
        public DbSet<tblS_IncidentesAgrupacionCCDet> tblS_IncidentesAgrupacionCCDet { get; set; }
        #endregion

        #region Capacitación Operativa
        public DbSet<tblS_Capacitacion_PCAdministracionInstructores> tblS_Capacitacion_PCAdministracionInstructores { get; set; }
        public DbSet<tblS_CapacitacionRolesGrupoTrabajo> tblS_CapacitacionRolesGrupoTrabajo { get; set; }
        public DbSet<tblS_CapacitacionCursos> tblS_CapacitacionCursos { get; set; }
        public DbSet<tblS_CapacitacionCursosExamen> tblS_CapacitacionCursosExamen { get; set; }
        public DbSet<tblS_CapacitacionCursosPuestos> tblS_CapacitacionCursosPuestos { get; set; }
        public DbSet<tblS_CapacitacionCursosPuestosAutorizacion> tblS_CapacitacionCursosPuestosAutorizacion { get; set; }
        public DbSet<tblS_CapacitacionControlAsistencia> tblS_CapacitacionControlAsistencia { get; set; }
        public DbSet<tblS_CapacitacionControlAsistenciaDetalle> tblS_CapacitacionControlAsistenciaDetalle { get; set; }
        public DbSet<tblS_CapacitacionAutorizacion> tblS_CapacitacionAutorizacion { get; set; }
        public DbSet<tblS_CapacitacionEmpleadoPrivilegio> tblS_CapacitacionEmpleadoPrivilegio { get; set; }
        public DbSet<tblS_CapacitacionExtracurricular> tblS_CapacitacionExtracurricular { get; set; }
        public DbSet<tblS_CapacitacionRelacionCCAutorizante> tblS_CapacitacionRelacionCCAutorizante { get; set; }
        public DbSet<tblS_CapacitacionRelacionCCDepartamentoRazonSocial> tblS_CapacitacionRelacionCCDepartamentoRazonSocial { get; set; }
        public DbSet<tblS_CapacitacionRazonSocial> tblS_CapacitacionRazonSocial { get; set; }
        public DbSet<tblS_CapacitacionPuestoMando> tblS_CapacitacionPuestoMando { get; set; }
        public DbSet<tblS_CapacitacionCursosCC> tblS_CapacitacionCursosCC { get; set; }
        public DbSet<tblS_CapacitacionCursosMando> tblS_CapacitacionCursosMando { get; set; }
        public DbSet<tblS_CapacitacionListaAutorizacion> tblS_CapacitacionListaAutorizacion { get; set; }
        public DbSet<tblS_CapacitacionListaAutorizacionRFC> tblS_CapacitacionListaAutorizacionRFC { get; set; }
        public DbSet<tblS_CapacitacionListaAutorizacionAsistentes> tblS_CapacitacionListaAutorizacionAsistentes { get; set; }
        public DbSet<tblS_CapacitacionListaAutorizacionInteresados> tblS_CapacitacionListaAutorizacionInteresados { get; set; }
        public DbSet<tblS_CapacitacionListaAutorizacionCC> tblS_CapacitacionListaAutorizacionCC { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajo> tblS_CapacitacionDNCicloTrabajo { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoAreas> tblS_CapacitacionDNCicloTrabajoAreas { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoCriterio> tblS_CapacitacionDNCicloTrabajoCriterio { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoRegistro> tblS_CapacitacionDNCicloTrabajoRegistro { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoRegistroRevisiones> tblS_CapacitacionDNCicloTrabajoRegistroRevisiones { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> tblS_CapacitacionDNCicloTrabajoRegistroPropuestas { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoRegistroAreas> tblS_CapacitacionDNCicloTrabajoRegistroAreas { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoAreaSeguimiento> tblS_CapacitacionDNCicloTrabajoAreaSeguimiento { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoControlAreaSeguimiento> tblS_CapacitacionDNCicloTrabajoControlAreaSeguimiento { get; set; }
        public DbSet<tblS_CapacitacionDNCicloTrabajoInteresados> tblS_CapacitacionDNCicloTrabajoInteresados { get; set; }
        public DbSet<tblS_CapacitacionDNDeteccionPrimaria> tblS_CapacitacionDNDeteccionPrimaria { get; set; }
        public DbSet<tblS_CapacitacionDNDeteccionPrimariaNecesidad> tblS_CapacitacionDNDeteccionPrimariaNecesidad { get; set; }
        public DbSet<tblS_CapacitacionDNDeteccionPrimariaAreas> tblS_CapacitacionDNDeteccionPrimariaAreas { get; set; }
        public DbSet<tblS_CapacitacionDNRecorrido> tblS_CapacitacionDNRecorrido { get; set; }
        public DbSet<tblS_CapacitacionDNRecorridoHallazgo> tblS_CapacitacionDNRecorridoHallazgo { get; set; }
        public DbSet<tblS_CapacitacionDNRecorridoHallazgoLider> tblS_CapacitacionDNRecorridoHallazgoLider { get; set; }
        public DbSet<tblS_CapacitacionDNRecorridoAreas> tblS_CapacitacionDNRecorridoAreas { get; set; }
        public DbSet<tblS_CapacitacionIHHColaboradorCapacitacion> tblS_CapacitacionIHHColaboradorCapacitacion { get; set; }
        public DbSet<tblS_CapacitacionIHHControlHoras> tblS_CapacitacionIHHControlHoras { get; set; }
        public DbSet<tblS_CapacitacionIHHControlActividad> tblS_CapacitacionIHHControlActividad { get; set; }
        public DbSet<tblS_CapacitacionIHHEquipoAdiestramiento> tblS_CapacitacionIHHEquipoAdiestramiento { get; set; }
        public DbSet<tblS_CapacitacionCentroCostoDivision> tblS_CapacitacionCentroCostoDivision { get; set; }
        public DbSet<tblS_CapacitacionCO_GCArchivos> tblS_CapacitacionCO_GCArchivos { get; set; }
        public DbSet<tblS_CapacitacionIHHActividad> tblS_CapacitacionIHHActividad { get; set; }
        public DbSet<tblS_CapacitacionIHHInteresado> tblS_CapacitacionIHHInteresado { get; set; }
        public DbSet<tblS_CapacitacionDNRecorridoEvidencias> tblS_CapacitacionDNRecorridoEvidencias { get; set; }
        public DbSet<tblS_CapacitacionFirmaInstructor> tblS_CapacitacionFirmaInstructor { get; set; }
        public DbSet<tblS_CapacitacionDNCiclosRequeridos> tblS_CapacitacionDNCiclosRequeridos { get; set; }
        #endregion

        #region 5s
        public DbSet<tbl5s_5s> tbl5s_5s { get; set; }
        public DbSet<tbl5s_Area> tbl5s_Area { get; set; }
        public DbSet<tbl5s_Calendario> tbl5s_Calendario { get; set; }
        public DbSet<tbl5s_CC_Usuario> tbl5s_CC_Usuario { get; set; }
        public DbSet<tbl5s_CC> tbl5s_CC { get; set; }
        public DbSet<tbl5s_CheckList> tbl5s_CheckList { get; set; }
        public DbSet<tbl5s_InspeccionDet> tbl5s_InspeccionDet { get; set; }
        public DbSet<tbl5s_Inspeccion> tbl5s_Inspeccion { get; set; }
        public DbSet<tbl5s_LiderArea> tbl5s_LiderArea { get; set; }
        public DbSet<tbl5s_Privilegio> tbl5s_Privilegio { get; set; }
        public DbSet<tbl5s_SubArea> tbl5s_SubArea { get; set; }
        public DbSet<tbl5s_Usuario> tbl5s_Usuario { get; set; }
        public DbSet<tbl5s_Auditorias> tbl5s_Auditorias { get; set; }
        public DbSet<tbl5s_AuditoriasDet> tbl5s_AuditoriasDet { get; set; }
        public DbSet<tbl5s_AuditoriasArchivos> tbl5s_AuditoriasArchivos { get; set; }
        public DbSet<tbl5s_AreaOperativa> tbl5s_AreaOperativa { get; set; }
        public DbSet<tbl5s_UsuarioPrivilegios> tbl5s_UsuarioPrivilegios { get; set; }
        public DbSet<tbl5s_UsuarioAreaOperativa> tbl5s_UsuarioAreaOperativa { get; set; }
        #endregion

        #region Capacitación Seguridad
        public DbSet<tblS_CapacitacionSeguridad_PCAdministracionInstructores> tblS_CapacitacionSeguridad_PCAdministracionInstructores { get; set; }
        public DbSet<tblS_CapacitacionSeguridadRolesGrupoTrabajo> tblS_CapacitacionSeguridadRolesGrupoTrabajo { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCursos> tblS_CapacitacionSeguridadCursos { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCursosExamen> tblS_CapacitacionSeguridadCursosExamen { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCursosPuestos> tblS_CapacitacionSeguridadCursosPuestos { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCursosPuestosAutorizacion> tblS_CapacitacionSeguridadCursosPuestosAutorizacion { get; set; }
        public DbSet<tblS_CapacitacionSeguridadControlAsistencia> tblS_CapacitacionSeguridadControlAsistencia { get; set; }
        public DbSet<tblS_CapacitacionSeguridadControlAsistenciaDetalle> tblS_CapacitacionSeguridadControlAsistenciaDetalle { get; set; }
        public DbSet<tblS_CapacitacionSeguridadAutorizacion> tblS_CapacitacionSeguridadAutorizacion { get; set; }
        public DbSet<tblS_CapacitacionSeguridadEmpleadoPrivilegio> tblS_CapacitacionSeguridadEmpleadoPrivilegio { get; set; }
        public DbSet<tblS_CapacitacionSeguridadExtracurricular> tblS_CapacitacionSeguridadExtracurricular { get; set; }
        public DbSet<tblS_CapacitacionSeguridadRelacionCCAutorizante> tblS_CapacitacionSeguridadRelacionCCAutorizante { get; set; }
        public DbSet<tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial> tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial { get; set; }
        public DbSet<tblS_CapacitacionSeguridadRazonSocial> tblS_CapacitacionSeguridadRazonSocial { get; set; }
        public DbSet<tblS_CapacitacionSeguridadPuestoMando> tblS_CapacitacionSeguridadPuestoMando { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCursosCC> tblS_CapacitacionSeguridadCursosCC { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCursosMando> tblS_CapacitacionSeguridadCursosMando { get; set; }
        public DbSet<tblS_CapacitacionSeguridadListaAutorizacion> tblS_CapacitacionSeguridadListaAutorizacion { get; set; }
        public DbSet<tblS_CapacitacionSeguridadListaAutorizacionRFC> tblS_CapacitacionSeguridadListaAutorizacionRFC { get; set; }
        public DbSet<tblS_CapacitacionSeguridadListaAutorizacionAsistentes> tblS_CapacitacionSeguridadListaAutorizacionAsistentes { get; set; }
        public DbSet<tblS_CapacitacionSeguridadListaAutorizacionInteresados> tblS_CapacitacionSeguridadListaAutorizacionInteresados { get; set; }
        public DbSet<tblS_CapacitacionSeguridadListaAutorizacionCC> tblS_CapacitacionSeguridadListaAutorizacionCC { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajo> tblS_CapacitacionSeguridadDNCicloTrabajo { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> tblS_CapacitacionSeguridadDNCicloTrabajoAreas { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> tblS_CapacitacionSeguridadDNCicloTrabajoCriterio { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> tblS_CapacitacionSeguridadDNCicloTrabajoRegistro { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones> tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas> tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNDeteccionPrimaria> tblS_CapacitacionSeguridadDNDeteccionPrimaria { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad> tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas> tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNRecorrido> tblS_CapacitacionSeguridadDNRecorrido { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNRecorridoHallazgo> tblS_CapacitacionSeguridadDNRecorridoHallazgo { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNRecorridoHallazgoLider> tblS_CapacitacionSeguridadDNRecorridoHallazgoLider { get; set; }
        public DbSet<tblS_CapacitacionSeguridadDNRecorridoAreas> tblS_CapacitacionSeguridadDNRecorridoAreas { get; set; }
        public DbSet<tblS_CapacitacionSeguridadIHHColaboradorCapacitacion> tblS_CapacitacionSeguridadIHHColaboradorCapacitacion { get; set; }
        public DbSet<tblS_CapacitacionSeguridadIHHControlHoras> tblS_CapacitacionSeguridadIHHControlHoras { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCentroCostoDivision> tblS_CapacitacionSeguridadCentroCostoDivision { get; set; }
        public DbSet<tblS_CapacitacionSeguridadCO_GCArchivos> tblS_CapacitacionSeguridadCO_GCArchivos { get; set; }
        #endregion

        #region Capacitación Capital Humano
        public DbSet<tblS_CapacitacionCapitalHumanoPuestoMando> tblS_CapacitacionCapitalHumanoPuestoMando { get; set; }
        #endregion

        #region ConciliacionHorometros
        public DbSet<tblM_EncCaratula> tblM_EncCaratula { get; set; }
        public DbSet<tblM_CapCaratula> tblM_CapCaratula { get; set; }
        public DbSet<tblM_CapConciliacionHorometros> tblM_CapConciliacionHorometros { get; set; }
        public DbSet<tblM_CatConsideracionCostoHora> tblM_CatConsideracionCostoHora { get; set; }
        public DbSet<tblM_EncCaratula_Concideracion> tblM_EncCaratulatblM_CatConsideracionCostoHora { get; set; }
        public DbSet<tblM_AutorizacionCaratulaPreciosU> tblM_AutorizacionCaratulaPreciosU { get; set; }
        #endregion

        #region Facultamiento
        public DbSet<tblFa_CatAutorizacion> tblFa_CatAutorizacion { get; set; }
        public DbSet<tblFa_CatFacultamiento> tblFa_CatFacultamiento { get; set; }
        public DbSet<tblFa_CatMonto> tblFa_CatMonto { get; set; }
        public DbSet<tblFa_CatAuth> tblFa_CatAuth { get; set; }
        public DbSet<tblFa_CatPuesto> tblFa_CatPuesto { get; set; }
        #endregion

        #region Kubrix
        public DbSet<tblK_catDivision> tblK_catDivision { get; set; }
        public DbSet<tblK_catCcDiv> tblK_catCcDiv { get; set; }
        public DbSet<tblK_Bal12> tblK_Bal12 { get; set; }
        public DbSet<tblK_CatAvance> tblK_CatAvance { get; set; }
        public DbSet<tblM_Hor_CorteKubrix> tblM_Hor_CorteKubrix { get; set; }
        public DbSet<tblM_Hor_UsuarioExtraEnCorreo> tblM_Hor_UsuarioExtraEnCorreo { get; set; }
        #endregion

        public DbSet<tblP_Usuario_Starsoft> tblP_Usuario_Starsoft { get; set; }
        public DbSet<tblP_Usuario_Starsoft_Compradores> tblP_Usuario_Starsoft_Compradores { get; set; }

        #region enkontrol
        public DbSet<tblP_Usuario_Enkontrol> tblP_Usuario_Enkontrol { get; set; }
        public DbSet<tblCom_Insumos> tblCom_Insumos { get; set; }
        public DbSet<tblCom_PeriodoContable> tblCom_PeriodoContable { get; set; }
        public DbSet<tblCom_InsumosConsigna> tblCom_InsumosConsigna { get; set; }
        public DbSet<tblCom_InsumosLicitacion> tblCom_InsumosLicitacion { get; set; }
        public DbSet<tblCom_InsumosConvenio> tblCom_InsumosConvenio { get; set; }
        public DbSet<tblCom_InsumosConsignaPeru> tblCom_InsumosConsignaPeru{ get; set; }
        public DbSet<tblCom_InsumosLicitacionPeru> tblCom_InsumosLicitacionPeru { get; set; }
        public DbSet<tblCom_InsumosConvenioPeru> tblCom_InsumosConvenioPeru { get; set; }
        public DbSet<tblCom_BloqueoCentroCosto> tblCom_BloqueoCentroCosto { get; set; }
        public DbSet<tblCom_BloqueoAreaCuenta> tblCom_BloqueoAreaCuenta { get; set; }
        public DbSet<tblCom_MAEPROV> tblCom_MAEPROV { get; set; }
        public DbSet<tblCom_CuentasBancosProveedor> tblCom_CuentasBancosProveedor { get; set; }
        public DbSet<tblCom_AutorizarProveedor> tblCom_AutorizarProveedor { get; set; }
        public DbSet<tblCom_VoboAutorizacionEspecial> tblCom_VoboAutorizacionEspecial { get; set; }
        public DbSet<tblCom_sp_proveedoresColombia> tblCom_sp_proveedoresColombia { get; set; }
        public DbSet<tblCom_sp_proveedores> tblCom_sp_proveedores { get; set; }
        public DbSet<tblCom_sp_ciudades> tblCom_sp_ciudades { get; set; }
        public DbSet<tblCom_sp_tipo_prov> tblCom_sp_tipo_prov { get; set; }
        public DbSet<tblCom_sp_tercero> tblCom_sp_tercero { get; set; }
        public DbSet<tblCom_sp_pago_tercero> tblCom_sp_pago_tercero { get; set; }
        public DbSet<tblCom_sp_operacion> tblCom_sp_operacion { get; set; }
        public DbSet<tblCom_sp_tm> tblCom_sp_tm { get; set; }
        public DbSet<tblCom_sb_bancos> tblCom_sb_bancos { get; set; }
        public DbSet<tblCom_sr_regimen> tblCom_sr_regimen { get; set; }
        public DbSet<tblCom_ArchivosAdjuntosProveedores> tblCom_ArchivosAdjuntosProveedores { get; set; }
        public DbSet<tblCom_ProveedoresSocios> tblCom_ProveedoresSocios { get; set; }
        public DbSet<tblCom_sp_proveedores_cta_dep> tblCom_sp_proveedores_cta_dep { get; set; }
        public DbSet<tblCom_FamiliaStarsoftEnkontrol> tblCom_FamiliaStarsoftEnkontrol { get; set; }
        public DbSet<tblCom_CC_Autorizaciones> tblCom_CC_Autorizaciones { get; set; }
        
        #region Requisicion
        public DbSet<tblCom_Req> tblCom_Req { get; set; }
        public DbSet<tblCom_ReqDet> tblCom_ReqDet { get; set; }
        public DbSet<tblCom_ReqDet_Comentarios> tblCom_ReqDet_Comentarios { get; set; }
        public DbSet<tblCom_ReqTipo> tblCom_ReqTipo { get; set; }
        public DbSet<tblCom_Surtido> tblCom_Surtido { get; set; }
        public DbSet<tblCom_SurtidoDet> tblCom_SurtidoDet { get; set; }
        public DbSet<tblCom_InsumosExcepcionUsuarioUso> tblCom_InsumosExcepcionUsuarioUso { get; set; }
        public DbSet<tblCom_ProveedoresLinks> tblCom_ProveedoresLinks { get; set; }
        public DbSet<tblCom_ResponsableCC> tblCom_ResponsableCC { get; set; }
        #endregion
        #region Orden de Compra
        public DbSet<tblCom_OrdenCompra> tblCom_OrdenCompra { get; set; }
        public DbSet<tblCom_OrdenCompraDet> tblCom_OrdenCompraDet { get; set; }
        public DbSet<tblCom_OrdenCompra_Interna> tblCom_OrdenCompra_Interna { get; set; }
        public DbSet<tblCom_OrdenCompraDet_Interna> tblCom_OrdenCompraDet_Interna { get; set; }
        public DbSet<tblCom_OrdenCompra_Retenciones_Interna> tblCom_OrdenCompra_Retenciones_Interna { get; set; }

        public DbSet<tblCom_Pagos> tblCom_Pagos { get; set; }
        public DbSet<tblCom_Retenciones> tblCom_Retenciones { get; set; }
        public DbSet<tblCom_Retenciones_Cat> tblCom_Retenciones_Cat { get; set; }
        public DbSet<tblCom_OCRetenciones> tblCom_OCRetenciones { get; set; }
        public DbSet<tblCom_VoboEmpleadoTipoGrupo> tblCom_VoboEmpleadoTipoGrupo { get; set; }
        public DbSet<tblCom_Comprador> tblCom_Comprador { get; set; }
        public DbSet<tblCom_FamiliasExcepcionInventariables> tblCom_FamiliasExcepcionInventariables { get; set; }
        public DbSet<tblCom_ValidacionInsumoSubcontratista> tblCom_ValidacionInsumoSubcontratista { get; set; }
        public DbSet<tblCom_ComprasReset> tblCom_ComprasReset { get; set; }
        public DbSet<tblCom_CuadroComparativo> tblCom_CuadroComparativo { get; set; }
        public DbSet<tblCom_CuadroComparativoDet> tblCom_CuadroComparativoDet { get; set; }
        public DbSet<tblCom_AutorizanteCentroCosto> tblCom_AutorizanteCentroCosto { get; set; }
        #region Cuadro Comparativo
        public DbSet<tblCom_CC_CatConfiabilidad> tblCom_CC_CatConfiabilidad { get; set; }
        public DbSet<tblCom_CC_Calificacion> tblCom_CC_Calificacion { get; set; }
        public DbSet<tblCom_CC_CalificacionPartida> tblCom_CC_CalificacionPartida { get; set; }
        public DbSet<tblCom_CC_ProveedorNoOptimo> tblCom_CC_ProveedorNoOptimo { get; set; }
        public DbSet<tblCom_CC_PermisoVoboProvNoOptimo> tblCom_CC_PermisoVoboProvNoOptimo { get; set; }
        public DbSet<tblCom_CC_TipoCalificacion> tblCom_CC_TipoCalificacion { get; set; }
        public DbSet<tblCom_CC_PermisoCompradorCalificarOC> tblCom_CC_PermisoCompradorCalificarOC { get; set; }
        #endregion
        #endregion
        public DbSet<tblAlm_Movimientos> tblAlm_Movimientos { get; set; }
        public DbSet<tblAlm_MovimientosDet> tblAlm_MovimientosDet { get; set; }
        public DbSet<tblAlm_StockMinimo> tblAlm_StockMinimo { get; set; }
        public DbSet<tblAlm_Traspaso> tblAlm_Traspaso { get; set; }
        public DbSet<tblAlm_Resguardo> tblAlm_Resguardo { get; set; }
        public DbSet<tblAlm_Insumo> tblAlm_Insumo { get; set; }
        public DbSet<tblAlm_Unidad> tblAlm_Unidad { get; set; }
        public DbSet<tblCom_Comprador_Admin> tblCom_Comprador_Admin { get; set; }
        public DbSet<tblAlm_PermisoFamilias> tblAlm_PermisoFamilias { get; set; }
        public DbSet<tblAlm_PermisoAltaInsumo> tblAlm_PermisoAltaInsumo { get; set; }
        public DbSet<tblAlm_PermisoMovimientoAdministrador> tblAlm_PermisoMovimientoAdministrador { get; set; }
        public DbSet<tblAlm_Validacion_101_102_InsumosExcepciones> tblAlm_Validacion_101_102_InsumosExcepciones { get; set; }
        public DbSet<tblAlm_Validacion_101_102_AlmacenesExcepciones> tblAlm_Validacion_101_102_AlmacenesExcepciones { get; set; }
        public DbSet<tblAlm_FamiliasResguardables> tblAlm_FamiliasResguardables { get; set; }
        public DbSet<tblAlm_RelacionAlmacenPrincipalVirtual> tblAlm_RelacionAlmacenPrincipalVirtual { get; set; }
        public DbSet<tblAlm_BloqueoCCMovimiento> tblAlm_BloqueoCCMovimiento { get; set; }
        public DbSet<tblAlm_FamiliasInventariablesSinValidacion> tblAlm_FamiliasInventariablesSinValidacion { get; set; }
        public DbSet<tblAlm_PermisoDevolucion> tblAlm_PermisoDevolucion { get; set; }
        public DbSet<tblAlm_RelacionCorreoAlmacenCC> tblAlm_RelacionCorreoAlmacenCC { get; set; }
        public DbSet<tblAlm_CierreInventarioFisico> tblAlm_CierreInventarioFisico { get; set; }
        public DbSet<tblAlm_PermisoCierreInventario> tblAlm_PermisoCierreInventario { get; set; }
        public DbSet<tblAlm_Remanente> tblAlm_Remanente { get; set; }
        public DbSet<tblAlm_Almacenistas> tblAlm_Almacenistas { get; set; }
        public DbSet<tblAlm_Almacen> tblAlm_Almacen { get; set; }
        public DbSet<tblAlm_EmpleadoResguardo> tblAlm_EmpleadoResguardo { get; set; }
        public DbSet<tblAlm_Fisico> tblAlm_Fisico { get; set; }
        public DbSet<tblAlm_FisicoDet> tblAlm_FisicoDet { get; set; }
        #endregion

        #region MAZDA
        public DbSet<tblMAZ_Actividad> tblMAZ_Actividad { get; set; }
        public DbSet<tblMAZ_Actividad_Periodo> tblMAZ_Actividad_Periodo { get; set; }
        public DbSet<tblMAZ_Area> tblMAZ_Area { get; set; }
        public DbSet<tblMAZ_Cuadrilla> tblMAZ_Cuadrilla { get; set; }
        public DbSet<tblMAZ_Usuario_Actividad> tblMAZ_Usuario_Actividad { get; set; }
        public DbSet<tblMAZ_Usuario_Cuadrilla> tblMAZ_Usuario_Cuadrilla { get; set; }
        public DbSet<tblMAZ_Equipo_AC> tblMAZ_Equipo_AC { get; set; }
        public DbSet<tblMAZ_PlanMes> tblMAZ_PlanMes { get; set; }
        public DbSet<tblMAZ_PlanMes_Detalle> tblMAZ_PlanMes_Detalle { get; set; }
        public DbSet<tblMAZ_PlanMes_Detalle_Dia> tblMAZ_PlanMes_Detalle_Dia { get; set; }
        public DbSet<tblMAZ_Revision_AC> tblMAZ_Revision_AC { get; set; }
        public DbSet<tblMAZ_Revision_AC_Detalle> tblMAZ_Revision_AC_Detalle { get; set; }
        public DbSet<tblMAZ_Revision_AC_Evidencia> tblMAZ_Revision_AC_Evidencia { get; set; }
        public DbSet<tblMAZ_Revision_Cuadrilla> tblMAZ_Revision_Cuadrilla { get; set; }
        public DbSet<tblMAZ_Revision_Cuadrilla_Detalle> tblMAZ_Revision_Cuadrilla_Detalle { get; set; }
        public DbSet<tblMAZ_Revision_Cuadrilla_Evidencia> tblMAZ_Revision_Cuadrilla_Evidencia { get; set; }
        public DbSet<tblMAZ_Revision_AC_Ayudantes> tblMAZ_Revision_AC_Ayudantes { get; set; }
        public DbSet<tblMAZ_Revision_Cuadrilla_Ayudantes> tblMAZ_Revision_Cuadrilla_Ayudantes { get; set; }
        public DbSet<tblMAZ_Area_Referencia> tblMAZ_Area_Referencia { get; set; }
        public DbSet<tblMAZ_Equipo_Referencia> tblMAZ_Equipo_Referencia { get; set; }
        public DbSet<tblMAZ_Reporte_Actividades> tblMAZ_Reporte_Actividades { get; set; }
        public DbSet<tblMAZ_Reporte_Actividades_Equipo> tblMAZ_Reporte_Actividades_Equipo { get; set; }
        public DbSet<tblMAZ_Reporte_Actividades_Evidencia> tblMAZ_Reporte_Actividades_Evidencia { get; set; }
        public DbSet<tblMAZ_SubArea> tblMAZ_SubArea { get; set; }
        public DbSet<tblMAZ_Subarea_Referencia> tblMAZ_Subarea_Referencia { get; set; }
        #endregion

        #region GestorArchivos
        public DbSet<tblGA_Directorio> tblGA_Directorio { get; set; }
        public DbSet<tblGA_Version> tblGA_Version { get; set; }
        public DbSet<tblGA_Permisos> tblGA_Permisos { get; set; }
        public DbSet<tblGA_Vistas> tblGA_Vistas { get; set; }
        public DbSet<tblGA_AccesoDepartamento> tblGA_AccesoDepartamento { get; set; }

        #endregion

        #region DashboardAlertas
        public DbSet<tblM_DisponibilidadMaquina> tblM_DisponibilidadMaquina { get; set; }
        public DbSet<tblM_RendimientoMaquina> tblM_RendimientoMaquina { get; set; }
        #endregion

        #region Overhaul
        public DbSet<tblM_CatLocacionesComponentes> tblM_CatLocacionesComponentes { get; set; }
        public DbSet<tblM_trackComponentes> tblM_trackComponentes { get; set; }
        public DbSet<tblM_CatModeloEquipotblM_CatSubConjunto> tblM_CatModeloEquipotblM_CatSubConjunto { get; set; }
        public DbSet<tblM_ReporteRemocionComponente> tblM_ReporteRemocionComponente { get; set; }
        public DbSet<tblM_ReporteFalla> tblM_ReporteFalla { get; set; }
        public DbSet<tblM_ReporteFalla_Archivo> tblM_ReporteFalla_Archivo { get; set; }
        public DbSet<tblM_ReporteFalla_Componente> tblM_ReporteFalla_Componente { get; set; }
        public DbSet<tblM_ReporteFalla_Reparacion> tblM_ReporteFalla_Reparacion { get; set; }
        public DbSet<tblM_CapPlaneacionOverhaul> tblM_CapPlaneacionOverhaul { get; set; }
        public DbSet<tblM_CalendarioPlaneacionOverhaul> tblM_CalendarioPlaneacionOverhaul { get; set; }
        public DbSet<tblM_CatServicioOverhaul> tblM_CatServicioOverhaul { get; set; }
        public DbSet<tblM_CatTipoServicioOverhaul> tblM_CatTipoServicioOverhaul { get; set; }
        public DbSet<tblM_CatActividadOverhaul> tblM_CatActividadOverhaul { get; set; }
        public DbSet<tblM_trackServicioOverhaul> tblM_trackServicioOverhaul { get; set; }
        public DbSet<tblM_ComentarioActividadOverhaul> tblM_ComentarioActividadOverhaul { get; set; }

        public DbSet<tblM_PresupuestoOverhaul> tblM_PresupuestoOverhaul { get; set; }
        public DbSet<tblM_PresupuestoHC> tblM_PresupuestoHC { get; set; }
        public DbSet<tblM_DetallePresupuestoOverhaul> tblM_DetallePresupuestoOverhaul { get; set; }


        #endregion

        #region Facultamientos (Departamentos)
        public DbSet<tblFA_Plantilla> tblFA_Plantilla { get; set; }
        public DbSet<tblFA_ConceptoPlantilla> tblFA_ConceptoPlantilla { get; set; }
        public DbSet<tblFA_PlantillatblFA_Grupos> tblFA_PlantillatblFA_Grupos { get; set; }
        public DbSet<tblFA_Paquete> tblFA_Paquete { get; set; }
        public DbSet<tblFA_Empleado> tblFA_Empleado { get; set; }
        public DbSet<tblFA_Autorizante> tblFA_Autorizante { get; set; }
        public DbSet<tblFA_Facultamiento> tblFA_Facultamiento { get; set; }
        #endregion

        #region Bitacora de errores
        public DbSet<tblP_LogError> tblP_LogError { get; set; }
        #endregion

        #region Propuesta de pagos
        public DbSet<tblC_CatReserva> tblC_CatReserva { get; set; }
        public DbSet<tblC_CatCalculoCatReserva> tblC_CatCalculoCatReserva { get; set; }
        public DbSet<tblC_RelCatReservaTp> tblC_RelCatReservaTp { get; set; }
        public DbSet<tblC_RelCatReservaTm> tblC_RelCatReservaTm { get; set; }
        public DbSet<tblC_RelCatReservaCc> tblC_RelCatReservaCc { get; set; }
        public DbSet<tblC_RelCatReservaCalculo> tblC_RelCatReservaCalculo { get; set; }
        public DbSet<tblC_Reserva> tblC_Reserva { get; set; }
        public DbSet<tblC_ReservaDetalle> tblC_ReservaDetalle { get; set; }
        public DbSet<tblC_SaldoConciliado> tblC_SaldoConciliado { get; set; }
        public DbSet<tblC_SaldoConcentrado> tblC_SaldoConcentrado { get; set; }
        public DbSet<tblC_NominaPoliza> tblC_NominaPoliza { get; set; }
        public DbSet<tblC_NominaResumen> tblC_NominaResumen { get; set; }
        public DbSet<tblC_SaldosCondensados> tblC_SaldosCondensados { get; set; }
        public DbSet<tblF_EstimacionResumen> tblF_EstimacionResumen { get; set; }
        public DbSet<tblF_AuthResumenEstimacion> tblF_AuthResumenEstimacion { get; set; }
        public DbSet<tblC_sp_gastos_prov> tblC_sp_gastos_prov { get; set; }
        public DbSet<tblC_sp_gastos_prov_activofijo> tblC_sp_gastos_prov_activofijo { get; set; }
        public DbSet<tblC_CostoEstimado> tblC_CostoEstimado { get; set; }
        #endregion

        #region PRESTAMOS
        public DbSet<tblRH_EK_Prestamos> tblRH_EK_Prestamos { get; set; }
        public DbSet<tblRH_Prestamos_ConfiguracionPrestamo> tblRH_Prestamos_ConfiguracionPrestamo { get; set; }
        public DbSet<tblRH_EK_PrestamosArchivos> tblRH_EK_PrestamosArchivos { get; set; }
        public DbSet<tblRH_EK_PrestamosAutorizaciones> tblRH_EK_PrestamosAutorizaciones { get; set; }
        #endregion

        #region RESERVACION VEHICULO
        public DbSet<tblRV_Solicitudes> tblRV_Solicitudes { get; set; }
        #endregion

        #region Salas
        public DbSet<tblP_Salas> tblP_Salas { get; set; }
        public DbSet<tblOS_SALAS_CatEdificios> tblOS_SALAS_CatEdificios { get; set; }
        public DbSet<tblOS_SALAS_CatEdificiosRelSalas> tblOS_SALAS_CatEdificiosRelSalas { get; set; }
        public DbSet<tblOS_SALAS_Facultamientos> tblOS_SALAS_Facultamientos { get; set; }
        public DbSet<tblOS_SALAS_SalaJuntas> tblOS_SALAS_SalaJuntas { get; set; }
        #endregion

        #region Juntas
        public DbSet<tblP_SalaJunta> tblP_SalaJunta { get; set; }
        #endregion





        #region Captura Nomina Mensual CC
        public DbSet<tblM_CapNominaMensualCC> tblM_CapNominaMensualCC { get; set; }
        #endregion

        #region File Manager
        public DbSet<tblFM_Archivo> tblFM_Archivo { get; set; }
        public DbSet<tblFM_Permiso> tblFM_Permiso { get; set; }
        public DbSet<tblFM_Permisos_Usuario> tblFM_Permisos_Usuario { get; set; }
        public DbSet<tblFM_TipoArchivo> tblFM_TipoArchivo { get; set; }
        public DbSet<tblFM_Version> tblFM_Version { get; set; }
        public DbSet<tblFM_ArchivotblFM_TipoArchivo> tblFM_ArchivotblFM_TipoArchivo { get; set; }
        public DbSet<tblFM_EnvioDocumento> tblFM_EnvioDocumento { get; set; }
        public DbSet<tblFM_PermisoEspecialObra> tblFM_PermisoEspecialObra { get; set; }
        public DbSet<tblFM_Archivo_Base> tblFM_Archivo_Base { get; set; }
        public DbSet<tblFM_Version_Base> tblFM_Version_Base { get; set; }
        public DbSet<tblFM_ArchivotblFM_TipoArchivo_Base> tblFM_ArchivotblFM_TipoArchivo_Base { get; set; }
        #endregion

        #region Divisiones Construplan
        public DbSet<tblP_Division> tblP_Division { get; set; }
        public DbSet<tblP_Subdivision> tblP_Subdivision { get; set; }
        public DbSet<tblC_RelCuentaDivision> tblC_RelCuentaDivision { get; set; }
        #endregion
        #region Plantilla Personal
        public DbSet<tblRH_PP_PlantillaPersonal> tblRH_PP_PlantillaPersonal { get; set; }
        public DbSet<tblRH_PP_PlantillaPersonal_Det> tblRH_PP_PlantillaPersonal_Det { get; set; }
        public DbSet<tblRH_PP_PlantillaPersonal_Aut> tblRH_PP_PlantillaPersonal_Aut { get; set; }
        #endregion

        #region Permiso Especial FileManager
        public DbSet<tblFM_PermisoEspecial> tblFM_PermisoEspecial { get; set; }
        #endregion

        #region Barrenacion

        public DbSet<tblB_OtrosGastos> tblB_OtrosGastos { get; set; }
        public DbSet<tblB_Barrenadora> tblB_Barrenadora { get; set; }
        public DbSet<tblB_PiezaBarrenadora> tblB_PiezaBarrenadora { get; set; }
        public DbSet<tblB_ManoObra> tblB_ManoObra { get; set; }
        public DbSet<tblB_CapturaDiaria> tblB_CapturaDiaria { get; set; }
        public DbSet<tblB_DetalleCaptura> tblB_DetalleCaptura { get; set; }
        public DbSet<tblB_HistorialPieza> tblB_HistorialPieza { get; set; }
        public DbSet<tblB_CatalogoPieza> tblB_CatalogoPieza { get; set; }
        public DbSet<tblB_CatalogoBanco> tblB_CatalogoBanco { get; set; }

        //raguilar incorporacion nuevo modulo 14/01/20
        public DbSet<tblB_BarrenacionCosto> tblB_BarrenacionCosto { get; set; }
        public DbSet<tblB_BarrenacionCostoPiezaDetalle> tblB_BarrenacionCostoPiezaDetalle { get; set; }
        public DbSet<tblB_BarrenacionCostoOtroDetalle> tblB_BarrenacionCostoOtroDetalle { get; set; }
        #endregion

        #region Gestor Corporativo
        public DbSet<tblGC_Archivo> tblGC_Archivo { get; set; }
        public DbSet<tblGC_Permiso> tblGC_Permiso { get; set; }
        #endregion

        #region Flujo Efectivo
        public DbSet<tblC_FE_CatConcepto> tblC_FE_CatConcepto { get; set; }
        public DbSet<tblC_FE_RelConceptoTm> tblC_FE_RelConceptoTm { get; set; }
        public DbSet<tblC_FE_MovPol> tblC_FE_MovPol { get; set; }
        public DbSet<tblC_FE_SaldoInicial> tblC_FE_SaldoInicial { get; set; }
        public DbSet<tblC_FED_Corte> tblC_FED_Corte { get; set; }
        public DbSet<tblC_FED_CatConcepto> tblC_FED_CatConcepto { get; set; }
        public DbSet<tblC_FED_RelConceptoTm> tblC_FED_RelConceptoTm { get; set; }
        public DbSet<tblC_FE_FechaCorte> tblC_FE_FechaCorte { get; set; }
        public DbSet<tblC_FED_CapPlaneacion> tblC_FED_CapPlaneacion { get; set; }
        public DbSet<tblC_FED_SaldoInicial> tblC_FED_SaldoInicial { get; set; }
        public DbSet<tblC_FED_PlaneacionDet> tblC_FED_PlaneacionDet { get; set; }
        public DbSet<tblC_FED_CcVisto> tblC_FED_CcVisto { get; set; }
        public DbSet<tblC_FED_DetProyeccionCierre> tblC_FED_DetProyeccionCierre { get; set; }
        public DbSet<tblC_FED_CatProvedorArrendadora> tblC_FED_CatProvedorArrendadora { get; set; }
        public DbSet<tblC_FED_CatGrupoReserva> tblC_FED_CatGrupoReserva { get; set; }
        public DbSet<tblC_FED_RelObraUsuario> tblC_FED_RelObraUsuario { get; set; }
        #endregion

        public DbSet<tblP_CorreosVinculados> tblP_CorreosVinculados { get; set; }

        public DbSet<excelCaratula> excelCaratula { get; set; }



        #region Kubrix
        public DbSet<tblM_KBCorte> tblM_KBCorte { get; set; }
        public DbSet<tblM_KBCorteDet> tblM_KBCorteDet { get; set; }
        public DbSet<tblM_KBCatCuenta> tblM_KBCatCuenta { get; set; }
        public DbSet<tblM_KBDivision> tblM_KBDivision { get; set; }
        public DbSet<tblM_KBDivisionDetalle> tblM_KBDivisionDetalle { get; set; }
        public DbSet<tblM_KBFletes> tblM_KBFletes { get; set; }
        public DbSet<tblM_KBUsuarioResponsable> tblM_KBUsuarioResponsable { get; set; }
        public DbSet<tblM_KBAreaCuentaResponsable> tblM_KBAreaCuentaResponsable { get; set; }
        public DbSet<tblM_KBCatCC> tblM_KBCatCC { get; set; }

        public DbSet<tblM_KB_Corte> tblM_KB_Corte { get; set; }
        public DbSet<tblM_KB_CorteDet> tblM_KB_CorteDet { get; set; }

        #endregion

        #region Tablas Temporales
        public DbSet<tbl_TraspasosPendientesTEMP> tbl_TraspasosPendientesTEMP { get; set; }
        #endregion
        public DbSet<tblM_SM_Justificacion> tblM_SM_Justificacion { get; set; }

        #region Seguridad - Actos y Condiciones Inseguras
        public DbSet<tblSAC_Acto> tblSAC_Acto { get; set; }
        public DbSet<tblSAC_Clasificacion> tblSAC_Clasificacion { get; set; }
        public DbSet<tblSAC_Accion> tblSAC_Accion { get; set; }
        public DbSet<tblSAC_Condicion> tblSAC_Condicion { get; set; }
        public DbSet<tblSAC_MatrizAccionesDisciplinarias> tblSAC_MatrizAccionesDisciplinarias { get; set; }
        public DbSet<tblSAC_AccionReaccionContactoPersonal> tblSAC_AccionReaccionContactoPersonal { get; set; }
        public DbSet<tblSAC_ClasificacionPrioridad> tblSAC_ClasificacionPrioridad { get; set; }
        public DbSet<tblSAC_ActoAccionReaccion> tblSAC_ActoAccionReaccion { get; set; }
        public DbSet<tblSAC_ClasificacionGeneral> tblSAC_ClasificacionGeneral { get; set; }
        public DbSet<tblSAC_Departamentos> tblSAC_Departamentos { get; set; }
        public DbSet<tblSAC_SubclasificacionDepartamentos> tblSAC_SubclasificacionDepartamentos { get; set; }
        #endregion

        #region CH / ACTOS Y CONDICIONES
        public DbSet<tblRH_AC_Acto> tblRH_AC_Acto { get; set; }
        public DbSet<tblRH_AC_Clasificacion> tblRH_AC_Clasificacion { get; set; }
        public DbSet<tblRH_AC_Accion> tblRH_AC_Accion { get; set; }
        public DbSet<tblRH_AC_Condicion> tblRH_AC_Condicion { get; set; }
        public DbSet<tblRH_AC_MatrizAccionesDisciplinarias> tblRH_AC_MatrizAccionesDisciplinarias { get; set; }
        public DbSet<tblRH_AC_AccionReaccionContactoPersonal> tblRH_AC_AccionReaccionContactoPersonal { get; set; }
        public DbSet<tblRH_AC_ClasificacionPrioridad> tblRH_AC_ClasificacionPrioridad { get; set; }
        public DbSet<tblRH_AC_ActoAccionReaccion> tblRH_AC_ActoAccionReaccion { get; set; }
        public DbSet<tblRH_AC_ClasificacionGeneral> tblRH_AC_ClasificacionGeneral { get; set; }
        public DbSet<tblRH_AC_Departamentos> tblRH_AC_Departamentos { get; set; }
        public DbSet<tblRH_AC_SubclasificacionDepartamentos> tblRH_AC_SubclasificacionDepartamentos { get; set; }
        public DbSet<tblRH_AC_ActoArchivos> tblRH_AC_ActoArchivos { get; set; }
        #endregion

        #region Seguridad - Evaluación de Desempeño
        public DbSet<tblSED_Actividad> tblSED_Actividad { get; set; }
        public DbSet<tblSED_Puesto> tblSED_Puesto { get; set; }
        public DbSet<tblSED_Empleado> tblSED_Empleado { get; set; }
        public DbSet<tblSED_RelPuestoActividad> tblSED_RelPuestoActividad { get; set; }
        public DbSet<tblSED_RelEmpleadoEvaluador> tblSED_RelEmpleadoEvaluador { get; set; }
        public DbSet<tblSED_Evaluacion> tblSED_Evaluacion { get; set; }
        #endregion

        #region Seguridad - Normatividad
        public DbSet<tblNOM_Norma> tblNOM_Norma { get; set; }
        public DbSet<tblNOM_Indicador> tblNOM_Indicador { get; set; }
        public DbSet<tblNOM_Asignacion> tblNOM_Asignacion { get; set; }
        public DbSet<tblNOM_Evidencia> tblNOM_Evidencia { get; set; }
        public DbSet<tblNOM_Evaluacion> tblNOM_Evaluacion { get; set; }
        #endregion

        #region Seguridad - Requerimientos
        public DbSet<tblS_Req_Actividad> tblS_Req_Actividad { get; set; }
        public DbSet<tblS_Req_Condicionante> tblS_Req_Condicionante { get; set; }
        public DbSet<tblS_Req_Seccion> tblS_Req_Seccion { get; set; }
        public DbSet<tblS_Req_Requerimiento> tblS_Req_Requerimiento { get; set; }
        public DbSet<tblS_Req_Punto> tblS_Req_Punto { get; set; }
        public DbSet<tblS_Req_Asignacion> tblS_Req_Asignacion { get; set; }
        public DbSet<tblS_Req_Evidencia> tblS_Req_Evidencia { get; set; }
        public DbSet<tblS_Req_Evidencia_Auditoria> tblS_Req_Evidencia_Auditoria { get; set; }
        public DbSet<tblS_Req_CentroCostoDivision> tblS_Req_CentroCostoDivision { get; set; }
        public DbSet<tblS_Req_Division> tblS_Req_Division { get; set; }
        public DbSet<tblS_Req_Color> tblS_Req_Color { get; set; }
        public DbSet<tblS_Req_EmpleadoAreaCC> tblS_Req_EmpleadoAreaCC { get; set; }
        public DbSet<tblS_Req_LineaNegocio> tblS_Req_LineaNegocio { get; set; }
        #endregion

        #region Cheque
        public DbSet<tblC_sb_cheques> tblC_sb_cheques { get; set; }
        public DbSet<tblC_sb_banco> tblC_sb_banco { get; set; }
        public DbSet<tblC_sb_cuenta> tblC_sb_cuenta { get; set; }
        public DbSet<tblC_sp_movprov> tblC_sp_movpro { get; set; }
        public DbSet<tblC_sc_polizas> tblC_sc_polizas { get; set; }
        public DbSet<tblC_sc_movpol> tblC_sc_movpol { get; set; }
        #endregion

        #region SAAP
        public DbSet<tblSAAP_Actividad> tblSAAP_Actividad { get; set; }
        public DbSet<tblSAAP_RelacionEmpleadoAreaAgrupacion> tblSAAP_RelacionEmpleadoAreaAgrupacion { get; set; }
        public DbSet<tblSAAP_Evidencia> tblSAAP_Evidencia { get; set; }
        public DbSet<tblSAAP_Asignacion> tblSAAP_Asignacion { get; set; }
        public DbSet<tblSAAP_Area> tblSAAP_Area { get; set; }
        #endregion

        #region Seguimiento Compromisos
        public DbSet<tblSC_Actividad> tblSC_Actividad { get; set; }
        public DbSet<tblSC_RelacionEmpleadoAreaAgrupacion> tblSC_RelacionEmpleadoAreaAgrupacion { get; set; }
        public DbSet<tblSC_Evidencia> tblSC_Evidencia { get; set; }
        public DbSet<tblSC_Asignacion> tblSC_Asignacion { get; set; }
        public DbSet<tblSC_Area> tblSC_Area { get; set; }
        #endregion

        #region Documentos por pagar
        public DbSet<tblAF_DxP_Contrato> tblAF_DxP_Contratos { get; set; }
        public DbSet<tblAF_DxP_ContratoDetalle> tblAF_DxP_ContratosDetalle { get; set; }
        public DbSet<tblAF_DxP_ContratoMaquina> tblAF_DxP_ContratoMaquinas { get; set; }
        public DbSet<tblAF_DxP_ContratoMaquinaDetalle> tblAF_DxP_ContratoMaquinasDetalle { get; set; }
        public DbSet<tblAF_DxP_Pago> tblAF_DxP_Pagos { get; set; }
        public DbSet<tblAF_DxP_PagoMaquina> tblAF_DxP_PagosMaquina { get; set; }
        public DbSet<tblAF_DxP_Institucion> tblAF_DxP_Instituciones { get; set; }
        public DbSet<tblAF_DxP_TipoFechaVencimiento> tblAF_DxP_TiposFechaVencimiento { get; set; }
        public DbSet<tblAF_DxP_Deuda> tblAF_DxP_Deuda { get; set; }

        public DbSet<tblAF_DxP_RelInstitucionCta> tblAF_DxP_RelInstitucionCta { get; set; }
        public DbSet<tblAF_DxP_InstitucionesCtasInteres> tblAF_DxP_InstitucionesCtasInteres { get; set; }
        public DbSet<tblP_VistasExcepcionPalabraCC> tblP_VistasExcepcionPalabraCC { get; set; }

        public DbSet<tblAF_DxP_CCContrato> tblAF_DxP_CCContrato { get; set; }
        #endregion

        #region PQ
        public DbSet<tblAF_DxP_PQ> tblAF_DxP_PQ { get; set; }
        public DbSet<tblAF_DxP_PQ_Archivo> tblAF_DxP_PQ_Archivo { get; set; }
        public DbSet<tblAF_DxP_PQ_TipoMovimiento> tblAF_DxP_PQ_TipoMovimiento { get; set; }
        public DbSet<tblAF_DxP_PQ_RelacionMovimiento> tblAF_DxP_PQ_RelacionMovimiento { get; set; }
        public DbSet<tblAF_DxP_PQ_Abono> tblAF_DxP_PQ_Abono { get; set; }
        #endregion

        #region Recursos Humanos - Evaluacion Desempeño
        public DbSet<tblRH_ED_CatEstrategia> tblRH_ED_CatEstrategia { get; set; }
        public DbSet<tblRH_ED_CatEvaluacion> tblRH_ED_CatEvaluacion { get; set; }
        public DbSet<tblRH_ED_CatProceso> tblRH_ED_CatProceso { get; set; }
        public DbSet<tblRH_ED_CatSemaforo> tblRH_ED_CatSemaforo { get; set; }
        public DbSet<tblRH_ED_DetMetas> tblRH_ED_DetMetas { get; set; }
        public DbSet<tblRH_ED_DetObservacion> tblRH_ED_DetObservacion { get; set; }
        public DbSet<tblRH_ED_DetObservacionEvidencia> tblRH_ED_DetObservacionEvidencia { get; set; }
        public DbSet<tblRH_ED_Empleado> tblRH_ED_Empleado { get; set; }
        public DbSet<tblRH_ED_RelacionEmpleadoProceso> tblRH_ED_RelacionesEmpleadoProceso { get; set; }
        #endregion
        #region MURAL
        public DbSet<tblMural_Data> tblMural_Data { get; set; }
        public DbSet<tblMural_Workspace> tblMural_Workspace { get; set; }
        public DbSet<tblMural_Workspace_Members> tblMural_Workspace_Members { get; set; }
        public DbSet<tblRH_Mural> tblRH_Mural { get; set; }
        public DbSet<tblRH_Mural_PostIt> tblRH_Mural_PostIt { get; set; }
        public DbSet<tblRH_Mural_Seccion> tblRH_Mural_Seccion { get; set; }
        #endregion
        public DbSet<tblM_STB_CapturaStandBy> tblM_STB_CapturaStandBy { get; set; }
        public DbSet<tblM_STB_EconomicoBloqueado> tblM_STB_EconomicoBloqueado { get; set; }
        public DbSet<tblM_STB_AccionActivacionEconomico> tblM_STB_AccionActivacionEconomico { get; set; }
        public DbSet<tblM_STB_BitacoraActivacionEconomico> tblM_STB_BitacoraActivacionEconomico { get; set; }
        public DbSet<tblM_STB_UsuarioTipoAutorizacion> tblM_STB_UsuarioTipoAutorizacion { get; set; }
        #region Catalago Centro Costos y Area Cuenta
        public DbSet<tblM_O_CatCCAC> tblM_O_CatCCAC { get; set; }
        public DbSet<tblM_O_CatCCAC_Auth> tblM_O_CatCCAC_Auth { get; set; }
        #endregion
        public DbSet<tblFM_UsuariosPerfil> tblFM_UsuariosPerfil { get; set; }
        public DbSet<tblFA_Grupos> tblFA_Grupos { get; set; }
        public DbSet<tblM_ControlEnvioMaquinaria_CorreosCC> tblM_ControlEnvioMaquinaria_CorreosCC { get; set; }
        #region Sistema Contable - Cuentas
        public DbSet<tblC_PolizaTP> tblC_PolizaTP { get; set; }
        public DbSet<tblC_PolizaITM> tblC_PolizaITM { get; set; }
        public DbSet<tblC_Cta_RelCuentas> tblC_Cta_RelCuentas { get; set; }
        public DbSet<tblC_CP_CuentasProveedores> tblC_CP_CuentasProveedores { get; set; }
        #endregion
        #region Sistema Contable - iTipoMovimiento
        public DbSet<tblC_TM_Relitm> tblC_TM_Relitm { get; set; }
        #endregion
        #region Sistema Contable - Centro Costos
        public DbSet<tblC_CC_RelObras> tblC_CC_RelObras { get; set; }
        #endregion
        #region Sistema Contable - Moneda
        public DbSet<tblC_SC_CatMoneda> tblC_SC_CatMoneda { get; set; }
        public DbSet<tblC_SC_TipoCambio> tblC_SC_TipoCambio { get; set; }
        #endregion
        #region AdministradorProyectos CGP
        public DbSet<tblAP_CGP_MenuArchivos> tblAP_CGP_MenuArchivos { get; set; }
        #endregion
        #region Cuadro Comparativo Equipo
        public DbSet<tblM_CCE_Auth> tblM_CCE_Auth { get; set; }
        public DbSet<tblM_CCE_CatConcepto> tblM_CCE_CatConcepto { get; set; }
        public DbSet<tblM_CCE_DetCaracteristicas> tblM_CCE_DetCaracteristicas { get; set; }
        public DbSet<tblM_CCE_DetEquipo> tblM_CCE_DetEquipo { get; set; }
        public DbSet<tblM_CCE_EncEquipo> tblM_CCE_EncEquipo { get; set; }
        #endregion
        #region Cuadro Comparativo Financiero
        public DbSet<tblM_CCF_Auth> tblM_CCF_Auth { get; set; }
        public DbSet<tblM_CCF_CatConcepto> tblM_CCF_CatConcepto { get; set; }
        public DbSet<tblM_CCF_DetFinanciero> tblM_CCF_DetFinanciero { get; set; }
        public DbSet<tblM_CCF_EncFinanciero> tblM_CCF_EncFinanciero { get; set; }
        #endregion
        public DbSet<tblM_CatMaquina_DocumentosAplica> tblM_CatMaquina_DocumentosAplica { get; set; }

        #region SubContratistas
        public DbSet<tblX_DocumentacionFija> tblX_DocumentacionFija { get; set; }
        public DbSet<tblX_SubContratista> tblX_SubContratista { get; set; }
        public DbSet<tblX_RelacionSubContratistaDocumentacion> tblX_RelacionSubContratistaDocumentacion { get; set; }
        public DbSet<tblX_Contrato> tblX_Contrato { get; set; }
        public DbSet<tblX_Proyecto> tblX_Proyecto { get; set; }
        public DbSet<tblX_Cliente> tblX_Cliente { get; set; }
        #endregion

        #region Control Presupuestal
        public DbSet<tblM_ControlPresupuestalConcepto> tblM_ControlPresupuestalConcepto { get; set; }
        public DbSet<tblM_ControlPresupuestalConceptoCuenta> tblM_ControlPresupuestalConceptoCuenta { get; set; }
        #endregion

        #region Estado Financiero
        public DbSet<tblEF_CorteMes> tblEF_CorteMes { get; set; }
        public DbSet<tblEF_Balanza> tblEF_Balanza { get; set; }
        public DbSet<tblEF_SaldoCC> tblEF_SaldoCC { get; set; }
        public DbSet<tblEF_MovimientoCliente> tblEF_MovimientoCliente { get; set; }
        public DbSet<tblEF_MovimientoProveedor> tblEF_MovimientoProveedor { get; set; }
        public DbSet<tblEF_EdoFinancieroConcepto> tblEF_EdoFinancieroConcepto { get; set; }
        public DbSet<tblEF_TipoMovimientoFlujo> tblEF_TipoMovimientoFlujo { get; set; }
        public DbSet<tblEF_CuentaConcepto> tblEF_CuentaConcepto { get; set; }
        public DbSet<tblEF_CuentaExcepcionConcepto> tblEF_CuentaExcepcionConcepto { get; set; }
        public DbSet<tblEF_GrupoConceptoFlujo> tblEF_GrupoConceptoFlujo { get; set; }
        public DbSet<tblEF_TipoResultado> tblEF_TipoResultado { get; set; }
        public DbSet<tblEF_GrupoConsolidado> tblEF_GrupoConsolidado { get; set; }
        public DbSet<tblEF_ConceptoConsolidados> tblEF_ConceptoConsolidados { get; set; }
        public DbSet<tblEF_ConceptoClienteProveedor> tblEF_ConceptoClienteProveedor { get; set; }
        public DbSet<tblEF_BalanceTipoBalance> tblEF_BalanceTipoBalance { get; set; }
        public DbSet<tblEF_BalanceGrupo> tblEF_BalanceGrupo { get; set; }
        public DbSet<tblEF_BalanceConcepto> tblEF_BalanceConcepto { get; set; }
        public DbSet<tblEF_BalanceCuenta> tblEF_BalanceCuenta { get; set; }
        public DbSet<tblEF_TipoCuenta> tblEF_TipoCuenta { get; set; }
        public DbSet<tblEF_ParteRelacionada> tblEF_ParteRelacionada { get; set; }
        public DbSet<tblEF_ParteRelacionadaDetalle> tblEF_ParteRelacionadaDetalle { get; set; }
        public DbSet<tblEF_Inversion> tblEF_Inversion { get; set; }
        public DbSet<tblEF_InversionDetalle> tblEF_InversionDetalle { get; set; }
        public DbSet<tblEF_BalanceGrupoConsolidado> tblEF_BalanceGrupoConsolidado { get; set; }
        public DbSet<tblEF_BalanceConceptoConsolidado> tblEF_BalanceConceptoConsolidado { get; set; }
        public DbSet<tblEF_BalanceConceptoEmpresaConsolidado> tblEF_BalanceConceptoEmpresaConsolidado { get; set; }
        public DbSet<tblEF_Obra100Porcentaje> tblEF_Obra100Porcentaje { get; set; }
        public DbSet<tblEF_BancoConcepto> tblEF_BancoConcepto { get; set; }
        public DbSet<tblEF_GrupoBancoConcepto> tblEF_GrupoBancoConcepto { get; set; }
        public DbSet<tblEF_BancoConceptoDetalle> tblEF_BancoConceptoDetalle { get; set; }
        public DbSet<tblEF_ClienteProveedorRelacion> tblEF_ClienteProveedorRelacion { get; set; }
        public DbSet<tblEF_Indicadores> tblEF_Indicadores { get; set; }
        #endregion

        #region Transferencias Bancarias
        public DbSet<tblTB_ProveedoresBanorte> tblTB_ProveedoresBanorte { get; set; }
        #endregion

        #region CxC CUENTAS POR COBRAR
        public DbSet<tblCXC_Convenios> tblCXC_Convenios { get; set; }
        public DbSet<tblCXC_Convenios_Det> tblCXC_Convenios_Det { get; set; }
        public DbSet<tblCXC_AutorizantesCC> tblCXC_AutorizantesCC { get; set; }
        public DbSet<tblCXC_Corte> tblCXC_Corte { get; set; }
        public DbSet<tblCXC_Corte_Det> tblCXC_Corte_Det { get; set; }
        public DbSet<tblCXC_Comentarios> tblCXC_Comentarios { get; set; }
        public DbSet<tblCXC_Comentarios_Tipos> tblCXC_Comentarios_Tipos { get; set; }
        public DbSet<tblCXC_CuentasPorCobrar> tblCXC_CuentasPorCobrar { get; set; }
        public DbSet<tblCXC_Permisos> tblCXC_Permisos { get; set; }
        public DbSet<tblCXC_Division> tblCXC_Division { get; set; }
        public DbSet<tblCXC_DivisionDetalle> tblCXC_DivisionDetalle { get; set; }
        public DbSet<tblCXC_FacturasMod> tblCXC_FacturasMod { get; set; }        
        
        #endregion

        //#region CONTROL OBRA
        //public DbSet<tblCO_OC_GestionFirmas> tblCO_OC_GestionFirmas { get; set; }
        //public DbSet<tblCO_OC_RelClavesEmpleadosEntreBD> tblCO_OC_RelClavesEmpleadosEntreBD { get; set; }
        //public DbSet<tblCO_OC_Notificantes> tblCO_OC_Notificantes { get; set; }
        //public DbSet<tblCO_OrdenDeCambio> tblCO_OrdenDeCambio { get; set; }
        //public DbSet<tblCO_OC_Firmas> tblCO_OC_Firmas { get; set; }
        //public DbSet<tblCO_OC_SoportesEvidencia> tblCO_OC_SoportesEvidencia { get; set; }
        //public DbSet<tblCO_OC_Montos> tblCO_OC_Montos { get; set; }
        public DbSet<tblCO_foliador> tblCO_foliador { get; set; }


        //#endregion
        #region MATRIZ DE RIESGO
        public DbSet<tblCO_MatrizDeRiesgo> tblCO_MatrizDeRiesgo { get; set; }
        public DbSet<tblCO_MatrizDeRiesgoDet> tblCO_MatrizDeRiesgoDet { get; set; }
        public DbSet<tblCO_MR_ImpractosSobreObjetivosDelProyecto> tblCO_MR_ImpractosSobreObjetivosDelProyecto { get; set; }
        public DbSet<tblCO_MR_CategoriaDeRiesgo> tblCO_MR_CategoriaDeRiesgo { get; set; }
        public DbSet<tblCO_MR_TipoDeRespuestas> tblCO_MR_TipoDeRespuestas { get; set; }


        #endregion

        #region Conectores
        public MainContext()
            : base(vSesiones.sesionEmpresaActual <= 1 ? "MainContext" : vSesiones.sesionEmpresaActual == 2 ? "MainContextArrendadora" : vSesiones.sesionEmpresaActual == 3 ? "MainContextColombia" : vSesiones.sesionEmpresaActual == 4 ? "MainContextEICI" : vSesiones.sesionEmpresaActual == 5 ? "MainContextIntegradora" : vSesiones.sesionEmpresaActual == 6 ? "MainContextPeru" : vSesiones.sesionEmpresaActual == 8 ? "MainContextGCPLAN" : vSesiones.sesionEmpresaActual == 11 ? "MainContextPruebas" : "")
        {
            //Disable initializer
            Database.SetInitializer<MainContext>(null);
        }

        public MainContext(int bd)
            : base(bd <= 1 ? "MainContext" : bd == 2 ? "MainContextArrendadora" : bd == 3 ? "MainContextColombia" : bd == 4 ? "MainContextEICI" : bd == 5 ? "MainContextIntegradora" : bd == 6 ? "MainContextPeru" : bd == 8 ? "MainContextGCPLAN" : bd == 11 ? "MainContextPruebas" : "")
        {
            //Disable initializer
            Database.SetInitializer<MainContext>(null);
            Database.CommandTimeout = 5000;
        }
        //public MainContextPeruStarSoft(int bd)
        //    : base(bd == 6 : "MainContextPeruStarSoft" : "")
        //{
        //    //Disable initializer
        //    Database.SetInitializer<MainContext>(null);
        //    Database.CommandTimeout = 5000;
        //}

        //   public MainContext(int bd)
        //    : base(bd <= 1 ? "MainContext" : bd == 2 ? "MainContextArrendadora" : bd == 3 ? "MainContextColombia" : bd == 4 ? "MainContextEICI" : bd == 5 ? "MainContextIntegradora" : bd == 6 ? "MainContextPeru" : "")
        //{
        //    //Disable initializer
        //    Database.SetInitializer<MainContext>(null);
        //    Database.CommandTimeout = 5000;
        //}
        public MainContext(EmpresaEnum bd)
        {
            var connectionString = getConectionStringName(bd);
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            Database.SetInitializer<MainContext>(null);
            Database.CommandTimeout = 5000;
        }
        public MainContext(string cadenaConexion)
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings[cadenaConexion].ConnectionString;
            Database.SetInitializer<MainContext>(null);
            Database.CommandTimeout = 5000;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => !String.IsNullOrEmpty(type.Namespace))
                    .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
                foreach(var type in typesToRegister)
                {
                    dynamic configurationInstancie = Activator.CreateInstance(type);
                    modelBuilder.Configurations.Add(configurationInstancie);
                }

                //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
                base.OnModelCreating(modelBuilder);
            }
            catch(Exception e)
            {

            }
        }
        #endregion
        #region stored procedure
        /// <summary>
        /// Guarda o actualiza por medio de prodecimiento almacenado
        /// </summary>
        /// <param name="consulta">DTO de conexión</param>
        /// <returns>respuesta de procedimiento</returns>
        public bool sp_SaveUpdate(StoreProcedureDTO consulta)
        {
            var connectionString = VerificarConectionStringName(consulta);
            using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            using(var command = new SqlCommand(consulta.nombre, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
                try
                {
                    consulta.parametros.ForEach(parametro =>
                        command.Parameters.Add(new SqlParameter(parametro.nombre, parametro.valor))
                    );
                    conn.Open();
                    var i = command.ExecuteNonQuery();
                    return i > 0;
                }
                catch(Exception o_O)
                {
                    return false;
                }
        }
        public List<T> sp_Select<T>(StoreProcedureDTO consulta)
        {
            var connectionString = VerificarConectionStringName(consulta);
            using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            using(var command = new SqlCommand(consulta.nombre, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
                try
                {
                    command.CommandTimeout = 180;
                    consulta.parametros.ForEach(parametro =>
                        command.Parameters.Add(new SqlParameter(parametro.nombre, parametro.valor))
                    );
                    conn.Open();
                    
                    using(SqlDataReader dt = command.ExecuteReader())
                    {
                        return dt.Parse<T>().ToList();
                    }
                }
                catch(Exception o_O)
                {
                    return new List<T>();
                }
        }
        public IEnumerable<T> sp_SelectQuery<T>(StoreProcedureDTO consulta)
        {
            var connectionString = VerificarConectionStringName(consulta);
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            using (var command = new SqlCommand(consulta.nombre, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
                try
                {
                    command.CommandTimeout = 180;
                    consulta.parametros.ForEach(parametro =>
                        command.Parameters.Add(new SqlParameter(parametro.nombre, parametro.valor))
                    );
                    conn.Open();

                    using (SqlDataReader dt = command.ExecuteReader())
                    {
                        return dt.Parse<T>();
                    }
                }
                catch (Exception o_O)
                {
                    return new List<T>();
                }
        }
        #endregion
        #region Dapper
        public List<T> Select<T>(DapperDTO consulta)
        {
            var connectionString = VerificarConectionStringName(consulta);
            using(var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                try
                {
                    return connection.Query<T>(consulta.consulta, consulta.parametros).ToList();
                }
                catch(Exception o_O)
                {
                    return new List<T>();
                }
            }
        }
        public List<dynamic> Select(DapperDTO consulta)
        {
            var connectionString = VerificarConectionStringName(consulta);
            using(var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                try
                {
                    return connection.Query(consulta.consulta, consulta.parametros).ToList();
                }
                catch(Exception o_O)
                {
                    return new List<dynamic>();
                }
            }
        }
        #endregion
        #region Auxiliares
        protected string VerificarConectionStringName(StoreProcedureDTO consulta)
        {
            var connectionString = consulta.baseDatos.GetDescription();
            if(connectionString == "0")
            {
                connectionString = getConectionStringName((EmpresaEnum)consulta.baseDatos);
            }
            return connectionString;
        }
        protected string VerificarConectionStringName(DapperDTO consulta)
        {
            var connectionString = consulta.baseDatos.GetDescription();
            if(connectionString == "0")
            {
                connectionString = getConectionStringName((EmpresaEnum)consulta.baseDatos);
            }
            return connectionString;
        }
        protected string getConectionStringName(EmpresaEnum bd)
        {
            var connectionString = string.Empty;
            if(bd == 0)
            {
                bd = (EmpresaEnum)vSesiones.sesionEmpresaActual;
            }
            switch(bd)
            {
                case EmpresaEnum.Construplan:
                    connectionString = "MainContext";
                    break;
                case EmpresaEnum.Arrendadora:
                    connectionString = "MainContextArrendadora";
                    break;
                case EmpresaEnum.Colombia:
                    connectionString = "MainContextColombia";
                    break;
                case EmpresaEnum.EICI:
                    connectionString = "MainContextEICI";
                    break;
                case EmpresaEnum.Integradora:
                    connectionString = "MainContextIntegradora";
                    break;
                case EmpresaEnum.Peru:
                    connectionString = "MainContextPeru";
                    break;
                case EmpresaEnum.PeruStarSoft:
                    connectionString = "MainContextPeruStarSoft";
                    break;
                case EmpresaEnum.GCPLAN:
                    connectionString = "MainContextGCPLAN";
                    break;
                case EmpresaEnum.Pruebas:
                    connectionString = "MainContextPruebas";
                    break;
                default:
                    //var o_O = new Exception();
                    //throw o_O;
                    connectionString = "MainContext";
                    break;
            }
            return connectionString;
        }
        #endregion
    }
}
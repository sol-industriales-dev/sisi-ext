using Core.DAO.Enkontrol.General.CC;
using Core.DAO.RecursosHumanos.Bajas;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.DTO.RecursosHumanos.Reclutamientos;
using Core.DTO.Utils;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.RecursosHumanos.Reclutamientos;
using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using Core.Entity.Administrativo.Seguridad.Capacitacion;
using Core.Entity.Principal.Catalogos;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Bajas;
using Core.Entity.RecursosHumanos.BajasPersonal;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Entity.RecursosHumanos.Reportes;
using Core.Enum;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal.Usuario;
using Core.Enum.RecursosHumanos.BajasPersonal;
using Core.Enum.RecursosHumanos.CatNotificantes;
using Core.Enum.RecursosHumanos.Reclutamientos;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.RecursosHumanos.ReportesRH;
using Infrastructure.Utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Data.DAO.RecursosHumanos.BajasPersonal
{
    public class BajasPersonalDAO : GenericDAO<tblP_Usuario>, IBajasPersonalDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "BajasPersonal";
        private const int _SISTEMA = (int)SistemasEnum.RH;
        private int idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
        private int idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;

        private readonly string RutaFiniquitos = @"C:\dev\sisi_ext\DOCUMENTOS\CAPITALHUMANO\BAJAS\FINIQUITOS";
        private readonly string RutaFiniquitosLocal = @"C:\dev\sisi_ext\DOCUMENTOS\CAPITALHUMANO\BAJAS\FINIQUITOS";

        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";
        ReportesRHFactoryServices reportesRHFactoryServices;
        ICCDAO _ccFS = new CCFactoryService().getCCService();
        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();

        #region CRUD BAJAS
        public Dictionary<string, object> GetBajasPersonal(List<string> listaCC, int contratable, int prioridad, DateTime? fechaInicio, DateTime? fechaFin,
                                                            int? clave_empleado, string nombre_empleado, DateTime? fechaContaInicio, DateTime? fechaContaFin, string anticipada)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE ESTADOS Y MUNICIPIOS
                List<dynamic> lstEstados = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idEstado, Estado FROM tblP_Estados"
                }).ToList();

                List<dynamic> lstMunicipios = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idMunicipio, Municipio FROM tblP_Municipios"
                }).ToList();
                #endregion

                #region SE OBTIENE CONCEPTOS
                List<tblRH_Baja_Entrevista_Conceptos> lstConceptos = _context.Select<tblRH_Baja_Entrevista_Conceptos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id, preguntaID, concepto, orden, estatus FROM tblRH_Baja_Entrevista_Conceptos WHERE estatus = estatus",
                    parametros = new { estatus = true }
                }).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE EMPLEADOS DE EK
                //                    var odbc = new OdbcConsultaDTO()
                //                    {
                //                        consulta = @"SELECT CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, t1.clave_empleado AS gerente_clave
                //                                        FROM sn_empleados AS t1"
                //                    };
                //                    List<BajaPersonalDTO> lstEmpleadosEK = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.CplanRh, odbc).ToList();
                //                    if (lstEmpleadosEK == null)
                //                        lstEmpleadosEK = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.ArrenRh, odbc).ToList();
                var lstEmpleadosEK = _context.tblRH_EK_Empleados.Select(e => new BajaPersonalDTO
                {
                    nombreCompleto = e.ape_paterno + " " + e.ape_materno + " " + e.nombre,
                    gerente_clave = e.clave_empleado
                }).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE BAJAS
                var filtroCCString = "";
                var ccsPermitidos = new List<string>();

                //if (listaCC != null && listaCC.Count() > 0)
                //{
                //    filtroCCString = "AND t1.cc IN (" + string.Join(", ", listaCC.Select(x => "'" + x + "'").ToList()) + ")";
                //}
                //else
                //{
                //    var permisosUsuarioCC = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).ToList();

                //    if (permisosUsuarioCC.Count() == 0)
                //    {
                //        throw new Exception("No tiene permisos de centros de costo.");
                //    }

                //    if (!permisosUsuarioCC.Select(x => x.cc).Contains("*"))
                //    {
                //        filtroCCString = "AND t1.cc IN (" + string.Join(", ", permisosUsuarioCC.Select(x => "'" + x.cc + "'").ToList()) + ")";
                //    }
                //}

                //if (listaCC == null || listaCC.Count == 0)
                //{
                //    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR || vSesiones.sesionUsuarioDTO.esAuditor)
                //    {
                //        listaCC = null;
                //    }
                //    else
                //    {
                //        ccsPermitidos = ccPermitidos().Select(x => x.cc).ToList();
                //        listaCC = ccsPermitidos.ToList();
                //    }
                //}
                //else
                //{
                //    ccsPermitidos = ccPermitidos().Select(x => x.cc).ToList();
                //    //listaCC = listaCC.Where(x => ccsPermitidos.Contains(x)).ToList();
                //}

                if (listaCC != null && listaCC.Count() > 0)
                {
                    filtroCCString = "AND t1.cc IN (" + string.Join(", ", listaCC.Select(x => "'" + x + "'").ToList()) + ")";
                }

                string stringCondicion = "";

                if (clave_empleado.HasValue && clave_empleado.Value > 0)
                {
                    stringCondicion = " AND t1.numeroEmpleado = " + clave_empleado.Value;
                }
                else if (!string.IsNullOrEmpty(nombre_empleado))
                {
                    stringCondicion = " AND t1. nombre LIKE ('%" + nombre_empleado + "%')";
                }

                if (fechaInicio.HasValue && fechaInicio.Value.Year >= 1900)
                {
                    stringCondicion += " AND t1.fechaBaja >= '" + fechaInicio.Value.ToShortDateString() + "'";
                }
                if (fechaFin.HasValue && fechaFin.Value.Year >= 1900)
                {
                    stringCondicion += " AND t1.fechaBaja <= '" + fechaFin.Value.ToShortDateString() + "'";
                }

                string condicionAnticipada = "";

                if (!string.IsNullOrEmpty(anticipada))
                {
                    if (anticipada == "2")
                    {
                        // PENDIENTE DE NOTIFICAR
                        condicionAnticipada = "AND t1.esPendienteNoti = 1";
                    }
                }

                //List<BajaPersonalDTO> lstUBajas = new List<BajaPersonalDTO>();
                List<BajaPersonalDTO> lstSinIMSSBajas = new List<BajaPersonalDTO>();

                List<BajaPersonalDTO> lstBajas = _context.Select<BajaPersonalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = string.Format(@"
                            SELECT
                                t1.*,
		                        t2.id AS entrevista_id, t2.cc, t2.cc_nombre, t2.gerente_clave, t2.fecha_ingreso, t2.fecha_salida, t2.fecha_nacimiento, t2.anios, t2.estado_civil_clave, t2.estado_civil_nombre, 
		                        t2.escolaridad_clave, t2.escolaridad_nombre, t2.p1_clave, t2.p1_concepto, t2.p2_clave, t2.p2_concepto, t2.p3_1_clave, t2.p3_1_concepto, t2.p3_2_clave, 
		                        t2.p3_2_concepto, t2.p3_3_clave, t2.p3_3_concepto, t2.p3_4_clave, t2.p3_4_concepto, t2.p3_5_clave, t2.p3_5_concepto, t2.p3_6_clave, t2.p3_6_concepto, 
		                        t2.p3_7_clave, t2.p3_7_concepto, t2.p3_8_clave, t2.p3_8_concepto, t2.p3_9_clave, t2.p3_9_concepto, t2.p3_10_clave, t2.p3_10_concepto, t2.p4_clave, 
		                        t2.p4_concepto, t2.p5_clave, t2.p5_concepto, t2.p6_concepto, t2.p7_concepto, t2.p8_clave, t2.p8_concepto, t2.p8_porque, t2.p9_clave, t2.p9_concepto, 
		                        t2.p9_porque, t2.p10_clave, t2.p10_concepto, t2.p10_porque, t2.p11_1_clave, t2.p11_1_concepto, t2.p11_2_clave, t2.p11_2_concepto, t2.p12_clave, t2.p12_concepto, 
		                        t2.p12_porque, t2.p13_clave, t2.p13_concepto, t2.p14_clave, t2.p14_concepto, t2.p14_fecha,
                                t2.p14_porque,(u.apellidoPaterno + ' ' + u.apellidoMaterno + ' ' + u.nombre) as usuarioCreacion_Nombre,
                                t1.dni, t1.idDepartamento, t1.cedula_ciudadania
			                FROM tblRH_Baja_Registro AS t1
			                    LEFT JOIN tblRH_Baja_Entrevista AS t2 ON t1.id = t2.registroID
                                LEFT JOIN tblP_Usuario AS u on u.id = t1.idUsuarioCreacion
				            WHERE t1.registroActivo = @registroActivo AND t1.autorizada <> 2 {0} {1} {2}
					        ORDER BY t1.fechaCreacion", filtroCCString, stringCondicion, condicionAnticipada),
                    parametros = new { registroActivo = true }
                }).ToList();
                 

                if (clave_empleado != null && clave_empleado > 0)
                {
                    lstBajas = lstBajas.Where(x => x.numeroEmpleado == clave_empleado).ToList();
                }

                if (nombre_empleado != null || nombre_empleado != "")
                {
                    lstBajas = lstBajas.Where(x => x.nombre.ToUpper().Trim().Contains(nombre_empleado.ToUpper().Trim())).ToList();
                }

                if (contratable > 0)
                {
                    lstBajas = lstBajas.Where(x => (contratable == 1 ? x.esContratable : !x.esContratable)).ToList();
                }

                if (prioridad > 0)
                {
                    lstBajas = lstBajas.Where(x => (contratable == 1 && prioridad > 0 ? x.prioridad == prioridad : true)).ToList();
                }

                if (fechaInicio != null && fechaFin != null)
                {
                    lstBajas = lstBajas.Where(x => x.fechaBaja > fechaInicio && fechaFin > x.fechaBaja).ToList();
                }

                if (fechaContaInicio != null && fechaContaFin != null)
                {
                    lstBajas = lstBajas.Where(x => x.est_contabilidad == "A" && x.est_contabilidad_fecha.HasValue
                                                && x.est_contabilidad_fecha.Value >= fechaContaInicio && fechaContaFin >= x.est_contabilidad_fecha.Value).ToList();

                }

                #region SE OBTIENE LISTADO DE MOTIVOS DE BAJA
                List<tblRH_EK_Razones_Baja> lstMotivosBaja = new List<tblRH_EK_Razones_Baja>();
                if (lstBajas.Count() > 0)
                {
                    //odbc = new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT clave_razon_baja, desc_motivo_baja FROM sn_razones_baja"
                    //};
                    //lstMotivosBaja = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc).ToList();

                    lstMotivosBaja = _context.tblRH_EK_Razones_Baja.ToList();
                }
                #endregion

                List<int> lstAutorizantesCancel = _context.tblRH_Baja_AutorizantesCancel.Where(e => e.registroActivo).Select(e => e.idUsuario).ToList();

                bool esDiana = false;

                foreach (var item in lstBajas)
                {
                    //if (item.fechaBaja != null)
                    //{
                    //    var objBaja = lstUBajas.FirstOrDefault(e => e.numeroEmpleado == item.numeroEmpleado && e.autorizada != AutorizacionEnum.RECHAZADA);

                    //    if (objBaja == null)
                    //    {
                    //        lstUBajas.Add(item);
                    //    }
                    //    else
                    //    {
                    //        if (item.fechaBaja != null)
                    //        {
                    //            if (objBaja.fechaBaja < item.fechaBaja)
                    //            {
                    //                lstUBajas.Remove(objBaja);
                    //                lstUBajas.Add(item);
                    //            }

                    //        }
                    //    }

                    //}

                    if (item.usuarioCreacion_Nombre != null && item.idUsuarioCreacion != 79397)
                    {
                        
                        #region SE OBTIENE CAPACITACIONES, ACTOS Y HISTORICO DE CC

                        var listaActos = _context.tblSAC_Acto.Where(x => x.activo && x.tipoActo == Core.Enum.Administracion.Seguridad.ActoCondicion.TipoActo.Inseguro && x.claveEmpleado == item.numeroEmpleado).Select(x => x.id).ToList();
                        var listaCapacitaciones = (
                            from cap in _context.tblS_CapacitacionControlAsistencia.Where(x => x.activo && x.asistentes.Select(y => y.claveEmpleado).Contains(item.numeroEmpleado)).ToList()
                            join det in _context.tblS_CapacitacionControlAsistenciaDetalle.Where(x => x.claveEmpleado == item.numeroEmpleado).ToList() on cap.id equals det.controlAsistenciaID
                            select new capacitacionesDTO
                            {
                                id = det.id,
                                cursoID = cap.cursoID,
                                //curso = cap.curso.nombre,
                                curso = cap.curso != null ? cap.curso.nombre : "",
                                cc = cap.cc,
                                examenID = det.examenID,
                                calificacion = det.calificacion,
                                divison = det.division,
                                fecha = cap.fechaCapacitacion.ToShortDateString()
                            }
                        ).ToList();

                        List<tblRH_FormatoCambio> historialLst = _context.tblRH_FormatoCambio.Where(e => e.Clave_Empleado == item.numeroEmpleado && e.Aprobado).ToList();

                        item.cantidadCursos = listaCapacitaciones.GroupBy(x => x.cursoID).Select(x => x.Key).Count();
                        item.cantidadActos = listaActos.Count();
                        item.cantidadHistorico = historialLst.Count();
                        #endregion

                    }
                    else
                    {
                        item.cantidadCursos = 0;
                        item.cantidadActos = 0;
                        item.cantidadHistorico = 0;
                        item.usuarioCreacion_Nombre = null;
                    }


                    #region SE OBTIENE MOTIVOS DE BAJA
                    int idMotivo = item.motivoBajaDeSistema != "--Seleccione--" ? Convert.ToInt32(item.motivoBajaDeSistema) : 0;
                    if (idMotivo > 0)
                        //item.motivoBajaDeSistema = lstMotivosBaja.Where(w => w.clave_razon_baja == idMotivo).Select(s => s.desc_motivo_baja).FirstOrDefault();
                        item.motivoBajaDeSistema = lstMotivosBaja.Where(w => w.clave_razon_baja == idMotivo).Select(s => s.desc_motivo_baja).FirstOrDefault();
                    else
                        item.motivoBajaDeSistema = string.Empty;
                    #endregion

                    #region SE OBTIENE ESTADO Y MUNICIPIO
                    int idEstado = item.idEstado;
                    if (idEstado > 0)
                        item.estado = lstEstados.Where(w => w.idEstado == item.idEstado).Select(s => s.Estado).FirstOrDefault();
                    else
                        item.estado = string.Empty;

                    int idCiudad = item.idCiudad;
                    if (idCiudad > 0)
                        item.ciudad = lstMunicipios.Where(w => w.idMunicipio == item.idCiudad).Select(s => s.Municipio).FirstOrDefault();
                    else
                        item.municipio = string.Empty;

                    int idMunicipio = item.idMunicipio;
                    if (idMunicipio > 0)
                        item.municipio = lstMunicipios.Where(w => w.idMunicipio == item.idMunicipio).Select(s => s.Municipio).FirstOrDefault();
                    else
                        item.municipio = string.Empty;
                    #endregion

                    #region SE OBTIENE EL NIVEL DE PRIORIDA EN STRING
                    if (item.prioridad == (int)PrioridadEnum.alta)
                        item.strPrioridad = EnumHelper.GetDescription(PrioridadEnum.alta);
                    else if (item.prioridad == (int)PrioridadEnum.media)
                        item.strPrioridad = EnumHelper.GetDescription(PrioridadEnum.media);
                    else if (item.prioridad == (int)PrioridadEnum.baja)
                        item.strPrioridad = EnumHelper.GetDescription(PrioridadEnum.baja);
                    else
                        item.strPrioridad = string.Empty;
                    #endregion

                    #region SE OBTIENE ESTADO CIVIL
                    int idEstadoCivil = item.estado_civil_clave;
                    if (idEstadoCivil > 0)
                        item.estado_civil_nombre = _context.tblP_EstadoCivil.Where(w => w.id == idEstadoCivil).Select(s => s.estadoCivil).FirstOrDefault();
                    #endregion

                    #region SE OBTIENE ESCOLARIDAD
                    int idEscolaridad = item.escolaridad_clave;
                    if (idEstadoCivil > 0)
                        item.escolaridad_nombre = _context.tblP_CatEscolaridades.Where(w => w.id == idEscolaridad).Select(s => s.escolaridad).FirstOrDefault();
                    #endregion

                    #region SE OBTIENE LOS CONCEPTOS EN BASE A LA CLAVE SELECCIONADA
                    item.p1_concepto = GetConcepto(lstConceptos, item.p1_clave);
                    item.p2_concepto = GetConcepto(lstConceptos, item.p2_clave);
                    item.p3_1_concepto = GetConcepto(lstConceptos, item.p3_1_clave);
                    item.p3_2_concepto = GetConcepto(lstConceptos, item.p3_2_clave);
                    item.p3_3_concepto = GetConcepto(lstConceptos, item.p3_3_clave);
                    item.p3_4_concepto = GetConcepto(lstConceptos, item.p3_4_clave);
                    item.p3_5_concepto = GetConcepto(lstConceptos, item.p3_5_clave);
                    item.p3_6_concepto = GetConcepto(lstConceptos, item.p3_6_clave);
                    item.p3_7_concepto = GetConcepto(lstConceptos, item.p3_7_clave);
                    item.p3_8_concepto = GetConcepto(lstConceptos, item.p3_8_clave);
                    item.p3_9_concepto = GetConcepto(lstConceptos, item.p3_9_clave);
                    item.p3_10_concepto = GetConcepto(lstConceptos, item.p3_10_clave);
                    item.p4_concepto = GetConcepto(lstConceptos, item.p4_clave);
                    item.p5_concepto = GetConcepto(lstConceptos, item.p5_clave);
                    item.p8_concepto = GetConcepto(lstConceptos, item.p8_clave);
                    item.p9_concepto = GetConcepto(lstConceptos, item.p9_clave);
                    item.p10_concepto = GetConcepto(lstConceptos, item.p10_clave);
                    item.p11_1_concepto = GetConcepto(lstConceptos, item.p11_1_clave);
                    item.p11_2_concepto = GetConcepto(lstConceptos, item.p11_2_clave);
                    item.p12_concepto = GetConcepto(lstConceptos, item.p12_clave);
                    item.p13_concepto = GetConcepto(lstConceptos, item.p13_clave);
                    item.p14_concepto = GetConcepto(lstConceptos, item.p14_clave);
                    #endregion

                    #region SE OBTIENE NOMBRE DEL GERENTE
                    if (string.IsNullOrEmpty(item.nombreGerente) && lstEmpleadosEK.Count() > 0)
                    {

                        var objGerente = lstEmpleadosEK.Where(w => w.gerente_clave == item.gerente_clave).FirstOrDefault();
                        if (objGerente != null)
                        {
                            item.nombreGerente = objGerente.nombreCompleto;

                        }
                        else
                        {
                            item.nombreGerente = "";
                        }

                    }
                    #endregion

                    #region NOMBRES DE EMPLEADOS LIBERACION
                    if (item.est_baja == "A" && item.est_baja_usuario != null && item.est_baja_usuario != 0)
                    {
                        //var objBajaAut = lstEmpleadosEK.FirstOrDefault(w => w.gerente_clave == item.est_baja_usuario).);
                        var objBajaAut = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.est_baja_usuario);
                        if (objBajaAut != null)
                        {
                            item.est_baja_nombre = objBajaAut.apellidoPaterno + " " + objBajaAut.apellidoMaterno + " " + objBajaAut.nombre;
                            item.est_baja_nombre = item.est_baja_nombre.ToUpper();

                        }
                        else
                        {
                            item.est_baja_nombre = "";
                        }

                    }
                    if (item.est_inventario == "A" && item.est_inventario_usuario != null && item.est_inventario_usuario != 0)
                    {
                        var objInvenAut = lstEmpleadosEK.Where(w => w.gerente_clave == item.est_inventario_usuario).FirstOrDefault();
                        if (objInvenAut != null)
                        {
                            item.est_inventario_nombre = objInvenAut.nombreCompleto;

                        }
                        else
                        {
                            item.est_inventario_nombre = "";
                        }

                    }
                    if (item.est_compras == "A" && item.est_compras_usuario != null && item.est_compras_usuario != 0)
                    {
                        var objCompAut = lstEmpleadosEK.Where(w => w.gerente_clave == item.est_compras_usuario).FirstOrDefault();
                        if (objCompAut != null)
                        {
                            item.est_compras_nombre = objCompAut.nombreCompleto;

                        }
                        else
                        {
                            item.est_compras_nombre = "";
                        }

                    }
                    if (item.est_contabilidad == "A" && item.est_contabilidad_usuario != null && item.est_contabilidad_usuario != 0)
                    {
                        var objContaAut = lstEmpleadosEK.Where(w => w.gerente_clave == item.est_contabilidad_usuario).FirstOrDefault();
                        if (objContaAut != null)
                        {
                            item.est_contabilidad_nombre = objContaAut.nombreCompleto;

                        }
                        else
                        {
                            item.est_contabilidad_nombre = "";
                        }
                    }
                    #endregion

                    #region SE OBTIENE RUTAS Y MONTOS DE FINIQUITOS EXISTENTES
                    List<tblRH_Baja_Finiquitos> lstFiniquitos = _context.tblRH_Baja_Finiquitos.Where(e => e.idBaja == item.id && e.registroActivo).ToList();
                    var objIMS = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 3);

                    if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 0) != null)
                    {
                        item.montoInicial = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 0).monto;
                        item.rutaFiniquito = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 0).rutaFiniquito;
                        item.archivoFiniquito = Path.GetFileName(item.rutaFiniquito);
                    }
                    if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 1) != null)
                    {
                        item.montoRecalc = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 1).monto;
                        item.rutaRecalc = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 1).rutaFiniquito;
                        item.archivoRecalc = Path.GetFileName(item.rutaRecalc);
                    }
                    if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 2) != null)
                    {
                        item.montoCierre = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 2).monto;
                        item.rutaCierre = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 2).rutaFiniquito;
                        item.archivoCierre = Path.GetFileName(item.rutaCierre);
                    }

                    var dateFechaLimite = item.fechaBaja.AddDays(5);

                    bool esAddSinIMSS = false;
                    if (objIMS != null)
                    {
                        item.montoIMS = objIMS.monto;
                        item.rutaIMS = objIMS.rutaFiniquito;
                        item.archivoIMS = Path.GetFileName(item.rutaIMS);
                        item.fechaCapturaIMS = objIMS.fechaCreacion;

                        //CHECAR SI LA CAPTURA DEL IMS ESTA VENCIDA
                        if (objIMS.fechaCreacion.Date > dateFechaLimite.Date)
                        {
                            item.esVencidoIMS = true;
                        }
                        else
                        {
                            item.esVencidoIMS = false;
                        }
                    }
                    else
                    {
                        esAddSinIMSS = true;
                        if (DateTime.Now.Date > dateFechaLimite.Date)
                        {
                            item.esVencidoIMS = true;
                        }
                        else
                        {
                            item.esVencidoIMS = false;
                        }
                    }
                    
                    #endregion

                    #region FINIQUITO
                    int numDias = (DateTime.Now - item.fechaBaja).Days;
                    item.numDiasFiniquito = numDias;
                    #endregion

                    #region CANCELAR BAJA
                    if (lstAutorizantesCancel.Contains(vSesiones.sesionUsuarioDTO.id))
                    {
                        item.esCancelar = true;
                    }
                    else
                    {
                        item.esCancelar = false;
                    }
                    #endregion

                    #region ESDIANA
                    if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                    {
                        item.esDiana = true;
                    }
                    else
                    {
                        if (vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552 || vSesiones.sesionUsuarioDTO.id == 1041 || vSesiones.sesionUsuarioDTO.id == 3381) //Diana Alvarez, ARANZA, RAFAEL, Maricela
                        {
                            item.esDiana = true;

                        }
                        else
                        {
                            item.esDiana = false;

                        }

                    }
                    #endregion

                    if (esAddSinIMSS)
                    {
                        lstSinIMSSBajas.Add(item);
                    }
                }
                #endregion

                //Sin confirmación de baja IMSS
                if (anticipada == "3")
                {
                    lstBajas = lstSinIMSSBajas;
                }

                //resultado.Add("lstBajas", lstUBajas);
                resultado.Add("lstBajas", lstBajas);
                resultado.Add("esDiana", esDiana);
                resultado.Add("empresa", vSesiones.sesionEmpresaActual);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetBajasPersonal", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetConcepto(List<tblRH_Baja_Entrevista_Conceptos> lstConceptos = null, int idClave = 0)
        {
            string concepto = string.Empty;
            if (idClave > 0)
                concepto = lstConceptos.Where(w => w.id == idClave).Select(s => s.concepto).FirstOrDefault();

            return concepto;
        }

        public Dictionary<string, object> CrearEditarBajaPersonal(BajaPersonalDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();

                #region VALIDACIONES

                #endregion

                #region SE OBTIENE CONCEPTOS
                List<tblRH_Baja_Entrevista_Conceptos> lstConceptos = _context.tblRH_Baja_Entrevista_Conceptos.ToList();
                #endregion

                tblRH_Baja_Registro objCEBaja = new tblRH_Baja_Registro();
                tblRH_Baja_Entrevista objCEEntrevista = new tblRH_Baja_Entrevista();

                var entrevistaCapturada = _context.tblRH_Baja_Entrevista.FirstOrDefault(x => x.registroActivo && x.registroID == objDTO.id);
                int cveEmpleado = Convert.ToInt32(objDTO.numeroEmpleado);

                var dicDatosPersona = GetDatosPersonaReporte(objDTO.numeroEmpleado, objDTO.nombre);
                BajaPersonalDTO bajaPersonalDTO = dicDatosPersona["objDatosPersona"] as BajaPersonalDTO;
                string numCC = bajaPersonalDTO.numCC;
                string descripcionCC = bajaPersonalDTO.descripcionCC;
                int idPuesto = bajaPersonalDTO.idPuesto;
                string nombrePuesto = bajaPersonalDTO.nombrePuesto;
                DateTime? fechaAntiguedad = bajaPersonalDTO.fechaAntiguedad;

                //PERMISOS DE GUARDADO
                #region PERMISOS DE GUARDADO

                var ccs = new List<string>();
                if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                {
                    //ccs = _ccFS.GetCCs().Select(e => e.cc).ToList();
                    ccs = _ccFS_SP.GetCCs().Select(e => e.cc).ToList();
                }
                if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO)
                {
                    var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                    if (usuarioCCs.Count > 0)
                    {
                        if (usuarioCCs.Any(x => x == "*"))
                        {
                            //ccs = _ccFS.GetCCs().Select(e => e.cc).ToList();
                            ccs = _ccFS_SP.GetCCs().Select(e => e.cc).ToList();
                        }
                        else
                        {
                            //ccs = _ccFS.GetCCs(usuarioCCs).Select(e => e.cc).ToList();
                            ccs = _ccFS_SP.GetCCs(usuarioCCs).Select(e => e.cc).ToList();
                        }
                    }
                }

                if (!ccs.Contains(numCC))
                {
                    throw new Exception("Este usuario no tiene permisos para registrar bajas en el cc: " + descripcionCC);
                }

                #endregion

                try
                {
                    if (objDTO.id > 0)
                    {
                        objCEBaja = _context.tblRH_Baja_Registro.Where(w => w.id == objDTO.id).FirstOrDefault();
                        if (objCEBaja == null)
                            throw new Exception("Ocurrió un error al actualizar la baja.");

                        #region se valida fechas de dias de bajas permitidas
                        var fechaActual = DateTime.Now;

                        var periodosPermitidos = _context.tblRH_Bajas_DiasPermitidos.FirstOrDefault(x => x.registroActivo);
                        if (!objDTO.esAnticipada)
                        {
                            if (periodosPermitidos != null)
                            {
                                var fechaMin = fechaActual.AddDays(-periodosPermitidos.anteriores);
                                var fechaMax = fechaActual.AddDays(periodosPermitidos.posteriores);

                                if (_context.tblP_AccionesVistatblP_Usuario.Any(x => x.tblP_AccionesVista_id == 4033 && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id) || vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR ||
                                    objCEBaja.fechaBaja.Value == objDTO.fechaBaja)
                                {

                                }
                                else
                                {
                                    throw new Exception("La fecha de baja tiene que estar entre: " + fechaMin.ToShortDateString() + " y " + fechaMax.ToShortDateString());
                                }
                            }
                            else
                            {
                                if (objCEBaja.fechaBaja.Value.Date != fechaActual.Date)
                                {
                                    throw new Exception("La fecha de baja tiene que ser la actual: " + fechaActual.ToShortDateString());
                                }
                            }
                        }
                        
                        #endregion

                        #region SE OBTIENE EL CC Y PUESTO EN BASE A LA CLAVE DEL EMPLEADO

                        fechaAntiguedad = bajaPersonalDTO.fechaAntiguedad;
                        #endregion

                        #region ACTUALIZA BAJA PERSONAL
                        objCEBaja.numeroEmpleado = objDTO.numeroEmpleado;
                        objCEBaja.nombre = !string.IsNullOrEmpty(objDTO.nombre) ? objDTO.nombre.Trim() : null;
                        objCEBaja.cc = !string.IsNullOrEmpty(numCC) ? numCC.Trim() : null;
                        objCEBaja.descripcionCC = !string.IsNullOrEmpty(descripcionCC) ? descripcionCC.Trim() : null;
                        objCEBaja.idPuesto = idPuesto;
                        objCEBaja.nombrePuesto = !string.IsNullOrEmpty(nombrePuesto) ? nombrePuesto.Trim() : null;
                        objCEBaja.fechaIngreso = fechaAntiguedad;
                        objCEBaja.habilidadesConEquipo = !string.IsNullOrEmpty(objDTO.habilidadesConEquipo) ? objDTO.habilidadesConEquipo : null;
                        objCEBaja.telPersonal = !string.IsNullOrEmpty(objDTO.telPersonal) ? objDTO.telPersonal.Trim() : null;
                        objCEBaja.tieneWha = objDTO.strTieneWha == 1 ? true : false;
                        objCEBaja.telCasa = !string.IsNullOrEmpty(objDTO.telCasa) ? objDTO.telCasa.Trim() : null;
                        objCEBaja.contactoFamilia = !string.IsNullOrEmpty(objDTO.contactoFamilia) ? objDTO.contactoFamilia.Trim() : null;
                        objCEBaja.idDepartamento = objDTO.idDepartamento;
                        objCEBaja.idEstado = objDTO.idEstado;
                        objCEBaja.idCiudad = objDTO.idCiudad;
                        objCEBaja.idMunicipio = objDTO.idMunicipio;
                        objCEBaja.direccion = !string.IsNullOrEmpty(objDTO.direccion) ? objDTO.direccion : null;
                        objCEBaja.facebook = !string.IsNullOrEmpty(objDTO.facebook) ? objDTO.facebook.Trim() : null;
                        objCEBaja.instagram = !string.IsNullOrEmpty(objDTO.instagram) ? objDTO.instagram.Trim() : null;
                        objCEBaja.correo = !string.IsNullOrEmpty(objDTO.correo) ? objDTO.correo.Trim() : null;
                        objCEBaja.fechaBaja = objDTO.fechaBaja;
                        objCEBaja.motivoBajaDeSistema = !string.IsNullOrEmpty(objDTO.motivoBajaDeSistema) ? objDTO.motivoBajaDeSistema.Trim() : null;
                        objCEBaja.motivoSeparacionDeEmpresa = !string.IsNullOrEmpty(objDTO.motivoSeparacionDeEmpresa) ? objDTO.motivoSeparacionDeEmpresa.Trim() : null;
                        objCEBaja.regresariaALaEmpresa = objDTO.strRegresariaALaEmpresa == 1 ? true : false;
                        objCEBaja.porqueRegresariaALaEmpresa = !string.IsNullOrEmpty(objDTO.porqueRegresariaALaEmpresa) ? objDTO.porqueRegresariaALaEmpresa.Trim() : null;
                        objCEBaja.dispuestoCambioDeProyecto = !string.IsNullOrEmpty(objDTO.dispuestoCambioDeProyecto) ? objDTO.dispuestoCambioDeProyecto.Trim() : null;
                        objCEBaja.experienciaEnCP = !string.IsNullOrEmpty(objDTO.experienciaEnCP) ? objDTO.experienciaEnCP.Trim() : null;
                        objCEBaja.esContratable = objDTO.strContratable == 1 ? true : false;
                        objCEBaja.prioridad = objDTO.prioridad;
                        objCEBaja.clave_autoriza = objDTO.clave_autoriza;
                        objCEBaja.nombre_autoriza = objDTO.nombre_autoriza;
                        objCEBaja.idUsuarioModificacion = idUsuarioModificacion;
                        objCEBaja.fechaModificacion = DateTime.Now;
                        objCEBaja.curp = objDTO.curp;
                        objCEBaja.rfc = objDTO.rfc;
                        objCEBaja.nss = objDTO.nss;
                        objCEBaja.dni = objDTO.dni;
                        objCEBaja.cedula_ciudadania = objDTO.cedula_ciudadania;
                        objCEBaja.comentarios = objDTO.comentarios;
                        objCEBaja.comentariosRecontratacion = objDTO.comentariosRecontratacion;
                        objCEBaja.esAnticipada = objDTO.esAnticipada;
                        _context.SaveChanges();
                        #endregion

                        var empleado = _context.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == objCEBaja.numeroEmpleado);
                        if (empleado != null)
                        {
                            if (empleado.estatus_empleado == "A")
                            {
                                if (objCEBaja.esContratable)
                                {
                                    empleado.contratable = "S";
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    empleado.contratable = "N";
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                //throw new Exception("El empleado ya esta dado de baja");
                            }
                        }
                        else
                        {
                            throw new Exception("No se encontro la información del empleado");
                        }

                        #region ACTUALIZA BAJA PERSONAL MIGRADA
                        var objEmplBaja = _context.tblRH_EK_Empl_Baja.FirstOrDefault(e => e.clave_empleado == cveEmpleado);

                        if (objEmplBaja != null)
                        {
                            objEmplBaja.fecha_baja = objDTO.fechaBaja;
                            objEmplBaja.motivo_baja = Convert.ToInt32(objDTO.motivoBajaDeSistema);
                            objEmplBaja.comentarios = objDTO.comentarios;
                            objEmplBaja.clave_autoriza = objDTO.clave_autoriza;
                            _context.SaveChanges();

                        }
                        #endregion
                        
                        if (objDTO.gerente_clave > 0 && entrevistaCapturada == null)
                        {
                            #region REGISTRA ENTREVISTA
                            objCEEntrevista.registroID = objCEBaja.id;
                            objCEEntrevista.fecha_ingreso = objDTO.fechaBaja;
                            objCEEntrevista.fecha_salida = objDTO.fechaBaja;
                            objCEEntrevista.cc = numCC;
                            objCEEntrevista.cc_nombre = descripcionCC;
                            objCEEntrevista.gerente_clave = objDTO.gerente_clave;
                            objCEEntrevista.nombreGerente = objDTO.nombreGerente;
                            objCEEntrevista.fecha_nacimiento = objDTO.fecha_nacimiento;
                            objCEEntrevista.estado_civil_clave = objDTO.estado_civil_clave;
                            objCEEntrevista.estado_civil_nombre = objDTO.estado_civil_clave > 0 ? _context.tblP_EstadoCivil.Where(w => w.id == objCEEntrevista.estado_civil_clave).Select(s => s.estadoCivil).FirstOrDefault() : string.Empty;
                            objCEEntrevista.escolaridad_clave = objDTO.escolaridad_clave;
                            objCEEntrevista.p1_clave = objDTO.p1_clave;
                            objCEEntrevista.p1_concepto = lstConceptos.Where(w => w.id == objDTO.p1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p2_clave = objDTO.p2_clave;
                            objCEEntrevista.p2_concepto = lstConceptos.Where(w => w.id == objDTO.p2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_1_clave = objDTO.p3_1_clave;
                            objCEEntrevista.p3_1_concepto = lstConceptos.Where(w => w.id == objDTO.p3_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_2_clave = objDTO.p3_2_clave;
                            objCEEntrevista.p3_2_concepto = lstConceptos.Where(w => w.id == objDTO.p3_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_3_clave = objDTO.p3_3_clave;
                            objCEEntrevista.p3_3_concepto = lstConceptos.Where(w => w.id == objDTO.p3_3_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_4_clave = objDTO.p3_4_clave;
                            objCEEntrevista.p3_4_concepto = lstConceptos.Where(w => w.id == objDTO.p3_4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_5_clave = objDTO.p3_5_clave;
                            objCEEntrevista.p3_5_concepto = lstConceptos.Where(w => w.id == objDTO.p3_5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_6_clave = objDTO.p3_6_clave;
                            objCEEntrevista.p3_6_concepto = lstConceptos.Where(w => w.id == objDTO.p3_6_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_7_clave = objDTO.p3_7_clave;
                            objCEEntrevista.p3_7_concepto = lstConceptos.Where(w => w.id == objDTO.p3_7_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_8_clave = objDTO.p3_8_clave;
                            objCEEntrevista.p3_8_concepto = lstConceptos.Where(w => w.id == objDTO.p3_8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_9_clave = objDTO.p3_9_clave;
                            objCEEntrevista.p3_9_concepto = lstConceptos.Where(w => w.id == objDTO.p3_9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_10_clave = objDTO.p3_10_clave;
                            objCEEntrevista.p3_10_concepto = lstConceptos.Where(w => w.id == objDTO.p3_10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p4_clave = objDTO.p4_clave;
                            objCEEntrevista.p4_concepto = lstConceptos.Where(w => w.id == objDTO.p4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p5_clave = objDTO.p5_clave;
                            objCEEntrevista.p5_concepto = lstConceptos.Where(w => w.id == objDTO.p5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p6_concepto = objDTO.p6_concepto;
                            objCEEntrevista.p7_concepto = objDTO.p7_concepto;
                            objCEEntrevista.p8_clave = objDTO.p8_clave;
                            objCEEntrevista.p8_concepto = lstConceptos.Where(w => w.id == objDTO.p8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p8_porque = objDTO.p8_porque;
                            objCEEntrevista.p9_clave = objDTO.p9_clave;
                            objCEEntrevista.p9_concepto = lstConceptos.Where(w => w.id == objDTO.p9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p9_porque = objDTO.p9_porque;
                            objCEEntrevista.p10_clave = objDTO.p10_clave;
                            objCEEntrevista.p10_concepto = lstConceptos.Where(w => w.id == objDTO.p10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p10_porque = objDTO.p10_porque;
                            objCEEntrevista.p11_1_clave = objDTO.p11_1_clave;
                            objCEEntrevista.p11_1_concepto = lstConceptos.Where(w => w.id == objDTO.p11_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p11_2_clave = objDTO.p11_2_clave;
                            objCEEntrevista.p11_2_concepto = lstConceptos.Where(w => w.id == objDTO.p11_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_clave = objDTO.p12_clave;
                            objCEEntrevista.p12_concepto = lstConceptos.Where(w => w.id == objDTO.p12_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_porque = objDTO.p12_porque;
                            objCEEntrevista.p13_clave = objDTO.p13_clave;
                            objCEEntrevista.p13_concepto = lstConceptos.Where(w => w.id == objDTO.p13_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_clave = objDTO.p14_clave;
                            objCEEntrevista.p14_concepto = lstConceptos.Where(w => w.id == objDTO.p14_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_fecha = objDTO.p14_fecha;
                            objCEEntrevista.p14_porque = objDTO.p14_porque;
                            objCEEntrevista.registroActivo = true;
                            _context.tblRH_Baja_Entrevista.Add(objCEEntrevista);
                            _context.SaveChanges();
                            #endregion
                        }

                        #region ACTUALIZAR ENTREVISTA
                        //objCEEntrevista.fecha_ingreso = objDTO.fechaIngreso;
                        //objCEEntrevista.fecha_salida = objDTO.fechaBaja;
                        //objCEEntrevista.cc = numCC;
                        //objCEEntrevista.cc_nombre = descripcionCC;
                        //objCEEntrevista.gerente_clave = objDTO.gerente_clave;
                        //objCEEntrevista.nombreGerente = objDTO.nombreGerente;
                        //objCEEntrevista.fecha_nacimiento = objDTO.fecha_nacimiento;
                        //objCEEntrevista.estado_civil_clave = objDTO.estado_civil_clave;
                        //objCEEntrevista.escolaridad_clave = objDTO.escolaridad_clave;
                        //objCEEntrevista.p1_clave = objDTO.p1_clave;
                        //objCEEntrevista.p1_concepto = lstConceptos.Where(w => w.id == objDTO.p1_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p2_clave = objDTO.p2_clave;
                        //objCEEntrevista.p2_concepto = lstConceptos.Where(w => w.id == objDTO.p2_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_1_clave = objDTO.p3_1_clave;
                        //objCEEntrevista.p3_1_concepto = lstConceptos.Where(w => w.id == objDTO.p3_1_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_2_clave = objDTO.p3_2_clave;
                        //objCEEntrevista.p3_2_concepto = lstConceptos.Where(w => w.id == objDTO.p3_2_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_3_clave = objDTO.p3_3_clave;
                        //objCEEntrevista.p3_3_concepto = lstConceptos.Where(w => w.id == objDTO.p3_3_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_4_clave = objDTO.p3_4_clave;
                        //objCEEntrevista.p3_4_concepto = lstConceptos.Where(w => w.id == objDTO.p3_4_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_5_clave = objDTO.p3_5_clave;
                        //objCEEntrevista.p3_5_concepto = lstConceptos.Where(w => w.id == objDTO.p3_5_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_6_clave = objDTO.p3_6_clave;
                        //objCEEntrevista.p3_6_concepto = lstConceptos.Where(w => w.id == objDTO.p3_6_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_7_clave = objDTO.p3_7_clave;
                        //objCEEntrevista.p3_7_concepto = lstConceptos.Where(w => w.id == objDTO.p3_7_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_8_clave = objDTO.p3_8_clave;
                        //objCEEntrevista.p3_8_concepto = lstConceptos.Where(w => w.id == objDTO.p3_8_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_9_clave = objDTO.p3_9_clave;
                        //objCEEntrevista.p3_9_concepto = lstConceptos.Where(w => w.id == objDTO.p3_9_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p3_10_clave = objDTO.p3_10_clave;
                        //objCEEntrevista.p3_10_concepto = lstConceptos.Where(w => w.id == objDTO.p3_10_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p4_clave = objDTO.p4_clave;
                        //objCEEntrevista.p4_concepto = lstConceptos.Where(w => w.id == objDTO.p4_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p5_clave = objDTO.p5_clave;
                        //objCEEntrevista.p5_concepto = lstConceptos.Where(w => w.id == objDTO.p5_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p6_concepto = objDTO.p6_concepto;
                        //objCEEntrevista.p7_concepto = objDTO.p7_concepto;
                        //objCEEntrevista.p8_clave = objDTO.p8_clave;
                        //objCEEntrevista.p8_concepto = lstConceptos.Where(w => w.id == objDTO.p8_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p8_porque = objDTO.p8_porque;
                        //objCEEntrevista.p9_clave = objDTO.p9_clave;
                        //objCEEntrevista.p9_concepto = lstConceptos.Where(w => w.id == objDTO.p9_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p9_porque = objDTO.p9_porque;
                        //objCEEntrevista.p10_clave = objDTO.p10_clave;
                        //objCEEntrevista.p10_concepto = lstConceptos.Where(w => w.id == objDTO.p10_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p10_porque = objDTO.p10_porque;
                        //objCEEntrevista.p11_1_clave = objDTO.p11_1_clave;
                        //objCEEntrevista.p11_1_concepto = lstConceptos.Where(w => w.id == objDTO.p11_1_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p11_2_clave = objDTO.p11_2_clave;
                        //objCEEntrevista.p11_2_concepto = lstConceptos.Where(w => w.id == objDTO.p11_2_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p12_clave = objDTO.p12_clave;
                        //objCEEntrevista.p12_concepto = lstConceptos.Where(w => w.id == objDTO.p12_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p12_porque = objDTO.p12_porque;
                        //objCEEntrevista.p13_clave = objDTO.p13_clave;
                        //objCEEntrevista.p13_concepto = lstConceptos.Where(w => w.id == objDTO.p13_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p14_clave = objDTO.p14_clave;
                        //objCEEntrevista.p14_concepto = lstConceptos.Where(w => w.id == objDTO.p14_clave).Select(s => s.concepto).FirstOrDefault();
                        //objCEEntrevista.p14_fecha = objDTO.p14_fecha;
                        //objCEEntrevista.p14_porque = objDTO.p14_porque;
                        //_context.SaveChanges();
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(MESSAGE, "Se ha actualizado con éxito.");
                    }
                    else
                    {
                        #region VALIDAR QUE NO EXISTE UNA BAJA EN PROCESO PARA EL EMPLEADO
                        var objUltimaBaja = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == objDTO.numeroEmpleado).OrderByDescending(e => e.id).ToList().FirstOrDefault();

                        if (objUltimaBaja != null && (objUltimaBaja.est_compras == "P" || objUltimaBaja.est_inventario == "P" || objUltimaBaja.est_contabilidad == "P"))
                        {
                            LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarBajaPersonal", new Exception("El empleado ya tiene una baja en curso"), AccionEnum.CONSULTA, objUltimaBaja.id, objUltimaBaja);
                            throw new Exception("El empleado ya tiene una baja en curso");
                        }
                        #endregion

                        #region se valida fechas de dias de bajas permitidas
                        var fechaActual = DateTime.Now;

                        var periodosPermitidos = _context.tblRH_Bajas_DiasPermitidos.FirstOrDefault(x => x.registroActivo);
                        if (!objDTO.esAnticipada)
                        {
                            if (periodosPermitidos != null)
                            {
                                var fechaMin = fechaActual.AddDays(-periodosPermitidos.anteriores);
                                var fechaMax = fechaActual.AddDays(periodosPermitidos.posteriores);

                                if (objDTO.fechaBaja.Date >= fechaMin.Date && objDTO.fechaBaja.Date <= fechaMax.Date)
                                {

                                }
                                else
                                {
                                    throw new Exception("La fecha de baja tiene que estar entre: " + fechaMin.ToShortDateString() + " y " + fechaMax.ToShortDateString());
                                }
                            }
                            else
                            {
                                if (objDTO.fechaBaja.Date != fechaActual.Date)
                                {
                                    throw new Exception("La fecha de baja tiene que ser la actual: " + fechaActual.ToShortDateString());
                                }
                            }
                        }
                        
                        #endregion

                        var empleado = _context.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == objDTO.numeroEmpleado);
                        if (empleado != null)
                        {
                            if (empleado.estatus_empleado == "A")
                            {
                                //var bajasPendiente = _context.tblRH_Baja_Registro.Where(x => x.registroActivo && x.numeroEmpleado == objDTO.numeroEmpleado && x.est_baja != "C").ToList();
                                //if (bajasPendiente != null && bajasPendiente.Count() > 0)
                                //{
                                //    var objBajaPendiente = bajasPendiente.OrderByDescending(e => e.fechaBaja).FirstOrDefault();

                                //    DateTime oneWeek = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));

                                //    if (objBajaPendiente.fechaBaja.Value >= oneWeek)
                                //    {
                                //        throw new Exception("Ya se encuentra registrada una solicitud de baja para este empleado");

                                //    }
                                //}
                            }
                            else
                            {
                                throw new Exception("El empleado ya esta dado de baja");
                            }
                        }
                        else
                        {
                            throw new Exception("No se encontro la información del empleado");
                        }

                        #region REGISTRAR BAJA
                        objCEBaja.numeroEmpleado = objDTO.numeroEmpleado;
                        objCEBaja.nombre = !string.IsNullOrEmpty(objDTO.nombre) ? objDTO.nombre.Trim() : null;
                        objCEBaja.cc = !string.IsNullOrEmpty(numCC) ? numCC.Trim() : null;
                        objCEBaja.descripcionCC = !string.IsNullOrEmpty(descripcionCC) ? descripcionCC.Trim() : null;
                        objCEBaja.idPuesto = idPuesto;
                        objCEBaja.nombrePuesto = !string.IsNullOrEmpty(nombrePuesto) ? nombrePuesto.Trim() : null;
                        objCEBaja.fechaIngreso = fechaAntiguedad;
                        objCEBaja.habilidadesConEquipo = !string.IsNullOrEmpty(objDTO.habilidadesConEquipo) ? objDTO.habilidadesConEquipo : null;
                        objCEBaja.telPersonal = !string.IsNullOrEmpty(objDTO.telPersonal) ? objDTO.telPersonal.Trim() : null;
                        objCEBaja.tieneWha = objDTO.strTieneWha == 1 ? true : false;
                        objCEBaja.telCasa = !string.IsNullOrEmpty(objDTO.telCasa) ? objDTO.telCasa.Trim() : null;
                        objCEBaja.contactoFamilia = !string.IsNullOrEmpty(objDTO.contactoFamilia) ? objDTO.contactoFamilia.Trim() : null;
                        objCEBaja.idDepartamento = objDTO.idDepartamento;
                        objCEBaja.idEstado = objDTO.idEstado;
                        objCEBaja.idCiudad = objDTO.idCiudad;
                        objCEBaja.idMunicipio = objDTO.idMunicipio;
                        objCEBaja.direccion = !string.IsNullOrEmpty(objDTO.direccion) ? objDTO.direccion : null;
                        objCEBaja.facebook = !string.IsNullOrEmpty(objDTO.facebook) ? objDTO.facebook.Trim() : null;
                        objCEBaja.instagram = !string.IsNullOrEmpty(objDTO.instagram) ? objDTO.instagram.Trim() : null;
                        objCEBaja.correo = !string.IsNullOrEmpty(objDTO.correo) ? objDTO.correo.Trim() : null;
                        objCEBaja.fechaBaja = objDTO.fechaBaja;
                        objCEBaja.motivoBajaDeSistema = !string.IsNullOrEmpty(objDTO.motivoBajaDeSistema) ? objDTO.motivoBajaDeSistema.Trim() : null;
                        objCEBaja.motivoSeparacionDeEmpresa = !string.IsNullOrEmpty(objDTO.motivoSeparacionDeEmpresa) ? objDTO.motivoSeparacionDeEmpresa.Trim() : null;
                        objCEBaja.regresariaALaEmpresa = objDTO.strRegresariaALaEmpresa == 1 ? true : false;
                        objCEBaja.porqueRegresariaALaEmpresa = !string.IsNullOrEmpty(objDTO.porqueRegresariaALaEmpresa) ? objDTO.porqueRegresariaALaEmpresa.Trim() : null;
                        objCEBaja.dispuestoCambioDeProyecto = !string.IsNullOrEmpty(objDTO.dispuestoCambioDeProyecto) ? objDTO.dispuestoCambioDeProyecto.Trim() : null;
                        objCEBaja.experienciaEnCP = !string.IsNullOrEmpty(objDTO.experienciaEnCP) ? objDTO.experienciaEnCP.Trim() : null;
                        objCEBaja.esContratable = objDTO.strContratable == 1 ? true : false;
                        objCEBaja.prioridad = objDTO.prioridad;
                        objCEBaja.clave_autoriza = objDTO.clave_autoriza;
                        objCEBaja.nombre_autoriza = objDTO.nombre_autoriza;
                        objCEBaja.idUsuarioCreacion = idUsuarioCreacion;
                        objCEBaja.fechaCreacion = DateTime.Now;
                        objCEBaja.registroActivo = true;
                        objCEBaja.curp = objDTO.curp;
                        objCEBaja.rfc = objDTO.rfc;
                        objCEBaja.nss = objDTO.nss;
                        objCEBaja.dni = objDTO.dni;
                        objCEBaja.cedula_ciudadania = objDTO.cedula_ciudadania;
                        objCEBaja.est_baja = "A";
                        objCEBaja.est_baja_fecha = DateTime.Now;
                        objCEBaja.est_baja_usuario = vSesiones.sesionUsuarioDTO.id;
                        objCEBaja.est_inventario = "P";
                        objCEBaja.est_contabilidad = "P";
                        objCEBaja.est_compras = "P";
                        objCEBaja.comentarios = objDTO.comentarios;
                        objCEBaja.comentariosRecontratacion = objDTO.comentariosRecontratacion;
                        objCEBaja.esAnticipada = objDTO.esAnticipada;
                        objCEBaja.autorizada = AutorizacionEnum.AUTORIZADA;
                        _context.tblRH_Baja_Registro.Add(objCEBaja);
                        _context.SaveChanges();
                        #endregion

                        if (objCEBaja.esContratable)
                        {
                            empleado.contratable = "S";
                            _context.SaveChanges();
                        }
                        else
                        {
                            empleado.contratable = "N";
                            _context.SaveChanges();
                        }

                        #region REGISTRA BAJA PERSONAL MIGRADA
                        _context.tblRH_EK_Empl_Baja.Add(new tblRH_EK_Empl_Baja
                        {
                            clave_empleado = objDTO.numeroEmpleado,
                            fecha_baja = objDTO.fechaBaja,
                            motivo_baja = Convert.ToInt32(objDTO.motivoBajaDeSistema),
                            otros_motivos = "",
                            comentarios = objDTO.comentarios,
                            clave_solicita = vSesiones.sesionUsuarioDTO.id,
                            clave_autoriza = objDTO.clave_autoriza,
                            estatus = "P",
                            fecha_solicitud = DateTime.Now,
                            //fecha_autoriza
                            archivo_enviado = false,
                            estatus_inventario = "P",
                            estatus_contabilidad = "P",
                            //fecha_autoriza_conta
                            //fecha_autoriza_inventario
                            //usuario_autoriza_conta
                            //usuario_autoriza_inventario
                            //comentarios_inventario
                            //comentarios_conta
                            //observaciones
                            //fecha_antiguedad
                            //almacen
                            //numero
                            cc_contable = objDTO.cc,
                            //requisicion
                            estatus_compras = "P",
                        });
                        _context.SaveChanges();
                        #endregion

                        #region SE OBTIENE ID DE LA BAJA RECIEN REGISTRADA
                        int id = _context.tblRH_Baja_Registro.OrderByDescending(o => o.id).Select(s => s.id).FirstOrDefault();
                        if (id <= 0)
                            throw new Exception("Ocurrió un error al registrar la baja.");
                        #endregion

                        if (objDTO.gerente_clave > 0 && entrevistaCapturada == null)
                        {
                            #region REGISTRA ENTREVISTA
                            objCEEntrevista.registroID = id;
                            objCEEntrevista.fecha_ingreso = objDTO.fechaBaja;
                            objCEEntrevista.fecha_salida = objDTO.fechaBaja;
                            objCEEntrevista.cc = numCC;
                            objCEEntrevista.cc_nombre = descripcionCC;
                            objCEEntrevista.gerente_clave = objDTO.gerente_clave;
                            objCEEntrevista.nombreGerente = objDTO.nombreGerente;
                            objCEEntrevista.fecha_nacimiento = objDTO.fecha_nacimiento;
                            objCEEntrevista.estado_civil_clave = objDTO.estado_civil_clave;
                            objCEEntrevista.estado_civil_nombre = objDTO.estado_civil_clave > 0 ? _context.tblP_EstadoCivil.Where(w => w.id == objCEEntrevista.estado_civil_clave).Select(s => s.estadoCivil).FirstOrDefault() : string.Empty;
                            objCEEntrevista.escolaridad_clave = objDTO.escolaridad_clave;
                            objCEEntrevista.p1_clave = objDTO.p1_clave;
                            objCEEntrevista.p1_concepto = lstConceptos.Where(w => w.id == objDTO.p1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p2_clave = objDTO.p2_clave;
                            objCEEntrevista.p2_concepto = lstConceptos.Where(w => w.id == objDTO.p2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_1_clave = objDTO.p3_1_clave;
                            objCEEntrevista.p3_1_concepto = lstConceptos.Where(w => w.id == objDTO.p3_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_2_clave = objDTO.p3_2_clave;
                            objCEEntrevista.p3_2_concepto = lstConceptos.Where(w => w.id == objDTO.p3_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_3_clave = objDTO.p3_3_clave;
                            objCEEntrevista.p3_3_concepto = lstConceptos.Where(w => w.id == objDTO.p3_3_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_4_clave = objDTO.p3_4_clave;
                            objCEEntrevista.p3_4_concepto = lstConceptos.Where(w => w.id == objDTO.p3_4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_5_clave = objDTO.p3_5_clave;
                            objCEEntrevista.p3_5_concepto = lstConceptos.Where(w => w.id == objDTO.p3_5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_6_clave = objDTO.p3_6_clave;
                            objCEEntrevista.p3_6_concepto = lstConceptos.Where(w => w.id == objDTO.p3_6_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_7_clave = objDTO.p3_7_clave;
                            objCEEntrevista.p3_7_concepto = lstConceptos.Where(w => w.id == objDTO.p3_7_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_8_clave = objDTO.p3_8_clave;
                            objCEEntrevista.p3_8_concepto = lstConceptos.Where(w => w.id == objDTO.p3_8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_9_clave = objDTO.p3_9_clave;
                            objCEEntrevista.p3_9_concepto = lstConceptos.Where(w => w.id == objDTO.p3_9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p3_10_clave = objDTO.p3_10_clave;
                            objCEEntrevista.p3_10_concepto = lstConceptos.Where(w => w.id == objDTO.p3_10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p4_clave = objDTO.p4_clave;
                            objCEEntrevista.p4_concepto = lstConceptos.Where(w => w.id == objDTO.p4_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p5_clave = objDTO.p5_clave;
                            objCEEntrevista.p5_concepto = lstConceptos.Where(w => w.id == objDTO.p5_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p6_concepto = objDTO.p6_concepto;
                            objCEEntrevista.p7_concepto = objDTO.p7_concepto;
                            objCEEntrevista.p8_clave = objDTO.p8_clave;
                            objCEEntrevista.p8_concepto = lstConceptos.Where(w => w.id == objDTO.p8_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p8_porque = objDTO.p8_porque;
                            objCEEntrevista.p9_clave = objDTO.p9_clave;
                            objCEEntrevista.p9_concepto = lstConceptos.Where(w => w.id == objDTO.p9_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p9_porque = objDTO.p9_porque;
                            objCEEntrevista.p10_clave = objDTO.p10_clave;
                            objCEEntrevista.p10_concepto = lstConceptos.Where(w => w.id == objDTO.p10_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p10_porque = objDTO.p10_porque;
                            objCEEntrevista.p11_1_clave = objDTO.p11_1_clave;
                            objCEEntrevista.p11_1_concepto = lstConceptos.Where(w => w.id == objDTO.p11_1_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p11_2_clave = objDTO.p11_2_clave;
                            objCEEntrevista.p11_2_concepto = lstConceptos.Where(w => w.id == objDTO.p11_2_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_clave = objDTO.p12_clave;
                            objCEEntrevista.p12_concepto = lstConceptos.Where(w => w.id == objDTO.p12_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p12_porque = objDTO.p12_porque;
                            objCEEntrevista.p13_clave = objDTO.p13_clave;
                            objCEEntrevista.p13_concepto = lstConceptos.Where(w => w.id == objDTO.p13_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_clave = objDTO.p14_clave;
                            objCEEntrevista.p14_concepto = lstConceptos.Where(w => w.id == objDTO.p14_clave).Select(s => s.concepto).FirstOrDefault();
                            objCEEntrevista.p14_fecha = objDTO.p14_fecha;
                            objCEEntrevista.p14_porque = objDTO.p14_porque;
                            objCEEntrevista.registroActivo = true;
                            _context.tblRH_Baja_Entrevista.Add(objCEEntrevista);
                            _context.SaveChanges();
                            #endregion

                        }

                        #region AUTORIZACION DENTRO DEL CREAR 24/05/2023
                        objCEBaja.est_baja_firma = GlobalUtils.CrearFirmaDigital(objCEBaja.id, DocumentosEnum.LiberacionContabilidad, vSesiones.sesionUsuarioDTO.id);
                        _context.SaveChanges();

                        //EMPLEADO
                        var objEmpleado = _context.tblRH_EK_Empleados.Where(e => e.clave_empleado == objCEBaja.numeroEmpleado).FirstOrDefault();

                        string descRegPat = "";
                        string cc = objEmpleado.cc_contable;
                        if (objEmpleado.id_regpat != null)
                        {

                            var objRegPat = _context.tblRH_EK_Registros_Patronales.FirstOrDefault(e => e.clave_reg_pat == objEmpleado.id_regpat);
                            if (objRegPat != null)
                            {
                                descRegPat = objRegPat.nombre_corto;
                            }
                        }


                        //if (!objCEBaja.esAnticipada)
                        if (objCEBaja.fechaBaja.Value.Date <= DateTime.Now.Date)
                        {
                            objCEBaja.esPendienteNoti = true;
                            _context.SaveChanges();

                            #region Actualizar estatus de empleado
                            if (objEmpleado != null)
                            {
                                objEmpleado.contratable = objCEBaja.esContratable ? "S" : "N";
                                objEmpleado.estatus_empleado = "B";
                                _context.SaveChanges();
                                SaveBitacora(3, (int)AccionEnum.ACTUALIZAR, objEmpleado.clave_empleado, JsonUtils.convertNetObjectToJson(objEmpleado));
                            }
                            #endregion

                            #region Eliminar Incidencias Futuras Empleado
                            var fechaBaja = objCEBaja.fechaBaja;
                            var claveEmpleadoBaja = objCEBaja.numeroEmpleado;
                            var registroPeriodoSemanal = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_inicial >= fechaBaja && x.tipo_nomina == 1 && x.estatus).OrderBy(x => x.fecha_inicial).FirstOrDefault();
                            var registroPeriodoQuincenal = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_inicial >= fechaBaja && x.tipo_nomina == 4 && x.estatus).OrderBy(x => x.fecha_inicial).FirstOrDefault();

                            if (registroPeriodoSemanal != null)
                            {
                                var periodoSemanal = registroPeriodoSemanal.periodo;
                                var anioPeriodoSemanal = registroPeriodoSemanal.anio;
                                var incidenciasSemanales = _context.tblRH_BN_Incidencia.Where(x => ((x.anio == anioPeriodoSemanal && x.periodo >= periodoSemanal) || (x.anio > anioPeriodoSemanal)) && x.tipo_nomina == 1 && x.estatus != "A").Select(x => x.id).ToList();
                                if (incidenciasSemanales.Count() > 0)
                                {
                                    var incidenciasSemanalesDetalle = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasSemanales.Contains(x.incidenciaID) && x.clave_empleado == claveEmpleadoBaja).ToList();
                                    _context.tblRH_BN_Incidencia_det.RemoveRange(incidenciasSemanalesDetalle);
                                    _context.SaveChanges();
                                }
                            }
                            if (registroPeriodoQuincenal != null)
                            {
                                var periodoQuincenal = registroPeriodoQuincenal.periodo;
                                var anioPeriodoQuincenal = registroPeriodoQuincenal.anio;
                                var incidenciasQuincenales = _context.tblRH_BN_Incidencia.Where(x => ((x.anio == anioPeriodoQuincenal && x.periodo >= periodoQuincenal) || (x.anio > anioPeriodoQuincenal)) && x.tipo_nomina == 4 && x.estatus != "A").Select(x => x.id).ToList();
                                if (incidenciasQuincenales.Count() > 0)
                                {
                                    var incidenciasQuincenalesDetalle = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasQuincenales.Contains(x.incidenciaID) && x.clave_empleado == claveEmpleadoBaja).ToList();
                                    _context.tblRH_BN_Incidencia_det.RemoveRange(incidenciasQuincenalesDetalle);
                                    _context.SaveChanges();
                                }
                            }

                            #endregion

                        }
                        else
                        {
                            //PONER COMO "PENDIENTE DAR DE BAJA" LAS BAJAS POSTERIORES
                            objCEBaja.esPendienteDarBaja = true;
                            objCEBaja.esPendienteNoti = true;
                            _context.SaveChanges();
                        }

                        #endregion

                        //var objAutoriza = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == objDTO.clave_autoriza.Value.ToString());
                        string empresaDesc = "";

                        #region EMPRESA
                        switch (vSesiones.sesionEmpresaActual)
                        {
                            case 1:
                                empresaDesc = "CONSTRUPLAN";
                                break;
                            case 2:
                                empresaDesc = "ARRENDADORA";
                                break;
                            case 3:
                                empresaDesc = "CONSTRUPLAN COLOMBIA";
                                break;
                            case 6:
                                empresaDesc = "CONSTRUPLAN PERÚ";
                                break;

                            default:
                                break;
                        }
                        #endregion

                        var correos = new List<string>();
                        var asunto = ". BAJA APLICADA en el CC " + bajaPersonalDTO.cc + " " + bajaPersonalDTO.nombre_corto;
                        var mensaje = @"El siguiente empleado se necesita notificar y liberar:<br/><br/>
                                        " + objDTO.numeroEmpleado + " – " + objDTO.nombre + ", para el centro de costos: " + bajaPersonalDTO.cc + ", con el puesto de: " + nombrePuesto + ".";
                        mensaje += "<p>FECHA BAJA: " + (objCEBaja.fechaBaja.HasValue ? objCEBaja.fechaBaja.Value.ToShortDateString() : "") + "</p>";

                        if (objDTO.esAnticipada)
                        {
                            asunto += " - ( *BAJA ANTICIPADA* )";
                        }

                        //List<int> listaUsuariosCorreos = _context.tblRH_REC_Notificantes_Altas.Where(w => bajaPersonalDTO.cc == w.cc || w.cc == "*" && w.esAuth && w.esActivo && w.notificarBaja).Select(s => s.idUsuario).ToList();
                        var lstUsuarios = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                        //foreach (var usu in listaUsuariosCorreos)
                        //{
                        //    correos.Add(lstUsuarios.FirstOrDefault(x => x.id == usu).correo);
                        //}

                        List<int> lstNotificantes = _context.tblRH_Notis_RelConceptoUsuario.
                            Where(e => e.cc == cc && (e.idConcepto == (int)ConceptosNotificantesEnum.Bajas
                                || e.idConcepto == (int)ConceptosNotificantesEnum.CH || e.idConcepto == (int)ConceptosNotificantesEnum.Almacen || e.idConcepto == (int)ConceptosNotificantesEnum.Taller)).
                            Select(e => e.idUsuario).ToList();

                        foreach (var usu in lstNotificantes)
                        {
                            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        }

                        List<string> lstCorreoGenerales = _context.tblRH_Notis_RelConceptoCorreo.
                            Where(e => (e.cc == "*" || e.cc == cc) && (e.idConcepto == (int)ConceptosNotificantesEnum.Bajas
                                || e.idConcepto == (int)ConceptosNotificantesEnum.CH || e.idConcepto == (int)ConceptosNotificantesEnum.Almacen || e.idConcepto == (int)ConceptosNotificantesEnum.Taller)).
                            Select(e => e.correo).ToList();

                        foreach (var correo in lstCorreoGenerales)
                        {
                            correos.Add(correo);
                        }

                        var usuarios = new List<tblP_Usuario>();

                        var plantillasResponsableCC = new List<int>();

                        tblFA_Paquete paquete = null;

                        switch (vSesiones.sesionEmpresaActual)
                        {
                            case (int)EmpresaEnum.Construplan:
                            case (int)EmpresaEnum.GCPLAN:
                                plantillasResponsableCC = new List<int> { 123 };
                                paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                                break;
                            case (int)EmpresaEnum.Arrendadora:


                                plantillasResponsableCC = new List<int> { 123 };
                                if (!string.IsNullOrEmpty(cc))
                                {
                                    var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                                    paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                                }
                                break;
                            case (int)EmpresaEnum.Colombia:
                                plantillasResponsableCC = new List<int> { 123 };
                                if (!string.IsNullOrEmpty(cc))
                                {
                                    var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                                    paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                                }
                                break;
                            case (int)EmpresaEnum.Peru:
                                plantillasResponsableCC = new List<int> { 123 };
                                if (!string.IsNullOrEmpty(cc))
                                {
                                    var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                                    paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                                }
                                break;
                        }

                        if (paquete != null)
                        {
                            foreach (var facultamiento in paquete.facultamientos.Where(x => plantillasResponsableCC.Contains(x.plantillaID) && x.aplica))
                            {
                                foreach (var item in facultamiento.empleados.Where(x => x.esActivo && x.aplica))
                                {
                                    var usuario = lstUsuarios.FirstOrDefault(e => e.cveEmpleado == item.claveEmpleado.ToString());
                                    usuarios.Add(usuario);
                                }
                            }
                        }

                        if (usuarios.Count() > 0 )
	                    {
                            correos.Add(usuarios.FirstOrDefault().correo);
		 
	                    }

                        correos.Add("diana.alvarez@construplan.com.mx");
                        correos.Add("keyla.vasquez@construplan.com.mx");
                        correos.Add("rigoberto.coronado@construplan.com.mx");

                        correos = correos.Distinct().ToList();
#if DEBUG
                        correos = new List<string> { "miguel.buzani@construplan.com.mx" };
#endif

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos);
                        

                        resultado = new Dictionary<string, object>();
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                    }

                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    resultado = new Dictionary<string, object>();
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearEditarBajaPersonal", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarBajaPersonal(int idBaja)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE ELIMINA LA BAJA SELECCIONADA
                    tblRH_Baja_Registro objEliminar = _context.tblRH_Baja_Registro.Where(w => w.id == idBaja).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar la baja.");

                    if (objEliminar.est_baja == "P")
                    {
                        objEliminar.fechaModificacion = DateTime.Now;
                        objEliminar.idUsuarioModificacion = idUsuarioModificacion;
                        objEliminar.registroActivo = false;
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("La baja necesita estar cancelada antes de eliminarse.");
                    }
                    
                    #endregion

                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();

                    //SAVE BITACORA
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarBajaPersonal", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarBajaPersonal(int idBaja)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENEN LOS DATOS DE LA BAJA SELECCIONADA
                    BajaPersonalDTO objBaja = _context.Select<BajaPersonalDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT
                                        *,
                                        fechaIngreso AS fechaAntiguedad
                                    FROM
                                        tblRH_Baja_Registro AS baja
                                    WHERE
                                        baja.id = @idBaja",
                        parametros = new { idBaja = idBaja }
                    }).FirstOrDefault();
                    if (objBaja == null)
                        throw new Exception("Ocurrió un error al obtener la información de la baja.");

                    //var cc = _ccFS.GetCC(objBaja.cc);
                    var cc = _ccFS_SP.GetCC(objBaja.cc);
                    if (cc != null)
                    {
                        objBaja.cc = objBaja.cc + " - " + cc.descripcion;
                        var empleado = _context.tblRH_EK_Empleados.FirstOrDefault(x => x.clave_empleado == objBaja.numeroEmpleado);
                        //objBaja.fechaIngreso = objBaja.fechaAntiguedad.HasValue ? objBaja.fechaAntiguedad.Value : empleado.fecha_antiguedad.Value;
                        if (!objBaja.fechaAntiguedad.HasValue)
                        {
                            objBaja.fechaIngreso = empleado.fecha_antiguedad.Value;
                        }
                        else
                        {
                            objBaja.fechaIngreso = objBaja.fechaAntiguedad.Value;
                        }

                        //var fechaIngresoAntesDeLaBaja = _context.tblRH_EK_Contratos_Empleados.Where(x => x.clave_empleado == objBaja.numeroEmpleado && x.esActivo.HasValue && x.esActivo.Value && x.fecha_aplicacion < objBaja.fechaBaja).OrderByDescending(x => x.fecha_aplicacion).FirstOrDefault();
                        //if (fechaIngresoAntesDeLaBaja != null && fechaIngresoAntesDeLaBaja.fecha_aplicacion.HasValue)
                        //{
                        //    objBaja.fechaIngreso = fechaIngresoAntesDeLaBaja.fecha_aplicacion.Value;
                        //}
                    }
                    #endregion

                    #region SE OBTIENEN LOS DATOS DE LA ENTREVISTA
                    BajaPersonalDTO objEntrevista = _context.Select<BajaPersonalDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id, registroID, cc, cc_nombre, gerente_clave, nombreGerente, fecha_ingreso, fecha_salida, fecha_nacimiento, anios, estado_civil_clave, estado_civil_nombre, 
                                            escolaridad_clave, escolaridad_nombre, p1_clave, p1_concepto, p2_clave, p2_concepto, p3_1_clave, p3_1_concepto, p3_2_clave, p3_2_concepto, 
                                            p3_3_clave, p3_3_concepto, p3_4_clave, p3_4_concepto, p3_5_clave, p3_5_concepto, p3_6_clave, p3_6_concepto, p3_7_clave, p3_7_concepto, 
                                            p3_8_clave, p3_8_concepto, p3_9_clave, p3_9_concepto, p3_10_clave, p3_10_concepto, p4_clave, p4_concepto, p5_clave, p5_concepto, p6_concepto, 
                                            p7_concepto, p8_clave, p8_concepto, p8_porque, p9_clave, p9_concepto, p9_porque, p10_clave, p10_concepto, p10_porque, p11_1_clave, p11_1_concepto, 
                                            p11_2_clave, p11_2_concepto, p12_clave, p12_concepto, p12_porque, p13_clave, p13_concepto, p14_clave, p14_concepto, p14_fecha, p14_porque, dni, idDepartamento,
                                            cedula_ciudadania
                                                FROM tblRH_Baja_Entrevista 
                                                    WHERE registroID = @registroID AND registroActivo = @registroActivo",
                        parametros = new { registroID = idBaja, registroActivo = true }
                    }).FirstOrDefault();
                    if (objBaja == null)
                        throw new Exception("Ocurrió un error al obtener la información de la entrevista.");

                    int idMotivo = !string.IsNullOrEmpty(objBaja.motivoBajaDeSistema) ? Convert.ToInt32(objBaja.motivoBajaDeSistema) : 0;

                    var objMotivo = _context.tblRH_EK_Razones_Baja.FirstOrDefault(e => e.clave_razon_baja == idMotivo);

                    objBaja.strMotivoBaja = objMotivo != null ? objMotivo.desc_motivo_baja : "";

                    List<tblRH_Baja_Finiquitos> lstFiniquitos = _context.tblRH_Baja_Finiquitos.Where(e => e.idBaja == objBaja.id && e.registroActivo).ToList();

                    if (lstFiniquitos != null && lstFiniquitos.Count > 0)
                    {
                        if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 0) != null)
                        {
                            objBaja.montoInicial = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 0).monto;
                            objBaja.rutaFiniquito = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 0).rutaFiniquito;
                        }
                        if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 1) != null)
                        {
                            objBaja.montoRecalc = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 1).monto;
                            objBaja.rutaRecalc = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 1).rutaFiniquito;
                        }
                        if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 2) != null)
                        {
                            objBaja.montoCierre = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 2).monto;
                            objBaja.rutaCierre = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 2).rutaFiniquito;
                        }
                        if (lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 3) != null)
                        {
                            objBaja.montoIMS = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 3).monto;
                            objBaja.rutaIMS = lstFiniquitos.FirstOrDefault(e => e.tipoFiniquito == 3).rutaFiniquito;
                        }
                    }

                    resultado.Add("objBaja", objBaja);
                    resultado.Add("objEntrevista", objEntrevista);
                    #endregion

                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarBajaPersonal", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> VisualizarDocumento(int idBaja, int tipoFiniquito)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE DOCUMENTO DEL FINIQUITO
                tblRH_Baja_Finiquitos objDocumento = _context.tblRH_Baja_Finiquitos.Where(w => w.idBaja == idBaja && w.tipoFiniquito == tipoFiniquito && w.registroActivo).FirstOrDefault();
                if (objDocumento == null)
                    throw new Exception("No se encontro el documento a visualizar.");

#if DEBUG
                string documento = string.Format("C:\\Proyecto\\SIGOPLAN\\CAPITAL_HUMANO\\BAJAS\\FINIQUITOS\\{0}", "07-07-2022 18-36-03 CH 0063315 CHENO MADRID HORACIO.pdf");
                Stream fileStream = GlobalUtils.GetFileAsStream(documento);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);
#else
                Stream fileStream = GlobalUtils.GetFileAsStream(objDocumento.rutaFiniquito);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);
#endif

                resultado.Add(SUCCESS, true);
                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(objDocumento.rutaFiniquito).ToUpper());
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "VisualizarDocumento", e, AccionEnum.CONSULTA, idBaja, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosPersona(int claveEmpleado, string nombre)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE CC, PUESTO Y FECHA DE INGRESO EN BASE AL CLAVE DEL EMPLEADO
                //                var odbc = new OdbcConsultaDTO()
                //                {
                //                    consulta = @"
                //                        SELECT
                //                            CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
                //                            CONVERT(VARCHAR(200), t3.cc) + ' - ' + CONVERT(VARCHAR(200), t3.descripcion) AS cc, 
                //                            t2.descripcion AS nombrePuesto,  t1.fecha_antiguedad AS fechaIngreso, t1.puesto, t3.cc AS numCC, t3.descripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss,
                //                            t1.clave_ciudad_nac AS idCiudad,
                //                            t1.clave_estado_nac AS idEstado
                //                        FROM sn_empleados AS t1
                //                            INNER JOIN si_puestos AS t2 ON t1.puesto = t2.puesto
                //                            INNER JOIN cc AS t3 ON t1.cc_contable = t3.cc
                //                        WHERE t1.clave_empleado = ?",
                //                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
                //                }; //OMAR
                //                BajaPersonalDTO objDatosPersona = new BajaPersonalDTO();
                //                objDatosPersona = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.CplanRh, odbc).FirstOrDefault();
                //                if (objDatosPersona == null)
                //                    objDatosPersona = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.ArrenRh, odbc).FirstOrDefault();

                var objDatosPersona = _context.Select<BajaPersonalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT
                                    CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
                                    CONVERT(VARCHAR(200), t3.cc) + ' - ' + CONVERT(VARCHAR(200), t3.descripcion) AS cc, 
                                    t2.descripcion AS nombrePuesto,  t1.fecha_antiguedad AS fechaIngreso, t1.puesto as idPuesto, t3.cc AS numCC, t3.descripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss,
                                    t1.clave_ciudad_nac AS idCiudad,
                                    t1.clave_estado_nac AS idEstado,
	                                t1.contratable AS altaContratable,
									t5.nombre_corto,
                                    t1.clave_departamento_nac_PERU as idDepartamento,
                                    t6.num_dni as dni,
                                    t6.cedula_cuidadania as cedula_ciudadania
                                FROM tblRH_EK_Empleados AS t1
                                    INNER JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
                                    INNER JOIN tblP_CC AS t3 ON t1.cc_contable = t3.cc
									INNER JOIN tblRH_EK_Registros_Patronales as t5 ON t1.id_regpat = t5.clave_reg_pat
                                    LEFT JOIN tblRH_EK_Empl_Grales as t6 ON t1.clave_empleado = t6.clave_empleado
                                WHERE t1.clave_empleado = @claveEmpleado AND t1.esActivo = 1",
                    parametros = new { claveEmpleado }
                }).FirstOrDefault();

                if (objDatosPersona == null)
                {
                    throw (new Exception("Clave de usuario no encontrada"));
                }

                #region SE OBTIENE CAPACITACIONES Y ACTOS
                var listaActos = _context.tblSAC_Acto.Where(x => x.activo && x.tipoActo == Core.Enum.Administracion.Seguridad.ActoCondicion.TipoActo.Inseguro && x.claveEmpleado == claveEmpleado).Select(x => x.id).ToList();
                var listaCapacitaciones = (
                    from cap in _context.tblS_CapacitacionControlAsistencia.Where(x => x.activo && x.asistentes.Select(y => y.claveEmpleado).Contains(claveEmpleado)).ToList()
                    join det in _context.tblS_CapacitacionControlAsistenciaDetalle.Where(x => x.claveEmpleado == claveEmpleado).ToList() on cap.id equals det.controlAsistenciaID
                    select new
                    {
                        id = det.id,
                        cursoID = cap.cursoID,
                        curso = cap.curso.nombre,
                        cc = cap.cc,
                        examenID = det.examenID,
                        calificacion = det.calificacion,
                        divison = det.division,
                        fecha = cap.fechaCapacitacion.ToShortDateString()
                    }
                ).ToList();

                objDatosPersona.cantidadCursos = listaCapacitaciones.GroupBy(x => x.cursoID).Select(x => x.Key).Count();
                objDatosPersona.cantidadActos = listaActos.Count();
                #endregion
                #endregion

                resultado.Add("objDatosPersona", objDatosPersona);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosPersonaReporte(int claveEmpleado, string nombre)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE CC, PUESTO Y FECHA DE INGRESO EN BASE AL CLAVE DEL EMPLEADO
                //                var odbc = new OdbcConsultaDTO()
                //                {
                //                    consulta = @"
                //                        SELECT
                //                            CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
                //                            CONVERT(VARCHAR(200), t3.cc) + ' - ' + CONVERT(VARCHAR(200), t3.descripcion) AS cc, 
                //                            t2.descripcion AS nombrePuesto,  t1.fecha_antiguedad AS fechaIngreso, t1.puesto, t3.cc AS numCC, t3.descripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss,
                //                            t1.clave_ciudad_nac AS idCiudad,
                //                            t1.clave_estado_nac AS idEstado
                //                        FROM sn_empleados AS t1
                //                            INNER JOIN si_puestos AS t2 ON t1.puesto = t2.puesto
                //                            INNER JOIN cc AS t3 ON t1.cc_contable = t3.cc
                //                        WHERE t1.clave_empleado = ?",
                //                    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
                //                }; //OMAR
                //                BajaPersonalDTO objDatosPersona = new BajaPersonalDTO();
                //                objDatosPersona = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.CplanRh, odbc).FirstOrDefault();
                //                if (objDatosPersona == null)
                //                    objDatosPersona = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.ArrenRh, odbc).FirstOrDefault();

                var objDatosPersona = _context.Select<BajaPersonalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT
                                    CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, 
                                    CONVERT(VARCHAR(200), t3.cc) + ' - ' + CONVERT(VARCHAR(200), t3.descripcion) AS cc, 
                                    t2.descripcion AS nombrePuesto,  t1.fecha_antiguedad AS fechaIngreso, t1.puesto as idPuesto, t3.cc AS numCC, t3.descripcion AS descripcionCC,t1.curp,t1.rfc,t1.nss,
                                    t1.clave_ciudad_nac AS idCiudad,
                                    t1.clave_estado_nac AS idEstado,
	                                t1.contratable AS altaContratable,
									t5.nombre_corto,
                                    t1.fecha_antiguedad AS fechaAntiguedad
                                FROM tblRH_EK_Empleados AS t1
                                    LEFT JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
                                    LEFT JOIN tblP_CC AS t3 ON t1.cc_contable = t3.cc
									LEFT JOIN tblRH_EK_Registros_Patronales as t5 ON t1.id_regpat = t5.clave_reg_pat
                                WHERE t1.clave_empleado = @claveEmpleado AND t1.esActivo = 1",
                    parametros = new { claveEmpleado }
                }).FirstOrDefault();

                if (objDatosPersona == null)
                {
                    throw (new Exception("Clave de usuario no encontrada"));
                }

                #region SE OBTIENE CAPACITACIONES Y ACTOS
                var listaActos = _context.tblSAC_Acto.Where(x => x.activo && x.tipoActo == Core.Enum.Administracion.Seguridad.ActoCondicion.TipoActo.Inseguro && x.claveEmpleado == claveEmpleado).Select(x => x.id).ToList();
                var listaCapacitaciones = (
                    from cap in _context.tblS_CapacitacionControlAsistencia.Where(x => x.activo && x.asistentes.Select(y => y.claveEmpleado).Contains(claveEmpleado)).ToList()
                    join det in _context.tblS_CapacitacionControlAsistenciaDetalle.Where(x => x.claveEmpleado == claveEmpleado).ToList() on cap.id equals det.controlAsistenciaID
                    select new
                    {
                        id = det.id,
                        cursoID = cap.cursoID,
                        curso = cap.curso.nombre,
                        cc = cap.cc,
                        examenID = det.examenID,
                        calificacion = det.calificacion,
                        divison = det.division,
                        fecha = cap.fechaCapacitacion.ToShortDateString()
                    }
                ).ToList();

                objDatosPersona.cantidadCursos = listaCapacitaciones.GroupBy(x => x.cursoID).Select(x => x.Key).Count();
                objDatosPersona.cantidadActos = listaActos.Count();
                #endregion
                #endregion

                resultado.Add("objDatosPersona", objDatosPersona);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPreguntas(int idPregunta)
        {
            try
            {
                #region FILL CBO PREGUNTA #1 ENTREVISTA
                List<ComboDTO> lstConceptos = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, concepto AS Text FROM tblRH_Baja_Entrevista_Conceptos WHERE preguntaID = @preguntaID AND estatus = @estatus ORDER BY orden",
                    parametros = new { preguntaID = idPregunta, estatus = true }
                });
                resultado.Add(ITEMS, lstConceptos);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboPreguntas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado)
        {
            try
            {
                var listaActos = _context.tblSAC_Acto
                    .Where(x => x.activo && x.tipoActo == Core.Enum.Administracion.Seguridad.ActoCondicion.TipoActo.Inseguro && x.claveEmpleado == claveEmpleado)
                    .Select(s => new
                    {
                        fechaCreacion = s.fechaCreacion,
                        descripcion = s.descripcion
                    }).ToList();

                var listaCapacitaciones = (
                    from cap in _context.tblS_CapacitacionControlAsistencia.Where(x => x.activo && x.asistentes.Select(y => y.claveEmpleado).Contains(claveEmpleado)).ToList()
                    join det in _context.tblS_CapacitacionControlAsistenciaDetalle.Where(x => x.claveEmpleado == claveEmpleado && x.calificacion > 0).ToList() on cap.id equals det.controlAsistenciaID
                    select new
                    {
                        id = det.id,
                        cursoID = cap.cursoID,
                        curso = cap.curso.nombre,
                        cc = cap.cc,
                        examenID = det.examenID,
                        calificacion = det.calificacion,
                        divison = det.division,
                        fecha = cap.fechaCapacitacion.ToShortDateString()
                    }
                ).ToList();

                var data = new
                {
                    actos = listaActos,
                    cantidadActos = listaActos.Count,
                    capacitaciones = listaCapacitaciones,
                    cantidadCursos = listaCapacitaciones.GroupBy(x => x.cursoID).Select(x => x.Key).Count()
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEmpleadoCursosActos", e, AccionEnum.CONSULTA, 0, claveEmpleado);
            }
            return resultado;
        }

        public Dictionary<string, object> GetHistorialCC(int cvEmpleado)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                //List<HistorialCCDTO> historialLst = _context.tblRH_FormatoCambio.Where(e => e.Clave_Empleado == cvEmpleado).Select(e => new HistorialCCDTO
                //{
                //    nombreCC = e.CC,
                //    fechaAlta = e.Fecha_Alta

                //}).Distinct().ToList();

                List<tblRH_FormatoCambio> historialLst = _context.tblRH_FormatoCambio.Where(e => e.Clave_Empleado == cvEmpleado && e.Aprobado).ToList();
                List<HistorialCCDTO> dataLst = new List<HistorialCCDTO>();
                foreach (var i in historialLst)
                {
                    dataLst.Add(new HistorialCCDTO
                    {
                        cc_origen = i.CCAntID + " - " + i.CCAnt,
                        cc_nuevo = i.CcID + " - " + i.CC,
                        puesto_origen = i.PuestoAnt,
                        puesto_nuevo = i.Puesto,
                        jefe_origen = i.Nombre_Jefe_InmediatoAnt,
                        jefe_nuevo = i.Nombre_Jefe_Inmediato,
                        patron_origen = i.RegistroPatronalAnt,
                        patron_nuevo = i.RegistroPatronal,
                        fechaInicio = i.FechaInicioCambio.ToShortDateString()
                    });
                }


                result.Add(ITEMS, dataLst.Distinct());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> GetDetalleAut(int id, int tipo)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {

                var data = _context.tblRH_Baja_Registro.FirstOrDefault(x => x.id == id);
                //if (tipo == 1)
                //{
                //    result.Add("fecha", ((DateTime)data.est_baja_fecha).ToShortDateString());
                //    result.Add("usuario", "ADMINISTRADOR");
                //    result.Add("comentario", "PRUEBA DE BAJA");
                //}
                //else if (tipo == 1)
                //{
                //    result.Add("fecha", ((DateTime)data.est_inventario_fecha).ToShortDateString());
                //    result.Add("usuario", "ADMINISTRADOR");
                //    result.Add("comentario", "PRUEBA DE BAJA");
                //}
                //else if (tipo == 1)
                //{
                //    result.Add("fecha", ((DateTime)data.est_contabilidad_fecha).ToShortDateString());
                //    result.Add("usuario", "ADMINISTRADOR");
                //    result.Add("comentario", "PRUEBA DE BAJA");
                //}
                //else if (tipo == 1)
                //{
                //    result.Add("fecha", ((DateTime)data.est_compras_fecha).ToShortDateString());
                //    result.Add("usuario", "ADMINISTRADOR");
                //    result.Add("comentario", "PRUEBA DE BAJA");
                //}


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> GetFacultamientosAutorizante(string cc)
        {
            resultado.Clear();

            try
            {
                var usuarios = new List<ComboDTO>();

                var plantillasRequisiciones = new List<int> { 111, 112 };
                tblFA_Paquete paquete = null;

                if ((int)EmpresaEnum.Colombia == vSesiones.sesionEmpresaActual || (int)EmpresaEnum.Peru == vSesiones.sesionEmpresaActual)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                        paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                    }
                }
                else
                {
                    paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                }

                if (paquete != null)
                {
                    foreach (var facultamiento in paquete.facultamientos.Where(x => plantillasRequisiciones.Contains(x.plantillaID) && x.aplica))
                    {
                        foreach (var item in facultamiento.empleados.Where(x => x.esActivo && x.aplica))
                        {
                            var usuario = new ComboDTO();
                            usuario.Value = item.claveEmpleado.ToString();
                            usuario.Text = item.nombreEmpleado;
                            usuarios.Add(usuario);
                        }
                    }
                }

                var usuariosSinRepetir = new List<ComboDTO>();
                foreach (var item in usuarios.GroupBy(x => x.Value))
                {
                    usuariosSinRepetir.Add(item.First());
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, usuariosSinRepetir);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        public tblRH_EK_Empleados GetFacultamientosResponsableCC(string cc)
        {
            tblRH_EK_Empleados objResponsable = null;

            try
            {
                var usuarios = new List<int?>();

                var plantillasRequisiciones = new List<int> { 123 };
                tblFA_Paquete paquete = null;

                if ((int)EmpresaEnum.Colombia == vSesiones.sesionEmpresaActual || (int)EmpresaEnum.Peru == vSesiones.sesionEmpresaActual)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                        paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                    }
                }
                else
                {
                    paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                }

                if (paquete != null)
                {
                    foreach (var facultamiento in paquete.facultamientos.Where(x => plantillasRequisiciones.Contains(x.plantillaID) && x.aplica))
                    {
                        foreach (var item in facultamiento.empleados.Where(x => x.esActivo && x.aplica).ToList())
                        {
                            //var usuario = new ComboDTO();
                            //usuario.Value = item.claveEmpleado.ToString();
                            //usuario.Text = item.nombreEmpleado;
                            usuarios.Add(item.claveEmpleado);
                        }
                    }
                }

                if (usuarios.Count() > 0 )
                {
                    int claveResponsable = usuarios[0].Value;
                    objResponsable = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == claveResponsable);
                }

                //var usuariosSinRepetir = new List<ComboDTO>();
                //foreach (var item in usuarios.GroupBy(x => x.Value))
                //{
                //    usuariosSinRepetir.Add(item.First());
                //}
            }
            catch (Exception)
            {
                
                throw;
            }

            return objResponsable;
        }
        #endregion

        #region FILL COMBOS
        public Dictionary<string, object> GetCCs()
        {
            resultado.Clear();

            try
            {
                var ccs = new List<ComboDTO>();

                //ccs = _ccFS.GetCCs().Select(x => new ComboDTO
                //{
                //    Value = x.cc,
                //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //}).OrderBy(x => x.Value).ToList();
                ccs = _ccFS_SP.GetCCsNominaInactivos().Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                }).OrderBy(x => x.Value).ToList();

                //if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                //{
                //    //ccs = _ccFS.GetCCs().Select(x => new ComboDTO
                //    //{
                //    //    Value = x.cc,
                //    //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //    //}).OrderBy(x => x.Value).ToList();

                //    ccs = _ccFS_SP.GetCCs().Select(x => new ComboDTO
                //    {
                //        Value = x.cc,
                //        Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //    }).OrderBy(x => x.Value).ToList();
                //}
                //if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO)
                //{
                //    var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                //    if (usuarioCCs.Count > 0)
                //    {
                //        if (usuarioCCs.Any(x => x == "*"))
                //        {
                //            //ccs = _ccFS.GetCCs().Select(x => new ComboDTO
                //            //{
                //            //    Value = x.cc,
                //            //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //            //}).OrderBy(x => x.Value).ToList();

                //            ccs = _ccFS_SP.GetCCs().Select(x => new ComboDTO
                //            {
                //                Value = x.cc,
                //                Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //            }).OrderBy(x => x.Value).ToList();
                //        }
                //        else
                //        {
                //            //ccs = _ccFS.GetCCs(usuarioCCs).Select(x => new ComboDTO
                //            //{
                //            //    Value = x.cc,
                //            //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //            //}).OrderBy(x => x.Value).ToList();

                //            ccs = _ccFS_SP.GetCCs(usuarioCCs).Select(x => new ComboDTO
                //            {
                //                Value = x.cc,
                //                Text = "[" + x.cc + "] " + x.descripcion.Trim()
                //            }).OrderBy(x => x.Value).ToList();
                //        }
                //    }
                //}

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, ccs);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }
        public Dictionary<string, object> FillCboEstados()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                #region SE OBTIENE LISTADO DE ESTADOS
                List<ComboDTO> lstEstados = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idEstado AS Value, Estado AS Text FROM tblP_Estados ORDER BY Text"
                });

                resultado.Add(ITEMS, lstEstados);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstados", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboMunicipios(int idEstado)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                #region SE OBTIENE LISTADO DE MUNICIPIOS
                List<ComboDTO> lstMunicipios = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idMunicipio AS Value, Municipio AS Text FROM tblP_Municipios WHERE idEstado = @idEstado ORDER BY Text",
                    parametros = new { idEstado = idEstado }
                });

                resultado.Add(ITEMS, lstMunicipios);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillMunicipios", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboEstadosEK()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                //var listaEstados = _contextEnkontrol.Select<ComboDTO>(vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_estado AS Value, descripcion AS Text FROM sn_estados WHERE clave_pais = 1 ORDER BY clave_estado"
                //}).ToList();
                int clave_pais = 1;

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    clave_pais = 7;
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    clave_pais = 8;
                }

                var listaEstados = _context.tblRH_EK_Estados.Where(e => e.clave_pais == clave_pais).Select(e => new ComboDTO
                {
                    Value = e.clave_estado.ToString(),
                    Text = e.descripcion
                }).ToList();

                resultado.Add(ITEMS, listaEstados);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstadosEK", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboMunicipiosEK(int idEstado)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                //var listaMunicipios = _contextEnkontrol.Select<ComboDTO>(vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = string.Format(@"SELECT clave_ciudad AS Value, descripcion AS Text FROM sn_ciudades WHERE clave_pais = 1 AND clave_estado = {0} ORDER BY clave_ciudad", idEstado)
                //}).ToList();

                int clave_pais = 1;

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    clave_pais = 7;
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    clave_pais = 8;
                }

                var listaMunicipios = _context.tblRH_EK_Cuidades.Where(e => e.clave_pais == clave_pais && e.clave_estado == idEstado).OrderBy(e => e.clave_cuidad).Select(e => new ComboDTO
                {
                    Value = e.clave_cuidad.ToString(),
                    Text = e.descripcion
                }).ToList();

                resultado.Add(ITEMS, listaMunicipios);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboMunicipiosEK", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboEstadosCiviles()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region ES OBTIENE LISTADO DE ESTADO CIVIL
                List<ComboDTO> lstEstadoCivil = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, estadoCivil AS Text FROM tblP_EstadoCivil ORDER BY Text"
                });
                resultado.Add(ITEMS, lstEstadoCivil);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstadosCiviles", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEscolaridades()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region FILL CBO
                    List<ComboDTO> lstEscolaridades = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id AS Value, escolaridad AS Text FROM tblP_CatEscolaridades WHERE registroActivo = @registroActivo ORDER BY orden",
                        parametros = new { registroActivo = true }
                    });
                    resultado.Add(ITEMS, lstEscolaridades);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEscolaridades", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCCByBajas()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var lstCCBajas = _context.tblRH_Baja_Registro.Where(e => e.registroActivo).Select(e => new { Value = e.cc, Text = e.descripcionCC }).Distinct().ToList();

                resultado.Add(ITEMS, lstCCBajas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                //dbContextTransaction.Rollback();
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboCCByBajas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region FUNCIONES GENERALES
        int calcularEdad(DateTime fechaNacimiento)
        {
            // Calculate the age.
            var age = DateTime.Today.Year - fechaNacimiento.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public Dictionary<string, object> EnviarCorreo(string email, string link, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objBaja = _context.tblRH_Baja_Registro.Where(e => e.id == id).FirstOrDefault();

                if (objBaja == null)
                    throw new Exception("Algo salio mal");
                List<string> correos = new List<string>();
                correos.Add(email);

                var correo = new Infrastructure.DTO.CorreoDTO();

                correo.asunto = "ENTREVISTA DE SALIDA CONSTRUPLAN";

                correo.correos.AddRange(correos);
                correo.cuerpo = "Num. Empleado: " + objBaja.numeroEmpleado + "\nNombre: " + objBaja.nombre + "\nFecha de Baja: " + (objBaja.fechaBaja == null ? objBaja.fechaBaja.Value.ToString("dd/MM/yyyy") : "n/a") + "\nLink: " + link + ".";
                correo.Enviar();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, true);
            }
            return result;
        }

        public Dictionary<string, object> GuardarArchivoFiniquito(int idBaja, HttpPostedFileBase archivo, int tipoFiniquito, decimal monto)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {


                    var rutaBase = "";

#if DEBUG
                    rutaBase = RutaFiniquitosLocal;
#else
                    rutaBase = RutaFiniquitos;
#endif

                    var rutaArchivo = Path.Combine(rutaBase, DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + " " + archivo.FileName);

                    GlobalUtils.SaveHTTPPostedFileValidacion(archivo, rutaArchivo);

                    tblRH_Baja_Finiquitos objExists = _context.tblRH_Baja_Finiquitos.FirstOrDefault(e => e.registroActivo && e.idBaja == idBaja && e.tipoFiniquito == tipoFiniquito);
                    tblRH_Baja_Finiquitos objFiniquito = new tblRH_Baja_Finiquitos();

                    if (objExists == null)
                    {
                        objFiniquito.idBaja = idBaja;
                        objFiniquito.monto = monto;
                        objFiniquito.rutaFiniquito = rutaArchivo;
                        objFiniquito.tipoFiniquito = tipoFiniquito;
                        objFiniquito.fechaCreacion = DateTime.Now;
                        objFiniquito.fechaModificacion = DateTime.Now;
                        objFiniquito.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objFiniquito.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objFiniquito.registroActivo = true;
                        _context.tblRH_Baja_Finiquitos.Add(objFiniquito);
                    }
                    else
                    {
                        objExists.monto = monto;
                        objExists.rutaFiniquito = rutaArchivo;
                        objExists.tipoFiniquito = tipoFiniquito;
                        objExists.fechaModificacion = DateTime.Now;
                        objExists.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    }

                    //objBaja.rutaFiniquito = rutaArchivo.ToString();

                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.ACTUALIZAR, idBaja, JsonUtils.convertNetObjectToJson(new { idBaja = idBaja }));
                    resultado.Add(SUCCESS, true);

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "BajasPersonalController", "GuardarArchivoFiniquito", e, AccionEnum.AGREGAR, 0, new { idBaja = idBaja });
                }
            }

            return resultado;
        }

        private List<ccDTO> ccPermitidos()
        {
            var ccs = new List<ccDTO>();

            if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR || vSesiones.sesionUsuarioDTO.esAuditor)
            {
                //ccs = _ccFS.GetCCs().OrderBy(x => x.cc).ToList();
                ccs = _ccFS_SP.GetCCs().OrderBy(x => x.cc).ToList();
            }
            else
            {
                var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                if (usuarioCCs.Count > 0)
                {
                    if (usuarioCCs.Any(x => x == "*"))
                    {
                        //ccs = _ccFS.GetCCs().OrderBy(x => x.cc).ToList();
                        ccs = _ccFS_SP.GetCCs().OrderBy(x => x.cc).ToList();
                    }
                    else
                    {
                        //ccs = _ccFS.GetCCs(usuarioCCs).OrderBy(x => x.cc).ToList();
                        ccs = _ccFS_SP.GetCCs(usuarioCCs).OrderBy(x => x.cc).ToList();
                    }
                }
            }

            return ccs;
        }

        public Dictionary<string, object> GetAutorizantes(string cc, int? clave_empleado, string nombre_empleado)
        {
            resultado.Clear();

            try
            {
                var usuarios = new List<ComboDTO>();

                var plantillasRequisiciones = new List<int> { 124 };
                tblFA_Paquete paquete = null;

                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                    case (int)EmpresaEnum.GCPLAN:
                        paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == cc).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        break;
                    case (int)EmpresaEnum.Colombia:
                        if (!string.IsNullOrEmpty(cc))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                    case (int)EmpresaEnum.Peru:
                        if (!string.IsNullOrEmpty(cc))
                        {
                            var ccPaquete = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == cc);
                            paquete = _context.tblFA_Paquete.Where(x => x.ccID == ccPaquete.id).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                        }
                        break;
                }

                if (paquete != null)
                {
                    foreach (var facultamiento in paquete.facultamientos.Where(x => plantillasRequisiciones.Contains(x.plantillaID) && x.aplica))
                    {
                        foreach (var item in facultamiento.empleados.Where(x => x.esActivo && x.aplica))
                        {
                            var usuario = new ComboDTO();
                            usuario.Value = item.claveEmpleado.ToString();
                            usuario.Text = item.nombreEmpleado;
                            usuarios.Add(usuario);
                        }
                    }
                }

                var usuariosSinRepetir = new List<ComboDTO>();
                foreach (var item in usuarios.GroupBy(x => x.Value))
                {
                    usuariosSinRepetir.Add(item.First());
                }

                if (clave_empleado != null)
                {
                    var objEmpleado = usuariosSinRepetir.FirstOrDefault(e => e.Value == clave_empleado.ToString());

                    if (objEmpleado == null)
                    {
                        usuariosSinRepetir.Add(new ComboDTO
                        {
                            Value = clave_empleado.ToString(),
                            Text = nombre_empleado ?? ""
                        });
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, usuariosSinRepetir);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }
        
        //METODO PARA MANDAR CORREO DE APLICACION DE BAJA (EL CORREO DE BAJAS ANTICIPADAS AUTORIZADAS SE MANDA AUTOMATICAMENTE POR SISTEMA)
        public Dictionary<string , object> NotificarBajas(List<int> lstClavesEmps)
        {
            resultado.Clear();

            string empresaDesc = "";
            var asunto = empresaDesc + "BAJAS al dia " + DateTime.Now.ToString("dd/MM/yyyy");
            var mensaje = "Los siguientes empleados han sido dados de baja :<br/><br/>";
            List<string> correos = new List<string>();
            List<string> lstStrClaveEmpleados = new List<string>();

            #region EMPRESA
            switch (vSesiones.sesionEmpresaActual)
            {
                case 1:
                    empresaDesc = "CONSTRUPLAN";
                    break;
                case 2:
                    empresaDesc = "ARRENDADORA";
                    break;
                case 3:
                    empresaDesc = "COLOMBIA";
                    break;
                case 6:
                    empresaDesc = "PERÚ";
                    break;

                default:
                    break;
            }
            #endregion

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //LIMPIAR CLAVES DE EMPLEADOS

                    lstClavesEmps = lstClavesEmps.Distinct().ToList();

                    var lstBajas = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.est_baja == "A" && e.esPendienteNoti.HasValue && e.esPendienteNoti.Value).ToList();
                    var lstCCs = _context.tblC_Nom_CatalogoCC.ToList();
                    var lstPuestos = _context.tblRH_EK_Puestos.ToList();
                    var lstRegPats = _context.tblRH_EK_Registros_Patronales.Where(e => e.esActivo).ToList();
                    var lstEmpleados = _context.tblRH_EK_Empleados.Where(e => e.esActivo && lstClavesEmps.Contains(e.clave_empleado)).ToList();

                    if (lstBajas.Count() > 0)
                    {
                        string personasLiberadasString = "";
                        List<string> lstCC = new List<string>();

                        #region Correo Notificación Autorización

                        string conceptoHead = "";
                        string conceptoBody = "";
                        string conceptoFooter = "";

                        //var asunto = @"Se ha dado de baja un empleado en el centro de costos [" + registroBajaSIGOPLAN.cc + "]";

                        foreach (var item in lstClavesEmps)
                        {
                            var objEmpleado = lstEmpleados.FirstOrDefault(e => e.clave_empleado == item);

                            #region CUERPO CORREO
                            var registroBajaSIGOPLAN = lstBajas.FirstOrDefault(e => e.numeroEmpleado == item);

                            lstStrClaveEmpleados.Add(registroBajaSIGOPLAN.numeroEmpleado.ToString());

                            var objCC = lstCCs.FirstOrDefault(e => e.cc == registroBajaSIGOPLAN.cc);
                            var objPuesto = lstPuestos.FirstOrDefault(e => e.puesto == registroBajaSIGOPLAN.idPuesto);
                            var objRegPat = lstRegPats.FirstOrDefault(e => e.clave_reg_pat == objEmpleado.id_regpat);

                            lstCC.Add(registroBajaSIGOPLAN.cc);

                            //ACTUALIZAR ESTATUS DE NOTIFICADA
                            registroBajaSIGOPLAN.esPendienteNoti = false;
                            registroBajaSIGOPLAN.usuarioNoti = vSesiones.sesionUsuarioDTO.id;
                            registroBajaSIGOPLAN.fechaNoti = DateTime.Now;
                            _context.SaveChanges();

                            SaveBitacora(16, (int)AccionEnum.ACTUALIZAR, registroBajaSIGOPLAN.id, JsonUtils.convertNetObjectToJson(registroBajaSIGOPLAN));

                            mensaje += string.Format("{0} – {1}, para el centro de costos: {2} – {3}, con el puesto de: {4}. {5}, Registo patronal: {6}<br/>", registroBajaSIGOPLAN.numeroEmpleado, registroBajaSIGOPLAN.nombre, registroBajaSIGOPLAN.cc, objCC.ccDescripcion, objPuesto.descripcion, conceptoFooter, objRegPat.nombre_corto);

                            #endregion
                        }

                        //List<int> listaUsuariosCorreos = _context.tblRH_REC_Notificantes_Altas.Where(w => (lstCC.Contains(w.cc) || w.cc == "*") && w.esAuth && w.esActivo && w.notificarBaja).Select(s => s.idUsuario).ToList();

                        //foreach (var usu in listaUsuariosCorreos)
                        //{
                        //    correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        //}

                        List<int> lstNotificantes = _context.tblRH_Notis_RelConceptoUsuario.
                            Where(e => lstCC.Contains(e.cc) && (e.idConcepto == (int)ConceptosNotificantesEnum.Bajas
                                || e.idConcepto == (int)ConceptosNotificantesEnum.CH)).
                            Select(e => e.idUsuario).ToList();

                        foreach (var usu in lstNotificantes)
                        {
                            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        }

                        List<string> lstCorreoGenerales = _context.tblRH_Notis_RelConceptoCorreo.
                            Where(e => (e.cc == "*" || lstCC.Contains(e.cc)) && (e.idConcepto == (int)ConceptosNotificantesEnum.Bajas
                                || e.idConcepto == (int)ConceptosNotificantesEnum.CH)).
                            Select(e => e.correo).ToList();

                        foreach (var correo in lstCorreoGenerales)
                        {
                            correos.Add(correo);
                        }

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan 
                            || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora
                            || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                        {
                            correos.AddRange(new List<string> 
                            { 
                                "auxnominas.hmo@taxandlegal.com.mx",
                                "aux.seguridadsocialhmo@taxandlegal.com.mx",
                                "auxoperacionfiscal.hmo@taxandlegal.com.mx",
                                "seguridadsocial.hmo@taxandlegal.com.mx",
                                "operacionfiscalhmo@taxandlegal.com.mx",
                                "seguridadsocialhmo.taxandlegal@gmail.com",
                                "nominas.hmo@taxandlegal.com.mx",
                                "despacho@construplan.com.mx",
                            });
                        }
                        
#if DEBUG
                        correos = new List<string> { "miguel.buzani@construplan.com.mx" };
#endif

                        //GlobalUtils.sendEmail(asunto, mensaje, correos);
                        #endregion

                    }

                    dbSigoplanTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "BajasPersonalController", "NotificarBajas", e, AccionEnum.ACTUALIZAR, 0, lstClavesEmps);

                    dbSigoplanTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            #region ADJUNTAR ARCHIVO DE LAYOUT

            try
            {
                var result = _context.Select<RepBajasDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                            SELECT
                                eb.cc AS cCSolo,
                                sip.puesto AS puestoID,
                                srp.nombre_corto AS regPatronal,
                                sip.descripcion AS puestosDes,
                                (eb.cc +' - '+ c.ccDescripcion) AS cC,
                                e.clave_empleado AS empleadoID,
                                (e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) AS empleado,
                                rz.desc_motivo_baja AS Concepto,
							    rz.clave_razon_baja as claveConcepto,
                                e.jefe_inmediato AS jefeInmediatoID,
                                (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) AS jefeInmediato,
                                eb.fechaBaja AS fechaBaja,
                                CONVERT( CHAR( 20 ), eb.fechaBaja, 103 ) AS fechaBajaStr,
                                e.contratable AS recontratable,
                                CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS FechaRec,
                                CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS fechaAltaStr,
                                e.nss AS nss,
                                eb.est_baja,
                                eb.est_inventario,
                                est_contabilidad,
                                est_compras,
                                eb.est_inventario_comentario,
                                eb.est_compras_comentario,
                                eb.est_contabilidad_comentario,
                                (
                                    SELECT TOP 1
                                        recontratacion.fecha_reingreso
                                    FROM
                                        tblRH_EK_Empl_Recontratacion AS recontratacion
                                    WHERE
                                        recontratacion.clave_empleado = e.clave_empleado AND
                                        ((recontratacion.esActivo IS NOT NULL AND recontratacion.esActivo = 1) OR (recontratacion.esActivo IS NULL)) AND
                                        recontratacion.fecha_reingreso < eb.fechaBaja
                                    ORDER BY
                                        recontratacion.fecha_reingreso DESC
                                ) AS fechaAlta,
                                eb.fechaIngreso AS fechaIngreso,
                                e.fecha_antiguedad AS fechaAntiguedad,
                                eb.comentarios
                            FROM tblRH_EK_Empleados AS e 
					            LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON e.id_regpat = srp.clave_reg_pat 
					            LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
					            LEFT JOIN tblRH_Baja_Registro AS eb ON e.clave_empleado=eb.numeroEmpleado 
					            LEFT JOIN tblRH_EK_Puestos AS sip ON sip.puesto = eb.idPuesto 
					            LEFT JOIN tblRH_EK_Razones_Baja AS rz ON eb.motivoBajaDeSistema = rz.clave_razon_baja 
					            LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = eb.cc
                            WHERE e.clave_empleado IN (" + String.Join(", ", lstClavesEmps) + ")",
                });

                result = result.OrderByDescending(e => e.fechaBaja).ToList();
                var resultGroupped = result.GroupBy(e => e.empleadoID).Select(e => new { e.Key, bajas = e }).ToList();
                List<string> lstClaveEmpleado = resultGroupped.Select(e => e.Key).ToList();
                List<RepBajasDTO> resultEmpleado = new List<RepBajasDTO>();

                foreach (var item in resultGroupped)
                {
                    resultEmpleado.Add(item.bajas.FirstOrDefault());
                }

                byte[] archive = getFileDownloadBajas(resultEmpleado, lstClaveEmpleado);

                var lstArchives = new List<adjuntoCorreoDTO>();

                lstArchives.Add(new adjuntoCorreoDTO
                {
                    archivo = archive,
                    nombreArchivo = "layout",
                    extArchivo = ".xlsx"
                });
                #endregion

                var successCorreo = GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos, lstArchives);

                if (!successCorreo)
                {
                    throw new Exception("Ocurrio algo mal al mandar el correo");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "BajasPersonalController", "NotificarBajasCORREO", e, AccionEnum.ACTUALIZAR, 0, lstClavesEmps);

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                throw;
            }
                                  

            return resultado;
        }
        #endregion

        #region DASHBOARD

        public Dictionary<string, object> getMesdeBaja(List<int> año, string idCC)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            List<string> meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Agosto", "Septiembre", "Octurbre", "Noviembre", "Diciembre" };

            try
            {

                var data = _context.tblRH_Baja_Registro.Where(e => año.Contains(e.fechaBaja.Value.Year) && (string.IsNullOrEmpty(idCC) ? true : e.cc == idCC) && e.registroActivo).GroupBy(e => e.fechaBaja.Value.Year + "-" + e.fechaBaja.Value.Month).Select(ee => new DashboardDTO { mes = ee.Key, cantidad = ee.Count() }).ToList();

                var dataResult = data;

                foreach (var item in dataResult)
                {
                    var info = item.mes.Split('-');

                    if (info[1].Length == 1)
                    {
                        item.mes = info[0] + "-0" + info[1];

                    }
                    else
                    {
                        item.mes = info[0] + "-" + info[1];
                    }

                    item.añoFront = info[0];
                    item.mesFront = info[1];
                }

                result.Add(ITEMS, dataResult.OrderBy(e => e.mes));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, true);
            }
            return result;
        }

        public Dictionary<string, object> getMotivoSeparacion(List<int> año, bool filterData, string idCC)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                //cbo_registro_motivoBajaSistema.fillCombo('/Administrativo/ReportesRH/FillComboConceptosBaja', { est: true }, false, "--Seleccione--");

                List<motivoBajasGraficaDTO> data = _context.tblRH_Baja_Registro.Where(e => año.Contains(e.fechaCreacion.Value.Year) && string.IsNullOrEmpty(idCC) ? true : e.cc == idCC && e.registroActivo).GroupBy(e => e.motivoBajaDeSistema).Select(ee => new motivoBajasGraficaDTO { concepto = ee.Key.ToString(), cantidad = ee.Count() }).ToList();
                //List<ComboDTO> enkData = ContextEnKontrolNomina.Where("SELECT clave_razon_baja as Value,desc_motivo_baja as Text FROM sn_razones_baja").ToObject<List<ComboDTO>>();

                List<ComboDTO> enkData = _context.tblRH_EK_Razones_Baja.Select(e => new ComboDTO
                {
                    Value = e.clave_razon_baja.ToString(),
                    Text = e.desc_motivo_baja
                }).ToList();

                var dataResult = data;

                foreach (var item in dataResult)
                {

                    var matches = Regex.Match(item.concepto, @".+\D.+");
                    if (matches.Length == 0)
                    {
                        if (item.concepto != "0")
                        {

                            item.concepto = enkData.Where(e => e.Value.ToString() == item.concepto).FirstOrDefault().Text;
                        }
                        else
                        {
                            item.concepto = "Sin asignar";
                        }
                    }
                    else
                    {
                        if (item.concepto == "--Seleccione--")
                        {
                            item.concepto = "Sin asignar";
                        }
                    }
                }

                if (filterData)
                {
                    foreach (var item in enkData)
                    {
                        var itemData = data.Where(e => e.concepto == item.Value.ToString()).FirstOrDefault();

                        if (itemData == null)
                        {
                            dataResult.Add(new motivoBajasGraficaDTO { concepto = item.Text, cantidad = 0 });
                        }

                    }
                }

                result.Add(ITEMS, dataResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return result;
        }

        #endregion

        #region AUTORIZACIÓN
        public Dictionary<string, object> FillCboCCByBajasPermiso()
        {
            resultado = new Dictionary<string, object>();

            try
            {

                var permisosUsuarioCC = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).ToList();
                var lstCCBajas = _context.tblRH_Baja_Registro.Where(e => e.registroActivo).Select(e => new { Value = e.cc, Text = e.cc + " - " + e.descripcionCC }).Distinct().ToList().Where(x => permisosUsuarioCC.Select(y => y.cc).Contains("*") ? true : permisosUsuarioCC.Select(y => y.cc).Contains(x.Value)).ToList();

                resultado.Add(ITEMS, lstCCBajas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                //dbContextTransaction.Rollback();
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboCCByBajas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GetBajasPersonalAutorizacion(List<string> listaCC)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                #region SE OBTIENE ESTADOS Y MUNICIPIOS
                List<dynamic> lstEstados = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idEstado, Estado FROM tblP_Estados"
                }).ToList();

                List<dynamic> lstMunicipios = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idMunicipio, Municipio FROM tblP_Municipios"
                }).ToList();
                #endregion

                #region SE OBTIENE CONCEPTOS
                List<tblRH_Baja_Entrevista_Conceptos> lstConceptos = _context.Select<tblRH_Baja_Entrevista_Conceptos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id, preguntaID, concepto, orden, estatus FROM tblRH_Baja_Entrevista_Conceptos WHERE estatus = estatus",
                    parametros = new { estatus = true }
                }).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE EMPLEADOS DE EK
                //                var odbc = new OdbcConsultaDTO()
                //                {
                //                    consulta = @"SELECT CONVERT(VARCHAR(200), t1.ape_paterno) + ' ' + CONVERT(VARCHAR(200), t1.ape_materno) + ' ' + CONVERT(VARCHAR(200), t1.nombre) AS nombreCompleto, t1.clave_empleado AS gerente_clave
                //                                        FROM sn_empleados AS t1"
                //                };
                //                List<BajaPersonalDTO> lstEmpleadosEK = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.CplanRh, odbc).ToList();
                //                if (lstEmpleadosEK == null)
                //                    lstEmpleadosEK = _contextEnkontrol.Select<BajaPersonalDTO>(EnkontrolEnum.ArrenRh, odbc).ToList();

                var lstEmpleadosEK = _context.tblRH_EK_Empleados.Select(e => new BajaPersonalDTO
                {
                    nombreCompleto = e.ape_paterno + " " + e.ape_materno + " " + e.nombre,
                    gerente_clave = e.clave_empleado
                }).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE BAJAS
                var filtroCCString = "";

                if (listaCC != null && listaCC.Count() > 0)
                {
                    filtroCCString = "AND t1.cc IN (" + string.Join(", ", listaCC.Select(x => "'" + x + "'").ToList()) + ")";
                }
                else
                {
                    var permisosUsuarioCC = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).ToList();

                    if (permisosUsuarioCC.Count() == 0)
                    {
                        throw new Exception("No tiene permisos de centros de costo.");
                    }

                    filtroCCString = "AND t1.cc IN (" + string.Join(", ", permisosUsuarioCC.Select(x => "'" + x.cc + "'").ToList()) + ")";
                }

                #region V1
                //                List<BajaPersonalDTO> lstBajas = _context.Select<BajaPersonalDTO>(new DapperDTO
                //                {
                //                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                //                    consulta = string.Format(@"
                //                            SELECT
                //                                t1.id, t1.numeroEmpleado, t1.nombre, t1.cc, t1.descripcionCC, t1.idPuesto, t1.nombrePuesto, t1.habilidadesConEquipo, t1.telPersonal, t1.tieneWha, 
                //                                t1.telCasa, t1.contactoFamilia, t1.idEstado, t1.idCiudad, t1.idMunicipio, t1.direccion, t1.facebook, t1.instagram, t1.correo, t1.fechaBaja, 
                //                                t1.motivoBajaDeSistema, t1.motivoSeparacionDeEmpresa, t1.regresariaALaEmpresa, t1.porqueRegresariaALaEmpresa, t1.dispuestoCambioDeProyecto, 
                //                                t1.experienciaEnCP, t1.esContratable, t1.prioridad, t1.idUsuarioCreacion,(u.nombre) as usuarioCreacion_Nombre, t1.idUsuarioModificacion, t1.fechaCreacion,
                //                                t1.fechaModificacion, t1.registroActivo, t1.curp, t1.rfc, t1.nss, t1.rutaFiniquito, t1.comentariosAutorizacion,
                //		                        t2.cc, t2.cc_nombre, t2.gerente_clave, t2.fecha_ingreso, t2.fecha_salida, t2.fecha_nacimiento, t2.anios, t2.estado_civil_clave, t2.estado_civil_nombre, 
                //		                        t2.escolaridad_clave, t2.escolaridad_nombre, t2.p1_clave, t2.p1_concepto, t2.p2_clave, t2.p2_concepto, t2.p3_1_clave, t2.p3_1_concepto, t2.p3_2_clave, 
                //		                        t2.p3_2_concepto, t2.p3_3_clave, t2.p3_3_concepto, t2.p3_4_clave, t2.p3_4_concepto, t2.p3_5_clave, t2.p3_5_concepto, t2.p3_6_clave, t2.p3_6_concepto, 
                //		                        t2.p3_7_clave, t2.p3_7_concepto, t2.p3_8_clave, t2.p3_8_concepto, t2.p3_9_clave, t2.p3_9_concepto, t2.p3_10_clave, t2.p3_10_concepto, t2.p4_clave, 
                //		                        t2.p4_concepto, t2.p5_clave, t2.p5_concepto, t2.p6_concepto, t2.p7_concepto, t2.p8_clave, t2.p8_concepto, t2.p8_porque, t2.p9_clave, t2.p9_concepto, 
                //		                        t2.p9_porque, t2.p10_clave, t2.p10_concepto, t2.p10_porque, t2.p11_1_clave, t2.p11_1_concepto, t2.p11_2_clave, t2.p11_2_concepto, t2.p12_clave, t2.p12_concepto, 
                //		                        t2.p12_porque, t2.p13_clave, t2.p13_concepto, t2.p14_clave, t2.p14_concepto, t2.p14_fecha, t2.p14_porque, 
                //                                t1.est_baja, t1.est_baja_usuario, t1.est_baja_fecha, t1.est_baja_comentario, 
                //                                t1.est_inventario, t1.est_inventario_usuario, t1.est_inventario_fecha, t1.est_inventario_comentario, 
                //                                t1.est_contabilidad, t1.est_contabilidad_usuario, t1.est_contabilidad_fecha, t1.est_contabilidad_comentario, 
                //                                t1.est_compras, t1.est_compras_usuario, t1.est_compras_fecha, t1.est_compras_comentario
                //			                FROM tblRH_Baja_Registro AS t1
                //			                    LEFT JOIN tblRH_Baja_Entrevista AS t2 ON t1.id = t2.registroID
                //                                INNER join tblP_Usuario AS u on u.id = t1.idUsuarioCreacion
                //				            WHERE t1.registroActivo = 1 AND t1.est_baja != 'A' AND t1.est_inventario != 'A' AND t1.est_contabilidad != 'A' AND t1.est_compras != 'A' AND t1.autorizada = 0 {0}
                //					        ORDER BY t1.fechaCreacion", filtroCCString)
                //                }).ToList();
                #endregion

                var today = DateTime.Now.AddDays(-6);

                List<BajaPersonalDTO> lstBajasByUsr = new List<BajaPersonalDTO>();

                List<BajaPersonalDTO> lstBajas = _context.Select<BajaPersonalDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = string.Format(@"
                                            SELECT
                                                t1.*,
                		                        t2.cc, t2.cc_nombre, t2.gerente_clave, t2.fecha_ingreso, t2.fecha_salida, t2.fecha_nacimiento, t2.anios, t2.estado_civil_clave, t2.estado_civil_nombre, 
                		                        t2.escolaridad_clave, t2.escolaridad_nombre, t2.p1_clave, t2.p1_concepto, t2.p2_clave, t2.p2_concepto, t2.p3_1_clave, t2.p3_1_concepto, t2.p3_2_clave, 
                		                        t2.p3_2_concepto, t2.p3_3_clave, t2.p3_3_concepto, t2.p3_4_clave, t2.p3_4_concepto, t2.p3_5_clave, t2.p3_5_concepto, t2.p3_6_clave, t2.p3_6_concepto, 
                		                        t2.p3_7_clave, t2.p3_7_concepto, t2.p3_8_clave, t2.p3_8_concepto, t2.p3_9_clave, t2.p3_9_concepto, t2.p3_10_clave, t2.p3_10_concepto, t2.p4_clave, 
                		                        t2.p4_concepto, t2.p5_clave, t2.p5_concepto, t2.p6_concepto, t2.p7_concepto, t2.p8_clave, t2.p8_concepto, t2.p8_porque, t2.p9_clave, t2.p9_concepto, 
                		                        t2.p9_porque, t2.p10_clave, t2.p10_concepto, t2.p10_porque, t2.p11_1_clave, t2.p11_1_concepto, t2.p11_2_clave, t2.p11_2_concepto, t2.p12_clave, t2.p12_concepto, 
                		                        t2.p12_porque, t2.p13_clave, t2.p13_concepto, t2.p14_clave, t2.p14_concepto, t2.p14_fecha, t2.p14_porque, (u.apellidoPaterno + ' ' + u.apellidoMaterno + ' ' + u.nombre) as usuarioCreacion_Nombre,
                                                t1.dni, t1.idDepartamento, t1.cedula_ciudadania
                			                FROM tblRH_Baja_Registro AS t1
                			                    LEFT JOIN tblRH_Baja_Entrevista AS t2 ON t1.id = t2.registroID
                                                INNER join tblP_Usuario AS u on u.id = t1.idUsuarioCreacion
                				            WHERE t1.registroActivo = 1 AND t1.autorizada <> 2 AND t1.est_baja = 'P' AND (t1.fechaCreacion > @today OR t1.esAnticipada = 1) {0}
                					        ORDER BY t1.fechaCreacion", filtroCCString),
                    parametros = new { today }
                }).ToList();

                #region SE OBTIENE LISTADO DE MOTIVOS DE BAJA
                List<tblRH_EK_Razones_Baja> lstMotivosBaja = new List<tblRH_EK_Razones_Baja>();
                if (lstBajas.Count() > 0)
                {
                    //odbc = new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT clave_razon_baja, desc_motivo_baja FROM sn_razones_baja"
                    //};
                    //lstMotivosBaja = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc).ToList();
                    lstMotivosBaja = _context.tblRH_EK_Razones_Baja.ToList();

                }
                #endregion

                // SE OBTIENE LISTADO DE ESTADO CIVIL
                List<tblP_EstadoCivil> lstEstadoCivil = _context.tblP_EstadoCivil.Where(w => w.registroActivo).ToList();

                // SE OBTIENE LISTADO DE ESCOLARIDAD
                List<tblP_CatEscolaridades> lstEscolaridad = _context.tblP_CatEscolaridades.Where(w => w.registroActivo).ToList();

                // SE OBTIENE LISTADO DE FORMATO CAMBIO
                List<tblRH_FormatoCambio> lstFormatoCambio = _context.tblRH_FormatoCambio.Where(w => w.Aprobado).ToList();

                foreach (var item in lstBajas)
                {
                    #region SE OBTIENE CAPACITACIONES, ACTOS Y HISTORICO DE CC
                    var listaActos = _context.tblSAC_Acto.Where(x => x.activo && x.tipoActo == Core.Enum.Administracion.Seguridad.ActoCondicion.TipoActo.Inseguro && x.claveEmpleado == item.numeroEmpleado).Select(x => x.id).ToList();
                    //var listaCapacitaciones = (
                    //    from cap in _context.tblS_CapacitacionControlAsistencia.Where(x => x.activo && x.asistentes.Select(y => y.claveEmpleado).Contains(item.numeroEmpleado)).ToList()
                    //    join det in _context.tblS_CapacitacionControlAsistenciaDetalle.Where(x => x.claveEmpleado == item.numeroEmpleado).ToList() on cap.id equals det.controlAsistenciaID
                    //    select new
                    //    {
                    //        id = det.id > 0 ? det.id : 0,
                    //        cursoID = cap.cursoID > 0 ? cap.cursoID : 0,
                    //        curso = !string.IsNullOrEmpty(cap.curso.nombre) ? cap.curso.nombre : string.Empty,
                    //        cc = !string.IsNullOrEmpty(cap.cc) ? cap.cc : string.Empty,
                    //        examenID = det.examenID > 0 ? det.examenID : 0,
                    //        calificacion = det.calificacion > 0 ? det.calificacion : 0,
                    //        divison = det.division > 0 ? det.division : 0,
                    //        fecha = cap.fechaCapacitacion != null ? cap.fechaCapacitacion.ToShortDateString() : string.Empty
                    //    }
                    //).ToList();

                    string strQuery = string.Format(@"SELECT det.id AS id, cap.cursoID AS cursoID, t3.nombre AS curso, cap.cc AS cc, det.examenID AS examenID, det.calificacion AS calificacion, det.division AS division, cap.fechaCapacitacion AS fecha
	                                                        FROM tblS_CapacitacionControlAsistencia AS cap
	                                                        JOIN tblS_CapacitacionControlAsistenciaDetalle AS det ON cap.id = det.controlAsistenciaID
	                                                        JOIN tblS_CapacitacionCursos AS t3 ON cap.cursoID = t3.id
		                                                        WHERE cap.activo = 1 AND det.claveEmpleado IN (SELECT claveEmpleado FROM tblS_CapacitacionControlAsistencia) AND det.claveEmpleado = {0}", item.numeroEmpleado);
                    List<dynamic> listaCapacitaciones = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    List<tblRH_FormatoCambio> historialLst = lstFormatoCambio.Where(e => e.Clave_Empleado == item.numeroEmpleado && e.Aprobado).ToList();

                    item.cantidadCursos = listaCapacitaciones.GroupBy(x => x.cursoID).Select(x => x.Key).Count();
                    item.cantidadActos = listaActos.Count();
                    item.cantidadHistorico = historialLst.Count();
                    #endregion

                    #region SE OBTIENE MOTIVOS DE BAJA
                    int idMotivo = item.motivoBajaDeSistema != "--Seleccione--" ? Convert.ToInt32(item.motivoBajaDeSistema) : 0;
                    if (idMotivo > 0)
                        item.motivoBajaDeSistema = lstMotivosBaja.Where(w => w.clave_razon_baja == idMotivo).Select(s => s.desc_motivo_baja).FirstOrDefault();
                    else
                        item.motivoBajaDeSistema = string.Empty;
                    #endregion

                    #region SE OBTIENE ESTADO Y MUNICIPIO
                    int idEstado = item.idEstado;
                    if (idEstado > 0)
                        item.estado = lstEstados.Where(w => w.idEstado == item.idEstado).Select(s => s.Estado).FirstOrDefault();
                    else
                        item.estado = string.Empty;

                    int idCiudad = item.idCiudad;
                    if (idCiudad > 0)
                        item.ciudad = lstMunicipios.Where(w => w.idMunicipio == item.idCiudad).Select(s => s.Municipio).FirstOrDefault();
                    else
                        item.municipio = string.Empty;

                    int idMunicipio = item.idMunicipio;
                    if (idMunicipio > 0)
                        item.municipio = lstMunicipios.Where(w => w.idMunicipio == item.idMunicipio).Select(s => s.Municipio).FirstOrDefault();
                    else
                        item.municipio = string.Empty;
                    #endregion

                    #region SE OBTIENE EL NIVEL DE PRIORIDA EN STRING
                    if (item.prioridad == (int)PrioridadEnum.alta)
                        item.strPrioridad = EnumHelper.GetDescription(PrioridadEnum.alta);
                    else if (item.prioridad == (int)PrioridadEnum.media)
                        item.strPrioridad = EnumHelper.GetDescription(PrioridadEnum.media);
                    else if (item.prioridad == (int)PrioridadEnum.baja)
                        item.strPrioridad = EnumHelper.GetDescription(PrioridadEnum.baja);
                    else
                        item.strPrioridad = string.Empty;
                    #endregion

                    #region SE OBTIENE ESTADO CIVIL
                    int idEstadoCivil = item.estado_civil_clave;
                    if (idEstadoCivil > 0)
                        item.estado_civil_nombre = lstEstadoCivil.Where(w => w.id == idEstadoCivil).Select(s => s.estadoCivil).FirstOrDefault();
                    #endregion

                    #region SE OBTIENE ESCOLARIDAD
                    int idEscolaridad = item.escolaridad_clave;
                    if (idEstadoCivil > 0)
                        item.escolaridad_nombre = lstEscolaridad.Where(w => w.id == idEscolaridad).Select(s => s.escolaridad).FirstOrDefault();
                    #endregion

                    #region SE OBTIENE LOS CONCEPTOS EN BASE A LA CLAVE SELECCIONADA
                    item.p1_concepto = GetConcepto(lstConceptos, item.p1_clave);
                    item.p2_concepto = GetConcepto(lstConceptos, item.p2_clave);
                    item.p3_1_concepto = GetConcepto(lstConceptos, item.p3_1_clave);
                    item.p3_2_concepto = GetConcepto(lstConceptos, item.p3_2_clave);
                    item.p3_3_concepto = GetConcepto(lstConceptos, item.p3_3_clave);
                    item.p3_4_concepto = GetConcepto(lstConceptos, item.p3_4_clave);
                    item.p3_5_concepto = GetConcepto(lstConceptos, item.p3_5_clave);
                    item.p3_6_concepto = GetConcepto(lstConceptos, item.p3_6_clave);
                    item.p3_7_concepto = GetConcepto(lstConceptos, item.p3_7_clave);
                    item.p3_8_concepto = GetConcepto(lstConceptos, item.p3_8_clave);
                    item.p3_9_concepto = GetConcepto(lstConceptos, item.p3_9_clave);
                    item.p3_10_concepto = GetConcepto(lstConceptos, item.p3_10_clave);
                    item.p4_concepto = GetConcepto(lstConceptos, item.p4_clave);
                    item.p5_concepto = GetConcepto(lstConceptos, item.p5_clave);
                    item.p8_concepto = GetConcepto(lstConceptos, item.p8_clave);
                    item.p9_concepto = GetConcepto(lstConceptos, item.p9_clave);
                    item.p10_concepto = GetConcepto(lstConceptos, item.p10_clave);
                    item.p11_1_concepto = GetConcepto(lstConceptos, item.p11_1_clave);
                    item.p11_2_concepto = GetConcepto(lstConceptos, item.p11_2_clave);
                    item.p12_concepto = GetConcepto(lstConceptos, item.p12_clave);
                    item.p13_concepto = GetConcepto(lstConceptos, item.p13_clave);
                    item.p14_concepto = GetConcepto(lstConceptos, item.p14_clave);
                    #endregion

                    #region SE OBTIENE NOMBRE DEL GERENTE
                    if (string.IsNullOrEmpty(item.nombreGerente))
                        item.nombreGerente = lstEmpleadosEK.Where(w => w.gerente_clave == item.gerente_clave).Select(s => s.nombreCompleto).FirstOrDefault();
                    #endregion


                    if (vSesiones.sesionUsuarioDTO.idPerfil != 1)
                    {
                        if (item.clave_autoriza.HasValue && item.clave_autoriza.Value.ToString() == vSesiones.sesionUsuarioDTO.cveEmpleado)
                        {
                            lstBajasByUsr.Add(item);
                        }
                    }
                    else
                    {
                        lstBajasByUsr.Add(item);

                    }

                }
                #endregion

                resultado.Add("lstBajas", lstBajasByUsr);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetBajasPersonal", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion)
        {
            try
            {

                var registroBajaSIGOPLAN = _context.tblRH_Baja_Registro.FirstOrDefault(x => x.registroActivo && x.id == objAutorizacion.baja_id);

                string descRegPat = "";
                string cc = "";

                if (registroBajaSIGOPLAN != null)
                {
                    var objEmpleado = _context.tblRH_EK_Empleados.Where(e => e.clave_empleado == registroBajaSIGOPLAN.numeroEmpleado).FirstOrDefault();

                    descRegPat = "";
                    cc = objEmpleado.cc_contable;
                    if (objEmpleado.id_regpat != null)
                    {

                        var objRegPat = _context.tblRH_EK_Registros_Patronales.FirstOrDefault(e => e.clave_reg_pat == objEmpleado.id_regpat);
                        if (objRegPat != null)
                        {
                            descRegPat = objRegPat.nombre_corto;
                        }
                    }

                    using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            #region SIGOPLAN
                            registroBajaSIGOPLAN.autorizada = objAutorizacion.autorizada;
                            registroBajaSIGOPLAN.est_baja = objAutorizacion.autorizada == AutorizacionEnum.AUTORIZADA ? "A" : "C";
                            registroBajaSIGOPLAN.est_baja_comentario = objAutorizacion.comentariosAutorizacion;

                            if (objAutorizacion.autorizada == AutorizacionEnum.AUTORIZADA)
                            {
                                registroBajaSIGOPLAN.est_baja_firma = GlobalUtils.CrearFirmaDigital(registroBajaSIGOPLAN.id, DocumentosEnum.LiberacionContabilidad, vSesiones.sesionUsuarioDTO.id);
                                registroBajaSIGOPLAN.est_baja_fecha = DateTime.Now;
                                registroBajaSIGOPLAN.est_baja_usuario = vSesiones.sesionUsuarioDTO.id;
                            }

                            registroBajaSIGOPLAN.comentariosAutorizacion = objAutorizacion.comentariosAutorizacion;
                            _context.SaveChanges();
                            #endregion

                            //if (objAutorizacion.autorizada == AutorizacionEnum.PENDIENTE)
                            if (!registroBajaSIGOPLAN.esAnticipada)
                            {
                                registroBajaSIGOPLAN.esPendienteNoti = true;
                                _context.SaveChanges();

                                #region Actualizar estatus de empleado

                                if (objAutorizacion.autorizada == AutorizacionEnum.AUTORIZADA)
                                {

                                    if (objEmpleado != null)
                                    {
                                        objEmpleado.contratable = registroBajaSIGOPLAN.esContratable ? "S" : "N";
                                        objEmpleado.estatus_empleado = "B";
                                        _context.SaveChanges();
                                    }
                                }

                                #endregion

                                #region Eliminar Incidencias Futuras Empleado
                                var fechaBaja = registroBajaSIGOPLAN.fechaBaja;
                                var claveEmpleadoBaja = registroBajaSIGOPLAN.numeroEmpleado;
                                var registroPeriodoSemanal = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_inicial >= fechaBaja && x.tipo_nomina == 1 && x.estatus).OrderBy(x => x.fecha_inicial).FirstOrDefault();
                                var registroPeriodoQuincenal = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_inicial >= fechaBaja && x.tipo_nomina == 4 && x.estatus).OrderBy(x => x.fecha_inicial).FirstOrDefault();
                                
                                if (registroPeriodoSemanal != null) 
                                {
                                    var periodoSemanal = registroPeriodoSemanal.periodo;
                                    var anioPeriodoSemanal = registroPeriodoSemanal.anio;
                                    var incidenciasSemanales = _context.tblRH_BN_Incidencia.Where(x => ((x.anio == anioPeriodoSemanal && x.periodo >= periodoSemanal) || (x.anio > anioPeriodoSemanal)) && x.tipo_nomina == 1 && x.estatus != "A").Select(x => x.id).ToList();
                                    if (incidenciasSemanales.Count() > 0)
                                    {
                                        var incidenciasSemanalesDetalle = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasSemanales.Contains(x.incidenciaID) && x.clave_empleado == claveEmpleadoBaja).ToList();
                                        _context.tblRH_BN_Incidencia_det.RemoveRange(incidenciasSemanalesDetalle);
                                        _context.SaveChanges();
                                    }
                                }
                                if (registroPeriodoQuincenal != null)
                                {
                                    var periodoQuincenal = registroPeriodoQuincenal.periodo;
                                    var anioPeriodoQuincenal = registroPeriodoQuincenal.anio;
                                    var incidenciasQuincenales = _context.tblRH_BN_Incidencia.Where(x => ((x.anio == anioPeriodoQuincenal && x.periodo >= periodoQuincenal) || (x.anio > anioPeriodoQuincenal)) && x.tipo_nomina == 4 && x.estatus != "A").Select(x => x.id).ToList();
                                    if (incidenciasQuincenales.Count() > 0)
                                    {
                                        var incidenciasQuincenalesDetalle = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasQuincenales.Contains(x.incidenciaID) && x.clave_empleado == claveEmpleadoBaja).ToList();
                                        _context.tblRH_BN_Incidencia_det.RemoveRange(incidenciasQuincenalesDetalle);
                                        _context.SaveChanges();
                                    }
                                }

                                #endregion

                            }
                            else
                            {
                                registroBajaSIGOPLAN.esPendienteDarBaja = true;
                                _context.SaveChanges();
                            }

                            dbSigoplanTransaction.Commit();

                        }
                        catch (Exception e)
                        {
                            dbSigoplanTransaction.Rollback();

                            throw e;
                        }

                    }



                    //GlobalUtils.sendMailWithFilesReclutamientos(asunto, mensaje, new List<string> { "martin.zayas@construplan.com.mx" }, lstArchives);
                }

                //trans.Commit();
                SaveBitacora(16, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objAutorizacion));

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                //trans.Rollback();

                GlobalUtils.sendEmail(string.Format("{0}, ERROR BAJA EMPLEADOS", PersonalUtilities.GetNombreEmpresa()), e.Message + " " + e.StackTrace, new List<string> { "martin.zayas@construplan.com.mx" });

                LogError(16, 16, _NOMBRE_CONTROLADOR, "GuardarAutorizacionBajas", e, AccionEnum.ACTUALIZAR, 0, objAutorizacion);

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private OdbcConnection getEnkontrolConexion()
        {
            if (productivo)
            {
                return new Conexion().ConnectRH();
            }
            else
            {
                return new Conexion().ConnectPruebaRH();
            }
        }

        public byte[] getFileDownloadBajas(List<RepBajasDTO> lstIF, List<string> clavesEmpleados)
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);

            var lstConceptosBajas = _context.tblRH_EK_Razones_Baja.ToList();
            // string Cadena = "EMP_TRAB|EMP_NOM|EMP_NSS|EMP_BAJA_FECHA|CB_CLAVE|CB_DESCRIPCION";
            // string Cadena = "EMP_TRAB|EMP_ULTIMA_BAJA|EMP_ANTIGUEDAD|EMP_PUESTO|EMP_PUESTO_DESCRIPCION";

            string Cadena = "EMP_TRAB|EMP_NOM|EMP_DP|EMP_NSS|EMP_ALTA|EMP_ULTIMA_BAJA|EMP_PUESTO|EMP_PUESTO_DESCRIPCION|CB_CLAVE|CB_DESCRIPCION|EMPRESA";
            streamWriter.WriteLine(Cadena);
            //List<RepBajasDTO> listaIF = (List<RepBajasDTO>)Session["ListaLayoutBajasRHDTO"];

            //var EmpleadosSEleccionados = (List<string>)Session["setListEmpleados"];

            List<RepBajasDTO> listaIF = lstIF;

            var EmpleadosSEleccionados = clavesEmpleados;

            DataTable dtLayout = new DataTable();

            var EncabezadoTabla = Cadena.Split('|');

            for (int i = 0; i < EncabezadoTabla.Length; i++)
            {
                dtLayout.Columns.Add(EncabezadoTabla[i]);
            }

            foreach (var listaRow in listaIF.Where(x => EmpleadosSEleccionados.Contains(x.empleadoID)))
            {
                tblRH_LayautBajaEmpleados LayautBajaEmpleadosObj = new tblRH_LayautBajaEmpleados();

                LayautBajaEmpleadosObj.empleadoID = listaRow.empleadoID;
                LayautBajaEmpleadosObj.fechaCaptura = DateTime.Now;
                LayautBajaEmpleadosObj.usuarioCaptura = vSesiones.sesionUsuarioDTO.id;


                //reportesRHFactoryServices.getReportesRHService().setUsuariosBaja(LayautBajaEmpleadosObj);

                dtLayout.Rows.Add(

                    listaRow.empleadoID,
                    listaRow.empleado,
                    listaRow.cC,
                    listaRow.nss,
                    listaRow.fechaAltaStr,
                    listaRow.fechaBajaStr,
                    listaRow.puestoID,
                    listaRow.puestosDes,
                    (listaRow.claveConcepto),
                    listaRow.concepto,
                    listaRow.regPatronal
               );

            }
            return WriteExcelWithNPOIBajas(dtLayout);



            //return null;
        }

        private byte[] WriteExcelWithNPOIBajas(DataTable table)
        {

            try
            {
                MemoryStream file = new MemoryStream();

                using (var pack = new ExcelPackage())
                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Layout");
                    ws.Cells["A1"].LoadFromDataTable(table, true);
                    //ms.WriteTo(file);
                    return pack.GetAsByteArray();

                }
            }
            catch (Exception e)
            {

                return null;
            }


        }


        #endregion

        #region CRUD BAJAS (VER BAJA DE RECLUTAMIENTOS EN EL MODULOD DE BAJAS)
        public Dictionary<string, object> GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado)
        {
            try
            {
                string strQuery = string.Empty;
                Dictionary<string, object> dicListas = new Dictionary<string, object>();

                #region SE OBTIENE LA INFORMACIÓN DEL EMPLEADO, DE LA SECCIÓN: DATOS EMPLEADO | COMPAÑIA
                //                strQuery = @"SELECT t1.estatus_empleado, t1.clave_empleado, t1.nombre, t1.ape_paterno, t1.ape_materno, t1.fecha_nac, t1.clave_pais_nac, t1.clave_estado_nac, t1.clave_ciudad_nac, t1.localidad_nacimiento, 
                //                                    t1.fecha_alta, t1.sexo, t1.rfc, t1.curp, t1.requisicion, t1.id_regpat, t1.cc_contable, t1.puesto, t1.duracion_contrato, t1.jefe_inmediato, t1.autoriza, t1.usuario_compras, t1.sindicato, 
                //                                    t1.clave_depto, t1.nss, t1.unidad_medica, t1.tipo_formula_imss, t1.fecha_contrato, t2.descripcion_puesto, t2.descripcion, antiguedad = 0, nombreJefeInmediato = '', nombreAutoriza = '', nombreRegPat = '',
                //                                    t3.descripcion AS nombreCC, t4.nombre AS nombreTipoContrato, t5.desc_depto
                //                                        FROM sn_empleados AS t1
                //                                        INNER JOIN si_puestos AS t2 ON t1.puesto = t2.puesto
                //                                        INNER JOIN cc AS t3 ON t1.cc_contable = t3.cc
                //                                        INNER JOIN sn_empl_duracion_contrato AS t4 ON t1.duracion_contrato = t4.clave_duracion
                //                                        INNER JOIN sn_departamentos AS t5 ON t1.clave_depto = t5.clave_depto AND t1.cc_contable = t5.cc
                //                                            WHERE clave_empleado = {0}";
                //                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //                odbc.consulta = String.Format(strQuery, claveEmpleado);
                //                List<DatosActualizarEmpleadoDTO> lstDatos = Data.EntityFramework.Context._contextEnkontrol.Select<DatosActualizarEmpleadoDTO>(EnkontrolAmbienteEnum.Rh, odbc);

                var lstDatos = _context.Select<DatosActualizarEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.estatus_empleado, t1.clave_empleado, t1.nombre, t1.ape_paterno, t1.ape_materno, t1.fecha_nac, t1.clave_pais_nac, t1.clave_estado_nac, t1.clave_ciudad_nac, t1.localidad_nacimiento, 
		                                t1.fecha_alta, t1.sexo, t1.rfc, t1.curp, t1.requisicion, t1.id_regpat, t1.cc_contable, t1.puesto, t1.duracion_contrato, t1.jefe_inmediato, t1.autoriza, t1.usuario_compras, t1.sindicato, 
		                                t1.clave_depto, t1.nss, t1.unidad_medica, t1.tipo_formula_imss, t1.fecha_contrato, t2.descripcion_puesto, t2.descripcion, antiguedad = 0, nombreJefeInmediato = '', nombreAutoriza = '', nombreRegPat = '',
		                                t3.descripcion AS nombreCC, t4.nombre AS nombreTipoContrato, t5.desc_depto
                                FROM tblRH_EK_Empleados AS t1
	                                INNER JOIN tblRH_EK_Puestos AS t2 ON t1.puesto = t2.puesto
	                                INNER JOIN tblP_CC AS t3 ON t1.cc_contable = t3.cc
	                                INNER JOIN tblRH_EK_Empl_Duracion_Contrato AS t4 ON t1.duracion_contrato = t4.clave_duracion
	                                INNER JOIN tblRH_EK_Departamentos AS t5 ON t1.clave_depto = t5.clave_depto AND t1.cc_contable = t5.cc
                                WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                }).ToList();

                foreach (var item in lstDatos)
                {
                    #region SE OBTIENE LA CANTIDAD DE DÍAS DE ANTIGUEDAD DEL EMPLEADO
                    DateTime fechaAlta = Convert.ToDateTime(item.fecha_alta);
                    int antiguedad = (DateTime.Now - fechaAlta).Days;
                    item.antiguedad = antiguedad.ToString();
                    #endregion

                    #region SE OBTIENE EL NOMBRE DEL JEFE INMEDIATO Y DEL AUTORIZANTE
                    if (item.jefe_inmediato > 0)
                    {
                        //strQuery = string.Empty;
                        //strQuery = @"SELECT nombre, ape_paterno, ape_materno FROM sn_empleados WHERE clave_empleado = {0}";
                        //odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        //odbc.consulta = String.Format(strQuery, item.jefe_inmediato);
                        //List<dynamic> lstJefeInmediato = Data.EntityFramework.Context._contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Rh, odbc);

                        var lstJefeInmediato = _context.tblRH_EK_Empleados.Where(e => e.clave_empleado == item.jefe_inmediato).ToList();

                        if (lstJefeInmediato.Count() == 1)
                            item.nombreJefeInmediato = lstJefeInmediato[0].nombre + " " + lstJefeInmediato[0].ape_paterno + " " + lstJefeInmediato[0].ape_materno;
                    }

                    if (item.autoriza > 0)
                    {
                        //strQuery = string.Empty;
                        //strQuery = @"SELECT nombre, ape_paterno, ape_materno FROM sn_empleados WHERE clave_empleado = {0}";
                        //odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        //odbc.consulta = String.Format(strQuery, item.autoriza);
                        //List<dynamic> lstAutorizante = Data.EntityFramework.Context._contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Rh, odbc);

                        var lstAutorizante = _context.tblRH_EK_Empleados.Where(e => e.clave_empleado == item.autoriza).ToList();

                        if (lstAutorizante.Count() == 1)
                            item.nombreAutoriza = lstAutorizante[0].nombre + " " + lstAutorizante[0].ape_paterno + " " + lstAutorizante[0].ape_materno;
                    }
                    #endregion

                    #region SE OBTIENE EL NOMBRE DEL REG. PATRONAL
                    if (item.id_regpat > 0)
                    {
                        //strQuery = string.Empty;
                        //strQuery = @"SELECT nombre_corto FROM sn_registros_patronales WHERE clave_reg_pat = {0}";
                        //odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        //odbc.consulta = String.Format(strQuery, item.id_regpat);
                        //List<dynamic> lstNombreRegPat = Data.EntityFramework.Context._contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Rh, odbc);

                        var lstNombreRegPat = _context.tblRH_EK_Registros_Patronales.Where(e => e.clave_reg_pat == item.id_regpat).ToList();

                        if (lstNombreRegPat.Count() == 1)
                            item.nombreRegPat = lstNombreRegPat[0].nombre_corto;
                    }
                    #endregion

                    #region SE OBTIENE EL NOMBRE DEL USUARIO QUE REGISTRO AL EMPLEADO SELECCIONADO
                    if (item.usuario_compras > 0)
                    {
                        //NOMIGRADO
                        strQuery = string.Empty;
                        strQuery = @"SELECT descripcion FROM empleados WHERE empleado = {0}";
                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, item.usuario_compras);
                        List<dynamic> lstNombreUsuarioReg = Data.EntityFramework.Context._contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, odbc);

                        if (lstNombreUsuarioReg.Count() == 1)
                            item.nombreUsuarioReg = lstNombreUsuarioReg[0].descripcion;
                    }
                    #endregion
                }

                #region SE VERIFICA SI EL EMPLEADO A CONSULTAR, EXISTE EN SIGOPLAN CP/ARR
                int idUsuarioEK = 0;
                string usuarioModificacion = string.Empty;
                tblRH_REC_Empleados objEmpleado = _context.tblRH_REC_Empleados.Where(w => w.clave_empleado == claveEmpleado && w.esActivo).FirstOrDefault();
                if (objEmpleado != null)
                {
                    if (objEmpleado.idUsuarioModificacion == 0)
                    {
                        idUsuarioEK = _context.tblP_Usuario_Enkontrol.Where(w => w.idUsuario == objEmpleado.idUsuarioCreacion).Select(s => s.empleado).FirstOrDefault();
                        usuarioModificacion = _context.tblP_Usuario.Where(w => w.id == objEmpleado.idUsuarioCreacion).Select(s => s.apellidoPaterno + " " + s.apellidoMaterno + " " + s.nombre).FirstOrDefault();
                    }
                    else if (objEmpleado.idUsuarioModificacion > 0)
                    {
                        idUsuarioEK = _context.tblP_Usuario_Enkontrol.Where(w => w.idUsuario == objEmpleado.idUsuarioModificacion).Select(s => s.empleado).FirstOrDefault();
                        usuarioModificacion = _context.tblP_Usuario.Where(w => w.id == objEmpleado.idUsuarioModificacion).Select(s => s.apellidoPaterno + " " + s.apellidoMaterno + " " + s.nombre).FirstOrDefault();
                    }
                }
                else
                {
                    idUsuarioEK = lstDatos[0].usuario_compras;
                    usuarioModificacion = lstDatos[0].nombreUsuarioReg;
                }

                lstDatos[0].idUsuarioEK = idUsuarioEK;
                lstDatos[0].usuarioModificacion = usuarioModificacion;
                #endregion

                dicListas.Add("lstDatos", lstDatos);
                #endregion

                #region SE OBTIENE: GENERALES Y CONTACTO - BENEFICIARIO - EN CASO DE EMERGENCIA.
                //                strQuery = @"SELECT clave_empleado, estado_civil, fecha_planta, ocupacion, ocupacion_abrev, num_cred_elector, domicilio, numero_exterior, numero_interior, colonia, estado_dom, ciudad_dom, codigo_postal, 
                //                                    tel_casa, tel_cel, email, tipo_casa, tipo_sangre, alergias, parentesco_ben, fecha_nac_ben, codigo_postal_ben, paterno_ben, materno_ben, nombre_ben, estado_ben, ciudad_ben, colonia_ben, 
                //                                    domicilio_ben, num_ext_ben, num_int_ben, en_accidente_nombre, en_accidente_telefono, en_accidente_direccion
                //                                        FROM sn_empl_grales 
                //                                            WHERE clave_empleado = {0}";
                //                odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //                odbc.consulta = String.Format(strQuery, claveEmpleado);
                //                List<DatosActualizarEmpleadoDTO> lstGenerales = Data.EntityFramework.Context._contextEnkontrol.Select<DatosActualizarEmpleadoDTO>(EnkontrolAmbienteEnum.Rh, odbc);

                var lstGenerales = _context.Select<DatosActualizarEmpleadoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, estado_civil, fecha_planta, ocupacion, ocupacion_abrev, num_cred_elector, domicilio, numero_exterior, numero_interior, colonia, estado_dom, cuidado_dom as ciudad_dom, codigo_postal, 
                                    tel_casa, tel_cel, email, tipo_casa, tipo_sangre, alergias, parentesco_ben, fecha_nac_ben, codigo_postal_ben, parterno_ben as paterno_ben, materno_ben, nombre_ben, estado_ben, cuidad_ben as ciudad_ben, colonia_ben, 
                                    domicilio_ben, num_ext_ben, num_int_ben, en_accidente_nombre, en_accidente_telefono, en_accidente_direccion
                                        FROM tblRH_EK_Empl_Grales 
                                            WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                }).ToList();

                dicListas.Add("lstGenerales", lstGenerales);
                #endregion

                return dicListas;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetDatosActualizarEmpleado", e, AccionEnum.CONSULTA, claveEmpleado, 0);
                return null;
            }
        }

        public List<FamiliaresDTO> GetFamiliares(int clave_empleado)
        {
            try
            {
                string strQuery = string.Empty;
                #region SE OBTIENE LISTADO DEL TIPO PARENTESCO
                //List<dynamic> lstParentesco = new List<dynamic>();
                //strQuery = "SELECT id, descripcion FROM sn_parentesco";
                //var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //odbc.consulta = String.Format(strQuery);
                //lstParentesco = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Rh, odbc);

                var lstParentesco = _context.tblRH_EK_Parentesco.ToList();
                #endregion

                //                #region SE OBTIENE LOS FAMILAIRES
                //                strQuery = string.Empty;
                //                List<FamiliaresDTO> lstFamiliaresEK = new List<FamiliaresDTO>();
                //                strQuery = @"SELECT id_familia as idEKFam, clave_empleado, nombre, apellido_paterno, apellido_materno, fecha_de_nacimiento, parentesco, grado_de_estudios, estado_civil, estudia, genero, vive, beneficiario, trabaja, comentarios 
                //                                FROM sn_empl_familia 
                //                                    WHERE clave_empleado = '{0}'";
                //                odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //                odbc.consulta = String.Format(strQuery, clave_empleado);
                //                #endregion

                //                if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                //                    lstFamiliaresEK = _contextEnkontrol.Select<FamiliaresDTO>(EnkontrolAmbienteEnum.Rh, odbc);
                //                else if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                //                    lstFamiliaresEK = _contextEnkontrol.Select<FamiliaresDTO>(EnkontrolAmbienteEnum.RhArre, odbc);

                var lstFamiliaresEK = _context.Select<FamiliaresDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id_familia as idEKFam, clave_empleado, nombre, apellido_paterno, apellido_materno, fecha_de_nacimiento, parentesco, grado_de_estudios, estado_civil, estudia, genero, vive, beneficiario, trabaja
                                FROM tblRH_EK_Empl_Familia 
                                    WHERE clave_empleado = @clave_empleado",
                    parametros = new { clave_empleado }
                }).ToList();

                foreach (var item in lstFamiliaresEK)
                {
                    var objSigoFam = _context.tblRH_REC_EmplFamiliares.FirstOrDefault(e => e.idEKFam == item.idEKFam);

                    if (objSigoFam != null)
                    {
                        item.id = objSigoFam.id;
                    }
                    //if (item.fecha_de_nacimiento.Year < 1000)
                    //    item.fecha_de_nacimiento = new DateTime(2000, 01, 01);

                    int idParentesco = Convert.ToInt32(item.parentesco);
                    item.strParentesco = lstParentesco.Where(w => w.id == idParentesco).Select(s => s.descripcion).FirstOrDefault();
                }

                return lstFamiliaresEK;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetFamiliares", e, AccionEnum.CONSULTA, clave_empleado, 0);
                return null;
            }
        }

        public UniformesDTO GetUniformes(int claveEmpleado)
        {
            try
            {
                string strQuery = string.Empty;

                //var requisiciones = _contextEnkontrol.Select<GetRequisicionesDTO>(vSesiones.sesionAmbienteEnkontrolRh, query_requi);
                //                UniformesDTO objUniformes = new UniformesDTO();

                //                strQuery = string.Format(@"SELECT clave_empleado, calzado, camisa, pantalon, overol, fecha_entrega as fechaEntrega, entrego_calzado, entrego_camisa, entrego_pantalon, entrego_overol, comentarios, uniforme_dama, entrego_uniforme_dama, otros
                //                                            FROM sn_empl_complementaria 
                //                                            WHERE clave_empleado = '{0}'", claveEmpleado);

                //                var uniformes = _contextEnkontrol.Select<UniformesDTO>(vSesiones.sesionAmbienteEnkontrolRh, strQuery).FirstOrDefault();

                var uniformes = _context.Select<UniformesDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, calzado, camisa, pantalon, overol, fecha_entrega as fechaEntrega, entrego_calzado, entrego_camisa, entrego_pantalon, entregro_overol as entrego_overol, comentarios, uniforme_dama, entrego_uniforme_dama, otros
                                            FROM tblRH_EK_Empl_Complementaria 
                                            WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                }).FirstOrDefault();

                if (uniformes != null)
                {
                    if (uniformes.fechaEntrega.Year < 1000)
                        uniformes.fechaEntrega = new DateTime(2000, 01, 01);
                }


                //var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //odbc.consulta = String.Format(strQuery, claveEmpleado);

                //if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                //    objUniformes = _contextEnkontrol.Select<UniformesDTO>(EnkontrolAmbienteEnum.Rh, odbc).FirstOrDefault();
                //else if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                //    objUniformes = _contextEnkontrol.Select<UniformesDTO>(EnkontrolAmbienteEnum.RhArre, odbc).FirstOrDefault();

                #region LISTADO DE UNIFORMES DEL EMPLEADO SELECCIONADO
                var objUniformes = _context.tblRH_REC_Uniformes.Where(w => w.clave_empleado == claveEmpleado && w.esActivo).Select(s => new UniformesDTO
                {
                    id = s.id,
                    clave_empleado = s.clave_empleado,
                    fechaEntrega = s.fechaEntrega,
                    calzado = s.calzado,
                    camisa = s.camisa,
                    pantalon = s.pantalon,
                    overol = s.overol,
                    uniforme_dama = s.uniforme_dama,
                    otros = s.otros,
                    comentarios = s.comentarios,
                    entrego_calzado = s.entrego_calzado == true ? "S" : "N",
                    entrego_camisa = s.entrego_camisa == true ? "S" : "N",
                    entrego_pantalon = s.entrego_pantalon == true ? "S" : "N",
                    entrego_overol = s.entrego_overol == true ? "S" : "N",
                    entrego_uniforme_dama = s.entrego_uniforme_dama == true ? "S" : "N"
                }).FirstOrDefault();
                #endregion

                #region SE CREA BITACORA
                SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(claveEmpleado));
                #endregion

                //return objUniformes;
                return uniformes;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetUniformes", e, AccionEnum.CONSULTA, 0, claveEmpleado);
                return null;
            }
        }

        public List<ContratoDTO> GetContratos(int clave_empleado)
        {
            try
            {
                //var contratos = _contextEnkontrol.Select<ContratoDTO>((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolAmbienteEnum.RhCplan : EnkontrolAmbienteEnum.RhArre, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT * FROM sn_contratos_empleados WHERE clave_empleado = " + clave_empleado
                //});

                var contratos = _context.tblRH_EK_Contratos_Empleados.Where(e => e.clave_empleado == clave_empleado && e.esActivo.HasValue && e.esActivo.Value).Select(e => new ContratoDTO
                {
                    id_contrato_empleado = e.id_contrato_empleado,
                    clave_empleado = e.clave_empleado,
                    clave_duracion = e.clave_duracion,
                    fecha = e.fecha,
                }).ToList();

                foreach (var con in contratos)
                {
                    con.fechaString = con.fecha.ToShortDateString();
                    con.fecha_aplicacionString = con.fecha_aplicacion.HasValue ? con.fecha_aplicacion.Value.ToString("DD/MM/YYYY") : "";
                }

                return contratos;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetContratos", e, AccionEnum.CONSULTA, clave_empleado, 0);
                return null;
            }
        }

        public List<ArchivosDTO> GetArchivoExamenMedico(int claveEmpleado)
        {
            try
            {
                string strQuery = string.Empty;

                #region LISTADO DE EXAMEN MEDICO DEL EMPLEADO SELECCIONADO
                List<ArchivosDTO> lstArchivos = _context.tblRH_REC_Archivos.Where(w => w.claveEmpleado == claveEmpleado && w.tipoArchivo == (int)TipoArchivoEnum.examenMedico && w.esActivo).Select(s => new ArchivosDTO
                {
                    id = s.id,
                    nombreArchivo = s.nombreArchivo,
                    descripcion = s.descripcion
                }).ToList();
                #endregion

                #region SE CREA BITACORA
                SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(claveEmpleado));
                #endregion

                return lstArchivos;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetArchivoExamenMedico", e, AccionEnum.CONSULTA, 0, claveEmpleado);
                return null;
            }
        }

        public List<TabuladoresDTO> GetTabuladores(TabuladoresDTO objTabDTO)
        {
            try
            {
                //string strQuery = string.Empty;

                //#region SE OBTIENE LA NOMINA DEL EMPLEADO
                //strQuery = string.Empty;
                //strQuery = @"SELECT fecha_cambio, suma, salario_base, complemento, bono_zona FROM sn_tabulador_historial WHERE clave_empleado = '{0}' ORDER BY fecha_cambio DESC";
                //var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //odbc.consulta = String.Format(strQuery, objTabDTO.clave_empleado);
                //List<TabuladoresDTO> lstNomina = Data.EntityFramework.Context._contextEnkontrol.Select<TabuladoresDTO>(EnkontrolAmbienteEnum.Rh, odbc);

                var lstNomina = _context.Select<TabuladoresDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT fecha_cambio, suma, salario_base, complemento, bono_zona FROM tblRH_EK_Tabulador_Historial WHERE clave_empleado = @clave_empleado ORDER BY fecha_cambio DESC",
                    parametros = new { objTabDTO.clave_empleado }
                }).ToList();

                foreach (var item in lstNomina)
                {
                    item.fechaRealNomina = item.fecha_cambio.ToShortDateString();
                }
                //#endregion

                return lstNomina;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetTabuladores", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<ComboDTO> FillComboRegistroPatronal(string cc)
        {
            try
            {
                //var listaRegistrosPatronales = _contextEnkontrol.Select<ComboDTO>((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolAmbienteEnum.Rh : EnkontrolAmbienteEnum.RhArre, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_reg_pat AS Value, nombre_corto AS Text FROM sn_registros_patronales ORDER BY nombre_corto"
                //});

                var listaRegistrosPatronales = _context.tblRH_EK_Registros_Patronales.OrderBy(e => e.nombre_corto).Select(e => new ComboDTO
                {
                    Value = e.clave_reg_pat.ToString(),
                    Text = e.nombre_corto
                }).ToList();

                if (cc != "" && cc != null)
                {
                    var listaRelacionRegistrosPatronalesCC = _context.tblRH_REC_RelacionRegistroPatronalCC.Where(x => x.registroActivo && x.cc == cc).ToList();

                    listaRegistrosPatronales = listaRegistrosPatronales.Where(x => listaRelacionRegistrosPatronalesCC.Select(y => y.clave_reg_pat).Contains(Int32.Parse(x.Value))).ToList();
                }

                return listaRegistrosPatronales;
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "FillComboRegistroPatronal", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<ComboDTO> FillComboDuracionContrato()
        {
            try
            {
                //return _contextEnkontrol.Select<ComboDTO>((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolAmbienteEnum.Rh : EnkontrolAmbienteEnum.RhArre, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_duracion AS Value, nombre AS Text FROM sn_empl_duracion_contrato"
                //});

                return _context.tblRH_EK_Empl_Duracion_Contrato.Select(e => new ComboDTO
                {
                    Value = e.clave_duracion.ToString(),
                    Text = e.nombre
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "FillComboDuracionContrato", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public Dictionary<string, object> FillDepartamentos(string cc)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE DEPARTAMENTOS DE EK
                //string strQuery = string.Format("SELECT clave_depto AS VALUE, desc_depto AS TEXT FROM sn_departamentos WHERE TEXT NOT LIKE '%NO USAR%' {0} ORDER BY clave_depto", !string.IsNullOrEmpty(cc) ? "AND cc = '" + cc + "'" : string.Empty);
                //List<ComboDTO> lstDepartamentos = _contextEnkontrol.Select<ComboDTO>(vSesiones.sesionAmbienteEnkontrolRh, new OdbcConsultaDTO()
                //{
                //    consulta = strQuery
                //});

                var lstDepartamentos = _context.tblRH_EK_Departamentos.Where(e => !e.desc_depto.Contains("NO USAR") && (!string.IsNullOrEmpty(cc) ? e.cc == cc : true)).Select(e => new ComboDTO
                {
                    Value = e.clave_depto.ToString(),
                    Text = e.desc_depto,
                }).ToList();

                resultado.Add(ITEMS, lstDepartamentos);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillDepartamentos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDocs(int? clave_empleado, int? id_candidato)
        {
            resultado.Clear();

            try
            {
                var listaArchivos = _context.tblRH_REC_ED_Archivo.Where(x => x.registroActivo).ToList(); //Catalogo
                List<tblRH_REC_ED_RelacionExpedienteArchivo> listaRelacionExpedienteArchivo = new List<tblRH_REC_ED_RelacionExpedienteArchivo>();
                ExpedienteDigitalDTO objExpedientes = new ExpedienteDigitalDTO();

                #region GET ARCHIVOS DE EXPEDIENTE DIGITAL
                if (clave_empleado != null)
                {

                    objExpedientes = _context.tblRH_REC_ED_Expediente.Where(x => x.registroActivo && x.claveEmpleado == clave_empleado).Select(e => new ExpedienteDigitalDTO
                    {
                        id = e.id,
                        claveEmpleado = e.claveEmpleado,
                        //archivos = new List<ArchivoExpedienteDigitalDTO>()
                    }).FirstOrDefault();

                    if (objExpedientes != null)
                    {
                        objExpedientes.archivos = new List<ArchivoExpedienteDigitalDTO>();

                        listaRelacionExpedienteArchivo = _context.tblRH_REC_ED_RelacionExpedienteArchivo.Where(x => x.registroActivo && x.expediente_id == objExpedientes.id).ToList();


                        var archivosAplicables = listaRelacionExpedienteArchivo;
                        foreach (var arc in listaArchivos)
                        {
                            var archivoAplicable = archivosAplicables.FirstOrDefault(x => x.archivo_id == arc.id);

                            if (archivoAplicable != null)
                            {
                                objExpedientes.archivos.Add(new ArchivoExpedienteDigitalDTO
                                {
                                    expediente_id = objExpedientes.id,
                                    archivo_id = arc.id,
                                    archivoCargado_id = archivoAplicable.id,
                                    rutaArchivo = archivoAplicable.rutaArchivo,
                                    aplica = true,
                                    estado = true,
                                    archiveDesc = arc.descripcion,
                                });
                            }
                            else
                            {
                                objExpedientes.archivos.Add(new ArchivoExpedienteDigitalDTO
                                {
                                    expediente_id = objExpedientes.id,
                                    archivo_id = arc.id,
                                    archivoCargado_id = 0,
                                    rutaArchivo = null,
                                    aplica = false,
                                    estado = false,
                                    archiveDesc = arc.descripcion,

                                });
                            }
                        }
                    }
                }
                #endregion

                #region GET ARCHIVOS FASES

                tblRH_REC_GestionCandidatos objCandidato = new tblRH_REC_GestionCandidatos();
                List<tblRH_REC_Archivos> archivoSeg = new List<tblRH_REC_Archivos>() { };
                List<ArchivoExpedienteDigitalDTO> archivosSeguimiento = new List<ArchivoExpedienteDigitalDTO>();
                List<ArchivoExpedienteDigitalDTO> archivosSeguimientoFormatted = new List<ArchivoExpedienteDigitalDTO>();

                if (clave_empleado != null)
                {
                    // CANDIDATO DATO DE ALTA
                    objCandidato = _context.tblRH_REC_GestionCandidatos.Where(e => e.esActivo && e.clave_empleado == clave_empleado).FirstOrDefault();

                    if (objCandidato != null)
                    {
                        archivoSeg = _context.tblRH_REC_Archivos.Where(e => e.esActivo && e.idCandidato == objCandidato.id).ToList();

                    }

                }
                else
                {

                    // CANDIDATO NO DADO ALTA
                    if (id_candidato != null)
                    {
                        archivoSeg = _context.tblRH_REC_Archivos.Where(e => e.esActivo && e.idCandidato == id_candidato).ToList();
                    }

                }

                foreach (var item in archivoSeg)
                {
                    tblRH_REC_Actividades objActividad = _context.tblRH_REC_Actividades.Where(e => e.esActivo && e.id == item.idActividad).FirstOrDefault();

                    if (objActividad != null)
                    {

                        if (clave_empleado != null)
                        {
                            //AÑADIR ARCHIVOS DE SEGUIMIENTO DE CANDIDATO AL EMPLEADO
                            ArchivoExpedienteDigitalDTO archive = objExpedientes.archivos.Where(e => e.archivo_id == objActividad.tipoArchivo).FirstOrDefault();

                            //archive.archivoCargado_id = objActividad.tipoArchivo.Value;

                            if (archive.rutaArchivo == null)
                            {
                                var archivoRel = listaRelacionExpedienteArchivo.FirstOrDefault(x => x.archivo_id == archive.archivo_id);
                                if (archivoRel != null)
                                {
                                    archive.rutaArchivo = item.ubicacionArchivo;
                                    archivoRel.rutaArchivo = item.ubicacionArchivo;
                                    archive.estado = true;
                                }
                            }

                        }
                        else
                        {
                            //CREAR LISTA SOLO CON LOS ARCHIVOS DE SEGUIMINETO DEL CANDIDATO
                            if (objActividad.tipoArchivo != null)
                            {
                                archivosSeguimiento.Add(new ArchivoExpedienteDigitalDTO
                                {
                                    archivo_id = objActividad.tipoArchivo.Value,
                                    rutaArchivo = item.ubicacionArchivo,
                                    estado = true,
                                });
                            }

                        }
                    }
                    else
                    {
                        //AÑADIR CV Y FOTO DEL CANDIDATO (NO TIENEN ACTIVIDAD)
                        if (item.tipoArchivo == 5 || item.tipoArchivo == 30)
                        {
                            archivosSeguimiento.Add(new ArchivoExpedienteDigitalDTO
                            {
                                archivo_id = item.tipoArchivo,
                                rutaArchivo = item.ubicacionArchivo,
                                estado = true,
                            });
                        }
                    }
                }

                #endregion


                if (clave_empleado != null && objExpedientes != null)
                {
                    //REGRESAR LISTA DE LOS ARCHIVOS DEL EMPLEADO CON EXPEDIENTE
                    resultado.Add(ITEMS, objExpedientes.archivos);

                }
                else
                {
                    if (id_candidato != null && archivosSeguimiento.Count() > 0)
                    {
                        archivosSeguimientoFormatted.AddRange(archivosSeguimiento);

                        foreach (var item in listaArchivos)
                        {
                            var objArchivo = archivosSeguimiento.Where(e => e.archivo_id == item.id).FirstOrDefault();
                            if (objArchivo == null)
                            {
                                archivosSeguimientoFormatted.Add(new ArchivoExpedienteDigitalDTO
                                {
                                    archivo_id = item.id,
                                    archiveDesc = item.descripcion,
                                    estado = false,
                                });
                            }
                            else
                            {
                                objArchivo.archiveDesc = item.descripcion;
                            }
                        }

                        //REGRESAR LISTA DE LOS ARCHIVOS DEL CANDIDATO SIN EXPEDIENTE
                        resultado.Add(ITEMS, archivosSeguimientoFormatted);
                    }
                    else
                    {
                        foreach (var item in listaArchivos)
                        {

                            archivosSeguimientoFormatted.Add(new ArchivoExpedienteDigitalDTO
                            {
                                archivo_id = item.id,
                                archiveDesc = item.descripcion,
                                estado = false,
                            });

                        }


                        //REGRESAR LISTA DE LOS ARCHIVOS SIN RELACION
                        resultado.Add(ITEMS, archivosSeguimientoFormatted);
                    }
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region COMENTARIOS/CANCELACION DE BAJA

        public Dictionary<string, object> CancelarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion)
        {
            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                //using (var con = getEnkontrolConexion())

                //using (var trans = con.BeginTransaction())

                try
                {
                    #region V1

                    //foreach (var item in listaAutorizacion)
                    //{
                    //    var registroBajaSIGOPLAN = _context.tblRH_Baja_Registro.FirstOrDefault(x => x.registroActivo && x.id == item.baja_id);

                    //    if (registroBajaSIGOPLAN != null)
                    //    {
                    //        registroBajaSIGOPLAN.autorizada = item.autorizada;
                    //        _context.SaveChanges();

                    //        if (item.autorizada == AutorizacionEnum.AUTORIZADA)
                    //        {
                    //            #region Actualizar estatus de empleado
                    //            #region SIGOPLAN

                    //            #endregion

                    //            #region Enkontrol
                    //            var count = 0;

                    //            using (var cmd = new OdbcCommand(@"UPDATE sn_empleados SET estatus_empleado = 'B' WHERE clave_empleado = ?"))
                    //            {
                    //                OdbcParameterCollection parametersExplosion = cmd.Parameters;

                    //                parametersExplosion.Add("@clave_empleado", OdbcType.Numeric).Value = registroBajaSIGOPLAN.numeroEmpleado;

                    //                cmd.Connection = trans.Connection;
                    //                cmd.Transaction = trans;
                    //                count += cmd.ExecuteNonQuery();
                    //            }
                    //            #endregion
                    //            #endregion
                    //        }
                    //    }
                    //}
                    #endregion

                    var registroBajaSIGOPLAN = _context.tblRH_Baja_Registro.FirstOrDefault(x => x.registroActivo && x.id == objAutorizacion.baja_id);

                    if (registroBajaSIGOPLAN != null)
                    {
                        #region SIGOPLAN
                        registroBajaSIGOPLAN.est_baja = "P"; // "A" 
                        registroBajaSIGOPLAN.autorizada = AutorizacionEnum.PENDIENTE; //AUTORIZADA
                        registroBajaSIGOPLAN.comentariosCancelacion = objAutorizacion.comentariosCancelacion;
                        _context.SaveChanges();
                        #endregion
                        
                        var objEmpleado = _context.tblRH_EK_Empleados.Where(e => e.clave_empleado == registroBajaSIGOPLAN.numeroEmpleado).FirstOrDefault();

                        //if (objAutorizacion.autorizada == AutorizacionEnum.AUTORIZADA)
                        if (true)
                        {
                            #region Actualizar estatus de empleado
                            #region Enkontrol
                            //var count = 0;

                            //using (var cmd = new OdbcCommand(@"UPDATE sn_empleados SET estatus_empleado = 'A' WHERE clave_empleado = ?"))
                            //{
                            //    OdbcParameterCollection parametersExplosion = cmd.Parameters;

                            //    parametersExplosion.Add("@clave_empleado", OdbcType.Numeric).Value = registroBajaSIGOPLAN.numeroEmpleado;

                            //    cmd.Connection = trans.Connection;
                            //    cmd.Transaction = trans;
                            //    count += cmd.ExecuteNonQuery();
                            //}


                            if (objEmpleado != null)
                            {

                                objEmpleado.estatus_empleado = "A";
                                _context.SaveChanges();

                            }

                            var objEmplBaja = _context.tblRH_EK_Empl_Baja.FirstOrDefault(e => e.clave_empleado == registroBajaSIGOPLAN.numeroEmpleado);
                            if (objEmplBaja != null)
                            {
                                objEmplBaja.estatus = "P";
                                _context.SaveChanges();

                            }
                            #endregion
                            #endregion
                        }

                        #region Actualizar Incidencias Futuras Empleado A Baja (EDITABLE EN FRONT)
                        var fechaBaja = registroBajaSIGOPLAN.fechaBaja;
                        var claveEmpleadoBaja = registroBajaSIGOPLAN.numeroEmpleado;
                        var registroPeriodoSemanal = _context.tblRH_BN_EstatusPeriodos.Where(x => x.anio == fechaBaja.Value.Year && x.fecha_inicial <= fechaBaja && x.tipo_nomina == 1 && x.estatus).ToList();
                        var registroPeriodoQuincenal = _context.tblRH_BN_EstatusPeriodos.Where(x => x.anio == fechaBaja.Value.Year && x.fecha_inicial <= fechaBaja && x.tipo_nomina == 4 && x.estatus).ToList();

                        var lstPeriodosSemanal = registroPeriodoSemanal.Select(e => e.periodo).ToList();
                        var lstPeriodosQuincenal = registroPeriodoQuincenal.Select(e => e.periodo).ToList();

                        if (registroPeriodoSemanal != null)
                        {
                            //var periodoSemanal = registroPeriodoSemanal.periodo;
                            //var anioPeriodoSemanal = registroPeriodoSemanal.anio;
                            var incidenciasSemanales = _context.tblRH_BN_Incidencia.Where(x => x.anio == fechaBaja.Value.Year && lstPeriodosSemanal.Contains(x.periodo) && x.tipo_nomina == 1 && x.estatus != "A").Select(x => x.id).ToList();
                            if (incidenciasSemanales.Count() > 0)
                            {

                                var incidenciasSemanalesDetalle = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasSemanales.Contains(x.incidenciaID) && x.clave_empleado == claveEmpleadoBaja).ToList();

                                foreach (var item in incidenciasSemanalesDetalle)
                                {
                                    if (item.dia1 == 20) item.dia1 = 18;
                                    if (item.dia2 == 20) item.dia2 = 18;
                                    if (item.dia3 == 20) item.dia3 = 18;
                                    if (item.dia4 == 20) item.dia4 = 18;
                                    if (item.dia5 == 20) item.dia5 = 18;
                                    if (item.dia6 == 20) item.dia6 = 18;
                                    if (item.dia7 == 20) item.dia7 = 18;
                                    if (item.dia8 == 20) item.dia8 = 18;
                                    if (item.dia9 == 20) item.dia9 = 18;
                                    if (item.dia10 == 20) item.dia10 = 18;
                                    if (item.dia11 == 20) item.dia11 = 18;
                                    if (item.dia12 == 20) item.dia12 = 18;
                                    if (item.dia13 == 20) item.dia13 = 18;
                                    if (item.dia14 == 20) item.dia14 = 18;
                                    if (item.dia15 == 20) item.dia15 = 18;
                                    if (item.dia16 == 20) item.dia16 = 18;

                                    _context.SaveChanges();
                                }

                            }
                        }
                        if (registroPeriodoQuincenal != null)
                        {
                            //var periodoQuincenal = registroPeriodoQuincenal.periodo;
                            //var anioPeriodoQuincenal = registroPeriodoQuincenal.anio;
                            var incidenciasQuincenales = _context.tblRH_BN_Incidencia.Where(x => x.anio == fechaBaja.Value.Year && lstPeriodosQuincenal.Contains(x.periodo) && x.tipo_nomina == 4 && x.estatus != "A").Select(x => x.id).ToList();
                            if (incidenciasQuincenales.Count() > 0)
                            {
                                var incidenciasQuincenalesDetalle = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasQuincenales.Contains(x.incidenciaID) && x.clave_empleado == claveEmpleadoBaja).ToList();

                                foreach (var item in incidenciasQuincenalesDetalle)
                                {
                                    if (item.dia1 == 20) item.dia1 = 18;
                                    if (item.dia2 == 20) item.dia2 = 18;
                                    if (item.dia3 == 20) item.dia3 = 18;
                                    if (item.dia4 == 20) item.dia4 = 18;
                                    if (item.dia5 == 20) item.dia5 = 18;
                                    if (item.dia6 == 20) item.dia6 = 18;
                                    if (item.dia7 == 20) item.dia7 = 18;
                                    if (item.dia8 == 20) item.dia8 = 18;
                                    if (item.dia9 == 20) item.dia9 = 18;
                                    if (item.dia10 == 20) item.dia10 = 18;
                                    if (item.dia11 == 20) item.dia11 = 18;
                                    if (item.dia12 == 20) item.dia12 = 18;
                                    if (item.dia13 == 20) item.dia13 = 18;
                                    if (item.dia14 == 20) item.dia14 = 18;
                                    if (item.dia15 == 20) item.dia15 = 18;
                                    if (item.dia16 == 20) item.dia16 = 18;

                                    _context.SaveChanges();
                                }
                            }
                        }

                        #endregion

                        #region CORREO
                        string descRegPat = "";
                        //cc = objEmpleado.cc_contable;
                        if (objEmpleado.id_regpat != null)
                        {

                            var objRegPat = _context.tblRH_EK_Registros_Patronales.FirstOrDefault(e => e.clave_reg_pat == objEmpleado.id_regpat);
                            if (objRegPat != null)
                            {
                                descRegPat = objRegPat.nombre_corto;
                            }
                        }
                        var ccDesc = _ccFS_SP.GetCC(registroBajaSIGOPLAN.cc);
                        var puestoDesc = _context.tblRH_EK_Puestos.FirstOrDefault(x => x.puesto == registroBajaSIGOPLAN.idPuesto).descripcion;

                        List<string> correos = new List<string>();

                        string conceptoHead = "";
                        string conceptoBody = "";
                        string conceptoFooter = "";


                        conceptoBody = "cancelado de las Bajas de empleado";
                        conceptoHead = "CANCELACION de Baja de empleado";
                        conceptoFooter = "<br/><br/><b>NOTA, FAVOR DE REACTIVAR EL EMPLEADO EN EL IMSS CON UN DÍA POSTERIOR A SU FECHA DE BAJA</b>";


                        //var asunto = @"Se ha dado de baja un empleado en el centro de costos [" + registroBajaSIGOPLAN.cc + "]";
                        var asunto = conceptoHead + @" en el CC " + registroBajaSIGOPLAN.cc + " " + descRegPat;
                        var mensaje = string.Format(@"
                        El siguiente empleado ha sido " + conceptoBody + " :<br/><br/>" +
                        "{0} – {1}, para el centro de costos: {2} – {3}, con el puesto de: {4}. {5}", registroBajaSIGOPLAN.numeroEmpleado, registroBajaSIGOPLAN.nombre, registroBajaSIGOPLAN.cc, ccDesc.descripcion, puestoDesc, conceptoFooter);

                        //var listaUsuariosCorreos = _context.tblRH_UsuariosCorreoAutorizacionAlta.Where(x => x.registroActivo).Select(x => x.usuario_id).ToList();

                        //foreach (var usu in listaUsuariosCorreos)
                        //{
                        //    correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        //}

                        List<int> listaUsuariosCorreos = _context.tblRH_REC_Notificantes_Altas.Where(w => (w.cc == registroBajaSIGOPLAN.cc || w.cc == "*") && w.esAuth && w.esActivo && w.notificarBaja).Select(s => s.idUsuario).ToList();

                        foreach (var usu in listaUsuariosCorreos)
                        {
                            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        }

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan 
                            || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora
                            || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                        {
                            correos.AddRange(new List<string> 
                            { 
                                "auxnominas.hmo@taxandlegal.com.mx",
                                "aux.seguridadsocialhmo@taxandlegal.com.mx",
                                "auxoperacionfiscal.hmo@taxandlegal.com.mx",
                                "seguridadsocial.hmo@taxandlegal.com.mx",
                                "operacionfiscalhmo@taxandlegal.com.mx",
                                "seguridadsocialhmo.taxandlegal@gmail.com",
                                "nominas.hmo@taxandlegal.com.mx",
                                "despacho@construplan.com.mx",
                            });
                        }

#if DEBUG
                        correos = new List<string> { "miguel.buzani@construplan.com.mx" };
#endif

                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos);
                        #endregion
                    }

                    //trans.Commit();
                    dbSigoplanTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objAutorizacion));

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    //trans.Rollback();
                    dbSigoplanTransaction.Rollback();

                    LogError(0, 0, _NOMBRE_CONTROLADOR, "GuardarAutorizacionBajas", e, AccionEnum.ACTUALIZAR, 0, objAutorizacion);

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }


            }

            return resultado;
        }

        #endregion

        #region PERMISOS
        public bool UsuarioDeConsulta()
        {
            var esConsulta = true;

            try
            {
                esConsulta = _context.tblP_AccionesVistatblP_Usuario.Any(x => x.tblP_AccionesVista_id == 4029 && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);
            }
            catch (Exception ex)
            {
                esConsulta = true;
            }

            return esConsulta;
        }

        public bool UsuarioNotificar()
        {
            var esNoti = false;

            try
            {
                esNoti = _context.tblP_AccionesVistatblP_Usuario.Any(e => e.tblP_AccionesVista_id == 4041 && e.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);

                if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                {
                    esNoti = true;
                }
            }
            catch (Exception e)
            {

            }

            return esNoti;
        }

        public bool UsuarioLiberarNominas()
        {
            bool esLiberarNominas = false;

            try
            {
                esLiberarNominas = _context.tblP_AccionesVistatblP_Usuario.Any(x => x.tblP_AccionesVista_id == 4058 && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id);
            }
            catch (Exception)
            {
                esLiberarNominas = false;
            }

            return esLiberarNominas;
        }
        #endregion

        #region DIAS PARA BAJAS
        public bool GetPuedeEditarDiasDisponibles()
        {
            var puedeEditar = false;

            using (var _ctx = new MainContext())
            {
                try
                {
                    puedeEditar = _ctx.tblP_AccionesVistatblP_Usuario.Any(x => x.tblP_AccionesVista_id == 4033 && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id) || vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR;
                }
                catch
                {
                    puedeEditar = false;
                }
            }

            return puedeEditar;
        }

        public Dictionary<string, object> GetDiasDisponibles()
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                try
                {
                    if (_ctx.tblP_AccionesVistatblP_Usuario.Any(x => x.tblP_AccionesVista_id == 4033 && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id) || vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                    {
                        var diasDisponibles = _ctx.tblRH_Bajas_DiasPermitidos.Where(x => x.registroActivo)
                        .Select(x => new
                        {
                            x.anteriores,
                            x.posteriores
                        }).FirstOrDefault();

                        if (diasDisponibles != null)
                        {
                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, diasDisponibles);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información de los periodos de bajas");
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, new { anteriores = 0, posteriores = 0 });
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetDiasDisponiblesParaLimitarFecha()
        {
            resultado.Clear();

            var fechaActual = DateTime.Now;

            using (var _ctx = new MainContext())
            {
                try
                {
                    var diasDisponibles = _ctx.tblRH_Bajas_DiasPermitidos.Where(x => x.registroActivo).ToList()
                        .Select(x => new DiasDisponiblesIngresosDTO
                        {
                            anterior = fechaActual.AddDays(-x.anteriores).ToString("yyyy-MM-dd"),
                            posterior = fechaActual.AddDays(x.posteriores).ToString("yyyy-MM-dd")
                        }).FirstOrDefault();

                    if (diasDisponibles != null)
                    {
                        resultado.Add(ITEMS, diasDisponibles);
                    }
                    else
                    {
                        resultado.Add(ITEMS, new DiasDisponiblesIngresosDTO { anterior = fechaActual.ToString("yyyy-MM-dd"), posterior = fechaActual.ToString("yyyy-MM-dd") });
                    }

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, new DiasDisponiblesIngresosDTO { anterior = fechaActual.ToString("yyyy-MM-dd"), posterior = fechaActual.ToString("yyyy-MM-dd") });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> SetDiasDisponibles(int anteriores, int posteriores)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                try
                {
                    if (_ctx.tblP_AccionesVistatblP_Usuario.Any(x => x.tblP_AccionesVista_id == 4033 && x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id) || vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR)
                    {
                        var diasDisponibles = _ctx.tblRH_Bajas_DiasPermitidos.FirstOrDefault(x => x.registroActivo);
                        if (diasDisponibles != null)
                        {
                            diasDisponibles.anteriores = anteriores;
                            diasDisponibles.posteriores = posteriores;
                            diasDisponibles.fechaUltimaModificacion = DateTime.Now;
                            diasDisponibles.usuarioUltimaModificacion = vSesiones.sesionUsuarioDTO.id;
                            _ctx.SaveChanges();

                            var fechasDisponibles = new DiasDisponiblesIngresosDTO();
                            fechasDisponibles.anterior = DateTime.Now.AddDays(-diasDisponibles.anteriores).ToString("yyyy-MM-dd");
                            fechasDisponibles.posterior = DateTime.Now.AddDays(diasDisponibles.posteriores).ToString("yyyy-MM-dd");

                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, fechasDisponibles);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró información para modificar los periodos de bajas");
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No tiene permiso para modificar los periodos de bajas");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }
        #endregion

        #region LIBERAR NOMINA
        public Dictionary<string,object> SetNominaLiberada(int idBaja, string comentario)
        {
            resultado.Clear();

            using (var ctx = new MainContext())
            {
                var objBaja = ctx.tblRH_Baja_Registro.FirstOrDefault(e => e.id == idBaja);

                using (var dbTransac = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        objBaja.est_nominas = "A";
                        objBaja.est_nominas_usuario = vSesiones.sesionUsuarioDTO.id;
                        objBaja.est_nominas_fecha = DateTime.Now;
                        objBaja.est_nominas_comentario = comentario;
                        objBaja.est_nominas_firma = GlobalUtils.CrearFirmaDigital(objBaja.id, DocumentosEnum.LiberacionContabilidad, vSesiones.sesionUsuarioDTO.id);

                        #region CORREO
                        string asunto = "CONFIRMACION DEL DEPARTAMENTO DE NOMINAS - GESTION DE FINIQUITO CC " + objBaja.cc + " " + objBaja.descripcionCC;
                        string mensaje = @"
                            <html>
                                <head>
                                    <style>
                                        table {
                                            font-family: arial, sans-serif;
                                            border-collapse: collapse;
                                            width: 100%;
                                        }

                                        td, th {
                                            border: 1px solid #dddddd;
                                            text-align: left;
                                            padding: 8px;
                                        }

                                        tr:nth-child(even) {
                                            background-color: #ffcc5c;
                                        }
                                    </style>
                                </head>
                                <body lang=ES-MX link='#0563C1' vlink='#954F72'>Los siguientes finiquitos ya se encuentran gestionados:<br/>
                                    Se confirma él envió del recurso vía transferencia para su entrega en sitio.<br/>
                                    En el caso del personal de CC correspondientes a Oficinas Centrales, TMC, CRC y/o Arrendadora, el cheque se encuentra disponible en el Departamento de Nominas para su entrega.<br/><br/>
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>NOMBRE</th>
                                                <th>CC</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>" +
                                                "<td>" + objBaja.numeroEmpleado + "</td>"+
                                                "<td>" + objBaja.nombre + "</td>"+
                                                "<td>" + objBaja.cc + " " + objBaja.descripcionCC + "</td>"+
                                            @"</tr>
                                        </tbody>
                                    </table>
                                    <br><br><br>
                                    Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>
                                    Construplan > Capital Humano > Administración de Personal > Incidencias > Control de Ausencias > Gestion > Justificaciones.<br><br>
                                    Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>
                                    (http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.
                                </body>
                            </html>";
                        string cuerpoCorreo = mensaje;
                        //List<string> correo = new List<string> { "despacho@construplan.com.mx" };

                        List<string> correos = new List<string>();

                        //List<int> listaUsuariosCorreos = _context.tblRH_REC_Notificantes_Altas.Where(w => (w.cc == objBaja.cc || w.cc == "*") && w.esAuth && w.esActivo && w.notificarBaja).Select(s => s.idUsuario).ToList();

                        //foreach (var usu in listaUsuariosCorreos)
                        //{
                        //    correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        //}
                        
                        //if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        //{
                        //    correos.AddRange(new List<string> 
                        //    { 
                        //        "despacho@construplan.com.mx",
                        //    });
                        //}

                        List<int> lstNotificantes = _context.tblRH_Notis_RelConceptoUsuario.
                            Where(e => objBaja.cc == e.cc && (e.idConcepto == (int)ConceptosNotificantesEnum.Bajas
                                || e.idConcepto == (int)ConceptosNotificantesEnum.CH)).
                            Select(e => e.idUsuario).ToList();

                        foreach (var usu in lstNotificantes)
                        {
                            correos.Add(_context.tblP_Usuario.FirstOrDefault(x => x.id == usu).correo);
                        }

                        List<string> lstCorreoGenerales = _context.tblRH_Notis_RelConceptoCorreo.
                            Where(e => (e.cc == "*" || objBaja.cc == e.cc) && (e.idConcepto == (int)ConceptosNotificantesEnum.Bajas
                                || e.idConcepto == (int)ConceptosNotificantesEnum.CH)).
                            Select(e => e.correo).ToList();

                        foreach (var correo in lstCorreoGenerales)
                        {
                            correos.Add(correo);
                        }

                        correos.Add("serviciosalpersonal@construplan.com.mx");
                        correos.Add("rigoberto.coronado@construplan.com.mx");

#if DEBUG
                        correos = new List<string> { "miguel.buzani@construplan.com.mx" };
                        //correos.Add("omar.nunez@construplan.com.mx");
#endif
                        var correoEnviado = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), cuerpoCorreo, correos);

                        if (!correoEnviado)
                            LogError(0, 0, "BajasPersonalController", "SetNominaLiberada_enviarCorreo", null, AccionEnum.ACTUALIZAR, 0, objBaja);
                        #endregion

                        SaveBitacora(0, (int)AccionEnum.AGREGAR, objBaja.id, JsonUtils.convertNetObjectToJson(objBaja));

                        ctx.SaveChanges();
                        dbTransac.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbTransac.Rollback();

                        LogError(16, 0, "BajaPersonal", "SetNominaLiberada", e, AccionEnum.ACTUALIZAR, objBaja.id, objBaja);

                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }
            
            return resultado;
        }
        #endregion
    }
}

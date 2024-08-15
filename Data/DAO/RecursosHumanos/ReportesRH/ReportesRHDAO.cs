using Core.DAO.Enkontrol.General.CC;
using Core.DAO.Maquinaria.Reporte;
using Core.DAO.RecursosHumanos.ReportesRH;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.ActoCondicion.Graficas;
using Core.DTO.RecursosHumanos.Constancias;
using Core.DTO.RecursosHumanos.Prestamos;
//using Core.DTO.RecursosHumanos.Reclutamientos;
using Core.DTO.RecursosHumanos.ReportesRH;
using Core.DTO.Utils;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.FileManager;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Entity.RecursosHumanos.Reportes;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal.Usuario;
using Core.Enum.RecursosHumanos.Prestamos;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using Reportes.Reports.ControlObra;
using Reportes.Reports.RecursosHumanos.ReportesRH.rptConstancias;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Data.DAO.RecursosHumanos.ReportesRH
{
    public class ReportesRHDAO : GenericDAO<tblRH_LayautBajaEmpleados>, IReportesRHDAO
    {
        ICCDAO _ccFS = new CCFactoryService().getCCService();
        ICCDAO _ccFS_SP = new CCFactoryService().getCCServiceSP();
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        IEncabezadoDAO encabezadoFactoryServices = new EncabezadoFactoryServices().getEncabezadoServices();

        private string NombreControlador = "ReportesRHController";
        private readonly string RutaServidor;
        private readonly string RutaServidorFormatosLocal = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\FORMATOS";
        private readonly string RutaServidorFormatos = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS\FORMATOS";

        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\PRESTAMOS";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\PRESTAMOS";

        public ReportesRHDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\RECLUTAMIENTOS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\CAPITAL_HUMANO\RECLUTAMIENTOS";
#endif
        }

        public List<ComboDTO> getListaCC()
        {
            if (vSesiones.sesionEmpresaActual == 1)
                return (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();
            else
                return (List<ComboDTO>)ContextArrendadora.Where("SELECT DISTINCT (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))) AS Value, (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))+'-'+descripcion) AS Text, area, cuenta FROM si_area_cuenta ORDER BY area, cuenta").ToObject<List<ComboDTO>>();
        }
        public List<ComboDTO> getListaCCRH()
        {
            var ccs = new List<ComboDTO>();
            //return (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();

            if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR || vSesiones.sesionUsuarioDTO.esAuditor)
            {
                ccs = _ccFS_SP.GetCCsNominaInactivos().Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = x.cc + '-' + x.descripcion
                }).ToList();
            }

            if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO || vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.MEDICO)
            {
                var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                if (usuarioCCs.Count > 0)
                {
                    if (usuarioCCs.Any(x => x == "*"))
                    {
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
                    }
                    else
                    {
                        //ccs = _ccFS.GetCCs(usuarioCCs).Select(x => new ComboDTO
                        //{
                        //    Value = x.cc,
                        //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                        //}).OrderBy(x => x.Value).ToList();

                        ccs = _ccFS_SP.GetCCsNominaInactivos(usuarioCCs).Select(x => new ComboDTO
                        {
                            Value = x.cc,
                            Text = "[" + x.cc + "] " + x.descripcion.Trim()
                        }).OrderBy(x => x.Value).ToList();
                    }
                }
            }

            if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                ccs = ccs.Where(e => Regex.IsMatch(e.Value, @"^\d+$")).ToList();
            }

            return ccs;
        }
        public List<ComboDTO> getListaCCRHBajas()
        {
            var ccs = new List<ComboDTO>();
            //return (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();

            if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.ADMINISTRADOR || vSesiones.sesionUsuarioDTO.esAuditor)
            {
                ccs = _ccFS_SP.GetCCsNominaInactivos().Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = x.cc + '-' + x.descripcion
                }).ToList();
            }

            if (vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.USUARIO || vSesiones.sesionUsuarioDTO.idPerfil == (int)PerfilUsuarioEnum.MEDICO)
            {
                var usuarioCCs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                if (usuarioCCs.Count > 0)
                {
                    if (usuarioCCs.Any(x => x == "*"))
                    {
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
                    }
                    else
                    {
                        //ccs = _ccFS.GetCCs(usuarioCCs).Select(x => new ComboDTO
                        //{
                        //    Value = x.cc,
                        //    Text = "[" + x.cc + "] " + x.descripcion.Trim()
                        //}).OrderBy(x => x.Value).ToList();

                        ccs = _ccFS_SP.GetCCsNominaInactivos().Select(x => new ComboDTO
                        {
                            Value = x.cc,
                            Text = "[" + x.cc + "] " + x.descripcion.Trim()
                        }).OrderBy(x => x.Value).ToList();
                    }
                }
            }

            if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                ccs = ccs.Where(e => Regex.IsMatch(e.Value, @"^\d+$")).ToList();
            }

            return ccs;
        }
        public List<ComboDTO> getListaConceptosBaja()
        {
            //return (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT clave_razon_baja as Value,desc_motivo_baja as Text FROM sn_razones_baja").ToObject<List<ComboDTO>>();

            return _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = "SELECT clave_razon_baja as Value,desc_motivo_baja as Text FROM tblRH_EK_Razones_Baja",

            });
        }
        public List<ComboDTO> getListaEmpleadosByCC(List<string> cc)
        {
            string ccS = @"";
            foreach (var i in cc)
            {
                ccS += "'" + i + "',";
            }
            ccS = ccS.TrimEnd(',');
            //string query = "SELECT clave_empleado as Value,(ape_paterno+' '+ape_materno+' '+nombre) as Text FROM sn_empleados where cc_contable in (" + ccS + ") and clave_empleado not in (select clave_empleado from sn_empl_baja) and estatus_empleado='A'";
            //var result = (List<ComboDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<ComboDTO>>();

            var result = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT clave_empleado as Value,(ape_paterno+' '+ape_materno+' '+nombre) as Text 
                                FROM tblRH_EK_Empleados 
                                where cc_contable in (" + ccS + ") and clave_empleado not in (select numeroEmpleado from tblRH_Baja_Registro WHERE registroActivo = 1 AND est_baja = 'A') and estatus_empleado='A'",
            });
            return result;
        }
        public List<RepAltasDTO> getListaAltas(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {

            #region REPORTE VIEJO
            
//            var resultFiltered = new List<RepAltasDTO>();

//            var result = _context.Select<RepAltasDTO>(new DapperDTO
//            {
//                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
//                consulta = @"SELECT 
//                                e.estatus_empleado,
//                                e.cc_contable +' '+c.ccDescripcion AS cCdes,e.nss,
//                                e.cc_contable AS cC,
//                                e.clave_empleado AS empleadoID,
//                                (e.ape_paterno+' '+e.ape_materno+' '+e.nombre) AS empleado, 
//                                pu.descripcion as puesto,
//							    tRecon.fecha_reingreso,
//							    e.fecha_antiguedad,
//                                CONVERT(CHAR(20) ,e.fecha_antiguedad ,103) AS fechaAltaStr, 
//                                e.fecha_antiguedad ,(ne.nombre+' '+ne.ape_paterno+' '+ne.ape_materno) as jefeInmediato,
//	                            srp.clave_reg_pat,
//	                            srp.nombre_corto,
//                                tn.descripcion as tipo_nomina,
//	                            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as salario_base,
//                                (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as complemento,
//                                (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as bono_zona,
//                                tRecon.cc as ccRecon,
//								tCambio.CcID as ccCambio,
//                                tCambio.FechaInicioCambio as fechaCambio
//                            FROM tblRH_EK_Empleados AS e 
//                            LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado  
//                            LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = e.cc_contable  
//                            INNER JOIN tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto  
//                            INNER JOIN tblRH_EK_Tipos_Nomina AS tn ON e.tipo_nomina = tn.tipo_nomina  
//                            LEFT JOIN tblRH_EK_Departamentos AS dp ON dp.clave_depto = e.clave_depto 
//                            LEFT JOIN tblRH_EK_Empl_Grales AS empg ON empg.clave_empleado = e.clave_empleado  
//                            LEFT JOIN tblRH_EK_Estados AS est ON (est.clave_estado = empg.estado_dom and est.clave_pais=e.clave_pais_nac) 
//                            LEFT JOIN tblRH_EK_Ciudades AS cd ON (cd.clave_cuidad = empg.cuidado_dom and cd.clave_pais=e.clave_pais_nac and cd.clave_estado=empg.estado_dom) 
//	                        INNER JOIN tblRH_EK_Registros_Patronales srp on e.id_regpat = srp.clave_reg_pat
//							LEFT JOIN tblRH_EK_Empl_Recontratacion as tRecon ON e.clave_empleado = tRecon.clave_empleado
//							LEFT JOIN tblRH_FormatoCambio as tCambio ON e.clave_empleado = tCambio.Clave_Empleado AND tCambio.Aprobado = 1
//                            WHERE e.esActivo = 1 AND (e.cc_contable in (" + getStringInlineArray(cc) + ") OR tRecon.cc in (" + getStringInlineArray(cc) + ") OR tCambio.CcID in (" + getStringInlineArray(cc) + "))"
//            });

//            var listaCCDescripciones = _ccFS_SP.GetCCsNominaFiltrados(result.Select(x => x.cC).ToList());

//            foreach (var item in result)
//            {
//                if (item.ccRecon != null && cc.Contains(item.ccRecon))
//                {
//                    item.cCdes = "[" + item.ccRecon + "] " + listaCCDescripciones.Where(x => x.cc == item.ccRecon).Select(x => x.descripcion).FirstOrDefault();

//                }
//                else
//                {
//                    if (item.ccCambio != null && cc.Contains(item.ccCambio))
//                    {
//                        item.fecha_reingreso = item.fechaCambio;
//                        item.cCdes = "[" + item.ccCambio + "] " + listaCCDescripciones.Where(x => x.cc == item.ccCambio).Select(x => x.descripcion).FirstOrDefault();

//                    }
//                    else
//                    {
//                        item.cCdes = "[" + item.cC + "] " + listaCCDescripciones.Where(x => x.cc == item.cC).Select(x => x.descripcion).FirstOrDefault();

//                    }

//                }
//                int cveEmpleado = Convert.ToInt32(item.empleadoID);

//                //CultureInfo ci = CultureInfo.CurrentCulture; 
//                //int weekNumber = ci.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday

//                if (item.tipo_nomina == "SEMANAL")
//                {
//                    item.total_mensual = ((item.salario_base + item.complemento + item.bono_zona) / 7) * 30.4M;
//                }
//                else
//                {
//                    item.total_mensual = (item.salario_base + item.complemento + item.bono_zona) * 2;
//                }

//                if (item.fecha_reingreso == null && (item.estatus_empleado != "P" && item.estatus_empleado != "C"))
//                {
//                    item.fecha_reingreso = item.fecha_antiguedad;
//                }


//                if (item.fecha_antiguedad != null)
//                {
//                    //DateTime fechaReingreso = new DateTime(Convert.ToInt32(item.EMP_ULTIMO_REINGRESO.Substring(6, 4)), Convert.ToInt32(item.EMP_ULTIMO_REINGRESO.Substring(3, 2)), Convert.ToInt32(item.EMP_ULTIMO_REINGRESO.Substring(0, 2)));
//                    if (item.fecha_antiguedad.HasValue && item.fecha_antiguedad.Value.Date <= fechaFin.Date && item.fecha_antiguedad.Value.Date >= fechaInicio.Date)
//                    {
//                        if ((item.estatus_empleado == "C" || item.estatus_empleado == "P") && item.fecha_reingreso.Value.Date == item.fecha_antiguedad.Value.Date)
//                        {
//                            continue;
//                        }
//                        var next = result.Where(x => x.empleadoID == item.empleadoID && x.fecha_reingreso.HasValue && x.fecha_reingreso.Value.Date > item.fecha_reingreso.Value.Date).OrderBy(x => x.fecha_reingreso.Value).FirstOrDefault();
//                        if (
//                            next != null &&
//                            !_context.tblRH_Baja_Registro.Any(x => x.registroActivo && x.est_baja == "A" && x.numeroEmpleado == cveEmpleado && x.fechaBaja.Value >= item.fecha_reingreso.Value && x.fechaBaja.Value <= next.fecha_reingreso.Value))
//                        {
//                            continue;
//                        }

//                        if (item.ccRecon != null)
//                        {
//                            var objEmpleado = resultFiltered.FirstOrDefault(e => e.empleadoID == item.empleadoID && e.ccRecon == item.ccRecon);

//                            if (objEmpleado == null && cc.Contains(item.ccRecon))
//                            {
//                                resultFiltered.Add(item);
                                
//                            }
//                        }
//                        else if (item.ccCambio != null )
//                        {
//                            //var objEmpleado = resultFiltered.FirstOrDefault(e => e.empleadoID == item.empleadoID && e.ccRecon == item.ccRecon);

//                            if (cc.Contains(item.ccCambio))
//                            {
//                                resultFiltered.Add(item);                     
//                            }
//                        }
//                        else
//                        {
//                            var objEmpleado = resultFiltered.FirstOrDefault(e => e.empleadoID == item.empleadoID && e.ccRecon == null);

//                            if (objEmpleado == null)
//                            {
//                                resultFiltered.Add(item);
//                            }
//                            else
//                            {

//                                objEmpleado.fecha_reingreso = item.fecha_reingreso;

//                            }
//                        }
                        
//                    }
//                }
//            }

//            resultFiltered = resultFiltered.Distinct().ToList();

//            //var R = resultFiltered.Where(x => cc.Contains(x.cC)).ToList();

//            return resultFiltered.OrderBy(x => x.cCdes).ThenBy(x => x.empleado).ThenBy(x => x.fechaAlta).ToList();
            #endregion

            List<RepAltasDTO> lstAltas = new List<RepAltasDTO>();

            var result = _context.Select<RepAltasDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT 
                                    e.estatus_empleado,
                                    e.cc_contable +' '+c.ccDescripcion AS cCdes,e.nss,
                                    e.cc_contable AS cC,
                                    e.clave_empleado AS empleadoID,
                                    (e.ape_paterno+' '+e.ape_materno+' '+e.nombre) AS empleado, 
                                    pu.descripcion as puesto,
                                    tabCat.concepto as categoria,
	                                e.fecha_antiguedad,
                                    CONVERT(CHAR(20) ,e.fecha_antiguedad ,103) AS fechaAltaStr, 
									tBajas.fechaIngreso as bajaIngreso,
									e.fecha_alta,
                                    e.fecha_antiguedad ,(ne.nombre+' '+ne.ape_paterno+' '+ne.ape_materno) as jefeInmediato,
	                                srp.clave_reg_pat,
	                                srp.nombre_corto,
                                    tn.descripcion as tipo_nomina,
	                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as salario_base,
                                    (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as complemento,
                                    (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as bono_zona
                                FROM tblRH_EK_Empleados AS e 
                                LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado  
                                LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = e.cc_contable  
                                INNER JOIN tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto  
                                INNER JOIN tblRH_EK_Tipos_Nomina AS tn ON e.tipo_nomina = tn.tipo_nomina  
                                LEFT JOIN tblRH_EK_Departamentos AS dp ON dp.clave_depto = e.clave_depto 
                                LEFT JOIN tblRH_EK_Empl_Grales AS empg ON empg.clave_empleado = e.clave_empleado  
                                LEFT JOIN tblRH_EK_Estados AS est ON (est.clave_estado = empg.estado_dom and est.clave_pais=e.clave_pais_nac) 
                                LEFT JOIN tblRH_EK_Ciudades AS cd ON (cd.clave_cuidad = empg.cuidado_dom and cd.clave_pais=e.clave_pais_nac and cd.clave_estado=empg.estado_dom) 
                                LEFT JOIN tblRH_EK_Registros_Patronales srp on e.id_regpat = srp.clave_reg_pat
                                LEFT JOIN tblRH_REC_Requisicion AS tabReq ON tabReq.id = e.requisicion
							    LEFT JOIN tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = tabReq.idTabuladorDet
							    LEFT JOIN tblRH_TAB_CatCategorias AS tabCat ON tabDet.FK_Categoria = tabCat.id
                                LEFT JOIN tblRH_Baja_Registro AS tBajas ON e.fecha_alta != e.fecha_antiguedad AND tBajas.registroActivo = 1 AND tBajas.est_baja = 'A' AND tBajas.numeroEmpleado = e.clave_empleado 
                            WHERE e.esActivo = 1 AND e.estatus_empleado NOT IN ('C','P') AND e.cc_contable in (" + getStringInlineArray(cc) + ")"
            });

            var listaCCDescripciones = _ccFS_SP.GetCCsNominaFiltrados(result.Select(x => x.cC).ToList());

            foreach (var item in result)
            {
                if (item.fecha_antiguedad != item.fecha_alta && item.bajaIngreso != null)
                {
                    if (item.estatus_empleado == "A")
                    {
                        var objRegistroActual = result.FirstOrDefault(e => e.empleadoID == item.empleadoID && e.fecha_alta == e.fecha_antiguedad);

                        if (objRegistroActual == null && item.fecha_antiguedad >= fechaInicio && item.fecha_antiguedad <= fechaFin)
                        {
                            item.fecha_alta = item.fecha_antiguedad.Value;
                            lstAltas.Add(item);
                        }
                    }
                    else
                    {
                        item.fecha_alta = item.bajaIngreso;

                    }
                }

                if (item.fecha_alta >= fechaInicio && item.fecha_alta <= fechaFin)
                {
                    item.cCdes = "[" + item.cC + "] " + listaCCDescripciones.Where(x => x.cc == item.cC).Select(x => x.descripcion).FirstOrDefault();

                    if (item.tipo_nomina == "SEMANAL")
                    {
                        item.total_mensual = ((item.salario_base + item.complemento + item.bono_zona) / 7) * 30.4M;
                    }
                    else
                    {
                        item.total_mensual = (item.salario_base + item.complemento + item.bono_zona) * 2;
                    }

                    lstAltas.Add(item);
                }
            }

            return lstAltas.Distinct().OrderBy(x => x.cCdes).ThenBy(x => x.empleado).ThenBy(x => x.fechaAlta).ToList();
        }

        public List<RepBajasDTO> getLayoutBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin)
        {
            var listaEmpleadosAlta = _context.tblRH_LayautBajaEmpleados.Select(x => x.empleadoID).ToList();
            var ccs = cc != null ? getStringInlineArray(cc) : "''";
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
                                                e.jefe_inmediato AS jefeInmediatoID,
                                                (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) AS jefeInmediato,
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
            					            WHERE eb.cc in (" + ccs + ") and eb.fechaBaja between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and eb.motivoBajaDeSistema in (" + String.Join(",", concepto) + ") and eb.registroActivo = 1 and eb.est_baja='A'" +
                            ((fechaContaInicio.HasValue && fechaContaFin.HasValue) ? (" AND eb.est_contabilidad_fecha BETWEEN '" + fechaContaInicio.Value.ToString("yyyMMdd") + "' AND '" + fechaContaFin.Value.ToString("yyyMMdd") + "' ") : "") +
                            "ORDER BY e.ape_paterno",
            });
            var resultU = new List<RepBajasDTO>();
            foreach (var i in result)
            {
                i.fechaBaja = Convert.ToDateTime(i.fechaBajaStr);
                i.recontratable = !string.IsNullOrEmpty(i.recontratable) ? i.recontratable.Equals("S") ? "SI" : "NO" : "";
                //i.fechaAltaBaja = string.IsNullOrEmpty(i.FechaRec) ? i.fechaAltaStr : i.FechaRec;
                //i.fechaAltaBaja = i.fechaAlta.HasValue ? i.fechaAlta.Value.ToShortDateString() : i.fechaAntiguedad.ToShortDateString();
                //i.fechaAltaBaja = i.fechaAntiguedad.ToShortDateString();
                i.fechaAltaBaja = i.fechaIngreso.HasValue ? i.fechaIngreso.Value.ToShortDateString() : i.fechaAlta.HasValue ? i.fechaAlta.Value.ToShortDateString() : i.fechaAntiguedad.ToShortDateString();

                var objBaja = resultU.Where(e => e.empleadoID == i.empleadoID && e.puestoID == i.puestoID).FirstOrDefault();

                if (objBaja == null)
                {

                    resultU.Add(i);
                }
                else
                {
                    if (i.fechaBaja != null)
                    {
                        if (objBaja.fechaBaja < i.fechaBaja)
                        {
                            resultU.Remove(objBaja);
                            resultU.Add(i);
                        }
                    }
                }

            }

            #region Estatus Baja
            foreach (var registro in resultU)
            {
                if (registro.est_contabilidad == "A")
                {
                    registro.estatus_baja = 3;
                    registro.estatus_bajaDesc = "LIBERADA";
                    registro.est_contabilidad_comentario = string.IsNullOrEmpty(registro.est_contabilidad_comentario) ? "LIBERADA" : registro.est_contabilidad_comentario;

                    if (registro.est_inventario == "A")
                    {
                        registro.est_inventario_comentario = string.IsNullOrEmpty(registro.est_inventario_comentario) ? "LIBERADA" : registro.est_inventario_comentario;
                    }
                    else
                    {
                        registro.est_inventario_comentario = "NO LIBERADA";
                    }

                    if (registro.est_compras == "A")
                    {
                        registro.est_compras_comentario = string.IsNullOrEmpty(registro.est_compras_comentario) ? "LIBERADA" : registro.est_compras_comentario;
                    }
                    else
                    {
                        registro.est_compras_comentario = "NO LIBERADA";
                    }

                }
                else
                {
                    registro.est_contabilidad_comentario = "NO LIBERADA";

                    if (registro.est_baja == "A")
                    {
                        registro.estatus_baja = 1;
                        registro.estatus_bajaDesc = "AUTORIZADA";
                    }
                    else
                    {
                        registro.estatus_baja = 2;
                        registro.estatus_bajaDesc = "PENDIENTE";
                    }

                    if (registro.est_inventario == "A")
                    {
                        registro.est_inventario_comentario = string.IsNullOrEmpty(registro.est_inventario_comentario) ? "LIBERADA" : registro.est_inventario_comentario;
                    }
                    else
                    {
                        registro.est_inventario_comentario = "NO LIBERADA";
                    }

                    if (registro.est_compras == "A")
                    {
                        registro.est_compras_comentario = string.IsNullOrEmpty(registro.est_compras_comentario) ? "LIBERADA" : registro.est_compras_comentario;
                    }
                    else
                    {
                        registro.est_compras_comentario = "NO LIBERADA";
                    }

                }
            }
            #endregion
            if (estatus != null && estatus.Count() > 0)
            {
                resultU = resultU.Where(x => estatus.Contains(x.estatus_baja)).ToList();
            }

            return resultU.OrderBy(x => x.cC).ThenBy(x => x.empleado).ThenBy(x => x.fechaBaja).ToList();
        }

        public List<RepBajasDTO> getListaBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin, int? tipoBajas)
        {
            var listaEmpleadosAlta = _context.tblRH_LayautBajaEmpleados.Select(x => x.empleadoID).ToList();
            var ccs = cc != null ? getStringInlineArray(cc) : "''";

            //string query = "SELECT eb.cc_contable as cCSolo, sip.puesto as puestoID,srp.nombre_corto as regPatronal, sip.descripcion as puestosDes, (eb.cc_contable +' - '+ c.descripcion) as cC,e.clave_empleado as empleadoID,(e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) as empleado,rz.desc_motivo_baja as Concepto,e.jefe_inmediato as jefeInmediatoID,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,CONVERT( CHAR( 20 ), eb .fecha_baja, 103 ) as fechaBajaStr,e.contratable as recontratable";
            //query += " ,CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as FechaRec,";
            //query += "CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as fechaAltaStr,";
            //query += "e.nss as nss ";
            //query += " FROM sn_empleados as e inner join sn_registros_patronales srp on e.id_regpat = srp.clave_reg_pat inner join sn_empleados as ne on e.jefe_inmediato=ne.clave_empleado inner join sn_empl_baja as eb on e.clave_empleado=eb.clave_empleado inner join si_puestos as sip on sip.puesto = e.puesto inner join sn_razones_baja as rz on eb.motivo_baja = rz.clave_razon_baja inner join DBA.cc as c ON c.cc = eb.cc_contable where eb.cc_contable in (" + getStringInlineArray(cc) + ") and eb.fecha_baja between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and eb.motivo_baja in (" + String.Join(",", concepto) + ") and eb.estatus='A'";
            //query += " ORDER BY e.ape_paterno";

            //var result = (List<RepBajasDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<RepBajasDTO>>();
            #region V1 MIGRADO

            //            var result = _context.Select<RepBajasDTO>(new DapperDTO 
            //            {
            //                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
            //                consulta = @"SELECT eb.cc_contable as cCSolo, sip.puesto as puestoID,srp.nombre_corto as regPatronal, sip.descripcion as puestosDes, (eb.cc_contable +' - '+ c.descripcion) as cC,e.clave_empleado as empleadoID,(e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) as empleado,rz.desc_motivo_baja as Concepto,e.jefe_inmediato as jefeInmediatoID,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,CONVERT( CHAR( 20 ), eb.fecha_baja, 103 ) as fechaBajaStr,e.contratable as recontratable
            //                             ,CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as FechaRec,
            //                            CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as fechaAltaStr,
            //                            e.nss as nss 
            //                             FROM tblRH_EK_Empleados as e inner join tblRH_EK_Registros_Patronales srp on e.id_regpat = srp.clave_reg_pat inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado inner join tblRH_EK_Empl_Baja as eb on e.clave_empleado=eb.clave_empleado inner join tblRH_EK_Puestos as sip on sip.puesto = e.puesto inner join tblRH_EK_Razones_Baja as rz on eb.motivo_baja = rz.clave_razon_baja inner join tblP_CC as c ON c.cc = eb.cc_contable where eb.cc_contable in (" + getStringInlineArray(cc) + ") and eb.fecha_baja between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and eb.motivo_baja in (" + String.Join(",", concepto) + ") and eb.estatus='A'" +
            //                             "ORDER BY e.ape_paterno",
            //            });
            #endregion

            #region SIN DUPLICADOS
            //                        var result = _context.Select<RepBajasDTO>(new DapperDTO
            //            {
            //                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //                consulta = @"
            //                    SELECT
            //                        eb.cc AS cCSolo,
            //                        sip.puesto AS puestoID,
            //                        srp.nombre_corto AS regPatronal,
            //                        sip.descripcion AS puestosDes,
            //                        (eb.cc +' - '+ c.descripcion) AS cC,
            //                        e.clave_empleado AS empleadoID,
            //                        (e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) AS empleado,
            //                        rz.desc_motivo_baja AS Concepto,
            //                        e.jefe_inmediato AS jefeInmediatoID,
            //                        (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) AS jefeInmediato,
            //                        CONVERT( CHAR( 20 ), eb.fechaBaja, 103 ) AS fechaBajaStr,
            //                        e.contratable AS recontratable,
            //                        CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS FechaRec,
            //                        CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS fechaAltaStr,
            //                        e.nss AS nss,
            //                        eb.est_baja,
            //                        eb.est_inventario,
            //                        est_contabilidad,
            //                        est_compras,
            //                        eb.est_inventario_comentario,
            //                        eb.est_compras_comentario,
            //                        eb.est_contabilidad_comentario,
            //                        (
            //                            SELECT TOP 1
            //                                recontratacion.fecha_reingreso
            //                            FROM
            //                                tblRH_EK_Empl_Recontratacion AS recontratacion
            //                            WHERE
            //                                recontratacion.clave_empleado = e.clave_empleado AND
            //                                ((recontratacion.esActivo IS NOT NULL AND recontratacion.esActivo = 1) OR (recontratacion.esActivo IS NULL)) AND
            //                                recontratacion.fecha_reingreso < eb.fechaBaja
            //                            ORDER BY
            //                                recontratacion.fecha_reingreso DESC
            //                        ) AS fechaAlta,
            //                        eb.fechaIngreso AS fechaIngreso,
            //                        e.fecha_antiguedad AS fechaAntiguedad,
            //                        eb.comentarios
            //                    FROM tblRH_EK_Empleados AS e 
            //					    LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON e.id_regpat = srp.clave_reg_pat 
            //					    LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
            //					    LEFT JOIN tblRH_Baja_Registro AS eb ON e.clave_empleado=eb.numeroEmpleado 
            //					    LEFT JOIN tblRH_EK_Puestos AS sip ON sip.puesto = eb.idPuesto 
            //					    LEFT JOIN tblRH_EK_Razones_Baja AS rz ON eb.motivoBajaDeSistema = rz.clave_razon_baja 
            //					    LEFT JOIN tblP_CC AS c ON c.cc = eb.cc
            //					WHERE eb.cc in (" + ccs + ") and eb.fechaBaja between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and eb.motivoBajaDeSistema in (" + String.Join(",", concepto) + ") and eb.registroActivo = 1 and eb.est_baja='A'" +
            //                            ((fechaContaInicio.HasValue && fechaContaFin.HasValue) ? (" AND eb.est_contabilidad_fecha BETWEEN '" + fechaContaInicio.Value.ToString("yyyMMdd") + "' AND '" + fechaContaFin.Value.ToString("yyyMMdd")+"' ") : "") +
            //                            "ORDER BY e.ape_paterno",
            //            });
            //            var resultU = new List<RepBajasDTO>();
            //            foreach (var i in result)
            //            {
            //                i.fechaBaja = Convert.ToDateTime(i.fechaBajaStr);
            //                i.recontratable = !string.IsNullOrEmpty(i.recontratable) ? i.recontratable.Equals("S") ? "SI" : "NO" : "";
            //                //i.fechaAltaBaja = string.IsNullOrEmpty(i.FechaRec) ? i.fechaAltaStr : i.FechaRec;
            //                //i.fechaAltaBaja = i.fechaAlta.HasValue ? i.fechaAlta.Value.ToShortDateString() : i.fechaAntiguedad.ToShortDateString();
            //                //i.fechaAltaBaja = i.fechaAntiguedad.ToShortDateString();
            //                i.fechaAltaBaja = i.fechaIngreso.HasValue ? i.fechaIngreso.Value.ToShortDateString() : i.fechaAlta.HasValue ? i.fechaAlta.Value.ToShortDateString() : i.fechaAntiguedad.ToShortDateString();

            //                var objBaja = resultU.Where(e => e.empleadoID == i.empleadoID && e.puestoID == i.puestoID).FirstOrDefault();

            //                if (objBaja == null)
            //                {

            //                    resultU.Add(i);
            //                }
            //                else
            //                {
            //                    if (i.fechaBaja != null)
            //                    {
            //                        if (objBaja.fechaBaja < i.fechaBaja)
            //                        {
            //                            resultU.Remove(objBaja);
            //                            resultU.Add(i);
            //                        }
            //                    }
            //                }

            //            }

            //            #region Estatus Baja
            //            foreach (var registro in resultU)
            //            {
            //                if (registro.est_contabilidad == "A")
            //                {
            //                    registro.estatus_baja = 3;
            //                    registro.estatus_bajaDesc = "LIBERADA";
            //                    registro.est_contabilidad_comentario = string.IsNullOrEmpty(registro.est_contabilidad_comentario) ? "LIBERADA" : registro.est_contabilidad_comentario;

            //                    if (registro.est_inventario == "A")
            //                    {
            //                        registro.est_inventario_comentario = string.IsNullOrEmpty(registro.est_inventario_comentario) ? "LIBERADA" : registro.est_inventario_comentario;
            //                    }
            //                    else
            //                    {
            //                        registro.est_inventario_comentario = "NO LIBERADA";
            //                    }

            //                    if (registro.est_compras == "A")
            //                    {
            //                        registro.est_compras_comentario = string.IsNullOrEmpty(registro.est_compras_comentario) ? "LIBERADA" : registro.est_compras_comentario;
            //                    }
            //                    else
            //                    {
            //                        registro.est_compras_comentario = "NO LIBERADA";
            //                    }

            //                }
            //                else
            //                {
            //                    registro.est_contabilidad_comentario = "NO LIBERADA";

            //                    if (registro.est_baja == "A")
            //                    {
            //                        registro.estatus_baja = 1;
            //                        registro.estatus_bajaDesc = "AUTORIZADA";
            //                    }
            //                    else
            //                    {
            //                        registro.estatus_baja = 2;
            //                        registro.estatus_bajaDesc = "PENDIENTE";
            //                    }

            //                    if (registro.est_inventario == "A")
            //                    {
            //                        registro.est_inventario_comentario = string.IsNullOrEmpty(registro.est_inventario_comentario) ? "LIBERADA" : registro.est_inventario_comentario;
            //                    }
            //                    else
            //                    {
            //                        registro.est_inventario_comentario = "NO LIBERADA";
            //                    }

            //                    if (registro.est_compras == "A")
            //                    {
            //                        registro.est_compras_comentario = string.IsNullOrEmpty(registro.est_compras_comentario) ? "LIBERADA" : registro.est_compras_comentario;
            //                    }
            //                    else
            //                    {
            //                        registro.est_compras_comentario = "NO LIBERADA";
            //                    }

            //                }
            //            }
            //#endregion
            #endregion

            var result = new List<RepBajasDTO>();

            if (ccs.Count() > 5)
            {

                var grouposCC = new List<List<string>>();
                var lstGroups = new List<string>();
                int numCCsTotales = 0 ;

                foreach (var item in cc)
                {
                    numCCsTotales ++;
                    lstGroups.Add(item);

                    if (lstGroups.Count() == 5 || numCCsTotales == cc.Count())
                    {
                        grouposCC.Add(lstGroups);

                        lstGroups = new List<string>();
                    }
                    
                }

                foreach (var item in grouposCC)
                {
                    
                    result.AddRange(
                        _context.Select<RepBajasDTO>(new DapperDTO
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
                                    e.jefe_inmediato AS jefeInmediatoID,
                                    (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) AS jefeInmediato,
                                    CONVERT( CHAR( 20 ), eb.fechaBaja, 103 ) AS fechaBajaStr,
                                    e.contratable AS recontratable,
                                    CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS FechaRec,
                                    CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS fechaAltaStr,
                                    e.nss AS nss,
                                    eb.est_baja,
                                    eb.est_inventario,
                                    est_contabilidad,
                                    est_compras,
                                    eb.est_nominas,
                                    eb.est_inventario_comentario,
                                    eb.est_compras_comentario,
                                    eb.est_contabilidad_comentario,
                                    eb.est_nominas_comentario,
                                    eb.est_inventario_fecha,
                                    eb.est_contabilidad_fecha,
                                    eb.est_compras_fecha,
                                    eb.est_nominas_fecha,
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
                                    eb.comentarios,
                                    eb.esAnticipada
                                FROM tblRH_EK_Empleados AS e 
					                LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON e.id_regpat = srp.clave_reg_pat 
					                LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
					                LEFT JOIN tblRH_Baja_Registro AS eb ON e.clave_empleado=eb.numeroEmpleado 
					                LEFT JOIN tblRH_EK_Puestos AS sip ON sip.puesto = eb.idPuesto 
					                LEFT JOIN tblRH_EK_Razones_Baja AS rz ON eb.motivoBajaDeSistema = rz.clave_razon_baja 
					                LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = eb.cc
					            WHERE eb.cc in (" + getStringInlineArray(item) + ") and eb.fechaBaja between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and eb.motivoBajaDeSistema in (" + String.Join(",", concepto) + ") and eb.registroActivo = 1 and eb.est_baja='A'" +
                            ((fechaContaInicio.HasValue && fechaContaFin.HasValue) ? (" AND eb.est_contabilidad_fecha BETWEEN '" + fechaContaInicio.Value.ToString("yyyMMdd") + "' AND '" + fechaContaFin.Value.ToString("yyyMMdd") + "' ") : "") +
                            "ORDER BY e.ape_paterno",
                        })
                    );
                }

            }
            else
            {
                result = _context.Select<RepBajasDTO>(new DapperDTO
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
                        e.jefe_inmediato AS jefeInmediatoID,
                        (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) AS jefeInmediato,
                        CONVERT( CHAR( 20 ), eb.fechaBaja, 103 ) AS fechaBajaStr,
                        e.contratable AS recontratable,
                        CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS FechaRec,
                        CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) AS fechaAltaStr,
                        e.nss AS nss,
                        eb.est_baja,
                        eb.est_inventario,
                        est_contabilidad,
                        est_compras,
                        eb.est_nominas,
                        eb.est_inventario_comentario,
                        eb.est_compras_comentario,
                        eb.est_contabilidad_comentario,
                        eb.est_nominas_comentario,
                        eb.est_inventario_fecha,
                        eb.est_contabilidad_fecha,
                        eb.est_compras_fecha,
                        eb.est_nominas_fecha,
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
                        eb.comentarios,
                        eb.esAnticipada
                    FROM tblRH_EK_Empleados AS e 
					    LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON e.id_regpat = srp.clave_reg_pat 
					    LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
					    LEFT JOIN tblRH_Baja_Registro AS eb ON e.clave_empleado=eb.numeroEmpleado 
					    LEFT JOIN tblRH_EK_Puestos AS sip ON sip.puesto = eb.idPuesto 
					    LEFT JOIN tblRH_EK_Razones_Baja AS rz ON eb.motivoBajaDeSistema = rz.clave_razon_baja 
					    LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = eb.cc
					WHERE eb.cc in (" + ccs + ") and eb.fechaBaja between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and eb.motivoBajaDeSistema in (" + String.Join(",", concepto) + ") and eb.registroActivo = 1 and eb.est_baja='A'" +
                                "ORDER BY e.ape_paterno",
                });
            }

            var resultU = new List<RepBajasDTO>();

            foreach (var i in result)
            {
                i.fechaBaja = Convert.ToDateTime(i.fechaBajaStr);
                i.recontratable = !string.IsNullOrEmpty(i.recontratable) ? i.recontratable.Equals("S") ? "SI" : "NO" : "";
                //i.fechaAltaBaja = string.IsNullOrEmpty(i.FechaRec) ? i.fechaAltaStr : i.FechaRec;
                //i.fechaAltaBaja = i.fechaAlta.HasValue ? i.fechaAlta.Value.ToShortDateString() : i.fechaAntiguedad.ToShortDateString();
                //i.fechaAltaBaja = i.fechaAntiguedad.ToShortDateString();
                i.fechaAltaBaja = i.fechaIngreso.HasValue ? i.fechaIngreso.Value.ToShortDateString() : i.fechaAlta.HasValue ? i.fechaAlta.Value.ToShortDateString() : i.fechaAntiguedad.ToShortDateString();

                if (i.est_contabilidad == "A" && i.est_compras == "A" && i.est_inventario == "A")
                {
                    i.estatus_baja = 3;
                    i.estatus_bajaDesc = "LIBERADA";
                    i.est_contabilidad_comentario = string.IsNullOrEmpty(i.est_contabilidad_comentario) ? "LIBERADA" : i.est_contabilidad_comentario;
                    i.est_inventario_comentario = string.IsNullOrEmpty(i.est_inventario_comentario) ? "LIBERADA" : i.est_inventario_comentario;
                    i.est_compras_comentario = string.IsNullOrEmpty(i.est_compras_comentario) ? "LIBERADA" : i.est_compras_comentario;
                    i.est_nominas_comentario = string.IsNullOrEmpty(i.est_nominas_comentario) ? "LIBERADA" : i.est_nominas_comentario;

                    var lstFechasLiberacion = new List<DateTime>();

                    if (i.est_compras_fecha.HasValue)
                    {
                        lstFechasLiberacion.Add(i.est_compras_fecha.Value);
                    }

                    if (i.est_contabilidad_fecha.HasValue)
                    {
                        lstFechasLiberacion.Add(i.est_contabilidad_fecha.Value);
                    }

                    if (i.est_inventario_fecha.HasValue)
                    {
                        lstFechasLiberacion.Add(i.est_inventario_fecha.Value);
                    }

                    if (lstFechasLiberacion.Count > 0)
                    {
                        i.fechaLiberacion = lstFechasLiberacion.Max();                        
                    }
                }
                else
                {
                    i.est_contabilidad_comentario = "NO LIBERADA";

                    if (i.est_baja == "A")
                    {
                        i.estatus_baja = 1;
                        i.estatus_bajaDesc = "AUTORIZADA";
                    }
                    else
                    {
                        i.estatus_baja = 2;
                        i.estatus_bajaDesc = "PENDIENTE";
                    }

                    if (i.est_contabilidad == "A")
                    {
                        i.est_contabilidad_comentario = string.IsNullOrEmpty(i.est_contabilidad_comentario) ? "LIBERADA" : i.est_contabilidad_comentario;
                    }
                    else
                    {
                        i.est_inventario_comentario = "NO LIBERADA";
                    }

                    if (i.est_inventario == "A")
                    {
                        i.est_inventario_comentario = string.IsNullOrEmpty(i.est_inventario_comentario) ? "LIBERADA" : i.est_inventario_comentario;
                    }
                    else
                    {
                        i.est_inventario_comentario = "NO LIBERADA";
                    }

                    if (i.est_compras == "A")
                    {
                        i.est_compras_comentario = string.IsNullOrEmpty(i.est_compras_comentario) ? "LIBERADA" : i.est_compras_comentario;
                    }
                    else
                    {
                        i.est_compras_comentario = "NO LIBERADA";
                    }

                    if (i.est_nominas == "A")
                    {
                        i.est_nominas_comentario = string.IsNullOrEmpty(i.est_nominas_comentario) ? "LIBERADA" : i.est_nominas_comentario;
                    }
                    else
                    {
                        i.est_nominas_comentario = "NO LIBERADA";
                    }
                }

                //var objBaja = resultU.Where(e => e.empleadoID == i.empleadoID && e.puestoID == i.puestoID).FirstOrDefault();

                //if (objBaja == null)
                //{

                //    resultU.Add(i);
                //}
                //else
                //{
                //    if (i.fechaBaja != null)
                //    {
                //        if (objBaja.fechaBaja < i.fechaBaja)
                //        {
                //            resultU.Remove(objBaja);
                //            resultU.Add(i);
                //        }
                //    }
                //}

            }

            if (estatus != null && estatus.Count() > 0)
            {
                result = result.Where(x => estatus.Contains(x.estatus_baja)).ToList();
            }

            if (fechaContaInicio.HasValue && fechaContaFin.HasValue)
            {
                result = result.Where(e => e.fechaLiberacion.HasValue && e.fechaLiberacion.Value >= fechaContaInicio.Value.Date && e.fechaLiberacion.Value.Date <= fechaContaFin.Value.Date).ToList();
            }

            if (tipoBajas.HasValue && tipoBajas.Value == 1)
            {
                result = result.Where(e => e.esAnticipada.HasValue && e.esAnticipada.Value).ToList();
            }

            return result.OrderBy(x => x.cC).ThenBy(x => x.empleado).ThenBy(x => x.fechaBaja).ToList();

        }
        public List<RepCambiosDTO> getListaCambios(List<string> cc, List<string> concepto, List<string> empleado, DateTime fechaInicio, DateTime fechaFin)
        {
            var condQuery = @"";
            if (concepto != null)
            {
                condQuery += "AND (";
                foreach (var i in concepto.Select((value, index) => new { index, value }))
                {
                    if (i.index == 0)
                    {
                        if (i.value.Equals("Puesto"))
                            condQuery += "e.CamposCambiados LIKE '%Puesto%'";
                        else if (i.value.Equals("Sueldo"))
                            condQuery += "e.CamposCambiados LIKE '%Sueldo%'";
                        else if (i.value.Equals("Jefe"))
                            condQuery += "e.CamposCambiados LIKE '%Jefe Inmediato%'";
                        else if (i.value.Equals("CC"))
                            condQuery += "e.CamposCambiados LIKE '%CC%'";
                        else if (i.value.Equals("Patronal"))
                            condQuery += "e.CamposCambiados LIKE '%Registro Patronal%'";
                        else if (i.value.Equals("Nomina"))
                            condQuery += "e.CamposCambiados LIKE '%Tipo Nomina%'";
                    }
                    else
                    {
                        if (i.value.Equals("Puesto"))
                            condQuery += " OR e.CamposCambiados LIKE '%Puesto%'";
                        else if (i.value.Equals("Sueldo"))
                            condQuery += " OR e.CamposCambiados LIKE '%Sueldo%'";
                        else if (i.value.Equals("Jefe"))
                            condQuery += " OR e.CamposCambiados LIKE '%Jefe Inmediato%'";
                        else if (i.value.Equals("CC"))
                            condQuery += " OR e.CamposCambiados LIKE '%CC%'";
                        else if (i.value.Equals("Patronal"))
                            condQuery += " OR e.CamposCambiados LIKE '%Registro Patronal%'";
                        else if (i.value.Equals("Nomina"))
                            condQuery += " OR e.CamposCambiados LIKE '%Tipo Nomina%'";
                    }
                }
                condQuery += ")";
            }

            var empQuery = @"";
            if (empleado != null)
            {
                //empQuery = " and e.clave_empleado in (" + getStringInlineArray(empleado) + ") ";
                empQuery = " AND e.clave_empleado in @paramCE ";
            }

            //string query = "SELECT e.cc_nuevo as cC,ne.clave_empleado as empleadoID,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as empleado,CONVERT( CHAR( 20 ), e.fecha_aplicacion, 103 ) as fechaCambioStr,(e.cambio_puesto+','+e.cambio_sueldo+','+e.cambio_jefe_inmediato+','+cambio_cc+','+e.cambio_reg_patronal+','+e.cambio_tipo_nomina) as cambios FROM sn_cambios as e inner join sn_empleados as ne on e.clave_empleado=ne.clave_empleado where ne.cc_contable in (" + getStringInlineArray(cc) + ") and e.fecha_aplicacion between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" + empQuery + condQuery + "";
            //var result = (List<RepCambiosDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<RepCambiosDTO>>();

            var result = _context.Select<RepCambiosDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT
	                            e.CcID AS cC,
	                            ne.clave_empleado AS empleadoID,
	                            (ne.ape_paterno + ' '  +ne.ape_materno + ' ' + ne.nombre) AS empleado,
	                            CONVERT(CHAR(20), e.FechaInicioCambio, 103) AS fechaCambioStr,
	                            (e.CamposCambiados) AS cambios
                            FROM
	                            tblRH_FormatoCambio AS e
                            INNER JOIN
	                            tblRH_EK_Empleados AS ne
	                            ON
		                            e.clave_empleado = ne.clave_empleado
                            WHERE
	                            ne.cc_contable in @paramCCs AND
                                e.FechaInicioCambio BETWEEN @paramFechaInicio AND @paramFechaFin " + empQuery + condQuery,
                parametros = new
                {
                    paramCCs = cc,
                    paramFechaInicio = fechaInicio.ToString("dd/MM/yyyy"),
                    paramFechaFin = fechaFin.ToString("dd/MM/yyyy"),
                    paramCE = empleado
                }
            });

            // e.FechaInicioCambio between @paramFechaInicio and @paramFechaFin" + empQuery + condQuery + "'" + empQuery + condQuery,

            foreach (var i in result)
            {
                //var ar = i.cambios.Split(',');
                //List<string> temp = new List<string>();
                //if (ar[0].Equals("S"))
                //{
                //    temp.Add("Puesto");
                //    i.cPuesto = 1;
                //}
                //if (ar[1].Equals("S"))
                //{
                //    temp.Add("Sueldo");
                //    i.cSueldo = 1;
                //}
                //if (ar[2].Equals("S"))
                //{
                //    temp.Add("Jefe Inmediato");
                //    i.cJeIn = 1;
                //}
                //if (ar[3].Equals("S"))
                //{
                //    temp.Add("CC");
                //    i.cCC = 1;

                //}
                //if (ar[4].Equals("S"))
                //{
                //    temp.Add("Registro Patronal");
                //    i.cRePa = 1;
                //}
                //if (ar[5].Equals("S"))
                //{
                //    temp.Add("Tipo de Nomina");
                //    i.cTiN = 1;
                //}
                //if (string.IsNullOrEmpty(i.cC))
                //{
                //    //var ccOrigin = "SELECT cc_contable as cC FROM sn_empleados WHERE clave_empleado = '" + i.empleadoID + "'";

                //    //var resultCc = (List<RepAltasDTO>)ContextEnKontrolNomina.Where(ccOrigin).ToObject<List<RepAltasDTO>>();

                //    var resultCc = _context.Select<RepAltasDTO>(new DapperDTO 
                //    {
                //        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                //        consulta = @"SELECT cc_contable as cC FROM tblRH_EK_Empleados WHERE clave_empleado = " + i.empleadoID,
                //    });

                //    foreach (var ccc in resultCc)
                //    {
                //        i.cC = ccc.cC;
                //    }
                //}

                //i.cambios = String.Join(",", temp);
                i.fechaCambio = Convert.ToDateTime(i.fechaCambioStr);
            }
            return result;
        }
        public List<RepModificacionesDTO> getListaModificaciones(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin)
        {


            //string query = "SELECT e.cc as cC,e.tipo as concepto,p.descripcion as puesto,e.cantidad as cantidad,CONVERT( CHAR( 20 ), e.fecha_solicita, 103 ) as fechaStr,ne.clave_empleado as solicitaID,(ne.nombre+' '+ne.ape_paterno+' '+ne.ape_materno) as solicita,e.observaciones as observacion FROM sn_plantilla_aditiva as e inner join sn_empleados as ne on e.solicita=ne.clave_empleado inner join si_puestos as p on e.puesto=p.puesto where e.cc in (" + getStringInlineArray(cc) + ") and e.fecha_solicita between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and e.tipo in (" + getStringInlineArray(concepto) + ") and ne.estatus_empleado='A'";
            //string query = " SELECT (e.cc + ' - ' + c.descripcion) as cC,e.tipo as concepto,p.descripcion as puesto,e.cantidad as cantidad,";
            //query += " CONVERT( CHAR( 20 ), e.fecha_solicita, 103 ) as fechaStr,ne.clave_empleado as solicitaID,";
            //query += " (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as solicita,e.observaciones as observacion,";
            //query += " (au.ape_paterno+' '+au.ape_materno+' '+au.nombre) as autoriza";
            //query += " FROM sn_plantilla_aditiva as e ";
            //query += " inner join sn_empleados as ne on e.solicita=ne.clave_empleado ";
            //query += " inner join DBA.cc as c ON c.cc = e.cc";
            //query += " inner join sn_empleados as au on e.autoriza=au.clave_empleado ";
            //query += " inner join si_puestos as p on e.puesto=p.puesto ";
            //query += " where e.cc in (" + getStringInlineArray(cc) + ") and e.fecha_solicita between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'";

            string query = @"SELECT (e.cc + ' - ' + c.ccDescripcion) as cC,e.tipo as concepto,p.descripcion as puesto,ISNULL(tabCat.concepto, 'S/N') as categoria,e.cantidad as cantidad,
                            CONVERT( CHAR( 20 ), e.fecha_solicita, 103 ) as fechaStr,ne.clave_empleado as solicitaID,
                            (ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as solicita,e.observaciones as observacion,
                            (au.ape_paterno+' '+au.ape_materno+' '+au.nombre) as autoriza
                            FROM tblRH_EK_Plantilla_Aditiva as e 
                            inner join tblRH_EK_Empleados as ne on e.solicita =ne.clave_empleado 
                            inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc
                            inner join tblRH_EK_Empleados as au on e.autoriza=au.clave_empleado 
                            inner join tblRH_EK_Puestos as p on e.puesto=p.puesto 
							left join tblRH_REC_Requisicion AS tabReq ON tabReq.id = ne.requisicion
							left join tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = tabReq.idTabuladorDet
							left join tblRH_TAB_CatCategorias AS tabCat ON tabDet.FK_Categoria = tabCat.id
                            where e.cc in (" + getStringInlineArray(cc) + ") and e.fecha_solicita between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'";

            List<string> tipo = new List<string>();
            bool vanderaAjuste = false;
            foreach (var con in concepto)
            {
                if (con.Equals("J"))
                {
                    query += " AND (e.observaciones LIKE '%AJUSTE%'";
                    vanderaAjuste = true;
                }
                else
                {
                    tipo.Add(con);
                }
            }

            if (tipo.Count > 0)
            {
                query += vanderaAjuste ? " or e.tipo in (" + getStringInlineArray(tipo) + "))" : " and e.tipo in (" + getStringInlineArray(tipo) + ")";
            }
            else { query += ")"; }

            //var result = (List<RepModificacionesDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<RepModificacionesDTO>>();

            var result = _context.Select<RepModificacionesDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = query,
            });

            foreach (var i in result)
            {
                i.fecha = Convert.ToDateTime(i.fechaStr);
            }
            return result.OrderBy(x => x.solicita).ToList();
        }
        public string anosMesesDias(System.DateTime fechaInicio, System.DateTime fechaFin)
        {
            int anos = 0;
            int meses = 0;
            int dias = 0;
            int m = 0;

            anos = fechaFin.Year - fechaInicio.Year;
            if (fechaInicio.Month > fechaFin.Month)
            {
                anos = anos - 1;
            }
            if (fechaFin.Month < fechaInicio.Month)
            {
                meses = 12 - fechaInicio.Month + fechaFin.Month;
            }
            else
            {
                meses = fechaFin.Month - fechaInicio.Month;
            }
            if (fechaFin.Day < fechaInicio.Day)
            {
                meses = meses - 1;
                if (fechaFin.Month == fechaInicio.Month)
                {
                    anos = anos - 1;
                    meses = 11;
                }
            }
            dias = fechaFin.Day - fechaInicio.Day;
            if (dias < 0)
            {
                m = Convert.ToInt32(fechaFin.Month) - 1;
                if (m == 0)
                {
                    m = 12;
                }
                switch (m)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        dias = 31 + dias;
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        dias = 30 + dias;
                        break;
                    case 2:
                        if ((fechaFin.Year % 4 == 0 & fechaFin.Year % 100 != 0) | fechaFin.Year % 400 == 0)
                        {
                            dias = 29 + dias;
                        }
                        else
                        {
                            dias = 28 + dias;
                        }
                        break;
                }
            }
            return Convert.ToString(anos) + " años, " + Convert.ToString(meses) + " meses, " + Convert.ToString(dias) + " días ";
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
        public List<RepActivosDTO> getListaActivos(List<string> cc)
        {
            #region MIGRADO
            //string query = "SELECT (e.cc_contable+' - '+c.descripcion) as cC,e.clave_empleado as empleadoID,(e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as empleado,";
            //query += " pu.descripcion as puesto,tn.descripcion as tipo_nomina,e.nss,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,";
            //query += " CONVERT( CHAR( 20 ),(SELECT TOP 1 ser.fecha_reingreso FROM sn_empl_recontratacion as ser where ser.clave_empleado = e.clave_empleado";
            //query += " AND ser.cc = e.cc_contable AND ser.fecha_reingreso > e.fecha_alta ORDER BY ser.fecha_reingreso DESC), 103 ) as fechaRe,";
            //query += " CONVERT( CHAR( 20 ), e.fecha_alta, 103 ) as fechaAltaStr";
            //query += " FROM sn_empleados as e ";
            //query += " inner join sn_empleados as ne on e.jefe_inmediato=ne.clave_empleado ";
            //query += " inner join DBA.cc as c ON c.cc = e.cc_contable";
            //query += " inner join si_puestos as pu on e.puesto = pu.puesto";
            //query += " inner join sn_tipos_nomina as tn on e.tipo_nomina = tn.tipo_nomina";
            //query += " where e.cc_contable in (" + getStringInlineArray(cc) + ") AND e.estatus_empleado = 'A'";

            //var result = (List<RepActivosDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<RepActivosDTO>>();
            #endregion

            var resultFiltered = new List<RepActivosDTO>();

            #region V1
            var result = _context.Select<RepActivosDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,

                consulta = @"SELECT (e.cc_contable+' - '+c.ccDescripcion) as cC,e.clave_empleado as empleadoID,(e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as empleado,
            		                        pu.descripcion as puesto,ISNULL(tabCat.concepto, 'S/N') as categoria,tn.descripcion as tipo_nomina,e.nss,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,
            		                        CONVERT( CHAR( 20 ),(SELECT TOP 1 ser.fecha_reingreso FROM tblRH_EK_Empl_Recontratacion as ser where ser.clave_empleado = e.clave_empleado
            		                        AND ser.cc = e.cc_contable AND ser.fecha_reingreso > e.fecha_alta ORDER BY ser.fecha_reingreso DESC), 103 ) as fechaRe,
            		                        CONVERT( CHAR( 20 ), e.fecha_alta, 103 ) as fechaAltaStr
            		                        FROM tblRH_EK_Empleados as e 
            		                        inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado 
            		                        inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc_contable
            		                        inner join tblRH_EK_Puestos as pu on e.puesto = pu.puesto
            		                        inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
											 left join tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = ne.idTabuladorDet
											 left join tblRH_TAB_CatCategorias AS tabCat ON tabDet.FK_Categoria = tabCat.id 
            		                        where e.cc_contable in (" + getStringInlineArray(cc) + ") AND e.estatus_empleado = 'A'",
            });
            #endregion

            foreach (var i in result)
            {
                i.fechaAltaRe = string.IsNullOrEmpty(i.fechaRe) ? i.fechaAltaStr : i.fechaRe;

                var objAlta = resultFiltered.Where(e => e.empleadoID == i.empleadoID && e.puesto == i.puesto).FirstOrDefault();
                //var lstFamiliares = _context.tblRH_EK_Empl_Familia.Where(e => e.esActivo && e.clave_empleado == i.empleadoID && e.parentesco == 4).ToList();

                //if (lstFamiliares.Count > 0)
                //{
                //    i.hijos.AddRange(lstFamiliares);
                //}

                if (objAlta == null)
                {
                    resultFiltered.Add(i);
                }

                //if (i.tipo_nomina == "SEMANAL")
                //{
                //    i.total_mensual = ((i.salario_base + i.complemento) / 7) * 30.4M;
                //}
                //else
                //{
                //    i.total_mensual = (i.salario_base + i.complemento) * 2;
                //}
            }

            return resultFiltered.OrderBy(x => x.empleado).ToList();
        }

        public Dictionary<string, object> GetRptActivos(List<string> cc)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var permisoGerenciaDireccion = _context.tblP_AccionesVistatblP_Usuario
                    .Any(x =>
                        (
                            x.sistema == 16 &&
                            x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id &&
                            x.tblP_AccionesVista_id == 4036
                        ));

                bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4032).Count()) > 0;
                bool permisoOcultarSalarioByCC = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4038).Count()) > 0;

                var resultFiltered = new List<RepActivosDTO>();

                var result = _context.Select<RepActivosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT 
                                e.cc_contable,
	                            (e.cc_contable+' - '+c.ccDescripcion) as cC,
                                e.clave_empleado as empleadoID,(e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as empleado,
	                            pu.descripcion as puesto, pu.puesto as idPuesto, tn.descripcion as tipo_nomina,e.nss,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,
--	                            CONVERT( CHAR( 20 ),
--		                            (|
--		                            SELECT TOP 1 ser.fecha_reingreso 
--		                            FROM tblRH_EK_Empl_Recontratacion as ser 
--		                            where ser.clave_empleado = e.clave_empleado
--		                            AND ser.cc = e.cc_contable AND ser.fecha_reingreso > e.fecha_alta ORDER BY ser.fecha_reingreso DESC
--		                            ), 103 
--	                            ) as fechaRe,
                                CONVERT(CHAR(20), e.fecha_alta, 103) as fechaAltaStr,
	                            CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as fechaAltaRe,
	                            tDeps.desc_depto as departamento,
	                            e.requisicion,
	                            tRegPat.nombre_corto as regpat,
	                            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as salario_base,
                                (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as complemento,
                                (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as bono_zona,
	                            (tGrales.domicilio + ' #' + tGrales.numero_exterior + ' Col. '+tGrales.colonia) as domicilio,
	                            tEstado.descripcion as nombre_estado_nac,
	                            tCuidad.descripcion as nombre_ciudad_nac,
	                            CONVERT(CHAR(20), e.fecha_nac, 103) as fecha_nac,
	                            tGrales.email,
	                            tGrales.tel_cel,
	                            tGrales.tel_casa,
	                            tGrales.en_accidente_nombre,
	                            tGrales.en_accidente_telefono,
	                            e.sexo,
	                            e.rfc,
	                            e.curp,
	                            tGrales.estado_civil,
	                            --estudios
	                            tGrales.codigo_postal,
	                            tTipoSangre.tipoSangre,
	                            tGrales.alergias,
	                            tTipoCasa.tipoCasa,
	                            tGrales.ocupacion,
	                            (
		                            SELECT COUNT(*) FROM tblRH_EK_Empl_Familia as tFam WHERE e.clave_empleado = tFam.clave_empleado AND tFam.parentesco = 4
	                            ) as numHijos,
	                            (
		                            SELECT TOP 1 apellido_paterno+' '+apellido_materno+' '+nombre FROM tblRH_EK_Empl_Familia as tFam WHERE e.clave_empleado = tFam.clave_empleado AND tFam.parentesco = 3 ORDER BY id DESC
	                            ) as nombreConyuge,
                                tContratos.nombre as contratoDesc,
								tabCat.concepto as descCategoria,
								eCompl.camisa as camisa,
								eCompl.calzado as calzado,
								eCompl.pantalon as pantalon,
                                tGrales.num_dni as dni,
								e.cuspp,
                                (
									select top 1
										ciudadContacto.descripcion
									from
										tblRH_EK_Ciudades as ciudadContacto
									where
										ciudadContacto.clave_cuidad = tGrales.cuidado_dom and
										ciudadContacto.clave_estado = tGrales.estado_dom and
										ciudadContacto.clave_pais = tGrales.pais_dom
								) as ciudadContacto
                            FROM tblRH_EK_Empleados as e 
                            inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado 
                            inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc_contable
                            inner join tblRH_EK_Puestos as pu on e.puesto = pu.puesto
                            inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
                            LEFT JOIN tblRH_EK_Departamentos as tDeps ON e.clave_depto = tDeps.clave_depto
                            LEFT JOIN tblRH_EK_Registros_Patronales as tRegPat ON e.id_regpat = tRegPat.clave_reg_pat
                            LEFT JOIN tblRH_EK_Empl_Grales as tGrales ON e.clave_empleado = tGrales.clave_empleado
                            LEFT JOIN tblRH_EK_Estados as tEstado ON (e.clave_estado_nac = tEstado.clave_estado AND e.clave_pais_nac = tEstado.clave_pais)
                            LEFT JOIN tblRH_EK_Ciudades as tCuidad ON (e.clave_ciudad_nac = tCuidad.clave_cuidad AND e.clave_estado_nac = tCuidad.clave_estado AND e.clave_pais_nac = tCuidad.clave_pais)
                            LEFT JOIN tblP_CatTipoSangre as tTipoSangre ON tTipoSangre.id = tGrales.tipo_sangre
                            LEFT JOIN tblP_CatTipoCasa as tTipoCasa ON tGrales.tipo_casa = tTipoCasa.id
                            LEFT JOIN tblRH_EK_Empl_Duracion_Contrato as tContratos ON e.duracion_contrato = tContratos.clave_duracion
							LEFT JOIN tblRH_REC_Requisicion AS tabReq ON tabReq.id = e.requisicion
							LEFT JOIN tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = tabReq.idTabuladorDet
							LEFT JOIN tblRH_TAB_CatCategorias AS tabCat ON tabDet.FK_Categoria = tabCat.id
						    LEFT JOIN tblRH_EK_Empl_Complementaria AS eCompl ON e.clave_empleado = eCompl.clave_empleado
            		        WHERE e.cc_contable IN @cc AND e.estatus_empleado = 'A'" + (permisoGerenciaDireccion ? "" : " AND (pu.descripcion NOT LIKE '%GERENCIA%' AND pu.descripcion NOT LIKE '%GERENTE%' AND pu.descripcion NOT LIKE '%DIRECCI%' AND pu.descripcion NOT LIKE '%DIRECTOR%')"),
                    parametros = new { cc }
                });


                var listaCCDescripciones = _ccFS_SP.GetCCsNominaFiltrados(result.Select(x => x.cc_contable).ToList());
                var lstOcultarCC = _context.tblRH_REC_OcultarCC.Where(e => e.esActivo).Select(e => e.cc).ToList();

                foreach (var i in result)
                {
                    //i.fechaAltaRe = string.IsNullOrEmpty(i.fechaRe) ? i.fechaAltaStr : i.fechaRe;
                    //i.fechaAltaRe = i.fechaAltaStr;
                    i.cC = "[" + i.cc_contable + "] " + listaCCDescripciones.Where(x => x.cc == i.cc_contable).Select(x => x.descripcion).FirstOrDefault().Trim();

                    var objAlta = resultFiltered.Where(e => e.empleadoID == i.empleadoID && e.puesto == i.puesto).FirstOrDefault();
                    var lstFamiliares = _context.tblRH_EK_Empl_Familia.Where(e => e.esActivo && e.clave_empleado == i.empleadoID && e.parentesco == 4).ToList();

                    if (lstFamiliares.Count > 0)
                    {
                        i.hijxs = lstFamiliares;
                    }
                    else
                    {
                        i.hijxs = new List<tblRH_EK_Empl_Familia>();
                    }

                    if (objAlta == null)
                    {
                        resultFiltered.Add(i);
                    }

                    i.total_nominal = ((i.salario_base + i.complemento + i.bono_zona));

                    if (i.tipo_nomina == "SEMANAL")
                    {
                        i.total_mensual = ((i.salario_base + i.complemento + i.bono_zona) / 7) * 30.4M;

                    }
                    else
                    {
                        i.total_mensual = (i.salario_base + i.complemento + i.bono_zona) * 2;
                    }

                    if (!permisoSueldos)
                    {
                        i.bono_zona = 0;
                        i.complemento = 0;
                        i.salario_base = 0;
                        i.total_mensual = 0;
                        i.total_nominal = 0;
                    }

                    if (permisoOcultarSalarioByCC && lstOcultarCC.Contains(i.cc_contable))
                    {
                        i.bono_zona = 0;
                        i.complemento = 0;
                        i.salario_base = 0;
                        i.total_mensual = 0;
                        i.total_nominal = 0;
                    }
                }

                //return resultFiltered.OrderBy(x => x.empleado).ToList();
                resultado.Add(ITEMS, resultFiltered);
                resultado.Add("empresa", (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? true : false);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public List<LayoutAltasRHDTO> getListaEmpleados(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {

            #region MIGRADO
            //string query = "SELECT ";
            //query += " e.tipo_nomina AS dias , e.clave_empleado AS EMP_TRAB,";
            //query += "CONVERT(CHAR(20) ,e.fecha_antiguedad ,103)AS EMP_ALTA, ";
            //query += "(e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) AS EMP_NOM, ";
            //query += "e.rfc AS EMP_RFC, ";
            //query += "e.curp AS EMP_CURP,";
            //query += "1 AS EMP_CC,";
            //query += "e.cc_contable +' '+c.descripcion AS EMP_DP,";
            //query += "2 AS EMP_SM,";
            //query += "e.sexo AS EMP_SEXO,";
            //query += "e.clave_estado_nac AS EMP_NAC_EF,";
            //query += "CONVERT(CHAR(20), e.fecha_nac,103) AS EMP_NAC_FECHA,";
            //query += "CONVERT(CHAR(20),(CASE WHEN";
            //query += "(SELECT TOP 1 ser.fecha_reingreso ";
            //query += "FROM sn_empl_recontratacion as ser where ser.clave_empleado = e.clave_empleado ";
            //query += "AND ser.cc = e.cc_contable ";
            //query += "AND ser.fecha_reingreso > e.fecha_alta  ";
            //query += "ORDER BY ser.fecha_reingreso DESC) ";
            //query += "IS NULL THEN null ELSE ";
            //query += "(SELECT TOP 1 ser.fecha_reingreso ";
            //query += "FROM sn_empl_recontratacion as ser where ser.clave_empleado = e.clave_empleado ";
            //query += "AND ser.cc = e.cc_contable ";
            //query += "AND ser.fecha_reingreso > e.fecha_alta  ";
            //query += "ORDER BY ser.fecha_reingreso DESC) END) ,103) AS EMP_ULTIMO_REINGRESO,";
            //query += "15 AS EMP_AGUINALDO,";
            //query += "6 AS EMP_VACACIONES,";
            //query += "(case e.tipo_nomina when 1 then 1 when 4 then 2 else 2 end) AS EMP_NOMINA,";
            //query += "1 AS EMP_IMSS_TIPO,";
            //query += "(SELECT top 1 ( case reg_pat when 31 then 5  when 33 then 6 when 24 then 2 when 30 then 4 when 23 then 1 else 1 end) as EMP_SUCURSAL FROM sn_empl_reg_pat_historial where clave_empleado=e.clave_empleado order by fecha_cambio desc),";
            //query += "CASE e.sindicato WHEN 'S' THEN 1 WHEN 'N' THEN 2 ELSE 2 END  AS EMP_CL,";
            //query += "1 AS EMP_TURNO,";
            //query += "e.puesto AS EMP_PUESTO,";
            //query += "pu.descripcion as EMP_PUESTO_DESCRIPCION,";
            //query += "e.nss AS EMP_NSS,";
            //query += "e.unidad_medica AS EMP_UMF,";
            //query += "CONVERT(CHAR(20),(CASE WHEN";
            //query += "(SELECT TOP 1 ser.fecha_reingreso ";
            //query += "FROM sn_empl_recontratacion as ser where ser.clave_empleado = e.clave_empleado ";
            //query += "AND ser.cc = e.cc_contable ";
            //query += "AND ser.fecha_reingreso > e.fecha_alta  ";
            //query += "ORDER BY ser.fecha_reingreso DESC) ";
            //query += "IS NULL THEN e.fecha_alta ELSE ";
            //query += "(SELECT TOP 1 ser.fecha_reingreso ";
            //query += "FROM sn_empl_recontratacion as ser where ser.clave_empleado = e.clave_empleado ";
            //query += "AND ser.cc = e.cc_contable ";
            //query += "AND ser.fecha_reingreso > e.fecha_alta  ";
            //query += "ORDER BY ser.fecha_reingreso DESC) END) ,103) AS EMP_FSUELDO,";
            //query += "(SELECT top 1 salario_base FROM sn_tabulador_historial TBH WHERE TBH.clave_empleado =  e.clave_empleado ORDER BY id desc) AS EMP_SUELDO,";
            //query += "(SELECT top 1 complemento FROM sn_tabulador_historial TBH WHERE TBH.clave_empleado =  e.clave_empleado ORDER BY id desc) AS EMP_SUELDO1,";
            //query += "(SELECT top 1 bono_zona FROM sn_tabulador_historial TBH WHERE TBH.clave_empleado =  e.clave_empleado ORDER BY id desc) AS EMP_SUELDO2,";
            //query += "'' AS EMP_SDI, ";
            //query += "CASE empg.tel_casa WHEN NULL THEN '' ELSE empg.tel_casa END AS EMP_DIR_TELEFONO,  ";
            //query += "CASE empg.tel_cel WHEN NULL THEN '' ELSE empg.tel_cel END AS EMP_DIR_CELULAR,  ";
            //query += "empg.domicilio AS EMP_DIR_CALLE,empg.numero_exterior AS EMP_DIR_NO,empg.colonia AS EMP_DIR_COLONIA,empg.codigo_postal AS EMP_DIR_CP,";
            //query += "cd.descripcion AS EMP_DIR_MUNICIPIO,cd.descripcion AS EMP_DIR_POBLACION, est.descripcion AS EMP_DIR_ESTADO,";
            //query += "(empg.nombre_ben+'/'+empg.paterno_ben+'/'+empg.materno_ben) AS NOM_BENEF,(select top 1 SNP.descripcion FROM sn_parentesco SNP where SNP.id = empg.parentesco_ben) AS PARENT_BENEF, empg.fecha_nac_ben AS BENEF_NAC_FECHA ";
            //query += "FROM sn_empleados AS e ";
            //query += "INNER JOIN DBA.cc AS c ON c.cc = e.cc_contable  ";
            //query += "INNER JOIN si_puestos AS pu ON e.puesto = pu.puesto  ";
            //query += "INNER JOIN sn_tipos_nomina AS tn ON e.tipo_nomina = tn.tipo_nomina  ";
            //query += "INNER JOIN sn_departamentos AS dp ON dp.clave_depto = e.clave_depto ";
            //query += "INNER JOIN sn_empl_grales AS empg ON empg.clave_empleado = e.clave_empleado  ";
            //query += "INNER JOIN sn_estados AS est ON (est.clave_estado = empg.estado_dom and est.clave_pais=e.clave_pais_nac) ";
            //query += "INNER JOIN sn_ciudades AS cd ON (cd.clave_ciudad = empg.ciudad_dom and cd.clave_pais=e.clave_pais_nac and cd.clave_estado=empg.estado_dom) ";
            //query += "WHERE e.cc_contable in (" + getStringInlineArray(cc) + ") AND e.estatus_empleado = 'A'  AND ";
            //query += "CASE WHEN EMP_ULTIMO_REINGRESO IS NULL THEN  convert(datetime, EMP_ALTA , 103) ELSE  convert(datetime, EMP_ULTIMO_REINGRESO , 103) END ";
            //query += "BETWEEN '" + fechaInicio.ToString("yyyyMMdd") + "' AND '" + fechaFin.ToString("yyyyMMdd") + "';";


            //var result = (List<LayoutAltasRHDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<LayoutAltasRHDTO>>();
            #endregion

            bool permisoGerencia = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4046).Count()) > 0;
            bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4047).Count()) > 0;

            var resultFiltered = new List<LayoutAltasRHDTO>();

            var result = _context.Select<LayoutAltasRHDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT 
                                e.tipo_nomina AS dias , e.clave_empleado AS EMP_TRAB,
                                fecha_antiguedad,
                                --CONVERT(CHAR(20) ,e.fecha_antiguedad ,103)AS EMP_ALTA,
                                (e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) AS EMP_NOM, 
                                e.rfc AS EMP_RFC, 
                                e.curp AS EMP_CURP,
                                1 AS EMP_CC,
                                e.cc_contable + ' ' + c.ccDescripcion AS EMP_DP,
                                2 AS EMP_SM,
                                e.sexo AS EMP_SEXO,
                                e.clave_estado_nac AS EMP_NAC_EF,
                                CONVERT(CHAR(20), e.fecha_nac,103) AS EMP_NAC_FECHA,
                                CONVERT(CHAR(20),(CASE WHEN
                                (SELECT TOP 1 ser.fecha_reingreso 
                                FROM tblRH_EK_Empl_Recontratacion as ser where ser.clave_empleado = e.clave_empleado 
                                AND ser.cc = e.cc_contable 
                                AND ser.fecha_reingreso > e.fecha_alta  
                                ORDER BY ser.fecha_reingreso DESC) 
                                IS NULL THEN null ELSE 
                                (SELECT TOP 1 ser.fecha_reingreso 
                                FROM tblRH_EK_Empl_Recontratacion as ser where ser.clave_empleado = e.clave_empleado 
                                AND ser.cc = e.cc_contable 
                                AND ser.fecha_reingreso > e.fecha_alta  
                                ORDER BY ser.fecha_reingreso DESC) END) ,103) AS EMP_ULTIMO_REINGRESO,
                                15 AS EMP_AGUINALDO,
                                6 AS EMP_VACACIONES,
                                (case e.tipo_nomina when 1 then 1 when 4 then 2 else 2 end) AS EMP_NOMINA,
                                1 AS EMP_IMSS_TIPO,
                                (SELECT top 1 ( case reg_pat when 31 then 5  when 33 then 6 when 24 then 2 when 30 then 4 when 23 then 1 else 1 end) as EMP_SUCURSAL FROM tblRH_EK_Empl_Reg_Pat_Historial where clave_empleado=e.clave_empleado order by fecha_cambio desc),
                                CASE e.sindicato WHEN 'S' THEN 1 WHEN 'N' THEN 2 ELSE 2 END  AS EMP_CL,
                                1 AS EMP_TURNO,
                                e.puesto AS EMP_PUESTO,
                                pu.descripcion as EMP_PUESTO_DESCRIPCION,
                                e.nss AS EMP_NSS,
                                e.unidad_medica AS EMP_UMF,
                                CONVERT(CHAR(20),(CASE WHEN
                                (SELECT TOP 1 ser.fecha_reingreso 
                                FROM tblRH_EK_Empl_Recontratacion as ser where ser.clave_empleado = e.clave_empleado 
                                AND ser.cc = e.cc_contable 
                                AND ser.fecha_reingreso > e.fecha_alta  
                                ORDER BY ser.fecha_reingreso DESC) 
                                IS NULL THEN e.fecha_alta ELSE 
                                (SELECT TOP 1 ser.fecha_reingreso 
                                FROM tblRH_EK_Empl_Recontratacion as ser where ser.clave_empleado = e.clave_empleado 
                                AND ser.cc = e.cc_contable 
                                AND ser.fecha_reingreso > e.fecha_alta  
                                ORDER BY ser.fecha_reingreso DESC) END) ,103) AS EMP_FSUELDO,
                                (SELECT top 1 salario_base FROM tblRH_EK_Tabulador_Historial TBH WHERE TBH.clave_empleado =  e.clave_empleado ORDER BY id desc) AS EMP_SUELDO,
                                (SELECT top 1 complemento FROM tblRH_EK_Tabulador_Historial TBH WHERE TBH.clave_empleado =  e.clave_empleado ORDER BY id desc) AS EMP_SUELDO1,
                                (SELECT top 1 bono_zona FROM tblRH_EK_Tabulador_Historial TBH WHERE TBH.clave_empleado =  e.clave_empleado ORDER BY id desc) AS EMP_SUELDO2,
                                '' AS EMP_SDI, 
                                CASE empg.tel_casa WHEN NULL THEN '' ELSE empg.tel_casa END AS EMP_DIR_TELEFONO,  
                                CASE empg.tel_cel WHEN NULL THEN '' ELSE empg.tel_cel END AS EMP_DIR_CELULAR,  
                                empg.domicilio AS EMP_DIR_CALLE,empg.numero_exterior AS EMP_DIR_NO,empg.colonia AS EMP_DIR_COLONIA,empg.codigo_postal AS EMP_DIR_CP,
                                cd.descripcion AS EMP_DIR_MUNICIPIO,cd.descripcion AS EMP_DIR_POBLACION, est.descripcion AS EMP_DIR_ESTADO,
                                (empg.nombre_ben+'/'+empg.parterno_ben+'/'+empg.materno_ben) AS NOM_BENEF,(select top 1 SNP.descripcion FROM tblRH_EK_Parentesco SNP where SNP.id = empg.parentesco_ben) AS PARENT_BENEF, empg.fecha_nac_ben AS BENEF_NAC_FECHA,
                                empg.email as EMP_MAIL,
                                e.CPCIF as AP 
                                FROM tblRH_EK_Empleados AS e 
                                INNER JOIN tblC_Nom_CatalogoCC AS c ON c.cc = e.cc_contable  
                                INNER JOIN tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto  
                                INNER JOIN tblRH_EK_Tipos_Nomina AS tn ON e.tipo_nomina = tn.tipo_nomina  
                                INNER JOIN tblRH_EK_Departamentos AS dp ON dp.clave_depto = e.clave_depto 
                                INNER JOIN tblRH_EK_Empl_Grales AS empg ON empg.clave_empleado = e.clave_empleado  
                                INNER JOIN tblRH_EK_Estados AS est ON (est.clave_estado = empg.estado_dom and est.clave_pais=empg.pais_dom) 
                                INNER JOIN tblRH_EK_Ciudades AS cd ON (cd.clave_cuidad = empg.cuidado_dom and cd.clave_pais=est.clave_pais and cd.clave_estado=empg.estado_dom) 
                                WHERE e.cc_contable in (" + getStringInlineArray(cc) + ") AND e.estatus_empleado = 'A'" +
                                    (permisoGerencia ? "" : " AND (pu.descripcion NOT LIKE '%GERENCIA%' AND pu.descripcion NOT LIKE '%GERENTE%' AND pu.descripcion NOT LIKE '%DIRECCI%' AND pu.descripcion NOT LIKE '%DIRECTOR%')"),
                //" AND CASE WHEN EMP_ULTIMO_REINGRESO IS NULL THEN  convert(datetime, EMP_ALTA , 103) ELSE  convert(datetime, EMP_ULTIMO_REINGRESO , 103) END " +
                //"BETWEEN '" + fechaInicio.ToString("yyyyMMdd") + "' AND '" + fechaFin.ToString("yyyyMMdd") + "';",
            });

            foreach (var item in result)
            {
                item.EMP_ALTA = item.fecha_antiguedad.ToString("dd/MM/yyyy");
                if (item.EMP_ULTIMO_REINGRESO == null)
                {
                    item.EMP_ULTIMO_REINGRESO = item.EMP_ALTA;
                }

                if (!permisoGerencia)
                {
                    item.EMP_SUELDO = "0";
                    item.EMP_SUELDO1 = "0";
                    item.EMP_SUELDO2 = "0";
                }

                if (item.EMP_ALTA != null)
                {
                    DateTime fechaReingreso = new DateTime(Convert.ToInt32(item.EMP_ULTIMO_REINGRESO.Substring(6, 4)), Convert.ToInt32(item.EMP_ULTIMO_REINGRESO.Substring(3, 2)), Convert.ToInt32(item.EMP_ULTIMO_REINGRESO.Substring(0, 2)));
                    if (fechaReingreso <= fechaFin && fechaReingreso >= fechaInicio)
                    {
                        var objAlta = resultFiltered.Where(e => e.EMP_TRAB == item.EMP_TRAB && e.EMP_PUESTO == item.EMP_PUESTO).FirstOrDefault();

                        if (objAlta == null)
                        {
                            resultFiltered.Add(item);
                        }
                    }
                }
            }

            //foreach (var i in result)
            //{
            //    i.fechaAltaRe = string.IsNullOrEmpty(i.fechaRe) ? i.fechaAltaStr : i.fechaRe;
            //}

            return resultFiltered.OrderBy(x => x.EMP_NOM).ToList();
        }
        public List<LayoutIncidenciasRHDTO> getListaEmpleadosIncidencias(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            //string query = "SELECT ";
            //query += "e.cc_contable AS EMP_CC,";
            //query += "e.clave_empleado AS EMP_CLAVE,";
            //query += "(e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) AS EMP_NOM,";
            //query += "SUM(";
            //query += "(case when imd.dia1=2 then 1 else 0 end)+";
            //query += "(case when imd.dia2=2 then 1 else 0 end)+";
            //query += "(case when imd.dia3=2 then 1 else 0 end)+";
            //query += "(case when imd.dia4=2 then 1 else 0 end)+";
            //query += "(case when imd.dia5=2 then 1 else 0 end)+";
            //query += "(case when imd.dia6=2 then 1 else 0 end)+";
            //query += "(case when imd.dia7=2 then 1 else 0 end)+";
            //query += "(case when imd.dia8=2 then 1 else 0 end)+";
            //query += "(case when imd.dia9=2 then 1 else 0 end)+";
            //query += "(case when imd.dia10=2 then 1 else 0 end)+";
            //query += "(case when imd.dia11=2 then 1 else 0 end)+";
            //query += "(case when imd.dia12=2 then 1 else 0 end)+";
            //query += "(case when imd.dia13=2 then 1 else 0 end)+";
            //query += "(case when imd.dia14=2 then 1 else 0 end)+";
            //query += "(case when imd.dia15=2 then 1 else 0 end)+";
            //query += "(case when imd.dia16=2 then 1 else 0 end)";
            //query += ") AS EMP_FALTAS ";
            //query += "FROM sn_empleados AS e";
            //query += " inner join sn_incidencias_empl_det as imd on e.clave_empleado=imd.clave_empleado";
            //query += " inner join sn_incidencias_empl as im on imd.id_incidencia=im.id_incidencia";
            //query += " inner join sn_periodos as p on (im.anio=p.year and im.periodo=p.periodo and im.tipo_nomina=p.tipo_nomina)";
            //query += " WHERE e.cc_contable in (" + getStringInlineArray(cc) + ") AND e.estatus_empleado = 'A'  AND ";
            //query += " (imd.dia1=2 or imd.dia2=2 or imd.dia3=2 or imd.dia4=2 or imd.dia5=2 or imd.dia6=2 or imd.dia7=2 or imd.dia8=2 or imd.dia9=2 or imd.dia10=2 or imd.dia11=2 or imd.dia12=2 or imd.dia13=2 or imd.dia14=2 or imd.dia15=2 or imd.dia16=2)";
            //query += " and p.fecha_inicial>='" + fechaInicio.ToString("yyyyMMdd") + "' AND p.fecha_final<='" + fechaFin.ToString("yyyyMMdd") + "'";
            //query += " group by e.cc_contable,e.clave_empleado,(e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) order by e.cc_contable,e.clave_empleado;";

            //var result = (List<LayoutIncidenciasRHDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<LayoutIncidenciasRHDTO>>();

            var result = _context.Select<LayoutIncidenciasRHDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT 
                                e.cc_contable AS EMP_CC,
                                e.clave_empleado AS EMP_CLAVE,
                                (e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) AS EMP_NOM,
                                SUM(
                                (case when imd.dia1=2 then 1 else 0 end)+
                                (case when imd.dia2=2 then 1 else 0 end)+
                                (case when imd.dia3=2 then 1 else 0 end)+
                                (case when imd.dia4=2 then 1 else 0 end)+
                                (case when imd.dia5=2 then 1 else 0 end)+
                                (case when imd.dia6=2 then 1 else 0 end)+
                                (case when imd.dia7=2 then 1 else 0 end)+
                                (case when imd.dia8=2 then 1 else 0 end)+
                                (case when imd.dia9=2 then 1 else 0 end)+
                                (case when imd.dia10=2 then 1 else 0 end)+
                                (case when imd.dia11=2 then 1 else 0 end)+
                                (case when imd.dia12=2 then 1 else 0 end)+
                                (case when imd.dia13=2 then 1 else 0 end)+
                                (case when imd.dia14=2 then 1 else 0 end)+
                                (case when imd.dia15=2 then 1 else 0 end)+
                                (case when imd.dia16=2 then 1 else 0 end)
                                ) AS EMP_FALTAS 
                                FROM tblRH_EK_Empleados AS e
                                 inner join tblRH_BN_Incidencia_det as imd on e.clave_empleado=imd.clave_empleado
                                 inner join tblRH_BN_Incidencia as im on imd.id_incidencia=im.id_incidencia
                                 inner join tblRH_EK_Periodos as p on (im.anio=p.year and im.periodo=p.periodo and im.tipo_nomina=p.tipo_nomina)
                                 WHERE --e.cc_contable in (" + getStringInlineArray(cc) + ") AND e.estatus_empleado = 'A'  AND " +
                                 "(imd.dia1=2 or imd.dia2=2 or imd.dia3=2 or imd.dia4=2 or imd.dia5=2 or imd.dia6=2 or imd.dia7=2 or imd.dia8=2 or imd.dia9=2 or imd.dia10=2 or imd.dia11=2 or imd.dia12=2 or imd.dia13=2 or imd.dia14=2 or imd.dia15=2 or imd.dia16=2)" +
                                 "and p.fecha_inicial>='" + fechaInicio.ToString("yyyyMMdd") + "' AND p.fecha_final<='" + fechaFin.ToString("yyyyMMdd") + "'" +
                                 "group by e.cc_contable,e.clave_empleado,(e.ape_paterno+'/'+e.ape_materno+'/'+e.nombre) order by e.cc_contable,e.clave_empleado;"
            });

            return result.OrderBy(x => x.EMP_NOM).ToList();
        }
        public bool setUsuariosBaja(tblRH_LayautBajaEmpleados obj)
        {

            if (!Exists(obj.empleadoID))
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.LAYAUTBAJARH);
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.LAYAUTBAJARH);
                }
                return true;
            }
            else
            {
                SaveEntity(obj, (int)BitacoraEnum.LAYAUTBAJARH);
                return true;
            }

        }
        public bool Exists(string id)
        {
            return _context.tblRH_LayautBajaEmpleados.Where(x => x.empleadoID.Equals(id)).ToList().Count > 0 ? true : false;
        }

        #region REPORTE GENERAL
        public Dictionary<string, object> GetRptGeneral(List<string> cc)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                //var permisoGerenciaDireccion = _context.tblP_AccionesVistatblP_Usuario
                //    .Any(x =>
                //        (
                //            x.sistema == 16 &&
                //            x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id &&
                //            x.tblP_AccionesVista_id == 4036
                //        ));

                //bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4032).Count()) > 0;
                //bool permisoOcultarSalarioByCC = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4038).Count()) > 0;

                var resultFiltered = new List<RepActivosDTO>();

                var result = _context.Select<RepActivosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT 
                                e.cc_contable,
	                            (e.cc_contable+' - '+c.ccDescripcion) as cC,
                                e.clave_empleado as empleadoID,(e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as empleado,
	                            pu.descripcion as puesto, pu.puesto as idPuesto, tn.descripcion as tipo_nomina,e.nss,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,
--	                            CONVERT( CHAR( 20 ),
--		                            (|
--		                            SELECT TOP 1 ser.fecha_reingreso 
--		                            FROM tblRH_EK_Empl_Recontratacion as ser 
--		                            where ser.clave_empleado = e.clave_empleado
--		                            AND ser.cc = e.cc_contable AND ser.fecha_reingreso > e.fecha_alta ORDER BY ser.fecha_reingreso DESC
--		                            ), 103 
--	                            ) as fechaRe,
                                CONVERT(CHAR(20), e.fecha_alta, 103) as fechaAltaStr,
	                            CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as fechaAltaRe,
	                            tDeps.desc_depto as departamento,
	                            e.requisicion,
	                            tRegPat.nombre_corto as regpat,
	                            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as salario_base,
                                (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as complemento,
                                (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as bono_zona,
	                            (tGrales.domicilio + ' #' + tGrales.numero_exterior + ' Col. '+tGrales.colonia) as domicilio,
	                            tEstado.descripcion as nombre_estado_nac,
	                            tCuidad.descripcion as nombre_ciudad_nac,
	                            CONVERT(CHAR(20), e.fecha_nac, 103) as fecha_nac,
	                            tGrales.email,
	                            tGrales.tel_cel,
	                            tGrales.tel_casa,
	                            tGrales.en_accidente_nombre,
	                            tGrales.en_accidente_telefono,
	                            e.sexo,
	                            e.rfc,
	                            e.curp,
	                            tGrales.estado_civil,
	                            --estudios
	                            tGrales.codigo_postal,
	                            tTipoSangre.tipoSangre,
	                            tGrales.alergias,
	                            tTipoCasa.tipoCasa,
	                            tGrales.ocupacion,
	                            (
		                            SELECT COUNT(*) FROM tblRH_EK_Empl_Familia as tFam WHERE e.clave_empleado = tFam.clave_empleado AND tFam.parentesco = 4
	                            ) as numHijos,
	                            (
		                            SELECT TOP 1 apellido_paterno+' '+apellido_materno+' '+nombre FROM tblRH_EK_Empl_Familia as tFam WHERE e.clave_empleado = tFam.clave_empleado AND tFam.parentesco = 3 ORDER BY id DESC
	                            ) as nombreConyuge,
                                tContratos.nombre as contratoDesc,
								tabCat.concepto as descCategoria,
								eCompl.camisa as camisa,
								eCompl.calzado as calzado,
								eCompl.pantalon as pantalon,
                                tGrales.num_dni as dni,
								e.cuspp,
                                (
									select top 1
										ciudadContacto.descripcion
									from
										tblRH_EK_Ciudades as ciudadContacto
									where
										ciudadContacto.clave_cuidad = tGrales.cuidado_dom and
										ciudadContacto.clave_estado = tGrales.estado_dom and
										ciudadContacto.clave_pais = tGrales.pais_dom
								) as ciudadContacto,
                                e.estatus_empleado
                            FROM tblRH_EK_Empleados as e 
                            inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado 
                            inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc_contable
                            inner join tblRH_EK_Puestos as pu on e.puesto = pu.puesto
                            inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
                            LEFT JOIN tblRH_EK_Departamentos as tDeps ON e.clave_depto = tDeps.clave_depto
                            LEFT JOIN tblRH_EK_Registros_Patronales as tRegPat ON e.id_regpat = tRegPat.clave_reg_pat
                            LEFT JOIN tblRH_EK_Empl_Grales as tGrales ON e.clave_empleado = tGrales.clave_empleado
                            LEFT JOIN tblRH_EK_Estados as tEstado ON (e.clave_estado_nac = tEstado.clave_estado AND e.clave_pais_nac = tEstado.clave_pais)
                            LEFT JOIN tblRH_EK_Ciudades as tCuidad ON (e.clave_ciudad_nac = tCuidad.clave_cuidad AND e.clave_estado_nac = tCuidad.clave_estado AND e.clave_pais_nac = tCuidad.clave_pais)
                            LEFT JOIN tblP_CatTipoSangre as tTipoSangre ON tTipoSangre.id = tGrales.tipo_sangre
                            LEFT JOIN tblP_CatTipoCasa as tTipoCasa ON tGrales.tipo_casa = tTipoCasa.id
                            LEFT JOIN tblRH_EK_Empl_Duracion_Contrato as tContratos ON e.duracion_contrato = tContratos.clave_duracion
							LEFT JOIN tblRH_REC_Requisicion AS tabReq ON tabReq.id = e.requisicion
							LEFT JOIN tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = tabReq.idTabuladorDet
							LEFT JOIN tblRH_TAB_CatCategorias AS tabCat ON tabDet.FK_Categoria = tabCat.id
						    LEFT JOIN tblRH_EK_Empl_Complementaria AS eCompl ON e.clave_empleado = eCompl.clave_empleado
                            WHERE e.cc_contable IN @cc AND e.esActivo = 1 AND (e.estatus_empleado = 'A' OR e.estatus_empleado = 'B')",
                    parametros = new { cc }
                });

                //"WHERE e.cc_contable IN @cc AND e.estatus_empleado = 'A'" + (permisoGerenciaDireccion ? "" : " AND (pu.descripcion NOT LIKE '%GERENCIA%' AND pu.descripcion NOT LIKE '%GERENTE%' AND pu.descripcion NOT LIKE '%DIRECCI%' AND pu.descripcion NOT LIKE '%DIRECTOR%')"),"


                var listaCCDescripciones = _ccFS_SP.GetCCsNominaFiltrados(result.Select(x => x.cc_contable).ToList());

                foreach (var i in result)
                {
                    //i.fechaAltaRe = string.IsNullOrEmpty(i.fechaRe) ? i.fechaAltaStr : i.fechaRe;
                    //i.fechaAltaRe = i.fechaAltaStr;
                    i.cC = "[" + i.cc_contable + "] " + listaCCDescripciones.Where(x => x.cc == i.cc_contable).Select(x => x.descripcion).FirstOrDefault().Trim();

                    var objAlta = resultFiltered.Where(e => e.empleadoID == i.empleadoID && e.puesto == i.puesto).FirstOrDefault();
                    

                    if (objAlta == null)
                    {
                        resultFiltered.Add(i);
                    }

                    i.total_nominal = ((i.salario_base + i.complemento + i.bono_zona));

                    if (i.tipo_nomina == "SEMANAL")
                    {
                        i.total_mensual = ((i.salario_base + i.complemento + i.bono_zona) / 7) * 30.4M;

                    }
                    else
                    {
                        i.total_mensual = (i.salario_base + i.complemento + i.bono_zona) * 2;
                    }

                    //if (permisoSueldos)
                    //{
                    //    i.bono_zona = 0;
                    //    i.complemento = 0;
                    //    i.salario_base = 0;
                    //    i.total_mensual = 0;
                    //    i.total_nominal = 0;
                    //}

                    //if (permisoOcultarSalarioByCC && lstOcultarCC.Contains(i.cc_contable))
                    //{
                    //    i.bono_zona = 0;
                    //    i.complemento = 0;
                    //    i.salario_base = 0;
                    //    i.total_mensual = 0;
                    //    i.total_nominal = 0;
                    //}
                }

                //return resultFiltered.OrderBy(x => x.empleado).ToList();
                resultado.Add(ITEMS, resultFiltered);
                resultado.Add("empresa", (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? true : false);
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

        #region FILL COMBO
        public List<ComboDTO> CatIncidencia()
        {
            //string Query = "SELECT id, concepto FROM DBA.sn_incidencias_conceptos";

            //var resultado = (IList<CatIncidencias>)ContextEnKontrolNomina.Where(Query).ToObject<IList<CatIncidencias>>();

            //List<CatIncidencias> lista = new List<CatIncidencias>();

            //foreach (var item in resultado)
            //{
            //    lista.Add(item);

            //}

            //return lista;

            return _context.tblRH_EK_Incidencias_Conceptos.Select(e => new ComboDTO
            {
                Value = e.id.ToString(),
                Text = e.concepto
            }).ToList();
        }
        #endregion

        #region RPT STAFFING GUIDE
        public Dictionary<string, object> GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto)
        {

            //resultado.Clear();

            var resultado = new Dictionary<string, object>();
            try
            {
                #region SE VERIFICA QUE EMPRESA SE ENCUENTRA LOGUEADA PARA PREPARAR EL CONTEXT DE EK
                EnkontrolAmbienteEnum idEmpresa = (int)EmpresaEnum.Construplan == (int)vSesiones.sesionEmpresaActual ? EnkontrolAmbienteEnum.Rh : EnkontrolAmbienteEnum.RhArre;
                #endregion

                //CODIGO ANTERIOR FUNCIONAL
                //#region SE OBTIENE id_plantilla EN BASE AL CC
                //int idPlantilla = 0;
                //if (!string.IsNullOrEmpty(_cc))
                //{
                //    //string strQuery = string.Empty;
                //    //strQuery = @"SELECT id_plantilla FROM sn_plantilla_personal WHERE cc = '{0}'";
                //    //var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //    //odbc.consulta = String.Format(strQuery, _cc);
                //    //List<dynamic> lstPlantillas = _contextEnkontrol.Select<dynamic>(idEmpresa, odbc);
                //    var lstPlantillas = _context.tblRH_EK_Plantilla_Personal.Where(x => x.cc == _cc).ToList();
                //    if (lstPlantillas.Count() > 0)
                //        idPlantilla = lstPlantillas[0].id_plantilla;
                //}
                //#endregion

                //var lstPuestos = _context.tblRH_EK_Plantilla_Puesto.Where(x => x.id_plantilla == idPlantilla && !x.virtualPuesto.descripcion.Contains("NO USA")).Select(x => new
                //{
                //    puesto = x.puesto,
                //    descripcion = x.virtualPuesto.descripcion,
                //    tipo_nomina = x.virtualPuesto.FK_TipoNomina,
                //    addPlantilla = false,
                //}).ToList();
                //FIN CODIGO ANTERIOR FUNCIONAL

                //NUEVO CODIGO PARA TABULADORES
                var idPlantilla = 0;
                if (!string.IsNullOrEmpty(_cc))
                {
                    var plantilla = _context.tblRH_TAB_PlantillasPersonal.FirstOrDefault(x => x.cc == _cc && x.registroActivo);
                    if (plantilla != null)
                    {
                        idPlantilla = plantilla.id;
                    }
                }

                var lstPuestos = _context.tblRH_TAB_PlantillasPersonalDet.Where(x => x.FK_Plantilla == idPlantilla && x.registroActivo).Select(x => new
                {
                    puesto = x.FK_Puesto,
                    descripcion = x.puesto.descripcion,
                    tipo_nomina = x.puesto.FK_TipoNomina,
                    cantidad = x.personalNecesario,
                    addPlantilla = false
                }).ToList();
                //FIN CODIGO NUEVO PARA TABULADORES

                List<Core.DTO.RecursosHumanos.Enkontrol.CatCategoriasAditivas> lstDataPuestos = new List<Core.DTO.RecursosHumanos.Enkontrol.CatCategoriasAditivas>();
                List<string> lstTotalesPuestos = new List<string>();

                int totalPlantilla = 0;
                int totalContratados = 0;
                int totalXContratar = 0;
                decimal porcContratado = 0;
                decimal porcXContratar = 0;

                foreach (var item in lstPuestos)
                {

                    Core.DTO.RecursosHumanos.Enkontrol.CatCategoriasAditivas objAditivaPersonal = new Core.DTO.RecursosHumanos.Enkontrol.CatCategoriasAditivas();

                    objAditivaPersonal.id_puesto = item.puesto;
                    objAditivaPersonal.puesto = item.descripcion;
                    int id_plantilla = idPlantilla;

                    //ANTERIOR
                    //var result = _context.Select<Core.DTO.RecursosHumanos.AditivaPersonal>(new DapperDTO
                    //{
                    //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    //    consulta = "SELECT * FROM tblRH_EK_Plantilla_Puesto AS a INNER JOIN tblRH_EK_Puestos AS b ON a.puesto = b.puesto WHERE id_plantilla = @paramPlantilla AND b.descripcion = @paramDesc",
                    //    parametros = new { paramPlantilla = id_plantilla, paramDesc = item.descripcion }
                    //}).ToList();

                    //foreach (var itemPPuesto in result)
                    //{
                    //    objAditivaPersonal.cantidad = itemPPuesto.cantidad;
                    //    //objAditivaPersonal.puesto = itemPPuesto.puesto;
                    //    objAditivaPersonal.id_plantilla = itemPPuesto.id_plantilla;
                    //}
                    //ANTERIOR FIN

                    //NUEVO
                    objAditivaPersonal.cantidad = item.cantidad;
                    //NUEVO FIN

                    var result2 = _context.Select<Core.DTO.RecursosHumanos.AditivaPersonal>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = "SELECT * FROM tblRH_EK_Empleados WHERE puesto = @paramPuesto AND cc_contable = @paramCC AND estatus_empleado = 'A'",
                        parametros = new { paramPuesto = item.puesto, paramCC = _cc }
                    }).ToList();
                    objAditivaPersonal.altas = result2.Count;

                    //ANTERIOR
                    ////var result3 = (IList<AditivaPersonal>)ContextEnKontrolNomina.Where(cantidad).ToObject<IList<AditivaPersonal>>();
                    //var result3 = _context.Select<Core.DTO.RecursosHumanos.AditivaPersonal>(new DapperDTO
                    //{
                    //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    //    consulta = "SELECT * FROM tblRH_EK_Plantilla_Aditiva WHERE puesto = @paramPuesto AND id_plantilla = @paramPlantilla AND estatus = 'A'",
                    //    parametros = new { paramPuesto = objAditivaPersonal.id_puesto, paramPlantilla = objAditivaPersonal.id_plantilla }
                    //}).ToList();
                    //int contador = 0;
                    //foreach (var itemPAditiva in result3)
                    //{
                    //    //objAditivaPersonal.altas = item.altas;
                    //    contador = itemPAditiva.cantidad + contador;

                    //}

                    //objAditivaPersonal.cantidad = (objAditivaPersonal.cantidad + contador);
                    //ANTERIOR FIN

                    //
                    var aditivas = _context.tblRH_EK_Plantilla_Aditiva.Where(x => x.puesto == item.puesto && x.estatus == "A" && x.cc == _cc).ToList();
                    objAditivaPersonal.cantidad = objAditivaPersonal.cantidad + aditivas.Sum(x => x.cantidad);
                    //

                    //FOOTER RPT
                    totalPlantilla += objAditivaPersonal.cantidad.Value;
                    totalContratados += objAditivaPersonal.altas.Value;
                    totalXContratar += (objAditivaPersonal.cantidad.Value - objAditivaPersonal.altas.Value);
                    objAditivaPersonal.totalXContratar = (objAditivaPersonal.cantidad.Value - objAditivaPersonal.altas.Value);

                    lstDataPuestos.Add(objAditivaPersonal);
                }

                //FOOTER RPT
                porcContratado = (decimal)((decimal)totalContratados / (decimal)totalPlantilla) * 100;
                porcXContratar = (decimal)((decimal)totalXContratar / (decimal)totalPlantilla) * 100;

                lstDataPuestos = lstDataPuestos.OrderBy(e => e.puesto).ToList();

                lstTotalesPuestos.Add(totalPlantilla.ToString());
                lstTotalesPuestos.Add(totalContratados.ToString());
                lstTotalesPuestos.Add(totalXContratar.ToString());
                lstTotalesPuestos.Add(porcContratado.ToString("#.##") + "%");
                lstTotalesPuestos.Add(porcXContratar.ToString("#.##") + "%");

                //GET DESCRIPCION DEL CC
                tblC_Nom_CatalogoCC objCC = _context.tblC_Nom_CatalogoCC.FirstOrDefault(e => e.cc == _cc);
                string ccDesc = "[" + objCC.cc + "] " + objCC.ccDescripcion;

                //resultado.Add("lstCategoriasRelPuesto", lstCategoriasRelPuesto);
                resultado.Add("lstPuestosRelPuesto", lstDataPuestos);
                resultado.Add("lstTotalesRelPuesto", lstTotalesPuestos);
                resultado.Add("ccDesc", ccDesc);
                resultado.Add(SUCCESS, true);


            }
            catch (Exception e)
            {
                LogError(16, 16, "ReclutamientosController", "GetCategoriasRelPuesto", e, AccionEnum.CONSULTA, 0, 0);
            }


            return resultado;
        }
        public MemoryStream crearReporte(string cc)
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {

                    var hoja1 = excel.Workbook.Worksheets.Add("Nomina");

                    #region Titulo

                    hoja1.Cells["B1:M1"].Merge = true;

                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case (int)MainContextEnum.Construplan:
                            hoja1.Cells["B1"].Value = "GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV.";

                            break;
                        case (int)MainContextEnum.Arrendadora:
                            hoja1.Cells["B1"].Value = "ARRENDADORA CONSTRUPLAN SA DE CV";

                            break;
                        case (int)MainContextEnum.EICI:
                            hoja1.Cells["B1"].Value = "EICI";

                            break;
                        case (int)MainContextEnum.INTEGRADORA:
                            hoja1.Cells["B1"].Value = "INTEGRADORA";

                            break;
                    }

                    hoja1.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["B1"].Style.Font.Bold = true;
                    hoja1.Cells["B1"].Style.Font.Size = 14;

                    hoja1.Cells["B2:M2"].Merge = true;
                    hoja1.Cells["B2"].Value = "REPORTE DE STAFFING GUIDE EN GENERAL DETALLADO";
                    hoja1.Cells["B2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    hoja1.Cells["B3:M3"].Merge = true;
                    hoja1.Cells["B3"].Value = "Registros encontrados: " + 0;
                    hoja1.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    hoja1.Cells["B4:D4"].Merge = true;
                    hoja1.Cells["B4"].Value = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
                    hoja1.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    #endregion

                    List<string> headerRow = new List<string>() { "ID", "PUESTO", "PERSONAL EN PLANTILLA", "PERSONAL EXISTENTE", "PERSONAL FALTANTE" };
                    List<string> footerRow = new List<string>() { "-", "RESULTADO POR OBRA", "PLANTILLA", "CONTRATADOS", "POR CONTRATAR", "PORCENTAJE CONTRATADO", "PORCENTAJE POR CONTRATAR" };
                    var dictRpt = GetPuestosCategoriasRelPuesto(cc, null);
                    var lstReporte = dictRpt["lstPuestosRelPuesto"] as List<Core.DTO.RecursosHumanos.Enkontrol.CatCategoriasAditivas>;
                    var lstFooterReporte = dictRpt["lstTotalesRelPuesto"] as List<string>;

                    int j = 0;
                    for (int i = 2; i < headerRow.Count + 2; i++)
                    {
                        hoja1.Cells[5, i].Value = headerRow[j];
                        j++;
                    }

                    int tblWidth = headerRow.Count;
                    int tblHeight = lstReporte.Count + 6;
                    tblWidth++;

                    var cellData = new List<object[]>();


                    foreach (var item in lstReporte)
                    {
                        cellData.Add(new object[] {
                            item.id_puesto,
                            item.puesto,
                            item.cantidad,
                            item.altas,
                            (item.cantidad - item.altas),
                            string.Empty,
                            string.Empty,
                            string.Empty,
                        });

                    }

                    hoja1.Cells[6, 2].LoadFromArrays(cellData); // LOAD 

                    var rowTotales = new List<dynamic>() {
                        string.Empty,
                        "Total PLantilla",
                        Convert.ToInt32(lstFooterReporte[0]),
                        Convert.ToInt32(lstFooterReporte[1]),
                        Convert.ToInt32(lstFooterReporte[2]),
                        lstFooterReporte[3],
                        lstFooterReporte[4],};

                    j = 0;
                    for (int i = 2; i < headerRow.Count + 4; i++)
                    {
                        hoja1.Cells[tblHeight + 1, i].Value = footerRow[j];
                        hoja1.Cells[tblHeight + 2, i].Value = rowTotales[j];
                        j++;
                    }

                    for (int i = 2; i < tblWidth; i++)
                    {
                        hoja1.Column(i).Width = 15;

                    }

                    hoja1.Column(2).Width = 15;
                    hoja1.Column(3).Width = 55;
                    hoja1.Column(4).Width = 15;
                    hoja1.Column(5).Width = 15;
                    hoja1.Column(6).Width = 15;
                    hoja1.Column(7).Width = 15;
                    hoja1.Column(8).Width = 15;
                    hoja1.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    hoja1.Row(5).Style.WrapText = true;
                    hoja1.Row(tblHeight + 1).Style.WrapText = true;

                    //hoja1.Cells[tblHeight + 1, 4, tblHeight + 1, 6].Style.;

                    ExcelRange rangeTable = hoja1.Cells[5, 2, tblHeight - 1, tblWidth];

                    ExcelTable tbl1 = hoja1.Tables.Add(rangeTable, "Staffing");

                    tbl1.TableStyle = TableStyles.Medium17;

                    ExcelRange rangeTable2 = hoja1.Cells[tblHeight + 1, 2, tblHeight + 2, tblWidth + 2];

                    ExcelTable tbl2 = hoja1.Tables.Add(rangeTable2, "Staffing2");

                    tbl2.TableStyle = TableStyles.Medium17;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    var pieChart = hoja1.Drawings.AddChart("Plantilla", eChartType.Pie);
                    //Set top left corner to row 1 column 2
                    pieChart.SetPosition(tblHeight + 3, 2, 2, tblWidth);
                    pieChart.SetSize(400, 400);
                    pieChart.Series.Add(hoja1.Cells[tblHeight + 2, 5, tblHeight + 2, 6], hoja1.Cells[tblHeight + 1, 5, tblHeight + 1, 6]);
                    pieChart.Title.Text = "Plantilla";

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetDashboard(List<string> ccs, DateTime fechaInicio, DateTime fechaFin)
        {
            resultado.Clear();

            try
            {
                if (ccs == null)
                {
                    ccs = getListaCCRH().Select(e => e.Value).ToList();
                }
                else
                {
                    if (ccs.Count() == 0)
                    {
                        ccs = getListaCCRH().Select(e => e.Value).ToList();

                    }
                }

                #region TOTALES

                var totalBajas = _context.Select<int?>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT SUM(cantidad) FROM 
                                (
                                    SELECT e.cc_contable, '[' + e.cc_contable + '] ' + c.ccDescripcion as ccDesc, COUNT (*) AS cantidad 
                                    FROM tblRH_EK_Empleados AS e 
								    LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON e.id_regpat = srp.clave_reg_pat 
								    LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
								    LEFT JOIN tblRH_Baja_Registro AS eb ON e.clave_empleado=eb.numeroEmpleado 
								    LEFT JOIN tblRH_EK_Puestos AS sip ON sip.puesto = eb.idPuesto 
								    LEFT JOIN tblRH_EK_Razones_Baja AS rz ON eb.motivoBajaDeSistema = rz.clave_razon_baja 
								    LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = eb.cc
                                    WHERE eb.registroActivo = 1 AND eb.est_baja = 'A' AND eb.fechaBaja BETWEEN @fechaInicio AND @fechaFin AND eb.cc IN @ccs
								    GROUP BY e.cc_contable, '[' + e.cc_contable + '] ' + c.ccDescripcion
                                ) tT",
                    parametros = new { fechaInicio, fechaFin, ccs }
                }).FirstOrDefault();

                var totalInicioAltas = _context.Select<int?>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT SUM(cantidad) FROM (
                                SELECT t1.cc_contable as cc, '[' + t1.cc_contable + '] ' + t2.descripcion as ccDesc, COUNT(*) AS cantidad
                                FROM tblRH_EK_Empleados as t1
                                INNER JOIN tblP_CC AS t2 ON t1.cc_contable = t2.cc
                                WHERE t1.esActivo = 1 AND t1.estatus_empleado = 'A' AND fecha_alta < @fechaInicio AND t1.cc_contable IN @ccs
                                GROUP BY cc_contable, '[' + t1.cc_contable + '] ' + t2.descripcion) tT",
                    parametros = new { fechaInicio, fechaFin, ccs }
                }).FirstOrDefault();

                var totalFinAltas = _context.Select<int?>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT SUM(cantidad) FROM (
                                SELECT t1.cc_contable as cc, '[' + t1.cc_contable + '] ' + t2.descripcion as ccDesc, COUNT(*) AS cantidad
                                FROM tblRH_EK_Empleados as t1
                                INNER JOIN tblP_CC AS t2 ON t1.cc_contable = t2.cc
                                WHERE t1.esActivo = 1 AND t1.estatus_empleado = 'A' AND fecha_alta < @fechaFin AND t1.cc_contable IN @ccs
                                GROUP BY cc_contable, '[' + t1.cc_contable + '] ' + t2.descripcion) tT",
                    parametros = new { fechaInicio, fechaFin, ccs }
                }).FirstOrDefault();

                var totalCambios = _context.Select<int?>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT SUM(cantidad) FROM (
                                SELECT t1.CcID, '[' + t1.CcID + '] ' + t2.descripcion as ccDesc, COUNT(*) AS cantidad FROM tblRH_FormatoCambio AS t1 
                                INNER JOIN tblP_CC AS t2 ON t1.CcID = t2.cc
                                WHERE Aprobado = 1 AND FechaInicioCambio BETWEEN @fechaInicio AND @fechaFin AND CamposCambiados LIKE ('%Sueldo%') AND t1.CcID IN @ccs
                                GROUP BY t1.CcID, '[' + t1.CcID + '] ' + t2.descripcion) tT
                                ",
                    parametros = new { fechaInicio, fechaFin, ccs }
                }).FirstOrDefault();

                var totalMotivos = _context.Select<int?>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT SUM(cantidad) FROM (SELECT desc_motivo_baja, COUNT(*)  AS cantidad
                                FROM tblRH_Baja_Registro AS t1
                                INNER JOIN tblRH_EK_Razones_Baja AS t2 ON t1.motivoBajaDeSistema = t2.clave_razon_baja
                                WHERE t1.registroActivo = 1 AND t1.fechaBaja BETWEEN @fechaInicio AND @fechaFin AND t1.cc IN @ccs
                                GROUP BY desc_motivo_baja) tT
                                ",
                    parametros = new { fechaInicio, fechaFin, ccs }
                }).FirstOrDefault();

                var totalTipoCambios = _context.Select<int?>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT SUM(cantidad) FROM (SELECT t1.CamposCambiados, COUNT(*)  AS cantidad
                                FROM tblRH_FormatoCambio AS t1 
                                WHERE Aprobado = 1 AND FechaInicioCambio BETWEEN @fechaInicio AND @fechaFin AND CamposCambiados LIKE ('%Sueldo%') AND t1.CcID IN @ccs
                                GROUP BY t1.CamposCambiados) tT",
                    parametros = new { fechaInicio, fechaFin, ccs }
                }).FirstOrDefault();
                #endregion

                #region DATA

                var dataBajas = _context.Select<ReporteDashboardDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT e.cc_contable, '[' + e.cc_contable + '] ' + c.ccDescripcion as ccDesc, COUNT (*) AS cantidad 
                                FROM tblRH_EK_Empleados AS e 
								LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON e.id_regpat = srp.clave_reg_pat 
								LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
								LEFT JOIN tblRH_Baja_Registro AS eb ON e.clave_empleado=eb.numeroEmpleado 
								LEFT JOIN tblRH_EK_Puestos AS sip ON sip.puesto = eb.idPuesto 
								LEFT JOIN tblRH_EK_Razones_Baja AS rz ON eb.motivoBajaDeSistema = rz.clave_razon_baja 
								LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = eb.cc
                                WHERE eb.registroActivo = 1 AND eb.est_baja = 'A' AND eb.fechaBaja BETWEEN @fechaInicio AND @fechaFin AND e.cc_contable IN @ccs
								GROUP BY e.cc_contable, '[' + e.cc_contable + '] ' + c.ccDescripcion
                                ORDER BY ccDesc",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });

                var dataAltas = _context.Select<RepAltasDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT 
                                    e.cc_contable +' '+c.ccDescripcion AS cCdes,e.nss,
                                    e.cc_contable AS cC,
                                    e.clave_empleado AS empleadoID,
                                    (e.ape_paterno+' '+e.ape_materno+' '+e.nombre) AS empleado, 
                                    pu.descripcion as puesto,
							        tRecon.fecha_reingreso,
							        e.fecha_antiguedad,
                                    CONVERT(CHAR(20) ,e.fecha_antiguedad ,103) AS fechaAltaStr, 
                                    e.fecha_antiguedad ,(ne.nombre+' '+ne.ape_paterno+' '+ne.ape_materno) as jefeInmediato,
	                                srp.clave_reg_pat,
	                                srp.nombre_corto,
                                    tn.descripcion as tipo_nomina,
	                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as salario_base,
                                    (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as complemento,
                                    (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as bono_zona
                                    FROM tblRH_EK_Empleados AS e 
                                LEFT JOIN tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado  
                                LEFT JOIN tblC_Nom_CatalogoCC AS c ON c.cc = e.cc_contable  
                                INNER JOIN tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto  
                                INNER JOIN tblRH_EK_Tipos_Nomina AS tn ON e.tipo_nomina = tn.tipo_nomina  
                                LEFT JOIN tblRH_EK_Departamentos AS dp ON dp.clave_depto = e.clave_depto 
                                LEFT JOIN tblRH_EK_Empl_Grales AS empg ON empg.clave_empleado = e.clave_empleado  
                                LEFT JOIN tblRH_EK_Estados AS est ON (est.clave_estado = empg.estado_dom and est.clave_pais=e.clave_pais_nac) 
                                LEFT JOIN tblRH_EK_Ciudades AS cd ON (cd.clave_cuidad = empg.cuidado_dom and cd.clave_pais=e.clave_pais_nac and cd.clave_estado=empg.estado_dom) 
	                            INNER JOIN tblRH_EK_Registros_Patronales srp on e.id_regpat = srp.clave_reg_pat
							    LEFT JOIN tblRH_EK_Empl_Recontratacion as tRecon ON e.clave_empleado = tRecon.clave_empleado
                                WHERE e.esActivo = 1  AND e.cc_contable IN @ccs ",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });

                var dataInicioActivos = _context.Select<ReporteDashboardDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT t1.cc_contable as cc, '[' + t1.cc_contable + '] ' + t2.descripcion as ccDesc, COUNT(*) AS cantidad
                                FROM tblRH_EK_Empleados as t1
                                INNER JOIN tblP_CC AS t2 ON t1.cc_contable = t2.cc
                                WHERE t1.esActivo = 1 AND t1.estatus_empleado = 'A' AND fecha_alta < @fechaInicio AND t1.cc_contable IN @ccs
                                GROUP BY cc_contable, '[' + t1.cc_contable + '] ' + t2.descripcion
                                ORDER BY ccDesc",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });

                var dataFinActivos = _context.Select<ReporteDashboardDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT t1.cc_contable as cc, '[' + t1.cc_contable + '] ' + t2.descripcion as ccDesc, COUNT(*) AS cantidad
                                FROM tblRH_EK_Empleados as t1
                                INNER JOIN tblP_CC AS t2 ON t1.cc_contable = t2.cc
                                WHERE t1.esActivo = 1 AND t1.estatus_empleado = 'A' AND fecha_alta < @fechaFin AND t1.cc_contable IN @ccs
                                GROUP BY cc_contable, '[' + t1.cc_contable + '] ' + t2.descripcion
                                ORDER BY ccDesc",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });

                var dataCambios = _context.Select<ReporteDashboardDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT t1.CcID as cc, '[' + t1.CcID + '] ' + t2.descripcion as ccDesc, COUNT(*) AS cantidad 
                                FROM tblRH_FormatoCambio AS t1 
                                INNER JOIN tblP_CC AS t2 ON t1.CcID = t2.cc
                                WHERE Aprobado = 1 AND FechaInicioCambio BETWEEN @fechaInicio AND @fechaFin AND CamposCambiados LIKE ('%Sueldo%') AND t1.CcID IN @ccs
                                GROUP BY t1.CcID, '[' + t1.CcID + '] ' + t2.descripcion
                                ORDER BY ccDesc",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });

                var dataMotivos = _context.Select<ReporteDashboardMotivosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"   
                                SELECT desc_motivo_baja as descMotivo, COUNT(*)  AS cantidad
                                FROM tblRH_Baja_Registro AS t1
                                INNER JOIN tblRH_EK_Razones_Baja AS t2 ON t1.motivoBajaDeSistema = t2.clave_razon_baja
                                WHERE t1.registroActivo = 1 AND t1.fechaBaja BETWEEN @fechaInicio AND @fechaFin AND t1.cc IN @ccs
                                GROUP BY desc_motivo_baja",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });

                var dataTipoCambios = _context.Select<ReporteDashboardTiposCambiosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"   
                                SELECT t1.CamposCambiados, COUNT(*)  AS cantidad
                                FROM tblRH_FormatoCambio AS t1 
                                WHERE Aprobado = 1 AND FechaInicioCambio BETWEEN @fechaInicio AND @fechaFin AND CamposCambiados LIKE ('%Sueldo%') AND t1.CcID IN @ccs
                                GROUP BY t1.CamposCambiados",
                    parametros = new { fechaInicio, fechaFin, ccs }
                });
                #endregion

                #region CALC PORCENTAJES

                var dataAltasBajas = new List<ReporteDashboardDTO>();
                var resultFiltered = new List<RepAltasDTO>();
                var ccAltas = new List<ComboDTO>();

                int numTotalBajas = totalBajas != null ? totalBajas.Value : 1;
                int numTotalAltas = 0;

                foreach (var item in dataAltas)
                {
                    ccAltas.Add(new ComboDTO { Value = item.cC, Text = item.cCdes });

                    if (item.fecha_reingreso == null)
                    {
                        item.fecha_reingreso = item.fecha_antiguedad;
                    }
                    if (item.fecha_reingreso <= fechaFin && item.fecha_reingreso >= fechaInicio)
                    {
                        //var objAlta = resultFiltered.Where(e => e.empleadoID == item.empleadoID && e.puesto == item.puesto).FirstOrDefault();

                        //if (objAlta == null)
                        //{
                        //    resultFiltered.Add(item);
                        //}
                        resultFiltered.Add(item);

                    }
                    //decimal numPorcAltas = (decimal)item.cantidad / (decimal)numTotalAltas;

                    //item.cantAltas = item.cantidad;
                    //item.porcAltas = numPorcAltas * 100;
                }

                var grpDataAltas = from item in resultFiltered
                                   group item by item.cC into newGroup
                                   orderby newGroup.Key
                                   select new ReporteDashboardDTO { cc = newGroup.Key, ccDesc = ccAltas.FirstOrDefault(e => e.Value == newGroup.Key).Text, cantAltas = newGroup.Count(), porcAltas = (decimal)((decimal)newGroup.Count() / (decimal)resultFiltered.Count()) * 100 };

                foreach (var item in dataBajas)
                {
                    decimal numPorcBajas = (decimal)item.cantidad / (decimal)numTotalBajas;

                    item.cantBajas = item.cantidad;
                    item.porcBajas = numPorcBajas * 100;
                }
                #endregion

                foreach (var item in dataMotivos)
                {
                    item.porcMotivo = (decimal)((decimal)item.cantidad / (decimal)totalMotivos) * 100;
                }

                foreach (var item in dataCambios)
                {
                    item.porcAltas = (decimal)((decimal)item.cantidad / (decimal)totalCambios) * 100;
                }

                foreach (var item in dataTipoCambios)
                {
                    item.porcCambios = (decimal)((decimal)item.cantidad / (decimal)totalTipoCambios) * 100;
                }

                //ROTACION DEL RANGO
                decimal rotacion = 0;
                if (resultFiltered.Count() > 0 && totalBajas != null)
                {
                    rotacion = (decimal)((((decimal)resultFiltered.Count() + (decimal)totalBajas) / 2) * 100 / (((decimal)totalInicioAltas + (decimal)totalFinAltas) / 2));
                }


                //resultado.Add("dataAltasBajas", dataAltasBajas);
                resultado.Add("dataAltas", grpDataAltas);
                resultado.Add("dataBajas", dataBajas);
                resultado.Add("dataMotivos", dataMotivos);
                resultado.Add("dataInicioActivos", dataInicioActivos);
                resultado.Add("dataFinActivos", dataFinActivos);
                resultado.Add("dataCambios", dataCambios);
                resultado.Add("dataTipoCambios", dataTipoCambios);
                resultado.Add("totalBajas", totalBajas != null ? totalBajas : 0);
                resultado.Add("totalAltas", resultFiltered.Count());
                resultado.Add("totalInicioAltas", totalInicioAltas);
                resultado.Add("totalFinAltas", totalFinAltas);
                resultado.Add("totalCambios", rotacion);
                resultado.Add("totalTipoCambios", totalTipoCambios);
                resultado.Add("totalNumCambios", totalCambios != null ? totalCambios : 0);
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

        #region Constancias

        public Dictionary<string, object> GetEmpleadosPrestamos(List<string> cc)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool permisoGerencia = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4052).Count()) > 0;

                #region SE OBTIENE LISTADO DE EMPLEADOS ACTIVOS
                List<string> lstCC = new List<string>();
                if (cc != null)
                {
                    foreach (var item in cc)
                    {
                        lstCC.Add(string.Format("'{0}'", item));
                    }
                }

                string strQuery = @"SELECT t1.clave_empleado, t1.nombre, t1.ape_paterno, t1.ape_materno, t2.descripcion, t1.sindicato
	                                FROM tblRH_EK_Empleados AS t1
	                                INNER JOIN tblRH_EK_Puestos AS t2 ON t2.puesto = t1.puesto
		                            WHERE t1.estatus_empleado = 'A'" +
                                        (permisoGerencia ? "" : " AND (t2.descripcion NOT LIKE '%GERENCIA%' AND t2.descripcion NOT LIKE '%GERENTE%' AND t2.descripcion NOT LIKE '%DIRECCI%' AND t2.descripcion NOT LIKE '%DIRECTOR%')");
                                    
                if (lstCC.Count() > 0)
                    strQuery += string.Format(" AND t1.cc_contable IN ({0})", string.Join(",", lstCC));

                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<Core.DTO.RecursosHumanos.Reclutamientos.EmpleadosDTO> lstEmpleados = _context.Select<Core.DTO.RecursosHumanos.Reclutamientos.EmpleadosDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = strQuery
                }).ToList();

                foreach (var item in lstEmpleados)
                {
                    #region SE OBTIENE EL NOMBRE COMPLETO
                    item.nombreCompleto = GetNombreCompletoSIGOPLAN(item.nombre, item.ape_paterno, item.ape_materno);
                    #endregion


                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstEmpleados);
                #endregion

                //var lisEmpleados = _context.tblRH_EK_Empleados.Where(e => e.estatus_empleado == "A" && cc.Contains(e.cc_contable)).ToList();
                //resultado.Add(SUCCESS, true);
                //resultado.Add(ITEMS, lisEmpleados);
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetNombreCompletoSIGOPLAN(string nombre, string ape_paterno, string ape_materno)
        {
            string nombreCompleto = string.Empty;
            try
            {
                #region SE OBTIENE EL NOMBRE COMPLETO
                if (!string.IsNullOrEmpty(nombre))
                    nombreCompleto = nombre.Trim();
                if (!string.IsNullOrEmpty(ape_paterno))
                    nombreCompleto += string.Format(" {0}", ape_paterno);
                if (!string.IsNullOrEmpty(ape_materno))
                    nombreCompleto += string.Format(" {0}", ape_materno);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetNombreCompletoSIGOPLAN", e, AccionEnum.CONSULTA, 0, new { nombre = nombre, ape_paterno = ape_paterno, ape_materno = ape_materno });
                return string.Empty;
            }
            return nombreCompleto;
        }

        private MainContextEnum GetEmpresaLogueada()
        {
            MainContextEnum objEmpresa = new MainContextEnum();
            try
            {
                #region SE OBTIENE MainContextEnum DE LA EMPRESA LOGUEADA
                switch ((int)vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                        objEmpresa = MainContextEnum.Construplan;
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        objEmpresa = MainContextEnum.Arrendadora;
                        break;
                    case (int)EmpresaEnum.Colombia:
                        objEmpresa = MainContextEnum.Colombia;
                        break;
                    case (int)EmpresaEnum.Peru:
                        objEmpresa = MainContextEnum.PERU;
                        break;
                    default:
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetEmpresaLogueada", e, AccionEnum.CONSULTA, (int)vSesiones.sesionEmpresaActual, new { idEmpresa = (int)vSesiones.sesionEmpresaActual });
                return objEmpresa;
            }
            return objEmpresa;
        }

        public Dictionary<string, object> GetConsultaCC(List<string> cc, string estatus)
        {

            var resultado = new Dictionary<string, object>();
            //string cc = vSesiones.sesionUsuarioDTO.cc;

            try
            {
                var result = _context.Select<repConsultaCCDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                           SELECT 
                                tEmpleados.clave_empleado as clave_empleado,
								(tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,	 
                                tEmpleados.contratable as contratable,
	                            tPuesto.descripcion as nombrePuesto
								FROM tblRH_EK_Empleados as tEmpleados                                
                                INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina								                              
                                WHERE tEmpleados.cc_contable IN @cc AND tEmpleados.estatus_empleado=@estatus",
                    parametros = new { cc, estatus }


                });

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result);
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetPrestamos(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                           SELECT 
                                tEmpleados.cc_contable as cc,
	                            tEmpleados.clave_empleado as clave_empleado,
                                tPuesto.puesto as puesto,
	                            (tCC.cc + ' ' + tCC.descripcion) as ccDescripcion,								
	                            tPuesto.descripcion as nombrePuesto,							
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,
	                            tEmpleados.fecha_antiguedad as fecha_alta,							
	                            tn.descripcion as tipoNomina,
	                            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldo_base,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento                        
                                FROM tblRH_EK_Empleados as tEmpleados
                                INNER JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                INNER JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetConsultaPrestamos(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                         SELECT 
								            TOP (1) tEmpleados.cc_contable as cc,
                                            (tCC.cc + ' ' + tCC.descripcion) as ccDescripcion,	
                                            tPuesto.puesto as puesto,
                                            tPuesto.descripcion as nombrePuesto,	
                                            tEmpleados.clave_empleado as clave_empleado,                         								                            							
	                                        (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,                               	                            
                                            tEmpleados.fecha_antiguedad as fecha_alta,								
	                                        tn.descripcion as tipoNomina,
	                                        (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldo_base,
	                                        (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento,                    
                                            prestamo.totalN as totalN,
								            prestamo.totalM as totalM,
								            prestamo.otrosDescuento,
								            prestamo.cantidadMax,
								            prestamo.cantidadSoli,
								            prestamo.cantidadLetra,
								            prestamo.cantidadDescontar,								
								            prestamo.formaPago,
								            prestamo.justificacion,
                                            prestamo.tipoSolicitud,
								            prestamo.tipoPrestamo,
								            prestamo.tipoPuesto,	
                                            prestamo.empresa,
                                            prestamo.motivoPrestamo,								           
											(select TOP (1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idResponsableCC))as nombreResponsableCC,
                                           	(select TOP (1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idDirectorLineaN))as nombreDirectorLineaN,
                                          	(select TOP (1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idGerenteOdirector))as nombreGerenteOdirector,
                                      		(select TOP (1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idDirectorGeneral))as nombreDirectorGeneral,	
                                      		(select TOP (1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idCapitalHumano))as nombreCapitalHumano,
                                            prestamo.consecutivo,
                                            prestamo.fecha_creacion
								            FROM tblRH_EK_Empleados as tEmpleados
                                            INNER JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                            INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto                          
								            INNER JOIN tblRH_EK_Prestamos AS prestamo ON tEmpleados.clave_empleado = prestamo.clave_empleado
                                            INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                                            INNER JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat 
                                WHERE prestamo.registroActivo = 1 and tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new {clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarEditarPrestamos(tblRH_EK_Prestamos data)
        {
            var resultado = new Dictionary<string, object>();
            data.idUsuaioCreacion = vSesiones.sesionUsuarioDTO.id;
            data.empresa = vSesiones.sesionEmpresaActualNombre;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (data.clave_empleado == 0)
                    {
                        throw new Exception("Favor de capturar la informacion necesaria");
                    }
                    #region CONSECUTIVO
                    var lstPrestamosEmpleado = _context.tblRH_EK_Prestamos.Where(e => e.registroActivo && e.clave_empleado == data.clave_empleado).ToList();

                    int consecutivo = 0;
                    if (lstPrestamosEmpleado.Count() > 0) consecutivo = lstPrestamosEmpleado.Max(x => x.consecutivo);
                    #endregion

                    DateTime primerDia = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);
                    
                    var objPrestamo = _context.tblRH_EK_Prestamos.Where(e => e.registroActivo && e.clave_empleado == data.clave_empleado).FirstOrDefault();
                    
                    if (objPrestamo!= null && objPrestamo.estatus == "P")
                    {
                        throw new Exception("El usuario cuenta con un prestamo en espera de autorizacion");
                    }
                    else if (objPrestamo != null && objPrestamo.estatus == "A")
                    {
                        var fechaConcluye = objPrestamo.fechaModificacion.AddMonths(6);

                        if (DateTime.Now.Date < fechaConcluye)
                        {
                            throw new Exception("El usuario cuenta con un prestamo autorizado y que concluye el " + fechaConcluye.ToString("dd/MM/yyyy") + ".");                            
                        }
                    }
                    if (data.motivoPrestamo == 5)
                    {
                        List<string> ccSindicato = new List<string> () { "146", "180", "010" };
                        List<Tuple<string, decimal>> ccSindicatoLimites = new List<Tuple<string, decimal>>();
                        // --> Limites apra préstamo sindicato por centro de costo
                        ccSindicatoLimites.Add(new Tuple<string, decimal>("146", 60000));
                        ccSindicatoLimites.Add(new Tuple<string, decimal>("180", 0));
                        if (vSesiones.sesionUsuarioDTO.id == 13 || vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79552)
                        {
                            ccSindicatoLimites.Add(new Tuple<string, decimal>("010", 20000));
                        }
                        else 
                        {
                            ccSindicatoLimites.Add(new Tuple<string, decimal>("010", 20000));
                        }

                        if (ccSindicato.Contains(data.cc))
                        {
                            var tuplelimite = ccSindicatoLimites.FirstOrDefault(x => x.Item1 == data.cc);
                            if (tuplelimite != null)
                            {
                                decimal limitePrestamo = tuplelimite.Item2;
                                var prestamosSindicato = _context.tblRH_EK_Prestamos.Where(e => e.registroActivo && e.cc == data.cc && (e.fecha_creacion >= primerDia && e.fecha_creacion <= ultimoDia) && e.motivoPrestamo == 5).ToList();
                                decimal montoTotalSindicatoMes = prestamosSindicato.Count() > 0 ? prestamosSindicato.Sum(m => m.cantidadSoli) : 0;
                                decimal totalMasCantidadSolicitada = montoTotalSindicatoMes + data.cantidadSoli;
                                decimal totalRestante = 0;
                                if (totalMasCantidadSolicitada > limitePrestamo)
                                {
                                    totalRestante = limitePrestamo - montoTotalSindicatoMes;
                                    throw new Exception("No se puede realizar captura, El monto total del mes excede el limite indicado por sindicato. El Monto disponible a solicitar es de $" + totalRestante + "");
                                }
                            }
                            else 
                            {
                                throw new Exception("No se encontró límite registrado para este centro de costos, favor de comunicarse con el departamento de Capital Humano");
                            }
                        }
                        else 
                        {
                            throw new Exception("No se permiten las capturas de préstamos sindicales para el centro de costo solicitado");
                        }
                    }

                    data.consecutivo = consecutivo + 1;
                    data.idUsuaioCreacion = vSesiones.sesionUsuarioDTO.id;
                    data.fecha_creacion = DateTime.Now;
                    data.fechaModificacion = data.fecha_creacion;

                    data.estatus = "P";
                    data.registroActivo = true;
                    _context.tblRH_EK_Prestamos.Add(data);
                    _context.SaveChanges();

                    #region AGERGAR ALERTA AL PRIMER AUTORIZANTE COMENTADO *AHORA SE MANDA LA PRIMERA ALERTA AL NOTIFICAR*

//                    bool esAlertar = true;

//                    #region REPOSABLE CC
//                    if (data.idResponsableCC > 0 && esAlertar)
//                    {
//                        string cveEmpleado = data.idResponsableCC.ToString();
//                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == cveEmpleado);

//                        #region Alerta SIGOPLAN
//                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
//                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
//                        objNuevaAlerta.userRecibeID = objUsuario.id;
//#if DEBUG
//                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
//                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
//                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
//#endif
//                        objNuevaAlerta.tipoAlerta = 2;
//                        objNuevaAlerta.sistemaID = 16;
//                        objNuevaAlerta.visto = false;
//                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + data.clave_empleado + "&ccEmpleado=" + data.cc + "&tipoDePrestamo=" + data.tipoPrestamo + "&statusPrestamo=" + data.estatus;
//                        objNuevaAlerta.objID = data.id;
//                        objNuevaAlerta.obj = "AutorizacionPrestamos";
//                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + data.clave_empleado;
//                        objNuevaAlerta.documentoID = 0;
//                        objNuevaAlerta.moduloID = 0;
//                        _context.tblP_Alerta.Add(objNuevaAlerta);
//                        _context.SaveChanges();
//                        #endregion //ALERTA SIGPLAN

//                        //CAMBIAR BANDERA
//                        esAlertar = false;
//                    }
//                    #endregion

//                    #region GERENTE/DIRECTOR
//                    if (data.idGerenteOdirector > 0 && esAlertar)
//                    {
//                        string cveEmpleado = data.idGerenteOdirector.ToString();
//                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == cveEmpleado);

//                        #region Alerta SIGOPLAN
//                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
//                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
//                        objNuevaAlerta.userRecibeID = objUsuario.id;
//#if DEBUG
//                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
//                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
//                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
//#endif
//                        objNuevaAlerta.tipoAlerta = 2;
//                        objNuevaAlerta.sistemaID = 16;
//                        objNuevaAlerta.visto = false;
//                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + data.clave_empleado + "&ccEmpleado=" + data.cc + "&tipoDePrestamo=" + data.tipoPrestamo + "&statusPrestamo=" + data.estatus;
//                        objNuevaAlerta.objID = data.id;
//                        objNuevaAlerta.obj = "AutorizacionPrestamos";
//                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + data.clave_empleado;
//                        objNuevaAlerta.documentoID = 0;
//                        objNuevaAlerta.moduloID = 0;
//                        _context.tblP_Alerta.Add(objNuevaAlerta);
//                        _context.SaveChanges();
//                        #endregion //ALERTA SIGPLAN

//                        //CAMBIAR BANDERA
//                        esAlertar = false;
//                    }
//                    #endregion

//                    #region DIRECTOR LINEA NEGOCIO
//                    if (data.idDirectorLineaN > 0 && esAlertar)
//                    {
//                        string cveEmpleado = data.idDirectorLineaN.ToString();
//                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == cveEmpleado);

//                        #region Alerta SIGOPLAN
//                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
//                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
//                        objNuevaAlerta.userRecibeID = objUsuario.id;
//#if DEBUG
//                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
//                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
//                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
//#endif
//                        objNuevaAlerta.tipoAlerta = 2;
//                        objNuevaAlerta.sistemaID = 16;
//                        objNuevaAlerta.visto = false;
//                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + data.clave_empleado + "&ccEmpleado=" + data.cc + "&tipoDePrestamo=" + data.tipoPrestamo + "&statusPrestamo=" + data.estatus;
//                        objNuevaAlerta.objID = data.id;
//                        objNuevaAlerta.obj = "AutorizacionPrestamos";
//                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + data.clave_empleado;
//                        objNuevaAlerta.documentoID = 0;
//                        objNuevaAlerta.moduloID = 0;
//                        _context.tblP_Alerta.Add(objNuevaAlerta);
//                        _context.SaveChanges();
//                        #endregion //ALERTA SIGPLAN

//                        //CAMBIAR BANDERA
//                        esAlertar = false;
//                    }
//                    #endregion

//                    #region DIRECTOR GENERAL
//                    if (data.idDirectorGeneral > 0 && esAlertar)
//                    {
//                        string cveEmpleado = data.idDirectorGeneral.ToString();
//                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == cveEmpleado);

//                        #region Alerta SIGOPLAN
//                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
//                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
//                        objNuevaAlerta.userRecibeID = objUsuario.id;
//#if DEBUG
//                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
//                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
//                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
//#endif
//                        objNuevaAlerta.tipoAlerta = 2;
//                        objNuevaAlerta.sistemaID = 16;
//                        objNuevaAlerta.visto = false;
//                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + data.clave_empleado + "&ccEmpleado=" + data.cc + "&tipoDePrestamo=" + data.tipoPrestamo + "&statusPrestamo=" + data.estatus;
//                        objNuevaAlerta.objID = data.id;
//                        objNuevaAlerta.obj = "AutorizacionPrestamos";
//                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + data.clave_empleado;
//                        objNuevaAlerta.documentoID = 0;
//                        objNuevaAlerta.moduloID = 0;
//                        _context.tblP_Alerta.Add(objNuevaAlerta);
//                        _context.SaveChanges();
//                        #endregion //ALERTA SIGPLAN

//                        //CAMBIAR BANDERA
//                        esAlertar = false;
//                    }
//                    #endregion

//                    #region CH
//                    if (data.idCapitalHumano > 0 && esAlertar)
//                    {
//                        string cveEmpleado = data.idCapitalHumano.ToString();
//                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.cveEmpleado == cveEmpleado);

//                        #region Alerta SIGOPLAN
//                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
//                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
//                        objNuevaAlerta.userRecibeID = objUsuario.id;
//#if DEBUG
//                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
//                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
//                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
//#endif
//                        objNuevaAlerta.tipoAlerta = 2;
//                        objNuevaAlerta.sistemaID = 16;
//                        objNuevaAlerta.visto = false;
//                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + data.clave_empleado + "&ccEmpleado=" + data.cc + "&tipoDePrestamo=" + data.tipoPrestamo + "&statusPrestamo=" + data.estatus;
//                        objNuevaAlerta.objID = data.id;
//                        objNuevaAlerta.obj = "AutorizacionPrestamos";
//                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + data.clave_empleado;
//                        objNuevaAlerta.documentoID = 0;
//                        objNuevaAlerta.moduloID = 0;
//                        _context.tblP_Alerta.Add(objNuevaAlerta);
//                        _context.SaveChanges();
//                        #endregion //ALERTA SIGPLAN

//                        //CAMBIAR BANDERA
//                        esAlertar = false;
//                    }
//                    #endregion

                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        public Dictionary<string, object> GetConfiguracionPrestamos()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var listPeriodos = _context.tblRH_Prestamos_ConfiguracionPrestamo.Where(e => e.registroActivo == true).ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listPeriodos);
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GuardarConfiguracionPrestamo(tblRH_Prestamos_ConfiguracionPrestamo data)
        {
            #region
            var resultado = new Dictionary<string, object>();


            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                var configuracionPrestamos = _context.tblRH_Prestamos_ConfiguracionPrestamo.Where(a => a.id == data.id).FirstOrDefault();

                if (configuracionPrestamos != null)
                {
                    data.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    data.fecha_modificacion = DateTime.Now;
                }
                else
                {
                    data.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    data.fecha_creacion = DateTime.Now;
                    data.estatus = "D";
                    //data.fecha_modificacion = data.fecha_creacion;
                    data.registroActivo = true;
                    _context.tblRH_Prestamos_ConfiguracionPrestamo.Add(data);
                }


                _context.SaveChanges();
                dbContextTransaction.Commit();
                resultado.Add(SUCCESS, true);

            }
            return resultado;
            #endregion
        }
        public Dictionary<string, object> EliminarConfiguracion(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registroConfiguracion = _context.tblRH_Prestamos_ConfiguracionPrestamo.FirstOrDefault(x => x.id == id);

                    registroConfiguracion.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        public Dictionary<string, object> GetFechasPeriodos()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var lisPeriodos = _context.tblRH_Prestamos_ConfiguracionPrestamo.Where(e => e.estatus == "A").ToList();
                bool aviso = false;
                if (lisPeriodos.FirstOrDefault().fechaInicioPeriodo <= DateTime.Now && lisPeriodos.FirstOrDefault().fechaFinPeriodo >= DateTime.Now)
                {
                    aviso = true;

                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, aviso);
                resultado.Add("lstPeriodos", lisPeriodos);

            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> ActivarDesactivarPeriodo(int id, string estatus)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var activarDesactivarPeriodo = _context.tblRH_Prestamos_ConfiguracionPrestamo.FirstOrDefault(x => x.id == id);



                    activarDesactivarPeriodo.registroActivo = true;
                    activarDesactivarPeriodo.estatus = estatus;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        //public Dictionary<string, object> AutorizarRechazarPrestamo(string estatus)
        //{
        //    using (var dbContextTransaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var autorizaRechazaPrestamo = _context.tblRH_EK_Prestamos.FirstOrDefault(x => x.registroActivo == true);

        //            if (autorizaRechazaPrestamo != null)
        //            {

        //                autorizaRechazaPrestamo.registroActivo = false;
        //                autorizaRechazaPrestamo.estatus = estatus;
        //                _context.SaveChanges();
        //            }
        //            else
        //            {
        //                throw new Exception("No se encuentra la información del prestamo.");
        //            }

        //            dbContextTransaction.Commit();
        //            resultado.Add(SUCCESS, true);
        //        }
        //        catch (Exception e)
        //        {

        //            resultado.Add(SUCCESS, false);
        //            resultado.Add(MESSAGE, e.Message);
        //        }
        //    }

        //    return resultado;
        //}
        public Dictionary<string, object> EliminarPrestamo(int prestamo_id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registroPrestamo = _context.tblRH_EK_Prestamos.FirstOrDefault(x => x.id == prestamo_id);

                    if (registroPrestamo != null)
                    {
                        SaveBitacora(16, (int)AccionEnum.ELIMINAR, registroPrestamo.id, JsonUtils.convertNetObjectToJson(registroPrestamo));

                        registroPrestamo.registroActivo = false;
                        registroPrestamo.estatus = "C";
                        registroPrestamo.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        registroPrestamo.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del prestamo.");
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        public void NotificarSolicitudPrestamo()
        {

            var correos = new List<string>();
            var archivo = new List<adjuntoCorreoDTO>();

            var asunto = "S/N";
            var mensaje = "S/N";

            correos = new List<string> { "miguel.buzani@construplan.com.mx" };
            correos = correos.Distinct().ToList();
            GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correos, archivo);
            //GlobalUtils.sendEmail(asunto, mensaje, correos);

        }
        public Dictionary<string, object> GetSolicitudPrestamos(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4053).Count()) > 0;

                var result = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                                SELECT 
                                tEmpleados.cc_contable as cc,
	                            tEmpleados.clave_empleado as clave_empleado,
                                tPuesto.puesto as puesto,
	                            ('[' + tCC.cc + '] ' + tCC.descripcion) as ccDescripcion,
	                            tPuesto.descripcion as nombrePuesto,							
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,
	                            tEmpleados.fecha_antiguedad as fecha_alta,							
	                            tEmpleados.fecha_antiguedad as fecha_altaEmpleado,							
	                            tn.descripcion as tipoNomina,
	                            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldo_base,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento,
	                            (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = 27681 order by id desc) as bono_zona,
                                tEmpleados.sindicato
                                FROM tblRH_EK_Empleados as tEmpleados
                                INNER JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                INNER JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });

                 //PERMISO SUELDOS

                //if (!permisoSueldos)
                //{
                //    var objPrestamo = result.FirstOrDefault();
                //    objPrestamo.sueldo_base = 0;
                //    objPrestamo.complemento = 0;
                //    objPrestamo.bono_zona = 0;
                //    objPrestamo.sueldo_base = 0;
                //}

                result[0].fecha_alta = result[0].fecha_alta.Substring(0,10);

                DateTime fechaMinus = DateTime.Now;
                string minusDesc = "";

                //if (result[0].FK_Sindicato == 1)
                //{
                //    minusDesc = "6 meses";
                //    fechaMinus = fechaMinus.AddMonths(-6);
                //}
                //else
                //{
                //    //minusDesc = "1 año";    
                //    //fechaMinus = fechaMinus.AddMonths(-12);
                //    minusDesc = "6 Meses";
                //    fechaMinus = fechaMinus.AddMonths(-6);
                //}

                if (result[0].sindicato == "S")
                {
                    minusDesc = "6 meses";
                    fechaMinus = fechaMinus.AddMonths(-6);
                    result[0].FK_Sindicato = 1;
                }
                else
                {
                    //minusDesc = "1 año";    
                    //fechaMinus = fechaMinus.AddMonths(-12);
                    minusDesc = "6 Meses";
                    fechaMinus = fechaMinus.AddMonths(-6);
                    result[0].FK_Sindicato = 2;

                }

                if (result[0].fecha_altaEmpleado.Date > fechaMinus.Date)
                {
                    resultado.Add(MESSAGE, "El empleado no cuenta con la antiguedad necesaria ( " + minusDesc + " )");
                    resultado.Add(SUCCESS, false);
                    return resultado;
                }

                var empleadoSoli = result.FirstOrDefault();
                var SolicitudPrestamo = _context.tblRH_EK_Prestamos.FirstOrDefault(x => x.registroActivo && x.clave_empleado == empleadoSoli.clave_empleado );
                if (SolicitudPrestamo != null)
                {
                    resultado.Add(MESSAGE, "El empleado ya solicito un prestamo");
                    resultado.Add(SUCCESS, false);
                    return resultado;
                }



                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Construplan:
                        result[0].empresa = "GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV";
                        break;
                    case EmpresaEnum.Arrendadora:
                        result[0].empresa = "CONSTRUPLAN ARRENDADORA SA DE CV";
                        break;
                    case EmpresaEnum.Colombia:
                        result[0].empresa = "CONSTRUPLAN COLOMBIA SUCURSAL";
                        break;
                    case EmpresaEnum.Peru:
                        result[0].empresa = "GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV SUCURSAL PERÚ";
                        break;
                    default:
                        result[0].empresa = "GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV";
                        break;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetSolicitudLactancia(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<repLactanciaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    //tEmpleados.clave_empleado as clave_empleado,    
                    consulta = @"
                           SELECT 	                        
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompletoLact,   
								tEmpleados.sexo,
                                tPuesto.descripcion as nombrePuestoLact,
								tbInca.fechaInicio as fechaInicioInca,
								tbInca.fechaTerminacion as fechaFinInca								
                                FROM tblRH_EK_Empleados as tEmpleados								
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc   
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto 
								LEFT JOIN  tblRH_Vacaciones_Incapacidades as tbInca ON tEmpleados.clave_empleado = tbInca.clave_empleado
                                INNER JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado								
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetSolicitudFonacot(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<repFonacotDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                          SELECT 	      
	                            tPuesto.descripcion as nombrePuesto,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,                                
	                            tEmpleados.nss as imss,
                                tCC.descripcion as ccDescripcionFonacot,	
	                            tEmpleados.curp as curp,
	                            tEmpleados.rfc as rfc,
	                            tEmpleados.fecha_antiguedad as fechaIngreso,                                
								srp.registro_patronal as regPatron,
                                tn.descripcion as tipoNomina,
                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldoBase,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento, 
	                            srp.desc_reg_pat as nombrePatron
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                LEFT JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                LEFT JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                                LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
								LEFT JOIN tblRH_EK_Prestamos AS prestamo ON tEmpleados.clave_empleado = prestamo.clave_empleado								
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetSolicitudGuarderia(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<repGuarderiaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                          SELECT 	      
                                tCC.descripcion as descripcionCCGuard,
	                            tPuesto.descripcion as nombrePuestoGuard,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompletoGuard,                                
	                            tEmpleados.nss as imss,
	                            tEmpleados.curp as curp,
	                            tEmpleados.rfc as rfc,
	                            tEmpleados.fecha_antiguedad as fechaIngreso,                                
								srp.registro_patronal as regPatron,
                                tn.descripcion as tipoNomina,
                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldoBase,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento, 
	                            srp.desc_reg_pat as nombrePatron
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                INNER JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                                LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
								LEFT JOIN tblRH_EK_Prestamos AS prestamo ON tEmpleados.clave_empleado = prestamo.clave_empleado								
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetHijos(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<ConsultaHijosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                     SELECT 	      
                                tEmpleados.clave_empleado as clave_empleado,
								(familia.apellido_paterno + ' ' + familia.apellido_materno + ' ' + familia.nombre) as nombreCompleto,
								familia.parentesco as parentesco
                                FROM tblRH_EK_Empleados as tEmpleados                              
								INNER JOIN tblRH_EK_Empl_Familia as familia on tEmpleados.clave_empleado = familia.clave_empleado 
								INNER JOIN tblRH_EK_Parentesco as parentesco on familia.parentesco = parentesco.id														
                                WHERE familia.parentesco= 4	AND tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.ToList());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> GetSolicitudLaboral(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<repLaboralDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                      SELECT 	      
	                            tPuesto.descripcion as nombrePuestoLab,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompletoLab,
                                ISNULL(tCC.descripcion, ISNULL(tCCRH.descripcionRH, tEmpleados.cc_contable)) as proyectoCCLab,
	                            tEmpleados.nss as numeroSeguroLab,
	                            tEmpleados.curp as curpLab,
	                            tEmpleados.rfc as rfcLab,
                                tn.descripcion as tipoNominaLab,
                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldoBaseLab,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complementoLab, 
	                            tEmpleados.fecha_antiguedad as fechaAltaLab,
                                tBajas.fechabaja as fechaBajaLab,
                                tEmpleados.contratable as contratable,
								srp.registro_patronal as numeroPatronal,
	                            srp.desc_reg_pat as nombreRegPatronal
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblP_CC AS tCCRH ON tEmpleados.cc_contable = tCCRH.ccRH
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                LEFT JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                LEFT JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
								LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat							
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public repLiberacionDTO GetInfoLiberacion(int clave_empleado)
        {
            var result = _context.Select<repLiberacionDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"        
	                 SELECT     
								tEmpleados.clave_empleado as idEmpleado,
								tEmpleados.jefe_inmediato as idJefe, 
                                tPuesto.descripcion as nombrePuesto,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,
                                tCC.descripcion as ccDescripcion,
                                tEmpleados.fecha_antiguedad as fechaAlta,
                                tBajas.fechabaja as fechaBaja,
                                tEmpleados.contratable as contratable,
								tRazBaja.desc_motivo_baja as motivo
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto								
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                LEFT JOIN tblRH_EK_Razones_Baja as tRazBaja on tBajas.motivoBajaDeSistema = tRazBaja.clave_razon_baja                               
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                parametros = new { clave_empleado }

            });
            return result.FirstOrDefault();
        }
        public repPagareDTO GetInfoPagare(int clave_empleado)
        {
            var result = _context.Select<repPagareDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"        
	                           SELECT      
                                tEmpleados.clave_empleado as idEmpleado,
								tEmpleados.jefe_inmediato as idJefe,     
                                srp.direccion as direccionPatronal,
	                            srp.desc_reg_pat as nombreRegPatronal,
                                (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,
								(tGrales.domicilio + ' ' + tGrales.num_ext_ben + ' ' +tGrales.colonia ) as domicilio,
                                (tCuidad.descripcion + ' ' + tEstado.descripcion) as poblacion,
                                prestamo.cantidadSoli,
                                prestamo.cantidadLetra,                               
	                            tGrales.tel_cel as telefono
                                FROM tblRH_EK_Empleados as tEmpleados
                                INNER JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
                                LEFT JOIN tblRH_EK_Empl_Grales as tGrales ON tEmpleados.clave_empleado = tGrales.clave_empleado								
                                LEFT JOIN tblRH_EK_Estados as tEstado ON (tEmpleados.clave_estado_nac = tEstado.clave_estado AND tEmpleados.clave_pais_nac = tEstado.clave_pais)
                                LEFT JOIN tblRH_EK_Ciudades as tCuidad ON (tEmpleados.clave_ciudad_nac = tCuidad.clave_cuidad AND tEmpleados.clave_estado_nac = tCuidad.clave_estado AND tEmpleados.clave_pais_nac = tCuidad.clave_pais)
                                LEFT JOIN tblRH_EK_Prestamos AS prestamo ON tEmpleados.clave_empleado = prestamo.clave_empleado
                                WHERE prestamo.registroActivo = 1 and tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                parametros = new { clave_empleado }

            });
            return result.FirstOrDefault();
        }
        public repLaboralDTO GetInfoLaboral(int clave_empleado)
        {
            var result = _context.Select<repLaboralDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"
                SELECT 	      
								tEmpleados.clave_empleado as idEmpleado,
								tInmediato.clave_empleado as idJefe,
	                            tPuesto.descripcion as nombrePuestoLab,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompletoLab,
                                tCC.descripcion as proyectoCCLab,
	                            tEmpleados.nss as numeroSeguroLab,
	                            tEmpleados.curp as curpLab,
	                            tEmpleados.rfc as rfcLab,
                                tn.descripcion as tipoNominaLab,
                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldoBaseLab,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complementoLab, 
	                            tEmpleados.fecha_antiguedad as fechaAltaLab,
                                tBajas.fechabaja as fechaBajaLab,
                                tEmpleados.contratable as contratable,
								srp.registro_patronal as numeroPatronal,
	                            srp.desc_reg_pat as nombreRegPatronal
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                LEFT JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                LEFT JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
								LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat	
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                parametros = new { clave_empleado }

            });
            return result.FirstOrDefault();
        }
        public repPrestamosDTO GetInfoPrestamos(int clave_empleado)
        {
            bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4051).Count()) > 0;

            var result = _context.Select<repPrestamosDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT top(1) 
                    prestamo.id,
                    tEmpleados.cc_contable as cc,
                    (tCC.cc + ' ' + tCC.descripcion) as ccDescripcion,	
                    tPuesto.puesto as puesto,
                    tPuesto.descripcion as nombrePuesto,	
                    tEmpleados.clave_empleado as clave_empleado,                         								                            							
	                (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,                               	                            
                    tEmpleados.fecha_antiguedad as fecha_alta,								
	                tn.descripcion as tipoNomina,
	                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldo_base,
	                (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento,                    
                    prestamo.totalN as totalN,
		            prestamo.totalM as totalM,
		            prestamo.otrosDescuento,
		            prestamo.cantidadMax,
		            prestamo.cantidadSoli,
		            prestamo.cantidadLetra,
		            prestamo.cantidadDescontar,								
		            prestamo.formaPago,
		            prestamo.justificacion,
                    prestamo.tipoSolicitud,
		            prestamo.tipoPrestamo,
		            prestamo.tipoPuesto,	
                    prestamo.empresa + ' ' + srp.nombre_corto as empresa,
                    prestamo.motivoPrestamo,
		            prestamo.idResponsableCC,
		            (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idResponsableCC))as nombreResponsableCC,
                    prestamo.idDirectorLineaN,
		            (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idDirectorLineaN))as nombreDirectorLineaN,
                    prestamo.idGerenteOdirector,
		            (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idGerenteOdirector))as nombreGerenteOdirector,
                    prestamo.idDirectorGeneral,
		            (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idDirectorGeneral))as nombreDirectorGeneral,
                    prestamo.idCapitalHumano,
		            (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idCapitalHumano))as nombreCapitalHumano,
					tabCat.concepto as descCategoriaPuesto,
                    prestamo.consecutivo
		            FROM tblRH_EK_Empleados as tEmpleados
                    INNER JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                    INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto                          
		            INNER JOIN tblRH_EK_Prestamos AS prestamo ON tEmpleados.clave_empleado = prestamo.clave_empleado
                    INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                    INNER JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
                    inner join tblRH_EK_Tabulador_Historial as s on tEmpleados.clave_empleado=s.clave_empleado AND s.esActivo = 1
                    LEFT JOIN
	                    tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = s.FK_TabuladorDet
                    LEFT JOIN 
	                    tblRH_TAB_CatCategorias tabCat ON tabDet.FK_Categoria = tabCat.id
			        WHERE prestamo.registroActivo = 1 and tEmpleados.clave_empleado = @clave_empleado"+
					" ORDER BY s.id DESC", //Agarra el usuario
                parametros = new {clave_empleado }
            });

            List<AutorizantesPerstamosDTO> lstAuth = new List<AutorizantesPerstamosDTO>();
            var objPrestamo = result.FirstOrDefault();
            var lstUsuario = _context.tblP_Usuario.Where(e => e.estatus).ToList();

            string cveEmpDirectorLinea = objPrestamo.idDirectorLineaN.ToString();
            string cveEmpGerente = objPrestamo.idGerenteOdirector.ToString();
            string cveEmpDirectoGral = objPrestamo.idDirectorGeneral.ToString();
            string cveEmpResponsableCC = objPrestamo.idResponsableCC.ToString();
            string cveEmpCH = objPrestamo.idCapitalHumano.ToString();

            #region RESPONSABLE CC

            tblP_Usuario objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpResponsableCC && w.estatus).FirstOrDefault();

            if (objPrestamo.idResponsableCC > 0)
            {
                var objAuthResponsableCC = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idResponsableCC && w.registroActivo);
                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                if (objAuthResponsableCC == null)
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idResponsableCC.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Responsable CC",
                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                        cveEmpleado = cveEmpResponsableCC
                    });
                }
                else
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idResponsableCC.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Responsable CC",
                        descEstatus = objAuthResponsableCC.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                        cveEmpleado = cveEmpResponsableCC
                    });

                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                    if (objAlerta != null)
                    {
                        objAlerta.visto = true;
                        _context.SaveChanges();
                    }
                }
            }
            #endregion

            #region DIRECTOR GENERAL

            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpDirectoGral && w.estatus).FirstOrDefault();
            if (objPrestamo.idDirectorGeneral > 0)
            {
                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                var objAuthDireGral = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorGeneral && w.registroActivo);

                if (objAuthDireGral == null)
                {

                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idDirectorGeneral.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Director General",
                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                        cveEmpleado = cveEmpDirectoGral
                    });
                }
                else
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idDirectorGeneral.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Director General",
                        descEstatus = objAuthDireGral.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                        cveEmpleado = cveEmpDirectoGral
                    });

                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                    if (objAlerta != null)
                    {
                        objAlerta.visto = true;
                        _context.SaveChanges();
                    }
                }
            }
            #endregion

            #region DIRE LINEA NEGOCIOS

            objUsuario = lstUsuario.FirstOrDefault(w => w.cveEmpleado == cveEmpDirectorLinea && w.estatus);

            if (objPrestamo.idDirectorLineaN > 0)
            {
                var objAutorizacionLineaNegocios = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorLineaN && w.registroActivo);
                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                if (objAutorizacionLineaNegocios == null)
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idDirectorLineaN.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Director de Linea de Negocios",
                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                        cveEmpleado = cveEmpDirectorLinea
                    });
                }
                else
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idDirectorLineaN.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Director de Linea de Negocios",
                        descEstatus = objAutorizacionLineaNegocios.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                        cveEmpleado = cveEmpDirectorLinea
                    });

                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                    if (objAlerta != null)
                    {
                        objAlerta.visto = true;
                        _context.SaveChanges();
                    }
                }
            }
            #endregion

            #region GERENTE/DIRECTOR

            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpGerente && w.estatus).FirstOrDefault();

            if (objPrestamo.idGerenteOdirector > 0)
            {
                var objAuthGerenteDirector = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idGerenteOdirector && w.registroActivo);
                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                if (objAuthGerenteDirector == null)
                {

                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idGerenteOdirector.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Gerente / Director",
                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                        cveEmpleado = cveEmpGerente
                    });
                }
                else
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idGerenteOdirector.Value,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Gerente / Director",
                        descEstatus = objAuthGerenteDirector.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                        cveEmpleado = cveEmpGerente
                    });

                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                    if (objAlerta != null)
                    {
                        objAlerta.visto = true;
                        _context.SaveChanges();
                    }
                }
            }
            #endregion

            #region CH

            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpCH && w.estatus).FirstOrDefault();

            if (objPrestamo.idCapitalHumano > 0)
            {
                var objAuthCH = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idCapitalHumano && w.registroActivo);
                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                if (objAuthCH == null)
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idCapitalHumano,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Capital Humano",
                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                        cveEmpleado = cveEmpCH
                    });
                }
                else
                {
                    lstAuth.Add(new AutorizantesPerstamosDTO
                    {
                        idUsuario = objPrestamo.idCapitalHumano,
                        nombreCompleto = nombreCompleto,
                        descPuesto = "Capital Humano",
                        descEstatus = objAuthCH.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                        cveEmpleado = cveEmpCH
                    });

                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                    if (objAlerta != null)
                    {
                        objAlerta.visto = true;
                        _context.SaveChanges();
                    }
                }
            }

            #endregion

            //SET LISTA DE AUTH
            objPrestamo.lstAuth = lstAuth;

            //PERMISO SALARIO
            //if (!permisoSueldos)
            //{
            //    objPrestamo.sueldo_base = 0;
            //    objPrestamo.complemento = 0;
            //    objPrestamo.bono_zona = 0;
            //    objPrestamo.totalM = 0;
            //    objPrestamo.totalN = 0;
            //}

            return objPrestamo;
        }
        public repFonacotDTO GetInfoFonacot(int clave_empleado)
        {
            var result = _context.Select<repFonacotDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"
                  SELECT 	      
                                tEmpleados.clave_empleado as idEmpleado,
								tInmediato.clave_empleado as idJefe,
	                            tPuesto.descripcion as nombrePuesto,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,                                
	                            tEmpleados.nss as imss,
	                            tEmpleados.curp as curp,
	                            tEmpleados.rfc as rfc,
                                tCC.descripcion as ccDescripcionFonacot,	
	                            tEmpleados.fecha_antiguedad as fechaIngreso,                                
								srp.registro_patronal as regPatron,
                                tn.descripcion as tipoNomina,
                                (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldoBase,
	                            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento, 
	                            srp.desc_reg_pat as nombrePatron
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                LEFT JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                LEFT JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
								LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                parametros = new { clave_empleado }

            });
            return result.FirstOrDefault();
        }
        public repGuarderiaDTO GetInfoGuarderia(int clave_empleado)
        {
            var result = _context.Select<repGuarderiaDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"
                 SELECT 	      
                                tEmpleados.clave_empleado as idEmpleado,
								tInmediato.clave_empleado as idJefe,
                                tCC.descripcion as descripcionCCGuard,
	                            tPuesto.descripcion as nombrePuestoGuard,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompletoGuard,                                
	                            tEmpleados.nss as imss,
	                            tEmpleados.curp as curp,
	                            tEmpleados.rfc as rfc,
	                            tEmpleados.fecha_antiguedad as fechaIngreso,                                
								srp.registro_patronal as regPatron,
	                            srp.desc_reg_pat as nombrePatron
                                FROM tblRH_EK_Empleados as tEmpleados
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto
                                LEFT JOIN tblRH_Baja_Registro as tBajas on tEmpleados.clave_empleado = tBajas.numeroEmpleado AND tBajas.registroActivo=1 AND tBajas.est_baja='A' AND tBajas.fechaBaja > tEmpleados.fecha_antiguedad
                                LEFT JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado
                                LEFT JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
								LEFT JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                parametros = new { clave_empleado }

            });
            return result.FirstOrDefault();
        }
        public repLactanciaDTO GetInfoLactancia(int clave_empleado)
        {
            var result = _context.Select<repLactanciaDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"
                  SELECT 	    
                                tEmpleados.clave_empleado as idEmpleado,
								tInmediato.clave_empleado as idJefe,
                                tEmpleados.clave_empleado as clave_empleado,                     
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompletoLact,   
								tEmpleados.sexo,
                                tPuesto.descripcion as nombrePuestoLact,
								tbInca.fechaInicio as fechaInicioInca,
								tbInca.fechaTerminacion as fechaFinInca								
                                FROM tblRH_EK_Empleados as tEmpleados								
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc   
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto 
								LEFT JOIN  tblRH_Vacaciones_Incapacidades as tbInca ON tEmpleados.clave_empleado = tbInca.clave_empleado
                                INNER JOIN tblRH_EK_Empleados as tInmediato ON tEmpleados.jefe_inmediato = tInmediato.clave_empleado	
                                WHERE tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario
                parametros = new { clave_empleado }

            });
            return result.FirstOrDefault();
        }

        #region CboFirmasPrestamos
        public Dictionary<string, object> GetResponsableCC(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<getEmpleadoFirmaPrestamos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                     SELECT 	      	                  
                                tEmpleados.clave_empleado,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,   
                                tPuesto.descripcion as descripcionPuesto										
                                FROM tblRH_EK_Empleados as tEmpleados								
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc 
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto 								
								where tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario 
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDirectorGeneral(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var result = _context.Select<getEmpleadoFirmaPrestamos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                     SELECT 	      	                  
                                tEmpleados.clave_empleado,
	                            (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,   
                                tPuesto.descripcion as descripcionPuesto										
                                FROM tblRH_EK_Empleados as tEmpleados								
                                LEFT JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc 
                                LEFT JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto 								
								where tEmpleados.clave_empleado = @clave_empleado", //Agarra el usuario 
                    parametros = new { clave_empleado }

                });
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillComboAutorizantesPrestamos(int clave_empleado, string tipoPrestamo)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener a los autorizantes.";
                if (clave_empleado <= 0) { throw new Exception(mensajeError); }
                if (string.IsNullOrEmpty(tipoPrestamo)) { throw new Exception(mensajeError); }
                #endregion

                List<ComboDTO> usuariosSinRepetir = new List<ComboDTO>();
                tblRH_EK_Empleados objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == clave_empleado);
                if (objEmpleado == null)
                    throw new Exception("Ocurrio algo mal con el empleado ingresado");

                List<ComboDTO> usuarios = new List<ComboDTO>();
                List<int> lstPlantillasRequisiciones = new List<int>();

                switch (tipoPrestamo)
                {
                    case "SINDICATO":
                        lstPlantillasRequisiciones.Add(104);
                        break;
                    case "MayorIgualA10":
                        lstPlantillasRequisiciones.Add(103);
                        break;
                    case "MenorA10":
                        lstPlantillasRequisiciones.Add(102);
                        break;
                    default:
                        break;
                }

                tblFA_Paquete paquete = _context.tblFA_Paquete.Where(x => x.cc.cc == objEmpleado.cc_contable).OrderByDescending(x => x.fechaCreacion).FirstOrDefault();
                if (paquete != null)
                {
                    foreach (var facultamiento in paquete.facultamientos.Where(x => lstPlantillasRequisiciones.Contains(x.plantillaID) && x.aplica))
                    {
                        foreach (var item in facultamiento.empleados.Where(x => x.esActivo && x.aplica))
                        {
                            ComboDTO usuario = new ComboDTO();
                            usuario.Value = item.claveEmpleado.ToString();
                            usuario.Text = item.nombreEmpleado;
                            usuario.Prefijo = item.conceptoID.ToString();
                            usuarios.Add(usuario);
                        }
                    }
                }

                foreach (var item in usuarios.GroupBy(x => x.Prefijo))
                {
                    usuariosSinRepetir.Add(item.First());
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, usuariosSinRepetir);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillCboCapitalHumano()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO CAPITAL HUMANO | MANUEL CRUZ
                string claveEmpleado = "113"; // MANUEL CRUZ
                tblP_Usuario objUsuario = _context.Select<tblP_Usuario>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT id, nombre, apellidoPaterno, apellidoMaterno, cveEmpleado FROM tblP_Usuario WHERE cveEmpleado = @claveEmpleado",
                    parametros = new { claveEmpleado = claveEmpleado }
                }).FirstOrDefault();
                if (objUsuario == null)
                    throw new Exception("Ocurrió un error al obtener el usuario de Capital Humano.");

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Value = objUsuario.cveEmpleado.ToString();
                objComboDTO.Text = GetNombreCompletoSIGOPLAN(objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno);
                lstComboDTO.Add(objComboDTO);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillCboCapitalHumano", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #endregion

        #region EXPEDICIONES
        public Dictionary<string, object> GetExpediciones(string cc, int? tipoReporte, int? claveEmpleado, string nombreEmpleado)
        {
            resultado.Clear();

            try
            {
                var result = _context.Select<ExpedicionesDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"
                            SELECT 
	                            tExp.*, (tEmpl.ape_paterno + ' ' + tEmpl.ape_materno + ' ' + tEmpl.nombre) AS nombreEmpleado, 
	                            (tUsr.apellidoPaterno + ' ' + tUsr.apellidoMaterno + ' ' + tUsr.nombre) AS nombreExpidio,
	                            '['+tCC.cc+'] '+tCC.ccDescripcion AS ccDesc,
								tArch.id AS idArchivoExp,
                                tCC.cc
                            FROM tblRH_REC_Expediciones AS tExp 
                            INNER JOIN tblRH_EK_Empleados AS tEmpl ON tExp.cveEmpleado = tEmpl.clave_empleado
                            INNER JOIN tblP_Usuario AS tUsr ON tExp.idUsuario = tUsr.id
                            INNER JOIN tblC_Nom_CatalogoCC AS tCC ON tExp.cc = tCC.cc
							INNER JOin tblRH_REC_Expediciones_Archivos AS tArch ON tExp.id = tArch.idExpedicion
                            WHERE tExp.esActivo = 1" +
                                (!string.IsNullOrEmpty(cc) ? " AND tExp.cc = @cc" : "") +
                                (tipoReporte.HasValue && tipoReporte.Value >= 0 ? " AND tExp.idReporte = @tipoReporte" : "") +
                                (claveEmpleado.HasValue && claveEmpleado.Value > 0 ? " AND tExp.cveEmpleado = @claveEmpleado" : "") +
                                (!string.IsNullOrEmpty(nombreEmpleado) ? " AND (tEmpl.ape_paterno + ' ' + tEmpl.ape_materno + ' ' + tEmpl.nombre) LIKE @nombreEmpleado" : "")
                                ,
                    parametros = new { cc, tipoReporte, claveEmpleado, nombreEmpleado = "%" + nombreEmpleado + "%" }
                });

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, result);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarExpediciones(tblRH_REC_Expediciones objExpedicion)
        {
            resultado.Clear();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == objExpedicion.cveEmpleado);

                    if (objEmpleado == null)
                    {
                        throw new Exception("Ocurrio algo mal con el empleado seleccionado");
                    }

                    int claveEmpleado = objEmpleado.clave_empleado > 0 ? objEmpleado.clave_empleado : 0;
                    int claveJefe = objEmpleado.jefe_inmediato > 0 ? objEmpleado.jefe_inmediato.Value : 0;
                    string firma = GlobalUtils.CrearFirmaDigital(claveEmpleado, DocumentosEnum.FirmasPrestamos, claveJefe);

                    //COMPLETAR LA INFO DEL OBJ
                    objExpedicion.idArchivo = 0;
                    objExpedicion.cveEmpleado = claveEmpleado;
                    objExpedicion.cc = objEmpleado.cc_contable;
                    //objExpedicion.idReporte = objExpedicion.idReporte;
                    objExpedicion.idUsuario = vSesiones.sesionUsuarioDTO.id;
                    objExpedicion.firmaElect = firma;
                    objExpedicion.fechaCreacion = DateTime.Now;
                    objExpedicion.fechaModificacion = DateTime.Now;
                    objExpedicion.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objExpedicion.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objExpedicion.esActivo = true;

                    _context.tblRH_REC_Expediciones.Add(objExpedicion);
                    //_context.tblRH_REC_Archivos.Add(objArchivo);
                    _context.SaveChanges();

                    var newExpedicion = _context.tblRH_REC_Expediciones.OrderByDescending(e => e.id).FirstOrDefault();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, newExpedicion);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, e.Message);
                    dbContextTransaction.Rollback();

                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarArchivoExpedicion(int idExp, Byte[] archivo)
        {
            resultado.Clear();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objExpedicionArchive = _context.tblRH_REC_Expediciones_Archivos.FirstOrDefault(e => e.idExpedicion == idExp);

                    if (objExpedicionArchive == null)
                    {

                        var objExpedicion = _context.tblRH_REC_Expediciones.FirstOrDefault(e => e.id == idExp);

#if DEBUG
                        var CarpetaNueva = Path.Combine(RutaServidorFormatosLocal, objExpedicion.cveEmpleado.ToString());
#else
                        var CarpetaNueva = Path.Combine(RutaServidorFormatos, objExpedicion.cveEmpleado.ToString());

#endif
                        ExisteCarpeta(CarpetaNueva, true);

                        //string nombreArchivo = (FormatosEnum)objExpedicion.idReporte+"_" + DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-") + ".pdf";
                        string nombreArchivo = "Formato_" + DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-") + ".pdf";
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);

                        _context.tblRH_REC_Expediciones_Archivos.Add(new tblRH_REC_Expediciones_Archivos
                        {
                            idExpedicion = idExp,
                            idReporte = objExpedicion.idReporte,
                            rutaArchivo = rutaArchivo,
                            fechaCreacion = DateTime.Now,
                            fechaModificacion = DateTime.Now,
                            idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id,
                            idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id,
                            esActivo = true,
                        });
                        _context.SaveChanges();

                        File.WriteAllBytes(rutaArchivo, archivo);

                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                    }


                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, e.Message);
                    dbContextTransaction.Rollback();

                }
            }

            return resultado;
        }

        private string ObtenerFormatoNombreArchivoA(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        private static bool ExisteCarpeta(string path, bool crear = false)
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

        public byte[] GetFirmaFormatos()
        {

            try
            {

                string rutaFirma = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\FIRMAS DIGITALES\firma digital francisco coronado.png";
#if DEBUG
                rutaFirma = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\FIRMAS DIGITALES\firma digital francisco coronado.png";
#endif

                byte[] imageArray = System.IO.File.ReadAllBytes(rutaFirma);
                //string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                return imageArray;
            }
            catch (Exception e)
            {
                string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                string targetPath = startupPath + "Content\\img\\nodisponible.png";
                System.Drawing.Image newImage = System.Drawing.Image.FromFile(targetPath);
                MemoryStream stream = new MemoryStream();
                newImage.Save(stream, newImage.RawFormat);
                byte[] data = stream.ToArray();
                return data;

                throw;
            }
        }

        public tblRH_REC_Expediciones_Archivos GetArchivoExpedicion(int idExpArchivo)
        {
            try
            {
                var objArchExpediente = _context.tblRH_REC_Expediciones_Archivos.FirstOrDefault(e => e.id == idExpArchivo);

                return objArchExpediente;
            }
            catch (Exception e)
            {

                return null;
            }
        }
        #endregion

        #region MODULO DE PRESTAMOS
        #region CAPTURA DE PRESTAMOS

        #endregion

        #region CONSULTA DE PRESTAMOS
        public Dictionary<string, object> NotificarPrestamo(int FK_Prestamo)
        {
            tblRH_EK_Prestamos objPrestamo = new tblRH_EK_Prestamos();
            repPrestamosDTO infoPrestamo = new repPrestamosDTO();

            try
            {
                objPrestamo = _context.tblRH_EK_Prestamos.Where(w => w.id == FK_Prestamo && w.registroActivo).FirstOrDefault();
                if (objPrestamo == null)
                    throw new Exception("Ocurrió un error al indicar que el prestamo fue notificado.");

                infoPrestamo = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT top(1) 
                            prestamo.id,
                            tEmpleados.cc_contable as cc,
                            (tCC.cc + ' ' + tCC.descripcion) as ccDescripcion,	
                            tPuesto.puesto as puesto,
                            tPuesto.descripcion as nombrePuesto,	
                            tEmpleados.clave_empleado as clave_empleado,                         								                            							
	                        (tEmpleados.ape_paterno + ' ' + tEmpleados.ape_materno + ' ' + tEmpleados.nombre) as nombreCompleto,                               	                            
                            tEmpleados.fecha_antiguedad as fecha_alta,								
	                        tn.descripcion as tipoNomina,
	                        (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as sueldo_base,
	                        (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = tEmpleados.clave_empleado order by id desc) as complemento,                    
                            prestamo.totalN as totalN,
		                    prestamo.totalM as totalM,
		                    prestamo.otrosDescuento,
		                    prestamo.cantidadMax,
		                    prestamo.cantidadSoli,
		                    prestamo.cantidadLetra,
		                    prestamo.cantidadDescontar,
		                    prestamo.formaPago,
		                    prestamo.justificacion,
                            prestamo.tipoSolicitud,
		                    prestamo.tipoPrestamo,
		                    prestamo.tipoPuesto,
                            prestamo.empresa + ' ' + srp.nombre_corto as empresa,
                            prestamo.motivoPrestamo,
                            prestamo.idResponsableCC,
		                    (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idResponsableCC))as nombreResponsableCC,
                            prestamo.idDirectorLineaN,
		                    (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idDirectorLineaN))as nombreDirectorLineaN,
                            prestamo.idGerenteOdirector,
		                    (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idGerenteOdirector))as nombreGerenteOdirector,
                            prestamo.idDirectorGeneral,
		                    (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idDirectorGeneral))as nombreDirectorGeneral,
                            prestamo.idCapitalHumano,
		                    (select top(1) apellidoPaterno+' '+ apellidoMaterno+' '+ nombre  from tblP_Usuario where cveEmpleado = convert(nvarchar,prestamo.idCapitalHumano))as nombreCapitalHumano,
					        tabCat.concepto as descCategoriaPuesto,
                            prestamo.consecutivo
		                    FROM tblRH_EK_Empleados as tEmpleados
                            INNER JOIN tblP_CC AS tCC ON tEmpleados.cc_contable = tCC.cc
                            INNER JOIN tblRH_EK_Puestos as tPuesto ON tEmpleados.puesto = tPuesto.puesto                          
		                    INNER JOIN tblRH_EK_Prestamos AS prestamo ON tEmpleados.clave_empleado = prestamo.clave_empleado
                            INNER JOIN tblRH_EK_Tipos_Nomina as tn on tEmpleados.tipo_nomina = tn.tipo_nomina
                            INNER JOIN tblRH_EK_Registros_Patronales AS srp ON tEmpleados.id_regpat = srp.clave_reg_pat
                            inner join tblRH_EK_Tabulador_Historial as s on tEmpleados.clave_empleado=s.clave_empleado AND s.esActivo = 1
                            LEFT JOIN
	                            tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = s.FK_TabuladorDet
                            LEFT JOIN 
	                            tblRH_TAB_CatCategorias tabCat ON tabDet.FK_Categoria = tabCat.id
			                WHERE prestamo.registroActivo = 1 and tEmpleados.clave_empleado = @clave_empleado
						    ORDER BY s.id DESC",
                    parametros = new { objPrestamo.clave_empleado }
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "NotificarPrestamo", e, AccionEnum.CONSULTA, FK_Prestamo, new { FK_Prestamo = FK_Prestamo });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (FK_Prestamo <= 0) { throw new Exception("Ocurrió un error al notificar el prestamo."); }
                    #endregion

                    #region SE INDICA QUE EL PRESTAMO SE ENCUENTRA EN ESTATUS NOTIFICADO


                    objPrestamo.notificadoParaGestion = true;
                    objPrestamo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objPrestamo.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    #region AUTORIZANTES
                    int idCC = GetCCID(objPrestamo.cc);
                    List<AutorizantesPerstamosDTO> lstAuth = new List<AutorizantesPerstamosDTO>();

                    // SE OBTIENE PAQUETE ID
                    tblFA_Paquete objPaquete = _context.tblFA_Paquete.Where(w => w.ccID == idCC).OrderByDescending(o => o.id).FirstOrDefault();
                    int paqueteID = 0;
                    if (objPaquete != null)
                        paqueteID = objPaquete.id;

                    // SE OBTIENE FACULTAMIENTO ID
                    int plantillaID = GetPlantillaID(objPrestamo.tipoPrestamo);
                    tblFA_Facultamiento objFacultamiento = _context.tblFA_Facultamiento.Where(w => w.plantillaID == plantillaID && w.paqueteID == paqueteID && w.aplica).FirstOrDefault();
                    int facultamientoID = objFacultamiento.id;     

                    //string claveEmpleado_PrimerAutorizante = lstEmpleadosRelFacultamiento[0].claveEmpleado.ToString();
              
                    var lstUsuario = _context.tblP_Usuario.Where(e => e.estatus).ToList();

                    // SE OBTIENE EL CORREO DEL CREADOR DEL PRESTAMO
                    tblP_Usuario objUsuarioCreadorPrestamo = lstUsuario.Where(w => w.id == objPrestamo.idUsuaioCreacion).FirstOrDefault();
                    if (objUsuarioCreadorPrestamo == null)
                        throw new Exception("No se encuentra el correo del creador del prestamo.");

                    if (string.IsNullOrEmpty(objUsuarioCreadorPrestamo.correo))
                        throw new Exception("No se encuentra el correo del primer autorizante.");

                    #region SE ENVIA CORREO A DIANA y KEYLA AL PRIMER AUTORIZANTE
                    List<string> lstCorreos = new List<string>();

                    #region AUTORIZANTES ORDENADOS
                    string cveEmpDirectorLinea = objPrestamo.idDirectorLineaN.ToString();
                    string cveEmpGerente = objPrestamo.idGerenteOdirector.ToString();
                    string cveEmpDirectoGral = objPrestamo.idDirectorGeneral.ToString();
                    string cveEmpResponsableCC = objPrestamo.idResponsableCC.ToString();
                    string cveEmpCH = objPrestamo.idCapitalHumano.ToString();

                    bool esAlertar = true;

                    #region RESPONSABLE CC

                    var objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpResponsableCC && w.estatus).FirstOrDefault();

                    if (objPrestamo.idResponsableCC > 0)
                    {
                        if (lstCorreos.Count() == 0)
                        {
                            lstCorreos.Add(objUsuario.correo);
                        }

                        var objAuthResponsableCC = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idResponsableCC && w.registroActivo);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                        if (objAuthResponsableCC == null)
                        {

                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                            //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                            //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                            objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                            objNuevaAlerta.objID = objPrestamo.id;
                            objNuevaAlerta.obj = "AutorizacionPrestamos";
                            objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            //CAMBIAR BANDERA
                            esAlertar = false;

                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Responsable CC",
                                descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                cveEmpleado = cveEmpResponsableCC
                            });
                        }
                        else
                        {
                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Responsable CC",
                                descEstatus = objAuthResponsableCC.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                cveEmpleado = cveEmpResponsableCC
                            });

                            var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                            if (objAlerta != null)
                            {
                                objAlerta.visto = true;
                                _context.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region GERENTE/DIRECTOR

                    objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpGerente && w.estatus).FirstOrDefault();

                    if (objPrestamo.idGerenteOdirector > 0)
                    {
                        if (lstCorreos.Count() == 0)
                        {
                            lstCorreos.Add(objUsuario.correo);
                        }

                        var objAuthGerenteDirector = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idGerenteOdirector && w.registroActivo);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        if (objAuthGerenteDirector == null)
                        {
                            if (esAlertar)
                            {
                                #region Alerta SIGOPLAN
                                tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                objNuevaAlerta.tipoAlerta = 2;
                                objNuevaAlerta.sistemaID = 16;
                                objNuevaAlerta.visto = false;
                                objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                objNuevaAlerta.objID = objPrestamo.id;
                                objNuevaAlerta.obj = "AutorizacionPrestamos";
                                objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                objNuevaAlerta.documentoID = 0;
                                objNuevaAlerta.moduloID = 0;
                                _context.tblP_Alerta.Add(objNuevaAlerta);
                                _context.SaveChanges();
                                #endregion //ALERTA SIGPLAN

                                //CAMBIAR BANDERA
                                esAlertar = false;
                            }

                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Gerente / Director",
                                descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                cveEmpleado = cveEmpGerente
                            });
                        }
                        else
                        {
                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Gerente / Director",
                                descEstatus = objAuthGerenteDirector.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                cveEmpleado = cveEmpGerente
                            });

                            var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                            if (objAlerta != null)
                            {
                                objAlerta.visto = true;
                                _context.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region DIRE LINEA NEGOCIOS
                    objUsuario = lstUsuario.FirstOrDefault(w => w.cveEmpleado == cveEmpDirectorLinea && w.estatus);

                    if (objPrestamo.idDirectorLineaN > 0)
                    {
                        lstCorreos.Add(objUsuario.correo);

                        var objAutorizacionLineaNegocios = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorLineaN && w.registroActivo);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                        if (objAutorizacionLineaNegocios == null)
                        {
                            if (esAlertar)
                            {
                                #region Alerta SIGOPLAN
                                tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                objNuevaAlerta.tipoAlerta = 2;
                                objNuevaAlerta.sistemaID = 16;
                                objNuevaAlerta.visto = false;
                                objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                objNuevaAlerta.objID = objPrestamo.id;
                                objNuevaAlerta.obj = "AutorizacionPrestamos";
                                objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                objNuevaAlerta.documentoID = 0;
                                objNuevaAlerta.moduloID = 0;
                                _context.tblP_Alerta.Add(objNuevaAlerta);
                                _context.SaveChanges();
                                #endregion //ALERTA SIGPLAN

                                //CAMBIAR BANDERA
                                esAlertar = false;
                            }

                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Director de Linea de Negocios",
                                descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                cveEmpleado = cveEmpDirectorLinea
                            });
                        }
                        else
                        {
                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Director de Linea de Negocios",
                                descEstatus = objAutorizacionLineaNegocios.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                cveEmpleado = cveEmpDirectorLinea
                            });

                            var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                            if (objAlerta != null)
                            {
                                objAlerta.visto = true;
                                _context.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region DIRECTOR GENERAL

                    objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpDirectoGral && w.estatus).FirstOrDefault();
                    if (objPrestamo.idDirectorGeneral > 0)
                    {
                        if (lstCorreos.Count() == 0)
                        {
                            lstCorreos.Add(objUsuario.correo);
                        }

                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        var objAuthDireGral = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorGeneral && w.registroActivo);

                        if (objAuthDireGral == null)
                        {

                            if (esAlertar)
                            {
                                #region Alerta SIGOPLAN
                                tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                objNuevaAlerta.tipoAlerta = 2;
                                objNuevaAlerta.sistemaID = 16;
                                objNuevaAlerta.visto = false;
                                objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                objNuevaAlerta.objID = objPrestamo.id;
                                objNuevaAlerta.obj = "AutorizacionPrestamos";
                                objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                objNuevaAlerta.documentoID = 0;
                                objNuevaAlerta.moduloID = 0;
                                _context.tblP_Alerta.Add(objNuevaAlerta);
                                _context.SaveChanges();
                                #endregion //ALERTA SIGPLAN

                                //CAMBIAR BANDERA
                                esAlertar = false;
                            }

                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Director General",
                                descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                cveEmpleado = cveEmpDirectoGral
                            });
                        }
                        else
                        {
                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Director General",
                                descEstatus = objAuthDireGral.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                cveEmpleado = cveEmpDirectoGral
                            });

                            var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                            if (objAlerta != null)
                            {
                                objAlerta.visto = true;
                                _context.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region CH

                    objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpCH && w.estatus).FirstOrDefault();

                    if (objPrestamo.idCapitalHumano > 0)
                    {
                        if (lstCorreos.Count() == 0)
                        {
                            lstCorreos.Add(objUsuario.correo);
                        }

                        var objAuthCH = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idCapitalHumano && w.registroActivo);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                        if (objAuthCH == null)
                        {

                            if (esAlertar)
                            {
                                #region Alerta SIGOPLAN
                                tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                objNuevaAlerta.tipoAlerta = 2;
                                objNuevaAlerta.sistemaID = 16;
                                objNuevaAlerta.visto = false;
                                objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                objNuevaAlerta.objID = objPrestamo.id;
                                objNuevaAlerta.obj = "AutorizacionPrestamos";
                                objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                objNuevaAlerta.documentoID = 0;
                                objNuevaAlerta.moduloID = 0;
                                _context.tblP_Alerta.Add(objNuevaAlerta);
                                _context.SaveChanges();
                                #endregion //ALERTA SIGPLAN

                                //CAMBIAR BANDERA
                                esAlertar = false;
                            }

                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Capital Humano",
                                descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                cveEmpleado = cveEmpCH
                            });
                        }
                        else
                        {
                            lstAuth.Add(new AutorizantesPerstamosDTO
                            {
                                nombreCompleto = nombreCompleto,
                                descPuesto = "Capital Humano",
                                descEstatus = objAuthCH.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                cveEmpleado = cveEmpCH
                            });

                            var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                            if (objAlerta != null)
                            {
                                objAlerta.visto = true;
                                _context.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #endregion
                    #endregion
                   
                    lstCorreos.Add(objUsuarioCreadorPrestamo.correo);
                    lstCorreos.Add("keyla.vasquez@construplan.com.mx");
                    lstCorreos.Add("diana.alvarez@construplan.com.mx");

                    #region CUERPO DEL CORREO

                    string cuerpo =
                                    @"<html>
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
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Buen día, prestamo listo para su gestion. CC " + GetCCDescripcion(objPrestamo.cc) + @"<br><br>
                                                    </p>
                                                    <br><br><br>
                                            ";

                    #region TABLA AUTORIZANTES
                    cuerpo += @"
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                    bool esPrimero = true;

                    //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                    foreach (var itemDet in lstAuth)
                    {
                        cuerpo += "<tr>" +
                                    "<td>" + itemDet.nombreCompleto + "</td>" +
                                    "<td>" + itemDet.descPuesto + "</td>" +
                                    getEstatus((int)itemDet.descEstatus, esPrimero) +
                                "</tr>";

                        if (esPrimero)
                        {
                            esPrimero = false;
                        }
                    }

                    cuerpo += "</tbody>" +
                                "</table>" +
                                "<br><br><br>";


                    #endregion

                    cuerpo += "<br><br><br>" +
                          "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                          "Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br><br>" +
                          "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                          "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                            " </body>" +
                          "</html>";
                    #endregion

                    #region PDF

                    string RutaServidor = "";
#if DEBUG
                    RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                    RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif

                    ReportDocument rptCV = new rptPrestamos();

                    //string path = Path.Combine(RutaServidor, "rptPrestamos.rpt");
                    //rptCV.Load(path);

                    //rd.SetParameterValue("ubicacion", "HERMOSILLO, SON");
                    rptCV.SetParameterValue("fechaActual", DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                    rptCV.SetParameterValue("ccDescripcion", infoPrestamo.ccDescripcion.Trim());
                    rptCV.SetParameterValue("nombrePuesto", infoPrestamo.nombrePuesto.Trim());
                    rptCV.SetParameterValue("categoriaPuesto", !string.IsNullOrEmpty(infoPrestamo.descCategoriaPuesto) ? (infoPrestamo.descCategoriaPuesto.Trim()) : "S/N");
                    rptCV.SetParameterValue("nombreCompleto", infoPrestamo.nombreCompleto.Trim());

                    string empresa = null;
                    if (infoPrestamo.empresa == empresa)
                    {
                        rptCV.SetParameterValue("empresa", "");
                    }
                    else
                    {
                        rptCV.SetParameterValue("empresa", infoPrestamo.empresa.Trim());
                    }

                    rptCV.SetParameterValue("fecha_alta", Convert.ToDateTime(infoPrestamo.fecha_alta).ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                    rptCV.SetParameterValue("tipoNomina", infoPrestamo.tipoNomina.Trim());
                    rptCV.SetParameterValue("sueldo_base", infoPrestamo.sueldo_base);
                    rptCV.SetParameterValue("complemento", infoPrestamo.complemento);
                    rptCV.SetParameterValue("bono_zona", infoPrestamo.bono_zona);
                    rptCV.SetParameterValue("totalN", infoPrestamo.totalN);
                    rptCV.SetParameterValue("totalM", infoPrestamo.totalM);

                    #region asignacion a casillas
                    string pago1 = "";
                    string pago2 = "";

                    if (infoPrestamo.formaPago == "12 Quincenas")
                    {
                        pago1 = "X";
                        pago2 = " ";
                        rptCV.SetParameterValue("formaPago12", pago1);
                        rptCV.SetParameterValue("formaPago24", pago2);
                    }
                    else
                    {
                        pago1 = " ";
                        pago2 = "X";
                        rptCV.SetParameterValue("formaPago12", pago1);
                        rptCV.SetParameterValue("formaPago24", pago2);

                    }


                    string salud = "";
                    string defuncion = "";
                    string daño = "";
                    string apoyo = "";
                    string sindicato = "";

                    if (infoPrestamo.motivoPrestamo == 1)
                    {
                        salud = "X";
                        defuncion = "";
                        daño = "";
                        apoyo = "";
                        sindicato = "";

                        rptCV.SetParameterValue("motivoSalud", salud);
                        rptCV.SetParameterValue("motivoDef", defuncion);
                        rptCV.SetParameterValue("motivoDaño", daño);
                        rptCV.SetParameterValue("motivoApoyo", apoyo);
                        rptCV.SetParameterValue("motivoSindicato", sindicato);

                    }
                    else if (infoPrestamo.motivoPrestamo == 2)
                    {
                        salud = "";
                        defuncion = "X";
                        daño = "";
                        apoyo = "";
                        sindicato = "";

                        rptCV.SetParameterValue("motivoSalud", salud);
                        rptCV.SetParameterValue("motivoDef", defuncion);
                        rptCV.SetParameterValue("motivoDaño", daño);
                        rptCV.SetParameterValue("motivoApoyo", apoyo);
                        rptCV.SetParameterValue("motivoSindicato", sindicato);

                    }
                    else if (infoPrestamo.motivoPrestamo == 3)
                    {
                        salud = "";
                        defuncion = "";
                        daño = "X";
                        apoyo = "";
                        sindicato = "";

                        rptCV.SetParameterValue("motivoSalud", salud);
                        rptCV.SetParameterValue("motivoDef", defuncion);
                        rptCV.SetParameterValue("motivoDaño", daño);
                        rptCV.SetParameterValue("motivoApoyo", apoyo);
                        rptCV.SetParameterValue("motivoSindicato", sindicato);
                    }
                    else if (infoPrestamo.motivoPrestamo == 4)
                    {
                        salud = "";
                        defuncion = "";
                        daño = "";
                        apoyo = "X";
                        sindicato = "";

                        rptCV.SetParameterValue("motivoSalud", salud);
                        rptCV.SetParameterValue("motivoDef", defuncion);
                        rptCV.SetParameterValue("motivoDaño", daño);
                        rptCV.SetParameterValue("motivoApoyo", apoyo);
                        rptCV.SetParameterValue("motivoSindicato", sindicato);
                    }
                    else if (infoPrestamo.motivoPrestamo == 5)
                    {
                        salud = "";
                        defuncion = "";
                        daño = "";
                        apoyo = "";
                        sindicato = "X";

                        rptCV.SetParameterValue("motivoSalud", salud);
                        rptCV.SetParameterValue("motivoDef", defuncion);
                        rptCV.SetParameterValue("motivoDaño", daño);
                        rptCV.SetParameterValue("motivoApoyo", apoyo);
                        rptCV.SetParameterValue("motivoSindicato", sindicato);
                    }

                    if (infoPrestamo.cantidadSoli < 10000)
                    {
                        rptCV.SetParameterValue("IFE", "X");
                        rptCV.SetParameterValue("SOPORTE", "X");
                        rptCV.SetParameterValue("PAGARE", "");
                    }
                    if (infoPrestamo.cantidadSoli >= 10000)
                    {
                        rptCV.SetParameterValue("IFE", "X");
                        rptCV.SetParameterValue("SOPORTE", "X");
                        rptCV.SetParameterValue("PAGARE", "X");
                    }
                    #endregion

                    string descPrestamos = "";

                    if (infoPrestamo.tipoPrestamo == "MayorIgualA10")
                    {
                        descPrestamos = "MAYOR O IGUAL A $10,000.00";

                    }
                    else if (infoPrestamo.tipoPrestamo == "MenorA10")
                    {
                        descPrestamos = "MENOR A $10,000.00";

                    }
                    else
                    {
                        descPrestamos = " ";

                    }

                    rptCV.SetParameterValue("cantSoli", infoPrestamo.cantidadSoli);
                    rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra);
                    rptCV.SetParameterValue("cantMax", infoPrestamo.cantidadMax);
                    rptCV.SetParameterValue("cantDescontar", infoPrestamo.cantidadDescontar);
                    rptCV.SetParameterValue("otrosDesc", infoPrestamo.otrosDescuento);
                    rptCV.SetParameterValue("justificacion", infoPrestamo.justificacion.Trim());
                    rptCV.SetParameterValue("tipoSolicitud", infoPrestamo.tipoSolicitud.Trim());
                    rptCV.SetParameterValue("tipoPrestamo", descPrestamos);
                    rptCV.SetParameterValue("tipoPuesto", infoPrestamo.tipoPuesto.Trim());
                    rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra.Trim());

                    if (infoPrestamo.idResponsableCC == null || infoPrestamo.idGerenteOdirector == null || infoPrestamo.idDirectorGeneral == null || infoPrestamo.idDirectorLineaN == null)
                    {
                        rptCV.SetParameterValue("nombreResponsableCC", "");
                        rptCV.SetParameterValue("puestoResponsableCC", "");
                        rptCV.SetParameterValue("nombreDirectorGeneral", "");
                        rptCV.SetParameterValue("puestoDirectorGeneral", "");
                        rptCV.SetParameterValue("nombreDirectorLineaN", "");
                        rptCV.SetParameterValue("puestoDirectorLineaN", "");
                        rptCV.SetParameterValue("nombreGerenteOdirector", "");
                        rptCV.SetParameterValue("puestoGerenteOdirector", "");
                        rptCV.SetParameterValue("nombreCapitalHumano", "");
                        rptCV.SetParameterValue("puestoCapitalHumano", "");
                    }
                    else
                    {
                        rptCV.SetParameterValue("nombreResponsableCC", infoPrestamo != null && infoPrestamo.idResponsableCC > 0 ? infoPrestamo.nombreResponsableCC.Trim() : string.Empty);
                        rptCV.SetParameterValue("puestoResponsableCC", "RESPONSABLE DE CC");
                        rptCV.SetParameterValue("nombreDirectorGeneral", infoPrestamo != null && infoPrestamo.idDirectorGeneral > 0 ? infoPrestamo.nombreDirectorGeneral.Trim() : string.Empty);
                        rptCV.SetParameterValue("puestoDirectorGeneral", "DIRECTOR GENERAL");
                        rptCV.SetParameterValue("nombreDirectorLineaN", infoPrestamo != null && infoPrestamo.idDirectorLineaN > 0 ? infoPrestamo.nombreDirectorLineaN.Trim() : string.Empty);
                        rptCV.SetParameterValue("puestoDirectorLineaN", "DIRECTOR LINEA DE NEGOCIO");
                        rptCV.SetParameterValue("nombreGerenteOdirector", infoPrestamo != null && infoPrestamo.idGerenteOdirector > 0 ? infoPrestamo.nombreGerenteOdirector.Trim() : string.Empty);
                        rptCV.SetParameterValue("puestoGerenteOdirector", "GERENTE/DIRECTOR DE AREA");
                        rptCV.SetParameterValue("nombreCapitalHumano", infoPrestamo != null && infoPrestamo.idCapitalHumano > 0 ? infoPrestamo.nombreCapitalHumano.Trim() : string.Empty);
                        rptCV.SetParameterValue("puestoCapitalHumano", "GERENTE/DIRECTOR DE CAPITAL HUMANO");

                        //Firmas
                        rptCV.SetParameterValue("FirmaResponsableCC", " ");
                        rptCV.SetParameterValue("FirmaSolicitante", GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.clave_empleado)));
                        rptCV.SetParameterValue("FirmaDirectorGeneral", " ");
                        rptCV.SetParameterValue("FirmaDirectorLineaN", " ");
                        rptCV.SetParameterValue("FirmaGerenteOdirector", " ");
                        rptCV.SetParameterValue("FirmaCapitalHumano", " ");

                    }

                    //FOLIO
                    rptCV.SetParameterValue("folio", ((infoPrestamo.cc == null ? "" : infoPrestamo.cc) + "-" + (infoPrestamo.clave_empleado) + "-" + (infoPrestamo.consecutivo.ToString().PadLeft(3, '0'))));

                    Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                    #endregion

#if DEBUG
                    lstCorreos = new List<string>();
                    lstCorreos.Add("aaron.gracia@construplan.com.mx");
                    lstCorreos.Add("miguel.buzani@construplan.com.mx");
#endif
                    string subject = "Prestamos";
                    //string body = string.Format("Buen día, se informa que el prestamo del CC {0}, se encuentra listo para ser autorizado" +
                    //                            "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br>" +
                    //                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>)." +
                    //                            "No es necesario dar una respuesta. Gracias.", GetCCDescripcion(objPrestamo.cc));
                    //GlobalUtils.sendEmail(subject, body, lstCorreos);

                    var lstArchives = new List<adjuntoCorreoDTO>();

                    List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.FK_Prestamo == FK_Prestamo && w.registroActivo).ToList();

                    foreach (var item in lstArchivos)
                    {
                        var fileStream = GlobalUtils.GetFileAsStream(item.rutaArchivo);
                        using (var streamReader = new MemoryStream()){
                            fileStream.CopyTo(streamReader);
                            //downloadFiles.Add(streamReader.ToArray());

                            lstArchives.Add(new adjuntoCorreoDTO
                            {
                                archivo = streamReader.ToArray(),
                                nombreArchivo = Path.GetFileNameWithoutExtension(item.rutaArchivo),
                                extArchivo = Path.GetExtension(item.rutaArchivo)
                            });
                        }
                    }

                    using (var streamReader = new MemoryStream())
                    {
                        stream.CopyTo(streamReader);
                        //downloadFiles.Add(streamReader.ToArray());
                        lstArchives.Add(new adjuntoCorreoDTO
                        {
                            archivo = streamReader.ToArray(),
                            nombreArchivo = "Prestamo",
                            extArchivo = ".pdf"
                        });

                        GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject),
                            cuerpo, lstCorreos, lstArchives);
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha notificado con éxito");

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "NotificarPrestamo", e, AccionEnum.CONSULTA, FK_Prestamo, new { FK_Prestamo = FK_Prestamo });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        private string GetCCDescripcion(string cc)
        {
            string DescripcionCC = string.Empty;
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la descripción del CC.";
                if (string.IsNullOrEmpty(cc)) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA DESCRIPCIÓN DEL CC
                tblP_CC objCC = _context.tblP_CC.Where(w => w.cc == cc && w.estatus).FirstOrDefault();
                if (objCC == null)
                    throw new Exception(mensajeError);

                DescripcionCC = string.Format("[{0}] {1}", objCC.cc.Trim(), objCC.descripcion.Trim());
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetCCDescripcion", e, AccionEnum.CONSULTA, 0, new { cc = cc });
                return DescripcionCC;
            }
            return DescripcionCC;
        }

        public Dictionary<string, object> GetPrestamosFiltro(FiltroPrestamosDTO objFiltro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool permisoGerencia = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4050).Count()) > 0;

                #region SE OBTIENE LISTADO DE PRESTAMOS
                List<repPrestamosDTO> lstPrestamos = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.clave_empleado, t1.id, t2.nombre, t2.ape_paterno, t2.ape_materno, t3.descripcion AS DescripcionPuesto,t1.tipoPuesto AS tipoPuesto,
                                    '[' + t2.cc_contable + '] ' + t4.descripcion AS ccDescripcion, t1.otrosDescuento, t1.cantidadSoli, t1.tipoPrestamo,
                                    t1.idUsuaioCreacion, t1.fecha_creacion, t1.fechaModificacion, t1.estatus, t1.cantidadDescontar, t1.formaPago, t1.cc, t1.motivoPrestamo, t1.consecutivo
	                            FROM tblRH_EK_Prestamos AS t1
	                            INNER JOIN tblRH_EK_Empleados AS t2 ON t2.clave_empleado = t1.clave_empleado
	                            INNER JOIN tblRH_EK_Puestos AS t3 ON t3.puesto = t2.puesto
	                            INNER JOIN tblP_CC AS t4 ON t4.cc = t2.cc_contable
		                        WHERE t1.registroActivo = @registroActivo AND t2.esActivo = @esActivo AND t2.estatus_empleado = @estatus_empleado" +
                                (permisoGerencia ? "" : " AND (t3.descripcion NOT LIKE '%GERENCIA%' AND t3.descripcion NOT LIKE '%GERENTE%' AND t3.descripcion NOT LIKE '%DIRECCI%' AND t3.descripcion NOT LIKE '%DIRECTOR%')"),
                    parametros = new { registroActivo = true, esActivo = true, estatus_empleado = "A", estatus = true }
                }).ToList();

                #region FILTROS
                if (objFiltro.lstCC != null)
                {
                    List<string> lstCC = new List<string>();
                    foreach (var item in objFiltro.lstCC)
                    {
                        if (!string.IsNullOrEmpty(item))
                            lstCC.Add(item.Trim().ToUpper());
                    }

                    if (lstCC.Count() > 0)
                        lstPrestamos = lstPrestamos.Where(w => lstCC.Contains(w.cc)).ToList();
                }

                if (!string.IsNullOrEmpty(objFiltro.estatus))
                    lstPrestamos = lstPrestamos.Where(w => w.estatus == objFiltro.estatus).ToList();

                if (!string.IsNullOrEmpty(objFiltro.tipoPrestamo))
                {
                    if (objFiltro.tipoPrestamo == "SINDICATO")
                    {
                        lstPrestamos = lstPrestamos.Where(w => w.motivoPrestamo == 5).ToList();
                        
                    }
                    else
                    {
                        lstPrestamos = lstPrestamos.Where(w => w.motivoPrestamo != 5).ToList();
                    }
                }

                if (objFiltro.cantidad > 0)
                {
                    if (objFiltro.cantidad == 10000)
                    {
                        lstPrestamos = lstPrestamos.Where(w => w.cantidadSoli >= objFiltro.cantidad).ToList();

                    }
                    else
                    {
                        lstPrestamos = lstPrestamos.Where(w => w.cantidadSoli <= objFiltro.cantidad).ToList();

                    }
                }

                if (objFiltro.fechaInicio != null && objFiltro.fechaFin != null)
                {
                    lstPrestamos = lstPrestamos.Where(w => w.fecha_creacion >= objFiltro.fechaInicio.Value.Date && w.fecha_creacion <= objFiltro.fechaFin).ToList();
                }
                #endregion

                #region SE OBTIENE LISTADO DE ARCHIVOS DE LOS PRESTAMOS
                List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.registroActivo).ToList();
                #endregion

                foreach (var item in lstPrestamos)
                {
                    #region SE OBTIENE EL NOMBRE COMPLETO DEL USUARIO
                    item.nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        item.nombreCompleto = item.nombre.Trim();
                    if (!string.IsNullOrEmpty(item.ape_paterno))
                        item.nombreCompleto += string.Format(" {0}", item.ape_paterno.Trim());
                    if (!string.IsNullOrEmpty(item.ape_materno))
                        item.nombreCompleto += string.Format(" {0}", item.ape_materno.Trim());
                    #endregion

                    #region SE OBTIENE EL TIPO DE PRESTAMO Y SE VERIFICA SI CUENTA CON LA DOCUMENTACIÓN NECESARIA PARA PASAR A LA GESTIÓN DE FIRMAS
                    int CantArchivosPrestamo = lstArchivos.Where(w => w.FK_Prestamo == item.id).Count();

                    if (item.tipoPuesto == "SINDICALIZADO")
                    {
                        item.esSindicalizado = CantArchivosPrestamo >= 1 ? true : false;
                        switch (item.tipoPrestamo)
                        {
                            case "SINDICATO":
                                item.descripcionTipoPrestamo = "Sindicato";
                                break;
                            case "MayorIgualA10":
                                item.descripcionTipoPrestamo = "Mayor o igual a $10,000.00";
                                break;
                            case "MenorA10":
                                item.descripcionTipoPrestamo = "Menor a $10,000.00";
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        switch (item.tipoPrestamo)
                        {
                            case "SINDICATO":
                                item.descripcionTipoPrestamo = "Sindicato";
                                item.puedeAutorizarRechazar = CantArchivosPrestamo >= 2 ? true : false;
                                break;
                            case "MayorIgualA10":
                                item.descripcionTipoPrestamo = "Mayor o igual a $10,000.00";
                                item.puedeAutorizarRechazar = CantArchivosPrestamo >= 3 ? true : false;
                                break;
                            case "MenorA10":
                                item.descripcionTipoPrestamo = "Menor a $10,000.00";
                                item.puedeAutorizarRechazar = CantArchivosPrestamo >= 2 ? true : false;
                                break;
                            default:
                                break;
                        }
                    }
                    
                    #endregion

                    var objUsuarioCreacion = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.idUsuaioCreacion);

                    item.nombreUsuarioCreacion = objUsuarioCreacion != null ? (objUsuarioCreacion.apellidoPaterno + " " + objUsuarioCreacion.apellidoMaterno + " " + objUsuarioCreacion.nombre) : "";

                    //MONTOS DESCONTADOS HASTA EL DIA DE HOY
                    if (item.estatus == "A")
	                {

                        var numDias = (DateTime.Now.Date - item.fechaModificacion.Value.Date).Days;

		                if (item.formaPago == "12 Quincenas")
                        {
                            int multiplo = (int) (numDias / 15);

                            if (multiplo > 12)
	                        {
		                        multiplo = 12;
	                        }

                            item.montoDescontados = (item.cantidadDescontar * multiplo);
                        }
                        else if (item.formaPago == "24 Semanas")
                        {
                            int multiplo = (int) (numDias / 7);

                            if (multiplo > 24)
                            {
                                multiplo = 24;
                            }

                            item.montoDescontados = (item.cantidadDescontar * multiplo);
                        }
	                }
                }
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstPrestamos);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "GetPrestamosFiltro", ex, AccionEnum.CONSULTA, 0, new
                {
                    lstCC = objFiltro.lstCC,
                    estatus = objFiltro.estatus,
                    tipoPrestamo = objFiltro.tipoPrestamo,
                    cantidad = objFiltro.cantidad,
                    fechaInicio = objFiltro.fechaInicio,
                    fechaFin = objFiltro.fechaFin
                });
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CARGA DE ARCHIVOS
        public Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Prestamo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region VALIDACIONES
                    if (FK_Prestamo <= 0) { throw new Exception("Ocurrió un error al registar el archivo."); }              

                    #endregion

                    #region SE REGISTRA EL ARCHIVO ADJUNTO
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                    var CarpetaNueva = Path.Combine(RutaLocal, FK_Prestamo.ToString());
#else
                    var CarpetaNueva = Path.Combine(RutaBase, FK_Prestamo.ToString());
#endif
                    // Verifica si existe la carpeta y si no, la crea.
                    if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                        return resultado;
                    }

                    foreach (var objArchivo in lstArchivos)
                    {
                        string nombreArchivo = SetNombreArchivo("Prestamo", objArchivo.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(objArchivo, rutaArchivo));

                        // GUARDAR TABLA ARCHIVOS
                        tblRH_EK_PrestamosArchivos objEvidencia = new tblRH_EK_PrestamosArchivos()
                        {
                            FK_Prestamo = FK_Prestamo,
                            nombreArchivo = nombreArchivo,
                            rutaArchivo = rutaArchivo,
                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        };

                        _context.tblRH_EK_PrestamosArchivos.Add(objEvidencia);
                        _context.SaveChanges();
                    }

                    foreach (var objArchivo in listaRutaArchivos)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(objArchivo.Item1, objArchivo.Item2) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(MESSAGE, "Se ha registado con éxito.");
                    resultado.Add(SUCCESS, true);
                    #endregion
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarArchivoAdjunto", e, AccionEnum.AGREGAR, FK_Prestamo, new { lstArchivos = lstArchivos, FK_Prestamo = FK_Prestamo });
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarArchivoAdjuntoEnCaptura(List<HttpPostedFileBase> lstArchivos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region VALIDACIONES
                    if (lstArchivos == null) { throw new Exception("Prestamo ingresado con exito. Faltan evidencias por cargar, favor de cargarlas en la pantalla de captura."); }

                    int FK_Prestamo = _context.tblRH_EK_Prestamos.OrderByDescending(x => x.id).First().id;

                    //int fkPrestamo = ultimoIdPrestamo.id;
                    #endregion

                    #region SE REGISTRA EL ARCHIVO ADJUNTO
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                    var CarpetaNueva = Path.Combine(RutaLocal, FK_Prestamo.ToString());
#else
                    var CarpetaNueva = Path.Combine(RutaBase, FK_Prestamo.ToString());
#endif
                    // Verifica si existe la carpeta y si no, la crea.
                    if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                        return resultado;
                    }

                    foreach (var objArchivo in lstArchivos)
                    {
                        string nombreArchivo = SetNombreArchivo("Prestamo", objArchivo.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(objArchivo, rutaArchivo));

                        // GUARDAR TABLA ARCHIVOS
                        tblRH_EK_PrestamosArchivos objEvidencia = new tblRH_EK_PrestamosArchivos()
                        {
                            FK_Prestamo = FK_Prestamo,
                            nombreArchivo = nombreArchivo,
                            rutaArchivo = rutaArchivo,
                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        };

                        _context.tblRH_EK_PrestamosArchivos.Add(objEvidencia);
                        _context.SaveChanges();
                    }

                    foreach (var objArchivo in listaRutaArchivos)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(objArchivo.Item1, objArchivo.Item2) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(MESSAGE, "Se han cargado las evidencias con éxito, Favor de consultar en en apartado de Consultas.");
                    resultado.Add(SUCCESS, true);
                    #endregion
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        private string SetNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0}{1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        public Dictionary<string, object> GetArchivosAdjuntos(int FK_Prestamo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (FK_Prestamo <= 0) { throw new Exception("Ocurrió un error al obtener el listado de archivos adjuntos."); }
                #endregion

                #region SE OBTIENE LISTADO DE ARCHIVOS ADJUNTOS EN BASE AL ACTO SELECCIONADO
                List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.FK_Prestamo == FK_Prestamo && w.registroActivo).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("lstArchivos", lstArchivos);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetArchivosAdjuntos", e, AccionEnum.CONSULTA, 0, new { FK_Prestamo = FK_Prestamo });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                tblRH_EK_PrestamosArchivos objArchivoAdjunto = _context.tblRH_EK_PrestamosArchivos.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();
                if (objArchivoAdjunto == null)
                    throw new Exception("Ocurrió un error al visualizar el archivo.");

                //Stream fileStream = GlobalUtils.GetFileAsStream(objArchivoAdjunto.rutaArchivo);
                //var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                //resultado.Add("archivo", byteArray);
                //resultado.Add("extension", Path.GetExtension(objArchivoAdjunto.rutaArchivo).ToUpper());
                resultado.Add("ruta", objArchivoAdjunto.rutaArchivo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "VisualizarArchivoAdjunto", e, AccionEnum.CONSULTA, idArchivo, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idArchivo <= 0) { throw new Exception("Ocurrió un error al eliminar el archivo."); }
                #endregion

                #region SE ELIMINA EL ARCHIVO
                tblRH_EK_PrestamosArchivos objEliminar = _context.tblRH_EK_PrestamosArchivos.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();
                if (objEliminar == null)
                    throw new Exception("Ocurrió un error al eliminar el archivo.");

                objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                objEliminar.fechaModificacion = DateTime.Now;
                objEliminar.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "EliminarArchivoAdjunto", e, AccionEnum.ELIMINAR, idArchivo, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region GESTIÓN DE PRESTAMOS (AUTORIZACIONES)
        //public Dictionary<string, object> AutorizarRechazarPrestamo(AutorizarRechazarPrestamoDTO objFiltroDTO)
        //{
        //    resultado = new Dictionary<string, object>();
        //    try
        //    {
        //        #region VALIDACIONES
        //        string mensajeError = string.Format("Ocurrió un error al {0} el prestamo.", objFiltroDTO.esPrestamoAutorizado ? "autorizar" : "rechazar");
        //        if (objFiltroDTO.FK_Prestamo <= 0) { throw new Exception(mensajeError); }
        //        if (string.IsNullOrEmpty(objFiltroDTO.tipoAutorizante)) { throw new Exception(mensajeError); }
        //        if (objFiltroDTO.cveEmpleado <= 0) { throw new Exception(mensajeError); }
        //        #endregion

        //        #region SE AUTORIZA/RECHAZA EL PRESTAMO
        //        tblRH_EK_PrestamosAutorizaciones objPrestamo = new tblRH_EK_PrestamosAutorizaciones();
        //        objPrestamo.FK_Prestamo = objFiltroDTO.FK_Prestamo;
        //        objPrestamo.tipoAutorizante = objFiltroDTO.tipoAutorizante;
        //        objPrestamo.cveEmpleado = objFiltroDTO.cveEmpleado;
        //        objPrestamo.esPrestamoAutorizado = objFiltroDTO.esPrestamoAutorizado;
        //        objPrestamo.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
        //        objPrestamo.fechaCreacion = DateTime.Now;
        //        objPrestamo.registroActivo = true;
        //        _context.tblRH_EK_PrestamosAutorizaciones.Add(objPrestamo);
        //        _context.SaveChanges();
        //        #endregion

        //        resultado.Add(SUCCESS, true);
        //        resultado.Add(MESSAGE, objFiltroDTO.esPrestamoAutorizado ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(0, 0, NombreControlador, "AutorizarRechazarPrestamo", e, AccionEnum.CONSULTA, objFiltroDTO.FK_Prestamo, objFiltroDTO);
        //        resultado.Add(MESSAGE, e.Message);
        //        resultado.Add(SUCCESS, false);
        //    }
        //    return resultado;
        //}

        public Dictionary<string, object> GetListadoAutorizantes(int idPrestamo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idPrestamo <= 0) throw new Exception("Ocurrió un error al obtener el listado de autorizantes.");
                #endregion

                #region SE OBTIENE LISTADO DE AUTORIZANTES
                repPrestamosDTO objPrestamo = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idDirectorLineaN, idGerenteOdirector, idDirectorGeneral, idResponsableCC, idCapitalHumano, estatus, tipoPrestamo, cc
	                                    FROM tblRH_EK_Prestamos
		                                    WHERE id = @idPrestamo AND registroActivo = @registroActivo",
                    parametros = new { idPrestamo = idPrestamo, registroActivo = true }
                }).FirstOrDefault();

                #region SE OBTIENE ORDEN DE LOS AUTORIZADORES EN BASE A LOS FACULTAMIENTOS
                int idCC = GetCCID(objPrestamo.cc);

                // SE OBTIENE PAQUETE ID
                tblFA_Paquete objPaquete = _context.tblFA_Paquete.Where(w => w.ccID == idCC).OrderByDescending(o => o.id).FirstOrDefault();
                int paqueteID = 0;
                if (objPaquete != null)
                    paqueteID = objPaquete.id;

                // SE OBTIENE FACULTAMIENTO ID
                int plantillaID = GetPlantillaID(objPrestamo.tipoPrestamo);
                tblFA_Facultamiento objFacultamiento = _context.tblFA_Facultamiento.Where(w => w.plantillaID == plantillaID && w.paqueteID == paqueteID && w.aplica).FirstOrDefault();
                int facultamientoID = objFacultamiento.id;

                // SE OBTIENE LISTADO DE AUTORIZANTES EN BASE AL FACULTAMIENTO DEL CC
                List<tblFA_Empleado> lstEmpleadosRelFacultamiento = _context.tblFA_Empleado.Where(w => w.facultamientoID == facultamientoID && w.aplica && w.esActivo).ToList();
                #endregion

                #region NOMBRE AUTORIZANTES
                List<repPrestamosDTO> lstAutorizantesOrdenado = new List<repPrestamosDTO>();
                List<repPrestamosDTO> lstAutorizantes = new List<repPrestamosDTO>();
                repPrestamosDTO objAutorizante = new repPrestamosDTO();

                if (objPrestamo.idResponsableCC > 0)
                {
                    objAutorizante = new repPrestamosDTO();
                    string claveEmpleado = objPrestamo.idResponsableCC.ToString();
                    tblP_Usuario objEmpleado = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado).FirstOrDefault();
                    objAutorizante.tipoPuesto = "Responsable CC";
                    objAutorizante.nombreCompleto = GetNombreCompletoSIGOPLAN(objEmpleado.nombre, objEmpleado.apellidoPaterno, objEmpleado.apellidoMaterno);
                    objAutorizante.estatus = GetEstatusPrestamo(idPrestamo, objPrestamo.estatus, (int)objPrestamo.idResponsableCC);
                    lstAutorizantes.Add(objAutorizante);
                }

                if (objPrestamo.idGerenteOdirector > 0)
                {
                    objAutorizante = new repPrestamosDTO();
                    string claveEmpleado = objPrestamo.idGerenteOdirector.ToString();
                    tblP_Usuario objEmpleado = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado).FirstOrDefault();
                    objAutorizante.tipoPuesto = "Gerente Área";
                    objAutorizante.nombreCompleto = GetNombreCompletoSIGOPLAN(objEmpleado.nombre, objEmpleado.apellidoPaterno, objEmpleado.apellidoMaterno);
                    objAutorizante.estatus = GetEstatusPrestamo(idPrestamo, objPrestamo.estatus, (int)objPrestamo.idGerenteOdirector);
                    lstAutorizantes.Add(objAutorizante);
                }

                if (objPrestamo.idDirectorLineaN > 0)
                {
                    objAutorizante = new repPrestamosDTO();
                    string claveEmpleado = objPrestamo.idDirectorLineaN.ToString();
                    tblP_Usuario objEmpleado = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado).FirstOrDefault();
                    objAutorizante.tipoPuesto = "Director linea";
                    objAutorizante.nombreCompleto = GetNombreCompletoSIGOPLAN(objEmpleado.nombre, objEmpleado.apellidoPaterno, objEmpleado.apellidoMaterno);
                    objAutorizante.estatus = GetEstatusPrestamo(idPrestamo, objPrestamo.estatus, (int)objPrestamo.idDirectorLineaN);
                    lstAutorizantes.Add(objAutorizante);
                }

                if (objPrestamo.idDirectorGeneral > 0)
                {
                    objAutorizante = new repPrestamosDTO();
                    string claveEmpleado = objPrestamo.idDirectorGeneral.ToString();
                    tblP_Usuario objEmpleado = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado).FirstOrDefault();
                    objAutorizante.tipoPuesto = "Director General";
                    objAutorizante.nombreCompleto = GetNombreCompletoSIGOPLAN(objEmpleado.nombre, objEmpleado.apellidoPaterno, objEmpleado.apellidoMaterno);
                    objAutorizante.estatus = GetEstatusPrestamo(idPrestamo, objPrestamo.estatus, (int)objPrestamo.idDirectorGeneral);
                    lstAutorizantes.Add(objAutorizante);
                }

                //if (objPrestamo.idCapitalHumano > 0)
                //{
                //    objAutorizante = new repPrestamosDTO();
                //    string claveEmpleado = objPrestamo.idCapitalHumano.ToString();
                //    tblP_Usuario objEmpleado = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado).FirstOrDefault();
                //    objAutorizante.tipoPuesto = "Capital Humano";
                //    objAutorizante.nombreCompleto = GetNombreCompletoSIGOPLAN(objEmpleado.nombre, objEmpleado.apellidoPaterno, objEmpleado.apellidoMaterno);
                //    objAutorizante.estatus = GetEstatusPrestamo(idPrestamo, objPrestamo.estatus, (int)objPrestamo.idCapitalHumano);
                //    lstAutorizantes.Add(objAutorizante);
                //}

                if (objPrestamo.idCapitalHumano > 0)
                {
                    objAutorizante = new repPrestamosDTO();
                    string claveEmpleado = objPrestamo.idCapitalHumano.ToString();
                    tblP_Usuario objEmpleado = _context.tblP_Usuario.Where(w => w.cveEmpleado == claveEmpleado).FirstOrDefault();
                    objAutorizante.tipoPuesto = "Gerente o Director CH";
                    objAutorizante.nombreCompleto = GetNombreCompletoSIGOPLAN(objEmpleado.nombre, objEmpleado.apellidoPaterno, objEmpleado.apellidoMaterno);
                    objAutorizante.estatus = GetEstatusPrestamo(idPrestamo, objPrestamo.estatus, (int)objPrestamo.idCapitalHumano);
                    lstAutorizantes.Add(objAutorizante);
                }

                #endregion

                #region SE ORDENA LOS AUTORIZANTES
                repPrestamosDTO objAutorizanteOrdenado = new repPrestamosDTO();
                foreach (var item in lstEmpleadosRelFacultamiento)
                {
                    objAutorizanteOrdenado = lstAutorizantesOrdenado.Where(w => w.nombreCompleto == item.nombreEmpleado).FirstOrDefault();
                    if (objAutorizanteOrdenado == null)
                    {
                        objAutorizanteOrdenado = new repPrestamosDTO();
                        objAutorizanteOrdenado.tipoPuesto = GetPuestoDeFacultamiento(item.conceptoID, plantillaID);
                        objAutorizanteOrdenado.nombreCompleto = item.nombreEmpleado;
                        objAutorizanteOrdenado.estatus = lstAutorizantes.Where(w => w.nombreCompleto == item.nombreEmpleado).Select(s => s.estatus).FirstOrDefault();
                        lstAutorizantesOrdenado.Add(objAutorizanteOrdenado);
                    }
                }
                #endregion
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("lstAutorizantes", lstAutorizantes);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetListadoAutorizantes", e, AccionEnum.CONSULTA, idPrestamo, new { idPrestamo = idPrestamo });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetEstatusPrestamo(int idPrestamo, string estatus, int cveEmpleado)
        {
            string estatusPrestamo = string.Empty;
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el estatus del prestamo.";
                if (idPrestamo <= 0) { throw new Exception(mensajeError); }
                if (string.IsNullOrEmpty(estatus)) { throw new Exception(mensajeError); }
                if (cveEmpleado <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL ESTATUS DEL PRESTAMO
                tblRH_EK_PrestamosAutorizaciones objPrestamo = _context.tblRH_EK_PrestamosAutorizaciones.Where(w => w.FK_Prestamo == idPrestamo && w.cveEmpleado == cveEmpleado && w.registroActivo).FirstOrDefault();
                if (objPrestamo == null)
                    estatusPrestamo = "Pendiente";
                else if (objPrestamo.esPrestamoAutorizado)
                    estatusPrestamo = "Autorizado";
                else if (!objPrestamo.esPrestamoAutorizado)
                    estatusPrestamo = "Rechazado";
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetEstatusPrestamo", e, AccionEnum.CONSULTA, 0, new { idPrestamo = idPrestamo, estatus = estatus });
                return string.Empty;
            }
            return estatusPrestamo;
        }

        private int GetCCID(string cc)
        {
            int idCC = 0;
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el ID del CC.";
                if (string.IsNullOrEmpty(cc)) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL ID DEL CC
                tblP_CC objCC = _context.tblP_CC.Where(w => w.cc == cc && w.estatus).FirstOrDefault();
                if (objCC == null)
                    throw new Exception(mensajeError);

                idCC = objCC.id;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetEstatusPrestamo", e, AccionEnum.CONSULTA, 0, new { cc = cc });
                return idCC;
            }
            return idCC;
        }

        private int GetPlantillaID(string tipoPrestamo)
        {
            int facultamientoID = 0;
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(tipoPrestamo)) { throw new Exception("Ocurrió un error al obtener el ID del facultamiento."); }
                #endregion

                #region SE OBTIENE EL ID DEL FACULTAMIENTO
                switch (tipoPrestamo)
                {
                    case "SINDICATO":
                        facultamientoID = 104;
                        break;
                    case "MayorIgualA10":
                        facultamientoID = 103;
                        break;
                    case "MenorA10":
                        facultamientoID = 102;
                        break;
                    default:
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetPlantillaID", e, AccionEnum.CONSULTA, 0, new { tipoPrestamo = tipoPrestamo });
                return facultamientoID;
            }
            return facultamientoID;
        }

        private string GetPuestoDeFacultamiento(int conceptoID, int plantillaID)
        {
            string concepto = string.Empty;
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el puesto en base al facultamiento.";
                if (conceptoID <= 0) { throw new Exception(mensajeError); }
                if (plantillaID <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL PUESTO EN BASE AL FACULTAMIENTO
                tblFA_ConceptoPlantilla objConcepto = _context.tblFA_ConceptoPlantilla.Where(w => w.plantillaID == plantillaID && w.id == conceptoID && w.esActivo).FirstOrDefault();
                if (objConcepto == null)
                    throw new Exception(mensajeError);

                concepto = objConcepto.concepto;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetPuestoDeFacultamiento", e, AccionEnum.CONSULTA, 0, new { conceptoID = conceptoID, plantillaID = plantillaID });
                return concepto;
            }
            return concepto;
        }

        public Dictionary<string, object> GetPrestamosFiltroGestion(List<string> cc3, string estatus, string tipoPrestamo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE PRESTAMOS
                List<repPrestamosDTO> lstPrestamos = _context.Select<repPrestamosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.clave_empleado, t1.id, t2.nombre, t2.ape_paterno, t2.ape_materno, t3.descripcion AS DescripcionPuesto, t2.cc_contable as cc, '[' + t2.cc_contable + '] ' + t4.descripcion AS ccDescripcion, 
                                        t1.estatus, t1.tipoPrestamo, t1.idDirectorLineaN, t1.idGerenteOdirector, t1.idDirectorGeneral, t1.idResponsableCC, t1.idCapitalHumano, t1.comentarioRechazo, t1.tipoPuesto AS tipoPuesto, t1.consecutivo, t1.fecha_creacion
	                                    FROM tblRH_EK_Prestamos AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON t2.clave_empleado = t1.clave_empleado
	                                    INNER JOIN tblRH_EK_Puestos AS t3 ON t3.puesto = t2.puesto
	                                    INNER JOIN tblP_CC AS t4 ON t4.cc = t2.cc_contable
		                                    WHERE t1.registroActivo = @registroActivo AND t2.esActivo = @esActivoEmp AND t2.estatus_empleado = @estatus_empleado AND t3.registroActivo = @esActivo AND t4.estatus = @estatus",
                    parametros = new { registroActivo = true, esActivoEmp = true, estatus_empleado = "A", esActivo = true, estatus = true }
                }).ToList();

                #region FILTROS
                if (cc3 != null)
                {
                    List<string> lstCC = new List<string>();
                    foreach (var item in cc3)
                    {
                        if (!string.IsNullOrEmpty(item))
                            lstCC.Add(item.Trim().ToUpper());
                    }

                    if (lstCC.Count() > 0)
                        lstPrestamos = lstPrestamos.Where(w => lstCC.Contains(w.cc)).ToList();
                }

                if (!string.IsNullOrEmpty(estatus))
                    lstPrestamos = lstPrestamos.Where(w => w.estatus == estatus).ToList();

                if (!string.IsNullOrEmpty(tipoPrestamo))
                    lstPrestamos = lstPrestamos.Where(w => w.tipoPrestamo == tipoPrestamo).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE ARCHIVOS DE LOS PRESTAMOS
                List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.registroActivo).ToList();
                #endregion

                foreach (var item in lstPrestamos)
                {
                    #region SE OBTIENE EL NOMBRE COMPLETO DEL USUARIO
                    item.nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        item.nombreCompleto = item.nombre.Trim();
                    if (!string.IsNullOrEmpty(item.ape_paterno))
                        item.nombreCompleto += string.Format(" {0}", item.ape_paterno.Trim());
                    if (!string.IsNullOrEmpty(item.ape_materno))
                        item.nombreCompleto += string.Format(" {0}", item.ape_materno.Trim());
                    #endregion

                    #region SE OBTIENE EL TIPO DE PRESTAMO Y SE VERIFICA SI CUENTA CON LA DOCUMENTACIÓN NECESARIA PARA PASAR A LA GESTIÓN DE FIRMAS
                    int CantArchivosPrestamo = lstArchivos.Where(w => w.FK_Prestamo == item.id).Count();
                    int totalArchivosRestantes=0;
                    if (item.tipoPuesto == "SINDICALIZADO")
                    {
                        item.puedeAutorizarRechazar = CantArchivosPrestamo >= 1 ? true : false;
                        if (CantArchivosPrestamo >= 1)
                        {
                            totalArchivosRestantes = 0;
                        }
                        else
                        {
                            totalArchivosRestantes = 1;
                            item.cantidadArchivos = totalArchivosRestantes;
                        }
                 
                    }
                    else
                    {
                        switch (item.tipoPrestamo)
                        {
                            case "SINDICATO":
                                item.puedeAutorizarRechazar = CantArchivosPrestamo >= 1 ? true : false;
                                if (CantArchivosPrestamo >= 1)
                                {
                                    totalArchivosRestantes = 0;
                                }
                                else
                                {
                                    totalArchivosRestantes = 1 - CantArchivosPrestamo;
                                    item.cantidadArchivos = totalArchivosRestantes;
                                }
                                break;
                            case "MayorIgualA10":
                                item.puedeAutorizarRechazar = CantArchivosPrestamo >= 3 ? true : false;
                                if (CantArchivosPrestamo >= 3)
                                {
                                    totalArchivosRestantes = 0;
                                }
                                else
                                {
                                    totalArchivosRestantes = 3 - CantArchivosPrestamo;
                                    item.cantidadArchivos = totalArchivosRestantes;
                                }
                                break;
                            case "MenorA10":
                                item.puedeAutorizarRechazar = CantArchivosPrestamo >= 2 ? true : false;
                                if (CantArchivosPrestamo >= 2)
                                {
                                    totalArchivosRestantes = 0;
                                }
                                else
                                {
                                    totalArchivosRestantes = 2 - CantArchivosPrestamo;
                                    item.cantidadArchivos = totalArchivosRestantes;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    

                    #endregion

                    if (item.puedeAutorizarRechazar == true)
                    {
                        #region SE VERIFICA SI LA PERSONA LOGUEADA PUEDE AUTORIZAR O SI YA LO HIZO, NO MOSTRAR EL BOTON
                        string cveEmpleado = vSesiones.sesionUsuarioDTO.cveEmpleado.ToString();
                        int claveEmpleado = Convert.ToInt32(cveEmpleado);

                        // SE VERIFICA SI LA PERSONA LOGUEADA PUEDE AUTORIZAR
                        bool PuedeAutorizar = false;
                        if (claveEmpleado == item.idDirectorLineaN)
                            PuedeAutorizar = true;
                        else if (claveEmpleado == item.idDirectorGeneral)
                            PuedeAutorizar = true;
                        else if (claveEmpleado == item.idResponsableCC)
                            PuedeAutorizar = true;
                        else if (claveEmpleado == item.idCapitalHumano)
                            PuedeAutorizar = true;
                        else if (claveEmpleado == item.idGerenteOdirector)
                            PuedeAutorizar = true;
                        else
                            item.puedeAutorizarRechazar = false;

                        if (PuedeAutorizar)
                            item.puedeAutorizarRechazar = true;

                        // SE VERIFICA SI YA CUENTA CON AUTORIZACIÓN
                        bool SeAutorizo = false;
                        if (claveEmpleado == item.idDirectorLineaN)
                            SeAutorizo = VerificarAutorizacion(claveEmpleado, item.id);
                        else if (claveEmpleado == item.idDirectorGeneral)
                            SeAutorizo = VerificarAutorizacion(claveEmpleado, item.id);
                        else if (claveEmpleado == item.idResponsableCC)
                            SeAutorizo = VerificarAutorizacion(claveEmpleado, item.id);
                        else if (claveEmpleado == item.idCapitalHumano)
                            SeAutorizo = VerificarAutorizacion(claveEmpleado, item.id);
                        else if (claveEmpleado == item.idGerenteOdirector)
                            SeAutorizo = VerificarAutorizacion(claveEmpleado, item.id);
                        else
                            item.puedeAutorizarRechazar = false;

                        if (SeAutorizo)
                            item.puedeAutorizarRechazar = false;
                        #endregion
                    }

                    #region ESCALONAMIENTO

                    string cveEmpDirectorLinea = item.idDirectorLineaN.ToString();
                    string cveEmpGerente = item.idGerenteOdirector.ToString();
                    string cveEmpDirectoGral = item.idDirectorGeneral.ToString();
                    string cveEmpResponsableCC = item.idResponsableCC.ToString();
                    string cveEmpCH = item.idCapitalHumano.ToString();

                    List<string> lstClaves = new List<string>() { cveEmpDirectorLinea, cveEmpGerente, cveEmpDirectoGral, cveEmpResponsableCC, cveEmpCH };
                    List<tblP_Usuario> lstUsuario = _context.tblP_Usuario.Where(e => lstClaves.Contains(e.cveEmpleado)).ToList();

                    string sigFirmar = null;

                    #region RESPONSABLE CC

                    tblP_Usuario objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpResponsableCC && w.estatus).FirstOrDefault();

                    if (item.idResponsableCC > 0)
                    {
                        var objAuthResponsableCC = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == item.id && w.cveEmpleado == item.idResponsableCC && w.registroActivo);
                        string strIdResponsableCC = item.idResponsableCC.ToString();

                        if (objAuthResponsableCC == null)
                        {
                            if (sigFirmar == null)
                            {
                                sigFirmar = strIdResponsableCC;

                            }
                        }
                    }
                    #endregion

                    #region GERENTE/DIRECTOR

                    objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpGerente && w.estatus).FirstOrDefault();

                    if (item.idGerenteOdirector > 0)
                    {
                        var objAuthGerenteDirector = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == item.id && w.cveEmpleado == item.idGerenteOdirector && w.registroActivo);
                        string strIdGerenteDirector = item.idGerenteOdirector.ToString();

                        if (objAuthGerenteDirector == null)
                        {
                            if (sigFirmar == null)
                            {
                                sigFirmar = strIdGerenteDirector;

                            }
                        }
                    }
                    #endregion

                    #region DIRE LINEA NEGOCIOS

                    objUsuario = lstUsuario.FirstOrDefault(w => w.cveEmpleado == cveEmpDirectorLinea && w.estatus);

                    if (item.idDirectorLineaN > 0)
                    {
                        var objAutorizacionLineaNegocios = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == item.id && w.cveEmpleado == item.idDirectorLineaN && w.registroActivo);
                        string strIdDirectorLineaN = item.idDirectorLineaN.ToString();

                        if (objAutorizacionLineaNegocios == null)
                        {
                            if (sigFirmar == null)
                            {
                                sigFirmar = strIdDirectorLineaN;

                            }
                        }
                    }
                    #endregion

                    #region DIRECTOR GENERAL

                    objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpDirectoGral && w.estatus).FirstOrDefault();
                    if (item.idDirectorGeneral > 0)
                    {
                        var objAuthDireGral = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == item.id && w.cveEmpleado == item.idDirectorGeneral && w.registroActivo);
                        string strIdDirectorGeneral = item.idDirectorGeneral.ToString();

                        if (objAuthDireGral == null)
                        {
                            if (sigFirmar == null)
                            {
                                sigFirmar = strIdDirectorGeneral;

                            }
                        }
                    }
                    #endregion

                    #region CH

                    objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpCH && w.estatus).FirstOrDefault();

                    if (item.idCapitalHumano > 0)
                    {
                        var objAuthCH = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == item.id && w.cveEmpleado == item.idCapitalHumano && w.registroActivo);
                        string strIdCapitalHumano = item.idCapitalHumano.ToString();

                        if (objAuthCH == null)
                        {
                            if (sigFirmar == null)
                            {
                                sigFirmar = strIdCapitalHumano;
                            }
                        }
                    }

                    #endregion
                    #endregion

                    bool esSigFirmar = false;
                    esSigFirmar = vSesiones.sesionUsuarioDTO.cveEmpleado == sigFirmar;

                    item.puedeAutorizarRechazar = esSigFirmar && item.puedeAutorizarRechazar;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstPrestamos);
                #endregion
            }
            catch (Exception ex)
            {
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private bool VerificarAutorizacion(int cveEmpleado, int FK_Empleado)
        {
            bool SeAutorizo = false;
            try
            {
                #region SE VERIFICA SI EL AUTORIZANTE YA AUTORIZO/RECHAZO EL PRESTAMO
                tblRH_EK_PrestamosAutorizaciones objAutorizacion = _context.tblRH_EK_PrestamosAutorizaciones.Where(w => w.cveEmpleado == cveEmpleado && w.FK_Prestamo == FK_Empleado && w.registroActivo).FirstOrDefault();
                if (objAutorizacion != null)
                    SeAutorizo = true;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "VerificarAutorizacion", e, AccionEnum.CONSULTA, 0, new { cveEmpleado = cveEmpleado, FK_Empleado = FK_Empleado });
                return SeAutorizo;
            }
            return SeAutorizo;
        }

        public Dictionary<string, object> AutorizarRechazarPrestamo(repPrestamosDTO objFiltroDTO)
        {
            repPrestamosDTO infoPrestamo = new repPrestamosDTO();
            tblRH_EK_Prestamos objPrestamo = new tblRH_EK_Prestamos();

            try
            {
                objPrestamo = _context.tblRH_EK_Prestamos.Where(w => w.id == objFiltroDTO.FK_Prestamo && w.registroActivo).FirstOrDefault();
                if (objPrestamo == null)
                    throw new Exception("Ocurrió un error al obtener la información del prestamo.");

                infoPrestamo = GetInfoPrestamos(objPrestamo.clave_empleado);

            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "AutorizarRechazarPrestamo", e, AccionEnum.AGREGAR, 0, objFiltroDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = string.Format("Ocurrió un error al {0} el prestamo.", objFiltroDTO.esAutorizar ? "autorizar" : "rechazar");
                    if (objFiltroDTO.FK_Prestamo <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    #region SE AUTORIZA/RECHAZAR EL PRESTAMO

                    List<AutorizantesPerstamosDTO> lstAuth = new List<AutorizantesPerstamosDTO>();

                    // SE OBTIENE LA INFORMACIÓN DEL PRESTAMO


                    var objEmpleadoPrestamo = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.esActivo && e.clave_empleado == objPrestamo.clave_empleado);
                    string nombreCompletoEmpPrestamo = objEmpleadoPrestamo != null ? (objEmpleadoPrestamo.nombre + " " + objEmpleadoPrestamo.ape_paterno + " " + objEmpleadoPrestamo.ape_materno) : "";

                    string cveEmpDirectorLinea = objPrestamo.idDirectorLineaN.ToString();
                    string cveEmpGerente = objPrestamo.idGerenteOdirector.ToString();
                    string cveEmpDirectoGral = objPrestamo.idDirectorGeneral.ToString();
                    string cveEmpResponsableCC = objPrestamo.idResponsableCC.ToString();
                    string cveEmpCH = objPrestamo.idCapitalHumano.ToString();

                    List<string> lstClaves = new List<string>() { cveEmpDirectorLinea, cveEmpGerente, cveEmpDirectoGral, cveEmpResponsableCC, cveEmpCH };
                    List<tblP_Usuario> lstUsuario = _context.tblP_Usuario.Where(e => lstClaves.Contains(e.cveEmpleado)).ToList();

                    // SE REGISTRA LA AUTORIZACIÓN/RECHAZO DEL PRESTAMO
                    string cveEmpleado = vSesiones.sesionUsuarioDTO.cveEmpleado.ToString();
                    tblRH_EK_PrestamosAutorizaciones objAutorizacion = new tblRH_EK_PrestamosAutorizaciones();
                    objAutorizacion.FK_Prestamo = objFiltroDTO.FK_Prestamo;
                    objAutorizacion.tipoAutorizante = string.Empty;
                    objAutorizacion.cveEmpleado = Convert.ToInt32(cveEmpleado);
                    objAutorizacion.esPrestamoAutorizado = objFiltroDTO.esAutorizar;
                    objAutorizacion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objAutorizacion.fechaCreacion = DateTime.Now;
                    objAutorizacion.registroActivo = true;
                    _context.tblRH_EK_PrestamosAutorizaciones.Add(objAutorizacion);
                    _context.SaveChanges();
                    #endregion

                    #region SE INDICA SI EL PRESTAMO FUE TOTALMENTE AUTORIZADO O RECHAZADO
                    if (!objFiltroDTO.esAutorizar)
                    {
                        tblRH_EK_Prestamos objRechazar = _context.tblRH_EK_Prestamos.Where(w => w.id == objFiltroDTO.FK_Prestamo).FirstOrDefault();
                        if (objRechazar == null)
                            throw new Exception("Ocurrió un error al obtener el prestamo a rechazar.");

                        objRechazar.estatus = "C";
                        objRechazar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objRechazar.fechaModificacion = DateTime.Now;
                        objRechazar.comentarioRechazo = objFiltroDTO.comentarioRechazo;
                        _context.SaveChanges();

                        #region RESPONSABLE CC

                        tblP_Usuario objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpResponsableCC && w.estatus).FirstOrDefault();

                        if (objPrestamo.idResponsableCC > 0)
                        {
                            var objAuthResponsableCC = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idResponsableCC && w.registroActivo);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                            if (objAuthResponsableCC == null)
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idResponsableCC,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Responsable CC",
                                    descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                    cveEmpleado = cveEmpResponsableCC
                                });
                            }
                            else
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idResponsableCC,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Responsable CC",
                                    descEstatus = objAuthResponsableCC.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                    cveEmpleado = cveEmpResponsableCC
                                });

                                var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                if (objAlerta != null)
                                {
                                    objAlerta.visto = true;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        #endregion

                        #region GERENTE/DIRECTOR

                        objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpGerente && w.estatus).FirstOrDefault();

                        if (objPrestamo.idGerenteOdirector > 0)
                        {
                            var objAuthGerenteDirector = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idGerenteOdirector && w.registroActivo);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            if (objAuthGerenteDirector == null)
                            {

                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idGerenteOdirector,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Gerente / Director",
                                    descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                    cveEmpleado = cveEmpGerente
                                });
                            }
                            else
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idGerenteOdirector,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Gerente / Director",
                                    descEstatus = objAuthGerenteDirector.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                    cveEmpleado = cveEmpGerente
                                });

                                var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                if (objAlerta != null)
                                {
                                    objAlerta.visto = true;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        #endregion

                        #region DIRE LINEA NEGOCIOS

                        objUsuario = lstUsuario.FirstOrDefault(w => w.cveEmpleado == cveEmpDirectorLinea && w.estatus);

                        if (objPrestamo.idDirectorLineaN > 0)
                        {
                            var objAutorizacionLineaNegocios = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorLineaN && w.registroActivo);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                            if (objAutorizacionLineaNegocios == null)
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idDirectorLineaN,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Director de Linea de Negocios",
                                    descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                    cveEmpleado = cveEmpDirectorLinea
                                });
                            }
                            else
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idDirectorLineaN,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Director de Linea de Negocios",
                                    descEstatus = objAutorizacionLineaNegocios.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                    cveEmpleado = cveEmpDirectorLinea
                                });

                                var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                if (objAlerta != null)
                                {
                                    objAlerta.visto = true;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        #endregion

                        #region DIRECTOR GENERAL

                        objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpDirectoGral && w.estatus).FirstOrDefault();
                        if (objPrestamo.idDirectorGeneral > 0)
                        {
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            var objAuthDireGral = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorGeneral && w.registroActivo);

                            if (objAuthDireGral == null)
                            {

                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idDirectorGeneral,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Director General",
                                    descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                    cveEmpleado = cveEmpDirectoGral
                                });
                            }
                            else
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    idUsuario = objPrestamo.idDirectorGeneral,
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Director General",
                                    descEstatus = objAuthDireGral.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                    cveEmpleado = cveEmpDirectoGral
                                });

                                var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                if (objAlerta != null)
                                {
                                    objAlerta.visto = true;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        #endregion

                        #region CH

                        objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpCH && w.estatus).FirstOrDefault();

                        if (objPrestamo.idCapitalHumano > 0)
                        {
                            var objAuthCH = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idCapitalHumano && w.registroActivo);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                            if (objAuthCH == null)
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Capital Humano",
                                    descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                    cveEmpleado = cveEmpCH
                                });
                            }
                            else
                            {
                                lstAuth.Add(new AutorizantesPerstamosDTO
                                {
                                    nombreCompleto = nombreCompleto,
                                    descPuesto = "Capital Humano",
                                    descEstatus = objAuthCH.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                    cveEmpleado = cveEmpCH
                                });

                                var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                if (objAlerta != null)
                                {
                                    objAlerta.visto = true;
                                    _context.SaveChanges();
                                }
                            }
                        }

                        #endregion

                        #region CUERPO DEL CORREO

                        string cuerpo =
                                        @"<html>
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
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Buen día, se informa que el prestamo del CC " + GetCCDescripcion(objPrestamo.cc) + @", ha sido rechazado<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "Motivo rechazo: " + objRechazar.comentarioRechazo + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";

                        #region TABLA AUTORIZANTES
                        cuerpo += @"
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                        bool esAuth = false;
                        int totalSiguientes = 0;

                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        foreach (var itemDet in lstAuth)
                        {
                            cuerpo += "<tr>" +
                                        "<td>" + itemDet.nombreCompleto + "</td>" +
                                        "<td>" + itemDet.descPuesto + "</td>" +
                                        getEstatus((int)itemDet.descEstatus, false) +
                                    "</tr>";
                        }

                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        cuerpo += "<br><br><br>" +
                              "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                              "Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br><br>" +
                              "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                              "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";
                        #endregion

                        #region PDF

                        string RutaServidor = "";
#if DEBUG
                        RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                        RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif

                        ReportDocument rptCV = new rptPrestamos();

                        //string path = Path.Combine(RutaServidor, "rptPrestamos.rpt");
                        //rptCV.Load(path);

                        //rd.SetParameterValue("ubicacion", "HERMOSILLO, SON");
                        rptCV.SetParameterValue("fechaActual", DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                        rptCV.SetParameterValue("ccDescripcion", infoPrestamo.ccDescripcion.Trim());
                        rptCV.SetParameterValue("nombrePuesto", infoPrestamo.nombrePuesto.Trim());
                        rptCV.SetParameterValue("categoriaPuesto", !string.IsNullOrEmpty(infoPrestamo.descCategoriaPuesto) ? (infoPrestamo.descCategoriaPuesto.Trim()) : "S/N");
                        rptCV.SetParameterValue("nombreCompleto", infoPrestamo.nombreCompleto.Trim());

                        string empresa = null;
                        if (infoPrestamo.empresa == empresa)
                        {
                            rptCV.SetParameterValue("empresa", "");
                        }
                        else
                        {
                            rptCV.SetParameterValue("empresa", infoPrestamo.empresa.Trim());
                        }

                        rptCV.SetParameterValue("fecha_alta", Convert.ToDateTime(infoPrestamo.fecha_alta).ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                        rptCV.SetParameterValue("tipoNomina", infoPrestamo.tipoNomina.Trim());
                        rptCV.SetParameterValue("sueldo_base", infoPrestamo.sueldo_base);
                        rptCV.SetParameterValue("complemento", infoPrestamo.complemento);
                        rptCV.SetParameterValue("bono_zona", infoPrestamo.bono_zona);
                        rptCV.SetParameterValue("totalN", infoPrestamo.totalN);
                        rptCV.SetParameterValue("totalM", infoPrestamo.totalM);

                        #region asignacion a casillas
                        string pago1 = "";
                        string pago2 = "";

                        if (infoPrestamo.formaPago == "12 Quincenas")
                        {
                            pago1 = "X";
                            pago2 = " ";
                            rptCV.SetParameterValue("formaPago12", pago1);
                            rptCV.SetParameterValue("formaPago24", pago2);
                        }
                        else
                        {
                            pago1 = " ";
                            pago2 = "X";
                            rptCV.SetParameterValue("formaPago12", pago1);
                            rptCV.SetParameterValue("formaPago24", pago2);

                        }


                        string salud = "";
                        string defuncion = "";
                        string daño = "";
                        string apoyo = "";
                        string sindicato = "";

                        if (infoPrestamo.motivoPrestamo == 1)
                        {
                            salud = "X";
                            defuncion = "";
                            daño = "";
                            apoyo = "";
                            sindicato = "";

                            rptCV.SetParameterValue("motivoSalud", salud);
                            rptCV.SetParameterValue("motivoDef", defuncion);
                            rptCV.SetParameterValue("motivoDaño", daño);
                            rptCV.SetParameterValue("motivoApoyo", apoyo);
                            rptCV.SetParameterValue("motivoSindicato", sindicato);

                        }
                        else if (infoPrestamo.motivoPrestamo == 2)
                        {
                            salud = "";
                            defuncion = "X";
                            daño = "";
                            apoyo = "";
                            sindicato = "";

                            rptCV.SetParameterValue("motivoSalud", salud);
                            rptCV.SetParameterValue("motivoDef", defuncion);
                            rptCV.SetParameterValue("motivoDaño", daño);
                            rptCV.SetParameterValue("motivoApoyo", apoyo);
                            rptCV.SetParameterValue("motivoSindicato", sindicato);

                        }
                        else if (infoPrestamo.motivoPrestamo == 3)
                        {
                            salud = "";
                            defuncion = "";
                            daño = "X";
                            apoyo = "";
                            sindicato = "";

                            rptCV.SetParameterValue("motivoSalud", salud);
                            rptCV.SetParameterValue("motivoDef", defuncion);
                            rptCV.SetParameterValue("motivoDaño", daño);
                            rptCV.SetParameterValue("motivoApoyo", apoyo);
                            rptCV.SetParameterValue("motivoSindicato", sindicato);
                        }
                        else if (infoPrestamo.motivoPrestamo == 4)
                        {
                            salud = "";
                            defuncion = "";
                            daño = "";
                            apoyo = "X";
                            sindicato = "";

                            rptCV.SetParameterValue("motivoSalud", salud);
                            rptCV.SetParameterValue("motivoDef", defuncion);
                            rptCV.SetParameterValue("motivoDaño", daño);
                            rptCV.SetParameterValue("motivoApoyo", apoyo);
                            rptCV.SetParameterValue("motivoSindicato", sindicato);
                        }
                        else if (infoPrestamo.motivoPrestamo == 5)
                        {
                            salud = "";
                            defuncion = "";
                            daño = "";
                            apoyo = "";
                            sindicato = "X";

                            rptCV.SetParameterValue("motivoSalud", salud);
                            rptCV.SetParameterValue("motivoDef", defuncion);
                            rptCV.SetParameterValue("motivoDaño", daño);
                            rptCV.SetParameterValue("motivoApoyo", apoyo);
                            rptCV.SetParameterValue("motivoSindicato", sindicato);
                        }

                        if (infoPrestamo.cantidadSoli < 10000)
                        {
                            rptCV.SetParameterValue("IFE", "X");
                            rptCV.SetParameterValue("SOPORTE", "X");
                            rptCV.SetParameterValue("PAGARE", "");
                        }
                        if (infoPrestamo.cantidadSoli >= 10000)
                        {
                            rptCV.SetParameterValue("IFE", "X");
                            rptCV.SetParameterValue("SOPORTE", "X");
                            rptCV.SetParameterValue("PAGARE", "X");
                        }
                        #endregion

                        string descPrestamos = "";

                        if (infoPrestamo.tipoPrestamo == "MayorIgualA10")
                        {
                            descPrestamos = "MAYOR O IGUAL A $10,000.00";

                        }
                        else if (infoPrestamo.tipoPrestamo == "MenorA10")
                        {
                            descPrestamos = "MENOR A $10,000.00";

                        }
                        else
                        {
                            descPrestamos = " ";

                        }

                        rptCV.SetParameterValue("cantSoli", infoPrestamo.cantidadSoli);
                        rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra);
                        rptCV.SetParameterValue("cantMax", infoPrestamo.cantidadMax);
                        rptCV.SetParameterValue("cantDescontar", infoPrestamo.cantidadDescontar);
                        rptCV.SetParameterValue("otrosDesc", infoPrestamo.otrosDescuento);
                        rptCV.SetParameterValue("justificacion", infoPrestamo.justificacion.Trim());
                        rptCV.SetParameterValue("tipoSolicitud", infoPrestamo.tipoSolicitud.Trim());
                        rptCV.SetParameterValue("tipoPrestamo", descPrestamos);
                        rptCV.SetParameterValue("tipoPuesto", infoPrestamo.tipoPuesto.Trim());
                        rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra.Trim());

                        if (infoPrestamo.idResponsableCC == null || infoPrestamo.idGerenteOdirector == null || infoPrestamo.idDirectorGeneral == null || infoPrestamo.idDirectorLineaN == null)
                        {
                            rptCV.SetParameterValue("nombreResponsableCC", "");
                            rptCV.SetParameterValue("puestoResponsableCC", "");
                            rptCV.SetParameterValue("nombreDirectorGeneral", "");
                            rptCV.SetParameterValue("puestoDirectorGeneral", "");
                            rptCV.SetParameterValue("nombreDirectorLineaN", "");
                            rptCV.SetParameterValue("puestoDirectorLineaN", "");
                            rptCV.SetParameterValue("nombreGerenteOdirector", "");
                            rptCV.SetParameterValue("puestoGerenteOdirector", "");
                            rptCV.SetParameterValue("nombreCapitalHumano", "");
                            rptCV.SetParameterValue("puestoCapitalHumano", "");
                        }
                        else
                        {
                            rptCV.SetParameterValue("nombreResponsableCC", infoPrestamo != null && infoPrestamo.idResponsableCC > 0 ? infoPrestamo.nombreResponsableCC.Trim() : string.Empty);
                            rptCV.SetParameterValue("puestoResponsableCC", "RESPONSABLE DE CC");
                            rptCV.SetParameterValue("nombreDirectorGeneral", infoPrestamo != null && infoPrestamo.idDirectorGeneral > 0 ? infoPrestamo.nombreDirectorGeneral.Trim() : string.Empty);
                            rptCV.SetParameterValue("puestoDirectorGeneral", "DIRECTOR GENERAL");
                            rptCV.SetParameterValue("nombreDirectorLineaN", infoPrestamo != null && infoPrestamo.idDirectorLineaN > 0 ? infoPrestamo.nombreDirectorLineaN.Trim() : string.Empty);
                            rptCV.SetParameterValue("puestoDirectorLineaN", "DIRECTOR LINEA DE NEGOCIO");
                            rptCV.SetParameterValue("nombreGerenteOdirector", infoPrestamo != null && infoPrestamo.idGerenteOdirector > 0 ? infoPrestamo.nombreGerenteOdirector.Trim() : string.Empty);
                            rptCV.SetParameterValue("puestoGerenteOdirector", "GERENTE/DIRECTOR DE AREA");
                            rptCV.SetParameterValue("nombreCapitalHumano", infoPrestamo != null && infoPrestamo.idCapitalHumano > 0 ? infoPrestamo.nombreCapitalHumano.Trim() : string.Empty);
                            rptCV.SetParameterValue("puestoCapitalHumano", "GERENTE/DIRECTOR DE CAPITAL HUMANO");

                            //Firmas
                            var objIdResponsable = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idResponsableCC.Value.ToString());
                            var objIdDirectorGeneral = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idDirectorGeneral.Value.ToString());
                            var objIdDirectorLineaN = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idDirectorLineaN.Value.ToString());
                            var objIdGerenteOdirector = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idGerenteOdirector.Value.ToString());
                            var objidCapitalHumano = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idCapitalHumano.ToString());

                            rptCV.SetParameterValue("FirmaResponsableCC", (
                                objIdResponsable != null && objIdResponsable.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ? 
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idResponsableCC)) : " "));
                            rptCV.SetParameterValue("FirmaSolicitante", GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.clave_empleado)));
                            rptCV.SetParameterValue("FirmaDirectorGeneral", (
                                objIdDirectorGeneral != null && objIdDirectorGeneral.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ? 
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idDirectorGeneral)) : " "));
                            rptCV.SetParameterValue("FirmaDirectorLineaN", (
                                objIdDirectorLineaN != null && objIdDirectorLineaN.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ? 
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idDirectorLineaN)) : " "));
                            rptCV.SetParameterValue("FirmaGerenteOdirector", (
                                objIdGerenteOdirector != null && objIdGerenteOdirector.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ? 
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idGerenteOdirector)) : " "));
                            rptCV.SetParameterValue("FirmaCapitalHumano", (
                                objidCapitalHumano != null && objidCapitalHumano.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idCapitalHumano)) : " "));
                        }
                        rptCV.SetParameterValue("folio", ((infoPrestamo.cc == null ? "" : infoPrestamo.cc) + "-" + (infoPrestamo.clave_empleado) + "-" + (infoPrestamo.consecutivo.ToString().PadLeft(3, '0'))));

                        Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                        #endregion

                        // SE ENVIA CORREO INDICANDO QUE EL PRESTAMO FUE RECHAZADO
                        List<string> lstCorreos = new List<string>();
                        tblP_Usuario objUsuarioCreadorPrestamo = _context.tblP_Usuario.Where(w => w.id == objPrestamo.idUsuaioCreacion && w.estatus).FirstOrDefault();
                        if (objUsuarioCreadorPrestamo != null)
                            lstCorreos.Add(objUsuarioCreadorPrestamo.correo);

                        lstCorreos.Add("keyla.vasquez@construplan.com.mx");
                        lstCorreos.Add("diana.alvarez@construplan.com.mx");
#if DEBUG
                        lstCorreos = new List<string>();
                        //lstCorreos.Add("omar.nunez@construplan.com.mx");
                        lstCorreos.Add("miguel.buzani@construplan.com.mx");
#endif
                        string subject = "Prestamos Empleado: #" + objPrestamo.clave_empleado + " " + nombreCompletoEmpPrestamo;
                        //string body = string.Format("Buen día, se informa que el prestamo del CC {0}, ha sido rechazado" +
                        //                            "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br>" +
                        //                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>)." +
                        //                            "No es necesario dar una respuesta. Gracias.", GetCCDescripcion(objPrestamo.cc));
                        //GlobalUtils.sendEmail(subject, cuerpo, lstCorreos);

                        var lstArchives = new List<adjuntoCorreoDTO>();
                        List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.FK_Prestamo == objPrestamo.id && w.registroActivo).ToList();

                        foreach (var item in lstArchivos)
                        {
                            var fileStream = GlobalUtils.GetFileAsStream(item.rutaArchivo);
                            using (var streamReader = new MemoryStream())
                            {
                                fileStream.CopyTo(streamReader);
                                //downloadFiles.Add(streamReader.ToArray());

                                lstArchives.Add(new adjuntoCorreoDTO
                                {
                                    archivo = streamReader.ToArray(),
                                    nombreArchivo = Path.GetFileNameWithoutExtension(item.rutaArchivo),
                                    extArchivo = Path.GetExtension(item.rutaArchivo)
                                });
                            }
                        }

                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            //downloadPDFs.Add(streamReader.ToArray());

                            lstArchives.Add(new adjuntoCorreoDTO
                            {
                                archivo = streamReader.ToArray(),
                                nombreArchivo = "Prestamo",
                                extArchivo = ".pdf"
                            });


                            GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject),
                                cuerpo, lstCorreos, lstArchives);
                        }
                    }
                    else
                    {
                        int CantAutorizaciones = _context.tblRH_EK_PrestamosAutorizaciones.Where(w => w.FK_Prestamo == objFiltroDTO.FK_Prestamo && w.registroActivo).Count();
                        int CantAutorizacionesRealizadas = 0;

                        if (objPrestamo.idDirectorLineaN > 0)
                            CantAutorizacionesRealizadas++;

                        if (objPrestamo.idGerenteOdirector > 0)
                            CantAutorizacionesRealizadas++;

                        if (objPrestamo.idDirectorGeneral > 0)
                            CantAutorizacionesRealizadas++;

                        if (objPrestamo.idResponsableCC > 0)
                            CantAutorizacionesRealizadas++;

                        if (objPrestamo.idCapitalHumano > 0)
                            CantAutorizacionesRealizadas++;

                        if (CantAutorizaciones == CantAutorizacionesRealizadas)
                        {
                            // SE MARCA COMO AUTORIZADO EL PRESTAMO
                            objPrestamo.estatus = "A";
                            objPrestamo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objPrestamo.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();

                            #region RESPONSABLE CC

                            tblP_Usuario objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpResponsableCC && w.estatus).FirstOrDefault();

                            if (objPrestamo.idResponsableCC > 0)
                            {
                                var objAuthResponsableCC = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idResponsableCC && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAuthResponsableCC == null)
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Responsable CC",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpResponsableCC
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Responsable CC",
                                        descEstatus = objAuthResponsableCC.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                        cveEmpleado = cveEmpResponsableCC
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }


                            }
                            #endregion

                            #region GERENTE/DIRECTOR

                            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpGerente && w.estatus).FirstOrDefault();

                            if (objPrestamo.idGerenteOdirector > 0)
                            {
                                var objAuthGerenteDirector = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idGerenteOdirector && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAuthGerenteDirector == null)
                                {

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Gerente / Director",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpGerente
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Gerente / Director",
                                        descEstatus = objAuthGerenteDirector.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                        cveEmpleado = cveEmpGerente
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }


                            }
                            #endregion

                            #region DIRE LINEA NEGOCIOS

                            objUsuario = lstUsuario.FirstOrDefault(w => w.cveEmpleado == cveEmpDirectorLinea && w.estatus);

                            if (objPrestamo.idDirectorLineaN > 0)
                            {
                                var objAutorizacionLineaNegocios = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorLineaN && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAutorizacionLineaNegocios == null)
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director de Linea de Negocios",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpDirectorLinea
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director de Linea de Negocios",
                                        descEstatus = objAutorizacionLineaNegocios.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                        cveEmpleado = cveEmpDirectorLinea
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region DIRECTOR GENERAL

                            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpDirectoGral && w.estatus).FirstOrDefault();
                            if (objPrestamo.idDirectorGeneral > 0)
                            {
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                var objAuthDireGral = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorGeneral && w.registroActivo);

                                if (objAuthDireGral == null)
                                {

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director General",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpDirectoGral
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director General",
                                        descEstatus = objAuthDireGral.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                        cveEmpleado = cveEmpDirectoGral
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }


                            }
                            #endregion

                            #region CH

                            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpCH && w.estatus).FirstOrDefault();

                            if (objPrestamo.idCapitalHumano > 0)
                            {
                                var objAuthCH = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idCapitalHumano && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAuthCH == null)
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Capital Humano",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpCH
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Capital Humano",
                                        descEstatus = objAuthCH.esPrestamoAutorizado ? EstatusAutorizacionPrestamosEnum.AUTORIZADO : EstatusAutorizacionPrestamosEnum.RECHAZADO,
                                        cveEmpleado = cveEmpCH
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            #endregion

                            #region CUERPO DEL CORREO
                            string cuerpo =
                                            @"<html>
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
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Buen día, se informa que el prestamo del CC " + GetCCDescripcion(objPrestamo.cc) + @", ha sido autorizado por todos los firmantes.<br><br>
                                                    </p>
                                                    <br><br><br>
                                            ";

                            #region TABLA AUTORIZANTES
                            cuerpo += @"
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                            //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                            foreach (var itemDet in lstAuth)
                            {
                                cuerpo += "<tr>" +
                                            "<td>" + itemDet.nombreCompleto + "</td>" +
                                            "<td>" + itemDet.descPuesto + "</td>" +
                                            getEstatus(1, false) +
                                        "</tr>";
                            }

                            cuerpo += "</tbody>" +
                                        "</table>" +
                                        "<br><br><br>";


                            #endregion

                            cuerpo += "<br><br><br>" +
                                  "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                  "Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br><br>" +
                                  "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                  "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                    " </body>" +
                                  "</html>";

                            #endregion

                            #region PDF

                            string RutaServidor = "";
#if DEBUG
                            RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif

                            ReportDocument rptCV = new rptPrestamos();

                            //string path = Path.Combine(RutaServidor, "rptPrestamos.rpt");
                            //rptCV.Load(path);

                            //repPrestamosDTO infoPrestamo = GetInfoPrestamos(objPrestamo.clave_empleado);
                            //rd.SetParameterValue("ubicacion", "HERMOSILLO, SON");
                            rptCV.SetParameterValue("fechaActual", DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                            rptCV.SetParameterValue("ccDescripcion", infoPrestamo.ccDescripcion.Trim());
                            rptCV.SetParameterValue("nombrePuesto", infoPrestamo.nombrePuesto.Trim());
                            rptCV.SetParameterValue("categoriaPuesto", !string.IsNullOrEmpty(infoPrestamo.descCategoriaPuesto) ? (infoPrestamo.descCategoriaPuesto.Trim()) : "S/N");
                            rptCV.SetParameterValue("nombreCompleto", infoPrestamo.nombreCompleto.Trim());

                            string empresa = null;
                            if (infoPrestamo.empresa == empresa)
                            {
                                rptCV.SetParameterValue("empresa", "");
                            }
                            else
                            {
                                rptCV.SetParameterValue("empresa", infoPrestamo.empresa.Trim());
                            }

                            rptCV.SetParameterValue("fecha_alta", Convert.ToDateTime(infoPrestamo.fecha_alta).ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                            rptCV.SetParameterValue("tipoNomina", infoPrestamo.tipoNomina.Trim());
                            rptCV.SetParameterValue("sueldo_base", infoPrestamo.sueldo_base);
                            rptCV.SetParameterValue("complemento", infoPrestamo.complemento);
                            rptCV.SetParameterValue("bono_zona", infoPrestamo.bono_zona);
                            rptCV.SetParameterValue("totalN", infoPrestamo.totalN);
                            rptCV.SetParameterValue("totalM", infoPrestamo.totalM);

                            #region asignacion a casillas
                            string pago1 = "";
                            string pago2 = "";

                            if (infoPrestamo.formaPago == "12 Quincenas")
                            {
                                pago1 = "X";
                                pago2 = " ";
                                rptCV.SetParameterValue("formaPago12", pago1);
                                rptCV.SetParameterValue("formaPago24", pago2);
                            }
                            else
                            {
                                pago1 = " ";
                                pago2 = "X";
                                rptCV.SetParameterValue("formaPago12", pago1);
                                rptCV.SetParameterValue("formaPago24", pago2);

                            }


                            string salud = "";
                            string defuncion = "";
                            string daño = "";
                            string apoyo = "";
                            string sindicato = "";

                            if (infoPrestamo.motivoPrestamo == 1)
                            {
                                salud = "X";
                                defuncion = "";
                                daño = "";
                                apoyo = "";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);

                            }
                            else if (infoPrestamo.motivoPrestamo == 2)
                            {
                                salud = "";
                                defuncion = "X";
                                daño = "";
                                apoyo = "";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);

                            }
                            else if (infoPrestamo.motivoPrestamo == 3)
                            {
                                salud = "";
                                defuncion = "";
                                daño = "X";
                                apoyo = "";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);
                            }
                            else if (infoPrestamo.motivoPrestamo == 4)
                            {
                                salud = "";
                                defuncion = "";
                                daño = "";
                                apoyo = "X";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);
                            }
                            else if (infoPrestamo.motivoPrestamo == 5)
                            {
                                salud = "";
                                defuncion = "";
                                daño = "";
                                apoyo = "";
                                sindicato = "X";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);
                            }

                            if (infoPrestamo.cantidadSoli < 10000)
                            {
                                rptCV.SetParameterValue("IFE", "X");
                                rptCV.SetParameterValue("SOPORTE", "X");
                                rptCV.SetParameterValue("PAGARE", "");
                            }
                            if (infoPrestamo.cantidadSoli >= 10000)
                            {
                                rptCV.SetParameterValue("IFE", "X");
                                rptCV.SetParameterValue("SOPORTE", "X");
                                rptCV.SetParameterValue("PAGARE", "X");
                            }
                            #endregion

                            string descPrestamos = "";

                            if (infoPrestamo.tipoPrestamo == "MayorIgualA10")
                            {
                                descPrestamos = "MAYOR O IGUAL A $10,000.00";

                            }
                            else if (infoPrestamo.tipoPrestamo == "MenorA10")
                            {
                                descPrestamos = "MENOR A $10,000.00";

                            }
                            else
                            {
                                descPrestamos = " ";

                            }

                            rptCV.SetParameterValue("cantSoli", infoPrestamo.cantidadSoli);
                            rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra);
                            rptCV.SetParameterValue("cantMax", infoPrestamo.cantidadMax);
                            rptCV.SetParameterValue("cantDescontar", infoPrestamo.cantidadDescontar);
                            rptCV.SetParameterValue("otrosDesc", infoPrestamo.otrosDescuento);
                            rptCV.SetParameterValue("justificacion", infoPrestamo.justificacion.Trim());
                            rptCV.SetParameterValue("tipoSolicitud", infoPrestamo.tipoSolicitud.Trim());
                            rptCV.SetParameterValue("tipoPrestamo", descPrestamos);
                            rptCV.SetParameterValue("tipoPuesto", infoPrestamo.tipoPuesto.Trim());
                            rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra.Trim());

                            if (infoPrestamo.idResponsableCC == null || infoPrestamo.idGerenteOdirector == null || infoPrestamo.idDirectorGeneral == null || infoPrestamo.idDirectorLineaN == null)
                            {
                                rptCV.SetParameterValue("nombreResponsableCC", "");
                                rptCV.SetParameterValue("puestoResponsableCC", "");
                                rptCV.SetParameterValue("nombreDirectorGeneral", "");
                                rptCV.SetParameterValue("puestoDirectorGeneral", "");
                                rptCV.SetParameterValue("nombreDirectorLineaN", "");
                                rptCV.SetParameterValue("puestoDirectorLineaN", "");
                                rptCV.SetParameterValue("nombreGerenteOdirector", "");
                                rptCV.SetParameterValue("puestoGerenteOdirector", "");
                                rptCV.SetParameterValue("nombreCapitalHumano", "");
                                rptCV.SetParameterValue("puestoCapitalHumano", "");
                            }
                            else
                            {
                                rptCV.SetParameterValue("nombreResponsableCC", infoPrestamo != null && infoPrestamo.idResponsableCC > 0 ? infoPrestamo.nombreResponsableCC.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoResponsableCC", "RESPONSABLE DE CC");
                                rptCV.SetParameterValue("nombreDirectorGeneral", infoPrestamo != null && infoPrestamo.idDirectorGeneral > 0 ? infoPrestamo.nombreDirectorGeneral.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoDirectorGeneral", "DIRECTOR GENERAL");
                                rptCV.SetParameterValue("nombreDirectorLineaN", infoPrestamo != null && infoPrestamo.idDirectorLineaN > 0 ? infoPrestamo.nombreDirectorLineaN.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoDirectorLineaN", "DIRECTOR LINEA DE NEGOCIO");
                                rptCV.SetParameterValue("nombreGerenteOdirector", infoPrestamo != null && infoPrestamo.idGerenteOdirector > 0 ? infoPrestamo.nombreGerenteOdirector.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoGerenteOdirector", "GERENTE/DIRECTOR DE AREA");
                                rptCV.SetParameterValue("nombreCapitalHumano", infoPrestamo != null && infoPrestamo.idCapitalHumano > 0 ? infoPrestamo.nombreCapitalHumano.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoCapitalHumano", "GERENTE/DIRECTOR DE CAPITAL HUMANO");

                                //Firmas

                                var objIdResponsable = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idResponsableCC.Value.ToString());
                                var objIdDirectorGeneral = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idDirectorGeneral.Value.ToString());
                                var objIdDirectorLineaN = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idDirectorLineaN.Value.ToString());
                                var objIdGerenteOdirector = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idGerenteOdirector.Value.ToString());
                                var objidCapitalHumano = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idCapitalHumano.ToString());

                                rptCV.SetParameterValue("FirmaResponsableCC", (
                                objIdResponsable != null && objIdResponsable.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idResponsableCC)) : " "));
                                rptCV.SetParameterValue("FirmaSolicitante", GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.clave_empleado)));
                                rptCV.SetParameterValue("FirmaDirectorGeneral", (
                                    objIdDirectorGeneral != null && objIdDirectorGeneral.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idDirectorGeneral)) : " "));
                                rptCV.SetParameterValue("FirmaDirectorLineaN", (
                                    objIdDirectorLineaN != null && objIdDirectorLineaN.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idDirectorLineaN)) : " "));
                                rptCV.SetParameterValue("FirmaGerenteOdirector", (
                                    objIdGerenteOdirector != null && objIdGerenteOdirector.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idGerenteOdirector)) : " "));
                                rptCV.SetParameterValue("FirmaCapitalHumano", (
                                    objidCapitalHumano != null && objidCapitalHumano.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idCapitalHumano)) : " "));
                            }
                            rptCV.SetParameterValue("folio", ((infoPrestamo.cc == null ? "" : infoPrestamo.cc) + "-" + (infoPrestamo.clave_empleado) + "-" + (infoPrestamo.consecutivo.ToString().PadLeft(3, '0'))));

                            Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                            #endregion

                            // SE ENVIA CORREO INDICANDO QUE EL PRESTAMO FUE AUTORIZADO
                            List<string> lstCorreos = new List<string>();
                            tblP_Usuario objUsuarioCreadorPrestamo = _context.tblP_Usuario.Where(w => w.id == objPrestamo.idUsuaioCreacion && w.estatus).FirstOrDefault();
                            if (objUsuarioCreadorPrestamo != null)
                                lstCorreos.Add(objUsuarioCreadorPrestamo.correo);

                            lstCorreos.Add("keyla.vasquez@construplan.com.mx");
                            lstCorreos.Add("diana.alvarez@construplan.com.mx");
                            lstCorreos.Add("despacho@construplan.com.mx");
#if DEBUG
                            lstCorreos = new List<string>();
                            //lstCorreos.Add("omar.nunez@construplan.com.mx");
                            lstCorreos.Add("aaron.gracia@construplan.com.mx");
                            lstCorreos.Add("miguel.buzani@construplan.com.mx");
#endif
                            string subject = "Prestamos Empleado: #" + objPrestamo.clave_empleado + " " + nombreCompletoEmpPrestamo;

                            var lstArchives = new List<adjuntoCorreoDTO>();
                            List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.FK_Prestamo == objPrestamo.id && w.registroActivo).ToList();

                            foreach (var item in lstArchivos)
                            {
                                var fileStream = GlobalUtils.GetFileAsStream(item.rutaArchivo);
                                using (var streamReader = new MemoryStream())
                                {
                                    fileStream.CopyTo(streamReader);

                                    lstArchives.Add(new adjuntoCorreoDTO
                                    {
                                        archivo = streamReader.ToArray(),
                                        nombreArchivo = Path.GetFileNameWithoutExtension(item.rutaArchivo),
                                        extArchivo = Path.GetExtension(item.rutaArchivo)
                                    });
                                }
                            }

                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);

                                lstArchives.Add(new adjuntoCorreoDTO
                                {
                                    archivo = streamReader.ToArray(),
                                    nombreArchivo = "Prestamo",
                                    extArchivo = ".pdf"
                                });


                                GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject),
                                    cuerpo, lstCorreos, lstArchives);
                            }
                        }
                        else
                        {

                            // SE NOTIFICA AL SIGUIENTE AUTORIZANTE
                            List<string> lstCorreos = new List<string>();

                            #region RESPONSABLE CC

                            var objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpResponsableCC && w.estatus).FirstOrDefault();

                            if (objPrestamo.idResponsableCC > 0)
                            {
                                var objAuthResponsableCC = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idResponsableCC && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAuthResponsableCC == null)
                                {
                                    if (lstCorreos.Count() == 0)
                                    {
                                        lstCorreos.Add(objUsuario.correo);

                                        #region Alerta SIGOPLAN
                                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                        objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                        objNuevaAlerta.tipoAlerta = 2;
                                        objNuevaAlerta.sistemaID = 16;
                                        objNuevaAlerta.visto = false;
                                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                        objNuevaAlerta.objID = objPrestamo.id;
                                        objNuevaAlerta.obj = "AutorizacionPrestamos";
                                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                        objNuevaAlerta.documentoID = 0;
                                        objNuevaAlerta.moduloID = 0;
                                        _context.tblP_Alerta.Add(objNuevaAlerta);
                                        _context.SaveChanges();
                                        #endregion //ALERTA SIGPLAN
                                    }

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Responsable CC",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpResponsableCC
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Responsable CC",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.AUTORIZADO,
                                        cveEmpleado = cveEmpResponsableCC
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region GERENTE/DIRECTOR

                            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpGerente && w.estatus).FirstOrDefault();

                            if (objPrestamo.idGerenteOdirector > 0)
                            {
                                var objAuthGerenteDirector = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idGerenteOdirector && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAuthGerenteDirector == null)
                                {
                                    if (lstCorreos.Count() == 0)
                                    {
                                        lstCorreos.Add(objUsuario.correo);

                                        #region Alerta SIGOPLAN
                                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                        objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                        objNuevaAlerta.tipoAlerta = 2;
                                        objNuevaAlerta.sistemaID = 16;
                                        objNuevaAlerta.visto = false;
                                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                        objNuevaAlerta.objID = objPrestamo.id;
                                        objNuevaAlerta.obj = "AutorizacionPrestamos";
                                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                        objNuevaAlerta.documentoID = 0;
                                        objNuevaAlerta.moduloID = 0;
                                        _context.tblP_Alerta.Add(objNuevaAlerta);
                                        _context.SaveChanges();
                                        #endregion //ALERTA SIGPLAN
                                    }

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Gerente / Director",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpGerente
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Gerente / Director",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.AUTORIZADO,
                                        cveEmpleado = cveEmpGerente
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region DIRE LINEA NEGOCIOS
                            
                            objUsuario = lstUsuario.FirstOrDefault(w => w.cveEmpleado == cveEmpDirectorLinea && w.estatus);

                            if (objPrestamo.idDirectorLineaN > 0)
                            {
                                var objAutorizacionLineaNegocios = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorLineaN && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAutorizacionLineaNegocios == null)
                                {
                                    if (lstCorreos.Count() == 0)
                                    {
                                        lstCorreos.Add(objUsuario.correo);

                                        #region Alerta SIGOPLAN
                                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                        objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                        objNuevaAlerta.tipoAlerta = 2;
                                        objNuevaAlerta.sistemaID = 16;
                                        objNuevaAlerta.visto = false;
                                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                        objNuevaAlerta.objID = objPrestamo.id;
                                        objNuevaAlerta.obj = "AutorizacionPrestamos";
                                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                        objNuevaAlerta.documentoID = 0;
                                        objNuevaAlerta.moduloID = 0;
                                        _context.tblP_Alerta.Add(objNuevaAlerta);
                                        _context.SaveChanges();
                                        #endregion //ALERTA SIGPLAN
                                    }

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director de Linea de Negocios",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpDirectorLinea
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director de Linea de Negocios",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.AUTORIZADO,
                                        cveEmpleado = cveEmpDirectorLinea
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region DIRECTOR GENERAL

                            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpDirectoGral && w.estatus).FirstOrDefault();
                            if (objPrestamo.idDirectorGeneral > 0 )
                            {
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                var objAuthDireGral = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idDirectorGeneral && w.registroActivo);

                                if (objAuthDireGral == null)
                                {
                                    if (lstCorreos.Count() == 0)
                                    {
                                        lstCorreos.Add(objUsuario.correo);

                                        #region Alerta SIGOPLAN
                                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                        objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                        objNuevaAlerta.tipoAlerta = 2;
                                        objNuevaAlerta.sistemaID = 16;
                                        objNuevaAlerta.visto = false;
                                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                        objNuevaAlerta.objID = objPrestamo.id;
                                        objNuevaAlerta.obj = "AutorizacionPrestamos";
                                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                        objNuevaAlerta.documentoID = 0;
                                        objNuevaAlerta.moduloID = 0;
                                        _context.tblP_Alerta.Add(objNuevaAlerta);
                                        _context.SaveChanges();
                                        #endregion //ALERTA SIGPLAN
                                    }

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director General",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpDirectoGral
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Director General",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.AUTORIZADO,
                                        cveEmpleado = cveEmpDirectoGral
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region CH

                            objUsuario = lstUsuario.Where(w => w.cveEmpleado == cveEmpCH && w.estatus).FirstOrDefault();
                            
                            if (objPrestamo.idCapitalHumano > 0)
                            {
                                var objAuthCH = _context.tblRH_EK_PrestamosAutorizaciones.FirstOrDefault(w => w.FK_Prestamo == objPrestamo.id && w.cveEmpleado == objPrestamo.idCapitalHumano && w.registroActivo);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;

                                if (objAuthCH == null)
                                {
                                    if (lstCorreos.Count() == 0)
                                    {
                                        lstCorreos.Add(objUsuario.correo);

                                        #region Alerta SIGOPLAN
                                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                        objNuevaAlerta.userRecibeID = objUsuario.id;
#if DEBUG
                                        //objNuevaAlerta.userRecibeID = 7939; //USUARIO ID: Omar Nuñez.
                                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
                                        objNuevaAlerta.userRecibeID = 13; //USUARIO ID: ADMIN
#endif
                                        objNuevaAlerta.tipoAlerta = 2;
                                        objNuevaAlerta.sistemaID = 16;
                                        objNuevaAlerta.visto = false;
                                        objNuevaAlerta.url = "/Administrativo/ReportesRH/GestionPrestamo?cargarModal=1" + "&idEmpleado=" + objPrestamo.clave_empleado + "&ccEmpleado=" + objPrestamo.cc + "&tipoDePrestamo=" + objPrestamo.tipoPrestamo + "&statusPrestamo=" + objPrestamo.estatus;
                                        objNuevaAlerta.objID = objPrestamo.id;
                                        objNuevaAlerta.obj = "AutorizacionPrestamos";
                                        objNuevaAlerta.msj = "Prestamo Pendiente de Autorizar - Empleado: " + objPrestamo.clave_empleado;
                                        objNuevaAlerta.documentoID = 0;
                                        objNuevaAlerta.moduloID = 0;
                                        _context.tblP_Alerta.Add(objNuevaAlerta);
                                        _context.SaveChanges();
                                        #endregion //ALERTA SIGPLAN
                                    }

                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Capital Humano",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.PENDIENTE,
                                        cveEmpleado = cveEmpCH
                                    });
                                }
                                else
                                {
                                    lstAuth.Add(new AutorizantesPerstamosDTO
                                    {
                                        nombreCompleto = nombreCompleto,
                                        descPuesto = "Capital Humano",
                                        descEstatus = EstatusAutorizacionPrestamosEnum.AUTORIZADO,
                                        cveEmpleado = cveEmpCH
                                    });

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == objUsuario.id && e.visto == false && e.objID == objPrestamo.id && e.obj == "AutorizacionPrestamos");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                            }

                            #endregion

                            #region CUERPO DEL CORREO
                            string cuerpo =
                                            @"<html>
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
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Buen día, se informa que el prestamo del CC " + GetCCDescripcion(objPrestamo.cc) + @", ha sido autorizado<br><br>
                                                    </p>
                                                    <br><br><br>
                                            ";

                            #region TABLA AUTORIZANTES
                            cuerpo += @"
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                            bool esAuth = false;
                            int totalSiguientes = 0;

                            //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                            foreach (var itemDet in lstAuth)
                            {
                                cuerpo += "<tr>" +
                                            "<td>" + itemDet.nombreCompleto + "</td>" +
                                            "<td>" + itemDet.descPuesto + "</td>" +
                                            getEstatus((int)itemDet.descEstatus, esAuth) +
                                        "</tr>";

                                if (vSesiones.sesionUsuarioDTO.cveEmpleado == itemDet.cveEmpleado && totalSiguientes == 0)
                                {
                                    esAuth = true;
                                    totalSiguientes++;
                                }
                                else
                                {
                                    if (esAuth)
                                    {
                                        esAuth = false;

                                        if (itemDet.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO)
                                        {
                                            esAuth = true;
                                            totalSiguientes = 0;
                                        }
                                    }
                                }
                            }

                            cuerpo += "</tbody>" +
                                        "</table>" +
                                        "<br><br><br>";


                            #endregion

                            cuerpo += "<br><br><br>" +
                                  "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                  "Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br><br>" +
                                  "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                  "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                    " </body>" +
                                  "</html>";
                            #endregion

                            #region PDF

                            ReportDocument rptCV = new rptPrestamos();

                            //string path = Path.Combine(RutaServidor, "rptPrestamos.rpt");
                            //rptCV.Load(path);

                            //repPrestamosDTO infoPrestamo = GetInfoPrestamos(objPrestamo.clave_empleado);
                            //rd.SetParameterValue("ubicacion", "HERMOSILLO, SON");
                            rptCV.SetParameterValue("fechaActual", DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                            rptCV.SetParameterValue("ccDescripcion", infoPrestamo.ccDescripcion.Trim());
                            rptCV.SetParameterValue("nombrePuesto", infoPrestamo.nombrePuesto.Trim());
                            rptCV.SetParameterValue("categoriaPuesto", !string.IsNullOrEmpty(infoPrestamo.descCategoriaPuesto) ? (infoPrestamo.descCategoriaPuesto.Trim()) : "S/N");
                            rptCV.SetParameterValue("nombreCompleto", infoPrestamo.nombreCompleto.Trim());

                            string empresa = null;
                            if (infoPrestamo.empresa == empresa)
                            {
                                rptCV.SetParameterValue("empresa", "");
                            }
                            else
                            {
                                rptCV.SetParameterValue("empresa", infoPrestamo.empresa.Trim());
                            }

                            rptCV.SetParameterValue("fecha_alta", Convert.ToDateTime(infoPrestamo.fecha_alta).ToString("dd \\de MMMM \\de yyyy", new System.Globalization.CultureInfo("es-ES")));
                            rptCV.SetParameterValue("tipoNomina", infoPrestamo.tipoNomina.Trim());
                            rptCV.SetParameterValue("sueldo_base", infoPrestamo.sueldo_base);
                            rptCV.SetParameterValue("complemento", infoPrestamo.complemento);
                            rptCV.SetParameterValue("bono_zona", infoPrestamo.bono_zona);
                            rptCV.SetParameterValue("totalN", infoPrestamo.totalN);
                            rptCV.SetParameterValue("totalM", infoPrestamo.totalM);

                            #region asignacion a casillas
                            string pago1 = "";
                            string pago2 = "";

                            if (infoPrestamo.formaPago == "12 Quincenas")
                            {
                                pago1 = "X";
                                pago2 = " ";
                                rptCV.SetParameterValue("formaPago12", pago1);
                                rptCV.SetParameterValue("formaPago24", pago2);
                            }
                            else
                            {
                                pago1 = " ";
                                pago2 = "X";
                                rptCV.SetParameterValue("formaPago12", pago1);
                                rptCV.SetParameterValue("formaPago24", pago2);

                            }


                            string salud = "";
                            string defuncion = "";
                            string daño = "";
                            string apoyo = "";
                            string sindicato = "";

                            if (infoPrestamo.motivoPrestamo == 1)
                            {
                                salud = "X";
                                defuncion = "";
                                daño = "";
                                apoyo = "";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);

                            }
                            else if (infoPrestamo.motivoPrestamo == 2)
                            {
                                salud = "";
                                defuncion = "X";
                                daño = "";
                                apoyo = "";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);

                            }
                            else if (infoPrestamo.motivoPrestamo == 3)
                            {
                                salud = "";
                                defuncion = "";
                                daño = "X";
                                apoyo = "";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);
                            }
                            else if (infoPrestamo.motivoPrestamo == 4)
                            {
                                salud = "";
                                defuncion = "";
                                daño = "";
                                apoyo = "X";
                                sindicato = "";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);
                            }
                            else if (infoPrestamo.motivoPrestamo == 5)
                            {
                                salud = "";
                                defuncion = "";
                                daño = "";
                                apoyo = "";
                                sindicato = "X";

                                rptCV.SetParameterValue("motivoSalud", salud);
                                rptCV.SetParameterValue("motivoDef", defuncion);
                                rptCV.SetParameterValue("motivoDaño", daño);
                                rptCV.SetParameterValue("motivoApoyo", apoyo);
                                rptCV.SetParameterValue("motivoSindicato", sindicato);
                            }

                            if (infoPrestamo.cantidadSoli < 10000)
                            {
                                rptCV.SetParameterValue("IFE", "X");
                                rptCV.SetParameterValue("SOPORTE", "X");
                                rptCV.SetParameterValue("PAGARE", "");
                            }
                            if (infoPrestamo.cantidadSoli >= 10000)
                            {
                                rptCV.SetParameterValue("IFE", "X");
                                rptCV.SetParameterValue("SOPORTE", "X");
                                rptCV.SetParameterValue("PAGARE", "X");
                            }
                            #endregion

                            string descPrestamos = "";

                            if (infoPrestamo.tipoPrestamo == "MayorIgualA10")
                            {
                                descPrestamos = "MAYOR O IGUAL A $10,000.00";

                            }
                            else if (infoPrestamo.tipoPrestamo == "MenorA10")
                            {
                                descPrestamos = "MENOR A $10,000.00";

                            }
                            else
                            {
                                descPrestamos = " ";

                            }

                            rptCV.SetParameterValue("cantSoli", infoPrestamo.cantidadSoli);
                            rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra);
                            rptCV.SetParameterValue("cantMax", infoPrestamo.cantidadMax);
                            rptCV.SetParameterValue("cantDescontar", infoPrestamo.cantidadDescontar);
                            rptCV.SetParameterValue("otrosDesc", infoPrestamo.otrosDescuento);
                            rptCV.SetParameterValue("justificacion", infoPrestamo.justificacion.Trim());
                            rptCV.SetParameterValue("tipoSolicitud", infoPrestamo.tipoSolicitud.Trim());
                            rptCV.SetParameterValue("tipoPrestamo", descPrestamos);
                            rptCV.SetParameterValue("tipoPuesto", infoPrestamo.tipoPuesto.Trim());
                            rptCV.SetParameterValue("cantidadLetra", infoPrestamo.cantidadLetra.Trim());

                            if (infoPrestamo.idResponsableCC == null || infoPrestamo.idGerenteOdirector == null || infoPrestamo.idDirectorGeneral == null || infoPrestamo.idDirectorLineaN == null)
                            {
                                rptCV.SetParameterValue("nombreResponsableCC", "");
                                rptCV.SetParameterValue("puestoResponsableCC", "");
                                rptCV.SetParameterValue("nombreDirectorGeneral", "");
                                rptCV.SetParameterValue("puestoDirectorGeneral", "");
                                rptCV.SetParameterValue("nombreDirectorLineaN", "");
                                rptCV.SetParameterValue("puestoDirectorLineaN", "");
                                rptCV.SetParameterValue("nombreGerenteOdirector", "");
                                rptCV.SetParameterValue("puestoGerenteOdirector", "");
                                rptCV.SetParameterValue("nombreCapitalHumano", "");
                                rptCV.SetParameterValue("puestoCapitalHumano", "");
                            }
                            else
                            {
                                rptCV.SetParameterValue("nombreResponsableCC", infoPrestamo != null && infoPrestamo.idResponsableCC > 0 ? infoPrestamo.nombreResponsableCC.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoResponsableCC", "RESPONSABLE DE CC");
                                rptCV.SetParameterValue("nombreDirectorGeneral", infoPrestamo != null && infoPrestamo.idDirectorGeneral > 0 ? infoPrestamo.nombreDirectorGeneral.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoDirectorGeneral", "DIRECTOR GENERAL");
                                rptCV.SetParameterValue("nombreDirectorLineaN", infoPrestamo != null && infoPrestamo.idDirectorLineaN > 0 ? infoPrestamo.nombreDirectorLineaN.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoDirectorLineaN", "DIRECTOR LINEA DE NEGOCIO");
                                rptCV.SetParameterValue("nombreGerenteOdirector", infoPrestamo != null && infoPrestamo.idGerenteOdirector > 0 ? infoPrestamo.nombreGerenteOdirector.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoGerenteOdirector", "GERENTE/DIRECTOR DE AREA");
                                rptCV.SetParameterValue("nombreCapitalHumano", infoPrestamo != null && infoPrestamo.idCapitalHumano > 0 ? infoPrestamo.nombreCapitalHumano.Trim() : string.Empty);
                                rptCV.SetParameterValue("puestoCapitalHumano", "GERENTE/DIRECTOR DE CAPITAL HUMANO");

                                //Firmas
                                var objIdResponsable = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idResponsableCC.Value.ToString());
                                var objIdDirectorGeneral = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idDirectorGeneral.Value.ToString());
                                var objIdDirectorLineaN = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idDirectorLineaN.Value.ToString());
                                var objIdGerenteOdirector = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idGerenteOdirector.Value.ToString());
                                var objidCapitalHumano = lstAuth.FirstOrDefault(e => e.cveEmpleado == infoPrestamo.idCapitalHumano.ToString());

                                rptCV.SetParameterValue("FirmaResponsableCC", (
                                objIdResponsable != null && objIdResponsable.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idResponsableCC)) : " "));
                                rptCV.SetParameterValue("FirmaSolicitante", GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.clave_empleado)));
                                rptCV.SetParameterValue("FirmaDirectorGeneral", (
                                    objIdDirectorGeneral != null && objIdDirectorGeneral.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idDirectorGeneral)) : " "));
                                rptCV.SetParameterValue("FirmaDirectorLineaN", (
                                    objIdDirectorLineaN != null && objIdDirectorLineaN.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idDirectorLineaN)) : " "));
                                rptCV.SetParameterValue("FirmaGerenteOdirector", (
                                    objIdGerenteOdirector != null && objIdGerenteOdirector.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idGerenteOdirector)) : " "));
                                rptCV.SetParameterValue("FirmaCapitalHumano", (
                                    objidCapitalHumano != null && objidCapitalHumano.descEstatus == EstatusAutorizacionPrestamosEnum.AUTORIZADO ?
                                    GlobalUtils.CrearFirmaDigital(infoPrestamo.id, DocumentosEnum.FirmasPrestamos, Convert.ToInt32(infoPrestamo.idCapitalHumano)) : " "));
                            }
                            rptCV.SetParameterValue("folio", ((infoPrestamo.cc == null ? "" : infoPrestamo.cc) + "-" + (infoPrestamo.clave_empleado) + "-" + (infoPrestamo.consecutivo.ToString().PadLeft(3, '0'))));

                            Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                            #endregion

                            #region SE ENVIA CORREO A DIANA, ARANZA Y AL PRIMER AUTORIZANTE
                            tblP_Usuario objUsuarioCreadorPrestamo = _context.tblP_Usuario.Where(w => w.id == objPrestamo.idUsuaioCreacion && w.estatus).FirstOrDefault();
                            if (objUsuarioCreadorPrestamo != null)
                                lstCorreos.Add(objUsuarioCreadorPrestamo.correo);

                            lstCorreos.Add("keyla.vasquez@construplan.com.mx");
                            lstCorreos.Add("diana.alvarez@construplan.com.mx");
#if DEBUG
                            lstCorreos = new List<string>();
                            lstCorreos.Add("aaron.gracia@construplan.com.mx");
                            lstCorreos.Add("miguel.buzani@construplan.com.mx");
#endif
                            string subject = "Prestamo Empleado: #" + objPrestamo.clave_empleado + " " + nombreCompletoEmpPrestamo;
                            //string body = string.Format("Buen día, se informa que el prestamo del CC {0}, se encuentra listo para ser autorizado" +
                            //                            "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan > Capital Humano > Administración de personal > Prestamos > Gestión.<br>" +
                            //                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>)." +
                            //                            "No es necesario dar una respuesta. Gracias.", GetCCDescripcion(objPrestamo.cc));
                            //GlobalUtils.sendEmail(subject, cuerpo, lstCorreos);

                            var lstArchives = new List<adjuntoCorreoDTO>();
                            List<tblRH_EK_PrestamosArchivos> lstArchivos = _context.tblRH_EK_PrestamosArchivos.Where(w => w.FK_Prestamo == objPrestamo.id && w.registroActivo).ToList();

                            foreach (var item in lstArchivos)
                            {
                                var fileStream = GlobalUtils.GetFileAsStream(item.rutaArchivo);
                                using (var streamReader = new MemoryStream())
                                {
                                    fileStream.CopyTo(streamReader);
                                    //downloadFiles.Add(streamReader.ToArray());

                                    lstArchives.Add(new adjuntoCorreoDTO
                                    {
                                        archivo = streamReader.ToArray(),
                                        nombreArchivo = Path.GetFileNameWithoutExtension(item.rutaArchivo),
                                        extArchivo = Path.GetExtension(item.rutaArchivo)
                                    });
                                }
                            }

                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);

                                lstArchives.Add(new adjuntoCorreoDTO
                                {
                                    archivo = streamReader.ToArray(),
                                    nombreArchivo = "Prestamo",
                                    extArchivo = ".pdf"
                                });


                                GlobalUtils.sendMailWithFilesReclutamientos(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject),
                                    cuerpo, lstCorreos, lstArchives);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objFiltroDTO.esAutorizar ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "AutorizarRechazarPrestamo", e, AccionEnum.AGREGAR, 0, objFiltroDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public DataTable getInfoEnca(string nombreReporte, string area)
        {
            string RutaServidor = "";

#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\ENCABEZADOS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\ENCABEZADOS";
#endif

            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("logo", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("nombreEmpresa", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("nombreReporte", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("area", System.Type.GetType("System.String"));

            var data = encabezadoFactoryServices.getEncabezadoDatos();
            string path = data.logo;
            //string path = Path.Combine(RutaServidor, Path.GetFileName(data.logo));
            byte[] imgdata = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath(path));
            string empresa = data.nombreEmpresa;

            tableEncabezado.Rows.Add(imgdata, empresa, nombreReporte, area);

            return tableEncabezado;
        }

        private string getEstatus(int est, bool aut)
        {
            if ((int)EstatusAutorizacionPrestamosEnum.PENDIENTE == (est) && aut)
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            else if ((int)EstatusAutorizacionPrestamosEnum.AUTORIZADO == (est))
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else
                if ((int)EstatusAutorizacionPrestamosEnum.RECHAZADO == (est))
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                else
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }
        #endregion

        #region DASHBOARD PRESTAMOS
        public Dictionary<string, object> GetDashboardPrestamos(FiltroPrestamosDTO objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE PRESTAMOS
                List<string> lstCC = new List<string>();
                if (objFiltroDTO.lstCC != null)
                {
                    foreach (var item in objFiltroDTO.lstCC)
                    {
                        if (!string.IsNullOrEmpty(item))
                            lstCC.Add(item.Trim());
                    }
                }

                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<FiltroPrestamosDTO> lstPrestamos = _context.Select<FiltroPrestamosDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT cc, fecha_creacion, tipoPrestamo FROM tblRH_EK_Prestamos WHERE registroActivo = @registroActivo ORDER BY cc",
                    parametros = new { registroActivo = true }
                }).ToList();

                if (lstCC.Count() > 0)
                    lstPrestamos = lstPrestamos.Where(w => lstCC.Contains(w.cc)).ToList();

                if (objFiltroDTO.fechaInicio.Value.Year > 2000 && objFiltroDTO.fechaFin.Value.Year > 2000)
                    lstPrestamos = lstPrestamos.Where(w => w.fecha_creacion >= objFiltroDTO.fechaInicio && w.fecha_creacion <= objFiltroDTO.fechaFin).ToList();
                #endregion

                #region SE OBTIENE LA CANTIDAD DE PRESTAMOS POR CC
                List<GraficaDTO> lstGraficaCantidadPrestamosPorCC = new List<GraficaDTO>();
                GraficaDTO objGraficaDTO = new GraficaDTO();
                foreach (var item in lstPrestamos)
                {

                    if (!string.IsNullOrEmpty(item.cc))
                    {
                        GraficaDTO objCC_EnLaLista = lstGraficaCantidadPrestamosPorCC.Where(w => w.name == item.cc.Trim().ToUpper()).FirstOrDefault();
                        if (objCC_EnLaLista == null)
                        {
                            objGraficaDTO = new GraficaDTO();
                            objGraficaDTO.name = !string.IsNullOrEmpty(item.cc) ? string.Format("[{0}]", item.cc.Trim().ToUpper()) : string.Empty;
                            objGraficaDTO.y = lstPrestamos.Where(w => w.cc == item.cc).Count();
                            objGraficaDTO.drilldown = objGraficaDTO.name;
                            lstGraficaCantidadPrestamosPorCC.Add(objGraficaDTO);
                        }
                    }
                }
                #endregion

                #region SE OBTIENE LA CANTIDAD DE PRESTAMOS POR TIPO DE PRESTAMO
                List<GraficaDTO> lstGraficaCantidadPrestamosPorTipoPrestamos = new List<GraficaDTO>();
                objGraficaDTO = new GraficaDTO();

                // SINDICATO
                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Sindicato";
                objGraficaDTO.y = lstPrestamos.Where(w => w.tipoPrestamo == "SINDICATO").Count();
                objGraficaDTO.drilldown = objGraficaDTO.name;
                lstGraficaCantidadPrestamosPorTipoPrestamos.Add(objGraficaDTO);

                // MAYOR A 10K
                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Mayor a 10k";
                objGraficaDTO.y = lstPrestamos.Where(w => w.tipoPrestamo == "MayorIgualA10").Count();
                objGraficaDTO.drilldown = objGraficaDTO.name;
                lstGraficaCantidadPrestamosPorTipoPrestamos.Add(objGraficaDTO);

                // MENOR A 10K
                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Menor a 10k";
                objGraficaDTO.y = lstPrestamos.Where(w => w.tipoPrestamo == "MenorA10").Count();
                objGraficaDTO.drilldown = objGraficaDTO.name;
                lstGraficaCantidadPrestamosPorTipoPrestamos.Add(objGraficaDTO);
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("lstGraficaCantidadPrestamosPorCC", lstGraficaCantidadPrestamosPorCC);
                resultado.Add("lstGraficaCantidadPrestamosPorTipoPrestamos", lstGraficaCantidadPrestamosPorTipoPrestamos);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetDashboardPrestamos", e, AccionEnum.CONSULTA, 0, objFiltroDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion
        #endregion

        public List<RepActivosDTO> GetEmpleadosActivos_CC(string cc)
        {
            var resultFiltered = new List<RepActivosDTO>();
            try
            {

                var result = _context.Select<RepActivosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT 
                                e.cc_contable,
	                            (e.cc_contable+' - '+c.ccDescripcion) as cC,
                                e.clave_empleado as empleadoID,(e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as empleado,
	                            pu.descripcion as puesto, pu.puesto as idPuesto, tn.tipo_nomina as tipo_nominaID, tn.descripcion as tipo_nomina,e.nss,(ne.ape_paterno+' '+ne.ape_materno+' '+ne.nombre) as jefeInmediato,
--	                            CONVERT( CHAR( 20 ),
--		                            (|
--		                            SELECT TOP 1 ser.fecha_reingreso 
--		                            FROM tblRH_EK_Empl_Recontratacion as ser 
--		                            where ser.clave_empleado = e.clave_empleado
--		                            AND ser.cc = e.cc_contable AND ser.fecha_reingreso > e.fecha_alta ORDER BY ser.fecha_reingreso DESC
--		                            ), 103 
--	                            ) as fechaRe,
                                CONVERT(CHAR(20), e.fecha_alta, 103) as fechaAltaStr,
	                            CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as fechaAltaRe,
	                            tDeps.desc_depto as departamento,
                                tDeps.clave_depto as departamentoID,
	                            e.requisicion,
	                            tRegPat.nombre_corto as regpat,
	                            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as salario_base,
                                (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as complemento,
                                (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado = e.clave_empleado order by id desc) as bono_zona,
	                            (tGrales.domicilio + ' #' + tGrales.numero_exterior + ' Col. '+tGrales.colonia) as domicilio,
	                            tEstado.descripcion as nombre_estado_nac,
	                            tCuidad.descripcion as nombre_ciudad_nac,
	                            CONVERT(CHAR(20), e.fecha_nac, 103) as fecha_nac,
	                            tGrales.email,
	                            tGrales.tel_cel,
	                            tGrales.tel_casa,
	                            tGrales.en_accidente_nombre,
	                            tGrales.en_accidente_telefono,
	                            e.sexo,
	                            e.rfc,
	                            e.curp,
	                            tGrales.estado_civil,
	                            --estudios
	                            tGrales.codigo_postal,
	                            tTipoSangre.tipoSangre,
	                            tGrales.alergias,
	                            tTipoCasa.tipoCasa,
	                            tGrales.ocupacion,
	                            (
		                            SELECT COUNT(*) FROM tblRH_EK_Empl_Familia as tFam WHERE e.clave_empleado = tFam.clave_empleado AND tFam.parentesco = 4
	                            ) as numHijos,
	                            (
		                            SELECT TOP 1 apellido_paterno+' '+apellido_materno+' '+nombre FROM tblRH_EK_Empl_Familia as tFam WHERE e.clave_empleado = tFam.clave_empleado AND tFam.parentesco = 3 ORDER BY id DESC
	                            ) as nombreConyuge,
                                tContratos.nombre as contratoDesc,
								tabCat.concepto as descCategoria,
								eCompl.camisa as camisa,
								eCompl.calzado as calzado,
								eCompl.pantalon as pantalon,
                                tGrales.num_dni as dni,
								e.cuspp,
                                (
									select top 1
										ciudadContacto.descripcion
									from
										tblRH_EK_Ciudades as ciudadContacto
									where
										ciudadContacto.clave_cuidad = tGrales.cuidado_dom and
										ciudadContacto.clave_estado = tGrales.estado_dom and
										ciudadContacto.clave_pais = tGrales.pais_dom
								) as ciudadContacto
                            FROM tblRH_EK_Empleados as e 
                            inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado 
                            inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc_contable
                            inner join tblRH_EK_Puestos as pu on e.puesto = pu.puesto
                            inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
                            LEFT JOIN tblRH_EK_Departamentos as tDeps ON e.clave_depto = tDeps.clave_depto
                            LEFT JOIN tblRH_EK_Registros_Patronales as tRegPat ON e.id_regpat = tRegPat.clave_reg_pat
                            LEFT JOIN tblRH_EK_Empl_Grales as tGrales ON e.clave_empleado = tGrales.clave_empleado
                            LEFT JOIN tblRH_EK_Estados as tEstado ON (e.clave_estado_nac = tEstado.clave_estado AND e.clave_pais_nac = tEstado.clave_pais)
                            LEFT JOIN tblRH_EK_Ciudades as tCuidad ON (e.clave_ciudad_nac = tCuidad.clave_cuidad AND e.clave_estado_nac = tCuidad.clave_estado AND e.clave_pais_nac = tCuidad.clave_pais)
                            LEFT JOIN tblP_CatTipoSangre as tTipoSangre ON tTipoSangre.id = tGrales.tipo_sangre
                            LEFT JOIN tblP_CatTipoCasa as tTipoCasa ON tGrales.tipo_casa = tTipoCasa.id
                            LEFT JOIN tblRH_EK_Empl_Duracion_Contrato as tContratos ON e.duracion_contrato = tContratos.clave_duracion
							LEFT JOIN tblRH_REC_Requisicion AS tabReq ON tabReq.id = e.requisicion
							LEFT JOIN tblRH_TAB_TabuladoresDet AS tabDet ON tabDet.id = tabReq.idTabuladorDet
							LEFT JOIN tblRH_TAB_CatCategorias AS tabCat ON tabDet.FK_Categoria = tabCat.id
						    LEFT JOIN tblRH_EK_Empl_Complementaria AS eCompl ON e.clave_empleado = eCompl.clave_empleado
            		        WHERE e.cc_contable = @cc AND e.estatus_empleado = 'A' AND tDeps.clave_depto=2",
                    parametros = new { cc }
                });

                foreach (var i in result)
                {
                    var objAlta = resultFiltered.Where(e => e.empleadoID == i.empleadoID && e.puesto == i.puesto).FirstOrDefault();

                    if (objAlta == null)
                    {
                        var bono_cuadrado = _context.tblRH_BN_Plantilla_Cuadrado_Det.FirstOrDefault(x => x.empleado == i.empleadoID);
                        if(bono_cuadrado != null)
                        {
                            i.bono_cuadrado = bono_cuadrado.monto;
                        }
                        resultFiltered.Add(i);
                    }

                }

                //return resultFiltered.OrderBy(x => x.empleado).ToList();
                
            }
            catch (Exception e)
            {

            }

            return resultFiltered.ToList();
        }
    }
}
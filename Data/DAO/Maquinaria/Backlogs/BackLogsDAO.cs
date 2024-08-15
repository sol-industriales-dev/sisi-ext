using Core.DAO.Maquinaria.BackLogs;
using Core.DTO;
using Core.DTO.Maquinaria;
using Core.DTO.Maquinaria.BackLogs;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.BackLogs;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Enum;
using Core.Enum.Administracion.Cotizaciones;
using Core.Enum.Maquinaria.BackLogs;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.Dxf;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Enum.Maquinaria;
using Core.Entity.Administrativo.Almacen;
using Core.DTO.Principal.Usuarios;
using Core.Enum.RecursosHumanos.Reclutamientos;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using System.Data.Entity;
using Core.Entity.StarSoft.Requisiciones;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Enkontrol.Compras.Requisicion;

namespace Data.DAO.Maquinaria.Backlogs
{
    public class BackLogsDAO : GenericDAO<tblBL_CatBackLogs>, IBackLogsDAO
    {
        #region INIT
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "BackLogsController";
        private const int _SISTEMA = (int)SistemasEnum.MAQUINARIA;
        private List<int> _LST_TIPO_EQUIPO = new List<int>();
        private readonly string _RUTA_BACKLOGS_SERVIDOR;
        private readonly string _RUTA_BASE = @"\\10.1.0.112\Proyecto\SIGOPLANARRENDADORA\BACKLOGS";
        private readonly string _RUTA_LOCAL = @"C:\MAQUINARIA\BACKLOGS";

        public BackLogsDAO()
        {
            #region SE INDICA LA RUTA DE LOCAL/SERVIDOR
#if DEBUG
            _RUTA_BACKLOGS_SERVIDOR = Path.Combine(_RUTA_LOCAL, @"C:\MAQUINARIA\BACKLOGS");
#else
            _RUTA_BACKLOGS_SERVIDOR = Path.Combine(_RUTA_BASE, @"\\10.1.0.112\Proyecto\SIGOPLANARRENDADORA\BACKLOGS");
#endif
            #endregion

            #region SE INDICA EL TIPO DE EQUIPO EN LISTA GLOBAL
            _LST_TIPO_EQUIPO.Add((int)TipoMaquinaEnum.Menor);
            _LST_TIPO_EQUIPO.Add((int)TipoMaquinaEnum.Mayor);
            _LST_TIPO_EQUIPO.Add((int)TipoMaquinaEnum.Transporte);
            #endregion
        }
        #endregion

        #region BACKLOGS OBRA
        #region INDEX
        public Dictionary<string, object> GetBackLogsGraficaIndex(BackLogsDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE BACKLOGS EN BASE AL MES SELECCIONADO
                DateTime fechaBL = new DateTime(objDTO.anio, objDTO.Mes, 01);

                int MM = Convert.ToInt32(fechaBL.Month);
                int yyyy = Convert.ToInt32(fechaBL.Year);
                int dd = DateTime.DaysInMonth(yyyy, MM);

                string mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                //string fechaInicio = yyyy + "-" + mmInicio + "-" + "01";
                string fechaInicio = string.Format("{0}-{1}-{2}", 2021, 01, 01);

                string ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                string mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                string fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                List<BackLogsDTO> lstBL = _context.Select<BackLogsDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT folioBL, cc, descripcion, idEstatus, fechaInspeccion, fechaModificacionBL FROM tblBL_CatBackLogs AS t1 
                                        WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND 
                                              (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)  ORDER BY fechaInspeccion DESC",
                    parametros = new { areaCuenta = objDTO.areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).ToList();

                foreach (var item in lstBL)
                {
                    switch (item.idEstatus)
                    {
                        case 1:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.ElaboracionInspeccion));
                            break;
                        case 2:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.ElaboracionRequisicion));
                            break;
                        case 3:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.ElaboracionOC));
                            break;
                        case 4:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.SuministroRefacciones));
                            break;
                        case 5:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.RehabilitacionProgramada));
                            break;
                        case 6:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.ProcesoInstalacion));
                            break;
                        case 7:
                            item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum.BackLogsInstalado));
                            break;
                        default:
                            item.estatus = string.Empty;
                            break;
                    }
                }

                resultado.Add("lstBL", lstBL);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region REGISTRO DE BACKLOGS
        public List<BackLogsDTO> GetBackLogsFiltros(BackLogsDTO objBackLog, bool esObra)
        {
            try
            {
                #region SE OBTIENE LISTADO DE SUBCONJUNTOS
                List<tblBL_CatSubconjuntos> lstCatSubconjuntos = _context.Select<tblBL_CatSubconjuntos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT * FROM tblBL_CatSubconjuntos"
                }).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE CONJUNTOS
                List<tblBL_CatConjuntos> lstCatConjuntos = _context.Select<tblBL_CatConjuntos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT * FROM tblBL_CatConjuntos"
                }).ToList();
                #endregion

                if (esObra)
                {
                    #region LISTADO BL OBRA
                    List<tblBL_CatBackLogs> lstBLDapper = _context.Select<tblBL_CatBackLogs>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT * FROM tblBL_CatBackLogs 
                                        WHERE esActivo = 1 AND 
                                              tipoBL = 1 AND 
                                              areaCuenta = @areaCuenta",
                        parametros = new { areaCuenta = objBackLog.areaCuenta }
                    }).ToList();

                    tblBL_CatConjuntos obj = new tblBL_CatConjuntos();
                    List<BackLogsDTO> lstBackLogs = lstBLDapper.Where(x =>
                        (!string.IsNullOrEmpty(objBackLog.noEconomico) ? x.noEconomico == objBackLog.noEconomico : true) &&
                        (objBackLog.idSubconjunto > 0 ? x.idSubconjunto == objBackLog.idSubconjunto : true) &&
                        (objBackLog.Mes > 0 ? x.fechaInspeccion.Month == objBackLog.Mes : true) &&
                        (objBackLog.anio == x.fechaCreacionBL.Year) &&
                        (objBackLog.lstEstatus != null ? objBackLog.lstEstatus.Contains(x.idEstatus) : true) && x.esActivo && x.tipoBL == (int)TipoBackLogEnum.Obra)
                    .OrderByDescending(x => x.folioBL).Select(x => new BackLogsDTO
                    {
                        id = x.id,
                        folioBL = x.folioBL,
                        fechaInspeccion = x.fechaInspeccion,
                        cc = x.cc,
                        noEconomico = x.noEconomico,
                        horas = x.horas,
                        subconjunto = string.Empty,
                        idSubconjunto = x.idSubconjunto,
                        conjunto = string.Empty,
                        descripcion = x.descripcion,
                        parte = x.parte,
                        manoObra = x.manoObra,
                        estatus = EnumHelper.GetDescription((EstatusBackLogEnum)x.idEstatus),
                        idEstatus = x.idEstatus,
                        diasTotales = DiasTranscurridos(x.id, x.idEstatus, Convert.ToDateTime(x.fechaCreacionBL), Convert.ToDateTime(x.fechaModificacionBL)),
                        fechaModificacionBL = x.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado ? Convert.ToDateTime(x.fechaInstaladoBL) : Convert.ToDateTime(null),
                        fechaCreacionBL = x.fechaCreacionBL
                    }).ToList();

                    foreach (var item in lstBackLogs)
                    {
                        #region SE OBTIENE EL SUBCONJUNTO
                        int idConjunto = 0;
                        string subconjunto = string.Empty;
                        if (item.idSubconjunto > 0)
                        {
                            tblBL_CatSubconjuntos objSubconjunto = lstCatSubconjuntos.Where(w => w.id == item.idSubconjunto).FirstOrDefault();
                            if (objSubconjunto != null)
                            {
                                item.subconjunto = !string.IsNullOrEmpty(objSubconjunto.abreviacion) ? objSubconjunto.abreviacion.ToUpper() : objSubconjunto.descripcion.ToUpper();
                                idConjunto = objSubconjunto.idConjunto;
                            }
                        }
                        #endregion

                        #region SE OBTIENE EL CONJUNTO
                        string conjunto = string.Empty;
                        if (idConjunto > 0)
                        {
                            tblBL_CatConjuntos objConjunto = lstCatConjuntos.Where(w => w.id == idConjunto).FirstOrDefault();
                            if (objConjunto != null)
                                item.conjunto = !string.IsNullOrEmpty(objConjunto.abreviacion) ? objConjunto.abreviacion.ToUpper() : objConjunto.descripcion.ToUpper();
                        }
                        #endregion
                    }
                    return lstBackLogs;
                    #endregion
                }
                else
                {
                    #region LISTADO BL TMC
                    List<tblBL_Requisiciones> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.esActivo).ToList();
                    List<tblBL_OrdenesCompra> lstOC = _context.tblBL_OrdenesCompra.Where(x => x.esActivo).ToList();

                    tblBL_CatConjuntos obj = new tblBL_CatConjuntos();
                    List<BackLogsDTO> lstBackLogs = _context.tblBL_CatBackLogs
                        .Where(x => x.areaCuenta == objBackLog.areaCuenta && x.esLiberado == false).ToList().Where(x =>
                        (objBackLog.noEconomico != null ? x.noEconomico == objBackLog.noEconomico : true) &&
                        (objBackLog.idSubconjunto > 0 ? x.idSubconjunto == objBackLog.idSubconjunto : true) &&
                        (objBackLog.Mes > 0 ? x.fechaInspeccion.Month == objBackLog.Mes : true) &&
                        (objBackLog.lstEstatus != null ? objBackLog.lstEstatus.Contains(x.idEstatus) : true) && x.esActivo && x.tipoBL == (int)TipoBackLogEnum.TMC)
                    .OrderByDescending(x => x.folioBL).Select(x => new BackLogsDTO
                    {
                        id = x.id,
                        folioBL = x.folioBL,
                        fechaInspeccion = x.fechaInspeccion,
                        noEconomico = x.noEconomico,
                        horas = x.horas,
                        subconjunto = !string.IsNullOrEmpty(x.subconjunto.abreviacion) ? x.subconjunto.abreviacion : x.subconjunto.descripcion,
                        idSubconjunto = x.idSubconjunto,
                        conjunto = !string.IsNullOrEmpty(x.subconjunto.CatConjuntos.abreviacion) ? x.subconjunto.CatConjuntos.abreviacion : x.subconjunto.CatConjuntos.descripcion,
                        idConjunto = x.subconjunto.CatConjuntos.id,
                        descripcion = x.descripcion,
                        parte = x.parte,
                        manoObra = x.manoObra,
                        estatus = EnumHelper.GetDescription((EstatusBackLogEnum)x.idEstatus),
                        idEstatus = x.idEstatus,
                        totalMX = 0,
                        diasTotales = DiasTranscurridos(x.id, x.idEstatus, Convert.ToDateTime(x.fechaCreacionBL), Convert.ToDateTime(x.fechaModificacionBL)),
                        fechaModificacionBL = x.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado ? Convert.ToDateTime(x.fechaModificacionBL) : Convert.ToDateTime(null),
                        presupuestoEstimado = x.presupuestoEstimado
                    }).ToList();

                    foreach (var item in lstBackLogs)
                    {
                        // SE CAMBIA A MAYUSCULAS EL TEXTO DE CONJUNTOS Y SUBCONJUNTOS
                        if (!string.IsNullOrEmpty(item.conjunto))
                        {
                            string conjunto = item.conjunto.Trim().ToUpper();
                            item.conjunto = conjunto;
                        }

                        if (!string.IsNullOrEmpty(item.subconjunto))
                        {
                            string subconjunto = item.subconjunto.Trim().ToUpper();
                            item.subconjunto = subconjunto;
                        }
                    }

                    return lstBackLogs;
                    #endregion
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public decimal GetTotalOCRehabilitacion(GetTotalMXDTO objTotalDTO)
        {
            try
            {
                #region SE OBTIENE AREA CUENTA
                objTotalDTO.areaCuenta = _context.Select<string>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT areaCuenta WHERE id = @idBL",
                    parametros = new { objTotalDTO.idBL }
                }).FirstOrDefault();
                #endregion

                tblAlm_RelAreaCuentaXAlmacen objAreaAlmacen = _context.tblAlm_RelAreaCuentaXAlmacen.Where(r => r.AreaCuenta == objTotalDTO.areaCuenta).FirstOrDefault();
                List<tblAlm_RelAreaCuentaXAlmacenDet> lstAlmRelAreaCuentaXAlmacenDet = _context.tblAlm_RelAreaCuentaXAlmacenDet.ToList();
                List<tblBL_OrdenesCompra> objOrdenesCompra = _context.tblBL_OrdenesCompra.Where(x => x.esActivo).ToList();
                List<tblBL_Partes> lstPartes = _context.tblBL_Partes.Where(w => w.esActivo).ToList();

                return GetTotalOC(objTotalDTO.areaCuenta, objTotalDTO.idBL, objTotalDTO.noEconomico, objAreaAlmacen, lstAlmRelAreaCuentaXAlmacenDet, objOrdenesCompra, lstPartes);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return 0;
            }
        }

        public decimal GetCostoPromedio(int almacen, int insumo)
        {
            try
            {
                List<dynamic> objCC = new List<dynamic>();
                string strQuery = @"SELECT
                                    SUM((CASE WHEN existencia_ent_ini IS NOT NULL THEN existencia_ent_ini ELSE 0 END) + existencia_ent_ene + existencia_ent_feb + existencia_ent_mar + existencia_ent_abr + existencia_ent_may + existencia_ent_jun + existencia_ent_jul + existencia_ent_ago + existencia_ent_sep + existencia_ent_oct + existencia_ent_nov + existencia_ent_dic) AS entradas,
                                    SUM((CASE WHEN existencia_sal_ini IS NOT NULL THEN existencia_sal_ini ELSE 0 END) + existencia_sal_ene + existencia_sal_feb + existencia_sal_mar + existencia_sal_abr + existencia_sal_may + existencia_sal_jun + existencia_sal_jul + existencia_sal_ago + existencia_sal_sep + existencia_sal_oct + existencia_sal_nov + existencia_sal_dic) AS salidas,
                                    SUM((CASE WHEN importe_ent_ini IS NOT NULL THEN importe_ent_ini ELSE 0 END) + importe_ent_ene + importe_ent_feb + importe_ent_mar + importe_ent_abr + importe_ent_may + importe_ent_jun + importe_ent_jul + importe_ent_ago + importe_ent_sep + importe_ent_oct + importe_ent_nov + importe_ent_dic) AS montoEntradas,
                                    SUM((CASE WHEN importe_sal_ini IS NOT NULL THEN importe_sal_ini ELSE 0 END) + importe_sal_ene + importe_sal_feb + importe_sal_mar + importe_sal_abr + importe_sal_may + importe_sal_jun + importe_sal_jul + importe_sal_ago + importe_sal_sep + importe_sal_oct + importe_sal_nov + importe_sal_dic) AS montoSalidas,
                                    entradas - salidas AS existencias,
                                    montoEntradas - montoSalidas AS montoResultado,
                                        CASE WHEN existencias > 0 THEN (montoResultado / existencias) ELSE 0 END AS costoPromedio
                                            FROM si_acumula_almacen
                                                WHERE insumo = {0} AND ano >= {1} AND almacen = {2}";

                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery, insumo, DateTime.Now.Year, almacen);
                objCC = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);
                return Convert.ToDecimal(objCC[0].costoPromedio);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return 0;
            }
        }

        public int RetornarAlmacen(string areaCuenta)
        {
            try
            {
                int almacen = 0;
                tblAlm_RelAreaCuentaXAlmacen obj = _context.tblAlm_RelAreaCuentaXAlmacen.Where(r => r.AreaCuenta == areaCuenta).FirstOrDefault();
                if (obj != null)
                {
                    int ultimoAlmacen = _context.tblAlm_RelAreaCuentaXAlmacenDet.Where(r => r.idRelacion == obj.id).OrderBy(n => n.Prioridad).Select(y => y.Almacen).FirstOrDefault();
                    almacen = ultimoAlmacen;
                }
                else
                    almacen = 400;

                return almacen;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return 0;
            }
        }

        private decimal GetPresupuestoEstimado(int _idBL, string _cc, List<tblBL_Requisiciones> _lstRequisiciones, List<tblBL_OrdenesCompra> _lstOC)
        {
            try
            {
                int idBL = _idBL;
                string cc = _cc;

                #region SE OBTIENE LAS REQUISICIONES DEL BACKLOG
                List<tblBL_Requisiciones> objRequisicionesBL = _lstRequisiciones.Where(x => x.idBackLog == idBL && x.esActivo).ToList();
                List<string> lstRequisiciones = new List<string>();
                foreach (var item in objRequisicionesBL)
                {
                    lstRequisiciones.Add(item.numRequisicion);
                }
                #endregion

                #region SE OBTIENE LAS ORDENES DE COMPRA DE LA REQUISICIONES ANTERIORES
                List<tblBL_OrdenesCompra> objOC = _lstOC.Where(x => lstRequisiciones.Contains(x.numRequisicion)).ToList();
                List<string> lstOC = new List<string>();
                foreach (var item in objOC)
                {
                    lstOC.Add(item.numOC);
                }
                #endregion

                #region SE OBTIENE EL TOTAL DE LA ORDEN DE COMPRA
                List<BackLogsDTO> objCC = new List<BackLogsDTO>();
                string strQuery = "SELECT total FROM so_orden_compra WHERE cc = '{0}' AND numero = '{1}'";
                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery, cc, string.Join(",", lstOC.ToList()));
                objCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);

                decimal total = objCC.Sum(x => x.total);
                return total;
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return 0;
            }
        }

        public List<InsumosDTO> GetInsumos(InsumosDTO objFiltro)
        {
            try
            {
                List<InsumosDTO> lstInsumosDTO = new List<InsumosDTO>();
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        List<MAEART> lstCatInsumos = _starsoft.MAEART.ToList();

                        #region FILTROS
                        if (!string.IsNullOrEmpty(objFiltro.PERU_insumo))
                            lstCatInsumos = lstCatInsumos.Where(w => objFiltro.PERU_insumo.Contains(w.ACODIGO)).ToList();

                        if (!string.IsNullOrEmpty(objFiltro.descripcion))
                            lstCatInsumos = lstCatInsumos.Where(w => objFiltro.descripcion.Contains(w.ADESCRI)).ToList();
                        #endregion

                        InsumosDTO objInsumoDTO = new InsumosDTO();
                        foreach (var item in lstCatInsumos)
                        {
                            if (!string.IsNullOrEmpty(item.ACODIGO) && !string.IsNullOrEmpty(item.ADESCRI))
                            {
                                objInsumoDTO = new InsumosDTO();
                                objInsumoDTO.PERU_insumo = item.ACODIGO.Trim().ToUpper();
                                objInsumoDTO.descripcion = item.ADESCRI.Trim().ToUpper();
                                objInsumoDTO.unidad = !string.IsNullOrEmpty(item.AUNIDAD) ? item.AUNIDAD.Trim().ToUpper() : string.Empty;
                                lstInsumosDTO.Add(objInsumoDTO);
                            }
                        }
                    }
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    #region ARRENDADORA
                    string strQuery = string.Empty;
                    strQuery += @"SELECT TOP 1000 insumo, descripcion, unidad FROM DBA.insumos WHERE cancelado = 'A' ";

                    if (objFiltro.insumo > 0 && !string.IsNullOrEmpty(objFiltro.descripcion))
                        strQuery += "AND insumo LIKE '%{0}%' AND descripcion LIKE '%{1}%'";
                    if (objFiltro.insumo > 0 && string.IsNullOrEmpty(objFiltro.descripcion))
                        strQuery += "AND insumo LIKE '%{0}%'";
                    if (objFiltro.insumo <= 0 && !string.IsNullOrEmpty(objFiltro.descripcion))
                        strQuery += "AND descripcion LIKE '%{0}%'";

                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };

                    if (objFiltro.insumo > 0 && !string.IsNullOrEmpty(objFiltro.descripcion))
                        odbc.consulta = String.Format(strQuery, objFiltro.insumo, objFiltro.descripcion.Trim());
                    if (objFiltro.insumo > 0 && string.IsNullOrEmpty(objFiltro.descripcion))
                        odbc.consulta = String.Format(strQuery, objFiltro.insumo);
                    if (objFiltro.insumo <= 0 && !string.IsNullOrEmpty(objFiltro.descripcion))
                        odbc.consulta = String.Format(strQuery, objFiltro.descripcion.Trim());

                    lstInsumosDTO = _contextEnkontrol.Select<InsumosDTO>(EnkontrolEnum.ArrenProd, odbc);
                    #endregion
                }
                return lstInsumosDTO;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public Dictionary<string, object> GetDatosGraficasBLObra(BackLogsDTO objParamDTO)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                int anioActual = objParamDTO.anio;
                int anioAnterior = anioActual - 1;

                List<tblBL_CatBackLogs> lstBLAnioActual = _context.tblBL_CatBackLogs.Where(w => w.esActivo && w.areaCuenta == objParamDTO.areaCuenta && w.tipoBL == (int)TipoBackLogEnum.Obra && w.fechaInspeccion.Year == anioActual).ToList();
                List<tblBL_CatBackLogs> lstBLAniosAnteriores = _context.tblBL_CatBackLogs.Where(w => w.esActivo && w.areaCuenta == objParamDTO.areaCuenta && w.tipoBL == (int)TipoBackLogEnum.Obra && w.fechaInspeccion.Year == anioAnterior).ToList();

                #region GRAFICA: ESTATUS DE BACKLOGS GRAFICA PASTEL
                var resultadosCantEstatus = GetEstatusBackLogs(lstBLAnioActual);
                resultados.Add("resultadosCantEstatus", resultadosCantEstatus);
                #endregion

                #region GRAFICA: ESTATUS DE BACKLOGS GRAFICA LINEAS
                var resultadosCantEstatusLineas = GetEstatusBackLogsLineas(objParamDTO.areaCuenta, objParamDTO.anio);
                resultados.Add("resultadosCantEstatusLineas", resultadosCantEstatusLineas);
                #endregion

                #region GRAFICA: ACUMULADO AÑOS ANTERIORES
                var resultadosAñosAnteriores = GetEstatusBackLogsAñosAnteriores(lstBLAniosAnteriores, anioAnterior, objParamDTO.areaCuenta);
                resultados.Add("resultadosAñosAnteriores", resultadosAñosAnteriores);
                #endregion

                #region TIEMPO PROMEDIO TABLA INFERIOR DASHBOARD
                var resultadosTiempoPromedio = GetTiempoPromedioBLObra(lstBLAnioActual);
                resultados.Add("resultadosTiempoPromedio", resultadosTiempoPromedio);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
            return resultados;
        }

        public Dictionary<string, object> GetEstatusBackLogsAñosAnteriores(List<tblBL_CatBackLogs> lstBLAniosAnteriores, int anioAnterior, string areaCuenta)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                int cantAcumulados = 0;
                DateTime fechaInicio = new DateTime(anioAnterior, 01, 01);
                DateTime fechaFinal = new DateTime(anioAnterior, 12, 31);

                int cantBLRegistrados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT COUNT(id) AS cantBLAcumuladosAnioPasados FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                    parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).FirstOrDefault();

                int cantBLInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT COUNT(id) AS cantBLAcumuladosAnioPasados FROM tblBL_CatBackLogs WHERE idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                    parametros = new { idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).FirstOrDefault();

                cantAcumulados = cantBLRegistrados - cantBLInstalados;

                List<int> lstContadorAcumulado = new List<int>();
                lstContadorAcumulado.Add(cantAcumulados);

                resultados.Add("lstContadorAcumulado", lstContadorAcumulado.ToList());
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
            return resultados;
        }

        public Dictionary<string, object> GetTiempoPromedioBLObra(List<tblBL_CatBackLogs> lstBL)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (lstBL.Count() <= 0)
                {
                    result.Add("lstTiempoPromedio", null);
                    return result;
                }

                string areaCuenta = lstBL[0].areaCuenta;
                List<int> lstIDBL = new List<int>();
                foreach (var item in lstBL)
                {
                    lstIDBL.Add(item.id);
                }
                List<tblBL_BitacoraEstatusBL> lstBitacora = _context.tblBL_BitacoraEstatusBL.Where(w => w.areaCuenta == areaCuenta && lstIDBL.Contains(w.idBL) && w.esActivo).ToList();

                List<decimal> lstTiempoPromedio = new List<decimal>();
                int tiempoPromedioEstatus20 = 0,
                    tiempoPromedioEstatus40 = 0,
                    tiempoPromedioEstatus50 = 0,
                    tiempoPromedioEstatus60 = 0,
                    tiempoPromedioEstatus80 = 0,
                    tiempoPromedioEstatus90 = 0,
                    tiempoPromedioEstatus100 = 0;
                decimal tiempoTotalPromedio;

                tiempoPromedioEstatus20 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.ElaboracionInspeccion && w.esActivo).Count();
                tiempoPromedioEstatus40 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.ElaboracionRequisicion && w.esActivo).Count();
                tiempoPromedioEstatus50 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.ElaboracionOC && w.esActivo).Count();
                tiempoPromedioEstatus60 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.SuministroRefacciones && w.esActivo).Count();
                tiempoPromedioEstatus80 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.RehabilitacionProgramada && w.esActivo).Count();
                tiempoPromedioEstatus90 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.ProcesoInstalacion && w.esActivo).Count();
                tiempoPromedioEstatus100 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado && w.esActivo).Count();

                #region SE OBTIENE LISTADO DE DÍAS DE LO QUE TARDA UN BL EN CIERTO ESTATUS
                List<int> lstDiasEstatus20 = new List<int>();
                List<int> lstDiasEstatus40 = new List<int>();
                List<int> lstDiasEstatus50 = new List<int>();
                List<int> lstDiasEstatus60 = new List<int>();
                List<int> lstDiasEstatus80 = new List<int>();
                List<int> lstDiasEstatus90 = new List<int>();
                List<int> lstDiasEstatus100 = new List<int>();

                foreach (var item in lstBitacora)
                {
                    switch (item.idEstatus)
                    {
                        case (int)EstatusBackLogEnum.ElaboracionInspeccion:
                            lstDiasEstatus20.Add(item.diasTranscurridos);
                            break;
                        case (int)EstatusBackLogEnum.ElaboracionRequisicion:
                            lstDiasEstatus40.Add(item.diasTranscurridos);
                            break;
                        case (int)EstatusBackLogEnum.ElaboracionOC:
                            lstDiasEstatus50.Add(item.diasTranscurridos);
                            break;
                        case (int)EstatusBackLogEnum.SuministroRefacciones:
                            lstDiasEstatus60.Add(item.diasTranscurridos);
                            break;
                        case (int)EstatusBackLogEnum.RehabilitacionProgramada:
                            lstDiasEstatus80.Add(item.diasTranscurridos);
                            break;
                        case (int)EstatusBackLogEnum.ProcesoInstalacion:
                            lstDiasEstatus90.Add(item.diasTranscurridos);
                            break;
                        case (int)EstatusBackLogEnum.BackLogsInstalado:
                            lstDiasEstatus100.Add(item.diasTranscurridos);
                            break;
                    }
                }

                decimal estatus20a40 = 0;
                if ((decimal)lstDiasEstatus40.Sum() > 0 && lstDiasEstatus40.Count() > 0)
                    estatus20a40 = ((decimal)lstDiasEstatus40.Sum()) / lstDiasEstatus40.Count();

                decimal estatus40a50 = 0;
                if ((decimal)lstDiasEstatus50.Sum() > 0 && lstDiasEstatus50.Count() > 0)
                    estatus40a50 = ((decimal)lstDiasEstatus50.Sum()) / lstDiasEstatus50.Count();

                decimal estatus50a60 = 0;
                if ((decimal)lstDiasEstatus60.Sum() > 0 && lstDiasEstatus60.Count() > 0)
                    estatus50a60 = ((decimal)lstDiasEstatus60.Sum()) / lstDiasEstatus60.Count();

                decimal estatus60a80 = 0;
                if ((decimal)lstDiasEstatus80.Sum() > 0 && lstDiasEstatus80.Count() > 0)
                    estatus60a80 = ((decimal)lstDiasEstatus80.Sum()) / lstDiasEstatus80.Count();

                decimal estatus80a90 = 0;
                if ((decimal)lstDiasEstatus90.Sum() > 0 && lstDiasEstatus90.Count() > 0)
                    estatus80a90 = ((decimal)lstDiasEstatus90.Sum()) / lstDiasEstatus90.Count();

                decimal estatus90a100 = 0;
                if ((decimal)lstDiasEstatus100.Sum() > 0 && lstDiasEstatus100.Count() > 0)
                    estatus90a100 = ((decimal)lstDiasEstatus100.Sum()) / lstDiasEstatus100.Count();
                #endregion

                if (tiempoPromedioEstatus20 > 1)
                    tiempoPromedioEstatus20 /= 2;
                if (tiempoPromedioEstatus40 > 1)
                    tiempoPromedioEstatus40 /= 2;
                if (tiempoPromedioEstatus50 > 1)
                    tiempoPromedioEstatus50 /= 2;
                if (tiempoPromedioEstatus60 > 1)
                    tiempoPromedioEstatus60 /= 2;
                if (tiempoPromedioEstatus80 > 1)
                    tiempoPromedioEstatus80 /= 2;
                if (tiempoPromedioEstatus90 > 1)
                    tiempoPromedioEstatus90 /= 2;
                if (tiempoPromedioEstatus100 > 1)
                    tiempoPromedioEstatus100 /= 2;

                tiempoTotalPromedio = ((decimal)estatus20a40 + (decimal)estatus40a50 + (decimal)estatus50a60 + (decimal)estatus60a80 + (decimal)estatus80a90 + (decimal)estatus90a100);

                lstTiempoPromedio.Add(estatus20a40); // 20
                lstTiempoPromedio.Add(estatus40a50); // 40
                lstTiempoPromedio.Add(estatus50a60); // 50
                lstTiempoPromedio.Add(estatus60a80); // 60
                lstTiempoPromedio.Add(estatus80a90); // 80
                lstTiempoPromedio.Add(estatus90a100); // 90
                lstTiempoPromedio.Add(0); // 100
                lstTiempoPromedio.Add(tiempoTotalPromedio);

                result.Add("lstTiempoPromedio", lstTiempoPromedio);
                return result;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public Dictionary<string, object> GetEstatusBackLogsLineas(string areaCuenta, int anioActual)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                #region VARIABLES CONTADORES BL REGISTRADOS
                int cantBLEnero = 0;
                int cantBLFebrero = 0;
                int cantBLMarzo = 0;
                int cantBLAbril = 0;
                int cantBLMayo = 0;
                int cantBLJunio = 0;
                int cantBLJulio = 0;
                int cantBLAgosto = 0;
                int cantBLSeptiembre = 0;
                int cantBLOctubre = 0;
                int cantBLNoviembre = 0;
                int cantBLDiciembre = 0;
                #endregion

                #region VARIABLES CONTADORES BL INSTALADOS
                int cantBLEneroInstalados = 0;
                int cantBLFebreroInstalados = 0;
                int cantBLMarzoInstalados = 0;
                int cantBLAbrilInstalados = 0;
                int cantBLMayoInstalados = 0;
                int cantBLJunioInstalados = 0;
                int cantBLJulioInstalados = 0;
                int cantBLAgostoInstalados = 0;
                int cantBLSeptiembreInstalados = 0;
                int cantBLOctubreInstalados = 0;
                int cantBLNoviembreInstalados = 0;
                int cantBLDiciembreInstalados = 0;
                #endregion

                #region VARIABLES CONTADORES BL ACUMULADOS
                int cantBLEneroAcumulados = 0;
                int cantBLFebreroAcumulados = 0;
                int cantBLMarzoAcumulados = 0;
                int cantBLAbrilAcumulados = 0;
                int cantBLMayoAcumulados = 0;
                int cantBLJunioAcumulados = 0;
                int cantBLJulioAcumulados = 0;
                int cantBLAgostoAcumulados = 0;
                int cantBLSeptiembreAcumulados = 0;
                int cantBLOctubreAcumulados = 0;
                int cantBLNoviembreAcumulados = 0;
                int cantBLDiciembreAcumulados = 0;
                #endregion

                string fechaInicio = string.Empty;
                string fechaFin = string.Empty;
                DateTime date = new DateTime();
                int MM = 0, yyyy = 0, dd = 0;
                string strQuery = string.Empty;

                #region ENERO
                date = new DateTime(anioActual, 01, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                string fechaInicioAcumulados = string.Format("{0}-{1}-{2}", 2021, 01, 01);
                string fechaFinAcumulados = string.Format("{0}-{1}-{2}", anioActual, 01, 31);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLEnero = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLEneroInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicioAcumulados, fechaFinAcumulados);
                cantBLEneroAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region FEBRERO
                date = new DateTime(anioActual, 02, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLFebrero = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLFebreroInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLFebreroAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region MARZO
                date = new DateTime(anioActual, 03, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLMarzo = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLMarzoInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLMarzoAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region ABRIL
                date = new DateTime(anioActual, 04, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLAbril = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLAbrilInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLAbrilAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region MAYO
                date = new DateTime(anioActual, 05, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLMayo = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLMayoInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLMayoAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region JUNIO
                date = new DateTime(anioActual, 06, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLJunio = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLJunioInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLJunioAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region JULIO
                date = new DateTime(anioActual, 07, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLJulio = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLJulioInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLJulioAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region AGOSTO
                date = new DateTime(anioActual, 08, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLAgosto = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLAgostoInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLAgostoAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region SEPTIEMBRE
                date = new DateTime(anioActual, 09, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLSeptiembre = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLSeptiembreInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLSeptiembreAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region OCTUBRE
                date = new DateTime(anioActual, 10, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLOctubre = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLOctubreInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLOctubreAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region NOVIEMBRE
                date = new DateTime(anioActual, 11, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLNoviembre = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLNoviembreInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLNoviembreAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                #region DICIEMBRE
                date = new DateTime(anioActual, 12, 01);
                MM = Convert.ToInt32(date.Month);
                yyyy = Convert.ToInt32(date.Year);
                dd = DateTime.DaysInMonth(yyyy, MM);
                fechaInicio = string.Format("{0}-{1}-{2}", yyyy, MM, 01);
                fechaFin = string.Format("{0}-{1}-{2}", yyyy, MM, dd);

                // REGISTRADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{2}' AND '{3}')", areaCuenta, 1, fechaInicio, fechaFin);
                cantBLDiciembre = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // INSTALADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroInstalados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND idEstatus = {2} AND (CONVERT(DATE, fechaInstaladoBL) BETWEEN '{3}' AND '{4}')", areaCuenta, 1, (int)EstatusBackLogEnum.BackLogsInstalado, fechaInicio, fechaFin);
                cantBLDiciembreInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();

                // ACUMULADOS
                strQuery = string.Format(@"SELECT COUNT(id) AS cantBLEneroAcumulados FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND idEstatus != {1} AND esActivo = {2} AND (CONVERT(DATE, fechaInspeccion) BETWEEN '{3}' AND '{4}')", areaCuenta, (int)EstatusBackLogEnum.BackLogsInstalado, 1, fechaInicio, fechaFin);
                cantBLDiciembreAcumulados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).FirstOrDefault();
                #endregion

                List<int> lstcontador = new List<int>();
                lstcontador.Add(cantBLEnero);
                lstcontador.Add(cantBLFebrero);
                lstcontador.Add(cantBLMarzo);
                lstcontador.Add(cantBLAbril);
                lstcontador.Add(cantBLMayo);
                lstcontador.Add(cantBLJunio);
                lstcontador.Add(cantBLJulio);
                lstcontador.Add(cantBLAgosto);
                lstcontador.Add(cantBLSeptiembre);
                lstcontador.Add(cantBLOctubre);
                lstcontador.Add(cantBLNoviembre);
                lstcontador.Add(cantBLDiciembre);

                List<int> IstContadorInstalados = new List<int>();
                IstContadorInstalados.Add(cantBLEneroInstalados);
                IstContadorInstalados.Add(cantBLFebreroInstalados);
                IstContadorInstalados.Add(cantBLMarzoInstalados);
                IstContadorInstalados.Add(cantBLAbrilInstalados);
                IstContadorInstalados.Add(cantBLMayoInstalados);
                IstContadorInstalados.Add(cantBLJunioInstalados);
                IstContadorInstalados.Add(cantBLJulioInstalados);
                IstContadorInstalados.Add(cantBLAgostoInstalados);
                IstContadorInstalados.Add(cantBLSeptiembreInstalados);
                IstContadorInstalados.Add(cantBLOctubreInstalados);
                IstContadorInstalados.Add(cantBLNoviembreInstalados);
                IstContadorInstalados.Add(cantBLDiciembreInstalados);

                List<int> IstContadorAcumulados = new List<int>();
                IstContadorAcumulados.Add(cantBLEneroAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados + cantBLOctubreAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados + cantBLOctubreAcumulados + cantBLNoviembreAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados + cantBLOctubreAcumulados + cantBLNoviembreAcumulados + cantBLDiciembreAcumulados);

                resultados.Add("lstcontador", lstcontador.ToList());
                resultados.Add("IstContadorInstalados", IstContadorInstalados.ToList());
                resultados.Add("IstContadorAcumulados", IstContadorAcumulados.ToList());
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
            return resultados;
        }

        public Dictionary<string, object> GetEstatusBackLogs(List<tblBL_CatBackLogs> lstBL)
        {
            var resultados = new Dictionary<string, object>();
            try
            {

                int totalBL = 0;
                List<int> lstCantEstatus = new List<int>();
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.ElaboracionInspeccion).Count());
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.ElaboracionRequisicion).Count());
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.ElaboracionOC).Count());
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.SuministroRefacciones).Count());
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.RehabilitacionProgramada).Count());
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.ProcesoInstalacion).Count());
                lstCantEstatus.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado).Count());
                for (int i = 0; i < lstCantEstatus.Count(); i++)
                {
                    totalBL += lstCantEstatus[i];
                }
                lstCantEstatus.Add(totalBL);
                resultados.Add("lstCantEstatus", lstCantEstatus.ToList());
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
            return resultados;
        }

        private decimal GetTotalOC(string AreaCuenta, int idBL, string noEconomico, tblAlm_RelAreaCuentaXAlmacen _objAreaAlmacen, List<tblAlm_RelAreaCuentaXAlmacenDet> _lstAlmRelAreaCuentaXAlmacenDet, List<tblBL_OrdenesCompra> _lstOrdenesCompra,
                                   List<tblBL_Partes> _lstPartes, List<BackLogsDTO> _objCC = null, List<BackLogsDTO> _lstDetalleOC = null, List<BackLogsDTO> _objMoneda = null, List<BackLogsDTO> _lstInsumosOC = null)
        {
            try
            {
                int Almacen = 0;
                if (_objAreaAlmacen != null)
                    Almacen = _lstAlmRelAreaCuentaXAlmacenDet.Where(r => r.idRelacion == _objAreaAlmacen.id).OrderBy(n => n.id).Select(y => y.Almacen).FirstOrDefault();

                string cc = string.Empty;
                string strQuery = string.Empty;
                decimal totalFinal = 0;
                decimal totalInsumoPromedio = 0;
                List<BackLogsDTO> lstOC = new List<BackLogsDTO>();
                if (!string.IsNullOrEmpty(noEconomico) && idBL > 0)
                {
                    #region SE OBTIENE LAS ORDENES DE COMPRA
                    List<tblBL_OrdenesCompra> objOrdenesCompra = new List<tblBL_OrdenesCompra>();
                    objOrdenesCompra = _lstOrdenesCompra.Where(x => x.idBackLog == idBL && x.esActivo).ToList();
                    #endregion

                    #region SE OBTIENE CC EN BASE AL NO. ECONOMICO
                    List<BackLogsDTO> objCC = new List<BackLogsDTO>();
                    strQuery = @"SELECT cc FROM cc WHERE descripcion LIKE '%{0}%'";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, noEconomico);
                    if (productivo)
                        objCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        objCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                    cc = objCC.Select(x => x.cc).First();
                    #endregion

                    #region SE OBTIENE EL TOTAL DE LA ORDEN DE COMPRA
                    if (objOrdenesCompra.Count() != 0)
                    {
                        #region 
                        #region SE OBTIENE EL DETALLE DE LA ORDEN DE COMPRA
                        List<BackLogsDTO> lstDetalleOC = new List<BackLogsDTO>();
                        strQuery = "SELECT cc, numero, insumo, cantidad, precio, importe, num_requisicion FROM so_orden_compra_det WHERE cc LIKE '%{0}%' AND numero IN ({1})";
                        odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, cc, string.Join(",", objOrdenesCompra.Select(s => s.numOC).ToList()));
                        if (productivo)
                            lstDetalleOC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                        else
                            lstDetalleOC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                        #endregion

                        #region SE OBTIENE LOS INSUMOS QUE CONTIENE EL BL
                        List<tblBL_Partes> lstPartes = _lstPartes.Where(w => w.idBacklog == idBL && w.esActivo).ToList();
                        List<BackLogsDTO> lstPartesRelBL = lstDetalleOC.Where(w => lstPartes.Select(s => s.insumo).Contains(w.insumo)).ToList();
                        #endregion

                        foreach (var item in lstPartesRelBL)
                        {
                            BackLogsDTO objBL = new BackLogsDTO();
                            objCC = new List<BackLogsDTO>();
                            strQuery = "SELECT moneda, tipo_cambio, total FROM DBA.so_orden_compra WHERE cc LIKE '%{0}%' AND numero IN ({1})";
                            odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery, cc, item.numero);
                            objCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);

                            int tipoMoneda = objCC.Select(s => s.moneda).FirstOrDefault();
                            if (tipoMoneda == (int)TipoMonedaEnum.MXN)
                                totalFinal += item.importe;
                            else if (tipoMoneda == (int)TipoMonedaEnum.USD)
                                totalFinal += (item.importe * objCC.Select(s => s.tipo_cambio).FirstOrDefault());
                        }
                        #endregion

                        #region SE OBTIENE EL LISTADO DE INSUMOS QUE CONTIENE LA ORDEN DE COMPRA
                        List<BackLogsDTO> lstInsumosOC = new List<BackLogsDTO>();
                        strQuery = "SELECT insumo FROM DBA.so_orden_compra_det WHERE cc LIKE '%{0}%' AND numero IN ({1})";
                        odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, cc, string.Join(",", objOrdenesCompra.Select(s => s.numOC).ToList()));
                        lstInsumosOC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                        #endregion

                        #region SE OBTIENE EL COSTO PROMEDIO DE LOS INSUMOS QUE SALIERON DE ALMACEN.
                        List<int> lstInsumosPartes = lstPartes.Where(w => !lstInsumosOC.Select(s => s.insumo).Contains(w.insumo)).Select(s => s.insumo).ToList();
                        totalInsumoPromedio = 0;
                        foreach (var item in lstInsumosPartes)
                        {
                            totalInsumoPromedio += GetCostoPromedio(Almacen, item);
                        }
                        #endregion
                    }
                    else
                    {
                        List<int> lstInsumosPartes = _lstPartes.Where(w => w.idBacklog == idBL).Select(s => s.insumo).ToList();
                        totalInsumoPromedio = 0;
                        foreach (var item in lstInsumosPartes)
                        {
                            totalInsumoPromedio += GetCostoPromedio(Almacen, item);
                        }
                    }
                    #endregion
                }
                return totalFinal + totalInsumoPromedio;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return 0;
            }
        }

        private int DiasTranscurridos(int idBL, int idEstatus, DateTime fechaCreacionBL, DateTime fechaModificacionBL)
        {
            int dias = 0;
            try
            {
                if (idEstatus != 7)
                {
                    TimeSpan difFechas = DateTime.Now.Date - fechaCreacionBL.Date;
                    dias = difFechas.Days == 0 ? 1 : difFechas.Days;
                }
                else
                {
                    TimeSpan difFechas = fechaModificacionBL.Date - fechaCreacionBL.Date;
                    dias = difFechas.Days == 0 ? 1 : difFechas.Days;
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "DiasTranscurridos", e, AccionEnum.CONSULTA, idBL, new { idBL = idBL, idEstatus = idEstatus, fechaCreacionBL = fechaCreacionBL, fechaModificacionBL = fechaModificacionBL });
            }
            return dias;
        }

        public List<tblBL_CatBackLogs> GetNumBackLogs(string areaCuenta)
        {
            try
            {
                return _context.tblBL_CatBackLogs.Where(x => x.areaCuenta == areaCuenta && x.fechaInspeccion.Year == DateTime.Now.Year && x.esActivo && x.tipoBL == (int)TipoBackLogEnum.Obra).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetNumBackLogs", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public int GetNumBacklogsAniosAnteriores(string areaCuenta)
        {
            try
            {
                int BLRegistrados = 0, BLCerrados = 0, BLAcumuladosAniosAnteriores = 0;
                var BackLogs = _context.tblBL_CatBackLogs.Where(x => x.areaCuenta == areaCuenta && x.fechaInspeccion.Year < DateTime.Now.Year && x.idEstatus < 8 && x.esActivo && x.tipoBL == (int)TipoBackLogEnum.Obra).Select(x => x.idEstatus).ToList();

                foreach (var item in BackLogs)
                {
                    if (item == 7)
                        BLCerrados++;
                }
                BLRegistrados = Convert.ToInt32(BackLogs.Count);
                BLAcumuladosAniosAnteriores = Convert.ToInt32(BLRegistrados) - BLCerrados;

                return BLAcumuladosAniosAnteriores;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetNumBacklogsAniosAnteriores", e, AccionEnum.CONSULTA, 0, 0);
                return 0;
            }
        }

        public List<tblBL_Partes> GetPartes(int idBackLog)
        {
            try
            {
                List<tblBL_Partes> tblPartes = _context.tblBL_Partes.Where(x => x.idBacklog == idBackLog && x.esActivo).ToList();
                return tblPartes;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetPartes", e, AccionEnum.CONSULTA, idBackLog, 0);
                return null;
            }
        }

        public List<tblBL_CatSubconjuntos> GetSubconjuntos(List<int> idSubconjunto)
        {
            try
            {
                return _context.tblBL_CatSubconjuntos.Where(s => idSubconjunto.Contains(s.id) && s.esActivo).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetSubconjuntos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<tblBL_CatConjuntos> GetConjuntos(List<int> idConjunto)
        {
            try
            {
                return _context.tblBL_CatConjuntos.Where(s => idConjunto.Contains(s.id) && s.esActivo).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetConjuntos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool CrearBackLog(tblBL_CatBackLogs objBL, List<tblBL_Partes> lstPartes, List<tblBL_ManoObra> lstManoObra, bool esParte, bool esManoObra, bool esObra, int idUsuarioResponsable)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE REGISTRA EL BACKLOG
                    decimal presupuestoEstimado = 0;
                    if (lstPartes != null)
                    {
                        foreach (var item in lstPartes)
                        {
                            presupuestoEstimado += item.costoPromedio;
                        }
                    }
                    objBL.presupuestoEstimado = presupuestoEstimado;
                    objBL.fechaModificacionBL = DateTime.Now;
                    objBL.idEstatus = (int)EstatusBackLogEnum.ElaboracionInspeccion;
                    objBL.esActivo = true;

                    if (esObra)
                        objBL.tipoBL = (int)TipoBackLogEnum.Obra;
                    else
                        objBL.tipoBL = (int)TipoBackLogEnum.TMC;
                    objBL.idUsuarioResponsable = idUsuarioResponsable;
                    objBL.fechaCreacionBL = DateTime.Now;
                    objBL.fechaLiberadoBL = new DateTime(2000, 01, 01);
                    objBL.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    _context.tblBL_CatBackLogs.Add(objBL);
                    _context.SaveChanges();
                    #endregion

                    #region SE OBTIENE ID DEL BACKLOG QUE SE ACABA DE REGISTRAR
                    var getBackLogID = _context.tblBL_CatBackLogs.Where(x => x.folioBL == objBL.folioBL).Select(x => new
                    {
                        idBackLog = x.id
                    }).ToList();
                    int idBackLog = getBackLogID[0].idBackLog;
                    #endregion

                    #region SE REGISTRA LOS INSUMOS CON RELACION AL BACKLOG
                    if (lstPartes != null && esParte)
                        CrearPartes(lstPartes, idBackLog);
                    #endregion

                    #region SE REGISTRA LA MANO DE OBRA CON RELACION AL BACKLOG
                    if (lstManoObra != null && esManoObra)
                        CrearManoObra(lstManoObra, idBackLog);
                    #endregion

                    #region SE CREA BITACORA DE ESTATUS DEL BL
                    tblBL_BitacoraEstatusBL objGuardarBitacoraEstatus = new tblBL_BitacoraEstatusBL();
                    objGuardarBitacoraEstatus.idBL = idBackLog;
                    objGuardarBitacoraEstatus.areaCuenta = objBL.areaCuenta;
                    objGuardarBitacoraEstatus.diasTranscurridos = 0;
                    objGuardarBitacoraEstatus.idEstatus = esObra ? (int)EstatusBackLogEnum.ElaboracionInspeccion : (int)EstatusBackLogsTMCEnum.ElaboracionPresupuesto;
                    objGuardarBitacoraEstatus.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objGuardarBitacoraEstatus.idUsuarioModificacion = 0;
                    objGuardarBitacoraEstatus.fechaCreacion = DateTime.Now;
                    objGuardarBitacoraEstatus.fechaModificacion = new DateTime(2000, 01, 01);
                    objGuardarBitacoraEstatus.esActivo = true;
                    _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatus);
                    _context.SaveChanges();
                    #endregion

                    dbContextTransaction.Commit();

                    #region SE CREA BITACORA
                    SaveBitacora(12, (int)AccionEnum.AGREGAR, idBackLog, JsonUtils.convertNetObjectToJson(objBL));
                    #endregion

                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearBackLog", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objBL));
                    return false;
                }
        }

        private void CrearPartes(List<tblBL_Partes> lstPartes, int idBackLog)
        {
            try
            {
                #region VALIDACIONES
                if (lstPartes.Count() <= 0) { throw new Exception("Ocurrió un error al registrar los insumos al BL."); }
                if (idBackLog <= 0) { throw new Exception("Ocurrió un error al registrar los insumos al BL."); }
                #endregion

                #region SE REGISTRA LOS INSUMOS AL BL
                List<tblBL_Partes> lstPartesDTO = new List<tblBL_Partes>();
                tblBL_Partes objParteDTO = new tblBL_Partes();
                foreach (var item in lstPartes)
                {
                    objParteDTO = new tblBL_Partes();
                    objParteDTO.idBacklog = item.idBacklog;
                    objParteDTO.insumo = item.insumo;
                    objParteDTO.cantidad = item.cantidad;
                    objParteDTO.parte = item.parte;
                    objParteDTO.articulo = item.articulo;
                    objParteDTO.unidad = item.unidad;
                    objParteDTO.tipoMoneda = item.tipoMoneda;
                    objParteDTO.costoPromedio = item.costoPromedio;
                    objParteDTO.PERU_insumo = item.PERU_insumo;
                    objParteDTO.FK_UsuarioCreacion = item.FK_UsuarioCreacion;
                    objParteDTO.fechaCreacion = DateTime.Now;
                    objParteDTO.esActivo = true;
                    lstPartesDTO.Add(objParteDTO);
                }
                _context.tblBL_Partes.AddRange(lstPartesDTO);
                _context.SaveChanges();
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearPartes", e, AccionEnum.AGREGAR, idBackLog, JsonUtils.convertNetObjectToJson(lstPartes));
            }
        }

        private void CrearManoObra(List<tblBL_ManoObra> datosManoObra, int idBackLog)
        {
            try
            {
                datosManoObra.Where(s => s.idBackLog == 0).ToList().ForEach(s => s.idBackLog = idBackLog);
                datosManoObra.Where(s => s.esActivo == false).ToList().ForEach(s => s.esActivo = true);
                _context.tblBL_ManoObra.AddRange(datosManoObra);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearManoObra", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(datosManoObra));
            }
        }

        public bool ActualizarBackLog(tblBL_CatBackLogs objBL, bool esActualizarCC)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region SE ACTUALIZA EL BL
                    var ActualizarBackLog = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == objBL.id);
                    ActualizarBackLog.fechaInspeccion = objBL.fechaInspeccion;

                    if (!string.IsNullOrEmpty(objBL.noEconomico))
                        ActualizarBackLog.noEconomico = objBL.noEconomico;

                    ActualizarBackLog.horas = objBL.horas;
                    ActualizarBackLog.idSubconjunto = objBL.idSubconjunto;
                    ActualizarBackLog.parte = objBL.parte;
                    ActualizarBackLog.manoObra = objBL.manoObra;
                    ActualizarBackLog.descripcion = objBL.descripcion;
                    ActualizarBackLog.fechaModificacionBL = DateTime.Now;
                    ActualizarBackLog.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    ActualizarBackLog.idUsuarioResponsable = objBL.idUsuarioResponsable;
                    _context.SaveChanges();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, (int)objBL.id, JsonUtils.convertNetObjectToJson(ActualizarBackLog));
                    #endregion

                    if (esActualizarCC)
                    {
                        #region EN CASO QUE SE ACTUALICE EL CC, SE ELIMINAN SUS OC DEL BL
                        var EliminarOC = _context.tblBL_OrdenesCompra.Where(x => x.idBackLog == objBL.id && x.esActivo).ToList();
                        foreach (var item in EliminarOC)
                        {
                            item.esActivo = false;
                            item.fechaModificacionNumOC = DateTime.Now;
                        }
                        _context.SaveChanges();
                        SaveBitacora(0, (int)AccionEnum.ELIMINAR, (int)objBL.id, JsonUtils.convertNetObjectToJson(EliminarOC));
                        #endregion

                        #region EN CASO QUE SE ACTUALICE EL CC, SE ELIMINAN LAS REQUISICIONES DEL BL
                        var EliminarRequisicion = _context.tblBL_Requisiciones.Where(x => x.idBackLog == objBL.id && x.esActivo).ToList();
                        foreach (var item in EliminarRequisicion)
                        {
                            item.esActivo = false;
                            item.fechaModificacionRequisicion = DateTime.Now;
                        }
                        _context.SaveChanges();
                        SaveBitacora(0, (int)AccionEnum.ELIMINAR, (int)objBL.id, JsonUtils.convertNetObjectToJson(EliminarRequisicion));
                        #endregion

                        #region SE ACTUALIZA ESTATUS DEL BACKLOG
                        bool exitoActualizarEstatusBL = ActualizarEstatusBL(objBL.id);
                        if (!exitoActualizarEstatusBL)
                            throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                        #endregion
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarBackLog", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objBL));
                    return false;
                }
            }
        }

        public bool ActualizarParte(tblBL_Partes datosParte)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE ACTUALIZA EL INSUMO DEL BACKLOG (Parte)
                    tblBL_Partes ActualizarParte = _context.tblBL_Partes.FirstOrDefault(x => x.id == datosParte.id);
                    ActualizarParte.insumo = datosParte.insumo;
                    ActualizarParte.cantidad = datosParte.cantidad;
                    ActualizarParte.parte = datosParte.parte;
                    ActualizarParte.articulo = datosParte.articulo;
                    ActualizarParte.unidad = datosParte.unidad;
                    ActualizarParte.costoPromedio = datosParte.costoPromedio;
                    _context.SaveChanges();
                    #endregion

                    #region SE OBTIENE SUMA TOTAL DE LOS INSUMOS RELACIONADOS AL BACKLOG
                    decimal pptoEstimado = _context.tblBL_Partes.Where(w => w.idBacklog == datosParte.idBacklog && w.esActivo).Sum(s => s.costoPromedio);
                    #endregion

                    #region SE ACTUALIZA EL PRESUPUESTO ESTIMADO DEL BACKLOG
                    tblBL_CatBackLogs objActualizarPptoBL = _context.tblBL_CatBackLogs.Where(w => w.id == datosParte.idBacklog && w.esActivo).FirstOrDefault();
                    if (objActualizarPptoBL == null)
                        throw new Exception("Ocurrió un error al actualizar el presupuesto estimado del BL.");

                    objActualizarPptoBL.presupuestoEstimado = (decimal)pptoEstimado;
                    _context.SaveChanges();
                    #endregion

                    dbContextTransaction.Commit();

                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, (int)ActualizarParte.id, JsonUtils.convertNetObjectToJson(ActualizarParte));
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarParte", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(datosParte));
                    return false;
                }
        }

        public bool CrearParte(tblBL_Partes datosParte)
        {
            try
            {
                #region VALIDACIONES
                if (datosParte.idBacklog <= 0) { throw new Exception("Ocurrió un error al registrar los insumos al BL."); }
                if (string.IsNullOrEmpty(datosParte.PERU_insumo)) { throw new Exception("Ocurrió un error al registrar los insumos al BL."); }
                #endregion

                #region SE REGISTRA INSUMO AL BL (Parte)
                datosParte.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                datosParte.fechaCreacion = DateTime.Now;
                datosParte.esActivo = true;
                _context.tblBL_Partes.Add(datosParte);
                _context.SaveChanges();
                #endregion

                #region SE OBTIENE SUMA TOTAL DE LOS INSUMOS RELACIONADOS AL BACKLOG
                decimal pptoEstimado = _context.tblBL_Partes.Where(w => w.idBacklog == datosParte.idBacklog && w.esActivo).Sum(s => s.costoPromedio);
                #endregion

                #region SE ACTUALIZA PPTO DEL BACKLOG
                tblBL_CatBackLogs objActualizarPptoBL = _context.tblBL_CatBackLogs.Where(w => w.id == datosParte.idBacklog && w.esActivo).FirstOrDefault();
                if (objActualizarPptoBL == null)
                    throw new Exception("Ocurrió un error al registrar el ppto al BL.");

                objActualizarPptoBL.presupuestoEstimado = (decimal)pptoEstimado;
                _context.SaveChanges();
                #endregion

                SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(datosParte));
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearParte", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(datosParte));
                return false;
            }
        }

        public bool ActualizarManoObra(tblBL_ManoObra datosManoObra)
        {
            try
            {
                var ActualizarManoObra = _context.tblBL_ManoObra.FirstOrDefault(x => x.id == datosManoObra.id);
                ActualizarManoObra.descripcion = datosManoObra.descripcion;
                ActualizarManoObra.esActivo = true;
                _context.SaveChanges();
                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, ActualizarManoObra.id, JsonUtils.convertNetObjectToJson(datosManoObra));
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarManoObra", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(datosManoObra));
                return false;
            }
        }

        public bool CrearManoObra(tblBL_ManoObra datosManoObra)
        {
            try
            {
                datosManoObra.esActivo = true;
                _context.tblBL_ManoObra.Add(datosManoObra);
                _context.SaveChanges();
                SaveBitacora(0, (int)AccionEnum.AGREGAR, datosManoObra.id, JsonUtils.convertNetObjectToJson(datosManoObra));
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearManoObra", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(datosManoObra));
                return false;
            }
        }

        public List<ComboDTO> FillAreasCuentas()
        {
            string strQuery = @"SELECT CONVERT(VARCHAR(10), area) + '-' + CONVERT(VARCHAR(10), cuenta) AS Value, CONVERT(VARCHAR(10), area) + '-' + CONVERT(VARCHAR(10), cuenta) + '  ' + descripcion AS Text 
                                FROM si_area_cuenta
                                    WHERE cc_activo = 1
                                        GROUP BY Value, Text";
            var odbc = new OdbcConsultaDTO() { consulta = strQuery };

            var lstCCArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenProd, odbc);
            return lstCCArrendadora.ToList();
        }

        public List<ComboDTO> FillCboCC(string areaCuenta, bool esObra)
        {
            if (!string.IsNullOrEmpty(areaCuenta) && esObra)
            {
                #region v2
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region SE OBTIENE LISTADO DE MAQUINAS DEL AREA CUENTA SELECCIONADO
                    List<dynamic> lstCatMaquinas = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.grupoMaquinariaID, t1.noEconomico, t1.modeloEquipoID, t1.estatus, t1.descripcion, t1.centro_costos, t3.cc
		                                    FROM tblM_CatMaquina AS t1
		                                    INNER JOIN tblM_CatGrupoMaquinaria AS t2 ON t1.grupoMaquinariaID = t2.id
		                                    INNER JOIN tblP_CC AS t3 ON t3.areaCuenta = t1.centro_costos
			                                    WHERE t1.centro_costos = @centro_costos AND t1.estatus = @estatus AND t2.tipoEquipoID = @tipoEquipoID",
                        parametros = new { centro_costos = areaCuenta, estatus = 1, tipoEquipoID = 1 }
                    });
                    #endregion

                    #region SE CONSTRUYE COMBO DTO
                    List<ComboDTO> lstMaquinasCboDTO = new List<ComboDTO>();
                    lstMaquinasCboDTO = lstCatMaquinas.Where(w => w.cc != null).Select(s => new ComboDTO
                    {
                        Value = s.noEconomico.ToString(),
                        Text = s.noEconomico,
                        Prefijo = s.noEconomico
                    }).ToList();

                    return lstMaquinasCboDTO.OrderBy(o => o.Text).ToList();
                    #endregion
                }
                else
                {
                    #region SE OBTIENE LISTADO DE MAQUINAS DEL AREA CUENTA SELECCIONADO
                    List<dynamic> lstCatMaquinas = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.grupoMaquinariaID, t1.noEconomico, t1.modeloEquipoID, t1.estatus, t1.descripcion, t1.centro_costos, cc = ''
		                                FROM tblM_CatMaquina AS t1
		                                INNER JOIN tblM_CatGrupoMaquinaria AS t2 ON t1.grupoMaquinariaID = t2.id
			                                WHERE t1.centro_costos = @centro_costos AND t1.estatus = @estatus",
                        parametros = new { centro_costos = areaCuenta, estatus = 1 }
                    });
                    #endregion

                    #region SE OBTIENE EL CC DE LAS MAQUINAS CONSULTAS PREVIAMENTE
                    List<string> lstStrMaquinas = new List<string>();
                    foreach (var item in lstCatMaquinas)
                    {
                        lstStrMaquinas.Add("'" + item.noEconomico + "'");
                    }
                    string strQuery = @"SELECT cc, descripcion FROM cc WHERE descripcion IN ({0})";
                    if (vSesiones.sesionEmpresaActual == 3)
                        strQuery = @"SELECT cc, descripcion FROM DBA.cc";

                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, String.Join(",", lstStrMaquinas));
                    List<dynamic> lstCCRelNoEconomico = new List<dynamic>();
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        lstCCRelNoEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);
                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                        lstCCRelNoEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, odbc);

                    foreach (var item in lstCatMaquinas)
                    {
                        item.cc = lstCCRelNoEconomico.Where(w => w.descripcion == item.noEconomico).Select(s => s.cc).FirstOrDefault();
                    }
                    #endregion

                    #region SE CONSTRUYE COMBO DTO
                    List<ComboDTO> lstMaquinasCboDTO = new List<ComboDTO>();
                    lstMaquinasCboDTO = lstCatMaquinas.Where(w => w.cc != null).Select(s => new ComboDTO
                    {
                        Value = s.cc.ToString(),
                        Text = s.noEconomico,
                        Prefijo = s.noEconomico
                    }).ToList();
                    #endregion

                    return lstMaquinasCboDTO.ToList();
                }
                #endregion
            }
            else if (!esObra)
            {
                #region SE OBTIENE LOS ECONOMICOS DEL AREA CUENTA SELECCIONADA (TMC).
                List<tblM_CatMaquina> lstEconomicos = _context.tblM_CatMaquina.Where(x => x.centro_costos == areaCuenta && x.estatus == 1).ToList();
                List<string> lstEconomicosStr = new List<string>();
                foreach (var item in lstEconomicos)
                {
                    lstEconomicosStr.Add("'" + item.noEconomico + "'");
                }
                #endregion

                #region SE OBTIENE EL AREA CUENTA POR CADA ECONOMICO QUE SE OBTUVO DE TMC.
                string strQuery = @"SELECT cc, descripcion FROM cc WHERE descripcion IN ({0})";
                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery, string.Join(",", lstEconomicosStr));
                if (vSesiones.sesionEmpresaActual == 3)
                {
                    strQuery = @"SELECT cc, descripcion FROM DBA.cc)";
                    odbc.consulta = String.Format(strQuery, string.Join(",", lstEconomicosStr));
                }

                List<BackLogsDTO> lstNoEconomicos = new List<BackLogsDTO>();
                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Arrendadora:
                        lstNoEconomicos = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                        break;
                    case (int)EmpresaEnum.Colombia:
                        lstNoEconomicos = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ColombiaProductivo, odbc);
                        break;
                }
                #endregion

                #region SE CREA EL COMBO DE CC.
                var dataCC = lstNoEconomicos.Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = x.descripcion,
                    Prefijo = x.descripcion
                }).ToList();
                #endregion

                return dataCC.ToList();
            }
            return null;
        }

        public List<ComboDTO> FillCboConjunto()
        {
            var dataConjunto = _context.tblBL_CatConjuntos.Where(x => x.esActivo).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion.ToUpper()
            }).ToList();

            return dataConjunto;
        }

        public List<ComboDTO> FillCboSubconjunto(int idConjunto)
        {
            var dataSubconjunto = _context.tblBL_CatSubconjuntos.Where(x => x.idConjunto == idConjunto && x.esActivo).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion.ToUpper()
            }).ToList();

            return dataSubconjunto;
        }

        public List<ComboDTO> FillCboModelo()
        {
            var dataModelo = _context.tblM_CatModeloEquipo.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

            return dataModelo;
        }

        public List<ComboDTO> FillCboGrupo(BackLogsDTO objParamsDTO)
        {
            try
            {
                #region V1 || NO ELIMINAR, PREGUNTAR A OMAR PRIMERO
                //List<tblM_CatGrupoMaquinaria> lstCatGrupos = _context.tblM_CatGrupoMaquinaria.Where(w => w.tipoEquipoID == 1 && w.estatus).ToList();
                //if (lstCatGrupos.Count() <= 0)
                //    throw new Exception("No se encontró información en el catálogo de Grupo de maquinarias. Favor de notificarlo.");

                //List<ComboDTO> lstCatGruposDTO = new List<ComboDTO>();
                //ComboDTO objCatGrupoDTO = new ComboDTO();
                //foreach (var item in lstCatGrupos)
                //{
                //    if (item.id > 0 && !string.IsNullOrEmpty(item.descripcion))
                //    {
                //        objCatGrupoDTO = new ComboDTO();
                //        objCatGrupoDTO.Value = item.id.ToString();
                //        objCatGrupoDTO.Text = item.descripcion.Trim().ToUpper();
                //        lstCatGruposDTO.Add(objCatGrupoDTO);
                //    }
                //}

                //return lstCatGruposDTO;
                #endregion

                #region V2
                #region VALIDACIONES
                if (string.IsNullOrEmpty(objParamsDTO.areaCuenta)) { throw new Exception("Es necesario seleccionar un área cuenta."); }
                if (objParamsDTO.tipoEquipoID <= 0) { throw new Exception("Es necesario seleccionar el tipo de equipo."); }
                #endregion

                #region SE OBTIENE LISTADO DE GRUPOS QUE TENGA RELACIONADO EL AREA CUENTA
                List<int> lstGruposFK = _context.tblM_CatMaquina.Where(w => w.centro_costos == objParamsDTO.areaCuenta && w.estatus == 1).Select(s => s.grupoMaquinariaID).ToList();
                List<tblM_CatGrupoMaquinaria> lstCatGrupos = _context.tblM_CatGrupoMaquinaria.Where(w => lstGruposFK.Contains(w.id) && w.tipoEquipoID == objParamsDTO.tipoEquipoID && _LST_TIPO_EQUIPO.Contains(w.tipoEquipoID) && w.estatus).ToList();
                if (lstCatGrupos.Count() <= 0)
                    throw new Exception("No se encontró información en el catálogo de Grupo de maquinarias. Favor de notificarlo.");

                List<ComboDTO> lstCatGruposDTO = new List<ComboDTO>();
                ComboDTO objCatGrupoDTO = new ComboDTO();
                foreach (var item in lstCatGrupos)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objCatGrupoDTO = new ComboDTO();
                        objCatGrupoDTO.Value = item.id.ToString();
                        objCatGrupoDTO.Text = item.descripcion.Trim().ToUpper();
                        lstCatGruposDTO.Add(objCatGrupoDTO);
                    }
                }

                return lstCatGruposDTO;
                #endregion
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboGrupo", e, AccionEnum.FILLCOMBO, 0, 0);
                return null;
            }
        }

        public List<tblM_CatMaquina> GetMaquina(string areaCuenta, string noEconomico)
        {
            try
            {
                if (!string.IsNullOrEmpty(areaCuenta) && !string.IsNullOrEmpty(noEconomico))
                {
                    List<tblM_CatMaquina> dataMaquina = _context.tblM_CatMaquina.Where(x => x.centro_costos == areaCuenta && x.noEconomico == noEconomico).ToList();
                    return dataMaquina;
                }
                else
                    throw new Exception("Ocurrió un error al obtener el modelo y grupo del CC.");
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetMaquina", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public tblM_CapHorometro GetHorometroActual(string areaCuenta, string noEconomico)
        {
            try
            {
                List<tblM_CapHorometro> objHorometros = _context.tblM_CapHorometro.Where(x => x.CC == areaCuenta && x.Economico == noEconomico).OrderByDescending(x => x.id).ToList();
                if (objHorometros.Count() > 0)
                    return objHorometros.FirstOrDefault();
                else
                {
                    tblM_CapHorometro obj = new tblM_CapHorometro();
                    obj.Horometro = 0;
                    return obj;
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetHorometroActual", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public Dictionary<string, object> GetCantBLObligatorios(string areaCuenta, string noEconomico)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LA CANTIDAD DE BACKLOGS INSPECCIONADOS DEL ECONOMICO SELECCIONADO
                int cantInspeccionesBL = 0;
                List<tblBL_Inspecciones> lstCantInspecciones = _context.tblBL_Inspecciones.Where(w => w.areaCuenta == areaCuenta && w.noEconomico == noEconomico && w.esActivo).ToList();
                if (lstCantInspecciones.Count() <= 0)
                    throw new Exception("El económico no cuenta con inspecciones.");

                cantInspeccionesBL = _context.tblBL_Inspecciones.Where(w => w.areaCuenta == areaCuenta && w.noEconomico == noEconomico && w.esActivo).Sum(s => s.cantBackLogs);
                #endregion

                // SE OBTIENE LA CANTIDAD DE BACKLOGS REGISTRADOS
                int cantRegistroBL = _context.tblBL_CatBackLogs.Where(x => x.areaCuenta == areaCuenta && x.noEconomico == noEconomico && x.esActivo).Count();

                List<tblBL_Inspecciones> lstInspecciones = _context.tblBL_Inspecciones.Where(w => w.areaCuenta == areaCuenta && w.noEconomico == noEconomico && w.esActivo).ToList();
                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(w => w.areaCuenta == areaCuenta && w.noEconomico == noEconomico && w.esActivo).ToList();

                if (cantRegistroBL > cantInspeccionesBL)
                {
                    // SE VERIFICA QUE PERIODO (BL), SE ENCUENTRA REGISTRADO DE MAS
                    foreach (var item in lstInspecciones)
                    {
                        int cantInspeccion = lstInspecciones.Where(w => w.periodo == item.periodo).Sum(s => s.cantBackLogs);
                        int cantRegistro = lstBL.Where(w => w.periodoRegistro == item.periodo).Count();

                        //if (cantRegistro > cantInspeccion)
                        //{
                        //    string mensajeError = string.Format("Periodo {0}<br>Cantidad BL en inspección: {1}.<br>Cantidad BL registrados: {2}.<br><br>Nota: Label", item.periodo, cantInspeccion, cantRegistro);
                        //    throw new Exception(mensajeError);
                        //}
                    }
                }
                else
                {
                    // SE VERIFICA QUE PERIODOS QUEDA PENDIENTE POR REGISTRAR LOS BACKLOGS
                    cantRegistroBL = cantInspeccionesBL;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("cantRegistroBL", cantRegistroBL);
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetCantBLObligatorios", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public List<tblM_CatModeloEquipo> GetModeloEquipo(List<int> idModelo)
        {
            try
            {
                return _context.tblM_CatModeloEquipo.Where(s => idModelo.Contains(s.id)).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetModeloEquipo", e, AccionEnum.CONSULTA, idModelo[0], 0);
                return null;
            }
        }

        public List<tblBL_CatBackLogs> GetUltimoFolio(bool esObra)
        {
            try
            {
                if (esObra)
                {
                    List<tblBL_CatBackLogs> lstCatBackLogsObra = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.Obra).OrderByDescending(x => x.id).ToList();
                    return lstCatBackLogsObra;
                }
                else
                {
                    List<tblBL_CatBackLogs> lstCatBackLogsTMC = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.TMC).OrderByDescending(x => x.id).ToList();
                    return lstCatBackLogsTMC;
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetUltimoFolio", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<tblBL_CatBackLogs> VerificarDisponibilidadFolio(string folioBL)
        {
            try
            {
                return _context.tblBL_CatBackLogs.Where(x => x.folioBL == folioBL).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "VerificarDisponibilidadFolio", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool EliminarBackLog(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region SE ACTUALIZA EL NUEVO PPTO YA QUE SE ELIMINA EL BL SELECCIONADO
                    tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.Where(x => x.id == id && x.esActivo).FirstOrDefault();
                    int idSegPpto = objBL.idSegPpto;
                    decimal pptoBL = objBL.presupuestoEstimado;

                    tblBL_SeguimientoPptos objActualizarPpto = _context.tblBL_SeguimientoPptos.Where(x => x.id == idSegPpto).FirstOrDefault();
                    if (objActualizarPpto != null)
                    {
                        objActualizarPpto.Ppto -= (decimal)pptoBL;
                        _context.SaveChanges();
                    }
                    #endregion

                    #region SE ELIMINA EL BACKLOG
                    var EliminarBackLog = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == id);
                    EliminarBackLog.esActivo = false;
                    _context.SaveChanges();
                    #endregion

                    #region SE ELIMINAN LAS PARTES RELACIONADAS AL BACKLOG
                    List<tblBL_Partes> lstPartes = _context.tblBL_Partes.Where(x => x.idBacklog == id && x.esActivo).ToList();
                    foreach (var item in lstPartes)
                    {
                        item.esActivo = false;
                    }
                    _context.SaveChanges();
                    #endregion

                    #region SE ELIMINA LAS MANOS DE OBRA RELACIONADAS AL BACKLOG
                    List<tblBL_ManoObra> lstManoObra = _context.tblBL_ManoObra.Where(x => x.idBackLog == id && x.esActivo).ToList();
                    foreach (var item in lstManoObra)
                    {
                        item.esActivo = false;
                    }
                    _context.SaveChanges();
                    #endregion

                    #region SE ELIMINAN LAS ORDENES DE COMPRA
                    List<tblBL_OrdenesCompra> lstOC = _context.tblBL_OrdenesCompra.Where(x => x.idBackLog == id && x.esActivo).ToList();
                    foreach (var item in lstOC)
                    {
                        item.esActivo = false;
                        item.fechaModificacionNumOC = DateTime.Now;
                    }
                    _context.SaveChanges();
                    #endregion

                    #region SE ELIMINAN LAS REQUISICIONES
                    List<tblBL_Requisiciones> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.idBackLog == id && x.esActivo).ToList();
                    foreach (var item in lstRequisiciones)
                    {
                        item.esActivo = false;
                        item.fechaModificacionRequisicion = DateTime.Now;
                    }
                    _context.SaveChanges();
                    #endregion

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(EliminarBackLog));
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarBackLog", e, AccionEnum.ELIMINAR, id, 0);
                    return false;
                }
            }
        }

        public bool EliminarParte(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region SE ACTUALIZA EL PPTO
                    tblBL_Partes objParte = _context.tblBL_Partes.Where(x => x.id == id).FirstOrDefault();
                    int idBL = objParte.idBacklog;
                    decimal pptoInsumo = objParte.costoPromedio;
                    int idSegPpto = _context.tblBL_CatBackLogs.Where(x => x.id == idBL && x.tipoBL == (int)TipoBackLogEnum.TMC && x.esActivo).Select(s => s.idSegPpto).FirstOrDefault();
                    tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBL).FirstOrDefault();
                    objBL.presupuestoEstimado -= (decimal)pptoInsumo;
                    _context.SaveChanges();

                    tblBL_SeguimientoPptos objActualizarSegPpto = _context.tblBL_SeguimientoPptos.Where(x => x.id == idSegPpto).FirstOrDefault();
                    if (objActualizarSegPpto != null)
                    {
                        objActualizarSegPpto.Ppto -= (decimal)pptoInsumo;
                        _context.SaveChanges();
                    }


                    #endregion

                    #region SE ELIMINA EL INSUMO ASIGNADO AL BL
                    var EliminarParte = _context.tblBL_Partes.FirstOrDefault(x => x.id == id);
                    EliminarParte.esActivo = false;
                    _context.SaveChanges();
                    #endregion

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(EliminarParte));
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarParte", e, AccionEnum.ELIMINAR, id, 0);
                    return false;
                }
            }
        }

        public bool EliminarManoObra(int id)
        {
            try
            {
                var EliminarManoObra = _context.tblBL_ManoObra.FirstOrDefault(x => x.id == id);
                EliminarManoObra.esActivo = false;
                _context.SaveChanges();
                SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(EliminarManoObra));
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarManoObra", e, AccionEnum.ELIMINAR, id, 0);
                return false;
            }
        }

        public List<tblBL_ManoObra> GetManoObra(int idBackLog)
        {
            try
            {
                List<tblBL_ManoObra> lstManoObra = _context.tblBL_ManoObra.Where(x => x.idBackLog == idBackLog && x.esActivo).ToList();
                return lstManoObra;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetManoObra", e, AccionEnum.ELIMINAR, idBackLog, 0);
                return null;
            }
        }

        public bool ActualizarEstatusBL(int idBackLog)
        {
            try
            {
                #region SE ACTUALIZA EL ESTATUS DE BL
                var ActualizarEstatusBL = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == idBackLog && x.esActivo);

                List<tblBL_Requisiciones> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.idBackLog == idBackLog && x.esActivo).ToList();
                List<tblBL_OrdenesCompra> lstOC = _context.tblBL_OrdenesCompra.Where(x => x.idBackLog == idBackLog && x.esActivo).ToList();

                if (lstRequisiciones.Count() == 0)
                    ActualizarEstatusBL.idEstatus = (int)EstatusBackLogEnum.ElaboracionInspeccion;
                else if (lstRequisiciones.Count() > 0 && lstOC.Count() == 0)
                    ActualizarEstatusBL.idEstatus = (int)EstatusBackLogEnum.ElaboracionRequisicion;
                else if (lstRequisiciones.Count() > 0 && lstOC.Count() > 0)
                    ActualizarEstatusBL.idEstatus = (int)EstatusBackLogEnum.ElaboracionOC;

                int idEstatus = ActualizarEstatusBL.idEstatus;
                ActualizarEstatusBL.fechaModificacionBL = DateTime.Now;
                _context.SaveChanges();
                #endregion

                #region SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                int diasTranscurridos = 0;
                tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == idBackLog && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                if (objBitacoraBL != null)
                    diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;

                tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                objGuardarBitacoraEstatusBL.idBL = idBackLog;
                objGuardarBitacoraEstatusBL.areaCuenta = ActualizarEstatusBL.areaCuenta;
                objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                objGuardarBitacoraEstatusBL.idEstatus = idEstatus;
                objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                objGuardarBitacoraEstatusBL.esActivo = true;
                _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                _context.SaveChanges();
                #endregion

                #region SE CREA BITACORA
                SaveBitacora(12, (int)AccionEnum.ACTUALIZAR, idBackLog, JsonUtils.convertNetObjectToJson(idBackLog));
                #endregion

                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarEstatusBL", e, AccionEnum.ACTUALIZAR, idBackLog, JsonUtils.convertNetObjectToJson(idBackLog));
                return false;
            }
        }

        public bool VerificarEntradaAlmacenOC()
        {
            try
            {
                //List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.idEstatus == (int)EstatusBackLogEnum.ElaboracionOC && x.esActivo && x.tipoBL == (int)TipoBackLogEnum.Obra).ToList();
                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.idEstatus == (int)EstatusBackLogEnum.ElaboracionOC && x.esActivo).ToList();
                List<int> lstBLID = new List<int>();
                foreach (var item in lstBL)
                {
                    lstBLID.Add(item.id);
                }
                List<tblBL_OrdenesCompra> lstOrdenesCompra = _context.tblBL_OrdenesCompra.Where(x => lstBLID.Contains(x.idBackLog) && x.esActivo).ToList();

                if (lstBL.Count() > 0)
                {
                    List<BackLogsDTO> lstBLDTO = new List<BackLogsDTO>();
                    foreach (var item in lstBL)
                    {
                        BackLogsDTO objBL = new BackLogsDTO();
                        objBL.id = item.id;
                        objBL.cc = "'" + item.cc + "'";
                        lstBLDTO.Add(objBL);
                    }

                    #region SE OBTIENE LAS ORDENES DE COMPRA EN BASE A LOS NUMERO ECONOMICOS ENCONTRADOS DE LOS BL
                    string strQuery = @"SELECT cc, numero, estatus FROM DBA.so_orden_compra WHERE cc IN ({0}) AND estatus = {1}";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, string.Join(",", lstBLDTO.Select(s => s.cc).ToList()), "'T'");

                    List<BackLogsDTO> lstOCEK = new List<BackLogsDTO>();
                    if (productivo)
                        lstOCEK = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstOCEK = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                    #endregion

                    #region SE ACTUALIZA EL ESTATUS DEL BL DE ORDEN DE COMPRA A SUMINISTRO DE REFACCIONES
                    foreach (var item in lstOCEK)
                    {
                        string cc = item.cc;
                        string numOC = item.numero;
                        List<tblBL_OrdenesCompra> lstOC = lstOrdenesCompra.Where(x => x.cc == cc && x.numOC == numOC).ToList();
                        List<tblBL_CatBackLogs> lstBLActualizar = lstBL.Where(x => lstOC.Select(s => s.idBackLog).Contains(x.id) && x.cc == cc).ToList();
                        if (lstBLActualizar.Count > 0)
                        {
                            List<int> lstID = new List<int>();
                            for (int i = 0; i < lstBLActualizar.Count(); i++)
                            {
                                lstID.Add(lstBLActualizar[i].id);
                            }
                            var ActualizarBL = _context.tblBL_CatBackLogs.First(x => lstID.Contains(x.id));
                            ActualizarBL.idEstatus = (int)EstatusBackLogEnum.SuministroRefacciones;
                            ActualizarBL.fechaModificacionBL = DateTime.Now;
                            _context.SaveChanges();

                            #region SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                            for (int i = 0; i < lstID.Count(); i++)
                            {
                                int idBL = lstID[i];
                                tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                                if (objBitacoraBL != null)
                                {
                                    string _areaCuenta = lstBL.Where(w => w.id == idBL && w.esActivo).Select(s => s.areaCuenta).FirstOrDefault();
                                    int diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;
                                    tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                                    objGuardarBitacoraEstatusBL.idBL = idBL;
                                    objGuardarBitacoraEstatusBL.areaCuenta = _areaCuenta;
                                    objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                                    objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.SuministroRefacciones;
                                    objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                    objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                                    objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                                    objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                                    objGuardarBitacoraEstatusBL.esActivo = true;
                                    _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                                    _context.SaveChanges();
                                }
                            }
                            #endregion

                            #region SE ENVIA CORREO PARA AVISAR QUE YA LLEGARON LOS INSUMOS AL ALMACEN
                            //List<string> lstCorreos = new List<string>();
                            //lstCorreos.Add(vSesiones.sesionUsuarioDTO.correo);

                            //string tipoBL = lstBL[0].tipoBL == 1 ? "OBRA" : "TMC";
                            //string areaCuenta = lstBL[0].areaCuenta.Trim();
                            //string folioBL = lstBL[0].folioBL.Trim();
                            //string descripcion = lstBL[0].descripcion.Trim();

                            //string html = string.Empty;
                            //html +=
                            //    "<h3>BackLogs - " + tipoBL + "</h3>" +
                            //    "<p>El almacen ha sido surtido</p>" +

                            //    "<p>Proyecto: " + areaCuenta + " </p>" +
                            //    "<p>Folio BL: " + folioBL + " </p>" +
                            //    "<p>CC: " + cc + " </p>" +
                            //    "<p>No. OC: " + numOC + " </p>" +
                            //    "<p>Descripción: " + descripcion + " </p>";

                            //GlobalUtils.sendEmail("PRUEBA", html, lstCorreos);
                            #endregion
                        }
                    }
                    #endregion
                }
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "VerificarEntradaAlmacenOC", e, AccionEnum.ACTUALIZAR, 0, 0);
                return false;
            }
        }

        public bool ConfirmarRehabilitacionProgramada(int idBL)
        {
            try
            {
                tblBL_CatBackLogs objActualizar = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == idBL && x.esActivo);
                if (objActualizar != null)
                {
                    objActualizar.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                    _context.SaveChanges();

                    #region SE CREA BITACORA DEL ESTATUS DEL BL
                    tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objBitacoraBL != null)
                    {
                        string _areaCuenta = objActualizar.areaCuenta;
                        int diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;
                        tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                        objGuardarBitacoraEstatusBL.idBL = idBL;
                        objGuardarBitacoraEstatusBL.areaCuenta = _areaCuenta;
                        objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                        objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                        objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                        objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                        objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                        objGuardarBitacoraEstatusBL.esActivo = true;
                        _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                        _context.SaveChanges();
                    }
                    #endregion

                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "VerificarEntradaAlmacenOC", e, AccionEnum.ACTUALIZAR, 0, 0);
                return false;
            }
        }

        public bool ConfirmarProcesoInstalacion(int idBL)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE ACTUALIZA EL ESTATUS DEL BL A PROCESO DE INSTALACIÓN
                    tblBL_CatBackLogs objActualizar = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == idBL && x.esActivo);
                    if (objActualizar != null)
                    {
                        objActualizar.idEstatus = (int)EstatusBackLogEnum.ProcesoInstalacion;
                        _context.SaveChanges();
                    }
                    else
                        return false;
                    #endregion

                    #region SE CREA BITACORA DEL ESTATUS DEL BL
                    tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objBitacoraBL != null)
                    {
                        string _areaCuenta = objActualizar.areaCuenta;
                        int diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;
                        tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                        objGuardarBitacoraEstatusBL.idBL = idBL;
                        objGuardarBitacoraEstatusBL.areaCuenta = _areaCuenta;
                        objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                        objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.ProcesoInstalacion;
                        objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                        objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                        objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                        objGuardarBitacoraEstatusBL.esActivo = true;
                        _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ConfirmarProcesoInstalacion", e, AccionEnum.ACTUALIZAR, 0, 0);
                    return false;
                }
        }

        public bool ConfirmarBackLogInstalado(int idBL)
        {
            try
            {
                tblBL_CatBackLogs objActualizar = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == idBL);
                if (objActualizar != null)
                {
                    objActualizar.idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado;
                    objActualizar.fechaInstaladoBL = DateTime.Now;
                    _context.SaveChanges();

                    #region SE CREA BITACORA DEL ESTATUS DEL BL
                    tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objBitacoraBL != null)
                    {
                        string _areaCuenta = objActualizar.areaCuenta;
                        int diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;
                        tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                        objGuardarBitacoraEstatusBL.idBL = idBL;
                        objGuardarBitacoraEstatusBL.areaCuenta = _areaCuenta;
                        objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                        objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado;
                        objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                        objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                        objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                        objGuardarBitacoraEstatusBL.esActivo = true;
                        _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                        _context.SaveChanges();
                    }
                    #endregion

                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ConfirmarBackLogInstalado", e, AccionEnum.ACTUALIZAR, 0, 0);
                return false;
            }
        }

        public bool ExisteEvidenciaLiberacion(int idBL)
        {
            try
            {
                List<tblBL_Evidencias> lstEvidencias = _context.tblBL_Evidencias.Where(w => w.tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaOTLiberar && w.idBL == idBL && w.esActivo).ToList();
                if (lstEvidencias.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 12, _NOMBRE_CONTROLADOR, "ExisteEvidenciaLiberacion", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool CancelarRequisicion(tblBL_MotivoCancelacionReq objMotivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE REGISTRA EL MOTIVO DE LA CANCELACIÓN DE LA REQUISICIÓN
                    objMotivo.idUsuario = vSesiones.sesionUsuarioDTO.id;
                    objMotivo.fechaCreacion = DateTime.Now;
                    objMotivo.fechaModificacion = DateTime.Now;
                    objMotivo.esActivo = true;
                    _context.tblBL_MotivoCancelacionReq.Add(objMotivo);
                    _context.SaveChanges();
                    #endregion

                    #region SE ACTUALIZA EL ESTATUS DEL BACKLOG
                    tblBL_CatBackLogs objActualizarEstatusBL = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == objMotivo.idBL && x.esActivo);
                    objActualizarEstatusBL.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                    _context.SaveChanges();
                    #endregion

                    #region SE CREA BITACORA DEL ESTATUS DEL BL
                    tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == objMotivo.idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objBitacoraBL != null)
                    {
                        string _areaCuenta = objActualizarEstatusBL.areaCuenta;
                        int diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;
                        tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                        objGuardarBitacoraEstatusBL.idBL = objMotivo.idBL;
                        objGuardarBitacoraEstatusBL.areaCuenta = _areaCuenta;
                        objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                        objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                        objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                        objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                        objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                        objGuardarBitacoraEstatusBL.esActivo = true;
                        _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CancelarRequisicion", e, AccionEnum.ACTUALIZAR, 0, 0);
                    return false;
                }
        }

        public BackLogsDTO GetDatosBL(int idBL)
        {
            try
            {
                List<tblBL_CatBackLogs> dataBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBL && x.esActivo).ToList();
                List<tblBL_Partes> dataPartes = _context.tblBL_Partes.Where(x => x.idBacklog == idBL && x.esActivo).ToList();
                string noEconomico = dataBL[0].noEconomico;
                List<tblM_CatMaquina> dataMaquina = _context.tblM_CatMaquina.Where(x => x.noEconomico == noEconomico && x.estatus == 1).ToList();
                int idCatMaquina = dataMaquina.FirstOrDefault().id;

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    string areaCuenta = dataBL[0].areaCuenta;
                    string PERU_cc = _context.tblP_CC.Where(w => w.areaCuenta == areaCuenta && w.estatus).Select(s => s.cc).FirstOrDefault();

                    foreach (var item in dataBL)
                    {
                        item.cc = PERU_cc;
                    }
                    #endregion
                }

                BackLogsDTO lstBL = dataBL.Select(x => new BackLogsDTO
                {
                    id = x.id,
                    areaCuenta = x.areaCuenta,
                    noEconomico = x.noEconomico,
                    modelo = string.Empty,
                    idCatMaquina = idCatMaquina,
                    horometro = x.horas,
                    folioBL = x.folioBL,
                    cc = x.cc,
                    lstPartes = dataPartes,
                    descripcion = x.descripcion,
                    idUsuarioResponsable = x.idUsuarioResponsable,
                    responsable = string.Empty,
                    fechaInspeccion = x.fechaInspeccion
                }).FirstOrDefault();

                if (lstBL.idUsuarioResponsable > 0)
                {
                    #region SE OBTIENE LISTADO DE USUARIOS
                    var lstUsuarios = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados WHERE clave_empleado = @usr",
                        parametros = new { usr = lstBL.idUsuarioResponsable }
                    });

                    string nombreCompleto = string.Empty;
                    if (lstUsuarios.Count() > 0)
                    {
                        nombreCompleto = lstUsuarios[0].nombre + " " + lstUsuarios[0].ape_paterno + " " + lstUsuarios[0].ape_materno;
                        lstBL.responsable = nombreCompleto;
                    }
                    else
                    {
                        lstUsuarios = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados 
                                    WHERE clave_empleado = @usr",
                            parametros = new { usr = lstBL.idUsuarioResponsable }
                        });
                        nombreCompleto = lstUsuarios[0].nombre + " " + lstUsuarios[0].ape_paterno + " " + lstUsuarios[0].ape_materno;
                        lstBL.responsable = nombreCompleto;
                    }
                    #endregion
                }

                string modelo = dataMaquina.Where(w => w.noEconomico == noEconomico).Select(s => s.modeloEquipo.descripcion).FirstOrDefault();
                if (!string.IsNullOrEmpty(modelo))
                    lstBL.modelo = modelo;

                return lstBL;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDatosBL", e, AccionEnum.ACTUALIZAR, idBL, idBL);
                return null;
            }
        }

        public Dictionary<string, object> GetIDOT(int idBL)
        {
            try
            {
                resultado = new Dictionary<string, object>();

                #region SE OBTIENE ID DE LA OT DEL BACKLOG SELECCIONADO
                int idOT = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT idOT FROM tblBL_OT WHERE idBL = @idBL AND esActivo = @esActivo",
                    parametros = new { idBL = idBL, esActivo = true }
                }).FirstOrDefault();

                // SI NO SE ENCUENTRA idOT, SIGNIFICA QUE NO HAY RELACIÓN DE OT CON UN BL.
                if (idOT <= 0)
                    throw new Exception("No se encuentra relación de una OT al BackLog seleccionado.");

                resultado.Add("idOT", idOT);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetIDOT", e, AccionEnum.CONSULTA, idBL, idBL);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public bool GetTipoBL(int idBL)
        {
            try
            {
                bool esObra = true;
                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBL && x.esActivo).ToList();
                if (lstBL.Count() > 0)
                {
                    int tipoBL = lstBL.FirstOrDefault().tipoBL;
                    if (tipoBL == (int)TipoBackLogEnum.Obra)
                        esObra = true;
                    else
                        esObra = false;
                }
                return esObra;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTipoBL", e, AccionEnum.ACTUALIZAR, idBL, idBL);
                return true;
            }
        }

        public Dictionary<string, object> MostrarEvidencia(int idEvidencia)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                tblBL_Evidencias captura = _context.tblBL_Evidencias.Where(w => w.id == idEvidencia).FirstOrDefault();
                Stream fileStream = GlobalUtils.GetFileAsStream(captura.rutaArchivo);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);
                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(captura.rutaArchivo).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public List<BackLogsDTO> GetPorEquipoDet(BackLogsDTO objFiltro)
        {
            return null;
        }

        public List<FoliosTraspasosDTO> GetFoliosTraspasos(int idBL)
        {
            try
            {
                #region SE OBTIENE LOS FOLIOS RELACIONADOS DEL BL
                List<FoliosTraspasosDTO> lstFoliosTraspasos = _context.tblBL_FoliosTraspasos.Where(w => w.idBL == idBL && w.esActivo).Select(s => new FoliosTraspasosDTO
                {
                    id = s.id,
                    idBL = s.idBL,
                    almacenID = s.almacenID,
                    almacen = string.Empty,
                    numero = s.numero,
                    cc = s.cc,
                    almDestinoID = s.almDestinoID,
                    almDestino = string.Empty,
                    ccDestino = s.ccDestino,
                    folioTraspaso = s.folioTraspaso,
                    traspasoCompleto = s.esTraspasoCompleto ? "Completo" : "Pendiente"
                }).ToList();

                #region SE OBTIENE LOS ALMACENES DE ARRENDADORA
                string strQuery = @"SELECT almacen, descripcion FROM si_almacen";
                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery);
                List<dynamic> lstAlmacenes = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);

                foreach (var item in lstFoliosTraspasos)
                {
                    item.almacen = item.almacenID + " - " + lstAlmacenes.Where(w => w.almacen == item.almacenID).Select(s => s.descripcion).FirstOrDefault();
                    item.almDestino = item.almDestinoID + " - " + lstAlmacenes.Where(w => w.almacen == item.almDestinoID).Select(s => s.descripcion).FirstOrDefault();
                }
                #endregion

                #region SE OBTIENE EL ESTATUS DEL TRASPASO
                //strQuery = string.Empty;
                //strQuery = "SELECT * FROM si_movimientos WHERE tipo_mov = 52 AND alm_destino = {0} AND cc = {1}"
                #endregion

                #endregion

                return lstFoliosTraspasos;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetFoliosTraspasos", e, AccionEnum.CONSULTA, idBL, idBL);
                return null;
            }
        }

        public bool CrearEditarTraspasoFolio(FoliosTraspasosDTO objCE)
        {
            try
            {
                if (objCE.id > 0)
                {
                    #region SE ACTUALIZA EL FOLIO
                    tblBL_FoliosTraspasos objActualizar = _context.tblBL_FoliosTraspasos.Where(w => w.id == objCE.id && w.esActivo).FirstOrDefault();
                    if (objActualizar != null)
                    {
                        objActualizar.almacenID = objCE.almacenID;
                        objActualizar.numero = objCE.numero;
                        objActualizar.cc = objCE.cc;
                        objActualizar.ccDestino = objCE.ccDestino;
                        objActualizar.almDestinoID = objCE.almDestinoID;
                        objActualizar.ccDestino = objCE.ccDestino;
                        objActualizar.folioTraspaso = objCE.folioTraspaso;
                        objActualizar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                        objActualizar.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                    }
                    #endregion
                }
                else
                {
                    #region SE OBTIENE EL CC DEL BL
                    tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.Where(w => w.id == objCE.idBL).FirstOrDefault();
                    objCE.ccDestino = objBL.cc;
                    objCE.areaCuenta = objBL.areaCuenta;
                    #endregion

                    #region SE CREA EL FOLIO
                    tblBL_FoliosTraspasos objNuevo = new tblBL_FoliosTraspasos();
                    objNuevo.idBL = objCE.idBL;
                    objNuevo.areaCuenta = objCE.areaCuenta;
                    objNuevo.almacenID = objCE.almacenID;
                    objNuevo.numero = objCE.numero;
                    objNuevo.cc = objCE.cc;
                    objNuevo.ccDestino = objCE.ccDestino;
                    objNuevo.almDestinoID = objCE.almDestinoID;
                    objNuevo.ccDestino = objCE.ccDestino;
                    objNuevo.folioTraspaso = objCE.folioTraspaso;
                    objNuevo.esTraspasoCompleto = false;
                    objNuevo.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objNuevo.idUsuarioModificacion = 0;
                    objNuevo.fechaCreacion = DateTime.Now;
                    objNuevo.fechaModificacion = new DateTime(2000, 01, 01);
                    objNuevo.esActivo = true;
                    _context.tblBL_FoliosTraspasos.Add(objNuevo);
                    _context.SaveChanges();
                    #endregion
                }
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearEditarTraspasoFolio", e, AccionEnum.CONSULTA, objCE.id, objCE);
                return false;
            }
        }

        public bool VerificarTraspasosBL(string areaCuenta)
        {
            try
            {
                // SE OBTIENE LOS BL CON FOLIO, RELACIONADOS AL AREA CUENTA SELECCIONADO
                List<tblBL_FoliosTraspasos> lstFoliosRelAreaCuenta = _context.tblBL_FoliosTraspasos.Where(w => !string.IsNullOrEmpty(areaCuenta) ? areaCuenta.Contains(w.areaCuenta) : true && !w.esTraspasoCompleto && w.esActivo).ToList();

                string strQuery = string.Empty;
                foreach (var item in lstFoliosRelAreaCuenta)
                {
                    int almacenID = item.almacenID;
                    int numero = item.numero;
                    string cc = item.cc;
                    int almDestinoID = item.almDestinoID;
                    string ccDestino = item.ccDestino;
                    int folioTraspaso = item.folioTraspaso;

                    List<dynamic> lstMovimientos = new List<dynamic>();
                    strQuery = string.Empty;
                    strQuery = "SELECT * FROM si_movimientos WHERE (tipo_mov = 52 and cc = '{0}' AND almacen = {1}) OR (almacen = {2} AND tipo_mov = 2 and folio_traspaso = {3} AND alm_destino = {4} AND cc_destino = '{5}')";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, cc, almDestinoID, almacenID, folioTraspaso, almDestinoID, ccDestino);
                    lstMovimientos = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);

                    if (lstMovimientos.Count() > 0)
                    {
                        decimal totalSalida = 0, totalEntrada = 0;
                        foreach (var itemSalida in lstMovimientos.Where(w => w.tipo_mov == 52))
                        {
                            decimal total = itemSalida.total;
                            totalSalida += total;
                        }
                        foreach (var itemEntrada in lstMovimientos.Where(w => w.tipo_mov == 2))
                        {
                            decimal total = itemEntrada.total;
                            totalEntrada += total;
                        }

                        if (totalEntrada == totalSalida)
                        {
                            #region ENTRADA COMPLETA
                            // SE ACTUALIZA ESTATUS DEL BL AL 80%
                            tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.Where(w => w.id == item.idBL).FirstOrDefault();
                            if (objBL != null)
                            {
                                objBL.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                                objBL.fechaModificacionBL = DateTime.Now;
                                _context.SaveChanges();
                            }

                            // SE ACTUALIZA QUE ES COMPLETA LA ENTRADA DEL FOLIO
                            tblBL_FoliosTraspasos objFolio = _context.tblBL_FoliosTraspasos.Where(w => w.idBL == item.idBL).FirstOrDefault();
                            if (objFolio != null)
                            {
                                objFolio.esTraspasoCompleto = true;
                                objFolio.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                objFolio.fechaModificacion = DateTime.Now;
                                _context.SaveChanges();
                            }

                            // SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                            int diasTranscurridos = 0;
                            tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == item.idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                            if (objBitacoraBL != null)
                                diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;

                            tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                            objGuardarBitacoraEstatusBL.idBL = item.idBL;
                            objGuardarBitacoraEstatusBL.areaCuenta = objBL.areaCuenta;
                            objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                            objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                            objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                            objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                            objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                            objGuardarBitacoraEstatusBL.esActivo = true;
                            _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                            _context.SaveChanges();
                            #endregion
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "VerificarTraspasosBL", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool EliminarFolioTraspaso(int idFolioTraspaso)
        {
            try
            {
                #region SE ELIMINA EL REGISTO SELECCIONADO
                tblBL_FoliosTraspasos objEliminar = _context.tblBL_FoliosTraspasos.Where(w => w.id == idFolioTraspaso && w.esActivo).FirstOrDefault();
                if (objEliminar != null)
                {
                    objEliminar.esActivo = false;
                    objEliminar.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    return true;
                }
                return false;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarFolioTraspaso", e, AccionEnum.CONSULTA, idFolioTraspaso, idFolioTraspaso);
                return false;
            }
        }

        public bool CambiarEstatusBL90a80(int idBL)
        {
            try
            {
                #region SE ACTUALIZA EL ESTATUS DEL BL DE 90% A 80%
                tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.Where(w => w.id == idBL).FirstOrDefault();
                if (objBL != null)
                {
                    objBL.idEstatus = (int)EstatusBackLogEnum.RehabilitacionProgramada;
                    objBL.fechaModificacionBL = DateTime.Now;
                    _context.SaveChanges();
                    return true;
                }
                return false;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CambiarEstatusBL90a80", e, AccionEnum.CONSULTA, idBL, idBL);
                return false;
            }
        }

        public Dictionary<string, object> VisualizarEvidencia(int idEvidencia)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblBL_Evidencias.FirstOrDefault(x => x.id == idEvidencia && x.esActivo);

                Stream fileStream = GlobalUtils.GetFileAsStream(captura.rutaArchivo);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(captura.rutaArchivo).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public List<tblBL_Partes> GetPartesRelBL(int idBL)
        {
            List<tblBL_Partes> lstPartes = new List<tblBL_Partes>();
            try
            {
                lstPartes = _context.tblBL_Partes.Where(w => w.idBacklog == idBL && w.esActivo).ToList();
                if (lstPartes == null)
                {
                    tblBL_Partes obj = new tblBL_Partes();
                    lstPartes.Add(obj);
                }

                foreach (var item in lstPartes)
                {
                    if (!string.IsNullOrEmpty(item.parte))
                        item.parte = item.parte.Trim().ToUpper();

                    if (!string.IsNullOrEmpty(item.articulo))
                        item.articulo = item.articulo.Trim().ToUpper();

                    if (!string.IsNullOrEmpty(item.unidad))
                        item.unidad = item.unidad.Trim().ToUpper();
                }
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetPartesRelBL", ex, AccionEnum.CONSULTA, idBL, new { idBL = idBL });
                return lstPartes;
            }
            return lstPartes;
        }

        public List<ParteDTO> GetPartesRptOrdebBL(int idBL)
        {
            List<ParteDTO> lstPartesDTO = new List<ParteDTO>();
            try
            {
                List<tblBL_Partes> lstPartes = _context.tblBL_Partes.Where(w => w.idBacklog == idBL && w.esActivo).ToList();
                if (lstPartes == null)
                {
                    ParteDTO obj = new ParteDTO();
                    lstPartesDTO.Add(obj);
                }

                ParteDTO objParteDTO = new ParteDTO();
                foreach (var item in lstPartes)
                {
                    objParteDTO.insumo = item.insumo;
                    objParteDTO.cantidad = item.cantidad;
                    objParteDTO.parte = !string.IsNullOrEmpty(item.parte) ? item.parte.Trim().ToUpper() : "-";
                    objParteDTO.articulo = !string.IsNullOrEmpty(item.articulo) ? item.articulo.Trim().ToUpper() : "-";
                    objParteDTO.unidad = !string.IsNullOrEmpty(item.unidad) ? item.unidad.Trim().ToUpper() : "-";
                    objParteDTO.strMoneda = GetNombreTipoMoneda(item.tipoMoneda);
                    objParteDTO.costoPromedio = item.costoPromedio.ToString("C");
                    lstPartesDTO.Add(objParteDTO);
                }
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetPartesRelBL", ex, AccionEnum.CONSULTA, idBL, new { idBL = idBL });
                return lstPartesDTO;
            }
            return lstPartesDTO;
        }
        #endregion

        #region CATALOGO CONJUNTOS
        public List<CatConjuntosDTO> GetConjuntos()
        {
            try
            {
                List<CatConjuntosDTO> lstConjuntos = _context.tblBL_CatConjuntos.Where(x => x.esActivo).OrderBy(x => x.descripcion).Select(x => new CatConjuntosDTO
                {
                    id = x.id,
                    descripcion = x.descripcion.ToUpper(),
                    abreviacion = x.abreviacion.ToUpper()
                }).ToList();
                return lstConjuntos;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetConjuntos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool CrearConjunto(tblBL_CatConjuntos objConjunto)
        {
            try
            {
                if (!string.IsNullOrEmpty(objConjunto.descripcion.ToUpper().Trim()))
                {
                    objConjunto.esActivo = true;
                    if (objConjunto.abreviacion != null)
                    {
                        objConjunto.abreviacion = objConjunto.abreviacion;
                    }
                    _context.tblBL_CatConjuntos.Add(objConjunto);
                    _context.SaveChanges();
                }
                else
                    throw new Exception("Es necesario indicar el nombre del conjunto.");

                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearConjunto", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objConjunto));
                return false;
            }
        }

        public bool ActualizarConjunto(tblBL_CatConjuntos objConjunto)
        {
            try
            {

                if (objConjunto.id > 0)
                {
                    if (!string.IsNullOrEmpty(objConjunto.descripcion.Trim()))
                    {
                        var ActualizarConjunto = _context.tblBL_CatConjuntos.Where(x => x.id == objConjunto.id).First();
                        ActualizarConjunto.descripcion = objConjunto.descripcion.Trim();
                        if (ActualizarConjunto.abreviacion == null)
                        {
                            ActualizarConjunto.abreviacion = objConjunto.abreviacion;
                        }
                        else
                        {
                            if (ActualizarConjunto.abreviacion != null)
                            {
                                ActualizarConjunto.abreviacion = objConjunto.abreviacion;
                            }
                        }
                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Es necesario indicar el nombre del conjunto.");
                }
                return false;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarConjunto", ex, AccionEnum.ACTUALIZAR, objConjunto.id, JsonUtils.convertNetObjectToJson(objConjunto));
                return false;
            }
        }

        public bool EliminarConjunto(int idConjunto)
        {
            try
            {
                if (idConjunto > 0)
                {
                    var EliminarConjunto = _context.tblBL_CatConjuntos.Where(x => x.id == idConjunto).First();
                    if (EliminarConjunto != null)
                    {
                        EliminarConjunto.esActivo = false;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Hubo un error al obtener los datos del conjunto.");
                }
                else
                    throw new Exception("Hubo un error al obtener los datos del conjunto.");
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarConjunto", ex, AccionEnum.ELIMINAR, idConjunto, JsonUtils.convertNetObjectToJson(idConjunto));
                return false;
            }
        }
        #endregion

        #region CATALOGO SUBCONJUNTOS
        public List<CatSubconjuntosDTO> GetSubconjuntos()
        {
            try
            {
                List<CatSubconjuntosDTO> lstCatSubconjuntos = _context.tblBL_CatSubconjuntos.Where(x => x.esActivo).OrderBy(x => x.descripcion).Select(x => new CatSubconjuntosDTO
                {
                    id = x.id,
                    descripcion = x.descripcion.ToUpper(),
                    idConjunto = x.idConjunto,
                    conjunto = x.CatConjuntos.descripcion.ToUpper(),
                    abreviacion = x.abreviacion.ToUpper()
                }).ToList();
                return lstCatSubconjuntos;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetSubconjuntos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool CrearSubconjunto(tblBL_CatSubconjuntos objSubconjunto)
        {
            try
            {
                objSubconjunto.descripcion = objSubconjunto.descripcion.Trim();
                if (objSubconjunto.abreviacion != null)
                {
                    objSubconjunto.abreviacion = objSubconjunto.abreviacion;
                }
                objSubconjunto.esActivo = true;
                _context.tblBL_CatSubconjuntos.Add(objSubconjunto);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearSubconjunto", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objSubconjunto));
                return false;
            }
        }

        public bool ActualizarSubconjunto(tblBL_CatSubconjuntos objSubconjunto)
        {
            try
            {
                if (objSubconjunto.id > 0)
                {
                    if (!string.IsNullOrEmpty(objSubconjunto.descripcion.Trim()))
                    {
                        var ActualizarSubconjunto = _context.tblBL_CatSubconjuntos.Where(x => x.id == objSubconjunto.id).First();
                        ActualizarSubconjunto.idConjunto = objSubconjunto.idConjunto;
                        ActualizarSubconjunto.descripcion = objSubconjunto.descripcion.Trim();

                        if (ActualizarSubconjunto.abreviacion == null)
                        {
                            ActualizarSubconjunto.abreviacion = objSubconjunto.abreviacion;
                        }
                        else
                        {
                            if (ActualizarSubconjunto.abreviacion != null)
                            {
                                ActualizarSubconjunto.abreviacion = objSubconjunto.abreviacion;
                            }
                        }
                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Es necesario indiciar el nombre del subconjunto.");
                }
                return false;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarSubconjunto", ex, AccionEnum.ACTUALIZAR, objSubconjunto.id, JsonUtils.convertNetObjectToJson(objSubconjunto));
                return false;
            }
        }

        public bool EliminarSubconjunto(int idSubconjunto)
        {
            try
            {
                if (idSubconjunto > 0)
                {
                    var EliminarSubconjunto = _context.tblBL_CatSubconjuntos.Where(x => x.id == idSubconjunto).First();
                    if (EliminarSubconjunto != null)
                    {
                        EliminarSubconjunto.esActivo = false;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Hubo un error al obtener los datos del subconjunto.");
                }
                else
                    throw new Exception("Hubo un error al obtener los datos del subconjunto.");
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarSubconjunto", ex, AccionEnum.ELIMINAR, idSubconjunto, JsonUtils.convertNetObjectToJson(idSubconjunto));
                return false;
            }
        }
        #endregion

        #region CATALOGO REQUISICIONES
        public List<RequisicionesDTO> GetRequisiciones(RequisicionesDTO objRequisicion)
        {
            try
            {
                List<RequisicionesDTO> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.idBackLog == objRequisicion.idBackLog && x.esActivo).Select(x => new RequisicionesDTO
                {
                    id = x.id,
                    numRequisicion = x.numRequisicion
                }).ToList();
                return lstRequisiciones;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetRequisiciones", ex, AccionEnum.CONSULTA, 0, objRequisicion);
                return null;
            }
        }

        public bool ValidaNumeroRequisicion(tblBL_Requisiciones objRequisiciones)
        {
            try
            {
                string cc = string.Empty;
                string noEconomico = string.Empty;
                int numRequisicion = Convert.ToInt32(objRequisiciones.numRequisicion) > 0 ? Convert.ToInt32(objRequisiciones.numRequisicion) : 0;

                #region SE OBTIENE EL NÚMERO ECONOMICO DEL BACKLOG
                List<tblBL_CatBackLogs> lstBackLogs = _context.tblBL_CatBackLogs.Where(x => x.id == objRequisiciones.idBackLog && x.esActivo).ToList();
                if (lstBackLogs.Count() > 0)
                    noEconomico = "'" + lstBackLogs[0].noEconomico + "'";
                #endregion

                #region SE OBTIENE EL CC EN BASE AL NO ECONOMICO QUE CONTENGA EL BACKLOG
                string strQuery = @"SELECT * FROM cc WHERE descripcion = {0}";
                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery, noEconomico);

                List<BackLogsDTO> lstCC = new List<BackLogsDTO>();
                if (productivo)
                    lstCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                else
                    lstCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                if (lstCC.Count() > 0)
                    cc = "'" + lstCC[0].cc + "'";
                #endregion

                #region SE VERIFICA QUE EXISTA EL NÚMERO DE REQUISICIÓN INGRESADO
                strQuery = @"SELECT * FROM so_requisicion WHERE cc = {0} AND numero = {1}";
                odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery, cc, numRequisicion);

                List<RequisicionesDTO> lstNumRequisiciones = new List<RequisicionesDTO>();
                if (productivo)
                    lstNumRequisiciones = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolEnum.ArrenProd, odbc);
                else
                    lstNumRequisiciones = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                if (lstNumRequisiciones.Count() > 0)
                    return true;
                else
                    return false;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ValidaNumeroRequisicion", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool EliminarRequisicion(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region SE OBTIENE BACKLOG ID
                    int idBackLog = _context.tblBL_Requisiciones.First(x => x.id == id && x.esActivo).idBackLog;
                    #endregion

                    #region SE ELIMINA LA REQUISICION DEL BACKLOG
                    var EliminarRequisicion = _context.tblBL_Requisiciones.FirstOrDefault(x => x.id == id);
                    EliminarRequisicion.esActivo = false;
                    _context.SaveChanges();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(EliminarRequisicion));
                    #endregion

                    #region SE ACTUALIZA ESTATUS DEL BACKLOG
                    int tipoBL = _context.tblBL_CatBackLogs.Where(w => w.id == idBackLog).Select(s => s.tipoBL).FirstOrDefault();
                    if (tipoBL == (int)TipoBackLogEnum.Obra)
                    {
                        bool exitoActualizarEstatusBL = ActualizarEstatusBL(idBackLog);
                        if (!exitoActualizarEstatusBL)
                            throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarRequisiciones", ex, AccionEnum.ELIMINAR, id, 0);
                    return false;
                }
            }
        }

        public bool ActualizarRequisicion(tblBL_Requisiciones objRequisicion)
        {
            try
            {
                #region SE ACTUALIZA REQUISICION
                var ActualizarRequisicion = _context.tblBL_Requisiciones.FirstOrDefault(x => x.id == objRequisicion.id);
                ActualizarRequisicion.numRequisicion = objRequisicion.numRequisicion;
                ActualizarRequisicion.fechaModificacionRequisicion = DateTime.Now;
                _context.SaveChanges();
                #endregion

                SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, (int)ActualizarRequisicion.id, JsonUtils.convertNetObjectToJson(objRequisicion));
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarRequisicion", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objRequisicion));
                return false;
            }
        }

        public bool CrearRequisicion(tblBL_Requisiciones objRequisicion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE CREA REQUISICION
                    objRequisicion.esActivo = true;
                    objRequisicion.fechaCreacionRequisicion = DateTime.Now;
                    objRequisicion.fechaModificacionRequisicion = DateTime.Now;
                    _context.tblBL_Requisiciones.Add(objRequisicion);
                    _context.SaveChanges();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objRequisicion));
                    #endregion

                    #region DATOS DEL BL
                    tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.Where(w => w.id == objRequisicion.idBackLog && w.esActivo).FirstOrDefault();
                    #endregion

                    #region SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                    int diasTranscurridos = 0;
                    tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == objRequisicion.idBackLog && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objBitacoraBL != null)
                        diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;

                    tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                    objGuardarBitacoraEstatusBL.idBL = objRequisicion.idBackLog;
                    objGuardarBitacoraEstatusBL.areaCuenta = objBL.areaCuenta;
                    objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                    objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.ElaboracionRequisicion;
                    objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                    objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                    objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                    objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                    objGuardarBitacoraEstatusBL.esActivo = true;
                    _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                    _context.SaveChanges();
                    #endregion

                    #region SE ACTUALIZA ESTATUS DEL BACKLOG
                    bool exitoActualizarEstatusBL = ActualizarEstatusBL(objRequisicion.idBackLog);
                    if (!exitoActualizarEstatusBL)
                        throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearRequisicion", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objRequisicion));
                    dbContextTransaction.Rollback();
                    return false;
                }
        }

        public List<RequisicionesDTO> GetAllRequisiciones(RequisicionesDTO objReq)
        {
            try
            {
                string cc = string.Empty;
                string noEconomico = string.Empty;
                int idBackLog = objReq.idBackLog > 0 ? objReq.idBackLog : 0;

                #region SE OBTIENE EL CC DEL BACKLOG
                tblBL_CatBackLogs lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBackLog && x.esActivo).FirstOrDefault();
                if (lstBL != null)
                    cc = lstBL.cc;
                #endregion

                #region SE VERIFICA SI HAY REQUISICIONES EN BASE AL CC
                List<RequisicionesDTO> lstRequisiciones = new List<RequisicionesDTO>();
                if (!string.IsNullOrEmpty(cc))
                {
                    string strQuery = @"SELECT numero, fecha, comentarios FROM so_requisicion WHERE cc LIKE '%{0}%' AND st_autoriza = '{1}' ORDER BY numero DESC"; //S T
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, cc, "S");

                    if (productivo)
                        lstRequisiciones = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstRequisiciones = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                }
                #endregion

                if (lstRequisiciones.Count > 0)
                    return lstRequisiciones.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetAllRequisiciones", ex, AccionEnum.CONSULTA, 0, objReq);
                return null;
            }
        }

        public List<RequisicionesDTO> GetAllDetRequisiciones(RequisicionesDTO objReq)
        {
            List<RequisicionesDTO> lstRequisicionesDet = new List<RequisicionesDTO>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    int idBL = objReq.idBackLog;
                    string numero = objReq.numero;
                    string areaCuenta = string.Empty;
                    string cc = string.Empty;

                    #region SE OBTIENE EL CC DEL BACKLOG
                    tblBL_CatBackLogs lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBL && x.esActivo).FirstOrDefault();
                    if (lstBL != null)
                        areaCuenta = lstBL.areaCuenta;

                    cc = _context.tblP_CC.Where(w => w.areaCuenta == areaCuenta && w.estatus).Select(s => s.cc).FirstOrDefault();
                    #endregion

                    string numReq = _context.tblBL_Requisiciones.Where(w => w.idBackLog == idBL && w.esActivo).Select(s => s.numRequisicion).FirstOrDefault();
                    int num = Convert.ToInt32(numReq);
                    tblCom_Req objRequisicion = _context.tblCom_Req.Where(w => w.cc == cc && w.numero == num && w.PERU_tipoRequisicion == "RQ" && w.estatusRegistro).FirstOrDefault();
                    List<tblCom_ReqDet> lstRequisicionesDet_BL = _context.tblCom_ReqDet.Where(w => w.idReq == objRequisicion.id && w.estatusRegistro).ToList();

                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        List<MAEART> lstCatInsumos = new List<MAEART>();
                        var insumosID = lstRequisicionesDet_BL.Select(x => x.insumo.ToString().PadLeft(11, '0')).ToList();

                        #region FILTROS
                        lstCatInsumos = _starsoft.MAEART.Where(w => insumosID.Contains(w.ACODIGO)).ToList();
                        #endregion

                        InsumosDTO objInsumoDTO = new InsumosDTO();
                        foreach (var item in lstRequisicionesDet_BL)
                        {
                            var insumoStr = item.insumo.ToString().PadLeft(11, '0');
                            var insumo = lstCatInsumos.FirstOrDefault(x => x.ACODIGO == insumoStr);
                            if (insumo != null)
                            {
                                item.descripcion = insumo.ADESCRI;
                            }
                        }
                    }

                    RequisicionesDTO objRequisicionDTO = new RequisicionesDTO();
                    foreach (var item in lstRequisicionesDet_BL)
                    {
                        objRequisicionDTO = new RequisicionesDTO();
                        objRequisicionDTO.numero = numReq;
                        objRequisicionDTO.partida = item.partida.ToString();
                        objRequisicionDTO.insumo = item.insumo;
                        objRequisicionDTO.fecha_requerido = objReq.fecha_requerido;
                        objRequisicionDTO.cantidad = Convert.ToInt32(item.cantidad);
                        objRequisicionDTO.fecha_ordenada = objReq.fecha_ordenada;
                        objRequisicionDTO.comentarios = item.descripcion;
                        lstRequisicionesDet.Add(objRequisicionDTO);
                    }
                    #endregion
                }
                else
                {
                    #region ARRENDADORA
                    int idBL = objReq.idBackLog;
                    string numero = objReq.numero;
                    string cc = string.Empty;

                    #region SE OBTIENE EL CC DEL BACKLOG
                    tblBL_CatBackLogs lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBL && x.esActivo).FirstOrDefault();
                    if (lstBL != null)
                        cc = lstBL.cc;
                    #endregion

                    #region SE OBTIENE LAS PARTIDAS DE LA REQUISICION - CC, SELECCIONADA
                    if (!string.IsNullOrEmpty(cc))
                    {
                        #region SE OBTIENE REQUISICIONES_DET
                        string strQuery = @"SELECT numero, partida, insumo, fecha_requerido, cantidad, fecha_ordenada FROM so_requisicion_det WHERE cc LIKE '%{0}%' AND numero = {1} ORDER BY numero";
                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, cc, numero);

                        if (productivo)
                            lstRequisicionesDet = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolEnum.ArrenProd, odbc);
                        else
                            lstRequisicionesDet = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                        #endregion

                        #region SE OBTIENE LOS COMENTARIOS DE CADA PARTIDA
                        List<RequisicionesDTO> lstRequisicionesComentarios = new List<RequisicionesDTO>();
                        strQuery = @"SELECT cc, numero, partida, descripcion FROM so_req_det_linea WHERE cc = '{0}' AND numero = {1}";
                        odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, cc, numero);

                        if (productivo)
                            lstRequisicionesComentarios = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolEnum.ArrenProd, odbc);
                        else
                            lstRequisicionesComentarios = _contextEnkontrol.Select<RequisicionesDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                        #endregion

                        foreach (var item in lstRequisicionesDet)
                        {
                            item.comentarios = lstRequisicionesComentarios.Where(w => w.partida == item.partida).Select(s => s.descripcion).FirstOrDefault();
                        }
                    }
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetAllDetRequisiciones", ex, AccionEnum.CONSULTA, 0, objReq);
                return null;
            }
            return lstRequisicionesDet.ToList();
        }

        public List<MotivoCancelacionReq> GetMotivosCancelacion(int idBL)
        {
            try
            {
                List<tblBL_MotivoCancelacionReq> objMotivo = _context.tblBL_MotivoCancelacionReq.Where(x => x.idBL == idBL && x.esActivo).ToList();
                List<MotivoCancelacionReq> lstMotivo = objMotivo.Select(x => new MotivoCancelacionReq
                {
                    id = x.id,
                    idBL = x.idBL,
                    usuario = x.lstUsuarios.nombre + " " + x.lstUsuarios.apellidoPaterno + " " + x.lstUsuarios.apellidoMaterno,
                    motivo = x.motivo.Trim(),
                    fechaCreacion = Convert.ToDateTime(x.fechaCreacion)
                }).ToList();
                return lstMotivo;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetMotivosCancelacion", ex, AccionEnum.CONSULTA, idBL, idBL);
                return null;
            }
        }
        #endregion

        #region ORDENES DE COMPRA
        public List<OrdenCompraDTO> GetOrdenesCompra(OrdenCompraDTO objOC)
        {
            List<OrdenCompraDTO> lstOC = new List<OrdenCompraDTO>();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    #region LISTADO DE OC RELACIONADOS AL BL
                    lstOC = _context.tblBL_OrdenesCompra.Where(x => x.idBackLog == objOC.idBackLog && x.esActivo).Select(x => new OrdenCompraDTO
                    {
                        id = x.id,
                        numRequisicion = x.numRequisicion,
                        numOC = x.numOC,
                        estatus = string.Empty
                    }).ToList();

                    List<int> lstNumeroOC = new List<int>();
                    for (int i = 0; i < lstOC.Count(); i++)
                    {
                        int numOC = Convert.ToInt32(lstOC[i].numOC);
                        lstNumeroOC.Add(numOC);
                    }
                    #endregion

                    // SE OBTIENE EL CC DEL ECONOMICO
                    string areaCuenta = _context.tblBL_CatBackLogs.Where(w => w.id == objOC.idBackLog).Select(s => s.areaCuenta).FirstOrDefault();
                    objOC.cc = _context.tblP_CC.Where(w => w.areaCuenta == areaCuenta && w.estatus).Select(s => s.cc).FirstOrDefault();
                    //objOC.cc = _context.tblBL_CatBackLogs.Where(w => w.id == objOC.idBackLog).Select(s => s.cc).FirstOrDefault();

                    // SE OBTIENE LOS NUMEROS DE REQUISICIONES QUE TIENE EL BACKLOG
                    List<string> lstRequisicionesIDstr = _context.tblBL_Requisiciones.Where(w => w.idBackLog == objOC.idBackLog && w.esActivo).Select(s => s.numRequisicion).ToList();
                    List<int> lstRequisicionesID = new List<int>();
                    foreach (var item in lstRequisicionesIDstr)
                    {
                        lstRequisicionesID.Add(Convert.ToInt32(item));
                    }

                    List<dynamic> lstOrdenesComprasRelRequisiciones = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT cc, numRequisicion, numOC FROM tblBL_OrdenesCompra WHERE idBackLog = @idBackLog AND esActivo = @esActivo",
                        parametros = new { idBackLog = objOC.idBackLog, esActivo = true }
                    }).FirstOrDefault();

                    if (lstOrdenesComprasRelRequisiciones != null)
                    {
                        foreach (var item in lstOrdenesComprasRelRequisiciones)
                        {
                            OrdenCompraDTO obj = new OrdenCompraDTO();
                            obj.numRequisicion = item.num_requisicion.ToString();
                            obj.numOC = item.numero.ToString();
                            lstOC.Add(obj);
                        }

                        for (int i = 0; i < lstOC.Count(); i++)
                        {
                            int numOC = Convert.ToInt32(lstOC[i].numOC);
                            lstNumeroOC.Add(numOC);
                        }

                        #region SE VERIFICA EL ESTATUS DE LA OC
                        if (lstOC.Count() > 0)
                        {
                            areaCuenta = string.Empty;
                            List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(w => w.id == objOC.idBackLog).ToList();
                            if (lstBL.Count() > 0)
                                areaCuenta = lstBL[0].cc;

                            List<BackLogsDTO> lstCC = _context.Select<dynamic>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT numero, estatus FROM tblCom_OrdenCompra WHERE cc = '@cc' AND numero IN (@numero) AND estatusRegistro = @estatusRegistro AND PERU_tipoCompra = '@PERU_tipoCompra'",
                                parametros = new { cc = objOC.cc, numero = string.Join(",", lstOC), estatusRegistro = true, PERU_tipoCompra = "RQ" }
                            }).FirstOrDefault();

                            foreach (var item in lstOC)
                            {
                                string estatus = string.Empty;
                                if (item.numOC != null)
                                    estatus = lstCC.Where(w => w.numero == item.numOC).Select(s => s.estatus).FirstOrDefault();

                                item.estatus = estatus;
                            }
                        }
                    }
                        #endregion
                    #endregion
                }
                else
                {
                    #region ARRENDADORA
                    #region LISTADO DE OC RELACIONADOS AL BL
                    lstOC = _context.tblBL_OrdenesCompra.Where(x => x.idBackLog == objOC.idBackLog && x.esActivo).Select(x => new OrdenCompraDTO
                    {
                        id = x.id,
                        numRequisicion = x.numRequisicion,
                        numOC = x.numOC,
                        estatus = string.Empty
                    }).ToList();

                    List<int> lstNumeroOC = new List<int>();
                    for (int i = 0; i < lstOC.Count(); i++)
                    {
                        int numOC = Convert.ToInt32(lstOC[i].numOC);
                        lstNumeroOC.Add(numOC);
                    }

                    if (lstOC.Count() <= 0)
                    {
                        // SE OBTIENE EL CC DEL ECONOMICO
                        objOC.cc = _context.tblBL_CatBackLogs.Where(w => w.id == objOC.idBackLog).Select(s => s.cc).FirstOrDefault();

                        // SE OBTIENE LOS NUMEROS DE REQUISICIONES QUE TIENE EL BACKLOG
                        List<string> lstRequisicionesIDstr = _context.tblBL_Requisiciones.Where(w => w.idBackLog == objOC.idBackLog && w.esActivo).Select(s => s.numRequisicion).ToList();
                        List<int> lstRequisicionesID = new List<int>();
                        foreach (var item in lstRequisicionesIDstr)
                        {
                            lstRequisicionesID.Add(Convert.ToInt32(item));
                        }

                        string strQuery = @"SELECT cc, numero, num_requisicion FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = ({1})";
                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, objOC.cc, string.Join(",", lstRequisicionesID));

                        List<dynamic> lstOrdenesComprasRelRequisiciones = new List<dynamic>();
                        lstOrdenesComprasRelRequisiciones = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);

                        foreach (var item in lstOrdenesComprasRelRequisiciones)
                        {
                            OrdenCompraDTO obj = new OrdenCompraDTO();
                            obj.numRequisicion = item.num_requisicion.ToString();
                            obj.numOC = item.numero.ToString();
                            lstOC.Add(obj);
                        }

                        for (int i = 0; i < lstOC.Count(); i++)
                        {
                            int numOC = Convert.ToInt32(lstOC[i].numOC);
                            lstNumeroOC.Add(numOC);
                        }
                    }
                    #endregion

                    #region SE VERIFICA EL ESTATUS DE LA OC
                    if (lstOC.Count() > 0)
                    {
                        string areaCuenta = string.Empty;
                        List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(w => w.id == objOC.idBackLog).ToList();
                        if (lstBL.Count() > 0)
                            areaCuenta = lstBL[0].cc;

                        string strQuery = @"SELECT numero, estatus FROM so_orden_compra WHERE cc = '{0}' AND numero IN ({1})";
                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, areaCuenta, string.Join(",", lstNumeroOC));

                        List<BackLogsDTO> lstCC = new List<BackLogsDTO>();
                        if (productivo)
                            lstCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                        else
                            lstCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                        foreach (var item in lstOC)
                        {
                            string estatus = string.Empty;
                            if (item.numOC != null)
                                estatus = lstCC.Where(w => w.numero == item.numOC).Select(s => s.estatus).FirstOrDefault();

                            item.estatus = estatus;
                        }
                    }
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetOrdenesCompra", ex, AccionEnum.CONSULTA, objOC.id, objOC);
                return null;
            }
            return lstOC;
        }

        public List<OrdenCompraDTO> GetAllOrdenesCompra(OrdenCompraDTO objOC)
        {
            try
            {
                string cc = string.Empty;
                string noEconomico = string.Empty;
                int idBackLog = objOC.idBackLog > 0 ? objOC.idBackLog : 0;

                List<tblBL_Requisiciones> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.idBackLog == idBackLog && x.esActivo).ToList();

                #region SE OBTIENE EL NÚMERO ECONOMICO DEL BACKLOG
                List<tblBL_CatBackLogs> lstBackLogs = _context.tblBL_CatBackLogs.Where(x => x.id == objOC.idBackLog && x.esActivo).ToList();
                if (lstBackLogs.Count() > 0)
                    noEconomico = "'" + lstBackLogs[0].noEconomico + "'";
                #endregion

                #region SE OBTIENE EL CC EN BASE AL NO ECONOMICO QUE CONTENGA EL BACKLOG
                string strQuery = @"SELECT * FROM cc WHERE descripcion = {0}";
                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                odbc.consulta = String.Format(strQuery, noEconomico);

                List<BackLogsDTO> lstCC = new List<BackLogsDTO>();
                if (productivo)
                    lstCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                else
                    lstCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                if (lstCC.Count() > 0)
                    cc = "'" + lstCC[0].cc + "'";
                #endregion

                #region SE VERIFICA SI HAY ORDENES DE COMPRA EN BASE AL NÚMERO DE REQUISICIÓN
                List<OrdenCompraDTO> lstOC = new List<OrdenCompraDTO>();
                if (lstRequisiciones.Count() > 0 && !string.IsNullOrEmpty(cc))
                {
                    strQuery = @"SELECT numero, num_requisicion FROM so_orden_compra_det WHERE cc = {0} AND num_requisicion IN ({1})";
                    odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, cc, string.Join(",", lstRequisiciones.Select(x => x.numRequisicion).ToList()));

                    if (productivo)
                        lstOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                }
                #endregion

                if (lstOC.Count > 0)
                    return lstOC.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetAllOrdenesCompra", ex, AccionEnum.CONSULTA, 0, objOC);
                return null;
            }
        }

        public bool EliminarOC(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region SE OBTIENE BACKLOG ID
                    int idBackLog = _context.tblBL_OrdenesCompra.First(x => x.id == id && x.esActivo).idBackLog;
                    #endregion

                    #region SE ELIMINA OC
                    var EliminarOC = _context.tblBL_OrdenesCompra.FirstOrDefault(x => x.id == id);
                    EliminarOC.esActivo = false;
                    _context.SaveChanges();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(EliminarOC));
                    #endregion

                    #region SE ACTUALIZA ESTATUS DEL BACKLOG
                    bool exitoActualizarEstatusBL = ActualizarEstatusBL(idBackLog);
                    if (!exitoActualizarEstatusBL)
                        throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarOC", ex, AccionEnum.ELIMINAR, id, 0);
                    return false;
                }
            }
        }

        public bool ActualizarOC(tblBL_OrdenesCompra objOC)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE BACKLOG ID
                    int idBackLog = _context.tblBL_OrdenesCompra.First(x => x.id == objOC.idBackLog && x.esActivo).idBackLog;
                    #endregion

                    #region SE ACTUALIZA LA ORDEN DE COMPRA DEL BACKLOG
                    var ActualizarOC = _context.tblBL_OrdenesCompra.FirstOrDefault(x => x.id == objOC.id);
                    ActualizarOC.numOC = objOC.numOC;
                    ActualizarOC.fechaModificacionNumOC = DateTime.Now;
                    _context.SaveChanges();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, (int)ActualizarOC.id, JsonUtils.convertNetObjectToJson(objOC));
                    #endregion

                    #region SE ACTUALIZA ESTATUS DEL BACKLOG
                    bool exitoActualizarEstatusBL = ActualizarEstatusBL(idBackLog);
                    if (!exitoActualizarEstatusBL)
                        throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                    #endregion

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarOC", e, AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(objOC));
                    return false;
                }
        }

        public bool CrearOC(List<tblBL_OrdenesCompra> objOC)
        {
            try
            {
                string cc = string.Empty;
                int idBackLog = objOC.Count > 0 ? objOC[0].idBackLog : 0;

                #region SE OBTIENE EL CC DEL BL SELECCIONADO.
                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBackLog && x.esActivo).ToList();
                if (lstBL.Count() > 0)
                    cc = lstBL[0].areaCuenta;
                else
                    throw new Exception("Ocurrió un error al obtener los datos del BackLog seleccionado.");
                #endregion

                List<tblBL_OrdenesCompra> lstOCGuardar = new List<tblBL_OrdenesCompra>();
                List<tblBL_OrdenesCompra> lstOC = _context.tblBL_OrdenesCompra.Where(x => x.esActivo).ToList();
                for (int i = 0; i < objOC.Count(); i++)
                {
                    string numRequisicion = objOC[i].numRequisicion.ToString();
                    string numOC = objOC[i].numOC;

                    List<tblBL_OrdenesCompra> ordenCompraExistente = lstOC.Where(x => x.idBackLog == idBackLog && x.numRequisicion == numRequisicion && x.numOC == numOC && x.esActivo).ToList();
                    if (ordenCompraExistente.Count() <= 0)
                        lstOCGuardar.Add(objOC[i]);
                }

                foreach (var item in lstOCGuardar)
                {
                    item.cc = cc;
                    item.esActivo = true;
                    item.fechaCreacionNumOC = DateTime.Now;
                    item.fechaModificacionNumOC = DateTime.Now;
                }
                _context.tblBL_OrdenesCompra.AddRange(lstOCGuardar);
                _context.SaveChanges();
                SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objOC));

                #region SE ACTUALIZA EL ESTATUS DEL BACKLOG
                bool exitoActualizarEstatusBL = ActualizarEstatusBL(idBackLog);
                if (!exitoActualizarEstatusBL)
                    throw new Exception("Ocurrió un error al actualizar el estatus del BackLog.");
                #endregion

                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearOC", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objOC));
                return false;
            }
        }

        public List<OrdenCompraDTO> GetLstOcReq(OrdenCompraDTO objOC)
        {
            try
            {
                string cc = string.Empty;
                int idBackLog = objOC.idBackLog > 0 ? objOC.idBackLog : 0;

                #region SE OBTIENE LAS REQUISICIONES RELACIONADAS AL BACKLOG.
                List<tblBL_Requisiciones> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.idBackLog == idBackLog && x.esActivo).ToList();
                #endregion

                #region SE OBTIENE EL CC DEL BL
                tblBL_CatBackLogs lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBackLog && x.esActivo).FirstOrDefault();
                if (lstBL != null && !string.IsNullOrEmpty(lstBL.cc))
                    cc = lstBL.cc;
                #endregion

                #region SE VERIFICA SI HAY ORDENES DE COMPRA EN BASE AL LISTADO DE REQUISICIONES. (DETALLE)
                List<OrdenCompraDTO> lstDetOC = new List<OrdenCompraDTO>();
                if (lstRequisiciones.Count() > 0 && !string.IsNullOrEmpty(cc))
                {
                    string strQuery = @"SELECT numero, partida, insumo, fecha_entrega, cantidad, precio, importe, num_requisicion FROM so_orden_compra_det WHERE cc LIKE '%{0}%' AND num_requisicion IN ({1})";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, cc, string.Join(",", lstRequisiciones.Select(x => x.numRequisicion).ToList()));

                    if (productivo)
                        lstDetOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstDetOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                }
                #endregion

                #region SE OBTIENE LA ORDEN DE COMPRA DE LA TABLA PRINCIPAL
                List<OrdenCompraDTO> lstOC = new List<OrdenCompraDTO>();
                if (lstDetOC.Count() > 0)
                {
                    string numOC = lstDetOC[0].numero;
                    string strQuery = @"SELECT numero, moneda, tipo_cambio, porcent_iva, sub_total, iva, total, comentarios FROM so_orden_compra WHERE cc LIKE '%{0}%' AND numero = {1}";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, cc, numOC);

                    if (productivo)
                        lstOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                }
                #endregion

                if (lstOC.Count > 0)
                    return lstOC.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetLstOcReq", ex, AccionEnum.CONSULTA, 0, objOC);
                return null;
            }
        }

        public List<OrdenCompraDTO> GetLstDetOcReq(OrdenCompraDTO objOC)
        {
            try
            {
                string cc = string.Empty;
                int idBackLog = objOC.idBackLog > 0 ? objOC.idBackLog : 0;

                #region SE OBTIENE LAS REQUISICIONES RELACIONADAS AL BACKLOG.
                List<tblBL_Requisiciones> lstRequisiciones = _context.tblBL_Requisiciones.Where(x => x.idBackLog == idBackLog && x.esActivo).ToList();
                #endregion

                #region SE OBTIENE EL CC DEL BL
                tblBL_CatBackLogs lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBackLog && x.esActivo).FirstOrDefault();
                if (lstBL != null && !string.IsNullOrEmpty(lstBL.cc))
                    cc = lstBL.cc;
                #endregion

                #region SE VERIFICA SI HAY ORDENES DE COMPRA EN BASE AL LISTADO DE REQUISICIONES. (DETALLE)
                List<OrdenCompraDTO> lstDetOC = new List<OrdenCompraDTO>();
                if (lstRequisiciones.Count() > 0 && !string.IsNullOrEmpty(cc))
                {
                    string strQuery = @"SELECT numero, partida, insumo, fecha_entrega, cantidad, precio, importe, num_requisicion FROM so_orden_compra_det WHERE cc LIKE '%{0}%' AND num_requisicion IN ({1})";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, cc, string.Join(",", lstRequisiciones.Select(x => x.numRequisicion).ToList()));

                    if (productivo)
                        lstDetOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstDetOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                }
                #endregion

                if (lstDetOC.Count > 0)
                    return lstDetOC.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetLstDetOcReq", ex, AccionEnum.CONSULTA, 0, objOC);
                return null;
            }
        }

        public bool GuardarOC(OrdenCompraDTO objOC)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int idBackLog = objOC.idBackLog > 0 ? objOC.idBackLog : 0;
                    string numOC = !string.IsNullOrEmpty(objOC.numero) ? objOC.numero : "";
                    string cc = string.Empty;

                    #region SE OBTIENE EL CC DEL BL
                    tblBL_CatBackLogs lstBL = _context.tblBL_CatBackLogs.Where(x => x.id == idBackLog && x.esActivo).FirstOrDefault();
                    if (lstBL != null && !string.IsNullOrEmpty(lstBL.cc))
                        cc = lstBL.cc;
                    #endregion

                    #region SE OBTIENE EL NUMERO DE REQUISICIÓN QUE CONTIENE LA OC.
                    List<OrdenCompraDTO> lstDetOC = new List<OrdenCompraDTO>();
                    if (!string.IsNullOrEmpty(cc) && !string.IsNullOrEmpty(numOC))
                    {
                        string strQuery = @"SELECT TOP 1 num_requisicion FROM so_orden_compra_det WHERE cc LIKE '%{0}%' AND numero = '{1}'";
                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                        odbc.consulta = String.Format(strQuery, cc, numOC);

                        if (productivo)
                            lstDetOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc);
                        else
                            lstDetOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                    }
                    #endregion

                    tblBL_OrdenesCompra objGuardar = new tblBL_OrdenesCompra();
                    objGuardar.cc = cc.Trim();
                    objGuardar.numRequisicion = lstDetOC[0].num_requisicion;
                    objGuardar.numOC = numOC;
                    objGuardar.idBackLog = idBackLog;
                    objGuardar.esActivo = true;
                    objGuardar.fechaCreacionNumOC = DateTime.Now;
                    objGuardar.fechaModificacionNumOC = DateTime.Now;
                    _context.tblBL_OrdenesCompra.Add(objGuardar);
                    _context.SaveChanges();

                    ActualizarEstatusBL(idBackLog);

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GuardarOC", ex, AccionEnum.CONSULTA, 0, objOC);
                    return false;
                }
            }
        }

        public bool VerificarDuplicadoOC(OrdenCompraDTO objOC)
        {
            try
            {
                int idBL = objOC.idBackLog;
                string numOC = objOC.numero;
                string cc = string.Empty;

                #region SE OBTIENE EL CC DEL BACKLOG SELECCIONADO.
                tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.id == idBL);
                if (objBL != null)
                    cc = objBL.cc;
                #endregion

                #region SE VERIFICA SI LA ORDEN DE COMPRA A GUARDAR, EXISTA EN EL BL
                int existeOC = _context.tblBL_OrdenesCompra.Where(x => x.cc == cc && x.numOC == numOC && x.esActivo).Count();
                if (existeOC > 0)
                    return true;
                else
                    return false;
                #endregion
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "VerificarDuplicadoOC", ex, AccionEnum.CONSULTA, 0, objOC);
                return false;
            }
        }
        #endregion

        #region PROGRAMA DE INSPECCIÓN
        private decimal GetMaquinasAInspeccion(string noEconomico, List<tblM_CapHorometro> lstHorometros, List<tblBL_Inspecciones> lstHorometrosInsp)
        {
            try
            {
                List<tblBL_Inspecciones> lstUltimoHorometroInsp = lstHorometrosInsp.Where(x => x.noEconomico == noEconomico && x.esActivo).OrderByDescending(x => x.id).ToList(); //ULTIMO HOROMETRO YA REGISTRADO EN INSPECCIÓN.
                List<tblM_CapHorometro> lstUltimoHorometroCaptura = lstHorometros.Where(x => x.Economico == noEconomico).OrderByDescending(x => x.id).ToList();
                if (lstUltimoHorometroInsp.Count() > 0)
                {
                    #region SI EL ECONOMICO YA TUVO ALGUNA INSPECCIÓN, SE VERIFICA SI TIENE 250 HORAS DE DIFERENCIA AL ULTIMO HOROMETRO DE LA INSPECCIÓN A LA ULTIMA CAPTURA DEL HOROMETRO EN tblM_CapHorometro.
                    //List<tblM_CapHorometro> lstUltimoHorometroCapturaOrderByTurno = lstUltimoHorometroCaptura.OrderByDescending(x => x.turno).ToList();

                    tblBL_Inspecciones ultimoHorometroInsp = lstUltimoHorometroInsp[0];
                    tblM_CapHorometro ultimoHorometroCaptura = lstUltimoHorometroCaptura[0];

                    if (ultimoHorometroInsp.horometro <= 0)
                        return ultimoHorometroInsp.horometro + 250;
                    else
                    {
                        decimal horometro = ultimoHorometroCaptura.Horometro - ultimoHorometroInsp.horometro;
                        return horometro;
                    }
                    #endregion
                }
                else
                {
                    #region SI EL ECONOMICO ES SU PRIMERA INSPECCIÓN, SE RESTA 250 HORAS DE SU ULTIMO HOROMETRO EN LA CAPTURA DEL HOROMETRO EN tblM_CapHorometro.
                    List<tblM_CapHorometro> lstUltimoHorometroCapturaOrderByTurno = lstUltimoHorometroCaptura.OrderByDescending(x => x.turno).ToList();

                    tblM_CapHorometro ultimoHorometro = lstUltimoHorometroCaptura[0];

                    decimal horometro = 250 - ultimoHorometro.Horometro;
                    return horometro;
                    #endregion
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetMaquinasAInspeccion", e, AccionEnum.CONSULTA, 0, 0);
                return 0;
            }
        }

        private bool GetMaquinasAInspeccionDias(string noEconomico, List<tblBL_Inspecciones> lstInspecciones, int periodo)
        {
            try
            {
                List<tblBL_Inspecciones> fechaPorEconomico = lstInspecciones.Where(x => x.noEconomico == noEconomico).OrderByDescending(s => s.id).ToList();

                if (fechaPorEconomico.Count() > 0)
                {
                    DateTime fechaUltInspeccion = fechaPorEconomico[0].fechaInspRealizada;
                    DateTime fechaActual = DateTime.Now;

                    TimeSpan difFechas = fechaActual.Date - fechaUltInspeccion.Date;
                    int dias = difFechas.Days;

                    int esPeriodoActual = fechaPorEconomico.Where(x => x.periodo == periodo && x.esActivo).Count();

                    if (dias >= 30 || esPeriodoActual > 0)
                        return true;
                }
                return false;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetMaquinasAInspeccionDias", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public List<InspeccionesDTO> GetInspecciones(InspeccionesDTO objParamsDTO)
        {
            try
            {
                #region SE OBTIENE LISTADO DE MAQUINAS A INSPECCIONAR
                if (!objParamsDTO.esPuroPrograma)
                {
                    /* MAQUINAS EN PROGRAMACION */
                    #region SE OBTIENE LAS MAQUINAS SELECCIONADAS EN EL FILTRO
                    List<tblM_CatMaquina> lstMaquinas = _context.tblM_CatMaquina.Where(x => objParamsDTO.lstNoEconomicosID.Contains(x.id) && x.centro_costos == objParamsDTO.areaCuenta && x.estatus == 1).ToList();
                    #endregion

                    #region SE OBTIENE LA CAPTURA DEL HOROMETRO DE CADA MAQUINA CONSULTADA EN EL FILTRO
                    List<string> lstEconomicosStr = new List<string>();
                    foreach (var item in lstMaquinas)
                    {
                        lstEconomicosStr.Add(item.noEconomico);
                    }
                    List<tblM_CapHorometro> lstHorometros = _context.tblM_CapHorometro.Where(x => lstEconomicosStr.Contains(x.Economico)).ToList();
                    List<tblBL_Inspecciones> lstHorometrosInsp = _context.tblBL_Inspecciones.Where(x => lstEconomicosStr.Contains(x.noEconomico) && x.esActivo && x.fechaInspRealizada.Year == DateTime.Now.Year).ToList();
                    #endregion

                    #region SE VERIFICA QUE MAQUINA HA SUPERADO O ES IGUAL A 250 HORAS DE DIFERENCIA EN SUS ULTIMOS DOS REGISTROS DE HOROMETROS.
                    List<InspeccionesDTO> lstMaquinasSuperior250Horas = new List<InspeccionesDTO>();
                    if ((lstHorometros.Count() > 0 && objParamsDTO.tipoEquipoID == (int)TipoMaquinaEnum.Mayor) || lstHorometros.Count() > 0)
                    {
                        List<InspeccionesDTO> lstMaquinasAInspeccion = new List<InspeccionesDTO>();
                        lstMaquinasAInspeccion = lstMaquinas.Select(x => new InspeccionesDTO
                        {
                            id = 0,
                            horometro = GetMaquinasAInspeccion(x.noEconomico, lstHorometros, lstHorometrosInsp), // SI ES IGUAL A 250 O MAS, ENTRA A INSPECCIÓN.
                            noEconomico = x.noEconomico,
                            descripcion = x.descripcion,
                            horasRestantes = x.horometroActual,
                            fechaInicioInsp = DateTime.Now,
                            fechaFinalInsp = DateTime.Now,
                            fechaInspRealizada = DateTime.Now,
                            cantBackLogs = 0
                        }).ToList();
                        lstMaquinasSuperior250Horas = lstMaquinasAInspeccion.ToList();
                    }
                    else
                    {
                        List<InspeccionesDTO> lstMaquinasAInspeccion = new List<InspeccionesDTO>();
                        lstMaquinasAInspeccion = lstMaquinas.Select(x => new InspeccionesDTO
                        {
                            id = 0,
                            horometro = 0,
                            noEconomico = x.noEconomico,
                            descripcion = x.descripcion,
                            horasRestantes = x.horometroActual,
                            fechaInicioInsp = DateTime.Now,
                            fechaFinalInsp = DateTime.Now,
                            fechaInspRealizada = DateTime.Now,
                            cantBackLogs = 0
                        }).ToList();
                        lstMaquinasSuperior250Horas = lstMaquinasAInspeccion.ToList();
                    }
                    #endregion
                    /* MAQUINAS EN PROGRAMACION */

                    /* MAQUINAS EN PROGRAMA */
                    #region SE OBTIENE LAS MAQUINAS QUE SE ENCUENTRAN EN PROGRAMA
                    tblC_Nom_CatPeriodo objPeriodo = _context.tblC_Nom_CatPeriodo.Where(w => w.tipoNomina == (int)TipoNominaEnum.semanal && w.anio == objParamsDTO.anio).FirstOrDefault();
                    if (objPeriodo == null)
                        throw new Exception("Ocurrió un error al obtener las fechas que conlleva el periodo seleccionado.");

                    string fechaInicio = objPeriodo.fechaInicio.ToShortDateString();
                    string fechaFin = objPeriodo.fechaFin.ToShortDateString();

                    string strQuery = string.Format(@"SELECT id, areaCuenta, periodo, idGrupo, noEconomico, horometro, idCatMaquina, fechaInicioInsp, fechaFinalInsp, fechaInspRealizada, cantBackLogs 
                                                            FROM tblBL_Inspecciones 
                                                                WHERE esActivo = {0} AND periodo = {1} AND areaCuenta = '{2}' AND fechaInicioInsp = (CONVERT(DATE, '{4}')) AND fechaFinalInsp = (CONVERT(DATE, '{5}'))",
                                                                        1, objParamsDTO.periodo, objParamsDTO.areaCuenta, objParamsDTO.anio, fechaInicio, fechaFin);
                    List<tblBL_Inspecciones> lstInspecciones = _context.Select<tblBL_Inspecciones>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();
                    #endregion

                    #region SE VERIFICA QUÉ MAQUINA HA SUPERADO O ES IGUAL A LOS 30 DÍAS PARA MANDAR LA MAQUINA A INSPECCIÓN.
                    List<InspeccionesDTO> lstMaquinasAInspeccion30Dias = new List<InspeccionesDTO>();
                    if (lstInspecciones.Count() > 0)
                    {
                        lstMaquinasAInspeccion30Dias = lstInspecciones.Select(x => new InspeccionesDTO
                        {
                            id = x.id,
                            mandarInspeccion = lstInspecciones.Count() > 0 ? GetMaquinasAInspeccionDias(x.noEconomico, lstInspecciones, objParamsDTO.periodo) : false, // SI YA TRANSCURRIERON 30 DÍAS DE LA ULTIMA INSPECCIÓN, SE MANDA A INSPECCIÓN.
                            noEconomico = x.noEconomico,
                            descripcion = GetDescripcionNoEconomico(x.noEconomico, lstMaquinas),
                            horometro = x.horometro,
                            fechaInicioInsp = lstInspecciones.Where(w => w.noEconomico == x.noEconomico).OrderByDescending(o => o.id).FirstOrDefault().fechaInicioInsp,
                            fechaFinalInsp = lstInspecciones.Where(w => w.noEconomico == x.noEconomico).OrderByDescending(o => o.id).FirstOrDefault().fechaFinalInsp,
                            fechaInspRealizada = lstInspecciones.Where(w => w.noEconomico == x.noEconomico).OrderByDescending(o => o.id).FirstOrDefault().fechaInspRealizada,
                            cantBackLogs = lstInspecciones.Where(w => w.noEconomico == x.noEconomico).OrderByDescending(o => o.id).FirstOrDefault().cantBackLogs,
                            periodo = x.periodo
                        }).ToList();
                    }
                    /* MAQUINAS EN PROGRAMA */
                    #endregion

                    List<string> lstMaquinasSuperior250HorasStr = new List<string>();
                    foreach (var item in lstMaquinasSuperior250Horas)
                    {
                        lstMaquinasSuperior250HorasStr.Add(item.noEconomico);
                    }

                    int prioridad = 1;
                    List<InspeccionesDTO> lstMaquinasMostrar = new List<InspeccionesDTO>();
                    lstMaquinasMostrar.AddRange(lstMaquinasAInspeccion30Dias.Where(x => x.mandarInspeccion && x.periodo == objParamsDTO.periodo).ToList());
                    List<string> lstMaquinasNoEconomicosMostrar = new List<string>();
                    foreach (var item in lstMaquinasMostrar)
                    {
                        lstMaquinasNoEconomicosMostrar.Add(item.noEconomico);
                    }
                    lstMaquinasMostrar.AddRange(lstMaquinasSuperior250Horas.Where(x => !lstMaquinasNoEconomicosMostrar.Contains(x.noEconomico)).ToList());
                    lstMaquinasMostrar.ToList();
                    foreach (var item in lstMaquinasMostrar.OrderBy(x => x.horometro).ToList())
                    {
                        item.prioridad = prioridad++;
                    }
                    return lstMaquinasMostrar;
                }
                else
                {
                    int prioridad = 1;

                    // SE OBTIENE LA FECHA INICIAL Y FECHA FINAL EN BASE AL PERIODO Y AÑO SELECCIONADO
                    tblC_Nom_CatPeriodo objPeriodo = _context.tblC_Nom_CatPeriodo.Where(w => w.periodo == objParamsDTO.periodo && w.tipoNomina == (int)TipoNominaEnum.semanal && w.anio == objParamsDTO.anio).FirstOrDefault();
                    if (objPeriodo == null)
                        throw new Exception("Ocurrió un error al obtener las fechas que conlleva el periodo seleccionado.");

                    string fechaInicio = objPeriodo.fechaInicio.ToShortDateString();
                    string fechaFin = objPeriodo.fechaFin.ToShortDateString();

                    string strQuery = string.Format(@"SELECT id, areaCuenta, periodo, idGrupo, noEconomico, horometro, idCatMaquina, fechaInicioInsp, fechaFinalInsp, fechaInspRealizada, cantBackLogs 
                                                            FROM tblBL_Inspecciones 
                                                                WHERE esActivo = {0} AND periodo = {1} AND areaCuenta = '{2}' AND fechaInicioInsp = (CONVERT(DATE, '{4}')) AND fechaFinalInsp = (CONVERT(DATE, '{5}'))",
                                                                        1, objParamsDTO.periodo, objParamsDTO.areaCuenta, objParamsDTO.anio, fechaInicio, fechaFin);
                    List<InspeccionesDTO> lstMaquinasEnPrograma = _context.Select<InspeccionesDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    if (objParamsDTO.lstNoEconomicosID.Count() > 0)
                        lstMaquinasEnPrograma = lstMaquinasEnPrograma.Where(w => objParamsDTO.lstNoEconomicosID.Contains(w.idCatMaquina)).ToList();

                    foreach (var item in lstMaquinasEnPrograma.OrderBy(x => x.horometro).ToList())
                    {
                        item.prioridad = prioridad++;
                    }
                    return lstMaquinasEnPrograma;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetInspecciones", e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                return null;
            }
        }

        public bool EliminarInspeccionObra(int id)
        {
            try
            {
                tblBL_Inspecciones objEliminar = _context.tblBL_Inspecciones.First(x => x.id == id && x.esActivo);
                objEliminar.esActivo = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarInspeccionObra", e, AccionEnum.CONSULTA, 0, id);
                return false;
            }
        }

        private decimal GetHorasRestantesNuevaInspeccion(List<tblM_CapHorometro> dataCapHorometro, string noEconomico)
        {
            try
            {
                List<tblM_CapHorometro> existeHorometro = dataCapHorometro.Where(x => x.Economico == noEconomico).ToList();
                if (existeHorometro.Count() > 0)
                {
                    List<tblM_CapHorometro> lstHorometroActual = dataCapHorometro.Where(x => x.Economico == noEconomico).OrderByDescending(x => x.Fecha).ToList();
                    if (lstHorometroActual.Count() > 0)
                    {
                        decimal horometroActual = lstHorometroActual[0].Horometro;
                        decimal horasRestantes = (500 - horometroActual);
                        return horasRestantes;
                    }
                }
                return 500;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetHorasRestantes", e, AccionEnum.CONSULTA, 0, 0);
                return 0;
            }
        }

        private string GetDescripcionNoEconomico(string noEconomico, List<tblM_CatMaquina> lstCatMaquinas)
        {
            try
            {
                string descripcion = string.Empty;
                if (!string.IsNullOrEmpty(noEconomico))
                    descripcion = lstCatMaquinas.First(x => x.noEconomico == noEconomico.Trim()).descripcion;
                else
                    descripcion = "-";

                return descripcion;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDescripcionNoEconomico", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public List<ComboDTO> FillCboNoEconomico(string areaCuenta, List<int> lstGrupos)
        {
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(areaCuenta)) { throw new Exception("Es necesario seleccionar un área cuenta."); }
                if (lstGrupos == null) { throw new Exception("Es necesario seleccionar al menos un grupo."); }
                #endregion

                #region SE OBTIENE LISTADO DE ECONOMICOS EN BASE AL AREA CUENTA Y LISTADO DE GRUPOS SELECCIONADOS
                List<tblM_CatMaquina> lstNoEconomicos = _context.tblM_CatMaquina.Where(w => w.centro_costos == areaCuenta && lstGrupos.Contains(w.grupoMaquinariaID) && w.estatus == 1).OrderBy(o => o.noEconomico).ToList();

                List<ComboDTO> lstNoEconomicosComboDTO = new List<ComboDTO>();
                ComboDTO objNoEconomicoComboDTO = new ComboDTO();
                foreach (var item in lstNoEconomicos)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.noEconomico))
                    {
                        objNoEconomicoComboDTO = new ComboDTO();
                        objNoEconomicoComboDTO.Value = item.id.ToString();
                        objNoEconomicoComboDTO.Text = item.noEconomico.Trim().ToUpper();
                        lstNoEconomicosComboDTO.Add(objNoEconomicoComboDTO);
                    }
                }
                return lstNoEconomicosComboDTO;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboNoEconomico", e, AccionEnum.FILLCOMBO, 0, new { areaCuenta = areaCuenta, lstGrupos = lstGrupos });
                return null;
            }
        }

        public List<ComboDTO> FillPeriodos(int anio)
        {
            try
            {
                #region VALIDACIONES
                if (anio <= 0) { throw new Exception("Es necesario seleccionar un año."); }
                #endregion

                #region SE OBTIENE LISTADO DE LOS PERIODOS EN BASE AL AÑO
                List<PeriodosDTO> lstPeriodos = _context.Select<PeriodosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT periodo, fechaInicio AS fecha_inicial, fechaFin AS fecha_final FROM tblC_Nom_CatPeriodo WHERE tipoNomina = @tipoNomina AND anio = @anio ORDER BY periodo",
                    parametros = new { tipoNomina = (int)TipoNominaEnum.semanal, anio = anio }
                }).ToList();

                List<ComboDTO> lstPeriodosDTO = lstPeriodos.Select(x => new ComboDTO
                {
                    Text = string.Format("Semana {0}", x.periodo), // "Semana " + x.periodo,
                    Value = x.periodo,
                    Prefijo = string.Format("{0}|{1}", x.fecha_inicial.ToShortDateString(), x.fecha_final.ToShortDateString())
                }).ToList();

                return lstPeriodosDTO;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillPeriodos", e, AccionEnum.FILLCOMBO, 0, anio);
                return null;
            }
        }

        public int GetPeriodoActual()
        {
            try
            {
                #region VERSION EK
                //DateTime fechaActual = DateTime.Now;
                //int dd = fechaActual.Day;
                //int MM = fechaActual.Month;
                //int yyyy = fechaActual.Year;
                //string fechaInicial = "'" + yyyy + '-' + MM + '-' + dd + "'";
                //string strQuery = string.Empty;
                //strQuery += "SELECT periodo, fecha_inicial, fecha_final FROM DBA.sn_periodos WHERE year = {0} AND tipo_nomina = {1} AND ";
                //strQuery += "fecha_inicial <= {2} AND fecha_final >= {2}";
                //var odbc = new OdbcConsultaDTO();
                //odbc.consulta = String.Format(strQuery, DateTime.Now.Year, 1, fechaInicial);
                //List<PeriodosDTO> lstPeriodos = _contextEnkontrol.Select<PeriodosDTO>(EnkontrolEnum.CplanRh, odbc);

                //if (lstPeriodos.Count() > 0)
                //{
                //    int periodoActual = Convert.ToInt32(lstPeriodos[0].periodo);
                //    return periodoActual;
                //}
                //else
                //    throw new Exception("Ocurrió un error al obtener el periodo actual.");
                #endregion

                #region SE OBTIENE EL PERIODO ACTUAL
                DateTime fechaActual = DateTime.Now;
                int dd = fechaActual.Day;
                int MM = fechaActual.Month;
                int yyyy = fechaActual.Year;
                DateTime fechaInicial = new DateTime(yyyy, MM, dd);//"'" + yyyy + '-' + MM + '-' + dd + "'";
                //List<PeriodosDTO> lstPeriodos = _context.Select<PeriodosDTO>(new DapperDTO
                //{
                //    baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : (MainContextEnum)vSesiones.sesionEmpresaActual,
                //    consulta = @"SELECT periodo, fechaInicio, fechaFin FROM tblC_Nom_CatPeriodo WHERE anio = @anio AND tipoNomina = @tipoNomina AND fechaInicio <= @fechaInicio AND fechaFin >= @fechaInicio",
                //    parametros = new { anio = DateTime.Now, tipoNomina = (int)TipoNominaEnum.semanal, fechaInicio = fechaInicial }
                //}).ToList();

                List<tblC_Nom_CatPeriodo> lstPeriodos = _context.tblC_Nom_CatPeriodo.Where(w => w.anio == DateTime.Now.Year && w.tipoNomina == (int)TipoNominaEnum.semanal && w.fechaInicio <= fechaInicial && w.fechaFin >= fechaInicial).ToList();

                if (lstPeriodos.Count() > 0)
                {
                    int periodoActual = Convert.ToInt32(lstPeriodos[0].periodo);
                    return periodoActual;
                }
                else
                    throw new Exception("Ocurrió un error al obtener el periodo actual.");
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetPeriodoActual", e, AccionEnum.CONSULTA, 0, 0);
                return 0;
            }
        }

        public bool GuardarInspecciones(List<tblBL_Inspecciones> objInspecciones)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    List<tblM_CatMaquina> lstCatMaquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 1).ToList();
                    foreach (var item in objInspecciones)
                    {
                        item.idCatMaquina = lstCatMaquinas.FirstOrDefault(x => x.noEconomico == item.noEconomico).id;
                        item.idGrupo = lstCatMaquinas.FirstOrDefault(x => x.noEconomico == item.noEconomico).grupoMaquinariaID;
                        item.fechaCreacionInsp = DateTime.Now;
                        item.fechaModificacionInsp = DateTime.Now;
                        item.esActivo = true;
                    }
                    _context.tblBL_Inspecciones.AddRange(objInspecciones);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objInspecciones));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GuardarInspecciones", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objInspecciones));
                    return false;
                }
                return true;
            }
        }

        public bool ActualizarInspeccion(tblBL_Inspecciones objInspecciones)
        {
            try
            {
                tblBL_Inspecciones objActualizar = _context.tblBL_Inspecciones.FirstOrDefault(x => x.id == objInspecciones.id);
                objActualizar.fechaInspRealizada = objInspecciones.fechaInspRealizada;
                objActualizar.cantBackLogs = objInspecciones.cantBackLogs;
                objActualizar.fechaModificacionInsp = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarInspeccion", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objInspecciones));
                return false;
            }
        }

        public List<InspeccionesObraExcelDTO> postObtenerTablaInspecciones(InspeccionesDTO parametros, bool ActivarHeader)
        {
            try
            {
                #region DECLARACION DE OBJETOS Y LISTAS
                List<InspeccionesObraExcelDTO> lstRetornar = new List<InspeccionesObraExcelDTO>();

                #endregion
                #region Agregar TABLA Header
                if (ActivarHeader == true)
                {

                }
                #endregion
                return lstRetornar;

            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, "", "postObtenerTablaHorasHombre", e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public MemoryStream GenerarExcelInspeccionesObras(BackLogsDTO objParamsDTO)
        {
            MemoryStream bytes = new MemoryStream();
            try
            {
                using (var _context = new MainContext(vSesiones.sesionEmpresaActual))
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objParamsDTO.noEconomico)) { throw new Exception("Es necesario seleccionar un CC."); }
                    if (objParamsDTO.anio <= 0) { throw new Exception("Es necesario seleccionar un año."); }
                    if (string.IsNullOrEmpty(objParamsDTO.areaCuenta)) { throw new Exception("Es necesario seleccionar un área cuenta."); }
                    #endregion

                    #region INIT INFORMACIÓN
                    List<BackLogsDTO> lstBackLogsDTO = GetListaBackLogs(new BackLogsDTO { noEconomico = objParamsDTO.noEconomico, areaCuenta = objParamsDTO.areaCuenta, anio = objParamsDTO.anio }).ToList();
                    List<int> lstBackLogsID = lstBackLogsDTO.Select(s => s.id).ToList();
                    if (lstBackLogsID.Count() <= 0)
                        throw new Exception("No se encuentran BackLogs en base a los filtros seleccioandos.");

                    CapHorometroBLDTO objUltimoHorometroDTO = GetListaCapHorometros(new CapHorometroBLDTO { Economico = objParamsDTO.noEconomico }).OrderByDescending(o => o.Horometro).FirstOrDefault();
                    List<ParteDTO> lstPartesDTO = GetListaPartes(new ParteDTO { lstBackLogsID = lstBackLogsID }).ToList();
                    List<RequisicionesDTO> lstRequisicionesDTO = GetListaRequisiciones().ToList();
                    List<CatMaquinaDTO> lstCatMaquinasDTO = GetListaCatMaquinasDTO().ToList();
                    List<CatModeloEquipoDTO> lstCatModelosEquiposDTO = GetListaCatModeloEquipo().ToList();

                    foreach (var item in lstBackLogsDTO)
                    {
                        item.descripcionBL = PersonalUtilities.PrimerLetraMayuscula(item.descripcionBL);
                        item.subconjunto = PersonalUtilities.PrimerLetraMayuscula(item.subconjunto);

                        Dictionary<string, object> resGetTotalPrecioBL = GetTotalPrecioBL(item, lstRequisicionesDTO);
                        if (!(bool)resGetTotalPrecioBL[SUCCESS])
                            throw new Exception((string)resGetTotalPrecioBL[MESSAGE]);
                        else
                        {
                            item.totalMX = (decimal)resGetTotalPrecioBL["totalMX"];
                            item.totalUSD = (decimal)resGetTotalPrecioBL["totalUSD"];
                        }
                    }
                    #endregion

                    Color colorDeCelda = ColorTranslator.FromHtml("#fff");
                    Color colorDeCeldas = ColorTranslator.FromHtml("#000");
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        var hoja1 = excel.Workbook.Worksheets.Add("Inspecciones");

                        #region HEADER DE EL EXCEL
                        List<string[]> Direccion = new List<string[]>() { new string[] { "Dirección de maquinaria y equipo" } };
                        string tituloDireccion = "M1:" + "P1";
                        hoja1.Cells[tituloDireccion].Merge = true;
                        hoja1.Cells[tituloDireccion].Style.Font.Bold = true;
                        hoja1.Cells[tituloDireccion].LoadFromArrays(Direccion);
                        hoja1.Cells[tituloDireccion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> informe = new List<string[]>() { new string[] { "Informe de rehabilitación" } };
                        string tituloInforme = "M2:" + "P2";
                        hoja1.Cells[tituloInforme].Merge = true;
                        hoja1.Cells[tituloInforme].Style.Font.Bold = true;
                        hoja1.Cells[tituloInforme].Style.Font.Size = 20;
                        hoja1.Cells[tituloInforme].LoadFromArrays(informe);
                        hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Row(2).Height = 24;

                        List<string[]> cc = new List<string[]>() { new string[] { "Centro de costo" } };
                        string titulorcc = "L4:" + "N4";
                        hoja1.Cells[titulorcc].Merge = true;
                        hoja1.Cells[titulorcc].LoadFromArrays(cc);
                        hoja1.Cells[titulorcc].Style.Font.Bold = true;
                        hoja1.Cells[titulorcc].Style.Font.Size = 14;
                        hoja1.Cells[titulorcc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        List<string[]> xx = new List<string[]>() { new string[] { objParamsDTO.areaCuenta } };

                        string XX = "O4:" + "O4";
                        hoja1.Cells[XX].LoadFromArrays(xx);
                        List<string[]> linecc = new List<string[]>() { new string[] { "" } };

                        string lineCC = "O4:" + "O4";
                        hoja1.Cells[lineCC].Merge = true;
                        hoja1.Cells[lineCC].LoadFromArrays(linecc);
                        hoja1.Cells[lineCC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[lineCC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> DatosMaquina = new List<string[]>() { new string[] { "Datos de la maquina" } };
                        string datos = "N6:" + "O6";
                        hoja1.Cells[datos].Merge = true;
                        hoja1.Cells[datos].LoadFromArrays(DatosMaquina);
                        hoja1.Cells[datos].Style.Font.Bold = true;
                        hoja1.Cells[datos].Style.Font.Size = 12;
                        hoja1.Cells[datos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> descripcionMaquin = new List<string[]>() { new string[] { "Descripción" } };
                        string maquina = "N7:" + "O7";
                        hoja1.Cells[maquina].Merge = true;
                        hoja1.Cells[maquina].LoadFromArrays(descripcionMaquin);
                        hoja1.Cells[maquina].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> lineDesc = new List<string[]>() { new string[] { "" } };
                        string linedesc = "P7:" + "U7";
                        hoja1.Cells[linedesc].Merge = true;
                        hoja1.Cells[linedesc].LoadFromArrays(lineDesc);
                        hoja1.Cells[linedesc].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[linedesc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> DescripcionM = new List<string[]>() { new string[] { lstBackLogsDTO[0].descripcionMaquina } };
                        string descripcionm = "P7:" + "U7";
                        hoja1.Cells[descripcionm].LoadFromArrays(DescripcionM);

                        List<string[]> NoSerie = new List<string[]>() { new string[] { "N° Serie" } };
                        string serie = "N8:" + "O8";
                        hoja1.Cells[serie].Merge = true;
                        hoja1.Cells[serie].LoadFromArrays(NoSerie);
                        hoja1.Cells[serie].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> lineSerie = new List<string[]>() { new string[] { "" } };
                        string lineserie = "P8:" + "U8";
                        hoja1.Cells[lineserie].Merge = true;
                        hoja1.Cells[lineserie].LoadFromArrays(lineSerie);
                        hoja1.Cells[lineserie].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[lineserie].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> serieM = new List<string[]>() { new string[] { lstBackLogsDTO[0].noSerie } };
                        string seriem = "P8:" + "U8";
                        hoja1.Cells[seriem].LoadFromArrays(serieM);
                        #endregion

                        #region LADO IZQUIERDO
                        List<string[]> NoEco = new List<string[]>() { new string[] { "N° Eco: " } };
                        string ECO = "E6:" + "E6";
                        hoja1.Cells[ECO].Merge = true;
                        hoja1.Cells[ECO].LoadFromArrays(NoEco);

                        List<string[]> lineNoEco = new List<string[]>() { new string[] { "" } };
                        string lineECO = "F6:" + "H6";
                        hoja1.Cells[lineECO].Merge = true;
                        hoja1.Cells[lineECO].LoadFromArrays(lineNoEco);
                        hoja1.Cells[lineECO].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> nECO = new List<string[]>() { new string[] { objParamsDTO.noEconomico } };
                        string neco = "F6:" + "H6";
                        hoja1.Cells[neco].LoadFromArrays(nECO);
                        hoja1.Cells[neco].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> mods = new List<string[]>() { new string[] { "Modelo:" } };
                        string mod = "E7:" + "E7";
                        hoja1.Cells[mod].Merge = true;
                        hoja1.Cells[mod].LoadFromArrays(mods);

                        List<string[]> lineMod = new List<string[]>() { new string[] { "" } };
                        string linemod = "F7:" + "H7";
                        hoja1.Cells[linemod].Merge = true;
                        hoja1.Cells[linemod].LoadFromArrays(lineMod);
                        hoja1.Cells[linemod].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> nModelo = new List<string[]>() { new string[] { lstBackLogsDTO[0].modelo } };
                        string nmodelo = "F7:" + "H7";
                        hoja1.Cells[nmodelo].LoadFromArrays(nModelo);
                        hoja1.Cells[nmodelo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> Frente = new List<string[]>() { new string[] { "Frente:" } };
                        string PosicionFrente = "E8:" + "E8";
                        hoja1.Cells[PosicionFrente].Merge = true;
                        hoja1.Cells[PosicionFrente].LoadFromArrays(Frente);

                        List<string[]> lineFrente = new List<string[]>() { new string[] { "" } };
                        string linefrente = "F8:" + "H8";
                        hoja1.Cells[linefrente].Merge = true;
                        hoja1.Cells[linefrente].LoadFromArrays(lineFrente);
                        hoja1.Cells[linefrente].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[linefrente].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> nFrente = new List<string[]>() { new string[] { lstBackLogsDTO[0].frente } };
                        string nfrente = "F8:" + "H8";
                        hoja1.Cells[nfrente].LoadFromArrays(nFrente);
                        hoja1.Cells[nfrente].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        #endregion

                        #region LADO DERECHO
                        List<string[]> Fecha = new List<string[]>() { new string[] { "Fecha: " } };
                        string fecha = "AC4:" + "AC4";
                        hoja1.Cells[fecha].Merge = true;
                        hoja1.Cells[fecha].LoadFromArrays(Fecha);

                        List<string[]> lineFecha = new List<string[]>() { new string[] { "" } };
                        string linefecha = "AD4:" + "AF4";
                        hoja1.Cells[linefecha].Merge = true;
                        hoja1.Cells[linefecha].LoadFromArrays(lineFecha);
                        hoja1.Cells[linefecha].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> nFecha = new List<string[]>() { new string[] { DateTime.Now.ToString() } };
                        string nfecha = "AD4:" + "AF4";
                        hoja1.Cells[nfecha].LoadFromArrays(nFecha);
                        hoja1.Cells[nfecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> Marca = new List<string[]>() { new string[] { "Marca: " } };
                        string marca = "AC7:" + "AC7";
                        hoja1.Cells[marca].Merge = true;
                        hoja1.Cells[marca].LoadFromArrays(Marca);

                        List<string[]> lineMarca = new List<string[]>() { new string[] { "" } };
                        string linemarca = "AD7:" + "AF7";
                        hoja1.Cells[linemarca].Merge = true;
                        hoja1.Cells[linemarca].LoadFromArrays(lineFecha);
                        hoja1.Cells[linemarca].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[linemarca].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> nMarca = new List<string[]>() { new string[] { lstBackLogsDTO[0].noSerie } };
                        string nmarca = "AD7:" + "AF7";
                        hoja1.Cells[nmarca].LoadFromArrays(nMarca);

                        List<string[]> Horometro = new List<string[]>() { new string[] { "Horometro: " } };
                        string horometro = "AC8:" + "AC8";
                        hoja1.Cells[horometro].Merge = true;
                        hoja1.Cells[horometro].LoadFromArrays(Horometro);

                        List<string[]> lineHorometro = new List<string[]>() { new string[] { "" } };
                        string linehorometro = "AD8:" + "AF8";
                        hoja1.Cells[linehorometro].Merge = true;
                        hoja1.Cells[linehorometro].LoadFromArrays(lineHorometro);
                        hoja1.Cells[linehorometro].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[linehorometro].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> nHora = new List<string[]>() { new string[] { objUltimoHorometroDTO.Horometro.ToString() } };
                        string nhora = "AD8:" + "AF8";
                        hoja1.Cells[nhora].LoadFromArrays(nHora);
                        hoja1.Cells[nhora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        #endregion

                        #region ENCABEZADO
                        List<string[]> headerrow = new List<string[]>() { new string[] { "Descripcion " } };
                        string titulorango = "A11:" + "F12";
                        hoja1.Cells[titulorango].Merge = true;
                        hoja1.Cells[titulorango].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorango].LoadFromArrays(headerrow);
                        hoja1.Cells[titulorango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorango].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorango].Style.Font.Bold = true;
                        hoja1.Cells[titulorango].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> sistema = new List<string[]>() { new string[] { "Sistema a reparar" } };
                        string titulorangoSistema = "G11:" + "I12";
                        hoja1.Cells[titulorangoSistema].Merge = true;
                        hoja1.Cells[titulorangoSistema].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoSistema].LoadFromArrays(sistema);
                        hoja1.Cells[titulorangoSistema].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoSistema].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoSistema].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoSistema].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoSistema].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoSistema].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> requisicion = new List<string[]>() { new string[] { "No. de requisición" } };
                        string titulorangorequisicion = "J11:" + "K12";
                        hoja1.Cells[titulorangorequisicion].Merge = true;
                        hoja1.Cells[titulorangorequisicion].LoadFromArrays(requisicion);
                        hoja1.Cells[titulorangorequisicion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangorequisicion].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangorequisicion].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangorequisicion].Style.Font.Bold = true;
                        hoja1.Cells[titulorangorequisicion].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangorequisicion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangorequisicion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> compra = new List<string[]>() { new string[] { "No. de orden de compra " } };
                        string titulorangoCompra = "L11:" + "M12";
                        hoja1.Cells[titulorangoCompra].Merge = true;
                        hoja1.Cells[titulorangoCompra].LoadFromArrays(compra);
                        hoja1.Cells[titulorangoCompra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoCompra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoCompra].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoCompra].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoCompra].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoCompra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoCompra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(12).Width = 13.43;

                        List<string[]> Folio = new List<string[]>() { new string[] { "N° Folio " } };
                        string titulorangoFolio = "N11:" + "N12";
                        hoja1.Cells[titulorangoFolio].Merge = true;
                        hoja1.Cells[titulorangoFolio].LoadFromArrays(Folio);
                        hoja1.Cells[titulorangoFolio].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoFolio].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoFolio].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoFolio].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoFolio].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoFolio].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoFolio].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(14).Width = 13.43;

                        List<string[]> FechaRefacciones = new List<string[]>() { new string[] { "Fecha de surtimiento de refacciones" } };
                        string titulorangoFecha = "O11:" + "P11";
                        hoja1.Cells[titulorangoFecha].Merge = true;
                        hoja1.Cells[titulorangoFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoFecha].LoadFromArrays(FechaRefacciones);
                        hoja1.Cells[titulorangoFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoFecha].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoFecha].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoFecha].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(15).Width = 17.50;
                        hoja1.Column(16).Width = 15;
                        hoja1.Row(11).Height = 35;
                        hoja1.Row(12).Height = 13;

                        List<string[]> FechaRefaccionesProm = new List<string[]>() { new string[] { "Prom." } };
                        string titulorangoFecha1 = "O12:" + "O12";
                        hoja1.Cells[titulorangoFecha1].Merge = true;
                        hoja1.Cells[titulorangoFecha1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoFecha1].LoadFromArrays(FechaRefaccionesProm);
                        hoja1.Cells[titulorangoFecha1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoFecha1].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoFecha1].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoFecha1].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoFecha1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoFecha1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> FechaRefaccionesReal = new List<string[]>() { new string[] { "Real." } };
                        string titulorangoFecha2 = "P12:" + "P12";
                        hoja1.Cells[titulorangoFecha2].Merge = true;
                        hoja1.Cells[titulorangoFecha2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoFecha2].LoadFromArrays(FechaRefaccionesReal);
                        hoja1.Cells[titulorangoFecha2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoFecha2].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoFecha2].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoFecha2].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoFecha2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoFecha2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> avance = new List<string[]>() { new string[] { "Avance %" } };
                        string titulorangoAvance = "Q11:" + "Z11";
                        hoja1.Cells[titulorangoAvance].Merge = true;
                        hoja1.Cells[titulorangoAvance].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoAvance].LoadFromArrays(avance);
                        hoja1.Cells[titulorangoAvance].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoAvance].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoAvance].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoAvance].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoAvance].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoAvance].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> porsentaje10 = new List<string[]>() { new string[] { "10" } };
                        string por10 = "Q12:" + "Q12";
                        hoja1.Cells[por10].LoadFromArrays(porsentaje10);
                        hoja1.Cells[por10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por10].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por10].Style.Font.Bold = true;
                        hoja1.Cells[por10].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(17).Width = 3.15;

                        List<string[]> porsentaje20 = new List<string[]>() { new string[] { "20" } };
                        string por20 = "R12:" + "R12";
                        hoja1.Cells[por20].LoadFromArrays(porsentaje20);
                        hoja1.Cells[por20].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por20].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por20].Style.Font.Bold = true;
                        hoja1.Cells[por20].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por20].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(18).Width = 3.15;

                        List<string[]> porsentaje30 = new List<string[]>() { new string[] { "30" } };
                        string por30 = "S12:" + "S12";
                        hoja1.Cells[por30].LoadFromArrays(porsentaje30);
                        hoja1.Cells[por30].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por30].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por30].Style.Font.Bold = true;
                        hoja1.Cells[por30].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por30].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por30].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(19).Width = 3.15;

                        List<string[]> porsentaje40 = new List<string[]>() { new string[] { "40" } };
                        string por40 = "T12:" + "T12";
                        hoja1.Cells[por40].LoadFromArrays(porsentaje40);
                        hoja1.Cells[por40].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por40].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por40].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por40].Style.Font.Bold = true;
                        hoja1.Cells[por40].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por40].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por40].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(20).Width = 3.15;

                        List<string[]> porsentaje50 = new List<string[]>() { new string[] { "50" } };
                        string por50 = "U12:" + "U12";
                        hoja1.Cells[por50].LoadFromArrays(porsentaje50);
                        hoja1.Cells[por50].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por50].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por50].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por50].Style.Font.Bold = true;
                        hoja1.Cells[por50].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por50].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por50].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(21).Width = 3.15;

                        List<string[]> porsentaje60 = new List<string[]>() { new string[] { "60" } };
                        string por60 = "V12:" + "V12";
                        hoja1.Cells[por60].LoadFromArrays(porsentaje60);
                        hoja1.Cells[por60].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por60].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por60].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por60].Style.Font.Bold = true;
                        hoja1.Cells[por60].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por60].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por60].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(22).Width = 3.15;

                        List<string[]> porsentaje70 = new List<string[]>() { new string[] { "70" } };
                        string por70 = "W12:" + "W12";
                        hoja1.Cells[por70].LoadFromArrays(porsentaje70);
                        hoja1.Cells[por70].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por70].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por70].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por70].Style.Font.Bold = true;
                        hoja1.Cells[por70].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por70].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por70].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(23).Width = 3.15;

                        List<string[]> porsentaje80 = new List<string[]>() { new string[] { "80" } };
                        string por80 = "X12:" + "X12";
                        hoja1.Cells[por80].LoadFromArrays(porsentaje80);
                        hoja1.Cells[por80].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por80].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por80].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por80].Style.Font.Bold = true;
                        hoja1.Cells[por80].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por80].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por80].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(24).Width = 3.15;

                        List<string[]> porsentaje90 = new List<string[]>() { new string[] { "90" } };
                        string por90 = "Y12:" + "Y12";
                        hoja1.Cells[por90].LoadFromArrays(porsentaje90);
                        hoja1.Cells[por90].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por90].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por90].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por90].Style.Font.Bold = true;
                        hoja1.Cells[por90].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por90].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por90].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(25).Width = 2.30;

                        List<string[]> porsentaje100 = new List<string[]>() { new string[] { "100" } };
                        string por100 = "Z12:" + "Z12";
                        hoja1.Cells[por100].LoadFromArrays(porsentaje100);
                        hoja1.Cells[por100].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[por100].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[por100].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[por100].Style.Font.Bold = true;
                        hoja1.Cells[por100].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[por100].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        hoja1.Cells[por100].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Column(26).Width = 3.15;

                        List<string[]> FechaTerminacion = new List<string[]>() { new string[] { "Fecha de terminación" } };
                        string titulorangoTerminacion = "AA11:" + "AB11";
                        hoja1.Cells[titulorangoTerminacion].Merge = true;
                        hoja1.Cells[titulorangoTerminacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoTerminacion].LoadFromArrays(FechaTerminacion);
                        hoja1.Cells[titulorangoTerminacion].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoTerminacion].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoTerminacion].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoTerminacion].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoTerminacion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoTerminacion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(27).Width = 17.50;
                        hoja1.Column(28).Width = 15;

                        List<string[]> FechaTerminacionProm = new List<string[]>() { new string[] { "Prom" } };
                        string titulorangoTemp = "AA12:" + "AA12";
                        hoja1.Cells[titulorangoTemp].Merge = true;
                        hoja1.Cells[titulorangoTemp].LoadFromArrays(FechaTerminacionProm);
                        hoja1.Cells[titulorangoTemp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoTemp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoTemp].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoTemp].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoTemp].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoTemp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoTemp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> FechaTerminacionReal = new List<string[]>() { new string[] { "Real" } };
                        string titulorangoTemp1 = "AB12:" + "AB12";
                        hoja1.Cells[titulorangoTemp1].Merge = true;
                        hoja1.Cells[titulorangoTemp1].LoadFromArrays(FechaTerminacionReal);
                        hoja1.Cells[titulorangoTemp1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoTemp1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoTemp1].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoTemp1].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoTemp1].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoTemp1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoTemp1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> costoUSD = new List<string[]>() { new string[] { "Costo de refacciones U.S.D." } };
                        string titulorangoCostoUSB = "AC11:" + "AC12";
                        hoja1.Cells[titulorangoCostoUSB].Merge = true;
                        hoja1.Cells[titulorangoCostoUSB].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoCostoUSB].LoadFromArrays(costoUSD);
                        hoja1.Cells[titulorangoCostoUSB].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoCostoUSB].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoCostoUSB].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoCostoUSB].Style.WrapText = true;
                        hoja1.Cells[titulorangoCostoUSB].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoCostoUSB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoCostoUSB].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(29).Width = 12.11;

                        List<string[]> costoMN = new List<string[]>() { new string[] { "Costo de refacciones M.N." } };
                        string titulorangoCostoMN = "AD11:" + "AD12";
                        hoja1.Cells[titulorangoCostoMN].Merge = true;
                        hoja1.Cells[titulorangoCostoMN].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoCostoMN].LoadFromArrays(costoMN);
                        hoja1.Cells[titulorangoCostoMN].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoCostoMN].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoCostoMN].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoCostoMN].Style.WrapText = true;
                        hoja1.Cells[titulorangoCostoMN].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoCostoMN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoCostoMN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(30).Width = 12.20;

                        List<string[]> costoObra = new List<string[]>() { new string[] { "Costo mano de obra" } };
                        string titulorangoCostoObra = "AE11:" + "AE12";
                        hoja1.Cells[titulorangoCostoObra].Merge = true;
                        hoja1.Cells[titulorangoCostoObra].LoadFromArrays(costoObra);
                        hoja1.Cells[titulorangoCostoObra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoCostoObra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoCostoObra].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoCostoObra].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoCostoObra].Style.WrapText = true;
                        hoja1.Cells[titulorangoCostoObra].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoCostoObra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoCostoObra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(31).Width = 12.20;

                        List<string[]> granTotalUSD = new List<string[]>() { new string[] { "Gran total en U.S.D" } };
                        string titulorangoTotal = "AF11:" + "AF12";
                        hoja1.Cells[titulorangoTotal].Merge = true;
                        hoja1.Cells[titulorangoTotal].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        hoja1.Cells[titulorangoTotal].LoadFromArrays(granTotalUSD);
                        hoja1.Cells[titulorangoTotal].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[titulorangoTotal].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[titulorangoTotal].Style.Font.Bold = true;
                        hoja1.Cells[titulorangoTotal].Style.WrapText = true;
                        hoja1.Cells[titulorangoTotal].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                        hoja1.Cells[titulorangoTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[titulorangoTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja1.Column(32).Width = 12.11;
                        #endregion

                        #region DIBUJAR BARRA DE AVANCE
                        var cellData = new List<object[]>();
                        int contador = 12;
                        foreach (var item in lstBackLogsDTO)
                        {
                            //OrdenCompraDTO objOrdenCompraBL = lstOrdenesComprasDTO.Where(w => w.idBackLog == item.id).FirstOrDefault();
                            //decimal totalUSD = 0, totalMXN = 0;
                            //tblCom_OrdenCompra objOC = new tblCom_OrdenCompra();
                            //if (objOrdenCompraBL != null)
                            //{
                            //    int numero = Convert.ToInt32(objOrdenCompraBL.numOC);
                            //    objOC = _context.tblCom_OrdenCompra.Where(w => w.cc == objOrdenCompraBL.cc && w.numero == numero).FirstOrDefault();
                            //    if (objOC.moneda == "2") // USD
                            //    {
                            //        totalMXN = objOC.total * objOC.tipo_cambio;
                            //        totalUSD = objOC.total;
                            //    }
                            //    else if (objOC.moneda == "1") // MXN
                            //        totalMXN = objOC.total;
                            //}

                            //var arregloValores = new object[32];
                            //arregloValores[0] = !string.IsNullOrEmpty(item.descripcionBL) ? item.descripcionBL : "--";
                            //arregloValores[6] = !string.IsNullOrEmpty(item.subconjunto) ? item.subconjunto : "--";
                            //arregloValores[9] = item.numRequisicion > 0 ? item.numRequisicion.ToString() : "--";
                            //arregloValores[11] = item.numOC > 0 ? item.numOC.ToString() : "--";
                            //arregloValores[13] = !string.IsNullOrEmpty(item.folioBL) ? item.folioBL : "--";
                            //arregloValores[14] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                            //arregloValores[15] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                            //arregloValores[26] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                            //arregloValores[27] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                            //arregloValores[28] = objOC.moneda == "2" ? totalUSD.ToString("C2") : "--";
                            //arregloValores[29] = totalMXN.ToString("C2");
                            //cellData.Add(arregloValores);

                            hoja1.Cells[13, 1].LoadFromArrays(cellData);
                            contador++;
                            hoja1.Cells[contador, 17, contador, 26].Merge = true;

                            var cellMerge = hoja1.MergedCells[contador, 26];
                            switch (item.idEstatus)
                            {
                                case (int)EstatusBackLogEnum.ElaboracionInspeccion: hoja1.Cells[cellMerge].Value = 20; break;
                                case (int)EstatusBackLogEnum.ElaboracionRequisicion: hoja1.Cells[cellMerge].Value = 40; break;
                                case (int)EstatusBackLogEnum.ElaboracionOC: hoja1.Cells[cellMerge].Value = 50; break;
                                case (int)EstatusBackLogEnum.SuministroRefacciones: hoja1.Cells[cellMerge].Value = 60; break;
                                case (int)EstatusBackLogEnum.RehabilitacionProgramada: hoja1.Cells[cellMerge].Value = 60; break;
                                case (int)EstatusBackLogEnum.ProcesoInstalacion: hoja1.Cells[cellMerge].Value = 90; break;
                                case (int)EstatusBackLogEnum.BackLogsInstalado: hoja1.Cells[cellMerge].Value = 100; break;
                            }

                            ExcelAddress columnasAvance = new ExcelAddress(cellMerge);
                            var reglaBarraProgresoAvance = hoja1.ConditionalFormatting.AddDatabar(columnasAvance, ColorTranslator.FromHtml("#1E90FF"));
                            reglaBarraProgresoAvance.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightDown;
                            reglaBarraProgresoAvance.LowValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                            reglaBarraProgresoAvance.LowValue.Value = 0;
                            reglaBarraProgresoAvance.HighValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                            reglaBarraProgresoAvance.HighValue.Value = 100;
                        }
                        #endregion

                        #region PIE DE PAGINA
                        contador += 2;
                        List<string[]> totalporsentaje = new List<string[]>() { new string[] { "100%" } };
                        string Total = "A" + contador + ":" + "A" + contador;
                        hoja1.Cells[Total].Merge = true;
                        hoja1.Cells[Total].LoadFromArrays(totalporsentaje);

                        List<string[]> numeroBLCompletados = new List<string[]>() { new string[] { lstBackLogsDTO.Where(w => w.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado).Count().ToString() } };
                        string NumeroInstalados = "B" + contador + ":" + "B" + contador;
                        hoja1.Cells[NumeroInstalados].Merge = true;
                        hoja1.Cells[NumeroInstalados].LoadFromArrays(numeroBLCompletados);

                        contador++;
                        List<string[]> TotalBacklogs = new List<string[]>() { new string[] { "#" } };
                        string totalbacklogs = "A" + contador + ":" + "A" + contador;
                        hoja1.Cells[totalbacklogs].Merge = true;
                        hoja1.Cells[totalbacklogs].LoadFromArrays(TotalBacklogs);

                        List<string[]> numeroBLDescripcion = new List<string[]>() { new string[] { lstBackLogsDTO.Count().ToString() } };
                        string NumeroDescripcion = "B" + contador + ":" + "B" + contador;
                        hoja1.Cells[NumeroDescripcion].Merge = true;
                        hoja1.Cells[NumeroDescripcion].LoadFromArrays(numeroBLDescripcion);

                        contador += 2;
                        List<string[]> Texto10 = new List<string[]>() { new string[] { "10 %  = Elaboración del control de calidad, inspección visual y diagnóstico de nivel II." } };
                        string texto10 = "A" + contador + ":" + "J" + contador;
                        hoja1.Cells[texto10].Merge = true;
                        hoja1.Cells[texto10].LoadFromArrays(Texto10);

                        contador++;
                        List<string[]> Texto20 = new List<string[]>() { new string[] { "20 % = Elaboración de requisiciones, ordenes de trabajo (en borrador) por parte del personal operativo." } };
                        string texto20 = "A" + contador + ":" + "J" + contador;
                        hoja1.Cells[texto20].Merge = true;
                        hoja1.Cells[texto20].LoadFromArrays(Texto20);

                        contador++;
                        List<string[]> Texto30 = new List<string[]>() { new string[] { "30 % = Elaboración del alcance de reparación e informe de rehabilitación." } };
                        string texto30 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto30].Merge = true;
                        hoja1.Cells[texto30].LoadFromArrays(Texto30);

                        contador++;
                        List<string[]> Texto40 = new List<string[]>() { new string[] { "40 % = Entrega de requisiciones autorizadas al almacén para su captura en sao y distribución al departamento de compras." } };
                        string texto40 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto40].Merge = true;
                        hoja1.Cells[texto40].LoadFromArrays(Texto40);

                        contador++;
                        List<string[]> Texto50 = new List<string[]>() { new string[] { "50 % =  Reproceso de almacén, cotizaciones, elaboración de ordenes de compra (gestión de almacén y compras)." } };
                        string texto50 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto50].Merge = true;
                        hoja1.Cells[texto50].LoadFromArrays(Texto50);

                        contador++;
                        List<string[]> Texto60 = new List<string[]>() { new string[] { "60 % y 70 % = suministro de refacciones, materiales y trabajos con talleres externos al 100 %." } };
                        string texto60 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto60].Merge = true;
                        hoja1.Cells[texto60].LoadFromArrays(Texto60);

                        contador++;
                        List<string[]> Texto70 = new List<string[]>() { new string[] { "80 % y 90 % = rehabilitación de equipo por parte del personal operativo." } };
                        string texto70 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto70].Merge = true;
                        hoja1.Cells[texto70].LoadFromArrays(Texto70);

                        contador++;
                        List<string[]> Texto80 = new List<string[]>() { new string[] { "95 % = Pruebas, ajustes y diagnóstico final." } };
                        string texto80 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto80].Merge = true;
                        hoja1.Cells[texto80].LoadFromArrays(Texto80);

                        contador++;
                        List<string[]> Texto90 = new List<string[]>() { new string[] { "100 % =  Entrega de la unidad al área de producción" } };
                        string texto90 = "A" + contador + ":J" + contador;
                        hoja1.Cells[texto90].Merge = true;
                        hoja1.Cells[texto90].LoadFromArrays(Texto90);

                        contador += 2;
                        List<string[]> firmaela = new List<string[]>() { new string[] { "Elaboró" } };
                        string firmaEla = "I" + contador + ":K" + contador;
                        hoja1.Cells[firmaEla].Merge = true;
                        hoja1.Cells[firmaEla].LoadFromArrays(firmaela);
                        hoja1.Cells[firmaEla].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> vobo = new List<string[]>() { new string[] { "Vo bo" } };
                        string Vobo = "P" + contador + ":R" + contador;
                        hoja1.Cells[Vobo].Merge = true;
                        hoja1.Cells[Vobo].LoadFromArrays(vobo);
                        hoja1.Cells[Vobo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        contador += 4;
                        List<string[]> firma = new List<string[]>() { new string[] { "Control de BackLogs" } };
                        string Firma = "H" + contador + ":L" + contador;
                        hoja1.Cells[Firma].Merge = true;
                        hoja1.Cells[Firma].LoadFromArrays(firma);
                        hoja1.Cells[Firma].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[Firma].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        List<string[]> firmaVobo = new List<string[]>() { new string[] { "Gerencia de maquinaria" } };
                        string Firmavobo = "O" + contador + ":V" + contador;
                        hoja1.Cells[Firmavobo].Merge = true;
                        hoja1.Cells[Firmavobo].LoadFromArrays(firmaVobo);
                        hoja1.Cells[Firmavobo].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[Firmavobo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        #endregion

                        excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;
                        List<byte[]> lista = new List<byte[]>();

                        using (var exportData = new MemoryStream())
                        {
                            excel.SaveAs(exportData);
                            bytes = exportData;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.GENERAR_EXCEL, objParamsDTO.id, objParamsDTO);
            }
            return bytes;
        }
        #endregion

        #region REPORTE E INDICADORES
        public List<ComboDTO> FillCboResponsables()
        {
            try
            {
                #region v1
                var lstUSuarios = FillcboUsuarios().Select(x => new tblP_Usuario
                {
                    id = variable(x.Value),
                    nombre = x.Text
                }).ToList();

                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.Obra && x.esActivo && x.idUsuarioResponsable != 0).ToList();

                List<ComboDTO> lstResponsables = lstBL.Where(x => x.esActivo).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = lstUSuarios.Where(w => w.id == x.idUsuarioResponsable).Select(s => s.nombre).FirstOrDefault()
                }).ToList();

                List<ComboDTO> lstResponsab = new List<ComboDTO>();
                ComboDTO objResponsab = new ComboDTO();
                foreach (var item in lstResponsables)
                {
                    objResponsab = lstResponsab.Where(r => r.Value == item.Value).FirstOrDefault();
                    if (objResponsab == null)
                        lstResponsab.Add(item);
                }
                return lstResponsab;
                #endregion

                #region SE OBTIENE LISTADO DE EMPLEADOS DE SIGOPLAN

                //// SE OBTIENE USUARIOS DE CONSTRUPLAN
                //List<UsuarioDTO> lstUsuariosCP = _context.Select<UsuarioDTO>(new DapperDTO
                //{
                //    baseDatos = MainContextEnum.Construplan,
                //    consulta = @"SELECT nombre, apellidoPaterno, apellidoMaterno, empresa FROM tblP_Usuario WHERE estatus = @estatus",
                //    parametros = new { estatus = true }
                //}).ToList();

                //// SE OBTIENE USUARIOS DE ARRENDADORA
                //List<UsuarioDTO> lstUsuariosARR = _context.Select<UsuarioDTO>(new DapperDTO
                //{
                //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                //    consulta = @"SELECT nombre, apellidoPaterno, apellidoMaterno, empresa FROM tblP_Usuario WHERE estatus = @estatus",
                //    parametros = new { estatus = true }
                //}).ToList();

                //// SE MEZCLA LOS USUARIOS DE CP Y ARR (SE VERIFICA QUE NO SE DUPLIQUEN)
                //List<UsuarioDTO> lstUsuariosDTO = new List<UsuarioDTO>();
                //lstUsuariosDTO.AddRange(lstUsuariosCP.ToList());
                //lstUsuariosDTO.AddRange(lstUsuariosARR.Where(w => !lstUsuariosCP.Select(s => s.id).Contains(w.id)).ToList());

                //List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                //ComboDTO objDTO = new ComboDTO();
                //foreach (var item in lstUsuariosDTO)
                //{
                //    string nombreCompleto = string.Empty;
                //    if (!string.IsNullOrEmpty(item.nombre))
                //        nombreCompleto = item.nombre.Trim().ToUpper();

                //    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                //        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                //    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                //        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                //    objDTO.Value = item.id.ToString();
                //    objDTO.Text = nombreCompleto;
                //    objDTO.Prefijo = !string.IsNullOrEmpty(item.empresa) ? item.empresa.Trim().ToUpper() : string.Empty;
                //    lstComboDTO.Add(objDTO);
                //}
                //return lstComboDTO;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboResponsables", e, AccionEnum.FILLCOMBO, 0, 0);
                return null;
            }
        }

        public Dictionary<string, object> GetReporteIndicadores(BackLogsDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    if (objParamsDTO.anio <= 0) { throw new Exception("Es necesario seleccionar un año."); }
                    if (objParamsDTO.lstMeses.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos un mes."); }

                    #region CATALOGOS
                    #region SE OBTIENE LISTADO DE EMPLEADOS DE CP
                    List<tblRH_EK_Empleados> lstUsuariosCP = _ctx.Select<tblRH_EK_Empleados>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados",
                    }).ToList();
                    #endregion

                    #region SE OBTIENE LISTADO DE EMPLEADOS DE ARR
                    List<tblRH_EK_Empleados> lstUsuariosArr = _ctx.Select<tblRH_EK_Empleados>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados",
                    }).ToList();
                    #endregion
                    #endregion

                    string strQuery = string.Format(@"SELECT t1.folioBL, t1.descripcion AS descripcionBL, t1.noEconomico, t3.descripcion AS conjunto, t3.abreviacion AS conjuntoAbreviacion, t1.idEstatus, t1.fechaCreacionBL, 
                                                                t1.fechaInstaladoBL, t1.idUsuarioResponsable
			                                                        FROM tblBL_CatBackLogs AS t1
			                                                        INNER JOIN tblBL_CatSubconjuntos AS t2 ON t2.id = t1.idSubconjunto
			                                                        INNER JOIN tblBL_CatConjuntos AS t3 ON t3.id = t2.idConjunto
				                                                        WHERE YEAR(t1.fechaCreacionBL) = {0} AND MONTH(t1.fechaCreacionBL) IN ({1}) AND t1.esActivo = {2} AND t1.areaCuenta = '{3}'",
                                                                            objParamsDTO.anio, string.Join(",", objParamsDTO.lstMeses), 1, objParamsDTO.areaCuenta);
                    List<BackLogsDTO> lstBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    foreach (var item in lstBL)
                    {
                        item.descripcionBL = PersonalUtilities.PrimerLetraMayuscula(item.descripcionBL);
                        item.conjunto = PersonalUtilities.PrimerLetraMayuscula(item.conjunto);
                        item.conjuntoAbreviacion = PersonalUtilities.PrimerLetraMayuscula(item.conjuntoAbreviacion);
                        item.strCostoTotalBL = GetCostoFolioBL(item.folioBL).ToString("C");
                        switch (item.idEstatus)
                        {
                            case (int)EstatusBackLogEnum.ElaboracionInspeccion: item.estatus = "<button class='btn boton-estatus-20'>20%</button>"; break;
                            case (int)EstatusBackLogEnum.ElaboracionRequisicion: item.estatus = "<button class='btn boton-estatus-40'>40%</button>"; break;
                            case (int)EstatusBackLogEnum.ElaboracionOC: item.estatus = "<button class='btn boton-estatus-50'>50%</button>"; break;
                            case (int)EstatusBackLogEnum.SuministroRefacciones: item.estatus = "<button class='btn boton-estatus-60'>60%</button>"; break;
                            case (int)EstatusBackLogEnum.RehabilitacionProgramada: item.estatus = "<button class='btn boton-estatus-80'>80%</button>"; break;
                            case (int)EstatusBackLogEnum.ProcesoInstalacion: item.estatus = "<button class='btn boton-estatus-90'>90%</button>"; break;
                            case (int)EstatusBackLogEnum.BackLogsInstalado: item.estatus = "<button class='btn boton-estatus-100'>100%</button>"; break;
                        }

                        // SE OBTIENE EL NOMBRE COMPLETO DEL RESPONSABLE
                        string strResponsable = string.Empty;
                        strResponsable = lstUsuariosCP.Where(w => w.clave_empleado == item.idUsuarioResponsable).Select(s => s.nombre + " " + s.ape_paterno + " " + s.ape_materno).FirstOrDefault();
                        if (string.IsNullOrEmpty(strResponsable))
                            strResponsable = lstUsuariosArr.Where(w => w.clave_empleado == item.idUsuarioResponsable).Select(s => s.nombre + " " + s.ape_paterno + " " + s.ape_materno).FirstOrDefault();

                        item.responsable = !string.IsNullOrEmpty(strResponsable) ? PersonalUtilities.NombreCompletoPrimerLetraMayuscula(strResponsable) : "--";
                        // END

                        // SE OBTIENE EL TIEMPO PROMEDIO EN DÍAS PARA QUE UN BL LLEGUE A 100%
                        DateTime fechaInicioBL = item.fechaCreacionBL;
                        DateTime fechaFinBL = item.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado ? item.fechaInstaladoBL != null ? Convert.ToDateTime(item.fechaInstaladoBL) : DateTime.Now : DateTime.Now;
                        if (fechaInicioBL != null && fechaFinBL != null)
                        {
                            TimeSpan timeSpan = fechaFinBL - fechaInicioBL;
                            double daysDifference = timeSpan.TotalDays;
                            double daysCount = daysDifference;
                            item.diasTotales = Convert.ToInt32(daysCount);
                        }
                        if (item.diasTotales == 0)
                            item.diasTotales = 1;
                        // END
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstBL);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetReporteIndicadoresGrafica(string areaCuenta)
        {
            try
            {
                if (string.IsNullOrEmpty(areaCuenta))
                    throw new Exception("Es necesario seleccionar un área cuenta.");

                List<tblM_CatMaquina> lstCatMaquinas = _context.tblM_CatMaquina.Where(x => x.estatus == 1).ToList();
                List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                List<tblBL_InspeccionesTMC> lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(x => x.esActivo).ToList();
                List<tblBL_SeguimientoPptos> lstSegPptos = _context.tblBL_SeguimientoPptos.Where(x => x.esActivo).ToList();
                List<tblBL_DetFrentes> lstDetFrentes = _context.tblBL_DetFrentes.Where(x => x.esActivo).ToList();
                List<tblBL_CatFrentes> lstCatFrentes = _context.tblbl_CatFrentes.Where(x => x.esActivo).ToList();
                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.Obra && x.areaCuenta == areaCuenta && x.esActivo == true).ToList();
                List<tblBL_CatConjuntos> lstCatConjuntos = _context.tblBL_CatConjuntos.Where(x => x.esActivo).ToList();
                List<tblBL_CatSubconjuntos> lstCatSubconjuntos = _context.tblBL_CatSubconjuntos.Where(x => x.esActivo).ToList();

                var lst = lstBL.GroupBy(y => y.idUsuarioResponsable).Select(x => new BackLogsDTO
                {
                    idResponsable = x.Key,
                    responsable = lstUsuarios.Where(w => w.cveEmpleado == x.Key.ToString()).Select(s => s.nombre + " " + s.apellidoPaterno + " " + s.apellidoMaterno).FirstOrDefault(),
                    cantbl = 0

                }).ToList();

                foreach (var item in lst)
                {
                    item.cantbl = lstBL.Where(x => x.idUsuarioResponsable == item.idResponsable).Count();
                }

                resultado.Add("lstGraficaResposable", lst);
                return resultado;
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public Dictionary<string, object> GetInfoGraficaResponsables(BackLogsDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    #region INIT VARIABLES
                    List<string> lstResponsables = new List<string>();
                    List<int> lstCantBL = new List<int>();
                    #endregion

                    #region VALIDACIONES
                    if (objParamsDTO.anio <= 0) { throw new Exception("Es necesario seleccionar un año."); }
                    if (objParamsDTO.lstMeses.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos un mes."); }
                    #endregion

                    #region CATALOGOS
                    #region SE OBTIENE LISTADO DE EMPLEADOS DE CP
                    List<tblRH_EK_Empleados> lstUsuariosCP = _ctx.Select<tblRH_EK_Empleados>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados",
                    }).ToList();
                    #endregion

                    #region SE OBTIENE LISTADO DE EMPLEADOS DE ARR
                    List<tblRH_EK_Empleados> lstUsuariosArr = _ctx.Select<tblRH_EK_Empleados>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados",
                    }).ToList();
                    #endregion
                    #endregion

                    string strQuery = string.Format(@"SELECT t1.idUsuarioResponsable, COUNT(t1.id) AS cantbl
			                                                        FROM tblBL_CatBackLogs AS t1
			                                                        INNER JOIN tblBL_CatSubconjuntos AS t2 ON t2.id = t1.idSubconjunto
			                                                        INNER JOIN tblBL_CatConjuntos AS t3 ON t3.id = t2.idConjunto
				                                                        WHERE YEAR(t1.fechaCreacionBL) = {0} AND MONTH(t1.fechaCreacionBL) IN ({1}) AND t1.esActivo = {2} AND t1.areaCuenta = '{3}'
                                                                            GROUP BY t1.idUsuarioResponsable",
                                                                            objParamsDTO.anio, string.Join(",", objParamsDTO.lstMeses), 1, objParamsDTO.areaCuenta);
                    List<BackLogsDTO> lstBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    foreach (var item in lstBL)
                    {
                        // SE OBTIENE EL NOMBRE COMPLETO DEL RESPONSABLE Y LA CANT DE BL QUE TIENE ASIGNADO
                        string strResponsable = lstUsuariosCP.Where(w => w.clave_empleado == item.idUsuarioResponsable).Select(s => s.nombre + " " + s.ape_paterno + " " + s.ape_materno).FirstOrDefault();
                        if (string.IsNullOrEmpty(strResponsable))
                            strResponsable = lstUsuariosArr.Where(w => w.clave_empleado == item.idUsuarioResponsable).Select(s => s.nombre + " " + s.ape_paterno + " " + s.ape_materno).FirstOrDefault();

                        if (!string.IsNullOrEmpty(strResponsable))
                        {
                            lstResponsables.Add(PersonalUtilities.NombreCompletoPrimerLetraMayuscula(strResponsable));
                            lstCantBL.Add(lstBL.Where(w => w.idUsuarioResponsable == item.idUsuarioResponsable).Select(s => s.cantbl).FirstOrDefault());
                        }
                        // END
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstResponsables", lstResponsables);
                    resultado.Add("lstCantBL", lstCantBL);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetReportePorResponsables(BackLogsDTO objFiltro, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    string strQuery = string.Format(@"SELECT * FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (MONTH(fechaInspeccion) BETWEEN {2} AND {3}) AND 
                                                        (YEAR(fechaInspeccion) BETWEEN {4} AND {5}) AND idUsuarioResponsable > {6}",
                                                        objFiltro.areaCuenta, 1, fechaInicio.Month, fechaFin.Month, fechaInicio.Year, fechaFin.Year, 0);
                    if (objFiltro.lstUsuarios.Count() > 0)
                        strQuery += string.Format(" AND idUsuarioResponsable IN ({0})", string.Join(",", objFiltro.lstUsuarios));
                    List<tblBL_CatBackLogs> lstBL = _ctx.Select<tblBL_CatBackLogs>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    if (lstBL.Count() > 0)
                    {
                        List<tblM_CatMaquina> lstCatMaquinas = _ctx.tblM_CatMaquina.Where(x => x.estatus == 1).ToList();
                        List<tblP_Usuario> lstUsuarios = _ctx.tblP_Usuario.Where(x => x.estatus).ToList();
                        List<tblBL_CatConjuntos> lstCatConjuntos = _ctx.tblBL_CatConjuntos.Where(x => x.esActivo).ToList();
                        List<tblBL_CatSubconjuntos> lstCatSubConjuntos = _ctx.tblBL_CatSubconjuntos.Where(x => x.esActivo).ToList();

                        #region SE OBTIENE LISTADO DE EMPLEADOS DE CP
                        List<tblRH_EK_Empleados> lstUsuariosCP = _ctx.Select<tblRH_EK_Empleados>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados",
                        }).ToList();
                        #endregion

                        #region SE OBTIENE LISTADO DE EMPLEADOS DE ARR
                        List<tblRH_EK_Empleados> lstUsuariosArr = _ctx.Select<tblRH_EK_Empleados>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno FROM tblRH_EK_Empleados",
                        }).ToList();
                        #endregion

                        #region FILL TABLA POR RESPONSABLE
                        List<BackLogsDTO> lstResponsables = lstBL.GroupBy(g => g.idUsuarioResponsable).Select(x => new BackLogsDTO
                        {
                            responsable = string.Empty,
                            idUsuarioResponsable = x.Key,
                            descripcion = string.Empty,
                            top1Conjunto = string.Empty,
                            top2Conjunto = string.Empty,
                            top3Conjunto = string.Empty,
                            cantBL50OMenos = 0,
                            cantBL70 = 0,
                            cantBL100 = 0,
                            cantBLTotal = 0,
                            tiempoProm100 = 0
                        }).ToList();
                        #endregion

                        foreach (var item in lstResponsables)
                        {
                            if (item.idUsuarioResponsable > 0)
                            {
                                #region SE OBTIENE EL RESPONSABLE DE LOS BACKLOGS.
                                string strResponsable = string.Empty;
                                strResponsable = lstUsuariosCP.Where(w => w.clave_empleado == item.idUsuarioResponsable).Select(s => s.nombre + " " + s.ape_paterno + " " + s.ape_materno).FirstOrDefault();
                                if (string.IsNullOrEmpty(strResponsable))
                                    strResponsable = lstUsuariosArr.Where(w => w.clave_empleado == item.idUsuarioResponsable).Select(s => s.nombre + " " + s.ape_paterno + " " + s.ape_materno).FirstOrDefault();

                                item.responsable = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(strResponsable);
                                #endregion

                                #region SE OBTIENE EL TOP 3 DE LOS CONJUNTOS MAS UTILIZADOS EN LOS BACKLOGS CON RELACIÓN AL RESPONSABLE DEL FRENTE.
                                List<int> lstSubconjuntosID = lstBL.Where(w => w.idUsuarioResponsable == item.idUsuarioResponsable).Select(s => s.idSubconjunto).ToList();
                                Dictionary<int, int> lstTopSubConjuntos = new Dictionary<int, int>();
                                foreach (var itemSub in lstSubconjuntosID)
                                {
                                    int existeEnDictionary = lstTopSubConjuntos.Where(w => w.Key == itemSub).Count();
                                    if (existeEnDictionary > 0)
                                        lstTopSubConjuntos[itemSub] += 1;
                                    else
                                        lstTopSubConjuntos.Add(itemSub, 1);
                                }
                                lstTopSubConjuntos.OrderByDescending(o => o.Value);

                                int idSubConjuntoTop1 = 0, idSubConjuntoTop2 = 0, idSubConjuntoTop3 = 0;
                                int idConjuntoTop1 = 0, idConjuntoTop2 = 0, idConjuntoTop3 = 0, contador = 0;
                                foreach (KeyValuePair<int, int> itemCon in lstTopSubConjuntos)
                                {
                                    if (contador == 0)
                                        idSubConjuntoTop1 = itemCon.Key;

                                    if (contador == 1)
                                        idSubConjuntoTop2 = itemCon.Key;

                                    if (contador == 2)
                                        idSubConjuntoTop3 = itemCon.Key;

                                    contador++;
                                }
                                idConjuntoTop1 = lstCatSubConjuntos.Where(w => w.id == idSubConjuntoTop1).Select(s => s.idConjunto).FirstOrDefault();
                                idConjuntoTop2 = lstCatSubConjuntos.Where(w => w.id == idSubConjuntoTop2).Select(s => s.idConjunto).FirstOrDefault();
                                idConjuntoTop3 = lstCatSubConjuntos.Where(w => w.id == idSubConjuntoTop3).Select(s => s.idConjunto).FirstOrDefault();

                                string top1Conjunto = lstCatConjuntos.Where(w => w.id == idConjuntoTop1).Select(s => s.descripcion).FirstOrDefault();
                                string top2Conjunto = lstCatConjuntos.Where(w => w.id == idConjuntoTop2).Select(s => s.descripcion).FirstOrDefault();
                                string top3Conjunto = lstCatConjuntos.Where(w => w.id == idConjuntoTop3).Select(s => s.descripcion).FirstOrDefault();

                                item.top1Conjunto = top1Conjunto != null ? PersonalUtilities.PrimerLetraMayuscula(top1Conjunto) : string.Empty;
                                item.top2Conjunto = top2Conjunto != null ? PersonalUtilities.PrimerLetraMayuscula(top2Conjunto) : string.Empty;
                                item.top3Conjunto = top3Conjunto != null ? PersonalUtilities.PrimerLetraMayuscula(top3Conjunto) : string.Empty;
                                #endregion

                                #region SE OBTIENE LA DESCRIPCIÓN DEL ECONOMICO MAS UTILIZADO POR EL RESPONSABLE

                                #region SE OBTIENE LOS BL ASIGNADOS AL RESPONSABLE
                                List<tblBL_CatBackLogs> _lstBLRelUsuario = lstBL.Where(w => w.idUsuarioResponsable == item.idUsuarioResponsable).ToList();
                                #endregion

                                Dictionary<string, int> lstTopEconomicos = new Dictionary<string, int>();
                                foreach (var itemEconomico in _lstBLRelUsuario)
                                {
                                    int existeEnDictionary = lstTopEconomicos.Where(w => w.Key == itemEconomico.noEconomico).Count();
                                    if (existeEnDictionary > 0)
                                        lstTopEconomicos[itemEconomico.noEconomico] += 1;
                                    else
                                        lstTopEconomicos.Add(itemEconomico.noEconomico, 1);
                                }
                                lstTopEconomicos.OrderByDescending(o => o.Value);
                                string noEconomico = string.Empty;
                                noEconomico = lstTopEconomicos.Select(s => s.Key).FirstOrDefault();
                                item.descripcion = lstCatMaquinas.Where(w => w.noEconomico == noEconomico).Select(s => s.descripcion.Trim()).FirstOrDefault();
                                item.descripcion = PersonalUtilities.PrimerLetraMayuscula(item.descripcion);
                                #endregion

                                // SE OBTIENE LA CANTIDAD DE BACKLOGS CON ESTATUS 50% O MENOS
                                item.cantBL50OMenos = _lstBLRelUsuario.Where(w => w.idEstatus <= 3).Count();

                                // SE OBTIENE LA CANTIDAD DE BACKLOGS CON ESTATUS 60% AL 90%
                                item.cantBL70 = _lstBLRelUsuario.Where(w => w.idEstatus > 3 && w.idEstatus < 7).Count();

                                // SE OBTIENE LA CANTIDAD DE BACKLOGS CON ESTATUS 100%
                                item.cantBL100 = _lstBLRelUsuario.Where(w => w.idEstatus == 7).Count();

                                // SE OBTIENE EL TOTAL DE BL ASIGNADOS AL RESPONSABLE
                                item.cantBLTotal = (item.cantBL50OMenos + item.cantBL70 + item.cantBL100);

                                #region SE OBTIENE EL TIEMPO PROMEDIO EN DÍAS PARA QUE UN BL LLEGUE A 100%
                                tblBL_CatBackLogs objBL = lstBL.Where(w => w.idUsuarioResponsable == item.idUsuarioResponsable && w.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado).OrderByDescending(o => o.id).FirstOrDefault();
                                if (objBL != null)
                                {
                                    DateTime fechaInicioBL = objBL.fechaCreacionBL;
                                    DateTime fechaFinBL = Convert.ToDateTime(objBL.fechaInstaladoBL);
                                    if (fechaInicioBL != null && fechaFinBL != null)
                                    {
                                        TimeSpan timeSpan = fechaFinBL - fechaInicioBL;
                                        double daysDifference = timeSpan.TotalDays;
                                        double daysCount = daysDifference;
                                        //item.tiempoProm100 = Convert.ToInt32((daysCount / 2));
                                        item.tiempoProm100 = Convert.ToInt32(daysCount);
                                    }
                                }
                                if (item.tiempoProm100 == 0)
                                    item.tiempoProm100 = 1;
                                #endregion
                            }
                        }

                        resultado.Add("lstResponsables", lstResponsables.Where(x => x.responsable != null).ToList());
                    }
                    return resultado;
                }
                catch (Exception e)
                {
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetReportePorResponsables", e, AccionEnum.CONSULTA, 0, 0);
                    return null;
                }
            }
        }
        #endregion

        public Dictionary<string, object> GetIndicadorBacklogPorEquipo(string AreaCuenta, DateTime fechaInicio, DateTime fechaFin, TipoMaquinaEnum tipoEquipo, EstatusBackLogEnum estatus)
        {
            try
            {
                #region ESTABLECIENDO CABECERA
                int Almacen = 0;
                tblAlm_RelAreaCuentaXAlmacen objAreaAlmacen = _context.tblAlm_RelAreaCuentaXAlmacen.Where(r => r.AreaCuenta == AreaCuenta).FirstOrDefault();
                if (objAreaAlmacen != null)
                    Almacen = _context.tblAlm_RelAreaCuentaXAlmacenDet.Where(r => r.idRelacion == objAreaAlmacen.id).OrderBy(n => n.id).Select(y => y.Almacen).FirstOrDefault();
                #endregion

                List<tblBL_OrdenesCompra> lstOrdenesDeComrpas = _context.tblBL_OrdenesCompra.Where(w => w.esActivo).ToList();
                List<int> lstTienenOrder = _context.tblBL_OrdenesCompra.Where(w => w.esActivo).Select(y => y.idBackLog).ToList();
                //List<tblBL_CatBackLogs> lstBackLogs = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.Obra &&
                //                                                        x.fechaCreacionBL >= fechaInicio &&
                //                                                        x.fechaCreacionBL <= fechaFin &&
                //                                                        (estatus == 0 ? true : x.idEstatus == (int)estatus) &&
                //                                                        x.areaCuenta == AreaCuenta &&
                //                                                        x.esActivo).ToList();
                string strQuery = string.Format(@"SELECT * FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (MONTH(fechaInspeccion) BETWEEN {2} AND {3}) AND 
                                                    (YEAR(fechaInspeccion) BETWEEN {4} AND {5}) AND idEstatus = {6}",
                                                    AreaCuenta, 1, fechaInicio.Month, fechaFin.Month, fechaInicio.Year, fechaFin.Year, (int)estatus);
                var lstBackLogs = _context.Select<tblBL_CatBackLogs>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();

                List<tblM_CatMaquina> maquina = _context.tblM_CatMaquina.ToList();
                List<ReporteBacklogsPorEquipoDTO> lstEquipos = new List<ReporteBacklogsPorEquipoDTO>();
                List<int> lstIdBackLogs = lstBackLogs.Where(w => w.esActivo).Select(y => y.id).ToList();
                List<tblBL_CatBackLogs> lstCantBL = _context.tblBL_CatBackLogs.Where(w => w.esActivo).ToList();

                foreach (var item in lstOrdenesDeComrpas)
                {
                    var obj = lstBackLogs.Where(r => r.id == item.idBackLog && r.esActivo).ToList().Select(y => new ReporteBacklogsPorEquipoDTO
                    {
                        noEconomico = y.noEconomico,
                        descripcion = maquina.Where(r => r.noEconomico == y.noEconomico).Select(n => n.descripcion).FirstOrDefault(),
                        modelo = maquina.Where(r => r.noEconomico == y.noEconomico).Select(n => n.modeloEquipo.descripcion).FirstOrDefault(),
                        horas = y.horas,
                        estatus = obtenerEstatus(y.idEstatus) + "%",
                        fechaUltimoBL = y.fechaCreacionBL,
                        cantidadBL = lstCantBL.Where(r => r.noEconomico == y.noEconomico && r.esActivo).ToList().Count(),
                        presupuestoMes = 0, //CalcularImporteOrdenCompra(AreaCuenta, item.numOC, y.cc, obtenerLstInsumops(y.id)),
                        presupuestoAcumulado = CalcularImporteOrdenCompra(AreaCuenta, item.numOC, y.cc, obtenerLstInsumops(y.id)),
                        costoHora = Math.Round((CalcularImporteOrdenCompra(AreaCuenta, item.numOC, y.cc, obtenerLstInsumops(y.id)) / (y.horas == 0 ? 1 : y.horas)), 2),
                        lstInsumos = obtenerLstInsumops(y.id),
                    }).FirstOrDefault();

                    if (obj != null)
                        lstEquipos.Add(obj);
                }

                var lstEquipos2 = lstBackLogs.Where(r => !lstTienenOrder.Contains(r.id) && r.esActivo).ToList().Select(y => new ReporteBacklogsPorEquipoDTO
                {
                    noEconomico = y.noEconomico,
                    descripcion = maquina.Where(r => r.noEconomico == y.noEconomico).Select(n => n.descripcion).FirstOrDefault(),
                    modelo = maquina.Where(r => r.noEconomico == y.noEconomico).Select(n => n.modeloEquipo.descripcion).FirstOrDefault(),
                    horas = y.horas,
                    estatus = obtenerEstatus(y.idEstatus) + "%",
                    fechaUltimoBL = y.fechaCreacionBL,
                    cantidadBL = lstCantBL.Where(r => r.noEconomico == y.noEconomico && r.esActivo).ToList().Count(),
                    strPresupuestoMes = GetCostoUltimoBL(AreaCuenta, y.noEconomico).ToString("C"),
                    strPresupuestoAcumulado = GetCostoTotalBLEconomico(AreaCuenta, y.noEconomico).ToString("C"),
                    costoHora = 0,//Math.Round((obteniendoCostoProm(Almacen, obtenerLstInsumops(y.id)) / (y.horas == 0 ? 1 : y.horas)), 2),
                    lstInsumos = obtenerLstInsumops(y.id),
                }).ToList();

                if (lstEquipos2.Count() != 0)
                    lstEquipos.AddRange(lstEquipos2);

                var lstNueva = lstEquipos.GroupBy(n => n.noEconomico).ToList().Select(y => new ReporteBacklogsPorEquipoDTO
                {
                    noEconomico = y.Select(n => n.noEconomico).FirstOrDefault(),
                    descripcion = y.Select(n => n.descripcion).FirstOrDefault(),
                    modelo = y.Select(n => n.modelo).FirstOrDefault(),
                    horas = y.Select(n => n.horas).FirstOrDefault(),
                    estatus = y.Select(n => n.estatus).FirstOrDefault(),
                    fechaUltimoBL = y.Max(n => n.fechaUltimoBL),
                    cantidadBL = y.Max(n => n.cantidadBL),
                    presupuestoMes = y.Max(n => n.presupuestoMes),
                    presupuestoAcumulado = y.Max(n => n.presupuestoAcumulado),
                    costoHora = y.Select(n => n.costoHora).FirstOrDefault(),
                }).ToList();

                foreach (var item in lstNueva)
                {
                    item.descripcion = PersonalUtilities.PrimerLetraMayuscula(item.descripcion);

                    switch (item.estatus)
                    {
                        case "20%": item.estatus = "<button class='btn boton-estatus-20'>20%</button>"; break;
                        case "40%": item.estatus = "<button class='btn boton-estatus-40'>40%</button>"; break;
                        case "50%": item.estatus = "<button class='btn boton-estatus-50'>50%</button>"; break;
                        case "60%": item.estatus = "<button class='btn boton-estatus-60'>60%</button>"; break;
                        case "80%": item.estatus = "<button class='btn boton-estatus-80'>80%</button>"; break;
                        case "90%": item.estatus = "<button class='btn boton-estatus-90'>90%</button>"; break;
                        case "100%": item.estatus = "<button class='btn boton-estatus-100'>100%</button>"; break;
                    }
                }

                resultado.Add("lstEquipos", lstNueva);
                resultado.Add("catGraficaPorEquipo", lstNueva.Select(x => x.noEconomico));
                resultado.Add("dataGraficaCostoHora", lstNueva.Select(x => x.costoHora));
                resultado.Add("dataGraficaCostoMes", lstNueva.Select(x => x.presupuestoMes));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetReporteIndicadores", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public decimal GetCostoUltimoBL(string areaCuenta, string noEconomico)
        {
            decimal costoTotalBL = 0;
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    string strQuery = string.Format(@"SELECT TOP(1)tipo_cambio * total
	                                                        FROM tblBL_CatBackLogs AS t1
	                                                        INNER JOIN tblBL_OrdenesCompra AS t2 ON t2.idBackLog = t1.id
	                                                        INNER JOIN tblCom_OrdenCompra AS t3 ON t3.cc = t2.cc AND t3.numero = t2.numOC
		                                                        WHERE t1.areaCuenta = '{0}' AND t1.noEconomico = '{1}' AND t1.esActivo = {2}
			                                                        ORDER BY t1.id DESC", areaCuenta, noEconomico, 1);
                    costoTotalBL = _ctx.Select<decimal>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { areaCuenta = areaCuenta, noEconomico = noEconomico });
                }
            }
            return costoTotalBL;
        }

        public decimal GetCostoTotalBLEconomico(string areaCuenta, string noEconomico)
        {
            decimal costoTotalBL = 0;
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    string strQuery = string.Format(@"SELECT SUM(t3.tipo_cambio * t3.total) AS costoTotalBL
	                                                        FROM tblBL_CatBackLogs AS t1
	                                                        INNER JOIN tblBL_OrdenesCompra AS t2 ON t2.idBackLog = t1.id
	                                                        INNER JOIN tblCom_OrdenCompra AS t3 ON t3.cc = t2.cc AND t3.numero = t2.numOC
		                                                        WHERE t1.areaCuenta = '{0}' AND t1.noEconomico = '{1}' AND t1.esActivo = {2}", areaCuenta, noEconomico, 1);
                    BackLogsDTO objBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).FirstOrDefault();
                    if (objBL != null)
                        costoTotalBL = objBL.costoTotalBL;
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { areaCuenta = areaCuenta, noEconomico = noEconomico });
                }
            }
            return costoTotalBL;
        }

        public decimal GetCostoFolioBL(string folioBL)
        {
            decimal costoTotalBL = 0;
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    string strQuery = string.Format(@"SELECT SUM(tipo_cambio * total) AS costoTotalBL
	                                                        FROM tblBL_CatBackLogs AS t1
	                                                        INNER JOIN tblBL_OrdenesCompra AS t2 ON t2.idBackLog = t1.id
	                                                        INNER JOIN tblCom_OrdenCompra AS t3 ON t3.cc = t2.cc AND t3.numero = t2.numOC
		                                                        WHERE t1.folioBL = '{0}' AND t1.esActivo = {1}", folioBL, 1);
                    BackLogsDTO objBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).FirstOrDefault();
                    if (objBL != null)
                        costoTotalBL = objBL.costoTotalBL;
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { folioBL = folioBL });
                }
            }
            return costoTotalBL;
        }

        public decimal obteniendoCostoProm(int Almacen, List<int> lst)
        {
            decimal sum = 0;
            foreach (var item in lst)
            {
                sum += GetCostoPromedio(Almacen, item);
            }
            return sum;
        }

        public List<int> obtenerLstInsumops(int id)
        {
            var lstPartes = _context.tblBL_Partes.Where(r => r.idBacklog == id).Select(y => y.insumo).ToList();
            return lstPartes;
        }

        public int obtenerEstatus(int auxEstatusBacklogEnum)
        {
            int auxEstatus = 0;
            switch (auxEstatusBacklogEnum)
            {
                case (int)EstatusBackLogEnum.ElaboracionInspeccion:
                    auxEstatus = 20;
                    break;
                case (int)EstatusBackLogEnum.ElaboracionRequisicion:
                    auxEstatus = 40;
                    break;
                case (int)EstatusBackLogEnum.ElaboracionOC:
                    auxEstatus = 50;
                    break;
                case (int)EstatusBackLogEnum.SuministroRefacciones:
                    auxEstatus = 60;
                    break;
                case (int)EstatusBackLogEnum.RehabilitacionProgramada:
                    auxEstatus = 80;
                    break;
                case (int)EstatusBackLogEnum.ProcesoInstalacion:
                    auxEstatus = 90;
                    break;
                case (int)EstatusBackLogEnum.BackLogsInstalado:
                    auxEstatus = 100;
                    break;
            }
            return auxEstatus;
        }

        private decimal CalcularImporteOrdenCompra(string AreaCuenta, string numeroOC, string cc, List<int> insumos)
        {
            int almacen = 0;
            tblAlm_RelAreaCuentaXAlmacen objAreaalmacen = _context.tblAlm_RelAreaCuentaXAlmacen.Where(r => r.AreaCuenta == AreaCuenta).FirstOrDefault();
            if (objAreaalmacen != null)
            {
                almacen = _context.tblAlm_RelAreaCuentaXAlmacenDet.Where(x => x.idRelacion == objAreaalmacen.id).OrderBy(y => y.id).Select(y => y.Almacen).FirstOrDefault();
            }

            List<int> lstInsumosSiOrden = new List<int>();
            List<int> lstInsumosNoOrden = new List<int>();
            decimal suma = 0;
            decimal importe = 0;
            int numero = 0;
            var auxParse = Int32.TryParse(numeroOC, out numero);
            if (auxParse)
            {
                //var detallesOrdenCompra = _context.tblCom_OrdenCompraDet.Where(x => x.numero == numero && x.cc == cc && x.estatusRegistro).ToList();


                foreach (var item in insumos)
                {
                    List<OrdenCompraDTO> lstInsumos = new List<OrdenCompraDTO>();
                    string strQuery2 = @"SELECT insumo FROM so_orden_compra_det WHERE cc ='{0}' AND numero={1} AND insumo={2}";
                    var odbc2 = new OdbcConsultaDTO() { consulta = strQuery2 };
                    odbc2.consulta = String.Format(strQuery2, cc, numero, item);
                    if (productivo)
                        lstInsumos = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc2);
                    else
                        lstInsumos = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc2);

                    if (lstInsumos.Count != 0)
                    {
                        lstInsumosSiOrden.Add(item);
                    }
                    else
                    {
                        lstInsumosNoOrden.Add(item);
                    }
                }

                if (lstInsumosSiOrden.Count() != 0)
                {
                    List<OrdenCompraDTO> lstInsumosOC = new List<OrdenCompraDTO>();
                    string strQuery = @"
                    SELECT SUM(compraDet.importe) * (1 + (compra.porcent_iva / 100)) AS importe FROM DBA.so_orden_compra AS compra 
                        INNER JOIN DBA.so_orden_compra_det AS compraDet ON compra.numero = compraDet.numero AND compra.cc = compraDet.cc
                            WHERE compra.numero = {0} AND compra.cc = '{1}'
                                GROUP BY compra.porcent_iva";
                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                    odbc.consulta = String.Format(strQuery, numero, cc);
                    if (productivo)
                        lstInsumosOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolEnum.ArrenProd, odbc);
                    else
                        lstInsumosOC = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                    suma += lstInsumosOC[0].importe;
                }

                foreach (var item in lstInsumosNoOrden)
                {
                    suma += GetCostoPromedio(almacen, item);
                }

                return suma;

                //var detallesVerificados = lstInsumosOC.Where(x => insumos.Contains(x.insumo)).ToList();
                //if (detallesVerificados.Count() > 0) importe = detallesVerificados.Sum(x => x.sub_total);
            }
            return importe;
        }
        #endregion

        public MemoryStream crearExcelInspeccionesTMC(string AreaCuenta)
        {
            try
            {
                List<string> lstNoEconomicos = new List<string>();
                string[] NoEconomicosSplit = AreaCuenta.Split(',');
                foreach (var item in NoEconomicosSplit)
                {
                    lstNoEconomicos.Add(item);
                }

                InspeccionesDTO objReturn = new InspeccionesDTO();
                Color colorDeCelda = ColorTranslator.FromHtml("#fff");
                Color colorDeCeldas = ColorTranslator.FromHtml("#000");

                List<RequisicionesDTO> lstRequisicionesDTO = GetListaRequisiciones().ToList();
                List<OrdenCompraDTO> lstOrdenesComprasDTO = GetListaOrdenesComprasDTO().ToList();

                var lstbacklogs = _context.tblBL_CatBackLogs.Where(x => x.esActivo && lstNoEconomicos.Contains(x.noEconomico) && x.tipoBL == (int)TipoBackLogEnum.TMC).ToList();

                var obj = _context.tblBL_InspeccionesTMC.Where(x => x.esActivo && lstNoEconomicos.Contains(x.noEconomico)).FirstOrDefault();
                var objCC = _context.tblP_CC.Where(x => x.estatus && x.areaCuenta == obj.areaCuenta).FirstOrDefault();
                var lstMaquina = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && lstNoEconomicos.Contains(x.noEconomico)).FirstOrDefault();

                var listaBL = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.tipoBL == (int)TipoBackLogEnum.TMC && lstNoEconomicos.Contains(x.noEconomico)).Count();
                var descripcionBL = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.descripcion == x.descripcion && x.tipoBL == (int)TipoBackLogEnum.TMC && lstNoEconomicos.Contains(x.noEconomico)).Count();
                string totalBLDescripcion = Convert.ToString(descripcionBL);
                string totalBL = Convert.ToString(listaBL);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var BL = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.noEconomico == AreaCuenta).FirstOrDefault();

                    var hoja1 = excel.Workbook.Worksheets.Add("Inspecciones TMC");

                    #region HEADER EXCEL

                    List<string[]> Direccion = new List<string[]>() { new string[] { "Direccion de Maquinaria y equipo ", } };
                    string tituloDireccion = "M1:" + "P1";
                    hoja1.Cells[tituloDireccion].Merge = true;
                    hoja1.Cells[tituloDireccion].Style.Font.Bold = true;
                    hoja1.Cells[tituloDireccion].LoadFromArrays(Direccion);
                    hoja1.Cells[tituloDireccion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> informe = new List<string[]>() { new string[] { "Informe de Rehabilitación", } };
                    string tituloInforme = "M2:" + "P2";
                    hoja1.Cells[tituloInforme].Merge = true;
                    hoja1.Cells[tituloInforme].Style.Font.Bold = true;
                    hoja1.Cells[tituloInforme].Style.Font.Size = 20;
                    hoja1.Cells[tituloInforme].LoadFromArrays(informe);
                    hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Row(2).Height = 24;

                    List<string[]> cc = new List<string[]>() { new string[] { "Centro de costo", } };
                    string titulorcc = "L4:" + "N4";
                    hoja1.Cells[titulorcc].Merge = true;
                    hoja1.Cells[titulorcc].LoadFromArrays(cc);
                    hoja1.Cells[titulorcc].Style.Font.Bold = true;
                    hoja1.Cells[titulorcc].Style.Font.Size = 14;
                    hoja1.Cells[titulorcc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> xx = new List<string[]>() { new string[] { objCC.cc.ToString() } };
                    string XX = "O4:" + "O4";
                    hoja1.Cells[XX].LoadFromArrays(xx);

                    List<string[]> linecc = new List<string[]>() { new string[] { "", } };
                    string lineCC = "O4:" + "O4";
                    hoja1.Cells[lineCC].Merge = true;
                    hoja1.Cells[lineCC].LoadFromArrays(linecc);
                    hoja1.Cells[lineCC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[lineCC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> DatosMaquina = new List<string[]>() { new string[] { "Datos de la maquina", } };
                    string datos = "N6:" + "O6";
                    hoja1.Cells[datos].Merge = true;
                    hoja1.Cells[datos].LoadFromArrays(DatosMaquina);
                    hoja1.Cells[datos].Style.Font.Bold = true;
                    hoja1.Cells[datos].Style.Font.Size = 12;
                    hoja1.Cells[datos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> descripcionMaquin = new List<string[]>() { new string[] { "Descripcion", } };
                    string maquina = "N7:" + "O7";
                    hoja1.Cells[maquina].Merge = true;
                    hoja1.Cells[maquina].LoadFromArrays(descripcionMaquin);
                    hoja1.Cells[maquina].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> lineDesc = new List<string[]>() { new string[] { "", } };
                    string linedesc = "P7:" + "U7";
                    hoja1.Cells[linedesc].Merge = true;
                    hoja1.Cells[linedesc].LoadFromArrays(lineDesc);
                    hoja1.Cells[linedesc].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[linedesc].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> DescripcionM = new List<string[]>() { new string[] { lstMaquina.descripcion.ToString() } };
                    string descripcionm = "P7:" + "U7";
                    hoja1.Cells[descripcionm].LoadFromArrays(DescripcionM);

                    List<string[]> NoSerie = new List<string[]>() { new string[] { "N° Serie", } };
                    string serie = "N8:" + "O8";
                    hoja1.Cells[serie].Merge = true;
                    hoja1.Cells[serie].LoadFromArrays(NoSerie);
                    hoja1.Cells[serie].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> lineSerie = new List<string[]>() { new string[] { "", } };
                    string lineserie = "P8:" + "U8";
                    hoja1.Cells[lineserie].Merge = true;
                    hoja1.Cells[lineserie].LoadFromArrays(lineSerie);
                    hoja1.Cells[lineserie].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[lineserie].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> serieM = new List<string[]>() { new string[] { lstMaquina.noSerie.ToString() } };
                    string seriem = "P8:" + "U8";
                    hoja1.Cells[seriem].LoadFromArrays(serieM);
                    #endregion

                    #region LADO IZQUIERDO
                    List<string[]> NoEco = new List<string[]>() { new string[] { "N° Eco: ", } };
                    string ECO = "E6:" + "E6";
                    hoja1.Cells[ECO].Merge = true;
                    hoja1.Cells[ECO].LoadFromArrays(NoEco);

                    List<string[]> lineNoEco = new List<string[]>() { new string[] { "", } };
                    string lineECO = "F6:" + "H6";
                    hoja1.Cells[lineECO].Merge = true;
                    hoja1.Cells[lineECO].LoadFromArrays(lineNoEco);
                    hoja1.Cells[lineECO].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> nECO = new List<string[]>() { new string[] { BL.noEconomico.ToString() } };
                    string neco = "F6:" + "H6";
                    hoja1.Cells[neco].LoadFromArrays(nECO);
                    hoja1.Cells[neco].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> mods = new List<string[]>() { new string[] { "Modelo:", } };
                    string mod = "E7:" + "E7";
                    hoja1.Cells[mod].Merge = true;
                    hoja1.Cells[mod].LoadFromArrays(mods);

                    List<string[]> lineMod = new List<string[]>() { new string[] { "", } };
                    string linemod = "F7:" + "H7";
                    hoja1.Cells[linemod].Merge = true;
                    hoja1.Cells[linemod].LoadFromArrays(lineMod);
                    hoja1.Cells[linemod].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> nModelo = new List<string[]>() { new string[] { lstMaquina.modeloEquipo.descripcion.ToString() } };
                    string nmodelo = "F7:" + "H7";
                    hoja1.Cells[nmodelo].LoadFromArrays(nModelo);
                    hoja1.Cells[nmodelo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> Frente = new List<string[]>() { new string[] { "Frente:", } };
                    string PosicionFrente = "E8:" + "E8";
                    hoja1.Cells[PosicionFrente].Merge = true;
                    hoja1.Cells[PosicionFrente].LoadFromArrays(Frente);

                    List<string[]> lineFrente = new List<string[]>() { new string[] { "", } };
                    string linefrente = "F8:" + "H8";
                    hoja1.Cells[linefrente].Merge = true;
                    hoja1.Cells[linefrente].LoadFromArrays(lineFrente);
                    hoja1.Cells[linefrente].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[linefrente].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> nFrente = new List<string[]>() { new string[] { objCC.descripcion.ToString() } };
                    string nfrente = "F8:" + "H8";
                    hoja1.Cells[nfrente].LoadFromArrays(nFrente);
                    hoja1.Cells[nfrente].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region LADO DERECHO
                    List<string[]> Fecha = new List<string[]>() { new string[] { "Fecha: ", } };
                    string fecha = "AC4:" + "AC4";
                    hoja1.Cells[fecha].Merge = true;
                    hoja1.Cells[fecha].LoadFromArrays(Fecha);

                    List<string[]> lineFecha = new List<string[]>() { new string[] { "", } };
                    string linefecha = "AD4:" + "AF4";
                    hoja1.Cells[linefecha].Merge = true;
                    hoja1.Cells[linefecha].LoadFromArrays(lineFecha);
                    hoja1.Cells[linefecha].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[tituloInforme].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> nFecha = new List<string[]>() { new string[] { DateTime.Now.ToString() } };
                    string nfecha = "AD4:" + "AF4";
                    hoja1.Cells[nfecha].LoadFromArrays(nFecha);
                    hoja1.Cells[nfecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> Marca = new List<string[]>() { new string[] { "Marca: ", } };
                    string marca = "AC7:" + "AC7";
                    hoja1.Cells[marca].Merge = true;
                    hoja1.Cells[marca].LoadFromArrays(Marca);

                    List<string[]> lineMarca = new List<string[]>() { new string[] { "", } };
                    string linemarca = "AD7:" + "AF7";
                    hoja1.Cells[linemarca].Merge = true;
                    hoja1.Cells[linemarca].LoadFromArrays(lineFecha);
                    hoja1.Cells[linemarca].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[linemarca].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> nMarca = new List<string[]>() { new string[] { lstMaquina.marca.descripcion } };
                    string nmarca = "AD7:" + "AF7";
                    hoja1.Cells[nmarca].LoadFromArrays(nMarca);

                    List<string[]> Horometro = new List<string[]>() { new string[] { "Horometro: ", } };
                    string horometro = "AC8:" + "AC8";
                    hoja1.Cells[horometro].Merge = true;
                    hoja1.Cells[horometro].LoadFromArrays(Horometro);

                    List<string[]> lineHorometro = new List<string[]>() { new string[] { "", } };
                    string linehorometro = "AD8:" + "AF8";
                    hoja1.Cells[linehorometro].Merge = true;
                    hoja1.Cells[linehorometro].LoadFromArrays(lineHorometro);
                    hoja1.Cells[linehorometro].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[linehorometro].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> nHora = new List<string[]>() { new string[] { BL.horas.ToString() } };
                    string nhora = "AD8:" + "AF8";
                    hoja1.Cells[nhora].LoadFromArrays(nHora);
                    hoja1.Cells[nhora].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region ENCABEZADO


                    List<string[]> headerrow = new List<string[]>() { new string[] { 
                        "Descripcion ",
                    } };

                    string titulorango = "A11:" + "F12";
                    hoja1.Cells[titulorango].Merge = true;
                    hoja1.Cells[titulorango].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorango].LoadFromArrays(headerrow);
                    hoja1.Cells[titulorango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorango].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorango].Style.Font.Bold = true;
                    hoja1.Cells[titulorango].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> sistema = new List<string[]>() { new string[] { 
                        "Sistema a reparar",
                    } };

                    string titulorangoSistema = "G11:" + "I12";
                    hoja1.Cells[titulorangoSistema].Merge = true;
                    hoja1.Cells[titulorangoSistema].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoSistema].LoadFromArrays(sistema);
                    hoja1.Cells[titulorangoSistema].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoSistema].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoSistema].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoSistema].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoSistema].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoSistema].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> requisicion = new List<string[]>() { new string[] { 
                        "No. de requisicion",
                    } };


                    string titulorangorequisicion = "J11:" + "K12";
                    hoja1.Cells[titulorangorequisicion].Merge = true;
                    hoja1.Cells[titulorangorequisicion].LoadFromArrays(requisicion);
                    hoja1.Cells[titulorangorequisicion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangorequisicion].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangorequisicion].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangorequisicion].Style.Font.Bold = true;
                    hoja1.Cells[titulorangorequisicion].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangorequisicion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangorequisicion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> compra = new List<string[]>() { new string[] { 
                        "No. de orden de compra ",
                    } };


                    string titulorangoCompra = "L11:" + "M12";
                    hoja1.Cells[titulorangoCompra].Merge = true;
                    hoja1.Cells[titulorangoCompra].LoadFromArrays(compra);
                    hoja1.Cells[titulorangoCompra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoCompra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoCompra].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoCompra].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoCompra].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoCompra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoCompra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(12).Width = 13.43;


                    List<string[]> Folio = new List<string[]>() { new string[] { 
                        "N° Folio ",
                    } };

                    string titulorangoFolio = "N11:" + "N12";
                    hoja1.Cells[titulorangoFolio].Merge = true;
                    hoja1.Cells[titulorangoFolio].LoadFromArrays(Folio);
                    hoja1.Cells[titulorangoFolio].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoFolio].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoFolio].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoFolio].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoFolio].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoFolio].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoFolio].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(14).Width = 13.43;


                    List<string[]> FechaRefacciones = new List<string[]>() { new string[] { 
                        "Fecha de surtimiento de refacciones",                           
                    } };

                    string titulorangoFecha = "O11:" + "P11";
                    hoja1.Cells[titulorangoFecha].Merge = true;
                    hoja1.Cells[titulorangoFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoFecha].LoadFromArrays(FechaRefacciones);
                    hoja1.Cells[titulorangoFecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoFecha].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoFecha].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoFecha].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(15).Width = 17.50;
                    hoja1.Column(16).Width = 15;
                    hoja1.Row(11).Height = 35;
                    hoja1.Row(12).Height = 13;

                    List<string[]> FechaRefaccionesProm = new List<string[]>() { new string[] { 
                        "Prom.",
                    } };

                    string titulorangoFecha1 = "O12:" + "O12";
                    hoja1.Cells[titulorangoFecha1].Merge = true;
                    hoja1.Cells[titulorangoFecha1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoFecha1].LoadFromArrays(FechaRefaccionesProm);
                    hoja1.Cells[titulorangoFecha1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoFecha1].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoFecha1].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoFecha1].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoFecha1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoFecha1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    List<string[]> FechaRefaccionesReal = new List<string[]>() { new string[] { 
                        "Real.",
                    } };

                    string titulorangoFecha2 = "P12:" + "P12";
                    hoja1.Cells[titulorangoFecha2].Merge = true;
                    hoja1.Cells[titulorangoFecha2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoFecha2].LoadFromArrays(FechaRefaccionesReal);
                    hoja1.Cells[titulorangoFecha2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoFecha2].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoFecha2].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoFecha2].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoFecha2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoFecha2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> avance = new List<string[]>() { new string[] { 
                        "Avance %",
                    } };

                    string titulorangoAvance = "Q11:" + "Z11";
                    hoja1.Cells[titulorangoAvance].Merge = true;
                    hoja1.Cells[titulorangoAvance].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoAvance].LoadFromArrays(avance);
                    hoja1.Cells[titulorangoAvance].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoAvance].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoAvance].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoAvance].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoAvance].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoAvance].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> porsentaje10 = new List<string[]>() { new string[] { 
                        "10",                     
                    } };

                    string por10 = "Q12:" + "Q12";
                    hoja1.Cells[por10].LoadFromArrays(porsentaje10);
                    hoja1.Cells[por10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por10].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por10].Style.Font.Bold = true;
                    hoja1.Cells[por10].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(17).Width = 3.15;

                    List<string[]> porsentaje20 = new List<string[]>() { new string[] { 
                        "20",                     
                    } };

                    string por20 = "R12:" + "R12";
                    hoja1.Cells[por20].LoadFromArrays(porsentaje20);
                    hoja1.Cells[por20].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por20].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por20].Style.Font.Bold = true;
                    hoja1.Cells[por20].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por20].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(18).Width = 3.15;

                    List<string[]> porsentaje30 = new List<string[]>() { new string[] { 
                        "30",                     
                    } };

                    string por30 = "S12:" + "S12";
                    hoja1.Cells[por30].LoadFromArrays(porsentaje30);
                    hoja1.Cells[por30].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por30].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por30].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por30].Style.Font.Bold = true;
                    hoja1.Cells[por30].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por30].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por30].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(19).Width = 3.15;

                    List<string[]> porsentaje40 = new List<string[]>() { new string[] { 
                        "40",                     
                    } };

                    string por40 = "T12:" + "T12";
                    hoja1.Cells[por40].LoadFromArrays(porsentaje40);
                    hoja1.Cells[por40].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por40].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por40].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por40].Style.Font.Bold = true;
                    hoja1.Cells[por40].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por40].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por40].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(20).Width = 3.15;

                    List<string[]> porsentaje50 = new List<string[]>() { new string[] { 
                        "50",                     
                    } };

                    string por50 = "U12:" + "U12";
                    hoja1.Cells[por50].LoadFromArrays(porsentaje50);
                    hoja1.Cells[por50].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por50].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por50].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por50].Style.Font.Bold = true;
                    hoja1.Cells[por50].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por50].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por50].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(21).Width = 3.15;

                    List<string[]> porsentaje60 = new List<string[]>() { new string[] { 
                        "60",                     
                    } };

                    string por60 = "V12:" + "V12";
                    hoja1.Cells[por60].LoadFromArrays(porsentaje60);
                    hoja1.Cells[por60].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por60].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por60].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por60].Style.Font.Bold = true;
                    hoja1.Cells[por60].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por60].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por60].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(22).Width = 3.15;

                    List<string[]> porsentaje70 = new List<string[]>() { new string[] { 
                        "70",                     
                    } };

                    string por70 = "W12:" + "W12";
                    hoja1.Cells[por70].LoadFromArrays(porsentaje70);
                    hoja1.Cells[por70].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por70].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por70].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por70].Style.Font.Bold = true;
                    hoja1.Cells[por70].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por70].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por70].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(23).Width = 3.15;

                    List<string[]> porsentaje80 = new List<string[]>() { new string[] { 
                        "80",                     
                    } };

                    string por80 = "X12:" + "X12";
                    hoja1.Cells[por80].LoadFromArrays(porsentaje80);
                    hoja1.Cells[por80].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por80].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por80].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por80].Style.Font.Bold = true;
                    hoja1.Cells[por80].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por80].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por80].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(24).Width = 3.15;

                    List<string[]> porsentaje90 = new List<string[]>() { new string[] { 
                        "90",                     
                    } };

                    string por90 = "Y12:" + "Y12";
                    hoja1.Cells[por90].LoadFromArrays(porsentaje90);
                    hoja1.Cells[por90].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por90].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por90].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por90].Style.Font.Bold = true;
                    hoja1.Cells[por90].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por90].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por90].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(25).Width = 2.30;

                    List<string[]> porsentaje100 = new List<string[]>() { new string[] { 
                        "100",                     
                    } };

                    string por100 = "Z12:" + "Z12";
                    hoja1.Cells[por100].LoadFromArrays(porsentaje100);
                    hoja1.Cells[por100].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[por100].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[por100].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[por100].Style.Font.Bold = true;
                    hoja1.Cells[por100].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[por100].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Cells[por100].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja1.Column(26).Width = 3.15;

                    ////DS

                    List<string[]> FechaTerminacion = new List<string[]>() { new string[] { 
                        "Fecha de terminacion",                           
                    } };

                    string titulorangoTerminacion = "AA11:" + "AB11";
                    hoja1.Cells[titulorangoTerminacion].Merge = true;
                    hoja1.Cells[titulorangoTerminacion].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoTerminacion].LoadFromArrays(FechaTerminacion);
                    hoja1.Cells[titulorangoTerminacion].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoTerminacion].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoTerminacion].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoTerminacion].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoTerminacion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoTerminacion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(27).Width = 17.50;
                    hoja1.Column(28).Width = 15;

                    List<string[]> FechaTerminacionProm = new List<string[]>() { new string[] { 
                        "Prom",
                    } };

                    string titulorangoTemp = "AA12:" + "AA12";
                    hoja1.Cells[titulorangoTemp].Merge = true;
                    hoja1.Cells[titulorangoTemp].LoadFromArrays(FechaTerminacionProm);
                    hoja1.Cells[titulorangoTemp].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoTemp].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoTemp].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoTemp].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoTemp].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoTemp].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoTemp].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    List<string[]> FechaTerminacionReal = new List<string[]>() { new string[] { 
                        "Real",
                    } };

                    string titulorangoTemp1 = "AB12:" + "AB12";
                    hoja1.Cells[titulorangoTemp1].Merge = true;
                    hoja1.Cells[titulorangoTemp1].LoadFromArrays(FechaTerminacionReal);
                    hoja1.Cells[titulorangoTemp1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoTemp1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoTemp1].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoTemp1].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoTemp1].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoTemp1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoTemp1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> costoUSD = new List<string[]>() { new string[] { 
                        "Costo de refacciones U.S.D.",
                    } };

                    string titulorangoCostoUSB = "AC11:" + "AC12";
                    hoja1.Cells[titulorangoCostoUSB].Merge = true;
                    hoja1.Cells[titulorangoCostoUSB].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoCostoUSB].LoadFromArrays(costoUSD);
                    hoja1.Cells[titulorangoCostoUSB].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoCostoUSB].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoCostoUSB].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoCostoUSB].Style.WrapText = true;
                    hoja1.Cells[titulorangoCostoUSB].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoCostoUSB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoCostoUSB].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(29).Width = 12.11;

                    List<string[]> costoMN = new List<string[]>() { new string[] { 
                        "Costo de refacciones M.N.",
                    } };

                    string titulorangoCostoMN = "AD11:" + "AD12";
                    hoja1.Cells[titulorangoCostoMN].Merge = true;
                    hoja1.Cells[titulorangoCostoMN].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoCostoMN].LoadFromArrays(costoMN);
                    hoja1.Cells[titulorangoCostoMN].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoCostoMN].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoCostoMN].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoCostoMN].Style.WrapText = true;
                    hoja1.Cells[titulorangoCostoMN].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoCostoMN].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoCostoMN].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(30).Width = 12.20;

                    List<string[]> costoObra = new List<string[]>() { new string[] { 
                        "Costo mano de obra",
                    } };

                    string titulorangoCostoObra = "AE11:" + "AE12";
                    hoja1.Cells[titulorangoCostoObra].Merge = true;
                    hoja1.Cells[titulorangoCostoObra].LoadFromArrays(costoObra);
                    hoja1.Cells[titulorangoCostoObra].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoCostoObra].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoCostoObra].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoCostoObra].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoCostoObra].Style.WrapText = true;
                    hoja1.Cells[titulorangoCostoObra].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoCostoObra].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoCostoObra].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(31).Width = 12.20;

                    List<string[]> totalUSD = new List<string[]>() { new string[] { 
                        "Gran total en U.S.D",
                    } };

                    string titulorangoTotal = "AF11:" + "AF12";
                    hoja1.Cells[titulorangoTotal].Merge = true;
                    hoja1.Cells[titulorangoTotal].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    hoja1.Cells[titulorangoTotal].LoadFromArrays(totalUSD);
                    hoja1.Cells[titulorangoTotal].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[titulorangoTotal].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[titulorangoTotal].Style.Font.Bold = true;
                    hoja1.Cells[titulorangoTotal].Style.WrapText = true;
                    hoja1.Cells[titulorangoTotal].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[titulorangoTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[titulorangoTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Column(32).Width = 12.11;
                    #endregion

                    #region Pie de pagina
                    List<string[]> firmaela = new List<string[]>() { new string[] { 
                        "Elaboró",
                    } };

                    string firmaEla = "I50:" + "K50";
                    hoja1.Cells[firmaEla].Merge = true;
                    hoja1.Cells[firmaEla].LoadFromArrays(firmaela);
                    hoja1.Cells[firmaEla].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    List<string[]> firma = new List<string[]>() { new string[] { 
                        "Control de backlogs",
                    } };

                    string Firma = "H54:" + "L54";
                    hoja1.Cells[Firma].Merge = true;
                    hoja1.Cells[Firma].LoadFromArrays(firma);
                    hoja1.Cells[Firma].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[Firma].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> vobo = new List<string[]>() { new string[] { 
                        "Vo bo",
                    } };

                    string Vobo = "P50:" + "R50";
                    hoja1.Cells[Vobo].Merge = true;
                    hoja1.Cells[Vobo].LoadFromArrays(vobo);
                    hoja1.Cells[Vobo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    List<string[]> firmaVobo = new List<string[]>() { new string[] {     
                        "Gerencia de maquinaria"
                    } };

                    string Firmavobo = "O54:" + "V54";
                    hoja1.Cells[Firmavobo].Merge = true;
                    hoja1.Cells[Firmavobo].LoadFromArrays(firmaVobo);
                    hoja1.Cells[Firmavobo].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells[Firmavobo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    List<string[]> totalporsentaje = new List<string[]>() { new string[] { 
                        "100%",
                    } };

                    string Total = "A37:" + "A37";
                    hoja1.Cells[Total].Merge = true;
                    hoja1.Cells[Total].LoadFromArrays(totalporsentaje);

                    List<string[]> numeroBLCompletados = new List<string[]>() { new string[] { 
                        totalBL,
                    } };

                    string NumeroInstalados = "B37:" + "B37";
                    hoja1.Cells[NumeroInstalados].Merge = true;
                    hoja1.Cells[NumeroInstalados].LoadFromArrays(numeroBLCompletados);



                    List<string[]> TotalBacklogs = new List<string[]>() { new string[] { 
                        "#",
                    } };

                    string totalbacklogs = "A38:" + "A38";
                    hoja1.Cells[totalbacklogs].Merge = true;
                    hoja1.Cells[totalbacklogs].LoadFromArrays(TotalBacklogs);

                    List<string[]> numeroBLDescripcion = new List<string[]>() { new string[] { 
                        totalBLDescripcion,
                    } };

                    string NumeroDescripcion = "B38:" + "B38";
                    hoja1.Cells[NumeroDescripcion].Merge = true;
                    hoja1.Cells[NumeroDescripcion].LoadFromArrays(numeroBLDescripcion);

                    List<string[]> Texto10 = new List<string[]>() { new string[] { 
                        "10 %  = Elaboración del control de calidad, inspección visual y diagnóstico de nivel II",
                    } };

                    string texto10 = "A40:" + "J40";
                    hoja1.Cells[texto10].Merge = true;
                    hoja1.Cells[texto10].LoadFromArrays(Texto10);

                    List<string[]> Texto20 = new List<string[]>() { new string[] { 
                        "20 % = Elaboración de requesones, ordenes de trabajo  ( en borrador ) por parte del personal operativo",
                    } };

                    string texto20 = "A41:" + "J41";
                    hoja1.Cells[texto20].Merge = true;
                    hoja1.Cells[texto20].LoadFromArrays(Texto20);

                    List<string[]> Texto30 = new List<string[]>() { new string[] { 
                        "30 % = Elaboración del alcance de reparación e informe de rehabilitación ",
                    } };

                    string texto30 = "A42:" + "J42";
                    hoja1.Cells[texto30].Merge = true;
                    hoja1.Cells[texto30].LoadFromArrays(Texto30);

                    List<string[]> Texto40 = new List<string[]>() { new string[] { 
                        "40 % = Entrega de requisiciones autorizadas al almacén para su captura en sao y distribución al depto. De compras",
                    } };

                    string texto40 = "A43:" + "J43";
                    hoja1.Cells[texto40].Merge = true;
                    hoja1.Cells[texto40].LoadFromArrays(Texto40);

                    List<string[]> Texto50 = new List<string[]>() { new string[] { 
                        "50 % =  Reproceso de almacén, cotizaciones, elaboración de órdenes de compra ( gestión de almacén y compras )",
                    } };

                    string texto50 = "A44:" + "J44";
                    hoja1.Cells[texto50].Merge = true;
                    hoja1.Cells[texto50].LoadFromArrays(Texto50);

                    List<string[]> Texto60 = new List<string[]>() { new string[] { 
                        "60 % y 70 % = suministro de refacciones, materiales y trabajos con talleres externos al 100 %",
                    } };

                    string texto60 = "A45:" + "J45";
                    hoja1.Cells[texto60].Merge = true;
                    hoja1.Cells[texto60].LoadFromArrays(Texto60);

                    List<string[]> Texto70 = new List<string[]>() { new string[] { 
                        "80 % y 90 % = rehabilitación de equipo por parte del personal operativo",
                    } };

                    string texto70 = "A46:" + "J46";
                    hoja1.Cells[texto70].Merge = true;
                    hoja1.Cells[texto70].LoadFromArrays(Texto70);

                    List<string[]> Texto80 = new List<string[]>() { new string[] { 
                        "95 % = Pruebas, ajustes y diagnostico final",
                    } };

                    string texto80 = "A47:" + "J47";
                    hoja1.Cells[texto80].Merge = true;
                    hoja1.Cells[texto80].LoadFromArrays(Texto80);

                    List<string[]> Texto90 = new List<string[]>() { new string[] { 
                        "100 % =  Entrega de la unidad al área de producción",
                    } };

                    string texto90 = "A48:" + "J48";
                    hoja1.Cells[texto90].Merge = true;
                    hoja1.Cells[texto90].LoadFromArrays(Texto90);



                    #endregion

                    #region dibujar barra en avance
                    var cellData = new List<object[]>();
                    int contador = 12;
                    decimal totalMX = 0;
                    foreach (var item in lstbacklogs)
                    {
                        var arregloValores = new object[32];
                        arregloValores[0] = (string)item.descripcion != "" ? (string)item.descripcion : "--";
                        arregloValores[6] = (string)item.subconjunto.descripcion != "" ? (string)item.subconjunto.descripcion : "--";
                        arregloValores[9] = (int)lstRequisicionesDTO.Where(x => x.idBackLog == item.id).Count() > 0 ? (string)lstRequisicionesDTO.Where(x => x.idBackLog == item.id).FirstOrDefault().numRequisicion : "--";
                        arregloValores[11] = (int)lstOrdenesComprasDTO.Where(x => x.idBackLog == item.id).Count() > 0 ? (string)lstOrdenesComprasDTO.Where(x => x.idBackLog == item.id).FirstOrDefault().numOC : "--";
                        arregloValores[13] = (string)item.folioBL != "" ? (string)item.folioBL : "--";
                        arregloValores[14] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                        arregloValores[15] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                        arregloValores[26] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                        arregloValores[27] = (string)item.fechaInspeccion.ToString("dd/MM/yyyy");
                        arregloValores[28] = 0;

                        string strQuery = string.Empty;

                        #region SE OBTIENE EL CC DEL ECONOMICO
                        string ccEconomico = string.Empty;
                        if (!string.IsNullOrEmpty(item.noEconomico))
                        {
                            strQuery = string.Format("SELECT cc FROM cc WHERE descripcion = '{0}'", item.noEconomico);
                            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery);
                            List<dynamic> objCCEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);
                            if (objCCEconomico.Count() > 0)
                                ccEconomico = objCCEconomico[0].cc;
                        }
                        #endregion

                        #region SE OBTIENE EL TOTAL EN BASE A LOS INSUMOS QUE CONTENGA EL BL
                        if (!string.IsNullOrEmpty(ccEconomico))
                        {
                            // SE OBTIENE LOS INSUMOS DEL BL
                            strQuery = string.Format("SELECT insumo FROM tblBL_Partes WHERE idBackLog = {0}", item.id);
                            List<int> lstInsumosDTO = _context.Select<int>(new DapperDTO { baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, consulta = strQuery }).ToList();

                            // SE OBTIENE LAS NUM_REQUISICIONES DEL BL
                            strQuery = string.Format("SELECT numRequisicion FROM tblBL_Requisiciones WHERE idBackLog = {0}", item.id);
                            List<int> lstRequisicionesDTO2 = _context.Select<int>(new DapperDTO { baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, consulta = strQuery }).ToList();

                            // SE OBTIENE EL DETALLE DE COMPRA EN BASE AL CC, NUM_OC, NUM_REQ E INSUMO
                            if (lstRequisicionesDTO2.Count() > 0)
                            {
                                for (int i = 0; i < lstRequisicionesDTO2.Count(); i++)
                                {
                                    int numReq = lstRequisicionesDTO2[i];
                                    int insumo = 0;

                                    decimal totalInsumo = 0;
                                    try
                                    {
                                        insumo = lstInsumosDTO[i];
                                        strQuery = string.Format(@"SELECT ISNULL(SUM(CASE WHEN t2.moneda = 2 
                                                                THEN (t2.tipo_cambio * (CASE WHEN t1.importe IS NOT NULL THEN t1.importe ELSE 0 END)) 
                                                                ELSE (CASE WHEN t1.importe IS NULL THEN t1.importe ELSE 0 END)
                                                                END), 0) AS totalInsumo
                                                                    FROM so_orden_compra_det AS t1
                                                                    INNER JOIN so_orden_compra AS t2 ON t1.cc = t2.cc AND t1.numero = t2.numero
                                                                        WHERE t1.cc = '{0}' AND t1.num_requisicion = {1} AND t1.insumo = {2}", ccEconomico, numReq, insumo);
                                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                                        odbc.consulta = String.Format(strQuery);
                                        List<decimal> objImporteTotalMX = _contextEnkontrol.Select<decimal>(EnkontrolEnum.ArrenProd, odbc);
                                        if (objImporteTotalMX.Count() > 0)
                                            totalInsumo = (decimal)objImporteTotalMX[0];

                                        totalMX += (decimal)totalInsumo;

                                        #region EN CASO QUE EL PPTO SEA 0 EN ENKONTROL, SE OBTIENE EL PPTO ESTIMADO REGISTRADO
                                        if (totalMX == 0)
                                        {
                                            for (int x = 0; x < lstInsumosDTO.Count(); x++)
                                            {
                                                insumo = lstInsumosDTO[x] > 0 ? Convert.ToInt32(lstInsumosDTO[x]) : 0;
                                                if (insumo > 0)
                                                {
                                                    decimal total = GetCostoPromedio(601, insumo);
                                                    if (total > 0)
                                                        totalMX += total;
                                                }
                                            }

                                            if (totalMX <= 0)
                                                totalMX += item.presupuestoEstimado;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < lstInsumosDTO.Count(); i++)
                                {
                                    int insumo = lstInsumosDTO[i] > 0 ? Convert.ToInt32(lstInsumosDTO[i]) : 0;
                                    if (insumo > 0)
                                    {
                                        decimal total = GetCostoPromedio(601, insumo);
                                        if (total > 0)
                                            totalMX += total;
                                    }
                                }

                                if (totalMX <= 0)
                                    totalMX += item.presupuestoEstimado;
                            }
                        }
                        arregloValores[29] = (decimal)totalMX;
                        totalMX = 0;
                        #endregion

                        cellData.Add(arregloValores);

                        hoja1.Cells[13, 1].LoadFromArrays(cellData);
                        contador++;
                        hoja1.Cells[contador, 17, contador, 26].Merge = true;

                        var cellMerge = hoja1.MergedCells[contador, 26];

                        switch (item.idEstatus)
                        {
                            case (int)EstatusBackLogEnum.ElaboracionInspeccion: 
                                    hoja1.Cells[cellMerge].Value = 20; 
                                break;
                            case (int)EstatusBackLogEnum.ElaboracionRequisicion: 
                                    hoja1.Cells[cellMerge].Value = 40; 
                                break;
                            case (int)EstatusBackLogEnum.ElaboracionOC:
                                hoja1.Cells[cellMerge].Value = 50;
                                break;
                            case (int)EstatusBackLogEnum.SuministroRefacciones:
                                hoja1.Cells[cellMerge].Value = 60;
                                break;
                            case (int)EstatusBackLogEnum.RehabilitacionProgramada:
                                hoja1.Cells[cellMerge].Value = 60;
                                break;
                            case (int)EstatusBackLogEnum.ProcesoInstalacion:
                                hoja1.Cells[cellMerge].Value = 90;
                                break;
                            case (int)EstatusBackLogEnum.BackLogsInstalado:
                                hoja1.Cells[cellMerge].Value = 100;
                                break;
                        }

                        ExcelAddress columnasAvance = new ExcelAddress(cellMerge);
                        var reglaBarraProgresoAvance = hoja1.ConditionalFormatting.AddDatabar(columnasAvance, ColorTranslator.FromHtml("#1E90FF"));

                        reglaBarraProgresoAvance.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightDown;
                        reglaBarraProgresoAvance.LowValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                        reglaBarraProgresoAvance.LowValue.Value = 0;
                        reglaBarraProgresoAvance.HighValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                        reglaBarraProgresoAvance.HighValue.Value = 100;
                    }
                    #endregion

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;
                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();
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
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.GENERAR_EXCEL, 0, new { AreaCuenta = AreaCuenta });
                return null;
            }
        }

        public int variable(string var)
        {
            return Convert.ToInt32(var);
        }

        public List<ComboDTO> FillCboResponsablesAnalisisBLResponsable(string areaCuenta)
        {
            List<ComboDTO> lstUsuarios = new List<ComboDTO>();
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(areaCuenta))
                    throw new Exception("Ocurrió un error al obtener listado de responsables.");
                #endregion

                #region SE OBTIENE LISTADO DE USUARIOS ASIGNADOS A UN BL EN BASE AL AREA CUENTA
                ComboDTO objComboDTO = new ComboDTO();
                List<ComboDTO> lstUsuariosCP = new List<ComboDTO>();
                List<ComboDTO> lstUsuariosARR = new List<ComboDTO>();

                List<int> lstResponsablesID = _context.tblBL_CatBackLogs.Where(w => w.areaCuenta == areaCuenta && w.esActivo).Select(s => s.idUsuarioResponsable).ToList();

                
                List<tblRH_EK_Empleados> lstUsuariosCP_Empleados = _context.tblRH_EK_Empleados.Where(w => lstResponsablesID.Contains(w.clave_empleado)).ToList();
                foreach (var item in lstUsuariosCP_Empleados)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.clave_empleado.ToString();
                    objComboDTO.Text = string.Format("{0} {1} {2}", item.nombre, item.ape_paterno, item.ape_materno);
                    lstUsuariosCP.Add(objComboDTO);
                }

                using (var _ctxCP = new MainContext(EmpresaEnum.Construplan))
                {
                    List<tblRH_EK_Empleados> lstUsuariosARR_Empleados = _ctxCP.tblRH_EK_Empleados.Where(w => lstResponsablesID.Contains(w.clave_empleado)).ToList();
                    foreach (var item in lstUsuariosARR_Empleados)
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.clave_empleado.ToString();
                        objComboDTO.Text = string.Format("{0} {1} {2}", item.nombre, item.ape_paterno, item.ape_materno);
                        lstUsuariosARR.Add(objComboDTO);
                    }
                }

                lstUsuarios.AddRange(lstUsuariosCP);
                lstUsuarios.AddRange(lstUsuariosARR);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboResponsablesAnalisisBLResponsable", e, AccionEnum.CONSULTA, 0, new { areaCuenta = areaCuenta });
            }
            return lstUsuarios;
        }

        public BackLogsDTO GetBLOTVacia(int idBL)
        {
            try
            {
                BackLogsDTO objBL = _context.tblBL_CatBackLogs.Where(w => w.id == idBL && w.esActivo).Select(s => new BackLogsDTO
                {
                    noEconomico = s.noEconomico.Trim(),
                    descripcion = s.descripcion.Trim()
                }).FirstOrDefault();

                return objBL;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetBLOTVacia", e, AccionEnum.CONSULTA, 0, idBL);
                return null;
            }
        }

        public Dictionary<string, object> GetTotalInfoEconomicoBL(BackLogsDTO objParamsDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    if (string.IsNullOrEmpty(objParamsDTO.areaCuenta)) { throw new Exception("Ocurrió un error al obtener la información del Económico."); }
                    if (string.IsNullOrEmpty(objParamsDTO.noEconomico)) { throw new Exception("Ocurrió un error al obtener la información del Económico."); }

                    List<BackLogsDTO> lstBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.noEconomico, t1.areaCuenta, t1.folioBL, t1.descripcion AS descripcionBL, t1.fechaInspeccion, t3.descripcion AS conjunto, t3.abreviacion AS conjuntoAbreviacion,
		                                    t1.idEstatus, t1.fechaInstaladoBL, t1.fechaCreacionBL
			                                    FROM tblBL_CatBackLogs AS t1
			                                    INNER JOIN tblBL_CatSubconjuntos AS t2 ON t2.id = t1.idSubconjunto
			                                    INNER JOIN tblBL_CatConjuntos AS t3 ON t3.id = t2.idConjunto
				                                    WHERE t1.areaCuenta = @areaCuenta AND t1.noEconomico = @noEconomico AND t1.esActivo = @esActivo
					                                    ORDER BY t1.id DESC",
                        parametros = new { areaCuenta = objParamsDTO.areaCuenta, noEconomico = objParamsDTO.noEconomico, esActivo = true }
                    }).ToList();

                    foreach (var item in lstBL)
                    {
                        item.strCostoTotalBL = GetCostoFolioBL(item.folioBL).ToString("C");
                        item.descripcionBL = PersonalUtilities.PrimerLetraMayuscula(item.descripcionBL);
                        item.conjunto = PersonalUtilities.PrimerLetraMayuscula(item.conjunto);
                        item.conjuntoAbreviacion = PersonalUtilities.PrimerLetraMayuscula(item.conjuntoAbreviacion);

                        switch (item.idEstatus)
                        {
                            case (int)EstatusBackLogEnum.ElaboracionInspeccion: item.estatus = "<button class='btn boton-estatus-20'>20%</button>"; break;
                            case (int)EstatusBackLogEnum.ElaboracionRequisicion: item.estatus = "<button class='btn boton-estatus-40'>40%</button>"; break;
                            case (int)EstatusBackLogEnum.ElaboracionOC: item.estatus = "<button class='btn boton-estatus-50'>50%</button>"; break;
                            case (int)EstatusBackLogEnum.SuministroRefacciones: item.estatus = "<button class='btn boton-estatus-60'>60%</button>"; break;
                            case (int)EstatusBackLogEnum.RehabilitacionProgramada: item.estatus = "<button class='btn boton-estatus-80'>80%</button>"; break;
                            case (int)EstatusBackLogEnum.ProcesoInstalacion: item.estatus = "<button class='btn boton-estatus-90'>90%</button>"; break;
                            case (int)EstatusBackLogEnum.BackLogsInstalado: item.estatus = "<button class='btn boton-estatus-100'>100%</button>"; break;
                        }

                        #region SE OBTIENE EL TIEMPO PROMEDIO EN DÍAS PARA QUE UN BL LLEGUE A 100%
                        DateTime fechaInicioBL = item.fechaCreacionBL;
                        DateTime fechaFinBL = item.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado ? Convert.ToDateTime(item.fechaInstaladoBL) : DateTime.Now;
                        if (fechaInicioBL != null && fechaFinBL != null)
                        {
                            TimeSpan timeSpan = fechaFinBL - fechaInicioBL;
                            double daysDifference = timeSpan.TotalDays;
                            double daysCount = daysDifference;
                            //item.diasTotales = Convert.ToInt32((daysCount / 2));
                            item.diasTotales = Convert.ToInt32(daysCount);
                        }
                        if (item.diasTotales == 0)
                            item.diasTotales = 1;
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstBL);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        #region BACKLOGS TMC
        public List<ComboDTO> FillAreasCuentasTMC()
        {
            List<int> lstID = new List<int>();
            lstID.Add(74);
            //lstID.Add(47);
            var data = _context.tblP_CC.Where(x => lstID.Contains(x.id)).Select(x => new ComboDTO
            {
                Value = x.areaCuenta,
                Text = x.descripcion
            }).ToList();

            if (vSesiones.sesionEmpresaActual == 3)
            {
                lstID.Add(8);//Para pruebas porque no hay tmc
                data = _context.tblP_CC.Where(x => lstID.Contains(x.id)).Select(x => new ComboDTO
                {
                    Value = x.areaCuenta,
                    Text = x.descripcion
                }).ToList();
            }
            else if (vSesiones.sesionEmpresaActual == 6)
            {
                lstID.Add(2189);//Para pruebas porque no hay tmc
                data = _context.tblP_CC.Where(x => lstID.Contains(x.id)).Select(x => new ComboDTO
                {
                    Value = x.areaCuenta,
                    Text = x.descripcion
                }).ToList();
            }
            return data.ToList();
        }

        public List<ComboDTO> FillTipoMaquinariaTMC()
        {
            var dataTipoMaquinariaTMC = _context.tblM_CatTipoMaquinaria.Where(x => x.estatus).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

            return dataTipoMaquinariaTMC;
        }

        public List<ProgramaInspTMCDTO> GetProgramacionInspeccionTMC(ProgramaInspTMCDTO objFiltro) //TODO
        {
            try
            {
                int tipoMaquina = objFiltro != null ? objFiltro.tipoMaquina : 0;

                #region SE OBTIENE EL GRUPO DE LAS MAQUINAS
                List<tblM_CatGrupoMaquinaria> lstGrupoMaquinas = _context.tblM_CatGrupoMaquinaria.Where(x => (tipoMaquina > 0 ? x.tipoEquipoID == tipoMaquina : true) && x.estatus).ToList();
                List<int> lstGrupoID = new List<int>();
                if (lstGrupoMaquinas.Count() > 0)
                {
                    foreach (var item in lstGrupoMaquinas)
                    {
                        lstGrupoID.Add(item.id);
                    }
                }
                #endregion

                #region SE OBTIENE LISTADO DE NO. ECONOMICOS EN BASE AL GRUPO DE MAQUINAS
                List<tblM_CatMaquina> lstCatMaquinas = new List<tblM_CatMaquina>();
                if (lstGrupoID.Count() > 0)
                    lstCatMaquinas = _context.tblM_CatMaquina.Where(x => x.centro_costos == objFiltro.areaCuenta && lstGrupoID.Contains(x.grupoMaquinariaID) && x.estatus == 1).ToList();
                #endregion

                #region SE OBTIENE LISTADO DEL HOROMETRO ACTUAL DE LAS MAQUINAS
                List<string> lstNoEconomicos = new List<string>();
                foreach (var item in lstCatMaquinas)
                {
                    lstNoEconomicos.Add(item.noEconomico.Trim());
                }
                List<tblM_CapHorometro> lstCapHorometro = _context.tblM_CapHorometro.Where(x => lstNoEconomicos.Contains(x.Economico.Trim())).OrderByDescending(s => s.Fecha).ToList();
                #endregion

                #region SE OBTIENE LAS MAQUINAS EN PROGRAMA
                List<tblBL_InspeccionesTMC> lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(x => x.esActivo).ToList();
                List<string> lstNoEconomicosEnPrograma = new List<string>();
                foreach (var item in lstInspeccionesTMC)
                {
                    lstNoEconomicosEnPrograma.Add(item.noEconomico);
                }
                #endregion

                #region SE OBTIENE LAS MAQUINAS EN PROGRAMACION (INICIAL)
                int partida = 1;
                List<ProgramaInspTMCDTO> lstMaquinasEnProgramacion = lstCatMaquinas.Where(w => !lstNoEconomicosEnPrograma.Contains(w.noEconomico)).Select(x => new ProgramaInspTMCDTO
                {
                    id = x.id,
                    partida = partida++,
                    noEconomico = x.noEconomico,
                    descripcion = x.descripcion,
                    modelo = x.modeloEquipo.descripcion,
                    horas = lstCapHorometro.Where(w => w.Economico == x.noEconomico).Count() > 0 ? lstCapHorometro.Where(w => w.Economico == x.noEconomico).FirstOrDefault().Horometro : 0
                }).ToList();
                #endregion

                return lstMaquinasEnProgramacion;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetProgramacionInspeccionTMC", e, AccionEnum.CONSULTA, 0, objFiltro);
                return null;
            }
        }

        public bool CrearProgramacionTMC(List<tblBL_InspeccionesTMC> objInsp)
        {
            try
            {
                #region SE OBTIENE EL ID DE LA MAQUINA
                List<string> lstNoEconomicos = new List<string>();
                foreach (var item in objInsp)
                {
                    lstNoEconomicos.Add(item.noEconomico);
                }
                List<tblM_CatMaquina> lstMaquinas = _context.tblM_CatMaquina.Where(x => lstNoEconomicos.Contains(x.noEconomico) && x.estatus == 1).ToList();
                #endregion

                #region
                foreach (var item in objInsp)
                {
                    item.idCatMaquina = lstMaquinas.Where(x => x.noEconomico == item.noEconomico).FirstOrDefault().id;
                    item.fechaCreacionInsp = DateTime.Now;
                    item.fechaModificacionInsp = DateTime.Now;
                    item.esActivo = true;
                }
                #endregion

                _context.tblBL_InspeccionesTMC.AddRange(objInsp);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearProgramacionTMC", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objInsp));
                return false;
            }
        }

        public bool EliminarProgramaInspTMC(int id)
        {
            try
            {
                tblBL_InspeccionesTMC objEliminar = _context.tblBL_InspeccionesTMC.Where(x => x.id == id).FirstOrDefault();
                objEliminar.esActivo = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarProgramaInspTMC", e, AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(id));
                return false;
            }
        }

        public List<ProgramaInspTMCDTO> GetProgramaInspeccionTMC(ProgramaInspTMCDTO objFiltro)
        {
            try
            {
                #region SE OBTIENE LAS MAQUINAS YA EN PROGRAMA
                int idMotivo = -1;
                if (objFiltro.motivo == "0" || objFiltro.motivo == "1")
                    idMotivo = Convert.ToInt32(objFiltro.motivo);

                List<tblBL_InspeccionesTMC> lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(w => idMotivo > -1 ? w.idMotivo == idMotivo : true && w.esActivo).ToList();

                int partida = 1;
                List<ProgramaInspTMCDTO> lstMaquinasEnPrograma = lstInspeccionesTMC.Select(x => new ProgramaInspTMCDTO
                {
                    id = x.id,
                    partida = partida++,
                    noEconomico = x.noEconomico,
                    descripcion = x.lstCatMaquinas.descripcion,
                    modelo = x.lstCatMaquinas.modeloEquipo.descripcion,
                    horas = x.horas,
                    motivo = x.idMotivo == 0 ? "OBRA" : "VENTA",
                    fechaProgramacion = x.fechaCreacionInsp,
                    fechaRequerido = x.fechaRequerido,
                    fechaPromesa = x.fechaPromesa
                }).ToList();
                #endregion

                return lstMaquinasEnPrograma.OrderByDescending(o => o.id).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetProgramaInspeccionTMC", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool ActualizarProgramaInspeccionTMC(tblBL_InspeccionesTMC objInsp)
        {
            try
            {
                tblBL_InspeccionesTMC objActualizar = _context.tblBL_InspeccionesTMC.First(x => x.id == objInsp.id && x.esActivo);
                objActualizar.fechaRequerido = objInsp.fechaRequerido;
                objActualizar.fechaPromesa = objInsp.fechaPromesa;
                objActualizar.fechaModificacionInsp = DateTime.Now;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarProgramaInspeccionTMC", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public Dictionary<string, object> GetDatosGraficasBLTMC(BackLogsDTO objBL)
        {
            var resultados = new Dictionary<string, object>();
            try
            {
                List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.areaCuenta == objBL.areaCuenta && x.tipoBL == (int)TipoBackLogEnum.TMC && x.fechaCreacionBL.Year == DateTime.Now.Year).ToList();

                #region GRAFICA: ESTATUS DE BACKLOGS GRAFICA PASTEL
                var resultadosCantEstatusTMC = GetEstatusBackLogsTMC(lstBL);
                resultados.Add("resultadosCantEstatusTMC", resultadosCantEstatusTMC);
                #endregion

                #region GRAFICA: ESTATUS DE BACKLOGS GRAFICA LINEAS
                var resultadosCantEstatusLineasTMC = GetEstatusBackLogsLineasTMC(lstBL);
                resultados.Add("resultadosCantEstatusLineasTMC", resultadosCantEstatusLineasTMC);
                #endregion

                #region GRAFICA: ACUMULADO AÑOS ANTERIORES
                var resultadosAñosAnterioresTMC = GetEstatusBackLogsAñosAnterioresTMC(lstBL, objBL.areaCuenta);
                resultados.Add("resultadosAñosAnterioresTMC", resultadosAñosAnterioresTMC);
                #endregion

                #region TIEMPO PROMEDIO TABLA INFERIOR DASHBOARD
                var resultadosTiempoPromedio = GetTiempoPromedioBLTMC(lstBL);
                resultados.Add("resultadosTiempoPromedio", resultadosTiempoPromedio);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDatosGraficasBLTMC", e, AccionEnum.CONSULTA, 0, objBL);
                return null;
            }
            return resultados;
        }

        public Dictionary<string, object> GetTiempoPromedioBLTMC(List<tblBL_CatBackLogs> lstBL)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string areaCuenta = lstBL[0].areaCuenta;
                List<int> lstIDBL = new List<int>();
                foreach (var item in lstBL)
                {
                    lstIDBL.Add(item.id);
                }
                List<tblBL_BitacoraEstatusBL> lstBitacora = _context.tblBL_BitacoraEstatusBL.Where(w => w.areaCuenta == areaCuenta && lstIDBL.Contains(w.idBL) && w.esActivo).ToList();

                List<int> lstTiempoPromedio = new List<int>();
                int tiempoPromedioEstatus20 = 0,
                    tiempoPromedioEstatus40 = 0,
                    tiempoPromedioEstatus50 = 0,
                    tiempoPromedioEstatus60 = 0,
                    tiempoPromedioEstatus80 = 0,
                    tiempoPromedioEstatus90 = 0,
                    tiempoPromedioEstatus100 = 0,
                    tiempoTotalPromedio = 0;

                tiempoPromedioEstatus20 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.ElaboracionPresupuesto && w.esActivo).Count();
                tiempoPromedioEstatus40 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.AutorizacionPresupuesto && w.esActivo).Count();
                tiempoPromedioEstatus50 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.ElaboracionOC && w.esActivo).Count();
                tiempoPromedioEstatus60 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.SuministroRefacciones && w.esActivo).Count();
                tiempoPromedioEstatus80 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.RehabilitacionProgramada && w.esActivo).Count();
                tiempoPromedioEstatus90 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.ProcesoInstalacion && w.esActivo).Count();
                tiempoPromedioEstatus100 = lstBitacora.Where(w => w.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && w.esActivo).Count();

                if (tiempoPromedioEstatus20 > 1)
                    tiempoPromedioEstatus20 /= 2;
                if (tiempoPromedioEstatus40 > 1)
                    tiempoPromedioEstatus40 /= 2;
                if (tiempoPromedioEstatus50 > 1)
                    tiempoPromedioEstatus50 /= 2;
                if (tiempoPromedioEstatus60 > 1)
                    tiempoPromedioEstatus60 /= 2;
                if (tiempoPromedioEstatus80 > 1)
                    tiempoPromedioEstatus80 /= 2;
                if (tiempoPromedioEstatus90 > 1)
                    tiempoPromedioEstatus90 /= 2;
                if (tiempoPromedioEstatus100 > 1)
                    tiempoPromedioEstatus100 /= 2;

                tiempoTotalPromedio = (tiempoPromedioEstatus20 + tiempoPromedioEstatus40 + tiempoPromedioEstatus50 + tiempoPromedioEstatus60 + tiempoPromedioEstatus80 + tiempoPromedioEstatus90 + tiempoPromedioEstatus100);

                lstTiempoPromedio.Add(tiempoPromedioEstatus20);
                lstTiempoPromedio.Add(tiempoPromedioEstatus40);
                lstTiempoPromedio.Add(tiempoPromedioEstatus50);
                lstTiempoPromedio.Add(tiempoPromedioEstatus60);
                lstTiempoPromedio.Add(tiempoPromedioEstatus80);
                lstTiempoPromedio.Add(tiempoPromedioEstatus90);
                lstTiempoPromedio.Add(tiempoPromedioEstatus100);
                lstTiempoPromedio.Add(tiempoTotalPromedio);
                result.Add("lstTiempoPromedio", lstTiempoPromedio);
                return result;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 12, _NOMBRE_CONTROLADOR, "GetTiempoPromedioBLTMC", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }


        public Dictionary<string, object> GetEstatusBackLogsTMC(List<tblBL_CatBackLogs> lstBL)
        {
            var resultados = new Dictionary<string, object>();
            try
            {

                int totalBL = 0;
                List<int> lstCantEstatusTMC = new List<int>();
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.ElaboracionPresupuesto).Count());
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.AutorizacionPresupuesto).Count());
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.ElaboracionOC).Count());
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.SuministroRefacciones).Count());
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.RehabilitacionProgramada).Count());
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.ProcesoInstalacion).Count());
                lstCantEstatusTMC.Add(lstBL.Where(x => x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado).Count());
                for (int i = 0; i < lstCantEstatusTMC.Count(); i++)
                {
                    totalBL += lstCantEstatusTMC[i];
                }
                lstCantEstatusTMC.Add(totalBL);
                resultados.Add("lstCantEstatusTMC", lstCantEstatusTMC.ToList());
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetEstatusBackLogsTMC", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
            return resultados;
        }

        public Dictionary<string, object> GetEstatusBackLogsAñosAnterioresTMC(List<tblBL_CatBackLogs> lstBL, string areaCuenta)
        {
            #region COMENTADO
            //var resultados = new Dictionary<string, object>();
            //try
            //{
            //    DateTime fechaAC = new DateTime(DateTime.Now.Year, 01, 01);
            //    int cantBLAcumuladosAniosPasados = lstBL.Where(x => fechaAC > x.fechaCreacionBL && x.idEstatus != (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.esActivo).Count();

            //    List<int> lstContadorAcumuladoTMC = new List<int>();
            //    lstContadorAcumuladoTMC.Add(cantBLAcumuladosAniosPasados);

            //    resultados.Add("lstContadorAcumuladoTMC", lstContadorAcumuladoTMC.ToList());
            //}
            //catch (Exception e)
            //{
            //    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetEstatusBackLogsAñosAnterioresTMC", e, AccionEnum.CONSULTA, 0, 0);
            //    return null;
            //}
            //return resultados;
            #endregion

            var resultados = new Dictionary<string, object>();
            try
            {
                int anioActual = DateTime.Now.Year;
                int anioAnterior = anioActual - 1;

                int cantAcumulados = 0;
                DateTime fechaInicio = new DateTime(anioAnterior, 01, 01);
                DateTime fechaFinal = new DateTime(anioAnterior, 12, 31);

                int cantBLRegistrados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT COUNT(id) AS cantBLAcumuladosAnioPasados FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                    parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).FirstOrDefault();

                if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    cantBLRegistrados = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT COUNT(id) AS cantBLAcumuladosAnioPasados FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                        parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                    }).FirstOrDefault();
                }

                int cantBLInstalados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT COUNT(id) AS cantBLAcumuladosAnioPasados FROM tblBL_CatBackLogs WHERE idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                    parametros = new { idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).FirstOrDefault();

                if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                {
                    cantBLInstalados = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT COUNT(id) AS cantBLAcumuladosAnioPasados FROM tblBL_CatBackLogs WHERE idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                        parametros = new { idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                    }).FirstOrDefault();
                }
                cantAcumulados = cantBLRegistrados - cantBLInstalados;

                List<int> lstContadorAcumulado = new List<int>();
                lstContadorAcumulado.Add(cantAcumulados);

                resultados.Add("resultadosAñosAnterioresTMC", lstContadorAcumulado.ToList());
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 12, _NOMBRE_CONTROLADOR, "GetEstatusBackLogsAñosAnteriores", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
            return resultados;
        }

        public Dictionary<string, object> GetEstatusBackLogsLineasTMC(List<tblBL_CatBackLogs> lstBL)
        {
            Dictionary<string, object> resultados = new Dictionary<string, object>();
            try
            {
                #region VARIABLES CONTADORES BL REGISTRADOS
                int cantBLEnero = 0;
                int cantBLFebrero = 0;
                int cantBLMarzo = 0;
                int cantBLAbril = 0;
                int cantBLMayo = 0;
                int cantBLJunio = 0;
                int cantBLJulio = 0;
                int cantBLAgosto = 0;
                int cantBLSeptiembre = 0;
                int cantBLOctubre = 0;
                int cantBLNoviembre = 0;
                int cantBLDiciembre = 0;
                #endregion

                #region VARIABLES CONTADORES BL INSTALADOS
                int cantBLEneroInstalados = 0;
                int cantBLFebreroInstalados = 0;
                int cantBLMarzoInstalados = 0;
                int cantBLAbrilInstalados = 0;
                int cantBLMayoInstalados = 0;
                int cantBLJunioInstalados = 0;
                int cantBLJulioInstalados = 0;
                int cantBLAgostoInstalados = 0;
                int cantBLSeptiembreInstalados = 0;
                int cantBLOctubreInstalados = 0;
                int cantBLNoviembreInstalados = 0;
                int cantBLDiciembreInstalados = 0;
                #endregion

                #region VARIABLES CONTADORES BL ACUMULADOS
                int cantBLEneroAcumulados = 0;
                int cantBLFebreroAcumulados = 0;
                int cantBLMarzoAcumulados = 0;
                int cantBLAbrilAcumulados = 0;
                int cantBLMayoAcumulados = 0;
                int cantBLJunioAcumulados = 0;
                int cantBLJulioAcumulados = 0;
                int cantBLAgostoAcumulados = 0;
                int cantBLSeptiembreAcumulados = 0;
                int cantBLOctubreAcumulados = 0;
                int cantBLNoviembreAcumulados = 0;
                int cantBLDiciembreAcumulados = 0;
                #endregion

                string areaCuenta = "1010";
                string mmInicio = string.Empty;
                string fechaInicio = string.Empty;
                string ddFinal = string.Empty;
                string mmFinal = string.Empty;
                string fechaFinal = string.Empty;

                #region FOREACH
                DateTime fechaActual;
                int contadorNumMes = 1;
                int anioActual = Convert.ToInt32(DateTime.Now.Year);
                List<int> lstMeses = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    lstMeses.Add(contadorNumMes);
                    contadorNumMes++;
                }
                contadorNumMes = 0;
                foreach (var numMes in lstMeses)
                {
                    fechaActual = new DateTime(anioActual, numMes, 1);
                    int MM = Convert.ToInt32(fechaActual.Month);
                    int yyyy = Convert.ToInt32(fechaActual.Year);
                    int dd = DateTime.DaysInMonth(yyyy, MM);
                    contadorNumMes++;

                    //string fechaCreacionBL = item.fechaCreacionBL.ToString();
                    //DateTime date = Convert.ToDateTime(fechaCreacionBL);
                    //int MM = Convert.ToInt32(date.Month);
                    //int yyyy = Convert.ToInt32(date.Year);
                    //int dd = DateTime.DaysInMonth(yyyy, MM);

                    switch (MM)
                    {
                        case 1:
                            #region ENERO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            if (areaCuenta == "3-1")
                            {
                                // SE TOMA fechaCreacion YA QUE NO TIENEN LAS FECHAS REALES FUERA DEL MODULO
                                cantBLEnero = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                                    parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                                }).FirstOrDefault();

                                cantBLEneroInstalados = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                    parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                                }).FirstOrDefault();
                            }
                            else
                            {
                                cantBLEnero = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLEnero FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                    parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                                }).FirstOrDefault();

                                cantBLEneroInstalados = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                    parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                                }).FirstOrDefault();
                            }

                            cantBLEneroAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 2:
                            #region FEBRERO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            if (areaCuenta == "1-10")
                            {
                                cantBLFebrero = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLFebrero FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal)",
                                    parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                                }).FirstOrDefault();

                                cantBLFebreroInstalados = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaCreacionBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                    parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                                }).FirstOrDefault();
                            }
                            else
                            {
                                cantBLFebrero = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLFebrero FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                    parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                                }).FirstOrDefault();

                                cantBLFebreroInstalados = _context.Select<int>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                    parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                                }).FirstOrDefault();
                            }

                            cantBLFebreroAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 3:
                            #region MARZO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLMarzo = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLMarzo FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLMarzoInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLMarzoAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 4:
                            #region ABRIL
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLAbril = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAbril FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLAbrilInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLAbrilAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 5:
                            #region MAYO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLMayo = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLMayo FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLMayoInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLMayoAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 6:
                            #region JUNIO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLJunio = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLJunio FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLJunioInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLJunioAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 7:
                            #region JULIO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLJulio = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLJulio FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLJulioInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLJulioAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 8:
                            #region AGOSTO
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLAgosto = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAgosto FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLAgostoInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLAgostoAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 9:
                            #region SEPTIEMBRE
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLSeptiembre = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLSeptiembre FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLSeptiembreInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLSeptiembreAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 10:
                            #region OCTUBRE
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLOctubre = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLOctubre FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLOctubreInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLOctubreAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 11:
                            #region NOVIEMBRE
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLNoviembre = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLNoviembre FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLNoviembreInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLNoviembreAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        case 12:
                            #region DICIEMBRE
                            mmInicio = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaInicio = yyyy + "-" + mmInicio + "-" + "01";

                            ddFinal = Convert.ToInt32(dd) >= 1 && Convert.ToInt32(dd) <= 9 ? "0" + dd.ToString() : dd.ToString();
                            mmFinal = Convert.ToInt32(MM) >= 1 && Convert.ToInt32(MM) <= 9 ? "0" + MM.ToString() : MM.ToString();
                            fechaFinal = yyyy + "-" + mmFinal + "-" + ddFinal;

                            cantBLDiciembre = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLDicimebre FROM tblBL_CatBackLogs WHERE areaCuenta = @areaCuenta AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            cantBLDiciembreInstalados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLInstalado FROM tblBL_CatBackLogs WHERE (CONVERT(DATE, fechaInstaladoBL) BETWEEN @fechaInicio AND @fechaFinal) AND idEstatus = @idEstatus AND areaCuenta = @areaCuenta AND esActivo = @esActivo",
                                parametros = new { fechaInicio = fechaInicio, fechaFinal = fechaFinal, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, areaCuenta = areaCuenta, esActivo = true }
                            }).FirstOrDefault();

                            cantBLDiciembreAcumulados = _context.Select<int>(new DapperDTO
                            {
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                consulta = @"SELECT COUNT(id) AS cantBLAcumulados FROM tblBL_CatBackLogs AS t1 WHERE areaCuenta = @areaCuenta AND idEstatus != @idEstatus AND esActivo = @esActivo AND (CONVERT(DATE, fechaInspeccion) BETWEEN @fechaInicio AND @fechaFinal)",
                                parametros = new { areaCuenta = areaCuenta, idEstatus = (int)EstatusBackLogEnum.BackLogsInstalado, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                            }).FirstOrDefault();

                            break;
                            #endregion
                        default:
                            break;
                    }
                }
                #endregion

                List<int> lstcontadorTMC = new List<int>();
                lstcontadorTMC.Add(cantBLEnero);
                lstcontadorTMC.Add(cantBLFebrero);
                lstcontadorTMC.Add(cantBLMarzo);
                lstcontadorTMC.Add(cantBLAbril);
                lstcontadorTMC.Add(cantBLMayo);
                lstcontadorTMC.Add(cantBLJunio);
                lstcontadorTMC.Add(cantBLJulio);
                lstcontadorTMC.Add(cantBLAgosto);
                lstcontadorTMC.Add(cantBLSeptiembre);
                lstcontadorTMC.Add(cantBLOctubre);
                lstcontadorTMC.Add(cantBLNoviembre);
                lstcontadorTMC.Add(cantBLDiciembre);

                List<int> IstContadorInstaladosTMC = new List<int>();
                IstContadorInstaladosTMC.Add(cantBLEneroInstalados);
                IstContadorInstaladosTMC.Add(cantBLFebreroInstalados);
                IstContadorInstaladosTMC.Add(cantBLMarzoInstalados);
                IstContadorInstaladosTMC.Add(cantBLAbrilInstalados);
                IstContadorInstaladosTMC.Add(cantBLMayoInstalados);
                IstContadorInstaladosTMC.Add(cantBLJunioInstalados);
                IstContadorInstaladosTMC.Add(cantBLJulioInstalados);
                IstContadorInstaladosTMC.Add(cantBLAgostoInstalados);
                IstContadorInstaladosTMC.Add(cantBLSeptiembreInstalados);
                IstContadorInstaladosTMC.Add(cantBLOctubreInstalados);
                IstContadorInstaladosTMC.Add(cantBLNoviembreInstalados);
                IstContadorInstaladosTMC.Add(cantBLDiciembreInstalados);

                List<int> IstContadorAcumulados = new List<int>();
                IstContadorAcumulados.Add(cantBLEneroAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados + cantBLOctubreAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados + cantBLOctubreAcumulados + cantBLNoviembreAcumulados);
                IstContadorAcumulados.Add(cantBLEneroAcumulados + cantBLFebreroAcumulados + cantBLMarzoAcumulados + cantBLAbrilAcumulados + cantBLMayoAcumulados + cantBLJunioAcumulados + cantBLJulioAcumulados + cantBLAgostoAcumulados + cantBLSeptiembreAcumulados + cantBLOctubreAcumulados + cantBLNoviembreAcumulados + cantBLDiciembreAcumulados);


                resultados.Add("lstcontadorTMC", lstcontadorTMC.ToList());
                resultados.Add("IstContadorInstaladosTMC", IstContadorInstaladosTMC.ToList());
                resultados.Add("IstContadorAcumuladosTMC", IstContadorAcumulados.ToList());
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetEstatusBackLogsLineasTMC", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
            return resultados;
        }

        public List<BackLogsDTO> GetBackLogsFiltrosTMC(BackLogsDTO objDTO)
        {
            try
            {
                #region VERSION ANTERIOR
                //                tblAlm_RelAreaCuentaXAlmacen objAreaAlmacen = _context.Select<tblAlm_RelAreaCuentaXAlmacen>(new DapperDTO
                //                {
                //                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                //                    consulta = @"SELECT * FROM tblAlm_RelAreaCuentaXAlmacen WHERE AreaCuenta = @AreaCuenta",
                //                    parametros = new { AreaCuenta = objDTO.areaCuenta }
                //                }).FirstOrDefault();

                //                if (vSesiones.sesionEmpresaActual == 3)
                //                {
                //                    objAreaAlmacen = _context.Select<tblAlm_RelAreaCuentaXAlmacen>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Colombia,
                //                        consulta = @"SELECT * FROM tblAlm_RelAreaCuentaXAlmacen WHERE AreaCuenta = @AreaCuenta",
                //                        parametros = new { AreaCuenta = objDTO.areaCuenta }
                //                    }).FirstOrDefault();
                //                }

                //                List<tblAlm_RelAreaCuentaXAlmacenDet> lstAlmRelAreaCuentaXAlmacenDet = _context.Select<tblAlm_RelAreaCuentaXAlmacenDet>(new DapperDTO
                //                {
                //                    baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : (MainContextEnum)vSesiones.sesionEmpresaActual,
                //                    consulta = @"SELECT * FROM tblAlm_RelAreaCuentaXAlmacenDet ORDER BY id DESC",
                //                    parametros = new { registroActivo = true }
                //                }).ToList();

                //                if (vSesiones.sesionEmpresaActual == 3)
                //                {
                //                    lstAlmRelAreaCuentaXAlmacenDet = _context.Select<tblAlm_RelAreaCuentaXAlmacenDet>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Colombia,
                //                        consulta = @"SELECT * FROM tblAlm_RelAreaCuentaXAlmacenDet ORDER BY id DESC",
                //                        parametros = new { registroActivo = true }
                //                    }).ToList();
                //                }

                //                List<tblBL_Partes> lstPartes = _context.Select<tblBL_Partes>(new DapperDTO
                //                {
                //                    baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : (MainContextEnum)vSesiones.sesionEmpresaActual,
                //                    consulta = @"SELECT * FROM tblBL_Partes WHERE esActivo = @esActivo",
                //                    parametros = new { esActivo = true }
                //                }).ToList();

                //                if (vSesiones.sesionEmpresaActual == 3)
                //                {
                //                    lstPartes = _context.Select<tblBL_Partes>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Colombia,
                //                        consulta = @"SELECT * FROM tblBL_Partes WHERE esActivo = @esActivo",
                //                        parametros = new { esActivo = true }
                //                    }).ToList();
                //                }

                //                List<tblBL_OrdenesCompra> objOrdenesCompra = _context.Select<tblBL_OrdenesCompra>(new DapperDTO
                //                {
                //                    baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : (MainContextEnum)vSesiones.sesionEmpresaActual,
                //                    consulta = @"SELECT * FROM tblBL_OrdenesCompra WHERE esActivo = @esActivo",
                //                    parametros = new { esActivo = true }
                //                }).ToList();

                //                if (vSesiones.sesionEmpresaActual == 3)
                //                {
                //                    objOrdenesCompra = _context.Select<tblBL_OrdenesCompra>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Colombia,
                //                        consulta = @"SELECT * FROM tblBL_OrdenesCompra WHERE esActivo = @esActivo",
                //                        parametros = new { esActivo = true }
                //                    }).ToList();
                //                }

                //                //List<BackLogsDTO> lstBackLogs = _context.tblBL_CatBackLogs
                //                //.Where(x => x.areaCuenta == objDTO.areaCuenta && x.esActivo).ToList().Where(x =>
                //                //(objDTO.noEconomico != null ? x.noEconomico == objDTO.noEconomico : true) &&
                //                //(objDTO.areaCuenta == "" ? x.areaCuenta == x.areaCuenta : x.areaCuenta == objDTO.areaCuenta) &&
                //                //(objDTO.idEstatus != null ? objDTO.lstEstatus.Contains(x.idEstatus) : true)
                //                //).OrderByDescending(x => x.folioBL).Select(x => new BackLogsDTO
                //                //{
                //                //    id = x.id,
                //                //    folioBL = x.folioBL,
                //                //    noEconomico = x.noEconomico,
                //                //    descripcion = x.descripcion,
                //                //    fechaInspeccion = x.fechaInspeccion,
                //                //    conjunto = x.subconjunto.CatConjuntos.descripcion,
                //                //    subconjunto = x.subconjunto.descripcion,
                //                //    //totalMX = GetTotalOC(x.areaCuenta, x.id, x.noEconomico, objAreaAlmacen, lstAlmRelAreaCuentaXAlmacenDet, objOrdenesCompra, lstPartes),
                //                //    totalMX = 0,
                //                //    idEstatus = x.idEstatus,
                //                //    estatus = EnumHelper.GetDescription((EstatusBackLogsTMCEnum)x.idEstatus),
                //                //    fechaModificacionBL = x.idEstatus == 7 ? Convert.ToDateTime(x.fechaModificacionBL) : Convert.ToDateTime(null),
                //                //    diasTotales = DiasTranscurridos(x.id, x.idEstatus, Convert.ToDateTime(x.fechaCreacionBL), Convert.ToDateTime(x.fechaModificacionBL)),
                //                //    idConjunto = x.subconjunto.CatConjuntos.id,
                //                //    esLiberado = x.esLiberado,
                //                //    presupuestoEstimado = x.presupuestoEstimado
                //                //}).ToList();

                //                string strQuery = string.Format(@"SELECT t1.id, t1.folioBL, t1.noEconomico, t1.descripcion, t1.fechaInspeccion, t3.descripcion AS conjunto, t2.descripcion AS subconjunto, t1.idEstatus, 
                //                                                         t1.fechaModificacionBL, t3.id AS idConjunto, t1.esLiberado, t1.presupuestoEstimado, t1.fechaCreacionBL
                //	                                                        FROM tblBL_CatBackLogs AS t1
                //	                                                        INNER JOIN tblBL_CatSubconjuntos AS t2 ON t1.idSubconjunto = t2.id
                //	                                                        INNER JOIN tblBL_CatConjuntos AS t3 ON t2.idConjunto = t3.id
                //		                                                        WHERE t1.tipoBL = {0} AND t1.esActivo = {1}", 2, 1);

                //                List<BackLogsDTO> lstBackLogs = _context.Select<BackLogsDTO>(new DapperDTO
                //                {
                //                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                //                    consulta = strQuery
                //                }).ToList();

                //                if (vSesiones.sesionEmpresaActual == 3)
                //                {
                //                    lstBackLogs = _context.Select<BackLogsDTO>(new DapperDTO
                //                    {
                //                        baseDatos = MainContextEnum.Colombia,
                //                        consulta = strQuery
                //                    }).ToList();
                //                }

                //                if (objDTO.arrNoEconomicos != null)
                //                    lstBackLogs = lstBackLogs.Where(w => objDTO.arrNoEconomicos.Contains(w.noEconomico)).ToList();

                //                if (objDTO.idConjunto > 0)
                //                    lstBackLogs = lstBackLogs.Where(w => w.idConjunto == objDTO.idConjunto).ToList();

                //                if (objDTO.idSubconjunto > 0)
                //                    lstBackLogs = lstBackLogs.Where(w => w.idSubconjunto == objDTO.idSubconjunto).ToList();

                //                if (objDTO.Mes > 0)
                //                    lstBackLogs = lstBackLogs.Where(w => w.fechaInspeccion.Month == objDTO.Mes).ToList();

                //                if (objDTO.lstEstatus != null)
                //                    lstBackLogs = lstBackLogs.Where(w => objDTO.lstEstatus.Contains(w.idEstatus)).ToList();

                //                foreach (var item in lstBackLogs)
                //                {
                //                    strQuery = string.Empty;

                //                    #region SE OBTIENE EL CC DEL ECONOMICO
                //                    string ccEconomico = string.Empty;
                //                    if (!string.IsNullOrEmpty(item.noEconomico))
                //                    {
                //                        strQuery = string.Format("SELECT cc FROM cc WHERE descripcion = '{0}'", item.noEconomico);
                //                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //                        odbc.consulta = String.Format(strQuery);
                //                        List<dynamic> objCCEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);
                //                        if (vSesiones.sesionEmpresaActual == 3)
                //                        {
                //                            objCCEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, odbc);
                //                        }
                //                        if (objCCEconomico.Count() > 0)
                //                            ccEconomico = objCCEconomico[0].cc;
                //                    }
                //                    #endregion

                //                    #region SE OBTIENE EL TOTAL EN BASE A LOS INSUMOS QUE CONTENGA EL BL
                //                    if (!string.IsNullOrEmpty(ccEconomico))
                //                    {
                //                        // SE OBTIENE LOS INSUMOS DEL BL
                //                        strQuery = string.Format("SELECT insumo FROM tblBL_Partes WHERE idBackLog = {0}", item.id);
                //                        List<int> lstInsumosDTO = _context.Select<int>(new DapperDTO { baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, consulta = strQuery }).ToList();
                //                        if (vSesiones.sesionEmpresaActual == 3)
                //                        {
                //                            lstInsumosDTO = _context.Select<int>(new DapperDTO { baseDatos = MainContextEnum.Colombia, consulta = strQuery }).ToList();
                //                        }

                //                        // SE OBTIENE LAS NUM_REQUISICIONES DEL BL
                //                        strQuery = string.Format("SELECT numRequisicion FROM tblBL_Requisiciones WHERE idBackLog = {0}", item.id);
                //                        List<int> lstRequisicionesDTO = _context.Select<int>(new DapperDTO { baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, consulta = strQuery }).ToList();
                //                        if (vSesiones.sesionEmpresaActual == 3)
                //                        {
                //                            lstRequisicionesDTO = _context.Select<int>(new DapperDTO { baseDatos = MainContextEnum.Colombia, consulta = strQuery }).ToList();
                //                        }

                //                        // SE OBTIENE LOS NUM_ORDENES_COMPRA EN BASE AL BL Y NUM_REQUISICIONES CONSULTADOS ANTERIORMENTE
                //                        //strQuery = string.Format("SELECT cc, numRequisicion, numOC FROM tblBL_OrdenesCompra WHERE cc = '{0}' AND numRequisicion = {1} AND esActivo = {2}", ccEconomico, string.Join(",", lstRequisicionesDTO.ToList(), 1));
                //                        //List<dynamic> lstNumOrdenesComprasDTO = _context.Select<dynamic>(new DapperDTO { baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, consulta = strQuery }).ToList();

                //                        // SE OBTIENE EL DETALLE DE COMPRA EN BASE AL CC, NUM_OC, NUM_REQ E INSUMO
                //                        if (lstRequisicionesDTO.Count() > 0)
                //                        {
                //                            for (int i = 0; i < lstRequisicionesDTO.Count(); i++)
                //                            {
                //                                int numReq = lstRequisicionesDTO[i];
                //                                int insumo = 0;

                //                                decimal totalInsumo = 0;
                //                                try
                //                                {
                //                                    insumo = lstInsumosDTO[i];
                //                                    strQuery = string.Format(@"SELECT ISNULL(SUM(CASE WHEN t2.moneda = 2 
                //                                                                THEN (t2.tipo_cambio * (CASE WHEN t1.importe IS NOT NULL THEN t1.importe ELSE 0 END)) 
                //                                                                ELSE (CASE WHEN t1.importe IS NULL THEN t1.importe ELSE 0 END)
                //                                                                END), 0) AS totalInsumo
                //                                                                    FROM so_orden_compra_det AS t1
                //                                                                    INNER JOIN so_orden_compra AS t2 ON t1.cc = t2.cc AND t1.numero = t2.numero
                //                                                                        WHERE t1.cc = '{0}' AND t1.num_requisicion = {1} AND t1.insumo = {2}", ccEconomico, numReq, insumo);

                //                                    if (vSesiones.sesionEmpresaActual == 3)
                //                                    {
                //                                        strQuery = string.Format(@"SELECT ISNULL(SUM(CASE WHEN t2.moneda = 2 
                //                                                                THEN (t2.tipo_cambio * (CASE WHEN t1.importe IS NOT NULL THEN t1.importe ELSE 0 END)) 
                //                                                                ELSE (CASE WHEN t1.importe IS NULL THEN t1.importe ELSE 0 END)
                //                                                                END), 0) AS totalInsumo
                //                                                                    FROM DBA.so_orden_compra_det AS t1
                //                                                                    INNER JOIN DBA.so_orden_compra AS t2 ON t1.cc = t2.cc AND t1.numero = t2.numero
                //                                                                        WHERE t1.cc = '{0}' AND t1.num_requisicion = {1} AND t1.insumo = {2}", ccEconomico, numReq, insumo);
                //                                    }
                //                                    var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                //                                    odbc.consulta = String.Format(strQuery);
                //                                    List<decimal> objImporteTotalMX = _contextEnkontrol.Select<decimal>(EnkontrolEnum.ArrenProd, odbc);
                //                                    if (objImporteTotalMX.Count() > 0)
                //                                        totalInsumo = (decimal)objImporteTotalMX[0];

                //                                    item.totalMX += (decimal)totalInsumo;

                //                                    #region EN CASO QUE EL PPTO SEA 0 EN ENKONTROL, SE OBTIENE EL PPTO ESTIMADO REGISTRADO
                //                                    if (item.totalMX == 0)
                //                                    {
                //                                        for (int x = 0; x < lstInsumosDTO.Count(); x++)
                //                                        {
                //                                            insumo = lstInsumosDTO[x] > 0 ? Convert.ToInt32(lstInsumosDTO[x]) : 0;
                //                                            if (insumo > 0)
                //                                            {
                //                                                decimal total = GetCostoPromedio(601, insumo);
                //                                                if (total > 0)
                //                                                    item.totalMX += total;
                //                                            }
                //                                        }

                //                                        if (item.totalMX <= 0)
                //                                            item.totalMX += item.presupuestoEstimado;
                //                                    }
                //                                    #endregion
                //                                }
                //                                catch (Exception)
                //                                {

                //                                }
                //                            }
                //                        }
                //                        else
                //                        {
                //                            for (int i = 0; i < lstInsumosDTO.Count(); i++)
                //                            {
                //                                int insumo = lstInsumosDTO[i] > 0 ? Convert.ToInt32(lstInsumosDTO[i]) : 0;
                //                                if (insumo > 0)
                //                                {
                //                                    decimal total = GetCostoPromedio(601, insumo);
                //                                    if (total > 0)
                //                                        item.totalMX += total;
                //                                }
                //                            }

                //                            if (item.totalMX <= 0)
                //                                item.totalMX += item.presupuestoEstimado;
                //                        }
                //                    }
                //                    #endregion

                //                    #region SE CAMBIA EL TEXTO DE CONJUNTOS Y SUBCONJUNTOS A MAYUSCULAS
                //                    string conjunto = item.conjunto;
                //                    if (!string.IsNullOrEmpty(conjunto))
                //                        item.conjunto = conjunto.Trim().ToUpper();

                //                    string subconjunto = item.subconjunto;
                //                    if (!string.IsNullOrEmpty(subconjunto))
                //                        item.subconjunto = subconjunto.Trim().ToUpper();
                //                    #endregion

                //                    // SE OBTIENE LOS DÍAS TRANSCURRIDOS
                //                    item.diasTotales = DiasTranscurridos(item.id, item.idEstatus, Convert.ToDateTime(item.fechaCreacionBL), Convert.ToDateTime(item.fechaModificacionBL));

                //                    // SE OBTIENE EL ESTATUS DEL BACKLOG
                //                    item.estatus = EnumHelper.GetDescription((EstatusBackLogsTMCEnum)item.idEstatus);

                //                    // SE OBTIENE LA FECHA TERMINACIÓN
                //                    if (item.idEstatus == 7)
                //                        item.fechaModificacionBL = item.fechaInstaladoBL.Year > 2000 ? Convert.ToDateTime(item.fechaInstaladoBL) : Convert.ToDateTime(item.fechaModificacionBL);
                //                    else
                //                        item.fechaModificacionBL = Convert.ToDateTime(null);
                //                }
                //                return lstBackLogs;
                #endregion

                #region VERSION ACTUAL
                using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
                {
                    tblAlm_RelAreaCuentaXAlmacen objAreaAlmacen = _ctx.tblAlm_RelAreaCuentaXAlmacen.Where(w => w.AreaCuenta == objDTO.areaCuenta).FirstOrDefault();
                    List<tblAlm_RelAreaCuentaXAlmacenDet> lstAlmRelAreaCuentaXAlmacenDet = _ctx.tblAlm_RelAreaCuentaXAlmacenDet.OrderByDescending(o => o.id).ToList();
                    List<tblBL_Partes> lstPartes = _ctx.tblBL_Partes.ToList();
                    List<tblBL_OrdenesCompra> objOrdenesCompra = _ctx.tblBL_OrdenesCompra.Where(w => w.esActivo).ToList();

                    List<BackLogsDTO> lstBackLogs = _context.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.folioBL, t1.noEconomico, t1.descripcion, t1.fechaInspeccion, t3.descripcion AS conjunto, t2.descripcion AS subconjunto, t1.idEstatus, 
                                            t1.fechaModificacionBL, t3.id AS idConjunto, t1.esLiberado, t1.presupuestoEstimado, t1.fechaCreacionBL
	                                            FROM tblBL_CatBackLogs AS t1
	                                            INNER JOIN tblBL_CatSubconjuntos AS t2 ON t1.idSubconjunto = t2.id
	                                            INNER JOIN tblBL_CatConjuntos AS t3 ON t2.idConjunto = t3.id
		                                            WHERE t1.tipoBL = 2 AND t1.esActivo = 1"
                    }).ToList();

                    #region FILTROS
                    if (objDTO.arrNoEconomicos != null)
                        lstBackLogs = lstBackLogs.Where(w => objDTO.arrNoEconomicos.Contains(w.noEconomico)).ToList();

                    if (objDTO.idConjunto > 0)
                        lstBackLogs = lstBackLogs.Where(w => w.idConjunto == objDTO.idConjunto).ToList();

                    if (objDTO.idSubconjunto > 0)
                        lstBackLogs = lstBackLogs.Where(w => w.idSubconjunto == objDTO.idSubconjunto).ToList();

                    if (objDTO.Mes > 0)
                        lstBackLogs = lstBackLogs.Where(w => w.fechaInspeccion.Month == objDTO.Mes).ToList();

                    if (objDTO.lstEstatus != null)
                        lstBackLogs = lstBackLogs.Where(w => objDTO.lstEstatus.Contains(w.idEstatus)).ToList();
                    #endregion

                    string strQuery = string.Empty;
                    foreach (var item in lstBackLogs)
                    {
                        strQuery = string.Empty;

                        #region SE OBTIENE EL CC DEL ECONOMICO
                        string ccEconomico = string.Empty;
                        if (!string.IsNullOrEmpty(item.noEconomico))
                        {
                            strQuery = string.Format("SELECT cc FROM cc WHERE descripcion = '{0}'", item.noEconomico);
                            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery);
                            List<dynamic> objCCEconomico = new List<dynamic>();
                            switch (vSesiones.sesionEmpresaActual)
                            {
                                case (int)EmpresaEnum.Arrendadora:
                                    objCCEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, odbc);
                                    break;
                                case (int)EmpresaEnum.Colombia:
                                    objCCEconomico = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, odbc);
                                    break;
                            }
                            if (objCCEconomico.Count() > 0)
                                ccEconomico = objCCEconomico[0].cc;
                        }
                        #endregion

                        #region SE OBTIENE EL TOTAL EN BASE A LOS INSUMOS QUE CONTENGA EL BL
                        if (!string.IsNullOrEmpty(ccEconomico))
                        {
                            // SE OBTIENE LOS INSUMOS DEL BL
                            strQuery = string.Format("SELECT insumo FROM tblBL_Partes WHERE idBackLog = {0}", item.id);
                            List<int> lstInsumosDTO = _context.Select<int>(new DapperDTO { 
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, 
                                consulta = strQuery 
                            }).ToList();

                            // SE OBTIENE LAS NUM_REQUISICIONES DEL BL
                            strQuery = string.Format("SELECT numRequisicion FROM tblBL_Requisiciones WHERE idBackLog = {0}", item.id);
                            List<int> lstRequisicionesDTO = _context.Select<int>(new DapperDTO { 
                                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual, 
                                consulta = strQuery 
                            }).ToList();

                            // SE OBTIENE EL DETALLE DE COMPRA EN BASE AL CC, NUM_OC, NUM_REQ E INSUMO
                            if (lstRequisicionesDTO.Count() > 0)
                            {
                                for (int i = 0; i < lstRequisicionesDTO.Count(); i++)
                                {
                                    int numReq = lstRequisicionesDTO[i];
                                    int insumo = 0;

                                    decimal totalInsumo = 0;
                                    try
                                    {
                                        insumo = lstInsumosDTO[i];
                                        strQuery = string.Format(@"SELECT 
                                                                        ISNULL(SUM(CASE WHEN t2.moneda = 2 THEN 
                                                                        (t2.tipo_cambio * (CASE WHEN t1.importe IS NOT NULL THEN t1.importe ELSE 0 END)) 
                                                                        ELSE (CASE WHEN t1.importe IS NULL THEN t1.importe ELSE 0 END) END), 0) AS totalInsumo
                                                                            FROM so_orden_compra_det AS t1
                                                                                INNER JOIN so_orden_compra AS t2 ON t1.cc = t2.cc AND t1.numero = t2.numero
                                                                                    WHERE t1.cc = '{0}' AND t1.num_requisicion = {1} AND t1.insumo = {2}", ccEconomico, numReq, insumo);
                                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                                        {
                                            strQuery = string.Format(@"SELECT 
                                                                            ISNULL(SUM(CASE WHEN t2.moneda = 2 THEN 
                                                                            (t2.tipo_cambio * (CASE WHEN t1.importe IS NOT NULL THEN t1.importe ELSE 0 END)) 
                                                                            ELSE (CASE WHEN t1.importe IS NULL THEN t1.importe ELSE 0 END) END), 0) AS totalInsumo
                                                                                FROM DBA.so_orden_compra_det AS t1
                                                                                    INNER JOIN DBA.so_orden_compra AS t2 ON t1.cc = t2.cc AND t1.numero = t2.numero
                                                                                        WHERE t1.cc = '{0}' AND t1.num_requisicion = {1} AND t1.insumo = {2}", ccEconomico, numReq, insumo);
                                        }
                                        var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                                        odbc.consulta = String.Format(strQuery);
                                        List<decimal> objImporteTotalMX = new List<decimal>();
                                        switch (vSesiones.sesionEmpresaActual)
                                        {
                                            case (int)EmpresaEnum.Arrendadora:
                                                objImporteTotalMX = _contextEnkontrol.Select<decimal>(EnkontrolEnum.ArrenProd, odbc);
                                                break;
                                            case (int)EmpresaEnum.Colombia:
                                                objImporteTotalMX = _contextEnkontrol.Select<decimal>(EnkontrolEnum.ColombiaProductivo, odbc);
                                                break;
                                        }
                                        if (objImporteTotalMX.Count() > 0)
                                            totalInsumo = (decimal)objImporteTotalMX[0];

                                        item.totalMX += (decimal)totalInsumo;

                                        #region EN CASO QUE EL PPTO SEA 0 EN ENKONTROL, SE OBTIENE EL PPTO ESTIMADO REGISTRADO
                                        if (item.totalMX == 0)
                                        {
                                            for (int x = 0; x < lstInsumosDTO.Count(); x++)
                                            {
                                                insumo = lstInsumosDTO[x] > 0 ? Convert.ToInt32(lstInsumosDTO[x]) : 0;
                                                if (insumo > 0)
                                                {
                                                    decimal total = GetCostoPromedio(4, insumo);
                                                    if (total > 0)
                                                        item.totalMX += total;
                                                }
                                            }
                                            if (item.totalMX <= 0)
                                                item.totalMX += item.presupuestoEstimado;
                                        }
                                        #endregion
                                    }
                                    catch (Exception e)
                                    {
                                        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetBackLogsFiltrosTMC/OBTENER DETALLE DE LA COMPRA", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < lstInsumosDTO.Count(); i++)
                                {
                                    int insumo = lstInsumosDTO[i] > 0 ? Convert.ToInt32(lstInsumosDTO[i]) : 0;
                                    if (insumo > 0)
                                    {
                                        decimal total = GetCostoPromedio(4, insumo);
                                        if (total > 0)
                                            item.totalMX += total;
                                    }
                                }

                                if (item.totalMX <= 0)
                                    item.totalMX += item.presupuestoEstimado;
                            }
                        }
                        #endregion

                        #region SE CAMBIA EL TEXTO DE CONJUNTOS Y SUBCONJUNTOS A MAYUSCULAS
                        string conjunto = item.conjunto;
                        if (!string.IsNullOrEmpty(conjunto))
                            item.conjunto = conjunto.Trim().ToUpper();

                        string subconjunto = item.subconjunto;
                        if (!string.IsNullOrEmpty(subconjunto))
                            item.subconjunto = subconjunto.Trim().ToUpper();
                        #endregion

                        #region SE INDICA SI EL BL TIENE PARTES
                        int cantPartes = lstPartes.Where(w => w.idBacklog == item.id && w.esActivo).Count();
                        if (cantPartes > 0)
                            item.tienePartes = true;
                        else
                            item.tienePartes = false;
                        #endregion

                        // SE OBTIENE LOS DÍAS TRANSCURRIDOS
                        item.diasTotales = DiasTranscurridos(item.id, item.idEstatus, Convert.ToDateTime(item.fechaCreacionBL), Convert.ToDateTime(item.fechaModificacionBL));

                        // SE OBTIENE EL ESTATUS DEL BACKLOG
                        item.estatus = EnumHelper.GetDescription((EstatusBackLogsTMCEnum)item.idEstatus);

                        // SE OBTIENE LA FECHA TERMINACIÓN
                        if (item.idEstatus == 7)
                            item.fechaModificacionBL = item.fechaInstaladoBL.Year > 2000 ? Convert.ToDateTime(item.fechaInstaladoBL) : Convert.ToDateTime(item.fechaModificacionBL);
                        else
                            item.fechaModificacionBL = Convert.ToDateTime(null);
                    }
                    return lstBackLogs;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetBackLogsFiltrosTMC", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                return null;
            }
        }

        public List<BackLogsDTO> FillTablaSolicitudPpto(BackLogsDTO objBL)
        {
            try
            {
                decimal pptoEstimado = 0;
                List<tblBL_SeguimientoPptos> lstSeguimientoPptos = _context.tblBL_SeguimientoPptos.Where(x => x.esActivo).ToList();
                List<tblBL_CatBackLogs> lstNoEconomicosBL = _context.tblBL_CatBackLogs.Where(x => x.cc == objBL.cc && x.areaCuenta == objBL.areaCuenta && x.esActivo && x.esLiberado == false).ToList();
                if (lstNoEconomicosBL.Count() > 0)
                {
                    #region SE SUMA LOS PRESUPUESTOS PARA OBTENER EL PRESTUPUESTO TOTAL
                    foreach (var item in lstNoEconomicosBL)
                    {
                        pptoEstimado += Convert.ToDecimal(item.presupuestoEstimado);
                    }
                    #endregion

                    #region SE OBTIENE LOS DATOS PARA MOSTRAR EL PPTO EN RAM
                    List<BackLogsDTO> lstSolicitudPpto = lstNoEconomicosBL.Select(x => new BackLogsDTO
                    {
                        folioPpto = GetSiguienteFolioPpto(lstSeguimientoPptos, x.noEconomico)[0],
                        fechaActual = DateTime.Now.ToShortDateString(),
                        noEconomico = x.noEconomico,
                        horas = x.horas,
                        presupuestoEstimado = pptoEstimado
                    }).ToList();
                    #endregion

                    #region SOLO SE MUESTRA UN PPTOS (UN REGISTRO)
                    List<BackLogsDTO> Ppto = new List<BackLogsDTO>();
                    for (int i = 0; i < 1; i++)
                    {
                        BackLogsDTO obj = new BackLogsDTO();
                        obj.folioPpto = lstSolicitudPpto[i].folioPpto;
                        obj.fechaActual = lstSolicitudPpto[i].fechaActual;
                        obj.noEconomico = lstSolicitudPpto[i].noEconomico;
                        obj.horas = lstSolicitudPpto[i].horas;
                        obj.presupuestoEstimado = lstSolicitudPpto[i].presupuestoEstimado;
                        Ppto.Add(obj);
                    }
                    #endregion

                    return Ppto.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillTablaSolicitudPpto", e, AccionEnum.CONSULTA, 0, objBL);
                return null;
            }
        }

        private List<string> GetSiguienteFolioPpto(List<tblBL_SeguimientoPptos> lstSeguimientoPptos, string noEconomico)
        {
            try
            {
                List<string> lstFolioPptoConsecutivo = new List<string>();
                string folioPpto = string.Empty;
                int consecutivo = 0;
                if (lstSeguimientoPptos.Count() > 0 && !string.IsNullOrEmpty(noEconomico))
                {
                    List<tblBL_SeguimientoPptos> lstConsecutivos = lstSeguimientoPptos.Where(x => x.noEconomico == noEconomico && x.esActivo).OrderByDescending(x => x.consecutivoFolio).ToList();
                    if (lstConsecutivos.Count() > 0)
                    {
                        consecutivo = lstConsecutivos[0].consecutivoFolio++;
                        folioPpto = "TMC-" + noEconomico + "-" + consecutivo;
                    }
                    else
                    {
                        consecutivo = 1;
                        folioPpto = "TMC-" + noEconomico + "-" + consecutivo;
                    }
                }
                else
                {
                    consecutivo = 1;
                    folioPpto = "TMC-" + noEconomico + "-" + consecutivo;
                }

                if (string.IsNullOrEmpty(folioPpto))
                    throw new Exception("Ocurrió un error al generar el folio ppto.");

                lstFolioPptoConsecutivo.Add(folioPpto);
                lstFolioPptoConsecutivo.Add(consecutivo.ToString());

                return lstFolioPptoConsecutivo;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetSiguienteFolioPpto", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool SolicitarAutorizacion(int idInsp, int idUsuario, SeguimientoPptoDTO objSegPpto, List<string> lstFoliosBL)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE CREA EL SEGUIMIENTO DE PRESUPUESTO
                    string noEconomico = objSegPpto.noEconomico;
                    List<tblBL_SeguimientoPptos> lstSeguimientoPptos = _context.tblBL_SeguimientoPptos.Where(x => x.esActivo).ToList();
                    tblBL_CatBackLogs objBL = _context.tblBL_CatBackLogs.FirstOrDefault(x => x.noEconomico == noEconomico && x.esActivo);
                    int idCatMaquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == noEconomico && x.estatus == 1).id;
                    List<string> lstFolioPptoConsecutivo = GetSiguienteFolioPpto(lstSeguimientoPptos, noEconomico);

                    tblBL_SeguimientoPptos objGuardar = new tblBL_SeguimientoPptos();
                    objGuardar.areaCuenta = objBL.areaCuenta;
                    objGuardar.folioPpto = lstFolioPptoConsecutivo[0]; //SE VALIDA QUE EL FOLIO SE ENCUENTRE DISPONIBLE, DE LO CONTRARIO, SE LE ASIGNA UNO DISPONIBLE.
                    objGuardar.consecutivoFolio = Convert.ToInt32(lstFolioPptoConsecutivo[1]); //SE OBTIENE EL CONSECUTIVO DEL FOLIO.
                    objGuardar.fechaPpto = DateTime.Now;
                    objGuardar.noEconomico = objBL.noEconomico;
                    objGuardar.idCatMaquina = idCatMaquina;
                    objGuardar.horas = objBL.horas;
                    objGuardar.Ppto = objSegPpto.presupuestoEstimado;
                    objGuardar.idInspTMC = idInsp;
                    objGuardar.esVobo1 = (int)EstatusSeguimientosPptoEnum.EN_ESPERA;
                    objGuardar.fechaVobo1 = DateTime.Now;
                    objGuardar.esVobo2 = (int)EstatusSeguimientosPptoEnum.EN_ESPERA;
                    objGuardar.fechaVobo2 = DateTime.Now;
                    objGuardar.esAutorizado = (int)EstatusSeguimientosPptoEnum.EN_ESPERA;
                    objGuardar.fechaAutorizado = DateTime.Now;
                    objGuardar.esActivo = true;
                    objGuardar.fechaCreacion = DateTime.Now;
                    objGuardar.fechaModificacion = DateTime.Now;
                    objGuardar.EstatusSegPpto = (int)EstatusSeguimientosPptoEnum.EN_ESPERA;
                    objGuardar.idFrente = 0;
                    objGuardar.idUserVobo1 = (int)AutorizadoresEnum.vobo1;
                    objGuardar.idUserVobo2 = (int)AutorizadoresEnum.vobo2;
                    objGuardar.idUserAutorizado = (int)AutorizadoresEnum.Autorizado;
                    _context.tblBL_SeguimientoPptos.Add(objGuardar);
                    _context.SaveChanges();
                    #endregion

                    #region SE OBTIENE EL ID DEL PPTO REGISTRADO
                    List<tblBL_SeguimientoPptos> lstSegPptos = _context.tblBL_SeguimientoPptos.Where(x => x.folioPpto == objSegPpto.folioPpto && x.esActivo).ToList();
                    int idSegPpto = lstSegPptos.Count() > 0 ? idSegPpto = lstSegPptos.FirstOrDefault().id : 0;
                    if (idSegPpto == 0)
                        throw new Exception("Ocurrió un error al solicitar el presupuesto.");
                    #endregion

                    #region SE OBTIENE LOS BL RELACIONADOS AL PPTOS ESTIMADO A SOLICITAR
                    List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.noEconomico == objSegPpto.noEconomico && x.cc == objSegPpto.cc && x.idSegPpto == 0 && x.esActivo).ToList();
                    lstBL.Where(s => s.idSegPpto == 0).ToList().ForEach(s => s.idSegPpto = idSegPpto);
                    foreach (var item in lstBL)
                    {
                        tblBL_CatBackLogs objActualizar = lstBL.FirstOrDefault(x => x.id == item.id);
                        _context.SaveChanges();
                    }
                    #endregion

                    #region SE CREA LA ALERTA DE SIGOPLAN.
                    var objTmc = _context.tblBL_SeguimientoPptos.OrderByDescending(r => r.id).FirstOrDefault();
                    tblP_Alerta objAlerta = new tblP_Alerta();
                    objAlerta.userEnviaID = idUsuario;
                    objAlerta.userRecibeID = (int)AutorizadoresEnum.vobo1;
                    objAlerta.tipoAlerta = 2;
                    objAlerta.sistemaID = (int)SistemasEnum.MAQUINARIA;
                    objAlerta.visto = false;
                    objAlerta.url = "/BackLogs/SeguimientoDePresupuestoTMC";
                    objAlerta.objID = objTmc.id;
                    objAlerta.obj = "";
                    objAlerta.msj = "Autorización pendiente " + objTmc.noEconomico;
                    objAlerta.moduloID = 9764;
                    _context.tblP_Alerta.Add(objAlerta);
                    _context.SaveChanges();
                    #endregion

                    #region SE ENVIA CORREO DE QUE SE CREO UNA SOLICITUD DE PRESUPUESTO.
                    List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                    CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                    List<tblP_Usuario> tblP_Usuario = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                    Obj = tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                    {
#if DEBUG
                        Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                        Correos = y.correo,
#endif
                        nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                        puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                        firma = objTmc.firmaVobo1,
                        dtFecha = objTmc.fechaVobo1,
                    }).FirstOrDefault();
                    lstUsuarios.Add(Obj);

                    Obj = new CorreoBackLogDTO();
                    Obj = tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                    {
#if DEBUG
                        Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                        Correos = y.correo,
#endif
                        nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                        puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                        firma = objTmc.firmaVobo2,
                        dtFecha = objTmc.fechaVobo2,
                    }).FirstOrDefault();
                    lstUsuarios.Add(Obj);

                    Obj = new CorreoBackLogDTO();
                    Obj = tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                    {
#if DEBUG
                        Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                        Correos = y.correo,
#endif
                        nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                        puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                        firma = objTmc.firmaAutorizado,
                        dtFecha = objTmc.fechaAutorizado,
                    }).FirstOrDefault();
                    lstUsuarios.Add(Obj);

                    foreach (var item in lstUsuarios)
                    {
                        var subject = "Autorización de BackLogs";
                        var body = @"Buen día " + item.nombreCompleto
                            + ", tiene pendiente una autorización de BackLogs <br>"
                            + htmlCorreo(lstUsuarios);
                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                    }
                    #endregion

                    #region SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                    List<tblBL_BitacoraEstatusBL> lstBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.esActivo).ToList();
                    for (int i = 0; i < lstBL.Count(); i++)
                    {
                        int idBL = lstBL[i].id;
                        string areaCuenta = lstBL.Select(s => s.areaCuenta).FirstOrDefault();
                        int diasTranscurridos = 0;
                        tblBL_BitacoraEstatusBL objBitacoraBL = lstBitacoraBL.Where(w => w.idBL == idBL).OrderByDescending(o => o.id).FirstOrDefault();
                        if (objBitacoraBL != null)
                            diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;

                        tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                        objGuardarBitacoraEstatusBL.idBL = idBL;
                        objGuardarBitacoraEstatusBL.areaCuenta = areaCuenta;
                        objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                        objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogsTMCEnum.AutorizacionPresupuesto;
                        objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                        objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                        objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                        objGuardarBitacoraEstatusBL.esActivo = true;
                        _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objBL));
                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "SolicitarAutorizacion", e, AccionEnum.AGREGAR, 0, objSegPpto);
                    return false;
                }
        }

        public List<BackLogsDTO> GetBLPptos(int idSegPpto)
        {
            try
            {
                List<BackLogsDTO> lstBL = _context.tblBL_CatBackLogs.Where(x => x.idSegPpto == idSegPpto && x.fechaInstaladoBL == null && x.tipoBL == 2 && x.esActivo).Select(x => new BackLogsDTO
                {
                    id = x.id,
                    folioBL = x.folioBL,
                    noEconomico = x.noEconomico,
                    horas = x.horas,
                    descripcion = x.descripcion,
                    presupuestoEstimado = x.presupuestoEstimado
                }).ToList();
                return lstBL;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetBLPptos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        #region LLENADO DE COMBOS
        public List<ComboDTO> FillCboModeloTMC()
        {
            var dataModelo = _context.tblM_CatModeloEquipo.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

            return dataModelo;
        }

        public List<ComboDTO> FillCboGrupoTMC()
        {
            var dataGrupo = _context.tblM_CatGrupoMaquinaria.Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = x.descripcion
            }).ToList();

            return dataGrupo;
        }
        #endregion

        #region CATÁLOGO DE FRENTES
        public List<ComboDTO> FillcboUsuarios()
        {
            List<ComboDTO> lstEmpleadosComboDTO = new List<ComboDTO>();
            ComboDTO objEmpleadoComboDTO = new ComboDTO();
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    List<tblRH_EK_Empleados> lstEmpleados = _context.tblRH_EK_Empleados.Where(w => w.estatus_empleado == "A" && w.esActivo).OrderBy(o => o.nombre).ToList();

                    foreach (var item in lstEmpleados)
                    {
                        objEmpleadoComboDTO = new ComboDTO();
                        objEmpleadoComboDTO.Value = item.clave_empleado.ToString();
                        objEmpleadoComboDTO.Text = PersonalUtilities.NombreCompletoMayusculas(item.nombre, item.ape_paterno, item.ape_materno);
                        lstEmpleadosComboDTO.Add(objEmpleadoComboDTO);
                    }
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                {
                    #region ARRENDADORA
                    using (var _ctx = new MainContext(EmpresaEnum.Construplan))
                    {
                        List<tblRH_EK_Empleados> lstEmpleados = _ctx.tblRH_EK_Empleados.Where(w => w.estatus_empleado == "A" && w.esActivo).OrderBy(o => o.nombre).ToList();

                        foreach (var item in lstEmpleados)
                        {
                            objEmpleadoComboDTO = new ComboDTO();
                            objEmpleadoComboDTO.Value = item.clave_empleado.ToString();
                            objEmpleadoComboDTO.Text = PersonalUtilities.NombreCompletoMayusculas(item.nombre, item.ape_paterno, item.ape_materno);
                            lstEmpleadosComboDTO.Add(objEmpleadoComboDTO);
                        }
                    }

                    using (var _ctx = new MainContext(EmpresaEnum.Arrendadora))
                    {
                        List<tblRH_EK_Empleados> lstEmpleados = _ctx.tblRH_EK_Empleados.Where(w => w.estatus_empleado == "A" && w.esActivo).OrderBy(o => o.nombre).ToList();

                        foreach (var item in lstEmpleados)
                        {
                            objEmpleadoComboDTO = new ComboDTO();
                            objEmpleadoComboDTO.Value = item.clave_empleado.ToString();
                            objEmpleadoComboDTO.Text = PersonalUtilities.NombreCompletoMayusculas(item.nombre, item.ape_paterno, item.ape_materno);
                            lstEmpleadosComboDTO.Add(objEmpleadoComboDTO);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillcboUsuarios", e, AccionEnum.FILLCOMBO, 0, 0);
            }
            return lstEmpleadosComboDTO;
        }



        public List<CatFrentesDTO> GetFrentes()
        {
            try
            {
                var lstUsuarios = FillcboUsuarios();
                var lst = new List<tblP_Usuario>();
                var obj = new tblP_Usuario();
                foreach (var item in lstUsuarios)
                {
                    obj = new tblP_Usuario();
                    obj.id = Convert.ToInt32(item.Value);
                    obj.nombre = item.Text;
                    lst.Add(obj);
                }

                List<int> lstUsuariosId = _context.tblbl_CatFrentes.Where(x => x.esActivo).Select(x => x.idUsuarioAsignado).ToList();

                List<tblP_Usuario> lstUsuario = lst.Where(x => lstUsuariosId.Contains(x.id)).ToList().Select(y => new tblP_Usuario
                {
                    id = y.id,
                    nombre = y.nombre
                }).ToList();

                var lstusuarioExis = lstUsuario.Select(y => y.id).ToList();
                List<tblBL_CatFrentes> lstFren = _context.tblbl_CatFrentes.Where(x => x.esActivo && lstusuarioExis.Contains(x.idUsuarioAsignado)).ToList();
                List<CatFrentesDTO> lstFrentes = lstFren.Select(x => new CatFrentesDTO
                {
                    id = x.id,
                    nombreFrente = x.nombreFrente,
                    UsuarioCreacion = lstUsuario.Where(r => r.id == x.idUsuarioAsignado).FirstOrDefault() == null ? "" : lstUsuario.Where(r => r.id == x.idUsuarioAsignado).Select(y => y.nombre).FirstOrDefault(),
                    idUsuarioAsignado = lstUsuario.Where(r => r.id == x.idUsuarioAsignado).FirstOrDefault() == null ? 0 : lstUsuario.Where(r => r.id == x.idUsuarioAsignado).Select(y => y.id).FirstOrDefault()
                }).ToList();
                return lstFrentes;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetFrentes", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool CrearFrente(tblBL_CatFrentes objFrente)
        {
            try
            {
                if (!string.IsNullOrEmpty(objFrente.nombreFrente.ToUpper().Trim()))
                {
                    objFrente.nombreFrente.Trim();
                    objFrente.fechaCreacion = DateTime.Now;
                    objFrente.fecheModificacion = DateTime.Now;
                    objFrente.esActivo = true;
                    _context.tblbl_CatFrentes.Add(objFrente);
                    _context.SaveChanges();
                }
                else
                    throw new Exception("Es necesario indicar el nombre del frente.");

                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearFrente", e, AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objFrente));
                return false;
            }
        }

        public bool ActualizarFrente(tblBL_CatFrentes objFrente)
        {
            try
            {

                if (objFrente.id > 0)
                {
                    if (!string.IsNullOrEmpty(objFrente.nombreFrente.Trim()))
                    {
                        var ActualizarFrente = _context.tblbl_CatFrentes.Where(x => x.id == objFrente.id).First();
                        ActualizarFrente.nombreFrente = objFrente.nombreFrente.Trim();
                        ActualizarFrente.idUsuarioAsignado = objFrente.idUsuarioAsignado;
                        ActualizarFrente.fechaCreacion = DateTime.Now;
                        ActualizarFrente.fecheModificacion = DateTime.Now;

                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Es necesario indicar el nombre del frente.");
                }
                return false;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarFrente", ex, AccionEnum.ACTUALIZAR, objFrente.id, JsonUtils.convertNetObjectToJson(objFrente));
                return false;
            }
        }

        public bool EliminarFrente(int idFrente)
        {
            try
            {

                tblBL_DetFrentes objSeguimiento = _context.tblBL_DetFrentes.Where(x => x.idFrente == idFrente && x.esActivo == true).FirstOrDefault();
                if (objSeguimiento == null)
                {
                    tblBL_CatFrentes EliminarFrente = _context.tblbl_CatFrentes.Where(x => x.id == idFrente).FirstOrDefault();
                    if (EliminarFrente != null)
                    {
                        EliminarFrente.esActivo = false;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Hubo un error al obtener los datos.");
                    }
                }
                else
                {
                    throw new Exception("No se puede eliminar este frente ya que se encuentra asignado a un presupuesto.");
                }




            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarFrente", ex, AccionEnum.ELIMINAR, idFrente, JsonUtils.convertNetObjectToJson(idFrente));
                return false;
            }
        }
        #endregion

        #region SEGUIMIENTO PPTO
        public List<SeguimientoPptoDTO> GetSeguimientoPpto(string AreaCuenta, string motivo, int estatusPpto)
        {
            try
            {
                int mov = 0;
                if (motivo != "")
                    mov = Convert.ToInt32(motivo);

                List<SeguimientoPptoDTO> lstSeguimiento = _context.tblBL_SeguimientoPptos
                    .Where(x => x.esActivo && x.lstInspeccionesTMC.idMotivo == (motivo == "" ? x.lstInspeccionesTMC.idMotivo : mov) && x.EstatusSegPpto == (int)estatusPpto)
                    .OrderByDescending(x => x.folioPpto)
                    .Select(x => new SeguimientoPptoDTO
                {
                    id = x.id,
                    folioPpto = x.folioPpto,
                    fechaPpto = x.fechaPpto,
                    cc = x.lstCatMaquinas.noEconomico,
                    modelo = x.lstCatMaquinas.modeloEquipo.descripcion,
                    descripcion = x.lstCatMaquinas.descripcion,
                    horas = x.horas,
                    vobo1 = x.esVobo1 == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.esVobo1 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    vobo2 = x.esVobo1 == (int)EstatusSeguimientosPptoEnum.RECHAZADO ? "" : x.esVobo1 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "N/A" : x.esVobo2 == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.esVobo2 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    autorizado = x.esVobo1 == (int)EstatusSeguimientosPptoEnum.RECHAZADO ? "" : x.esVobo2 == (int)EstatusSeguimientosPptoEnum.RECHAZADO ? "" : x.esVobo1 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "N/A" : x.esVobo2 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "N/A" : x.esAutorizado == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.esAutorizado == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    fechaVobo1 = x.fechaVobo1,
                    fechaVobo2 = x.fechaVobo2,
                    fechaAutorizado = x.fechaAutorizado,
                    //dias = GetDiasTranscurridosPpto(x.esAutorizado, x.fechaAutorizado, x.fechaCreacion, x.esVobo2, x.fechaVobo2, x.esVobo1, x.fechaVobo1),
                    dias = 0,
                    //x.esAutorizado == 1 ? x.fechaAutorizado.Day - x.fechaCreacion.Day : x.esAutorizado == 0 ? x.fechaAutorizado.Day - x.fechaCreacion.Day : x.esVobo2 == 0 ? x.fechaVobo2.Day - x.fechaCreacion.Day : x.esVobo1 == 0 ? x.fechaVobo1.Day - x.fechaCreacion.Day : DateTime.Now.Day - x.fechaCreacion.Day,
                    tipoMotivo = x.lstInspeccionesTMC.idMotivo == (int)MotivoInspeccionEnum.Obra ? "OBRA" : "VENTA",
                    fechaRequerido = _context.tblBL_InspeccionesTMC.Where(r => r.id == x.idInspTMC).ToList().Select(s => s.fechaRequerido).FirstOrDefault(),
                    estado = x.EstatusSegPpto == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.EstatusSegPpto == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    EstatusSegPpto = x.EstatusSegPpto,
                    Ppto = x.Ppto,
                    firmaVobo1 = x.firmaVobo1,
                    idPuestoVobo1 = x.idPuestoVobo1,
                    firmaVobo2 = x.firmaVobo2,
                    firmaAutorizado = x.firmaAutorizado,
                    idUserVobo1 = x.idUserVobo1,
                    idUserVobo2 = x.idUserVobo2,
                    idUserAutorizado = x.idUserAutorizado,
                    esVobo1 = x.esVobo1,
                    esVobo2 = x.esVobo2,
                    esAutorizado = x.esAutorizado,
                    fechaCreacion = x.fechaCreacion
                }).ToList();

                foreach (var item in lstSeguimiento)
                {
                    item.dias = GetDiasTranscurridosPpto(item.esAutorizado, item.fechaAutorizado, item.fechaCreacion, item.esVobo2, item.fechaVobo2, item.esVobo1, item.fechaVobo1);
                }

                var lst = lstSeguimiento.Select(x => new SeguimientoPptoDTO
                {
                    id = x.id,
                    folioPpto = x.folioPpto,
                    fechaPpto = x.fechaPpto,
                    cc = x.cc,
                    modelo = x.modelo,
                    descripcion = x.descripcion,
                    horas = x.horas,
                    vobo1 = x.vobo1,
                    vobo2 = x.vobo2,
                    autorizado = x.autorizado,
                    fechaVobo1 = x.fechaVobo1,
                    fechaVobo2 = x.fechaVobo2,
                    fechaAutorizado = x.fechaAutorizado,
                    dias = x.dias,
                    tipoMotivo = x.tipoMotivo,
                    fechaRequerido = x.fechaRequerido,
                    estado = x.estado,
                    Ppto = x.Ppto,
                    firmaVobo1 = x.firmaVobo1,
                    idPuestoVobo1 = x.idPuestoVobo1,
                    firmaVobo2 = x.firmaVobo2,
                    firmaAutorizado = x.firmaAutorizado,
                    idUserVobo1 = x.idUserVobo1,
                    idUserVobo2 = x.idUserVobo2,
                    idUserAutorizado = x.idUserAutorizado,
                    nombreUserVobo1 = nombreUsuario(x.idUserVobo1),
                    nombreUserVobo2 = nombreUsuario(x.idUserVobo2),
                    nombreUserAutorizado = nombreUsuario(x.idUserAutorizado),
                    esVobo1 = x.esVobo1,
                    esVobo2 = x.esVobo2,
                    esAutorizado = x.esAutorizado,
                }).ToList();

                return lst;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetSeguimientoPpto", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public int GetDiasTranscurridosPpto(int esAutorizado, DateTime fechaAutorizado, DateTime fechaCreacion, int esVobo2, DateTime fechaVobo2, int esVobo1, DateTime fechaVobo1)
        {
            int cantDiasTranscurridos = 0;
            try
            {
                //x.esAutorizado == 1 ? x.fechaAutorizado.Day - x.fechaCreacion.Day : 
                //x.esAutorizado == 0 ? x.fechaAutorizado.Day - x.fechaCreacion.Day : 
                //x.esVobo2 == 0 ? x.fechaVobo2.Day - x.fechaCreacion.Day : 
                //x.esVobo1 == 0 ? x.fechaVobo1.Day - x.fechaCreacion.Day : 
                //DateTime.Now.Day - x.fechaCreacion.Day,

                if (esAutorizado == 1)
                {
                    cantDiasTranscurridos = (fechaAutorizado - fechaCreacion).Days;
                }
                else if (esVobo2 == 1)
                {
                    cantDiasTranscurridos = (fechaVobo2 - fechaCreacion).Days;
                }
                else if (esVobo1 == 1)
                {
                    cantDiasTranscurridos = (fechaVobo1 - fechaCreacion).Days;
                }
                else
                {
                    cantDiasTranscurridos = (DateTime.Now - fechaCreacion).Days;
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDiasTranscurridosPpto", e, AccionEnum.CONSULTA, 0, 0);
            }
            return cantDiasTranscurridos;
        }

        public string nombreUsuario(int id)
        {
            try
            {
                if (id > 0)
                {
                    List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(r => r.estatus == true).ToList();
                    var retornar = lstUsuarios.Where(y => y.id == id).ToList().Select(n => n.nombre + " " + n.apellidoPaterno + " " + n.apellidoMaterno).FirstOrDefault().ToString();
                    return retornar;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "nombreUsuario", e, AccionEnum.CONSULTA, id, 0);
                return null;
            }
        }
        public List<SeguimientoPptoDTO> GetSeguimientoPptoFrentes(string AreaCuenta, string ObraoRenta, int idFrente)
        {
            try
            {
                int mov = 0;
                if (ObraoRenta != "")
                    mov = Convert.ToInt32(ObraoRenta);

                List<SeguimientoPptoDTO> lstSeguimiento = _context.tblBL_SeguimientoPptos
                    .Where(x => x.esActivo && x.EstatusSegPpto == 1 && x.lstInspeccionesTMC.idMotivo == (mov == 0 ? x.lstInspeccionesTMC.idMotivo : mov) && x.idFrente == idFrente)
                    .OrderByDescending(x => x.folioPpto).Select(x => new SeguimientoPptoDTO
                {
                    id = x.id,
                    idInspTMC = x.idInspTMC,
                    folioPpto = x.folioPpto,
                    fechaPpto = x.fechaPpto,
                    cc = x.lstCatMaquinas.noEconomico,
                    idFrente = x.idFrente,
                    modelo = x.lstCatMaquinas.modeloEquipo.descripcion,
                    descripcion = x.lstCatMaquinas.descripcion,
                    horas = x.horas,
                    vobo1 = x.esVobo1 == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.esVobo1 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    vobo2 = x.esVobo2 == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.esVobo2 == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    autorizado = x.esAutorizado == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.esAutorizado == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    fechaVobo1 = x.fechaVobo1,
                    fechaVobo2 = x.fechaVobo2,
                    fechaAutorizado = x.fechaAutorizado,
                    dias = 0,
                    tipoMotivo = x.lstInspeccionesTMC.idMotivo == (int)MotivoInspeccionEnum.Obra ? "OBRA" : "VENTA",
                    fechaRequerido = _context.tblBL_InspeccionesTMC.Where(r => r.id == x.idInspTMC).ToList().Select(s => s.fechaRequerido).FirstOrDefault(),
                    estado = x.EstatusSegPpto == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.EstatusSegPpto == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
                    Ppto = x.Ppto
                }).ToList();

                return lstSeguimiento;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetSeguimientoPpto", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        private int DiasTranscurridosSegPpto(DateTime fechaCreacionBL, DateTime fechaVobo1, DateTime fechaVobo2, DateTime fechaAutorizado)
        {
            return 0;

        }

        //public List<SeguimientoPptoDTO> GetSeguimientoPptoFiltros(tblBL_SeguimientoPptos obj)
        //{
        //    try
        //    { //_context.tblBL_SeguimientoPptos.Where(x => x.esActivo).OrderByDescending(x => x.folioPpto).Select(x => new SeguimientoPptoDTO
        //        List<SeguimientoPptoDTO> lstSeguimiento = _context.tblBL_SeguimientoPptos.Where(x => x.esActivo).Where(r=>
        //            (obj.idInspTMC !=null ? r.lstInspeccionesTMC.idMotivo == obj.idInspTMC : true)).OrderByDescending(x => x.folioPpto).ToList().Select(x => new SeguimientoPptoDTO
        //        {
        //            id = x.id,
        //            folioPpto = x.folioPpto,
        //            fechaPpto = x.fechaPpto, 
        //            cc = x.lstCatMaquinas.noEconomico,
        //            modelo = x.lstCatMaquinas.modeloEquipo.descripcion,
        //            descripcion = x.lstCatMaquinas.descripcion,                    
        //            horas = x.horas,
        //            tipoMotivo = x.lstInspeccionesTMC.idMotivo == (int)MotivoInspeccionEnum.Obra ? "OBRA" : "VENTA",                      
        //            fechaRequerido = _context.tblBL_InspeccionesTMC.Where(r => r.id == x.idInspTMC).ToList().Select(s => s.fechaRequerido).FirstOrDefault(),                            
        //            estado = x.EstatusSegPpto == (int)EstatusSeguimientosPptoEnum.AUTORIZADO ? "AUTORIZADO" : x.EstatusSegPpto == (int)EstatusSeguimientosPptoEnum.EN_ESPERA ? "EN ESPERA" : "RECHAZADO",
        //            Ppto = x.Ppto 
        //        }).ToList();
        //        return lstSeguimiento;
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetSeguimientoPpto", e, AccionEnum.CONSULTA, 0, 0);
        //        return null;
        //    }
        //}

        public List<ComboDTO> FillCboFrentes()
        {
            try
            {
                return _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, nombreFrente AS Text FROM tblBL_CatFrentes WHERE idUsuarioAsignado > 0 AND esActivo = 1"
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboFrentes", e, AccionEnum.FILLCOMBO, 0, 0);
                return null;
            }
        }

        #endregion

        #region DETALLE FRENTE
        public List<DetFrentesDTO> GetDetFrentes(string AreaCuenta, List<int> lstFrentes, int estatusSeguimientoFrente)
        {
            try
            {
                List<tblBL_CatBackLogs> lstCatBackLogs = _context.tblBL_CatBackLogs.Where(w => w.tipoBL == (int)TipoBackLogEnum.TMC && w.esActivo).ToList();
                List<tblBL_Evidencias> lstEvidencias = _context.tblBL_Evidencias.Where(w => w.esActivo).ToList();
                List<tblBL_CatFrentes> lstCatFrentes = _context.tblbl_CatFrentes.Where(w => w.esActivo).ToList();
                List<tblBL_InspeccionesTMC> lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(w => w.esActivo).ToList();

                List<tblBL_DetFrentes> lstDetFrentes = _context.tblBL_DetFrentes.Where(w => w.esActivo).ToList();
                if (lstFrentes != null)
                    lstDetFrentes = lstDetFrentes.Where(w => lstFrentes.Contains(w.idFrente)).ToList();

                List<DetFrentesDTO> lstDetFrentesDTO = _context.tblBL_DetFrentes.OrderByDescending(o => o.id).Select(x => new DetFrentesDTO
                {
                    id = x.id,
                    Frente = string.Empty,
                    lstSeguimiento = x.lstSeguimiento.noEconomico,
                    folioPpto = x.lstSeguimiento.folioPpto,
                    fechaAsignacion = x.fechaAsignacion,
                    avance = 0,
                    fechaRequerido = DateTime.Now,
                    fechaPromesa = DateTime.Now,
                    tipoMotivo = string.Empty,
                    areaCuenta = x.lstSeguimiento.areaCuenta,
                    idInspTMC = x.idInspTMC,
                    idSeguimientoPpto = x.idSeguimientoPpto
                }).ToList();

                List<int> lstInspeccionesDeshabilitadasID = new List<int>();
                foreach (var item in lstDetFrentesDTO)
                {
                    #region SE OBTIENE NOMBRE DEL FRENTE
                    tblBL_CatFrentes objCatFrente = lstCatFrentes.Where(w => w.id == item.idFrente).FirstOrDefault();
                    if (objCatFrente != null)
                        item.Frente = !string.IsNullOrEmpty(objCatFrente.nombreFrente) ? objCatFrente.nombreFrente.Trim().ToUpper() : string.Empty;
                    #endregion

                    #region SE OBTIENE FECHA REQUERIDO | FECHA PROMESA | TIPO MOTIVO
                    tblBL_InspeccionesTMC objInspeccionTMC = lstInspeccionesTMC.Where(w => w.id == item.idInspTMC).FirstOrDefault();
                    if (objInspeccionTMC != null)
                    {
                        item.fechaRequerido = objInspeccionTMC.fechaRequerido;
                        item.fechaPromesa = objInspeccionTMC.fechaPromesa;
                        item.tipoMotivo = (int)objInspeccionTMC.idMotivo == (int)MotivoInspeccionEnum.Obra ? "OBRA" : "VENTA";

                        #region SE VERIFICA SI EL SEGUIMIENTO HA CONCLUIDO
                        // CANT DE BACKLOG ASIGNADOS AL SEGUIMIENTO DEL PPTO
                        List<tblBL_CatBackLogs> lstBLRelPptoSeguimiento = lstCatBackLogs.Where(w => w.idSegPpto == item.idSeguimientoPpto && w.tipoBL == (int)TipoBackLogEnum.TMC && w.esActivo).ToList();

                        int cantBLPendientes = lstBLRelPptoSeguimiento.Where(w => w.idEstatus != (int)EstatusBackLogEnum.BackLogsInstalado).Count();
                        int cantBLTerminados = lstBLRelPptoSeguimiento.Where(w => w.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado).Count();

                        if (cantBLPendientes > 0 && cantBLTerminados > 0)
                            item.avance = ((decimal)cantBLTerminados / (decimal)cantBLPendientes) * 100;
                        else
                            item.avance = 100; // SEGUIMIENTO TERMINADO AL 100%
                        #endregion
                    }
                    else
                        lstInspeccionesDeshabilitadasID.Add(item.idInspTMC);
                    #endregion
                }

                switch (estatusSeguimientoFrente)
                {
                    case 1: // SEGUIMIENTOS PENDIENTES
                        lstDetFrentesDTO = lstDetFrentesDTO.Where(w => w.avance <= 99).ToList();
                        break;
                    case 2: // SEGUIMIENTOS TERMINADOS
                        lstDetFrentesDTO = lstDetFrentesDTO.Where(w => w.avance == 100).ToList();
                        break;
                }
                return lstDetFrentesDTO.Where(w => !lstInspeccionesDeshabilitadasID.Contains(w.idInspTMC)).ToList();
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDetFrentes", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool CrearDetFrentes(List<Seguimiento2DTO> parametros)
        {

            try
            {
                foreach (var item in parametros)
                {
                    var objPPseg = _context.tblBL_SeguimientoPptos.Where(r => r.id == item.idSeguimientoPpto).FirstOrDefault();
                    if (objPPseg != null)
                        objPPseg.idFrente = Convert.ToInt32(item.idFrente);

                    var objCatBacklogs = _context.tblBL_CatBackLogs.Where(r => r.idSegPpto == item.idSeguimientoPpto).FirstOrDefault();
                    if (objCatBacklogs != null)
                        objCatBacklogs.idEstatus = (int)EstatusBackLogsTMCEnum.RehabilitacionProgramada;

                    _context.SaveChanges();

                    var objDet = _context.tblBL_DetFrentes.Where(x => x.idSeguimientoPpto == item.idSeguimientoPpto).FirstOrDefault();
                    if (objDet == null)
                    {
                        #region SE RELACIONA EL FRENTE AL SEGUIMIENTO DE PPTO
                        objDet = new tblBL_DetFrentes();
                        objDet.idFrente = Convert.ToInt32(item.idFrente);
                        objDet.idSeguimientoPpto = Convert.ToInt32(item.idSeguimientoPpto);
                        objDet.fechaAsignacion = DateTime.Now;
                        objDet.avance = 0;
                        objDet.idInspTMC = Convert.ToInt32(item.idInspTMC);
                        objDet.fechaCreacion = DateTime.Now;
                        objDet.fechaModificacion = DateTime.Now;
                        objDet.esActivo = true;
                        _context.tblBL_DetFrentes.Add(objDet);
                        _context.SaveChanges();
                        #endregion

                        #region SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                        //List<tblBL_BitacoraEstatusBL> lstBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.esActivo).ToList();
                        //int idBL = 0;
                        //string areaCuenta = objCatBacklogs.areaCuenta;
                        //int diasTranscurridos = 0;
                        //tblBL_BitacoraEstatusBL objBitacoraBL = lstBitacoraBL.Where(w => w.idBL == idBL).OrderByDescending(o => o.id).FirstOrDefault();
                        //if (objBitacoraBL != null)
                        //    diasTranscurridos = (DateTime.Now - objBitacoraBL.fechaCreacion).Days;

                        //tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                        //objGuardarBitacoraEstatusBL.idBL = idBL;
                        //objGuardarBitacoraEstatusBL.areaCuenta = areaCuenta;
                        //objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                        //objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogsTMCEnum.SuministroRefacciones;
                        //objGuardarBitacoraEstatusBL.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        //objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                        //objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                        //objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                        //objGuardarBitacoraEstatusBL.esActivo = true;
                        //_context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                        //_context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        objDet.idFrente = Convert.ToInt32(item.idFrente);
                        objDet.idSeguimientoPpto = Convert.ToInt32(item.idSeguimientoPpto);
                        objDet.fechaAsignacion = DateTime.Now;
                        objDet.avance = 0;
                        objDet.idInspTMC = Convert.ToInt32(item.idInspTMC);
                        objDet.fechaCreacion = DateTime.Now;
                        objDet.fechaModificacion = DateTime.Now;
                        objDet.esActivo = true;
                        _context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, "ContratosController", "Divisiones_Proyectos", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool EliminarDetFrente(int idFrente)
        {
            try
            {

                if (idFrente > 0)
                {
                    var EliminarFrente = _context.tblBL_DetFrentes.Where(x => x.id == idFrente).First();
                    var objSeguimiento = _context.tblBL_SeguimientoPptos.Where(x => x.id == idFrente).FirstOrDefault();
                    var objBacklog = _context.tblBL_CatBackLogs.Where(x => x.id == idFrente).FirstOrDefault();

                    if (EliminarFrente != null)
                    {
                        EliminarFrente.esActivo = false;
                        objSeguimiento.idFrente = 0;
                        objBacklog.idEstatus = (int)EstatusBackLogsTMCEnum.SuministroRefacciones;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Hubo un error al obtener los datos.");
                }
                else
                    throw new Exception("Hubo un error al obtener los datos.");
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarFrente", ex, AccionEnum.ELIMINAR, idFrente, JsonUtils.convertNetObjectToJson(idFrente));
                return false;
            }
        }
        #endregion

        #region INFORME TMC
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

        public bool ActualizarLiberacion(string areaCuenta)
        {
            var r = new Dictionary<string, object>();

            try
            {
                List<tblBL_CatBackLogs> lstCatBacklogs = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.noEconomico == areaCuenta && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.areaCuenta == "1010").ToList();
                int BL = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.noEconomico == areaCuenta && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado).Count();

                List<tblBL_Evidencias> lstEvidencias = _context.tblBL_Evidencias.Where(x => x.esActivo).ToList();
                var lstBL = _context.tblBL_CatBackLogs.Where(x => x.esActivo).ToList();
                List<int> lstBLId = new List<int>();
                foreach (var item2 in lstCatBacklogs)
                {
                    lstBLId.Add(item2.id);
                }
                if (BL > 0)
                {
                    int cantTotal = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.tipoBL == (int)TipoBackLogEnum.TMC).Count();
                    for (int i = 0; i < lstBLId.Count(); i++)
                    {
                        int idBL = lstBLId[i];
                        int cantBLEvidencias = lstEvidencias.Where(x => x.idBL == lstBLId[i] && x.esActivo).Count();

                        if (cantBLEvidencias > 0)
                        {
                            int esTerminado = _context.tblBL_CatBackLogs.Where(x => x.id == idBL && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.esActivo).Count();
                            if (esTerminado > 0)
                            {
                                tblBL_CatBackLogs objActualizar = lstBL.Where(x => x.id == idBL && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.esActivo).FirstOrDefault();
                                objActualizar.esLiberado = true;
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            throw new Exception("no");
                        }
                    }
                }
                else
                {
                    throw new Exception("no");
                }
                return true;
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarLiberacion", ex, AccionEnum.ACTUALIZAR, 0, 0);
                return false;
            }
        }

        public Dictionary<string, object> GetModeloGrupoCCSeleccionado(string noEconomico)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(noEconomico)) { throw new Exception("Ocurrió un error al obtener el modelo y grupo del CC."); }
                #endregion

                #region SE OBTIENE MODELO Y GRUPO DEL CC SELECCIONADO
                tblM_CatMaquina objMaquina = _context.tblM_CatMaquina.Where(w => w.noEconomico == noEconomico && w.estatus == 1).FirstOrDefault();
                if (objMaquina == null)
                { throw new Exception("Ocurrió un error al obtener el modelo y grupo del CC."); }

                resultado.Add(SUCCESS, true);
                resultado.Add("modelo", objMaquina.modeloEquipo.descripcion);
                resultado.Add("grupo", objMaquina.grupoMaquinaria.descripcion);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetModeloGrupoCCSeleccionado", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #region SUBIR EVIDENCIAS EN INSPECCIONES
        public Dictionary<string, object> postSubirArchivos(int id, List<HttpPostedFileBase> archivo, int tipoEvidencia)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    tblBL_CatBackLogs obtenerClaveEmpleado = _context.tblBL_CatBackLogs.Where(r => r.id == id).FirstOrDefault();
                    var CarpetaNueva = Path.Combine(_RUTA_BACKLOGS_SERVIDOR, obtenerClaveEmpleado.id.ToString());
                    verificarExisteCarpeta(CarpetaNueva, true);
                    foreach (var arch in archivo)
                    {
                        string nombreArchivo = ObtenerFormatoNombreArchivoA("BackLogs_", arch.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(arch, rutaArchivo));

                        // GUARDAR TABLA ARCHIVOS
                        if (tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaOT)
                        {
                            var obj = new tblBL_Evidencias()
                            {
                                idBL = id,
                                nombreArchivo = nombreArchivo,
                                rutaArchivo = rutaArchivo,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = DateTime.Now,
                                tipoEvidencia = (int)TipoEvidenciaEnumBL.evidenciaOT,
                                esActivo = true
                            };
                            _context.tblBL_Evidencias.Add(obj);
                            _context.SaveChanges();
                        }
                        else if (tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaSeguridad)
                        {
                            var objTMC = new tblBL_Evidencias()
                            {
                                idBL = id,
                                nombreArchivo = nombreArchivo,
                                rutaArchivo = rutaArchivo,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = DateTime.Now,
                                tipoEvidencia = (int)TipoEvidenciaEnumBL.evidenciaSeguridad,
                                esActivo = true
                            };
                            _context.tblBL_Evidencias.Add(objTMC);
                            _context.SaveChanges();
                        }
                        else if (tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaOTVacia)
                        {
                            var objGuardar = new tblBL_Evidencias()
                            {
                                idBL = id,
                                nombreArchivo = nombreArchivo,
                                rutaArchivo = rutaArchivo,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = new DateTime(2000, 01, 01),
                                tipoEvidencia = (int)TipoEvidenciaEnumBL.evidenciaOTVacia,
                                esActivo = true
                            };
                            _context.tblBL_Evidencias.Add(objGuardar);
                            _context.SaveChanges();
                        }
                        else if (tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaOTLiberar)
                        {
                            var objGuardar = new tblBL_Evidencias()
                            {
                                idBL = id,
                                nombreArchivo = nombreArchivo,
                                rutaArchivo = rutaArchivo,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = new DateTime(2000, 01, 01),
                                tipoEvidencia = (int)TipoEvidenciaEnumBL.evidenciaOTLiberar,
                                esActivo = true
                            };
                            _context.tblBL_Evidencias.Add(objGuardar);
                            _context.SaveChanges();
                        }
                        else if (tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaOrdenBL)
                        {
                            var objGuardar = new tblBL_Evidencias()
                            {
                                idBL = id,
                                nombreArchivo = nombreArchivo,
                                rutaArchivo = rutaArchivo,
                                fechaCreacion = DateTime.Now,
                                fechaModificacion = new DateTime(2000, 01, 01),
                                tipoEvidencia = (int)TipoEvidenciaEnumBL.evidenciaOrdenBL,
                                esActivo = true
                            };
                            _context.tblBL_Evidencias.Add(objGuardar);
                            _context.SaveChanges();
                        }
                        index++;
                    }

                    foreach (var arch in listaRutaArchivos)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(arch.Item1, arch.Item2) == false)
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
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "postSubirArchivos", e, AccionEnum.AGREGAR, 0, new { });
                }
            }
            return resultado;
        }

        private string ObtenerFormatoNombreArchivoA(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        public Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio)
        {
            try
            {
                var result = _context.tblBL_Evidencias.Where(r => r.idBL == idFormatoCambio).ToList();

                resultado.Add("data", result);
            }
            catch (Exception o_O)
            {
                resultado.Add("data", "ocurrio algun error");
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region  MOSTRAR ARCHIVOS
        public List<BackLogs_ArchivosDTO> GetArchivos(int id)
        {
            try
            {
                List<BackLogs_ArchivosDTO> lstArchivos = _context.tblBL_Evidencias.Where(x => x.idBL == id && x.esActivo).Select(x => new BackLogs_ArchivosDTO
                {
                    id = x.id,
                    nombreArchivo = x.nombreArchivo,
                    tipoEvidencia = string.Empty,
                    intTipoEvidencia = x.tipoEvidencia,
                    rutaArchivo = x.rutaArchivo
                }).ToList();
                foreach (var item in lstArchivos)
                {
                    switch (item.intTipoEvidencia)
                    {
                        case 1:
                            item.tipoEvidencia = "Evidencia para la OT";
                            break;
                        case 2:
                            item.tipoEvidencia = "Evidencia de seguridad";
                            break;
                        case 3:
                            item.tipoEvidencia = "Evidencia OT vacía";
                            break;
                        case 4:
                            item.tipoEvidencia = "Evidencia OT liberar";
                            break;
                        case 5:
                            item.tipoEvidencia = "Evidencia Orden BL";
                            break;
                        default:
                            item.tipoEvidencia = string.Empty;
                            break;
                    }
                }
                return lstArchivos;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetArchivos", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }

        public bool EliminarArchivos(int id)
        {
            try
            {
                if (id > 0)
                {
                    var EliminarArchivo = _context.tblBL_Evidencias.Where(x => x.id == id).First();
                    if (EliminarArchivo != null)
                    {
                        EliminarArchivo.esActivo = false;
                        _context.SaveChanges();
                        return true;
                    }
                    else
                        throw new Exception("Hubo un error al obtener los datos.");
                }
                else
                    throw new Exception("Hubo un error al obtener los datos.");
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarFrente", ex, AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(id));
                return false;
            }
        }
        #endregion

        #region REPORTE GENERAL
        #endregion
        #endregion

        public Dictionary<string, object> GetReporteGeneral(int tipoBL, DateTime fechaInicio, DateTime fechaFin, string ac)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var infoReporteBL = new List<ReporteRehabilitacionDTO>();

                var economicos = _context.tblM_CatMaquina.ToList();

                var backlogs = _context.tblBL_CatBackLogs
                    .Where(w =>
                        w.tipoBL == tipoBL &&
                        w.esActivo &&
                        w.fechaInspeccion >= fechaInicio &&
                        w.fechaInspeccion <= fechaFin &&
                        w.areaCuenta == ac
                    ).ToList();

                var idsBacklogs = backlogs.Select(m => m.id).ToList();

                List<tblBL_OrdenesCompra> backlogsCompras = _context.tblBL_OrdenesCompra.Where(w => w.esActivo && idsBacklogs.Contains(w.idBackLog)).ToList();

                if (backlogsCompras != null && backlogsCompras.Count() > 0)
                {
                    List<string> listado = backlogsCompras.Select(m => m.cc + "-" + m.numRequisicion + "-" + m.numOC).ToList();

                    List<tblCom_OrdenCompra> lstComOrdenCompra = _context.tblCom_OrdenCompra.ToList();
                    List<tblCom_OrdenCompraDet> lstComOrdenCompraDet = _context.tblCom_OrdenCompraDet.ToList();

                    var infoCompra = lstComOrdenCompraDet
                        .Join(
                            lstComOrdenCompra,
                            detalle => detalle.idOrdenCompra,
                            orden => orden.id,
                            (detalle, orden) => new InfoOrdenCompraBLDTO
                            {
                                orden = orden,
                                ordenDet = detalle
                            }
                        )
                        .Where(w =>
                            w.ordenDet.estatusRegistro &&
                            w.orden.estatusRegistro &&
                            w.orden.estatus != "C" &&
                            listado.Contains((w.ordenDet.cc + "-" + w.ordenDet.num_requisicion + "-" + w.ordenDet.numero))
                        ).ToList();

                    foreach (var gbEconomicos in backlogs.GroupBy(g => g.noEconomico))
                    {
                        var economico = economicos.FirstOrDefault(f => f.noEconomico == gbEconomicos.Key);

                        var avances = AvanceBackLog(gbEconomicos.ToList());

                        var ordenCompraBL = backlogsCompras.Where(w => gbEconomicos.Select(m => m.id).Contains(w.idBackLog)).ToList();
                        var ordenCompra = infoCompra.Where(w => ordenCompraBL.Any(a => a.cc == w.ordenDet.cc && a.numRequisicion == w.ordenDet.num_requisicion.ToString() && a.numOC == w.ordenDet.numero.ToString())).ToList();

                        var costos = CostoRefaccion(ordenCompra);

                        var economicoBLogs = new ReporteRehabilitacionDTO();

                        economicoBLogs.numeroEconomico = gbEconomicos.Key.ToUpper();
                        economicoBLogs.descripcion = economico != null ? economico.grupoMaquinaria.descripcion.ToUpper() + " " + economico.modeloEquipo.descripcion.ToUpper() : "SIN DESCRIPCIÓN";
                        economicoBLogs.sistemaAReparar = string.Join(", ", gbEconomicos.Select(m => m.subconjunto.descripcion));
                        economicoBLogs.porcentajeAvance = (int)avances["avances"];
                        economicoBLogs.costoRefaccionDLL = (decimal)costos["usd"];
                        economicoBLogs.costoRefaccionMN = (decimal)costos["mxn"];
                        economicoBLogs.granTotal = economicoBLogs.costoRefaccionMN;
                        economicoBLogs.totalBL = gbEconomicos.Count();
                        economicoBLogs.totalBL = (int)avances["terminados"];

                        infoReporteBL.Add(economicoBLogs);
                    }

                    var descripcionAc = _context.tblP_CC.FirstOrDefault(f => f.areaCuenta == ac);
                    var excel = ExcelReporteGeneral(infoReporteBL, descripcionAc.descripcion);

                    r.Add(SUCCESS, true);
                    r.Add("excel", excel);
                }
                else
                {
                    r.Add(SUCCESS, false);
                    r.Add(MESSAGE, "No se encontraron compras relacionadas a Backlogs en el periodo seleccionado");
                }
            }
            catch (Exception ex)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetReporteGeneral", ex, AccionEnum.CONSULTA, 0, new { tipoBL, fechaInicio, fechaFin, ac });
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return r;
        }

        private MemoryStream ExcelReporteGeneral(List<ReporteRehabilitacionDTO> infoReporte, string ac)
        {
            using (var excel = new ExcelPackage())
            {
                //TITULO HOJA
                var excelDetalles = excel.Workbook.Worksheets.Add("REHABILITACION GENERAL");

                //MOTRAR CUADRICULA
                excelDetalles.View.ShowGridLines = false;

                //COMBINAR CELDAS TITULO
                excelDetalles.Cells[1, 1, 1, 12].Merge = true;
                excelDetalles.Cells[2, 1, 2, 12].Merge = true;
                excelDetalles.Cells[3, 1, 3, 12].Merge = true;
                excelDetalles.Cells[4, 1, 4, 12].Merge = true;
                excelDetalles.Cells[5, 1, 5, 12].Merge = true;
                excelDetalles.Cells[6, 1, 6, 12].Merge = true;

                //LOGOTIPO CONSTRUPLAN
                //var imgLogotipo = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/img/logo/logo.jpg"));
                var imgLogotipo = "";
                //var picLogoTipo = excelDetalles.Drawings.AddPicture("logotipoCP", imgLogotipo);
                //picLogoTipo.SetPosition(1, 0, 0, 50);

                var tituloDireccion = excelDetalles.MergedCells[3, 1];
                excelDetalles.Cells[tituloDireccion].Value = "Dirección de Maquinaria y Equipo";
                excelDetalles.Cells[tituloDireccion].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[tituloDireccion].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[tituloDireccion].Style.Font.Size = 16;
                excelDetalles.Cells[tituloDireccion].Style.Font.Bold = true;

                var tituloPrograma = excelDetalles.MergedCells[4, 1];
                excelDetalles.Cells[tituloPrograma].Value = "Programa de rehabilitación y mantenimiento de maquinaria y equipo";
                excelDetalles.Cells[tituloPrograma].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[tituloPrograma].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[tituloPrograma].Style.Font.Size = 20;
                excelDetalles.Cells[tituloPrograma].Style.Font.Bold = true;

                var tituloObra = excelDetalles.MergedCells[5, 1];
                excelDetalles.Cells[tituloObra].Value = "Obra: " + ac;
                excelDetalles.Cells[tituloObra].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[tituloObra].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[tituloObra].Style.Font.Size = 12;
                excelDetalles.Cells[tituloObra].Style.Font.Bold = true;

                var header = new List<string>
                {
                    "Número económico",
                    "Descripción",
                    "Sistema a reparar",
                    "Porcentaje de avance",
                    "Promedio",
                    "Real",
                    "Costo refacción (U.S.D.)",
                    "Costo refacción (M.N.)",
                    "Costo mano de obra",
                    "Gran total",
                    "Total",
                    "100%"
                };

                for (int i = 1; i <= header.Count; i++)
                {
                    excelDetalles.Cells[9, i].Value = header[i - 1];
                }

                var cellData = new List<object[]>();

                foreach (var info in infoReporte)
                {
                    cellData.Add(new object[]
                    {
                        info.numeroEconomico,
                        info.descripcion,
                        info.sistemaAReparar,
                        info.porcentajeAvance,
                        "",
                        "",
                        info.costoRefaccionDLL,
                        info.costoRefaccionMN,
                        info.costoManoObra,
                        info.granTotal,
                        info.totalBL,
                        info.totalInstalados
                    });
                }

                excelDetalles.Cells[10, 1].LoadFromArrays(cellData);

                //BARRA DE AVANCE
                ExcelAddress columnasAvance = new ExcelAddress(infoReporte.Count == 0 ? 9 : 10, 4, excelDetalles.Dimension.End.Row, 4);
                var reglaBarraProgresoAvance = excelDetalles.ConditionalFormatting.AddDatabar(columnasAvance, ColorTranslator.FromHtml("#1E90FF"));

                reglaBarraProgresoAvance.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightDown;
                reglaBarraProgresoAvance.LowValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                reglaBarraProgresoAvance.LowValue.Value = 0;
                reglaBarraProgresoAvance.HighValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                reglaBarraProgresoAvance.HighValue.Value = 100;

                ExcelRange range = excelDetalles.Cells[9, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];
                ExcelTable table = excelDetalles.Tables.Add(range, "TablaGeneral");

                table.TableStyle = TableStyles.Medium17;

                excelDetalles.Cells[infoReporte.Count == 0 ? 9 : 10, 7, excelDetalles.Dimension.End.Row, 10].Style.Numberformat.Format = "$#,##0.00";

                var renglonPromedio = excelDetalles.Dimension.End.Row + 1;

                //PROMEDIO
                excelDetalles.Cells[renglonPromedio, 2].Value = "PROMEDIO";
                excelDetalles.Cells[renglonPromedio, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 2].Style.Font.Size = 14;
                excelDetalles.Cells[renglonPromedio, 2].Style.Font.Bold = true;

                excelDetalles.Cells[renglonPromedio, 4].Value = infoReporte.Count == 0 ? 0 : infoReporte.Sum(s => s.porcentajeAvance) / infoReporte.Count;
                excelDetalles.Cells[renglonPromedio, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 4].Style.Font.Size = 14;
                excelDetalles.Cells[renglonPromedio, 4].Style.Font.Bold = true;

                ExcelAddress columnasAvancePromedio = new ExcelAddress(renglonPromedio, 4, renglonPromedio, 4);
                var reglaBarraProgresoAvancePromedio = excelDetalles.ConditionalFormatting.AddDatabar(columnasAvancePromedio, ColorTranslator.FromHtml("#1E90FF"));
                reglaBarraProgresoAvancePromedio.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightDown;
                reglaBarraProgresoAvancePromedio.LowValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                reglaBarraProgresoAvancePromedio.LowValue.Value = 0;
                reglaBarraProgresoAvancePromedio.HighValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                reglaBarraProgresoAvancePromedio.HighValue.Value = 100;

                excelDetalles.Cells[renglonPromedio, 11].Value = infoReporte.Sum(s => s.totalBL);
                excelDetalles.Cells[renglonPromedio, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 11].Style.Font.Size = 14;
                excelDetalles.Cells[renglonPromedio, 11].Style.Font.Bold = true;
                excelDetalles.Cells[renglonPromedio, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                excelDetalles.Cells[renglonPromedio, 11].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F0F8FF"));

                excelDetalles.Cells[renglonPromedio, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.Black);
                excelDetalles.Cells[renglonPromedio, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.Black);
                excelDetalles.Cells[renglonPromedio, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.Black);
                excelDetalles.Cells[renglonPromedio, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.Black);

                excelDetalles.Cells[renglonPromedio, 12].Value = infoReporte.Sum(s => s.totalInstalados);
                excelDetalles.Cells[renglonPromedio, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[renglonPromedio, 12].Style.Font.Size = 14;
                excelDetalles.Cells[renglonPromedio, 12].Style.Font.Bold = true;
                excelDetalles.Cells[renglonPromedio, 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                excelDetalles.Cells[renglonPromedio, 12].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F0F8FF"));

                var renglonPorcentajeInstalacion = renglonPromedio + 1;

                excelDetalles.Cells[renglonPorcentajeInstalacion, 8, renglonPorcentajeInstalacion, 10].Merge = true;
                var mergedPorcengajeInstalacion = excelDetalles.MergedCells[renglonPorcentajeInstalacion, 8];
                excelDetalles.Cells[mergedPorcengajeInstalacion].Value = "Porcentaje de Instalación";
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.Font.Size = 14;
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.Font.Bold = true;
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFA500"));
                excelDetalles.Cells[mergedPorcengajeInstalacion].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.Black);

                excelDetalles.Cells[renglonPorcentajeInstalacion, 11, renglonPorcentajeInstalacion, 12].Merge = true;
                var mergedPorcengajeInstalacionValor = excelDetalles.MergedCells[renglonPorcentajeInstalacion, 11];
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Value = infoReporte.Sum(s => s.totalBL) == 0 ? "0" : (((infoReporte.Sum(s => s.totalInstalados) * 100) / infoReporte.Sum(s => s.totalBL)) + "%");
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.Font.Size = 14;
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.Font.Bold = true;
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFA500"));
                excelDetalles.Cells[mergedPorcengajeInstalacionValor].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, Color.Black);

                var renglonInicioSimbologia = renglonPorcentajeInstalacion + 2;

                excelDetalles.Cells[renglonInicioSimbologia, 1].Value = "M=";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 1].Value = "T=";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 1].Value = "CON=";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 1].Value = "MF=";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 1].Value = "MAN=";
                excelDetalles.Cells[renglonInicioSimbologia, 2].Value = "MOTOR";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 2].Value = "TRANSMISION";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 2].Value = "CONVERTIDOR";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 2].Value = "MANDOS FINALES";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 2].Value = "MANIPULADOR";

                excelDetalles.Cells[renglonInicioSimbologia, 1, renglonInicioSimbologia + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                excelDetalles.Cells[renglonInicioSimbologia, 1, renglonInicioSimbologia + 4, 1].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 1, renglonInicioSimbologia + 4, 1].Style.Font.Bold = true;
                excelDetalles.Cells[renglonInicioSimbologia, 2, renglonInicioSimbologia + 4, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                excelDetalles.Cells[renglonInicioSimbologia, 2, renglonInicioSimbologia + 4, 2].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 2, renglonInicioSimbologia + 4, 2].Style.Font.Bold = true;

                excelDetalles.Cells[renglonInicioSimbologia, 3].Value = "D=";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 3].Value = "SE=";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 3].Value = "SH=";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 3].Value = "SN=";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 3].Value = "C=";
                excelDetalles.Cells[renglonInicioSimbologia, 4].Value = "DIFERENCIAL";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 4].Value = "SISTEMA ELECTRICO";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 4].Value = "SISTEMA HIDRAULICO";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 4].Value = "SISTEMA NEUMATICO";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 4].Value = "COMBUSTIBLE";

                excelDetalles.Cells[renglonInicioSimbologia, 3, renglonInicioSimbologia + 4, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                excelDetalles.Cells[renglonInicioSimbologia, 3, renglonInicioSimbologia + 4, 3].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 3, renglonInicioSimbologia + 4, 3].Style.Font.Bold = true;
                excelDetalles.Cells[renglonInicioSimbologia, 4, renglonInicioSimbologia + 4, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                excelDetalles.Cells[renglonInicioSimbologia, 4, renglonInicioSimbologia + 4, 4].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 4, renglonInicioSimbologia + 4, 4].Style.Font.Bold = true;

                excelDetalles.Cells[renglonInicioSimbologia, 5].Value = "TR=";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 5].Value = "LL=";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 5].Value = "CH=";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 5].Value = "S & D=";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 5].Value = "EQ=";
                excelDetalles.Cells[renglonInicioSimbologia, 6].Value = "TREN DE RODAJE";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 6].Value = "LLANTAS";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 6].Value = "CHASIS Y CARROCERIA";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 6].Value = "SUSPENSION Y DIRECCION";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 6].Value = "EQUIPO";

                excelDetalles.Cells[renglonInicioSimbologia, 5, renglonInicioSimbologia + 4, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                excelDetalles.Cells[renglonInicioSimbologia, 5, renglonInicioSimbologia + 4, 5].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 5, renglonInicioSimbologia + 4, 5].Style.Font.Bold = true;
                excelDetalles.Cells[renglonInicioSimbologia, 6, renglonInicioSimbologia + 4, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                excelDetalles.Cells[renglonInicioSimbologia, 6, renglonInicioSimbologia + 4, 6].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 6, renglonInicioSimbologia + 4, 6].Style.Font.Bold = true;

                excelDetalles.Cells[renglonInicioSimbologia, 7].Value = "COM=";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 7].Value = "CAB=";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 7].Value = "(A)=";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 7].Value = "(B)=";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 7].Value = "(C)=";
                excelDetalles.Cells[renglonInicioSimbologia, 8].Value = "COMPRESOR";
                excelDetalles.Cells[renglonInicioSimbologia + 1, 8].Value = "CABINA";
                excelDetalles.Cells[renglonInicioSimbologia + 2, 8].Value = "ACONDICIONAMIENTO";
                excelDetalles.Cells[renglonInicioSimbologia + 3, 8].Value = "REPARACION PARCIAL O MEDIA";
                excelDetalles.Cells[renglonInicioSimbologia + 4, 8].Value = "REPARACION GENERAL O TOTAL";

                excelDetalles.Cells[renglonInicioSimbologia, 7, renglonInicioSimbologia + 4, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                excelDetalles.Cells[renglonInicioSimbologia, 7, renglonInicioSimbologia + 4, 7].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 7, renglonInicioSimbologia + 4, 7].Style.Font.Bold = true;
                excelDetalles.Cells[renglonInicioSimbologia, 8, renglonInicioSimbologia + 4, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                excelDetalles.Cells[renglonInicioSimbologia, 8, renglonInicioSimbologia + 4, 8].Style.Font.Size = 10;
                excelDetalles.Cells[renglonInicioSimbologia, 8, renglonInicioSimbologia + 4, 8].Style.Font.Bold = true;

                excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();

                var bytes = new MemoryStream();
                using (var stream = new MemoryStream())
                {
                    excel.SaveAs(stream);
                    bytes = stream;
                }

                return bytes;
            }
        }

        private Dictionary<string, int> AvanceBackLog(List<tblBL_CatBackLogs> backLogs)
        {
            var avances = new Dictionary<string, int>();

            var avance = 0;
            var terminados = 0;

            foreach (var bl in backLogs)
            {
                if (bl.tipoBL == (int)TipoBackLogEnum.Obra)
                {
                    switch (bl.idEstatus)
                    {
                        case (int)EstatusBackLogEnum.ElaboracionInspeccion:
                            avance += 20;
                            break;
                        case (int)EstatusBackLogEnum.ElaboracionRequisicion:
                            avance += 40;
                            break;
                        case (int)EstatusBackLogEnum.ElaboracionOC:
                            avance += 50;
                            break;
                        case (int)EstatusBackLogEnum.SuministroRefacciones:
                            avance += 60;
                            break;
                        case (int)EstatusBackLogEnum.RehabilitacionProgramada:
                            avance += 80;
                            break;
                        case (int)EstatusBackLogEnum.ProcesoInstalacion:
                            avance += 90;
                            break;
                        case (int)EstatusBackLogEnum.BackLogsInstalado:
                            avance += 100;
                            terminados += 1;
                            break;
                    }
                }
                if (bl.tipoBL == (int)TipoBackLogEnum.TMC)
                {
                    switch (bl.idEstatus)
                    {
                        case (int)EstatusBackLogsTMCEnum.ElaboracionPresupuesto:
                            avance += 20;
                            break;
                        case (int)EstatusBackLogsTMCEnum.AutorizacionPresupuesto:
                            avance += 40;
                            break;
                        case (int)EstatusBackLogsTMCEnum.ElaboracionOC:
                            avance += 50;
                            break;
                        case (int)EstatusBackLogsTMCEnum.SuministroRefacciones:
                            avance += 60;
                            break;
                        case (int)EstatusBackLogsTMCEnum.RehabilitacionProgramada:
                            avance += 80;
                            break;
                        case (int)EstatusBackLogsTMCEnum.ProcesoInstalacion:
                            avance += 90;
                            break;
                        case (int)EstatusBackLogsTMCEnum.BackLogsInstalado:
                            avance += 100;
                            terminados += 1;
                            break;
                    }
                }
            }

            avance /= backLogs.Count > 0 ? backLogs.Count : 1;

            avances.Add("avances", avance);
            avances.Add("terminados", terminados);

            return avances;
        }

        private Dictionary<string, decimal> CostoRefaccion(List<InfoOrdenCompraBLDTO> ordenCompras)
        {
            var costos = new Dictionary<string, decimal>();

            var ordenDolares = 0.0M;
            var ordenPesos = 0.0M;

            foreach (var info in ordenCompras)
            {
                switch (Convert.ToInt32(info.orden.moneda))
                {
                    case (int)TipoMonedaEnum.MXN:
                        ordenPesos += info.ordenDet.importe + ((info.orden.porcent_iva / 100) * info.ordenDet.importe);
                        break;
                    case (int)TipoMonedaEnum.USD:
                        ordenDolares += info.ordenDet.importe + ((info.orden.porcent_iva / 100) * info.ordenDet.importe);
                        ordenPesos += (info.ordenDet.importe + ((info.orden.porcent_iva / 100) * info.ordenDet.importe)) * info.orden.tipo_cambio;
                        break;
                }
            }

            costos.Add("mxn", ordenPesos);
            costos.Add("usd", ordenDolares);

            return costos;
        }

        public string htmlCorreo(List<CorreoBackLogDTO> lstAutorizadores)
        {
            string html = "";

            html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            html += "border: 0px solid #81bd72 !important;}table.dataTable thead {font-size: 15px;background-color: #81bd72;color: white;}";
            html += ".select2-container {width: 100% !important;}.seccion {padding: 15px 25px 15px 25px;margin: 10px 5px;background-color: white;";
            html += "border-radius: 4px 4px;box-shadow: 0 0 2px 0 rgba(0,0,0,0.14), 0 2px 2px 0 rgba(0,0,0,0.12), 0 1px 3px 0 rgba(0,0,0,0.2);}";
            html += ".my-card {position: absolute;left: 40%;top: -20px;border-radius: 50%;}#txtFechaInicio {background-color: #fff;}";
            html += "</style><br><table id='tblM_autorizacion' class='table-bordered hover stripe order-column dataTable no-footer' role='grid'";
            html += "aria-describedby='tblM_autorizacion_info' style='width: 0px;'>";
            html += "<thead>";
            html += "<tr role='row'>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Nombre Completo</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Puesto</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>Firma</th>";
            html += "<th class='sorting_disabled' rowspan='1' colspan='1' style='width: 0px;'>fecha</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";

            foreach (var item in lstAutorizadores)
            {
                html += "<tr>";

                html += "<td>" + item.nombreCompleto + "</td>";
                html += "<td>" + item.puesto + "</td>";
                html += "<td>" + item.firma + "</td>";
                html += "<td>" + item.dtFecha + "</td>";

                html += "</tr>";
            }

            html += "</tbody>";
            html += "</table>";
            html += "</div>";

            return html;
        }

        public bool ModificarEstatus(int id, int idLogeado, int Autorizante, int Estatus, string Descripcion)
        {
            bool retornar = false;
            try
            {
                if (idLogeado == Autorizante)
                {
                    var objTmc = _context.tblBL_SeguimientoPptos.Where(r => r.id == id).FirstOrDefault();
                    switch (Autorizante)
                    {
                        case (int)AutorizadoresEnum.vobo1:
                            if (Estatus == (int)EstatusSeguimientosPptoEnum.AUTORIZADO)
                            {
                                objTmc.esVobo1 = Estatus;
                                objTmc.firmaVobo1 = GlobalUtils.CrearFirmaDigital(objTmc.id, DocumentosEnum.BackLogs, objTmc.idUserVobo1, TipoFirmaEnum.Autorizacion);
                                objTmc.fechaVobo1 = DateTime.Now;
                                objTmc.comentRechaVobo1 = Descripcion;
                                _context.SaveChanges();

                                var obtenerAlerta = _context.tblP_Alerta.Where(r => r.userRecibeID == Autorizante && r.moduloID == 9764 && r.tipoAlerta == 2 && r.sistemaID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                                if (obtenerAlerta != null)
                                {
                                    _context.tblP_Alerta.Remove(obtenerAlerta);
                                    _context.SaveChanges();
                                }


                                tblP_Alerta objAlerta = new tblP_Alerta();
                                objAlerta.userEnviaID = Autorizante;
                                objAlerta.userRecibeID = objTmc.idUserVobo2;
                                objAlerta.tipoAlerta = 2;
                                objAlerta.sistemaID = (int)SistemasEnum.MAQUINARIA;
                                objAlerta.visto = false;
                                objAlerta.url = "/BackLogs/SeguimientoDePresupuestoTMC";
                                objAlerta.objID = objTmc.id;
                                objAlerta.obj = "";
                                objAlerta.msj = "Autorizacion pendiente " + objTmc.noEconomico;
                                objAlerta.moduloID = 9764;
                                _context.tblP_Alerta.Add(objAlerta);
                                _context.SaveChanges();

                                List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                                CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo1,
                                    dtFecha = objTmc.fechaVobo1,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo2,
                                    dtFecha = objTmc.fechaVobo2,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaAutorizado,
                                    dtFecha = objTmc.fechaAutorizado,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);


                                foreach (var item in lstUsuarios)
                                {
                                    var subject = "Autorizacion de Back Logs";
                                    var body = @"Buen dia " + item.nombreCompleto
                                        + " Tiene pendiente una autorizacion de Back Logs <br>"
                                        + htmlCorreo(lstUsuarios);
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                                }
                            }
                            else
                            {
                                objTmc.esVobo1 = Estatus;
                                objTmc.comentRechaVobo1 = Descripcion;
                                objTmc.EstatusSegPpto = (int)EstatusSeguimientosPptoEnum.RECHAZADO;
                                objTmc.firmaVobo1 = GlobalUtils.CrearFirmaDigital(objTmc.id, DocumentosEnum.BackLogs, objTmc.idUserVobo1, TipoFirmaEnum.Rechazo);
                                objTmc.fechaVobo1 = DateTime.Now;
                                _context.SaveChanges();

                                var obtenerAlerta = _context.tblP_Alerta.Where(r => r.userRecibeID == Autorizante && r.moduloID == 9764 && r.tipoAlerta == 2 && r.sistemaID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                                if (obtenerAlerta != null)
                                {
                                    _context.tblP_Alerta.Remove(obtenerAlerta);
                                    _context.SaveChanges();
                                }


                                List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                                CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo1,
                                    dtFecha = objTmc.fechaVobo1,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo2,
                                    dtFecha = objTmc.fechaVobo2,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaAutorizado,
                                    dtFecha = objTmc.fechaAutorizado,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);


                                foreach (var item in lstUsuarios)
                                {
                                    var subject = "Autorizacion de Back Logs";
                                    var body = @"Buen dia " + item.nombreCompleto
                                        + " Han rechazado este backlogs <br>"
                                        + htmlCorreo(lstUsuarios);
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                                }
                            }
                            break;
                        case (int)AutorizadoresEnum.vobo2:
                            if (Estatus == (int)EstatusSeguimientosPptoEnum.AUTORIZADO)
                            {
                                objTmc.esVobo2 = Estatus;
                                objTmc.comentRechaVobo2 = Descripcion;
                                objTmc.firmaVobo2 = GlobalUtils.CrearFirmaDigital(objTmc.id, DocumentosEnum.BackLogs, objTmc.idUserVobo2, TipoFirmaEnum.Autorizacion);
                                objTmc.fechaVobo2 = DateTime.Now;
                                _context.SaveChanges();

                                var obtenerAlerta = _context.tblP_Alerta.Where(r => r.userRecibeID == Autorizante && r.moduloID == 9764 && r.tipoAlerta == 2 && r.sistemaID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                                if (obtenerAlerta != null)
                                {
                                    _context.tblP_Alerta.Remove(obtenerAlerta);
                                    _context.SaveChanges();
                                }


                                tblP_Alerta objAlerta = new tblP_Alerta();
                                objAlerta.userEnviaID = Autorizante;
                                objAlerta.userRecibeID = objTmc.idUserAutorizado;
                                objAlerta.tipoAlerta = 2;
                                objAlerta.sistemaID = (int)SistemasEnum.MAQUINARIA;
                                objAlerta.visto = false;
                                objAlerta.url = "/BackLogs/SeguimientoDePresupuestoTMC";
                                objAlerta.objID = objTmc.id;
                                objAlerta.obj = "";
                                objAlerta.msj = "Autorizacion pendiente " + objTmc.noEconomico;
                                objAlerta.moduloID = 9764;
                                _context.tblP_Alerta.Add(objAlerta);
                                _context.SaveChanges();

                                List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                                CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo1,
                                    dtFecha = objTmc.fechaVobo1,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo2,
                                    dtFecha = objTmc.fechaVobo2,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaAutorizado,
                                    dtFecha = objTmc.fechaAutorizado,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);


                                foreach (var item in lstUsuarios)
                                {
                                    var subject = "Autorizacion de Back Logs";
                                    var body = @"Buen dia " + item.nombreCompleto
                                        + " Tiene pendiente una autorizacion de Back Logs <br>"
                                        + htmlCorreo(lstUsuarios);
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                                }
                            }
                            else
                            {
                                objTmc.esVobo2 = Estatus;
                                objTmc.comentRechaVobo2 = Descripcion;
                                objTmc.EstatusSegPpto = (int)EstatusSeguimientosPptoEnum.RECHAZADO;
                                objTmc.firmaVobo2 = GlobalUtils.CrearFirmaDigital(objTmc.id, DocumentosEnum.BackLogs, objTmc.idUserVobo2, TipoFirmaEnum.Rechazo);
                                objTmc.fechaVobo2 = DateTime.Now;
                                _context.SaveChanges();

                                var obtenerAlerta = _context.tblP_Alerta.Where(r => r.userRecibeID == Autorizante && r.moduloID == 9764 && r.tipoAlerta == 2 && r.sistemaID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                                if (obtenerAlerta != null)
                                {
                                    _context.tblP_Alerta.Remove(obtenerAlerta);
                                    _context.SaveChanges();
                                }


                                List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                                CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo1,
                                    dtFecha = objTmc.fechaVobo1,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo2,
                                    dtFecha = objTmc.fechaVobo2,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaAutorizado,
                                    dtFecha = objTmc.fechaAutorizado,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);


                                foreach (var item in lstUsuarios)
                                {
                                    var subject = "Autorizacion de Back Logs";
                                    var body = @"Buen dia " + item.nombreCompleto
                                        + " Tiene pendiente una autorizacion de Back Logs <br>"
                                        + htmlCorreo(lstUsuarios);
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                                }
                            }
                            break;
                        case (int)AutorizadoresEnum.Autorizado:
                            if (Estatus == (int)EstatusSeguimientosPptoEnum.AUTORIZADO)
                            {
                                objTmc.esAutorizado = Estatus;
                                objTmc.comentRechaAutorizado = Descripcion;
                                objTmc.EstatusSegPpto = (int)EstatusSeguimientosPptoEnum.AUTORIZADO;
                                objTmc.firmaAutorizado = GlobalUtils.CrearFirmaDigital(objTmc.id, DocumentosEnum.BackLogs, objTmc.idUserVobo1, TipoFirmaEnum.Autorizacion);
                                objTmc.fechaAutorizado = DateTime.Now;
                                _context.SaveChanges();

                                var obtenerAlerta = _context.tblP_Alerta.Where(r => r.userRecibeID == Autorizante && r.moduloID == 9764 && r.tipoAlerta == 2 && r.sistemaID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                                if (obtenerAlerta != null)
                                {
                                    _context.tblP_Alerta.Remove(obtenerAlerta);
                                    _context.SaveChanges();
                                }


                                List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                                CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo1,
                                    dtFecha = objTmc.fechaVobo1,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo2,
                                    dtFecha = objTmc.fechaVobo2,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaAutorizado,
                                    dtFecha = objTmc.fechaAutorizado,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);


                                foreach (var item in lstUsuarios)
                                {
                                    var subject = "Autorizacion de Back Logs";
                                    var body = @"Buen dia " + item.nombreCompleto
                                        + " Tiene pendiente una autorizacion de Back Logs <br>"
                                        + htmlCorreo(lstUsuarios);
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                                }
                            }
                            else
                            {
                                objTmc.esAutorizado = Estatus;
                                objTmc.comentRechaAutorizado = Descripcion;
                                objTmc.EstatusSegPpto = (int)EstatusSeguimientosPptoEnum.RECHAZADO;
                                objTmc.firmaAutorizado = GlobalUtils.CrearFirmaDigital(objTmc.id, DocumentosEnum.BackLogs, objTmc.idUserVobo1, TipoFirmaEnum.Rechazo);
                                objTmc.fechaAutorizado = DateTime.Now;
                                _context.SaveChanges();

                                var obtenerAlerta = _context.tblP_Alerta.Where(r => r.userRecibeID == Autorizante && r.moduloID == 9764 && r.tipoAlerta == 2 && r.sistemaID == (int)SistemasEnum.MAQUINARIA).FirstOrDefault();
                                if (obtenerAlerta != null)
                                {
                                    _context.tblP_Alerta.Remove(obtenerAlerta);
                                    _context.SaveChanges();
                                }


                                List<CorreoBackLogDTO> lstUsuarios = new List<CorreoBackLogDTO>();
                                CorreoBackLogDTO Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo1).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo1,
                                    dtFecha = objTmc.fechaVobo1,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.vobo2).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaVobo2,
                                    dtFecha = objTmc.fechaVobo2,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);

                                Obj = new CorreoBackLogDTO();
                                Obj = _context.tblP_Usuario.Where(r => r.id == (int)AutorizadoresEnum.Autorizado).Select(y => new CorreoBackLogDTO
                                {
#if DEBUG
                                    Correos = vSesiones.sesionUsuarioDTO.correo,
#else
                                    Correos = y.correo,
#endif
                                    nombreCompleto = y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno,
                                    puesto = _context.tblP_Puesto.Where(n => n.id == y.puestoID).Select(s => s.descripcion).FirstOrDefault(),
                                    firma = objTmc.firmaAutorizado,
                                    dtFecha = objTmc.fechaAutorizado,
                                }).FirstOrDefault();
                                lstUsuarios.Add(Obj);


                                foreach (var item in lstUsuarios)
                                {
                                    var subject = "Autorizacion de Back Logs";
                                    var body = @"Buen dia " + item.nombreCompleto
                                        + " Tiene pendiente una autorizacion de Back Logs <br>"
                                        + htmlCorreo(lstUsuarios);
                                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstUsuarios.Select(n => n.Correos).ToList());
                                }
                            }
                            break;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return retornar;
                throw;
            }
            return retornar;
        }

        #region INDICADORES DE REHABILITACION TMC
        public Dictionary<string, object> GetIndicadoresRehabilitacionTMC(IndicadoresRehabilitacionTMCDTO objFiltro)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                List<tblBL_InspeccionesTMC> lstInspeccionesTMC = new List<tblBL_InspeccionesTMC>();

                switch (objFiltro.idMotivo)
                {
                    case 0:
                        lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(w => objFiltro.lstMeses.Contains(w.fechaPromesa.Month) && w.esActivo && w.idMotivo == 0).ToList();
                        break;
                    case 1:
                        lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(w => objFiltro.lstMeses.Contains(w.fechaPromesa.Month) && w.esActivo && w.idMotivo == 1).ToList();

                        break;
                    default:
                        lstInspeccionesTMC = _context.tblBL_InspeccionesTMC.Where(w => objFiltro.lstMeses.Contains(w.fechaPromesa.Month) && w.esActivo).ToList();
                        break;
                }

                if (lstInspeccionesTMC.Count() > 0)
                {
                    #region FILTRO POR TIPO DE MAQUINA
                    #region SE OBTIENE LAS MAQUINAS EN BASE A LOS FILTROS.
                    int tipoMaquina = objFiltro != null ? objFiltro.tipoMaquina : 0;

                    List<tblM_CatGrupoMaquinaria> lstGrupoMaquinas = _context.tblM_CatGrupoMaquinaria.Where(x => (tipoMaquina > 0 ? x.tipoEquipoID == tipoMaquina : true) && x.estatus).ToList();
                    List<int> lstGrupoID = new List<int>();
                    if (lstGrupoMaquinas.Count() > 0)
                    {
                        foreach (var item in lstGrupoMaquinas)
                        {
                            lstGrupoID.Add(item.id);
                        }
                    }

                    objFiltro.areaCuenta = "1010";
                    List<tblM_CatMaquina> _lstCatMaquinas = new List<tblM_CatMaquina>();
                    if (lstGrupoID.Count() > 0)
                        _lstCatMaquinas = _context.tblM_CatMaquina.Where(x => x.centro_costos == objFiltro.areaCuenta && lstGrupoID.Contains(x.grupoMaquinariaID) && x.estatus == 1).ToList();

                    List<string> lstCatMaquinasNoEconomicos = new List<string>();
                    foreach (var item in _lstCatMaquinas)
                    {
                        lstCatMaquinasNoEconomicos.Add(item.noEconomico);
                    }
                    #endregion
                    #endregion

                    List<tblM_CatMaquina> lstCatMaquinas = _context.tblM_CatMaquina.Where(x => lstCatMaquinasNoEconomicos.Contains(x.noEconomico) && x.estatus == 1).ToList();
                    List<tblBL_CatBackLogs> lstCatBL = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.TMC && x.esActivo).ToList();
                    List<tblBL_Evidencias> lstEvidencias = _context.tblBL_Evidencias.Where(x => x.esActivo).ToList();
                    List<tblBL_DetFrentes> lstDetFrentes = _context.tblBL_DetFrentes.Where(x => x.esActivo).ToList();
                    List<tblBL_CatFrentes> lstCatFrentes = _context.tblbl_CatFrentes.Where(x => x.esActivo).ToList();
                    List<tblBL_SeguimientoPptos> lstSegPptos = _context.tblBL_SeguimientoPptos.Where(x => x.esActivo).ToList();
                    List<tblBL_OrdenesCompra> lstOC = _context.tblBL_OrdenesCompra.Where(x => x.esActivo).ToList();

                    try
                    {
                        #region FILL TABLA #1
                        List<IndicadoresRehabilitacionTMCDTO> lstDataTbl1 = lstSegPptos.Select(x => new IndicadoresRehabilitacionTMCDTO
                        {
                            id = x.id,
                            idInspTMC = x.idInspTMC,
                            noEconomico = x.noEconomico,
                            descripcion = "",
                            modelo = "",
                            horometro = x.horas,
                            ppto = x.Ppto,
                            idMotivo = 0,
                            motivo = "",
                            estatus = 0,
                            fechaPromesa = new DateTime(2000, 01, 01),
                            fechaTermino = new DateTime(2000, 01, 01),
                            diasDesface = 0,
                            cantBLEjecutados = 0,
                            porcCump = 0,
                            mes = 0
                        }).ToList();

                        foreach (var item in lstDataTbl1)
                        {
                            item.descripcion = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.descripcion).FirstOrDefault();
                            item.modelo = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.modeloEquipo.descripcion).FirstOrDefault();
                            item.idMotivo = lstInspeccionesTMC.Where(w => w.id == item.idInspTMC).Select(s => s.idMotivo).FirstOrDefault();
                            item.fechaPromesa = lstInspeccionesTMC.Where(w => w.id == item.idInspTMC).Select(s => s.fechaPromesa).FirstOrDefault();
                            item.fechaTermino = lstCatBL.Where(w => w.idSegPpto == item.id).OrderByDescending(o => o.fechaLiberadoBL).Select(s => s.fechaLiberadoBL).FirstOrDefault(); //TRAER LA ULTIMA FECHA DE CUANDO SE LIBERO LA MAQUINA.
                            item.mes = item.fechaTermino.Month;

                            #region SE OBTIENE EL MOTIVO DEL PPTO.
                            item.motivo = item.idMotivo == (int)MotivoInspeccionEnum.Obra ? "OBRA" : "VENTA";
                            #endregion

                            #region SE OBTIENE EL ESTATUS DEL PPTO.
                            //SE OBTIENE EL TOTAL DE BL QUE CONTIENE EL PPTO.
                            decimal cantTotalBL = 0;
                            List<tblBL_CatBackLogs> lstBLRelSegPpto = lstCatBL.Where(x => x.idSegPpto == item.id).ToList();
                            cantTotalBL = lstBLRelSegPpto.Count();
                            List<int> lstBLID = new List<int>();
                            foreach (var itemBLInstalados in lstBLRelSegPpto)
                            {
                                lstBLID.Add(itemBLInstalados.id);
                            }

                            //SE OBTIENE EL TOTAL DE BL QUE TIENE SU EVIDENCIA DE SEGURIDAD.
                            decimal cantBLInstalados = 0;
                            List<tblBL_Evidencias> objEvidencia = lstEvidencias.Where(x => lstBLID.Contains(x.idBL) && x.tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaSeguridad && x.esActivo).ToList();
                            //List<tblBL_Evidencias> objEvidencia = lstEvidencias.Where(x => lstBLID.Contains(x.idBL) && x.esActivo).ToList();
                            cantBLInstalados = objEvidencia.Count();

                            //SE OBTIENE EL AVANCE
                            decimal estatusPpto = 0;
                            if (cantTotalBL > 0 && cantBLInstalados > 0)
                                estatusPpto = ((decimal)cantBLInstalados / (decimal)cantTotalBL) * 100;

                            item.estatus = estatusPpto;
                            #endregion

                            #region SE OBTIENE LOS DÍAS DE DESFACE
                            if (item.fechaTermino.Year > 2020 && item.fechaTermino != null && item.fechaPromesa.Year > 2020 && item.fechaPromesa != null)
                            {
                                TimeSpan difFechas = item.fechaTermino.Date - item.fechaPromesa.Date;
                                item.diasDesface = difFechas.Days;
                            }
                            else
                                item.diasDesface = 0;
                            #endregion

                            #region SE OBTIENE LA CANTIDAD DE BACKLOGS EJECUTADOS EN BASE AL MES SELECCIONADO
                            item.cantBLEjecutados = lstCatBL.Where(x => x.idSegPpto == item.id && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.esLiberado).Count();
                            #endregion

                            #region SE OBTIENE EL PORCENTAJE DEL CUMPLIMIENTO
                            item.porcCump = 100;
                            if (item.diasDesface > 0 && item.fechaTermino.Year > 2000)
                            {
                                item.porcCump = 100;
                                decimal porcCump = 0;
                                string porcRestar = item.diasDesface + "0";
                                porcCump = item.porcCump - Convert.ToDecimal(porcRestar);
                                if (porcCump < 0)
                                    porcCump = 0;

                                item.porcCump = porcCump;
                            }
                            #endregion
                        }

                        #region SE OBTIENE EL PROMEDIO, TOMANDO EN CUENTA TODOS LOS % DE CUMPLIMIENTO.
                        string categorieGrafica1 = string.Empty;
                        switch (objFiltro.idMes)
                        {
                            case 1:
                                categorieGrafica1 = "ENERO";
                                break;
                            case 2:
                                categorieGrafica1 = "FEBRERO";
                                break;
                            case 3:
                                categorieGrafica1 = "MARZO";
                                break;
                            case 4:
                                categorieGrafica1 = "ABRIL";
                                break;
                            case 5:
                                categorieGrafica1 = "MAYO";
                                break;
                            case 6:
                                categorieGrafica1 = "JUNIO";
                                break;
                            case 7:
                                categorieGrafica1 = "JULIO";
                                break;
                            case 8:
                                categorieGrafica1 = "AGOSTO";
                                break;
                            case 9:
                                categorieGrafica1 = "SEPTIEMBRE";
                                break;
                            case 10:
                                categorieGrafica1 = "OCTUBRE";
                                break;
                            case 11:
                                categorieGrafica1 = "NOVIEMBRE";
                                break;
                            case 12:
                                categorieGrafica1 = "DICIEMBRE";
                                break;
                            default:
                                break;
                        }
                        resultado.Add("categorieGrafica1", categorieGrafica1);

                        int cantPorcCumpl = lstDataTbl1.Count();
                        decimal promedioCumpl = 0;
                        decimal sumaPorcCumpl = lstDataTbl1.Sum(x => x.porcCump);
                        if (sumaPorcCumpl > 0 && cantPorcCumpl > 0)
                            promedioCumpl = (sumaPorcCumpl / cantPorcCumpl);
                        resultado.Add("dataGrafica1", promedioCumpl);
                        #endregion

                        resultado.Add("lstDataTbl1", lstDataTbl1);
                        #endregion
                    }
                    catch (Exception e)
                    {
                    }

                    try
                    {
                        #region FILL TABLA #2
                        List<IndicadoresRehabilitacionTMCDTO> lstDataTbl2 = lstSegPptos.Select(x => new IndicadoresRehabilitacionTMCDTO
                        {
                            id = x.id,
                            noEconomico = x.noEconomico,
                            descripcion = "",
                            modelo = "",
                            estatus = 0,
                            ppto = x.Ppto,
                            pptoReal = 0,
                            cumpDePpto = 0,
                            porcCump = 0
                        }).ToList();

                        foreach (var item in lstDataTbl2)
                        {
                            item.descripcion = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.descripcion).FirstOrDefault();
                            item.modelo = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.modeloEquipo.descripcion).FirstOrDefault();

                            #region SE OBTIENE EL ESTATUS DEL PPTO.
                            //SE OBTIENE EL TOTAL DE BL QUE CONTIENE EL PPTO.
                            decimal cantTotalBL = 0;
                            List<tblBL_CatBackLogs> lstBLRelSegPpto = lstCatBL.Where(x => x.idSegPpto == item.id).ToList();
                            cantTotalBL = lstBLRelSegPpto.Count();
                            List<int> lstBLID = new List<int>();
                            foreach (var itemBLInstalados in lstBLRelSegPpto)
                            {
                                lstBLID.Add(itemBLInstalados.id);
                            }

                            //SE OBTIENE EL TOTAL DE BL QUE TIENE SU EVIDENCIA DE SEGURIDAD.
                            decimal cantBLInstalados = 0;
                            List<tblBL_Evidencias> objEvidencia = lstEvidencias.Where(x => lstBLID.Contains(x.idBL) && x.tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaSeguridad && x.esActivo).ToList();
                            //List<tblBL_Evidencias> objEvidencia = lstEvidencias.Where(x => lstBLID.Contains(x.idBL) && x.esActivo).ToList();
                            cantBLInstalados = objEvidencia.Count();

                            //SE OBTIENE EL AVANCE
                            decimal estatusPpto = 0;
                            if (cantTotalBL > 0 && cantBLInstalados > 0)
                                estatusPpto = ((decimal)cantBLInstalados / (decimal)cantTotalBL) * 100;

                            item.estatus = estatusPpto;
                            #endregion

                            #region SE OBTIENE EL PPTO REAL DEL SEGUIMIENTO DEL PPTO.
                            //SE OBTIENE EL CC DEL ECONOMICO
                            string cc = string.Empty;
                            cc = lstCatBL.Where(x => x.noEconomico == item.noEconomico && x.idSegPpto == item.id).Select(s => s.cc).FirstOrDefault();

                            //SE OBTIENE LAS ORDENES DE COMPRA RELACIONADAS A LOS BACKLOGS QUE CONTIENE EL PPTO.
                            List<tblBL_OrdenesCompra> objOC = lstOC.Where(x => lstBLID.Contains(x.idBackLog)).ToList();
                            List<string> lstNumOC = new List<string>();
                            foreach (var itemLstNumOC in objOC)
                            {
                                lstNumOC.Add(itemLstNumOC.numOC);
                            }

                            if (lstNumOC.Count() > 0)
                            {
                                #region T
                                List<tblBL_OrdenesCompra> objOrdenesCompra = new List<tblBL_OrdenesCompra>();
                                objOrdenesCompra = _context.tblBL_OrdenesCompra.Where(x => lstBLID.Contains(x.idBackLog) && x.esActivo).ToList();

                                #region SE OBTIENE EL DETALLE DE LA ORDEN DE COMPRA
                                List<BackLogsDTO> lstDetalleOC = new List<BackLogsDTO>();
                                string strQuery = "SELECT cc, numero, insumo, cantidad, precio, importe, num_requisicion FROM so_orden_compra_det WHERE cc LIKE '%{0}%' AND numero IN ({1})";
                                var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                                odbc.consulta = String.Format(strQuery, cc, string.Join(",", objOrdenesCompra.Select(s => s.numOC).ToList()));
                                if (productivo)
                                    lstDetalleOC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                                else
                                    lstDetalleOC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);
                                #endregion

                                #region SE OBTIENE LOS INSUMOS QUE CONTIENE EL BL
                                List<tblBL_Partes> lstPartes = _context.tblBL_Partes.Where(w => lstBLID.Contains(w.idBacklog) && w.esActivo).ToList();
                                List<BackLogsDTO> lstPartesRelBL = lstDetalleOC.Where(w => lstPartes.Select(s => s.insumo).Contains(w.insumo)).ToList();
                                #endregion

                                //foreach (var item in objCC)
                                foreach (var item2 in lstPartesRelBL)
                                {
                                    BackLogsDTO objBL = new BackLogsDTO();
                                    List<BackLogsDTO> objCC = new List<BackLogsDTO>();
                                    strQuery = "SELECT moneda, tipo_cambio, total FROM DBA.so_orden_compra WHERE cc LIKE '%{0}%' AND numero IN ({1})";
                                    odbc = new OdbcConsultaDTO() { consulta = strQuery };
                                    odbc.consulta = String.Format(strQuery, cc, item2.numero);
                                    if (productivo)
                                        objCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolEnum.ArrenProd, odbc);
                                    else
                                        objCC = _contextEnkontrol.Select<BackLogsDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                                    int tipoMoneda = objCC.Select(s => s.moneda).FirstOrDefault();
                                    decimal tipoCambio = objCC.Select(s => s.tipo_cambio).FirstOrDefault();
                                    if (tipoMoneda == (int)TipoMonedaEnum.MXN)
                                        item.pptoReal += item2.importe;
                                    else if (tipoMoneda == (int)TipoMonedaEnum.USD)
                                        item.pptoReal += (item2.importe * tipoCambio);

                                    string ttttt = string.Empty;
                                }
                                #endregion

                                //List<IndicadoresRehabilitacionTMCDTO> lstPptoReal = new List<IndicadoresRehabilitacionTMCDTO>();
                                //string strQueryOC = @"SELECT total FROM so_orden_compra WHERE cc = '{0}' AND numero IN ({1}) AND estatus = '{2}'";
                                //var odbc = new OdbcConsultaDTO() { consulta = strQueryOC };
                                //odbc.consulta = String.Format(strQueryOC, cc, string.Join(",", lstNumOC), "T");

                                //if (productivo)
                                //    lstPptoReal = _contextEnkontrol.Select<IndicadoresRehabilitacionTMCDTO>(EnkontrolEnum.ArrenProd, odbc);
                                //else
                                //    lstPptoReal = _contextEnkontrol.Select<IndicadoresRehabilitacionTMCDTO>(EnkontrolAmbienteEnum.Prueba, odbc);

                                //decimal pptoReal = (decimal)lstPptoReal[0].total;
                                //item.pptoReal = pptoReal;
                            }
                            #endregion

                            #region SE OBTIENE EL CUMPLIMIENTO DE PPTO.
                            if ((decimal)item.pptoReal > 0)
                                item.cumpDePpto = (decimal)item.pptoReal - (decimal)item.ppto;
                            #endregion

                            #region SE OBTIENE EL PORCENTAJE DEL CUMPLIMIENTO.
                            if ((decimal)item.pptoReal > 0 && (decimal)item.ppto > 0)
                                item.porcCump = (decimal)item.pptoReal / (decimal)item.ppto;
                            #endregion

                            #region SE OBTIENE LOS DATOS PARA LA GRAFICA 2
                            item.categoriesGrafica2 = item.noEconomico;
                            item.dataGrafica2 = item.porcCump;
                            #endregion
                        }
                        resultado.Add("lstDataTbl2", lstDataTbl2);

                        #region SE OBTIENE LA COLUMNA GLOBAL
                        decimal pptoTotal = 0, pptoRealTotal = 0, porcGlobal = 0;
                        pptoTotal = lstDataTbl2.Sum(x => x.ppto);
                        pptoRealTotal = lstDataTbl2.Sum(x => x.pptoReal);
                        if (pptoTotal > 0 && pptoRealTotal > 0)
                            porcGlobal = pptoRealTotal / pptoTotal;

                        IndicadoresRehabilitacionTMCDTO dataPorcGlobal = new IndicadoresRehabilitacionTMCDTO();
                        dataPorcGlobal.categoriesGrafica2 = "<b>GLOBAL</b>";
                        dataPorcGlobal.dataGrafica2 = porcGlobal;
                        resultado.Add("dataPorcGlobal", dataPorcGlobal);
                        #endregion

                        #endregion
                    }
                    catch (Exception e)
                    {
                    }

                    try
                    {
                        #region FILL TABLA #3
                        List<IndicadoresRehabilitacionTMCDTO> lstDataTbl3 = lstSegPptos.Where(w => w.idFrente > 0).Select(x => new IndicadoresRehabilitacionTMCDTO
                        {
                            id = x.id,
                            frenteTrabajo = "",
                            idFrente = x.idFrente,
                            noEconomico = x.noEconomico,
                            descripcion = "",
                            modelo = "",
                            fechaAsignacion = new DateTime(2000, 01, 01),
                            cantBLEjecutados = 0
                        }).ToList();

                        foreach (var item in lstDataTbl3)
                        {
                            item.frenteTrabajo = lstCatFrentes.Where(x => x.id == item.idFrente).Select(s => s.nombreFrente).FirstOrDefault();
                            item.descripcion = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.descripcion).FirstOrDefault();
                            item.modelo = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.modeloEquipo.descripcion).FirstOrDefault();
                            item.fechaAsignacion = lstDetFrentes.Where(w => w.idFrente == item.idFrente && w.idSeguimientoPpto == item.id).Select(s => s.fechaAsignacion).FirstOrDefault();

                            #region SE OBTIENE LA CANTIDAD DE BACKLOGS EJECUTADOS EN BASE AL MES SELECCIONADO
                            item.cantBLEjecutados = lstCatBL.Where(x => x.idSegPpto == item.id && x.idEstatus == (int)EstatusBackLogsTMCEnum.BackLogsInstalado && x.esLiberado).Count();
                            #endregion
                        }
                        resultado.Add("lstDataTbl3", lstDataTbl3);

                        List<IndicadoresRehabilitacionTMCDTO> lstGrafica3 = new List<IndicadoresRehabilitacionTMCDTO>();
                        foreach (var item in lstDataTbl3)
                        {
                            IndicadoresRehabilitacionTMCDTO objGuardar = new IndicadoresRehabilitacionTMCDTO();
                            int existeFrenteEnListaGrafica3 = lstGrafica3.Where(x => x.idFrente == item.idFrente).Count();
                            if (existeFrenteEnListaGrafica3 <= 0)
                            {
                                objGuardar.idFrente = item.idFrente;
                                objGuardar.frenteTrabajo = item.frenteTrabajo;
                                objGuardar.cantBLEjecutados = item.cantBLEjecutados;
                                lstGrafica3.Add(objGuardar);
                            }
                            else
                            {
                                IndicadoresRehabilitacionTMCDTO objActualizar = lstGrafica3.Where(x => x.idFrente == item.idFrente).FirstOrDefault();
                                objActualizar.cantBLEjecutados += item.cantBLEjecutados;
                            }
                        }
                        resultado.Add("lstGrafica3", lstGrafica3);
                        #endregion
                    }
                    catch (Exception e)
                    {
                    }

                    try
                    {
                        #region FILL TABLA #4
                        List<IndicadoresRehabilitacionTMCDTO> lstDataTbl4 = lstSegPptos.Where(w => w.idFrente > 0).Select(x => new IndicadoresRehabilitacionTMCDTO
                        {
                            id = x.id,
                            noEconomico = x.noEconomico,
                            descripcion = "",
                            modelo = "",
                            horometro = x.horas,
                            ppto = x.Ppto,
                            idMotivo = 0,
                            motivo = "",
                            estatus = 0,
                            idInspTMC = x.idInspTMC,
                            fechaPromesa = new DateTime(2000, 01, 01),
                            fechaTermino = new DateTime(2000, 01, 01),
                            esLiberado = false
                        }).ToList();

                        foreach (var item in lstDataTbl4)
                        {
                            item.descripcion = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.descripcion).FirstOrDefault();
                            item.modelo = lstCatMaquinas.Where(w => w.noEconomico == item.noEconomico).Select(s => s.modeloEquipo.descripcion).FirstOrDefault();
                            item.idMotivo = lstInspeccionesTMC.Where(w => w.id == item.idInspTMC).Select(s => s.idMotivo).FirstOrDefault();
                            item.fechaPromesa = lstInspeccionesTMC.Where(w => w.id == item.idInspTMC).Select(s => s.fechaPromesa).FirstOrDefault();
                            item.fechaTermino = lstCatBL.Where(w => w.idSegPpto == item.id).OrderByDescending(o => o.fechaLiberadoBL).Select(s => s.fechaLiberadoBL).FirstOrDefault(); //TRAER LA ULTIMA FECHA DE CUANDO SE LIBERO LA MAQUINA.
                            item.esLiberado = lstCatBL.Where(w => w.idSegPpto == item.id && w.esLiberado).Count() > 0 ? true : false;

                            #region SE OBTIENE EL ESTATUS DEL PPTO.
                            //SE OBTIENE EL TOTAL DE BL QUE CONTIENE EL PPTO.
                            decimal cantTotalBL = 0;
                            List<tblBL_CatBackLogs> lstBLRelSegPpto = lstCatBL.Where(x => x.idSegPpto == item.id).ToList();
                            cantTotalBL = lstBLRelSegPpto.Count();
                            List<int> lstBLID = new List<int>();
                            foreach (var itemBLInstalados in lstBLRelSegPpto)
                            {
                                lstBLID.Add(itemBLInstalados.id);
                            }

                            //SE OBTIENE EL TOTAL DE BL QUE TIENE SU EVIDENCIA DE SEGURIDAD.
                            decimal cantBLInstalados = 0;
                            List<tblBL_Evidencias> objEvidencia = lstEvidencias.Where(x => lstBLID.Contains(x.idBL) && x.tipoEvidencia == (int)TipoEvidenciaEnumBL.evidenciaSeguridad && x.esActivo).ToList();
                            //List<tblBL_Evidencias> objEvidencia = lstEvidencias.Where(x => lstBLID.Contains(x.idBL) && x.esActivo).ToList();
                            cantBLInstalados = objEvidencia.Count();

                            //SE OBTIENE EL AVANCE
                            decimal estatusPpto = 0;
                            if (cantTotalBL > 0 && cantBLInstalados > 0)
                                estatusPpto = ((decimal)cantBLInstalados / (decimal)cantTotalBL) * 100;

                            item.estatus = estatusPpto;
                            #endregion

                            #region SE OBTIENE EL MOTIVO DEL PPTO.
                            item.motivo = item.idMotivo == (int)MotivoInspeccionEnum.Obra ? "OBRA" : "VENTA";
                            #endregion
                        }
                        resultado.Add("lstDataTbl4", lstDataTbl4);

                        #region SE OBTIENE LA CANTIDAD DE OBRAS SE HAN REALIZADO EN EL MES SELECCIONADO
                        int cantLiberadoObra = lstDataTbl4.Where(x => x.idMotivo == (int)MotivoInspeccionEnum.Obra && x.esLiberado).Count();
                        int cantLiberadoVenta = lstDataTbl4.Where(x => x.idMotivo == (int)MotivoInspeccionEnum.Venta && x.esLiberado).Count();
                        List<int> lstCantLiberados = new List<int>();
                        lstCantLiberados.Add(cantLiberadoObra);
                        lstCantLiberados.Add(cantLiberadoVenta);
                        resultado.Add("lstCantLiberados", lstCantLiberados);
                        #endregion
                        #endregion
                    }
                    catch (Exception e)
                    {
                    }
                }

                return resultado;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetIndicadoresRehabilitacionTMC", e, AccionEnum.CONSULTA, 0, objFiltro);
                return null;
            }
        }

        public List<BackLogsDTO> GetBackLogs(int id)
        {
            try
            {
                var OT = _context.tblBL_OT.Where(x => x.esActivo).ToList();
                var lstHoras = _context.tblM_CapOrdenTrabajo.ToList();
                var lstOrdenTrabajo = _context.tblBL_OrdenesCompra.Where(x => x.esActivo).ToList();
                List<BackLogsDTO> lstBL = _context.tblBL_CatBackLogs.Where(x => x.idSegPpto == id && x.tipoBL == (int)TipoBackLogEnum.TMC && x.esActivo).Select(x => new BackLogsDTO
                {
                    id = x.id,
                    folioBL = x.folioBL,
                    noEconomico = x.noEconomico,
                    horas = x.horas,
                    descripcion = x.descripcion,
                    fechaLiberado = x.fechaLiberadoBL,
                    horasTerminacion = 0,
                    idOT = 0
                }).ToList();
                foreach (var item in lstBL)
                {
                    item.idOT = OT.Where(x => x.idBL == item.id).Select(x => x.idOT).FirstOrDefault();
                    item.horasTerminacion = lstHoras.Where(x => x.id == item.idOT).Select(x => x.TiempoHorasReparacion).FirstOrDefault();
                }

                return lstBL;
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetBackLogs", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }
        #endregion

        public List<BackLogsDTO> GetBackLogsPresupuesto(string noEconomico)
        {
            try
            {

                tblBL_CatConjuntos obj = new tblBL_CatConjuntos();
                List<BackLogsDTO> lstBackLogs = _context.tblBL_CatBackLogs.Where(x => x.noEconomico == noEconomico && x.esActivo && x.tipoBL == (int)TipoBackLogEnum.TMC && x.esLiberado == false).OrderByDescending(x => x.folioBL).Select(x => new BackLogsDTO
                {
                    id = x.id,
                    folioBL = x.folioBL,
                    fechaInspeccion = x.fechaInspeccion,
                    noEconomico = x.noEconomico,
                    horas = x.horas,
                    subconjunto = !string.IsNullOrEmpty(x.subconjunto.abreviacion) ? x.subconjunto.abreviacion : x.subconjunto.descripcion,
                    idSubconjunto = x.idSubconjunto,
                    conjunto = !string.IsNullOrEmpty(x.subconjunto.CatConjuntos.abreviacion) ? x.subconjunto.CatConjuntos.abreviacion : x.subconjunto.CatConjuntos.descripcion,
                    idConjunto = x.subconjunto.CatConjuntos.id,
                    descripcion = x.descripcion,
                    parte = x.parte,
                    manoObra = x.manoObra,
                    estatus = "",
                    idEstatus = x.idEstatus,
                    diasTotales = 0,
                    fechaModificacionBL = x.fechaModificacionBL,
                    //presupuestoEstimado = GetPresupuestoEstimado(x.id, x.cc, lstRequisiciones, lstOC),
                    presupuestoEstimado = x.presupuestoEstimado
                }).ToList();

                foreach (var item in lstBackLogs)
                {
                    item.estatus = EnumHelper.GetDescription((EstatusBackLogEnum)item.idEstatus);
                    item.diasTotales = DiasTranscurridos(item.id, item.idEstatus, Convert.ToDateTime(item.fechaActual), Convert.ToDateTime(item.fechaModificacionBL));
                    item.fechaModificacionBL = item.idEstatus == (int)EstatusBackLogEnum.BackLogsInstalado ? Convert.ToDateTime(item.fechaModificacionBL) : Convert.ToDateTime(null);

                    // SE CAMBIA A MAYUSCULAS EL TEXTO DE CONJUNTOS Y SUBCONJUNTOS
                    if (!string.IsNullOrEmpty(item.conjunto))
                    {
                        string conjunto = item.conjunto.Trim().ToUpper();
                        item.conjunto = conjunto;
                    }

                    if (!string.IsNullOrEmpty(item.subconjunto))
                    {
                        string subconjunto = item.subconjunto.Trim().ToUpper();
                        item.subconjunto = subconjunto;
                    }
                }

                return lstBackLogs;

            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetBackLogsFiltros", e, AccionEnum.CONSULTA, 0, noEconomico);
                return null;
            }
        }

        public Dictionary<string, object> GetBackLogsGraficaresponsable(int inicioMes, int finMes, string areaCuenta, List<int> _lstResponsables, int inicioAnio, int finAnio)
        {
            Dictionary<string, object> lstResultado = new Dictionary<string, object>();
            List<object> lstResult = new List<object>();
            try
            {
                List<int> lstResponsables = new List<int>();
                if (_lstResponsables != null)
                {
                    foreach (var item in _lstResponsables)
                    {
                        lstResponsables.Add(item);
                    }
                }

                //var lst = _context.tblBL_CatBackLogs.Where(w => w.esActivo && 
                //            (w.fechaCreacionBL.Month >= inicioMes && w.fechaCreacionBL.Month <= finMes) &&
                //            (w.fechaCreacionBL.Year >= inicioAnio && w.fechaCreacionBL.Year <= finAnio) && 
                //            w.areaCuenta == areaCuenta && 
                //            lstResponsables.Contains(w.idUsuarioResponsable))
                //        .ToList().GroupBy(n => n.idEstatus).ToList();

                string strQuery = string.Format("SELECT * FROM tblBL_CatBackLogs WHERE areaCuenta = '{0}' AND esActivo = {1} AND (MONTH(fechaInspeccion) BETWEEN {2} AND {3}) AND (YEAR(fechaInspeccion) BETWEEN {4} AND {5})",
                                                    areaCuenta, 1, inicioMes, finMes, inicioAnio, finAnio);
                var lst = _context.Select<tblBL_CatBackLogs>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList().GroupBy(g => g.idEstatus).ToList();

                foreach (var item in lst)
                {
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.ElaboracionInspeccion)
                    {
                        var obj = new
                        {
                            name = "20%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.ElaboracionRequisicion)
                    {
                        var obj = new
                        {
                            name = "40%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.ElaboracionOC)
                    {
                        var obj = new
                        {
                            name = "50%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.SuministroRefacciones)
                    {
                        var obj = new
                        {
                            name = "60%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.RehabilitacionProgramada)
                    {
                        var obj = new
                        {
                            name = "80%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.ProcesoInstalacion)
                    {
                        var obj = new
                        {
                            name = "90%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                    if (item.Select(y => y.idEstatus).FirstOrDefault() == (int)EstatusBackLogEnum.BackLogsInstalado)
                    {
                        var obj = new
                        {
                            name = "100%",
                            y = item.Count(),
                        };
                        lstResult.Add(obj);
                    }
                }
                var listado = new
                {
                    name = "",
                    colorByPoint = true,
                    data = lstResult,
                };

                List<object> lstResult2 = new List<object>();
                lstResult2.Add(listado);
                lstResultado.Add("gpxBarra", lstResult2);
                lstResultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, lstResultado);
                return null;
            }
            return lstResultado;
        }

        public Dictionary<string, object> GetGraficaBLDias(BackLogsDTO objParamsDTO)
        {
            #region V1
            //try
            //{
            //    Dictionary<string, object> result = new Dictionary<string, object>();
            //    List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.Obra && x.esActivo).ToList();
            //    List<BackLogsDTO> lstGraficaDias = lstBL.Select(x => new BackLogsDTO
            //    {
            //        diasTotales = 0,
            //        fechaCreacionBL = x.fechaCreacionBL,
            //        fechaLiberadoBL = x.fechaLiberadoBL
            //    }).ToList();
            //    int cant0a40 = 0, cant41a60 = 0, cant61a80 = 0, cant81oMayor = 0;
            //    foreach (var item in lstGraficaDias)
            //    {
            //        if (item.fechaLiberadoBL.Year > 2000)
            //            item.diasTotales = (item.fechaCreacionBL - item.fechaLiberadoBL).Days;
            //        else
            //            item.diasTotales = (DateTime.Now - item.fechaCreacionBL).Days;

            //        int diasTranscurridos = item.diasTotales;
            //        if (diasTranscurridos >= 0 && diasTranscurridos <= 40)
            //            cant0a40++;
            //        else if (diasTranscurridos >= 41 && diasTranscurridos <= 60)
            //            cant41a60++;
            //        else if (diasTranscurridos >= 61 && diasTranscurridos <= 80)
            //            cant61a80++;
            //        else if (diasTranscurridos >= 81)
            //            cant81oMayor++;
            //    }
            //    List<int> lstDias = new List<int>();
            //    lstDias.Add(cant0a40);
            //    lstDias.Add(cant41a60);
            //    lstDias.Add(cant61a80);
            //    lstDias.Add(cant81oMayor);

            //    result.Add("lstGraficaDias", lstDias);
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetGraficaBLDias", ex, AccionEnum.CONSULTA, 0, 0);
            //    return null;
            //} 
            #endregion

            #region V2
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objParamsDTO.areaCuenta)) { throw new Exception("Es necesario seleccionar un área cuenta"); }
                    if (objParamsDTO.lstMeses.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos un mes"); }
                    #endregion

                    #region INIT VARIABLES
                    int cant0a40 = 0, cant41a60 = 0, cant61a80 = 0, cant81oMayor = 0;
                    #endregion

                    string strQuery = string.Format(@"SELECT t1.*
			                                                FROM tblBL_CatBackLogs AS t1
			                                                INNER JOIN tblBL_CatSubconjuntos AS t2 ON t2.id = t1.idSubconjunto
			                                                INNER JOIN tblBL_CatConjuntos AS t3 ON t3.id = t2.idConjunto
				                                                WHERE YEAR(t1.fechaCreacionBL) = {0} AND MONTH(t1.fechaCreacionBL) IN ({1}) AND t1.esActivo = {2} AND t1.areaCuenta = '{3}'",
                                                                    objParamsDTO.anio, string.Join(",", objParamsDTO.lstMeses), 1, objParamsDTO.areaCuenta);
                    List<BackLogsDTO> lstBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    List<BackLogsDTO> lstGraficaDias = lstBL.Select(x => new BackLogsDTO
                    {
                        diasTotales = 0,
                        fechaCreacionBL = x.fechaCreacionBL,
                        fechaLiberadoBL = x.fechaLiberadoBL
                    }).ToList();

                    foreach (var item in lstGraficaDias)
                    {
                        if (item.fechaLiberadoBL.Year > 2000)
                            item.diasTotales = (item.fechaCreacionBL - item.fechaLiberadoBL).Days;
                        else
                            item.diasTotales = (DateTime.Now - item.fechaCreacionBL).Days;

                        int diasTranscurridos = item.diasTotales;
                        if (diasTranscurridos >= 0 && diasTranscurridos <= 40)
                            cant0a40++;
                        else if (diasTranscurridos >= 41 && diasTranscurridos <= 60)
                            cant41a60++;
                        else if (diasTranscurridos >= 61 && diasTranscurridos <= 80)
                            cant61a80++;
                        else if (diasTranscurridos >= 81)
                            cant81oMayor++;
                    }

                    List<int> lstDias = new List<int>();
                    lstDias.Add(cant0a40);
                    lstDias.Add(cant41a60);
                    lstDias.Add(cant61a80);
                    lstDias.Add(cant81oMayor);

                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstGraficaDias", lstDias);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                }
                return resultado;
            }
            #endregion
        }

        public Dictionary<string, object> GetGraficaConjuntos(BackLogsDTO objParamsDTO)
        {
            #region VERSION 1
            //try
            //{
            //    Dictionary<string, object> result = new Dictionary<string, object>();
            //    List<tblBL_CatBackLogs> lstBL = _context.tblBL_CatBackLogs.Where(x => x.tipoBL == (int)TipoBackLogEnum.Obra && x.esActivo).ToList();
            //    List<BackLogsDTO> lstSubconjuntos = lstBL.GroupBy(g => g.idSubconjunto).Select(x =>
            //    {
            //        var idConjunto = _context.tblBL_CatSubconjuntos.Where(w => w.id == x.Key).Select(s => s.idConjunto).FirstOrDefault();
            //        var conjunto = _context.tblBL_CatConjuntos.Where(w => w.id == idConjunto).Select(s => s.descripcion).FirstOrDefault();
            //        var numeroSubCojunto = x.Count();
            //        return new BackLogsDTO
            //        {
            //            numeroSubConjunto = numeroSubCojunto,
            //            conjunto = conjunto
            //        };
            //    }).ToList();

            //    var lstConjuntos = lstSubconjuntos.GroupBy(g => g.conjunto).Select(s => new
            //    {
            //        name = s.Key,
            //        y = s.Sum(x => x.numeroSubConjunto)
            //    }).ToList();

            //    result.Add("lstConjuntos", lstConjuntos);
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetGraficaConjuntos", ex, AccionEnum.CONSULTA, 0, 0);
            //    return null;
            //}
            #endregion

            #region VERSION 2
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objParamsDTO.areaCuenta)) { throw new Exception("Es necesario seleccionar un área cuenta"); }
                    if (objParamsDTO.lstMeses.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos un mes"); }
                    #endregion

                    #region CATALOGOS
                    List<tblBL_CatConjuntos> lstConjuntos = _ctx.Select<tblBL_CatConjuntos>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT * FROM tblBL_CatConjuntos"
                    }).ToList();

                    List<tblBL_CatSubconjuntos> lstSubConjuntos = _ctx.Select<tblBL_CatSubconjuntos>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT * FROM tblBL_CatSubconjuntos"
                    }).ToList();
                    #endregion

                    string strQuery = string.Format(@"SELECT t1.*
			                                                FROM tblBL_CatBackLogs AS t1
			                                                INNER JOIN tblBL_CatSubconjuntos AS t2 ON t2.id = t1.idSubconjunto
			                                                INNER JOIN tblBL_CatConjuntos AS t3 ON t3.id = t2.idConjunto
				                                                WHERE YEAR(t1.fechaCreacionBL) = {0} AND MONTH(t1.fechaCreacionBL) IN ({1}) AND t1.esActivo = {2} AND t1.areaCuenta = '{3}'",
                                                                    objParamsDTO.anio, string.Join(",", objParamsDTO.lstMeses), 1, objParamsDTO.areaCuenta);
                    List<BackLogsDTO> lstBL = _ctx.Select<BackLogsDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery
                    }).ToList();

                    List<BackLogsDTO> lstCatSubConjuntos = lstBL.GroupBy(g => g.idSubconjunto).Select(x =>
                    {
                        int idConjunto = lstSubConjuntos.Where(w => w.id == x.Key).Select(s => s.idConjunto).FirstOrDefault();
                        string conjunto = lstConjuntos.Where(w => w.id == idConjunto).Select(s => s.descripcion).FirstOrDefault();
                        int numeroSubCojunto = x.Count();
                        return new BackLogsDTO
                        {
                            numeroSubConjunto = numeroSubCojunto,
                            conjunto = conjunto
                        };
                    }).ToList();

                    var lstCatConjuntos = lstCatSubConjuntos.GroupBy(g => g.conjunto).Select(s => new
                    {
                        name = PersonalUtilities.PrimerLetraMayuscula(s.Key),
                        y = s.Sum(x => x.numeroSubConjunto)
                    }).ToList();

                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstConjuntos", lstCatConjuntos);
                }
                catch (Exception e)
                {
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                }
                return resultado;
            }
            #endregion
        }
        #endregion

        #region EXCEL CARGO NÓMINA
        public Dictionary<string, object> FillComboCentroCostoBackLogs()
        {
            try
            {
                var listaCentroCosto = _context.tblBL_CatBackLogs.Where(x => x.esActivo && x.tipoBL == 2).ToList().Join(
                    _context.tblBL_OT.Where(x => x.esActivo).ToList(),
                    b => b.id,
                    o => o.idBL,
                    (b, o) => new { b, o }
                ).GroupBy(x => new { x.b.cc, x.b.noEconomico }).Select(x => new ComboDTO
                {
                    Value = x.Key.cc,
                    Text = x.Key.noEconomico
                }).OrderBy(x => x.Text).ToList();

                resultado.Add(ITEMS, listaCentroCosto);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillComboCentroCostoBackLogs", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public Dictionary<string, object> GetGraficaCargoNomina(List<string> listaEconomicos, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var listaBacklogs = _context.tblBL_CatBackLogs.Where(x =>
                    x.esActivo &&
                    x.tipoBL == 2 &&
                    DbFunctions.TruncateTime(x.fechaCreacionBL) >= DbFunctions.TruncateTime(fechaInicio) &&
                    DbFunctions.TruncateTime(x.fechaCreacionBL) <= DbFunctions.TruncateTime(fechaFin)
                ).ToList().Where(x => listaEconomicos != null && listaEconomicos.Count() > 0 ? listaEconomicos.Contains(x.noEconomico) : true).Join(
                    _context.tblBL_OT.Where(x => x.esActivo),
                    b => b.id,
                    o => o.idBL,
                    (b, o) => new { b, o }
                ).Join(
                    _context.tblM_DetOrdenTrabajo,
                    bo => bo.o.idOT,
                    d => d.OrdenTrabajoID,
                    (bo, d) => new { bo, d }
                ).Join(
                    _context.tblM_CatMaquina,
                    bod => bod.bo.b.noEconomico,
                    m => m.noEconomico,
                    (bod, m) => new { bod, m }
                ).ToList().Select(x => new CargoNominaDTO
                {
                    noEconomico = x.bod.bo.b.noEconomico,
                    areaCuenta = x.bod.bo.b.areaCuenta,
                    descripcion = x.m.descripcion,
                    cc = x.bod.bo.b.cc,
                    horasPeriodo = (decimal)(x.bod.d.HoraFin - x.bod.d.HoraInicio).TotalHours,
                    porcentajeCargo = 0
                }).ToList();

                var horasTotales = listaBacklogs.Sum(x => x.horasPeriodo);
                var graficaCargoNomina = new List<GraficaPastelDTO>();

                foreach (var backlog in listaBacklogs)
                {
                    backlog.porcentajeCargo = Math.Truncate(100 * (backlog.horasPeriodo * 100 / (horasTotales > 0 ? horasTotales : 1))) / 100;

                    graficaCargoNomina.Add(new GraficaPastelDTO() { name = backlog.noEconomico, y = backlog.porcentajeCargo });
                };

                HttpContext.Current.Session["listaBacklogsCargoNomina"] = listaBacklogs;
                HttpContext.Current.Session["horasTotalesCargoNomina"] = horasTotales;
                HttpContext.Current.Session["fechaInicio"] = fechaInicio.ToShortDateString();
                HttpContext.Current.Session["fechaFin"] = fechaFin.ToShortDateString();

                resultado.Add("data", graficaCargoNomina);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetGraficaCargoNomina", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public MemoryStream DescargarExcelCargoNomina(string imagenString)
        {
            try
            {
                var listaBacklogs = HttpContext.Current.Session["listaBacklogsCargoNomina"] as List<CargoNominaDTO>;
                var horasTotales = (decimal)HttpContext.Current.Session["horasTotalesCargoNomina"];
                var fechaInicio = HttpContext.Current.Session["fechaInicio"] as string;
                var fechaFin = HttpContext.Current.Session["fechaFin"] as string;

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Hoja1");

                    Color colorNaranjaOscuro = Color.FromArgb(237, 125, 49);
                    Color colorNaranjaClaro = Color.FromArgb(244, 176, 132);
                    Color colorAmarillo = Color.FromArgb(255, 192, 0);

                    #region Encabezado
                    hoja.Cells["B3:J3"].Merge = true;
                    hoja.Cells["B3"].Value = "Arrendadora Construplan S.A. de C.V.";
                    hoja.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["B3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["B3"].Style.Font.Bold = true;
                    hoja.Cells["B3"].Style.Font.Size = 14;
                    hoja.Cells["B4:J4"].Merge = true;
                    hoja.Cells["B4"].Value = "Cargo de Nómina por Centros de Costo.";
                    hoja.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["B4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["B4"].Style.Font.Bold = true;
                    hoja.Cells["B4"].Style.Font.Size = 12;

                    hoja.Cells["I7"].Value = "Inicial";
                    hoja.Cells["I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    hoja.Cells["J7"].Value = "Final";
                    hoja.Cells["J7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["J7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    hoja.Cells["B8:C8"].Merge = true;
                    hoja.Cells["B8"].Value = "Área Cuenta";
                    hoja.Cells["B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["B8"].Style.Font.Bold = true;
                    hoja.Cells["B8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["B8"].Style.Fill.BackgroundColor.SetColor(colorNaranjaOscuro);
                    hoja.Cells["B8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["D8:F8"].Merge = true;
                    hoja.Cells["D8"].Value = "Taller Mecánico Central";
                    hoja.Cells["D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["D8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["D8"].Style.Font.Bold = true;
                    hoja.Cells["D8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["D8"].Style.Fill.BackgroundColor.SetColor(colorAmarillo);
                    hoja.Cells["D8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["F8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["F8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["F8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["F8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["H8"].Value = "Periodo";
                    hoja.Cells["H8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["H8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["H8"].Style.Font.Bold = true;
                    hoja.Cells["H8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["H8"].Style.Fill.BackgroundColor.SetColor(colorNaranjaOscuro);
                    hoja.Cells["H8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["I8"].Value = fechaInicio;
                    hoja.Cells["I8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["I8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["I8"].Style.Font.Bold = true;
                    hoja.Cells["I8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["I8"].Style.Fill.BackgroundColor.SetColor(colorAmarillo);
                    hoja.Cells["I8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["J8"].Value = fechaFin;
                    hoja.Cells["J8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["J8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["J8"].Style.Font.Bold = true;
                    hoja.Cells["J8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["J8"].Style.Fill.BackgroundColor.SetColor(colorAmarillo);
                    hoja.Cells["J8"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["J8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["J8"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["J8"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["B10:C10"].Merge = true;
                    hoja.Cells["B10"].Value = "CC Nómina";
                    hoja.Cells["B10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["B10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["B10"].Style.Font.Bold = true;
                    hoja.Cells["B10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["B10"].Style.Fill.BackgroundColor.SetColor(colorNaranjaOscuro);
                    hoja.Cells["B10"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B10"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B10"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B10"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C10"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C10"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C10"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C10"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["D10"].Value = "100-1";
                    hoja.Cells["D10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["D10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["D10"].Style.Font.Bold = true;
                    hoja.Cells["D10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["D10"].Style.Fill.BackgroundColor.SetColor(colorAmarillo);
                    hoja.Cells["D10"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D10"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D10"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D10"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["G10:H10"].Merge = true;
                    hoja.Cells["G10"].Value = "Nómina Semanal";
                    hoja.Cells["G10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["G10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["G10"].Style.Font.Bold = true;
                    hoja.Cells["G10"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["G10"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["G10"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["G10"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H10"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H10"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H10"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H10"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["I10"].Value = "";
                    hoja.Cells["I10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["I10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["I10"].Style.Font.Bold = true;
                    hoja.Cells["I10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["I10"].Style.Fill.BackgroundColor.SetColor(colorAmarillo);
                    hoja.Cells["I10"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I10"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I10"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I10"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["J10"].Value = "M/N";
                    hoja.Cells["J10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> headerRow = new List<string[]> {
                        new string[] { "Económico", "Área-Cuenta", "Descripción", "", "", "Centro de Costo", "H-H del Periodo", "% cargo", "Cargo por Maquina $" }
                    };

                    hoja.Cells["D12:F12"].Merge = true;

                    hoja.Cells["B12:J12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["B12:J12"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["B12:J12"].Style.Font.Bold = true;
                    hoja.Cells["B12:J12"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells["B12:J12"].Style.Fill.BackgroundColor.SetColor(colorNaranjaOscuro);
                    hoja.Cells["B12:J12"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B12:J12"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B12:J12"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B12:J12"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B12:J12"].LoadFromArrays(headerRow);
                    #endregion

                    #region Datos
                    var cellData = new List<object[]>();
                    var contadorRenglonDato = 13;

                    foreach (var backlog in listaBacklogs)
                    {
                        cellData.Add(new object[] { backlog.noEconomico, backlog.areaCuenta, backlog.descripcion, "", "", backlog.cc, backlog.horasPeriodo, backlog.porcentajeCargo + "%", "" });

                        hoja.Cells[string.Format("D{0}:F{0}", contadorRenglonDato)].Merge = true;
                        hoja.Cells[string.Format("J{0}", contadorRenglonDato)].Formula = string.Format("=I{0}*$I$10", contadorRenglonDato);

                        contadorRenglonDato++;
                    }

                    hoja.Cells[13, 2].LoadFromArrays(cellData);

                    int numeroUltimoRenglonData = 12 + cellData.Count();
                    int numeroRenglonTotal1 = 13 + cellData.Count();
                    int numeroRenglonTotal2 = 14 + cellData.Count();

                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.Fill.BackgroundColor.SetColor(colorNaranjaClaro);
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    hoja.Cells[string.Format("B13:J{0}", numeroUltimoRenglonData)].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    hoja.Cells[string.Format("B{0}:H{0}", numeroRenglonTotal1)].Merge = true;
                    hoja.Cells["B" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["B" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["B" + numeroRenglonTotal1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["B" + numeroRenglonTotal1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["C" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["C" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["D" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["D" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["E" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["E" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["F" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["F" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["G" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["G" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["H" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["H" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["H" + numeroRenglonTotal1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    hoja.Cells["I" + numeroRenglonTotal1].Value = "100%";
                    hoja.Cells["I" + numeroRenglonTotal1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["I" + numeroRenglonTotal1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["I" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["I" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["I" + numeroRenglonTotal1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["I" + numeroRenglonTotal1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    hoja.Cells["J" + numeroRenglonTotal1].Formula = string.Format("=I{0}*$I$10", numeroRenglonTotal1);
                    hoja.Cells["J" + numeroRenglonTotal1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["J" + numeroRenglonTotal1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["J" + numeroRenglonTotal1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["J" + numeroRenglonTotal1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["J" + numeroRenglonTotal1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    hoja.Cells["J" + numeroRenglonTotal1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    hoja.Cells[string.Format("B{0}:D{0}", numeroRenglonTotal2)].Merge = true;
                    hoja.Cells["B" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B" + numeroRenglonTotal2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["B" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["C" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["D" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells[string.Format("E{0}:G{0}", numeroRenglonTotal2)].Merge = true;
                    hoja.Cells["E" + numeroRenglonTotal2].Value = "HORAS TOTALES";
                    hoja.Cells["E" + numeroRenglonTotal2].Style.Font.Bold = true;
                    hoja.Cells["E" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E" + numeroRenglonTotal2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["E" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["F" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["F" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["G" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["G" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["G" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["H" + numeroRenglonTotal2].Value = listaBacklogs.Sum(x => x.horasPeriodo);
                    hoja.Cells["H" + numeroRenglonTotal2].Style.Font.Bold = true;
                    hoja.Cells["H" + numeroRenglonTotal2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["H" + numeroRenglonTotal2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["H" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H" + numeroRenglonTotal2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["H" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["I" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I" + numeroRenglonTotal2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["I" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    hoja.Cells["J" + numeroRenglonTotal2].Formula = string.Format("=SUMA(J13:J{0})", numeroRenglonTotal1);
                    hoja.Cells["J" + numeroRenglonTotal2].Style.Font.Bold = true;
                    hoja.Cells["J" + numeroRenglonTotal2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["J" + numeroRenglonTotal2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["J" + numeroRenglonTotal2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells["J" + numeroRenglonTotal2].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    #endregion

                    #region Imágenes
                    var logoArrendadora = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\arrendadora.png"));

                    logoArrendadora = resizeImage(logoArrendadora, new Size(200, 136));

                    var picture = hoja.Drawings.AddPicture("logoArrendadora", logoArrendadora);
                    picture.SetPosition(0, 0, 1, 0);

                    picture = hoja.Drawings.AddPicture("distribucionCargosCC", ByteArrToImage(Convert.FromBase64String(imagenString.Split(',')[1])));
                    picture.SetPosition(16 + cellData.Count(), 0, 1, 0);
                    #endregion

                    hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

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

        public Image ByteArrToImage(byte[] array)
        {
            // Convert base 64 string to byte[]
            using (var ms = new MemoryStream(array))
            {
                return Image.FromStream(ms);
            }
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> FillCboTipoMonedas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO TIPO MONEDAS
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();

                // PESO MEXICANO
                int tipoMoneda = (int)TipoMonedaEnum.MXN;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoMoneda.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoMonedaEnum)tipoMoneda);
                lstComboDTO.Add(objComboDTO);

                // DOLAR USD
                tipoMoneda = (int)TipoMonedaEnum.USD;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoMoneda.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoMonedaEnum)tipoMoneda);
                lstComboDTO.Add(objComboDTO);

                // PESO COLOMBIANO
                tipoMoneda = (int)TipoMonedaEnum.COP;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoMoneda.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoMonedaEnum)tipoMoneda);
                lstComboDTO.Add(objComboDTO);

                // PESO PERUANO
                tipoMoneda = (int)TipoMonedaEnum.SOL;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoMoneda.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoMonedaEnum)tipoMoneda);
                lstComboDTO.Add(objComboDTO);

                string tipoMonedaMXN = TipoMonedaEnum.MXN.ToString();
                string tipoMonedaUSD = TipoMonedaEnum.USD.ToString();
                string tipoMonedaCOP = TipoMonedaEnum.COP.ToString();
                string tipoMonedaSOL = TipoMonedaEnum.SOL.ToString();
                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Arrendadora:
                        lstComboDTO = lstComboDTO.Where(w => w.Text == tipoMonedaMXN || w.Text == tipoMonedaUSD).ToList();
                        break;
                    case (int)EmpresaEnum.Colombia:
                        lstComboDTO = lstComboDTO.Where(w => w.Text == tipoMonedaMXN || w.Text == tipoMonedaUSD || w.Text == tipoMonedaCOP).ToList();
                        break;
                    case (int)EmpresaEnum.Peru:
                        lstComboDTO = lstComboDTO.Where(w => w.Text == tipoMonedaMXN || w.Text == tipoMonedaUSD || w.Text == tipoMonedaSOL).ToList();
                        break;
                    default:
                        break;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboTipoMonedas", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetNombreTipoMoneda(int tipoMoneda)
        {
            string nombreTipoMoneda = string.Empty;
            try
            {
                #region VALIDACIONES
                if (tipoMoneda <= 0) { throw new Exception("El insumo consultado, no tiene asignado el tipo de moneda."); }
                #endregion

                #region SE OBTIENE EL NOMBRE DEL TIPO DE LA MONEDA
                nombreTipoMoneda = EnumHelper.GetDescription((TipoMonedaEnum)tipoMoneda);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetNombreTipoMoneda", e, AccionEnum.CONSULTA, 0, new { tipoMoneda = tipoMoneda });
                return nombreTipoMoneda;
            }
            return nombreTipoMoneda;
        }

        public Dictionary<string, object> FillCboAC()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO AC
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    List<string> lstAC_FK = _context.tblP_CC_Usuario.Where(w => w.usuarioID == vSesiones.sesionUsuarioDTO.id && w.cc != "000" && w.cc != "032").Select(s => s.cc).ToList();
                    if (lstAC_FK.Count() <= 0)
                        throw new Exception("No cuenta con algún AC asignado. Favor de notificarlo");

                    List<tblP_CC> lstAC = _context.tblP_CC.Where(w => lstAC_FK.Contains(w.areaCuenta) && w.estatus).ToList();

                    List<ComboDTO> lstComboAC = new List<ComboDTO>();
                    ComboDTO objComboAC = new ComboDTO();
                    foreach (var item in lstAC)
                    {
                        if (!string.IsNullOrEmpty(item.areaCuenta) && !string.IsNullOrEmpty(item.descripcion))
                        {
                            objComboAC = new ComboDTO();
                            objComboAC.Value = item.areaCuenta.ToString();
                            objComboAC.Text = string.Format("[{0}] {1}", item.areaCuenta.Trim(), item.descripcion.Trim());
                            lstComboAC.Add(objComboAC);
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstComboAC);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboAC", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoEquipo()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<ComboDTO> lstTipoEquipoDTO = new List<ComboDTO>();

                ComboDTO objTipoEquipoDTO = new ComboDTO();
                int tipoEquipoID = (int)TipoMaquinaEnum.Mayor;
                objTipoEquipoDTO.Value = tipoEquipoID.ToString();
                objTipoEquipoDTO.Text = EnumHelper.GetDescription((TipoMaquinaEnum)tipoEquipoID);
                lstTipoEquipoDTO.Add(objTipoEquipoDTO);

                objTipoEquipoDTO = new ComboDTO();
                tipoEquipoID = (int)TipoMaquinaEnum.Menor;
                objTipoEquipoDTO.Value = tipoEquipoID.ToString();
                objTipoEquipoDTO.Text = EnumHelper.GetDescription((TipoMaquinaEnum)tipoEquipoID);
                lstTipoEquipoDTO.Add(objTipoEquipoDTO);

                objTipoEquipoDTO = new ComboDTO();
                tipoEquipoID = (int)TipoMaquinaEnum.Transporte;
                objTipoEquipoDTO.Value = tipoEquipoID.ToString();
                objTipoEquipoDTO.Text = EnumHelper.GetDescription((TipoMaquinaEnum)tipoEquipoID);
                lstTipoEquipoDTO.Add(objTipoEquipoDTO);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstTipoEquipoDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboTipoEquipo", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private List<RelAreaCuentaAlmacenDTO> GetListaRelAreasCuentasAlmacen(RelAreaCuentaAlmacenDTO objParamsDTO = null)
        {
            List<RelAreaCuentaAlmacenDTO> lstRelAreasCuentasAlmacenDTO = new List<RelAreaCuentaAlmacenDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblAlm_RelAreaCuentaXAlmacen";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += !string.IsNullOrEmpty(objParamsDTO.AreaCuenta) ? string.Format(" AND AreaCuenta = '{0}'", objParamsDTO.AreaCuenta) : string.Empty;
                }
                #endregion

                #region CONDICIONES
                lstRelAreasCuentasAlmacenDTO = _context.Select<RelAreaCuentaAlmacenDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstRelAreasCuentasAlmacenDTO;
        }

        private List<RelAreaCuentaAlmacenDetDTO> GetListaRelAreasCuentasAlmacenesDet(RelAreaCuentaAlmacenDetDTO objParamsDTO = null)
        {
            List<RelAreaCuentaAlmacenDetDTO> lstRelAreasCuentasAlmacenesDet = new List<RelAreaCuentaAlmacenDetDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblAlm_RelAreaCuentaXAlmacenDet";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstRelAreasCuentasAlmacenesDet = _context.Select<RelAreaCuentaAlmacenDetDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstRelAreasCuentasAlmacenesDet;
        }

        private List<RequisicionesDTO> GetListaRequisiciones(RequisicionesDTO objParamsDTO = null)
        {
            List<RequisicionesDTO> lstRequisicionesDTO = new List<RequisicionesDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblBL_Requisiciones WHERE esActivo = @esActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstRequisicionesDTO = _context.Select<RequisicionesDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { esActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstRequisicionesDTO;
        }

        private List<OrdenCompraDTO> GetListaOrdenesComprasDTO(OrdenCompraDTO objParamsDTO = null)
        {
            List<OrdenCompraDTO> lstOrdenesComprasDTO = new List<OrdenCompraDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblBL_OrdenesCompra WHERE esActivo = @esActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstOrdenesComprasDTO = _context.Select<OrdenCompraDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { esActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstOrdenesComprasDTO;
        }

        private List<BackLogsDTO> GetListaBackLogs(BackLogsDTO objParamsDTO)
        {
            List<BackLogsDTO> lstBackLogsDTO = new List<BackLogsDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblBL_CatBackLogs WHERE esActivo = @esActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += !string.IsNullOrEmpty(objParamsDTO.noEconomico) ? string.Format(" AND noEconomico = '{0}'", objParamsDTO.noEconomico) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstBackLogsDTO = _context.Select<BackLogsDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { esActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstBackLogsDTO;
        }

        private List<CapHorometroBLDTO> GetListaCapHorometros(CapHorometroBLDTO objParamsDTO = null)
        {
            List<CapHorometroBLDTO> lstCapHorometrosDTO = new List<CapHorometroBLDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblM_CapHorometro";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += !string.IsNullOrEmpty(objParamsDTO.Economico) ? string.Format(" AND Economico = '{0}'", objParamsDTO.Economico) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstCapHorometrosDTO = _context.Select<CapHorometroBLDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCapHorometrosDTO;
        }

        private List<ParteDTO> GetListaPartes(ParteDTO objParamsDTO)
        {
            List<ParteDTO> lstPartesDTO = new List<ParteDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblBL_Partes WHERE esActivo = @esActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.lstBackLogsID.Count() > 0 ? string.Format(" AND id IN ({0})", string.Join(",", objParamsDTO.lstBackLogsID)) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstPartesDTO = _context.Select<ParteDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { esActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstPartesDTO;
        }

        private List<CatSubconjuntosDTO> GetListaSubconjuntos(CatSubconjuntosDTO objParamsDTO)
        {
            List<CatSubconjuntosDTO> lstSubconjuntosDTO = new List<CatSubconjuntosDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblBL_CatSubconjuntos WHERE esActivo = @esActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
	            #endregion

                #region CONSULTA
		        lstSubconjuntosDTO = _context.Select<CatSubconjuntosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { esActivo = true }
                }).ToList();
	            #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstSubconjuntosDTO;
        }

        private List<CatMaquinaDTO> GetListaCatMaquinasDTO(CatMaquinaDTO objParamsDTO = null)
        {
            List<CatMaquinaDTO> lstCatMaquinasDTO = new List<CatMaquinaDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblM_CatMaquina";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstCatMaquinasDTO = _context.Select<CatMaquinaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCatMaquinasDTO;
        }

        private List<CatModeloEquipoDTO> GetListaCatModeloEquipo(CatModeloEquipoDTO objParamsDTO = null)
        {
            List<CatModeloEquipoDTO> lstCatModelosEquipos = new List<CatModeloEquipoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblM_CatModeloEquipo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstCatModelosEquipos = _context.Select<CatModeloEquipoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCatModelosEquipos;
        }

        private List<CCDTO> GetListaCC(CCDTO objParamsDTO = null)
        {
            List<CCDTO> lstCCDTO = new List<CCDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblP_CC WHERE estatus = @estatus";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstCCDTO = _context.Select<CCDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { estatus = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCCDTO;
        }

        private Dictionary<string, object> GetTotalPrecioBL(BackLogsDTO objBL, List<RequisicionesDTO> lstRequisicionesDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al obtener el total del BL";
                if (objBL == null) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                if (lstRequisicionesDTO.Count() <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }
                #endregion

                int numRequisicion = lstRequisicionesDTO.Where(w => w.idBackLog == objBL.id && w.esActivo).Select(s => s.id).FirstOrDefault();
                if (numRequisicion <= 0)
                {
                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, numRequisicion);
                    return resultado;
                }
                else
                {
                    tblCom_Req objRequisicion = _context.tblCom_Req.Where(w => w.numero == numRequisicion && w.cc == objBL.cc && w.stEstatus == "T").FirstOrDefault();
                    if (objRequisicion != null)
                    {
                        List<tblCom_OrdenCompraDet> lstOrdenCompraDet = _context.tblCom_OrdenCompraDet.Where(w => w.num_requisicion == numRequisicion && w.cc == objBL.cc && w.estatusRegistro).ToList();
                        if (lstOrdenCompraDet.Count() > 0)
                        {
                            tblCom_OrdenCompra objOrdenCompra = _context.tblCom_OrdenCompra.Where(w => w.id == lstOrdenCompraDet[0].idOrdenCompra).FirstOrDefault();
                            if (objOrdenCompra == null)
                                throw new Exception("Ocurrió un error al obtener el total de la OC.");

                            int tipoMoneda = Convert.ToInt32(objOrdenCompra.moneda);
                            decimal totalMX = 0, totalUSD = 0;
                            if (tipoMoneda == (int)TipoMonedaEnum.MXN)
                            {
                                totalMX = objOrdenCompra.total;
                            } 
                            else if (tipoMoneda == (int)TipoMonedaEnum.USD)
                            {
                                totalUSD = objOrdenCompra.total;
                                totalMX = (objOrdenCompra.total * objOrdenCompra.tipo_cambio);
                            }

                            resultado.Clear();
                            resultado.Add(SUCCESS, true);
                            resultado.Add("totalMX", totalMX);
                            resultado.Add("totalUSD", totalUSD);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objBL.id, new { objBL = objBL, lstRequisicionesDTO = lstRequisicionesDTO });
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
    }
}
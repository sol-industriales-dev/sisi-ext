using Core.DTO;
using Core.DTO.Contabilidad.ControlPresupuestal;
using Core.DTO.Enkontrol;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Principal.Generales;
using Core.DTO.Reportes;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.DTO.Utils.Data;
using System.Data.Odbc;

namespace Data.DAO.Maquinaria.Reporte
{
    public class RepGastosMaquinaDAO : GenericDAO<tblM_CatMaquina>
    {
        public IList<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria()
        {
            //string tipoMaquinaria = "SELECT tipo_activo AS id, descripcion AS descripcion FROM saf_tipos_activo WHERE tipo_activo <4;";
            return _context.tblM_CatTipoMaquinaria.Where(x => x.estatus == true).ToList();
            //return (IList<tblM_CatTipoMaquinaria>)ContextConstruplan.Where(tipoMaquinaria).ToObject<IList<tblM_CatTipoMaquinaria>>();
        }
        public IList<GrupoMaquinariaDTO> FillCboGrupoMaquinaria(int tipo)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == true && x.tipoEquipoID == tipo).Select(x => new GrupoMaquinariaDTO { id = x.id, tipoMaquina = x.tipoEquipo.descripcion, descripcion = x.descripcion }).OrderBy(x => x.descripcion).ToList();
            //string grupoMaquinaria = "SELECT tipo_activo as tipoMaquina, grupo_activo AS id, descripcion AS descripcion from saf_grupos_activo where tipo_activo =" + tipo + " ORDER BY descripcion;";
            //return (IList<GrupoMaquinariaDTO>)ContextConstruplan.Where(grupoMaquinaria).ToObject<IList<GrupoMaquinariaDTO>>();
        }
        public IList<RepGastosMaquinaDTO> FillCboReporteGastosMaquinaria(int idGrupo, int idTipo)
        {
            //string maquina = "SELECT a.Descripcion,a.area,a.cuenta FROM si_area_cuenta a " +
            //                 "inner join saf_activos_fijos b " +
            //                 "on TRIM(a.descripcion) = TRIM(b.num_economico) " +
            //                 "where a.maquinaria=1 AND  a.cc_activo=1 AND " +
            //                 "b.tipo_activo = " + idTipo + " AND b.grupo_activo = " + idGrupo + " GROUP BY a.Descripcion,a.area,a.cuenta ORDER BY a.cuenta;";
            return _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID == idGrupo && x.grupoMaquinaria.tipoEquipoID == idTipo && x.estatus == 1).Select(x => new RepGastosMaquinaDTO { descripcion = x.noEconomico, area = x.id.ToString(), cuenta = "" }).ToList();
            //return (IList<RepGastosMaquinaDTO>)ContextConstruplan.Where(maquina).ToObject<IList<RepGastosMaquinaDTO>>();
        }

        public IList<RepGastosMaquinariaInfoDTO> FillGraficaRepGasto(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string anio = regresarAnio(fechaInicio, fechaFin);


            string consulta = "SELECT " + (anio.Equals("0") ? "anio," : "mes,") + " sum(importe) as importe FROM (SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes,") + " CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe " +//,d.descripcion AS descripcion " +
                              "FROM insumos a INNER JOIN si_movimientos_det b " +
                              "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                              "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen INNER JOIN cc c2 on c2.cc = c.cc " +
                              "WHERE c2.descripcion='" + obj.maq.Trim() + "' and b.tipo_mov = 51 " +
                              "AND fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' " +
                              " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + " " +
                              " UNION ALL " +
                              "SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes,") + "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe " +
                              "FROM insumos AS a " +
                              "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                              "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen INNER JOIN cc c2 on c2.cc = c.cc " +
                              "where c2.descripcion='" + obj.maq.Trim() + "' " +
                              " AND fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' " +//) +
                              " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + " " +
                              ") x GROUP BY " + (anio.Equals("0") ? "x.anio ORDER BY x.anio" : "x.mes ORDER BY x.mes") + ";";

            var res1 = (IList<RepGastosMaquinariaInfoDTO>)ContextConstruplan.Where(consulta).ToObject<IList<RepGastosMaquinariaInfoDTO>>();


            return res1.ToList();
        }

        public IList<RepGastosMaquinariaGrid> FillGridReporteGastosMaquinaria(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string anio = regresarAnio(fechaInicio, fechaFin);
            string fechaXmes = regresaFechaMes(fechaInicio, fechaFin, obj.mesID);
            string fechaXAnio = regresarFechaXAnio(fechaInicio, fechaFin, anio);

            string consulta = "SELECT " + (anio.Equals("0") ? "anio," : "mes,") + " sum(importe) as importe,tipoInsumo,descripcion  FROM (" +
                              "SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes, ") +
                              "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion " +//,d.descripcion AS descripcion " +
                              "FROM insumos a INNER JOIN si_movimientos_det b " +
                              "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                              "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                              "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo INNER JOIN cc c2 on c2.cc = c.cc " +
                              "where b.tipo_mov = 51 and c2.descripcion='" + obj.maq.Trim() + "' and " + (anio.Equals("0") ? fechaXAnio : (fechaXmes + " and c.ano=" + fechaFin.Year + " ")) +
                              " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + ",a.tipo,d.descripcion" +
                              " UNION ALL " +
                              "SELECT " + (anio.Equals("0") ? "c.ano as anio, " : " c.periodo AS mes, ") +
                              "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion " +
                              "FROM insumos AS a " +
                              "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                              "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
                              "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo INNER JOIN cc c2 on c2.cc = c.cc " +
                              "where c2.descripcion='" + obj.maq.Trim() + "' and " + (anio.Equals("0") ? fechaXAnio : (fechaXmes + " and c.ano=" + fechaFin.Year + " ")) +
                              " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + ",a.tipo,d.descripcion" +
                              ") x GROUP BY " + (anio.Equals("0") ? "x.anio" : "x.mes") + ",x.tipoInsumo,x.descripcion ORDER BY x.tipoInsumo;";

            return (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();
        }
        public IList<RepGastosMaquinariaGrid> FillGridReporteGastosGrupoInsumos(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string anio = regresarAnio(fechaInicio, fechaFin);
            string fechaXmes = regresaFechaMes(fechaInicio, fechaFin, obj.mesID);
            string fechaXAnio = regresarFechaXAnio(fechaInicio, fechaFin, anio);

            string consulta = "SELECT " + (anio.Equals("0") ? "anio," : "mes,") + " sum(importe) as importe,tipoInsumo,descripcion  FROM (" +
                 "SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes, ") +
                 "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.grupo as tipoInsumo, d.descripcion as descripcion " +//,d.descripcion AS descripcion " +
                 "FROM insumos a INNER JOIN si_movimientos_det b " +
                 "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                 "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                 "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo INNER JOIN cc c2 on c2.cc = c.cc " +
                 "where b.tipo_mov = 51 AND c2.descripcion='" + obj.maq.Trim() + "' and " + (anio.Equals("0") ? fechaXAnio : (fechaXmes + " and c.ano=" + fechaFin.Year + " ")) + "and d.tipo_insumo=" + obj.idTipo + "and a.tipo=" + obj.idTipo + " " +
                 " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + ",a.grupo,d.descripcion" +
                 " UNION ALL " +
                 "SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes, ") +
                 "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.grupo as tipoInsumo, d.descripcion as descripcion " +
                 "FROM insumos AS a " +
                 "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                 "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
                 "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo INNER JOIN cc c2 on c2.cc = c.cc " +
                 "where c2.descripcion='" + obj.maq.Trim() + "' and " + (anio.Equals("0") ? fechaXAnio : (fechaXmes + " and c.ano=" + fechaFin.Year + " ")) + "and a.tipo=" + obj.idTipo + "and d.tipo_insumo=" + obj.idTipo +  //+ (anio.Equals("0") ? "" : "AND fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' ") +
                 " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + ",a.grupo,d.descripcion" +
                 ") x GROUP BY " + (anio.Equals("0") ? "x.anio" : "x.mes") + ",x.tipoInsumo,x.descripcion ORDER BY x.tipoInsumo;";

            return (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();
        }
        public IList<RepGastosMaquinariaGrid> FillGridReporteGastosInsumos(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string anio = regresarAnio(fechaInicio, fechaFin);
            string fechaXmes = regresaFechaMes(fechaInicio, fechaFin, obj.mesID);
            string fechaXAnio = regresarFechaXAnio(fechaInicio, fechaFin, anio);

            string consulta = "SELECT " + (anio.Equals("0") ? "anio," : "mes,") + " sum(importe) as importe,descripcion,CONVERT( CHAR( 20 ), fecha, 105 ) FROM (" +
           "SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes, ") +
           "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe ,a.descripcion as descripcion,c.fecha AS fecha " +
           "FROM insumos a INNER JOIN si_movimientos_det b " +
           "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
           "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
           "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
           "INNER JOIN tipos_insumo e on e.tipo_insumo = a.tipo INNER JOIN cc c2 on c2.cc = c.cc " +
           "where b.tipo_mov = 51 and c2.descripcion='" + obj.maq.Trim() + "' and " + (anio.Equals("0") ? fechaXAnio : (fechaXmes + " and c.ano=" + fechaFin.Year + " ")) + "and d.tipo_insumo=" + obj.idTipo + "and a.tipo=" + obj.idTipo + "and d.grupo_insumo=" + obj.idGrupo + " " +
           " GROUP BY " + (anio.Equals("0") ? "c.ano,c.periodo" : "c.periodo") + ",a.descripcion,c.fecha" +
           " UNION ALL " +
           "SELECT " + (anio.Equals("0") ? "c.ano as anio," : " c.periodo AS mes, ") +
           "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.descripcion as descripcion,c.fecha AS fecha " +
           "FROM insumos AS a " +
           "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
           "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
           "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
           "INNER JOIN tipos_insumo e on e.tipo_insumo = a.tipo INNER JOIN cc c2 on c2.cc = c.cc " +
           "where c2.descripcion='" + obj.maq.Trim() + "' and " + (anio.Equals("0") ? fechaXAnio : (fechaXmes + " and c.ano=" + fechaFin.Year + " ")) + "and a.tipo=" + obj.idTipo + "and d.tipo_insumo=" + obj.idTipo + " and d.grupo_insumo=" + obj.idGrupo + //+ (anio.Equals("0") ? "" : "AND fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' ") +
           " GROUP BY " + (anio.Equals("0") ? "c.ano" : "c.periodo") + ",a.descripcion,c.fecha" +
           ") x GROUP BY " + (anio.Equals("0") ? "x.anio" : "x.mes") + ",x.descripcion,x.fecha ORDER BY x.fecha;";

            return (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();
        }
        public IList<RepGastosMaquinaInfoDTO> FillInfoGastosMaquinaria(string maquina)
        {
            try
            {
                string consulta = "SELECT descripcion, marca, modelo, valor_moi as saldoinicial,CONVERT( CHAR( 20 ), fecha_adquisicion, 105 ) AS fechaAdquisicion, dep_contable as depreciacion from saf_activos_fijos " +
                             "WHERE num_economico='" + maquina.Trim() + "' ORDER BY fecha_adquisicion desc;";
                var data = _context.tblM_CatMaquina.Where(x => x.noEconomico.Equals(maquina.Trim())).ToList().Select(x => new RepGastosMaquinaInfoDTO { descripcion = x.descripcion, marca = x.modeloEquipo.marcaEquipo.descripcion, modelo = x.modeloEquipo.descripcion, saldoinicial = "0", fechaAdquisicion = x.fechaAdquisicion.ToShortDateString(), depreciacion = "0" });
                //if(vSesiones.sesionEmpresaActual==3)
                //{
                //    consulta = "SELECT descripcion, marca, modelo, valor_moi as saldoinicial,CONVERT( CHAR( 20 ), fecha_adquisicion, 105 ) AS fechaAdquisicion, dep_contable as depreciacion from DBA.saf_activos_fijos " +
                //             "WHERE num_economico='" + maquina.Trim() + "' ORDER BY fecha_adquisicion desc;";
                //    data = _context.tblM_CatMaquina.Where(x => x.noEconomico.Equals(maquina.Trim())).ToList().Select(x => new RepGastosMaquinaInfoDTO { descripcion = x.descripcion, marca = x.modeloEquipo.marcaEquipo.descripcion, modelo = x.modeloEquipo.descripcion, saldoinicial = "0", fechaAdquisicion = x.fechaAdquisicion.ToShortDateString(), depreciacion = "0" });
                //}
                return data.ToList();
                //return (IList<RepGastosMaquinaInfoDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinaInfoDTO>>();
            }
            catch (Exception)
            {

                return null;
            }

        }

        public int valorXoverhaul(string maquina, DateTime fechaInicio, DateTime fechaFin)
        {

            //string consulta = "SELECT CASE WHEN SUM(valor_moi)  IS NULL THEN 0 ELSE SUM(valor_moi) END AS suma FROM saf_componentes WHERE economico_comp = '" + maquina + "' AND fecha_adquisicion BETWEEN '" + fechaInicio.ToString("yyyyMMdd") + "' AND '" + fechaFin.ToString("yyyyMMdd") + "'; ";

            //var c = (IList<ReporteGastosGetCostoOverhaulDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<ReporteGastosGetCostoOverhaulDTO>>();
            var c = new List<ReporteGastosGetCostoOverhaulDTO>();
            int valor = 0;
            if (c.Count >= 1)
            {
                valor = c.FirstOrDefault().suma;
            }
            return valor;
        }
        public decimal valorXoverhaulAplicado(RepGastosFiltrosDTO obj)
        {
            obj.idTipo = 5;
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string anio = regresarAnio(fechaInicio, fechaFin);
            string fechaXmes = regresaFechaMes(fechaInicio, fechaFin, obj.mesID);
            string fechaXAnio = regresarFechaXAnio(fechaInicio, fechaFin, obj.mes);

            string consulta = "SELECT " +
                 "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.grupo as tipoInsumo, d.descripcion as descripcion " +//,d.descripcion AS descripcion " +
                 "FROM insumos a INNER JOIN si_movimientos_det b " +
                 "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                 "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                 "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo INNER JOIN cc c2 on c2.cc = c.cc " +
                 "where b.tipo_mov = 51 AND c2.descripcion='" + obj.maq.Trim() + "' and fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and d.tipo_insumo=" + obj.idTipo + "and a.tipo=" + obj.idTipo + " " +
                 " GROUP BY a.grupo,d.descripcion;";

            var data = (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();

            var filtrado = data.Where(x => x.descripcion.Equals("COSTOS POR OVERHAUL") || x.descripcion.Equals("CASCO REMAN") || x.descripcion.Equals("COMPONENTES RECONSTRUIDOS 777 Y 992"));
            decimal valor = filtrado.Sum(x => Convert.ToDecimal(x.importe));
            return valor;
        }

        public decimal valorXoverhaulAplicadoByMaquina(string obj)
        {

            try
            {
                int idTipo = 5;

                string consulta = "SELECT " +
                     "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.grupo as tipoInsumo, d.descripcion as descripcion " +//,d.descripcion AS descripcion " +
                     "FROM insumos a INNER JOIN si_movimientos_det b " +
                     "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                     "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                     "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo INNER JOIN cc c2 on c2.cc = c.cc " +
                     "where c2.descripcion='" + obj + "' and b.tipo_mov = 51 AND d.tipo_insumo=" + idTipo + "and a.tipo=" + idTipo + " " +
                     " GROUP BY a.grupo,d.descripcion;";

                var data = (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();

                var filtrado = data.Where(x => x.descripcion.Equals("COSTOS POR OVERHAUL") || x.descripcion.Equals("CASCO REMAN") || x.descripcion.Equals("COMPONENTES RECONSTRUIDOS 777 Y 992"));
                decimal valor = filtrado.Sum(x => Convert.ToDecimal(x.importe));
                return valor;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        public string regresarAnio(DateTime fechaInicio, DateTime fechaFin)
        {
            DateTime today = DateTime.Now;

            string anio = "";

            if (fechaInicio.Year == fechaFin.Year)
            {
                anio = fechaInicio.Year.ToString();
            }
            else
            {
                anio = "0";
            }

            return anio;
        }

        private string regresaFechaMes(DateTime fechaInicio, DateTime fechaFin, int mes)
        {
            string query = "";
            if (fechaInicio.Month == fechaFin.Month)
            {
                return "fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' ";
            }
            if (fechaInicio.Month == mes)
            {
                string fecha = (mes + 1) + "/01/" + fechaInicio.Year;
                DateTime fechaConsulta = Convert.ToDateTime(fecha);
                return "c.fecha >='" + fechaInicio.ToString("yyyyMMdd") + "' and  c.fecha <'" + fechaConsulta.ToString("yyyyMMdd") + "' ";
            }
            if (fechaFin.Month == mes)
            {
                string fecha = (mes) + "/01/" + fechaInicio.Year;
                DateTime fechaConsulta = Convert.ToDateTime(fecha);
                return "c.fecha >='" + fechaConsulta.ToString("yyyyMMdd") + "' and  c.fecha <='" + fechaFin.ToString("yyyyMMdd") + "' ";
            }
            else
            {
                return "c.fecha >='" + fechaInicio.Year + fechaInicio.Month + "01" + "' and  c.fecha <'" + fechaFin.Year + fechaFin.Month + "01' ";
            }
        }
        private string regresarFechaXAnio(DateTime fechaInicio, DateTime fechaFin, string anio)
        {

            if (fechaInicio.Year.ToString() == anio)
            {
                return " fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaInicio.Year + "1231' ";
            }
            if (fechaFin.Year.ToString() == anio)
            {
                return " fecha between '" + fechaFin.Year + "0101' and '" + fechaFin.ToString("yyyyMMdd") + "' ";
            }
            if (fechaFin.Year.ToString() != anio && fechaInicio.Year.ToString() != anio)
            {
                return " fecha between '" + anio + "0101' and '" + anio + "1231' ";
            }

            return "";
        }

        public string getRango(string fechainicio, string fechafinal, string parametro)
        {
            DateTime fechaInicio = Convert.ToDateTime(fechainicio);
            DateTime fechaFin = Convert.ToDateTime(fechafinal);
            string c = regresaFechaMes(fechaInicio, fechaFin, Convert.ToInt32(parametro));
            if (Convert.ToInt32(parametro) <= 12)
            {
                if (fechaInicio.Month == fechaFin.Month)
                {
                    return fechaInicio.ToString("dd/MM/yyyy") + " al " + fechaFin.ToString("dd/MM/yyyy");
                }
                if (fechaInicio.Month == Convert.ToInt32(parametro))
                {
                    string fecha = DateTime.DaysInMonth(fechaInicio.Year, Convert.ToInt32(parametro)).ToString() + "/" + Convert.ToInt32(parametro).ToString().PadLeft(2, '0') + "/" + +fechaInicio.Year;
                    return fechaInicio.ToString("dd/MM/yyyy") + " al " + fecha;
                }
                if (fechaFin.Month == Convert.ToInt32(parametro))
                {
                    string fecha = (Convert.ToInt32(parametro)) + "/01/" + fechaInicio.Year;
                    DateTime fechaConsulta = Convert.ToDateTime(fecha);
                    return fechaConsulta.ToString("dd/MM/yyyy") + " al " + fechaFin.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "01/" + Convert.ToInt32(parametro).ToString().PadLeft(2, '0') + "/" + fechaInicio.Year + " al " + DateTime.DaysInMonth(fechaInicio.Year, Convert.ToInt32(parametro)).ToString() + "/" + (Convert.ToInt32(parametro)).ToString().PadLeft(2, '0') + "/" + fechaFin.Year;
                }
            }
            else
            {
                if (fechaInicio.Year.ToString() == parametro)
                {
                    return fechaInicio.ToString("dd/MM/yyyy") + " al " + "31/12/" + fechaInicio.Year;
                }
                if (fechaFin.Year.ToString() == parametro)
                {
                    return "01/01/" + fechaFin.Year + " al " + fechaFin.ToString("dd/MM/yyyy");
                }
                if (fechaFin.Year.ToString() != parametro && fechaInicio.Year.ToString() != parametro)
                {
                    return "01/01/" + parametro + " al " + "31/12/" + parametro;
                }
            }

            return "";
        }
        public tendenciaGeneralDTO getDatosGeneralesEmpresa(int empresa, int anio, List<string> cc, int grupo, int modelo)
        {
            var result = new tendenciaGeneralDTO();
            result.esMultiCuenta = true;

            //if(empresa ==0)
            //{
            //    empresa = vSesiones.sesionEmpresaActual;
            //}
            var listaDatos = new List<tendenciasDTO>();
            var ccData = _context.tblP_CC.Where(x => cc.Contains(x.areaCuenta)).ToList();
            var mes = anio == DateTime.Now.Year ? DateTime.Now.Month : 12;
            if (empresa == 0 || empresa == 1)
            {
                var lstCC = ccData.Select(x => x.cc).ToList();
                var strCC = string.Join("','", lstCC);
                string consulta = @"select
                                    x.cta,
                                    SUM(CASE WHEN x.mes = 1 THEN x.monto ELSE 0 END) as ene, 
                                    SUM(CASE WHEN x.mes = 2 THEN x.monto ELSE 0 END) as feb, 
                                    SUM(CASE WHEN x.mes = 3 THEN x.monto ELSE 0 END) as mar,  
                                    SUM(CASE WHEN x.mes = 4 THEN x.monto ELSE 0 END) as abr,  
                                    SUM(CASE WHEN x.mes = 5 THEN x.monto ELSE 0 END) as may,  
                                    SUM(CASE WHEN x.mes = 6 THEN x.monto ELSE 0 END) as jun,  
                                    SUM(CASE WHEN x.mes = 7 THEN x.monto ELSE 0 END) as jul,  
                                    SUM(CASE WHEN x.mes = 8 THEN x.monto ELSE 0 END) as ago,  
                                    SUM(CASE WHEN x.mes = 9 THEN x.monto ELSE 0 END) as sep,  
                                    SUM(CASE WHEN x.mes = 10 THEN x.monto ELSE 0 END) as oct, 
                                    SUM(CASE WHEN x.mes = 11 THEN x.monto ELSE 0 END) as nov,  
                                    SUM(CASE WHEN x.mes = 12 THEN x.monto ELSE 0 END) as dic,
                                    ROUND(SUM(x.monto),0) as total,
                                    0 as porcentaje,
                                    (SUM(CASE WHEN x.mes = " + mes + @"-1 THEN x.monto ELSE 0 END) - (SUM(CASE WHEN (x.mes >= " + mes + @"-7 and x.mes < " + mes + @"-1) THEN x.monto ELSE 0 END)/6) ) as variabilidad,
                                    0 as porcentajeVariabilidad
                                from (
                                    SELECT
    	                                mov.mes, 
                                        mov.cta,
                                        ROUND(SUM((case mov.cta when 4000 then (-1*mov.monto) else (mov.monto) end)),0) as monto
                                    FROM sc_movpol mov
                                        INNER JOIN cc c ON mov.cc = c.cc
                                        INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
                                    WHERE (mov.year=" + anio + @") and mov.cc in ('" + strCC + @"') and 
                                        (
                                            (mov.cta=4000) or 
                                            (
                                                mov.cta=5000 and not
                                                (
                                                    mov.cta=5000 and mov.scta=18 and mov.sscta=0
                                                ) and not
                                                (
                                                    (mov.concepto='MANTENIMIENTO DE MAQUINARIA' and ((mov.year<2021) or (mov.year=2021 and (mov.mes>=1 and mov.mes<=3))))
                                                )
                                            ) 
                                        )  
                                    group by mov.mes,
                                        mov.cta
                                    ) x
                                group by 
                                    x.cta
                                order by 
                                    x.cta";

                var data = _contextEnkontrol.Select<tendenciasDTO>(EnkontrolAmbienteEnum.ProdCPLAN, consulta);
                data.ForEach(x => x.empresa = "CPLAN");
                data.Where(x => x.cta == 4000).ToList().ForEach(x => { x.descripcion = "INGRESOS"; x.cuenta = "4000"; });
                data.Where(x => x.cta == 5000).ToList().ForEach(x => { x.descripcion = "COSTOS"; x.cuenta = "5000"; });

                listaDatos.AddRange(data);
            }
            if (empresa == 0 || empresa == 2)
            {
                if (ccData.Any(x => x.areaCuenta == "1-9"))
                {
                    string consultaIngresoArrendadora = string.Format(
                                @"select
                                    x.cta,
                                    SUM(CASE WHEN x.mes = 1 THEN x.monto ELSE 0 END) as ene, 
                                    SUM(CASE WHEN x.mes = 2 THEN x.monto ELSE 0 END) as feb, 
                                    SUM(CASE WHEN x.mes = 3 THEN x.monto ELSE 0 END) as mar,  
                                    SUM(CASE WHEN x.mes = 4 THEN x.monto ELSE 0 END) as abr,  
                                    SUM(CASE WHEN x.mes = 5 THEN x.monto ELSE 0 END) as may,  
                                    SUM(CASE WHEN x.mes = 6 THEN x.monto ELSE 0 END) as jun,  
                                    SUM(CASE WHEN x.mes = 7 THEN x.monto ELSE 0 END) as jul,  
                                    SUM(CASE WHEN x.mes = 8 THEN x.monto ELSE 0 END) as ago,  
                                    SUM(CASE WHEN x.mes = 9 THEN x.monto ELSE 0 END) as sep,  
                                    SUM(CASE WHEN x.mes = 10 THEN x.monto ELSE 0 END) as oct, 
                                    SUM(CASE WHEN x.mes = 11 THEN x.monto ELSE 0 END) as nov,  
                                    SUM(CASE WHEN x.mes = 12 THEN x.monto ELSE 0 END) as dic,
                                    ROUND(SUM(x.monto),0) as total,
                                    0 as porcentaje,
                                    (SUM(CASE WHEN x.mes = " + mes + @"-1 THEN x.monto ELSE 0 END) - (SUM(CASE WHEN (x.mes >= " + mes + @"-7 and x.mes < " + mes + @"-1) THEN x.monto ELSE 0 END)/6) ) as variabilidad,
                                    0 as porcentajeVariabilidad
                                from (
                                    SELECT
    	                                mov.mes, 
                                        mov.cta,
                                        ROUND(SUM((case mov.cta when 4000 then (-1*mov.monto) else (mov.monto) end)),0) as monto
                                    FROM sc_movpol mov 
                                        INNER JOIN cc c ON mov.cc = c.cc
                                        INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
                                    WHERE
                                        mov.year = {0} and mov.cta = 4000 and mov.scta = 9 and mov.sscta = 0 and mov.cc = '227' and mov.area = 1 and mov.cuenta_oc = 9
                                    group by mov.mes,
                                        mov.cta
                                    ) x
                                group by 
                                    x.cta
                                order by 
                                    x.cta", anio);

                    var dataIngresoArrendadora = _contextEnkontrol.Select<tendenciasDTO>(EnkontrolAmbienteEnum.ProdARREND, consultaIngresoArrendadora);
                    dataIngresoArrendadora.ForEach(x =>
                        {
                            x.empresa = "ARREND";
                            x.descripcion = "INGRESOS ARREND MLH";
                            x.cuenta = "4000-9-0";
                        });
                    listaDatos.AddRange(dataIngresoArrendadora);
                }

                var lstCC = ccData.Select(x => " ( area=" + x.area + " and cuenta_oc=" + x.cuenta + " ) ").ToList();
                var strCC = string.Join(" or ", lstCC);
                string consulta = @"select
                                    x.cta,
                                    SUM(CASE WHEN x.mes = 1 THEN x.monto ELSE 0 END) as ene, 
                                    SUM(CASE WHEN x.mes = 2 THEN x.monto ELSE 0 END) as feb, 
                                    SUM(CASE WHEN x.mes = 3 THEN x.monto ELSE 0 END) as mar,  
                                    SUM(CASE WHEN x.mes = 4 THEN x.monto ELSE 0 END) as abr,  
                                    SUM(CASE WHEN x.mes = 5 THEN x.monto ELSE 0 END) as may,  
                                    SUM(CASE WHEN x.mes = 6 THEN x.monto ELSE 0 END) as jun,  
                                    SUM(CASE WHEN x.mes = 7 THEN x.monto ELSE 0 END) as jul,  
                                    SUM(CASE WHEN x.mes = 8 THEN x.monto ELSE 0 END) as ago,  
                                    SUM(CASE WHEN x.mes = 9 THEN x.monto ELSE 0 END) as sep,  
                                    SUM(CASE WHEN x.mes = 10 THEN x.monto ELSE 0 END) as oct, 
                                    SUM(CASE WHEN x.mes = 11 THEN x.monto ELSE 0 END) as nov,  
                                    SUM(CASE WHEN x.mes = 12 THEN x.monto ELSE 0 END) as dic,
                                    ROUND(SUM(x.monto),0) as total,
                                    0 as porcentaje,
                                    (SUM(CASE WHEN x.mes = " + mes + @"-1 THEN x.monto ELSE 0 END) - (SUM(CASE WHEN (x.mes >= " + mes + @"-7 and x.mes < " + mes + @"-1) THEN x.monto ELSE 0 END)/6) ) as variabilidad,
                                    0 as porcentajeVariabilidad
                                from (
                                    SELECT
    	                                mov.mes, 
                                        mov.cta,
                                        ROUND(SUM((case mov.cta when 4000 then (-1*mov.monto) else (mov.monto) end)),0) as monto
                                    FROM sc_movpol mov 
                                        INNER JOIN cc c ON mov.cc = c.cc
                                        INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
                                    WHERE (mov.year=" + anio + @") and (" + strCC + @") and 
                                        (
                                            (mov.cta=5000) or
                                            (mov.cta=5901 and mov.scta=6 and mov.sscta=0)
                                        ) 
                                    group by mov.mes,
                                        mov.cta
                                    ) x
                                group by 
                                    x.cta
                                order by 
                                    x.cta";

                var data = _contextEnkontrol.Select<tendenciasDTO>(EnkontrolAmbienteEnum.ProdARREND, consulta);
                data.ForEach(x => x.empresa = "ARREND");
                data.Where(x => x.cta == 5000).ToList().ForEach(x => { x.descripcion = "COSTOS"; x.cuenta = "5000"; });
                data.Where(x => x.cta == 5901).ToList().ForEach(x => { x.descripcion = "BAJAS COMP"; x.cuenta = "5901-6-0"; });
                listaDatos.AddRange(data);
            }
            result.datos = listaDatos.ToList();
            var totalDatos = result.datos.Sum(x => x.total);
            result.datos.ForEach(x => x.porcentaje = Math.Round(((x.total / totalDatos) * 100), 0));
            var totalVariabilidad = result.datos.Sum(x => x.variabilidad);
            result.datos.ForEach(x => x.porcentajeVariabilidad = totalVariabilidad == 0 ? 0 : Math.Round(((x.variabilidad / totalVariabilidad) * 100), 0));
            result.tieneOtros = false;
            var meses = MonthsBetween(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
            var graficaTendencia = new GraficaDTO();
            graficaTendencia.series = new List<SerieDTO>();
            foreach (var i in result.datos)
            {
                var serie = new SerieDTO();
                serie.name = i.descripcion + " - " + i.empresa;
                serie.data = new List<decimal>();
                serie.data.Add(i.ene);
                serie.data.Add(i.feb);
                serie.data.Add(i.mar);
                serie.data.Add(i.abr);
                serie.data.Add(i.may);
                serie.data.Add(i.jun);
                serie.data.Add(i.jul);
                serie.data.Add(i.ago);
                serie.data.Add(i.sep);
                serie.data.Add(i.oct);
                serie.data.Add(i.nov);
                serie.data.Add(i.dic);
                graficaTendencia.series.Add(serie);
            }
            if (empresa == 0)
            {
                var c = result.datos.FirstOrDefault(x => x.cta == 5000 && x.empresa == "CPLAN");
                var a = result.datos.FirstOrDefault(x => x.cta == 5000 && x.empresa == "ARREND");
                var ab = result.datos.FirstOrDefault(x => x.cta == 5901 && x.empresa == "ARREND");
                ab = ab ?? new tendenciasDTO();
                var serie = new SerieDTO();
                serie.name = "COSTO CONSOLIDADO";
                serie.data = new List<decimal>();
                serie.data.Add(c.ene + a.ene + ab.ene);
                serie.data.Add(c.feb + a.feb + ab.feb);
                serie.data.Add(c.mar + a.mar + ab.mar);
                serie.data.Add(c.abr + a.abr + ab.abr);
                serie.data.Add(c.may + a.may + ab.may);
                serie.data.Add(c.jun + a.jun + ab.jun);
                serie.data.Add(c.jul + a.jul + ab.jul);
                serie.data.Add(c.ago + a.ago + ab.ago);
                serie.data.Add(c.sep + a.sep + ab.sep);
                serie.data.Add(c.oct + a.oct + ab.oct);
                serie.data.Add(c.nov + a.nov + ab.nov);
                serie.data.Add(c.dic + a.dic + ab.dic);
                graficaTendencia.series.Add(serie);
            }
            result.grafica_tendencia = graficaTendencia;
            return result;
        }

        public Dictionary<string, object> EnviarCorreoTendenciasIngresosCostos(bool mensual)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var queryConstruplan = new OdbcConsultaDTO();
                queryConstruplan.consulta = string.Format(
                    @"select
                        x.cta,
                        x.cc,
                        sum(case when x.fechapol >= '2022/08/17' and x.fechapol <= '2022/08/23' then x.monto else 0 end) as semana1,
                        sum(case when x.fechapol >= '2022/08/24' and x.fechapol <= '2022/08/30' then x.monto else 0 end) as semana2,
                        sum(case when x.fechapol >= '2022/08/31' and x.fechapol <= '2022/09/06' then x.monto else 0 end) as semana3,
                        sum(case when x.fechapol >= '2022/09/07' and x.fechapol <= '2022/09/13' then x.monto else 0 end) as semana4,
                        sum(case when x.fechapol >= '2022/09/14' and x.fechapol <= '2022/09/20' then x.monto else 0 end) as semana5,
                        sum(case when x.fechapol >= '2022/08/01' and x.fechapol <= '2022/08/31' then x.monto else 0 end) as mesPromedio,
                        sum(case when x.fechapol >= '2022/09/01' and x.fechapol <= '2022/09/20' then x.monto else 0 end) as mesReal
                    from
                        (
                            select
                                pol.fechapol,
                                mov.cta,
                                mov.cc,
                                round(sum((case mov.cta when 4000 then (-1 * mov.monto) else (mov.monto) end)), 0) as monto
                            from
                                sc_movpol as mov
                            inner join
                                sc_polizas as pol
                                on pol.year = mov.year and pol.mes = mov.mes and pol.poliza = mov.poliza and pol.tp = mov.tp
                            inner join
                                cc as c
                                on c.cc = mov.cc
                            inner join
                                catcta as cuenta
                                on cuenta.cta = mov.cta and cuenta.scta = mov.scta and cuenta.sscta = mov.sscta
                            where
                                (mov.year = 2022) and
                                (
                                    (mov.cta = 4000) or
                                    (
                                        mov.cta = 5000 and not
                                        (
                                            mov.cta = 5000 and mov.scta = 18 and mov.sscta = 0
                                        ) and not
                                        (
                                            (mov.concepto = 'MANTENIMIENTO DE MAQUINARIA' and ((mov.year < 2021) or (mov.year = 2021 and (mov.mes >= 1 and mov.mes <= 3))))
                                        )
                                    )
                                ) and
                                pol.fechapol >= '20220801' and pol.fechapol <= '20220920' and
                                mov.cc in ('001','002','007','008','016','017','018','043','044','C60','C65','C68','C69','C70','C71','C72','559','015','010','1015','025','023','146','162','C73','999','027','028','019','1018','1010','163','164','006','166','029','C58','012','004','170','556','A04','074','C79','013','236','0','977','A63','A64','174','C89','997','C92','180','C93','182','D05','232','031','200','191','C94','C95','183','C96','A68','D06','291','C98','C11','A70','C36','186','A71','C37','C12','C38','188','C39','0','032','189','C40','187','190','C13')
                            group by
                                pol.fechapol, mov.cta, mov.cc
                        ) x
                    group by
                       x.cta, x.cc");
                var dataConstruplan = _contextEnkontrol.Select<TendenciaSemanalDTO>(EnkontrolAmbienteEnum.ProdCPLAN, queryConstruplan);

                var queryArrendadora = new OdbcConsultaDTO();
                queryArrendadora.consulta = string.Format(
                    @"select
                        x.cta,
                        x.cc,
                        sum(case when x.fechapol >= '2022/08/17' and x.fechapol <= '2022/08/23' then x.monto else 0 end) as semana1,
                        sum(case when x.fechapol >= '2022/08/24' and x.fechapol <= '2022/08/30' then x.monto else 0 end) as semana2,
                        sum(case when x.fechapol >= '2022/08/31' and x.fechapol <= '2022/09/06' then x.monto else 0 end) as semana3,
                        sum(case when x.fechapol >= '2022/09/07' and x.fechapol <= '2022/09/13' then x.monto else 0 end) as semana4,
                        sum(case when x.fechapol >= '2022/09/14' and x.fechapol <= '2022/09/20' then x.monto else 0 end) as semana5,
                        sum(case when x.fechapol >= '2022/08/01' and x.fechapol <= '2022/08/31' then x.monto else 0 end) as mesPromedio,
                        sum(case when x.fechapol >= '2022/09/01' and x.fechapol <= '2022/09/20' then x.monto else 0 end) as mesReal
                    from
                        (
                            select
                                pol.fechapol,
                                mov.cta,
                                (cast(mov.area as varchar(50)) + '-' + cast(mov.cuenta_oc as varchar(50))) as cc,
                                round(sum((case mov.cta when 4000 then (-1 * mov.monto) else (mov.monto) end)), 0) as monto
                            from
                                sc_movpol as mov
                            inner join
                                sc_polizas as pol
                                on pol.year = mov.year and pol.mes = mov.mes and pol.poliza = mov.poliza and pol.tp = mov.tp
                            inner join
                                cc as c
                                on c.cc = mov.cc
                            inner join
                                catcta as cuenta
                                on cuenta.cta = mov.cta and cuenta.scta = mov.scta and cuenta.sscta = mov.sscta
                            where
                                (mov.year = 2022) and
                                (
                                    (mov.cta = 5000) or
                                    (mov.cta = 5901 and mov.scta = 6 and mov.sscta = 0)
                                ) and
                                pol.fechapol >= '20220801' and pol.fechapol <= '20220920' and
                                (( area=9 and cuenta_oc=1 )  or  ( area=9 and cuenta_oc=2 )  or  ( area=9 and cuenta_oc=4 )  or  ( area=9 and cuenta_oc=5 )  or  ( area=9 and cuenta_oc=7 )  or  ( area=9 and cuenta_oc=8 )  or  ( area=9 and cuenta_oc=9 )  or  ( area=10 and cuenta_oc=2 )  or  ( area=10 and cuenta_oc=3 )  or  ( area=10 and cuenta_oc=6 )  or  ( area=10 and cuenta_oc=7 )  or  ( area=10 and cuenta_oc=8 )  or  ( area=10 and cuenta_oc=9 )  or  ( area=10 and cuenta_oc=10 )  or  ( area=10 and cuenta_oc=11 )  or  ( area=11 and cuenta_oc=1 )  or  ( area=11 and cuenta_oc=2 )  or  ( area=9 and cuenta_oc=11 )  or  ( area=9 and cuenta_oc=12 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=6 and cuenta_oc=5 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=2 and cuenta_oc=1 )  or  ( area=3 and cuenta_oc=1 )  or  ( area=10 and cuenta_oc=12 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=9 and cuenta_oc=19 )  or  ( area=9 and cuenta_oc=22 )  or  ( area=9 and cuenta_oc=17 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=4 and cuenta_oc=2 )  or  ( area=4 and cuenta_oc=3 )  or  ( area=9 and cuenta_oc=23 )  or  ( area=1 and cuenta_oc=9 )  or  ( area=9 and cuenta_oc=26 )  or  ( area=9 and cuenta_oc=28 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=1 and cuenta_oc=10 )  or  ( area=11 and cuenta_oc=9 )  or  ( area=9 and cuenta_oc=29 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=15 and cuenta_oc=6 )  or  ( area=9 and cuenta_oc=27 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=5 and cuenta_oc=5 )  or  ( area=0 and cuenta_oc=0 )  or  ( area=14 and cuenta_oc=1 )  or  ( area=10 and cuenta_oc=35 )  or  ( area=5 and cuenta_oc=6 )  or  ( area=10 and cuenta_oc=36 )  or  ( area=5 and cuenta_oc=7 )  or  ( area=17 and cuenta_oc=1 )  or  ( area=9 and cuenta_oc=32 )  or  ( area=9 and cuenta_oc=31 )  or  ( area=14 and cuenta_oc=1 )  or  ( area=16 and cuenta_oc=2 )  or  ( area=10 and cuenta_oc=37 )  or  ( area=10 and cuenta_oc=38 )  or  ( area=18 and cuenta_oc=1 )  or  ( area=11 and cuenta_oc=15 )  or  ( area=19 and cuenta_oc=1 )  or  ( area=10 and cuenta_oc=39 )  or  ( area=99 and cuenta_oc=1 )  or  ( area=10 and cuenta_oc=40 )  or  ( area=10 and cuenta_oc=41 )  or  ( area=11 and cuenta_oc=17 )  or  ( area=11 and cuenta_oc=19 )  or  ( area=3 and cuenta_oc=2 )  or  ( area=11 and cuenta_oc=20 )  or  ( area=11 and cuenta_oc=21 )  or  ( area=10 and cuenta_oc=42 )  or  ( area=11 and cuenta_oc=22 )  or  ( area=1 and cuenta_oc=13 )  or  ( area=11 and cuenta_oc=23 )  or  ( area=14 and cuenta_oc=2 )  or  ( area=30 and cuenta_oc=1 )  or  ( area=18 and cuenta_oc=2 )  or  ( area=11 and cuenta_oc=24 )  or  ( area=7 and cuenta_oc=17 )  or  ( area=20 and cuenta_oc=3 )  or  ( area=0 and cuenta_oc=0 ))
                            group by
                                pol.fechapol, mov.cta, mov.cc, mov.area, mov.cuenta_oc
                        ) x
                    group by
                       x.cta, x.cc");
                var dataArrendadora = _contextEnkontrol.Select<TendenciaSemanalDTO>(EnkontrolAmbienteEnum.ProdARREND, queryArrendadora);

                var queryArrendadoraHerradura = new OdbcConsultaDTO();
                queryArrendadoraHerradura.consulta = string.Format(
                    @"select
                        x.cta,
                        x.cc,
                        sum(case when x.fechapol >= '2022/08/17' and x.fechapol <= '2022/08/23' then x.monto else 0 end) as semana1,
                        sum(case when x.fechapol >= '2022/08/24' and x.fechapol <= '2022/08/30' then x.monto else 0 end) as semana2,
                        sum(case when x.fechapol >= '2022/08/31' and x.fechapol <= '2022/09/06' then x.monto else 0 end) as semana3,
                        sum(case when x.fechapol >= '2022/09/07' and x.fechapol <= '2022/09/13' then x.monto else 0 end) as semana4,
                        sum(case when x.fechapol >= '2022/09/14' and x.fechapol <= '2022/09/20' then x.monto else 0 end) as semana5,
                        sum(case when x.fechapol >= '2022/08/01' and x.fechapol <= '2022/08/31' then x.monto else 0 end) as mesPromedio,
                        sum(case when x.fechapol >= '2022/09/01' and x.fechapol <= '2022/09/20' then x.monto else 0 end) as mesReal
                    from
                        (
                            select
                                pol.fechapol,
                                mov.cta,
                                (cast(mov.area as varchar(50)) + '-' + cast(mov.cuenta_oc as varchar(50))) as cc,
                                round(sum((case mov.cta when 4000 then (-1 * mov.monto) else (mov.monto) end)), 0) as monto
                            from
                                sc_movpol as mov
                            inner join
                                sc_polizas as pol
                                on pol.year = mov.year and pol.mes = mov.mes and pol.poliza = mov.poliza and pol.tp = mov.tp
                            inner join
                                cc as c
                                on c.cc = mov.cc
                            inner join
                                catcta as cuenta
                                on cuenta.cta = mov.cta and cuenta.scta = mov.scta and cuenta.sscta = mov.sscta
                            where
                                (mov.year = 2022) and
                                (
                                    mov.cta = 4000 and mov.scta = 9 and mov.sscta = 0 and mov.cc = '227'
                                ) and
                                pol.fechapol >= '20220801' and pol.fechapol <= '20220920' and
                                (mov.area = 1 and mov.cuenta_oc = 9)
                            group by
                                pol.fechapol, mov.cta, mov.cc, mov.area, mov.cuenta_oc
                        ) x
                    group by
                       x.cta, x.cc");
                var dataArrendadoraHerradura = _contextEnkontrol.Select<TendenciaSemanalDTO>(EnkontrolAmbienteEnum.ProdARREND, queryArrendadoraHerradura);

                var tablaConstruplanIngresos = new List<TendenciaSemanalTablaDTO>();
                var tablaConstruplanCostos = new List<TendenciaSemanalTablaDTO>();
                var tablaArrendadoraCostos = new List<TendenciaSemanalTablaDTO>();

                var ingresoPromedioCplan = new TendenciaSemanalTablaDTO();
                ingresoPromedioCplan.concepto = "ingresos promedios";
                ingresoPromedioCplan.monto = dataConstruplan.Where(x => x.cta == 4000).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                ingresoPromedioCplan.montoMensual = dataConstruplan.Where(x => x.cta == 4000).Sum(x => x.mesPromedio);
                tablaConstruplanIngresos.Add(ingresoPromedioCplan);

                var ingresoRealCplan = new TendenciaSemanalTablaDTO();
                ingresoRealCplan.concepto = "ingresos reales";
                ingresoRealCplan.monto = dataConstruplan.Where(x => x.cta == 4000).Sum(x => x.semana5);
                ingresoRealCplan.montoMensual = dataConstruplan.Where(x => x.cta == 4000).Sum(x => x.mesReal);
                tablaConstruplanIngresos.Add(ingresoRealCplan);

                var diferenciaIngresoCplan = new TendenciaSemanalTablaDTO();
                diferenciaIngresoCplan.concepto = "diferencia";
                diferenciaIngresoCplan.monto = ingresoRealCplan.monto - ingresoPromedioCplan.monto;
                diferenciaIngresoCplan.montoMensual = ingresoRealCplan.montoMensual - ingresoPromedioCplan.montoMensual;
                tablaConstruplanIngresos.Add(diferenciaIngresoCplan);

                var costosPromedioCplan = new TendenciaSemanalTablaDTO();
                costosPromedioCplan.concepto = "costos promedios";
                costosPromedioCplan.monto = dataConstruplan.Where(x => x.cta == 5000).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                costosPromedioCplan.montoMensual = dataConstruplan.Where(x => x.cta == 5000).Sum(x => x.mesPromedio);
                tablaConstruplanCostos.Add(costosPromedioCplan);

                var costosRealCplan = new TendenciaSemanalTablaDTO();
                costosRealCplan.concepto = "costos reales";
                costosRealCplan.monto = dataConstruplan.Where(x => x.cta == 5000).Sum(x => x.semana5);
                costosRealCplan.montoMensual = dataConstruplan.Where(x => x.cta == 5000).Sum(x => x.mesReal);
                tablaConstruplanCostos.Add(costosRealCplan);

                var diferenciaCostosCplan = new TendenciaSemanalTablaDTO();
                diferenciaCostosCplan.concepto = "diferencia";
                diferenciaCostosCplan.monto = costosPromedioCplan.monto - costosRealCplan.monto;
                diferenciaCostosCplan.montoMensual = costosPromedioCplan.montoMensual - costosRealCplan.montoMensual;
                tablaConstruplanCostos.Add(diferenciaCostosCplan);

                var costosPromedioArre = new TendenciaSemanalTablaDTO();
                costosPromedioArre.concepto = "costos promedios";
                costosPromedioArre.monto = dataArrendadora.Where(x => x.cta == 5000).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                costosPromedioArre.montoMensual = dataArrendadora.Where(x => x.cta == 5000).Sum(x => x.mesPromedio);
                tablaArrendadoraCostos.Add(costosPromedioArre);

                var costosRealArre = new TendenciaSemanalTablaDTO();
                costosRealArre.concepto = "costos reales";
                costosRealArre.monto = dataArrendadora.Where(x => x.cta == 5000).Sum(x => x.semana5);
                costosRealArre.montoMensual = dataArrendadora.Where(x => x.cta == 5000).Sum(x => x.mesReal);
                tablaArrendadoraCostos.Add(costosRealArre);

                var diferenciaArre = new TendenciaSemanalTablaDTO();
                diferenciaArre.concepto = "diferencia";
                diferenciaArre.monto = costosPromedioArre.monto - costosRealArre.monto;
                diferenciaArre.montoMensual = costosPromedioArre.montoMensual - costosRealArre.montoMensual;
                tablaArrendadoraCostos.Add(diferenciaArre);

                string tblHtmlIngresosCplan = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                foreach (var item in tablaConstruplanIngresos)
                {
                    tblHtmlIngresosCplan += "<tr><td>" + item.concepto + "</td>";
                    if (mensual)
                    {
                        tblHtmlIngresosCplan += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                    }
                    else
                    {
                        tblHtmlIngresosCplan += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                    }
                    tblHtmlIngresosCplan += "</tr>";
                }
                tblHtmlIngresosCplan += "</tbody></table>";

                string tblHtmlCostoCplan = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                foreach (var item in tablaConstruplanCostos)
                {
                    tblHtmlCostoCplan += "<tr><td>" + item.concepto + "</td>";
                    if (mensual)
                    {
                        tblHtmlCostoCplan += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                    }
                    else
                    {
                        tblHtmlCostoCplan += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                    }
                    tblHtmlCostoCplan += "</tr>";
                }
                tblHtmlCostoCplan += "</tbody></table>";

                string tblHtmlCostoArre = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                foreach (var item in tablaArrendadoraCostos)
                {
                    tblHtmlCostoArre += "<tr><td>" + item.concepto + "</td>";
                    if (mensual)
                    {
                        tblHtmlCostoArre += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                    }
                    else
                    {
                        tblHtmlCostoArre += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                    }
                    tblHtmlCostoArre += "</tr>";
                }
                tblHtmlCostoArre += "</tbody></table>";

                if (mensual)
                {
                    GlobalUtils.sendEmail(
                    string.Format("{0}: Tendencia de Ingresos / Costos Mensual del 01/07/2022 al 31/07/2022", PersonalUtilities.GetNombreEmpresa()),
                    "<p>Buen día</p>" +
                    "<p>A continuación se muestran datos generales de costos e ingresos a nivel empresa del 01/07/2022 al 31/07/2022.</p>" +
                    "<strong>CONSTRUPLAN</strong><p>" + tblHtmlIngresosCplan + "</p><br><p>" + tblHtmlCostoCplan + "</p><br>" +
                    "<strong>ARRENDADORA</strong><p>" + tblHtmlCostoArre + "<p>" +
                    "<p>Este es un correo autogenerado por la plataforma SIGOPLAN, favor de no responderlo.</p>" +
                    "Saludos",
                        new List<string> { "jose.gaytan@construplan.com.mx", "martin.valle@construplan.com.mx", "g.reina@construplan.com.mx" });
                    //new List<string> { "martin.zayas@construplan.com.mx", "martin.valle@construplan.com.mx" });
                }
                else
                {
                    GlobalUtils.sendEmail(
                    string.Format("{0}: Tendencia de Ingresos / Costos Semana del 14/09/2022 al 20/09/2022", PersonalUtilities.GetNombreEmpresa()),
                    "<p>Buen día</p>" +
                    "<p>A continuación se muestran datos generales de costos e ingresos a nivel empresa del 14/09/2022 al 20/09/2022.</p>" +
                    "<strong>CONSTRUPLAN</strong><p>" + tblHtmlIngresosCplan + "</p><br><p>" + tblHtmlCostoCplan + "</p><br>" +
                    "<strong>ARRENDADORA</strong><p>" + tblHtmlCostoArre + "<p>" +
                    "<p>Este es un correo autogenerado por la plataforma SIGOPLAN, favor de no responderlo.</p>" +
                    "Saludos",
                        new List<string> { "jose.gaytan@construplan.com.mx", "martin.valle@construplan.com.mx", "g.reina@construplan.com.mx" });
                    //new List<string> { "martin.zayas@construplan.com.mx", "martin.valle@construplan.com.mx" });
                }

                var minadosAC = new List<string> { "2-1", "3-1", "4-3", "1-9", "1-10", "5-6", "18-1", "18-2" };
                var minadosCC = new List<string> { "146", "162", "164", "166", "170", "180", "183", "189" };

                for (int i = 0; i < minadosAC.Count; i++)
                {
                    var ac = minadosAC[i];
                    var _cc = _context.tblP_CC.FirstOrDefault(x => ac == x.areaCuenta);
                    var ccDesc = "";
                    if (_cc != null)
                    {
                        ccDesc = _cc.descripcion.Trim().ToUpper();
                    }

                    var tablaConstruplanIngresosCC = new List<TendenciaSemanalTablaDTO>();
                    var tablaConstruplanCostosCC = new List<TendenciaSemanalTablaDTO>();
                    var tablaArrendadoraIngresosAC = new List<TendenciaSemanalTablaDTO>();
                    var tablaArrendadoraCostosAC = new List<TendenciaSemanalTablaDTO>();

                    var ingresoPromedioCplanCC = new TendenciaSemanalTablaDTO();
                    ingresoPromedioCplanCC.concepto = "ingresos promedios";
                    ingresoPromedioCplanCC.monto = dataConstruplan.Where(x => x.cta == 4000 && x.cc == minadosCC[i]).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                    ingresoPromedioCplanCC.montoMensual = dataConstruplan.Where(x => x.cta == 4000 && x.cc == minadosCC[i]).Sum(x => x.mesPromedio);
                    tablaConstruplanIngresosCC.Add(ingresoPromedioCplanCC);

                    var ingresoRealCplanCC = new TendenciaSemanalTablaDTO();
                    ingresoRealCplanCC.concepto = "ingresos reales";
                    ingresoRealCplanCC.monto = dataConstruplan.Where(x => x.cta == 4000 && x.cc == minadosCC[i]).Sum(x => x.semana5);
                    ingresoRealCplanCC.montoMensual = dataConstruplan.Where(x => x.cta == 4000 && x.cc == minadosCC[i]).Sum(x => x.mesReal);
                    tablaConstruplanIngresosCC.Add(ingresoRealCplanCC);

                    var diferenciaIngresoCplanCC = new TendenciaSemanalTablaDTO();
                    diferenciaIngresoCplanCC.concepto = "diferencia";
                    diferenciaIngresoCplanCC.monto = ingresoRealCplanCC.monto - ingresoPromedioCplanCC.monto;
                    diferenciaIngresoCplanCC.montoMensual = ingresoRealCplanCC.montoMensual - ingresoPromedioCplanCC.montoMensual;
                    tablaConstruplanIngresosCC.Add(diferenciaIngresoCplanCC);

                    var costosPromedioCplanCC = new TendenciaSemanalTablaDTO();
                    costosPromedioCplanCC.concepto = "costos promedios";
                    costosPromedioCplanCC.monto = dataConstruplan.Where(x => x.cta == 5000 && x.cc == minadosCC[i]).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                    costosPromedioCplanCC.montoMensual = dataConstruplan.Where(x => x.cta == 5000 && x.cc == minadosCC[i]).Sum(x => x.mesPromedio);
                    tablaConstruplanCostosCC.Add(costosPromedioCplanCC);

                    var costosRealCplanCC = new TendenciaSemanalTablaDTO();
                    costosRealCplanCC.concepto = "costos reales";
                    costosRealCplanCC.monto = dataConstruplan.Where(x => x.cta == 5000 && x.cc == minadosCC[i]).Sum(x => x.semana5);
                    costosRealCplanCC.montoMensual = dataConstruplan.Where(x => x.cta == 5000 && x.cc == minadosCC[i]).Sum(x => x.mesReal);
                    tablaConstruplanCostosCC.Add(costosRealCplanCC);

                    var diferenciaCostosCplanCC = new TendenciaSemanalTablaDTO();
                    diferenciaCostosCplanCC.concepto = "diferencia";
                    diferenciaCostosCplanCC.monto = costosPromedioCplanCC.monto - costosRealCplanCC.monto;
                    diferenciaCostosCplanCC.montoMensual = costosPromedioCplanCC.montoMensual - costosRealCplanCC.montoMensual;
                    tablaConstruplanCostosCC.Add(diferenciaCostosCplanCC);

                    string tblHtmlIngresosArreAC = "";
                    if (ac == "1-9")
                    {
                        var ingresoPromedioArreAC = new TendenciaSemanalTablaDTO();
                        ingresoPromedioArreAC.concepto = "ingresos promedios";
                        ingresoPromedioArreAC.monto = dataArrendadoraHerradura.Where(x => x.cta == 4000).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                        ingresoPromedioArreAC.montoMensual = dataArrendadoraHerradura.Where(x => x.cta == 4000).Sum(x => x.mesPromedio);
                        tablaArrendadoraIngresosAC.Add(ingresoPromedioArreAC);

                        var ingresosRealArreAC = new TendenciaSemanalTablaDTO();
                        ingresosRealArreAC.concepto = "ingresos reales";
                        ingresosRealArreAC.monto = dataArrendadoraHerradura.Where(x => x.cta == 4000).Sum(x => x.semana5);
                        ingresosRealArreAC.montoMensual = dataArrendadoraHerradura.Where(x => x.cta == 4000).Sum(x => x.mesReal);
                        tablaArrendadoraIngresosAC.Add(ingresosRealArreAC);

                        var diferenciaIngresoArreAC = new TendenciaSemanalTablaDTO();
                        diferenciaIngresoArreAC.concepto = "diferencia";
                        diferenciaIngresoArreAC.monto = ingresosRealArreAC.monto - ingresoPromedioArreAC.monto;
                        diferenciaIngresoArreAC.montoMensual = ingresosRealArreAC.montoMensual - ingresoPromedioArreAC.montoMensual;
                        tablaArrendadoraIngresosAC.Add(diferenciaIngresoArreAC);

                        tblHtmlIngresosArreAC = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                        foreach (var item in tablaArrendadoraIngresosAC)
                        {
                            tblHtmlIngresosArreAC += "<tr><td>" + item.concepto + "</td>";
                            if (mensual)
                            {
                                tblHtmlIngresosArreAC += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                            }
                            else
                            {
                                tblHtmlIngresosArreAC += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                            }

                            tblHtmlIngresosArreAC += "</tr>";
                        }
                        tblHtmlIngresosArreAC += "</tbody></table>";
                    }

                    var costosPromedioArreAC = new TendenciaSemanalTablaDTO();
                    costosPromedioArreAC.concepto = "costos promedios";
                    costosPromedioArreAC.monto = dataArrendadora.Where(x => x.cta == 5000 && x.cc == minadosAC[i]).Sum(x => x.semana1 + x.semana2 + x.semana3 + x.semana4) / 4;
                    costosPromedioArreAC.montoMensual = dataArrendadora.Where(x => x.cta == 5000 && x.cc == minadosAC[i]).Sum(x => x.mesPromedio);
                    tablaArrendadoraCostosAC.Add(costosPromedioArreAC);

                    var costosRealArreAC = new TendenciaSemanalTablaDTO();
                    costosRealArreAC.concepto = "costos reales";
                    costosRealArreAC.monto = dataArrendadora.Where(x => x.cta == 5000 && x.cc == minadosAC[i]).Sum(x => x.semana5);
                    costosRealArreAC.montoMensual = dataArrendadora.Where(x => x.cta == 5000 && x.cc == minadosAC[i]).Sum(x => x.mesReal);
                    tablaArrendadoraCostosAC.Add(costosRealArreAC);

                    var diferenciaArreAC = new TendenciaSemanalTablaDTO();
                    diferenciaArreAC.concepto = "diferencia";
                    diferenciaArreAC.monto = costosPromedioArreAC.monto - costosRealArreAC.monto;
                    diferenciaArreAC.montoMensual = costosPromedioArreAC.montoMensual - costosRealArreAC.montoMensual;
                    tablaArrendadoraCostosAC.Add(diferenciaArreAC);

                    string tblHtmlIngresosCplanCC = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                    foreach (var item in tablaConstruplanIngresosCC)
                    {
                        tblHtmlIngresosCplanCC += "<tr><td>" + item.concepto + "</td>";
                        if (mensual)
                        {
                            tblHtmlIngresosCplanCC += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                        }
                        else
                        {
                            tblHtmlIngresosCplanCC += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                        }
                        tblHtmlIngresosCplanCC += "</tr>";
                    }
                    tblHtmlIngresosCplanCC += "</tbody></table>";

                    string tblHtmlCostoCplanCC = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                    foreach (var item in tablaConstruplanCostosCC)
                    {
                        tblHtmlCostoCplanCC += "<tr><td>" + item.concepto + "</td>";
                        if (mensual)
                        {
                            tblHtmlCostoCplanCC += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                        }
                        else
                        {
                            tblHtmlCostoCplanCC += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                        }
                        tblHtmlCostoCplanCC += "</tr>";
                    }
                    tblHtmlCostoCplanCC += "</tbody></table>";

                    string tblHtmlCostoArreAC = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" width=""256"" style=""border-collapse:collapse;width:192pt""><tbody>";
                    foreach (var item in tablaArrendadoraCostosAC)
                    {
                        tblHtmlCostoArreAC += "<tr><td>" + item.concepto + "</td>";
                        if (mensual)
                        {
                            tblHtmlCostoArreAC += "<td" + (item.montoMensual < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.montoMensual) + "</td>";
                        }
                        else
                        {
                            tblHtmlCostoArreAC += "<td" + (item.monto < 0 ? @" style=""color:red;""" : "") + "> $ " + String.Format("{0:n}", item.monto) + "</td>";
                        }
                        tblHtmlCostoArreAC += "</tr>";
                    }
                    tblHtmlCostoArreAC += "</tbody></table>";

                    if (mensual)
                    {
                        if (ac == "1-9")
                        {
                            GlobalUtils.sendEmail(
                            string.Format("{0}: Tendencia de Ingresos / Costos Mensual del 01/07/2022 al 31/07/2022", PersonalUtilities.GetNombreEmpresa()),
                            "<p>Buen día</p>" +
                            "<p>A continuación se muestran datos generales de costos e ingresos de " + ccDesc + " del 01/07/2022 al 31/07/2022.</p>" +
                            "<strong>CONSTRUPLAN</strong><p>" + tblHtmlIngresosCplanCC + "</p><br><p>" + tblHtmlCostoCplanCC + "</p><br>" +
                            "<strong>ARRENDADORA</strong><p>" + tblHtmlIngresosArreAC + " </p><br><p>" + tblHtmlCostoArreAC + "<p>" +
                            "<p>Este es un correo autogenerado por la plataforma SIGOPLAN, favor de no responderlo.</p>" +
                            "Saludos",
                                new List<string> { "jose.gaytan@construplan.com.mx", "martin.valle@construplan.com.mx", "g.reina@construplan.com.mx" });
                            //new List<string> { "martin.zayas@construplan.com.mx", "martin.valle@construplan.com.mx" });
                        }
                        else
                        {
                            GlobalUtils.sendEmail(
                            string.Format("{0}: Tendencia de Ingresos / Costos Mensual del 01/07/2022 al 31/07/2022", PersonalUtilities.GetNombreEmpresa()),
                            "<p>Buen día</p>" +
                            "<p>A continuación se muestran datos generales de costos e ingresos de " + ccDesc + " del 01/07/2022 al 31/07/2022.</p>" +
                            "<strong>CONSTRUPLAN</strong><p>" + tblHtmlIngresosCplanCC + "</p><br><p>" + tblHtmlCostoCplanCC + "</p><br>" +
                            "<strong>ARRENDADORA</strong><p>" + tblHtmlCostoArreAC + "<p>" +
                            "<p>Este es un correo autogenerado por la plataforma SIGOPLAN, favor de no responderlo.</p>" +
                            "Saludos",
                                new List<string> { "jose.gaytan@construplan.com.mx", "martin.valle@construplan.com.mx", "g.reina@construplan.com.mx" });
                            //new List<string> { "martin.zayas@construplan.com.mx", "martin.valle@construplan.com.mx" });
                        }
                    }
                    else
                    {
                        if (ac == "1-9")
                        {
                            GlobalUtils.sendEmail(
                            string.Format("{0}: Tendencia de Ingresos / Costos Semana del 14/09/2022 al 20/09/2022", PersonalUtilities.GetNombreEmpresa()),
                            "<p>Buen día</p>" +
                            "<p>A continuación se muestran datos generales de costos e ingresos de " + ccDesc + " del 14/09/2022 al 20/09/2022.</p>" +
                            "<strong>CONSTRUPLAN</strong><p>" + tblHtmlIngresosCplanCC + "</p><br><p>" + tblHtmlCostoCplanCC + "</p><br>" +
                            "<strong>ARRENDADORA</strong><p>" + tblHtmlIngresosArreAC + " </p><br><p>" + tblHtmlCostoArreAC + "<p>" +
                            "<p>Este es un correo autogenerado por la plataforma SIGOPLAN, favor de no responderlo.</p>" +
                            "Saludos",
                                new List<string> { "jose.gaytan@construplan.com.mx", "martin.valle@construplan.com.mx", "g.reina@construplan.com.mx" });
                            //new List<string> { "martin.zayas@construplan.com.mx", "martin.valle@construplan.com.mx" });
                        }
                        else
                        {
                            GlobalUtils.sendEmail(
                            string.Format("{0}: Tendencia de Ingresos / Costos Semana del 14/09/2022 al 20/09/2022", PersonalUtilities.GetNombreEmpresa()),
                            "<p>Buen día</p>" +
                            "<p>A continuación se muestran datos generales de costos e ingresos de " + ccDesc + " del 14/09/2022 al 20/09/2022.</p>" +
                            "<strong>CONSTRUPLAN</strong><p>" + tblHtmlIngresosCplanCC + "</p><br><p>" + tblHtmlCostoCplanCC + "</p><br>" +
                            "<strong>ARRENDADORA</strong><p>" + tblHtmlCostoArreAC + "<p>" +
                            "<p>Este es un correo autogenerado por la plataforma SIGOPLAN, favor de no responderlo.</p>" +
                            "Saludos",
                                new List<string> { "jose.gaytan@construplan.com.mx", "martin.valle@construplan.com.mx", "g.reina@construplan.com.mx" });
                            //new List<string> { "martin.zayas@construplan.com.mx", "martin.valle@construplan.com.mx" });
                        }
                    }
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return resultado;
        }

        public tendenciaGeneralDTO getDatosGenerales(int empresa, int anio, string cc, int grupo, int modelo)
        {
            var result = new tendenciaGeneralDTO();
            result.esMultiCuenta = false;
            var ccData = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == cc);
            string consulta = @"select 
                                    x.tipo,
                                    x.cta,
                                    x.scta,
                                    x.sscta,
                                    x.descripcion,
                                    SUM(CASE WHEN x.mes = 1 THEN x.monto ELSE 0 END) as ene, 
                                    SUM(CASE WHEN x.mes = 2 THEN x.monto ELSE 0 END) as feb, 
                                    SUM(CASE WHEN x.mes = 3 THEN x.monto ELSE 0 END) as mar,  
                                    SUM(CASE WHEN x.mes = 4 THEN x.monto ELSE 0 END) as abr,  
                                    SUM(CASE WHEN x.mes = 5 THEN x.monto ELSE 0 END) as may,  
                                    SUM(CASE WHEN x.mes = 6 THEN x.monto ELSE 0 END) as jun,  
                                    SUM(CASE WHEN x.mes = 7 THEN x.monto ELSE 0 END) as jul,  
                                    SUM(CASE WHEN x.mes = 8 THEN x.monto ELSE 0 END) as ago,  
                                    SUM(CASE WHEN x.mes = 9 THEN x.monto ELSE 0 END) as sep,  
                                    SUM(CASE WHEN x.mes = 10 THEN x.monto ELSE 0 END) as oct, 
                                    SUM(CASE WHEN x.mes = 11 THEN x.monto ELSE 0 END) as nov,  
                                    SUM(CASE WHEN x.mes = 12 THEN x.monto ELSE 0 END) as dic,
                                    SUM(x.monto) as total,
                                    0 as porcentaje,
                                    (SUM(CASE WHEN x.mes = (SELECT MONTH(GETDATE())-1) THEN x.monto ELSE 0 END) - AVG(CASE WHEN x.mes <= (SELECT MONTH(GETDATE())-1) THEN x.monto ELSE 0 END)) as variabilidad,
                                    0 as porcentajeVariabilidad
                                from (
                                    SELECT
    	                                pol.mes, 
                                        mov.cta,
                                        mov.scta,
                                        mov.sscta,
                                        b.descripcion,
                                        (case when s.id is null then 1 else 2 end) tipo,
                                        ROUND(SUM(mov.monto),0) as monto
                                    FROM sc_polizas pol
                                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                                        INNER JOIN cc c ON mov.cc = c.cc
                                        INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
                                        LEFT JOIN CPLAN_TENDENCIAS s ON s.economico = c.descripcion
                                    WHERE (YEAR(fechapol)=" + anio + @") and area=" + ccData.area + @" and cuenta_oc=" + ccData.cuenta + @" and (mov.cta=4000 or mov.cta=5000 or (mov.cta=5901 and mov.scta=6 and mov.sscta=0)) and not exists(select * from CPLAN_TENDENCIAS as a where a.economico=c.descripcion and (a.excluir=1)) and (mov.cta!=5000 and mov.scta!=8 and mov.sscta!=11) 
                                    group by pol.mes,
                                        mov.cta,
                                        mov.scta,
                                        mov.sscta,
                                        b.descripcion,
                                        tipo
                                    ) x
                                group by 
                                    x.tipo,
                                    x.cta,
                                    x.scta,
                                    x.sscta,
                                    x.descripcion
                                order by 
                                    x.tipo,
                                    x.cta,
                                    x.scta,
                                    x.sscta,
                                    x.descripcion";

            var data = _contextEnkontrol.Select<tendenciasDTO>(EnkontrolAmbienteEnum.Prod, consulta);

            result.datos = data.Where(x => x.tipo == 1).ToList();
            var totalDatos = result.datos.Sum(x => x.total);
            result.datos.ForEach(x => x.porcentaje = Math.Round(((x.total / totalDatos) * 100), 2));

            result.tieneOtros = data.Any(x => x.tipo == 2);
            if (result.tieneOtros)
            {
                result.datosOtros = data.Where(x => x.tipo == 2).ToList();
                var totalOtros = result.datos.Sum(x => x.total);
                result.datosOtros.ForEach(x => x.porcentaje = Math.Round(((x.total / totalOtros) * 100), 2));

                result.datosConcentrado = new List<tendenciasDTO>();
                var principales = new tendenciasDTO()
                {
                    descripcion = "COSTOS FLOTA PRINCIPAL",
                    ene = result.datos.Sum(x => x.ene),
                    feb = result.datos.Sum(x => x.feb),
                    mar = result.datos.Sum(x => x.mar),
                    abr = result.datos.Sum(x => x.abr),
                    may = result.datos.Sum(x => x.may),
                    jun = result.datos.Sum(x => x.jun),
                    jul = result.datos.Sum(x => x.jul),
                    ago = result.datos.Sum(x => x.ago),
                    sep = result.datos.Sum(x => x.sep),
                    oct = result.datos.Sum(x => x.oct),
                    nov = result.datos.Sum(x => x.nov),
                    dic = result.datos.Sum(x => x.dic),
                    total = result.datos.Sum(x => x.total)
                };
                result.datosConcentrado.Add(principales);
                var otros = new tendenciasDTO()
                {
                    descripcion = "COSTOS FLOTA OTROS",
                    ene = result.datosOtros.Sum(x => x.ene),
                    feb = result.datosOtros.Sum(x => x.feb),
                    mar = result.datosOtros.Sum(x => x.mar),
                    abr = result.datosOtros.Sum(x => x.abr),
                    may = result.datosOtros.Sum(x => x.may),
                    jun = result.datosOtros.Sum(x => x.jun),
                    jul = result.datosOtros.Sum(x => x.jul),
                    ago = result.datosOtros.Sum(x => x.ago),
                    sep = result.datosOtros.Sum(x => x.sep),
                    oct = result.datosOtros.Sum(x => x.oct),
                    nov = result.datosOtros.Sum(x => x.nov),
                    dic = result.datosOtros.Sum(x => x.dic),
                    total = result.datosOtros.Sum(x => x.total)
                };
                result.datosConcentrado.Add(otros);
                var totales = new tendenciasDTO()
                {
                    descripcion = "COSTOS TOTALES",
                    ene = result.datosConcentrado.Sum(x => x.ene),
                    feb = result.datosConcentrado.Sum(x => x.feb),
                    mar = result.datosConcentrado.Sum(x => x.mar),
                    abr = result.datosConcentrado.Sum(x => x.abr),
                    may = result.datosConcentrado.Sum(x => x.may),
                    jun = result.datosConcentrado.Sum(x => x.jun),
                    jul = result.datosConcentrado.Sum(x => x.jul),
                    ago = result.datosConcentrado.Sum(x => x.ago),
                    sep = result.datosConcentrado.Sum(x => x.sep),
                    oct = result.datosConcentrado.Sum(x => x.oct),
                    nov = result.datosConcentrado.Sum(x => x.nov),
                    dic = result.datosConcentrado.Sum(x => x.dic),
                    total = result.datosConcentrado.Sum(x => x.total)
                };
                result.datosConcentrado.Add(totales);
            }

            var meses = MonthsBetween(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
            var graficaTendencia = new GraficaDTO();
            foreach (var mes in meses)
            {


                graficaTendencia.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);

                if (mes.Item3 == 1)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").ene;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").ene;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").ene;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 2)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").feb;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").feb;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").feb;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 3)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").mar;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").mar;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").mar;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 4)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").abr;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").abr;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").abr;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 5)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").may;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").may;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").may;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 6)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").jun;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").jun;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").jun;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 7)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").jul;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").jul;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").jul;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 8)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").ago;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").ago;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").ago;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 9)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").sep;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").sep;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").sep;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 10)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").oct;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").oct;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").oct;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 11)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").nov;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").nov;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").nov;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
                else if (mes.Item3 == 12)
                {
                    var Principales = 0m;
                    var Otros = 0m;
                    var Totales = 0m;

                    Principales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA PRINCIPAL").dic;
                    graficaTendencia.serie1Descripcion = "COSTOS FLOTA PRINCIPAL";
                    graficaTendencia.serie1.Add(Principales);
                    if (result.tieneOtros)
                    {
                        Otros = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS FLOTA OTROS").dic;
                        graficaTendencia.serie2Descripcion = "COSTOS FLOTA OTROS";
                        graficaTendencia.serie2.Add(Otros);

                        Totales = result.datosConcentrado.FirstOrDefault(x => x.descripcion == "COSTOS TOTALES").dic;
                        graficaTendencia.serie3Descripcion = "COSTOS TOTALES";
                        graficaTendencia.serie3.Add(Totales);
                    }
                }
            }
            result.grafica_tendencia = graficaTendencia;
            return result;
        }
        public tendenciaGeneralDTO getDatosGeneralesCTA(int empresa, int cta, int anio, List<string> cc, int grupo, int modelo)
        {
            var result = new tendenciaGeneralDTO();
            result.esMultiCuenta = false;
            var mes = anio == DateTime.Now.Year ? DateTime.Now.Month : 12;
            var ccData = _context.tblP_CC.Where(x => cc.Contains(x.areaCuenta)).ToList();
            var lstCC = ccData.Select(x => x.cc).ToList();
            var lstAC = ccData.Select(x => " ( area=" + x.area + " and cuenta_oc=" + x.cuenta + " ) ").ToList();

            var cca = empresa == 1 ? @"mov.cc in ('" + string.Join("','", lstCC) + @"')" : string.Join(" or ", lstAC);
            var cta5000 = empresa == 1 ? @"(
                                mov.cta=5000 and not
                                (
                                    mov.cta=5000 and mov.scta=18 and mov.sscta=0
                                ) and not
                                (
                                    (mov.concepto='MANTENIMIENTO DE MAQUINARIA' and ((mov.year<2021) or (mov.year=2021 and (mov.mes>=1 and mov.mes<=3))))
                                )
                            )" : @"(
                                mov.cta=5000 and 
                                (
                                    (mov.scta!=18 and mov.sscta!=0) and 
                                    (mov.scta!=8 and mov.sscta!=11)
                                )
                            )";
            var ctaa = cta == 4000 ? @"mov.cta=4000" : cta == 5000 ? cta5000 : @"(mov.cta=5901 and mov.scta=6 and mov.sscta=0)";
            string consulta = @"select 
                                    x.tipo,
                                    x.cta,
                                    x.scta,
                                    x.sscta,
                                    x.descripcion,
                                    SUM(CASE WHEN x.mes = 1 THEN x.monto ELSE 0 END) as ene, 
                                    SUM(CASE WHEN x.mes = 2 THEN x.monto ELSE 0 END) as feb, 
                                    SUM(CASE WHEN x.mes = 3 THEN x.monto ELSE 0 END) as mar,  
                                    SUM(CASE WHEN x.mes = 4 THEN x.monto ELSE 0 END) as abr,  
                                    SUM(CASE WHEN x.mes = 5 THEN x.monto ELSE 0 END) as may,  
                                    SUM(CASE WHEN x.mes = 6 THEN x.monto ELSE 0 END) as jun,  
                                    SUM(CASE WHEN x.mes = 7 THEN x.monto ELSE 0 END) as jul,  
                                    SUM(CASE WHEN x.mes = 8 THEN x.monto ELSE 0 END) as ago,  
                                    SUM(CASE WHEN x.mes = 9 THEN x.monto ELSE 0 END) as sep,  
                                    SUM(CASE WHEN x.mes = 10 THEN x.monto ELSE 0 END) as oct, 
                                    SUM(CASE WHEN x.mes = 11 THEN x.monto ELSE 0 END) as nov,  
                                    SUM(CASE WHEN x.mes = 12 THEN x.monto ELSE 0 END) as dic,
                                    ROUND(SUM(x.monto),0) as total,
                                    0 as porcentaje,
                                    (SUM(CASE WHEN x.mes = " + mes + @"-1 THEN x.monto ELSE 0 END) - (SUM(CASE WHEN (x.mes >= " + mes + @"-7 and x.mes < " + mes + @"-1) THEN x.monto ELSE 0 END)/6) ) as variabilidad,
                                    0 as porcentajeVariabilidad,
                                    0 as es90_10
                                from (
                                    SELECT
    	                                mov.mes, 
                                        mov.cta,
                                        mov.scta,
                                        mov.sscta,
                                        b.descripcion,
                                        1 as tipo,
                                        ROUND(SUM(mov.monto),0) as monto
                                    FROM sc_movpol mov 
                                        INNER JOIN cc c ON mov.cc = c.cc
                                        INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
                                    WHERE (mov.year=" + anio + @") and (" + cca + @") and (" + ctaa + @") 
                                    group by mov.mes,
                                        mov.cta,
                                        mov.scta,
                                        mov.sscta,
                                        b.descripcion,
                                        tipo
                                    ) x
                                group by 
                                    x.tipo,
                                    x.cta,
                                    x.scta,
                                    x.sscta,
                                    x.descripcion
                                order by 
                                    x.tipo,
                                    x.cta,
                                    x.scta,
                                    x.sscta,
                                    x.descripcion";
            var con = empresa == 1 ? EnkontrolAmbienteEnum.ProdCPLAN : EnkontrolAmbienteEnum.ProdARREND;

            var data = _contextEnkontrol.Select<tendenciasDTO>(con, consulta);
            data.ForEach(x => x.empresa = "" + empresa);

            var totalDatos = data.Sum(x => x.total);
            data.ForEach(x => x.porcentaje = Math.Round(((x.total / totalDatos) * 100), 0));
            var totalVariabilidad = data.Sum(x => x.variabilidad);
            data.ForEach(x => x.porcentajeVariabilidad = totalVariabilidad == 0 ? 0 : Math.Round(((x.variabilidad / totalVariabilidad) * 100), 0));
            result.datos = data.OrderByDescending(x => x.porcentajeVariabilidad).ToList();
            decimal porcentaje_general = 0;
            bool porcentaje_general_estatus = true;

            foreach (var x in result.datos)
            {
                porcentaje_general += x.porcentajeVariabilidad;
                if (porcentaje_general_estatus || porcentaje_general <= 90)
                {
                    x.es90_10 = true;

                    if (porcentaje_general >= 90)
                    {
                        porcentaje_general_estatus = false;
                    }
                }
                else
                {
                    x.es90_10 = false;
                }
            }
            result.esMultiCuenta = false;
            result.tieneOtros = false;

            var meses = MonthsBetween(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
            var graficaTendencia = new GraficaDTO();

            result.grafica_tendencia = graficaTendencia;
            return result;
        }
        public List<tenecoDTO> getDatosDetalle(int empresa, int anio, int mes, List<string> cc, int cta, int scta, int sscta)
        {
            var ccData = _context.tblP_CC.Where(x => cc.Contains(x.areaCuenta)).ToList();
            var lstCC = ccData.Select(x => x.cc).ToList();
            var lstAC = ccData.Select(x => " ( area=" + x.area + " and cuenta_oc=" + x.cuenta + " ) ").ToList();

            var cca = empresa == 1 ? @"mov.cc in ('" + string.Join("','", lstCC) + @"')" : string.Join(" or ", lstAC);

            string consulta = @"SELECT
                c.cc as economico,
    	        c.descripcion,
                ROUND(SUM(mov.monto),0) as monto
            FROM sc_movpol mov 
                INNER JOIN cc c ON mov.cc = c.cc
                INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
            WHERE (mov.year=" + anio + @" and mov.mes=" + mes + @") and (" + cca + @") and (mov.cta=" + cta + @" and mov.scta=" + scta + @" and mov.sscta=" + sscta + @")
            group by c.cc,c.descripcion";

            var con = empresa == 1 ? EnkontrolAmbienteEnum.ProdCPLAN : EnkontrolAmbienteEnum.ProdARREND;
            var data = _contextEnkontrol.Select<tenecoDTO>(con, consulta);


            return data;
        }


        public List<tenecoDTO> getDatosDetalle_Movto(int empresa, int anio, int mes, List<string> cc, int cta, int scta, int sscta, string economico)
        {
            var ccData = _context.tblP_CC.Where(x => cc.Contains(x.areaCuenta)).ToList();
            var lstCC = ccData.Select(x => x.cc).ToList();
            var lstAC = ccData.Select(x => " ( area=" + x.area + " and cuenta_oc=" + x.cuenta + " ) ").ToList();

            var cca = empresa == 1 ? @"mov.cc in ('" + string.Join("','", lstCC) + @"')" : string.Join(" or ", lstAC);

            string consulta = @"SELECT
    	        c.cc as economico,
    	        c.descripcion,
                mov.cta,
                mov.scta,
                mov.sscta,
                mov.concepto,
                mov.year,
                mov.mes,
                mov.monto as monto
            FROM sc_movpol mov 
                INNER JOIN cc c ON mov.cc = c.cc
                INNER JOIN catcta b ON b.cta = mov.cta AND b.scta = mov.scta AND b.sscta = mov.sscta
            WHERE (mov.year=" + anio + @" and mov.mes=" + mes + @") and (" + cca + @") and (mov.cta=" + cta + @" and mov.scta=" + scta + @" and mov.sscta=" + sscta + @") and mov.cc='" + economico + @"' 
            ";

            var con = empresa == 1 ? EnkontrolAmbienteEnum.ProdCPLAN : EnkontrolAmbienteEnum.ProdARREND;
            var data = _contextEnkontrol.Select<tenecoDTO>(con, consulta);


            return data;
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

        public Dictionary<string, object> CargarReporteCostos(string cc, int anio)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var filtroCCString = "";

                if (cc != "")
                {
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        filtroCCString = string.Format(@"AND cc = '{0}'", cc);
                    }
                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        var area = cc.Split('-')[0];
                        var cuenta = cc.Split('-')[1];

                        filtroCCString = string.Format(@"AND mov.area = {0} AND mov.cuenta_oc = {1}", area, cuenta);
                    }
                }

                var data = _contextEnkontrol.Select<ReporteCostosDTO>(getEnkontrolEnumADM(), string.Format(@"
                    SELECT
                        cta.descripcion,
                        SUM(CASE WHEN mov.mes = 1 THEN mov.monto ELSE 0 END) AS enero,
                        SUM(CASE WHEN mov.mes = 2 THEN mov.monto ELSE 0 END) AS febrero,
                        SUM(CASE WHEN mov.mes = 3 THEN mov.monto ELSE 0 END) AS marzo,
                        SUM(CASE WHEN mov.mes = 4 THEN mov.monto ELSE 0 END) AS abril,
                        SUM(CASE WHEN mov.mes = 5 THEN mov.monto ELSE 0 END) AS mayo,
                        SUM(CASE WHEN mov.mes = 6 THEN mov.monto ELSE 0 END) AS junio,
                        SUM(CASE WHEN mov.mes = 7 THEN mov.monto ELSE 0 END) AS julio,
                        SUM(CASE WHEN mov.mes = 8 THEN mov.monto ELSE 0 END) AS agosto,
                        SUM(CASE WHEN mov.mes = 9 THEN mov.monto ELSE 0 END) AS septiembre,
                        SUM(CASE WHEN mov.mes = 10 THEN mov.monto ELSE 0 END) AS octubre,
                        SUM(CASE WHEN mov.mes = 11 THEN mov.monto ELSE 0 END) AS noviembre,
                        SUM(CASE WHEN mov.mes = 12 THEN mov.monto ELSE 0 END) AS diciembre,
                        SUM(mov.monto) AS total
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        LEFT JOIN catcta cta ON mov.cta = cta.cta AND mov.scta = cta.scta AND cta.sscta = 0
                    WHERE mov.cta = 5000 AND pol.year = {0} {1}
                    GROUP BY cta.descripcion
                    ORDER BY descripcion", anio, filtroCCString)
                );

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(1, 0, "RepGastosMaquinariaController", "CargarReporteCostos", e, AccionEnum.CONSULTA, 0, new { cc = cc, anio = anio });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public MemoryStream GenerarExcelReporteCostos(string cc, int anio)
        {
            try
            {
                #region Filtro Centro de Costo
                var filtroCCString = "";

                if (cc != "")
                {
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        filtroCCString = string.Format(@"AND cc = '{0}'", cc);
                    }
                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        var area = cc.Split('-')[0];
                        var cuenta = cc.Split('-')[1];

                        filtroCCString = string.Format(@"AND mov.area = {0} AND mov.cuenta_oc = {1}", area, cuenta);
                    }
                }
                #endregion

                #region Datos información
                var datosInformacion = _contextEnkontrol.Select<ReporteCostosExcelDTO>(getEnkontrolEnumADM(), string.Format(@"
                    SELECT
                        mov.cta,
                        mov.scta,
                        mov.sscta,
                        mov.concepto,
                        mov.monto,
                        mov.cc,
                        mov.area,
                        mov.cuenta_oc,
                        pol.fechapol,
                        CONVERT(varchar, pol.fechapol, 103) AS fechapolString,
                        cta.descripcion AS nombreCuenta,
                        sscta.descripcion AS nombreSubSubCuenta
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        LEFT JOIN catcta cta ON mov.cta = cta.cta AND mov.scta = cta.scta AND cta.sscta = 0
                        LEFT JOIN catcta sscta ON mov.cta = sscta.cta AND mov.scta = sscta.scta AND mov.sscta = sscta.sscta
                    WHERE mov.cta = 5000 AND pol.year = {0} {1}", anio, filtroCCString)
                );
                #endregion

                #region Información de los centros de costos
                var listaCentroCostoEK = _contextEnkontrol.Select<CentroCostoDTO>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM cc"));
                var listaCentroCostoSIGOPLAN = new List<tblP_CC>();

                using (MainContext _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                {
                    listaCentroCostoSIGOPLAN = _contextConstruplan.tblP_CC.Where(x => x.estatus).ToList();
                }
                #endregion

                Color blanco = ColorTranslator.FromHtml("#fff");

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hojaResumen = excel.Workbook.Worksheets.Add("Resumen");
                    var hojaDatos = excel.Workbook.Worksheets.Add("Datos");

                    #region Datos
                    List<string[]> headerRowDatos = new List<string[]>();
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        headerRowDatos = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "nombreSubSubCuenta", "centroCosto", "concepto", "monto", "fechapol", "nombreCuenta"
                    } };
                    }
                    else
                    {
                        headerRowDatos = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "nombreSubSubCuenta", "areaCuenta", "concepto", "monto", "fechapol", "nombreCuenta", "centroCosto"
                    } };
                    }

                    string TituloRangoDatos = "";
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        TituloRangoDatos = "A1:I1";
                    }
                    else
                    {
                        TituloRangoDatos = "A1:J1";
                    }
                    
                    hojaDatos.Cells[TituloRangoDatos].LoadFromArrays(headerRowDatos);
                    hojaDatos.Cells[TituloRangoDatos].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hojaDatos.Cells[TituloRangoDatos].Style.Font.Color.SetColor(blanco);
                    hojaDatos.Cells[TituloRangoDatos].Style.Font.Bold = true;
                    hojaDatos.Cells[TituloRangoDatos].Style.Fill.BackgroundColor.SetColor(1, 112, 173, 71);
                    hojaDatos.Cells[TituloRangoDatos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hojaDatos.Cells[TituloRangoDatos].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var contadorDatos = 2;

                    foreach (var reg in datosInformacion)
                    {
                        #region Nombre del centro de costo
                        var centroCostoNombre = "";
                        var noEconomico = "";

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                        {
                            centroCostoNombre = listaCentroCostoEK.Where(x => x.cc == reg.cc).Select(x => x.descripcion).FirstOrDefault();

                        }
                        else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        {
                            var centroCostoAreaCuentaDesc = listaCentroCostoSIGOPLAN.Where(x => x.area == reg.area && x.cuenta == reg.cuenta_oc).Select(x => x.descripcion).FirstOrDefault();

                            centroCostoNombre = centroCostoAreaCuentaDesc != "" ? centroCostoAreaCuentaDesc : (reg.area + " - " + reg.cuenta_oc);
                            noEconomico = listaCentroCostoEK.Where(x => x.cc == reg.cc).Select(x => x.descripcion).FirstOrDefault();
                        }
                        #endregion

                        var cellDataDatos = new List<object[]>();

                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                        {
                            cellDataDatos.Add(new object[] {
                            reg.cta,
                            reg.scta,
                            reg.sscta,
                            reg.nombreSubSubCuenta,
                            centroCostoNombre,
                            reg.concepto,
                            reg.monto,
                            reg.fechapolString,
                            reg.nombreCuenta
                        });

                            hojaDatos.Cells[string.Format(@"A{0}:I{0}", contadorDatos)].LoadFromArrays(cellDataDatos);
                            hojaDatos.Cells[string.Format(@"A{0}:I{0}", contadorDatos)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                        else
                        {
                            cellDataDatos.Add(new object[] {
                            reg.cta,
                            reg.scta,
                            reg.sscta,
                            reg.nombreSubSubCuenta,
                            centroCostoNombre,
                            reg.concepto,
                            reg.monto,
                            reg.fechapolString,
                            reg.nombreCuenta,
                            noEconomico
                        });

                            hojaDatos.Cells[string.Format(@"A{0}:J{0}", contadorDatos)].LoadFromArrays(cellDataDatos);
                            hojaDatos.Cells[string.Format(@"A{0}:J{0}", contadorDatos)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                        hojaDatos.Cells[string.Format(@"G{0}", contadorDatos)].Style.Numberformat.Format = "$#,##0.00";
                        hojaDatos.Cells[string.Format(@"H{0}", contadorDatos)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        hojaDatos.Cells[string.Format(@"H{0}", contadorDatos)].Formula = "=DATEVALUE(\"" + Convert.ToDateTime(hojaDatos.Cells[string.Format(@"H{0}", contadorDatos)].Value).ToString("dd/MM/yyyy") + "\")";

                        contadorDatos++;
                    }
                    #endregion

                    #region Resumen
                    var dataRange = hojaDatos.Cells[hojaDatos.Dimension.Address];
                    var pivotTable = hojaResumen.PivotTables.Add(hojaResumen.Cells["A1"], dataRange, "PivotTable");

                    pivotTable.RowFields.Add(pivotTable.Fields["nombreCuenta"]);
                    pivotTable.RowFields.Add(pivotTable.Fields["nombreSubSubCuenta"]);
                    pivotTable.RowFields.Add(pivotTable.Fields["concepto"]);
                    pivotTable.DataOnRows = false;

                    var rowField = pivotTable.RowFields.Add(pivotTable.Fields["nombreCuenta"]);
                    rowField.Sort = eSortType.Ascending;

                    var field = pivotTable.DataFields.Add(pivotTable.Fields["monto"]);
                    field.Name = "Sumatoria Montos";
                    field.Function = DataFieldFunctions.Sum;
                    field.Format = "$#,##0.00";

                    ExcelPivotTableField dayColumnField = pivotTable.ColumnFields.Add(pivotTable.Fields["fechapol"]);

                    dayColumnField.AddDateGrouping(eDateGroupBy.Months | eDateGroupBy.Days);
                    dayColumnField.Name = "Dia";
                    dayColumnField.Sort = eSortType.Ascending;

                    ExcelPivotTableField monthColumnField = pivotTable.Fields.GetDateGroupField(eDateGroupBy.Months);
                    monthColumnField.Name = "Mes";

                    //dayColumnField.AddDateGrouping(7, new DateTime(anio, 1, 1), new DateTime(anio, 1, 7));

                    var esMX = System.Globalization.CultureInfo.CreateSpecificCulture("es-MX");

                    for (int month = 1; month <= 12; month++)
                    {
                        monthColumnField.Items[month].Text = esMX.DateTimeFormat.GetAbbreviatedMonthName(month);
                    }
                    #endregion

                    hojaDatos.Cells[hojaDatos.Dimension.Address].AutoFitColumns();

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    excel.Workbook.CreateVBAProject();
                    excel.Workbook.CodeModule.Code = @"
                        Private wbOpenEventRun as Boolean

                        Private Sub Workbook_Open()
                            wbOpenEventRun = true

                            Dim pT As PivotTable
                            Dim pF As PivotField

                            Set pT = ActiveSheet.PivotTables(1)

                            With pT
                                For Each pF In pT.RowFields
                                    pF.DrillTo pF.Name
                                Next pF

                                For Each pF In pT.ColumnFields
                                    pF.DrillTo pF.Name
                                Next pF
                            End With
                        End Sub

                        Private Sub Workbook_SheetSelectionChange(ByVal Sh As Object, ByVal Target As Range)
                            If Not wbOpenEventRun Then Workbook_Open
                            ' perform tasks in reaction of selection change events, if required
                        End Sub
                    ";
                    excel.Save();

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
                LogError(1, 0, "RepGastosMaquinariaController", "GenerarExcelReporteCostos", e, AccionEnum.DESCARGAR, 0, new { cc = cc, anio = anio });

                return null;
            }
        }

        public Dictionary<string, object> FillComboCC()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var data = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, string.Format(@"
                    SELECT 
                        c.cc AS Value, 
                        (c.cc + '-' + c.descripcion) AS Text 
                    FROM cc c 
                    WHERE c.st_ppto != 'T' 
                    ORDER BY Value")
                );

                resultado.Add(ITEMS, data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(1, 0, "RepGastosMaquinariaController", "FillComboCC", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private EnkontrolEnum getEnkontrolEnumADM()
        {
            var baseDatos = new EnkontrolEnum();

            if (vSesiones.sesionEmpresaActual == 1)
            {
                baseDatos = EnkontrolEnum.CplanProd;
            }
            else if (vSesiones.sesionEmpresaActual == 2)
            {
                baseDatos = EnkontrolEnum.ArrenProd;
            }
            else
            {
                throw new Exception("Empresa distinta a Construplan y Arrendadora");
            }

            return baseDatos;
        }
    }
}

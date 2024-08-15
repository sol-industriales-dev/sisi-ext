using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Reportes;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Reporte
{
    public class RepComparativaTiposDAO : IRepComparativaTiposDAO
    {
        public IList<RepComparativaTiposDTO> getAmountbyType(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string hasTipo = "";
            if (obj.area != 0)
            {
                hasTipo = "b.area='" + obj.area + "'and ";
            }


            string consulta = "SELECT noEco,sum(importe) as importe,tipoInsumo,descripcion,area,cuenta  FROM (" +
                              "SELECT (select top 1 descripcion from si_area_cuenta as ac where ac.area=b.area and ac.cuenta=b.cuenta order by descripcion) as NoEco," +
                              "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area,b.cuenta " +
                              "FROM insumos a INNER JOIN si_movimientos_det b " +
                              "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                              "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                              "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                              "where b.tipo_mov = 51 and " + hasTipo + "fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
                              " GROUP BY a.tipo,d.descripcion,b.area,b.cuenta" +
                              " UNION ALL " +
                              "SELECT (select top 1 descripcion from si_area_cuenta as ac where ac.area=b.area and ac.cuenta=b.cuenta order by descripcion) as NoEco," +
                              "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area,b.cuenta " +
                              "FROM insumos AS a " +
                              "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                              "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
                              "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                              "where " + hasTipo + " fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
                              " GROUP BY a.tipo,d.descripcion,b.area,b.cuenta" +
                              ") x GROUP BY noEco,x.tipoInsumo,x.descripcion,x.area,x.cuenta ORDER BY x.tipoInsumo;";
            var res1 = (IList<RepComparativaTiposDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<RepComparativaTiposDTO>>();
            return res1.ToList();
        }
        public IList<RepComparativaTiposDTO> getAmountbyGroup(RepGastosFiltrosDTO obj, string cc)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;

            string cadena = "";
            if (cc != "")
            {
                cadena = " and cc='" + cc + "' ";
            }

            string consulta = "SELECT noEco,sum(importe) as importe,tipoInsumo,descripcion,area " +
                                "FROM (SELECT e.descripcion as noEco , " +
                                "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area  " +
                                "FROM insumos a  " +
                                "INNER JOIN si_movimientos_det b ON a.insumo=b.insumo   " +
                                "INNER JOIN si_movimientos c ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen  " + cadena +
                                "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo  " +
                                "INNER JOIN saf_grupos_activo e on e.grupo_activo = b.area " +
                                "where b.tipo_mov = 51 and  fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' AND e.tipo_activo ='" + obj.idTipo + "' " +
                                "GROUP BY a.tipo,d.descripcion,b.area,e.descripcion UNION ALL  " +
                                "SELECT e.descripcion as noEco, " +
                                "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area  " +
                                "FROM insumos AS a INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo  " +
                                "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen  " + cadena +
                                "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo  " +
                                "INNER JOIN saf_grupos_activo e on e.grupo_activo = b.area " +
                                "where fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' AND e.tipo_activo ='" + obj.idTipo + "' " +
                                "GROUP BY a.tipo,d.descripcion,b.area,e.descripcion) x GROUP BY x.tipoInsumo,x.descripcion,x.area,x.noEco ORDER BY x.tipoInsumo; ";
            var res1 = (IList<RepComparativaTiposDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<RepComparativaTiposDTO>>();
            return res1.ToList();
        }
        public IList<RepComparativaTiposDTO> getAmountbyTypeNoOverhaulByTipo(RepGastosFiltrosDTO obj, string cc)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string cadena = "";
            if (cc != "")
            {
                cadena = " and cc='" + cc + "' ";
            }


            string consulta = "SELECT noEco,sum(importe) as importe,tipoInsumo,descripcion,area " +
                                "FROM (SELECT e.descripcion as noEco , " +
                                "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area " +
                                "FROM insumos a INNER JOIN si_movimientos_det b ON a.insumo=b.insumo  " +
                                "INNER JOIN si_movimientos c ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " + cadena +
                                "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                                "INNER JOIN saf_grupos_activo e on e.grupo_activo = b.area " +
                                "where b.tipo_mov = 51 and fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' and a.grupo!=51 and a.grupo!=52 and a.grupo!=60 AND e.tipo_activo =" + obj.idTipo +
                                "GROUP BY a.tipo,d.descripcion,b.area,b.cuenta,e.descripcion " +
                                "UNION ALL SELECT e.descripcion as noEco, " +
                                "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area " +
                                "FROM insumos AS a INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                                "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " + cadena +
                                "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                                "INNER JOIN saf_grupos_activo e on e.grupo_activo = b.area " +
                                "where fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' AND e.tipo_activo = " + obj.idTipo +
                                "GROUP BY a.tipo,d.descripcion,b.area,e.descripcion) x " +
                                "GROUP BY noEco,x.tipoInsumo,x.descripcion,x.area ORDER BY x.tipoInsumo;";

            var res1 = (IList<RepComparativaTiposDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<RepComparativaTiposDTO>>();
            return res1.ToList();
        }

        public IList<RepComparativaTiposDTO> getAmountbyTypeNoOverhaul(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;
            string hasTipo = "";
            if (obj.area != 0)
            {
                hasTipo = "b.area='" + obj.area + "'and ";
            }

            string consulta = "SELECT noEco,sum(importe) as importe,tipoInsumo,descripcion,area,cuenta  FROM (" +
                              "SELECT (select top 1 descripcion from si_area_cuenta as ac where ac.area=b.area and ac.cuenta=b.cuenta order by descripcion) as NoEco," +
                              "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area,b.cuenta " +
                              "FROM insumos a INNER JOIN si_movimientos_det b " +
                              "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                              "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                              "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                              "where b.tipo_mov = 51 and " + hasTipo + " fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
                              " and a.grupo!=51 and a.grupo!=52 and a.grupo!=60 GROUP BY a.tipo,d.descripcion,b.area,b.cuenta" +
                              " UNION ALL " +
                              "SELECT (select top 1 descripcion from si_area_cuenta as ac where ac.area=b.area and ac.cuenta=b.cuenta order by descripcion) as NoEco," +
                              "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area,b.cuenta " +
                              "FROM insumos AS a " +
                              "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                              "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
                              "INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                              "where " + hasTipo + " fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
                              " GROUP BY a.tipo,d.descripcion,b.area,b.cuenta" +
                              ") x GROUP BY noEco,x.tipoInsumo,x.descripcion,x.area,x.cuenta ORDER BY x.tipoInsumo;";
            var res1 = (IList<RepComparativaTiposDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<RepComparativaTiposDTO>>();
            return res1.ToList();
        }

        public double getTotalImporte(string economico)
        {
            string consulta = @"SELECT noEco,sum(importe) as importe,tipoInsumo,descripcion,area,cuenta  
                                FROM ( 
                                SELECT ( 
                                select top 1 descripcion from si_area_cuenta as ac where ac.area=b.area and ac.cuenta=b.cuenta order by descripcion) as NoEco, 
                                CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area,b.cuenta  
                                FROM insumos a  
                                INNER JOIN si_movimientos_det b ON a.insumo=b.insumo   
                                INNER JOIN si_movimientos c ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen  
                                INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo  
                                inner join si_area_cuenta e on b.cuenta = e.cuenta and b.area =e.area 
                                where b.tipo_mov = 51 and e.descripcion = '" + economico + "' and a.grupo!=51 and a.grupo!=52 and a.grupo!=60 " +
                                " GROUP BY a.tipo,d.descripcion,b.area,b.cuenta " +
                                " UNION ALL SELECT (select top 1 descripcion from si_area_cuenta as ac where ac.area=b.area and ac.cuenta=b.cuenta order by descripcion) as NoEco," +
                                " CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.tipo as tipoInsumo, d.descripcion as descripcion ,b.area,b.cuenta " +
                                " FROM insumos AS a " +
                                " INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                                " INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
                                " INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo " +
                                " inner join si_area_cuenta e on b.cuenta = e.cuenta and b.area =e.area " +
                                " where  e.descripcion = '" + economico + "' " +
                                " GROUP BY a.tipo,d.descripcion,b.area,b.cuenta) x " +
                                " GROUP BY noEco,x.tipoInsumo,x.descripcion,x.area,x.cuenta " +
                                " ORDER BY x.tipoInsumo; ";

            var res1 = (IList<RepComparativaTiposDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<RepComparativaTiposDTO>>();
            var result = res1.Sum(z => Convert.ToDouble(z.importe));

            return result;
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
                DateTime fechaConsulta = Convert.ToDateTime(fecha, CultureInfo.InvariantCulture);
                return "c.fecha >='" + fechaInicio.ToString("yyyyMMdd") + "' and  c.fecha <'" + fechaConsulta.ToString("yyyyMMdd") + "' ";
            }
            if (fechaFin.Month == mes)
            {
                string fecha = (mes) + "/01/" + fechaInicio.Year;
                DateTime fechaConsulta = Convert.ToDateTime(fecha, CultureInfo.InvariantCulture);
                return "c.fecha >='" + fechaConsulta.ToString("yyyyMMdd") + "' and  c.fecha <='" + fechaFin.ToString("yyyyMMdd") + "' ";
            }
            else
            {
                return "c.fecha >='" + fechaInicio.Year + mes.ToString().PadLeft(2, '0') + "01" + "' and  c.fecha <'" + fechaFin.Year + (mes + 1).ToString().PadLeft(2, '0') + "01' ";
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

        public IList<RepGastosMaquinariaGrid> getGrupoInsumos(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;

            string consulta = "SELECT sum(importe) as importe,tipoInsumo,descripcion FROM (" +
                 "SELECT CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.grupo as tipoInsumo, " +
                 "d.descripcion as descripcion " +
                 "FROM insumos a INNER JOIN si_movimientos_det b " +
                 "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
                 "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                 "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
                 "where b.tipo_mov = 51 AND b.area='" + obj.area + "' and b.cuenta='" + obj.cuenta + "' " +
                 "and d.tipo_insumo=" + obj.idTipo + "and a.tipo=" + obj.idTipo + " " +
                 "and c.fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
                 " GROUP BY a.grupo,d.descripcion,c.fecha" +
                 " UNION ALL " +
                 "SELECT " +
                 "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.grupo as tipoInsumo, d.descripcion as descripcion " +
                 "FROM insumos AS a " +
                 "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
                 "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
                 "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
                 "where b.area='" + obj.area + "' and b.cuenta='" + obj.cuenta + "' and a.tipo='" + obj.idTipo + "' and d.tipo_insumo='" + obj.idTipo + "' " +
                 "and c.fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
                 "GROUP BY a.grupo,d.descripcion " +
                 ") x GROUP BY x.tipoInsumo,x.descripcion ORDER BY x.tipoInsumo;";
            var res1 = (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();


            return res1.ToList();
        }

        public IList<RepGastosMaquinariaGrid> getInsumos(RepGastosFiltrosDTO obj)
        {
            DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
            DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);
            DateTime today = DateTime.Now;


            string consulta = "SELECT sum(importe) as importe,descripcion,CONVERT( CHAR( 20 ), fecha, 105 ) as fecha FROM (" +
          "SELECT " +
          "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe ,a.descripcion as descripcion,c.fecha AS fecha " +
          "FROM insumos a INNER JOIN si_movimientos_det b " +
          "ON a.insumo=b.insumo  INNER JOIN si_movimientos c " +
          "ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
          "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
          "INNER JOIN tipos_insumo e on e.tipo_insumo = a.tipo INNER JOIN cc c2 on c2.cc = c.cc " +
          "where b.tipo_mov = 51 and c2.descripcion='" + obj.ac + "' and b.cuenta='" + obj.cuenta + "' and d.tipo_insumo=" + obj.idTipo + "and a.tipo=" + obj.idTipo + "and d.grupo_insumo=" + obj.idGrupo + " " +
          "and c.fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
          " GROUP BY a.descripcion,c.fecha" +
          " UNION ALL " +
          "SELECT " +
          "CONVERT(VARCHAR(20),SUM(b.cantidad*b.precio), 365)  as importe , a.descripcion as descripcion,c.fecha AS fecha " +
          "FROM insumos AS a " +
          "INNER JOIN so_movimientos_noinv_det AS b ON a.insumo = b.insumo " +
          "INNER JOIN so_movimientos_noinv as c on b.remision = c.remision and b.tipo_mov=c.tipo_mov and b.almacen=c.almacen " +
          "INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
          "INNER JOIN tipos_insumo e on e.tipo_insumo = a.tipo " +
          "where b.area='" + obj.area + "' and b.cuenta='" + obj.cuenta + "' and a.tipo=" + obj.idTipo + "and d.tipo_insumo=" + obj.idTipo + " and d.grupo_insumo=" + obj.idGrupo + //+ (anio.Equals("0") ? "" : "AND fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "' ") +
          "and c.fecha between '" + fechaInicio.ToString("yyyyMMdd") + "' and '" + fechaFin.ToString("yyyyMMdd") + "'" +
          " GROUP BY a.descripcion,c.fecha" +
          ") x GROUP BY x.descripcion,x.fecha ORDER BY x.fecha;";

            var res1 = (IList<RepGastosMaquinariaGrid>)_contextEnkontrol.Where(consulta).ToObject<IList<RepGastosMaquinariaGrid>>();
            return res1.ToList();
        }


        public IList<pruebaDto> getDataPrueba()
        {
            string consulta = @"select b.precio
, a.tipo , d.descripcion  ,b.area,b.cuenta FROM insumos a 
INNER JOIN si_movimientos_det b ON a.insumo=b.insumo  
INNER JOIN si_movimientos c ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen
 INNER JOIN tipos_insumo d on d.tipo_insumo = a.tipo where b.tipo_mov = 51 and b.area='72' and fecha between '20160101' and '20161228' GROUP BY a.tipo,d.descripcion,b.area,b.cuenta,b.cantidad,b.precio;";
            var res1 = (IList<pruebaDto>)_contextEnkontrol.Where(consulta).ToObject<IList<pruebaDto>>();
            return res1.ToList();
        }

        public IList<area_cuentaDTO> getEconomicosXCentroCostos(string centroCostos)
        {

            string consulta = "SELECT Centro_Costo AS centro_costos,descripcion AS Descripcion,cuenta AS Cuenta,Area as area FROM si_area_cuenta WHERE cc_activo=1 and " +
                              "Centro_Costos = '" + centroCostos + "';";

            var res1 = (IList<area_cuentaDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<area_cuentaDTO>>();
            return res1;
        }
    }
}

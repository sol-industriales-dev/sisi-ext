using Core.DAO.Enkontrol.Almacen;
using Core.DTO.Enkontrol.Alamcen;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.Almacen
{
    public class ValuacionDAO : IValuacionDAO
    {
        #region Entrada
        public List<ValuacionDTO> getValuacion(List<chkAlmacenDTO> lstGrupo)
        {
            try
            {
                var lst = new List<ValuacionDTO>();
                var lstCompania = new List<int>() { 1, 2 };

                lstCompania.ForEach(c =>
                {
                    var query = queryValuacion(c, lstGrupo);
                    var bd = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)c);
                    var acum = _contextEnkontrol.Select<ValuacionDTO>(bd, query);

                    lst.AddRange(acum.Where(w => w.importe != null && w.cantidad != null && w.cantidad > decimal.Zero).ToList());
                });

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }

        string queryValuacion(int compania, List<chkAlmacenDTO> lstGrupo)
        {
            var hoy = DateTime.Now;
            var almacen = lstGrupo.Where(w => w.grupo.Any(g => g.compania.Equals(compania))).SelectMany(s => s.almacen).GroupBy(g => g).Select(s => s.Key).ToList();
            var grupos = lstGrupo.Where(w => w.grupo.Any(g => g.compania.Equals(compania))).SelectMany(s => s.grupo).GroupBy(g => g.grupo).Select(s => s.Key).ToList();
            var lnAlmacen = ListToLine(almacen.ConvertAll<string>(x => x.ToString()));
            var lnGrupos = ListToLine(grupos.ConvertAll<string>(x => x.ToString()));
            var query = string.Format(@"
                SELECT 
                    DISTINCT {2} AS compania, 
                    a.insumo, 
                    i.tipo, 
                    i.grupo, 
                    a.almacen, 
                    i.descripcion AS descInsumo, 
                    (SELECT TOP 1 g.descripcion FROM grupos_insumo g WHERE g.grupo_insumo = i.grupo AND g.tipo_insumo = i.tipo ) AS descripcion,
                    (SELECT TOP 1 REPLACE(REPLACE(n.descripcion,'ALMACEN ',''),'DE ','') FROM si_almacen n WHERE n.almacen = a.almacen ) AS nomAlmacen, 
                    SUM(
                        CASE WHEN importe_ent_ini IS NULL THEN 0 ELSE importe_ent_ini END + importe_ent_ene + importe_ent_feb + importe_ent_mar + importe_ent_abr + importe_ent_may + importe_ent_jun + importe_ent_jul + importe_ent_ago + importe_ent_sep + importe_ent_oct + importe_ent_nov + importe_ent_dic
                    ) - SUM(
                        CASE WHEN importe_sal_ini IS NULL THEN 0 ELSE importe_sal_ini END + importe_sal_ene + importe_sal_feb + importe_sal_mar + importe_sal_abr + importe_sal_may + importe_sal_jun + importe_sal_jul + importe_sal_ago + importe_sal_sep + importe_sal_oct + importe_sal_nov + importe_sal_dic
                    ) AS importe, 
                    SUM(
                        CASE WHEN existencia_ent_ini IS NULL THEN 0 ELSE existencia_ent_ini END + existencia_ent_ene + existencia_ent_feb + existencia_ent_mar + existencia_ent_abr + existencia_ent_may + existencia_ent_jun + existencia_ent_jul + existencia_ent_ago + existencia_ent_sep + existencia_ent_oct + existencia_ent_nov + existencia_ent_dic
                    ) - SUM(
                        CASE WHEN existencia_sal_ini IS NULL THEN 0 ELSE existencia_sal_ini END + existencia_sal_ene + existencia_sal_feb + existencia_sal_mar + existencia_sal_abr + existencia_sal_may + existencia_sal_jun + existencia_sal_jul + existencia_sal_ago + existencia_sal_sep + existencia_sal_oct + existencia_sal_nov + existencia_sal_dic
                    ) AS cantidad 
                FROM si_acumula_almacen a 
                    INNER JOIN insumos i ON a.insumo = i.insumo AND i.grupo IN ({1}) 
                WHERE a.ano = {3} AND a.almacen IN ({0}) 
                GROUP BY a.insumo, i.tipo, i.grupo, descripcion, a.almacen, descInsumo", lnAlmacen, lnGrupos, compania, hoy.Year);

            return query;
        }

        public List<chkAlmacenDTO> getAlamences(List<int> lstComp)
        {
            try
            {
                var lst = new List<chkAlmacenDTO>();

                lstComp.ForEach(c =>
                {
                    var query = queryAlamences();
                    var bd = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)c);
                    var res = _contextEnkontrol.Select<chkAlmacenDTO>(bd, query);

                    res.Where(i => i.cantidad > 0 && i.cantidad != null).GroupBy(a => a.Value).Select(a => new chkAlmacenDTO
                    {
                        Value = a.Key,
                        Text = a.FirstOrDefault().Text,
                        cantidad = a.Sum(s => s.cantidad),
                        grupo = new List<Insumogrupo>(),
                        almacen = new List<int>(),
                        tipo = 0
                    }).Where(w => w.cantidad > 0).ToList().ForEach(a =>
                    {
                        if (lst.Any(e => e.almacen.Any(ee => ee.Equals(a.Value))))
                        {
                            lst.FirstOrDefault(w => w.almacen.Any(aa => aa.Equals(a.Value))).grupo.Add(new Insumogrupo() { compania = c, grupo = 0 });
                        }
                        else
                        {
                            a.grupo = new List<Insumogrupo> { new Insumogrupo {
                                compania = c,
                                grupo = 0
                            }};
                            a.almacen = new List<int>();
                            a.almacen.Add(a.Value);
                            lst.Add(a);
                        }
                    });
                });

                return lst.OrderBy(o => o.Text).ToList();
            }
            catch (Exception) { return null; }
        }

        string queryAlamences()
        {
            var hoy = DateTime.Now;
            var query = string.Format(@"
                            SELECT
                                acu.almacen AS Value, 
                                acu.insumo, 
                                SUM( 
                                    CASE WHEN existencia_ent_ini IS NULL THEN 0 ELSE existencia_ent_ini END + existencia_ent_ene + existencia_ent_feb + existencia_ent_mar + existencia_ent_abr + existencia_ent_may + existencia_ent_jun + existencia_ent_jul + existencia_ent_ago + existencia_ent_sep + existencia_ent_oct + existencia_ent_nov + existencia_ent_dic
                                ) - SUM( 
                                    CASE WHEN existencia_sal_ini IS NULL THEN 0 ELSE existencia_sal_ini END + existencia_sal_ene + existencia_sal_feb + existencia_sal_mar + existencia_sal_abr + existencia_sal_may + existencia_sal_jun + existencia_sal_jul + existencia_sal_ago + existencia_sal_sep + existencia_sal_oct + existencia_sal_nov + existencia_sal_dic
                                ) AS cantidad, 
                                REPLACE(REPLACE(alm.descripcion,'ALMACEN ',''),'DE ','') AS Text 
                            FROM si_acumula_almacen acu, si_almacen alm 
                            WHERE alm.almacen = acu.almacen AND acu.almacen < 899 AND acu.ano = {0} 
                            GROUP BY acu.almacen, acu.ano, acu.insumo, Text", hoy.Year);

            return query;
        }

        public List<chkAlmacenDTO> getGruposInsumos(List<chkAlmacenDTO> lstAlmacenes)
        {
            try
            {
                var lst = new List<chkAlmacenDTO>();
                var lstCompania = new List<int>() { 1, 2 };
                lstCompania.ForEach(c =>
                {
                    var almacen = lstAlmacenes.Where(w => w.grupo.Any(g => g.compania.Equals(c))).SelectMany(s => s.almacen).ToList();
                    var query = queryGruposInsumos(almacen);
                    var bd = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)c);
                    var res = _contextEnkontrol.Select<chkAlmacenDTO>(bd, query);

                    res.ForEach(g =>
                    {
                        if (lst.Any(a => a.Text.Equals(g.Text)))
                        {
                            lst.FirstOrDefault(a => a.Text.Equals(g.Text)).grupo.Add(new Insumogrupo() { compania = c, grupo = g.Value, tipo = g.tipo });
                        }
                        else
                        {
                            g.almacen = new List<int>();
                            g.grupo = new List<Insumogrupo> { new Insumogrupo {
                                compania = c,
                                grupo = g.Value,
                                tipo = g.tipo
                            }};
                            g.almacen.AddRange(almacen);
                            lst.Add(g);
                        }
                    });
                });

                return lst.OrderBy(o => o.Text).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        string queryGruposInsumos(List<int> almacen)
        {
            var hoy = DateTime.Now;
            var lnAlmacen = ListToLine(almacen.ConvertAll<string>(x => x.ToString()));
            var query = string.Format(@"
                SELECT 
                    i.grupo AS Value, 
                    g.descripcion AS Text, 
                    acu.insumo, 
                    i.tipo, 
                    SUM( 
                        CASE WHEN existencia_ent_ini IS NULL THEN 0 ELSE existencia_ent_ini END + existencia_ent_ene + existencia_ent_feb + existencia_ent_mar + existencia_ent_abr + existencia_ent_may + existencia_ent_jun + existencia_ent_jul + existencia_ent_ago + existencia_ent_sep + existencia_ent_oct + existencia_ent_nov + existencia_ent_dic
                    ) - SUM(
                        CASE WHEN existencia_sal_ini IS NULL THEN 0 ELSE existencia_sal_ini END + existencia_sal_ene + existencia_sal_feb + existencia_sal_mar + existencia_sal_abr + existencia_sal_may + existencia_sal_jun + existencia_sal_jul + existencia_sal_ago + existencia_sal_sep + existencia_sal_oct + existencia_sal_nov + existencia_sal_dic
                    ) AS cantidad 
                FROM si_acumula_almacen acu, insumos i, grupos_insumo g 
                WHERE acu.insumo = i.insumo AND g.grupo_insumo = i.grupo AND g.tipo_insumo = i.tipo AND acu.almacen < 899 AND acu.ano = {1} AND acu.almacen IN ({0})
                GROUP BY Value, Text, acu.insumo, i.tipo", lnAlmacen, hoy.Year);

            return query;
        }
        #endregion
        #region Salida
        /// <summary>
        /// Insumos con salida de inventario
        /// </summary>
        /// <returns>Importes por insumos</returns>
        public List<ValuacionDTO> getValuacionSalida(DateTime fecha)
        {
            try
            {
                var lst = new List<ValuacionDTO>();
                var lstCompania = new List<int>() { 1, 2 };

                lstCompania.ForEach(c =>
                {
                    var query = queryValuacionSalida(c, fecha);
                    //var acum = (List<ValuacionDTO>)_contextEnkontrol.Where(query, c).ToObject<List<ValuacionDTO>>();
                    var bd = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)c);
                    var acum = _contextEnkontrol.Select<ValuacionDTO>(bd, query);
                    lst.AddRange(acum);
                });

                return lst;

            }
            catch (Exception) { return null; }
        }
        string queryValuacionSalida(int compania, DateTime fecha)
        {
            var query =
                string.Format(@"SELECT 
                                    DISTINCT m.almacen, 
                                    m.fecha, 
                                    m.periodo, 
                                    SUM(d.importe) AS importe, 
                                    {0} AS compania, 
                                    m.cc, 
                                    (SELECT TOP 1 REPLACE(REPLACE(n.descripcion,'ALMACEN ',''),'DE ','') FROM si_almacen n WHERE n.almacen = m.almacen) AS nomAlmacen 
                                FROM si_movimientos_det d 
                                    INNER JOIN insumos i ON d.insumo = i.insumo 
                                    INNER JOIN si_movimientos m ON m.almacen = d.almacen AND m.tipo_mov = d.tipo_mov AND m.numero = d.numero AND m.ano = {1} 
                                WHERE d.tipo_mov = 51 AND m.almacen < 399 AND m.almacen NOT IN (2, 30, 40, 60, 61, 62, 63, 64, 68, 80, 104, 130) 
                                GROUP BY m.almacen, m.periodo, m.fecha, m.cc", compania, fecha.Year);
            return query;
        }
        /// <summary>
        /// Consulta de periodos por salida
        /// </summary>
        /// <returns>Periodos</returns>
        public List<chkAlmacenSalidaDTO> getPeriodos(DateTime fecha)
        {
            try
            {
                var lst = new List<chkAlmacenSalidaDTO>();
                //var hoy = DateTime.Now;
                var lstComp = new List<int>() { 1, 2 };
                lstComp.ForEach(c =>
                {
                    var bd = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)c);
                    var res = _contextEnkontrol.Select<chkAlmacenSalidaDTO>(bd, string.Format(@"SELECT 
                                            periodo AS VALUE, 
                                            {1} AS compania, 
                                            SUM(total) AS importe 
                                        FROM si_movimientos 
                                        WHERE ano = {0} AND tipo_mov = 51 AND almacen < 399 AND almacen NOT IN (2, 30, 40, 60, 61, 62, 63, 64, 68, 80, 104, 130)
                                        GROUP BY periodo", fecha.Year, c));
                    lst.AddRange(res.Where(w => w.importe > 0));
                });
                return lst.GroupBy(g => g.Value).ToList().Select(p => new chkAlmacenSalidaDTO()
                {
                    Value = p.Key,
                    Text = new DateTime(2015, p.Key, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                    companias = p.Select(s => s.compania).ToList(),
                    periodo = p.Key
                }).OrderBy(o => o.Value).ToList();
            }
            catch (Exception) { return null; }
        }
        /// <summary>
        /// Consulta de alamcenes con salidas de inventarios
        /// </summary>
        /// <returns>Almacens</returns>
        public List<chkAlmacenSalidaDTO> getAlmacenesSalida(DateTime fecha)
        {
            try
            {
                var lst = new List<chkAlmacenSalidaDTO>();
                var lstComp = new List<int>() { 1, 2 };
                lstComp.ForEach(c =>
                {
                    var query = queryAlmacenesSalida(c, fecha);
                    var bd = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)c);
                    var res = _contextEnkontrol.Select<chkAlmacenSalidaDTO>(bd, query);
                    lst.AddRange(res.Where(w => w.importe > 0));
                });
                return lst.GroupBy(g => g.Value).ToList().Select(a => new chkAlmacenSalidaDTO()
                {
                    Value = a.Key,
                    Text = a.FirstOrDefault(w => w.Value.Equals(a.Key)).Text,
                    companias = a.Select(s => s.compania).ToList(),
                    almacen = a.Key
                }).OrderBy(o => o.Text).ToList();
            }
            catch (Exception) { return null; }
        }
        string queryAlmacenesSalida(int compania, DateTime fecha)
        {
            //var hoy = DateTime.Now;
            var query =
                string.Format(@"SELECT 
                                    m.almacen AS Value, 
                                    SUM(m.total) AS importe, 
                                    {1} AS compania, 
                                    (SELECT TOP 1 REPLACE(REPLACE(n.descripcion,'ALMACEN ',''),'DE ','') FROM si_almacen n WHERE n.almacen = m.almacen ) AS Text 
                                FROM si_movimientos m 
                                WHERE ano = {0} AND tipo_mov = 51 AND almacen < 399 AND m.almacen NOT IN (2, 30, 40, 60, 61, 62, 63, 64, 68, 80, 104, 130) 
                                GROUP BY m.almacen", fecha.Year, compania);
            return query;
        }
        #endregion
        /// <summary>
        /// Convertidos de lista a único string
        /// </summary>
        /// <param name="myList">lista</param>
        /// <returns>cadena</returns>
        string ListToLine(List<string> myList)
        {
            var sb = new System.Text.StringBuilder();
            foreach (string s in myList)
            {
                sb.Append("'" + s + "'").Append(",");
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
    }
}

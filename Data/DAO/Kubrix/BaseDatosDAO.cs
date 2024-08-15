using Core.DAO.Kubrix;
using Core.DTO.Kubrix;
using Core.Entity.Kubrix;
using Core.Entity.Maquinaria;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Kubrix
{
    public class BaseDatosDAO : GenericDAO<tblM_CapHorometro>, IBaseDatosDAO
    {
        public List<VencimientoDTO> lstVencimiento()
        {
            var cosulta = @"SELECT * FROM(
                                SELECT
                                a.numpro 
                               ,b.nombre 
                               ,a.factura 
                               ,b.moneda 
                               ,a.fecha 
                               ,a.fechavenc 
                               ,a.tm 
                               ,a.autorizapago
                               ,(SELECT SUM(g.total) FROM sp_movprov AS g WHERE g.numpro=a.numpro AND g.factura=a.factura) AS saldo_factura
                               ,a.cc 
                               ,c.descripcion 
                               ,a.tipocambio 
                               ,b.nomcorto 
                               ,c.corto 
                               ,b.telefono1 
                               ,a.concepto 
                               ,ROW_NUMBER() OVER(PARTITION BY a.numpro, a.factura ORDER BY a.numpro desc) AS contador
                           FROM sp_movprov A 
                           INNER JOIN sp_proveedores B ON A.numpro = B.numpro  
                           INNER JOIN CC  C ON c.cc = a.cc 
                               WHERE a.factura in 
                                       (SELECT factura
                                       FROM sp_movprov 
                                           WHERE numpro=a.numpro AND factura=a.factura
                                           GROUP BY factura
                                           HAVING sum(total) > 0)
                               ORDER BY a.numpro, a.factura
                           ) AS A
                       WHERE contador = 1";

            var resultado = (List<VencimientoDTO>)_contextEnkontrol.Where(cosulta).ToObject<List<VencimientoDTO>>();
            return resultado;
        }
        public List<SalContCCDTO> lstSalContCC(int anio)
        {
            var cosulta = string.Format("SELECT * FROM sc_salcont_cc where cc <> '*' and cta <> 0 and year = {0} order by cc", anio);
            var resultado = (List<SalContCCDTO>)_contextEnkontrol.Where(cosulta).ToObject<List<SalContCCDTO>>();
            resultado.ForEach(e =>
            {
                e.cc = e.cc.ParseInt().ToString("D3");
            });
            return resultado;
        }
        public List<object> getInfoMaquinaria(DateTime fechaInicio, DateTime fechaFin)
        {
            var lst = new List<object>();
            var objMaquinariaH = (from h in _context.tblM_CapHorometro
                                  where (h.Fecha <= fechaInicio && h.Fecha >= fechaFin)
                                  select h).ToList();
            var objMaquinarCombustible = (from c in _context.tblM_CapCombustible
                                          where (c.fecha <= fechaInicio && c.fecha >= fechaFin)
                                          select c).ToList();
            var objParo = (from c in _context.tblM_CapOrdenTrabajo
                           where (c.FechaCreacion <= fechaInicio && c.FechaCreacion >= fechaFin)
                           select c).ToList();
            var grupoMaquinariaH = objMaquinariaH.GroupBy(x => x.Economico).Select(x => x.Key).ToList();
            foreach (var item in grupoMaquinariaH)
            {
                var HorometroObj = objMaquinariaH.Where(y => y.Economico.Equals(item)).OrderBy(x => x.Fecha);
                var Result = objMaquinariaH
                            .Join(objMaquinarCombustible,
                                H => H.Economico,
                                C => C.Economico,
                                (H, C) => new { Horometro = H, Combustible = H })
                            .Where(x => x.Horometro.Economico.Equals(item));
                var paro = objParo.Exists(w => HorometroObj.FirstOrDefault().id == w.EconomicoID) ? objParo.FirstOrDefault(w => HorometroObj.FirstOrDefault().id == w.EconomicoID).TiempoHorasTotal : 0;

                if (Result.Count() != 0)
                    lst.Add(Result.Select(r => new
                    {
                        cc = r.Combustible.CC,
                        fecha = r.Combustible.FechaCaptura,
                        economico = r.Horometro.Economico,
                        turno = r.Horometro.turno,
                        HoroInicial = decimal.Round(r.Horometro.Horometro - r.Horometro.HorasTrabajo, 2),
                        HoroFinal = decimal.Round(r.Horometro.Horometro, 2),
                        ParoClima = paro,
                        HrsMtto = 0,
                        HrsTrab = r.Combustible.HorasTrabajo,
                        HrsProg = 0,
                        HrsEfect = r.Combustible.HorasTrabajo,
                        Efectivas = 0,
                        consumo = decimal.Round(r.Combustible.Horometro / r.Combustible.HorasTrabajo, 2),
                        gpo = new string(r.Horometro.Economico.TakeWhile(c => Char.IsLetter(c)).ToArray()),
                        RendTeorico = 0,
                        RendReal = decimal.Round((r.Combustible.Horometro / r.Combustible.HorasTrabajo) / r.Combustible.HorasTrabajo, 2),
                        Rendimiento = 0
                    }));
            }
            return lst
                .Cast<object>()
                .ToList();
        }

        public List<object> getInfoCapturaMaquinaria(DateTime fechaInicio, DateTime fechaFin, string cc)
        {
            var lst = new List<object>();
            var objMaquinariaH = (from h in _context.tblM_CapHorometro
                                  where (h.Fecha <= fechaInicio && h.Fecha >= fechaFin && h.CC == cc)
                                  select h).ToList();
            var objMaquinarCombustible = (from c in _context.tblM_CapCombustible
                                          where (c.fecha <= fechaInicio && c.fecha >= fechaFin && c.CC == cc)
                                          select c).ToList();
            var objParo = (from c in _context.tblM_CapOrdenTrabajo
                           where (c.FechaCreacion <= fechaInicio && c.FechaCreacion >= fechaFin && c.CC == cc)
                           select c).ToList();
            var grupoMaquinariaH = objMaquinariaH.GroupBy(x => x.Economico).Select(x => x.Key).ToList();

            var HorometroObj = objMaquinariaH
                //.Where(y => y.Economico.Equals(item))
                .OrderBy(x => x.Fecha);
            var Result = objMaquinariaH
                        .Join(objMaquinarCombustible,
                            H => H.Economico,
                            C => C.Economico,
                            (H, C) => new { Horometro = H, Combustible = H });
            //.Where(x => x.Horometro.Economico.Equals(item));
            var paro = objParo.Exists(w => HorometroObj.FirstOrDefault().id == w.EconomicoID) ? objParo.FirstOrDefault(w => HorometroObj.FirstOrDefault().id == w.EconomicoID).TiempoHorasTotal : 0;

            if (Result.Count() != 0)
            {
                var resultado = Result.GroupBy(x => new { x.Combustible.FechaCaptura.Date, x.Horometro.Economico }).Select(r => new
                {
                    sem = DatesAreInTheSameWeek(DateTime.Now, r.Select(y => y.Combustible.FechaCaptura.Date).FirstOrDefault()),
                    cc = r.Select(y => y.Combustible.CC).FirstOrDefault(),
                    fecha = r.Select(y => y.Combustible.FechaCaptura.Date.ToShortDateString()).FirstOrDefault(),
                    economico = r.Select(y => y.Horometro.Economico).FirstOrDefault(),
                    turno = r.Select(y => y.Horometro.turno).FirstOrDefault(),
                    HoroInicial = decimal.Round(r.Select(y => y.Horometro.Horometro).FirstOrDefault() - r.Select(y => y.Horometro.HorasTrabajo).FirstOrDefault(), 2),
                    HoroFinal = decimal.Round(r.Select(y => y.Horometro.Horometro).FirstOrDefault(), 2),
                    ParoClima = paro,
                    HrsMtto = 0,
                    HrsTrab = r.Select(y => y.Combustible.HorasTrabajo).FirstOrDefault(),
                    HrsProg = 0,
                    HrsEfect = r.Select(y => y.Combustible.HorasTrabajo).FirstOrDefault(),
                    Efectivas = 0,
                    consumo = decimal.Round(r.Select(y => y.Combustible.Horometro).FirstOrDefault() / r.Select(y => y.Combustible.HorasTrabajo).FirstOrDefault(), 2),
                    gpo = new string(r.Select(y => y.Horometro.Economico).FirstOrDefault().TakeWhile(c => Char.IsLetter(c)).ToArray()),
                    RendTeorico = 0,
                    RendReal = decimal.Round((r.Select(y => y.Combustible.Horometro).FirstOrDefault() / r.Select(y => y.Combustible.HorasTrabajo).FirstOrDefault()) / r.Select(y => y.Combustible.HorasTrabajo).FirstOrDefault(), 2),
                    Rendimiento = 0
                }).ToList();

                var capturados = _context.tblK_CapturaMaq.Select(x => new { x.ccObra, x.fecha, x.economico, x.turno }).ToList();

                var resultadoFiltrado = resultado.Where(x => 
                    !(capturados.Select(y => y.ccObra).Contains(x.cc) && 
                    capturados.Select(y => y.fecha.ToShortDateString()).Contains(x.fecha) && 
                    capturados.Select(y => y.economico).Contains(x.economico) &&
                    capturados.Select(y => y.turno).Contains(x.turno))).ToList();

                lst.Add(resultadoFiltrado);
            }

            return lst
                .Cast<object>()
                .ToList();
        }

        public void CapturarMaq(List<CapturaMaqDTO> arr)
        {
            try
            {
                foreach (var item in arr)
                {
                    tblK_CapturaMaq capt = new tblK_CapturaMaq();

                    capt.ccObra = item.ccObra;
                    capt.fecha = DateTime.Parse(item.fecha);
                    capt.economico = item.economico;
                    capt.turno = Int32.Parse(item.turno);
                    capt.horoInicial = Convert.ToDecimal(item.horoInicial, CultureInfo.InvariantCulture);
                    capt.horoFinal = Convert.ToDecimal(item.horoFinal, CultureInfo.InvariantCulture);
                    capt.paroClima = Int32.Parse(item.paroClima);
                    capt.hrsMtto = Convert.ToDecimal(item.hrsMtto, CultureInfo.InvariantCulture);
                    capt.horasTrab = Convert.ToDecimal(item.horasTrab, CultureInfo.InvariantCulture);
                    capt.horasProg = Convert.ToDecimal(item.horasProg, CultureInfo.InvariantCulture);
                    capt.horasEfectivas = Convert.ToDecimal(item.horasEfectivas, CultureInfo.InvariantCulture);
                    capt.eficiencia = Convert.ToDecimal(item.eficiencia, CultureInfo.InvariantCulture);
                    capt.consumo = Convert.ToDecimal(item.consumo, CultureInfo.InvariantCulture);
                    capt.grupoEquipo = item.grupoEquipo;
                    capt.rendTeorico = Convert.ToDecimal(item.rendTeorico, CultureInfo.InvariantCulture);
                    capt.rendReal = Convert.ToDecimal(item.rendReal, CultureInfo.InvariantCulture);
                    capt.rendimiento = Convert.ToDecimal(item.rendimiento, CultureInfo.InvariantCulture);

                    _context.tblK_CapturaMaq.Add(capt);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return ((d1 == d2) && (d1 >= d2));
        }

        public List<FactDTO> lstExplosion()
        {
            var cosulta = @"SELECT e.cc 
                                ,i.insumo
                                ,i.unidad
                                ,e.cantidad
                                ,e.precio
                                ,(e.cantidad * e.precio) as monto
                                ,i.descripcion
                                ,(SELECT TOP 1 p.cta + '-' + p.scta + '-' + p.sscta 
                                    FROM si_interfase_contabilidad_especifico p 
                                        WHERE p.tipo = i.tipo 
                                        AND p.grupo_ini >= i.tipo 
                                        AND p.grupo_fin <= i.tipo 
                                    ORDER BY cta DESC) as cta
                                 FROM so_explos_mat e
                                 INNER JOIN insumos i ON i.insumo = e.insumo
                                 WHERE e.cantidad * e.precio <> 0";
            var resultado = (List<FactDTO>)_contextEnkontrol.Where(cosulta).ToObject<List<FactDTO>>();
            return resultado;
        }
        public List<tblK_CatAvance> lstArchivos()
        {
            return _context.tblK_CatAvance.ToList();
        }
    }
}

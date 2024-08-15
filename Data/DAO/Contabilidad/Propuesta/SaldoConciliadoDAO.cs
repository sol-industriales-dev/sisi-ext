using Core.DAO.Contabilidad.Propuesta;
using Core.DTO;
using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Menus;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class SaldoConciliadoDAO : GenericDAO<tblC_SaldoConciliado>, ISaldoConciliadoDAO
    {
        MenuFactoryServices mfs = new MenuFactoryServices();
        #region Guardar
        public bool setConciliacion(List<ConcentradoDTO> lst)
        {
            var ultimoId = 0;
            var ahora = DateTime.Now;
            var semanaAnterior = ahora.AddDays(-7);
            var primero = lst.FirstOrDefault();
            var lstConciliado = _context.tblC_SaldoConciliado.ToList()
                .Where(c => c.esActivo)
                .Where(c => c.fecha.Year >= semanaAnterior.Year)
                .Where(c => c.fecha.noSemana() >= semanaAnterior.noSemana())
                .ToList();
            var lstConcentrado = _context.tblC_SaldoConcentrado.ToList()
                .Where(c => c.esActivo)
                .Where(c => c.fecha.Year >= semanaAnterior.Year)
                .Where(c => c.fecha.noSemana() >= semanaAnterior.noSemana())
                .ToList();
            lstConciliado.Where(c => c.fecha.noSemana().Equals(ahora.noSemana()))
                .ToList().ForEach(c =>
            {
                c.esActivo = false;
                _context.tblC_SaldoConciliado.AddOrUpdate(c);
                _context.SaveChanges();
            });
            var lstGrupo = lst.GroupBy(g => g.cc).ToList();
            lstGrupo.ForEach(conciliado =>
                {
                    var guardar = lstConciliado
                        .Where(c => c.fecha.noSemana().Equals(ahora.noSemana()))
                        .FirstOrDefault(c => c.cc.Equals(conciliado.Key));
                    if (guardar == null)
                        guardar = new tblC_SaldoConciliado()
                        {
                            cc = conciliado.Key,
                            fecha = ahora,
                        };
                    var Conci = lstConciliado.FirstOrDefault(c => c.cc.Equals(conciliado.Key));
                    var saldoConci = Conci == null ? 0 : Conci.saldo;
                    guardar.saldo = conciliado.Sum(s => s.cargo - s.abono) + saldoConci;
                    guardar.esActivo = true;
                    guardar.ultimoCambio = ahora;
                    _context.tblC_SaldoConciliado.AddOrUpdate(guardar);
                    SaveChanges();
                    ultimoId = guardar.id;
                    conciliado.ToList().ForEach(detalle =>
                    {
                        var moneda = detalle.sonDolares ? 2 : 1;
                        var folio = generarFolio(detalle);
                        var concentrado = lstConcentrado.FirstOrDefault(c => c.folio.Equals(folio));
                        if (concentrado == null)
                            concentrado = new tblC_SaldoConcentrado()
                            {
                                folio = folio
                            };
                        concentrado.noCheque = detalle.noCheque;
                        concentrado.fecha = detalle.fecha;
                        concentrado.cc = detalle.cc;
                        concentrado.moneda = moneda;
                        concentrado.idSaldoConciliado = guardar.id;
                        concentrado.cargo = detalle.cargo;
                        concentrado.abono = detalle.abono;
                        concentrado.concepto = detalle.concepto;
                        concentrado.beneficiario = detalle.beneficiario;
                        concentrado.esActivo = true;
                        concentrado.ultimoCambio = ahora;
                        _context.tblC_SaldoConcentrado.AddOrUpdate(concentrado);
                        SaveChanges();
                    });
                });
            if (mfs.getMenuService().isLiberado(vSesiones.sesionCurrentView))
            {
                SaveBitacora((int)BitacoraEnum.PropuestaSaldoConciliado, (int)AccionEnum.AGREGAR, ultimoId, JsonUtils.convertNetObjectToJson(primero));
                SaveChanges();
            }
            return ultimoId > 0;
        }
        #endregion
        string generarFolio(ConcentradoDTO obj)
        {
            return string.Format("{0:yyMM}-{1}{2:000000}-{3}-{4:000000}-{5}", obj.fecha, obj.tp, obj.poliza, obj.cc, obj.noCheque.ParseInt(), obj.sonDolares ? "DLL" : "NMX");
        }
        public List<tblC_SaldoConciliado> getLstSaldosConciliados(BusqConcentradoDTO busq)
        {
            return _context.tblC_SaldoConciliado.ToList()
                .Where(sc => sc.esActivo)
                .Where(sc => sc.fecha.Year.Equals(busq.min.Year))
                .Where(sc => sc.fecha.noSemana().Equals(busq.min.noSemana()))
                .ToList();
        }
        public List<tblC_SaldoConciliado> getLstSaldosConciliadosAnterior(BusqConcentradoDTO busq)
        {
            var semanaAnterior = new DateTime(busq.min.Year, busq.min.Month, busq.min.Day);
            semanaAnterior = semanaAnterior.AddDays(-7);
            return _context.tblC_SaldoConciliado.ToList()
                .Where(sc => sc.esActivo)
                .Where(sc => sc.fecha.Year.Equals(semanaAnterior.Year))
                .Where(sc => sc.fecha.noSemana().Equals(semanaAnterior.noSemana()))
                .Where(sc => busq.lstCC.Any(c => c.Equals(sc.cc)))
                .ToList();
        }
    }
}

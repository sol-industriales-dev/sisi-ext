using Core.DAO.Administracion.ControlInterno.Reporte;
using Core.DTO;
using Core.DTO.Administracion.ControlInterno.Reporte;
using Core.Entity.Administrativo.cotizaciones;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Administracion.ControlInterno.Reporte
{
    public class RepTrapasoDAO : GenericDAO<tblAlm_Movimientos>, IRepTrapasoDAO
    {
        public List<RepTraspasoDTO> getLstMovCerrados(string cc, string folio, string almacen, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<RepTraspasoDTO> listaTraspasos = new List<RepTraspasoDTO>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERÚ
                            listaTraspasos = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 52 && x.orden_ct > 0).ToList().Join(
                                _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.orden_ct > 0).ToList(),
                                sal => new { orden_ct = sal.orden_ct, almacen = sal.almacen, cc = sal.cc, alm_destino = sal.alm_destino, cc_destino = sal.cc_destino },
                                ent => new { orden_ct = ent.orden_ct, almacen = ent.alm_destino, cc = ent.cc_destino, alm_destino = ent.almacen, cc_destino = ent.cc },
                                (sal, ent) => new { sal, ent }
                            ).Where(x =>
                                x.ent.fecha.Date >= fechaIni.Date && x.ent.fecha.Date <= fechaFin.Date &&
                                (!string.IsNullOrEmpty(cc) ? x.sal.cc_destino == cc : true) &&
                                (!string.IsNullOrEmpty(folio) ? x.sal.orden_ct == Int32.Parse(folio) : true) &&
                                (!string.IsNullOrEmpty(almacen) ? x.sal.alm_destino == Int32.Parse(almacen) : true) &&
                                x.sal.total == x.ent.total
                            ).Select(x => new RepTraspasoDTO
                            {
                                folio = x.sal.orden_ct.ToString(),
                                mov52 = x.sal.tipo_mov.ToString(),
                                mov2 = x.ent.tipo_mov.ToString(),
                                almacen_origen = x.sal.almacen.ToString(),
                                almacen_destino = x.ent.almacen.ToString(),
                                numero52 = x.sal.numero.ToString(),
                                numero2 = x.ent.numero.ToString(),
                                fecha_salida = x.sal.fecha,
                                fecha_entrada = x.ent.fecha,
                                cc = x.sal.cc,
                                totalSalida = x.sal.total,
                                totalEntrada = x.ent.total,
                            }).ToList();
                            #endregion
                            break;
                        }
                    default:
                        {
                            #region DEMÁS EMPRESAS
                            var condicionTotalString = "";

                            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                            {
                                case EmpresaEnum.Construplan:
                                case EmpresaEnum.Arrendadora:
                                    condicionTotalString = "((movSal.total - movEnt.total) <= 50)";
                                    break;
                                default:
                                    condicionTotalString = "movSal.total = movEnt.total";
                                    break;
                            }

                            listaTraspasos = (List<RepTraspasoDTO>)_contextEnkontrol.Where(string.Format(@"
                                SELECT 
                                    movSal.folio_traspaso AS folio, 
                                    movSal.tipo_mov AS mov52, 
                                    movEnt.tipo_mov AS mov2, 
                                    movSal.almacen AS almacen_origen, 
                                    movEnt.almacen AS almacen_destino, 
                                    movSal.numero AS numero52, 
                                    movEnt.numero AS numero2, 
                                    movSal.fecha AS fecha_salida, 
                                    movEnt.fecha AS fecha_entrada, 
                                    movSal.cc AS cc, 
                                    movSal.total AS totalSalida, 
                                    movEnt.total AS totalEntrada 
                                FROM si_movimientos AS movSal 
                                    INNER JOIN si_movimientos AS movEnt ON movSal.folio_traspaso = movEnt.folio_traspaso 
                                WHERE 
                                    (movSal.tipo_mov = 52 AND movSal.folio_traspaso IS NOT NULL AND movSal.folio_traspaso != 0) AND 
                                    (movEnt.tipo_mov = 2 AND movEnt.folio_traspaso IS NOT NULL AND movEnt.folio_traspaso != 0) AND 
                                    (movEnt.fecha >= '{0}' AND movEnt.fecha <= '{1}') AND 
                                    movSal.alm_destino = movEnt.almacen 
                                    {2} 
                                    {3} 
                                    {4} AND 
                                    movSal.cc_destino = movEnt.cc AND 
                                    movSal.almacen = movEnt.alm_destino AND 
                                    movSal.cc = movEnt.cc_destino AND 
                                    {5} 
                                ORDER BY movSal.folio_traspaso",
                                fechaIni.ToString("yyyyMMdd"),
                                fechaFin.ToString("yyyyMMdd"),
                                (cc.Equals("") ? "" : ("AND movSal.cc_destino = '" + cc + "' ")),
                                (folio.Equals("") ? "" : ("AND movSal.folio_traspaso = " + folio + " ")),
                                (almacen.Equals("") ? "" : (" AND (movSal.alm_destino = " + almacen + ") ")),
                                condicionTotalString
                            )).ToObject<List<RepTraspasoDTO>>();
                            #endregion
                            break;
                        }
                }

                foreach (var traspaso in listaTraspasos)
                {
                    traspaso.strfecha_entrada = traspaso.fecha_entrada.ToShortDateString();
                    traspaso.strfecha_salida = traspaso.fecha_salida.ToShortDateString();
                    traspaso.mov52 = string.Format("{0:C}", traspaso.totalSalida); //Se usa la propiedad "mov52" para mostrar el importe de la salida.
                    traspaso.mov2 = string.Format("{0:C}", traspaso.totalEntrada); //Se usa la propiedad "mov2" para mostrar el importe de la entrada.
                }

                return listaTraspasos;
            }
            catch (Exception)
            {
                return new List<RepTraspasoDTO>();
            }
        }

        public List<RepTraspasoDTO> getLstMovAbiertos(string cc, string folio, string almacen, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                List<RepTraspasoDTO> listaTraspasos = new List<RepTraspasoDTO>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERÚ
                            listaTraspasos = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 52 && x.orden_ct > 0).ToList().Join(
                                _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.orden_ct > 0).ToList(),
                                sal => new { orden_ct = sal.orden_ct, almacen = sal.almacen, cc = sal.cc, alm_destino = sal.alm_destino, cc_destino = sal.cc_destino },
                                ent => new { orden_ct = ent.orden_ct, almacen = ent.alm_destino, cc = ent.cc_destino, alm_destino = ent.almacen, cc_destino = ent.cc },
                                (sal, ent) => new { sal, ent }
                            ).Where(x =>
                                x.ent.fecha.Date >= fechaIni.Date && x.ent.fecha.Date <= fechaFin.Date &&
                                (!string.IsNullOrEmpty(cc) ? x.sal.cc_destino == cc : true) &&
                                (!string.IsNullOrEmpty(folio) ? x.sal.orden_ct == Int32.Parse(folio) : true) &&
                                (!string.IsNullOrEmpty(almacen) ? x.sal.alm_destino == Int32.Parse(almacen) : true) &&
                                ((x.sal.total - x.ent.total) > 50)
                            ).Select(x => new RepTraspasoDTO
                            {
                                folio = x.sal.orden_ct.ToString(),
                                mov52 = x.sal.tipo_mov.ToString(),
                                mov2 = x.ent.tipo_mov.ToString(),
                                almacen_origen = x.sal.almacen.ToString(),
                                almacen_destino = x.ent.almacen.ToString(),
                                numero52 = x.sal.numero.ToString(),
                                numero2 = x.ent.numero.ToString(),
                                fecha_salida = x.sal.fecha,
                                fecha_entrada = x.ent.fecha,
                                cc = x.sal.cc,
                                totalSalida = x.sal.total,
                                totalEntrada = x.ent.total,
                                fecha = x.sal.fecha,
                                total = x.sal.total
                            }).ToList();
                            #endregion
                            break;
                        }
                    default:
                        {
                            #region DEMÁS EMPRESAS
                            var condicionTotalString = "";

                            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                            {
                                case EmpresaEnum.Construplan:
                                case EmpresaEnum.Arrendadora:
                                    condicionTotalString = "((movSal.total - totalEntradas) > 50)";
                                    break;
                                default:
                                    condicionTotalString = "movSal.total > totalEntradas";
                                    break;
                            }

                            listaTraspasos = (List<RepTraspasoDTO>)_contextEnkontrol.Where(string.Format(@"
                                SELECT 
                                    movSal.folio_traspaso AS folio, 
                                    movSal.almacen AS almacen_origen, 
                                    movSal.numero AS mov52, 
                                    movSal.alm_destino AS almacen_destino, 
                                    movSal.fecha, 
                                    movSal.cc, 
                                    movSal.total, 
                                    (
                                        SELECT SUM(total) 
                                        FROM si_movimientos movEnt 
                                        WHERE movEnt.tipo_mov = 2 AND movEnt.folio_traspaso = movSal.folio_traspaso AND movEnt.almacen = movSal.alm_destino AND movEnt.cc = movSal.cc_destino AND movEnt.alm_destino = movSal.almacen AND movEnt.cc_destino = movSal.cc 
                                    ) AS totalEntradas 
                                FROM si_movimientos AS movSal 
                                WHERE 
                                    (movSal.tipo_mov = 52 AND movSal.folio_traspaso IS NOT NULL AND movSal.folio_traspaso != 0) AND 
                                    (movSal.fecha >= '{0}' AND movSal.fecha <= '{1}') AND 
                                    movSal.alm_destino != 997 AND
                                    (totalEntradas IS NULL OR {2}) {3} {4} {5}",
                                fechaIni.ToString("yyyyMMdd"),
                                fechaFin.ToString("yyyyMMdd"),
                                condicionTotalString,
                                (string.IsNullOrEmpty(cc) ? "" : ("AND movSal.cc_destino = '" + cc + "' ")),
                                (string.IsNullOrEmpty(folio) ? "" : ("AND movSal.folio_traspaso = " + folio + " ")),
                                (string.IsNullOrEmpty(almacen) ? "" : ("AND movSal.alm_destino = " + almacen + " "))
                            )).ToObject<List<RepTraspasoDTO>>();
                            #endregion
                            break;
                        }
                }

                foreach (var traspaso in listaTraspasos)
                {
                    traspaso.strfecha = traspaso.fecha.ToShortDateString();
                    traspaso.numero2 = string.Format("{0:C}", traspaso.total);
                }

                return listaTraspasos;
            }
            catch (Exception)
            {
                return new List<RepTraspasoDTO>();
            }
        }
    }
}

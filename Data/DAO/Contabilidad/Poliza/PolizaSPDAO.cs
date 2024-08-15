using Core.DAO.Contabilidad.Poliza;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.Poliza;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Poliza
{
    public class PolizaSPDAO : IPolizaSPDAO
    {
        private DbContextTransaction _transaccion;
        private MainContext _ctx;

        public void SetContext(object context)
        {
            _ctx = context as MainContext;
        }

        public void SetTransaccion(object transaccion)
        {
            _transaccion = transaccion as DbContextTransaction;
        }

        public object GetTransaccion()
        {
            return _transaccion;
        }

        public string GuardarPoliza<Tpoliza, Tmovimientos>(Tpoliza poliza, List<Tmovimientos> movimientos)
        {
            tblC_Poliza polizaDTO = null;
            List<tblC_Movpol> movimientosDTO = null;

            if (poliza is sc_polizasDTO)
            {
                polizaDTO = sc_polizaDTO_a_tblC_Poliza(poliza as sc_polizasDTO);
            }
            else
            {
                polizaDTO = poliza as tblC_Poliza;
            }
            if (movimientos is List<sc_movpolDTO>)
            {
                movimientosDTO = sc_movpolDTO_a_TblC_Movpol(movimientos as List<sc_movpolDTO>);
            }
            else
            {
                movimientosDTO = movimientos as List<tblC_Movpol>;
            }

            //var numeroPoliza = GetNumeroPolizaNueva(polizaDTO.year, polizaDTO.mes, polizaDTO.tp);

            polizaDTO.estatus = true;
            polizaDTO.fechaCreacion = DateTime.Now;
            polizaDTO.fechaModificacion = polizaDTO.fechaCreacion;
            //polizaDTO.poliza = numeroPoliza;
            polizaDTO.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
            polizaDTO.usuarioModificacionId = polizaDTO.usuarioCreacionId;

            _ctx.tblC_Poliza.Add(polizaDTO);
            _ctx.SaveChanges();

            foreach (var movimiento in movimientosDTO)
            {
                movimiento.polizaId = polizaDTO.id;
                movimiento.poliza = polizaDTO.poliza;
            }

            _ctx.tblC_Movpol.AddRange(movimientosDTO);
            _ctx.SaveChanges();

            //return polizaDTO.year + "-" + polizaDTO.mes + "-" + numeroPoliza + "-" + polizaDTO.tp;
            return polizaDTO.year + "-" + polizaDTO.mes + "-" + polizaDTO.poliza + "-" + polizaDTO.tp;
        }

        public int GetNumeroPolizaNueva(int year, int mes, string tp)
        {
            return _ctx.tblC_Poliza
                .Where(w =>
                    w.year == year &&
                    w.mes == mes &&
                    w.tp == tp
                ).Select(m => m.poliza).OrderByDescending(o => o).FirstOrDefault() + 1;
        }

        public bool ReferenciaDisponible(DateTime fechapol, int cta, int referencia)
        {
            throw new NotImplementedException();
        }

        public bool GuardarParaConciliar<T>(T chequera)
        {
            throw new NotImplementedException();
        }

        private tblC_Poliza sc_polizaDTO_a_tblC_Poliza(sc_polizasDTO poliza)
        {
            var polizaSP = new tblC_Poliza();

            polizaSP.year = poliza.year;
            polizaSP.mes = poliza.mes;
            polizaSP.poliza = poliza.poliza;
            polizaSP.tp = poliza.tp;
            polizaSP.fechapol = poliza.fechapol;
            polizaSP.cargos = poliza.cargos;
            polizaSP.abonos = poliza.abonos;
            polizaSP.generada = poliza.generada ?? "C";
            polizaSP.status = poliza.status ?? "C";
            polizaSP.error = poliza.error ?? "";
            polizaSP.status_lock = poliza.status_lock ?? "N";
            polizaSP.fec_hora_movto = poliza.fec_hora_movto;
            polizaSP.usuario_movto = poliza.usuario_movto;
            polizaSP.fecha_hora_crea = poliza.fecha_hora_crea;
            polizaSP.usuario_crea = poliza.usuario_crea;
            polizaSP.socio_inversionista = poliza.socio_inversionista;
            polizaSP.status_carga_pol = poliza.status_carga_pol ?? "";
            polizaSP.concepto = poliza.concepto;

            return polizaSP;
        }

        private List<tblC_Movpol> sc_movpolDTO_a_TblC_Movpol(List<sc_movpolDTO> movimientos)
        {
            var movimientosSP = new List<tblC_Movpol>();

            foreach (var mov in movimientos)
            {
                var movSP = new tblC_Movpol();

                movSP.year = mov.year;
                movSP.mes = mov.mes;
                movSP.poliza = mov.poliza;
                movSP.tp = mov.tp;
                movSP.linea = mov.linea;
                movSP.cta = mov.cta;
                movSP.scta = mov.scta;
                movSP.sscta = mov.sscta;
                movSP.digito = mov.digito;
                movSP.tm = mov.tm;
                movSP.referencia = mov.referencia;
                movSP.cc = mov.cc;
                movSP.concepto = mov.concepto;
                movSP.monto = mov.monto;
                movSP.iclave = mov.iclave;
                movSP.itm = mov.itm;
                movSP.st_par = mov.st_par ?? "";
                movSP.orden_compra = mov.orden_compra ?? 0;
                movSP.numpro = mov.numpro;
                movSP.socio_inversionista = mov.socio_inversionista;
                movSP.istm = mov.istm;
                movSP.folio_imp = mov.folio_imp;
                movSP.linea_imp = mov.linea_imp;
                movSP.num_emp = mov.num_emp;
                movSP.folio_gxc = mov.folio_gxc;
                movSP.cfd_ruta_pdf = mov.cfd_ruta_pdf ?? "";
                movSP.cfd_ruta_xml = mov.cfd_ruta_xml ?? "";
                movSP.UUID = mov.UUID;
                movSP.cfd_rfc = mov.cfd_rfc;
                movSP.cfd_tipocambio = mov.cfd_tipocambio;
                movSP.cfd_total = mov.cfd_total;
                movSP.cfd_moneda = mov.cfd_moneda;
                movSP.metodo_pago_sat = mov.metodo_pago_sat;
                movSP.ruta_comp_ext = mov.ruta_comp_ext;
                movSP.factura_comp_ext = mov.factura_comp_ext;
                movSP.taxid = mov.taxid;
                movSP.forma_pago = mov.forma_pago;
                movSP.cfd_fecha_expedicion = mov.cfd_fecha_expedicion;
                movSP.cfd_tipocomprobante = mov.cfd_tipocomprobante;
                movSP.cfd_metodo_pago_sat = mov.cfd_metodo_pago_sat;
                movSP.area = mov.area;
                movSP.cuenta_oc = mov.cuenta_oc;

                movimientosSP.Add(movSP);
            }

            return movimientosSP;
        }

        public string EstatusPoliza(int year, int mes, int poliza, string tp)
        {
            throw new NotImplementedException();
        }
    }
}

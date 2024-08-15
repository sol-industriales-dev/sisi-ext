using Core.DAO.Contabilidad.Poliza;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.Poliza;
using Core.DTO.Utils.Data;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.General.Poliza
{
    public class PolizaEkDAO : IPolizaSPDAO
    {
        private OdbcTransaction _transaccion;
        private OdbcConnection _ctx;

        public void SetContext(object context)
        {
            _ctx = context as OdbcConnection;
        }

        public void SetTransaccion(object transaccion)
        {
            _transaccion = transaccion as OdbcTransaction;
        }

        public object GetTransaccion()
        {
            return _transaccion;
        }

        public string GuardarPoliza<Tpoliza, Tmovimientos>(Tpoliza poliza, List<Tmovimientos> movimientos)
        {
            var polizaDTO = poliza as sc_polizasDTO;
            var movimientosDTO = movimientos as List<sc_movpolDTO>;

            var numeroPoliza = GetNumeroPolizaNueva(polizaDTO.year, polizaDTO.mes, polizaDTO.tp);

            var query_sc_polizas =
                @"INSERT INTO
                    dba.sc_polizas
                    (
                        year,
                        mes,
                        poliza,
                        tp,
                        fechapol,
                        cargos,
                        abonos,
                        generada,
                        status,
                        error,
                        status_lock,
                        fec_hora_movto,
                        usuario_movto,
                        fecha_hora_crea,
                        usuario_crea,
                        socio_inversionista,
                        status_carga_pol,
                        concepto
                    )
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            using (var cmd = new OdbcCommand(query_sc_polizas))
            {
                var parametros = cmd.Parameters;

                parametros.Add("@year", OdbcType.Numeric).Value = polizaDTO.year;
                parametros.Add("@mes", OdbcType.Numeric).Value = polizaDTO.mes;
                parametros.Add("@poliza", OdbcType.Numeric).Value = numeroPoliza;
                parametros.Add("@tp", OdbcType.Char).Value = polizaDTO.tp;
                parametros.Add("@fechapol", OdbcType.Date).Value = polizaDTO.fechapol;
                parametros.Add("@cargos", OdbcType.Numeric).Value = polizaDTO.cargos;
                parametros.Add("@abonos", OdbcType.Numeric).Value = polizaDTO.abonos;
                parametros.Add("@generada", OdbcType.Char).Value = polizaDTO.generada ?? "C";
                parametros.Add("@status", OdbcType.Char).Value = polizaDTO.status ?? "C";
                parametros.Add("@error", OdbcType.VarChar).Value = polizaDTO.error ?? "";
                parametros.Add("@status_lock", OdbcType.Char).Value = polizaDTO.status_lock ?? "N";
                parametros.Add("@fec_hora_movto", OdbcType.DateTime).Value = polizaDTO.fec_hora_movto ?? (object)DBNull.Value;
                parametros.Add("@usuario_movto", OdbcType.Char).Value = polizaDTO.usuario_movto ?? (object)DBNull.Value;
                parametros.Add("@fecha_hora_crea", OdbcType.DateTime).Value = polizaDTO.fecha_hora_crea ?? (object)DBNull.Value;
                parametros.Add("@usuario_crea", OdbcType.Char).Value = polizaDTO.usuario_crea ?? (object)DBNull.Value;
                parametros.Add("@socio_inversionista", OdbcType.Numeric).Value = polizaDTO.socio_inversionista ?? (object)DBNull.Value;
                parametros.Add("@status_carga_pol", OdbcType.VarChar).Value = polizaDTO.status_carga_pol ?? "";
                parametros.Add("@concepto", OdbcType.VarChar).Value = polizaDTO.concepto ?? (object)DBNull.Value;

                cmd.Connection = _transaccion.Connection;
                cmd.Transaction = _transaccion;
                cmd.ExecuteNonQuery();
            }

            foreach (var movimiento in movimientosDTO)
            {
                var query_sc_movpol =
                @"INSERT INTO
                        dba.sc_movpol
                        (
                            year,
                            mes,
                            poliza,
                            tp,
                            linea,
                            cta,
                            scta,
                            sscta,
                            digito,
                            tm,
                            referencia,
                            cc,
                            concepto,
                            monto,
                            iclave,
                            itm,
                            st_par,
                            orden_compra,
                            numpro,
                            socio_inversionista,
                            istm,
                            folio_imp,
                            linea_imp,
                            num_emp,
                            folio_gxc,
                            cfd_ruta_pdf,
                            cfd_ruta_xml,
                            UUID,
                            cfd_rfc,
                            cfd_tipocambio,
                            cfd_total,
                            cfd_moneda,
                            metodo_pago_sat,
                            ruta_comp_ext,
                            factura_comp_ext,
                            taxid,
                            forma_pago,
                            cfd_fecha_expedicion,
                            cfd_tipocomprobante,
                            cfd_metodo_pago_sat,
                            area,
                            cuenta_oc
                        )
                        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                using (var cmd = new OdbcCommand(query_sc_movpol))
                {
                    OdbcParameterCollection parametros = cmd.Parameters;

                    parametros.Add("@year", OdbcType.Numeric).Value = movimiento.year;
                    parametros.Add("@mes", OdbcType.Numeric).Value = movimiento.mes;
                    parametros.Add("@poliza", OdbcType.Numeric).Value = numeroPoliza;
                    parametros.Add("@tp", OdbcType.Char).Value = movimiento.tp;
                    parametros.Add("@linea", OdbcType.Numeric).Value = movimiento.linea;
                    parametros.Add("@cta", OdbcType.Numeric).Value = movimiento.cta;
                    parametros.Add("@scta", OdbcType.Numeric).Value = movimiento.scta;
                    parametros.Add("@sscta", OdbcType.Numeric).Value = movimiento.sscta;
                    parametros.Add("@digito", OdbcType.Numeric).Value = movimiento.digito;
                    parametros.Add("@tm", OdbcType.Numeric).Value = movimiento.tm;
                    parametros.Add("@referencia", OdbcType.Char).Value = movimiento.referencia;
                    parametros.Add("@cc", OdbcType.Char).Value = movimiento.cc ?? (object)DBNull.Value;
                    parametros.Add("@concepto", OdbcType.Char).Value = movimiento.concepto;
                    parametros.Add("@monto", OdbcType.Numeric).Value = movimiento.monto;
                    parametros.Add("@iclave", OdbcType.Numeric).Value = movimiento.iclave;
                    parametros.Add("@itm", OdbcType.Numeric).Value = movimiento.itm;
                    parametros.Add("@st_par", OdbcType.Char).Value = movimiento.st_par ?? "";
                    parametros.Add("@orden_compra", OdbcType.Numeric).Value = movimiento.orden_compra ?? 0;
                    parametros.Add("@numpro", OdbcType.Numeric).Value = movimiento.numpro ?? (object)DBNull.Value;
                    parametros.Add("@socio_inversionista", OdbcType.Numeric).Value = movimiento.socio_inversionista ?? (object)DBNull.Value;
                    parametros.Add("@istm", OdbcType.Numeric).Value = movimiento.istm ?? (object)DBNull.Value;
                    parametros.Add("@folio_imp", OdbcType.Numeric).Value = movimiento.folio_imp ?? (object)DBNull.Value;
                    parametros.Add("@linea_imp", OdbcType.Numeric).Value = movimiento.linea_imp ?? (object)DBNull.Value;
                    parametros.Add("@num_emp", OdbcType.Int).Value = movimiento.num_emp ?? (object)DBNull.Value;
                    parametros.Add("@folio_gxc", OdbcType.Int).Value = movimiento.folio_gxc ?? (object)DBNull.Value;
                    parametros.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = movimiento.cfd_ruta_pdf ?? "";
                    parametros.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = movimiento.cfd_ruta_xml ?? "";
                    parametros.Add("@UUID", OdbcType.VarChar).Value = movimiento.UUID ?? (object)DBNull.Value;
                    parametros.Add("@cfd_rfc", OdbcType.VarChar).Value = movimiento.cfd_rfc ?? (object)DBNull.Value;
                    parametros.Add("@cfd_tipocambio", OdbcType.Numeric).Value = movimiento.cfd_tipocambio ?? (object)DBNull.Value;
                    parametros.Add("@cfd_total", OdbcType.Numeric).Value = movimiento.cfd_total ?? (object)DBNull.Value;
                    parametros.Add("@cfd_moneda", OdbcType.Char).Value = movimiento.cfd_moneda ?? (object)DBNull.Value;
                    parametros.Add("@metodo_pago_sat", OdbcType.Numeric).Value = movimiento.metodo_pago_sat ?? (object)DBNull.Value;
                    parametros.Add("@ruta_comp_ext", OdbcType.Char).Value = movimiento.ruta_comp_ext ?? (object)DBNull.Value;
                    parametros.Add("@factura_comp_ext", OdbcType.Char).Value = movimiento.factura_comp_ext ?? (object)DBNull.Value;
                    parametros.Add("@taxid", OdbcType.Char).Value = movimiento.taxid ?? (object)DBNull.Value;
                    parametros.Add("@forma_pago", OdbcType.VarChar).Value = movimiento.forma_pago ?? (object)DBNull.Value;
                    parametros.Add("@cfd_fecha_expedicion", OdbcType.SmallDateTime).Value = movimiento.cfd_fecha_expedicion ?? (object)DBNull.Value;
                    parametros.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = movimiento.cfd_tipocomprobante ?? (object)DBNull.Value;
                    parametros.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = movimiento.cfd_metodo_pago_sat ?? (object)DBNull.Value;
                    parametros.Add("@area", OdbcType.Numeric).Value = movimiento.area ?? (object)DBNull.Value;
                    parametros.Add("@cuenta_oc", OdbcType.Numeric).Value = movimiento.cuenta_oc ?? (object)DBNull.Value;

                    cmd.Connection = _transaccion.Connection;
                    cmd.Transaction = _transaccion;
                    cmd.ExecuteNonQuery();
                }
            }

            return polizaDTO.year + "-" + polizaDTO.mes + "-" + numeroPoliza + "-" + polizaDTO.tp;
        }

        public int GetNumeroPolizaNueva(int year, int mes, string tp)
        {
            var query_sc_polizas = new OdbcConsultaDTO();

            query_sc_polizas.consulta =
                @"SELECT TOP 1
                    poliza
                FROM
                    dba.sc_polizas
                WHERE
                    year = ? AND
                    mes = ? AND
                    tp = ?
                ORDER BY
                    poliza DESC";

            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "year",
                tipo = OdbcType.Int,
                valor = year
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "mes",
                tipo = OdbcType.Int,
                valor = mes
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "tp",
                tipo = OdbcType.Char,
                valor = tp
            });

            return _contextEnkontrol.Select<sc_polizasDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_polizas).Select(m => m.poliza).FirstOrDefault() + 1;
        }

        public bool ReferenciaDisponible(DateTime fechapol, int cta, int referencia)
        {
            var query_sc_movpol = new OdbcConsultaDTO();

            query_sc_movpol.consulta =
                @"SELECT TOP 1
                    referencia
                FROM
                    dba.sc_movpol AS MOV
                INNER JOIN
                    dba.sc_polizas AS POL
                    ON
                        POL.year = MOV.year AND
                        POL.mes = MOV.mes AND
                        POL.poliza = MOV.poliza AND
                        POL.tp = MOV.tp
                WHERE
                    MOV.year = ? AND
                    MOV.mes = ? AND
                    MOV.cta = ? AND
                    MOV.referencia = ?";

            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "year",
                tipo = OdbcType.Int,
                valor = fechapol.Year
            });
            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "mes",
                tipo = OdbcType.Int,
                valor = fechapol.Month
            });
            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "cta",
                tipo = OdbcType.Int,
                valor = cta
            });
            query_sc_movpol.parametros.Add(new OdbcParameterDTO
            {
                nombre = "referencia",
                tipo = OdbcType.VarChar,
                valor = referencia.ToString()
            });

            var existe = _contextEnkontrol.Select<sc_movpolDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_movpol).FirstOrDefault();

            return existe == null;
        }

        public bool GuardarParaConciliar<T>(T chequera)
        {
            var chequeraDTO = chequera as sb_edo_cta_chequeraDTO;

            var query_sb_edo_cta_chequera =
                @"INSERT INTO
                    dba.sb_edo_cta_chequera
                    (
                        cuenta,
                        fecha_mov,
                        tm,
                        numero,
                        cc,
                        descripcion,
                        monto,
                        tc,
                        origen_mov,
                        generada,
                        st_consilia,
                        num_consilia,
                        st_che,
                        ref_che_inverso,
                        ref_tm_inverso,
                        motivo_cancelado,
                        iyear,
                        imes,
                        ipoliza,
                        itp,
                        ilinea,
                        banco,
                        tp,
                        [desc],
                        folio,
                        tipo_iva,
                        porc_iva,
                        monto_iva,
                        folio_iva,
                        fecha_reten,
                        fecha_reten_fin,
                        id_num_credito,
                        prototipo,
                        consecutivo_minist,
                        acredita_iva,
                        clave_sub_tm,
                        folio_imp,
                        linea_imp
                    )
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            using (var cmd = new OdbcCommand(query_sb_edo_cta_chequera))
            {
                var parametros = cmd.Parameters;

                parametros.Add("@cuenta", OdbcType.Numeric).Value = chequeraDTO.cuenta;
                parametros.Add("@fecha_mov", OdbcType.DateTime).Value = chequeraDTO.fecha_mov;
                parametros.Add("@tm", OdbcType.Numeric).Value = chequeraDTO.tm;
                parametros.Add("@numero", OdbcType.Numeric).Value = chequeraDTO.numero;
                parametros.Add("@cc", OdbcType.VarChar).Value = chequeraDTO.cc;
                parametros.Add("@descripcion", OdbcType.VarChar).Value = chequeraDTO.descripcion ?? (object)DBNull.Value;
                parametros.Add("@monto", OdbcType.Decimal).Value = chequeraDTO.monto;
                parametros.Add("@tc", OdbcType.Decimal).Value = chequeraDTO.tc;
                parametros.Add("@origen_mov", OdbcType.Char).Value = chequeraDTO.origen_mov;
                parametros.Add("@generada", OdbcType.Char).Value = chequeraDTO.generada;
                parametros.Add("@st_consilia", OdbcType.Char).Value = chequeraDTO.st_consilia ?? (object)DBNull.Value;
                parametros.Add("@num_consilia", OdbcType.Int).Value = chequeraDTO.num_consilia ?? (object)DBNull.Value;
                parametros.Add("@st_che", OdbcType.Char).Value = chequeraDTO.st_che ?? "";
                parametros.Add("@ref_che_inverso", OdbcType.Int).Value = chequeraDTO.ref_che_inverso ?? (object)DBNull.Value;
                parametros.Add("@ref_tm_inverso", OdbcType.Int).Value = chequeraDTO.ref_tm_inverso ?? (object)DBNull.Value;
                parametros.Add("@motivo_cancelado", OdbcType.Char).Value = chequeraDTO.motivo_cancelado ?? "";
                parametros.Add("@iyear", OdbcType.Numeric).Value = chequeraDTO.iyear ?? (object)DBNull.Value;
                parametros.Add("@imes", OdbcType.Numeric).Value = chequeraDTO.imes ?? (object)DBNull.Value;
                parametros.Add("@ipoliza", OdbcType.Numeric).Value = chequeraDTO.ipoliza ?? (object)DBNull.Value;
                parametros.Add("@itp", OdbcType.VarChar).Value = chequeraDTO.itp ?? (object)DBNull.Value;
                parametros.Add("@ilinea", OdbcType.Numeric).Value = chequeraDTO.ilinea ?? (object)DBNull.Value;
                parametros.Add("@banco", OdbcType.Numeric).Value = chequeraDTO.banco ?? (object)DBNull.Value;
                parametros.Add("@tp", OdbcType.Char).Value = chequeraDTO.tp ?? (object)DBNull.Value;
                parametros.Add("@desc", OdbcType.Char).Value = chequeraDTO.desc ?? (object)DBNull.Value;
                parametros.Add("@folio", OdbcType.Numeric).Value = chequeraDTO.folio ?? (object)DBNull.Value;
                parametros.Add("@tipo_iva", OdbcType.Char).Value = chequeraDTO.tipo_iva ?? (object)DBNull.Value;
                parametros.Add("@porc_iva", OdbcType.Numeric).Value = chequeraDTO.porc_iva ?? (object)DBNull.Value;
                parametros.Add("@monto_iva", OdbcType.Numeric).Value = chequeraDTO.monto_iva ?? (object)DBNull.Value;
                parametros.Add("@folio_iva", OdbcType.Numeric).Value = chequeraDTO.folio_iva ?? (object)DBNull.Value;
                parametros.Add("@fecha_reten", OdbcType.DateTime).Value = chequeraDTO.fecha_reten ?? (object)DBNull.Value;
                parametros.Add("@fecha_reten_fin", OdbcType.DateTime).Value = chequeraDTO.fecha_reten_fin ?? (object)DBNull.Value;
                parametros.Add("@id_num_credito", OdbcType.Numeric).Value = chequeraDTO.id_num_credito ?? (object)DBNull.Value;
                parametros.Add("@prototipo", OdbcType.Char).Value = chequeraDTO.prototipo ?? (object)DBNull.Value;
                parametros.Add("@consecutivo_minist", OdbcType.Numeric).Value = chequeraDTO.consecutivo_minist ?? (object)DBNull.Value;
                parametros.Add("@acredita_iva", OdbcType.Char).Value = chequeraDTO.acredita_iva ?? (object)DBNull.Value;
                parametros.Add("@clave_sub_tm", OdbcType.Numeric).Value = chequeraDTO.clave_sub_tm ?? (object)DBNull.Value;
                parametros.Add("@folio_imp", OdbcType.Numeric).Value = chequeraDTO.folio_imp ?? (object)DBNull.Value;
                parametros.Add("@linea_imp", OdbcType.Numeric).Value = chequeraDTO.linea_imp ?? (object)DBNull.Value;

                cmd.Connection = _transaccion.Connection;
                cmd.Transaction = _transaccion;
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public string EstatusPoliza(int year, int mes, int poliza, string tp)
        {
            var query_sc_polizas = new OdbcConsultaDTO();

            query_sc_polizas.consulta =
                @"SELECT
                    status
                FROM
                    dba.sc_polizas AS POL
                WHERE
                    POL.year = ? AND
                    POL.mes = ? AND
                    POL.poliza = ? AND
                    POL.tp = ?";

            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "year",
                tipo = OdbcType.Int,
                valor = year
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "mes",
                tipo = OdbcType.Int,
                valor = mes
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "poliza",
                tipo = OdbcType.Int,
                valor = poliza
            });
            query_sc_polizas.parametros.Add(new OdbcParameterDTO
            {
                nombre = "tp",
                tipo = OdbcType.VarChar,
                valor = tp
            });

            var existe = _contextEnkontrol.Select<sc_polizasDTO>(vSesiones.sesionAmbienteEnkontrolAdm, query_sc_polizas).FirstOrDefault();

            return existe != null ? existe.status : null;
        }
    }
}

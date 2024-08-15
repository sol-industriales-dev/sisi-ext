using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class FacturaDTO
    {
        public int factura { get; set; }
        public int numcte { get; set; }
        public int sucursal { get; set; }
        public DateTime fecha { get; set; }
        public int pedido { get; set; }
        public int remision { get; set; }
        public string requisicion { get; set; }
        public int vendedor { get; set; }
        public string consignado { get; set; }
        public string transporte { get; set; }
        public int cond_pago { get; set; }
        public int talon { get; set; }
        public string moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public decimal porcent_iva { get; set; }
        public string tipo { get; set; }
        public int lista_precio { get; set; }
        public string cc { get; set; }
        public int tm { get; set; }
        public decimal descuento { get; set; }
        public decimal porcent_descto { get; set; }
        public string nombre { get; set; }
        public string rfc { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public string estatus { get; set; }
        public string obs { get; set; }
        public string compania { get; set; }
        public string talon_emb { get; set; }
        public string camion { get; set; }
        public string placas { get; set; }
        public string pais { get; set; }
        public string estado { get; set; }
        public string tipo_clase { get; set; }
        public int cia_sucursal { get; set; }
        public int numero_nc { get; set; }
        public int tipo_nc { get; set; }
        public string status_notacredito { get; set; }
        public decimal retencion { get; set; }
        public int? cfd_folio { get; set; }
        public Nullable<DateTime> cfd_fecha { get; set; }
        public string cfd_no_certificado { get; set; }
        public int? cfd_ano_aprob { get; set; }
        public int? cfd_num_aprob { get; set; }
        public string cfd_cadena_original { get; set; }
        public string cfd_certificado_sello { get; set; }
        public string cfd_sello_digital { get; set; }
        public string cfd_serie { get; set; }
        public Nullable<DateTime> fecha_cancelacion { get; set; }
        public int? usuario_cancela { get; set; }
        public decimal? cfd_subtotal { get; set; }
        public decimal? cfd_descuento { get; set; }
        public decimal? cfd_iva { get; set; }
        public decimal? cfd_retenciones { get; set; }
        public decimal? cfd_total { get; set; }
        public decimal? enviado { get; set; }
        public string tipo_documento { get; set; }
        public string autoriza_factura { get; set; }
        public int? empleado_autoriza { get; set; }
        public Nullable<DateTime> fecha_autoriza { get; set; }
        public int? idPac { get; set; }
        public string gsdb { get; set; }
        public string cve_formulario { get; set; }
        public string cfd_mn { get; set; }
        public string cfd_enviada { get; set; }
        public decimal? cfd_ret_iva { get; set; }
        public int? id_metodo_pago { get; set; }
        public int? id_regimen_fiscal { get; set; }
        public string cfd_num_cta_pago { get; set; }
        public string comentarios_cfdi { get; set; }
        public int? bit_cfdi { get; set; }
        public string uuid { get; set; }
        public Nullable<DateTime> fecha_timbrado { get; set; }
        public string certificado_sat { get; set; }
        public string sello_sat { get; set; }
        public string asn { get; set; }

    }
}

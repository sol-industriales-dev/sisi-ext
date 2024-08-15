using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class BigFacturaDTO
    {
        #region Pedido
        public int pedido { get; set; }
        public string cc { get; set; }
        public int cia_sucursal { get; set; }
        public int cond_pago { get; set; }
        public string condicion_entrega { get; set; }
        public decimal descuento { get; set; }
        public int elaboro { get; set; }
        public DateTime fecha { get; set; }
        public decimal iva { get; set; }
        public string moneda { get; set; }
        public string obs { get; set; }
        public decimal porcent_descto { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal sub_total { get; set; }
        public int sucursal { get; set; }
        public decimal tipo_cambio { get; set; }
        public string tipo_flete { get; set; }
        public string tipo_pedido { get; set; }
        public string tipo { get; set; }
        public string tipo_credito { get; set; }
        public int tm { get; set; }
        public decimal total_dec { get; set; }
        public int vendedor { get; set; }
        public int zona { get; set; }
        public string requisicion { get; set; }
        public decimal retencion { get; set; }
        public decimal total { get; set; }
        public decimal aplica_total_antes_iva { get; set; }
        #endregion
        #region Cliente
        public int numcte { get; set; }
        public string nombre { get; set; }
        public string telefono1 { get; set; }
        public string cp { get; set; }
        public string nomcorto { get; set; }
        public string rfc { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        #endregion
        #region Remision
        public int remision { get; set; }
        public string transporte { get; set; }
        public string talon { get; set; }
        public string consignado { get; set; }
        public string entregado { get; set; }
        #endregion
        #region Factura
        public int factura { get; set; }
        public int id_regimen_fiscal { get; set; }
        public string gsdb { get; set; }
        public string asn { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }
        public int id_metodo_pago { get; set; }
        public int cfd_num_cta_pago { get; set; }
        public string cfd_serie { get; set; }
        public string cfd_folio { get; set; }
        public string tipo_clase { get; set; }
        public string cfd_no_certificado { get; set; }
        public string cfd_certificado_sello { get; set; }
        public string cfd_cadena_original { get; set; }
        #endregion
        public string UsuarioNombre { get; set; }
    }
}

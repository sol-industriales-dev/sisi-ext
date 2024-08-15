using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.Poliza
{
    public class sp_movprovDTO
    {
        public int numpro { get; set; }
        public int factura { get; set; }
        public DateTime fecha { get; set; }
        public int tm { get; set; }
        public DateTime fechavenc { get; set; }        
        public string concepto { get; set; }
        public string cc { get; set; }
        public int referenciaoc { get; set; }
        public decimal monto { get; set; }
        public decimal tipocambio { get; set; }
        public decimal iva { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public string generado { get; set; }
        public string es_factura { get; set; }
        public string moneda { get; set; }
        public string autorizapago { get; set; }
        public decimal total { get; set; }
        public string st_pago { get; set; }
        public int folio { get; set; }
        public decimal autoincremento { get; set; }
        public decimal tipocambio_oc { get; set; }
        public decimal empleado_modifica { get; set; }
        public DateTime fecha_modifica { get; set; }
        public TimeSpan hora_modifica { get; set; }
        public string pago_factoraje { get; set; }
        public decimal inst_factoraje { get; set; }
        public string st_factoraje { get; set; }
        public decimal socio_inversionista { get; set; }
        public string bit_factoraje { get; set; }
        public string bit_autoriza { get; set; }
        public string bit_transferida { get; set; }
        public string bit_pagada { get; set; }
        public decimal empleado { get; set; }
        public int folio_gxc { get; set; }
        public string cfd_serie { get; set; }
        public decimal cfd_folio { get; set; }
        public DateTime cfd_fecha { get; set; }
        public string cfd_certificado { get; set; }
        public decimal cfd_ano_aprob { get; set; }
        public decimal cfd_num_aprob { get; set; }
        public string ruta_rec_xml { get; set; }
        public string ruta_rec_pdf { get; set; }
        public string afectacompra { get; set; }
        public string val_ref { get; set; }
        public decimal suma_o_resta { get; set; }
        public string pide_iva { get; set; }
        public string valida_recibido { get; set; }
        public string valida_almacen { get; set; }
        public string valida_recibido_autorizar { get; set; }
        public decimal empleado_autorizo { get; set; }
        public DateTime fecha_autoriza { get; set; }
        public decimal empleado_vobo { get; set; }
        public DateTime fecha_vobo { get; set; }
        public string UUID { get; set; }
        public decimal tipo_factoraje { get; set; }
        public decimal folio_retencion { get; set; }
        public string cfd_metodo_pago { get; set; }
        public string moneda_oc_nom { get; set; }
        public string moneda_oc { get; set; }
        public int usuario { get; set; }               
    }
}

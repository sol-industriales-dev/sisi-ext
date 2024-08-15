using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Proveedores
{
    public class sp_gastos_prov
    {
        public int numpro { get; set; }
        public string cfd_serie { get; set; }
        public Int64 cfd_folio { get; set; }
        public string referenciaoc { get; set; }
        public int tipo_prov { get; set; }
        public string cc { get; set; }
        public int tm { get; set; }
        public string factura { get; set; }
        public decimal monto { get; set; }
        public decimal iva { get; set; }
        public decimal tipocambio { get; set; }
        public decimal total { get; set; }
        public string fecha_timbrado { get; set; }
        public string estatus { get; set; }
        public string concepto { get; set; }
        public DateTime fecha { get; set; }
        public string moneda { get; set; }
        public string uuid { get; set; }
        public string ruta_rec_xml { get; set; }
        public string ruta_rec_pdf { get; set; }
        public DateTime fecha_autoriza_portal { get; set; }
        public decimal descuento { get; set; }
        public string validacion { get; set; }
        public string uuid_original { get; set; }
        public bool bit_nc { get; set; }
        public bool bit_compu { get; set; }
        public int bit_carga { get; set; }
        public int compuesta { get; set; }
        public string email_carga { get; set; }
        public int id { get; set; }
        public string comentario_rechazo { get; set; }
        public string uuid_rechazo { get; set; }
        public decimal total_xml { get; set; }
        public DateTime fecha_autoriza_factura { get; set; }
        public int usuario_autoriza { get; set; }
        public int bit_antnc { get; set; }
        public int nivel_aut { get; set; }
        public bool cerrado { get; set; }
        public string ruta_rec_xml_depura { get; set; }
        public string ruta_rec_pdf_depura { get; set; }
        public string parametros_portal { get; set; }
        public string metodopago { get; set; }
        public string formapago { get; set; }
        public string usoCFDI { get; set; }
        public int usuario_carga { get; set; }
        public DateTime fecha_reporte_validacion { get; set; }
        public bool bitCancelar { get; set; }
        public bool bitAcuse { get; set; }
    }
}

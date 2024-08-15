using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class DocumentoNegociableDTO
    {
        //Num Proveedor
        public string proveedor { get; set; }
        public string noProveedor { get; set; }
        //No Documento
        public string noDocumento { get; set; }
        public DateTime fechaEmision { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public int moneda { get; set; }
        public decimal monto { get; set; }
        public string tipoDocumento { get; set; }
        public string referencia { get; set; }
        public string campoAdicional1 { get; set; }
        public string campoAdicional2 { get; set; }
        public string campoAdicional3 { get; set; }
        public string campoAdicional4 { get; set; }
        public string campoAdicional5 { get; set; }
        public string claveIF { get; set; }

    }
}

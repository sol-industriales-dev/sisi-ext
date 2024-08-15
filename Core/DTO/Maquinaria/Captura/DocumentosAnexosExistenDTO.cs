using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class DocumentosAnexosExistenDTO
    {
        public bool factura { get; set; }
        public int facturaID { get; set; }
        public bool pedimento { get; set; }
        public int pedimentoID { get; set; }
        public bool poliza { get; set; }
        public int polizaID { get; set; }
        public bool tarjetaCirculacion { get; set; }
        public int tarjetaCirculacionID { get; set; }
        public bool permisoCarga { get; set; }
        public int permisoCargaID { get; set; }
        public bool certificacion { get; set; }
        public int certificacionID { get; set; }
    }
}

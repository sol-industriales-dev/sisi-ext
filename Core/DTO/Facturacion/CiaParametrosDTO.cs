using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion
{
    public class CiaParametrosDTO
    {
        public int cia_sucursal { get; set; }
        public string ruta_pdf { get; set; }
        public string ruta_xml { get; set; }
        public string ruta_mensual { get; set; }
        public string aviso_termino_folios { get; set; }
        public int copia_cfdi { get; set; }
        public string manejo_dlls { get; set; }
        //public Nullable<string> pass_pfx { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Facturacion.Prefacturacion
{
    public class tblF_FilePrefactura
    {
        public int id { get; set; }
        public int idRepFactura { get; set; }
        public string rutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public DateTime FechaSubida { get; set; }
        public int tipoArchivo { get; set; }
        public int usuario { get; set; }
    }
}

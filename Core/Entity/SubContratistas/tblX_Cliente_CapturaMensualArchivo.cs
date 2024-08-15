using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_Cliente_CapturaMensualArchivo
    {
        public int id { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool actualizacion { get; set; }
        public int capturaMensualID { get; set; }
        public bool estatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_Cliente_CapturaMensual
    {
        public int id { get; set; }
        public int archivoMensualID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool actualizacion { get; set; }
        public int contratoPeriodoID { get; set; }
        public bool estatus { get; set; }
    }
}

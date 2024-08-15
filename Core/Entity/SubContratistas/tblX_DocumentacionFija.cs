using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_DocumentacionFija
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public bool aplicaFechaVencimiento { get; set; }
        public bool aplicaFechaSolicitud { get; set; }
        public int mesesVigenciaMinima { get; set; }
        public int mesesNotificacion { get; set; }
        public bool fisica { get; set; }
        public bool moral { get; set; }
        public bool opcional { get; set; }
        public bool estatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_ArchivoMensual
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public DateTime fechaInicioAplica { get; set; }
        public bool obligatorio { get; set; }
        public bool estatus { get; set; }
    }
}

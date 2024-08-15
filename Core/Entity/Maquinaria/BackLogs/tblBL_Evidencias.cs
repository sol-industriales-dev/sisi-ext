using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_Evidencias
    {
        public int id { get; set; }
        public int idBL { get; set; }
        public string nombreArchivo { get; set; }
        public int tipoEvidencia { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

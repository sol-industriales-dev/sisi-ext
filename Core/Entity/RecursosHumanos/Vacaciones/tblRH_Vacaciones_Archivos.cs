using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Archivos
    {
        public int id { get; set; }
        public int idIncapacidad { get; set; }
        public int tipoArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public string descripcion { get; set; }
        public string ubicacionArchivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

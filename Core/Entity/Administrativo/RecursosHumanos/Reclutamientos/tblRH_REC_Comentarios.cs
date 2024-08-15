using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Comentarios
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string comentario { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

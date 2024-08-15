using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_EmplFamiliares
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public DateTime? fecha_de_nacimiento { get; set; }
        public string parentesco { get; set; }
        public string grado_de_estudios { get; set; }
        public string estado_civil { get; set; }
        public string estudia { get; set; }
        public string genero { get; set; }
        public string vive { get; set; }
        public string beneficiario { get; set; }
        public string trabaja { get; set; }
        public string comentarios { get; set; }
        public bool esActivo { get; set; }
        public int idEKFam { get; set; }
        public int? num_dni { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}

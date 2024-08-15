using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Familia
    {
        public int id { get; set; }
        public int id_familia { get; set; }
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public DateTime? fecha_de_nacimiento { get; set; }
        public int? parentesco { get; set; }
        public string grado_de_estudios { get; set; }
        public string estado_civil { get; set; }
        public string estudia { get; set; }
        public string genero { get; set; }
        public string vive { get; set; }
        public DateTime? fecha_matrimonio { get; set; }
        public string beneficiario { get; set; }
        public string trabaja { get; set; }
        public string comentarios { get; set; }
        public string num_dni { get; set; }//PERU
        public string cedula_cuidadania { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

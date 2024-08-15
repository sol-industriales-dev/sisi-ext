using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class FamiliaresDTO
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public DateTime? fecha_de_nacimiento { get; set; }
        public int? parentesco { get; set; }
        public string parentescoStr { get; set; }
        public string strParentesco { get; set; }
        public string grado_de_estudios { get; set; }
        public string estado_civil { get; set; }
        public string estudia { get; set; }
        public string genero { get; set; }
        public string vive { get; set; }
        public string beneficiario { get; set; }
        public string trabaja { get; set; }
        public string comentarios { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActualizar { get; set; }
        public int idEKFam { get; set; }
        public string num_dni { get; set; }
        public string cedula_cuidadania { get; set; }

        /* CHECKBOX */
        public bool esVive { get; set; }
        public bool esBeneficiario { get; set; }
        public bool esTrabaja { get; set; }
        public bool esEstudia { get; set; }
        /* */
    }
}

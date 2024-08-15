using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class BeneficiariosDTO
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string parentesco_ben { get; set; }
        public string codigo_postal_ben { get; set; }
        public DateTime fecha_nac_ben { get; set; }
        public string paterno_ben { get; set; }
        public string materno_ben { get; set; }
        public string nombre_ben { get; set; }
        public int? pais_ben { get; set; }
        public int estado_ben { get; set; }
        public int ciudad_ben { get; set; }
        public string colonia_ben { get; set; }
        public string domicilio_ben { get; set; }
        public string num_ext_ben { get; set; }
        public string num_int_ben { get; set; }
        public string ben_num_dni { get; set; }
        public string cel { get; set; }
        public int? PERU_departamento_ben { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}
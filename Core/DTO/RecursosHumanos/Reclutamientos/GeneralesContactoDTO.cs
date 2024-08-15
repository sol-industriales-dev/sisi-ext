using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class GeneralesContactoDTO
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string estado_civil { get; set; }
        public DateTime fecha_planta { get; set; }
        public string ocupacion { get; set; }
        public string ocupacion_abrev { get; set; }
        public string num_cred_elector { get; set; }//MX
        public string num_dni { get; set; }//PERU
        public string cedula_cuidadania { get; set; }//COL
        public string domicilio { get; set; }
        public string numero_exterior { get; set; }
        public string numero_interior { get; set; }
        public string colonia { get; set; }
        public int? pais_dom { get; set; }
        public int estado_dom { get; set; }
        public int ciudad_dom { get; set; }
        public int codigo_postal { get; set; }
        public string tel_casa { get; set; }
        public string tel_cel { get; set; }
        public string email { get; set; }
        public int? tipo_casa { get; set; }
        public string tipo_sangre { get; set; }
        public string alergias { get; set; }
        public int? PERU_departamento_dom { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}

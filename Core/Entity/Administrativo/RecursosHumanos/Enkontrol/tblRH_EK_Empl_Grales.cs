using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Grales
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string estado_civil { get; set; }
        public DateTime? fecha_planta { get; set; }
        public string domicilio { get; set; }
        public string colonia { get; set; }
        public string codigo_postal { get; set; }
        public string tel_casa { get; set; }
        public string tel_cel { get; set; }
        public string num_cred_elector { get; set; }
        public string num_dni { get; set; }//PER
        public string cedula_cuidadania { get; set; }//COL
        public string email { get; set; }
        public string ocupacion { get; set; }
        public string nombre_ben { get; set; }
        public string parterno_ben { get; set; }
        public string materno_ben { get; set; }
        public string parentesco_ben { get; set; }
        public string domicilio_ben { get; set; }
        public DateTime? fecha_nac_ben { get; set; }
        public string colonia_ben { get; set; }
        public string codigo_postal_ben { get; set; }
        public int? pais_ben { get; set; }
        public int? cuidad_ben { get; set; }
        public int? estado_ben { get; set; }
        public int? pais_dom { get; set; }
        public int estado_dom { get; set; }
        public int cuidado_dom { get; set; }
        public string tipo_sangre { get; set; }
        public string alergias { get; set; }
        public string en_accidente_nombre { get; set; }
        public string en_accidente_telefono { get; set; }
        public string en_accidente_direccion { get; set; }
        public int? tipo_casa { get; set; }
        public string ocupacion_abrev { get; set; }
        public string numero_exterior { get; set; }
        public string numero_interior { get; set; }
        public string num_ext_ben { get; set; }
        public string num_int_ben { get; set; }
        public string ben_num_dni { get; set; }
        public int? PERU_departamento_ben { get; set; }
        public int? PERU_departamento_dom { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

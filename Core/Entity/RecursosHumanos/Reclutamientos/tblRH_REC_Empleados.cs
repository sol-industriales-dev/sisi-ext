using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Empleados
    {
        public int id { get; set; }
        public int idEstatus { get; set; }
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public DateTime? fecha_nac { get; set; }
        public int clave_pais_nac { get; set; }
        public int clave_estado_nac { get; set; }
        public int clave_ciudad_nac { get; set; }
        public string localidad_nacimiento { get; set; }
        public DateTime fecha_alta { get; set; }
        public string sexo { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public int banco { get; set; }
        public int num_cta_pago { get; set; }
        public int num_cta_pago_aho { get; set; }
        public int idCandidato { get; set; }
        public bool esPendiente { get; set; }
        public bool? esReingresoEmpleado { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}

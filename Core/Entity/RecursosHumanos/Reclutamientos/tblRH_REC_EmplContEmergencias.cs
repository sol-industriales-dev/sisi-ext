using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_EmplContEmergencias
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string en_accidente_nombre { get; set; }
        public string en_accidente_telefono { get; set; }
        public string en_accidente_direccion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }  
    }
}
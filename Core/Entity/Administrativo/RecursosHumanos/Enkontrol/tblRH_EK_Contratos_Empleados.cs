using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Contratos_Empleados
    {
        public int id { get; set; }
        public int id_contrato_empleado { get; set; }
        public int clave_empleado { get; set; }
        public int clave_duracion { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fecha_aplicacion { get; set; }
        public DateTime? fecha_fin { get; set; }
        public int clave_duracion_ant { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool? esActivo { get; set; }
    }
}

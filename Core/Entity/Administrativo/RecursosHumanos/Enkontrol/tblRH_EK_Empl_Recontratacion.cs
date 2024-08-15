using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Recontratacion
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string cc { get; set; }
        public int? tabulador { get; set; }
        public int? puesto { get; set; }
        public int? duracion_contrato { get; set; }
        public DateTime fecha_reingreso { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool? esActivo { get; set; }
    }
}

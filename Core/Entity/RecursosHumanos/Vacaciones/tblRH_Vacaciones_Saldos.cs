using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Saldos
    {
        public int id { get; set; }
        public int? idVacacionesPagadas { get; set; }
        public int clave_empleado { get; set; }
        public int num_dias { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}

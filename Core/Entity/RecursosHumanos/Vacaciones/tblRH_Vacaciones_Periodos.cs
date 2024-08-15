using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Periodos
    {

        public int id { get; set; }
        public string periodoDesc { get; set; }
        public bool estado { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFinal { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

    }
}

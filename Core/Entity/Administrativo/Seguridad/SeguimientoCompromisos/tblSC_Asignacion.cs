using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoCompromisos
{
    public class tblSC_Asignacion
    {
        public int id { get; set; }
        public int agrupacion_id { get; set; }
        public int area { get; set; }
        public int actividad_id { get; set; }
        public DateTime fechaInicio { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}

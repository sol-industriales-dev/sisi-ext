using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Fechas
    {

        public int id { get; set; }
        public int vacacionID { get; set; }
        public DateTime? fecha { get; set; }
        public int tipoInsidencia { get; set; }
        public bool esAplicadaIncidencias { get; set; }
        public int? idIncidencia { get; set; }
        public DateTime? fechaAplicadas { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public bool incidenciaAplicada { get; set; }
    }
}

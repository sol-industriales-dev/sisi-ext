using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class VacFechasDTO
    {
        public int id { get; set; }
        public int vacacionID { get; set; }
        public DateTime? fecha { get; set; }
        public int tipoInsidencia { get; set; }
        public bool esAplicadaIncidencias { get; set; }
        public DateTime? fechaAplicadas { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public bool incidenciaAplicada { get; set; }
        public int tipoVacaciones { get; set; }
        public string descTipoVacaciones { get; set; }
    }
}

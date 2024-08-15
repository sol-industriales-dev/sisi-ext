using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class ActividadesTipoDTO
    {
        public int creadorMinutaID { get; set; }
        public string creadorMinuta { get; set; }
        public int minutaID { get; set; }
        public string minuta { get; set; }
        public int departamentoID { get; set; }
        public string departamento { get; set; }
        public int actividadID { get; set; }
        public string actividad { get; set; }
        public int responsableID { get; set; }
        public string responsable { get; set; }
        public DateTime vFechaInicio { get; set; }
        public string fechaInicio { get; set; }
        public DateTime vFechaFin { get; set; }
        public string fechaFin { get; set; }
        public string prioridad { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class ReporteDiarioDTO
    {
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }
        public string actividad { get; set; }
        public int equipoID { get; set; }
        public List<int> equiposID { get; set; }
        public string equipo { get; set; }
        public string ultMantenimiento { get; set; }
        public string sigMantenimiento { get; set; }
        public int areaID { get; set; }
        public string areaEjecucion { get; set; }
        public int descripcionActividadID { get; set; }
        public string descripcionActividad { get; set; }
        public int semaforo { get; set; }
        public string reprogramacion { get; set; }
        public string estatus { get; set; }
        public List<int> evidenciasID { get; set; }
        public string evidencias { get; set; }
        public List<int> referenciasID { get; set; }
        public string referencias { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int monthFechaCaptura { get; set; }
        public int dayFechaCaptura { get; set; }
        public string fechaReprogramacion { get; set; }
        public int monthFechaReprogramacion { get; set; }
        public int dayFechaReprogramacion { get; set; }
        public int revisionTipo { get; set; }
        public int revisionID { get; set; }
        public int revisionDetID { get; set; }
    }
}

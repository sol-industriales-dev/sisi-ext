using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class tblM_RenderFullCalendar
    {
        //fulcalendar render
        public int id { get; set; }
        public int personalRealizo { get; set; }
        public string fechaMantenimientoActual { get; set; }
        public int tipoMantenimientoActual { get; set; }
        public string observaciones { get; set; }
        public string economicoID { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public decimal UltimoHorometro { get; set; }
        public decimal horometroProyectado { get; set; }
        public string fechaProyectada { get; set; }
        public int idMaquina { get; set; }
        public int idMantenimiento { get; set; }
        public decimal HorometroPm { get; set; }
        public string borderColor { get; set; }

        public int estadoMantenimiento { get; set; }
    }
}

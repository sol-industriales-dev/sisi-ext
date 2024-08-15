using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class SeguimientoPlanAccionDTO
    {
        public int id { get; set; }
       
        public string deteccion { get; set; }
        public string descripcion { get; set; }
        public string medida { get; set; }
        public DateTime fechaCompromiso { get; set; }
        public string lider { get; set; }
        public int tiempoTranscurrido { get; set; }


        public string cc { get; set; }
        public string area { get; set; }
    }
}

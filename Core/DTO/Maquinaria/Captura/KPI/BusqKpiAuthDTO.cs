using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.KPI
{
    public class BusqKpiAuthDTO
    {
        public string ac { get; set; }
        public int año { get; set; }
        public int semana { get; set; }
        public DateTime min { get; set; }
        public DateTime max { get; set; }
        public authEstadoEnum estatus { get; set; }
    }
}

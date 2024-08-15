using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class HorasTrackingDTO
    {
        public DateTime Fecha { get; set; }
        public string Economico { get; set; }
        public decimal HorometroAcumulado { get; set; }
        public decimal HorasTrabajo { get; set; }
        public int turno { get; set; }
    }
}

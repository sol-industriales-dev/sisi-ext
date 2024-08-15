using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepRelacionBajaDTO
    {
        public decimal NoAsignado { get; set; }
        public decimal VentaInterna { get; set; }
        public decimal VentaExterna { get; set; }
        public decimal TerminoVida { get; set; }
        public decimal Siniestro { get; set; }
        public decimal Robo { get; set; }
    }
}

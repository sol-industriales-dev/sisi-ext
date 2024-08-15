using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepBajaMaquinaria
    {
        public string Economico { get; set; }
        public int GrupoID { get; set; }
        public string Descripcion { get; set; }
        public decimal Horometro { get; set; }
        public decimal Promedio { get; set; }
        public bool NoAsignado { get; set; }
        public bool VentaInterna { get; set; }
        public bool VentaExterna { get; set; }
        public bool TerminoVida { get; set; }
        public bool Siniestro { get; set; }
        public bool Robo { get; set; }
    }
}

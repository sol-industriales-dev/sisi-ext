using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class ReporteConcentradoDTO
    {
        public int? capitulo_id { get; set; }
        public int? subcapituloN3_id { get; set; }
        public int? subcapituloN2_id { get; set; }
        public int? subcapituloN1_id { get; set; }
        public string capitulo { get; set; }
        public string subcapituloN1 { get; set; }
        public string subcapituloN2 { get; set; }
        public string subcapituloN3 { get; set; }

        public decimal? presupuesto { get; set; }
        public decimal? ejecutado { get; set; }
        public decimal? xEjecutar { get; set; }
        public decimal? porcentajeAvance { get; set; }
        public decimal? avanceSemana { get; set; }
        public decimal? importeSemana { get; set; }
        public decimal? facturado { get; set; }
        public decimal? xFacturar { get; set; }
    }
}

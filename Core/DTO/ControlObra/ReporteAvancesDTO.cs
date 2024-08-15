using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class ReporteAvancesDTO
    {
        public int id { get; set; }
        public int actividad_id { get; set; }
        public string actividad { get; set; }
        public decimal? actividadPU { get; set; }
        public decimal? actividadCosto { get; set; }
        public decimal? importeContratado { get; set; }
        public string unidad { get; set; }
        public decimal cantidad { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public decimal? acumAnterior { get; set; }
        public decimal? acumActual { get; set; }
        public decimal? avancePeriodo { get; set; }
        public decimal? avancePorcentaje { get; set; }
        public decimal? avanceAcumuladoPorcentaje { get; set; }
        public int? subcapituloN3_id { get; set; }
        public int? subcapituloN2_id { get; set; }
        public int? subcapituloN1_id { get; set; }
        public decimal? importeAvanceAnt { get; set; }
        public decimal? importeAvancePeriodo { get; set; }
        public decimal? importeAvanceAcumulado { get; set; }
        public decimal? importeAvancePorcentaje { get; set; }
        public decimal? volumenFacturado { get; set; }
        public decimal? volumenxFacturar { get; set; }
        public decimal? importeFacturado { get; set; }
        public decimal? importexFacturar { get; set; }
        public int? tipoPeriodoAvance { get; set; }
    }
}

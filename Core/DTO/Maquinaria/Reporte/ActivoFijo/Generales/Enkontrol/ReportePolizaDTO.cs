using Core.Enum.Maquinaria.Reportes.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Generales.Enkontrol
{
    public class ReportePolizaDTO
    {
        public string Poliza { get; set; }
        public bool EsCC { get; set; }
        public string CCInicial { get; set; }
        public string CCFinal { get; set; }
        public int PolizaInicial { get; set; }
        public int PolizaFinal { get; set; }
        public string TipoPolizaInicial { get; set; }
        public string TipoPolizaFinal { get; set; }
        public DateTime PeriodoInicial { get; set; }
        public DateTime PeriodoFinal { get; set; }
        public bool ReporteResumido { get; set; }
        public bool PolizaPorHoja { get; set; }
        public bool IncluirFirmas { get; set; }
        public string Estatus { get; set; }
        public string Reviso { get; set; }
        public string Autorizo { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoPolizasCCDTO
    {
        public int Year { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public int Linea { get; set; }
        public int TM { get; set; }
        public int Cta { get; set; }
        public int Scta { get; set; }
        public int Sscta { get; set; }
        public int? Factura { get; set; }
        public decimal Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime FechaPol { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public int MesesDepreciacion { get; set; }
        public string Concepto { get; set; }
    }
}
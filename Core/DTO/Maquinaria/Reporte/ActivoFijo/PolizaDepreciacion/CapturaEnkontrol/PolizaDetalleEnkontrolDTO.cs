using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion.CapturaEnkontrol
{
    public class PolizaDetalleEnkontrolDTO
    {
        public int Year { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public int Linea { get; set; }
        public int Cta { get; set; }
        public int Scta { get; set; }
        public int Sscta { get; set; }
        public int Digito { get; set; }
        public int TM { get; set; }
        public string Referencia { get; set; }
        public string CC { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public int IClave { get; set; }
        public int ITM { get; set; }
        public int? Area { get; set; }
        public int? Cuenta_OC { get; set; }
        public string AreaCuentaDescripcion { get; set; }

        public bool EsBaja { get; set; }
        public int IdCosto { get; set; }
    }
}
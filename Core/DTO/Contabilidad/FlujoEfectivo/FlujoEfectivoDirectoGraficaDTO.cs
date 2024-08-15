using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class FlujoEfectivoDirectoGraficaDTO
    {
        public int anio { get; set; }
        public int noSemana { get; set; }
        public int idConcepto { get; set; }
        public string concepto { get; set; }
        public object monto { get; set; }
        public string stack { get; set; }
        public string name { get; set; }
        public decimal data { get; set; }
    }
}

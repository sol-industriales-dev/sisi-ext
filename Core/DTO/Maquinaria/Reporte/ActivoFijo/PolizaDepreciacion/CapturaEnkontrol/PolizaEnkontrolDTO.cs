using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion.CapturaEnkontrol
{
    public class PolizaEnkontrolDTO
    {
        public int Year { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public DateTime FechaPol { get; set; }
        public decimal Cargos { get; set; }
        public decimal Abonos { get; set; }
        public string Concepto { get; set; }
        public char Generada { get; set; }
        public char Status { get; set; }
        public DateTime Fecha_hora_crea { get; set; }
        public int Usuario_Crea { get; set; }
    }
}
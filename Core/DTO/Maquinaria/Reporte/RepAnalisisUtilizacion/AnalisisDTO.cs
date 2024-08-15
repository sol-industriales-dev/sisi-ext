using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion
{
    public class AnalisisDTO
    {
        public string noEco { get; set; }
        public string grupo { get; set; }
        public string modelo { get; set; }
        public decimal hi { get; set; }
        public decimal hf { get; set; }
        public decimal ht { get; set; }
        public decimal promSem { get; set; }
        public decimal totalUSD { get; set; }
        public decimal totalMX { get; set; }
        public string cc { get; set; }
    }
}

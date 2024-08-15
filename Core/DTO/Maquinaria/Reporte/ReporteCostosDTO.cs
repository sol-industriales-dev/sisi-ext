using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class ReporteCostosDTO
    {
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public decimal enero { get; set; }
        public decimal febrero { get; set; }
        public decimal marzo { get; set; }
        public decimal abril { get; set; }
        public decimal mayo { get; set; }
        public decimal junio { get; set; }
        public decimal julio { get; set; }
        public decimal agosto { get; set; }
        public decimal septiembre { get; set; }
        public decimal octubre { get; set; }
        public decimal noviembre { get; set; }
        public decimal diciembre { get; set; }
        public decimal total { get; set; }
    }
}

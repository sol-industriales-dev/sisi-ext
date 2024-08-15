using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion
{
    public class BusqAnalisiDTO
    {
        public int cc { get; set; }
        public int tipo { get; set; }
        public List<int> grupo { get; set; }
        public List<int> modelo { get; set; }
        public List<string> noEco { get; set; }
        public DateTime ini { get; set; }
        public DateTime fin { get; set; }
        public decimal ritmoMin { get; set; }
        public decimal ritmoMax { get; set; }
        public int moneda { get; set; }
    }
}

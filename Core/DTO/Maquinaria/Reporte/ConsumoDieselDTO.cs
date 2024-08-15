using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class ConsumoDieselDTO
    {
        public string economico { get; set; }
        public string descripcion { get; set; }
        public decimal consumo { get; set; }
        public decimal importe { get; set; }
        public decimal importeKontrol { get; set; }

    }
}

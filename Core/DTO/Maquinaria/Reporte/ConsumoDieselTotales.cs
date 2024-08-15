using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class ConsumoDieselTotales
    {
        public List<ConsumoDieselDTO> lstConsumos { get; set; }

        public decimal totalConsumido { get; set; }
        public decimal totalEnKontrol { get; set; }
        public decimal totalContratistas { get; set; }
        public decimal totalProvisionar { get; set; }


        public decimal totalInventario { get; set; }
    }
}

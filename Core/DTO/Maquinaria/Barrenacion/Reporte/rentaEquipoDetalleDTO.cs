using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion.Reporte
{
    public class rentaEquipoDetalleDTO
    {

        public string modelo { get; set; }
        public decimal total { get; set; }
        public string moneda { get; set; }
        public decimal totalUSD { get; set; }
        public decimal horasPeriodo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDifDepMensualCCDTO
    {
        public string CC { get; set; }
        public decimal DepMensual { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoModPolDTO
    {
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPol { get; set; }
        public string Concepto { get; set; }
        public string AreaCuenta { get; set; }
    }
}
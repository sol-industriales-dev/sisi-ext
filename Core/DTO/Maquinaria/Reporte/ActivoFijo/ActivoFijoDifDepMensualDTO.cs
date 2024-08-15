using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDifDepMensualDTO
    {
        public decimal DepMensual { get; set; }
        public int Meses { get; set; }
        public int MesesOriginales { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? FechaBajaOriginal { get; set; }
        public string CC { get; set; }
    }
}
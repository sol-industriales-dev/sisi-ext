using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class GraficaKPIHomDTO
    {
        public decimal valorCodigo { get; set; }
        public string codigosJS { get; set; }
        public int frecuenciaCodigoParo { get; set; }
    }
}

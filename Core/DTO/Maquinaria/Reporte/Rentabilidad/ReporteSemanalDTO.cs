using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class ReporteSemanalDTO
    {
        public string areaCuenta { get; set; }
        public int corteID { get; set; }
        public string cuenta { get; set; }
        public decimal monto { get; set; }
    }
}

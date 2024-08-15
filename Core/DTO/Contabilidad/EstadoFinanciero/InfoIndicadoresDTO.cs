using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class InfoIndicadoresDTO
    {
        public EmpresaEnum? empresa { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public decimal activoCirculante { get; set; }
        public decimal pasivoCirculante { get; set; }
        public decimal pasivoTotal { get; set; }
        public decimal capitalContableCirculante { get; set; }
        public decimal inventario { get; set; }
        public decimal activoTotal { get; set; }
        public decimal ebitda { get; set; }
        public decimal dxpCortoPlazo { get; set; }
        public decimal dxpLargoPlazo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class CedulaMensualDTO
    {
        public string financiera { get; set; }
        public decimal nacionalCortoPlazo { get; set; }
        public decimal dolaresCortoPlazo { get; set; }
        public decimal contabilidadCortoPlazo { get; set; }
        public decimal sigoplanCortoPlazo { get; set; }
        public decimal diferenciaCortoPlazo { get; set; }
        public decimal nacionalLargoPlazo { get; set; }
        public decimal dolaresLargoPlazo { get; set; }
        public decimal contabilidadLargoPlazo { get; set; }
        public decimal sigoplanLargoPlazo { get; set; }
        public decimal diferenciaLargoPlazo { get; set; }
    }
}

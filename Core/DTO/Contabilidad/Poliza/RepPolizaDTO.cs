using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class RepPolizaDTO
    {
        public int poliza { get; set; }
        public string tp { get; set; }
        public DateTime fechapol { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public decimal diferencia { get; set; }
        public string generada { get; set; }
        public string status { get; set; }
        public decimal ccCargos { get; set; }
        public decimal ccAbonos { get; set; }
        public decimal ccDiferencia { get; set; }
    }
}

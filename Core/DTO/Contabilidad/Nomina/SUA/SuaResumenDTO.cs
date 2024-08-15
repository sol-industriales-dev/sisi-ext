using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.SUA
{
    public class SuaResumenDTO
    {
        public string cc { get; set; }
        public string estado { get; set; }
        public string registroPatronal { get; set; }
        public string ccDescripcion { get; set; }
        public decimal imssPatronal { get; set; }
        public decimal imssObrero { get; set; }
        public decimal rcvPatronal { get; set; }
        public decimal rcvObrero { get; set; }
        public decimal infonavit { get; set; }
        public decimal amortizacion { get; set; }
        public decimal total { get; set; }
    }
}

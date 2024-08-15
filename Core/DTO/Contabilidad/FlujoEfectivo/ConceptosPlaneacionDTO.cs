using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class ConceptosPlaneacionDTO
    {
        public int conceptoID { get; set; }
        public string operador { get; set; }
        public string concepto { get; set; }
        public string idPadre { get; set; }
        public decimal total { get; set; }
        public bool edit { get; set; }
    }
}

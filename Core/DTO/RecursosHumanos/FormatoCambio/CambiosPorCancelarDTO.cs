using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.FormatoCambio
{
    public class CambiosPorCancelarDTO
    {
        public int id { get; set; }
        public string firma { get; set; }
        public DateTime? fechaFirma { get; set; }
        public int idFirmaPendiente { get; set; }
        public DateTime? fechaCaptura { get; set; }
        public string CcID { get; set; }
        public string CC { get; set; }
    }
}

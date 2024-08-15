using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class GeneralConcentradoHHDTO
    {
        public string puesto { get; set; }
        public int puestoID { get; set; }
        public decimal horashombre { get; set; }
        public decimal costohorashombre { get; set; }
        public decimal porRegistro { get; set; }
        public decimal porHorasEfectivas { get; set; }
        public decimal CostoTotalHorasHombre { get; set; }
        public decimal TotalHorasHombre { get; set; }
        public string btn { get; set; }
    }
}

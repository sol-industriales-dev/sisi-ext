using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class tblHorasHombreConcentradoDTO
    {
        public int count { get; set; }
        public int puestoID { get; set; }
        public string descripcionPuesto { get; set; }
        public decimal totalHorasHombre { get; set; }
        public decimal costoHorasHombre { get; set; }
        public decimal porRegistro { get; set; }
        public decimal horasEfectivas { get; set; }
    }
}

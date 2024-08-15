using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class tblHorasHombreDTO
    {
        public int count { get; set; }
        public int puestoID { get; set; }
        public string descripcionPuesto { get; set; }
        public decimal totalHorasHombre { get; set; }
        public decimal costoHorasHombre { get; set; }

    }
}

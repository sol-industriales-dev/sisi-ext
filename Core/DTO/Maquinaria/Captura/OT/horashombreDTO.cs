using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class horashombreDTO
    {
        public int PuestoID { get; set; }
        public string Puesto { get; set; }
        public decimal TotalHorasHombre { get; set; }
        public decimal CostoHH { get; set; }
        public decimal CostoTotal { get; set; }
        public string btn { get; set; }
        public int No { get; set; }

    }
}

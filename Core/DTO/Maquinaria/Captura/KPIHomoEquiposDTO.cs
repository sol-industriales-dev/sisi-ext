using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class KPIHomoEquiposDTO
    {
        public string grupo { get; set; }
        public string modelo { get; set; }
        public string economico { get; set; }
        public int economicoID { get; set; }
        public int id { get; set; }
        public int grupoID { get; set; }
        public int modeloID { get; set; }
        public bool tieneRegistros { get; set; }
        public decimal horometro { get; set; }
    }
}

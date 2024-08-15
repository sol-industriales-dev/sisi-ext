using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria._Caratulas
{
    public class CaratulaEncDTO
    {
        public int id { get; set; }
        public string AgrupacionCaratula { get; set; }
        public bool esActivo { get; set; }
        public List<CaratulaDetDTO> lstDetalle { get; set; }
    }
}

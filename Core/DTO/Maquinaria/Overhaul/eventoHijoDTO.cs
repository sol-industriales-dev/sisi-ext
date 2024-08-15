using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class eventoHijoDTO
    {
        public bool removido { get; set; }
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool tipo { get; set; }
    }
}

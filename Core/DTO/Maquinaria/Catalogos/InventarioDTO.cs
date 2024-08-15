using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
   public class InventarioDTO
    {
        public int id { get; set; }
        public string Grupo { get; set; }
        public string Economico { get; set; }
        public string Tipo { get; set; }
        public string  Serie { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string CC { get; set; }
        public int idEconomico { get; set; }

        public string CCOrigen { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class objOTMaquinariaDTO
    {
        public string Economico { get; set; }
        public string Modelo { get; set; }
        public int ModeloID { get; set; }
        public string CCName { get; set; }
        public string CC { get; set; }
        public int tipoEquipo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class InsumoBarrenadoraDTO
    {
        public int insumo { get; set; }
        public int barrenadoraID { get; set; }
        public string noSerie { get; set; }
        public string noSerieSugerido { get; set; }
        public decimal precio { get; set; }
        public int id { get; set; }
    }
}

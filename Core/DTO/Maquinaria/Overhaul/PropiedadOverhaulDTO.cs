using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class PropiedadOverhaulDTO
    {
        public int Valor { get; set; }
        public string Propiedad { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public bool Autorizado { get; set; }
        public int maquinaID { get; set; }
    }
}

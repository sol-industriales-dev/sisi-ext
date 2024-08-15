using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class movDTO
    {//select numero, periodo, ano from si_movimientos;
        public int numero { get; set; }
        public int periodo { get; set; }
        public int ano { get; set; }
    }
}

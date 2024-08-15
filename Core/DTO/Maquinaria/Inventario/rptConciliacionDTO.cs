using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class rptConciliacionDTO
    {
        public string Economico { get; set; }
        public string Descripcion { get; set; }
        public string HorometroInicio { get; set; }
        public string HorometroFinal { get; set; }
        public string DiaParo { get; set; }
        public string TipoConsideracion { get; set; }
    }
}

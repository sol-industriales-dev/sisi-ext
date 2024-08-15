using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class RastreoAsignacionesDTO
    {
        public string LugarRecepcion { get; set; }
        public string LugarOrigen { get; set; }
        public string Economico { get; set; }
        public string TipoEconomico { get; set; }
        public string FechaPromesa { get; set; }
        public string Estado { get; set; }
    }
}

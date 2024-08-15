using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.RelacionPoliza
{
    public class ActivoDTO
    {
        public int? idActivo { get; set; }
        public bool esMaquina { get; set; }
        public string numeroEconomico { get; set; }
        public string descripcion { get; set; }
    }
}

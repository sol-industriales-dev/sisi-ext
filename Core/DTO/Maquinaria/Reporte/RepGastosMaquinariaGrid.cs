using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepGastosMaquinariaGrid
    {
        public string economico { get; set; }
        public string importe { get; set; }
        public string fecha { get; set; }
        public string descripcion { get; set; }
        public int tipoInsumo { get; set; }

    }
}

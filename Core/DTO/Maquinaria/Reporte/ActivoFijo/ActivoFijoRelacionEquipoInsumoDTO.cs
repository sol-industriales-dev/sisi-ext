using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoRelacionEquipoInsumoDTO
    {
        public List<string> insumo { get; set; }
        public int maximo { get; set; }
        public int subconjuntoID { get; set; }
        public string subconjunto { get; set; }
    }
}

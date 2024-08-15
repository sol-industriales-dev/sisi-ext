using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDepMaqDTO
    {
        public int Id { get; set; }
        public int IdCatMaquina { get; set; }
        public decimal? PorcentajeDepreciacion { get; set; }
        public int? MesesTotalesADepreciar { get; set; }
    }
}

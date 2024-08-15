using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class AsignacionParcialDTO
    {
        public int id { get; set; }
        public int idNoEconomico { get; set; }
        public DateTime pFechaObra { get; set; }
        public DateTime pFechaFin { get; set; }
        public DateTime pFechaInicio { get; set; }
        public int tipoAsignacion { get; set; }

    }
}

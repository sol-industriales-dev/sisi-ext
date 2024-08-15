using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class EquipoAreaDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public List<PlanMes_Detalle_DiaDTO> dias { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class ActividadesDNHisDTO
    {
        public string  actividad { get; set; }
        public decimal Hrsaplico { get; set; }
        public bool aplico { get; set; }
        public int perioricidad { get; set; }
    }
}

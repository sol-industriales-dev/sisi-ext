using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class actividadesExtraHisDTO
    {
        public string actividad { get; set; }
        public decimal Hrsaplico { get; set; }
        public int perioricidad { get; set; }
        public bool aplico { get; set; }
    }
}

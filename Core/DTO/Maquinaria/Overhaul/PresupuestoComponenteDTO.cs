using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class PresupuestoComponenteDTO
    {
        public int id { get; set; }
        public string noComponente { get; set; }
        public int modeloID { get; set; }
        public string modelo { get; set; }
        public int subconjuntoID { get; set; }
        public string subconjunto { get; set; }
        public decimal presupuestoInicial { get; set; }
        public decimal erogado { get; set; }
        public bool presupuestado { get; set; }
    }
}

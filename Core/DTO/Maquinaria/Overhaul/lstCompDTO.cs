using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class lstCompDTO
    {
        public string descripcion { get; set; }
        public string nombreCorto { get; set; }
        public string noComponente { get; set; }
        public decimal restaEstatus { get; set; }
        public bool falla { get; set; }
        public decimal horaCicloActual { get; set; }
        public DateTime fecha { get; set; }
        public double diasEnLocacion { get; set; }
    }
}

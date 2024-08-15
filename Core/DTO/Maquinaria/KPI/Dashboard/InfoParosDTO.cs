using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class InfoParosDTO
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int idTipo { get; set; }
        public string descripcionTipo { get; set; }
        public decimal valor { get; set; }
    }
}

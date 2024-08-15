using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_KPI
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int noEconomicoID { get; set; }
        public string noEconomico { get; set; }
        public decimal proyeccionHoras { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
    }
}

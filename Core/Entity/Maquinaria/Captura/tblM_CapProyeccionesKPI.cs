using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapProyeccionesKPI
    {
        public int id { get; set; }
        public string  economicoID { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public decimal horasUtilizacion { get; set; }
        public string CC { get; set; }
    }
}

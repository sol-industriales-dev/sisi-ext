using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapDesfase
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public decimal horasDesfase { get; set; }
        public decimal horasDesfaseAcumulado { get; set; }
        public bool estado { get; set; }
        public DateTime fecha { get; set; }
        public string CC { get; set; }
    }
}

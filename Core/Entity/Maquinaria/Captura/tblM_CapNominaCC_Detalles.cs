using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapNominaCC_Detalles
    {
        public int id { get; set; }
        public int idNomina { get; set; }
        public int idEconomico { get; set; }
        public string economico { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public decimal hh { get; set; }
        public decimal cargoP { get; set; }
        public decimal cargoD { get; set; }
       
    }
}

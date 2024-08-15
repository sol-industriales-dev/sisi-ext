using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapPrecioDiesel
    {
        public int id { get; set; }
        public decimal precio { get; set; }
        public int idUsuario { get; set; }
        public DateTime fecha { get; set; }
    }
}

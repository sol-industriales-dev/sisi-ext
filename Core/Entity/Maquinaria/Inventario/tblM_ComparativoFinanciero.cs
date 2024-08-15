using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ComparativoFinanciero
    {
        public int id { get; set; }
        public int idAsignacion { get; set; }
        public string financiera { get; set; }
        public bool esActivo { get; set; }

    }
}

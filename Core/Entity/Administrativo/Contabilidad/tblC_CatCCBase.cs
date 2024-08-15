using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_CatCCBase
    {
        public int id { get; set; }
        public string centro_costos { get; set; }
        public string nombCC { get; set; }
        public decimal total { get; set; }
        public string descripcion { get; set; }
    }
}

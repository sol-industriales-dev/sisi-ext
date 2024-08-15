using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatParteVidaUtil
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal vidaUtilMin { get; set; }
        public decimal vidaUtilMax { get; set; }
    }
}

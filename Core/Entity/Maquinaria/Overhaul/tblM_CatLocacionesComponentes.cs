using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CatLocacionesComponentes
    {
        public int id { get; set; }
        public int tipoLocacion { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public string areaCuenta { get; set; }
        public string JsonCorreos { get; set; }
    }
}
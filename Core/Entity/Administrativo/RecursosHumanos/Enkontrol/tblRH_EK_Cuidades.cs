using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Cuidades
    {
        public int id { get; set; }
        public int clave_pais { get; set; }
        public int clave_estado { get; set; }
        public int clave_cuidad { get; set; }
        public string descripcion { get; set; }
        public int ind_precargado { get; set; }
    }
}

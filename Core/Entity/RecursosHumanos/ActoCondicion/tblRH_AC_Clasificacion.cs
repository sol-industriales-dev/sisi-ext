using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.RecursosHumanos.ActoCondicion;

namespace Core.Entity.RecursosHumanos.ActoCondicion
{
    public class tblRH_AC_Clasificacion
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public TipoRiesgoCH tipoRiesgo { get; set; }
    }
}

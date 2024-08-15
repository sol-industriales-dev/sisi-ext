using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_Corte
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int semana { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public DateTime fecha_registro { get; set; }
        public bool actual { get; set; }
    }
}

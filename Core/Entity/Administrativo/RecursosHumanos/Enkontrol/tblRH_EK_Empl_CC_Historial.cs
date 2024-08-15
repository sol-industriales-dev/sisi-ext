using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_CC_Historial
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string cc { get; set; }
        public string cc_anterior { get; set; }
        public DateTime fecha_cambio { get; set; }
        public int? id_cambio { get; set; }
    }
}

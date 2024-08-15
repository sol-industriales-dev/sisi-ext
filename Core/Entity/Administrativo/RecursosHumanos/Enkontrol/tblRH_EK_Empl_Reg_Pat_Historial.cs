using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Reg_Pat_Historial
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public int reg_pat { get; set; }
        public int reg_pat_anterior { get; set; }
        public DateTime fecha_cambio { get; set; }
        public TimeSpan? hora { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Vacaciones
{
    public class FiltroSaldosDTO
    {
        public string cc { get; set; }
        public int clave_empleado { get; set; }
        public string nombre_empleado { get; set; }
        public string estatusEmpleado { get; set; }
    }
}

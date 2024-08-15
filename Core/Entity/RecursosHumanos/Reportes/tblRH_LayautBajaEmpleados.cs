using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reportes
{
    public class tblRH_LayautBajaEmpleados
    {
        public int id { get; set; }
        public string empleadoID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCaptura { get; set; }
    }
}

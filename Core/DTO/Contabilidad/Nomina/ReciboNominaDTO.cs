using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class ReciboNominaDTO
    {
        public ReciboNominaDTO()
        {
            lstClaveEmpleados = new List<int>();
        }

        public int clave_empleado { get; set; }
        public int tipoNomina { get; set; }
        public string cc { get; set; }
        public int prenominaID { get; set; }
        public List<int> lstClaveEmpleados { get; set; }
        public int periodo { get; set; }
    }
}

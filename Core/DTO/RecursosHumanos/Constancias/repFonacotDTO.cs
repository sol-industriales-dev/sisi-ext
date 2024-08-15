using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repFonacotDTO
    {
        public string clave_empleado { get; set; }
        public string nombreCompleto { get; set; }
        public string ccDescripcionFonacot { get; set; }        
        public string regPatron { get; set; }
        public string nombrePatron { get; set; }
        public string imss { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string fechaIngreso { get; set; }
        public string nombrePuesto { get; set; }
        public string mensualNeto { get; set; }
        public string sueldoBase { get; set; }
        public string complemento { get; set; }
        public string tipoNomina { get; set; }  
        public string valorLetra { get; set; }

        public int? idEmpleado { get; set; }
        public int? idJefe { get; set; }
        public string cc { get; set; }
    }
}

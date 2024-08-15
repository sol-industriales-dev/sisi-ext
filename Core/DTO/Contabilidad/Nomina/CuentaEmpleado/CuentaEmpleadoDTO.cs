using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.CuentaEmpleado
{
    public class CuentaEmpleadoDTO
    {
        public int id { get; set; }
        public int numero { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombreCompleto { get; set; }
        public int tipoCuentaId { get; set; }
        public string tipoCuentaDescripcion { get; set; }
        public string cuenta { get; set; }
        public string cuentaDescripcion { get; set; }
        public bool validada { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
    }
}

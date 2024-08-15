using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores
{
    public class tblC_CostoEstimado
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }
        public decimal estimacion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}

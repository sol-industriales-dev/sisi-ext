using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_ConceptoClienteProveedor
    {
        public int id { get; set; }
        public int empresaPrincipalId { get; set; }
        public int empresaSecundariaId { get; set; }
        public int conceptoPrincipalId { get; set; }
        public int conceptoId { get; set; }
        public bool estatus { get; set; }
    }
}

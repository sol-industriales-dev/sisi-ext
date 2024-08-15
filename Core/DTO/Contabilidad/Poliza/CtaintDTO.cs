using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class CtaintDTO
    {
        public int cuenta { get; set; }
        public string sistema { get; set; }
        public string nivel { get; set; }
        public int numini { get; set; }
        public int numfin { get; set; }
        public int subcuenta { get; set; }
        public int subsubcuenta { get; set; }
        public string cve_monitoreo { get; set; }
        public string tipo_configura { get; set; }
        public string bit_desglosa_proveedor { get; set; }
    }
}

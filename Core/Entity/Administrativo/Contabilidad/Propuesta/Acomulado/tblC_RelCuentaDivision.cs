using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_RelCuentaDivision
    {
        public int id { get; set; }
        /// <summary>
        /// cuenta de sb_cuenta
        /// </summary>
        public int cuenta { get; set; }
        public int division { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}

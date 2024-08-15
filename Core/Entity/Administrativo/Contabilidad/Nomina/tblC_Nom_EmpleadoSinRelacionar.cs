using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_EmpleadoSinRelacionar
    {
        public int id { get; set; }
        public int numeroEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public int tipoCuentaId { get; set; }
        public string cc { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual tblC_Nom_TipoCuenta tipoCuenta { get; set; }
    }
}

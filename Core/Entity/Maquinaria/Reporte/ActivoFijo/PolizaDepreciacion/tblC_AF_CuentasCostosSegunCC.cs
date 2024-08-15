using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_CuentasCostosSegunCC
    {
        public int Id { get; set; }
        public string CcActivoFijo { get; set; }
        public int IdCuenta { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("IdCuenta")]
        public virtual tblC_AF_Cuenta Cuenta { get; set; }
    }
}

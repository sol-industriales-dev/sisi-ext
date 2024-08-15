using Core.DTO;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_TipoCuenta
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipoMovimientoId { get; set; }

        [ForeignKey("tipoMovimientoId")]
        public virtual tblC_TipoMovimiento tipoMovimiento { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual ICollection<tblC_Nom_Cuenta> cuenta { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_Cuenta
    {
        public int Id { get; set; }
        public int Cuenta { get; set; }
        public string Descripcion { get; set; }
        public int TipoCuentaId { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("TipoCuentaId")]
        public virtual tblC_AF_TipoCuenta TipoCuenta { get; set; }

        [ForeignKey("CuentaId")]
        public virtual List<tblC_AF_Subcuenta> Subcuentas { get; set; }
    }
}
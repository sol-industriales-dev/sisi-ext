using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_RelSubCuentas
    {
        public int Id { get; set; }
        public int IdCuenta { get; set; }
        public int Año { get; set; }
        public int Subcuenta { get; set; }
        public int SubSubcuenta { get; set; }
        public bool EsOverhaul { get; set; }
        public bool EsCuentaDepreciacion { get; set; }
        public int? CuentaDepreciacion { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public int MesesMaximoDepreciacion { get; set; }
        public bool Excluir { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioCreacion { get; set; }

        [ForeignKey("IdCuenta")]
        public virtual tblC_AF_Cuentas Cuenta { get; set; }

        [ForeignKey("IdUsuarioCreacion")]
        public virtual tblP_Usuario Usuario { get; set; }
    }
}
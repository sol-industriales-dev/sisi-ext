using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_Cuentas
    {
        public int Id { get; set; }
        public int Cuenta { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int MesesDeDepreciacion { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public bool Estatus { get; set; }
        public bool EsMaquinaria { get; set; }
        
        [ForeignKey("IdUsuarioCreacion")]
        public virtual tblP_Usuario Usuario { get; set; }
        [ForeignKey("IdCuenta")]
        public virtual List<tblC_AF_RelSubCuentas> SubCuentas { get; set; }
    }
}
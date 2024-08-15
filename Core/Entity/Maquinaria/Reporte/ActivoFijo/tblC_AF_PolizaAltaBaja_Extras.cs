using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_PolizaAltaBaja_Extras
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public int Linea { get; set; }
        public int TM { get; set; }
        public int Cuenta { get; set; }
        public int? Subcuenta { get; set; }
        public int? SubSubcuenta { get; set; }
        public string Concepto { get; set; }
        public decimal? Monto { get; set; }
        public string Factura { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int TipoActivo { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public bool? Excepcion { get; set; }

        [ForeignKey("TipoActivo")]
        public virtual tblC_AF_CatTipoActivo DescripcionTipoActivo { get; set; }

        [ForeignKey("IdUsuarioCreacion")]
        public virtual tblP_Usuario Usuario { get; set; }

        [ForeignKey("IdUsuarioModificacion")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }
    }
}

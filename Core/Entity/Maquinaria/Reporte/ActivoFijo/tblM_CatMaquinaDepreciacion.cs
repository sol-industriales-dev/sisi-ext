using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblM_CatMaquinaDepreciacion
    {
        public int Id { get; set; }
        public int IdCatMaquina { get; set; }
        public int IdPoliza { get; set; }
        public DateTime? FechaInicioDepreciacion { get; set; }
        public decimal? PorcentajeDepreciacion { get; set; }
        public int? MesesTotalesDepreciacion { get; set; }
        public int TipoDelMovimiento { get; set; }
        public int? IdPolizaReferenciaAlta { get; set; }
        public bool CapturaAutomatica { get; set; }
        public int? IdBajaCosto { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdUsuarioModificacion { get; set; }

        //[ForeignKey("IdCatMaquina")]
        //public virtual tblM_CatMaquina Maquina { get; set; }
        [NotMapped]
        public tblM_CatMaquina Maquina { get; set; }

        [ForeignKey("IdBajaCosto")]
        public virtual tblC_AF_EnviarCosto Costo { get; set; }

        [ForeignKey("IdPoliza")]
        public virtual tblC_AF_PolizaAltaBaja Poliza { get; set; }

        [ForeignKey("IdPolizaReferenciaAlta")]
        public virtual tblC_AF_PolizaAltaBaja PolizaAlta { get; set; }

        [ForeignKey("TipoDelMovimiento")]
        public virtual tblC_AF_CatTipoMovimiento Movimiento { get; set; }

        [ForeignKey("IdUsuarioCreacion")]
        public virtual tblP_Usuario Usuario { get; set; }

        [ForeignKey("IdUsuarioModificacion")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }


    }
}

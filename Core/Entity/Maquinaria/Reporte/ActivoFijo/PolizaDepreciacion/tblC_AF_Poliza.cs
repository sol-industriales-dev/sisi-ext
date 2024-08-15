using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_Poliza
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Semana { get; set; }
        public int Poliza { get; set; }
        public string TipoPoliza { get; set; }
        public DateTime FechaPoliza { get; set; }
        public decimal Cargos { get; set; }
        public decimal Abonos { get; set; }
        public int ModuloEnkontrolId { get; set; }
        public int EstatusPolizaId { get; set; }
        public bool Estatus { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioModificacionId { get; set; }
        public DateTime FechaModificacion { get; set; }

        [ForeignKey("ModuloEnkontrolId")]
        public virtual tblC_AF_ModuloEnkontrol Modulo { get; set; }
        [ForeignKey("EstatusPolizaId")]
        public virtual tblC_AF_EstatusPoliza EstatusPoliza { get; set; }
        [ForeignKey("UsuarioCreacionId")]
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioModificacionId")]
        public virtual tblP_Usuario UsuarioModificacion { get; set; }

        [ForeignKey("PolizaId")]
        public virtual List<tblC_AF_PolizaDetalle> PolizaDetalle { get; set; }
    }
}
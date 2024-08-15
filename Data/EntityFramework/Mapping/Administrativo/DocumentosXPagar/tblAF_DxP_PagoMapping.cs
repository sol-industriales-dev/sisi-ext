using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_PagoMapping : EntityTypeConfiguration<tblAF_DxP_Pago>
    {
        public tblAF_DxP_PagoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.PeriodoId).HasColumnName("periodoId");
            Property(x => x.Monto).HasPrecision(24, 6).HasColumnName("monto");
            Property(x => x.FechaPago).HasColumnName("fechaPago");
            Property(x => x.ArchivoPago).HasColumnName("archivoPago");
            Property(x => x.Estatus).HasColumnName("estatus");
            Property(x => x.FechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.UsuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.FechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.UsuarioModificacionId).HasColumnName("usuarioModificacionId");
            Property(x => x.PagoParcial).HasColumnName("pagoParcial");
            Property(r => r.ContratoID).HasColumnName("ContratoID");
           // HasRequired(x => x.ContratoDetalle).WithMany().HasForeignKey(y => y.PeriodoId);
            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.UsuarioCreacionId);
            HasRequired(x => x.UsuarioModificacion).WithMany().HasForeignKey(y => y.UsuarioModificacionId);
            ToTable("tblAF_DxP_Pago");
        }
    }
}
using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQMapping : EntityTypeConfiguration<tblAF_DxP_PQ>
    {
        public tblAF_DxP_PQMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.bancoId).HasColumnName("bancoId");
            Property(x => x.fechaFirma).HasColumnName("fechaFirma");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.monedaId).HasColumnName("monedaId");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.interes).HasColumnName("interes");
            Property(x => x.tipoCambio).HasPrecision(24, 4).HasColumnName("tipoCambio");
            Property(x => x.tipoMovimientoId).HasColumnName("tipoMovimientoId");
            Property(x => x.ctaAbonoBanco).HasColumnName("ctaAbonoBanco");
            Property(x => x.sctaAbonoBanco).HasColumnName("sctaAbonoBanco");
            Property(x => x.ssctaAbonoBanco).HasColumnName("ssctaAbonoBanco");
            Property(x => x.digitoAbonoBanco).HasColumnName("digitoAbonoBanco");
            Property(x => x.ctaCargoBanco).HasColumnName("ctaCargoBanco");
            Property(x => x.sctaCargoBanco).HasColumnName("sctaCargoBanco");
            Property(x => x.ssctaCargoBanco).HasColumnName("ssctaCargoBanco");
            Property(x => x.digitoCargoBanco).HasColumnName("digitoCargoBanco");
            Property(x => x.archivoId).HasColumnName("archivoId");
            Property(x => x.fechaLiquidacion).HasColumnName("fechaLiquidacion");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.banco).WithMany().HasForeignKey(y => y.bancoId);
            HasRequired(x => x.moneda).WithMany().HasForeignKey(y => y.monedaId);
            HasRequired(x => x.tipoMovimiento).WithMany().HasForeignKey(y => y.tipoMovimientoId);
            HasRequired(x => x.archivoPQ).WithMany().HasForeignKey(y => y.archivoId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblAF_DxP_PQ");
        }
    }
}

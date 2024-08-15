using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_PolizaDeAjusteMapping : EntityTypeConfiguration<tblC_AF_PolizaDeAjuste>
    {
        public tblC_AF_PolizaDeAjusteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.yearPoliza).HasColumnName("yearPoliza");
            Property(x => x.mesPoliza).HasColumnName("mesPoliza");
            Property(x => x.polizaPoliza).HasColumnName("polizaPoliza");
            Property(x => x.tpPoliza).HasMaxLength(2).HasColumnName("tpPoliza");
            Property(x => x.lineaPoliza).HasColumnName("lineaPoliza");
            Property(x => x.ctaPoliza).HasColumnName("ctaPoliza");
            Property(x => x.tmPoliza).HasColumnName("tmPoliza");
            Property(x => x.referenciaPoliza).HasMaxLength(15).HasColumnName("referenciaPoliza");
            Property(x => x.ccPoliza).HasMaxLength(3).HasColumnName("ccPoliza");
            Property(x => x.conceptoPoliza).HasMaxLength(40).HasColumnName("conceptoPoliza");
            Property(x => x.montoPoliza).HasPrecision(18, 2).HasColumnName("montoPoliza");
            Property(x => x.fechaPoliza).HasColumnName("fechaPoliza");
            Property(x => x.tipoPolizaDeAjusteId).HasColumnName("tipoPolizaDeAjusteId");
            Property(x => x.polizaAltaBajaId).HasColumnName("polizaAltaBajaId");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasRequired(x => x.tipoMovimiento).WithMany().HasForeignKey(y => y.tmPoliza);
            HasRequired(x => x.tipoAjuste).WithMany().HasForeignKey(y => y.tipoPolizaDeAjusteId);
            HasRequired(x => x.polizaAltaBaja).WithMany().HasForeignKey(y => y.polizaAltaBajaId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblC_AF_PolizaDeAjuste");
        }
    }
}

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
    public class tblC_AF_PolizaAltaBaja_NoMaquinaMapping : EntityTypeConfiguration<tblC_AF_PolizaAltaBaja_NoMaquina>
    {
        public tblC_AF_PolizaAltaBaja_NoMaquinaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.inicioDepreciacion).HasColumnName("inicioDepreciacion");
            Property(x => x.porcentajeDepreciacion).HasPrecision(18, 5).HasColumnName("porcentajeDepreciacion");
            Property(x => x.mesesDepreciacion).HasColumnName("mesesDepreciacion");
            Property(x => x.tipoMovimientoId).HasColumnName("tipoMovimientoId");
            Property(x => x.polizaBajaId).HasColumnName("polizaBajaId");
            Property(x => x.capturaAutomatica).HasColumnName("capturaAutomatica");
            Property(x => x.cc).HasMaxLength(3).HasColumnName("cc");
            Property(x => x.ccDescripcion).HasMaxLength(200).HasColumnName("ccDescripcion");
            Property(x => x.yearPoliza).HasColumnName("yearPoliza");
            Property(x => x.mesPoliza).HasColumnName("mesPoliza");
            Property(x => x.polizaPoliza).HasColumnName("polizaPoliza");
            Property(x => x.tpPoliza).HasMaxLength(2).HasColumnName("tpPoliza");
            Property(x => x.lineaPoliza).HasColumnName("lineaPoliza");
            Property(x => x.tmPoliza).HasColumnName("tmPoliza");
            Property(x => x.ctaPoliza).HasColumnName("ctaPoliza");
            Property(x => x.sctaPoliza).HasColumnName("sctaPoliza");
            Property(x => x.ssctaPoliza).HasColumnName("ssctaPoliza");
            Property(x => x.ccPoliza).HasMaxLength(3).HasColumnName("ccPoliza");
            Property(x => x.referenciaPoliza).HasMaxLength(15).HasColumnName("referenciaPoliza");
            Property(x => x.conceptoPoliza).HasMaxLength(350).HasColumnName("conceptoPoliza");
            Property(x => x.montoPoliza).HasPrecision(18,2).HasColumnName("montoPoliza");
            Property(x => x.fechaPoliza).HasColumnName("fechaPoliza");
            Property(x => x.factura).HasMaxLength(50).HasColumnName("factura");
            Property(x => x.fechaMovimiento).HasColumnName("fechaMovimiento");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreacionId).HasColumnName("usuarioCreacionId");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.usuarioModificacionId).HasColumnName("usuarioModificacionId");
            HasOptional(x => x.tipoMovimiento).WithMany().HasForeignKey(y => y.tipoMovimientoId);
            HasOptional(x => x.polizaBaja).WithMany().HasForeignKey(y => y.polizaBajaId);
            HasRequired(x => x.tipoMovimientoPoliza).WithMany().HasForeignKey(y => y.tmPoliza);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            ToTable("tblC_AF_PolizaAltaBaja_NoMaquina");
        }
    }
}

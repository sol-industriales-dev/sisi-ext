using Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_PolizaDetalleMapping : EntityTypeConfiguration<tblC_AF_PolizaDetalle>
    {
        public tblC_AF_PolizaDetalleMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.PolizaId).HasColumnName("polizaId");
            Property(x => x.Linea).HasColumnName("linea");
            Property(x => x.RelacionCuentaAñoId).HasColumnName("relacionCuentaAñoId");
            Property(x => x.TipoMovimientoId).HasColumnName("tipoMovimientoId");
            Property(x => x.Referencia).HasColumnName("referencia");
            Property(x => x.CC).HasColumnName("cc");
            Property(x => x.CatMaquinaId).HasColumnName("catMaquinaId");
            Property(x => x.NumeroEconomico).HasColumnName("numeroEconomico");
            Property(x => x.Concepto).HasColumnName("concepto");
            Property(x => x.Monto).HasPrecision(24, 6).HasColumnName("monto");
            Property(x => x.IClave).HasColumnName("iClave");
            Property(x => x.ITM).HasColumnName("itm");
            Property(x => x.CcId).HasColumnName("ccId");
            Property(x => x.Area).HasColumnName("area");
            Property(x => x.Cuenta_OC).HasColumnName("cuenta_oc");
            Property(x => x.AreaCuenta).HasColumnName("areaCuenta");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.Poliza).WithMany().HasForeignKey(y => y.PolizaId);
            HasRequired(x => x.RelacionCuentaAño).WithMany().HasForeignKey(y => y.RelacionCuentaAñoId);
            HasRequired(x => x.TipoMovimiento).WithMany().HasForeignKey(y => y.TipoMovimientoId);
            HasOptional(x => x.CatMaquina).WithMany().HasForeignKey(y => y.CatMaquinaId);
            HasOptional(x => x.CC).WithMany().HasForeignKey(y => y.CcId);
            ToTable("tblC_AF_PolizaDetalle");
        }
    }
}
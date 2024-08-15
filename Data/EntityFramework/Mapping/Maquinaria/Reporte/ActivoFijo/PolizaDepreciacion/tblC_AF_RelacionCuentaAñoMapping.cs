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
    public class tblC_AF_RelacionCuentaAñoMapping : EntityTypeConfiguration<tblC_AF_RelacionCuentaAño>
    {
        public tblC_AF_RelacionCuentaAñoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.SubcuentaId).HasColumnName("subcuentaId");
            Property(x => x.Año).HasColumnName("año");
            Property(x => x.CuentaMovimientoId).HasColumnName("CuentaMovimientoId");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.Subcuenta).WithMany().HasForeignKey(y => y.SubcuentaId);
            HasOptional(x => x.Cuenta).WithMany().HasForeignKey(y => y.CuentaMovimientoId);
            ToTable("tblC_AF_RelacionCuentaAño");
        }
    }
}
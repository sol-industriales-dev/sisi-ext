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
    public class tblC_AF_InfoDepreciacionMapping : EntityTypeConfiguration<tblC_AF_InfoDepreciacion>
    {
        public tblC_AF_InfoDepreciacionMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.SubCuentaId).HasColumnName("subCuentaId");
            Property(x => x.PorcentajeDepreciacion).HasColumnName("mesesDepreciacion");
            Property(x => x.FechaComienzo).HasColumnName("fechaComienzo");
            Property(x => x.TipoMovimientoId).HasColumnName("tipoMovimientoId");
            Property(x => x.Estatus).HasColumnName("estatus");
            HasRequired(x => x.SubCuenta).WithMany().HasForeignKey(y => y.SubCuentaId);
            HasOptional(x => x.TipoMovimiento).WithMany().HasForeignKey(y => y.TipoMovimientoId);
            ToTable("tblC_AF_InfoDepreciacion");
        }
    }
}
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Casos_especiales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Casos_especiales
{
    public class tblC_AF_DepreciacionEspecialMapping : EntityTypeConfiguration<tblC_AF_DepreciacionEspecial>
    {
        public tblC_AF_DepreciacionEspecialMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.monto).HasColumnName("monto");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.porcentajeDepreciacion).HasColumnName("porcentajeDepreciacion");
            Property(x => x.mesesDepreciacion).HasColumnName("mesesDepreciacion");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.ctaDepreciacion).HasColumnName("ctaDepreciacion");
            Property(x => x.sctaDepreciacion).HasColumnName("sctaDepreciacion");
            Property(x => x.ssctaDepreciacion).HasColumnName("ssctaDepreciacion");
            Property(x => x.ctaSaldo).HasColumnName("ctaSaldo");
            Property(x => x.sctaSaldo).HasColumnName("sctaSaldo");
            Property(x => x.ssctaSaldo).HasColumnName("ssctaSaldo");
            Property(x => x.estatus);
            ToTable("tblC_AF_DepreciacionEspecial");
        }
    }
}

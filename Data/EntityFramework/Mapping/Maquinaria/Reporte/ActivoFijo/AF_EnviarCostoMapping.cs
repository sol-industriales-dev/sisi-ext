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
    public class AF_EnviarCostoMapping : EntityTypeConfiguration<tblC_AF_EnviarCosto>
    {
        AF_EnviarCostoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.idEconomico).HasColumnName("idEconomico");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.mesesMaximoDepreciacion).HasColumnName("mesesMaximoDepreciacion");
            Property(x => x.porcentajeDepreciacion).HasColumnName("porcentajeDepreciacion");
            Property(x => x.mesesDepreciados).HasColumnName("mesesDepreciados");
            Property(x => x.mesesFaltantes).HasColumnName("mesesFaltantes");
            Property(x => x.semanasUltimoMesDep).HasColumnName("semanasUltimoMesDep");
            Property(x => x.depActual).HasPrecision(18, 2).HasColumnName("depActual");
            Property(x => x.depFaltante).HasPrecision(18, 2).HasColumnName("depFaltante");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.polizaBaja).HasColumnName("polizaBaja");
            Property(x => x.polizaCosto).HasColumnName("polizaCosto");
            Property(x => x.polizaAlta).HasColumnName("polizaAlta");
            Property(x => x.monto).HasPrecision(18, 2).HasColumnName("monto");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.fechaInicioDep).HasColumnName("fechaInicioDep");
            Property(x => x.enviaACosto).HasColumnName("enviaACosto");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.economico).WithMany().HasForeignKey(d => d.idEconomico);
            ToTable("tblC_AF_EnviarCosto");
        }
    }
}
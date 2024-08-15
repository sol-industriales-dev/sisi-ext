using Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Colombia
{
    public class tblC_AF_RelacionCuentaYearColombiaMapping : EntityTypeConfiguration<tblC_AF_RelacionCuentaYearColombia>
    {
        public tblC_AF_RelacionCuentaYearColombiaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subcuentaId).HasColumnName("subcuentaId");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.cuentaMovimientoId).HasColumnName("cuentaMovimientoId");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.subcuentaColombia).WithMany().HasForeignKey(y => y.subcuentaId);
            HasOptional(x => x.cuentaColombia).WithMany().HasForeignKey(y => y.cuentaMovimientoId);
            ToTable("tblC_AF_RelacionCuentaYearColombia");
        }
    }
}

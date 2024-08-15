using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class PeriodoCapturaMapping : EntityTypeConfiguration<tblX_PeriodoCaptura>
    {
        public PeriodoCapturaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_PeriodoCaptura");
        }
    }
}

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
    public class ExcepcionPeriodoCapturaMapping : EntityTypeConfiguration<tblX_ExcepcionPeriodoCaptura>
    {
        public ExcepcionPeriodoCapturaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.subcontratistaID).HasColumnName("subcontratistaID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_ExcepcionPeriodoCaptura");
        }
    }
}

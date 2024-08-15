using Core.Entity.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.ActoCondicion
{
    public class ActoAccionReaccionMapping : EntityTypeConfiguration<tblRH_AC_ActoAccionReaccion>
    {
        public ActoAccionReaccionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.acto).WithMany().HasForeignKey(x => x.actoID);
            HasRequired(x => x.accionReaccion).WithMany().HasForeignKey(x => x.accionReaccionID);
            ToTable("tblRH_AC_ActoAccionReaccion");
        }
    }
}

using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    class tblCO_DivisionCCMapping: EntityTypeConfiguration<tblCO_DivisionCC>
    {
        public tblCO_DivisionCCMapping()
         {
             HasKey(x => x.id);
             Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
             Property(x => x.division_id).HasColumnName("cc");

             Property(x => x.division_id).HasColumnName("division_id");
             HasRequired(x => x.division).WithMany(x => x.centrosCostos).HasForeignKey(d => d.division_id);


             ToTable("tblCO_DivisionCC");
         }
    }
}

using Core.Entity.StarSoft.Requisiciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Requisiciones
{
    public class REQUISCMapping : EntityTypeConfiguration<REQUISC>
    {
        public REQUISCMapping()
        {
            HasKey(x => new {x.NROREQUI, x.TIPOREQUI});
            Property(x => x.NROREQUI).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("NROREQUI");
            Property(x => x.TIPOREQUI).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("TIPOREQUI");
            ToTable("REQUISC");
        }
    }
}

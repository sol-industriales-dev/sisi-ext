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
    public class REQUISDMapping : EntityTypeConfiguration<REQUISD>
    {
        public REQUISDMapping()
        {
            HasKey(x => new { x.NROREQUI,x.REQITEM ,x.TIPOREQUI});
            Property(x => x.NROREQUI).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("NROREQUI");
            Property(x => x.REQITEM).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("REQITEM");
            Property(x => x.TIPOREQUI).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("TIPOREQUI");
            ToTable("REQUISD");
        }
    }
}

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
    public class TABAYUMapping : EntityTypeConfiguration<TABAYU>
    {
        public TABAYUMapping()
        {
            HasKey(e => new { e.TCOD, e.TCLAVE});
            Property(x => x.TCOD).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("TCOD");
            Property(x => x.TCLAVE).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("TCLAVE");
            ToTable("TABAYU");
        }
    }
}

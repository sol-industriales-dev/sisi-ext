using Core.Entity.StarSoft.Plantillas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Plantillas
{
    public class TIPOSTRABMapping : EntityTypeConfiguration<TIPOSTRAB>
    {
        public TIPOSTRABMapping() 
        {
            HasKey(x => x.TIPTRAB);
            Property(x => x.TIPTRAB).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("TIPTRAB");
            ToTable("TIPOSTRAB");
        }
    }
}
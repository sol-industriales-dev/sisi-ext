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
    public class TABUNIMEDMapping : EntityTypeConfiguration<TABUNIMED>
    {
        public TABUNIMEDMapping()
        {
            HasKey(x => x.UM_ABREV);
            Property(x => x.UM_ABREV).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("UM_ABREV");
            ToTable("TABUNIMED");
        }
    }
}

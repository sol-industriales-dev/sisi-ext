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
    public class MAEARTMapping : EntityTypeConfiguration<MAEART>
    {
        public MAEARTMapping()
        {
            HasKey(x => x.ACODIGO);
            Property(x => x.ACODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("ACODIGO");
            ToTable("MAEART");
        }
    }
}

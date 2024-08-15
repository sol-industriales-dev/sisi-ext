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
    public class NUM_DOCCOMPRASMapping : EntityTypeConfiguration<NUM_DOCCOMPRAS>
    {
        public NUM_DOCCOMPRASMapping()
        {
            HasKey(x => x.CTNCODIGO);
            Property(x => x.CTNCODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("ACODIGO");
            ToTable("NUM_DOCCOMPRAS");
        }
    }
}

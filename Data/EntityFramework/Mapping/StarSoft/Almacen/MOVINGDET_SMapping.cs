using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class MOVINGDET_SMapping : EntityTypeConfiguration<MOVINGDET_S>
    {
        public MOVINGDET_SMapping()
        {
            HasKey(x => new { x.DETD, x.DENUMDOC, x.DEITEM });
            Property(x => x.DETD).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("DETD");
            Property(x => x.DENUMDOC).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("DENUMDOC");
            Property(x => x.DEITEM).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("DEITEM");
            ToTable("MOVINGDET_S");
        }
    }
}

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
    public class MOVINGCAB_SMapping : EntityTypeConfiguration<MOVINGCAB_S>
    {
        public MOVINGCAB_SMapping()
        {
            HasKey(x => new { x.CATD, x.CANUMDOC });
            Property(x => x.CATD).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CATD");
            Property(x => x.CANUMDOC).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("CANUMDOC");
            ToTable("MOVINGCAB_S");
        }
    }
}

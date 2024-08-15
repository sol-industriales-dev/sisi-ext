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
    public class AREAMapping : EntityTypeConfiguration<AREA>
    {
        public AREAMapping()
        {
            HasKey(x => x.AREA_CODIGO);
            Property(x => x.AREA_CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("AREA_CODIGO");
            ToTable("AREA");
        }
    }
}

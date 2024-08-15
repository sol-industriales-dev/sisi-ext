using Core.Entity.StarSoft.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Compras
{
    public class COMOVD_SMapping : EntityTypeConfiguration<COMOVD_S>
    {
        public COMOVD_SMapping()
        {
            HasKey(x => new { x.OC_CNUMORD, x.OC_CITEM });
            Property(x => x.OC_CNUMORD).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("OC_CNUMORD");
            Property(x => x.OC_CITEM).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("OC_CITEM");
            Property(x => x.OC_CODSERVICIO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("OC_CODSERVICIO");
            Property(x => x.OC_NPREUNI).HasPrecision(24, 6).HasColumnName("OC_NPREUNI");
            Property(x => x.OC_NPRENET).HasPrecision(24, 6).HasColumnName("OC_NPRENET");
            Property(x => x.OC_NTOTVEN).HasPrecision(24, 6).HasColumnName("OC_NTOTVEN");
            ToTable("COMOVD_S");
        }
    }
}

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
    public class COMOVC_SMapping : EntityTypeConfiguration<COMOVC_S>
    {
        public COMOVC_SMapping()
        {
            HasKey(e => e.OC_CNUMORD);
            Property(x => x.OC_CNUMORD).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("OC_CNUMORD");
            ToTable("COMOVC_S");
        }
    }
}

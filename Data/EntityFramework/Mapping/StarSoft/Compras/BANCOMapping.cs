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
    public class BANCOMapping : EntityTypeConfiguration<BANCO>
    {
        public BANCOMapping()
        {
            HasKey(x => x.BAN_CODIGO);
            Property(x => x.BAN_CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("BAN_CODIGO");
            ToTable("BANCO");
        }
    }
}

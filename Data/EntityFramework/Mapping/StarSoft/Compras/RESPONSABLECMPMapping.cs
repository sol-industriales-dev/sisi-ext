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
    public class RESPONSABLECMPMapping : EntityTypeConfiguration<RESPONSABLECMP>
    {
        public RESPONSABLECMPMapping()
        {
            HasKey(x => x.RESPONSABLE_CODIGO);
            Property(x => x.RESPONSABLE_CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("RESPONSABLE_CODIGO");
            ToTable("RESPONSABLECMP");
        }
    }
}

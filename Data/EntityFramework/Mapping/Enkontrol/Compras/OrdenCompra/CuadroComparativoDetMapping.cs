using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class CuadroComparativoDetMapping : EntityTypeConfiguration<tblCom_CuadroComparativoDet>
    {
        public CuadroComparativoDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.precio1).HasPrecision(18, 6);
            Property(x => x.precio2).HasPrecision(18, 6);
            Property(x => x.precio3).HasPrecision(18, 6);

            ToTable("tblCom_CuadroComparativoDet");
        }
    }
}

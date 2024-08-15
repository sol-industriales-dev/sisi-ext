using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class RelacionCorreoAlmacenCCMapping : EntityTypeConfiguration<tblAlm_RelacionCorreoAlmacenCC>
    {
        public RelacionCorreoAlmacenCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.almacen).HasColumnName("almacen");
            Property(x => x.almacenistaID).HasColumnName("almacenistaID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.compradorID).HasColumnName("compradorID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblAlm_RelacionCorreoAlmacenCC");
        }
    }
}

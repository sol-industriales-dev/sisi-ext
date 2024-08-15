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
    class RelacionAlmacenPrincipalVirtualMapping : EntityTypeConfiguration<tblAlm_RelacionAlmacenPrincipalVirtual>
    {
        public RelacionAlmacenPrincipalVirtualMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.almacenPrincipal).HasColumnName("almacenPrincipal");
            Property(x => x.almacenVirtual).HasColumnName("almacenVirtual");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblAlm_RelacionAlmacenPrincipalVirtual");
        }
    }
}

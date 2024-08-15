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
    class BloqueoCCMovimientoMapping : EntityTypeConfiguration<tblAlm_BloqueoCCMovimiento>
    {
        public BloqueoCCMovimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tipo_mov).HasColumnName("tipo_mov");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblAlm_BloqueoCCMovimiento");
        }
    }
}

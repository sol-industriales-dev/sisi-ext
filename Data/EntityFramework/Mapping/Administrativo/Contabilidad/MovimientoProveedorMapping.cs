using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    class MovimientoProveedorMapping : EntityTypeConfiguration<tblEF_MovimientoProveedor>
    {
        public MovimientoProveedorMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.numeroProveedor).HasColumnName("numeroProveedor");
            Property(x => x.tipoMovimiento).HasColumnName("tipoMovimiento");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.corteMesID).HasColumnName("corteMesID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblEF_MovimientoProveedor");
        }
    }
}

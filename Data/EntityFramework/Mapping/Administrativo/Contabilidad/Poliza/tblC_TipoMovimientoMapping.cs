using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Poliza
{
    public class tblC_TipoMovimientoMapping : EntityTypeConfiguration<tblC_TipoMovimiento>
    {
        public tblC_TipoMovimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            ToTable("tblC_TipoMovimiento");
        }
    }
}

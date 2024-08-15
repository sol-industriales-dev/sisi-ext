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
    class TipoMovimientoFlujoMapping : EntityTypeConfiguration<tblEF_TipoMovimientoFlujo>
    {
        public TipoMovimientoFlujoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.tipoMovimiento).HasColumnName("tipoMovimiento");
            Property(x => x.conceptoID).HasColumnName("conceptoID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblEF_TipoMovimientoFlujo");
        }
    }
}

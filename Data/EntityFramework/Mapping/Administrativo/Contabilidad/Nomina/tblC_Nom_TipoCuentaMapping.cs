using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    class tblC_Nom_TipoCuentaMapping : EntityTypeConfiguration<tblC_Nom_TipoCuenta>
    {
        public tblC_Nom_TipoCuentaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoMovimientoId).HasColumnName("tipoMovimientoId");
            HasRequired(x => x.tipoMovimiento).WithMany().HasForeignKey(y => y.tipoMovimientoId);
            ToTable("tblC_Nom_TipoCuenta");
        }
    }
}

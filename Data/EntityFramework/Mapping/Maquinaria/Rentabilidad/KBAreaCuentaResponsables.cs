using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
   public  class KBAreaCuentaResponsablesMapping: EntityTypeConfiguration<tblM_KBAreaCuentaResponsable>
    {
       public KBAreaCuentaResponsablesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.areaCuentaID).HasColumnName("areaCuentaID");
            Property(x => x.usuarioResponsableID).HasColumnName("usuarioResponsableID");
            Property(x => x.estatus).HasColumnName("estatus");

            HasRequired(x => x.usuarioResponsable).WithMany(x => x.areaCuentaResponsable).HasForeignKey(d => d.usuarioResponsableID);

            HasRequired(x => x.areaCuenta).WithMany().HasForeignKey(d => d.areaCuentaID);
            ToTable("tblM_KBDivisionDetalle");
        }
    }
}

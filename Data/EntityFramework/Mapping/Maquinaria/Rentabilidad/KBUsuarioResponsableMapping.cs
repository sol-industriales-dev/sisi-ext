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
    public class KBUsuarioResponsableMapping : EntityTypeConfiguration<tblM_KBUsuarioResponsable>
    {
        KBUsuarioResponsableMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioResponsableID).HasColumnName("usuarioResponsableID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuarioResponsable).WithMany().HasForeignKey(d => d.usuarioResponsableID);
            ToTable("tblM_KBUsuarioResponsable");

        }

    }
}

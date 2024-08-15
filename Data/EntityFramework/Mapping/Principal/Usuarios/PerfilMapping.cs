using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Usuarios
{
    class PerfilMapping : EntityTypeConfiguration<tblP_Perfil>
    {
        PerfilMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreación");
            Property(x => x.estatus).HasColumnName("estatus");
          //  HasRequired(x => x.tblP_Usuario).WithMany().HasForeignKey(y => y.id);
            //     HasRequired(x => x.puesto).WithMany().HasForeignKey(y => y.idPuesto);
            ToTable("tblP_Perfil");
        }
    }
}

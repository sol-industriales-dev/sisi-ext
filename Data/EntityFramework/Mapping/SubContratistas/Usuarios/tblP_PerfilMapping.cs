using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas.Usuarios
{
    class tblP_PerfilMapping : EntityTypeConfiguration<tblP_Perfil>
    {
        tblP_PerfilMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreación");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblP_Perfil");
        }
    }
}

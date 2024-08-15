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
    public class CorreosVinculadosMapping : EntityTypeConfiguration<tblP_CorreosVinculados>
    {
        public CorreosVinculadosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioPrincipalID).HasColumnName("usuarioPrincipalID");
            HasRequired(x => x.usuarioPrincipal).WithMany().HasForeignKey(y => y.usuarioPrincipalID);
            Property(x => x.usuarioVinculadoID).HasColumnName("usuarioVinculadoID");
            HasRequired(x => x.usuarioVinculado).WithMany().HasForeignKey(y => y.usuarioVinculadoID);

            
            ToTable("tblP_CorreosVinculados");
        }
    }
}

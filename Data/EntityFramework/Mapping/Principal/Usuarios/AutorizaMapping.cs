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
    public class AutorizaMapping : EntityTypeConfiguration<tblP_Autoriza>
    {
        public AutorizaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.cc_usuario_ID).HasColumnName("cc_usuario_ID");
            Property(x => x.perfilAutorizaID).HasColumnName("perfilAutorizaID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);
            ToTable("tblP_Autoriza");
        }
    }
}

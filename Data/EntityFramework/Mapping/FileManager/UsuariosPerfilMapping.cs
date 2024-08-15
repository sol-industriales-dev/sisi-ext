using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.FileManager
{
    public class UsuariosPerfilMapping : EntityTypeConfiguration<tblFM_UsuariosPerfil>
    {
        public UsuariosPerfilMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.perfilID).HasColumnName("perfilID");
            ToTable("tblFM_UsuariosPerfil");
        }
    }
}

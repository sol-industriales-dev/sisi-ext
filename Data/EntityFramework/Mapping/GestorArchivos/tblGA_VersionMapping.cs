using Core.Entity.GestorArchivos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.GestorArchivos
{
    public class tblGA_VersionMapping : EntityTypeConfiguration<tblGA_Version>
    {
        public tblGA_VersionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.directorioID).HasColumnName("directorioID");
            HasRequired(x => x.directorio).WithMany(x => x.versiones).HasForeignKey(d => d.directorioID);
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.versionesDirectorios).HasForeignKey(d => d.usuarioID);
            Property(x => x.version).HasColumnName("version");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblGA_Version");
        }
    }
}

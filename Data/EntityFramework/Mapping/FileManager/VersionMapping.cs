using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Data.EntityFramework.Mapping.FileManager
{
    public class VersionMapping : EntityTypeConfiguration<tblFM_Version>
    {
        public VersionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.archivoID).HasColumnName("archivoID");
            HasRequired(x => x.archivo).WithMany().HasForeignKey(d => d.archivoID);
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuario).WithMany(x => x.versionesArchivos).HasForeignKey(d => d.usuarioCreadorID);
            Property(x => x.version).HasColumnName("version");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaEdicion).HasColumnName("fechaEdicion");
            Property(x => x.numeroArchivo).HasColumnName("numeroArchivo");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.abreviacion).HasColumnName("abreviacion");
            Property(x => x.considerarse).HasColumnName("considerarse");
            ToTable("tblFM_Version");
        }
    }
}
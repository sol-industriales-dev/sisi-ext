using Core.Entity.FileManager;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Data.EntityFramework.Mapping.FileManager
{
    public class PermisoMapping : EntityTypeConfiguration<tblFM_Permiso>
    {
        public PermisoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.permisosGestorArchivos).HasForeignKey(d => d.usuarioID);
            Property(x => x.archivoID).HasColumnName("archivoID");
            HasRequired(x => x.archivo).WithMany().HasForeignKey(d => d.archivoID);
            Property(x => x.puedeSubir).HasColumnName("puedeSubir");
            Property(x => x.puedeEliminar).HasColumnName("puedeEliminar");
            Property(x => x.puedeDescargarArchivo).HasColumnName("puedeDescargarArchivo");
            Property(x => x.puedeDescargarCarpeta).HasColumnName("puedeDescargarCarpeta");
            Property(x => x.puedeActualizar).HasColumnName("puedeActualizar");
            Property(x => x.puedeCrear).HasColumnName("puedeCrear");
            Property(x => x.estatusVista).HasColumnName("estatusVista");
            ToTable("tblFM_Permiso");
        }
    }
}

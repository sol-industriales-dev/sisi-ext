using Core.Entity.GestorArchivos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Data.EntityFramework.Mapping.GestorArchivos
{
    public class tblGA_PermisosMapping : EntityTypeConfiguration<tblGA_Permisos>
    {
        public tblGA_PermisosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.permisosDirectorios).HasForeignKey(d => d.usuarioID);
            Property(x => x.directorioID).HasColumnName("directorioID");
            HasRequired(x => x.directorio).WithMany(x => x.permisos).HasForeignKey(d => d.directorioID);
            Property(x => x.puedeSubir).HasColumnName("puedeSubir");
            Property(x => x.puedeEliminar).HasColumnName("puedeEliminar");
            Property(x => x.puedeDescargarArchivo).HasColumnName("puedeDescargarArchivo");
            Property(x => x.puedeDescargarCarpeta).HasColumnName("puedeDescargarCarpeta");
            Property(x => x.puedeActualizar).HasColumnName("puedeActualizar");
            Property(x => x.puedeCrear).HasColumnName("puedeCrear");
            ToTable("tblGA_Permisos");
        }
    }
}

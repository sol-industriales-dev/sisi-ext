using Core.Entity.FileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.FileManager
{
 
    public class Permisos_UsuarioMapping : EntityTypeConfiguration<tblFM_Permisos_Usuario>
    {
        public Permisos_UsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany(x => x.permisosUsuarioGestorArchivos).HasForeignKey(d => d.usuarioID);
            Property(x => x.archivoID).HasColumnName("archivoID");
            HasRequired(x => x.archivo).WithMany().HasForeignKey(d => d.archivoID);
            Property(x => x.tipoPermiso).HasColumnName("tipoPermiso");
            Property(x => x.puedeSubir).HasColumnName("puedeSubir");
            Property(x => x.puedeEliminar).HasColumnName("puedeEliminar");
            Property(x => x.puedeDescargarArchivo).HasColumnName("puedeDescargarArchivo");
            Property(x => x.puedeDescargarCarpeta).HasColumnName("puedeDescargarCarpeta");
            Property(x => x.puedeActualizar).HasColumnName("puedeActualizar");
            Property(x => x.puedeCrear).HasColumnName("puedeCrear");
            Property(x => x.estatusVista).HasColumnName("estatusVista");
            ToTable("tblFM_Permisos_Usuario");
        }
    }
}

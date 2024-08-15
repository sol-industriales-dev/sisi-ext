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
    public class PermisoEspecialObraMapping : EntityTypeConfiguration<tblFM_PermisoEspecialObra>
    {
        public PermisoEspecialObraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.tipoPermiso).HasColumnName("tipoPermiso");
            Property(x => x.entidadID).HasColumnName("entidadID");
            Property(x => x.puedeSubir).HasColumnName("puedeSubir");
            Property(x => x.puedeEliminar).HasColumnName("puedeEliminar");
            Property(x => x.puedeDescargarArchivo).HasColumnName("puedeDescargarArchivo");
            Property(x => x.puedeDescargarCarpeta).HasColumnName("puedeDescargarCarpeta");
            Property(x => x.puedeActualizar).HasColumnName("puedeActualizar");
            Property(x => x.puedeCrear).HasColumnName("puedeCrear");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            ToTable("tblFM_PermisoEspecialObra");
        }
    }
}

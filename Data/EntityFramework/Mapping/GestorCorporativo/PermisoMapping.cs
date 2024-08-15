using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Core.Entity.GestorCorporativo;

namespace Data.EntityFramework.Mapping.GestorCorporativo
{
    public class PermisoMapping : EntityTypeConfiguration<tblGC_Permiso>
    {
        public PermisoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioID);
            Property(x => x.archivoID).HasColumnName("archivoID");
            HasRequired(x => x.archivo).WithMany().HasForeignKey(x => x.archivoID);
            Property(x => x.puedeSubir).HasColumnName("puedeSubir");
            Property(x => x.puedeEliminar).HasColumnName("puedeEliminar");
            Property(x => x.puedeDescargarArchivo).HasColumnName("puedeDescargarArchivo");
            Property(x => x.puedeDescargarCarpeta).HasColumnName("puedeDescargarCarpeta");
            Property(x => x.puedeCrear).HasColumnName("puedeCrear");
            ToTable("tblGC_Permiso");
        }
    }
}

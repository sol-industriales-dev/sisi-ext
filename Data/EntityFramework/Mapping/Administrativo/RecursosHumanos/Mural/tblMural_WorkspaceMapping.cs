using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Mural
{
    public class tblMural_WorkspaceMapping : EntityTypeConfiguration<tblMural_Workspace>
    {
        public tblMural_WorkspaceMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.icono).HasColumnName("icono");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.contenido).HasColumnName("contenido");
            Property(x => x.modificado).HasColumnName("modificado");
            Property(x => x.estatus).HasColumnName("estatus");
            //HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);
            ToTable("tblMural_Workspace");
        }
    }
}

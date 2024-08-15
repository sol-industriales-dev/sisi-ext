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
    public class tblMural_Workspace_MembersMapping : EntityTypeConfiguration<tblMural_Workspace_Members>
    {
        public tblMural_Workspace_MembersMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.workSpaceID).HasColumnName("workSpaceID");
            Property(x => x.usuarioNombre).HasColumnName("usuarioNombre");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            //HasRequired(x => x.workSpace).WithMany().HasForeignKey(y => y.workSpaceID);
            //HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);
            ToTable("tblMural_Workspace_Members");
        }
    }
}

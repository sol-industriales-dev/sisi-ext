using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Usuarios
{
    public class tblP_RolesGrupoTrabajoMapping : EntityTypeConfiguration<tblP_RolesGrupoTrabajo>
    {
        public tblP_RolesGrupoTrabajoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.cantDiasLaborales).HasColumnName("cantDiasLaborales");
            Property(x => x.cantDiasDescanso).HasColumnName("cantDiasDescanso");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblP_RolesGrupoTrabajo");
        }
    }
}

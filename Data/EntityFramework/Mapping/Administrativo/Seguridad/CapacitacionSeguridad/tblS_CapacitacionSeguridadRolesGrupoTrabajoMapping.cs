using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadRolesGrupoTrabajoMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadRolesGrupoTrabajo>
    {
        public tblS_CapacitacionSeguridadRolesGrupoTrabajoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.cantDiasLaborales).HasColumnName("cantDiasLaborales");
            Property(x => x.CantDiasDescando).HasColumnName("CantDiasDescando");
            Property(x => x.cantDiasLaborales2).HasColumnName("cantDiasLaborales2");
            Property(x => x.CantDiasDescando2).HasColumnName("CantDiasDescando2");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.mixto).HasColumnName("mixto");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblS_CapacitacionSeguridadRolesGrupoTrabajo");
        }
    }
}

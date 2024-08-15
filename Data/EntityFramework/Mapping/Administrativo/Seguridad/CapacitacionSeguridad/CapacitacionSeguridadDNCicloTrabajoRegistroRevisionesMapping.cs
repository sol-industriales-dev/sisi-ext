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
    public class CapacitacionSeguridadDNCicloTrabajoRegistroRevisionesMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones>
    {
        public CapacitacionSeguridadDNCicloTrabajoRegistroRevisionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.criterioID).HasColumnName("criterioID");
            Property(x => x.acredito).HasColumnName("acredito");
            Property(x => x.cicloRegistroID).HasColumnName("cicloRegistroID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones");
        }
    }
}

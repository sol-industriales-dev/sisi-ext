using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNCicloTrabajoAreaSeguimientoMapping : EntityTypeConfiguration<tblS_CapacitacionDNCicloTrabajoAreaSeguimiento>
    {
        public tblS_CapacitacionDNCicloTrabajoAreaSeguimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(x => x.usuarioModificacionId);
            ToTable("tblS_CapacitacionDNCicloTrabajoAreaSeguimiento");
        }
    }
}

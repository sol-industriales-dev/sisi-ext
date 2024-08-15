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
    public class tblS_CapacitacionDNCicloTrabajoControlAreaSeguimientoMapping : EntityTypeConfiguration<tblS_CapacitacionDNCicloTrabajoControlAreaSeguimiento>
    {
        public tblS_CapacitacionDNCicloTrabajoControlAreaSeguimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.areaSeguimiento).WithMany().HasForeignKey(x => x.areaSeguimientoId);
            HasRequired(x => x.cicloTrabajo).WithMany().HasForeignKey(x => x.cicloTrabajoId);
            ToTable("tblS_CapacitacionDNCicloTrabajoControlAreaSeguimiento");
        }
    }
}

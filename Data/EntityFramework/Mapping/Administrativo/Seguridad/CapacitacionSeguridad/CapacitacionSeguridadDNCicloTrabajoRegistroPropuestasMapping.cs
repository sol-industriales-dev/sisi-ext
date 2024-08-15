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
    public class CapacitacionSeguridadDNCicloTrabajoRegistroPropuestasMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas>
    {
        public CapacitacionSeguridadDNCicloTrabajoRegistroPropuestasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.propuesta).HasColumnName("propuesta");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.evaluador).HasColumnName("evaluador");
            Property(x => x.solventada).HasColumnName("solventada");
            Property(x => x.cicloRegistroID).HasColumnName("cicloRegistroID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas");
        }
    }
}

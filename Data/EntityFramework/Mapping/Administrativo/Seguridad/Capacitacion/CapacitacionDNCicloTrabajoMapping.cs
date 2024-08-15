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
    public class CapacitacionDNCicloTrabajoMapping : EntityTypeConfiguration<tblS_CapacitacionDNCicloTrabajo>
    {
        public CapacitacionDNCicloTrabajoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.titulo).HasColumnName("titulo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.tipoCiclo).HasColumnName("tipoCiclo");
            Property(x => x.fechaCiclo).HasColumnName("fechaCiclo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionDNCicloTrabajo");
        }
    }
}

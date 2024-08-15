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
    public class CapacitacionDNDeteccionPrimariaMapping : EntityTypeConfiguration<tblS_CapacitacionDNDeteccionPrimaria>
    {
        public CapacitacionDNDeteccionPrimariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionDNDeteccionPrimaria");
        }
    }
}

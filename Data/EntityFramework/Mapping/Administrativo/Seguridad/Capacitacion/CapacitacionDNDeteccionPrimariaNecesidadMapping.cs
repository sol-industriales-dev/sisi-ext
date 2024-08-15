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
    public class CapacitacionDNDeteccionPrimariaNecesidadMapping : EntityTypeConfiguration<tblS_CapacitacionDNDeteccionPrimariaNecesidad>
    {
        public CapacitacionDNDeteccionPrimariaNecesidadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.metodo).HasColumnName("metodo");
            Property(x => x.detecciones).HasColumnName("detecciones");
            Property(x => x.accionesCursoID).HasColumnName("accionesCursoID");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.deteccionPrimariaID).HasColumnName("deteccionPrimariaID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionDNDeteccionPrimariaNecesidad");
        }
    }
}

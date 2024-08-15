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
    public class CapacitacionDNRecorridoHallazgoMapping : EntityTypeConfiguration<tblS_CapacitacionDNRecorridoHallazgo>
    {
        public CapacitacionDNRecorridoHallazgoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.deteccion).HasColumnName("deteccion");
            Property(x => x.recomendacion).HasColumnName("recomendacion");
            Property(x => x.clasificacion).HasColumnName("clasificacion");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.evaluador).HasColumnName("evaluador");
            Property(x => x.solventada).HasColumnName("solventada");
            Property(x => x.recorridoID).HasColumnName("recorridoID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionDNRecorridoHallazgo");
        }
    }
}

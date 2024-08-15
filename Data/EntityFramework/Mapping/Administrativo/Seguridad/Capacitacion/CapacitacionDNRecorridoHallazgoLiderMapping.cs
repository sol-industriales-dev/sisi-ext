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
    public class CapacitacionDNRecorridoHallazgoLiderMapping : EntityTypeConfiguration<tblS_CapacitacionDNRecorridoHallazgoLider>
    {
        public CapacitacionDNRecorridoHallazgoLiderMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.lider).HasColumnName("lider");
            Property(x => x.hallazgoID).HasColumnName("hallazgoID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionDNRecorridoHallazgoLider");
        }
    }
}

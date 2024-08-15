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
    public class CapacitacionSeguridadDNRecorridoAreasMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadDNRecorridoAreas>
    {
        public CapacitacionSeguridadDNRecorridoAreasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.recorridoID).HasColumnName("recorridoID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadDNRecorridoAreas");
        }
    }
}

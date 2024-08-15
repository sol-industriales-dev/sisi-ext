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
    public class tblS_CapacitacionDNRecorridoEvidenciasMapping : EntityTypeConfiguration<tblS_CapacitacionDNRecorridoEvidencias>
    {
        public tblS_CapacitacionDNRecorridoEvidenciasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblS_CapacitacionDNRecorridoEvidencias");
        }
    }
}

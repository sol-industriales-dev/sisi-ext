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
    public class CapacitacionRazonSocialMapping : EntityTypeConfiguration<tblS_CapacitacionRazonSocial>
    {
        public CapacitacionRazonSocialMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.razonSocial).HasColumnName("razonSocial");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.division).HasColumnName("division");

            ToTable("tblS_CapacitacionRazonSocial");
        }
    }
}

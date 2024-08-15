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
    public class tblS_CapacitacionIHHInteresadoMapping : EntityTypeConfiguration<tblS_CapacitacionIHHInteresado>
    {
        public tblS_CapacitacionIHHInteresadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.colaboradorAdiestrador).WithMany().HasForeignKey(x => x.colaboradorAdiestradorId);
            HasRequired(x => x.interesado).WithMany().HasForeignKey(x => x.interesadoId);
            ToTable("tblS_CapacitacionIHHInteresado");
        }
    }
}

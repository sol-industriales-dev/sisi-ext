using Core.Entity.Administrativo.RecursosHumanos.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Capacitacion
{
    class tblS_CapacitacionCapitalHumanoPuestoMandoMapping : EntityTypeConfiguration<tblS_CapacitacionCapitalHumanoPuestoMando>
    {
        public tblS_CapacitacionCapitalHumanoPuestoMandoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblS_CapacitacionCapitalHumanoPuestoMando");
        }
    }
}

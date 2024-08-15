using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    class tblS_IncidentesEmpleadosTurnoMapping : EntityTypeConfiguration<tblS_IncidentesEmpleadosTurno>
    {
        public tblS_IncidentesEmpleadosTurnoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.turno).HasColumnName("turno");

            ToTable("tblS_IncidentesEmpleadosTurno");
        }
    }
}

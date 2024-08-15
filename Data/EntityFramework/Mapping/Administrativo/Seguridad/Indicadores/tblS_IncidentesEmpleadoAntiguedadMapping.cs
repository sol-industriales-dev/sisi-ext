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
    class tblS_IncidentesEmpleadoAntiguedadMapping : EntityTypeConfiguration<tblS_IncidentesEmpleadoAntiguedad>
    {
        public tblS_IncidentesEmpleadoAntiguedadMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.antiguedadEmpleado).HasColumnName("antiguedadEmpleado");

            ToTable("tblS_IncidentesEmpleadoAntiguedad");
        }
    }
}

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
    class tblS_IncidentesTipoLesionMapping : EntityTypeConfiguration<tblS_IncidentesTipoLesion>
    {
        public tblS_IncidentesTipoLesionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoLesion).HasColumnName("tipoLesion");

            ToTable("tblS_IncidentesTipoLesion");
        }
    }
}

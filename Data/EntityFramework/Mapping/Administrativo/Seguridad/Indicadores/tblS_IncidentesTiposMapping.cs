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
    class tblS_IncidentesTiposMapping : EntityTypeConfiguration<tblS_IncidentesTipos>
    {
        public tblS_IncidentesTiposMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.incidenteTipo).HasColumnName("incidenteTipo");

            Property(x => x.clasificacion_id).HasColumnName("clasificacion_id");
            HasRequired(x => x.clasificacion).WithMany(x => x.incidentesTipo).HasForeignKey(d => d.clasificacion_id);


            ToTable("tblS_IncidentesTipos");
        }

    }
}

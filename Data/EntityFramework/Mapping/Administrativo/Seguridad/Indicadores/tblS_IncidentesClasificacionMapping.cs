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
    class tblS_IncidentesClasificacionMapping : EntityTypeConfiguration<tblS_IncidentesClasificacion>
    {
        public tblS_IncidentesClasificacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.clasificacion).HasColumnName("clasificacion");

            Property(x => x.tipoEvento_id).HasColumnName("tipoEvento_id");
            HasRequired(x => x.tipoEvento).WithMany(x => x.clasificaciones).HasForeignKey(d => d.tipoEvento_id);


            ToTable("tblS_IncidentesClasificacion");
        }
    }
}

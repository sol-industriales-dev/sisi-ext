using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    class tblS_IncidentesTipoEventosMapping : EntityTypeConfiguration<tblS_IncidentesTipoEventos>
    {
        public tblS_IncidentesTipoEventosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoEvento).HasColumnName("tipoEvento");

            ToTable("tblS_IncidentesTipoEventos");
        }
    }
}

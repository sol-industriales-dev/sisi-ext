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
    class tblS_IncidentesCausasBasicasMapping : EntityTypeConfiguration<tblS_IncidentesCausasBasicas>
    {
        public tblS_IncidentesCausasBasicasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.causaBasica).HasColumnName("causaBasica");

            Property(x => x.incidente_id).HasColumnName("incidente_id");
            HasRequired(x => x.Incidente).WithMany(x => x.CausasBasicas).HasForeignKey(d => d.incidente_id);


            ToTable("tblS_IncidentesCausasBasicas");
        }
    }
}

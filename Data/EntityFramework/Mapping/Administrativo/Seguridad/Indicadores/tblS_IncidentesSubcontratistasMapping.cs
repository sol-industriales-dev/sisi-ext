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
    public class tblS_IncidentesSubcontratistasMapping : EntityTypeConfiguration<tblS_IncidentesSubcontratistas>
    {

        public tblS_IncidentesSubcontratistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
          

            ToTable("tblS_IncidentesSubcontratistas");
        }
    }
}

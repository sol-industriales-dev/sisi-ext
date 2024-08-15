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
    class tblS_IncidentesPartesCuerpoMapping : EntityTypeConfiguration<tblS_IncidentesPartesCuerpo>
    {
        public tblS_IncidentesPartesCuerpoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.parteCuerpo).HasColumnName("parteCuerpo");

            ToTable("tblS_IncidentesPartesCuerpo");
        }
    }
}

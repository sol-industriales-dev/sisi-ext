using Core.Entity.Administrativo.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Reclutamientos
{
    class tblRH_REC_ED_RelacionExpedienteArchivoMapping : EntityTypeConfiguration<tblRH_REC_ED_RelacionExpedienteArchivo>
    {
        public tblRH_REC_ED_RelacionExpedienteArchivoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_REC_ED_RelacionExpedienteArchivo");
        }
    }
}

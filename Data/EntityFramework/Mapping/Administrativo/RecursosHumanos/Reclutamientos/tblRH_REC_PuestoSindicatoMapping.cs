using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Reclutamientos
{
    class tblRH_REC_PuestoSindicatoMapping : EntityTypeConfiguration<tblRH_REC_PuestoSindicato>
    {
        public tblRH_REC_PuestoSindicatoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblRH_REC_PuestoSindicato");
        }
    }
}

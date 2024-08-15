
using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Expediciones_ArchivosMapping : EntityTypeConfiguration<tblRH_REC_Expediciones_Archivos>
    {
        public tblRH_REC_Expediciones_ArchivosMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_REC_Expediciones_Archivos");
        }
    }
}

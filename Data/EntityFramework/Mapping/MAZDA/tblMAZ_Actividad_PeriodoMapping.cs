using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.MAZDA
{
    class tblMAZ_Actividad_PeriodoMapping : EntityTypeConfiguration<tblMAZ_Actividad_Periodo>
    {
        public tblMAZ_Actividad_PeriodoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.periodo).HasColumnName("periodo");
            ToTable("tblMAZ_Actividad_Periodo");
        }
    }
}

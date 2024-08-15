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
    class tblMAZ_PlanMesMapping : EntityTypeConfiguration<tblMAZ_PlanMes>
    {
        public tblMAZ_PlanMesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuadrillaID).HasColumnName("cuadrillaID");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_PlanMes");
        }
    }
}

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
    class tblMAZ_PlanMes_Detalle_DiaMapping : EntityTypeConfiguration<tblMAZ_PlanMes_Detalle_Dia>
    {
        public tblMAZ_PlanMes_Detalle_DiaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.planMesDetalleID).HasColumnName("planMesDetalleID");
            Property(x => x.dia).HasColumnName("dia");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_PlanMes_Detalle_Dia");
        }
    }
}

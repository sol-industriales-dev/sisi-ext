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
    class tblMAZ_PlanMes_DetalleMapping : EntityTypeConfiguration<tblMAZ_PlanMes_Detalle>
    {
        public tblMAZ_PlanMes_DetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.planMesID).HasColumnName("planMesID");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.equipoID).HasColumnName("equipoAreaID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblMAZ_PlanMes_Detalle");
        }
    }
}

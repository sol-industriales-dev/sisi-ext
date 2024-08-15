using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class StandbyDetMapping : EntityTypeConfiguration<tblM_DetStandby>
    {
        public StandbyDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.DiaParo).HasColumnName("DiaParo");
            Property(x => x.FechaCaptura).HasColumnName("FechaCaptura");
            Property(x => x.HorometroFinal).HasColumnName("HorometroFinal");
            Property(x => x.HorometroInicial).HasColumnName("HorometroInicial");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.StandByID).HasColumnName("StandByID");
            Property(x => x.TipoConsideracion).HasColumnName("TipoConsideracion");
            Property(x => x.estatus).HasColumnName("StandByID");
            Property(x => x.FechaFinStandby).HasColumnName("TipoConsideracion");

            HasRequired(x => x.StandBy).WithMany().HasForeignKey(y => y.StandByID);


            ToTable("tblM_DetStandby");
        }

    }
}

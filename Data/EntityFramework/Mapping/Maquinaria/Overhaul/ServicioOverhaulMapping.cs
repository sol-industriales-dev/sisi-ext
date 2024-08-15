using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class ServicioOverhaulMapping : EntityTypeConfiguration<tblM_CatServicioOverhaul>
    {
        ServicioOverhaulMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoServicioID).HasColumnName("tipoServicioID");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            Property(x => x.centroCostos).HasColumnName("centroCostos");
            Property(x => x.cicloVidaHoras).HasColumnName("cicloVidaHoras");
            Property(x => x.horasCicloActual).HasColumnName("horasCicloActual");
            Property(x => x.fechaAsignacion).HasColumnName("fechaAsignacion");            
            Property(x => x.fechaAplicacion).HasColumnName("fechaAplicacion");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.maquina).WithMany().HasForeignKey(y => y.maquinaID);
            HasRequired(x => x.servicio).WithMany().HasForeignKey(y => y.tipoServicioID);
            ToTable("tblM_CatServicioOverhaul");
        }
    }
}
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
    public class trackServicioOverhaulMapping : EntityTypeConfiguration<tblM_trackServicioOverhaul>
    {
        trackServicioOverhaulMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.servicioID).HasColumnName("servicioID");
            Property(x => x.tipoServicioID).HasColumnName("tipoServicioID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.horasCiclo).HasColumnName("horasCiclo");
            Property(x => x.target).HasColumnName("target");
            Property(x => x.archivos).HasColumnName("archivos");
            HasRequired(x => x.servicio).WithMany().HasForeignKey(x => x.servicioID);
            ToTable("tblM_trackServicioOverhaul");
        }
    }
}
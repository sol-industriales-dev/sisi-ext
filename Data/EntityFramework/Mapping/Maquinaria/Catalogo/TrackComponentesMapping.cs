using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    class TrackComponentesMapping : EntityTypeConfiguration<tblM_trackComponentes>
    {
        TrackComponentesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.componenteID).HasColumnName("componenteID");
            Property(x => x.tipoLocacion).HasColumnName("tipoLocacion");
            Property(x => x.locacionID).HasColumnName("locacionID");
            Property(x => x.maquinariaID).HasColumnName("maquinariaID");
            Property(x => x.JsonFechasCRC).HasColumnName("JsonFechasCRC");
            Property(x => x.JsonArchivos).HasColumnName("JsonArchivos");
            Property(x => x.locacion).HasColumnName("locacion");
            Property(x => x.reciclado).HasColumnName("reciclado");
            Property(x => x.costoCRC).HasColumnName("costoCRC");
            Property(x => x.horasAcumuladas).HasColumnName("horasAcumuladas");
            Property(x => x.horasCiclo).HasColumnName("horasCiclo");
            HasRequired(x => x.componente).WithMany().HasForeignKey(x => x.componenteID);
            ToTable("tblM_trackComponentes");
            
        }
    }
}
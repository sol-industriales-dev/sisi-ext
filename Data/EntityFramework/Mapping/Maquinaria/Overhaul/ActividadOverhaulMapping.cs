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
    public class ActividadOverhaulMapping : EntityTypeConfiguration<tblM_CatActividadOverhaul>
    {
        ActividadOverhaulMapping() 
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.horasDuracion).HasColumnName("horasDuracion");
            Property(x => x.modeloID).HasColumnName("modeloID");
            Property(x => x.dia).HasColumnName("dia");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.reporteEjecutivo).HasColumnName("reporteEjecutivo");
            ToTable("tblM_CatActividadOverhaul");
        }
    }
}
using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MR_ImpractosSobreObjetivosDelProyectoMapping : EntityTypeConfiguration<tblCO_MR_ImpractosSobreObjetivosDelProyecto>
    {
        public tblCO_MR_ImpractosSobreObjetivosDelProyectoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idMatriz).HasColumnName("idMatriz");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.tiempo).HasColumnName("tiempo");
            Property(x => x.costo).HasColumnName("costo");
            Property(x => x.calidad).HasColumnName("calidad");
            Property(x => x.baja).HasColumnName("baja");
            Property(x => x.bajaFin).HasColumnName("bajaFin");

            ToTable("tblCO_MR_ImpractosSobreObjetivosDelProyecto");
        }
    }
}

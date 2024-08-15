using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    public class tblRH_PP_PlantillaPersonal_AutMapping : EntityTypeConfiguration<tblRH_PP_PlantillaPersonal_Aut>
    {
        public tblRH_PP_PlantillaPersonal_AutMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.plantillaID).HasColumnName("plantillaID");
            Property(x => x.aprobadorClave).HasColumnName("aprobadorClave");
            Property(x => x.aprobadorNombre).HasColumnName("aprobadorNombre");
            Property(x => x.aprobadorPuesto).HasColumnName("aprobadorPuesto");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.autorizando).HasColumnName("autorizando");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.fecha).HasColumnName("fecha");
            HasRequired(x => x.plantilla).WithMany().HasForeignKey(y => y.plantillaID);
            ToTable("tblRH_PP_PlantillaPersonal_Aut");
        }
    }
}

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
    public class tblRH_PP_PlantillaPersonalMapping : EntityTypeConfiguration<tblRH_PP_PlantillaPersonal>
    {
        public tblRH_PP_PlantillaPersonalMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ccID).HasColumnName("ccID");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fechaMod).HasColumnName("fechaMod");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);
            HasMany(x => x.listDetalle).WithRequired(x => x.plantilla).HasForeignKey(x => x.plantillaID);
            HasMany(x => x.listAutorizadores).WithRequired(x => x.plantilla).HasForeignKey(x => x.plantillaID);
            Property(x => x.plantillaEKID).HasColumnName("plantillaEKID");
            Property(x => x.tabuladorEKID).HasColumnName("tabuladorEKID");
            ToTable("tblRH_PP_PlantillaPersonal");
        }
    }
}

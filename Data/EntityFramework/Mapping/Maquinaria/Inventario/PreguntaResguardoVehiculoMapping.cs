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
    public class PreguntaResguardoVehiculoMapping : EntityTypeConfiguration<tblM_CatPreguntaResguardoVehiculo>
    {

        PreguntaResguardoVehiculoMapping(){

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.GrupoID).HasColumnName("GrupoID");
            Property(x => x.Pregunta).HasColumnName("Pregunta");
            Property(x => x.TipoPregunta).HasColumnName("TipoPregunta");
            Property(x => x.DescripcionGrupo).HasColumnName("DescripcionGrupo");
            

            ToTable("tblM_CatPreguntaResguardoVehiculo");

        }
      
    }
}

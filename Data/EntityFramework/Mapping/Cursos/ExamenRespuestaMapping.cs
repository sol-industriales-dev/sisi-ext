using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Core.Entity.Cursos;

namespace Data.EntityFramework.Mapping.Cursos
{
   public class ExamenRespuestaMapping: EntityTypeConfiguration<tblCU_ExamenRespuesta>
   {
       public ExamenRespuestaMapping()
           {
               HasKey(x => x.id);
               Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
               Property(x => x.idPregunta).HasColumnName("idPregunta");
               Property(x => x.opcion).HasColumnName("opcion");
               Property(x => x.respuesta).HasColumnName("respuesta");
               Property(x => x.correcta).HasColumnName("correcta");
               ToTable("tblCU_ExamenRespuesta");
           }
    }
}

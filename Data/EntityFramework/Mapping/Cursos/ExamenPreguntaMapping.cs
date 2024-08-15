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
    public class ExamenPreguntaMapping: EntityTypeConfiguration<tblCU_ExamenPregunta>
    {
        public ExamenPreguntaMapping()
           {
               HasKey(x => x.id);
               Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
               Property(x => x.idExamen).HasColumnName("idExamen");//llave foranea
               Property(x => x.abierta).HasColumnName("abierta");
               //Property(x => x.correcto).HasColumnName("respuesta");
               Property(x => x.pregunta).HasColumnName("pregunta");
               ToTable("tblCU_ExamenPregunta");
           }
    }
}

using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas
{
    public class PreguntasMapping : EntityTypeConfiguration<tblEN_Preguntas>
    {
        public PreguntasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.pregunta).HasColumnName("pregunta");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.visible).HasColumnName("visible");
            HasRequired(x => x.encuesta).WithMany(x => x.preguntas).HasForeignKey(y => y.encuestaID);
            Property(x => x.tipo).HasColumnName("tipo");
            ToTable("tblEN_Preguntas");
        }
    }
}

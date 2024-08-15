using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.SubContratista
{
    public class ResultadoSubContratistasMapping: EntityTypeConfiguration<tblEN_ResultadoSubContratistas>
    {
        public ResultadoSubContratistasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID"); ;
            Property(x => x.preguntaID).HasColumnName("preguntaID");
            Property(x => x.usuarioRespondioID).HasColumnName("usuarioRespondioID");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.encuestaFolioID).HasColumnName("encuestaFolioID");
            Property(x => x.porcentaje).HasColumnName("porcentaje");

            HasRequired(x => x.encuesta).WithMany().HasForeignKey(y => y.encuestaID);
            HasRequired(x => x.pregunta).WithMany().HasForeignKey(y => y.preguntaID);
            Property(x => x.respuesta).HasColumnName("respuesta");
            HasRequired(x => x.usuarioRespondio).WithMany().HasForeignKey(y => y.usuarioRespondioID);


            ToTable("tblEN_ResultadoSubContratistas");
        }
    }
}

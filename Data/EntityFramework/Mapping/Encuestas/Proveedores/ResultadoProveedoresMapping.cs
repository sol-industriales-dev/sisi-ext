using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.Proveedores
{
    public class ResultadoProveedoresMapping : EntityTypeConfiguration<tblEN_ResultadoProveedores>
    {
        public ResultadoProveedoresMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID"); ;
            Property(x => x.preguntaID).HasColumnName("preguntaID");
            Property(x => x.usuarioRespondioID).HasColumnName("usuarioRespondioID");
            Property(x => x.encuestaFolioID).HasColumnName("encuestaFolioID");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.tipoEncuesta).HasColumnName("tipoEncuesta");
            Property(x => x.respuesta).HasColumnName("respuesta");
            Property(x => x.porcentaje).HasColumnName("porcentaje");
            Property(x => x.calificacionPonderacion).HasColumnName("calificacionPonderacion");
            HasRequired(x => x.evaluador).WithMany().HasForeignKey(y => y.usuarioRespondioID);

            HasRequired(x => x.encuesta).WithMany().HasForeignKey(y => y.encuestaID);
            HasRequired(x => x.pregunta).WithMany().HasForeignKey(y => y.preguntaID);



            ToTable("tblEN_ResultadoProveedores");
        }
    }
}

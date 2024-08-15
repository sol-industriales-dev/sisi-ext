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
    public class ResultadosMapping : EntityTypeConfiguration<tblEN_Resultado>
    {
        public ResultadosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.preguntaID).HasColumnName("preguntaID");
            Property(x => x.usuarioRespondioID).HasColumnName("usuarioRespondioID");
            Property(x => x.encuestaUsuarioID).HasColumnName("encuestaUsuarioID");
            HasRequired(x => x.encuestaUsuario).WithMany().HasForeignKey(y => y.encuestaUsuarioID);
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.porcentaje).HasColumnName("porcentaje");

            ToTable("tblEN_Resultado");
        }
    }
}

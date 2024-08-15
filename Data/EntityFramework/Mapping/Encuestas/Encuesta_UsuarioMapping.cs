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
    public class Encuesta_UsuarioMapping : EntityTypeConfiguration<tblEN_Encuesta_Usuario>
    {
        public Encuesta_UsuarioMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.usuarioResponderID).HasColumnName("usuarioResponderID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.asunto).HasColumnName("asunto");
            Property(x => x.telefonica).HasColumnName("telefonica");
            Property(x => x.usuarioTelefonoID).HasColumnName("usuarioTelefonoID");
            Property(x => x.calificacion).HasColumnName("calificacion");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.tipoRespuesta).HasColumnName("tipoRespuesta");
            
            ToTable("tblEN_Encuesta_Usuario");
        }
    }
}

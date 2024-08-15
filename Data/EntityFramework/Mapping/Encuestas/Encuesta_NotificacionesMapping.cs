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
    class Encuesta_NotificacionesMapping : EntityTypeConfiguration<tblEN_Encuesta_Notificaciones>
    {
        public Encuesta_NotificacionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.encuestaID).HasColumnName("encuestaID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblEN_Encuesta_Notificaciones");
        }
    }
}

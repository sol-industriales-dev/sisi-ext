using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    public  class ComentariosObraMapping: EntityTypeConfiguration<tblPro_ComentariosObras>
    {
        public ComentariosObraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.capturadeObrasID).HasColumnName("capturadeObrasID");
            Property(x => x.registroID).HasColumnName("registroID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.usuarioNombre).HasColumnName("usuarioNombre");
            Property(x => x.estatusComentario).HasColumnName("estatusComentario");

            Property(x => x.adjunto).HasColumnName("adjunto");
            Property(x => x.adjuntoExt).HasColumnName("adjuntoExt");
            Property(x => x.adjuntoNombre).HasColumnName("adjuntoNombre");

            ToTable("tblPro_ComentariosObras");
        }
    }
}

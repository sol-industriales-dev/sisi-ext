using Core.Entity.Administrativo.cotizaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Cotizaciones
{
    public class CotizacionComentariosMapping : EntityTypeConfiguration<tblAD_CotizacionComentarios>
    {
        public CotizacionComentariosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cotizacionID).HasColumnName("cotizacionID");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.usuarioNombre).HasColumnName("usuarioNombre");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.adjuntoNombre).HasColumnName("adjuntoNombre");
            Property(x => x.adjuntoExt).HasColumnName("adjuntoExt");
            Property(x => x.adjunto).HasColumnName("adjunto");
            HasRequired(x => x.cotizacion).WithMany(x => x.comentarios).HasForeignKey(y => y.cotizacionID);
            ToTable("tblAD_CotizacionComentarios");
        }
    }
}

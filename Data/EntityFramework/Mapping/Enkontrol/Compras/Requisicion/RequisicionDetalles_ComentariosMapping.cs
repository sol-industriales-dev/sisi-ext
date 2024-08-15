using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Entity.Enkontrol.Compras.Requisicion;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.Requisicion
{
    public class RequisicionDetalles_ComentariosMapping : EntityTypeConfiguration<tblCom_ReqDet_Comentarios>
    {
        public RequisicionDetalles_ComentariosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.ReqDet_id).HasColumnName("ReqDet_id");
            HasRequired(x => x.reqDetalle).WithMany(x => x.comentarios).HasForeignKey(d => d.ReqDet_id);
            ToTable("tblCom_ReqDet_Comentarios");
        }
    }
}

using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class ComentariosNotaCreditoMapping : EntityTypeConfiguration<tblM_ComentariosNotaCredito>
    {
        public ComentariosNotaCreditoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioNombre).HasColumnName("usuarioNombre");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.notaCreditoID).HasColumnName("notaCreditoID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.nombreEvidencia).HasColumnName("nombreEvidencia");

            ToTable("tblM_ComentariosNotaCredito");
        }
    }
}

using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class RetencionesOCMapping : EntityTypeConfiguration<tblCom_OCRetenciones>
    {
        public RetencionesOCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.porcentajeRetencion).HasColumnName("porcentajeRetencion");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.facturado).HasColumnName("facturado");
            Property(x => x.retenido).HasColumnName("retenido");
            Property(x => x.aplica).HasColumnName("aplica");
            Property(x => x.ordenCompra_id).HasColumnName("ordenCompra_id");
            HasRequired(x => x.ordenCompra).WithMany(x => x.retenciones).HasForeignKey(d => d.ordenCompra_id);
            Property(x => x.retencion_id).HasColumnName("retencion_id");
            HasRequired(x => x.retencion).WithMany(x => x.retencionesOC).HasForeignKey(d => d.retencion_id);
            ToTable("tblCom_OCRetenciones");
        }
    }
}







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

    class OrdenCompra_Retenciones_InternaMapping : EntityTypeConfiguration<tblCom_OrdenCompra_Retenciones_Interna>
    {
        public OrdenCompra_Retenciones_InternaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.id_cpto).HasColumnName("id_cpto");
            Property(x => x.descRet).HasColumnName("descRet");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.porc_ret).HasColumnName("porc_ret");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.facturado).HasColumnName("facturado");
            Property(x => x.retenido).HasColumnName("retenido");
            Property(x => x.aplica).HasColumnName("aplica");
            Property(x => x.forma_pago).HasColumnName("forma_pago");
            Property(x => x.tm_descto).HasColumnName("tm_descto");
            Property(x => x.calc_iva).HasColumnName("calc_iva");
            Property(x => x.bit_afecta_oc).HasColumnName("bit_afecta_oc");
            Property(x => x.afecta_fac).HasColumnName("afecta_fac");
            Property(x => x.facturado_ret).HasColumnName("facturado_ret");
            Property(x => x.facturado_iva).HasColumnName("facturado_iva");
            Property(x => x.facturado_total).HasColumnName("facturado_total");
            Property(x => x.retenido_ret).HasColumnName("retenido_ret");
            Property(x => x.retenido_iva).HasColumnName("retenido_iva");
            Property(x => x.retenido_total).HasColumnName("retenido_total");

            ToTable("tblCom_OrdenCompra_Retenciones_Interna");
        }
    }
}

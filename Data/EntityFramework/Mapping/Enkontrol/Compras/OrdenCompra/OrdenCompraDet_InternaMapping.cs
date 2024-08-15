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
    class OrdenCompraDet_InternaMapping : EntityTypeConfiguration<tblCom_OrdenCompraDet_Interna>
    {
        public OrdenCompraDet_InternaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idOrdenCompra).HasColumnName("idOrdenCompra");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.partida).HasColumnName("partida");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.fecha_entrega).HasColumnName("fecha_entrega");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.precio).HasColumnName("precio");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.ajuste_cant).HasColumnName("ajuste_cant");
            Property(x => x.ajuste_imp).HasColumnName("ajuste_imp");
            Property(x => x.num_requisicion).HasColumnName("num_requisicion");
            Property(x => x.part_requisicion).HasColumnName("part_requisicion");
            Property(x => x.cant_recibida).HasColumnName("cant_recibida");
            Property(x => x.imp_recibido).HasColumnName("imp_recibido");
            Property(x => x.fecha_recibido).HasColumnName("fecha_recibido");
            Property(x => x.cant_canc).HasColumnName("cant_canc");
            Property(x => x.imp_canc).HasColumnName("imp_canc");
            Property(x => x.acum_ant).HasColumnName("acum_ant");
            Property(x => x.max_orig).HasColumnName("max_orig");
            Property(x => x.max_ppto).HasColumnName("max_ppto");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.porcent_iva).HasColumnName("porcent_iva");
            Property(x => x.iva).HasColumnName("iva");
            Property(x => x.partidaDescripcion).HasColumnName("partidaDescripcion");
            Property(x => x.estatusRegistro).HasColumnName("estatusRegistro");

            ToTable("tblCom_OrdenCompraDet_Interna");
        }
    }
}

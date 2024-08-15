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
    class MovimientosMapping : EntityTypeConfiguration<tblAlm_Movimientos>
    {
        public MovimientosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.almacen).HasColumnName("almacen");
            Property(x => x.tipo_mov).HasColumnName("tipo_mov");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.compania).HasColumnName("compania");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.ano).HasColumnName("ano");
            Property(x => x.orden_ct).HasColumnName("orden_ct");
            Property(x => x.frente).HasColumnName("frente");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.transferida).HasColumnName("transferida");
            Property(x => x.alm_destino).HasColumnName("alm_destino");
            Property(x => x.cc_destino).HasColumnName("cc_destino");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.tipo_trasp).HasColumnName("tipo_trasp");
            Property(x => x.tipo_cambio).HasColumnName("tipo_cambio");
            Property(x => x.estatusHabilitado).HasColumnName("estatusHabilitado");
            Property(x => x.ccReq).HasColumnName("ccReq");
            Property(x => x.numeroOC).HasColumnName("numeroOC");
            HasRequired(x => x.ordenCompra).WithMany(x => x.OC_Movimientos).HasForeignKey(d => d.numeroOC);
            Property(x => x.numeroReq).HasColumnName("numeroReq");
            HasRequired(x => x.requisicion).WithMany(x => x.req_Movimientos).HasForeignKey(d => d.numeroReq);

            ToTable("tblAlm_Movimientos");
        }
    }
}

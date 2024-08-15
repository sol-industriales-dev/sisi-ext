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
    class MovimientosDetMapping : EntityTypeConfiguration<tblAlm_MovimientosDet>
    {
        public MovimientosDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.almacen).HasColumnName("almacen");
            Property(x => x.tipo_mov).HasColumnName("tipo_mov");
            Property(x => x.partida).HasColumnName("partida");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.precio).HasColumnName("precio");
            Property(x => x.importe).HasColumnName("importe");
            Property(x => x.partida_oc).HasColumnName("partida_oc");
            Property(x => x.id_resguardo).HasColumnName("id_resguardo");
            Property(x => x.area_alm).HasColumnName("area_alm");
            Property(x => x.lado_alm).HasColumnName("lado_alm");
            Property(x => x.estante_alm).HasColumnName("estante_alm");
            Property(x => x.nivel_alm).HasColumnName("nivel_alm");
            Property(x => x.transporte).HasColumnName("transporte");
            Property(x => x.estatusHabilitado).HasColumnName("estatusHabilitado");
            Property(x => x.numero).HasColumnName("numero");
            HasRequired(x => x.movimiento).WithMany(x => x.mov_detalles).HasForeignKey(d => d.numero);

            ToTable("tblAlm_MovimientosDet");
        }
    }
}

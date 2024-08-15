using Core.Entity.Enkontrol.Compras.Requisicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.Requisicion
{
    class SurtidoDetMapping : EntityTypeConfiguration<tblCom_SurtidoDet>
    {
        public SurtidoDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.partidaRequisicion).HasColumnName("partidaRequisicion");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.almacenOrigenID).HasColumnName("almacenOrigenID");
            Property(x => x.almacenDestinoID).HasColumnName("almacenDestinoID");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.area_alm).HasColumnName("area_alm");
            Property(x => x.lado_alm).HasColumnName("lado_alm");
            Property(x => x.estante_alm).HasColumnName("estante_alm");
            Property(x => x.nivel_alm).HasColumnName("nivel_alm");
            Property(x => x.estadoSurtido).HasColumnName("estadoSurtido");
            Property(x => x.tipoSurtidoDetalle).HasColumnName("tipoSurtidoDetalle");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.surtidoID).HasColumnName("surtidoID");
            HasRequired(x => x.surtido).WithMany(x => x.surtido_detalle).HasForeignKey(d => d.surtidoID);

            ToTable("tblCom_SurtidoDet");
        }
    }
}

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
    public class RequisicionDetallesMapping : EntityTypeConfiguration<tblCom_ReqDet>
    {
        public RequisicionDetallesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idReq).HasColumnName("idReq");
            Property(x => x.partida).HasColumnName("partida");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.requerido).HasColumnName("requerido");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.precio).HasColumnName("precio");
            Property(x => x.cantOrdenada).HasColumnName("cantOrdenada");
            Property(x => x.ordenada).HasColumnName("ordenada");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.cantCancelada).HasColumnName("cantCancelada");
            Property(x => x.referencia).HasColumnName("referencia");
            Property(x => x.cantExcedida).HasColumnName("cantExcedida");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.comentarioSurtidoQuitar).HasColumnName("comentarioSurtidoQuitar");
            Property(x => x.estatusRegistro).HasColumnName("estatusRegistro");

            ToTable("tblCom_ReqDet");
        }
    }
}

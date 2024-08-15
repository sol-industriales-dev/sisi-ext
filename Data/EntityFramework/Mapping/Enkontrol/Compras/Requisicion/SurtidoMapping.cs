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
    class SurtidoMapping : EntityTypeConfiguration<tblCom_Surtido>
    {
        public SurtidoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.cantidadTotal).HasColumnName("cantidadTotal");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.tipo).HasColumnName("tipo");

            ToTable("tblCom_Surtido");
        }
    }
}

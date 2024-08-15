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
    class TraspasosPendientesTEMPMapping : EntityTypeConfiguration<tbl_TraspasosPendientesTEMP>
    {
        public TraspasosPendientesTEMPMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.numeroSalida).HasColumnName("numeroSalida");
            Property(x => x.ordenTraspaso).HasColumnName("ordenTraspaso");
            Property(x => x.almacenOrigen).HasColumnName("almacenOrigen");
            Property(x => x.almacenDestino).HasColumnName("almacenDestino");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.centroCostos).HasColumnName("centroCostos");

            ToTable("tbl_TraspasosPendientesTEMP");
        }
    }
}

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
    class TraspasoMapping : EntityTypeConfiguration<tblAlm_Traspaso>
    {
        public TraspasoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.ccOrigen).HasColumnName("ccOrigen");
            Property(x => x.almacenOrigen).HasColumnName("almacenOrigen");
            Property(x => x.ccDestino).HasColumnName("ccDestino");
            Property(x => x.almacenDestino).HasColumnName("almacenDestino");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.cantidadTraspasar).HasColumnName("cantidadTraspasar");
            Property(x => x.cantidadCancelada).HasColumnName("cantidadCancelada");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.autorizado).HasColumnName("autorizado");
            Property(x => x.rechazado).HasColumnName("rechazado");
            Property(x => x.folioInterno).HasColumnName("folioInterno");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.comentariosGestion).HasColumnName("comentariosGestion");
            Property(x => x.estado).HasColumnName("estado");
            Property(x => x.area_alm).HasColumnName("area_alm");
            Property(x => x.lado_alm).HasColumnName("lado_alm");
            Property(x => x.estante_alm).HasColumnName("estante_alm");
            Property(x => x.nivel_alm).HasColumnName("nivel_alm");
            Property(x => x.estatusRegistro).HasColumnName("estatusRegistro");

            ToTable("tblAlm_Traspaso");
        }
    }
}

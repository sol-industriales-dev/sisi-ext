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
    class OrdenCompra_InternaMapping : EntityTypeConfiguration<tblCom_OrdenCompra_Interna>
    {
        public OrdenCompra_InternaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.idLibreAbordo).HasColumnName("idLibreAbordo");
            Property(x => x.tipo_oc_req).HasColumnName("tipo_oc_req");
            Property(x => x.compradorSIGOPLAN).HasColumnName("compradorSIGOPLAN");
            Property(x => x.compradorEnkontrol).HasColumnName("compradorEnkontrol");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.tipo_cambio).HasColumnName("tipo_cambio");
            Property(x => x.porcent_iva).HasColumnName("porcent_iva");
            Property(x => x.sub_total).HasColumnName("sub_total");
            Property(x => x.iva).HasColumnName("iva");
            Property(x => x.total).HasColumnName("total");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.bienes_servicios).HasColumnName("bienes_servicios");
            Property(x => x.CFDI).HasColumnName("CFDI");
            Property(x => x.tiempoEntregaDias).HasColumnName("tiempoEntregaDias");
            Property(x => x.tiempoEntregaComentarios).HasColumnName("tiempoEntregaComentarios");
            Property(x => x.anticipo).HasColumnName("anticipo");
            Property(x => x.totalAnticipo).HasColumnName("totalAnticipo");
            Property(x => x.estatusRegistro).HasColumnName("estatusRegistro");
            Property(x => x.colocada).HasColumnName("colocada");
            Property(x => x.colocadaFecha).HasColumnName("colocadaFecha");
            Property(x => x.correoProveedor).HasColumnName("correoProveedor");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.st_impresa).HasColumnName("st_impresa");
            Property(x => x.autorizo).HasColumnName("autorizo");
            Property(x => x.usuario_autoriza).HasColumnName("usuario_autoriza");
            Property(x => x.fecha_autoriza).HasColumnName("fecha_autoriza");
            Property(x => x.ST_OC).HasColumnName("ST_OC");
            Property(x => x.empleado_autoriza).HasColumnName("empleado_autoriza");
            Property(x => x.empleadoUltimaAccion).HasColumnName("empleadoUltimaAccion");
            Property(x => x.fechaUltimaAccion).HasColumnName("fechaUltimaAccion");
            Property(x => x.tipoUltimaAccion).HasColumnName("tipoUltimaAccion");
            Property(x => x.categoria).HasColumnName("categoria");

            ToTable("tblCom_OrdenCompra_Interna");
        }
    }
}

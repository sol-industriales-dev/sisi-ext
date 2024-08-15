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
    class InsumoMapping : EntityTypeConfiguration<tblAlm_Insumo>
    {
        public InsumoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.grupo).HasColumnName("grupo");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.bloqueado).HasColumnName("bloqueado");
            Property(x => x.cancelado).HasColumnName("cancelado");
            Property(x => x.fijar_precio).HasColumnName("fijar_precio");
            Property(x => x.precio_a_fijar).HasColumnName("precio_a_fijar");
            Property(x => x.validar_lista_precios).HasColumnName("validar_lista_precios");
            Property(x => x.bit_pt).HasColumnName("bit_pt");
            Property(x => x.bit_mp).HasColumnName("bit_mp");
            Property(x => x.bit_factura).HasColumnName("bit_factura");
            Property(x => x.tolerancia).HasColumnName("tolerancia");
            Property(x => x.color_resguardo).HasColumnName("color_resguardo");
            Property(x => x.bit_rotacion).HasColumnName("bit_rotacion");
            Property(x => x.bit_area_cta).HasColumnName("bit_area_cta");
            Property(x => x.bit_af).HasColumnName("bit_af");
            Property(x => x.codigo_barras).HasColumnName("codigo_barras");
            Property(x => x.id_modelo_maquinaria).HasColumnName("id_modelo_maquinaria");
            Property(x => x.modeloMaquinariaDesc).HasColumnName("modeloMaquinariaDesc");
            Property(x => x.compras_req).HasColumnName("compras_req");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblAlm_Insumo");
        }
    }
}

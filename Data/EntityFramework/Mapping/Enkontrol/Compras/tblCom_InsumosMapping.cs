using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras
{
    class tblCom_InsumosMapping : EntityTypeConfiguration<tblCom_Insumos>
    {
        public tblCom_InsumosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.unidad).HasColumnName("unidad");
            Property(x => x.bit_area_cuenta).HasColumnName("bit_area_cuenta");
            Property(x => x.cancelado).HasColumnName("cancelado");
            Property(x => x.color_resguardo).HasColumnName("color_resguardo");
            Property(x => x.compras_req).HasColumnName("compras_req");

            ToTable("tblCom_Insumos");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Barrenacion;

namespace Data.EntityFramework.Mapping.Maquinaria.Barrenacion
{
    public class CatalogoPiezaMapping : EntityTypeConfiguration<tblB_CatalogoPieza>
    {
        public CatalogoPiezaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoPieza).HasColumnName("tipoPieza");
            Property(x => x.tipoBroca).HasColumnName("tipoBroca");
            Property(x => x.insumo).HasColumnName("insumo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            Property(x => x.incremento).HasColumnName("incremento");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");

            ToTable("tblB_CatalogoPieza");
        }
    }
}

using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo
{
    public class AF_CatTipoMovimientoMapping : EntityTypeConfiguration<tblC_AF_CatTipoMovimiento>
    {
        public AF_CatTipoMovimientoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.TipoDelMovimiento).HasColumnName("TipoDelMovimiento");
            Property(x => x.Descripcion).HasColumnName("Descripcion");
            ToTable("tblC_AF_CatTipoMovimiento");
        }
    }
}
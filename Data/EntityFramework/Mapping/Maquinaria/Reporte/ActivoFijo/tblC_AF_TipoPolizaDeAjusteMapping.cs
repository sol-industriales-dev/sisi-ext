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
    public class tblC_AF_TipoPolizaDeAjusteMapping : EntityTypeConfiguration<tblC_AF_TipoPolizaDeAjuste>
    {
        public tblC_AF_TipoPolizaDeAjusteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasMaxLength(50).HasColumnName("descripcion");
            ToTable("tblC_AF_TipoPolizaDeAjuste");
        }
    }
}

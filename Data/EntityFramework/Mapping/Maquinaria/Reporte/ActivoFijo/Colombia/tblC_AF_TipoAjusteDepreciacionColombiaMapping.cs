using Core.Entity.Maquinaria.Reporte.ActivoFijo.Colombia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Colombia
{
    public class tblC_AF_TipoAjusteDepreciacionColombiaMapping : EntityTypeConfiguration<tblC_AF_TipoAjusteDepreciacionColombia>
    {
        public tblC_AF_TipoAjusteDepreciacionColombiaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblC_AF_TipoAjusteDepreciacionColombia");
        }
    }
}

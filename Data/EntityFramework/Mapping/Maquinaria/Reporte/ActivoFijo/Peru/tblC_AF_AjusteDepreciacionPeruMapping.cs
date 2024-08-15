using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_AjusteDepreciacionPeruMapping : EntityTypeConfiguration<tblC_AF_AjusteDepreciacionPeru>
    {
        public tblC_AF_AjusteDepreciacionPeruMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.relacionPoliza).WithMany().HasForeignKey(y => y.relacionPolizaID);
            HasRequired(x => x.tipoAjuste).WithMany().HasForeignKey(y => y.tipoAjusteDepID);
            ToTable("tblC_AF_AjusteDepreciacionPeru");
        }
    }
}

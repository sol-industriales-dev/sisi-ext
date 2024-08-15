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
    public class tblC_AF_RelacionPolizaPeruAjusteMapping : EntityTypeConfiguration<tblC_AF_RelacionPolizaPeruAjuste>
    {
        public tblC_AF_RelacionPolizaPeruAjusteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tmPolizaId).HasColumnName("tmPolizaId");
            Property(x => x.tipoPolizaDeAjusteId).HasColumnName("tipoPolizaDeAjusteId");
            Property(x => x.relacionPolizaPeruId).HasColumnName("relacionPolizaPeruId");
            HasRequired(x => x.tmPoliza).WithMany().HasForeignKey(y => y.tmPolizaId);
            HasRequired(x => x.tipoPolizaAjuste).WithMany().HasForeignKey(y => y.tipoPolizaDeAjusteId);
            HasRequired(x => x.relacionPolizaPeru).WithMany().HasForeignKey(y => y.relacionPolizaPeruId);
            ToTable("tblC_AF_RelacionPolizaPeruAjuste");
        }
    }
}

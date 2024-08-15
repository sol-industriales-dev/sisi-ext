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
    public class tblC_AF_RelacionPolizaColombiaAjusteMapping : EntityTypeConfiguration<tblC_AF_RelacionPolizaColombiaAjuste>
    {
        public tblC_AF_RelacionPolizaColombiaAjusteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tmPolizaId).HasColumnName("tmPolizaId");
            Property(x => x.tipoPolizaDeAjusteId).HasColumnName("tipoPolizaDeAjusteId");
            Property(x => x.relacionPolizaColombiaId).HasColumnName("relacionPolizaColombiaId");
            HasRequired(x => x.tmPoliza).WithMany().HasForeignKey(y => y.tmPolizaId);
            HasRequired(x => x.tipoPolizaAjuste).WithMany().HasForeignKey(y => y.tipoPolizaDeAjusteId);
            HasRequired(x => x.relacionPolizaColombia).WithMany().HasForeignKey(y => y.relacionPolizaColombia);
            ToTable("tblC_AF_RelacionPolizaColombiaAjuste");
        }
    }
}

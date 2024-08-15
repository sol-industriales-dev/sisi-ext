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
    public class tblC_AF_RelacionPolizaPeruMapping : EntityTypeConfiguration<tblC_AF_RelacionPolizaPeru>
    {
        public tblC_AF_RelacionPolizaPeruMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.relacionPolizaPeruId_baja).HasColumnName("relacionPolizaPeruId_baja");
            Property(x => x.tmPolizaId).HasColumnName("tmPolizaId");
            HasOptional(x => x.relacionPolizaPeru_baja).WithMany().HasForeignKey(y => y.relacionPolizaPeruId_baja);
            HasRequired(x => x.tmPoliza).WithMany().HasForeignKey(y => y.tmPolizaId);
            ToTable("tblC_AF_RelacionPolizaPeru");
        }
    }
}

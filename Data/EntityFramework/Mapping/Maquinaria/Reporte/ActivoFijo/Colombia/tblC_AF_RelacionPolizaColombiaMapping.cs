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
    public class tblC_AF_RelacionPolizaColombiaMapping : EntityTypeConfiguration<tblC_AF_RelacionPolizaColombia>
    {
        public tblC_AF_RelacionPolizaColombiaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.relacionPolizaColombiaId_baja).HasColumnName("relacionPolizaColombiaId_baja");
            Property(x => x.tmPolizaId).HasColumnName("tmPolizaId");
            HasOptional(x => x.relacionPolizaColombia_baja).WithMany().HasForeignKey(y => y.relacionPolizaColombiaId_baja);
            HasRequired(x => x.tmPoliza).WithMany().HasForeignKey(y => y.tmPolizaId);
            ToTable("tblC_AF_RelacionPolizaColombia");
        }
    }
}

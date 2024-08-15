using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    class EdoFinancieroConceptoMapping : EntityTypeConfiguration<tblEF_EdoFinancieroConcepto>
    {
        public EdoFinancieroConceptoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.tipoOperacion).HasColumnName("tipoOperacion");
            Property(x => x.ordenReporte).HasColumnName("ordenReporte");
            Property(x => x.grupoReporteID).HasColumnName("grupoReporteID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.tieneEnlace).HasColumnName("tieneEnlace");
            Property(x => x.tipoEnlaceId).HasColumnName("tipoEnlaceId");
            HasRequired(x => x.grupo).WithMany().HasForeignKey(x => x.grupoReporteID);

            ToTable("tblEF_EdoFinancieroConcepto");
        }
    }
}

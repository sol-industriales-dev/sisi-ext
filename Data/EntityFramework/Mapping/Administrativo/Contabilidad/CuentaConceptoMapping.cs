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
    class CuentaConceptoMapping : EntityTypeConfiguration<tblEF_CuentaConcepto>
    {
        public CuentaConceptoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.conceptoID).HasColumnName("conceptoID");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.concepto).WithMany().HasForeignKey(x => x.conceptoID);

            ToTable("tblEF_CuentaConcepto");
        }
    }
}

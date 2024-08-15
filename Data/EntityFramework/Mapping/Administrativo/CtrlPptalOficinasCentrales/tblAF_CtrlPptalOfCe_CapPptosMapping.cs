using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_CapPptosMapping : EntityTypeConfiguration<tblAF_CtrlPptalOfCe_CapPptos>
    {
        public tblAF_CtrlPptalOfCe_CapPptosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.concepto).WithMany().HasForeignKey(x => x.idConcepto);
            HasRequired(x => x.agrupacion).WithMany().HasForeignKey(x => x.idAgrupacion);

            ToTable("tblAF_CtrlPptalOfCe_CapPptos");
        }
    }
}

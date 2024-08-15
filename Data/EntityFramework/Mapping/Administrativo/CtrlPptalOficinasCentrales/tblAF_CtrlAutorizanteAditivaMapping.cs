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
    public class tblAF_CtrlAutorizanteAditivaMapping : EntityTypeConfiguration<tblAF_CtrlAutorizanteAditiva>
    {
        public tblAF_CtrlAutorizanteAditivaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.autorizante).WithMany().HasForeignKey(x => x.idAutorizante);
            ToTable("tblAF_CtrlAutorizanteAditiva");
        }
    }
}

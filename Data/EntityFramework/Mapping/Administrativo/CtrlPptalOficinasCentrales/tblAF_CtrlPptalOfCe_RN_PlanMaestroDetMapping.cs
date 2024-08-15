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
    public class tblAF_CtrlPptalOfCe_RN_PlanMaestroDetMapping : EntityTypeConfiguration<tblAF_CtrlPptalOfCe_RN_PlanMaestroDet>
    {
        public tblAF_CtrlPptalOfCe_RN_PlanMaestroDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblAF_CtrlPptalOfCe_RN_PlanMaestroDet");
        }
    }
}

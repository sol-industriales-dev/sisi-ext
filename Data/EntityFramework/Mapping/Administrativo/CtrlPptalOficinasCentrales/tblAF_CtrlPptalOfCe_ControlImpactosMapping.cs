using Core.Entity.Administrativo.ControlPresupuestalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.ControlPresupuestalOficinasCentrales
{
    public class tblAF_CtrlPresupuestalOfCe_ControlImpactosMapping : EntityTypeConfiguration<tblAF_CtrlPptalOfCe_ControlImpactos>
    {
        public tblAF_CtrlPresupuestalOfCe_ControlImpactosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblAF_CtrlPptalOfCe_ControlImpactos");
        }
    }
}

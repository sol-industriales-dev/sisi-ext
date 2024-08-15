using Core.Entity.Administrativo.CtrlPresupuestalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.ControlPresupuestalOficinasCentrales
{
    public class tblAF_CtrlPresupuestalOfCe_MatrizEstrategicasMapping : EntityTypeConfiguration<tblAF_CtrlPptalOfCe_MatrizEstrategicas>
    {
        public tblAF_CtrlPresupuestalOfCe_MatrizEstrategicasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblAF_CtrlPptalOfCe_MatrizEstrategicas");
        }
    }
}

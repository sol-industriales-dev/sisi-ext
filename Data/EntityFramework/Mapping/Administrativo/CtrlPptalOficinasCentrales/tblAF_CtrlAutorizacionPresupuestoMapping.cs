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
    public class tblAF_CtrlAutorizacionPresupuestoMapping : EntityTypeConfiguration<tblAF_CtrlAutorizacionPresupuesto>
    {
        public tblAF_CtrlAutorizacionPresupuestoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(x => x.usuarioModificacionId);
            HasRequired(x => x.presupuesto).WithMany().HasForeignKey(x => x.presupuestoId);
            ToTable("tblAF_CtrlAutorizacionPresupuesto");
        }
    }
}

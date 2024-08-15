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
    public class tblAF_CtrlAutorizacionAditivaMapping : EntityTypeConfiguration<tblAF_CtrlAutorizacionAditiva>
    {
        public tblAF_CtrlAutorizacionAditivaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.aditiva).WithMany().HasForeignKey(x => x.aditivaId);
            HasRequired(x => x.autorizante).WithMany().HasForeignKey(x => x.autorizanteId);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(x => x.usuarioModificacionId);
            ToTable("tblAF_CtrlAutorizacionAditiva");
        }
    }
}

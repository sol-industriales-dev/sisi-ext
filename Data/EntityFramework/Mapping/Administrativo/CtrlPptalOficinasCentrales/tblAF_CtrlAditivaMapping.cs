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
    public class tblAF_CtrlAditivaMapping : EntityTypeConfiguration<tblAF_CtrlAditiva>
    {
        public tblAF_CtrlAditivaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.presupuesto).WithMany().HasForeignKey(x => x.capPptosId);
            HasOptional(x => x.autorizanteUno).WithMany().HasForeignKey(x => x.autorizante1);
            HasOptional(x => x.autorizanteDos).WithMany().HasForeignKey(x => x.autorizante2);
            HasOptional(x => x.autorizanteTres).WithMany().HasForeignKey(x => x.autorizante3);
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(x => x.idUsuarioCreacion);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(x => x.idUsuarioModificacion);
            ToTable("tblAF_CtrlAditiva");
        }
    }
}

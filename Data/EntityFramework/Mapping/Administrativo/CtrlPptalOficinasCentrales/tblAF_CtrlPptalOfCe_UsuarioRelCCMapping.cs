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
    public class tblAF_CtrlPptalOfCe_UsuarioRelCCMapping : EntityTypeConfiguration<tblAF_CtrlPptalOfCe_UsuarioRelCC>
    {
        public tblAF_CtrlPptalOfCe_UsuarioRelCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.idUsuario);

            ToTable("tblAF_CtrlPptalOfCe_UsuarioRelCC");
        }
    }
}

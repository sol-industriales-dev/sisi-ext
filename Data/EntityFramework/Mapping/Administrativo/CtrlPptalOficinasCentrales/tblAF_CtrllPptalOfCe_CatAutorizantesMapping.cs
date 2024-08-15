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
    public class tblAF_CtrllPptalOfCe_CatAutorizantesMapping: EntityTypeConfiguration<tblAF_CtrllPptalOfCe_CatAutorizantes>
    {
        public tblAF_CtrllPptalOfCe_CatAutorizantesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idRow).HasColumnName("idRow");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.idAutorizante).HasColumnName("idAutorizante");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.registroActivo).HasColumnName("registroActivo");

            ToTable("tblAF_CtrllPptalOfCe_CatAutorizantes");
        }
    }
}

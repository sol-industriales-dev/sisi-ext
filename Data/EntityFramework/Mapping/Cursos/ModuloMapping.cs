using Core.Entity.Cursos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Cursos
{
    public class ModuloMapping:EntityTypeConfiguration<tblCU_Modulo>
    {
        public ModuloMapping()
        {
        HasKey(x => x.id);
        Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
        Property(x => x.idCurso).HasColumnName("idCurso");//llave foranea
        Property(x => x.descripcion).HasColumnName("descripcion");
        Property(x => x.nombreModulo).HasColumnName("nombreModulo");
        Property(x => x.estado).HasColumnName("estado");
        Property(x => x.completo).HasColumnName("completo");
        ToTable("tblCu_Modulo");
        }
    }
}

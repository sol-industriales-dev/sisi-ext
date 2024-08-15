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
    public class ModuloDetMapping: EntityTypeConfiguration<tblCU_ModuloDet>
    {
           public ModuloDetMapping()
           {
               HasKey(x => x.id);
               Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
               Property(x => x.idModulo).HasColumnName("idModulo");//llave foranea
               Property(x => x.descripcion).HasColumnName("descripcion");
               Property(x => x.pagina).HasColumnName("pagina");
               Property(x => x.estado).HasColumnName("estado");
               ToTable("tblCu_ModuloDet");
           }
    }
}

using Core.Entity.Sistemas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Sistemas
{
    public class DirectorioArchivosMapping : EntityTypeConfiguration<tblP_DirArchivos>
    {
        public DirectorioArchivosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.dirFisico).HasColumnName("dirFisico");
            Property(x => x.dirVirtual).HasColumnName("dirVirtual");
            ToTable("tblP_DirArchivos");
        }
    }
}

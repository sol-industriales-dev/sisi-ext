using Core.Entity.Principal.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Catalogos
{
    public class tblP_CatEscolaridadesMapping : EntityTypeConfiguration<tblP_CatEscolaridades>
    {
        public tblP_CatEscolaridadesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblP_CatEscolaridades");
        }
    }
}

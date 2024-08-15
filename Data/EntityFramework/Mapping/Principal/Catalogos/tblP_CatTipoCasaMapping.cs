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
    public class tblP_CatTipoCasaMapping : EntityTypeConfiguration<tblP_CatTipoCasa>
    {
        public tblP_CatTipoCasaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoCasa).HasColumnName("tipoCasa");

            ToTable("tblP_CatTipoCasa");
        }
    }
}

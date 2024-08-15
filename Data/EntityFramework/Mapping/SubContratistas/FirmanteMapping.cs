using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class FirmanteMapping : EntityTypeConfiguration<tblX_Firmante>
    {
        public FirmanteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasOptional(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioId);
            ToTable("tblX_Evaluador");
        }
    }
}

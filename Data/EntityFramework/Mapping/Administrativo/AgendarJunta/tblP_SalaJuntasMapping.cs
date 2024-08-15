using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.AgendarJunta;

namespace Data.EntityFramework.Mapping.Administrativo.AgendarJunta
{
    public class tblP_SalaJuntasMapping : EntityTypeConfiguration<tblP_SalaJunta>
    {
        public tblP_SalaJuntasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblP_SalaJunta");
        }

    }
}

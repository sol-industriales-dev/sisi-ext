using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_TabuladoresMapping : EntityTypeConfiguration<tblRH_EK_Tabuladores>
    {
        public tblRH_EK_TabuladoresMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_EK_Tabuladores");
        }
    }
}

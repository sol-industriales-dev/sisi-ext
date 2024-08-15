using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_TabuladoresMapping : EntityTypeConfiguration<tblRH_TAB_Tabuladores>
    {
        public tblRH_TAB_TabuladoresMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.puesto).WithMany().HasForeignKey(x => x.FK_Puesto);
            ToTable("tblRH_TAB_Tabuladores");
        }
    }
}
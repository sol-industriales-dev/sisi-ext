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
    public class tblRH_TAB_TabuladoresDetMapping : EntityTypeConfiguration<tblRH_TAB_TabuladoresDet>
    {
        public tblRH_TAB_TabuladoresDetMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.tabulador).WithMany().HasForeignKey(x => x.FK_Tabulador);
            HasRequired(x => x.categoria).WithMany().HasForeignKey(x => x.FK_Categoria);
            ToTable("tblRH_TAB_TabuladoresDet");
        }
    }
}
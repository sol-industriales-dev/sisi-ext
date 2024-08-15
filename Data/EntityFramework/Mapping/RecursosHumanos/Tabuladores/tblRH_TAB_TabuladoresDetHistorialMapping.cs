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
    public class tblRH_TAB_TabuladoresDetHistorialMapping : EntityTypeConfiguration<tblRH_TAB_TabuladoresDetHistorial>
    {
        public tblRH_TAB_TabuladoresDetHistorialMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_TAB_TabuladoresDetHistorial");
        }
    }
}
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
    public class tblRH_TAB_PlantillasPersonalMapping : EntityTypeConfiguration<tblRH_TAB_PlantillasPersonal>
    {
        public tblRH_TAB_PlantillasPersonalMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_TAB_PlantillasPersonal");
        }
    }
}

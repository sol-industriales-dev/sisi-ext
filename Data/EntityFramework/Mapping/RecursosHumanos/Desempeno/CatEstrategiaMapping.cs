using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class CatEstrategiaMapping : EntityTypeConfiguration<tblRH_ED_CatEstrategia>
    {
        public CatEstrategiaMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            ToTable("tblRH_ED_CatEstrategia");
        }
    }
}

using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Catalogo
{
    public class CatPuestosMapping: EntityTypeConfiguration<tblRH_CatPuestos>
    {
        public CatPuestosMapping()
        {
            Property(x => x.puesto).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("puesto");
            Property(x => x.descripcion).HasColumnName("descripcion");
        }
    }
}

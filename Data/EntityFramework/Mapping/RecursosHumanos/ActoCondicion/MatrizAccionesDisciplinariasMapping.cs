using Core.Entity.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.ActoCondicion
{
    public class MatrizAccionesDisciplinariasMapping : EntityTypeConfiguration<tblRH_AC_MatrizAccionesDisciplinarias>
    {
        public MatrizAccionesDisciplinariasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblRH_AC_MatrizAccionesDisciplinarias");
        }
    }
}

using Core.Entity.Administrativo.Facultamiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Facultamiento
{
    public class FacultamientoMapping : EntityTypeConfiguration<tblFa_CatFacultamiento>
    {
        public FacultamientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.obra).HasColumnName("obra");
            ToTable("tblFa_CatFacultamiento");
        }
    }
}

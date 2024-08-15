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
    public class PuestoMapping : EntityTypeConfiguration<tblFa_CatPuesto>
    {
        public PuestoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idFacultamiento).HasColumnName("idFacultamiento");
            Property(x => x.idTabla).HasColumnName("idTabla");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.puesto).HasColumnName("puesto");
            ToTable("tblFa_CatPuesto");
        }
    }
}

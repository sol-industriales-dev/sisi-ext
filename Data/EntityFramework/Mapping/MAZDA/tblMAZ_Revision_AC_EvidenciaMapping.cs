using Core.Entity.MAZDA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.MAZDA
{
    public class tblMAZ_Revision_AC_EvidenciaMapping : EntityTypeConfiguration<tblMAZ_Revision_AC_Evidencia>
    {
        public tblMAZ_Revision_AC_EvidenciaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idRevision).HasColumnName("idRevision");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            ToTable("tblMAZ_Revision_AC_Evidencia");
        }
    }
}

using Core.Entity.Administrativo.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad
{
    public class PartesMapping : EntityTypeConfiguration<tblS_CatPartes>
    {
        public PartesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idTipo).HasColumnName("idTipo");
            Property(x => x.parte).HasColumnName("parte");
            ToTable("tblS_CatPartes");
        }
    }
}

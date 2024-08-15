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
    public class LicenciasMapping : EntityTypeConfiguration<tblS_CatLicencias>
    {
        public LicenciasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cve).HasColumnName("cve");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.vigencia).HasColumnName("vigencia");
            ToTable("tblS_CatLicencias");
        }
    }
}

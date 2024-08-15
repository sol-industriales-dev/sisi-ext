using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.SaludOcupacional
{
    public class tblS_SO_HistorialesClinicosMapping : EntityTypeConfiguration<tblS_SO_HistorialesClinicos>
    {
        public tblS_SO_HistorialesClinicosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblS_SO_HistorialesClinicos");
        }
    }
}

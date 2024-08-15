using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.ActoCondicion
{
    public class AccionReaccionContactoPersonalMapping : EntityTypeConfiguration<tblSAC_AccionReaccionContactoPersonal>
    {
        public AccionReaccionContactoPersonalMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblSAC_AccionReaccionContactoPersonal");
        }
    }
}

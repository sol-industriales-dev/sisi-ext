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
    public class ObservacionesMapping : EntityTypeConfiguration<tblS_Observaciones>
    {
        public ObservacionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idVehiculo).HasColumnName("idVehiculo");
            Property(x => x.idTipo).HasColumnName("idTipo");
            Property(x => x.idParte).HasColumnName("idParte");
            Property(x => x.anterior).HasColumnName("anterior");
            Property(x => x.actual).HasColumnName("actual");
            Property(x => x.observaciones).HasColumnName("observaciones");
            ToTable("tblS_Observaciones");
        }
    }
}

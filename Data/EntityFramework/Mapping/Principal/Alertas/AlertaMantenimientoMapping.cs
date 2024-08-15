using System;
using Core.Entity.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Principal.Alertas
{
    public class AlertaMantenimientoMapping : EntityTypeConfiguration<tblP_AlertaMantenimiento>
    {
        public AlertaMantenimientoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.mensaje).HasColumnName("mensaje");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.fechaProgramada).HasColumnName("fechaProgramada");
            ToTable("tblP_AlertaMantenimiento");
        }
    }
}

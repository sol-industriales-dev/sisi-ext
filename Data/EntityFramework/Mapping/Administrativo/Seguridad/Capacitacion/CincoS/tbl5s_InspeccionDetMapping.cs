using Core.Entity.Administrativo.Seguridad.Capacitacion.CincoS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion.CincoS
{
    public class tbl5s_InspeccionDetMapping : EntityTypeConfiguration<tbl5s_InspeccionDet>
    {
        public tbl5s_InspeccionDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.inspeccion).WithMany().HasForeignKey(x => x.inspeccionId);
            HasRequired(x => x.cincoS).WithMany().HasForeignKey(x => x.cincoId);
            ToTable("tbl5s_InspeccionDet");
        }
    }
}

using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class CapacitacionExtracurricularMapping : EntityTypeConfiguration<tblS_CapacitacionExtracurricular>
    {
        public CapacitacionExtracurricularMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.duracion).HasColumnName("duracion");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.fechaExpiracion).HasColumnName("fechaExpiracion");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.division).HasColumnName("division");
            ToTable("tblS_CapacitacionExtracurricular");
        }
    }
}

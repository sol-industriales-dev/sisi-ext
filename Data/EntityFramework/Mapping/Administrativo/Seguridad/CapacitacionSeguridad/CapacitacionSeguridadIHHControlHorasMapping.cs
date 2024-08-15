using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class CapacitacionSeguridadIHHControlHorasMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadIHHControlHoras>
    {
        public CapacitacionSeguridadIHHControlHorasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.horas).HasColumnName("horas");
            Property(x => x.colaboradorCapacitacionID).HasColumnName("colaboradorCapacitacionID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadIHHControlHoras");
        }
    }
}

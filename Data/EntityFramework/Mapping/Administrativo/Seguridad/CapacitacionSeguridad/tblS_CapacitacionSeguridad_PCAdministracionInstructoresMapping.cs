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
    public class tblS_CapacitacionSeguridad_PCAdministracionInstructoresMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridad_PCAdministracionInstructores>
    {
        public tblS_CapacitacionSeguridad_PCAdministracionInstructoresMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cveEmpleado).HasColumnName("cveEmpleado");
            Property(x => x.grupo).HasColumnName("grupo");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.instructor).HasColumnName("instructor");
            Property(x => x.tematica).HasColumnName("tematica");
            Property(x => x.esActivo).HasColumnName("esActivo");


            ToTable("tblS_CapacitacionSeguridad_PCAdministracionInstructores");
        }
    }
}

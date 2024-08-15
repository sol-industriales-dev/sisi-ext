using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Indicadores
{
    class tblS_IncidentesEmpleadosContratistasMappig : EntityTypeConfiguration<tblS_IncidentesEmpleadosContratistas>
    {
        public tblS_IncidentesEmpleadosContratistasMappig()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.claveContratista).HasColumnName("claveContratista");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.fechaNacimiento).HasColumnName("fechaNacimiento");
            Property(x => x.puesto).HasColumnName("puesto");

            ToTable("tblS_IncidentesEmpleadosContratistas");
        }
    }
}

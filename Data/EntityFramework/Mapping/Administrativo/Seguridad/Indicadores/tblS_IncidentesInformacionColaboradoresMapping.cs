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
    class tblS_IncidentesInformacionColaboradoresMapping : EntityTypeConfiguration<tblS_IncidentesInformacionColaboradores>
    {
        public tblS_IncidentesInformacionColaboradoresMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.horasHombre).HasColumnName("horasHombre");
            Property(x => x.lostDay).HasColumnName("lostDay");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            HasOptional(x => x.agrupacion).WithMany().HasForeignKey(x => x.idAgrupacion);

            ToTable("tblS_IncidentesInformacionColaboradores");
        }
    }
}

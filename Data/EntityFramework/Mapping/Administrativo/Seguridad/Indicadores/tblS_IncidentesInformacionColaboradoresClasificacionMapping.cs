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
    class tblS_IncidentesInformacionColaboradoresClasificacionMapping : EntityTypeConfiguration<tblS_IncidentesInformacionColaboradoresClasificacion>
    {
        public tblS_IncidentesInformacionColaboradoresClasificacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.horasMantenimiento).HasColumnName("horasMantenimiento");
            Property(x => x.horasOperativo).HasColumnName("horasOperativo");
            Property(x => x.horasAdministrativo).HasColumnName("horasAdministrativo");
            Property(x => x.informacionColaboradoresID).HasColumnName("informacionColaboradoresID");
            HasRequired(x => x.InformacionColaboradores).WithMany().HasForeignKey(y => y.informacionColaboradoresID);
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");


            ToTable("tblS_IncidentesInformacionColaboradoresClasificacion");
        }
    }
}

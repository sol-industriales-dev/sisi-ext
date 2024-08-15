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
    class tblS_IncidentesInformacionColaboradoresDetalleMapping : EntityTypeConfiguration<tblS_IncidentesInformacionColaboradoresDetalle>
    {
        public tblS_IncidentesInformacionColaboradoresDetalleMapping()
        {
            HasKey(x => x.id);
            Property(x => x.cveEmpleado).HasColumnName("cveEmpleado");
            Property(x => x.lostDayEmpleado).HasColumnName("lostDayEmpleado");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.clasificacion).HasColumnName("clasificacion");
            Property(x => x.estatus).HasColumnName("estatus");

            Property(x => x.idIncidente).HasColumnName("idIncidente");
            HasRequired(x => x.Incidente).WithMany().HasForeignKey(y => y.idIncidente);
            //HasRequired(x => x.modeloEquipo).WithMany().HasForeignKey(y => y.modeloEquipoID);
            ToTable("tblS_IncidentesInformacionColaboradoresDetalle");
        }
    }
}

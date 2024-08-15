using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.ReservacionVehiculo;

namespace Data.EntityFramework.Mapping.Administrativo.ReservacionVehiculo
{
    public class tblRV_SolicitudesMapping : EntityTypeConfiguration<tblRV_Solicitudes>
    {
        public tblRV_SolicitudesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaSalida).HasColumnName("fechaSalida");
            Property(x => x.fechaEntrega).HasColumnName("fechaEntrega");
            Property(x => x.vigenciaLicencia).HasColumnName("vigenciaLicencia");
            Property(x => x.motivo).HasColumnName("motivo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.solicitante).HasColumnName("solicitante");
            Property(x => x.autorizada).HasColumnName("autorizada");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.usuarioRegistroID).HasColumnName("usuarioRegistroID");
            HasRequired(x => x.usuarioRegistro).WithMany(x => x.solicitudesVehiculo).HasForeignKey(d => d.usuarioRegistroID);
            ToTable("tblRV_Solicitudes");
        }
    }
}

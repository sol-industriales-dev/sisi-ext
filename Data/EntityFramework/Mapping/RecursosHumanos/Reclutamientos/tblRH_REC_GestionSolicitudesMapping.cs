using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_GestionSolicitudesMapping : EntityTypeConfiguration<tblRH_REC_GestionSolicitudes>
    {
        public tblRH_REC_GestionSolicitudesMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idSolicitud).HasColumnName("idSolicitud");
            Property(x => x.esAutorizada).HasColumnName("esAutorizada");
            Property(x => x.motivoRechazo).HasColumnName("motivoRechazo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.virtualLstSolicitudes).WithMany().HasForeignKey(y => y.idSolicitud);

            ToTable("tblRH_REC_GestionSolicitudes");
        }
    }
}

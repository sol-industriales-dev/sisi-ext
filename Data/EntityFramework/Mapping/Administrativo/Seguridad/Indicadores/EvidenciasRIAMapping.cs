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
    public class EvidenciasRIAMapping : EntityTypeConfiguration<tblS_IncidentesEvidenciasRIA>
    {
        public EvidenciasRIAMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.incidente_id).HasColumnName("incidente_id");
            HasRequired(x => x.incidente).WithMany().HasForeignKey(x => x.incidente_id);
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioCreadorID);
            Property(x => x.tipoEvidenciaRIA).HasColumnName("tipoEvidenciaRIA");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_IncidentesEvidenciasRIA");
        }
    }
}

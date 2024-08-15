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
    public class EvidenciasMapping : EntityTypeConfiguration<tblS_IncidentesEvidencias>
    {
        public EvidenciasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.informe_id).HasColumnName("informe_id");
            HasRequired(x => x.informe).WithMany().HasForeignKey(x => x.informe_id);
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioCreadorID);
            Property(x => x.activa).HasColumnName("activa");
            ToTable("tblS_IncidentesEvidencias");
        }
    }
}

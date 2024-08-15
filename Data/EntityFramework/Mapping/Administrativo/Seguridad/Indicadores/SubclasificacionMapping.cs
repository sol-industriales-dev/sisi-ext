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
    public class SubclasificacionMapping : EntityTypeConfiguration<tblS_IncidentesSubclasificacion>
    {
        public SubclasificacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.subclasificacion).HasColumnName("subclasificacion");
            Property(x => x.tipoAccidenteID).HasColumnName("tipoAccidenteID");
            HasRequired(x => x.tipoAccidente).WithMany(y => y.subclasificaciones).HasForeignKey(x => x.tipoAccidenteID);
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            ToTable("tblS_IncidentesSubclasificacion");
        }
    }
}

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
    public class CapacitacionSeguridadAutorizacionMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadAutorizacion>
    {
        public CapacitacionSeguridadAutorizacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.controlAsistenciaID).HasColumnName("controlAsistenciaID");
            HasRequired(x => x.controlAsistencia).WithMany().HasForeignKey(x => x.controlAsistenciaID);
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioID);
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.tipoPuesto).HasColumnName("tipoPuesto");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.division).HasColumnName("division");
            ToTable("tblS_CapacitacionSeguridadAutorizacion");
        }
    }
}

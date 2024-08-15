using Core.Entity.Administrativo.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.Capacitacion
{
    public class CapacitacionListaAutorizacionRFCMapping : EntityTypeConfiguration<tblS_CapacitacionListaAutorizacionRFC>
    {
        public CapacitacionListaAutorizacionRFCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.razonSocial).HasColumnName("razonSocial");
            Property(x => x.listaAutorizacionID).HasColumnName("listaAutorizacionID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionListaAutorizacionRFC");
        }
    }
}

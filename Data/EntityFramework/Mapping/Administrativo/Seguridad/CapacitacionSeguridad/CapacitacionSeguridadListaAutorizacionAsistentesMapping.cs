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
    public class CapacitacionSeguridadListaAutorizacionAsistentesMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadListaAutorizacionAsistentes>
    {
        public CapacitacionSeguridadListaAutorizacionAsistentesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.fechaVencimiento).HasColumnName("fechaVencimiento");
            Property(x => x.listaAutorizacionID).HasColumnName("listaAutorizacionID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadListaAutorizacionAsistentes");
        }
    }
}

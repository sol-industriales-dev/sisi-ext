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
    public class CapacitacionSeguridadListaAutorizacionCCMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadListaAutorizacionCC>
    {
        public CapacitacionSeguridadListaAutorizacionCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.departamento).HasColumnName("departamento");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.listaAutorizacionID).HasColumnName("listaAutorizacionID");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblS_CapacitacionSeguridadListaAutorizacionCC");
        }
    }
}

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
    public class CapacitacionSeguridadRelacionCCAutorizanteMapping : EntityTypeConfiguration<tblS_CapacitacionSeguridadRelacionCCAutorizante>
    {
        public CapacitacionSeguridadRelacionCCAutorizanteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.tipoPuesto).HasColumnName("tipoPuesto");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.division).HasColumnName("division");

            ToTable("tblS_CapacitacionSeguridadRelacionCCAutorizante");
        }
    }
}

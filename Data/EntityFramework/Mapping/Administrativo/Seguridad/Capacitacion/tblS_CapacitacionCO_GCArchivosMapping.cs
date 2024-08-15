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
    public class tblS_CapacitacionCO_GCArchivosMapping : EntityTypeConfiguration<tblS_CapacitacionCO_GCArchivos>
    {

          public tblS_CapacitacionCO_GCArchivosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idFormatoCambio).HasColumnName("idFormatoCambio");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            ToTable("tblS_CapacitacionCO_GCArchivos");
        }
    }
}

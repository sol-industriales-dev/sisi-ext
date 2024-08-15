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
    public class tblRH_REC_TipoDocumentoMapping : EntityTypeConfiguration<tblRH_REC_TipoDocumento>
    {
        public tblRH_REC_TipoDocumentoMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.orden).HasColumnName("orden");

            ToTable("tblRH_REC_TipoDocumento");
        }
    }
}

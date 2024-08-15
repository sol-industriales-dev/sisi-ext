using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Desempeno
{
    public class DetObservacionEvidenciaMapping : EntityTypeConfiguration<tblRH_ED_DetObservacionEvidencia>
    {
        public DetObservacionEvidenciaMapping()
        {
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idObservacion).HasColumnName("idObservacion");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ruta).HasColumnName("ruta");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            HasRequired(x => x.observacion).WithMany().HasForeignKey(y => y.idObservacion);
            ToTable("tblRH_ED_DetObservacionEvidencia");
        }
    }
}

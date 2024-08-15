using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class EstimacionCobranzaMapping : EntityTypeConfiguration<tblC_EstimacionCobranza>
    {
        public EstimacionCobranzaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estimado).HasColumnName("estimado").HasPrecision(22, 4);
            Property(x => x.semana1).HasColumnName("semana1").HasPrecision(22, 4);
            Property(x => x.semana2).HasColumnName("semana2").HasPrecision(22, 4);
            Property(x => x.semana3).HasColumnName("semana3").HasPrecision(22, 4);
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_EstimacionCobranza");
        }
    }
}

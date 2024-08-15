using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.FlujoEfectivo
{
    public class CcVistoMapping : EntityTypeConfiguration<tblC_FED_CcVisto>
    {
        public CcVistoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.esVisto).HasColumnName("esVisto");
            ToTable("tblC_FED_CcVisto");
        }
    }
}

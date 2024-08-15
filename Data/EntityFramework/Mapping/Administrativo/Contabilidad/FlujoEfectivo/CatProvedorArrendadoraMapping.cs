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
    public class CatProvedorArrendadoraMapping : EntityTypeConfiguration<tblC_FED_CatProvedorArrendadora>
    {
        public CatProvedorArrendadoraMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.numrpo).HasColumnName("numrpo");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblC_FED_CatProvedorArrendadora");
        }
    }
}

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
    public class CatConceptoDirectoMapping : EntityTypeConfiguration<tblC_FED_CatConcepto>
    {
        public CatConceptoDirectoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idPadre).HasColumnName("idPadre");
            Property(x => x.Concepto).HasColumnName("Concepto");
            Property(x => x.operador).HasColumnName("operador");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.idAccion).HasColumnName("idAccion");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_FED_CatConcepto");
        }
    }
}

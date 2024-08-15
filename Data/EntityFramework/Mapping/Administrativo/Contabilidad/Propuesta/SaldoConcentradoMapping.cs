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
    public class SaldoConcentradoMapping : EntityTypeConfiguration<tblC_SaldoConcentrado>
    {
        public SaldoConcentradoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idSaldoConciliado).HasColumnName("idSaldoConciliado");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.beneficiario).HasColumnName("beneficiario");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.noCheque).HasColumnName("noCheque");
            Property(x => x.cargo).HasColumnName("cargo").HasPrecision(22, 4);
            Property(x => x.abono).HasColumnName("abono").HasPrecision(22, 4);
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.ultimoCambio).HasColumnName("ultimoCambio");
            ToTable("tblC_SaldoConcentrado");
        }
    }
}

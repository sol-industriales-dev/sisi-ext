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
    public class SaldoConciliadoMapping : EntityTypeConfiguration<tblC_SaldoConciliado>
    {
        public SaldoConciliadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.saldo).HasColumnName("saldo");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.ultimoCambio).HasColumnName("ultimoCambio");
            ToTable("tblC_SaldoConciliado");
        }
    }
}

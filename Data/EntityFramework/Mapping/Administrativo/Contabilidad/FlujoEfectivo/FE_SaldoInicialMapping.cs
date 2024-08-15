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
    public class FE_SaldoInicialMapping : EntityTypeConfiguration<tblC_FE_SaldoInicial>
    {
        public FE_SaldoInicialMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.saldo).HasColumnName("saldo").HasPrecision(22, 4);
            Property(x => x.idUsuarioRegistro).HasColumnName("idUsuarioRegistro");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaRegistro).HasColumnName("fechaRegistro");
            ToTable("tblC_FE_SaldoInicial");
        }
    }
}

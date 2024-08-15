using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad
{
    class SaldoCCMapping : EntityTypeConfiguration<tblEF_SaldoCC>
    {
        public SaldoCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.saldoInicial).HasColumnName("saldoInicial");
            Property(x => x.cargosMes).HasColumnName("cargosMes");
            Property(x => x.abonosMes).HasColumnName("abonosMes");
            Property(x => x.cargosAcumulados).HasColumnName("cargosAcumulados");
            Property(x => x.abonosAcumulados).HasColumnName("abonosAcumulados");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.area).HasColumnName("area");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.corteMesID).HasColumnName("corteMesID");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.corte).WithMany().HasForeignKey(x => x.corteMesID);

            ToTable("tblEF_SaldoCC");
        }
    }
}

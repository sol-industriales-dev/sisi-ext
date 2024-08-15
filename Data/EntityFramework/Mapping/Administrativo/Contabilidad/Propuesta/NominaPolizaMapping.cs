using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class NominaPolizaMapping : EntityTypeConfiguration<tblC_NominaPoliza>
    {
        public NominaPolizaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.mes).HasColumnName("mes");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.tipoCuenta).HasColumnName("tipoCuenta");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.tipoCuenta).HasColumnName("tipoCuenta");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.cargo).HasColumnName("cargo").HasPrecision(22, 4);
            Property(x => x.abono).HasColumnName("abono").HasPrecision(22, 4);
            Property(x => x.iva).HasColumnName("iva").HasPrecision(22, 4);
            Property(x => x.retencion).HasColumnName("retencion").HasPrecision(22, 4);
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblC_NominaPoliza");
        }
    }
}

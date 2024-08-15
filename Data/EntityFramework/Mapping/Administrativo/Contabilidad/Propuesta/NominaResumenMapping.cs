using Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class NominaResumenMapping : EntityTypeConfiguration<tblC_NominaResumen>
    {
        public NominaResumenMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.fecha_inicial).HasColumnName("fecha_inicial");
            Property(x => x.fecha_final).HasColumnName("fecha_final");
            Property(x => x.tipoCuenta).HasColumnName("tipoCuenta");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.nomina).HasColumnName("nomina").HasPrecision(22,4);
            Property(x => x.iva).HasColumnName("iva").HasPrecision(22, 4);
            Property(x => x.retencion).HasColumnName("retencion").HasPrecision(22, 4);
            Property(x => x.total).HasColumnName("total").HasPrecision(22, 4);
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblC_NominaResumen");
        }
    }
}

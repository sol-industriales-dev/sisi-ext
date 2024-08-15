using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Poliza
{
    public class MovimientoPolizaMapping : EntityTypeConfiguration<tblPo_MovimientoPoliza>
    {
        public MovimientoPolizaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idPoliza).HasColumnName("idPoliza");
            Property(x => x.linea).HasColumnName("linea");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.digito).HasColumnName("digito");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.movimiento).HasColumnName("movimiento");
            Property(x => x.numProverdor).HasColumnName("numProverdor");
            Property(x => x.referencia).HasColumnName("referencia");
            Property(x => x.orderCompraCliente).HasColumnName("orderCompraCliente");
            Property(x => x.tipoMovimiento).HasColumnName("tipoMovimiento");
            Property(x => x.Monto).HasColumnName("Monto");
            Property(x => x.tipoMoneda).HasColumnName("tipoMoneda");
            Property(x => x.cc).HasColumnName("cc");
            ToTable("tblPo_MovimientoPoliza");
        }
    }
}

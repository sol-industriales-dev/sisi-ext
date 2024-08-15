using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Propuesta
{
    public class SaldosCondensadosMapping : EntityTypeConfiguration<tblC_SaldosCondensados>
    {
        public SaldosCondensadosMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idGiro).HasColumnName("idGiro");
            Property(x => x.numpro).HasColumnName("numpro");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.factura).HasColumnName("factura");
            Property(x => x.fechaFactura).HasColumnName("fechaFactura");
            Property(x => x.fechaVence).HasColumnName("fechaVence");
            Property(x => x.tm).HasColumnName("tm");
            Property(x => x.total).HasColumnName("total").HasPrecision(22, 4);
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.esPropuesta).HasColumnName("esPropuesta");
            Property(x => x.fechaPropuesta).HasColumnName("fechaPropuesta");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblC_SaldosCondensados");
        }
    }
}

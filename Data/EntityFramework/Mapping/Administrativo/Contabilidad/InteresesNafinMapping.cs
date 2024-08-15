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
    public class InteresesNafinMapping : EntityTypeConfiguration<tblC_InteresesNafin>
    {
        public InteresesNafinMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.totalCadenas).HasColumnName("totalCadenas");
            Property(x => x.totalBanco).HasColumnName("totalBanco");
            Property(x => x.interes).HasColumnName("interes").HasPrecision(18, 6);
            Property(x => x.tipoCambio).HasColumnName("tipoCambio").HasPrecision(18, 6);
            Property(x => x.divisa).HasColumnName("divisa");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            ToTable("tblC_InteresesNafin");
        }
    }
}

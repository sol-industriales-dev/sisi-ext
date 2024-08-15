using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    class FiniquitoSalarioMinMapping : EntityTypeConfiguration<tblRH_FiniquitoSalarioMin>
    {
        public FiniquitoSalarioMinMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.salarioMinimo).HasColumnName("salarioMinimo");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblRH_FiniquitoSalarioMin");
        }
    }
}

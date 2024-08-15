using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Catalogo
{
    class FiniquitoConceptosMapping : EntityTypeConfiguration<tblRH_FiniquitoConceptos>
    {
        public FiniquitoConceptosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.concepto).HasColumnName("concepto");
            Property(x => x.detalle).HasColumnName("detalle");
            Property(x => x.operador).HasColumnName("operador");
            Property(x => x.estatus).HasColumnName("estatus");
            ToTable("tblRH_FiniquitoConceptos");
        }
    }
}

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
    public class tblRH_BN_EstatusPeriodosMapping : EntityTypeConfiguration<tblRH_BN_EstatusPeriodos>
    {
        public tblRH_BN_EstatusPeriodosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.tipo_nomina).HasColumnName("tipo_nomina");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.fecha_inicial).HasColumnName("fecha_inicial");
            Property(x => x.fecha_final).HasColumnName("fecha_final");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fecha_limite).HasColumnName("fecha_limite");
            

            ToTable("tblRH_BN_EstatusPeriodos");
        }
    }
}

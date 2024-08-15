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
    class tblC_FED_CorteMapping : EntityTypeConfiguration<tblC_FED_Corte>
    {
        public tblC_FED_CorteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.semana).HasColumnName("semana");
            Property(x => x.fecha_inicio).HasColumnName("fecha_inicio");
            Property(x => x.fecha_fin).HasColumnName("fecha_fin");
            Property(x => x.fecha_registro).HasColumnName("fecha_registro");
            Property(x => x.actual).HasColumnName("actual");

            ToTable("tblC_FED_Corte");
        }
    }
}

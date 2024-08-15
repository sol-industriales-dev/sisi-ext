using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class KPIMapping : EntityTypeConfiguration<tblM_KPI>
    {
        public KPIMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.proyeccionHoras).HasColumnName("proyeccionHoras");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.mes).HasColumnName("mes");
            ToTable("tblM_KPI");
        }
    }
}

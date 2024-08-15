using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Proyecciones
{
    class ActivoFijoMapping : EntityTypeConfiguration<tblPro_ActivoFijo>
    {
        public ActivoFijoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.CadenaJson).HasColumnName("CadenaJson");
            Property(x => x.Anio).HasColumnName("Anio");
            Property(x => x.Mes).HasColumnName("Mes");
            Property(x => x.Estatus).HasColumnName("Estatus");
            ToTable("tblPro_ActivoFijo");
        }
    }
}

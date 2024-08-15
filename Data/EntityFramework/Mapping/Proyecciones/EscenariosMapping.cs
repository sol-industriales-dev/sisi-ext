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
    public class EscenariosMapping : EntityTypeConfiguration<tblPro_CatEscenarios>
    {
        public EscenariosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.Pinicial).HasColumnName("Pinicial");
            Property(x => x.PFinal).HasColumnName("PFinal");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.PadreID).HasColumnName("PadreID");
            Property(x => x.nivel).HasColumnName("PadreID");
            Property(x => x.ordenID).HasColumnName("ordenID");

            ToTable("tblPro_CatEscenarios");
        }
    
    }
}
